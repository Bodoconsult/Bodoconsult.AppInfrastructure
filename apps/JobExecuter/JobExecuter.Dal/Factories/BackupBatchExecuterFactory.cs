// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Executer;

namespace JobExecuter.Dal.Factories;

/// <summary>
/// Factory for <see cref="BackupBatchExecuter"/>
/// </summary>
internal class BackupBatchExecuterFactory : ITaskExecuterFactory
{
    /// <summary>
    /// Create new executer
    /// </summary>
    /// <returns>New job task executer</returns>
    public ITaskExecuter New(IJobTask jobTask)
    {
        var e = new BackupBatchExecuter(jobTask);

        return e;
    }
}