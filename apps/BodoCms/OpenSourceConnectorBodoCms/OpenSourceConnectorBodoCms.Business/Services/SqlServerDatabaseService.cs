// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Database.Interfaces;
using Bodoconsult.Database.SqlClient;
using OpenSourceConnectorBodoCms.Business.Interfaces;

namespace OpenSourceConnectorBodoCms.Business.Services;

/// <summary>
/// Current implementation of <see cref="IDatabaseService"/> using a SqlServer instance
/// </summary>
public class SqlServerDatabaseService : IDatabaseService
{
    private readonly IConnManager _clientConnManager;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="connectionString">Connection string to use</param>
    public SqlServerDatabaseService(string connectionString)
    {
        ConnectionString = connectionString;
        _clientConnManager = SqlClientConnManager.GetConnManager(ConnectionString);
    }

    /// <summary>
    /// Current connection string
    /// </summary>
    public string ConnectionString { get; }

    /// <summary>
    /// Update the data of a folder in the database
    /// </summary>
    /// <param name="path">Folder path</param>
    /// <param name="name">Folder name</param>
    public void UpdateFolder(string path, string name)
    {
        var sql = $"UPDATE [dbo].[tFolder] SET [FO_Name1]='{name}' WHERE [FO_Name]='{path}'";
        _clientConnManager.Exec(sql);
    }
}