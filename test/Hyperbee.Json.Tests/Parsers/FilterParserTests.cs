using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Descriptors.Element;
using Hyperbee.Json.Descriptors.Node;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Filters.Parser;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Parsers;

[TestClass]
public class FilterParserTests : JsonTestBase
{
    [DataTestMethod]
    [DataRow( "((1 == 1))", true, typeof( JsonElement ) )]
    [DataRow( "((\"world\" == 'world') && (1 == 1))", true, typeof( JsonElement ) )]
    [DataRow( "1 == 1", true, typeof( JsonElement ) )]
    [DataRow( "(1 == 1)", true, typeof( JsonElement ) )]
    [DataRow( "(1 != 2)", true, typeof( JsonElement ) )]
    [DataRow( "!(1 == 2)", true, typeof( JsonElement ) )]
    [DataRow( "(\"world\" == 'world') || 1 == 1", true, typeof( JsonElement ) )]
    [DataRow( "!('World' != 'World') && !(1 == 2 || 1 == 3)", true, typeof( JsonElement ) )]
    [DataRow( "1 == 1", true, typeof( JsonNode ) )]
    [DataRow( "(1 == 1)", true, typeof( JsonNode ) )]
    [DataRow( "(1 != 2)", true, typeof( JsonNode ) )]
    [DataRow( "!(1 == 2)", true, typeof( JsonNode ) )]
    [DataRow( "(\"world\" == 'world') || 1 == 1", true, typeof( JsonNode ) )]
    [DataRow( "!('World' != 'World') && !(1 == 2 || 1 == 3)", true, typeof( JsonNode ) )]
    public void Should_MatchExpectedResult_WhenUsingConstants( string filter, bool expected, Type sourceType )
    {
        // arrange 
        var (expression, param) = GetExpression( filter, sourceType );

        // act
        var result = Execute( expression, param, sourceType );

        // assert
        Assert.AreEqual( expected, result );
    }

    [DataTestMethod]
    [DataRow( "true", typeof( JsonElement ) )]
    [DataRow( "false", typeof( JsonElement ) )]
    [DataRow( "true", typeof( JsonNode ) )]
    [DataRow( "false", typeof( JsonNode ) )]
    public void Should_Fail_WhenNotComparingLiterals( string filter, Type sourceType )
    {
        // arrange 

        // act & assert
        Assert.ThrowsException<NotSupportedException>( () =>
        {
            var (expression, param) = GetExpression( filter, sourceType );
            return Execute( expression, param, sourceType );
        } );
    }

    [DataTestMethod]
    [DataRow( "@.store.bicycle.price < 10", false, typeof( JsonElement ) )]
    [DataRow( "@.store.bicycle.price <= 10", false, typeof( JsonElement ) )]
    [DataRow( "@.store.bicycle.price < 20", true, typeof( JsonElement ) )]
    [DataRow( "@.store.bicycle.price <= 20", true, typeof( JsonElement ) )]
    [DataRow( "@.store.bicycle.price > 15", true, typeof( JsonElement ) )]
    [DataRow( "@.store.bicycle.price >= 15", true, typeof( JsonElement ) )]
    [DataRow( "@.store.bicycle.price > 20", false, typeof( JsonElement ) )]
    [DataRow( "@.store.bicycle.price >= 20", false, typeof( JsonElement ) )]
    [DataRow( "@.store.bicycle.price == 19.95", true, typeof( JsonElement ) )]
    [DataRow( "@.store.bicycle.price != 19.95", false, typeof( JsonElement ) )]
    [DataRow( "@.store.book[0].category == \"reference\"", true, typeof( JsonElement ) )]
    [DataRow( "@.store.book[0].category == 'reference'", true, typeof( JsonElement ) )]
    [DataRow( "@.store.book[0].category == @.store.book[1].category", false, typeof( JsonElement ) )]
    [DataRow( "@.store.bicycle.price > @.store.bicycle.price", false, typeof( JsonElement ) )]
    [DataRow( "@.store.bicycle", true, typeof( JsonElement ) )]
    [DataRow( "@.store.book", true, typeof( JsonElement ) )]
    [DataRow( "@.store.nothing", false, typeof( JsonElement ) )]
    [DataRow( "@.store.bicycle.price", true, typeof( JsonElement ) )]
    [DataRow( "@.store.book[0].category", true, typeof( JsonElement ) )]
    [DataRow( "@.store.bicycle.price < 10", false, typeof( JsonNode ) )]
    [DataRow( "@.store.bicycle.price > 15", true, typeof( JsonNode ) )]
    [DataRow( "@.store.book[0].category == \"reference\"", true, typeof( JsonNode ) )]
    [DataRow( "@.store.book[0].category == 'reference'", true, typeof( JsonNode ) )]
    [DataRow( "@.store.book[0].category == @.store.book[1].category", false, typeof( JsonNode ) )]
    [DataRow( "@.store.bicycle.price > @.store.bicycle.price", false, typeof( JsonNode ) )]
    [DataRow( "@.store.bicycle", true, typeof( JsonNode ) )]
    [DataRow( "@.store.book", true, typeof( JsonNode ) )]
    [DataRow( "@.store.nothing", false, typeof( JsonNode ) )]
    [DataRow( "@.store.bicycle.price", true, typeof( JsonNode ) )]
    [DataRow( "@.store.book[0].category", true, typeof( JsonNode ) )]
    public void Should_MatchExpectedResult_WhenUsingJsonPath( string filter, bool expected, Type sourceType )
    {
        // arrange & act
        var result = CompileAndExecuteFilter( filter, sourceType );

        // assert
        Assert.AreEqual( expected, result );
    }

