// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Memory based data export services
/// </summary>
public interface IMemoryDataExportService : IDataExportService<byte[]>
{
    /// <summary>
    /// Add an item to store in the export file
    /// </summary>
    /// <param name="data"></param>
    void Add(ReadOnlyMemory<byte> data);

    /// <summary>
    /// Add an item to store in the export file
    /// </summary>
    /// <param name="data">List with Memory&lt;byte&gt; elements</param>
    void AddRange(IEnumerable<ReadOnlyMemory<byte>> data);
}