// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace BodoWebReporting.Dal.Model;

/// <summary>
/// The kind of reporting to be created
/// </summary>
public enum ReportingTargets
{
    /// <summary>
    /// A PDF file
    /// </summary>
    PdfFile,

    /// <summary>
    /// A PDF stream
    /// </summary>
    PdfStream
}