using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Query;

[TestClass]
public class JsonPathBracketNotationTests : JsonTestBase
{
    [DataTestMethod]
    [DataRow( "$['key']", typeof(JsonDocument))]
    [DataRow( "$['key']", typeof(JsonNode))]
    public void BracketNotation( string query, Type sourceType )
    {
        //consensus: ["value"]

        const string json = "{\"key\": \"value\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$['key']" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$..[0]", typeof(JsonDocument))]
    [DataRow( "$..[0]", typeof(JsonNode))]
    public void BracketNotationAfterRecursiveDescent( string query, Type sourceType )
    {
        //consensus: ["deepest", "first nested", "first", "more", {"nested": ["deepest", "second"]}]
        //deviation: consensus results/different order //rfc in selector order

        const string json = "[\"first\", {\"key\": [\"first nested\", {\"more\": [{\"nested\": [\"deepest\", \"second\"]}, [\"more\", \"values\"]]}]}]";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$[0]" ),
            source.GetPropertyFromKey( "$[1]['key'][0]" ),
            source.GetPropertyFromKey( "$[1]['key'][1]['more'][0]" ),
            source.GetPropertyFromKey( "$[1]['key'][1]['more'][0]['nested'][0]" ),
            source.GetPropertyFromKey( "$[1]['key'][1]['more'][1][0]" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$['missing']", typeof(JsonDocument))]
    [DataRow( "$['missing']", typeof(JsonNode))]
    public void BracketNotationOnObjectWithoutKey( string query, Type sourceType )
    {
        //consensus: []

        const string json = "{\"key\": \"value\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$['ü']", typeof(JsonDocument))]
    [DataRow( "$['ü']", typeof(JsonNode))]
    public void BracketNotationWithNFCPathOnNFDKey( string query, Type sourceType )
    {
        //consensus: []

        const string json = "{\"u\\u0308\": 42}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$['two.some']", typeof(JsonDocument))]
    [DataRow( "$['two.some']", typeof(JsonNode))]
    public void BracketNotationWithDot( string query, Type sourceType )
    {
        //consensus: //none
        //deviation: "42" //support bracket notation on objects

        const string json = "{\"one\": {\"key\": \"value\"}, \"two\": {\"some\": \"more\", \"key\": \"other value\"}, \"two.some\": \"42\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );
        var matches = source.Select( query ).ToList();

        Assert.IsTrue( matches.Count == 1 );
        Assert.IsTrue( JsonValueHelper.GetString( matches[0] ) == "42");
    }

    [DataTestMethod]
    [DataRow( "$[\"key\"]", typeof(JsonDocument))]
    [DataRow( "$[\"key\"]", typeof(JsonNode))]
    public void BracketNotationWithDoubleQuotes( string query, Type sourceType )
    {
        //consensus: ["value"]

        const string json = "{\"key\": \"value\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );
        var matches = source.Select( query ).ToList();

        Assert.IsTrue( matches.Count == 1 );
        Assert.IsTrue( JsonValueHelper.GetString( matches[0] ) == "value" );
    }

    [DataTestMethod]
    [DataRow( "$[]", typeof(JsonDocument))]
    [DataRow( "$[]", typeof(JsonNode))]
    public void BracketNotationWithEmptyPath( string query, Type sourceType )
    {
        //consensus: NOT_SUPPORTED

        const string json = "{\"\": 42, \"''\": 123, \"\\\"\\\"\": 222}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        Assert.ThrowsException<NotSupportedException>( () =>
        {
            var _ = source.Select( query ).ToList();

        }, "Invalid bracket expression syntax. Bracket expression cannot be empty." );
    }

    [DataTestMethod]
    [DataRow( "$['']", typeof(JsonDocument))]
    [DataRow( "$['']", typeof(JsonNode))]
    public void BracketNotationWithEmptyString( string query, Type sourceType )
    {
        //consensus: [42]

        const string json = "{\"\": 42, \"''\": 123, \"\\\"\\\"\": 222}";
        var source = GetDocumentProxyFromSource( sourceType, json );
        var matches = source.Select( query ).ToList();

        Assert.IsTrue( matches.Count == 1 );
        Assert.IsTrue( JsonValueHelper.GetInt32( matches[0] ) == 42 );
    }

    [DataTestMethod]
    [DataRow( "$[\"\"]", typeof(JsonDocument))]
    [DataRow( "$[\"\"]", typeof(JsonNode))]
    public void BracketNotationWithEmptyStringDoubleQuoted( string query, Type sourceType )
    {
        //consensus: [42]

        const string json = "{\"\": 42, \"''\": 123, \"\\\"\\\"\": 222}";
        var source = GetDocumentProxyFromSource( sourceType, json );
        var matches = source.Select( query ).ToList();

        Assert.IsTrue( matches.Count == 1 );
        Assert.IsTrue( JsonValueHelper.GetInt32( matches[0] ) == 42 );
    }

