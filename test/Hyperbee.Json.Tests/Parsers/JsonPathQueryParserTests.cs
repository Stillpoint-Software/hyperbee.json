using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Parsers;

[TestClass]
public class JsonPathQueryParserTests
{
    [DataTestMethod]
    [DataRow( "$", "{$|s}" )]
    [DataRow( "$.two.some", "{$|s};{two|s};{some|s}" )]
    [DataRow( "$.thing[1:2:3]", "{$|s};{thing|s};{1:2:3|g}" )]
    [DataRow( "$.'thing'[1:2:3]", "{$|s};{thing|s};{1:2:3|g}" )]
    [DataRow( "$..thing[?(@.x == 1)]", "{$|s};{..|g};{thing|s};{?(@.x == 1)|g}" )]
    [DataRow( "$['two.some']", "{$|s};{two.some|s}" )]
    [DataRow( "$.two.some.thing['this.or.that']", "{$|s};{two|s};{some|s};{thing|s};{this.or.that|s}" )]
    [DataRow( "$.store.book[*].author", "{$|s};{store|s};{book|s};{*|g};{author|s}" )]
    [DataRow( "$.store.'book'[*].author", "{$|s};{store|s};{book|s};{*|g};{author|s}" )]
    [DataRow( "$.store.book[*].'author'", "{$|s};{store|s};{book|s};{*|g};{author|s}" )]
    [DataRow( "@..author", "{@|s};{..|g};{author|s}" )]
    [DataRow( "$.store.*", "{$|s};{store|s};{*|g}" )]
    [DataRow( "$.store..price", "{$|s};{store|s};{..|g};{price|s}" )]
    [DataRow( "$..book[2]", "{$|s};{..|g};{book|s};{2|s}" )]
    [DataRow( "$..book[-1:]", "{$|s};{..|g};{book|s};{-1:|g}" )]
    [DataRow( "$..book[:2]", "{$|s};{..|g};{book|s};{:2|g}" )]
    [DataRow( "$..book[0,1]", "{$|s};{..|g};{book|s};{1,0|g}" )]
    [DataRow( "$.store.book[0,1]", "{$|s};{store|s};{book|s};{1,0|g}" )]
    [DataRow( "$..book['category','author']", "{$|s};{..|g};{book|s};{author,category|g}" )]
    [DataRow( "$..book[?(@.isbn)]", "{$|s};{..|g};{book|s};{?(@.isbn)|g}" )]
    [DataRow( "$..book[?@.isbn]", "{$|s};{..|g};{book|s};{?@.isbn|g}" )]
    [DataRow( "$..book[?(@.price<10)]", "{$|s};{..|g};{book|s};{?(@.price<10)|g}" )]
    [DataRow( "$..book[?@.price<10]", "{$|s};{..|g};{book|s};{?@.price<10|g}" )]
    [DataRow( "$..*", "{$|s};{..|g};{*|g}" )]
    [DataRow( """$.store.book[?(@path !== "$['store']['book'][0]")]""", """{$|s};{store|s};{book|s};{?(@path !== "$['store']['book'][0]")|g}""" )]
    [DataRow( """$..book[?(@.price == 8.99 && @.category == "fiction")]""", """{$|s};{..|g};{book|s};{?(@.price == 8.99 && @.category == "fiction")|g}""" )]
    public void Should_TokenizeJsonPath( string jsonPath, string expected )
    {
        // act
        var pathSegment = JsonPathQueryParser.Parse( jsonPath );

        // arrange
        var result = SegmentsToString( pathSegment );

        // assert
        Assert.AreEqual( expected, result );
        return;

        static string SegmentsToString( JsonPathSegment segment )
        {
            return string.Join( ';', segment.AsEnumerable().Select( SegmentToString ) );

            static string SegmentToString( JsonPathSegment segment )
            {
                var (singular, selectors) = segment;
                var selectorType = singular ? "s" : "g"; // s:singular, g:group
                var selectorsString = string.Join( ',', selectors.Select( x => x.Value ) );

                return $"{{{selectorsString}|{selectorType}}}";
            }
        }
    }

    [TestMethod]
    public void ShouldFilterExpressionWithParentAxisOperator()
    {
        // NOT-SUPPORTED: parent axis operator is not supported

        // act & assert
        var jsonPath = "$[*].bookmarks[ ? (@.page == 45)]^^^";

        Assert.ThrowsException<NotSupportedException>( () =>
        {
            var pathSegment = JsonPathQueryParser.Parse( jsonPath );
        } );
    }
}
