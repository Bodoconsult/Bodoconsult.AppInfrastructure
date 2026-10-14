// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;

namespace JobExecuter.Dal.Helpers;

/// <summary>
/// Helper class for Windows process handling
/// </summary>
public class ProcessHelper
{
    /// <summary>
    /// Kill a process with all its child processes
    /// </summary>
    /// <param name="processId">process ID</param>
    public static void KillProcessAndChildrens(int processId)
    {

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new Exception("ProcessHelper.KillProcessAndChildrens: Works only on Windows OS.");
        }


        var processSearcher = new ManagementObjectSearcher
            ($"Select * From Win32_Process Where ParentProcessID={processId}");

        var processCollection = processSearcher.Get();

        // We must kill child processes first!
        //if (processCollection != null)
        //{
        foreach (var mo in processCollection)
        {
            KillProcessAndChildrens(Convert.ToInt32(mo["ProcessID"])); //kill child processes(also kills childrens of childrens etc.)
        }
        //}

        // Then kill parents.
        try
        {
            var proc = Process.GetProcessById(processId);
            if (!proc.HasExited)
            {
                proc.Kill();
            }
        }
        catch
        {
            // Process already exited.
        }
    }


}