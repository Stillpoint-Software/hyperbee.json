﻿using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Query;

[TestClass]
public class JsonPathArrayTests : JsonTestBase
{
    [DataTestMethod]
    [DataRow( "$[1:3]", typeof( JsonDocument ) )]
    [DataRow( "$[1:3]", typeof( JsonNode ) )]
    public void ArraySlice( string query, Type sourceType )
    {
        //consensus: ["second", "third"]

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[1]"),
            source.GetPropertyFromPath("$[2]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[0:5]", typeof( JsonDocument ) )]
    [DataRow( "$[0:5]", typeof( JsonNode ) )]
    public void ArraySliceOnExactMatch( string query, Type sourceType )
    {
        //consensus: ["first", "second", "third", "forth", "fifth"]

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[0]"),
            source.GetPropertyFromPath("$[1]"),
            source.GetPropertyFromPath("$[2]"),
            source.GetPropertyFromPath("$[3]"),
            source.GetPropertyFromPath("$[4]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[7:10]", typeof( JsonDocument ) )]
    [DataRow( "$[7:10]", typeof( JsonNode ) )]
    public void ArraySliceOnNonOverlappingArray( string query, Type sourceType )
    {
        //consensus: []

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[1:3]", typeof( JsonDocument ) )]
    [DataRow( "$[1:3]", typeof( JsonNode ) )]
    public void ArraySliceOnObject( string query, Type sourceType )
    {
        //consensus: []

        const string json = """
        {
            ":": 42,
            "more": "string",
            "a": 1,
            "b": 2,
            "c": 3,
            "1:3": "nice"
        }
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[1:10]", typeof( JsonDocument ) )]
    [DataRow( "$[1:10]", typeof( JsonNode ) )]
    public void ArraySliceOnPartiallyOverlappingArray( string query, Type sourceType )
    {
        //consensus: ["second", "third"]

        const string json = """
        [
            "first",
            "second",
            "third"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[1]"),
            source.GetPropertyFromPath("$[2]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[2:113667776004]", typeof( JsonDocument ) )]
    [DataRow( "$[2:113667776004]", typeof( JsonNode ) )]
    public void ArraySliceWithLargeNumberForEnd( string query, Type sourceType )
    {
        //consensus: ["third", "forth", "fifth"]

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[2]"),
            source.GetPropertyFromPath("$[3]"),
            source.GetPropertyFromPath("$[4]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[2:-113667776004:-1]", typeof( JsonDocument ) )]
    [DataRow( "$[2:-113667776004:-1]", typeof( JsonNode ) )]
    public void ArraySliceWithLargeNumberForEndAndNegativeStep( string query, Type sourceType )
    {
        //consensus: //none
        //implementation: ["third","second","first"] //rfc

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[2]"),
            source.GetPropertyFromPath("$[1]"),
            source.GetPropertyFromPath("$[0]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[-113667776004:2]", typeof( JsonDocument ) )]
    [DataRow( "$[-113667776004:2]", typeof( JsonNode ) )]
    public void ArraySliceWithLargeNumberForStart( string query, Type sourceType )
    {
        //consensus: ["first", "second"]

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[0]"),
            source.GetPropertyFromPath("$[1]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[113667776004:2:-1]", typeof( JsonDocument ) )]
    [DataRow( "$[113667776004:2:-1]", typeof( JsonNode ) )]
    public void ArraySliceWithLargeNumberForStartAndNegativeStep( string query, Type sourceType )
    {
        //consensus: [] //partial
        //deviation: ["fifth","forth"]  //rfc

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[4]"),
            source.GetPropertyFromPath("$[3]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[-4:-5]", typeof( JsonDocument ) )]
    [DataRow( "$[-4:-5]", typeof( JsonNode ) )]
    public void ArraySliceWithNegativeStartAndEndAndRangeOfNegative1( string query, Type sourceType )
    {
        //consensus: []

        const string json = """
        [
            2,
            "a",
            4,
            5,
            100,
            "nice"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[-4:-4]", typeof( JsonDocument ) )]
    [DataRow( "$[-4:-4]", typeof( JsonNode ) )]
    public void ArraySliceWithNegativeStartAndEndAndRangeOf0( string query, Type sourceType )
    {
        //consensus: []

        const string json = """
        [
            2,
            "a",
            4,
            5,
            100,
            "nice"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[-4:-3]", typeof( JsonDocument ) )]
    [DataRow( "$[-4:-3]", typeof( JsonNode ) )]
    public void ArraySliceWithNegativeStartAndEndAndRangeOf1( string query, Type sourceType )
    {
        //consensus: [4]

        const string json = """
        [
            2,
            "a",
            4,
            5,
            100,
            "nice"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[2]")


        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[-4:1]", typeof( JsonDocument ) )]
    [DataRow( "$[-4:1]", typeof( JsonNode ) )]
    public void ArraySliceWithNegativeStartAndPositiveEndAndRangeOfNegative1( string query, Type sourceType )
    {
        //consensus: []

        const string json = """
        [
            2,
            "a",
            4,
            5,
            100,
            "nice"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[-4:2]", typeof( JsonDocument ) )]
    [DataRow( "$[-4:2]", typeof( JsonNode ) )]
    public void ArraySliceWithNegativeStartAndPositiveEndAndRangeOf0( string query, Type sourceType )
    {
        //consensus: []

        const string json = """
        [
            2,
            "a",
            4,
            5,
            100,
            "nice"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[-4:3]", typeof( JsonDocument ) )]
    [DataRow( "$[-4:3]", typeof( JsonNode ) )]
    public void ArraySliceWithNegativeStartAndPositiveEndAndRangeOf1( string query, Type sourceType )
    {
        //consensus: [4]

        const string json = """
        [
            2,
            "a",
            4,
            5,
            100,
            "nice"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[2]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[3:0:-2]", typeof( JsonDocument ) )]
    [DataRow( "$[3:0:-2]", typeof( JsonNode ) )]
    public void ArraySliceWithNegativeStep( string query, Type sourceType )
    {
        //consensus: //none
        //deviation: ["forth","second"] //rfc

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[3]"),
            source.GetPropertyFromPath("$[1]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[0:3:-2]", typeof( JsonDocument ) )]
    [DataRow( "$[0:3:-2]", typeof( JsonNode ) )]
    public void ArraySliceWithNegativeStepAndStartGreaterThanEnd( string query, Type sourceType )
    {
        //consensus: //none
        //deviation: [] //rfc

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[7:3:-1]", typeof( JsonDocument ) )]
    [DataRow( "$[7:3:-1]", typeof( JsonNode ) )]
    public void ArraySliceWithNegativeStepOnPartiallyOverlappingArray( string query, Type sourceType )
    {
        //consensus: //none
        //deviation: ["fifth"] //rfc

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[4]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[::-2]", typeof( JsonDocument ) )]
    [DataRow( "$[::-2]", typeof( JsonNode ) )]
    public void ArraySliceWithNegativeStepOnly( string query, Type sourceType )
    {
        //consensus: //none
        //deviation: ["fifth","third","first"] //rfc

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[4]"),
            source.GetPropertyFromPath("$[2]"),
            source.GetPropertyFromPath("$[0]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[1:]", typeof( JsonDocument ) )]
    [DataRow( "$[1:]", typeof( JsonNode ) )]
    public void ArraySliceWithOpenEnd( string query, Type sourceType )
    {
        //consensus: ["second", "third", "forth", "fifth"]

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[1]"),
            source.GetPropertyFromPath("$[2]"),
            source.GetPropertyFromPath("$[3]"),
            source.GetPropertyFromPath("$[4]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[3::-1]", typeof( JsonDocument ) )]
    [DataRow( "$[3::-1]", typeof( JsonNode ) )]
    public void ArraySliceWithOpenEndAndNegativeStep( string query, Type sourceType )
    {
        //consensus: //none
        //deviation: ["forth","third","second","first"] //rfc

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[3]"),
            source.GetPropertyFromPath("$[2]"),
            source.GetPropertyFromPath("$[1]"),
            source.GetPropertyFromPath("$[0]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[:2]", typeof( JsonDocument ) )]
    [DataRow( "$[:2]", typeof( JsonNode ) )]
    public void ArraySliceWithOpenStart( string query, Type sourceType )
    {
        //consensus: ["first", "second"]

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[0]"),
            source.GetPropertyFromPath("$[1]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[::]", typeof( JsonDocument ) )]
    [DataRow( "$[::]", typeof( JsonNode ) )]
    public void ArraySliceWithOpenStartAndEndAndStepEmpty( string query, Type sourceType )
    {
        //consensus: ["first", "second"]

        const string json = """
        [
            "first",
            "second"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[0]"),
            source.GetPropertyFromPath("$[1]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[:]", typeof( JsonDocument ) )]
    [DataRow( "$[:]", typeof( JsonNode ) )]
    public void ArraySliceWithOpenStartAndEndOnObject( string query, Type sourceType )
    {
        //consensus: []

        const string json = """
        {
            ":": 42,
            "more": "string"
        }
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[:2:-1]", typeof( JsonDocument ) )]
    [DataRow( "$[:2:-1]", typeof( JsonNode ) )]
    public void ArraySliceWithOpenStartAndNegativeStep( string query, Type sourceType )
    {
        //consensus: //none
        //deviation: ["fifth","forth"] //rfc

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[4]"),
            source.GetPropertyFromPath("$[3]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[3:-4]", typeof( JsonDocument ) )]
    [DataRow( "$[3:-4]", typeof( JsonNode ) )]
    public void ArraySliceWithPositiveStartAndNegativeEndAndRangeOfNegative1( string query, Type sourceType )
    {
        //consensus: []

        const string json = """
        [
            2,
            "a",
            4,
            5,
            100,
            "nice"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[3:-3]", typeof( JsonDocument ) )]
    [DataRow( "$[3:-3]", typeof( JsonNode ) )]
    public void ArraySliceWithPositiveStartAndNegativeEndAndRangeOf0( string query, Type sourceType )
    {
        //consensus: []

        const string json = """
        [
            2,
            "a",
            4,
            5,
            100,
            "nice"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[3:-2]", typeof( JsonDocument ) )]
    [DataRow( "$[3:-2]", typeof( JsonNode ) )]
    public void ArraySliceWithPositiveStartAndNegativeEndAndRangeOf1( string query, Type sourceType )
    {
        //consensus: [5]

        const string json = """
        [
            2,
            "a",
            4,
            5,
            100,
            "nice"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[3]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[2:1]", typeof( JsonDocument ) )]
    [DataRow( "$[2:1]", typeof( JsonNode ) )]
    public void ArraySliceWithRangeOfNegative1( string query, Type sourceType )
    {
        //consensus: []

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[0:0]", typeof( JsonDocument ) )]
    [DataRow( "$[0:0]", typeof( JsonNode ) )]
    public void ArraySliceWithRangeOf0( string query, Type sourceType )
    {
        //consensus: []

        const string json = """
        [
            "first",
            "second"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[0:1]", typeof( JsonDocument ) )]
    [DataRow( "$[0:1]", typeof( JsonNode ) )]
    public void ArraySliceWithRangeOf1( string query, Type sourceType )
    {
        //consensus: ["first"]

        const string json = """
        [
            "first",
            "second"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[0]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[-1:]", typeof( JsonDocument ) )]
    [DataRow( "$[-1:]", typeof( JsonNode ) )]
    public void ArraySliceWithStartNegative1AndOpenEnd( string query, Type sourceType )
    {
        //consensus: ["third"]

        const string json = """
        [
            "first",
            "second",
            "third"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[2]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[-2:]", typeof( JsonDocument ) )]
    [DataRow( "$[-2:]", typeof( JsonNode ) )]
    public void ArraySliceWithStartMinus2AndOpenEnd( string query, Type sourceType )
    {
        //consensus: ["second", "third"]

        const string json = """
        [
            "first",
            "second",
            "third"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[1]"),
            source.GetPropertyFromPath("$[2]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[-4:]", typeof( JsonDocument ) )]
    [DataRow( "$[-4:]", typeof( JsonNode ) )]
    public void ArraySliceWithStartLargeNegativeNumberAndOpenEndOnShortArray( string query, Type sourceType )
    {
        //consensus: ["first", "second", "third"]

        const string json = """
        [
            "first",
            "second",
            "third"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[0]"),
            source.GetPropertyFromPath("$[1]"),
            source.GetPropertyFromPath("$[2]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[0:3:2]", typeof( JsonDocument ) )]
    [DataRow( "$[0:3:2]", typeof( JsonNode ) )]
    public void ArraySliceWithStep( string query, Type sourceType )
    {
        //consensus: ["first", "third"]

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[0]"),
            source.GetPropertyFromPath("$[2]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[0:3:0]", typeof( JsonDocument ) )]
    [DataRow( "$[0:3:0]", typeof( JsonNode ) )]
    public void ArraySliceWithStep0( string query, Type sourceType )
    {
        //consensus: //none
        //deviation: [] //rfc

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = source.ArrayEmpty;

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[0:3:1]", typeof( JsonDocument ) )]
    [DataRow( "$[0:3:1]", typeof( JsonNode ) )]
    public void ArraySliceWithStep1( string query, Type sourceType )
    {
        //consensus: ["first", "second", "third"]

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[0]"),
            source.GetPropertyFromPath("$[1]"),
            source.GetPropertyFromPath("$[2]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[010:024:010]", typeof( JsonDocument ) )]
    [DataRow( "$[010:024:010]", typeof( JsonNode ) )]
    public void ArraySliceWithStepAndLeadingZeros( string query, Type sourceType )
    {
        //consensus: [10, 20]

        const string json = "[ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25]";
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[10]"),
            source.GetPropertyFromPath("$[20]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[0:4:2]", typeof( JsonDocument ) )]
    [DataRow( "$[0:4:2]", typeof( JsonNode ) )]
    public void ArraySliceWithStepButEndNotAligned( string query, Type sourceType )
    {
        //consensus: ["first", "third"]

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[0]"),
            source.GetPropertyFromPath("$[2]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$[1:3:]", typeof( JsonDocument ) )]
    [DataRow( "$[1:3:]", typeof( JsonNode ) )]
    public void ArraySliceWithStepEmpty( string query, Type sourceType )
    {
        //consensus: ["second", "third"]

        const string json = """
        [
            "first",
            "second",
            "third",
            "forth",
            "fifth"
        ]
        """;
        var source = GetDocumentProxyFromSource( sourceType, json );

        var matches = source.Select( query );
        var expected = new[]
        {
            source.GetPropertyFromPath("$[1]"),
            source.GetPropertyFromPath("$[2]")
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }
}
