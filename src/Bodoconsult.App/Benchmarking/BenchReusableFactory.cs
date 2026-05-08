// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Benchmarking;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BufferPool;

namespace Bodoconsult.App.Benchmarking;

/// <summary>
/// Factory for <see cref="BenchReusable"/> instances using an internal buffer to avoid garbage pressure
/// </summary>
public class BenchReusableFactory
{
    private readonly BufferPool<BenchReusable> _pool= new(() => new BenchReusable());

    /// <summary>
    /// Default ctor
    /// </summary>
    public BenchReusableFactory()
    {
        _pool.Allocate(10);
    }

    /// <summary>
    /// Create a fresh <see cref="BenchReusable"/> instance
    /// </summary>
    /// <param name="proxy">Logger proxy</param>
    /// <param name="key">Key to identify the nechmarked object in the CSV file or Benchmark Viewer</param>
    /// <param name="comment">Your comment if required</param>
    /// <param name="autoStart">Start automatically</param>
    public BenchReusable CreateInstance(IAppBenchProxy proxy, string key, string comment = null, bool autoStart = true)
    {
        var result = _pool.Dequeue();
        result.Initialize(proxy, key, comment, autoStart);
        return result;
    }

    /// <summary>
    /// Pre-allocate a certain number of <see cref="BenchReusable"/> instances stored in the pool
    /// </summary>
    /// <param name="numberOfInstances">Number of <see cref="BenchReusable"/> instances stored in the pool</param>
    public void Allocate(int numberOfInstances)
    {
        _pool.Allocate(numberOfInstances);
    }

    /// <summary>
    /// Queue a used <see cref="BenchReusable"/>> back to the buffer pool
    /// </summary>
    /// <param name="buffer">Reusable object to store in the pool</param>
    public void Enqueue(BenchReusable buffer)
    {
        _pool.Enqueue(buffer);
    }
}