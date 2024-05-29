using System.Collections.Generic;
using System.Linq;
using Hyperbee.Json.Tokenizer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Tokenizer;

[TestClass]
public class JsonPathQueryTokenizerTests
{
    [DataTestMethod]
    [DataRow( "$", "{$|k}" )]
    [DataRow( "$.two.some", "{$|k};{two|k};{some|k}" )]
    [DataRow( "$.thing[1:2:3]", "{$|k};{thing|k};{1:2:3|s}" )]
    [DataRow( "$..thing[?(@.x == 1)]", "{$|k};{..|s};{thing|k};{?(@.x == 1)|s}" )]
    [DataRow( "$['two.some']", "{$|k};{two.some|k}" )]
    [DataRow( "$.two.some.thing['this.or.that']", "{$|k};{two|k};{some|k};{thing|k};{this.or.that|k}" )]
    [DataRow( "$.store.book[*].author", "{$|k};{store|k};{book|k};{*|s};{author|k}" )]
    [DataRow( "@..author", "{@|k};{..|s};{author|k}" )]
    [DataRow( "$.store.*", "{$|k};{store|k};{*|s}" )]
    [DataRow( "$.store..price", "{$|k};{store|k};{..|s};{price|k}" )]
    [DataRow( "$..book[2]", "{$|k};{..|s};{book|k};{2|k}" )]
    [DataRow( "$..book[-1:]", "{$|k};{..|s};{book|k};{-1:|s}" )]
    [DataRow( "$..book[:2]", "{$|k};{..|s};{book|k};{:2|s}" )]
    [DataRow( "$..book[0,1]", "{$|k};{..|s};{book|k};{1,0|s}" )]
    [DataRow( "$.store.book[0,1]", "{$|k};{store|k};{book|k};{1,0|s}" )]
    [DataRow( "$..book['category','author']", "{$|k};{..|s};{book|k};{author,category|s}" )]
    [DataRow( "$..book[?(@.isbn)]", "{$|k};{..|s};{book|k};{?(@.isbn)|s}" )]
    [DataRow( "$..book[?(@.price<10)]", "{$|k};{..|s};{book|k};{?(@.price<10)|s}" )]
    [DataRow( "$..*", "{$|k};{..|s};{*|s}" )]
    [DataRow( @"$.store.book[?(@path !== ""$['store']['book'][0]"")]", @"{$|k};{store|k};{book|k};{?(@path !== ""$['store']['book'][0]"")|s}" )]
    [DataRow( @"$..book[?(@.price == 8.99 && @.category == ""fiction"")]", @"{$|k};{..|s};{book|k};{?(@.price == 8.99 && @.category == ""fiction"")|s}" )]
    public void Should_tokenize_json_path( string jsonPath, string expected )
    {
        // arrange
        static string TokensToString( IEnumerable<JsonPathToken> tokens )
        {
            static string TokenToString( JsonPathToken token )
            {
                var (keySelector, selectors) = token;
                var selectorType = keySelector ? "k" : "s";
                var selectorsString = string.Join( ',', selectors.Select( x => x.Value ) );

                return $"{{{selectorsString}|{selectorType}}}";
            }

            return string.Join( ';', tokens.Select( TokenToString ) );
        }

        // act
        var tokens = JsonPathQueryTokenizer.Tokenize( jsonPath );

        // assert
        var result = TokensToString( tokens );

        Assert.AreEqual( expected, result );
    }
}
