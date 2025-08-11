using System;
using Hyperbee.Json.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Core
{
    [TestClass]
    public class SliceSyntaxHelperTests
    {
        [TestMethod]
        public void TestValidSliceExpression()
        {
            var result = SliceSyntaxHelper.ParseExpression( "1:5:2".AsSpan(), 10 );
            Assert.AreEqual( (1, 5, 2), result );
        }

        [TestMethod]
        public void TestDefaultStep()
        {
            var result = SliceSyntaxHelper.ParseExpression( "1:5".AsSpan(), 10 );
            Assert.AreEqual( (1, 5, 1), result );
        }

        [TestMethod]
        public void TestNegativeStep()
        {
            var result = SliceSyntaxHelper.ParseExpression( "5:1:-1".AsSpan(), 10 );
            Assert.AreEqual( (1, 5, -1), result );
        }

        [TestMethod]
        public void TestReverseOrder()
        {
            var result = SliceSyntaxHelper.ParseExpression( "1:5:2".AsSpan(), 10, reverse: true );
            Assert.AreEqual( (-1, 3, -2), result );
        }

        [TestMethod]
        public void TestInvalidSliceExpression()
        {
            Assert.ThrowsExactly<InvalidOperationException>( () => SliceSyntaxHelper.ParseExpression( "1:2:3:4".AsSpan(), 10 ) );
        }

        [TestMethod]
        public void TestEmptySliceExpression()
        {
            var result = SliceSyntaxHelper.ParseExpression( "".AsSpan(), 10 );
            Assert.AreEqual( (0, 10, 1), result );
        }

        [TestMethod]
        public void TestStepZero()
        {
            var result = SliceSyntaxHelper.ParseExpression( "1:5:0".AsSpan(), 10 );
            Assert.AreEqual( (0, 0, 0), result );
        }

        [TestMethod]
        public void TestNegativeIndices()
        {
            var result = SliceSyntaxHelper.ParseExpression( "-5:-1:1".AsSpan(), 10 );
            Assert.AreEqual( (5, 9, 1), result );
        }

        [TestMethod]
        public void TestLargeStep()
        {
            var result = SliceSyntaxHelper.ParseExpression( "1:5:100".AsSpan(), 10 );
            Assert.AreEqual( (1, 5, 10), result );
        }

        [TestMethod]
        public void TestLargeNegativeStep()
        {
            var result = SliceSyntaxHelper.ParseExpression( "5:1:-100".AsSpan(), 10 );
            Assert.AreEqual( (1, 5, -10), result );
        }
    }
}
