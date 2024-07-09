﻿using System.Linq.Expressions;

namespace Hyperbee.Json.Filters.Parser.Expressions;

internal class FunctionExpressionFactory : IExpressionFactory
{
    public static bool TryGetExpression<TNode>( ref ParserState state, out Expression expression, ref ExpressionInfo expressionInfo, FilterContext<TNode> context )
    {
        if ( context.Descriptor.Functions.TryGetCreator( state.Item.ToString(), out var functionCreator ) )
        {
            if ( state.TrailingWhitespace )
                throw new NotSupportedException( "Whitespace is not allowed after a function name." );

            expression = functionCreator()
                .GetExpression( ref state, context ); // will recurse for each function argument.

            expressionInfo.Kind = ExpressionKind.Function;
            return true;
        }

        expression = null;
        return false;
    }
}
