using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Evaluators.Parser;
using Microsoft.CSharp.RuntimeBinder;

namespace Hyperbee.Json.Evaluators;

public abstract class JsonPathExpressionEvaluator<TType> : IJsonPathFilterEvaluator<TType>
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly ConcurrentDictionary<string, Func<TType, TType, bool>> Compiled = new();

    public object Evaluator( string filter, TType current, TType root )
    {
        var compiled = Compiled.GetOrAdd( filter, _ => JsonPathExpression.Compile( filter, this ) );

        try
        {
            return compiled( current, root );
        }
        catch ( RuntimeBinderException )
        {
            return null; // missing members should act falsy
        }
        catch ( Exception ex )
        {
            throw new JsonPathEvaluatorException( "Error compiling JsonPath expression.", ex );
        }
    }
}

public class JsonPathExpressionElementEvaluator : JsonPathExpressionEvaluator<JsonElement>;
public class JsonPathExpressionNodeEvaluator : JsonPathExpressionEvaluator<JsonNode>;
