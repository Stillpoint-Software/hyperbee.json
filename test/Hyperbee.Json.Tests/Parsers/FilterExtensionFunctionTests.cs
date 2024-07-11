using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Nodes;
using Hyperbee.Json.Descriptors;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

    private class PathNodeFunction() : FilterExtensionFunction( argumentCount: 1 )
    {
        public const string Name = "path";
        private static readonly Expression PathExpression = Expression.Constant( (Func<INodeType, INodeType>) Path );

        protected override Expression GetExtensionExpression( Expression[] arguments, bool[] argumentInfo )
        {
            return Expression.Invoke( PathExpression,
                Expression.Convert( arguments[0], typeof( INodeType ) ) );
        }

        private static INodeType Path( INodeType arg )
        {
            if ( arg is NodesType<JsonNode> nodes )
            {
                var node = nodes.FirstOrDefault();
                return new ValueType<string>( node?.GetPath() );
            }

            return Descriptors.ValueType.Null;
        }
    }
}

