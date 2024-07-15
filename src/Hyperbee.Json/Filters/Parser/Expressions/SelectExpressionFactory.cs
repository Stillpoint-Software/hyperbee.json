using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Filters.Values;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class SelectExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo itemContext, ITypeDescriptor<TNode> descriptor )
    {
        var item = state.Item;
        expression = null;

        if ( item.IsEmpty || item[0] != '$' && item[0] != '@' )
            return false;

        expression = ExpressionHelper<TNode>.GetExpression( state.Item, state.IsArgument );

        if ( expression == null )
            return false;

        itemContext.Kind = ExpressionKind.Select;
        return true;
    }

    private static class ExpressionHelper<TNode>
    {
        private static readonly MethodInfo SelectMethod = typeof( ExpressionHelper<TNode> )
            .GetMethod( nameof( Select ), BindingFlags.NonPublic | BindingFlags.Static );

        public static MethodCallExpression GetExpression( ReadOnlySpan<char> item, bool allowDotWhitespace )
        {
            return Expression.Call(
                SelectMethod,
                Expression.Constant( item.ToString() ),
                Expression.Constant( allowDotWhitespace ),
                FilterParser<TNode>.RuntimeContextExpression );
        }

        private static IValueType Select( string query, bool allowDotWhitespace, FilterRuntimeContext<TNode> runtimeContext )
        {
            var compileQuery = JsonPathQueryParser.Parse( query, allowDotWhitespace );

            var value = query[0] == '$' ? runtimeContext.Root : runtimeContext.Current;

            return new NodeList<TNode>( JsonPath<TNode>.SelectInternal( value, runtimeContext.Root, compileQuery ), compileQuery.Normalized );
        }
    }
}
