using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Nodes;
using Hyperbee.Json.Filters.Parser;

namespace Hyperbee.Json.Descriptors.Node.Functions;

public class CountNodeFunction( string methodName, IList<string> arguments, ParseExpressionContext context ) :
    FilterExtensionFunction( methodName, arguments, context )
{
    public const string Name = "count";

    private static readonly MethodInfo CountMethod;

    static CountNodeFunction()
    {
        CountMethod = typeof( Enumerable )
            .GetMethods( BindingFlags.Static | BindingFlags.Public )
            .First( m =>
                m.Name == "Count" &&
                m.GetParameters().Length == 1 &&
                m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof( IEnumerable<> ) )
            .MakeGenericMethod( typeof( JsonNode ) );
    }

    public override Expression GetExtensionExpression( string methodName, IList<string> arguments, ParseExpressionContext context )
    {
        if ( arguments.Count != 1 )
        {
            return Expression.Throw( Expression.Constant( new Exception( $"Invalid use of {Name} function" ) ) );
        }

        var queryExp = Expression.Constant( arguments[0] );

        return Expression.Convert( Expression.Call(
                CountMethod,
                Expression.Call( FilterNodeHelper.SelectElementsMethod,
                    context.Current,
                    context.Root,
                    queryExp ) )
            , typeof( float ) );
    }
}
