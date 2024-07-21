using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.TestSupport;

public static class AssertExtensions
{
    public static void ThrowsAny<T1, T2>( Action action )
        where T1 : Exception
        where T2 : Exception
    {
        ThrowsAnyInternal( action, typeof( T1 ), typeof( T2 ) );
    }

    public static void ThrowsAny<T1, T2, T3>( Action action )
        where T1 : Exception
        where T2 : Exception
        where T3 : Exception
    {
        ThrowsAnyInternal( action, typeof( T1 ), typeof( T2 ), typeof( T3 ) );
    }

    public static void ThrowsAny<T1, T2, T3, T4>( Action action )
        where T1 : Exception
        where T2 : Exception
        where T3 : Exception
        where T4 : Exception
    {
        ThrowsAnyInternal( action, typeof( T1 ), typeof( T2 ), typeof( T3 ), typeof( T4 ) );
    }

    public static void ThrowsAny<T1, T2, T3, T4, T5>( Action action )
        where T1 : Exception
        where T2 : Exception
        where T3 : Exception
        where T4 : Exception
        where T5 : Exception
    {
        ThrowsAnyInternal( action, typeof( T1 ), typeof( T2 ), typeof( T3 ), typeof( T4 ), typeof( T5 ) );
    }

    private static void ThrowsAnyInternal( Action action, params Type[] expectedExceptionTypes )
    {
        Exception? caughtException = null;

        try
        {
            action();
        }
        catch ( Exception ex )
        {
            caughtException = ex;
        }

        if ( caughtException == null )
        {
            Assert.Fail( $"No exception was thrown. Expected one of: {string.Join( ", ", expectedExceptionTypes.Select( t => t.Name ) )}" );
        }
        else if ( !expectedExceptionTypes.Any( e => e.IsInstanceOfType( caughtException ) ) )
        {
            Assert.Fail( $"Exception of type {caughtException.GetType().Name} was thrown, but none of the expected types were: {string.Join( ", ", expectedExceptionTypes.Select( t => t.Name ) )}" );
        }
    }
}
