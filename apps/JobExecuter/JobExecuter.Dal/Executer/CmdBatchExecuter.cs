// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Enums;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <summary>
/// Cmd.exe batch executer for testing purposes
/// </summary>
internal class CmdBatchExecuter : BaseExecuter
{
    private readonly CmdBatchJobTask _jobTask;

    private int _processId;

    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {
        var result = true;

        try
        {
            if (!string.IsNullOrEmpty(_jobTask.BatchFileContent))
            {
                _jobTask.RunType = CmdBatchRunType.LineByLine;
            }

            if (_jobTask.RunType == CmdBatchRunType.LineByLine)
            {
                var content = string.IsNullOrEmpty(_jobTask.BatchFileContent) ? File.ReadAllText(_jobTask.Filename) : _jobTask.BatchFileContent;

                var cmds = content.Split(["\r\n"], StringSplitOptions.RemoveEmptyEntries);

                foreach (var cmd in cmds)
                {
                    RunCmd(cmd);
                }
            }
            else
            {
                SetStatus("Start running cmd batch {0}...\r\n", _jobTask.Filename);

                var proc = new Process
                {
                    StartInfo =
                    {
                        // ReSharper disable once AssignNullToNotNullAttribute
                        WorkingDirectory = new FileInfo(_jobTask.Filename).DirectoryName,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        FileName = _jobTask.Filename,
                        CreateNoWindow = false,
                        WindowStyle = ProcessWindowStyle.Hidden //this is for hiding the cmd window...so execution will happen in background.
                    }
                };


                proc.OutputDataReceived += (sender, e) => SetStatus(e.Data);
                proc.ErrorDataReceived += (sender, e) =>
                {
                    result = false;
                    var msg = $"CmdBatch>>{e.Data}";
                    Errors.Add(new Exception(msg));
                    SetStatus(msg);
                };

                proc.Start();
                _processId = proc.Id;
                proc.BeginOutputReadLine();
                proc.WaitForExit();
                proc.Close();

                SetStatus("\r\nCmd batch {0} done!!", _jobTask.Filename);
            }
        }
        catch (Exception ex)
        {
            result = false;
            var msg = $"CmdBatch:Error:{ex.Message}";
            SetStatus(msg);
            Errors.Add(new Exception(msg));
        }

        return result;
    }

    private void RunCmd(string cmd)
    {
        var test = cmd.Trim().ToLowerInvariant();

        if (string.IsNullOrEmpty(test))
        {
            return;
        }

        if (test.StartsWith("pause", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (test.StartsWith("exit", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        SetStatus($"\r\n{cmd}");
        if (test.StartsWith("rem", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        try
        {
            var proc = new Process
            {
                StartInfo =
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    //WorkingDirectory = new FileInfo(_jobTask.FileName).DirectoryName,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = "cmd.exe",
                    Arguments = $"/c {cmd}",
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Hidden //this is for hiding the cmd window...so execution will happen in background.
                }
            };


            proc.OutputDataReceived += (sender, e) => SetStatus(e.Data);
            proc.ErrorDataReceived += (sender, e) =>
            {
                var msg = $"CmdBatch:Error>>{cmd}>>{e.Data}";
                Errors.Add(new Exception(msg));
                SetStatus($"CmdBatch:Error>>{e.Data}");
            };

            proc.Start();
            _processId = proc.Id;
            proc.BeginOutputReadLine();
            proc.WaitForExit();
            if (proc.ExitCode > 0)
            {
                var msg = $"CmdBatch:Error>>{cmd}>>ExitCode {proc.ExitCode}";
                Errors.Add(new Exception(msg));
                SetStatus(msg);
            }
            proc.Close();
        }
        catch (Exception ex)
        {
            var msg = $"CmdBatch:Error>>{cmd}>>{ex.Data}";
            SetStatus(msg);
            Errors.Add(new Exception(msg, ex));
        }
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
    public CmdBatchExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (CmdBatchJobTask)jobTask;
    }
}