    [DataTestMethod]
    [DataRow( "$.store.book[?(@.price > 20)].price", 22.99F, typeof( JsonElement ) )]
    [DataRow( "$.store.book[?(@.category == 'reference')].price", 8.95F, typeof( JsonElement ) )]
    [DataRow( "$.store.book[?(@.price < 9.00 && @.category == 'reference')].price", 8.95F, typeof( JsonElement ) )]
    [DataRow( "$.store.book[?(match(@.title, \"Sayings.*\" ))].price", 8.95F, typeof( JsonElement ) )]
    [DataRow( "$.store.book[?(@.category == $.store.book[0].category)].price", 8.95F, typeof( JsonElement ) )]
    [DataRow( "$.store.book[?(@.price > 20)].price", 22.99F, typeof( JsonNode ) )]
    [DataRow( "$.store.book[?(@.category == 'reference')].price", 8.95F, typeof( JsonNode ) )]
    [DataRow( "$.store.book[?(@.price < 9.00 && @.category == 'reference')].price", 8.95F, typeof( JsonNode ) )]
    [DataRow( "$.store.book[?(match(@.title, \"Sayings.*\" ))].price", 8.95F, typeof( JsonNode ) )]
    [DataRow( "$.store.book[?(@.category == $.store.book[0].category)].price", 8.95F, typeof( JsonNode ) )]
    public void Should_ReturnExpectedResult_WhenUsingExpressionEvaluator( string filter, float expected, Type sourceType )
    {
        // arrange & act
        var result = Select( filter, sourceType );

        // assert
        Assert.AreEqual( expected, result );
    }

    [DataTestMethod]
    [DataRow( "count(@.store.book) == 1", true, typeof( JsonElement ) )]
    [DataRow( "count(@.store.book.*) == 4", true, typeof( JsonElement ) )]
    [DataRow( "length(@.store.book) == 4", true, typeof( JsonElement ) )]
    [DataRow( "length(@.store.book[0].category) == 9", true, typeof( JsonElement ) )]
    [DataRow( "match(@.store.book[0].title, \"Sayings.*\" )", true, typeof( JsonElement ) )]
    [DataRow( "search(@.store.book[0].author, \"[Nn]igel Rees\" )", true, typeof( JsonElement ) )]
    [DataRow( "value(@.store.book[0].author) == \"Nigel Rees\"", true, typeof( JsonElement ) )]
    [DataRow( "count(@.store.book) == 1", true, typeof( JsonNode ) )]
    [DataRow( "count(@.store.book.*) == 4", true, typeof( JsonNode ) )]
    [DataRow( "length(@.store.book) == 4", true, typeof( JsonNode ) )]
    [DataRow( "length(@.store.book[0].category) == 9", true, typeof( JsonNode ) )]
    [DataRow( "match(@.store.book[0].title, \"Sayings.*\" )", true, typeof( JsonNode ) )]
    [DataRow( "search(@.store.book[0].author, \"[Nn]igel Rees\" )", true, typeof( JsonNode ) )]
    [DataRow( "value(@.store.book[0].author) == \"Nigel Rees\"", true, typeof( JsonNode ) )]
    public void Should_MatchExpectedResult_WhenUsingFunctions( string filter, bool expected, Type sourceType )
    {
        // arrange & act
        var result = CompileAndExecuteFilter( filter, sourceType );

        // assert
        Assert.AreEqual( expected, result );
    }

