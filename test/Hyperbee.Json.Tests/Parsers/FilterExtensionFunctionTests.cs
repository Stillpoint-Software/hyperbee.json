using System.Linq;
using System.Reflection;
using System.Text.Json.Nodes;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Descriptors.Element.Functions;
using Hyperbee.Json.Descriptors.Types;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ValueType = Hyperbee.Json.Descriptors.Types.ValueType;

namespace Hyperbee.Json.Tests.Parsers;

[TestClass]
public class FilterExtensionFunctionTests : JsonTestBase
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

    private class PathNodeFunction() : FilterExtensionFunction( PathMethodInfo, FilterExtensionInfo.MustCompare )
    {
        public const string Name = "path";
        private static readonly MethodInfo PathMethodInfo = GetMethod<PathNodeFunction>( nameof( Path ) );

        private static INodeType Path( INodeType arg )
        {
            if ( arg is NodesType<JsonNode> nodes )
            {
                var node = nodes.FirstOrDefault();
                return new ValueType<string>( node?.GetPath() );
            }

            return ValueType.Null;
        }
    }
}

