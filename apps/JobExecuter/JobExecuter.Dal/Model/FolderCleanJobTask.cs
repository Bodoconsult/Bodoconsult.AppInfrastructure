// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Task to clean a folder from old files
/// </summary>
/// <example>
/// <include file='..\Samples\FolderCleanJobTask.xml' path='*'/>
///  </example>
public sealed class FolderCleanJobTask : BaseJobTask
{
    /// <summary>
    /// Path to clean
    /// </summary>
    public string FolderPath { get; set; }

    /// <summary>
    /// Clean folder from files older then X days
    /// </summary>
    public int Days { get; set; }

    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        return new FolderCleanExecuterFactory();
    }
}