using System;
using System.Linq;
using Hyperbee.Json.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Query
{
    [TestClass]
    public class JsonQueryParserRfc9535Tests
    {
        [DataTestMethod]
        [DataRow( "$", "[$ => singular]" )]
        [DataRow( "$.two.some", "[$ => singular][two => singular][some => singular]" )]
        [DataRow( "$.thing[1:2:3]", "[$ => singular][thing => singular][1:2:3 => group]" )]
        [DataRow( "$..thing[?(@.x == 1)]", "[$ => singular][.. => group][thing => singular][?(@.x == 1) => group]" )]
        [DataRow( "$['two.some']", "[$ => singular][two.some => singular]" )]
        [DataRow( "$.two.some.thing['this.or.that']", "[$ => singular][two => singular][some => singular][thing => singular][this.or.that => singular]" )]
        [DataRow( "$.store.book[*].author", "[$ => singular][store => singular][book => singular][* => group][author => singular]" )]
        [DataRow( "@..author", "[@ => singular][.. => group][author => singular]" )]
        [DataRow( "$.store.*", "[$ => singular][store => singular][* => group]" )]
        [DataRow( "$.store..price", "[$ => singular][store => singular][.. => group][price => singular]" )]
        [DataRow( "$..book[2]", "[$ => singular][.. => group][book => singular][2 => singular]" )]
        [DataRow( "$..book[-1:]", "[$ => singular][.. => group][book => singular][-1: => group]" )]
        [DataRow( "$..book[:2]", "[$ => singular][.. => group][book => singular][:2 => group]" )]
        [DataRow( "$..book[0,1]", "[$ => singular][.. => group][book => singular][0,1 => group]" )]
        [DataRow( "$.store.book[0,1]", "[$ => singular][store => singular][book => singular][0,1 => group]" )]
        [DataRow( "$..book['category','author']", "[$ => singular][.. => group][book => singular][category,author => group]" )]
        [DataRow( "$..book[?(@.isbn)]", "[$ => singular][.. => group][book => singular][?(@.isbn) => group]" )]
        [DataRow( "$..book[?@.isbn]", "[$ => singular][.. => group][book => singular][?@.isbn => group]" )]
        [DataRow( "$..book[?(@.price<10)]", "[$ => singular][.. => group][book => singular][?(@.price<10) => group]" )]
        [DataRow( "$..book[?@.price<10]", "[$ => singular][.. => group][book => singular][?@.price<10 => group]" )]
        [DataRow( "$..*", "[$ => singular][.. => group][* => group]" )]
        [DataRow( "$..book[?(@.price == 8.99 && @.category == \"fiction\")]", "[$ => singular][.. => group][book => singular][?(@.price == 8.99 && @.category == \"fiction\") => group]" )]
        public void ParseJsonPath_Rfc9535( string jsonPath, string expected )
        {
            // act
            var compiledQuery = JsonQueryParser.Parse( jsonPath, JsonQueryParserOptions.Rfc9535 );

            // arrange
            var result = GetResultString( compiledQuery.Segments );

            // assert
            Assert.AreEqual( expected, result );

            return;

            static string GetResultString( JsonSegment segment )
            {
                return string.Join( "", segment.AsEnumerable().Select( ConvertToString ) );

                static string ConvertToString( JsonSegment segment )
                {
                    var (singular, selectors) = segment;
                    var selectorType = singular ? "singular" : "group";
                    var selectorsString = string.Join( ',', selectors.Select( x => x.Value ).Reverse() );

                    return $"[{selectorsString} => {selectorType}]";
                }
            }
        }

        [TestMethod]
        public void ShouldFilterExpressionWithParentAxisOperator()
        {
            // NOT-SUPPORTED: parent axis operator is not supported

            // act & assert
            const string jsonPath = "$[*].bookmarks[ ? (@.page == 45)]^^^";

            Assert.ThrowsException<NotSupportedException>( () =>
            {
                JsonQueryParser.Parse( jsonPath );
            } );
        }
    }
}
