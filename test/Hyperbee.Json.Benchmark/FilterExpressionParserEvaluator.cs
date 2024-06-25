﻿using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using Hyperbee.Json.Descriptors.Element;
using Hyperbee.Json.Descriptors.Node;
using Hyperbee.Json.Filters.Parser;
using Expression = System.Linq.Expressions.Expression;

namespace Hyperbee.Json.Benchmark;

public class FilterExpressionParserEvaluator
{
    private FilterContext _nodeExecutionContext;
    private FilterContext _elementExecutionContext;

    [Params( "(\"world\" == 'world') && (true || false)" )]
    public string Filter;

    [GlobalSetup]
    public void Setup()
    {
        _nodeExecutionContext = new FilterContext(
            Expression.Parameter( typeof( JsonNode ) ),
            Expression.Parameter( typeof( JsonNode ) ),
            new NodeTypeDescriptor() );

        _elementExecutionContext = new FilterContext(
            Expression.Parameter( typeof( JsonElement ) ),
            Expression.Parameter( typeof( JsonElement ) ),
            new ElementTypeDescriptor() );
    }

    [Benchmark]
    public void JsonPathFilterParser_JsonElement()
    {
        FilterParser.Parse( Filter, _elementExecutionContext );
    }

    [Benchmark]
    public void JsonPathFilterParser_JsonNode()
    {
        FilterParser.Parse( Filter, _nodeExecutionContext );
    }
}
