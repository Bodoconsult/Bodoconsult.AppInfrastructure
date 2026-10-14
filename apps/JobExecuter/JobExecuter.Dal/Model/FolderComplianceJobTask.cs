// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <inheritdoc />
/// <summary>
/// Task to create a compliance report for a folder. 
/// </summary>
/// <remarks>FolderComplianceJobTask requires running under administrative privileges due to accessing file system access control list.</remarks>
/// <example>
/// <include file="..\Samples\FolderComplianceJobTask.xml" path="*" />
///  </example>
public sealed class FolderComplianceJobTask : DocuBaseJobTask
{

    /// <summary>
    /// Default ctor
    /// </summary>
    public FolderComplianceJobTask()
    {
        FolderPaths = new List<ComplianceFolder>();
    }

    /// <summary>
    /// Path to create compiance reporting for
    /// </summary>
    public IList<ComplianceFolder> FolderPaths { get; set; }


    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new Exception($"{GetType().Name}: Works only on Windows OS.");
        }

        return new FolderComplianceExecuterFactory();
    }

}