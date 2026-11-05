// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal.Interfaces;

public interface IWebReportHandler
{
    ReportingTargets? Target { get; set; }
    FileOptions? FileOption { get; set; }

    int StartExport();

    /// <summary>
    /// Export aller Berichte einer Reportdatei
    /// </summary>
    void StartExportReportFile(ReportingFile file, int id);

    void MoveToTarget();
}