    [DataTestMethod]
    [DataRow( "$[-2]", typeof(JsonDocument))]
    [DataRow( "$[-2]", typeof(JsonNode))]
    public void BracketNotationWithNegativeNumberOnShortArray( string query, Type sourceType )
    {
        //consensus: []
        //deviation: ["one element] //rfc (-2 => -2:1:1 => -1:1:1 [0])

        const string json = "[\"one element\"]";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query ).ToList();

        Assert.IsTrue( matches.Count == 1 );
        Assert.IsTrue( JsonValueHelper.GetString( matches[0] ) == "one element" );
    }

    [DataTestMethod]
    [DataRow( "$[2]", typeof(JsonDocument))]
    [DataRow( "$[2]", typeof(JsonNode))]
    public void BracketNotationWithNumber( string query, Type sourceType )
    {
        //consensus: ["third"]

        const string json = "[\"first\", \"second\", \"third\", \"forth\", \"fifth\"]";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query ).ToList();

        Assert.IsTrue( matches.Count == 1 );
        Assert.IsTrue( JsonValueHelper.GetString( matches[0] ) == "third" );
    }

    [DataTestMethod]
    [DataRow( "$[-1]", typeof(JsonDocument))]
    [DataRow( "$[-1]", typeof(JsonNode))]
    public void BracketNotationWithNumberNegative1( string query, Type sourceType )
    {
        //consensus: ["third"]

        const string json = "[\"first\", \"second\", \"third\"]";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query ).ToList();

        Assert.IsTrue( matches.Count == 1 );
        Assert.IsTrue( JsonValueHelper.GetString( matches[0] ) == "third" );
    }

    [DataTestMethod]
    [DataRow( "$[-1]", typeof(JsonDocument))]
    [DataRow( "$[-1]", typeof(JsonNode))]
    public void BracketNotationWithNumberNegative1OnEmptyArray( string query, Type sourceType )
    {
        //consensus: []

        const string json = "[]";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[0]", typeof(JsonDocument))]
    [DataRow( "$[0]", typeof(JsonNode))]
    public void BracketNotationWithNumber0( string query, Type sourceType )
    {
        //consensus: ["first"]

        const string json = "[\"first\", \"second\", \"third\", \"forth\", \"fifth\"]";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query ).ToList();

        Assert.IsTrue( matches.Count == 1 );
        Assert.IsTrue( JsonValueHelper.GetString( matches[0] ) == "first" );
    }

    [DataTestMethod]
    [DataRow( "$.*[1]", typeof(JsonDocument))]
    [DataRow( "$.*[1]", typeof(JsonNode))]
    public void BracketNotationWithNumberAfterDotNotationWithWildcardOnNestedArraysWithDifferentLength( string query, Type sourceType )
    {
        //consensus: [3]

        const string json = "[[1], [2, 3]]";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$[1][1]" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[0]", typeof(JsonDocument))]
    [DataRow( "$[0]", typeof(JsonNode))]
    public void BracketNotationWithNumberOnObject( string query, Type sourceType )
    {
        //consensus: []
        //deviation: ["value"]

        const string json = "{\"0\": \"value\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$['0']" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[1]", typeof(JsonDocument))]
    [DataRow( "$[1]", typeof(JsonNode))]
    public void BracketNotationWithNumberOnShortArray( string query, Type sourceType )
    {
        //consensus: []

        const string json = "[\"one element\"]";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    /*
    [DataTestMethod]
    [DataRow( "$[0]", typeof(JsonDocument))]
    [DataRow( "$[0]", typeof(JsonNode))]
    public void BracketNotationWithNumberOnString( string query, Type sourceType )
    {
        //consensus: []
        //deviation: NOT_SUPPORTED //JsonDocument can't parse

        const string json = "Hello World";
        var source = GetDocumentProxyFromSource( sourceType, json );
    }
    */

    [DataTestMethod]
    [DataRow( "$[':']", typeof(JsonDocument))]
    [DataRow( "$[':']", typeof(JsonNode))]
    public void BracketNotationWithQuotedArraySliceLiteral( string query, Type sourceType )
    {
        //consensus: ["value"]

        const string json = "{\":\": \"value\", \"another\": \"entry\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query ).ToList();

        Assert.IsTrue( matches.Count == 1 );
        Assert.IsTrue( JsonValueHelper.GetString( matches[0] ) == "value" );
    }

    [DataTestMethod]
    [DataRow( "$[']']", typeof(JsonDocument))]
    [DataRow( "$[']']", typeof(JsonNode))]
    public void BracketNotationWithQuotedClosingBracketLiteral( string query, Type sourceType )
    {
        //consensus: [42]

        const string json = "{\"]\": 42}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query ).ToList();

        Assert.IsTrue( matches.Count == 1 );
        Assert.IsTrue( JsonValueHelper.GetInt32( matches[0] ) == 42 );
    }

    [DataTestMethod]
    [DataRow( "$['@']", typeof(JsonDocument))]
    [DataRow( "$['@']", typeof(JsonNode))]
    public void BracketNotationWithQuotedCurrentObjectLiteral( string query, Type sourceType )
    {
        //consensus: ["value"]

        const string json = "{\"@\": \"value\", \"another\": \"entry\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query ).ToList();

        Assert.IsTrue( matches.Count == 1 );
        Assert.IsTrue( JsonValueHelper.GetString( matches[0] ) == "value" );
    }

    [DataTestMethod]
    [DataRow( "$['.']", typeof(JsonDocument))]
    [DataRow( "$['.']", typeof(JsonNode))]
    public void BracketNotationWithQuotedDotLiteral( string query, Type sourceType )
    {
        //consensus: ["value"]

        const string json = "{\".\": \"value\",\"another\": \"entry\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$['.']" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$['.*']", typeof(JsonDocument))]
    [DataRow( "$['.*']", typeof(JsonNode))]
    public void BracketNotationWithQuotedDotWildcard( string query, Type sourceType )
    {
        //consensus: [1]

        const string json = "{\"key\": 42, \".*\": 1, \"\": 10}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$['.*']" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    /*
    [DataTestMethod]
    [DataRow( "$['\"']", typeof(JsonDocument))]
    [DataRow( "$['\"']", typeof(JsonNode))]
    public void BracketNotationWithQuotedDoubleQuoteLiteral( string query, Type sourceType )
    {
        //consensus: ["value"]
        //deviation: NOT_SUPPORTED //JsonDocument can't parse

        const string json = "{ \"\"\": \"value\", \"another\": \"entry\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );
    }
    */

    [DataTestMethod]
    [DataRow( @"$['\\']", typeof(JsonDocument))]
    [DataRow( @"$['\\']", typeof(JsonNode))]
    public void BracketNotationWithQuotedEscapedBackslash( string query, Type sourceType )
    {
        //consensus: //none
        //deviation: ["value"] 

        const string json = @"{""\\"": ""value""}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( @"$['\']" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$['\\'']", typeof(JsonDocument))]
    [DataRow( "$['\\'']", typeof(JsonNode))]
    public void BracketNotationWithQuotedEscapedSingleQuote( string query, Type sourceType )
    {
        //consensus: //none
        //deviation: ["value"]

        const string json = @"{""'"": ""value""}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query ).ToList();

        Assert.IsTrue( matches.Count == 1 );
        Assert.IsTrue( JsonValueHelper.GetString( matches[0] ) == "value" );
    }

    [DataTestMethod]
    [DataRow( "$['0']", typeof(JsonDocument))]
    [DataRow( "$['0']", typeof(JsonNode))]
    public void BracketNotationWithQuotedNumberOnObject( string query, Type sourceType )
    {
        //consensus: ["value"]

        const string json = "{\"0\": \"value\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$['0']" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$['$']", typeof(JsonDocument))]
    [DataRow( "$['$']", typeof(JsonNode))]
    public void BracketNotationWithQuotedRootLiteral( string query, Type sourceType )
    {
        //consensus: ["value"]

        const string json = "{\"$\": \"value\", \"another\": \"entry\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$['$']" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( @"$[':@.\""$,*\'\\']", typeof(JsonDocument))] // $[':@.\"$,*'\\']
    [DataRow( @"$[':@.\""$,*\'\\']", typeof(JsonNode))] // $[':@.\"$,*'\\']
    public void BracketNotationWithQuotedSpecialCharactersCombined( string query, Type sourceType )
    {
        //consensus: //none
        //deviation: 42

        const string json = @"{"":@.\""$,*'\\"": 42}"; // property name is => :@.\"$,*'\\
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query ).ToList();

        Assert.IsTrue( matches.Count == 1 );
        Assert.IsTrue( JsonValueHelper.GetInt32( matches[0] ) == 42 );
    }

    [DataTestMethod]
    [DataRow( "$['single'quote']", typeof(JsonDocument))]
    [DataRow( "$['single'quote']", typeof(JsonNode))]
    public void BracketNotationWithQuotedStringAndUnescapedSingleQuote( string query, Type sourceType )
    {
        //consensus: NOT_SUPPORTED

        const string json = "{\"single'quote\":\"value\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        Assert.ThrowsException<NotSupportedException>( () =>
        {
            var _ = source.Select( query ).ToList();
        } );
    }

    [DataTestMethod]
    [DataRow( "$[',']", typeof(JsonDocument))]
    [DataRow( "$[',']", typeof(JsonNode))]
    public void BracketNotationWithQuotedUnionLiteral( string query, Type sourceType )
    {
        //consensus: ["value"]

        const string json = "{\",\": \"value\", \"another\": \"entry\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$[',']" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$['*']", typeof(JsonDocument))]
    [DataRow( "$['*']", typeof(JsonNode))]
    public void BracketNotationWithQuotedWildcardLiteral( string query, Type sourceType )
    {
        //consensus: ["value"]

        const string json = "{\"*\": \"value\", \"another\": \"entry\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$['*']" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$['*']", typeof(JsonDocument))]
    [DataRow( "$['*']", typeof(JsonNode))]
    public void BracketNotationWithQuotedWildcardLiteralOnObjectWithoutKey( string query, Type sourceType )
    {
        //consensus: []

        const string json = "{\"another\": \"entry\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[ 'a' ]", typeof(JsonDocument))]
    [DataRow( "$[ 'a' ]", typeof(JsonNode))]
    public void BracketNotationWithSpaces( string query, Type sourceType )
    {
        //consensus: [2]

        const string json = "{\" a\": 1, \"a\": 2, \" a \": 3, \"a \": 4, \" 'a' \": 5, \" 'a\": 6, \"a' \": 7, \" \\\"a\\\" \": 8, \"\\\"a\\\"\": 9}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$['a']" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$['ni.*']", typeof(JsonDocument))]
    [DataRow( "$['ni.*']", typeof(JsonNode))]
    public void BracketNotationWithStringIncludingDotWildcard( string query, Type sourceType )
    {
        //consensus: [1]

        const string json = "{\"nice\": 42, \"ni.*\": 1, \"mice\": 100}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$['ni.*']" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$['two'.'some']", typeof(JsonDocument))]
    [DataRow( "$['two'.'some']", typeof(JsonNode))]
    public void BracketNotationWithTwoLiteralsSeparatedByDot( string query, Type sourceType )
    {
        //consensus: NOT_SUPPORTED

        const string json = "{\"one\": {\"key\": \"value\"},\"two\": {\"some\": \"more\", \"key\": \"other value\"},\"two.some\": \"42\",\"two'.'some\": \"43\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        Assert.ThrowsException<NotSupportedException>( () =>
        {
            var _ = source.Select( query ).ToList();
        } );
    }

    [DataTestMethod]
    [DataRow( "$[two.some]", typeof(JsonDocument))]
    [DataRow( "$[two.some]", typeof(JsonNode))]
    public void BracketNotationWithTwoLiteralsSeparatedByDotWithoutQuotes( string query, Type sourceType )
    {
        //consensus: NOT_SUPPORTED

        const string json = "{\"one\": {\"key\": \"value\"},\"two\": {\"some\": \"more\", \"key\": \"other value\"},\"two.some\": \"42\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        Assert.ThrowsException<NotSupportedException>( () =>
        {
            var _ = source.Select( query ).ToList();
        } );
    }

    [DataTestMethod]
    [DataRow( "$[0:2][*]", typeof(JsonDocument))]
    [DataRow( "$[0:2][*]", typeof(JsonNode))]
    public void BracketNotationWithWildcardAfterArraySlice( string query, Type sourceType )
    {
        //consensus: [1, 2, "a", "b"]

        const string json = "[[1, 2], [\"a\", \"b\"], [0, 0]]";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query ).ToList();
        var expected = new[]
        {
            source.GetPropertyFromKey( "$[0][0]" ),
            source.GetPropertyFromKey( "$[0][1]" ),
            source.GetPropertyFromKey( "$[1][0]" ),
            source.GetPropertyFromKey( "$[1][1]" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
        Assert.IsTrue( JsonValueHelper.GetInt32( matches[0] ) == 1 );
        Assert.IsTrue( JsonValueHelper.GetInt32( matches[1] ) == 2 );
        Assert.IsTrue( JsonValueHelper.GetString( matches[2] ) == "a" );
        Assert.IsTrue( JsonValueHelper.GetString( matches[3] ) == "b" );
    }

    [DataTestMethod]
    [DataRow( "$[*].bar[*]", typeof(JsonDocument))]
    [DataRow( "$[*].bar[*]", typeof(JsonNode))]
    public void BracketNotationWithWildcardAfterDotNotationAfterBracketNotationWithWildcard( string query, Type sourceType )
    {
        //consensus: [42]

        const string json = "[{\"bar\": [42]}]";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$[0]['bar'][0]" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$..[*]", typeof(JsonDocument))]
    [DataRow( "$..[*]", typeof(JsonNode))]
    public void BracketNotationWithWildcardAfterRecursiveDescent( string query, Type sourceType )
    {
        //consensus: ["string", "value", 0, 1, [0, 1], {"complex": "string", "primitives": [0, 1]}]
        //deviation: consensus results/different order //rfc in selector order

        const string json = "{\"key\": \"value\", \"another key\": {\"complex\": \"string\", \"primitives\": [0, 1]}}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query ).ToList();
        var expected = new[]
        {
            source.GetPropertyFromKey( "$['key']" ),
            source.GetPropertyFromKey( "$['another key']" ),
            source.GetPropertyFromKey( "$['another key']['complex']" ),
            source.GetPropertyFromKey( "$['another key']['primitives']" ),
            source.GetPropertyFromKey( "$['another key']['primitives'][0]" ),
            source.GetPropertyFromKey( "$['another key']['primitives'][1]" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
        Assert.IsTrue( JsonValueHelper.GetString( matches[0] ) == "value" );
        Assert.IsTrue( JsonValueHelper.GetString( matches[1], minify: true ) == JsonValueHelper.MinifyJsonString( "{\"complex\": \"string\", \"primitives\": [0,1]}") );
        Assert.IsTrue( JsonValueHelper.GetString( matches[2] ) == "string" );
        Assert.IsTrue( JsonValueHelper.GetString( matches[3], minify: true ) == "[0,1]" );
        Assert.IsTrue( JsonValueHelper.GetInt32( matches[4] ) == 0 );
        Assert.IsTrue( JsonValueHelper.GetInt32( matches[5] ) == 1 );
    }

    [DataTestMethod]
    [DataRow( "$[*]", typeof(JsonDocument))]
    [DataRow( "$[*]", typeof(JsonNode))]
    public void BracketNotationWithWildcardOnArray( string query, Type sourceType )
    {
        //consensus: ["string", 42, {"key": "value"}, [0, 1]]

        const string json = "[\"string\", 42, {\"key\": \"value\"}, [0, 1]]";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$[0]" ),
            source.GetPropertyFromKey( "$[1]" ),
            source.GetPropertyFromKey( "$[2]" ),
            source.GetPropertyFromKey( "$[3]" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[*]", typeof(JsonDocument))]
    [DataRow( "$[*]", typeof(JsonNode))]
    public void BracketNotationWithWildcardOnEmptyArray( string query, Type sourceType )
    {
        //consensus: []

        const string json = "[]";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[*]", typeof(JsonDocument))]
    [DataRow( "$[*]", typeof(JsonNode))]
    public void BracketNotationWithWildcardOnEmptyObject( string query, Type sourceType )
    {
        //consensus: []

        const string json = "{}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[*]", typeof(JsonDocument))]
    [DataRow( "$[*]", typeof(JsonNode))]
    public void BracketNotationWithWildcardOnNullValueArray( string query, Type sourceType )
    {
        //consensus: [40, null, 42]

        const string json = "[40, null, 42]";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$[0]" ),
            source.GetPropertyFromKey( "$[1]" ),
            source.GetPropertyFromKey( "$[2]" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[*]", typeof(JsonDocument))]
    [DataRow( "$[*]", typeof(JsonNode))]
    public void BracketNotationWithWildcardOnObject( string query, Type sourceType )
    {
        //consensus: ["string", 42, [0, 1], {"key": "value"}]
        //deviation: consensus results/different order //rfc in selector order

        const string json = "{\"some\": \"string\", \"int\": 42, \"object\": {\"key\": \"value\"}, \"array\": [0, 1]}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromKey( "$['some']" ),
            source.GetPropertyFromKey( "$['int']" ),
            source.GetPropertyFromKey( "$['object']" ),
            source.GetPropertyFromKey( "$['array']" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[key]", typeof(JsonDocument))]
    [DataRow( "$[key]", typeof(JsonNode))]
    public void BracketNotationWithoutQuotes( string query, Type sourceType )
    {
        //consensus: NOT_SUPPORTED

        const string json = "{\"key\": \"value\"}";
        var source = GetDocumentProxyFromSource( sourceType, json );

        Assert.ThrowsException<NotSupportedException>( () =>
        {
            var _ = source.Select( query ).ToList();
        } );
    }
}