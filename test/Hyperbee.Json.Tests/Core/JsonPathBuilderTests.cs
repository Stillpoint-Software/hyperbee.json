using System.Text.Json;
using Hyperbee.Json.Core;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Core;

[TestClass]
public class JsonPathBuilderTests : JsonTestBase
{
    [TestMethod]
    [DataRow( "$['store']['book'][0]['author']", "$.store.book[0].author" )]
    [DataRow( "$['store']['book'][1]['author']", "$.store.book[1].author" )]
    [DataRow( "$['store']['book'][2]['author']", "$.store.book[2].author" )]
    [DataRow( "$['store']['book'][3]['author']", "$.store.book[3].author" )]
    [DataRow( "$['store']['book'][0]['category']", "$.store.book[0].category" )]
    [DataRow( "$['store']['book'][1]['title']", "$.store.book[1].title" )]
    [DataRow( "$['store']['book'][2]['isbn']", "$.store.book[2].isbn" )]
    [DataRow( "$['store']['book'][3]['price']", "$.store.book[3].price" )]
    [DataRow( "$['store']['bicycle']['color']", "$.store.bicycle.color" )]
    [DataRow( "$['store']['bicycle']['price']", "$.store.bicycle.price" )]
    public void GetPath( string pointer, string expected )
    {
        var source = GetDocument<JsonDocument>();
        var target = source.RootElement.FromJsonPathPointer( pointer );

        var builder = new JsonPathBuilder( source );
        var result = builder.GetPath( target );

        Assert.AreEqual( result, expected );
    }
}
