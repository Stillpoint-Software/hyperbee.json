using System;
using System.Text.Json;
using Hyperbee.Json.Dynamic;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Dynamic;

[TestClass]
public class JsonDynamicTests : JsonTestBase
{
    static readonly JsonSerializerOptions SerializerOptions = new() { Converters = { new DynamicJsonConverter() } };

    private enum Thing
    {
        ThisThing,
        ThatThing
    }

    [TestMethod]
    public void DynamicHelperConvert()
    {
        var source = GetDocument<JsonDocument>();
        var element = JsonDynamicHelper.ConvertToDynamic( source );

        var book = element.store.book[0];
        var author = book.author;
        var price = book.price;

        var path = book.Path();

        Assert.IsTrue( price == 8.95 );
        Assert.IsTrue( author == "Nigel Rees" );
        Assert.AreEqual( "$.store.book[0]", path );
    }

    [TestMethod]
    public void DynamicSerializerConverter()
    {
        var jobject = JsonSerializer.Deserialize<dynamic>( ReadJsonString(), SerializerOptions );

        jobject!.store.thing = Thing.ThatThing;

        var output = JsonSerializer.Serialize<dynamic>( jobject, SerializerOptions ) as string;

        Assert.IsTrue( jobject.store.bicycle.color == "red" );
        Assert.IsTrue( jobject.store.thing == Thing.ThatThing );
        Assert.IsNotNull( output );
        Assert.IsTrue( output.Contains( "ThatThing" ) );
    }

    [TestMethod]
    public void DynamicModifyExistingProperty()
    {
        var jobject = JsonSerializer.Deserialize<dynamic>( ReadJsonString(), SerializerOptions );

        jobject.store.book[0].price = 9.99;

        var modifiedPrice = jobject.store.book[0].price;

        Assert.AreEqual( 9.99, modifiedPrice );
    }

    [TestMethod]
    public void DynamicAddNewProperty()
    {
        var jobject = JsonSerializer.Deserialize<dynamic>( ReadJsonString(), SerializerOptions );

        jobject.store.newProperty = "NewValue";

        var newValue = jobject.store.newProperty;

        Assert.AreEqual( "NewValue", newValue );
    }

    [TestMethod]
    public void DynamicArrayAccessOutOfBounds()
    {
        var jobject = JsonSerializer.Deserialize<dynamic>( ReadJsonString(), SerializerOptions );

        Assert.ThrowsException<ArgumentOutOfRangeException>( () =>
        {
            _ = jobject.store.book[10];
        } );
    }
}

