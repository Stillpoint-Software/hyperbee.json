using System.Linq;
using Hyperbee.Json.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Query;

[TestClass]
public class JsonQueryParserRfc6901Tests
{
    [DataTestMethod]
    [DataRow("/", "[/ => singular]")]
    [DataRow("/two/some", "[/ => singular][two => singular][some => singular]")]
    [DataRow("/thing/1:2:3", "[/ => singular][thing => singular][1:2:3 => singular]")]
    [DataRow("/thing/~1", "[/ => singular][thing => singular][/ => singular]")]
    [DataRow("/thing/~0", "[/ => singular][thing => singular][~ => singular]")]
    [DataRow("/store/book/0/author", "[/ => singular][store => singular][book => singular][0 => singular][author => singular]")]
    [DataRow("/store/*", "[/ => singular][store => singular][* => singular]")]
    [DataRow("/book/2", "[/ => singular][book => singular][2 => singular]")]
    [DataRow("/book/-1", "[/ => singular][book => singular][-1 => singular]")]
    [DataRow("/book/0,1", "[/ => singular][book => singular][0,1 => singular]")]
    [DataRow("/book/category,author", "[/ => singular][book => singular][category,author => singular]")]
    public void ParseJsonPointer_Rfc6901(string jsonPointer, string expected)
    {
        // act
        var compiledQuery = JsonQueryParser.Parse(jsonPointer, JsonQueryParserOptions.Rfc6901);

        // arrange
        var result = GetResultString(compiledQuery.Segments);

        // assert
        Assert.AreEqual(expected, result);

        return;

        static string GetResultString(JsonSegment segment)
        {
            return string.Join("", segment.AsEnumerable().Select(ConvertToString));

            static string ConvertToString(JsonSegment segment)
            {
                var (singular, selectors) = segment;
                var selectorType = singular ? "singular" : "group";
                var selectorsString = string.Join(',', selectors.Select(x => x.Value).Reverse());

                return $"[{selectorsString} => {selectorType}]";
            }
        }
    }
}
