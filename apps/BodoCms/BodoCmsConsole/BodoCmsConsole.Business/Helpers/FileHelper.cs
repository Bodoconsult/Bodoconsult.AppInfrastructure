// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using BodoCmsConsole.Business.App;
using Bodoconsult.App.Abstractions.Interfaces;

namespace BodoCmsConsole.Business.Helpers;

public static class FileHelper
{
    private static readonly IAppLoggerProxy? Logger = Globals.Instance.Logger;

    /// <summary>
    /// Verzeichnis mit allen Dateien kopieren (ohne Unterordner)
    /// </summary>
    /// <param name="source">Quellverzeichnis</param>
    /// <param name="target">Zielverzeichnis</param>
    public static void CopyDir(string source, string target)
    {
        try
        {
            if (!source.EndsWith(@"\")) source += @"\";
            if (!target.EndsWith(@"\")) target += @"\";

            var dir = new DirectoryInfo(source);
            var fiArr = dir.GetFiles();

            // Display the names of the files.
            foreach (var fri in fiArr)
            {
                fri.CopyTo(target + fri.Name, true);
            }
        }
        catch (Exception ex)
        {
            Logger?.LogError($"Copying {source} to target {target} failed", ex);
        }
    }

    /// <summary>
    /// Check if directory exists and if not create it
    /// </summary>
    /// <param name="dirPath"></param>
    public static void CheckIfDirExists(string dirPath)
    {
        try
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }
        catch (Exception ex)
        {
            Logger?.LogError($"Creating folder {dirPath} failed", ex);
        }
    }

    /// <summary>
    /// Remove a certain directory from a list of directories
    /// </summary>
    /// <param name="name">Directory to remove</param>
    /// <param name="directories">List of directories</param>
    public static void RemoveDirectory(string name, IList<DirectoryInfo> directories)
    {
        var dir = directories.FirstOrDefault(x => x.Name.ToUpperInvariant() == name);
        if (dir == null) return;
        directories.Remove(dir);
    }


    /// <summary>
    /// Programm starten
    /// </summary>
    /// <param name="exe">Pfad zur EXE</param>
    /// <param name="args">Befehlszeilenargumente</param>
    public static void Shell(string exe, string args)
    {
        var psi = new ProcessStartInfo(exe, args) { WindowStyle = ProcessWindowStyle.Maximized };
        try
        {
            Process.Start(psi);
        }
        catch (Exception ex)
        {
            Logger?.LogError($"Exec {exe} failed with args {args}", ex);
        }
    }


    /// <summary>
    /// Prüfe, ob Verzeichnsname auf FTP-Server zulässig ist und ändere in ggfs.
    /// </summary>
    /// <param name="fi">zu prüfendes Verzeichnis</param>
    public static DirectoryInfo? CheckDirName(ref DirectoryInfo fi)
    {
        if (fi.Parent == null)
        {
            return null;
        }

        var parentDir = fi.Parent.FullName;

        var dirName = fi.Name;
        var altDirName = dirName;

        if (dirName.Contains("Ä")) dirName = dirName.Replace("Ä", "Ae");
        if (dirName.Contains("ä")) dirName = dirName.Replace("ä", "ae");

        if (dirName.Contains("Ö")) dirName = dirName.Replace("Ö", "Oe");
        if (dirName.Contains("ö")) dirName = dirName.Replace("ö", "oe");
        if (dirName.Contains("Ü")) dirName = dirName.Replace("Ü", "ue");
        if (dirName.Contains("ü")) dirName = dirName.Replace("ü", "ue");
        //if (Filename.Contains(".")) Filename = Filename.Replace(".", "_");
        if (dirName.Contains(" ")) dirName = dirName.Replace(" ", "_");
        if (dirName.Contains("ß")) dirName = dirName.Replace("ß", "ss");


        if (dirName == altDirName)
        {
            return fi;
        }
        if (Directory.Exists(Path.Combine(parentDir, dirName)))
        {
            var i = 1;
            while (true)
            {
                dirName += $"_{i}";
                if (!Directory.Exists(Path.Combine(parentDir, dirName)))
                {
                    break;
                }

                i++;
            }
        }

        try
        {
            fi.MoveTo(Path.Combine(fi.Parent.FullName, dirName));
            return new DirectoryInfo(Path.Combine(parentDir, dirName));
        }
        catch
        {
            return fi;
        }
    }


    /// <summary>
    /// Prüfe, ob Dateiname auf FTP-Server zulässig ist und ändere in ggfs.
    /// </summary>
    /// <param name="fi">zu prüfende Datei</param>
    public static void CheckFileName(FileInfo fi)
    {
        switch (fi.Extension)
        {
            case ".inc":
            case ".url":
                return;
        }

        var dirName = fi.DirectoryName;
        if (dirName == null)
        {
            return;
        }

        var fileName = fi.Name[..^fi.Extension.Length];
        var altFilename = fileName;

        if (fileName.Contains("Ä")) fileName = fileName.Replace("Ä", "Ae");
        if (fileName.Contains("ä")) fileName = fileName.Replace("ä", "ae");

        if (fileName.Contains("Ö")) fileName = fileName.Replace("Ö", "Oe");
        if (fileName.Contains("ö")) fileName = fileName.Replace("ö", "oe");
        if (fileName.Contains("Ü")) fileName = fileName.Replace("Ü", "ue");
        if (fileName.Contains("ü")) fileName = fileName.Replace("ü", "ue");
        //if (Filename.Contains(".")) Filename = Filename.Replace(".", "_");
        if (fileName.Contains(" ")) fileName = fileName.Replace(" ", "_");
        if (fileName.Contains("ß")) fileName = fileName.Replace("ß", "ss");


        if (fileName == altFilename)
        {
            return;
        }

        if (File.Exists(Path.Combine(dirName, fileName + fi.Extension)))
        {
            var i = 1;
            while (true)
            {
                fileName += $"_{i}";
                if (!File.Exists(Path.Combine(dirName, fileName + fi.Extension)))
                {
                    break;
                }

                i++;
            }

        }

        try
        {
            fi.MoveTo(Path.Combine(dirName, fileName + fi.Extension));
        }
        catch (Exception ex)
        {
            Logger?.LogError($"Moving dir {dirName} failed",ex);
        }
    }
}