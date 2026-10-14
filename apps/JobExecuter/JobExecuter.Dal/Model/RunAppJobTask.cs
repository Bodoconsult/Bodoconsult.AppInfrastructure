// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Runtime.InteropServices;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Run an app with or without parameters
/// </summary>
/// <example>
/// <include file="..\Samples\RunAppJobTask.xml" path="*" />
///  </example>
public sealed class RunAppJobTask : BaseJobTask
{
    /// <summary>
    /// Full path to app to run. Path may contain blanks
    /// </summary>
    public string Filename { get; set; }
    /// <summary>
    /// Arguments to start the app with
    /// </summary>
    public string Arguments { get; set; }


    /// <summary>
    /// Run app in interactive mode. default: false
    /// </summary>
    public bool InteractiveMode { get; set; }


    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new Exception($"{GetType().Name}: Works only on Windows OS.");
        }

        return new RunAppExecuterFactory();
    }
}