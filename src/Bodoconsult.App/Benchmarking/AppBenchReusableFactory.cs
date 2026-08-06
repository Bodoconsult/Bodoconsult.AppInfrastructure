// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Benchmarking;
using Bodoconsult.App.Abstractions.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Benchmarking;

/// <summary>
/// Current implementation of <see cref="IAppBenchReusableFactory"/> handling one bench log file with a separate buffer pool for reusing <see cref="BenchReusable"/> instances
/// </summary>
public class AppBenchReusableFactory : IAppBenchReusableFactory
{
    private readonly BenchReusableFactory _benchReusableFactory = new();
    private readonly IAppBenchProxy _proxy;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="logger">Current logger factory</param>
    /// <param name="logDataFactory">Current central <see cref="ILogDataFactory"/> instance</param>
    public AppBenchReusableFactory(ILoggerFactory logger, ILogDataFactory logDataFactory)
    {
        Logger = logger;
        LogDataFactory = logDataFactory;

        _proxy = new AppBenchProxy(logger, logDataFactory);
    }

    /// <summary>
    /// Current logger factory
    /// </summary>
    public ILoggerFactory Logger { get; }

    /// <summary>
    /// Current central <see cref="ILogDataFactory"/> instance
    /// </summary>
    public ILogDataFactory LogDataFactory { get; }

    /// <summary>
    /// Create a fresh <see cref="BenchReusable"/> instance
    /// </summary>
    /// <param name="key">Key to identify the nechmarked object in the CSV file or Benchmark Viewer</param>
    /// <param name="comment">Your comment if required</param>
    /// <param name="autoStart">Start automatically</param>
    public BenchReusable CreateInstance(string key, string? comment = null, bool autoStart = true)
    {
        return _benchReusableFactory.CreateInstance(_proxy, key, comment, autoStart);
    }

    /// <summary>
    /// Pre-allocate a certain number of <see cref="BenchReusable"/> instances stored in the pool
    /// </summary>
    /// <param name="numberOfInstances">Number of <see cref="BenchReusable"/> instances stored in the pool</param>
    public void Allocate(int numberOfInstances)
    {
        _benchReusableFactory.Allocate(numberOfInstances);
    }

    /// <summary>
    /// Queue a used <see cref="BenchReusable"/> instance back to the buffer pool
    /// </summary>
    /// <param name="buffer">Reusable <see cref="BenchReusable"/> instance to store in the pool</param>
    public void Enqueue(BenchReusable buffer)
    {
        _benchReusableFactory.Enqueue(buffer);
    }
}