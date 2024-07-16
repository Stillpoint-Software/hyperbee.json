using System.Linq;
using System.Reflection;
using System.Text.Json.Nodes;
using Hyperbee.Json.Descriptors.Element;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Filters.Values;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Parsers;

[TestClass]
public class ExtensionFunctionTests : JsonTestBase
{
    [TestMethod]
    public void Should_CallCustomFunction()
    {
        // arrange 
        var source = GetDocument<JsonNode>();

        JsonTypeDescriptorRegistry
            .GetDescriptor<JsonNode>()
            .Functions
            .Register( PathNodeFunction.Name, () => new PathNodeFunction() );

        // act
        var results = source.Select( "$..[?path(@) == '$.store.book[2].title']" ).ToList();

        // assert
        Assert.IsTrue( results.Count == 1 );
        Assert.AreEqual( "$.store.book[2].title", results[0].GetPath() );
    }

    private class PathNodeFunction() : ExtensionFunction( PathMethod, ExtensionInfo.MustCompare )
    {
        public const string Name = "path";
        private static readonly MethodInfo PathMethod = GetMethod<PathNodeFunction>( nameof( Path ) );

        private static ScalarValue<string> Path( IValueType argument )
        {
            return argument.TryGetNode<JsonNode>( out var node ) ? node?.GetPath() : null;
        }
    }
}

