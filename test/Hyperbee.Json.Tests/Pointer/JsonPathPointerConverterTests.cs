using System;
using Hyperbee.Json.Pointer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Pointer;

[TestClass]
public class JsonPathPointerConverterTests
{
    [DataTestMethod]
    [DataRow( "$['store'].book[0].title", "#/store/book/0/title", true )]
    [DataRow( "$['store'].book[0].title", "/store/book/0/title", false )]
    [DataRow( "$.store.book[0].title", "#/store/book/0/title", true )]
    [DataRow( "$.store.book[0].title", "/store/book/0/title", false )]
    [DataRow( "$", "#/", true )]
    [DataRow( "$", "/", false )]
    [DataRow( "$['store']['book'][1]['author']", "#/store/book/1/author", true )]
    [DataRow( "$['store']['book'][1]['author']", "/store/book/1/author", false )]
    [DataRow( "$.store.book[1].author", "#/store/book/1/author", true )]
    [DataRow( "$.store.book[1].author", "/store/book/1/author", false )]
    [DataRow( "$['store'].book[0].price", "#/store/book/0/price", true )]
    [DataRow( "$['store'].book[0].price", "/store/book/0/price", false )]
    [DataRow( "$.store.book[0].price", "#/store/book/0/price", true )]
    [DataRow( "$.store.book[0].price", "/store/book/0/price", false )]
    [DataRow( "$['store'].bestseller", "#/store/bestseller", true )]
    [DataRow( "$['store'].bestseller", "/store/bestseller", false )]
    [DataRow( "$.store.bestseller", "#/store/bestseller", true )]
    [DataRow( "$.store.bestseller", "/store/bestseller", false )]
    [DataRow( "$['store'].book[0].isbn", "#/store/book/0/isbn", true )]
    [DataRow( "$['store'].book[0].isbn", "/store/book/0/isbn", false )]
    [DataRow( "$.store.book[0].isbn", "#/store/book/0/isbn", true )]
    [DataRow( "$.store.book[0].isbn", "/store/book/0/isbn", false )]
    [DataRow( "$['complex~0name']", "#/complex~00name", true )]
    [DataRow( "$['complex~0name']", "/complex~00name", false )]
    [DataRow( "$.store['complex/name']", "#/store/complex~1name", true )]
    [DataRow( "$.store['complex/name']", "/store/complex~1name", false )]
    public void TestConvertJsonPathToJsonPointer( string jsonPath, string expected, bool asFragment )
    {
        var options = asFragment ? JsonPointerConvertOptions.Fragment : JsonPointerConvertOptions.Default;
        var jsonPointer = JsonPathPointerConverter.ConvertJsonPathToJsonPointer( jsonPath.AsSpan(), options );

        Assert.AreEqual( expected, jsonPointer );
    }

    [DataTestMethod]
    [DataRow( "/store/book/0/title", "$.store.book[0].title" )]
    [DataRow( "#/store/book/0/title", "$.store.book[0].title" )]
    [DataRow( "/", "$" )]
    [DataRow( "#/", "$" )]
    [DataRow( "/store/book/1/author", "$.store.book[1].author" )]
    [DataRow( "#/store/book/1/author", "$.store.book[1].author" )]
    [DataRow( "/store/book/0/price", "$.store.book[0].price" )]
    [DataRow( "#/store/book/0/price", "$.store.book[0].price" )]
    [DataRow( "/store/bestseller", "$.store.bestseller" )]
    [DataRow( "#/store/bestseller", "$.store.bestseller" )]
    [DataRow( "/store/book/0/isbn", "$.store.book[0].isbn" )]
    [DataRow( "#/store/book/0/isbn", "$.store.book[0].isbn" )]
    [DataRow( "/complex~0name", "$['complex~name']" )]
    [DataRow( "#/complex~0name", "$['complex~name']" )]
    [DataRow( "/complex~1name", "$['complex/name']" )]
    [DataRow( "#/complex~1name", "$['complex/name']" )]
    public void TestConvertJsonPointerToJsonPath( string jsonPointer, string expected )
    {
        var jsonPath = JsonPathPointerConverter.ConvertJsonPointerToJsonPath( jsonPointer.AsSpan() );
        Assert.AreEqual( expected, jsonPath );
    }
}
