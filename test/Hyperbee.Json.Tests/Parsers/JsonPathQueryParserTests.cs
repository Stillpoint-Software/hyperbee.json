using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Parsers;

[TestClass]
public class JsonPathQueryParserTests
{
    [DataTestMethod]
    [DataRow( "$", "[$ => 1]" )]
    [DataRow( "$.two.some", "[$ => 1][two => 1][some => 1]" )]
    [DataRow( "$.thing[1:2:3]", "[$ => 1][thing => 1][1:2:3 => #]" )]
    [DataRow( "$..thing[?(@.x == 1)]", "[$ => 1][.. => #][thing => 1][?(@.x == 1) => #]" )]
    [DataRow( "$['two.some']", "[$ => 1][two.some => 1]" )]
    [DataRow( "$.two.some.thing['this.or.that']", "[$ => 1][two => 1][some => 1][thing => 1][this.or.that => 1]" )]
    [DataRow( "$.store.book[*].author", "[$ => 1][store => 1][book => 1][* => #][author => 1]" )]
    [DataRow( "@..author", "[@ => 1][.. => #][author => 1]" )]
    [DataRow( "$.store.*", "[$ => 1][store => 1][* => #]" )]
    [DataRow( "$.store..price", "[$ => 1][store => 1][.. => #][price => 1]" )]
    [DataRow( "$..book[2]", "[$ => 1][.. => #][book => 1][2 => 1]" )]
    [DataRow( "$..book[-1:]", "[$ => 1][.. => #][book => 1][-1: => #]" )]
    [DataRow( "$..book[:2]", "[$ => 1][.. => #][book => 1][:2 => #]" )]
    [DataRow( "$..book[0,1]", "[$ => 1][.. => #][book => 1][0,1 => #]" )]
    [DataRow( "$.store.book[0,1]", "[$ => 1][store => 1][book => 1][0,1 => #]" )]
    [DataRow( "$..book['category','author']", "[$ => 1][.. => #][book => 1][category,author => #]" )]
    [DataRow( "$..book[?(@.isbn)]", "[$ => 1][.. => #][book => 1][?(@.isbn) => #]" )]
    [DataRow( "$..book[?@.isbn]", "[$ => 1][.. => #][book => 1][?@.isbn => #]" )]
    [DataRow( "$..book[?(@.price<10)]", "[$ => 1][.. => #][book => 1][?(@.price<10) => #]" )]
    [DataRow( "$..book[?@.price<10]", "[$ => 1][.. => #][book => 1][?@.price<10 => #]" )]
    [DataRow( "$..*", "[$ => 1][.. => #][* => #]" )]
    [DataRow( "$..book[?(@.price == 8.99 && @.category == \"fiction\")]", "[$ => 1][.. => #][book => 1][?(@.price == 8.99 && @.category == \"fiction\") => #]" )]
    public void TokenizeJsonPath( string jsonPath, string expected )
    {
        // act
        var compiledQuery = JsonPathQueryParser.Parse( jsonPath );

        // arrange
        var result = GetResultString( compiledQuery.Segments );

        // assert
        Assert.AreEqual( expected, result );

        return;

        static string GetResultString( JsonPathSegment segment )
        {
            return string.Join( "", segment.AsEnumerable().Select( ConvertToString ) );

            static string ConvertToString( JsonPathSegment segment )
            {
                var (singular, selectors) = segment;
                var selectorType = singular ? "1" : "#"; // 1:singular, #:group
                var selectorsString = string.Join( ',', selectors.Select( x => x.Value ).Reverse() );

                return $"[{selectorsString} => {selectorType}]";
            }
        }
    }
}
