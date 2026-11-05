// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Data;
using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal.Interfaces;

/// <summary>
/// Interface for data handling for BodoWebReporting
/// </summary>
public interface IDataHandler
{
    /// <summary>
    /// Get a data reader
    /// </summary>
    /// <param name="sql">SQL to run</param>
    /// <returns>IDataReader as result from SQL</returns>
    IDataReader GetDataReader(string sql);

    /// <summary>
    /// Get a data table
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    DataTable GetDataTable(string sql);

    void Exec(string sql);

    /// <summary>
    /// Get <see cref="WebTable"/> instances for a certain target path
    /// </summary>
    /// <param name="targetPath">Target path</param>
    /// <returns>List with <see cref="WebTable"/> instances</returns>
    IList<WebTable> GetWebTables(string targetPath);

    /// <summary>
    /// Get the WebTable related copy targets
    /// </summary>
    /// <param name="fileName">Filename</param>
    /// <param name="id">WebTable database ID</param>
    /// <returns>List with <see cref="CopyTask"/> instances</returns>
    IList<CopyTask> GetWebTableTargets(string fileName, int id);

    /// <summary>
    /// Get reporting files
    /// </summary>
    /// <param name="targetPath">Target path</param>
    /// <returns>List with <see cref="ReportingFile"/> instances</returns>
    IList<ReportingFile> GetReportingFiles(string targetPath);

    /// <summary>
    /// Send an error mail
    /// </summary>
    /// <param name="subject">subject for the mail</param>
    /// <param name="message">Mail message</param>
    void ErrorHandling(string subject, string message);

    /// <summary>
    /// Get webcharts for a target path
    /// </summary>
    /// <param name="targetPath">Target path</param>
    /// <returns>List with  <see cref="WebChart"/> instances</returns>
    IList<WebChart> GetWebCharts(string targetPath);

    IList<CopyTask> GetReportingTargets(string targetPath);

    /// <summary>
    /// Get the copy targets for webcharts
    /// </summary>
    /// <param name="sourcePath">Source path</param>
    /// <param name="includePath">Include path</param>
    /// <param name="webChartId">WebChart database ID</param>
    /// <returns>List with copy tasks</returns>
    IList<CopyTask> GetWebChartTargets(string sourcePath, string includePath, int webChartId);
}