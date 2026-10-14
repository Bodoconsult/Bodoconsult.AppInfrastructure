// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading;

namespace JobExecuter.Dal.Helpers;

/// <summary>
/// Class that assists in asynchronously executing and retrieving the results of a powershell script pipeline.
/// </summary>
internal class PipelineExecutor
{
    #region public types, members
    /// <summary>
    /// Gets the powershell Pipeline associated with this PipelineExecutor
    /// </summary>
    public Pipeline Pipeline { get; private set; }

    public delegate void DataReadyDelegate(ICollection<PSObject> data);
    public delegate void DataEndDelegate();
    public delegate void ErrorReadyDelegate(ICollection<object> data);

    /// <summary>
    /// Occurs when there is new data available from the powershell script.
    /// </summary>
    public event DataReadyDelegate OnDataReady;

    /// <summary>
    /// Occurs when powershell script completed its execution.
    /// </summary>
    public event DataEndDelegate OnDataEnd;

    /// <summary>
    /// Occurs when there is new errordata available from the powershell script.
    /// </summary>
    public event ErrorReadyDelegate OnErrorReady;

    #endregion

    #region private types, members

    /// <summary>
    /// Local delegate to a private method
    /// </summary>
    private readonly DataReadyDelegate _synchDataReady;

    /// <summary>
    /// Local delegate to a private method
    /// </summary>
    private readonly DataEndDelegate _synchDataEnd;

    /// <summary>
    /// Local delegate to a private method
    /// </summary>
    private readonly ErrorReadyDelegate _synchErrorReady;

    /// <summary>
    /// Event set when the user wants to stop script execution.
    /// </summary>
    private readonly ManualResetEvent _stopEvent;

    /// <summary>
    /// Array of waithandles, used in the StoppableInvoke() method.
    /// </summary>
    private readonly WaitHandle[] _waitHandles;
    #endregion

    #region public methods
    /// <summary>
    /// Constructor, creates a new PipelineExecutor for the given powershell script.
    /// </summary>
    /// <param name="runSpace">Powershell runspace to use for creating and executing the script.
    /// Normally you'd pass the form or component here.</param>
    /// <param name="command">The script to run</param>
    public PipelineExecutor(Runspace runSpace, string command)
    {
            


        // initialize delegates
        _synchDataReady = SynchDataReady;
        _synchDataEnd = SynchDataEnd;
        _synchErrorReady = SynchErrorReady;

        // initialize event members
        _stopEvent = new ManualResetEvent(false);
        _waitHandles = [null, _stopEvent];
        // create a pipeline and feed it the script text
        Pipeline = runSpace.CreatePipeline(command);

        // we'll listen for script output data by way of the DataReady event
        Pipeline.Output.DataReady += OutputDataReady;
        Pipeline.Error.DataReady += ErrorDataReady;
    }

    private void ErrorDataReady(object sender, EventArgs e)
    {
        // fetch all available objects
        var data = Pipeline.Error.NonBlockingRead();

        // if there were any, invoke the ErrorReady event
        if (data.Count <= 0) return;

        // The asynchronous method puts the thread id here.
        var asyncResult = _synchErrorReady.BeginInvoke(data, null, null);
        _waitHandles[0] = asyncResult.AsyncWaitHandle;

        if (WaitHandle.WaitAny(_waitHandles) == 0)
        {
            _synchErrorReady.EndInvoke(asyncResult);
        }
    }

    /// <summary>
    /// Start executing the script in the background.
    /// </summary>
    public void Start()
    {
            
        // ??????
        //if (Pipeline.PipelineStateInfo.State != PipelineState.NotStarted) return;

        // close the pipeline input. If you forget to do 
        // this it won't start processing the script.
        Pipeline.Input.Close();
        // invoke the pipeline. This will cause it to process the script in the background.
        Pipeline.InvokeAsync();
    }

