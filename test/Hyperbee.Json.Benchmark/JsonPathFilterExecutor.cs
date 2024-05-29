using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Evaluators;
using Hyperbee.Json.Evaluators.Parser;

namespace Hyperbee.Json.Benchmark;

public class JsonPathFilterExecutor
{
    [Benchmark]
    public void JsonPathFilterParser_JsonElement()
    {
        for ( int i = 0; i < 10_000; i++ )
        {
            JsonElementTest.Parse();
        }
    }

    [Benchmark]
    public void JsonPathFilterParser_JsonNode()
    {
        for ( int i = 0; i < 10_000; i++ )
        {
            JsonNodeTest.Parse();
        }
    }
}

public static class JsonElementTest
{
    public static JsonPathExpressionElementEvaluator Evaluator = new();
    public static Expression Param = Expression.Parameter( typeof( JsonElement ) );
    public const string Filter = "(\"world\" == 'world') && (true || false)";

    public static void Parse()
    {
        JsonPathExpression.Parse( Filter, Param, Param, Evaluator );
    }
}

public static class JsonNodeTest
{
    public static JsonPathExpressionNodeEvaluator Evaluator = new();
    public static Expression Param = Expression.Parameter( typeof( JsonNode ) );
    public const string Filter = "(\"world\" == 'world') && (true || false)";

    public static void Parse()
    {
        JsonPathExpression.Parse( Filter, Param, Param, Evaluator );
    }
}
