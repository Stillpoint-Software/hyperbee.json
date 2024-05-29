using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class JsonPathValueFunction<TType>( string methodName, string[] arguments, Expression currentExpression, Expression rootExpression, IJsonPathScriptEvaluator<TType> evaluator, string context = null ) : ParserExpressionFunction<TType>( methodName, arguments, currentExpression, rootExpression, evaluator, context )
{
    public const string Name = "value";

    public override Expression GetExpression( string methodName, string[] arguments, Expression currentExpression, Expression rootExpression, IJsonPathScriptEvaluator<TType> evaluator, string context )
    {
        if ( methodName != Name )
        {
            return Expression.Block(
                Expression.Throw( Expression.Constant( new Exception( $"Invalid function name {methodName} for {Name}" ) ) ),
                Expression.Constant( 0F )
            );
        }

        if ( arguments.Length != 1 )
        {
            return Expression.Block(
                Expression.Throw( Expression.Constant( new Exception( $"Invalid use of {Name} function" ) ) ),
                Expression.Constant( 0F )
            );
        }

        var queryExp = Expression.Constant( arguments[0] );
        var evaluatorExp = Expression.Constant( evaluator );

        return Expression.Call(
            JsonPathHelper<TType>.GetFirstElementValueMethod,
            currentExpression,
            rootExpression,
            queryExp,
            evaluatorExp );
    }
}
