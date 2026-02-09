// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;
using Bodoconsult.App.BufferPool;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Interfaces;
// ReSharper disable InconsistentlySynchronizedField

namespace Bodoconsult.App.DataExportServices;

/// <summary>
/// Base class for data export services
/// </summary>
/// <typeparam name="T">Type of the class to export</typeparam>
public abstract class DataExportServiceBase<T> : IDataExportService<T> where T : class
{
    private bool _isStarted;
    private readonly Lock _isStartedLock = new();
    private readonly Lock _cacheLock = new();
    private long _currentFileSize;
    private readonly List<ReadOnlyMemory<byte>> _cache = new();
    private readonly ProducerConsumerQueue2<ReadOnlyMemory<byte>> _cachingQueue = new();
    private readonly ProducerConsumerQueue<MemoryStream> _storingQueue = new();
    private readonly MemoryStreamBufferPool _memoryStreamBufferPool = new();

    /// <summary>
    /// Default ctor
    /// </summary>
    protected DataExportServiceBase()
    {
        _cachingQueue.ConsumerTaskDelegate = AddDataToCache;
        _storingQueue.ConsumerTaskDelegate = AddCacheToStoring;

        _memoryStreamBufferPool.Allocate(10);
    }

    /// <summary>
    /// Counts the rows since the service was started
    /// </summary>
    public int RowCounter { get; private set; }

    /// <summary>
    /// Maximum file size before rolling to next file. Default: 10 MB
    /// </summary>
    public long MaxFileSize { get; set; } = 10000000;

    /// <summary>
    /// Cache size as number of T instances to cache before saving to file
    /// </summary>
    public int CacheSize { get; set; } = 1000;

    /// <summary>
    /// The directory path for the export target. Default: Path.GetTempPath();
    /// </summary>
    public string TargetPath { get; set; } = Path.GetTempPath();

    /// <summary>
    /// The plain filename for the export file without extension, timestamp etc.
    /// </summary>
    public string FileName { get; set; } = "DataExport";

    /// <summary>
    /// Pattern for the full filename including timestamp etc.. Default: "{0}_{1}.{2}";
    /// {0} FileName
    /// {1} Timestamp
    /// {2} FileExtension
    /// </summary>
    public string FileNamePattern { get; set; } = "{0}_{1}.{2}";

    /// <summary>
    /// File extension to use for the export files without dot. Default: txt
    /// </summary>
    public string FileExtension { get; set; } = "txt";

    /// <summary>
    /// The current file path the data are stored in
    /// </summary>
    public string CurrentFilePath { get; set; }

    /// <summary>
    /// Header data to add at the start of the file. Mainly intended for XML or JSON
    /// </summary>
    public ReadOnlyMemory<byte>? HeaderData { get; set; }

    /// <summary>
    /// Footer data to add at the end of the file. Mainly intended for XML or JSON
    /// </summary>
    public ReadOnlyMemory<byte>? FooterData { get; set; }

    /// <summary>
    /// Byte data separating tokens in the file. Default: null
    /// </summary>
    public ReadOnlyMemory<byte>? TokenSeparatorData { get; set; }

