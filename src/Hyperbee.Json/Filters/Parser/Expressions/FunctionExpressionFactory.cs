using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class FunctionExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo exprInfo, ITypeDescriptor<TNode> descriptor )
    {
        if ( state.Item.IsEmpty || !char.IsLetter( state.Item[0] ) )
        {
            expression = null;
            return false;
        }

        if ( !descriptor.Functions.TryGetActivator( state.Item.ToString(), out var functionActivator ) )
        {
            expression = null;
            return false;
        }

        if ( state.TrailingWhitespace )
            throw new NotSupportedException( "Whitespace is not allowed after a function name." );

        var function = functionActivator();

        expression = function
            .GetExpression( ref state, descriptor ); // will recurse for each function argument.

        exprInfo.Kind = ExpressionKind.Function;
        exprInfo.FunctionInfo = function.FunctionInfo;
        return true;
    }
}
