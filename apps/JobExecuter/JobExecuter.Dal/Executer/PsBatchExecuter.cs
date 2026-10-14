// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Threading;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Enums;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;

// https://blogs.msdn.microsoft.com/kebab/2014/04/28/executing-powershell-scripts-from-c/

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Run a PowerShell script from a file
/// </summary>
internal class PsBatchExecuter : BaseExecuter
{
    private readonly PsBatchJobTask _jobTask;

    private PipelineExecutor _pipelineExecutor;
    private PowerShell _powerShellInstance;
    private IAsyncResult _asyncResult;

    //private int _processId;

    private bool _waitToken;

    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {
        try
        {
            SetStatus("Start running PowerShell script: {0}", _jobTask.Filename);

            if (_jobTask.PowerShellVersion == PowerShellVersion.V01)
            {
                RunPsV01();
            }
            if (_jobTask.PowerShellVersion == PowerShellVersion.V03)
            {
                RunPsV03();
            }

            SetStatus("Done PowerShell script: {0}", _jobTask.Filename);
        }
        catch (Exception ex)
        {
            SetStatus("PsBatch:Error: {0}\r\n{1}r\n{2}", _jobTask.Filename, ex.Message, _pipelineExecutor.Pipeline.PipelineStateInfo.Reason);
            Errors.Add(ex);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Run PowerShell V3.0
    /// </summary>
    private void RunPsV03()
    {
        _powerShellInstance = PowerShell.Create();

        _powerShellInstance.AddScript(_jobTask.Filename);

        // begin invoke execution on the pipeline
        _waitToken = false;
        // prepare a new collection to store output stream objects
        var outputCollection = new PSDataCollection<PSObject>();
        outputCollection.DataAdded += PsOnData;
        // the streams (Error, Debug, Progress, etc) are available on the PowerShell instance.
        // we can review them during or after execution.
        // we can also be notified when a new item is written to the stream (like this):
        _powerShellInstance.Streams.Error.DataAdded += PsOnError;
        // begin invoke execution on the pipeline
        // use this overload to specify an output stream buffer
        _asyncResult = _powerShellInstance.BeginInvoke<PSObject, PSObject>(null, outputCollection);


        // do something else until execution has completed.
        // this could be sleep/wait, or perhaps some other work
        while (!_waitToken)
        {
            Thread.Sleep(1000);
            // might want to place a timeout here...
            _waitToken = _asyncResult.IsCompleted;
        }

    }

    /// <summary>
    /// Run PowerShell V1.0 and V2.0
    /// </summary>
    private void RunPsV01()
    {
        const string msg = "PsBatch:Error: PowerShell 1.0 and 2.0 not supported anymore";
        Errors.Add(new Exception(msg));
        SetStatus(msg);

        ////var result = RunScript(_jobTask.Filename);

        //// create Powershell runspace
        //System.Management.Automation.Runspaces.Runspace runSpace = RunspaceFactory.CreateRunspace();

        //// open it
        //runSpace.Open();

        //// create a new PipelineExecutor instance

        //// 'this' is the form that will show the output of the script.
        //// it is needed to marshal the script output data from the
        //// powershell thread to the UI thread
        //_pipelineExecutor = new PipelineExecutor(runSpace, _jobTask.Filename);

        //// listen for new data
        //_pipelineExecutor.OnDataReady += PsOnData;

        //// listen for end of data
        //_pipelineExecutor.OnDataEnd += PipelineExecutorOnOnDataEnd;

        //// listen for errors
        //_pipelineExecutor.OnErrorReady += PsOnError;

        //// launch the script
        //_pipelineExecutor.Start();

        //_waitToken = false;

        //while (!_waitToken)
        //{
        //    Thread.Sleep(15);
        //}
    }

    private void PipelineExecutorOnOnDataEnd()
    {
        _waitToken = true;
    }

    private void PsOnError(ICollection<object> data)
    {
        foreach (var psObject in data)
        {
            var msg = $"PsBatch:Error: {psObject}";
            Errors.Add(new Exception(msg));
            SetStatus(msg);
        }
    }

    private void PsOnError(object sender, DataAddedEventArgs e)
    {
        var data = (PSDataCollection<ErrorRecord>)sender;

        var msg = $"PsBatch:Error: {data[e.Index].ErrorDetails}";
        Errors.Add(new Exception(msg));
        SetStatus(msg);

    }

    private void PsOnData(ICollection<PSObject> data)
    {
        foreach (var psObject in data)
        {
            SetStatus("{0}", psObject.ToString());
        }
    }

    private void PsOnData(object sender, DataAddedEventArgs e)
    {
        var data = (PSDataCollection<PSObject>)sender;

        var msg = $"{data[e.Index]}";
        SetStatus(msg);

    }

    //private string RunScript(string scriptText)
    //{
    //    // create Powershell runspace

    //    Runspace runspace = RunspaceFactory.CreateRunspace();

    //    // open it

    //    runspace.Open();

    //    // create a pipeline and feed it the script text

    //    var pipeline = runspace.CreatePipeline();
    //    pipeline.Commands.AddScript(scriptText);

    //    // add an extra command to transform the script
    //    // output objects into nicely formatted strings

    //    // remove this line to get the actual objects
    //    // that the script returns. For example, the script

    //    // "Get-Process" returns a collection
    //    // of System.Diagnostics.Process instances.

    //    pipeline.Commands.Add("Out-String");

    //    // execute the script

    //    var results = pipeline.Invoke();

    //    // close the runspace

    //    runspace.Close();

    //    // convert the script result into a single string

    //    var stringBuilder = new StringBuilder();
    //    foreach (var obj in results)
    //    {
    //        stringBuilder.AppendLine(obj.ToString());
    //    }

    //    return stringBuilder.ToString();
    //}


    /// <summary>
    /// Kill task when timed out
    /// </summary>
    /// <returns>true on success</returns>
    public override bool Kill()
    {
        try
        {
            _waitToken = true;
            _pipelineExecutor?.Stop();
            if (_powerShellInstance != null)
            {                  
                if (_asyncResult !=null) _powerShellInstance.EndInvoke(_asyncResult);
                _powerShellInstance.Dispose();
            }
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
    public PsBatchExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (PsBatchJobTask)jobTask;
    }
}