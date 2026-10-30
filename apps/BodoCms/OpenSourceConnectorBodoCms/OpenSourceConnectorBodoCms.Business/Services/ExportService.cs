// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using OpenSourceConnectorBodoCms.Business.App;
using OpenSourceConnectorBodoCms.Business.Exporter;
using OpenSourceConnectorBodoCms.Business.Helpers;
using OpenSourceConnectorBodoCms.Business.Interfaces;
using OpenSourceConnectorBodoCms.Business.Model;
using System.Diagnostics;
using System.Text;

namespace OpenSourceConnectorBodoCms.Business.Services;

/// <summary>
/// Current implementation of <see cref="IExportService"/> exporting folder metadata to BodoCms
/// </summary>
public class ExportService : IExportService
{
    private readonly string _targetPath;
    private readonly IDatabaseService _databaseService;
    private readonly IOpenSourceConnectorBodoCmsAppGlobals _globals;

    /// <summary>
    ///  Categories. Public only for unit testing
    /// </summary>
    public readonly List<string> Categories = new();

    /// <summary>
    ///  Packages. Public only for unit testing
    /// </summary>
    public readonly Dictionary<string, FolderData> Packages = new();

    /// <summary>
    ///  Categories with packages. Public only for unit testing
    /// </summary>
    public readonly Dictionary<string, List<string>> CategoriePackages = new();

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
        _globals = globals;

        Folders = globals.OpenSourceFolders;


        _targetPath = globals.TargetPath;
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
    /// Prepare the data for the export
    /// </summary>
    public void PrepareData()
    {
        Categories.AddRange(_globals.Categories.OrderBy(x => x)); 

        foreach (var folder in Folders)
        {
            foreach (var package in folder.NugetPackets)
            {
                Debug.Print(package);
                Packages.TryAdd(package, folder);
            }

            foreach (var meta in folder.MetaData)
            {
                if (CategoriePackages.TryGetValue(meta.Category, out var category))
                {
                    category.Add(meta.NugetPackage);
                }
                else
                {
                    CategoriePackages.Add(meta.Category, [meta.NugetPackage]);
                }
            }
        }
    }

    /// <summary>
    /// Create folders
    /// </summary>
    public void CreateFolders()
    {
        if (!Directory.Exists(_targetPath))
        {
            Directory.CreateDirectory(_targetPath);
        }

        foreach (var folder in Packages.Keys)
        {
            var folderName = folder.Replace(".", "_", StringComparison.OrdinalIgnoreCase);

            Status($"Create folder {folderName}...");

            var target = Path.Combine(_targetPath, folderName);

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
        foreach (var folder in Packages)
        {
            Status($"Create HTML for folder {folder.Key}...");

            var folderName = folder.Key.Replace(".", "_", StringComparison.OrdinalIgnoreCase);
            var dir = Path.Combine(_targetPath, folderName);
            var target = Path.Combine(dir, "index.inc");

            var exp = new HtmlExporter(folder.Key, folder.Value, target);
            exp.AddNugetLink();
            exp.AddGitHubLink();
            exp.AddContentLinks();
            exp.Save();

            _databaseService.UpdateFolder(dir, folderName);

            //var sql = $"UPDATE [dbo].[tFolder] SET [FO_Name1]='{folder.Name}' WHERE [FO_Name]='{dir}'";
            //db.Exec(sql);
        }
    }

    /// <summary>
    /// Create start page
    /// </summary>
    public void CreateStartpage()
    {
        var s = new StringBuilder();

        var html = TextHelper.GetTextResource("Startpage");

        var flyoutGroup = TextHelper.GetTextResource("FlyoutGroup");

        var flyoutItem = TextHelper.GetTextResource("FlyoutItem");

        var fileUrl = TextHelper.GetTextResource("FileUrl");

        for (var index = 0; index < Categories.Count; index++)
        {
            var category = Categories[index];
            var content = new StringBuilder();

            if (!CategoriePackages.TryGetValue(category, out var packages))
            {
                continue;
            }

            foreach (var package in packages)
            {
                content.AppendLine(fileUrl.Replace("{0}", package)
                    .Replace("{1}", package.Replace(".", "_")));
            }

            s.AppendLine(flyoutItem.Replace("{0}", content.ToString())
                .Replace("{1}", index.ToString())
                .Replace("{2}", category));
        }

        var fileName = Path.Combine(_targetPath, "index.inc");
        File.WriteAllText(fileName, html.Replace("{0}", flyoutGroup.Replace("{0}", s.ToString())), Encoding.UTF8);
    }

    private void Status(string msg)
    {
        StatusMessageDelegate.Invoke(msg);
    }


    private struct CategoryData
    {



    }
}