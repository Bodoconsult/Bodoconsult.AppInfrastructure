// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Executer;

namespace JobExecuter.Dal.Factories;

/// <summary>
/// Factory for <see cref="DiskSizeExecuter"/>
/// </summary>
internal class DiskSizeExecuterFactory : ITaskExecuterFactory
{
    /// <summary>
    /// Create new executer
    /// </summary>
    /// <returns>New job task executer</returns>
    public ITaskExecuter New(IJobTask jobTask)
    {
        var e = new DiskSizeExecuter(jobTask);

        return e;
    }
}