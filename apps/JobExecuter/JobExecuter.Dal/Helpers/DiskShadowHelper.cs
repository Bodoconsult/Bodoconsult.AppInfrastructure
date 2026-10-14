//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

//using System;
//using System.Diagnostics;
//using System.IO;

//namespace JobExecuter.Dal.Helpers;

///// <summary>
///// Helper class for handling DiskShadow volume shadow copies
///// </summary>
//public static class DiskShadowHelper
//{
//    private static readonly string TempDir;
//    private const string cmd = "diskshadow.exe";

//    static DiskShadowHelper()
//    {
//        TempDir = Path.GetTempPath();

//#if DEBUG
//        TempDir = @"D:\temp";
//#endif
//    }


//    /// <summary>
//    /// Clear a diskshadow drive
//    /// </summary>
//    /// <param name="drive"></param>
//    public static void ClearDrive(string drive)
//    {
//        if (!OsHelper.IsWindowsServer())
//        {
//            return;
//        }

//        var folder = drive;

//        if (!folder.EndsWith("\"", StringComparison.OrdinalIgnoreCase))
//        {
//            folder = $"{folder}\\";
//        }

//        if (!Directory.Exists(folder))
//        {
//            return;
//        }

        
//        var para = $"unexpose {drive}";
//        Execute(para);
//    }

//    /// <summary>
//    /// Execute a command 
//    /// </summary>
//    /// <param name="para">parameters</param>
//    private static void Execute(string para)
//    {
//        var proc = new Process
//        {
//            StartInfo =
//            {
//                // ReSharper disable once AssignNullToNotNullAttribute
//                UseShellExecute = false,
//                RedirectStandardOutput = true,
//                RedirectStandardError = true,
//                FileName = cmd,
//                Arguments = para,
//                CreateNoWindow = false,
//                WindowStyle =
//                    ProcessWindowStyle
//                        .Maximized //this is for hiding the cmd window...so execution will happen in background.
//            }
//        };

//        //proc.OutputDataReceived += (sender, e) => SetStatus(e.Data);
//        //proc.ErrorDataReceived += (sender, e) =>
//        //{
//        //    result = false;
//        //    var msg = "RunApp:Error>>" + e.Data;
//        //    Errors.Add(new Exception(msg));
//        //    SetStatus(msg);
//        //};

//        proc.Start();
//        proc.WaitForExit(5000);

//        if (proc.Responding)
//        {
//            //Process was responding; close the main window.
//            proc.CloseMainWindow();
//        }
//        else
//        {
//            //Process was not responding; force the process to close.
//            proc.Kill();
//        }
//    }

//    /// <summary>
//    /// Create a diskshadow drive
//    /// </summary>
//    /// <param name="sourceDrive">Source drive like C:</param>
//    /// <param name="targetDrive">Target drive like D:</param>
//    public static void CreateDrive(string sourceDrive, string targetDrive)
//    {
//        if (!OsHelper.IsWindowsServer())
//        {
//            return;
//        }

//        // Create backup.cab
//        var cabFile = Path.Combine(TempDir, "Backup.cab");
//        FileHelper.SaveBinaryResource("JobExecuter.Dal.Resources.Backup.cab", cabFile);

//        // create batch file
//        var batch = Path.Combine(TempDir, "JobExecuter_Diskshadow.txt");

//        var batchContent = FileHelper.GetTextResource("JobExecuter.Dal.Resources.Diskshadow.txt");

//        batchContent = batchContent
//            .Replace("{0}", sourceDrive)
//            .Replace("{1}", targetDrive)
//            .Replace("{2}", cabFile);


//        Debug.Print(batch);

//        File.WriteAllText(batch, batchContent);

//        var para = $"/s {batch}";

//        Execute( para);
//    }
//}