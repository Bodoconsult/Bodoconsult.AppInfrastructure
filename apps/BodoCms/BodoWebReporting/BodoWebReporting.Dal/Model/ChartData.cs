// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Data;
using Bodoconsult.Charting.Base.Interfaces;

namespace BodoWebReporting.Dal.Model;

public struct ChartData
{
    public DataTable Data;
    public string Title;
    public ChartType TypeOfChart;
    public string Columns;
    public string ColumnsDataTable;
    public int Left;
    public int LegendLength;
    public string YLabel;
}