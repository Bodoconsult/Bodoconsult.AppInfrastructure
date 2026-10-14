// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Executer;

namespace JobExecuter.Dal.Factories;

/// <summary>
/// Factory for <see cref="SqlBatchExecuter"/>
/// </summary>
internal class SqlBatchExecuterFactory : ITaskExecuterFactory
{
    /// <summary>
    /// Create new executer
    /// </summary>
    /// <returns>New job task executer</returns>
    public ITaskExecuter New(IJobTask jobTask)
    {
        var e = new SqlBatchExecuter(jobTask);

        return e;
    }
}