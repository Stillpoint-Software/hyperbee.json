using System.Linq.Expressions;
using static Hyperbee.Json.Filters.Parser.JsonPathExpression;

namespace Hyperbee.Json.Filters.Parser;

public class FilterFunction
{
    private readonly FilterFunction _implementation;

    public FilterFunction()
    {
        _implementation = this;
    }

    internal FilterFunction( ReadOnlySpan<char> item, FilterTokenType? type, ParseExpressionContext context )
    {
        if ( TryGetParenFunction( item, type, context, out _implementation ) )
        {
            return;
        }

        if ( TryGetFilterFunction( item, context, out _implementation ) )
        {
            return;
        }

        if ( TryGetExtensionFunction( item, context, out _implementation ) )
        {
            return;
        }

        // No functions not found, try to parse this as a literal value.
        var literalFunction = new LiteralFunction();
        _implementation = literalFunction;
    }

    public Expression GetExpression( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        return _implementation.GetExpressionImpl( data, item, ref start, ref from );
    }

    protected virtual Expression GetExpressionImpl( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        // The real implementation will be in the derived classes.
        return Expression.Throw( Expression.Constant( new NotImplementedException() ) );
    }

    private static bool TryGetParenFunction( ReadOnlySpan<char> item, FilterTokenType? type, ParseExpressionContext context, out FilterFunction function )
    {
        function = null;

        if ( item.Length != 0 || type != FilterTokenType.OpenParen )
        {
            return false;
        }

        function = new ParenFunction( context );
        return true;
    }

    private static bool TryGetFilterFunction( ReadOnlySpan<char> item, ParseExpressionContext context, out FilterFunction function )
    {
        switch ( item[0] )
        {
            case '@':
                function = context.Descriptor.GetFilterFunction( context );
                return true;
            case '$':
                // Current becomes root
                function = context.Descriptor.GetFilterFunction( context with { Current = context.Root } );
                return true;
        }

        function = null;
        return false;
    }

    private static bool TryGetExtensionFunction( ReadOnlySpan<char> item, ParseExpressionContext context, out FilterFunction function )
    {
        function = null;
        
        if ( !TryParseFunction( item, out var method, out var arguments ) )
            return false;

        if ( !context.Descriptor.Functions.TryGetValue( method, out var creator ) )
            return false;

        function = creator( method, [.. arguments], context );
        return true;

    }

    private static bool TryParseFunction( ReadOnlySpan<char> exprSpan, out string method, out List<string> arguments )
    {
        method = null;
        arguments = null;

        // Find parens

        var openParenIndex = exprSpan.IndexOf( '(' );

        if ( openParenIndex == -1 )
            return false;

        // Method name

        var methodSpan = exprSpan[..openParenIndex].Trim();

        if ( methodSpan.Length == 0 || !char.IsLower( methodSpan[0] ) )
            return false;

        for ( var i = 0; i < methodSpan.Length; i++ )
        {
            if ( !(char.IsLower( methodSpan[i] ) || char.IsDigit( methodSpan[i] ) || methodSpan[i] == '_') )
                return false;
        }

        method = new string( methodSpan );

        // Arguments
        
        var argsSpan = exprSpan[(openParenIndex + 1)..].Trim();
        arguments = [];

        if ( argsSpan.Length > 0 )
            arguments = ParseArguments( argsSpan );

        return true;
    }

    
    private static List<string> ParseArguments( ReadOnlySpan<char> argsSpan )
    {
        List<string> arguments = [];
        var length = argsSpan.Length;
        var inQuotes = false;
        var quoteChar = '\0';
        var start = 0;

        for ( var i = 0; i < length; i++ )
        {
            var c = argsSpan[i];

            switch ( c )
            {
                case '"':
                case '\'':
                    {
                        if ( inQuotes )
                        {
                            if ( c == quoteChar )
                            {
                                inQuotes = false;
                            }
                        }
                        else
                        {
                            inQuotes = true;
                            quoteChar = c;
                        }

                        break;
                    }
                case ',' when !inQuotes:
                    arguments.Add( argsSpan[start..i].Trim().ToString() );
                    start = i + 1;
                    break;
            }
        }

        if ( start < length )
            arguments.Add( argsSpan[start..length].Trim().ToString() );

        return arguments;
    }
}
