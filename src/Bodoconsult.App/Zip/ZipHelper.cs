// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.IO.Compression;

namespace Bodoconsult.App.Zip;

/// <summary>
/// General helper methods
/// </summary>
public static class ZipHelper
{
    /// <summary>
    /// Create a ZIP archive from a folder
    /// </summary>
    /// <param name="zipPath">Target path for the ZIP archive</param>
    /// <param name="folderPath">Folde rpath to archive</param>
    /// <param name="filter">Filter expression like *.log or null</param>
    public static void CreateZipArchive(string zipPath, string folderPath, string? filter = null)
    {
        if (zipPath is null)
        {
            throw new ArgumentNullException(nameof(zipPath));
        }

        if (File.Exists(zipPath))
        {
            File.Delete(zipPath);
        }

        using var zipToOpen = new FileStream(zipPath, FileMode.OpenOrCreate);
        using var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
        var dir = new DirectoryInfo(folderPath);

        var files = filter is null ? dir.GetFiles() : dir.GetFiles().Where(s => filter.Contains($"*{s.Extension}"));

        foreach (var file in files)
        {
            try
            {
                archive.CreateEntryFromFile(file.FullName, file.Name);
            }
            catch
            {
                // First retry
                Thread.Sleep(33);

                try
                {
                    archive.CreateEntryFromFile(file.FullName, file.Name);
                }
                catch
                {
                    // First retry
                    Thread.Sleep(33);

                    try
                    {
                        archive.CreateEntryFromFile(file.FullName, file.Name);
                    }
                    catch
                    {
                        // Do nothing
                    }
                }
            }
        }
    }
}