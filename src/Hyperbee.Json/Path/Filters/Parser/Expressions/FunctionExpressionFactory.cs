using System.Linq.Expressions;
using Hyperbee.Json.Descriptors;

namespace Hyperbee.Json.Path.Filters.Parser.Expressions;

internal class FunctionExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, out CompareConstraint compareConstraint, ITypeDescriptor<TNode> descriptor )
    {
        compareConstraint = CompareConstraint.None;
        expression = null;

        if ( state.Item.IsEmpty || !char.IsLetter( state.Item[0] ) )
        {
            return false;
        }

        if ( !descriptor.Functions.TryGetActivator( state.Item.ToString(), out var functionActivator ) )
        {
            return false;
        }

        if ( state.TrailingWhitespace )
            throw new NotSupportedException( "Whitespace is not allowed after a function name." );

        var function = functionActivator();

        expression = function.GetExpression<TNode>( ref state ); // will recurse for each function argument.
        compareConstraint = CompareConstraint.Function | function.CompareConstraint;

        return true;
    }
}
