// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.IO;
using System.Runtime.InteropServices;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Executer checks the disk size and places an error if free space is below a certain percentage
/// </summary>
internal class DiskSizeExecuter : BaseExecuter
{

    private readonly DiskSizeJobTask _jobTask;

    // System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
        out ulong lpFreeBytesAvailable,
        out ulong lpTotalNumberOfBytes,
        out ulong lpTotalNumberOfFreeBytes);


    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {
        var result = true;

        foreach (var drive in _jobTask.Drives)
        {

            var server = string.IsNullOrEmpty(drive.IpAddress) ? string.Empty : $" on {drive.IpAddress}";

            try
            {
                    
                double totalSize;
                double freeSize;
                if (drive.Name.StartsWith("\\"))
                {
                    var success = GetDiskFreeSpaceEx(drive.Name,
                        out _,
                        out var totalNumberOfBytes,
                        out var totalNumberOfFreeBytes);
                    if (!success)
                    {
                        Errors.Add(new System.ComponentModel.Win32Exception());
                        continue;
                    }


                    totalSize = Convert.ToDouble(totalNumberOfBytes / WmiHelper.BytesPerMb);
                    freeSize = Convert.ToDouble(totalNumberOfFreeBytes / WmiHelper.BytesPerMb);
                }
                else
                {
                    if (string.IsNullOrEmpty(drive.IpAddress))
                    {
                        var di = new DriveInfo(drive.Name);
                        totalSize = Convert.ToDouble((ulong)di.TotalSize / WmiHelper.BytesPerMb);
                        freeSize = Convert.ToDouble((ulong)di.TotalFreeSpace / WmiHelper.BytesPerMb);
                    }
                    else
                    {
                        var wmi = new WmiHelper
                        {
                            IpAddress = drive.IpAddress,
                            Drive = drive.Name
                        };

                        wmi.GetDiskData();
                        totalSize = wmi.TotalSize;
                        freeSize = wmi.FreeSize;
                    }
                }

                SetStatus("DiskSize: drive {0} {4}: {3} MB of {2} MB left free or {1:P} left free", drive.Name, freeSize / totalSize, totalSize, freeSize, server);

                if (freeSize / totalSize < drive.MinimumPercentageOfFreeSpace)
                {
                    result = false;
                    var msg = string.Format("DiskSize:Warning: Drive {0} {2}: Only {1:P} left free!!! Only {4} MB of {3} MB left free!", drive.Name, freeSize / totalSize, server, totalSize, freeSize);
                    SetStatus(msg);
                    Errors.Add(new Exception(msg));
                }
            }
            catch (Exception ex)
            {
                result = false;
                var msg = string.Format("DiskSize:Error: Drive {1} {2}: {0}", ex.Message,drive, server);
                SetStatus(msg);
                Errors.Add(new Exception(msg));
            }

        }

        return result;
    }

    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public DiskSizeExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (DiskSizeJobTask)jobTask;
    }
}