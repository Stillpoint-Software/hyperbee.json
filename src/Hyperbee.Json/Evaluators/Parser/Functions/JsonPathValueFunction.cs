using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class JsonPathValueFunction<TType>( string methodName, IList<string> arguments, ParseExpressionContext<TType> context ) : ParserExpressionFunction<TType>( methodName, arguments, context )
{
    public const string Name = "value";

    public override Expression GetExpression( string methodName, IList<string> arguments, ParseExpressionContext<TType> context )
    {
        if ( methodName != Name )
        {
            return Expression.Block(
                Expression.Throw( Expression.Constant( new Exception( $"Invalid function name {methodName} for {Name}" ) ) ),
                Expression.Constant( 0F )
            );
        }

        if ( arguments.Count != 1 )
        {
            return Expression.Block(
                Expression.Throw( Expression.Constant( new Exception( $"Invalid use of {Name} function" ) ) ),
                Expression.Constant( 0F )
            );
        }

        var queryExp = Expression.Constant( arguments[0] );
        var evaluatorExp = Expression.Constant( context.Evaluator );

        return Expression.Call(
            JsonPathHelper<TType>.GetFirstElementValueMethod,
            context.Current,
            context.Root,
            queryExp,
            evaluatorExp );
    }
}
