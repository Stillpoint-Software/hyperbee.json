using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class CountNodeFunction( string methodName, ParseExpressionContext context ) :
    FilterExtensionFunction( methodName, 1, context )
{
    public const string Name = "count";

    private static readonly MethodInfo CountMethod;

    static CountNodeFunction()
    {
        CountMethod = typeof( CountNodeFunction ).GetMethod( nameof( Count ), [typeof( IEnumerable<JsonNode> )] );
    }

    public override Expression GetExtensionExpression( string methodName, Expression[] arguments, ParseExpressionContext context )
    {
        if ( arguments.Length != 1 )
        {
            return Expression.Throw( Expression.Constant( new Exception( $"Invalid use of {Name} function" ) ) );
        }

        return Expression.Call( CountMethod, arguments[0] );
    }

    public static float Count( IEnumerable<JsonNode> elements )
    {
        return elements.Count();
    }
}
