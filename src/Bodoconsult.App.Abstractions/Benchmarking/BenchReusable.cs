// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Abstractions.Benchmarking;

/// <summary>
/// Log BenchReusable entity class for benchmark viewer https://github.com/crep4ever/benchmark-viewer
/// </summary>
public class BenchReusable : IBench, IResetable
{
    private IAppBenchProxy? _proxy;

    private bool Stopped { get; set; }
    private bool Started { get; set; }
    private string Key { get; set; } = string.Empty;


    /// <summary>
    /// Initialize the class
    /// </summary>
    /// <param name="proxy">Logger proxy</param>
    /// <param name="key">Key to identify the nechmarked object in the CSV file or Benchmark Viewer</param>
    /// <param name="comment">Your comment if required</param>
    /// <param name="autoStart">Start automatically</param>
    public void Initialize(IAppBenchProxy proxy, string key, string? comment = "", bool autoStart = true)
    {
        _proxy = proxy;
        Key = key;

        if (autoStart)
        {
            Start(comment);
        }
    }


    /// <summary>
    /// Start benchmark
    /// </summary>
    /// <param name="comment">Comment to add</param>
    public void Start(string? comment)
    {
        Started = true;
        Stopped = false;

        //write
        _proxy?.LogStart(Key, comment ?? "Start");
    }


    /// <summary>
    /// Stop benchmark
    /// </summary>
    /// <param name="comment"></param>
    public void Stop(string comment)
    {
        Stopped = true;

        _proxy?.LogStop(Key, comment);
    }

    /// <summary>
    /// Add Step benchmark
    /// </summary>
    /// <param name="comment"></param>
    public void AddStep(string comment)
    {
        if (_proxy != null && Started && !Stopped)
        {
            _proxy.LogStep(Key, comment);
        }
    }

    /// <summary>
    /// Dto. force Stop benchmark if not stoppped.
    /// It's advised to call it manually to produce the STOP trace when required, and not to wait for garbage collection...later
    /// </summary>
    public void Dispose()
    {
        if (Started && !Stopped)
        {
            Stop("");
        }
    }

    /// <summary>
    /// Reset the class to default values
    /// </summary>
    public void Reset()
    {
        _proxy = null;
        Started = false;
        Key = string.Empty;
        Stopped = false;
    }
}