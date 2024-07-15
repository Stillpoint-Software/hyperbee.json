using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class FunctionExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo expressionInfo, ITypeDescriptor<TNode> descriptor )
    {
        if ( state.Item.IsEmpty || !char.IsLetter( state.Item[0] ) )
        {
            expression = null;
            return false;
        }

        if ( descriptor.Functions.TryGetActivator( state.Item.ToString(), out var functionActivator ) )
        {
            if ( state.TrailingWhitespace )
                throw new NotSupportedException( "Whitespace is not allowed after a function name." );

            var function = functionActivator();

            expression = function
                .GetExpression( ref state, descriptor ); // will recurse for each function argument.

            expressionInfo.Kind = ExpressionKind.Function;
            expressionInfo.FunctionInfo = function.FunctionInfo;
            return true;
        }

        expression = null;
        return false;
    }
}
