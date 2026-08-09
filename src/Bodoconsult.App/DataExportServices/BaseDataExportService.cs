// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;
using System.Text;
using Bodoconsult.App.Abstractions.BufferPool;

// ReSharper disable InconsistentlySynchronizedField

namespace Bodoconsult.App.DataExportServices;

/// <summary>
/// Base class for data export services
/// </summary>
/// <typeparam name="T">Type of the class to export</typeparam>
public abstract class BaseDataExportService<T> : IDataExportService<T> where T : class
{
    private readonly Lock _cacheLock = new();
    private long _currentFileSize;
    private readonly Lock _currentFileSizeLock = new();
    private readonly List<ReadOnlyMemory<byte>> _cache = [];
    private byte _flushCounter;
    private readonly Func<T, ReadOnlyMemory<byte>> _toMemoryFunc;

    private AutoResetEvent? _closeEvent;

    private readonly ProducerConsumerQueueAsync<MemoryStream> _storingQueue = new()
    {
        ThreadPriority = ThreadPriority.AboveNormal
    };
    private readonly MemoryStreamBufferPool _storeDataBufferPool = new();

    private FileStream? _currentFileStream;

    /// <summary>
    /// Is the export service started
    /// </summary>
    protected bool IsStarted;
    /// <summary>
    /// Lock object for <see cref="IsStarted"/>
    /// </summary>
    protected readonly Lock IsStartedLock = new();

    /// <summary>
    /// The caching queue to add the data to for writing it to the file
    /// </summary>
    protected readonly CachingProducerConsumerQueue2<ReadOnlyMemory<byte>> CachingQueue = new();

    private int _cacheSize = 1000;

    /// <summary>
    /// Default ctor
    /// </summary>
    protected BaseDataExportService()
    {
        CachingQueue.ConsumerTaskDelegate = AddDataToCache;
        _storingQueue.ConsumerTaskDelegate = AddCacheToStoring;

        _storeDataBufferPool.Allocate(100);
        _toMemoryFunc = ToMemory;
    }

    /// <summary>
    /// Ctor supplying an encoding
    /// </summary>
    protected BaseDataExportService(Encoding encoding)
    {
        Encoding = encoding;

        CachingQueue.ConsumerTaskDelegate = AddDataToCache;
        _storingQueue.ConsumerTaskDelegate = AddCacheToStoring;

        _storeDataBufferPool.Allocate(100);
        _toMemoryFunc = ToMemory;
    }

    /// <summary>
    /// Flush to disk interval
    /// </summary>
    public byte FlushInterval { get; set; } = 5;

    /// <summary>
    /// Thread priority
    /// </summary>
    public ThreadPriority ThreadPriority { get; set; } = ThreadPriority.Normal;

    /// <summary>
    /// Encoding to use for string based exports like XML, JSON etc.
    /// </summary>
    public Encoding Encoding { get; set; } = Encoding.Unicode;

    /// <summary>
    /// Counts the rows since the service was started
    /// </summary>
    public ulong RowCounter { get; private set; }

    /// <summary>
    /// Counts the arrived rows since the service was started
    /// </summary>
    public ulong RowCounter2 { get; private set; }

    /// <summary>
    /// Maximum file size before rolling to next file. Default: 10 MB
    /// </summary>
    public long MaxFileSize { get; set; } = 10000000;

    /// <summary>
    /// Cache size as number of T instances to cache before saving to file. Default: 10
    /// </summary>
    public int CacheSize
    {
        get => _cacheSize;
        set
        {
            _cacheSize = value;
            CachingQueue.CacheSize = _cacheSize;
        }
    }

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
    public string CurrentFilePath { get; set; } = string.Empty;

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
        CurrentFilePath = CreateCurrentFilePath();

        _storingQueue.ThreadPriority = ThreadPriority;
        _storingQueue.StartConsumer();
        CachingQueue.StartConsumer();

        //Debug.Print($"Start: {CurrentFilePath}: {_currentFileSize} byte");
        StoreCacheToStoringQueue(FileState.Start);

