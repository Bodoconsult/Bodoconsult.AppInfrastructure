// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Enums;
using JobExecuter.Dal.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Run a PowerShell script file
/// </summary>
/// <example>
/// <include file='..\Samples\PsBatchJobTask.xml' path='*'/>
///  </example>
public sealed class PsBatchJobTask : BaseJobTask
{

    /// <summary>
    /// default ctor
    /// </summary>
    public PsBatchJobTask()
    {
        PowerShellVersion = PowerShellVersion.V03;
    }

    /// <summary>
    /// Full path to a powershell script file to run. Path may contain blanks
    /// </summary>
    public string Filename { get; set; }


    /// <summary>
    /// The version of PowerShell to run the <see cref="PsBatchJobTask"/> on
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public PowerShellVersion PowerShellVersion { get; set; }


    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        return new PsBatchExecuterFactory();
    }

}