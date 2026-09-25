// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using BodoFtpTransfer.Business.Model;
using BodoFtpTransfer.Business.Services;
using System.Collections.Generic;
using System.Data.Common;

namespace BodoFtpTransfer.Business.Interfaces;

/// <summary>
/// Interface for database services nacking the FTP transfer
/// </summary>
public interface IDatabaseService
{
    /// <summary>
    /// Current status message delegate
    /// </summary>
    StatusMessageDelegate StatusMessageDelegate { get; }

    /// <summary>
    /// Connection string
    /// </summary>
    string ConnectionString { get;  }

    /// <summary>
    /// Start command collecting for saving db connections
    /// </summary>
    void StartCommandCollecting();

    /// <summary>
    /// Run all the collected commands on one connection
    /// </summary>
    void RunCommandsFromCollection();

    /// <summary>
    /// Insert a data row into table Files from entity class Files object
    /// </summary>
    void AddNew(FtpFiles item);

    /// <summary>
    /// Update a data row in table Files from an entity class Files object
    /// </summary>
    void Update(FtpFiles item);

    /// <summary>
    /// Delete a row from table Files 
    /// </summary>
    void Delete(long id);

    /// <summary>
    /// Get all rows in table Files
    /// </summary>
    IList<FtpFiles> GetAll();

    /// <summary>
    /// Get all rows in table Files
    /// </summary>
    FtpFiles GetById(long pkId);

    /// <summary>
    /// Count all rows in table Files 
    /// </summary>
    int Count();

    /// <summary>
    /// Get file paths
    /// </summary>
    /// <returns>List with <see cref="FilePathItem"/> items</returns>
    IList<FilePathItem> GetFilePaths();

    /// <summary>
    /// Reset existing file data to start point
    /// </summary>
    void UpdateFiles();

    /// <summary>
    /// Update an existing remote file with its size
    /// </summary>
    /// <param name="file"></param>
    /// <param name="size"></param>
    void UpdateExistingRemoteFile(FilePathItem file, in long size);

    /// <summary>
    /// Update an existing local file with its size
    /// </summary>
    /// <param name="file">File data in the database</param>
    /// <param name="size">New size of the file</param>
    void UpdateExistingLocalFile(FilePathItem file, in long size);

    DbCommand SetHashForFileCmd(string path, string hash);
    void SetHashForRemoteFiles();
    IList<FilePathItem> GetRemoteFilePathToCreate();

    /// <summary>
    /// 
    /// </summary>
    void ClearFiles();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IList<FilePathItem> GetRemoteFilePathRemoveFiles();

    IList<FilePathItem> GetRemoteFilePathToRemove();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IList<FilePathItem> GetLocalFilesToCopy();

    void SetHashCodeForRemoteFile(string path);

    /// <summary>
    /// Execute a list of commands
    /// </summary>
    /// <param name="cmds">List with commands to execute</param>
    void Exec(List<DbCommand> cmds);
    void LoadAllFilePaths(IList<FilePathItem> allFiles);
}