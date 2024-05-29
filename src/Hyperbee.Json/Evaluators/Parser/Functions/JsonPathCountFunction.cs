using System.Linq.Expressions;
using System.Reflection;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class JsonPathCountFunction<TType>( string methodName, string[] arguments, Expression currentExpression, Expression rootExpression, IJsonPathScriptEvaluator<TType> evaluator, string context = null ) : ParserExpressionFunction<TType>( methodName, arguments, currentExpression, rootExpression, evaluator, context )
{
    public const string Name = "count";

    // ReSharper disable once StaticMemberInGenericType
    private static readonly MethodInfo CountMethod;

    static JsonPathCountFunction()
    {
        CountMethod = typeof( Enumerable )
            .GetMethods( BindingFlags.Static | BindingFlags.Public )
            .First( m =>
                m.Name == "Count" &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof( IEnumerable<> ) )
            .MakeGenericMethod( typeof( TType ) );
    }

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

        return Expression.Convert( Expression.Call(
            CountMethod,
            Expression.Call( JsonPathHelper<TType>.SelectMethod,
                currentExpression,
                rootExpression,
                queryExp,
                evaluatorExp ) ), typeof( float ) );
    }
}
