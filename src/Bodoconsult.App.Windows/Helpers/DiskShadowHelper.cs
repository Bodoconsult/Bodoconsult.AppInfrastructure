// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Bodoconsult.App.Abstractions.Helpers;

namespace Bodoconsult.App.Windows.Helpers;

/// <summary>
/// Helper class for handling DiskShadow volume shadow copies
/// </summary>
public static class DiskShadowHelper
{
    private static readonly string TempDir;
    private const string Cmd = "diskshadow.exe";

    private static readonly Assembly Ass = typeof(DiskShadowHelper).Assembly;

    static DiskShadowHelper()
    {
        TempDir = Path.GetTempPath();
    }

    /// <summary>
    /// Clear a diskshadow drive
    /// </summary>
    /// <param name="drive"></param>
    public static void ClearDrive(string drive)
    {
        if (!OsHelper.IsWindowsServer())
        {
            return;
        }


        var folder = drive;

        if (!folder.EndsWith("\"", StringComparison.OrdinalIgnoreCase))
        {
            folder = $"{folder}\\";
        }

        if (!Directory.Exists(folder))
        {
            return;
        }
        
        var para = $"unexpose {drive}";
        Execute(para);
    }

    /// <summary>
    /// Execute a command 
    /// </summary>
    /// <param name="para">parameters</param>
    private static void Execute(string para)
    {
        var proc = new Process
        {
            StartInfo =
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                FileName = Cmd,
                Arguments = para,
                CreateNoWindow = false,
                WindowStyle =
                    ProcessWindowStyle
                        .Maximized //this is for hiding the cmd window...so execution will happen in background.
            }
        };

        //proc.OutputDataReceived += (sender, e) => SetStatus(e.Data);
        //proc.ErrorDataReceived += (sender, e) =>
        //{
        //    result = false;
        //    var msg = "RunApp:Error>>" + e.Data;
        //    Errors.Add(new Exception(msg));
        //    SetStatus(msg);
        //};

        proc.Start();
        proc.WaitForExit(5000);

        if (proc.Responding)
        {
            //Process was responding; close the main window.
            proc.CloseMainWindow();
        }
        else
        {
            //Process was not responding; force the process to close.
            proc.Kill();
        }
    }

    /// <summary>
    /// Create a diskshadow drive
    /// </summary>
    /// <param name="sourceDrive">Source drive like C:</param>
    /// <param name="targetDrive">Target drive like D:</param>
    public static void CreateDrive(string sourceDrive, string targetDrive)
    {
        if (!OsHelper.IsWindowsServer())
        {
            return;
        }

        // Create backup.cab
        var cabFile = Path.Combine(TempDir, "Backup.cab");
        CreateCabFile(cabFile);

        // create batch file
        var batch = Path.Combine(TempDir, "JobExecuter_Diskshadow.txt");

        var batchContent = GetBatchContent;

        ArgumentNullException.ThrowIfNull(batchContent);

        batchContent = batchContent
            .Replace("{0}", sourceDrive)
            .Replace("{1}", targetDrive)
            .Replace("{2}", cabFile);


        Debug.Print(batch);

        File.WriteAllText(batch, batchContent);

        var para = $"/s {batch}";

        Execute( para);
    }

    /// <summary>
    /// Create the required cad-File for shadowing. Public only for unit testing
    /// </summary>
    /// <param name="cabFile"></param>
    public static void CreateCabFile(string cabFile)
    {
        ResourceHelper.SaveBinaryResource(Ass, "Bodoconsult.App.Windows.Resources.Backup.cab", cabFile);
    }

    /// <summary>
    /// Get the batch content. Public only for unit testing
    /// </summary>
    public static string? GetBatchContent => ResourceHelper.GetTextResource(Ass, "Bodoconsult.App.Windows.Resources.Diskshadow.txt");
}