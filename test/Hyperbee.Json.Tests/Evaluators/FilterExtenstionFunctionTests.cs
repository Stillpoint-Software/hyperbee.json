using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Descriptors.Element;
using Hyperbee.Json.Descriptors.Node;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Evaluators;

[TestClass]
public class FilterExtensionFunctionTests : JsonTestBase
{
    [TestMethod]
    public void Should_CallCustomFunction()
    {
        // arrange 
        var source = GetDocument<JsonNode>();

        JsonTypeDescriptorRegistry.GetDescriptor<JsonNode>().Functions
            .Register( PathNodeFunction.Name, () => new PathNodeFunction() );

        // act
        var results = source.Select( "$..[?path(@) == '$.store.book[2].title']" ).ToList();

        // assert
        Assert.IsTrue( results.Count == 1 );
        Assert.AreEqual( "$.store.book[2].title", results[0].GetPath() );
    }
}

public class PathNodeFunction() : FilterExtensionFunction( argumentCount: 1 )
{
    public const string Name = "path";
    private static readonly Expression PathExpression = Expression.Constant( (Func<IEnumerable<JsonNode>, string>) Path );

    protected override Expression GetExtensionExpression( Expression[] arguments )
    {
        return Expression.Invoke( PathExpression, arguments[0] );
    }

    public static string Path( IEnumerable<JsonNode> nodes )
    {
        var node = nodes.FirstOrDefault();
        return node?.GetPath();
    }
}
