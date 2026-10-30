// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using OpenSourceConnectorBodoCms.Business.Helpers;
using OpenSourceConnectorBodoCms.Business.Interfaces;
using OpenSourceConnectorBodoCms.Business.Model;

namespace OpenSourceConnectorBodoCms.Business.Services;

public class RegisterFolderService
{
    public RegisterFolderService(IAppGlobals appGlobals)
    {
        if (appGlobals is not IOpenSourceConnectorBodoCmsAppGlobals globals)
        {
            throw new ArgumentException("appGlobals is not IOpenSourceConnectorBodoCmsAppGlobals");
        }

        TargetPath = globals.TargetPath;
        FolderFilter = globals.FolderFilter;
        SourcePath = globals.SourcePath;
        FolderExcludeFilter = globals.FolderExcludeFilter;

        ArgumentNullException.ThrowIfNull(globals.StatusMessageDelegate);
        StatusMessageDelegate = globals.StatusMessageDelegate;
        OpenSourceFolders = globals.OpenSourceFolders;
        Categories = globals.Categories;
    }

    /// <summary>
    /// Status message handler
    /// </summary>
    public StatusMessageDelegate StatusMessageDelegate { get; }

    /// <summary>
    /// Source folder for open source projects
    /// </summary>
    public string SourcePath { get; }

    /// <summary>
    /// Filter expression for the folder
    /// </summary>
    public string FolderFilter { get; }

    /// <summary>
    /// Filter expression for the folder
    /// </summary>
    public string FolderExcludeFilter { get; }

    /// <summary>
    /// Publish folder
    /// </summary>
    public string TargetPath { get; }

    /// <summary>
    /// Current open source project folders
    /// </summary>
    public List<FolderData> OpenSourceFolders { get; }

    /// <summary>
    /// All categories
    /// </summary>
    public List<string> Categories { get; }

    /// <summary>
    /// Get all open source project folders
    /// </summary>
    public void GetAllProjectFolders()
    {
        var filter = FolderFilter.ToUpperInvariant();
        var excludes = FolderExcludeFilter.Split(',', StringSplitOptions.RemoveEmptyEntries);

        var folders = new DirectoryInfo(SourcePath).GetDirectories().Where(x => x.Name.Contains(filter, StringComparison.OrdinalIgnoreCase));

        Categories.Add("General");

        foreach (var folder in folders)
        {
            var excluded = false;
            foreach (var exclude in excludes)
            {
                if (!folder.Name.StartsWith(exclude, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                excluded = true;
                break;
            }

            if (excluded)
            {
                continue;
            }

            RegisterFolder(folder);
        }
    }

    /// <summary>
    /// Register a folder if relevante
    /// </summary>
    /// <param name="folder">Current folder</param>
    public void RegisterFolder(DirectoryInfo folder)
    {
        Status($"Register {folder.Name}...");

        // Check if there is a git directory
        var gitPath = Path.Combine(folder.FullName, ".git");

        var exists = Directory.Exists(gitPath);

        if (!exists)
        {
            return;
        }

        // Check if the git repository is public
        gitPath = Path.Combine(folder.FullName, ".git", "config");

        exists = File.Exists(gitPath);

        if (!exists)
        {
            return;
        }

        var content = File.ReadAllText(gitPath);

        var item = new FolderData
        {
            Name = folder.Name,
            Path = folder.FullName,
            GitUrl = GetRepositoryUrl(content)
        };

        gitPath = Path.Combine(folder.FullName, "nuget.txt");

        exists = File.Exists(gitPath);

        if (exists)
        {
            content = File.ReadAllText(gitPath).Replace("\r\n", "\r");

            var packages = content.Split('\r', StringSplitOptions.RemoveEmptyEntries);

            foreach (var package in packages)
            {
                item.NugetUrls.Add(package);
            }
        }

        // Readme
        gitPath = Path.Combine(folder.FullName, "readme.md");
        exists = File.Exists(gitPath);
        if (exists)
        {
            content = File.ReadAllText(gitPath);
            item.ReadMe = content;
        }

        // Licence
        gitPath = Path.Combine(folder.FullName, "license.md");
        exists = File.Exists(gitPath);
        if (exists)
        {
            content = File.ReadAllText(gitPath);
            item.Licence = content;
        }

        // Metadata
        GetMetaData(folder, item);

        // Add the item now to folders list
        OpenSourceFolders.Add(item);
    }

    private void GetMetaData(DirectoryInfo folder, FolderData item)
    {
        var path = Path.Combine(folder.FullName, "OpenSourceMetaData.json");
        var exists = File.Exists(path);

        var metas = new List<OpenSourceMetaData>();

        // A meta data file with potentially user updated info is existing
        if (exists)
        {
            metas.AddRange(JsonHelper.LoadJsonFile<List<OpenSourceMetaData>>(path));
        }

        // Now try to find missing packages from solution folder \packages
        var packagesPath = Path.Combine(folder.FullName, "packages");
        
        var packages = GetPackagesFromFolder(packagesPath);

        foreach (var package in packages)
        {
            if (metas.Any(x => x.NugetPackage.Equals(package)))
            {
                continue;
            }

            metas.Add(new OpenSourceMetaData
            {
                NugetPackage = package
            });
        }

        // Save meta data
        JsonHelper.SaveAsFile(path, metas);

        // Now handle the meta datas
        foreach (var meta in metas)
        {
            item.NugetUrls.Add(meta.NugetPackage);
            CheckCategorie(meta.Category);
        }
    }

    /// <summary>
    /// Get all unique package names in a package folder
    /// </summary>
    /// <param name="packagesPath">Package folder path</param>
    /// <returns>List with unique package names</returns>
    public static List<string> GetPackagesFromFolder(string packagesPath)
    {
        var result = new List<string>();

        if (Directory.Exists(packagesPath))
        {
            return result;
        }


        var files = new DirectoryInfo(packagesPath).GetFiles("*.nupkg");

        foreach (var file in files)
        {
            var packageName = GetPackageFromFileName(file);

            if (result.Contains(packageName))
            {
                continue;
            }

            result.Add(packageName);
        }

        return result;
    }

    /// <summary>
    /// Extract the package from a filename
    /// </summary>
    /// <param name="fi">Filename with extension *.nupkg</param>
    /// <returns></returns>
    public static string GetPackageFromFileName(FileInfo fi)
    {
        if (!fi.Extension.Equals(".nupkg"))
        {
            return string.Empty;
        }

        var s = fi.Name;

        var i = s.LastIndexOf('.');
        i = s.LastIndexOf('.', i - 1);
        i = s.LastIndexOf('.', i - 1);
        i = s.LastIndexOf('.', i - 1);

        return s.Substring(0, i);
    }


    private void CheckCategorie(string categorie)
    {
        if (Categories.Any(x => x == categorie))
        {
            return;
        }

        Categories.Add(categorie);
    }



    public string GetRepositoryUrl(string url)
    {
        var s = url.Split('\r', '\n');

        var item = s.FirstOrDefault(x => x.Contains("url = "));

        if (string.IsNullOrEmpty(item)) return "";

        var erg = item.Trim().Replace("\t", "").Replace("url = ", "").Trim();

        if (erg.EndsWith(".git"))
        {
            erg = erg.Substring(0, erg.Length - 4);
        }

        return erg;
    }


    private void Status(string msg)
    {
        StatusMessageDelegate?.Invoke(msg);
    }
}