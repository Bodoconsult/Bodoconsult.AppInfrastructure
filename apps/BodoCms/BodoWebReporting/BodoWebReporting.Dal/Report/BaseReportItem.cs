namespace BodoWebReporting.Dal.Report;

internal class BaseReportItem
{
    public ReportItemType TypeOfObject { get; set; }

    /// <summary>
    /// Add a page break after this item
    /// </summary>
    public bool PageBreakAfter { get; set; }
}