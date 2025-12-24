using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Hyperbee.Json.Path.Filters;
using Hyperbee.Json.Path.Filters.Parser;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Path.Parser;

[TestClass]
public class FilterParserTests : JsonTestBase
{
    [TestMethod]
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
    public void MatchExpectedResult_WhenUsingConstants( string filter, bool expected, Type sourceType )
    {
        // arrange 
        var (expression, param) = GetExpression( filter, sourceType );

        // act
        var result = ExecuteExpression( expression, param, sourceType );

        // assert
        Assert.AreEqual( expected, result );
    }

    [TestMethod]
    [DataRow( "true", typeof( JsonElement ) )]
    [DataRow( "false", typeof( JsonElement ) )]
    [DataRow( "true", typeof( JsonNode ) )]
    [DataRow( "false", typeof( JsonNode ) )]
    public void Fail_WhenNotComparingLiterals( string filter, Type sourceType )
    {
        // arrange 

        // act & assert
        Assert.ThrowsExactly<NotSupportedException>( () =>
        {
            var (expression, param) = GetExpression( filter, sourceType );
            return ExecuteExpression( expression, param, sourceType );
        } );
    }

    [TestMethod]
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
    public void MatchExpectedResult_WhenUsingJsonPath( string filter, bool expected, Type sourceType )
    {
        // arrange & act
        var result = CompileAndExecuteFilter( filter, sourceType );

        // assert
        Assert.AreEqual( expected, result );
    }

    [TestMethod]
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
    public void ReturnExpectedResult_WhenUsingExpressionEvaluator( string filter, float expected, Type sourceType )
    {
        // arrange & act
        var document = GetDocumentAdapter( sourceType );

        // act
        var matches = document.Select( filter ).ToArray();
        var result = TestHelper.GetSingle( matches[0] );

        // assert
        Assert.AreEqual( expected, result );
    }

    [DataTestMethod]
    [DataRow( "$.store.book[?(length(@.title) > 10)].title", "Sayings of the Century", typeof( JsonElement ) )]
    [DataRow( "$.store.book[?(length(@.title) > 10)].title", "Sayings of the Century", typeof( JsonNode ) )]
    public void ReturnExpectedResult_WhenUsingExpressionEvaluator( string filter, string expected, Type sourceType )
    {
        // arrange & act
        var document = GetDocumentAdapter( sourceType );

        // act
        var matches = document.Select( filter ).ToArray();
        var result = TestHelper.GetString( matches[0] );

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
    public void MatchExpectedResult_WhenUsingFunctions( string filter, bool expected, Type sourceType )
    {
        // arrange & act
        var result = CompileAndExecuteFilter( filter, sourceType );

        // assert
        Assert.AreEqual( expected, result );
    }

    [TestMethod]
    [DataRow( "length(@.store.book) == 4  ", true, typeof( JsonElement ) )]
    [DataRow( "  length(@.store.book) == 4", true, typeof( JsonElement ) )]
    [DataRow( "  length(@.store.book) == 4  ", true, typeof( JsonElement ) )]
    [DataRow( "  length( @.store.book ) == 4  ", true, typeof( JsonElement ) )]
    [DataRow( "4 == length( @.store.book )  ", true, typeof( JsonElement ) )]
    [DataRow( "  4 == length(@.store.book)", true, typeof( JsonElement ) )]
    [DataRow( "  4 == length(@.store.book)  ", true, typeof( JsonElement ) )]
    [DataRow( "  4 == length( @.store.book )  ", true, typeof( JsonElement ) )]
    public void MatchExpectedResult_WhenHasExtraSpaces( string filter, bool expected, Type sourceType )
    {
        // arrange & act
        var result = CompileAndExecuteFilter( filter, sourceType );

        // assert
        Assert.AreEqual( expected, result );
    }

    [TestMethod]
    [DataRow( "4 == length ( @.store.book )", typeof( JsonElement ) )]
    [DataRow( "length (@.store.book) == 4", typeof( JsonElement ) )]
    public void Fail_WhenHasInvalidWhitespace( string filter, Type sourceType )
    {
        Assert.ThrowsExactly<NotSupportedException>( () => CompileAndExecuteFilter( filter, sourceType ) );
    }

    [TestMethod]
    [DataRow( "unknown_literal", typeof( JsonElement ) )]
    [DataRow( "'unbalanced string\"", typeof( JsonElement ) )]
    [DataRow( " \t ", typeof( JsonElement ) )]
    [DataRow( "1 === 1", typeof( JsonElement ) )]
    [DataRow( "(1 == 1(", typeof( JsonElement ) )]
    [DataRow( "(1 == 1)(", typeof( JsonElement ) )]
    [DataRow( "(1 == ", typeof( JsonElement ) )]
    [DataRow( "== 1", typeof( JsonElement ) )]
    [DataRow( "badMethod(1)", typeof( JsonElement ) )]
    public void FailToParse_WhenUsingInvalidFilters( string filter, Type sourceType )
    {
        TestSupport.AssertExtensions.ThrowsAny<NotSupportedException, ArgumentException>( () => GetExpression( filter, sourceType ) );
    }

    // Helper methods

    public static T InvokeGenericMethod<T>( string methodName, Type sourceType, params object[] args )
    {
        try
        {
            var method = typeof( FilterParserTests )
                .GetMethods( BindingFlags.NonPublic | BindingFlags.Static )
                .First( x => x.Name == methodName && x.IsGenericMethodDefinition )
                .MakeGenericMethod( sourceType );

            return (T) method.Invoke( null, args )!;
        }
        catch ( Exception ex )
        {
            throw ex.InnerException!;
        }
    }

    public static (Expression, ParameterExpression) GetExpression( string filter, Type sourceType ) =>
        InvokeGenericMethod<(Expression, ParameterExpression)>( nameof( GetExpression ), sourceType, filter );

    public static bool CompileAndExecuteFilter( string filter, Type sourceType ) =>
        InvokeGenericMethod<bool>( nameof( CompileAndExecuteFilter ), sourceType, filter );

    public static bool ExecuteExpression( Expression expression, ParameterExpression param, Type sourceType ) =>
        InvokeGenericMethod<bool>( nameof( ExecuteExpression ), sourceType, expression, param );

    private static (Expression, ParameterExpression) GetExpression<T>( string filter )
    {
        var runtimeContext = Expression.Parameter( typeof( FilterRuntimeContext<T> ), "runtimeContext" );
        return (FilterParser<T>.Parse( filter ), runtimeContext);
    }

    private static bool ExecuteExpression<T>( Expression expression, ParameterExpression param )
    {
        var filterFunc = Expression.Lambda<Func<FilterRuntimeContext<T>, bool>>( expression, param ).Compile();
        var runtimeContext = new FilterRuntimeContext<T>( default, default ); // Initialize with default values
        return filterFunc( runtimeContext );
    }

    private static bool CompileAndExecuteFilter<T>( string filter )
    {
        var source = GetDocument<T>();
        var filterFunc = FilterParser<T>.Compile( filter );
        var runtimeContext = new FilterRuntimeContext<T>( source, source );
        return filterFunc( runtimeContext );
    }
}
