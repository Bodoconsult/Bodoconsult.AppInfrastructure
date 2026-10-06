// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Database.Dbase.DbReader;
using System.Collections.Generic;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.Database.Dbase.Interfaces;

/// <summary>
/// Map record to entity delegate
/// </summary>
/// <typeparam name="TData">Type of entity to return</typeparam>
/// <param name="dbf">DBase data table</param>
/// <param name="record">Current record to map</param>
/// <returns>Entity of the requested type</returns>
public delegate TData MapToEntityDelegate<out TData>(DbfResult dbf, List<string> record);

/// <summary>
/// Interface for Dbase file import tasks
/// </summary>
/// <typeparam name="TData"></typeparam>
public interface IFileImportTask<TData> where TData : class, new()
{
    /// <summary>
    /// Delegate to map a record to an entity
    /// </summary>
    public MapToEntityDelegate<TData> MapToEntityDelegate { get; }

    /// <summary>
    /// Current logger
    /// </summary>
    IAppLoggerProxy Log { get; }

    /// <summary>
    /// Name of the file to import
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Current folder path the file to import is in
    /// </summary>
    public string FolderPath { get; }

    /// <summary>
    /// The full path to file to import
    /// </summary>
    string FullPath { get; }

    /// <summary>
    /// Weight of the task
    /// 0 = task runs in front of all others
    /// 1 = low data load
    /// 2 = medium data load
    /// 3 = high data load
    /// </summary>
    int Weight { get; set; }

    /// <summary>
    /// Use SQL bulk insert instead of EF. Default: false
    /// </summary>
    bool UseBulkInsert { get; set; }

    /// <summary>
    /// Execute the import task
    /// </summary>
    void Execute();

    /// <summary>
    /// Store the read data from file into target. This method should be called batchwise by <see cref="Execute"/> 
    /// </summary>
    void StoreData(List<TData> data);

    /// <summary>
    /// Does the DBF file exist
    /// </summary>
    /// <returns>True if the DBF exists else false</returns>
    bool FileExists { get; }

    /// <summary>
    /// Count errors
    /// </summary>
    int Errors { get; set; }

    /// <summary>
    /// Count warnings
    /// </summary>
    int Warnings { get; set; }
}