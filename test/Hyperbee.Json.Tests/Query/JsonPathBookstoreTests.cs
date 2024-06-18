using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Query;

[TestClass]
public class JsonPathBookstoreTests : JsonTestBase
{
    [DataTestMethod]
    [DataRow( "$", typeof(JsonDocument) )]
    [DataRow( "$", typeof(JsonNode) )]
    public void TheRootOfEverything( string query, Type sourceType )
    {
        var source = GetDocumentProxy( sourceType );
        var matches = source.Select( query );
        var expected = new[] 
        { 
            source.GetPropertyFromKey( "$" ) 
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$.store.book[*].author", typeof(JsonDocument) )]
    [DataRow( "$.store.book[*].author", typeof(JsonNode) )]
    public void TheAuthorsOfAllBooksInTheStore( string query, Type sourceType )
    {
        var source = GetDocumentProxy( sourceType );
        var matches = source.Select( query );
        var expected = new[] 
        { 
            source.GetPropertyFromKey( "$['store']['book'][0]['author']" ), 
            source.GetPropertyFromKey( "$['store']['book'][1]['author']" ), 
            source.GetPropertyFromKey( "$['store']['book'][2]['author']" ), 
            source.GetPropertyFromKey( "$['store']['book'][3]['author']" ) 
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$..author", typeof(JsonDocument) )]
    [DataRow( "$..author", typeof(JsonNode) )]
    public void AllAuthors( string query, Type sourceType )
    {
        var source = GetDocumentProxy( sourceType );
        var matches = source.Select( query );
        var expected = new[] 
        { 
            source.GetPropertyFromKey( "$['store']['book'][0]['author']" ), 
            source.GetPropertyFromKey( "$['store']['book'][1]['author']" ), 
            source.GetPropertyFromKey( "$['store']['book'][2]['author']" ), 
            source.GetPropertyFromKey( "$['store']['book'][3]['author']" ) 
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$.store.*", typeof(JsonDocument) )]
    [DataRow( "$.store.*", typeof(JsonNode) )]
    public void AllThingsInStoreWhichAreSomeBooksAndOneRedBicycle( string query, Type sourceType )
    {
        var source = GetDocumentProxy( sourceType );
        var matches = source.Select( query );
        var expected = new[] 
        { 
            source.GetPropertyFromKey( "$['store']['book']" ), 
            source.GetPropertyFromKey( "$['store']['bicycle']" )
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$.store..price", typeof(JsonDocument) )]
    [DataRow( "$.store..price", typeof(JsonNode) )]
    public void ThePriceOfEverythingInTheStore( string query, Type sourceType )
    {
        var source = GetDocumentProxy( sourceType );
        var matches = source.Select( query );
        var expected = new[] 
        { 
            source.GetPropertyFromKey( "$['store']['book'][0]['price']" ), 
            source.GetPropertyFromKey( "$['store']['book'][1]['price']" ), 
            source.GetPropertyFromKey( "$['store']['book'][2]['price']" ), 
            source.GetPropertyFromKey( "$['store']['book'][3]['price']" ), 
            source.GetPropertyFromKey( "$['store']['bicycle']['price']" ) 
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$..book[2]", typeof(JsonDocument) )]
    [DataRow( "$..book[2]", typeof(JsonNode) )]
    public void TheThirdBook( string query, Type sourceType )
    {
        var source = GetDocumentProxy( sourceType );
        var match = source.Select( query ).ToList();
        var expected = source.GetPropertyFromKey( "$['store']['book'][2]" );

        Assert.IsTrue( match.Count == 1 );
        Assert.AreEqual( expected, match[0] );
    }

    [DataTestMethod]
    [DataRow( "$..book[-1:]", typeof(JsonDocument) )]
    [DataRow( "$..book[-1:]", typeof(JsonNode) )]
    public void TheLastBookInOrder( string query, Type sourceType )
    {
        var source = GetDocumentProxy( sourceType );
        var match = source.Select( query ).Single();
        var expected = source.GetPropertyFromKey( "$['store']['book'][3]" );

        Assert.AreEqual( expected, match );
    }

    [DataTestMethod]
    [DataRow( "$..book[:2]", typeof(JsonDocument) )]
    [DataRow( "$..book[0,1]", typeof(JsonDocument) )]
    [DataRow( "$.store.book[0,1]", typeof(JsonDocument) )]
    [DataRow( "$..book[:2]", typeof(JsonNode) )]
    [DataRow( "$..book[0,1]", typeof(JsonNode) )]
    [DataRow( "$.store.book[0,1]", typeof(JsonNode) )]
    public void TheFirstTwoBooks( string query, Type sourceType )
    {
        var source = GetDocumentProxy( sourceType );
        var matches = source.Select( query );
        var expected = new[] 
        { 
            source.GetPropertyFromKey( "$['store']['book'][0]" ), 
            source.GetPropertyFromKey( "$['store']['book'][1]" ) 
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$..book['category','author']", typeof(JsonDocument) )]
    [DataRow( "$..book['category','author']", typeof(JsonNode) )]
    public void TheCategoriesAndAuthorsOfAllBooks( string query, Type sourceType )
    {
        var source = GetDocumentProxy( sourceType );
        var matches = source.Select( query );
        var expected = new[] 
        { 
            source.GetPropertyFromKey( "$['store']['book'][0]['category']" ), 
            source.GetPropertyFromKey( "$['store']['book'][1]['category']" ), 
            source.GetPropertyFromKey( "$['store']['book'][2]['category']" ), 
            source.GetPropertyFromKey( "$['store']['book'][3]['category']" ), 
            source.GetPropertyFromKey( "$['store']['book'][0]['author']" ), 
            source.GetPropertyFromKey( "$['store']['book'][1]['author']" ), 
            source.GetPropertyFromKey( "$['store']['book'][2]['author']" ), 
            source.GetPropertyFromKey( "$['store']['book'][3]['author']" ) 
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$..book[?@.isbn]", typeof(JsonDocument) )]
    [DataRow( "$..book[?@.isbn]", typeof(JsonNode) )]
    [DataRow( "$..book[?(@.isbn)]", typeof(JsonDocument) )]
    [DataRow( "$..book[?(@.isbn)]", typeof(JsonNode) )]
    public void FilterAllBooksWithIsbnNumber( string query, Type sourceType )
    {
        var source = GetDocumentProxy( sourceType );
        var matches = source.Select( query );
        var expected = new[] 
        { 
            source.GetPropertyFromKey( "$['store']['book'][2]" ), 
            source.GetPropertyFromKey( "$['store']['book'][3]" ) 
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$..book[?(@.price<10)]", typeof(JsonDocument) )]
    [DataRow( "$..book[?(@.price<10)]", typeof(JsonNode) )]
    [DataRow( "$..book[?@.price<10]", typeof(JsonDocument) )]
    [DataRow( "$..book[?@.price<10]", typeof(JsonNode) )]
    public void FilterAllBooksCheaperThan10( string query, Type sourceType )
    {
        var source = GetDocumentProxy( sourceType );
        var matches = source.Select( query );
        var expected = new[] 
        { 
            source.GetPropertyFromKey( "$['store']['book'][0]" ), 
            source.GetPropertyFromKey( "$['store']['book'][2]" ) 
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( "$..*", typeof(JsonDocument) )]
    [DataRow( "$..*", typeof(JsonNode) )]
    public void AllMembersOfJsonStructure( string query, Type sourceType )
    {
        var source = GetDocumentProxy( sourceType );
        var matches = source.Select( query );
        var expected = new[] 
        { 
            source.GetPropertyFromKey( "$['store']" ), 
            source.GetPropertyFromKey( "$['store']['book']" ), 
            source.GetPropertyFromKey( "$['store']['bicycle']" ), 
            source.GetPropertyFromKey( "$['store']['book'][0]" ), 
            source.GetPropertyFromKey( "$['store']['book'][1]" ), 
            source.GetPropertyFromKey( "$['store']['book'][2]" ), 
            source.GetPropertyFromKey( "$['store']['book'][3]" ), 
            source.GetPropertyFromKey( "$['store']['book'][0]['category']" ), 
            source.GetPropertyFromKey( "$['store']['book'][0]['author']" ), 
            source.GetPropertyFromKey( "$['store']['book'][0]['title']" ), 
            source.GetPropertyFromKey( "$['store']['book'][0]['price']" ), 
            source.GetPropertyFromKey( "$['store']['book'][1]['category']" ), 
            source.GetPropertyFromKey( "$['store']['book'][1]['author']" ), 
            source.GetPropertyFromKey( "$['store']['book'][1]['title']" ), 
            source.GetPropertyFromKey( "$['store']['book'][1]['price']" ), 
            source.GetPropertyFromKey( "$['store']['book'][2]['category']" ), 
            source.GetPropertyFromKey( "$['store']['book'][2]['author']" ), 
            source.GetPropertyFromKey( "$['store']['book'][2]['title']" ), 
            source.GetPropertyFromKey( "$['store']['book'][2]['isbn']" ), 
            source.GetPropertyFromKey( "$['store']['book'][2]['price']" ), 
            source.GetPropertyFromKey( "$['store']['book'][3]['category']" ), 
            source.GetPropertyFromKey( "$['store']['book'][3]['author']" ), 
            source.GetPropertyFromKey( "$['store']['book'][3]['title']" ), 
            source.GetPropertyFromKey( "$['store']['book'][3]['isbn']" ), 
            source.GetPropertyFromKey( "$['store']['book'][3]['price']" ), 
            source.GetPropertyFromKey( "$['store']['bicycle']['color']" ), 
            source.GetPropertyFromKey( "$['store']['bicycle']['price']" ) 
        };

        Assert.IsTrue( expected.SequenceEqual( matches ) );
    }

    [DataTestMethod]
    [DataRow( @"$..book[?(@.price == 8.99 && @.category == ""fiction"")]", typeof(JsonDocument) )]
    [DataRow( @"$..book[?(@.price == 8.99 && @.category == ""fiction"")]", typeof(JsonNode) )]
    [DataRow( @"$..book[?@.price == 8.99 && @.category == ""fiction""]", typeof(JsonDocument) )]
    [DataRow( @"$..book[?@.price == 8.99 && @.category == ""fiction""]", typeof(JsonNode) )]
    public void FilterAllBooksUsingLogicalAndInScript( string query, Type sourceType )
    {
        var source = GetDocumentProxy( sourceType );
        var match = source.Select( query ).Single();
        var expected = source.GetPropertyFromKey( "$['store']['book'][2]" );

        Assert.AreEqual( expected, match );
    }


    [DataTestMethod]
    [DataRow( @"$..book[?@.price == 8.99 && (@.category == ""fiction"")]", typeof(JsonDocument) )]
    [DataRow( @"$..book[?@.price == 8.99 && (@.category == ""fiction"")]", typeof(JsonNode) )]
    public void FilterWithUnevenParentheses( string query, Type sourceType )
    {
        var source = GetDocumentProxy( sourceType );
        var match = source.Select( query ).Single();
        var expected = source.GetPropertyFromKey( "$['store']['book'][2]" );

        Assert.AreEqual( expected, match );
    }
}
