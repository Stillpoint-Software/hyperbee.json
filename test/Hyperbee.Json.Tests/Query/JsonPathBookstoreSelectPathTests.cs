using System.Linq;
using System.Text.Json;
using Hyperbee.Json.Extensions;
using Hyperbee.Json.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Query
{
    [TestClass]
    public class JsonPathBookstoreSelectPathTests : JsonTestBase
    {
        [DataTestMethod]
        [DataRow( "$.store.book[*].author" )]
        public void TheAuthorsOfAllBooksInTheStore( string query )
        {
            var source = GetDocument<JsonElement>();
            var matches = source.SelectPath( query ).ToList();

            var expected = new[]
            {
                PathNodePair(source, "$.store.book[0].author"),
                PathNodePair(source, "$.store.book[1].author"),
                PathNodePair(source, "$.store.book[2].author"),
                PathNodePair(source, "$.store.book[3].author")
            };

            Assert.IsTrue( expected.Select( e => e.Node ).SequenceEqual( matches.Select( x => x.Node ) ) );
            Assert.IsTrue( expected.Select( e => e.Path ).SequenceEqual( matches.Select( x => x.Path ) ) );
        }

        [DataTestMethod]
        [DataRow( "$..author" )]
        public void AllAuthors( string query )
        {
            var source = GetDocument<JsonElement>();
            var matches = source.SelectPath( query ).ToList();

            var expected = new[]
            {
                PathNodePair(source, "$.store.book[0].author"),
                PathNodePair(source, "$.store.book[1].author"),
                PathNodePair(source, "$.store.book[2].author"),
                PathNodePair(source, "$.store.book[3].author")
            };

            Assert.IsTrue( expected.Select( e => e.Node ).SequenceEqual( matches.Select( x => x.Node ) ) );
            Assert.IsTrue( expected.Select( e => e.Path ).SequenceEqual( matches.Select( x => x.Path ) ) );
        }

        [DataTestMethod]
        [DataRow( "$.store.*" )]
        public void AllThingsInStoreWhichAreSomeBooksAndOneRedBicycle( string query )
        {
            var source = GetDocument<JsonElement>();
            var matches = source.SelectPath( query ).ToList();

            var expected = new[]
            {
                PathNodePair(source, "$.store.book"),
                PathNodePair(source, "$.store.bicycle")
            };

            Assert.IsTrue( expected.Select( e => e.Node ).SequenceEqual( matches.Select( x => x.Node ) ) );
            Assert.IsTrue( expected.Select( e => e.Path ).SequenceEqual( matches.Select( x => x.Path ) ) );
        }

        [DataTestMethod]
        [DataRow( "$.store..price" )]
        public void ThePriceOfEverythingInTheStore( string query )
        {
            var source = GetDocument<JsonElement>();
            var matches = source.SelectPath( query ).ToList();

            var expected = new[]
            {
                PathNodePair(source, "$.store.book[0].price"),
                PathNodePair(source, "$.store.book[1].price"),
                PathNodePair(source, "$.store.book[2].price"),
                PathNodePair(source, "$.store.book[3].price"),
                PathNodePair(source, "$.store.bicycle.price")
            };

            Assert.IsTrue( expected.Select( e => e.Node ).SequenceEqual( matches.Select( x => x.Node ) ) );
            Assert.IsTrue( expected.Select( e => e.Path ).SequenceEqual( matches.Select( x => x.Path ) ) );
        }

        [DataTestMethod]
        [DataRow( "$..book[2]" )]
        public void TheThirdBook( string query )
        {
            var source = GetDocument<JsonElement>();
            var match = source.SelectPath( query ).ToList();

            var expected = PathNodePair( source, "$.store.book[2]" );

            Assert.IsTrue( match.Count == 1 );
            Assert.AreEqual( expected.Node, match[0].Node );
            Assert.AreEqual( expected.Path, match[0].Path );
        }

        [DataTestMethod]
        [DataRow( "$..book[-1:]" )]
        public void TheLastBookInOrder( string query )
        {
            var source = GetDocument<JsonElement>();
            var match = source.SelectPath( query ).Single();

            var expected = PathNodePair( source, "$.store.book[3]" );

            Assert.AreEqual( expected.Node, match.Node );
            Assert.AreEqual( expected.Path, match.Path );
        }

        [DataTestMethod]
        [DataRow( "$..book[:2]" )]
        [DataRow( "$..book[0,1]" )]
        [DataRow( "$.store.book[0,1]" )]
        public void TheFirstTwoBooks( string query )
        {
            var source = GetDocument<JsonElement>();
            var matches = source.SelectPath( query ).ToList();

            var expected = new[]
            {
                PathNodePair(source, "$.store.book[0]"),
                PathNodePair(source, "$.store.book[1]")
            };

            Assert.IsTrue( expected.Select( e => e.Node ).SequenceEqual( matches.Select( x => x.Node ) ) );
            Assert.IsTrue( expected.Select( e => e.Path ).SequenceEqual( matches.Select( x => x.Path ) ) );
        }

        [DataTestMethod]
        [DataRow( "$..book['category','author']" )]
        public void TheCategoriesAndAuthorsOfAllBooks( string query )
        {
            var source = GetDocument<JsonElement>();
            var matches = source.SelectPath( query ).ToList();

            var expected = new[]
            {
                PathNodePair(source, "$.store.book[0].category"),
                PathNodePair(source, "$.store.book[1].category"),
                PathNodePair(source, "$.store.book[2].category"),
                PathNodePair(source, "$.store.book[3].category"),
                PathNodePair(source, "$.store.book[0].author"),
                PathNodePair(source, "$.store.book[1].author"),
                PathNodePair(source, "$.store.book[2].author"),
                PathNodePair(source, "$.store.book[3].author")
            };

            Assert.IsTrue( expected.Select( e => e.Node ).SequenceEqual( matches.Select( x => x.Node ) ) );
            Assert.IsTrue( expected.Select( e => e.Path ).SequenceEqual( matches.Select( x => x.Path ) ) );
        }

        [DataTestMethod]
        [DataRow( "$..book[?@.isbn]" )]
        [DataRow( "$..book[?(@.isbn)]" )]
        public void FilterAllBooksWithIsbnNumber( string query )
        {
            var source = GetDocument<JsonElement>();
            var matches = source.SelectPath( query ).ToList();

            var expected = new[]
            {
                PathNodePair(source, "$.store.book[2]"),
                PathNodePair(source, "$.store.book[3]")
            };

            Assert.IsTrue( expected.Select( e => e.Node ).SequenceEqual( matches.Select( x => x.Node ) ) );
            Assert.IsTrue( expected.Select( e => e.Path ).SequenceEqual( matches.Select( x => x.Path ) ) );
        }

        [DataTestMethod]
        [DataRow( "$..book[?(@.price<10)]" )]
        [DataRow( "$..book[?@.price<10]" )]
        public void FilterAllBooksCheaperThan10( string query )
        {
            var source = GetDocument<JsonElement>();
            var matches = source.SelectPath( query ).ToList();

            var expected = new[]
            {
                PathNodePair(source, "$.store.book[0]"),
                PathNodePair(source, "$.store.book[2]")
            };

            Assert.IsTrue( expected.Select( e => e.Node ).SequenceEqual( matches.Select( x => x.Node ) ) );
            Assert.IsTrue( expected.Select( e => e.Path ).SequenceEqual( matches.Select( x => x.Path ) ) );
        }

        [DataTestMethod]
        [DataRow( "$..*" )]
        public void AllMembersOfJsonStructure( string query )
        {
            var source = GetDocument<JsonElement>();
            var matches = source.SelectPath( query ).ToList();

            var expected = new[]
            {
                PathNodePair(source, "$.store"),
                PathNodePair(source, "$.store.book"),
                PathNodePair(source, "$.store.bicycle"),
                PathNodePair(source, "$.store.book[0]"),
                PathNodePair(source, "$.store.book[1]"),
                PathNodePair(source, "$.store.book[2]"),
                PathNodePair(source, "$.store.book[3]"),
                PathNodePair(source, "$.store.book[0].category"),
                PathNodePair(source, "$.store.book[0].author"),
                PathNodePair(source, "$.store.book[0].title"),
                PathNodePair(source, "$.store.book[0].price"),
                PathNodePair(source, "$.store.book[1].category"),
                PathNodePair(source, "$.store.book[1].author"),
                PathNodePair(source, "$.store.book[1].title"),
                PathNodePair(source, "$.store.book[1].price"),
                PathNodePair(source, "$.store.book[2].category"),
                PathNodePair(source, "$.store.book[2].author"),
                PathNodePair(source, "$.store.book[2].title"),
                PathNodePair(source, "$.store.book[2].isbn"),
                PathNodePair(source, "$.store.book[2].price"),
                PathNodePair(source, "$.store.book[3].category"),
                PathNodePair(source, "$.store.book[3].author"),
                PathNodePair(source, "$.store.book[3].title"),
                PathNodePair(source, "$.store.book[3].isbn"),
                PathNodePair(source, "$.store.book[3].price"),
                PathNodePair(source, "$.store.bicycle.color"),
                PathNodePair(source, "$.store.bicycle.price")
            };

            Assert.IsTrue( expected.Select( e => e.Node ).SequenceEqual( matches.Select( x => x.Node ) ) );
            Assert.IsTrue( expected.Select( e => e.Path ).SequenceEqual( matches.Select( x => x.Path ) ) );
        }

        [DataTestMethod]
        [DataRow( @"$..book[?(@.price == 8.99 && @.category == ""fiction"")]" )]
        [DataRow( @"$..book[?@.price == 8.99 && @.category == ""fiction""]" )]
        public void FilterAllBooksUsingLogicalAndInScript( string query )
        {
            var source = GetDocument<JsonElement>();
            var match = source.SelectPath( query ).Single();

            var expected = PathNodePair( source, "$.store.book[2]" );

            Assert.AreEqual( expected.Node, match.Node );
            Assert.AreEqual( expected.Path, match.Path );
        }

        [DataTestMethod]
        [DataRow( @"$..book[?@.price == 8.99 && (@.category == ""fiction"")]" )]
        public void FilterWithUnevenParentheses( string query )
        {
            var source = GetDocument<JsonElement>();
            var match = source.SelectPath( query ).Single();

            var expected = PathNodePair( source, "$.store.book[2]" );

            Assert.AreEqual( expected.Node, match.Node );
            Assert.AreEqual( expected.Path, match.Path );
        }

        // Helper method to return a tuple of the path and the node at that path
        private static (string Path, JsonElement Node) PathNodePair( JsonElement source, string path )
        {
            return (path, source.FromJsonPathPointer( path ));
        }
    }
}
