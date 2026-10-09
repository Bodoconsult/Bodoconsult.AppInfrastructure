// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Helpers;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Bodoconsult.App.Windows.Helpers;

/// <summary>
/// Helper class for handling DiskShadow volume shadow copies
/// </summary>
public static class DiskShadowHelper
{
    private const string Cmd = "diskshadow.exe";

    // use diskshadow on windows 11

    private static readonly Assembly Ass = typeof(DiskShadowHelper).Assembly;

    static DiskShadowHelper()
    {
        TempDir = Path.GetTempPath();
    }

    /// <summary>
    /// Directory for storing temporary files
    /// </summary>
    public static string TempDir { get; set; }

    /// <summary>
    /// Timeout for the diskshadow command in ms. Default: 50000 ms
    /// </summary>
    public static int Timeout { get; set; } = 50000;

    /// <summary>
    /// Clear a diskshadow drive
    /// </summary>
    /// <param name="drive"></param>
    public static void ClearDrive(string drive)
    {
        // create batch file for starting VSS
        var batch = Path.Combine(TempDir, "JobExecuter_Diskshadow2.txt");

        if (!File.Exists(batch))
        {
            var batchContent = $"unexpose {drive}";

            Debug.Print(batch);

            File.WriteAllText(batch, batchContent);
        }

        var para = $"/s {batch}";
        Execute(para);
    }

    /// <summary>
    /// Execute a command 
    /// </summary>
    /// <param name="para">parameters</param>
    private static void Execute(string para)
    {
        if (!OsHelper.IsWindowsServer())
        {
            return;
        }

        var psi = new ProcessStartInfo("cmd.exe", $"/C {Cmd} {para}")
        {
            WorkingDirectory = TempDir,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true
        };

        var process = new Process
        {
            StartInfo = psi
        };

        //ArgumentNullException.ThrowIfNull(process);

        process.Start();
        process.BeginOutputReadLine();



        //proc.OutputDataReceived += (sender, e) => SetStatus(e.Data);
        //proc.ErrorDataReceived += (sender, e) =>
        //{
        //    result = false;
        //    var msg = "RunApp:Error>>" + e.Data;
        //    Errors.Add(new Exception(msg));
        //    SetStatus(msg);
        //};

        process.Start();
        process.WaitForExit(Timeout);

        if (process.Responding)
        {
            //Process was responding; close the main window.
            process.CloseMainWindow();
        }
        else
        {
            //Process was not responding; force the process to close.
            process.Kill();
        }

        process.Close();
    }

    /// <summary>
    /// Create a diskshadow drive
    /// </summary>
    /// <param name="sourceDrive">Source drive like C:</param>
    /// <param name="targetDrive">Target drive like D:</param>
    public static void CreateDrive(string sourceDrive, string targetDrive)
    {
        // Create backup.cab
        var cabFile = Path.Combine(TempDir, "Backup.cab");
        CreateCabFile(cabFile);

        // create batch file for starting VSS
        var batch = Path.Combine(TempDir, "JobExecuter_Diskshadow.txt");

        if (!File.Exists(batch))
        {
            var batchContent = GetBatchContent;

            ArgumentNullException.ThrowIfNull(batchContent);

            batchContent = batchContent
                .Replace("{0}", sourceDrive)
                .Replace("{1}", targetDrive)
                .Replace("{2}", cabFile);

            Debug.Print(batch);

            File.WriteAllText(batch, batchContent);
        }

        var para = $"/s {batch}";

        Execute(para);
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