// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.DataExportServices;

/// <summary>
/// Data export service for byte arrays
/// </summary>
public class ByteArrayDataExportService : BaseDataExportService<byte[]>, IMemoryDataExportService
{
    /// <summary>
    /// Converts an object of type T into a ReadOnlyMemory&lt;byte&gt; instance
    /// </summary>
    /// <param name="data">Data object to serialize</param>
    /// <returns>Byte array</returns>
    /// <exception cref="NotSupportedException">Thrown if type T is NOT string, ReadOnlyMemory&lt;byte&gt; or byte[]</exception>
    public override ReadOnlyMemory<byte> ToMemory(byte[] data)
    {
        return data;
    }

    /// <summary>
    /// Add an item to store in the export file
    /// </summary>
    /// <param name="data"></param>
    public void Add(ReadOnlyMemory<byte> data)
    {
        lock (IsStartedLock)
        {
            if (!IsStarted)
            {
                return;
            }
        }

        CachingQueue.Enqueue(data);
    }

    /// <summary>
    /// Add an item to store in the export file
    /// </summary>
    /// <param name="data">List with Memory&lt;byte&gt; elements</param>
    public void AddRange(IEnumerable<ReadOnlyMemory<byte>> data)
    {
        lock (IsStartedLock)
        {
            if (!IsStarted)
            {
                return;
            }
        }

        CachingQueue.Enqueue(data);
    }
}
