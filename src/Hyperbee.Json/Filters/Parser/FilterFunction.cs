using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser;

public class FilterFunction
{
    private readonly FilterFunction _implementation;

    public FilterFunction()
    {
        _implementation = this;
    }

    internal FilterFunction( ReadOnlySpan<char> item, FilterExpressionParser.FilterTokenType? type, ParseExpressionContext context )
    {
        if ( TryGetParenFunction( item, type, context, out _implementation ) )
            return;

        if ( TryGetFilterFunction( item, context, out _implementation ) )
            return;

        if ( TryGetExtensionFunction( item, context, out _implementation ) )
            return;

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

    private static bool TryGetParenFunction( ReadOnlySpan<char> item, FilterExpressionParser.FilterTokenType? type, ParseExpressionContext context, out FilterFunction function )
    {
        function = null;

        if ( item.Length != 0 || type != FilterExpressionParser.FilterTokenType.OpenParen )
            return false;

        function = new ParenFunction( context );
        return true;
    }

    private static bool TryGetFilterFunction( ReadOnlySpan<char> item, ParseExpressionContext context, out FilterFunction function )
    {
        switch ( item[0] )
        {
            case '@':
                function = context.Descriptor.GetSelectFunction( context );
                return true;
            case '$':
                // Current becomes root
                function = context.Descriptor.GetSelectFunction( context with { Current = context.Root } );
                return true;
        }

        function = null;
        return false;
    }

    private static bool TryGetExtensionFunction( ReadOnlySpan<char> item, ParseExpressionContext context, out FilterFunction function )
    {
        function = null;

        var method = item.ToString();

        if ( !context.Descriptor.Functions.TryGet( method, out var creator ) )
            return false;

        function = creator( context );
        return true;
    }
}
