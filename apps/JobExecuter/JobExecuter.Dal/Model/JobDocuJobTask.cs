// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;
using Newtonsoft.Json;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Task to document the current JobExecuter job
/// </summary>
public sealed class JobDocuJobTask : DocuBaseJobTask
{
    /// <summary>
    /// Full path of the CJSON file with JobExecuter job to document
    /// </summary>
    public string JobJsonFileName { get; set; }

    /// <summary>
    /// Current job. Will be used if <see cref="JobJsonFileName"/> is empty.
    /// </summary>
    [JsonIgnore]
    public IJob CurrentJob { get; set; }

    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        return new JobDocuExecuterFactory();
    }
}