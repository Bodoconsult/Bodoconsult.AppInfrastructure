// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Data;

namespace BodoWebReporting.Dal.Report;

internal class SmallTableReportItem : BaseReportItem
{

    public SmallTableReportItem()
    {
        TypeOfObject = ReportItemType.SmallTable;
    }

    public DataTable Data { get; set; }

    public string Heading { get; set; }

}