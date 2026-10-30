// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using OpenSourceConnectorBodoCms.Business.Interfaces;

namespace OpenSourceConnectorBodoCms.Business.Services;

/// <summary>
/// Current fake implementation of <see cref="IDatabaseService"/>
/// </summary>
public class FakeDatabaseService : IDatabaseService
{
    /// <summary>
    /// Current connection string
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Update the data of a folder in the database
    /// </summary>
    /// <param name="path">Folder path</param>
    /// <param name="name">Folder name</param>
    public void UpdateFolder(string path, string name)
    {
        var sql = $"UPDATE [dbo].[tFolder] SET [FO_Name1]='{name}' WHERE [FO_Name]='{path}'";
        Debug.Print(sql);
    }
}