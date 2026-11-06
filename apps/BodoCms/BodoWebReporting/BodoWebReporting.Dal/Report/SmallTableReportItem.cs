// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Data;

namespace BodoWebReporting.Dal.Report;

internal class SmallTableReportItem : BaseReportItem
{

    public SmallTableReportItem()
    {
        TypeOfObject = ReportItemType.SmallTable;
    }

    /// <summary>
    /// Chart or table data
    /// </summary>
    public DataTable? Data { get; set; }

    /// <summary>
    /// Heading text
    /// </summary>
    public string Heading { get; set; } = "Heading missing";

}