using System.Linq.Expressions;

namespace Hyperbee.Json.Evaluators.Parser.Element;

public class ValueElementFunction( string methodName, IList<string> arguments, ParseExpressionContext context ) : FilterExpressionFunction( methodName, arguments, context )
{
    public const string Name = "value";

    public override Expression GetExpression( string methodName, IList<string> arguments, ParseExpressionContext context )
    {
        if ( arguments.Count != 1 )
        {
            return Expression.Throw( Expression.Constant( new ArgumentException( $"{Name} function has invalid parameter count." ) ) );
        }

        var queryExp = Expression.Constant( arguments[0] );

        return Expression.Call(
            FilterElementHelper.SelectFirstElementValueMethod,
            context.Current,
            context.Root,
            queryExp );
    }
}
