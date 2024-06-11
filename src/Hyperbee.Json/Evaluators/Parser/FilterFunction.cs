using System.Linq.Expressions;
using static Hyperbee.Json.Evaluators.Parser.JsonPathExpression;

namespace Hyperbee.Json.Evaluators.Parser;

public class FilterFunction
{
    private readonly FilterFunction _implementation;

    public FilterFunction()
    {
        _implementation = this;
    }

    internal FilterFunction( ReadOnlySpan<char> item, FilterTokenType? type, ParseExpressionContext context )
    {
        if ( item.Length == 0 && type == FilterTokenType.OpenParen )
        {
            // There is no function, just an expression in parentheses.
            _implementation = new ParenFunction( context );
            return;
        }

        switch ( item[0] )
        {
            case '@':
                _implementation = context.Descriptor.GetFilterFunction( context );
                return;
            case '$':
                // Current becomes root
                _implementation = context.Descriptor.GetFilterFunction( context with { Current = context.Root } );
                return;
        }

        if ( TryGetExpressionFunction( item, context, out _implementation ) )
        {
            // Methods based on spec
            return;
        }

        // Function not found, will try to parse this as a literal value.
        var literalFunction = new LiteralFunction();
        _implementation = literalFunction;
    }

    public Expression GetExpression( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        return _implementation.Evaluate( data, item, ref start, ref from );
    }

    protected virtual Expression Evaluate( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from )
    {
        // The real implementation will be in the derived classes.
        return Expression.Throw( Expression.Constant( new NotImplementedException() ) );
    }

    private static bool TryGetExpressionFunction( ReadOnlySpan<char> item, ParseExpressionContext context, out FilterFunction function )
    {
        var match = FilterTokenizerRegex.RegexFunction().Match( item.ToString() );

        if ( match.Groups.Count != 3 )
        {
            function = null;
            return false;
        }

        var method = match.Groups[1].Value;
        var arguments = match.Groups[2].Value.Split( ',', options: StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries );

        if ( context.Descriptor.Functions.TryGetValue( method.ToLowerInvariant(), out var creator ) )
        {
            function = creator( method, arguments, context );
            return true;
        }

        function = null;
        return false;
    }
}
