// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace BodoWebReporting.Dal.Interfaces;

/// <summary>
/// Interface for the main business logic
/// </summary>
public interface IWebReportingHandler
{
    /// <summary>
    /// Current directory
    /// </summary>
    string CurrentDir { get; }

    /// <summary>
    /// Check the target directory
    /// </summary>
    void CheckTargetPath();

    /// <summary>
    /// Create web tables
    /// </summary>
    /// <returns>O on success else error code</returns>
    int CreateWebTables();

    /// <summary>
    /// Create web charts
    /// </summary>
    int CreateWebCharts();

    /// <summary>
    /// Create web reports
    /// </summary>
    /// <returns>O on success else error code</returns>
    int CreateWebReports();

    /// <summary>
    /// Run the post run SQL statement
    /// </summary>
    void RunPostRunSql();

    /// <summary>
    /// Run the pre run SQL statement
    /// </summary>
    void RunPreRunSql();
}