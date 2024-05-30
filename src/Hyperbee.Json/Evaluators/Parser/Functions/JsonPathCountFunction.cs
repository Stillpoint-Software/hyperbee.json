using System.Linq.Expressions;
using System.Reflection;

namespace Hyperbee.Json.Evaluators.Parser.Functions;

public class JsonPathCountFunction<TType>( string methodName, IList<string> arguments, ParseExpressionContext<TType> context ) : ParserExpressionFunction<TType>( methodName, arguments, context )
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

        return Expression.Convert( Expression.Call(
            CountMethod,
            Expression.Call( JsonPathHelper<TType>.SelectMethod,
                context.Current,
                context.Root,
                queryExp,
                evaluatorExp ) ), typeof( float ) );
    }
}