        lock (IsStartedLock)
        {
            IsStarted = true;
        }
    }

    /// <summary>
    /// Flush the cache to disk
    /// </summary>
    public void FlushCache()
    {
        CachingQueue.Flush();
    }

    /// <summary>
    /// Save all data and then stop the data export
    /// </summary>
    public void Stop()
    {
        lock (IsStartedLock)
        {
            IsStarted = false;
        }

        FlushCache();

        _currentFileStream?.Flush(true);

        _closeEvent = new AutoResetEvent(false);
        _closeEvent.WaitOne(10000);
        _closeEvent.Reset();

        //Debug.Print($"Cache {_cache.Count} Storing {_storingQueue.InternalQueue.Count}");

        //Debug.Print($"Stop: {CurrentFilePath}: {_currentFileSize} byte");
        StoreCacheToStoringQueue(FileState.Finalize);

        _closeEvent.WaitOne(500);

        CachingQueue.StopConsumer();
        _storingQueue.StopConsumer();

        // Now finalize the last filestream
        if (_currentFileStream is null)
        {
            return;
        }

        _currentFileStream.Flush(true);

        // Add a footer now if required
        if (FooterData != null)
        {
            _currentFileStream.Write(FooterData.Value.Span);
        }

        _currentFileStream.Close();
        _currentFileStream.Dispose();
    }

    private void StoreCacheToStoringQueue(FileState fileState)
    {
        List<ReadOnlyMemory<byte>> data;

        // Keep the lock as short as possible
        lock (_cacheLock)
        {
            data = _cache.ToList();
            _cache.Clear();
        }

        if (data.Count == 0)
        {
            return;
        }

        var ms = _storeDataBufferPool.Dequeue();

        // Now write the data tokens
        var separator = TokenSeparatorData != null ? TokenSeparatorData.Value.Span : default;

        var last = data.Count - 1;
        for (var index = 0; index <= last; index++)
        {
            // Write data token
            var memory = data[index];

            lock (_currentFileSizeLock)
            {
                _currentFileSize += memory.Length;
            }

            //Debug.Print(ArrayHelper.GetStringFromArray(memory));

            ms.Write(memory.Span);

            // Add separator if required
            if (fileState == FileState.Finalize && index < last && separator.Length > 0)
            {
                ms.Write(separator);
            }

            if (RowCounter == ulong.MaxValue)
            {
                RowCounter = 0;
            }
            RowCounter++;
        }

        if (ms.Length <= 0)
        {
            return;
        }

        _storingQueue.Enqueue(ms);
    }

    /// <summary>
    /// Add an item to store in the export file
    /// </summary>
    /// <param name="data"></param>
    public void Add(T data)
    {
        lock (IsStartedLock)
        {
            if (!IsStarted)
            {
                return;
            }
        }

        var rm = ToMemory(data);
        CachingQueue.Enqueue(rm);
    }

    /// <summary>
    /// Add a list of items to store in the export file
    /// </summary>
    /// <param name="data">List of data items to store</param>
    public void AddRange(IEnumerable<T> data)
    {
        lock (IsStartedLock)
        {
            if (!IsStarted)
            {
                return;
            }
        }

        // ReSharper disable once ConvertClosureToMethodGroup
        var mem = data.Select(_toMemoryFunc).ToList();
        CachingQueue.Enqueue(mem);
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
            var arr = Encoding.GetBytes(s);
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
    private async Task AddCacheToStoring(MemoryStream data)
    {
        if (data.Length == 0)
        {
            CheckCancellation();
            return;
        }

        //Debug.Print($"DataExportService: Waiting {_cache.Count} Storing {_storingQueue.InternalQueue.Count}");

        if (_currentFileStream is null || _currentFileSize >= MaxFileSize)
        {
            if (_currentFileStream != null)
            {
                // Add a footer now if required
                if (FooterData != null)
                {
                    _currentFileStream.Write(FooterData.Value.Span);
                }

                _currentFileStream.Close();
                await _currentFileStream.DisposeAsync();

                CurrentFilePath = CreateCurrentFilePath();
            }

            _currentFileStream = new FileStream(CurrentFilePath, FileMode.Append, FileAccess.Write, FileShare.None);
            lock (_currentFileSizeLock)
            {
                _currentFileSize = 0;
            }

            // Add a header now if required
            if (HeaderData != null)
            {
                _currentFileStream.Write(HeaderData.Value.Span);
            }
        }

        // Debug.Print($"Count: {data.Count}");
        data.Position = 0;
        await data.CopyToAsync(_currentFileStream);
        

        _flushCounter++;
        if (_flushCounter == FlushInterval)
        {
            _flushCounter = 0;
            _currentFileStream.Flush(true);
        }
        else
        {
            await _currentFileStream.FlushAsync();
        }

        CheckCancellation();
    }

    private void CheckCancellation()
    {
        if (_closeEvent is null || _cache.Count == 0)
        {
            return;
        }

        //Debug.Print($"DataExportService: Waiting {_cache.Count} Storing {_storingQueue.InternalQueue.Count}");
        _closeEvent.Set();

    }

    /// <summary>
    /// Add data to the internal cache waiting for storing
    /// </summary>
    /// <param name="data">Current data to be stored</param>
    private void AddDataToCache(ReadOnlyMemory<byte>[] data)
    {
        lock (_cacheLock)
        {
            _cache.AddRange(data);
            if (RowCounter2 == ulong.MaxValue)
            {
                RowCounter = 0;
            }
            RowCounter2 += (ulong)data.LongLength;
        }

        // cache size is reached. Is a new file required?
        bool isNewFile;
        lock (_currentFileSizeLock)
        {
            isNewFile = _currentFileSize > MaxFileSize;
        }

        if (isNewFile)
        {
            // Last writing to current file
            //Debug.Print($"Finalize: {CurrentFilePath}: {_currentFileSize} byte");
            StoreCacheToStoringQueue(FileState.Finalize);

            // Rolling to new current file
            //Debug.Print($"Start: {CurrentFilePath}: {_currentFileSize} byte");
            StoreCacheToStoringQueue(FileState.Start);

            return;
        }

        // No new file required
        StoreCacheToStoringQueue(FileState.AddData);
    }


    private enum FileState
    {
        Start,
        AddData,
        Finalize
    }
}