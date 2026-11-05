// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Charting.Base.Interfaces;

namespace BodoWebReporting.Dal.Model;

public class ReportingFileParts
{
    public bool Landscape{ get; set; }
    public string BackgroundImagePath { get; set; }

    public string LogoPath { get; set; }
    public string Title1 { get; set; }

    public string Subject { get; set; }
    public string Sql { get; set; }
    public string AddInfoSql { get; set; }
    public string ReportType { get; set; }
    public string Name { get; set; }
    public bool ShowInToc { get; set; }
    public string Title { get; set; }
    public string ChartColumns { get; set; }
    public string ChartColumnsDataTable { get; set; }
    public string ChartYUnit { get; set; }
    public short ChartLegendLength { get; set; }
    public ChartType ChartType { get; set; }
}