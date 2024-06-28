﻿using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Query;

[TestClass]
public class JsonPathRecursiveDescentTests : JsonTestBase
{
    [DataTestMethod]
    [DataRow( "$..", typeof( JsonDocument ) )]
    [DataRow( "$..", typeof( JsonNode ) )]
    [ExpectedException( typeof( NotSupportedException ) )]
    public void RecursiveDescent( string query, Type sourceType )
    {
        // no consensus

        const string json = """
        [
            {"a": {"b": "c"}},
            [0, 1]
        ]
        """;

        var source = GetDocumentFromSource( sourceType, json );

        _ = source.Select( query ).ToList();
    }

    [DataTestMethod]
    [DataRow( "$..*", typeof( JsonDocument ) )]
    [DataRow( "$..*", typeof( JsonNode ) )]
    public void RecursiveDescentOnNestedArrays( string query, Type sourceType )
    {
        const string json = """
        [
            [0],
            [1]
        ]
        """;

        var source = GetDocumentFromSource( sourceType, json );

        var matches = source.Select( query ).ToList();
        var expected = new[]
        {
            source.GetPropertyFromPath( "$[0]" ),
            source.GetPropertyFromPath( "$[1]" ),
            source.GetPropertyFromPath( "$[0][0]" ),
            source.GetPropertyFromPath( "$[1][0]" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$.key..", typeof( JsonDocument ) )]
    [DataRow( "$.key..", typeof( JsonNode ) )]
    [ExpectedException( typeof( NotSupportedException ) )]
    public void RecursiveDescentAfterDotNotation( string query, Type sourceType )
    {
        //consensus: NOT_SUPPORTED

        const string json = """
        {
            "some key": "value",
            "key": {
                "complex": "string",
                "primitives": [0, 1]
            }
        }
        """;

        var source = GetDocumentFromSource( sourceType, json );

        _ = source.Select( query ).ToList();
    }

    [DataTestMethod]
    [DataRow( "$..[1].key", typeof( JsonDocument ) )]
    [DataRow( "$..[1].key", typeof( JsonNode ) )]
    public void DotNotationAfterBracketNotationAfterRecursiveDescent( string query, Type sourceType )
    {
        // consensus: [200, 42, 500]

        const string json = """
        {
          "k": [
            {
              "key": "some value"
            },
            {
              "key": 42
            }
          ],
          "kk": [
            [
              {
                "key": 100
              },
              {
                "key": 200
              },
              {
                "key": 300
              }
            ],
            [
              {
                "key": 400
              },
              {
                "key": 500
              },
              {
                "key": 600
              }
            ]
          ],
          "key": [
            0,
            1
          ]
        }
        """;

        var source = GetDocumentFromSource( sourceType, json );

        var matches = source.Select( query ).ToList();
        var expected = new[]
        {
            source.GetPropertyFromPath( "$['k'][1]['key']" ),
            source.GetPropertyFromPath( "$['kk'][0][1]['key']" ),
            source.GetPropertyFromPath( "$['kk'][1][1]['key']" ),
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }
}