    [DataTestMethod]
    [DataRow( "length(@.store.book) == 4  ", true, typeof( JsonElement ) )]
    [DataRow( "  length(@.store.book) == 4", true, typeof( JsonElement ) )]
    [DataRow( "  length(@.store.book) == 4  ", true, typeof( JsonElement ) )]
    [DataRow( "  length( @.store.book ) == 4  ", true, typeof( JsonElement ) )]
    [DataRow( "4 == length( @.store.book )  ", true, typeof( JsonElement ) )]
    [DataRow( "  4 == length(@.store.book)", true, typeof( JsonElement ) )]
    [DataRow( "  4 == length(@.store.book)  ", true, typeof( JsonElement ) )]
    [DataRow( "  4 == length( @.store.book )  ", true, typeof( JsonElement ) )]
    public void Should_MatchExpectedResult_WhenHasExtraSpaces( string filter, bool expected, Type sourceType )
    {
        // arrange & act
        var result = CompileAndExecuteFilter( filter, sourceType );

        // assert
        Assert.AreEqual( expected, result );
    }

    [DataTestMethod]
    [DataRow( "4 == length ( @.store.book )", typeof( JsonElement ) )]
    [DataRow( "length (@.store.book) == 4", typeof( JsonElement ) )]
    public void Should_Fail_WhenHasInvalidWhitespace( string filter, Type sourceType )
    {
        Assert.ThrowsException<NotSupportedException>( () => CompileAndExecuteFilter( filter, sourceType ) );
    }

    [DataTestMethod]
    [DataRow( "unknown_literal", typeof( JsonElement ) )]
    [DataRow( "'unbalanced string\"", typeof( JsonElement ) )]
    [DataRow( " \t ", typeof( JsonElement ) )]
    [DataRow( "1 === 1", typeof( JsonElement ) )]
    [DataRow( "(1 == 1(", typeof( JsonElement ) )]
    [DataRow( "(1 == 1)(", typeof( JsonElement ) )]
    [DataRow( "(1 == ", typeof( JsonElement ) )]
    [DataRow( "== 1", typeof( JsonElement ) )]
    [DataRow( "badMethod(1)", typeof( JsonElement ) )]
    public void Should_FailToParse_WhenUsingInvalidFilters( string filter, Type sourceType )
    {
        try
        {
            GetExpression( filter, sourceType );
        }
        catch
        {
            // Most are FormatExceptions, but some are ArgumentExceptions 
            return;
        }

        Assert.Fail( "Did not throw an exception" );
    }

    private static (Expression, ParameterExpression) GetExpression( string filter, Type sourceType )
    {
        if ( sourceType == typeof( JsonElement ) )
        {
            var elementContext = new FilterParserContext<JsonElement>( new ElementTypeDescriptor() );
            return (FilterParser<JsonElement>.Parse( filter, elementContext ), elementContext.RuntimeContext);
        }

        var nodeContext = new FilterParserContext<JsonNode>( new NodeTypeDescriptor() );
        return (FilterParser<JsonNode>.Parse( filter, nodeContext ), nodeContext.RuntimeContext);
    }

    private static bool Execute( Expression expression, ParameterExpression param, Type sourceType )
    {
        if ( sourceType == typeof( JsonElement ) )
        {
            var func = Expression
                .Lambda<Func<FilterRuntimeContext<JsonElement>, bool>>( expression, param )
                .Compile();
            var descriptor = new ElementTypeDescriptor();
            return func( new FilterRuntimeContext<JsonElement>( new JsonElement(), new JsonElement(), descriptor ) );
        }

        if ( sourceType == typeof( JsonNode ) )
        {
            var func = Expression
                .Lambda<Func<FilterRuntimeContext<JsonNode>, bool>>( expression, param )
                .Compile();
            var descriptor = new NodeTypeDescriptor();
            return func( new FilterRuntimeContext<JsonNode>( new JsonObject(), new JsonObject(), descriptor ) );
        }

        throw new NotImplementedException();
    }

    private static bool CompileAndExecuteFilter( string filter, Type sourceType )
    {
        if ( sourceType == typeof( JsonElement ) )
        {
            var source = GetDocument<JsonDocument>();
            var descriptor = new ElementTypeDescriptor();
            var func = FilterParser<JsonElement>.Compile( filter, descriptor );

            return func( new FilterRuntimeContext<JsonElement>( source.RootElement, source.RootElement, descriptor ) );
        }
        else
        {
            // arrange 
            var source = GetDocument<JsonNode>();
            var descriptor = new NodeTypeDescriptor();
            var func = FilterParser<JsonNode>.Compile( filter, descriptor );

            // act
            return func( new FilterRuntimeContext<JsonNode>( source, source, descriptor ) );
        }
    }

    private static float Select( string filter, Type sourceType )
    {
        if ( sourceType == typeof( JsonElement ) )
        {
            // arrange 
            var source = GetDocument<JsonDocument>();

            // act
            return source.Select( filter ).First().GetSingle();
        }
        else
        {
            // arrange 
            var source = GetDocument<JsonNode>();

            // act
            return source.Select( filter ).First().GetValue<float>();
        }
    }
}
