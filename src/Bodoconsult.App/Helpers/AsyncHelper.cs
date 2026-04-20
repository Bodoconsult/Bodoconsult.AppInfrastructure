// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Extensions;

namespace Bodoconsult.App.Helpers;

/// <summary>
/// Helper class for async management
/// </summary>
public static class AsyncHelper
{
    private static readonly TaskFactory MyTaskFactory = new(CancellationToken.None, 
            TaskCreationOptions.None, 
            TaskContinuationOptions.None, 
            TaskScheduler.Default);

    /// <summary>
    /// Run an async method call in a syncron manner
    /// </summary>
    /// <typeparam name="TResult">Type of return value of the method call</typeparam>
    /// <param name="func">Method call</param>
    /// <returns>Return value of the method</returns>
    public static TResult RunSync<TResult>(Func<Task<TResult>> func)
    {
        return MyTaskFactory
            .StartNew(func)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }

    /// <summary>
    /// Run an async method call in a syncron manner
    /// </summary>
    /// <param name="func">Method call</param>
    public static void RunSync(Func<Task> func)
    {
        MyTaskFactory
            .StartNew(func)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }

    /// <summary>
    /// Fire and forget an action. Task is returned to have the chance to add ContinueWith if required.
    /// </summary>
    /// <param name="action">Action to fire and forget</param>
    /// <returns>Task of the fired action</returns>
    public static Task FireAndForget2(Action action)
    {
        return Task.Run(action);
    }

    /// <summary>
    /// Fire and forget an action.
    /// </summary>
    /// <param name="action">Action to fire and forget</param>
    /// <returns>Task of the fired action</returns>
    public static void FireAndForget(Action action)
    {
        Task.Run(action).Forget();
    }

    /// <summary>
    /// Create a waiting task, then wait and at the end return the result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="taskCompletionSource"><see cref="TaskCompletionSource&lt;T&gt;"/> to be handled by the consumer of the waiting task</param>
    /// <returns>Result of the waiting task</returns>
    public static T CreateWaitingTask<T>(out TaskCompletionSource<T> taskCompletionSource)
    {
        // Now wait
        var result = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        taskCompletionSource = result;

        // Wait
        var taskResult = RunSync(() => result.Task);
        taskCompletionSource = null;

        return taskResult;
    }

    /// <summary>
    /// Delay the current task
    /// </summary>
    /// <param name="milliSeconds">Milliseconds to delay the task</param>
    public static void Delay(int milliSeconds)
    {
        var t = Task.Run(async delegate
        {
            await Task.Delay(milliSeconds);
            return 42;
        });
        t.Wait();
    }

    /// <summary>
    /// Delay the current task
    /// </summary>
    /// <param name="milliSeconds">Milliseconds to delay the task</param>
    /// <param name="cancellationToken">Current cancellation token</param>
    public static void Delay(int milliSeconds, CancellationToken cancellationToken)
    {
        var t = Task.Run(async delegate
        {
            await Task.Delay(milliSeconds, cancellationToken);
            return 42;
        }, cancellationToken);
        t.Wait(cancellationToken);
    }
}
