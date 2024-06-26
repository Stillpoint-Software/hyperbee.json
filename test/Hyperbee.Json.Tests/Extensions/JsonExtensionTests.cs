﻿using System.Text.Json;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Extensions;

[TestClass]
public class JsonExtensionTests : JsonTestBase
{
    public struct TestItem
    {
        public string A { get; set; }
        public string B { get; set; }
    }

    [TestMethod]
    public void Should_serialize_json_element_to_object()
    {
        // arrange
        var source = new TestItem
        {
            A = "a",
            B = "b"
        };

        var json = JsonSerializer.Serialize( source );
        var document = JsonDocument.Parse( json );

        // act
        var result = document.RootElement.ToObject<TestItem>();

        // assert
        Assert.AreEqual( source, result );
    }

    [TestMethod]
    public void Should_return_property_value_for_property_path()
    {
        // arrange
        const string json = """
        {
            "message": "The operation was successful",
            "status": 200,
            "timestamp": {
                "$date": "2021-07-24T20:14:06.613Z"
            },
            "assets": [
            {
                "hash": "22e1ea7a1c694262159271851eb6cff001fb39bf8e5edc795a345a771b2c3ffc",
                "owners": [],
                "asset": {
                    "code": "#load"
                },
                "votes": []
            }
            ]
        }
        """;

        var document = JsonDocument.Parse( json );

        // act
        var result = document.RootElement.GetPropertyFromPath( "assets[0].asset.['code']" ).GetString();

        // asset
        Assert.AreEqual( "#load", result );
    }
}
