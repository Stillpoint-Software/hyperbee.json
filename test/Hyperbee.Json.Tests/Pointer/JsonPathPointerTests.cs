using System.Text.Json;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Pointer;

[TestClass]
public class JsonPathPointerTests : JsonTestBase
{
    [TestMethod]
    [DataRow( "$.message", "The operation was successful" )]
    [DataRow( "$.status", 200 )]
    [DataRow( "$.timestamp['$date']", "2021-07-24T20:14:06.613Z" )]
    [DataRow( "$.assets[0].hash", "22e1ea7a1c694262159271851eb6cff001fb39bf8e5edc795a345a771b2c3ffc" )]
    [DataRow( "$.assets[0].asset['code']", "#load" )]
    public void ValueFromJsonPathPointer( string pointer, object expected )
    {
        // arrange
        const string json =
            """
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
        var target = document.RootElement.FromJsonPathPointer( pointer );

        object result = target.ValueKind switch
        {
            JsonValueKind.String => target.GetString(),
            JsonValueKind.Number => target.GetInt32(),
            _ => target.GetRawText()
        };

        // assert
        Assert.AreEqual( expected, result );
    }
}
