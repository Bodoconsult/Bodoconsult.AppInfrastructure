// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace BodoWebReporting.Dal.Interfaces;

/// <summary>
/// App globals for BodoWebReporting
/// </summary>
public interface IBodoWebReportingAppGlobals : IAppGlobals
{
    /// <summary>
    /// Target path for reporting items
    /// </summary>
    string TargetPath { get; set; }

    /// <summary>
    /// Run SQL statement(s) at app start
    /// </summary>
    string PreRunSql { get; set; }

    /// <summary>
    /// Run SQL statement(s) at app's runtime end
    /// </summary>
    string PostRunSql { get; set; }
}