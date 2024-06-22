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
        if ( TryGetParenFunction( item, type, out _implementation ) )
            return;

        if ( TryGetFilterFunction( item, context, out _implementation ) )
            return;

        if ( TryGetExtensionFunction( item, context, out _implementation ) )
            return;

        // No functions found, try to parse this as a literal value.

        var literalFunction = new LiteralFunction();
        _implementation = literalFunction;
    }

    public Expression GetExpression( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from, ParseExpressionContext context )
    {
        return _implementation.GetExpressionImpl( data, item, ref start, ref from, context );
    }

    protected virtual Expression GetExpressionImpl( ReadOnlySpan<char> data, ReadOnlySpan<char> item, ref int start, ref int from, ParseExpressionContext context )
    {
        // The real implementation will be in the derived classes.
        return Expression.Throw( Expression.Constant( new NotImplementedException() ) );
    }

    private static bool TryGetParenFunction( ReadOnlySpan<char> item, FilterExpressionParser.FilterTokenType? type, out FilterFunction function )
    {
        function = null;

        if ( item.Length != 0 || type != FilterExpressionParser.FilterTokenType.OpenParen )
            return false;

        function = new ParenFunction();
        return true;
    }

    private static bool TryGetFilterFunction( ReadOnlySpan<char> item, ParseExpressionContext context, out FilterFunction function )
    {
        if ( item[0] == '$' || item[0] == '@' )
        {
            function = context.Descriptor.GetSelectFunction();
            return true;
        }

        function = null;
        return false;
    }

    private static bool TryGetExtensionFunction( ReadOnlySpan<char> item, ParseExpressionContext context, out FilterFunction function )
    {
        function = null;

        var method = item.ToString();

        if ( !context.Descriptor.Functions.TryGetCreator( method, out var creator ) )
            return false;

        function = creator();
        return true;
    }
}
