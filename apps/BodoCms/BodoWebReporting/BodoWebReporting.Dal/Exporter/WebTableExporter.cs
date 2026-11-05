using System.Data;
using Bodoconsult.Office;
using Bodoconsult.Web.Html.Html;
using BodoWebReporting.Dal.Handler;
using BodoWebReporting.Dal.Helpers;
using BodoWebReporting.Dal.Interfaces;
using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal.Exporter;

/// <summary>
/// Export reporting data to HTML fragment files with data structured in HTML-Table
/// </summary>
public class WebTableExporter
{
    private readonly IDataHandler _dataHandler;
    private readonly WebTable _webTable;

    public WebTableExporter(IDataHandler dataHandler, WebTable webTable)
    {
        _dataHandler = dataHandler;
        _webTable = webTable;
        Data = _dataHandler.GetDataTable(_webTable.Sql);
    }

    public DataTable Data { get; }

    /// <summary>
    /// Export the WebTable to the export directory
    /// </summary>
    public void Export()
    {
        try
        {
            switch (_webTable.TableType.ToLower())
            {
                case "xlsx":
                    var e = new XlsxBuilder();
                    //e.Status += StatusMsg;
                    e.NewWorkbook(_webTable.FileName);

                    if (e.ErrorCode != 0)
                    {
                        return;
                    }

                    e.NewSheet("Daten");
                    e.Header(_webTable.Name);
                    e.FillDataTable(Data, 4, 1);
                    e.Quit();
                    break;
                default:
                    var ex = new DataTableExport
                    {
                        CssTable = "wr_table",
                        CssTableCell = "wr_cell",
                        CssAlternatingRows = "_alt",
                        CssTableHeader = "wr_header",
                        Data = Data
                    };
                    ex.CreateTable();
                    ex.SaveAsFile(_webTable.FileName);
                    break;
            }
        }
        catch (Exception ex)
        {
            OnErrorHandlerFired($"BodoWebReporting:WebTables:Export:Id={_webTable.Id}", ex.Message);
        }
    }

    /// <summary>
    /// Copy the WebTable from the export directory to target paths
    /// </summary>
    public void CopyToTarget()
    {

        var ct = _dataHandler.GetWebTableTargets(_webTable.FileName, _webTable.Id);

        var errors = CopyTaskHelper.RunAllTasks(ct);

        if (!errors.Any()) return;

        var msg = errors.Aggregate("", (current, error) => current + "<br/>+" + error);

        _dataHandler.ErrorHandling("Error:WebTableExporter:CopyToTarget", $"<p>{msg}</p>");


        //try
        //{
        //    var df = _dataHandler.GetDataReader($"EXEC dbo.spWeb_Tables_Targets_Get {_webTable.Id}");
        //    while (df.Read())
        //    {

        //        var target = Helper.Parser(df["WTT_TargetPath"].ToString());
        //        try
        //        {
        //            //if (File.Exists(target)) File.Delete(target);
        //            File.Copy(_webTable.FileName, target, true);
        //        }
        //        catch (System.Exception ex)
        //        {
        //            OnErrorHandlerFired($"BodoWebReporting:WebTables:CopyToTarget:Id={_webTable.Id},Target={target}", ex.Message);
        //        }
        //    }
        //}
        //catch (System.Exception ex)
        //{
        //    OnErrorHandlerFired($"BodoWebReporting:WebTables:CopyToTarget:Id={_webTable.Id}", ex.Message);
        //}
    }

    protected void OnErrorHandlerFired(string subject, string msg)
    {
        _dataHandler.ErrorHandling(subject, msg);
    }
}