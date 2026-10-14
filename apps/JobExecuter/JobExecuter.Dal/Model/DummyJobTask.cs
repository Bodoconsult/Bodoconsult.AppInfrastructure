// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Dummy task for testing purposes
/// </summary>
public sealed class DummyJobTask : BaseJobTask
{
    /// <summary>
    /// message to deliver for testing purposes
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// simulated run time
    /// </summary>
    public int WaitTime { get; set; }

    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        return new DummyExecuterFactory();
    }
}