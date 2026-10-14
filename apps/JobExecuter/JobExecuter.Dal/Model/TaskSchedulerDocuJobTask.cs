// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Runtime.InteropServices;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Task to save TaskScheduler tasks as XML and document it
/// </summary>
/// <example>
/// <include file='..\Samples\TaskSchedulerDocuJobTask.xml' path='*'/>
///  </example>
public sealed class TaskSchedulerDocuJobTask : DocuBaseJobTask
{
    /// <summary>
    /// Folder to export the tasks in
    /// </summary>
    public string XmlTargetPath { get; set; }

    /// <summary>
    /// Filters task by the filter expression added here. Seperated values with semicolon. Task name has to start with filter expression
    /// </summary>
    /// <example>Test;Produktiv;System</example>
    public string Filter { get; set; }



    /// <summary>
    /// Target server to access for windows task scheduler. Default: null = localhost
    /// </summary>
    public string TargetServer { get; set; }


    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new Exception($"{GetType().Name}: Works only on Windows OS.");
        }

        return new TaskSchedulerDocuExecuterFactory();
    }

}