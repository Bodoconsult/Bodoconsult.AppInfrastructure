// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for bench data services for benchmark viewer https://github.com/crep4ever/benchmark-viewer
/// </summary>
public interface IAppBenchProxy : IDisposable
{

    /// <summary>
    /// LogStart public method
    /// </summary>
    /// <param name="key"></param>
    /// <param name="comment"></param>
    public void LogStart(string key, string comment);

    /// <summary>
    /// LogStep public method
    /// </summary>
    /// <param name="key"></param>
    /// <param name="comment"></param>
    public void LogStep(string key, string comment);

    /// <summary>
    /// LogStop public method
    /// </summary>
    /// <param name="task"></param>
    /// <param name="comment"></param>
    public void LogStop(string task, string comment);



    /// <summary>
    /// Start the logging
    /// </summary>
    void StartLogging();

    /// <summary>
    /// Stop the logging
    /// </summary>
    void StopLogging();
}