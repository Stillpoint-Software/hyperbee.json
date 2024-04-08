using System.Collections.Concurrent;

// synchronously execute an async method on the current thread using a custom synchronization context
// https://devblogs.microsoft.com/pfxteam/await-synchronizationcontext-and-console-apps/

// nameof(AsyncCurrentThreadHelper.RunSync) executes all the async operations on the calling thread
// by leveraging a custom nameof(SynchronizationContext). This prevents blocking the calling thread
// and waiting on the result of a Task.Run which can result in poor performance and thread
// starvation. this method only ever uses the calling thread to manage the async calls vs tying up
// thread pool threads.

namespace Hyperbee.Json.Extensions;

internal static class AsyncCurrentThreadHelper
{
    public static void RunSync( Func<Task> func )
    {
        ArgumentNullException.ThrowIfNull( func );

        var currentContext = SynchronizationContext.Current;
        var syncContext = new SingleThreadSynchronizationContext();

        try
        {
            SynchronizationContext.SetSynchronizationContext( syncContext );
            syncContext.InternalRunSync( func );
        }
        finally
        {
            SynchronizationContext.SetSynchronizationContext( currentContext );
        }
    }

    public static T RunSync<T>( Func<Task<T>> func )
    {
        ArgumentNullException.ThrowIfNull( func );

        var currentContext = SynchronizationContext.Current;
        var syncContext = new SingleThreadSynchronizationContext();

        try
        {
            SynchronizationContext.SetSynchronizationContext( syncContext );
            return syncContext.InternalRunSync( func );
        }
        finally
        {
            SynchronizationContext.SetSynchronizationContext( currentContext );
        }
    }

    private sealed class SingleThreadSynchronizationContext : SynchronizationContext
    {
        private readonly BlockingCollection<(SendOrPostCallback callback, object state)> _items = new();

        public void InternalRunSync( Func<Task> func )
        {
            var task = func() ?? throw new InvalidOperationException( "No task provided." );
            task.ContinueWith( _ => Complete(), TaskScheduler.Default ); // final continuation

            RunOnCurrentThread();

            task.GetAwaiter().GetResult();
        }

        public T InternalRunSync<T>( Func<Task<T>> func )
        {
            var task = func() ?? throw new InvalidOperationException( "No task provided." );
            task.ContinueWith( _ => Complete(), TaskScheduler.Default );

            RunOnCurrentThread();

            return task.GetAwaiter().GetResult();
        }

        public override void Post( SendOrPostCallback callback, object state )
        {
            // this method receives posted continuations from the task.
            // we add them to our queue for execution by the run loop. 

            ArgumentNullException.ThrowIfNull( callback );

            _items.Add( (callback, state) );
        }

        public override void Send( SendOrPostCallback callback, object state )
        {
            throw new NotSupportedException( "Synchronous sending is not supported." );
        }

        private void RunOnCurrentThread()
        {
            foreach ( var (callback, state) in _items.GetConsumingEnumerable() )
                callback( state );
        }

        private void Complete() => _items.CompleteAdding();
    }
}