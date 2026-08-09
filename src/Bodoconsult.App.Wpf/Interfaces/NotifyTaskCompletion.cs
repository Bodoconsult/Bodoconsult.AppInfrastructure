// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.ComponentModel;

namespace Bodoconsult.App.Wpf.Interfaces;

// Source: https://learn.microsoft.com/en-us/archive/msdn-magazine/2014/march/async-programming-patterns-for-asynchronous-mvvm-applications-data-binding

/// <summary>
/// Class for binding to asnyc calls results in XAML
/// </summary>
/// <typeparam name="TResult"></typeparam>
public sealed class NotifyTaskCompletion<TResult> : INotifyPropertyChanged
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="task">Task</param>
    public NotifyTaskCompletion(Task<TResult> task)
    {
        Task = task;
        if (!task.IsCompleted)
        {
            var _ = WatchTaskAsync(task);
        }
    }
    private async Task WatchTaskAsync(Task task)
    {
        try
        {
            await task;
        }
        catch
        {
            // Do nothing
        }
        var propertyChanged = PropertyChanged;
        if (propertyChanged is null)
        {
            return;
        }
        propertyChanged(this, new PropertyChangedEventArgs("Status"));
        propertyChanged(this, new PropertyChangedEventArgs("IsCompleted"));
        propertyChanged(this, new PropertyChangedEventArgs("IsNotCompleted"));
        if (task.IsCanceled)
        {
            propertyChanged(this, new PropertyChangedEventArgs("IsCanceled"));
        }
        else if (task.IsFaulted)
        {
            propertyChanged(this, new PropertyChangedEventArgs("IsFaulted"));
            propertyChanged(this, new PropertyChangedEventArgs("Exception"));
            propertyChanged(this, new PropertyChangedEventArgs("InnerException"));
            propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
        }
        else
        {
            propertyChanged(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
            propertyChanged(this, new PropertyChangedEventArgs("Result"));
        }
    }

    /// <summary>
    /// Current task
    /// </summary>
    public Task<TResult> Task { get; }

    /// <summary>
    /// Current result
    /// </summary>
    public TResult Result =>
        Task.Status == TaskStatus.RanToCompletion ?
            Task.Result : default(TResult);

    /// <summary>
    /// Current task status
    /// </summary>
    public TaskStatus Status => Task.Status;

    /// <summary>
    /// Is the task completed?
    /// </summary>
    public bool IsCompleted => Task.IsCompleted;

    /// <summary>
    /// Is the task not completed?
    /// </summary>
    public bool IsNotCompleted => !Task.IsCompleted;

    /// <summary>
    /// Is the task successfully completed?
    /// </summary>
    public bool IsSuccessfullyCompleted =>
        Task.Status ==
        TaskStatus.RanToCompletion;

    /// <summary>
    /// Is the task cancelled
    /// </summary>
    public bool IsCanceled => Task.IsCanceled;

    /// <summary>
    /// Is the task faulted
    /// </summary>
    public bool IsFaulted => Task.IsFaulted;

    /// <summary>
    /// The exception the task threw
    /// </summary>
    public AggregateException Exception => Task.Exception;

    /// <summary>
    /// Inner exception if available
    /// </summary>
    public Exception InnerException => Exception?.InnerException;

    /// <summary>
    /// Error message if available
    /// </summary>
    public string ErrorMessage => InnerException?.Message;

    /// <summary>Occurs when a property value changes.</summary>
    public event PropertyChangedEventHandler PropertyChanged;
}