    /// <summary>
    /// Create the current file path
    /// </summary>
    /// <returns>Current file path</returns>
    public string CreateCurrentFilePath()
    {
        return Path.Combine(TargetPath, string.Format(FileNamePattern, FileName, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff"), FileExtension));
    }

    /// <summary>
    /// Start the data export
    /// </summary>
    public void Start()
    {
        _cachingQueue.StartConsumer();
        _storingQueue.StartConsumer();
        CurrentFilePath = CreateCurrentFilePath();
        StoreStreamToStoringQueue(FileState.Start);

        lock (_isStartedLock)
        {
            _isStarted = true;
        }
    }

    /// <summary>
    /// Save all data and then stop the data export
    /// </summary>
    public void Stop()
    {
        lock (_isStartedLock)
        {
            _isStarted = false;
        }

        Thread.Sleep(500);
        
        StoreStreamToStoringQueue(FileState.Finalize);
       

        _cachingQueue.StopConsumer();
        _storingQueue.StopConsumer();
    }

    private void StoreStreamToStoringQueue(FileState fileState)
    {
        List<ReadOnlyMemory<byte>> data;

        // Keep the lock as short as possible
        lock (_cacheLock)
        {
            data = _cache.ToList();
            _cache.Clear();
        }

        var ms = _memoryStreamBufferPool.Dequeue();

        // Add a header now if required
        if (fileState == FileState.Start)
        {
            // Add a footer if required
            if (HeaderData != null)
            {
                ms.Write(HeaderData.Value.Span);
            }
        }

        // Now write the data tokens
        var separator = TokenSeparatorData != null ? TokenSeparatorData.Value.Span : default;

        var last = data.Count - 1;
        for (var index = 0; index <= last; index++)
        {
            // Write data token
            var memory = data[index];
            ms.Write(memory.Span);

            // Add separator if required
            if (index < last && separator.Length > 0)
            {
                ms.Write(separator);
            }

            RowCounter++;
        }

        // Add a footer now if required
        if (fileState == FileState.Finalize)
        {
            // Add a footer if required
            if (FooterData != null)
            {
                ms.Write(FooterData.Value.Span);
            }
        }

        _storingQueue.Enqueue(ms);
    }

    /// <summary>
    /// Add an item to store in the export file
    /// </summary>
    /// <param name="data"></param>
    public void Add(T data)
    {
        lock (_isStartedLock)
        {
            if (!_isStarted)
            {
                return;
            }
        }

        var rm = ToMemory(data);
        _cachingQueue.Enqueue(rm);
    }

    /// <summary>
    /// Converts an object of type T into a ReadOnlyMemory&lt;byte&gt; instance
    /// </summary>
    /// <param name="data">Data object to serialize</param>
    /// <returns>Byte array</returns>
    /// <exception cref="NotSupportedException">Thrown if type T is NOT string, ReadOnlyMemory&lt;byte&gt; or byte[]</exception>
    public virtual ReadOnlyMemory<byte> ToMemory(T data)
    {
        if (data is ReadOnlyMemory<byte> m)
        {
            return m;
        }

        if (data is string s)
        {
            var arr = Encoding.UTF8.GetBytes(s);
            return arr;
        }

        if (data is byte[] rs)
        {
            return rs.AsMemory();
        }

        throw new NotSupportedException("Implemented your own conversion by overriding this method");
    }

    /// <summary>
    /// Add data to the storing queue
    /// </summary>
    /// <param name="data">Current data to be stored</param>
    private void AddCacheToStoring(MemoryStream data)
    {
        // Debug.Print($"Count: {data.Count}");
        using var stream = new FileStream(CurrentFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
        data.Position = 0;
        data.CopyTo(stream);
        stream.Close();

        _memoryStreamBufferPool.Enqueue(data);
    }

    /// <summary>
    /// Add data to the internal cache waiting for storing
    /// </summary>
    /// <param name="data">Current data to be stored</param>
    private void AddDataToCache(ReadOnlyMemory<byte> data)
    {
        lock (_cacheLock)
        {
            if (_currentFileSize > MaxFileSize)
            {
                // Last writing to current file
                StoreStreamToStoringQueue(FileState.Finalize);

                // Rolling to new current file
                CurrentFilePath = CreateCurrentFilePath();
                _currentFileSize = 0;

                StoreStreamToStoringQueue(FileState.Start);
            }

            _cache.Add(data);
            _currentFileSize += data.Length;

            if (_cache.Count < CacheSize)
            {
                return;
            }

            StoreStreamToStoringQueue(FileState.AddData);
        }
    }

    private enum FileState
    {
        Start,
        AddData,
        Finalize
    }
}