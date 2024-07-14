using System.Linq.Expressions;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class SelectExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo itemContext, FilterParserContext<TNode> parserContext )
    {
        expression = ExpressionHelper<TNode>.GetExpression( state.Item, state.IsArgument, parserContext );

        if ( expression == null )
            return false;

        itemContext.Kind = ExpressionKind.Select;
        return true;
    }

    static class ExpressionHelper<TNode>
    {
        private static readonly Expression SelectExpression = Expression.Constant( (Func<string, bool, FilterRuntimeContext<TNode>, IValueType>) Select );

        public static Expression GetExpression( ReadOnlySpan<char> item, bool allowDotWhitespace, FilterParserContext<TNode> parserContext )
        {
            if ( item.IsEmpty )
                return null;

            if ( item[0] != '$' && item[0] != '@' )
                return null;

            return Expression.Invoke(
                SelectExpression,
                Expression.Constant( item.ToString() ),
                Expression.Constant( allowDotWhitespace ),
                parserContext.RuntimeContext );
        }

        private static IValueType Select( string query, bool allowDotWhitespace, FilterRuntimeContext<TNode> runtimeContext )
        {
            var compileQuery = JsonPathQueryParser.Parse( query, allowDotWhitespace );

            // Current becomes root
            return query[0] == '$'
                ? new NodeList<TNode>( JsonPath<TNode>.SelectInternal( runtimeContext.Root, runtimeContext.Root, compileQuery ), compileQuery.Normalized )
                : new NodeList<TNode>( JsonPath<TNode>.SelectInternal( runtimeContext.Current, runtimeContext.Root, compileQuery ), compileQuery.Normalized );
        }

    }
}