    /// <summary>
    /// Stop executing the script.
    /// </summary>
    public void Stop()
    {
        // first make sure StoppableInvoke() stops blocking
        _stopEvent.Set();
        // then tell the pipeline to stop the script
        Pipeline.Stop();
    }
    #endregion

    #region private methods

    ///// <summary>
    ///// Special Invoke method that operates similarly to <see cref="ISynchronizeInvoke.Invoke"/> but in addition to that, it can be 
    ///// aborted by setting the _stopEvent. This avoids potential deadlocks that are possible when using the regular 
    ///// <see cref="ISynchronizeInvoke.Invoke"/> method.
    ///// </summary>
    ///// <param name="method">A <see cref="System.Delegate"/> to a method that takes parameters of the same number and type that are 
    ///// contained in <paramref name="args"/></param>
    ///// <param name="args">An array of type <see cref="System.Object"/> to pass as arguments to the given method. This can be null if 
    ///// no arguments are needed </param>
    ///// <returns>The <see cref="Object"/> returned by the asynchronous operation</returns>
    //private object StoppableInvoke1(Delegate method, object[] args)
    //{
    //    method.

    //    var asyncResult = method.DynamicInvoke(args);
    //    _waitHandles[0] = asyncResult.AsyncWaitHandle;
    //    return WaitHandle.WaitAny(_waitHandles) == 0 ? _invoker.EndInvoke(asyncResult) : null;
    //}

    /// <summary>
    /// Event handler for the DataReady event of the powershell script pipeline.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OutputDataReady(object sender, EventArgs e)
    {
        // fetch all available objects
        var data = Pipeline.Output.NonBlockingRead();

        // if there were any, invoke the DataReady event
        if (data.Count > 0)
        {
            //StoppableInvoke(_synchDataReady, new object[] { this, data });

            // The asynchronous method puts the thread id here.
            var asyncResult = _synchDataReady.BeginInvoke(data, null, null);
            _waitHandles[0] = asyncResult.AsyncWaitHandle;

            if (WaitHandle.WaitAny(_waitHandles) == 0)
            {
                _synchDataReady.EndInvoke(asyncResult);
            }
        }

        if (Pipeline.Output.EndOfPipeline)
        {
            // we're done! invoke the DataEnd event
            var asyncResult = _synchDataEnd.BeginInvoke(null, null);
            _waitHandles[0] = asyncResult.AsyncWaitHandle;

            if (WaitHandle.WaitAny(_waitHandles) == 0)
            {
                _synchDataEnd.EndInvoke(asyncResult);
            }

        }
    }

    /// <summary>
    /// private DataReady handling method that will pass the call on to any event handlers that are
    /// attached to the OnDataReady event of this <see cref="PipelineExecutor"/> instance.
    /// </summary>
    /// <param name="data"></param>
    private void SynchDataReady(ICollection<PSObject> data)
    {
        var delegateDataReadyCopy = OnDataReady;
        if (delegateDataReadyCopy != null)
        {
            delegateDataReadyCopy(data);
        }
    }

    /// <summary>
    /// private DataEnd handling method that will pass the call on to any handlers that are
    /// attached to the OnDataEnd event of this <see cref="PipelineExecutor"/> instance.
    /// </summary>
    private void SynchDataEnd()
    {
        var delegateDataEndCopy = OnDataEnd;
        if (delegateDataEndCopy != null)
        {
            delegateDataEndCopy();
        }
    }

    /// <summary>
    /// private ErrorReady handling method that will pass the call on to any event handlers that are
    /// attached to the OnErrorReady event of this <see cref="PipelineExecutor"/> instance.
    /// </summary>
    /// <param name="data"></param>
    private void SynchErrorReady(ICollection<object> data)
    {
        var delegateErrorReadyCopy = OnErrorReady;
        if (delegateErrorReadyCopy != null)
        {
            delegateErrorReadyCopy(data);
        }
    }

    #endregion
}