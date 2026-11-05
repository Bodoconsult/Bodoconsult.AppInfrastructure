using BodoWebReporting.Dal.Exporter;

namespace BodoWebReporting.Dal.Handler;

public class WebTablesHandler
{

    public StatusHandler StatusChange;

    public string TargetPath; // { get; set; }

    public string PreRunSql; // { get; set; }

    private readonly DataHandler _dataHandler;

    public WebTablesHandler(DataHandler dataHandler)
    {
        _dataHandler = dataHandler;
    }


    public int ExportTables()
    {
        var data = _dataHandler.GetWebTables(TargetPath);

        var i = 0;

        foreach (var wt in data)

        {
            SetStatus($"WebTable {wt.Name}...");
            var wte = new WebTableExporter(_dataHandler, wt);
            wte.Export();
            wte.CopyToTarget();
            i++;

        }

        SetStatus("Exporting webtables is done...");

        return i;
    }


    public void OpenDatabase()
    {

        try
        {
            SetStatus("Check target directory...");
            SetStatus(TargetPath);
            if (!Directory.Exists(TargetPath))
            {
                Directory.CreateDirectory(TargetPath);
            }

            if (!string.IsNullOrEmpty(PreRunSql)) _dataHandler.Exec(PreRunSql);

            // Daten vor Reporting aufbereiten
            _dataHandler.Exec("EXEC dbo.[spGEN_Prepare]");
            _dataHandler.Exec("EXEC dbo.[spGEN_PrepareReporting]");
        }
        catch (Exception ex)
        {
            _dataHandler.ErrorHandling( "BodoWebReporting:WebTablesHandler:OpenDatabase", ex.Message);
        }

    }


    /// <summary>
    /// Statusanzeige
    /// </summary>
    /// <param name="msg">Anzuzeigende Nachricht</param>
    private void SetStatus(string msg)
    {
        var x = StatusChange;
        x?.Invoke(msg);
    }
}