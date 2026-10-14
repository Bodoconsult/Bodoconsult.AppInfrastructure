// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

#pragma warning disable CA1416

/// <inheritdoc />
/// <summary>
/// Run a app with or without parameters
/// </summary>
internal class RunAppExecuter : BaseExecuter
{
    private readonly RunAppJobTask _jobTask;


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

            SetStatus("Start running: {0} {1}", _jobTask.Filename, _jobTask.Arguments);

            var proc = new Process
            {
                StartInfo =
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    WorkingDirectory = new FileInfo(_jobTask.Filename).DirectoryName,
                    LoadUserProfile = _jobTask.InteractiveMode,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = _jobTask.Filename,
                    Arguments = _jobTask.Arguments,
                    CreateNoWindow = !_jobTask.InteractiveMode,
                    WindowStyle = ProcessWindowStyle.Maximized //this is for hiding the cmd window...so execution will happen in background.
                }
            };


            proc.OutputDataReceived += (sender, e) => SetStatusApp(e.Data);
            proc.ErrorDataReceived += (sender, e) =>
            {
                result = false;
                var msg = $"RunApp:Error>>{e.Data}";
                Errors.Add(new Exception(msg));
                SetStatus(msg);
            };

            proc.Start();

            _processId = proc.Id;
            proc.BeginOutputReadLine();
            proc.WaitForExit();
            proc.Close();
            SetStatus("Done: {0} {1}", _jobTask.Filename, _jobTask.Arguments);
        }
        catch (Exception ex)
        {
            result = false;
            SetStatus("RunApp:Error: {0} {1}\r\n", _jobTask.Filename, _jobTask.Arguments, ex.Message);
            Errors.Add(ex);
        }

        return result;
    }





    /// <summary>
    /// Kill task when timed out
    /// </summary>
    /// <returns></returns>
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
    public RunAppExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (RunAppJobTask)jobTask;
    }
}