using System.Text.Json;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Builder;

[TestClass]
public class JsonPathBuilderTests : JsonTestBase
{
    [DataTestMethod]
    [DataRow( "$['store']['book'][0]['author']", "$.store.book[0].author" )]
    [DataRow( "$['store']['book'][1]['author']", "$.store.book[1].author" )]
    [DataRow( "$['store']['book'][2]['author']", "$.store.book[2].author" )]
    [DataRow( "$['store']['book'][3]['author']", "$.store.book[3].author" )]
    public void Should_GetPath( string key, string expected )
    {
        var source = GetDocument<JsonDocument>();
        var target = source.RootElement.GetPropertyFromPath( key );

        var builder = new JsonPathResolver( source );
        var result = builder.GetPath( target );

        Assert.AreEqual( result, expected );

        var resultCached = builder.GetPath( target );

        Assert.AreEqual( resultCached, expected );
    }
}
