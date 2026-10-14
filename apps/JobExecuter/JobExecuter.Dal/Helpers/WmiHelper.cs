// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;

namespace JobExecuter.Dal.Helpers;

/// <summary>
/// Helper class for WMI functionality
/// </summary>
public class WmiHelper
{
    /// <summary>
    /// Number of Bytes per Megabyte
    /// </summary>
    public const ulong BytesPerMb = 1048576;

    /// <summary>
    /// IP address
    /// </summary>
    public string IpAddress { get; set; }

    /// <summary>
    /// Drive name to check disk size
    /// </summary>
    public string Drive { get; set; }

    /// <summary>
    /// Get disk size from remote computer
    /// </summary>
    internal void GetDiskData()
    {
        //ToDo: WMI??

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new Exception("WmiHelper.GetDiskData: Works only on Windows OS.");
        }

        var oConn = new ConnectionOptions
        {
            Impersonation = ImpersonationLevel.Impersonate,
            EnablePrivileges = true
        };

        var path = new ManagementPath
        {
            NamespacePath = @"root\cimv2",
            Server = IpAddress
        };
        var scope = new ManagementScope(path, oConn);
        var condition = $"DriveLetter = '{Drive}'";
        var selectedProperties = new[] { "FreeSpace", "Capacity" };
        var query = new SelectQuery("Win32_Volume", condition, selectedProperties);

        using var searcher = new ManagementObjectSearcher(scope, query);
        using var results = searcher.Get();
        var volume = results.Cast<ManagementObject>().SingleOrDefault();

        if (volume == null)
        {
            return;
        }

        var freeSpace = (ulong)volume.GetPropertyValue("FreeSpace");
        var totalSpace = (ulong)volume.GetPropertyValue("Capacity");
        // Use freeSpace here...

        TotalSize = Convert.ToDouble(totalSpace / BytesPerMb);
        FreeSize = Convert.ToDouble(freeSpace / BytesPerMb);
    }

    /// <summary>
    /// Total size of the drive
    /// </summary>
    public double TotalSize { get; set; }

    /// <summary>
    /// Total size of the drive
    /// </summary>
    public double FreeSize { get; set; }
}


/*
 * VirtualDesktop.cs - A desktop creation/deletion/switching class for Windows.
 * Copyright (c) 2013, 2014, 2016 Eyal Kalderon
 *
 * The MIT License (MIT)
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */


