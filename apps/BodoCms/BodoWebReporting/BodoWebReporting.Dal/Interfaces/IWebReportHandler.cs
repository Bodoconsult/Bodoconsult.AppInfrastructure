// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal.Interfaces;

public interface IWebReportHandler
{
    /// <summary>
    /// The kind of reporting to be created
    /// </summary>
    ReportingTargets? Target { get; set; }

    /// <summary>
    /// Steps
    /// </summary>
    int Steps { get; set; }

    /// <summary>
    /// Start the export tasks for all files
    /// </summary>
    /// <returns>Number of reports created</returns>
    int StartExport();

    /// <summary>
    /// Start a single report export to a file task
    /// </summary>
    void StartExportReportFile(ReportingFile file, int id);

    /// <summary>
    /// Wait for all export tasks
    /// </summary>
    void WaitForExportsDone();

    /// <summary>
    /// Copy the reports to their final targets
    /// </summary>
    void CopyToTarget();
}