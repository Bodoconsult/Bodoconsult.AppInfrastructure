// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using OpenSourceConnectorBodoCms.Business.Exporter;
using OpenSourceConnectorBodoCms.Business.Interfaces;
using OpenSourceConnectorBodoCms.Business.Model;

namespace OpenSourceConnectorBodoCms.Business.Services;

/// <summary>
/// Current implementation of <see cref="IExportService"/> exporting folder metadata to BodoCms
/// </summary>
public class ExportService : IExportService
{
    private readonly string _targetPath;
    private readonly IList<string> _categories;
    private readonly IDatabaseService _databaseService;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current app globals</param>
    /// <param name="databaseService">Current database service</param>
    public ExportService(IAppGlobals appGlobals, IDatabaseService databaseService)
    {
        if (appGlobals is not IOpenSourceConnectorBodoCmsAppGlobals globals)
        {
            throw new ArgumentException("appGlobals is not IOpenSourceConnectorBodoCmsAppGlobals");
        }

        _databaseService = databaseService;

        Folders = globals.OpenSourceFolders;
        _targetPath = globals.TargetPath;
        _categories = globals.Categories.OrderBy(x => x).ToList();

        ArgumentNullException.ThrowIfNull(globals.StatusMessageDelegate);
        StatusMessageDelegate = globals.StatusMessageDelegate;
    }

    /// <summary>
    /// Folders to export
    /// </summary>
    public IList<FolderData> Folders { get; }

    /// <summary>
    /// Status message delelgate
    /// </summary>
    public StatusMessageDelegate StatusMessageDelegate { get; }

    /// <summary>
    /// Create folders
    /// </summary>
    public void CreateFolders()
    {
        if (!Directory.Exists(_targetPath))
        {
            Directory.CreateDirectory(_targetPath);
        }

        foreach (var folder in Folders)
        {
            Status($"Create folder {folder.Name}...");

            var target = Path.Combine(_targetPath, folder.TargetName);

            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }
        }
    }

    /// <summary>
    /// Create HTML
    /// </summary>
    public void CreateHtml()
    {
        foreach (var folder in Folders)
        {
            Status($"Create HTML for folder {folder.Name}...");

            var dir = Path.Combine(_targetPath, folder.TargetName);
            var target = Path.Combine(dir, "index.inc");

            var exp = new HtmlExporter(folder, target);
            exp.AddNugetLink();
            exp.AddGitHubLink();
            exp.AddContent();
            exp.Save();

            _databaseService.UpdateFolder(dir, folder.Name);

            //var sql = $"UPDATE [dbo].[tFolder] SET [FO_Name1]='{folder.Name}' WHERE [FO_Name]='{dir}'";
            //db.Exec(sql);
        }
    }

    /// <summary>
    /// Create start page
    /// </summary>
    public void CreateStartpage()
    {
        //var s = new StringBuilder();

        //var html = TextHelper.GetTextResource("Startpage");

        //var flyoutGroup = TextHelper.GetTextResource("FlyoutGroup");

        //var flyoutItem = TextHelper.GetTextResource("FlyoutItem");

        //var fileUrl = TextHelper.GetTextResource("FileUrl");

        //for (var index = 0; index < _categories.Count; index++)
        //{
        //    var category = _categories[index];

        //    var content = new StringBuilder();

        //    foreach (var folder in Folders.Where(x => x.MetaData.Category == category).OrderBy(x => x.Name))
        //    {
        //        content.AppendLine(fileUrl.Replace("{0}", folder.Name)
        //            .Replace("{1}", folder.Name.Replace(".", "_")));
        //    }

        //    s.AppendLine(flyoutItem.Replace("{0}", content.ToString())
        //        .Replace("{1}", index.ToString())
        //        .Replace("{2}", category));
        //}

        //var fileName = Path.Combine(_targetPath, "index.inc");
        //File.WriteAllText(fileName, html.Replace("{0}", flyoutGroup.Replace("{0}", s.ToString())), Encoding.UTF8);
    }

    private void Status(string msg)
    {
        StatusMessageDelegate?.Invoke(msg);
    }
}