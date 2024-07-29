using System.Linq.Expressions;
using System.Reflection;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Path.Filters.Values;
using Hyperbee.Json.Query;

namespace Hyperbee.Json.Path.Filters.Parser.Expressions;

internal class SelectExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, out CompareConstraint compareConstraint, ITypeDescriptor<TNode> _ = null )
    {
        compareConstraint = CompareConstraint.None;
        var item = state.Item;

        if ( item.IsEmpty || item[0] != '$' && item[0] != '@' )
        {
            expression = null;
            return false;
        }

        expression = ExpressionHelper<TNode>.GetExpression( state.Item, state.IsArgument );
        return true;
    }

    private static class ExpressionHelper<TNode>
    {
        private static readonly MethodInfo SelectMethod =
            typeof( ExpressionHelper<TNode> )
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
            var options = allowDotWhitespace
                ? JsonQueryParserOptions.Rfc9535AllowDotWhitespace
                : JsonQueryParserOptions.Rfc9535;

            var compiledQuery = JsonQueryParser.Parse( query, options );

            var value = query[0] == '$'
                ? runtimeContext.Root
                : runtimeContext.Current; // @

            var nodes = JsonPath<TNode>.SelectInternal( value, runtimeContext.Root, compiledQuery );

            return new NodeList<TNode>( nodes, compiledQuery.Normalized );
        }
    }
}
