using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Evaluators;
using Hyperbee.Json.Evaluators.Parser;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Evaluators;

[TestClass]
public class JsonPathExpressionTests : JsonTestBase
{
    [DataTestMethod]
    [DataRow( "true", true, typeof( JsonElement ) )]
    [DataRow( "false", false, typeof( JsonElement ) )]
    [DataRow( "1 == 1", true, typeof( JsonElement ) )]
    [DataRow( "(1 == 1)", true, typeof( JsonElement ) )]
    [DataRow( "(1 != 2)", true, typeof( JsonElement ) )]
    [DataRow( "!(1 == 2)", true, typeof( JsonElement ) )]
    [DataRow( "(\"world\" == 'world') && (true || false)", true, typeof( JsonElement ) )]
    [DataRow( "(\"world\" == 'world') || true", true, typeof( JsonElement ) )]
    [DataRow( "(\"world\" == 'world') || 1 == 1", true, typeof( JsonElement ) )]
    [DataRow( "!('World' != 'World') && !(1 == 2 || 1 == 3)", true, typeof( JsonElement ) )]
    [DataRow( "true", true, typeof( JsonNode ) )]
    [DataRow( "false", false, typeof( JsonNode ) )]
    [DataRow( "1 == 1", true, typeof( JsonNode ) )]
    [DataRow( "(1 == 1)", true, typeof( JsonNode ) )]
    [DataRow( "(1 != 2)", true, typeof( JsonNode ) )]
    [DataRow( "!(1 == 2)", true, typeof( JsonNode ) )]
    [DataRow( "(\"world\" == 'world') && (true || false)", true, typeof( JsonNode ) )]
    [DataRow( "(\"world\" == 'world') || true", true, typeof( JsonNode ) )]
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
    [DataRow( "@.store.bicycle.price < 10", false, typeof( JsonElement ) )]
    [DataRow( "@.store.bicycle.price > 15", true, typeof( JsonElement ) )]
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
        var result = CompileAndExecute( filter, sourceType );

        // assert
        Assert.AreEqual( expected, result );
    }

    [DataTestMethod]
    [DataRow( "$.store.book[?(@.price > 20)].price", 22.99F, typeof( JsonElement ) )]
    [DataRow( "$.store.book[?(@.category == 'reference')].price", 8.95F, typeof( JsonElement ) )]
    [DataRow( "$.store.book[?(@.price < 9.00 && @.category == 'reference')].price", 8.95F, typeof( JsonElement ) )]
    [DataRow( "$.store.book[?(match(@.title, \"Sayings*\" ))].price", 8.95F, typeof( JsonElement ) )]
    [DataRow( "$.store.book[?(@.category == $.store.book[0].category)].price", 8.95F, typeof( JsonElement ) )]
    [DataRow( "$.store.book[?(@.price > 20)].price", 22.99F, typeof( JsonNode ) )]
    [DataRow( "$.store.book[?(@.category == 'reference')].price", 8.95F, typeof( JsonNode ) )]
    [DataRow( "$.store.book[?(@.price < 9.00 && @.category == 'reference')].price", 8.95F, typeof( JsonNode ) )]
    [DataRow( "$.store.book[?(match(@.title, \"Sayings*\" ))].price", 8.95F, typeof( JsonNode ) )]
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
    [DataRow( "match(@.store.book[0].title, \"Sayings*\" )", true, typeof( JsonElement ) )]
    [DataRow( "search(@.store.book[0].author, \"[Nn]igel Rees\" )", true, typeof( JsonElement ) )]
    [DataRow( "value(@.store.book[0].author) == \"Nigel Rees\"", true, typeof( JsonElement ) )]
    [DataRow( "count(@.store.book) == 1", true, typeof( JsonNode ) )]
    [DataRow( "count(@.store.book.*) == 4", true, typeof( JsonNode ) )]
    [DataRow( "length(@.store.book) == 4", true, typeof( JsonNode ) )]
    [DataRow( "length(@.store.book[0].category) == 9", true, typeof( JsonNode ) )]
    [DataRow( "match(@.store.book[0].title, \"Sayings*\" )", true, typeof( JsonNode ) )]
    [DataRow( "search(@.store.book[0].author, \"[Nn]igel Rees\" )", true, typeof( JsonNode ) )]
    [DataRow( "value(@.store.book[0].author) == \"Nigel Rees\"", true, typeof( JsonNode ) )]
    public void Should_MatchExpectedResult_WhenUsingFunctions( string filter, bool expected, Type sourceType )
    {
        // arrange & act
        var result = CompileAndExecute( filter, sourceType );

        // assert
        Assert.AreEqual( expected, result );
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
        var param = Expression.Parameter( sourceType );
        var expression = sourceType == typeof( JsonElement )
            ? JsonPathExpression.Parse( filter, new ParseExpressionContext<JsonElement>(
                param,
                param,
                new JsonPathExpressionElementEvaluator() ) )
            : JsonPathExpression.Parse( filter, new ParseExpressionContext<JsonNode>(
                param,
                param,
                new JsonPathExpressionNodeEvaluator() ) );

        return (expression, param);
    }

    private static bool Execute( Expression expression, ParameterExpression param, Type sourceType )
    {
        if ( sourceType == typeof( JsonElement ) )
        {
            var func = Expression
                .Lambda<Func<JsonElement, bool>>( expression, param )
                .Compile();

            return func( new JsonElement() );
        }
        else if ( sourceType == typeof( JsonNode ) )
        {
            var func = Expression
                .Lambda<Func<JsonNode, bool>>( expression, param )
                .Compile();

            return func( JsonNode.Parse( "{}" ) );
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    private static bool CompileAndExecute( string filter, Type sourceType )
    {
        if ( sourceType == typeof( JsonElement ) )
        {
            var source = GetDocument<JsonDocument>();
            var func = JsonPathExpression.Compile( filter, new JsonPathExpressionElementEvaluator() );

            return func( source.RootElement, source.RootElement );
        }
        else
        {
            // arrange 
            var source = GetDocument<JsonNode>();
            var func = JsonPathExpression.Compile( filter, new JsonPathExpressionNodeEvaluator() );

            // act
            return func( source, source );
        }
    }

    private static float Select( string filter, Type sourceType )
    {
        if ( sourceType == typeof( JsonElement ) )
        {
            // arrange 
            var source = GetDocument<JsonDocument>();

            // act
            return source.Select( filter, new JsonPathExpressionElementEvaluator() ).First().GetSingle();
        }
        else
        {
            // arrange 
            var source = GetDocument<JsonNode>();

            // act
            return source.Select( filter, new JsonPathExpressionNodeEvaluator() ).First().GetValue<float>();
        }
    }
}
