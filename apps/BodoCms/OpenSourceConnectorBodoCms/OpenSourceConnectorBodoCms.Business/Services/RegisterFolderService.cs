// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;
using OpenSourceConnectorBodoCms.Business.Interfaces;
using OpenSourceConnectorBodoCms.Business.Model;

namespace OpenSourceConnectorBodoCms.Business.Services;

public class RegisterFolderService : IRegisterFolderService
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

            // Git repo
            if (CheckIfFolderIsNoGitRepo(folder))
            {
                return;
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

        // Check if the git repository is public
        var gitPath = Path.Combine(folder.FullName, ".git", "config");

        var exists = File.Exists(gitPath);

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

        GetPackages(folder, item);

        // Metadata
        GetMetaData(folder, item);

        // Add the item now to folders list
        OpenSourceFolders.Add(item);
    }

    private static bool CheckIfFolderIsNoGitRepo(DirectoryInfo folder)
    {
        var gitPath = Path.Combine(folder.FullName, ".git");

        var exists = Directory.Exists(gitPath);
        return !exists;
    }

    /// <summary>
    /// Get the packages from nuget.txt
    /// </summary>
    /// <param name="folder">Current directory</param>
    /// <param name="item">Current folder data</param>
    private static void GetPackages(DirectoryInfo folder, FolderData item)
    {
        var gitPath = Path.Combine(folder.FullName, "nuget.txt");

        var exists = File.Exists(gitPath);

        if (!exists)
        {
            return;
        }

        var content = File.ReadAllText(gitPath).Replace("\r\n", "\r", StringComparison.OrdinalIgnoreCase);

        var packages = content.Split('\r', StringSplitOptions.RemoveEmptyEntries);

        foreach (var package in packages)
        {
            if (string.IsNullOrEmpty(package))
            {
                continue;
            }
            item.NugetPackets.Add(package);
        }
    }

    private void GetMetaData(DirectoryInfo folder, FolderData item)
    {
        var path = Path.Combine(folder.FullName, "OpenSourceMetaData.json");
        var exists = File.Exists(path);

        var metas = new List<OpenSourceMetaData>();

        // A meta data file with potentially user updated info is existing
        if (exists)
        {
            try
            {
                var list = SimpleJsonHelper.LoadJsonFile<List<OpenSourceMetaData>>(path);
                if (list is not null)
                {
                    metas.AddRange(list);
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
            }
        }

        // Now add missing packages from nuget.txt
        foreach (var package in item.NugetPackets)
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
        SimpleJsonHelper.SerializeAsFile(path, metas);

        // Now handle the meta datas
        foreach (var meta in metas)
        {
            Debug.Print(meta.Category);
            item.NugetPackets.Add(meta.NugetPackage);
            CheckCategorie(meta.Category);
            item.MetaData.Add(meta);
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

        if (!Directory.Exists(packagesPath))
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

        return s[..i];
    }


    private void CheckCategorie(string categorie)
    {
        if (Categories.Any(x => x == categorie))
        {
            return;
        }

        Categories.Add(categorie);
    }

    /// <summary>
    /// Get a clean repository URL
    /// </summary>
    /// <param name="url">Raw URL</param>
    /// <returns>Cleaned URL</returns>
    public string GetRepositoryUrl(string url)
    {
        var s = url.Split('\r', '\n');

        var item = s.FirstOrDefault(x => x.Contains("url = ", StringComparison.OrdinalIgnoreCase));

        if (string.IsNullOrEmpty(item))
        {
            return string.Empty;
        }

        var erg = item.Trim().Replace("\t", string.Empty).Replace("url = ", string.Empty, StringComparison.OrdinalIgnoreCase).Trim();

        if (erg.EndsWith(".git", StringComparison.OrdinalIgnoreCase))
        {
            erg = erg[..^4];
        }

        return erg;
    }


    private void Status(string msg)
    {
        StatusMessageDelegate?.Invoke(msg);
    }
}