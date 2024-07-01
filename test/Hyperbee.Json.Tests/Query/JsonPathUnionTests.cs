using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Query;

[TestClass]
public class JsonPathUnionTests : JsonTestBase
{
    [DataTestMethod]
    [DataRow( "$[0,0]", typeof( JsonDocument ) )]
    [DataRow( "$[0,0]", typeof( JsonNode ) )]
    public void UnionWithDuplicationFromArray( string query, Type sourceType )
    {
        const string json = """
        [
          "a"
        ]
        """;
        var source = GetDocumentFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.FromJsonPathPointer( "$[0]" ),
            source.FromJsonPathPointer( "$[0]" )
        };

        // consensus: ["a", "a"]

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$['a','a']", typeof( JsonDocument ) )]
    [DataRow( "$['a','a']", typeof( JsonNode ) )]
    public void UnionWithDuplicationFromObject( string query, Type sourceType )
    {
        const string json = """
        {
          "a": 1
        }
        """;
        var source = GetDocumentFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.FromJsonPathPointer( "$.a" ),
            source.FromJsonPathPointer( "$.a" )
        };

        // no consensus

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[?(@.key<3),?(@.key>6)]", typeof( JsonDocument ) )]
    [DataRow( "$[?(@.key<3),?(@.key>6)]", typeof( JsonNode ) )]
    [DataRow( "$[?@.key<3,?@.key>6]", typeof( JsonDocument ) )]
    [DataRow( "$[?@.key<3,?@.key>6]", typeof( JsonNode ) )]
    public void UnionWithFilter( string query, Type sourceType )
    {
        const string json = """
        [
          { "key": 1 },
          { "key": 8 },
          { "key": 3 },
          { "key": 10 },
          { "key": 7 },
          { "key": 2 },
          { "key": 6 },
          { "key": 4 }
        ]
        """;
        var source = GetDocumentFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.FromJsonPathPointer( "$[0]" ), // key: 1
            source.FromJsonPathPointer( "$[5]" ), // key: 2

            source.FromJsonPathPointer( "$[1]" ), // key: 8 
            source.FromJsonPathPointer( "$[3]" ), // key: 10
            source.FromJsonPathPointer( "$[4]" ) // key: 7 
        };

        // no consensus

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$['key','another']", typeof( JsonDocument ) )]
    [DataRow( "$['key','another']", typeof( JsonNode ) )]
    public void UnionWithKeys( string query, Type sourceType )
    {
        const string json = """
        {
          "key": "value",
          "another": "entry"
        }
        """;
        var source = GetDocumentFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.FromJsonPathPointer( "$.key" ),
            source.FromJsonPathPointer( "$.another" )
        };

        // consensus: ["value", "entry"]

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$['key','another','thing1']", typeof( JsonDocument ) )]
    [DataRow( "$['key','another','thing1']", typeof( JsonNode ) )]
    public void UnionWithMultipleKeys( string query, Type sourceType )
    {
        const string json = """
        {
          "key": "value",
          "another": "entry",
          "thing1": "thing2"
        }
        """;
        var source = GetDocumentFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.FromJsonPathPointer( "$.key" ),
            source.FromJsonPathPointer( "$.another" ),
            source.FromJsonPathPointer( "$.thing1" )
        };

        // consensus: ["value", "entry"]

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$..['c','d']", typeof( JsonDocument ) )]
    [DataRow( "$..['c','d']", typeof( JsonNode ) )]
    public void UnionWithKeysAfterRecursiveDescent( string query, Type sourceType )
    {
        const string json = """
        [
          {
            "c": "cc1",
            "d": "dd1",
            "e": "ee1"
          },
          {
            "c": "cc2",
            "child": {
              "d": "dd2"
             }
          },
          {
            "c": "cc3"
          },
          {
            "d": "dd4"
          },
          {
            "child": {
              "c": "cc5"
            }
          }
        ]
        """;

        var source = GetDocumentFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.FromJsonPathPointer( "$[0].c" ),
            source.FromJsonPathPointer( "$[0].d" ),
            source.FromJsonPathPointer( "$[1].c" ),
            source.FromJsonPathPointer( "$[1].child.d" ),
            source.FromJsonPathPointer( "$[2].c" ),
            source.FromJsonPathPointer( "$[3].d" ),
            source.FromJsonPathPointer( "$[4].child.c" )

        };

        // consensus: ["cc1", "cc2", "cc3", "cc5", "dd1", "dd2", "dd4"]
        // consensus: any order

        var equals = matches.SequenceEqual( expected );

        Assert.IsTrue( equals );
    }
}
