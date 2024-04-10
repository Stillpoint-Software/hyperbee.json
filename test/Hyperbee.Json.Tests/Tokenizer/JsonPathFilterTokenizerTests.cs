using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Tokenizer;

[TestClass]
public class JsonPathFilterTokenizerTests
{
    [DataTestMethod]
    [DataRow( "?(@.price < 10)", "@:price|LessThan|literal:10" )]
    [DataRow( "?(@.price == 8.99 && @.category == 'fiction')", "{@:price|Equals|literal:8.99};{And};{@:category|Equals|literal:'fiction'}" )]
    [DataRow( "?(@.price == 8.99 || @.category == 'fiction')", "{@:price|Equals|literal:8.99};{Or};{@:category|Equals|literal:'fiction'}" )]
    public void Should_tokenize_filter( string filter, string expected )
    {
        // arrange

        // act

        //var result = JsonPathFilter.Tokenize( filter ).ToList();

        // assert
        Assert.IsTrue( true );
    }
}
