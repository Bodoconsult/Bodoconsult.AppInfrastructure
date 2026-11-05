// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal.Interfaces;

/// <summary>
/// Interface for the creation process handler of <see cref="WebTable"/> instances
/// </summary>
public interface IWebTablesHandler
{
    /// <summary>
    /// Export the tables
    /// </summary>
    /// <returns></returns>
    int ExportTables();
}