// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace JobExecuter.Base.Interfaces;

/// <summary>
/// Factory for <see cref="ITaskExecuter"/> objects
/// </summary>
public interface ITaskExecuterFactory
{
    /// <summary>
    /// Create new executer
    /// </summary>
    /// <returns>New job task executer</returns>
    ITaskExecuter New(IJobTask jobTask);
}