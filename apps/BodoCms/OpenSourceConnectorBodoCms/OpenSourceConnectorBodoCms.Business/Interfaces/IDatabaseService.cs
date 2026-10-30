// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace OpenSourceConnectorBodoCms.Business.Interfaces;

public interface IDatabaseService
{
    /// <summary>
    /// Current connection string
    /// </summary>
    string ConnectionString { get; }

    /// <summary>
    /// Update the data of a folder in the database
    /// </summary>
    /// <param name="path">Folder path</param>
    /// <param name="name">Folder name</param>
    void UpdateFolder(string path, string name);
}