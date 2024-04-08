using System.Text.Json;
using Hyperbee.Json.Dynamic;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Dynamic;

[TestClass]
public class JsonDynamicTests : JsonTestBase
{
    private enum Thing
    {
        ThisThing,
        ThatThing
    }

    [TestMethod]
    public void Dynamic_json_element_should_return_correct_results()
    {
        var source = GetDocument<JsonDocument>();
        var element = source.ToDynamic();

        var book = element.store.book[0];
        var author = book.author;
        var price = book.price;

        Assert.IsTrue( price == 8.95 );
        Assert.IsTrue( author == "Nigel Rees" );
    }

    [TestMethod]
    public void Dynamic_json_converter_should_return_correct_results()
    {
        var serializerOptions = new JsonSerializerOptions
        {
            Converters = { new DynamicJsonConverter() }
        };

        var jobject = JsonSerializer.Deserialize<dynamic>( ReadJsonString(), serializerOptions );

        jobject!.store.thing = Thing.ThatThing;

        var output = JsonSerializer.Serialize<dynamic>( jobject, serializerOptions ) as string;

        Assert.IsTrue( jobject.store.bicycle.color == "red" );
        Assert.IsTrue( jobject.store.thing == Thing.ThatThing );
        Assert.IsNotNull( output );
        Assert.IsTrue( output.Contains( "ThatThing" ) );
    }
}

