// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Checks the disk size and places an error if free space is below a certain percentage or below a certain absolute value
/// </summary>
/// <example>
/// <include file='..\Samples\DiskSizeJobTask.xml' path='*'/>
///  </example>
public sealed class DiskSizeJobTask : BaseJobTask
{

    /// <summary>
    /// default ctor
    /// </summary>
    public DiskSizeJobTask()
    {
        Drives=new List<Drive>();
    }

    /// <summary>
    /// Drives to check for disk space
    /// </summary>
    public IList<Drive> Drives { get; private set; }


    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        return new DiskSizeExecuterFactory();
    }

}