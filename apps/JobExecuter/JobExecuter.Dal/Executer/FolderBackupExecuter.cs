// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Enums;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Take a backup of a folder with robocopy.exe. Use the follow command lines:
/// CopyOnlyNewerFiles: robocopy.exe {0} {1}  /XO {4} /MT:12 /fft  /R:1 /W:1 /IPG:0  /XA:SH /nodcopy {2} {3}
/// PlainCopy: robocopy.exe {0} {1}  {4} /MT:12 /fft /np /R:1 /W:1 /IPG:0  /XA:SH /nodcopy {2} {3}
/// Mirroring: robocopy.exe {0} {1}  /MIR {4} /MT:12 /fft /np /R:1 /W:1 /IPG:0  /XA:SH /nodcopy {2} {3}
/// </summary>
internal class FolderBackupExecuter : BaseExecuter
{
    private const string RoboCopyCommandOnlyNewerFiles = "{0} {1}  /XO {4} /MT:12 /fft  /R:1 /W:1 /IPG:0  /XA:SH /nodcopy {2} {3}";
    private const string RoboCopyCommandOnlyPlainCopy = "{0} {1}  {4} /MT:12 /fft /np /R:1 /W:1 /IPG:0  /XA:SH /nodcopy {2} {3}";
    private const string RoboCopyCommandOnlyMirror = "{0} {1}  /MIR {4} /MT:12 /fft /np /R:1 /W:1 /IPG:0  /XA:SH /nodcopy {2} {3}";

    private readonly FolderBackupJobTask _jobTask;

    private int _processId;

    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {
        SetStatus("Starting backup {0} to {1}...", _jobTask.SourcePath, _jobTask.TargetPath);
        var result = true;

        try
        {

            if (!Directory.Exists(_jobTask.SourcePath))
            {
                var msg =
                    $"Backup of folder '{_jobTask.SourcePath}' to '{_jobTask.TargetPath}' failed. RoboCopy:Error>>SourcePath not found";
                Errors.Add(new Exception(msg));
                SetStatus(msg);
                return false;
            }


            try
            {
                if (!Directory.Exists(_jobTask.TargetPath)) Directory.CreateDirectory(_jobTask.TargetPath);
            }
            catch (Exception e)
            {
                var msg =
                    $"Backup of folder '{_jobTask.SourcePath}' to '{_jobTask.TargetPath}' failed. RoboCopy:Error>>TargetPath not found and not possible to create";
                Errors.Add(new Exception(msg, e));
                SetStatus(msg);
                return false;
            }

                

            var xd = string.IsNullOrEmpty(_jobTask.ExcludeDirectories) ? string.Empty : $"/XD{_jobTask.ExcludeDirectories}";

            var s = _jobTask.IncludeSubDirectories ? "/S" : string.Empty;

            var np = _jobTask.ShowProgress ? string.Empty : "/np";

            string roboCommand;

            switch (_jobTask.BackupType)
            {
                case FolderBackupType.CopyOnlyNewerFiles:
                    roboCommand = RoboCopyCommandOnlyNewerFiles;
                    break;
                case FolderBackupType.Mirror:
                    roboCommand = RoboCopyCommandOnlyMirror;
                    break;
                default:
                    roboCommand = RoboCopyCommandOnlyPlainCopy;
                    break;
            }

            roboCommand = string.Format(roboCommand, $"\"{_jobTask.SourcePath}\"", $"\"{_jobTask.TargetPath}\"", xd, np, s);

            SetStatus("Robocopy.exe {0}", roboCommand);

            var proc = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = _jobTask.SourcePath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = "robocopy.exe",
                    Arguments = roboCommand,
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Hidden //this is for hiding the cmd window...so execution will happen in background.
                }
            };

            proc.OutputDataReceived += (sender, e) => SetStatus(e.Data);
            proc.ErrorDataReceived += (sender, e) =>
            {
                result = false;
                var msg =
                    $"Backup of folder '{_jobTask.SourcePath}' to '{_jobTask.TargetPath}' failed. RoboCopy:Error>> {e.Data}";
                Errors.Add(new Exception(msg));
                SetStatus(msg);
            };

            proc.Start();
            _processId = proc.Id;
            proc.BeginOutputReadLine();
            proc.WaitForExit();
            proc.Close();
        }
        catch (Exception ex)
        {
            result = false;
            var msg = $"Backup of folder '{_jobTask.SourcePath}' to '{_jobTask.TargetPath}' failed";
            Errors.Add(new Exception(msg, ex));
            SetStatus(msg);
        }

        SetStatus("Backup {0} to {1} done with {2} error(s)!", _jobTask.SourcePath, _jobTask.TargetPath, Errors.Count);
        return result;
    }


    /// <summary>
    /// Kill task when timed out
    /// </summary>
    /// <returns>true on success</returns>
    public override bool Kill()
    {
        try
        {
            ProcessHelper.KillProcessAndChildrens(_processId);
        }
        catch
        {
            // ignored
        }
        return true;
    }


    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public FolderBackupExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (FolderBackupJobTask)jobTask;
    }
}