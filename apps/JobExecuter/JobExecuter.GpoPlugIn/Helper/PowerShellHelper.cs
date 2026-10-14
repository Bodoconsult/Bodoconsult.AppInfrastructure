// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace JobExecuter.GpoPlugIn.Helper;

/// <summary>
/// Helper for powershell based features
/// </summary>
public static class PowerShellHelper
{
    public static RunspacePool RsPool { get; }

    /// <summary>
    /// Run a powrshell command
    /// </summary>
    /// <param name="command">Powerhell command as string</param>
    /// <returns>List of <see cref="PSObject"/> instances or null</returns>
    public static Collection<PSObject> Execute(string command)
    {
        using var ps = PowerShell.Create();
        ps.RunspacePool = RsPool;
        ps.AddScript(command);

        var pipelineObjects = ps.Invoke();

        return pipelineObjects;
    }

    /// <summary>
    /// Default ctor
    /// </summary>
    static PowerShellHelper()
    {
        var defaultSessionState = InitialSessionState.CreateDefault();
        defaultSessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Unrestricted;
        defaultSessionState.ImportPSModule("GroupPolicy");

        // use the runspace factory to create a pool of runspaces
        // with a minimum and maximum number of runspaces to maintain.

        RsPool = RunspaceFactory.CreateRunspacePool(defaultSessionState);
        RsPool.SetMinRunspaces(1);
        RsPool.SetMaxRunspaces(5);

        // set the pool options for thread use.
        // we can throw away or re-use the threads depending on the usage scenario.

        RsPool.ThreadOptions = PSThreadOptions.UseNewThread;
       
        // open the pool. 
        // this will start by initializing the minimum number of runspaces.
       
        RsPool.Open();
    }
}