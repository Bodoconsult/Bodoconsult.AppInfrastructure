// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Enums;
using JobExecuter.Dal.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JobExecuter.Dal.Model;

/// <summary>
    /// Task to run a cmd.exe batch from a file or a string.  
    /// </summary>
    /// <remarks>Set either <see cref="Filename"/> or <see cref="BatchFileContent"/>. In doubt <see cref="BatchFileContent"/> wins!
    /// The advantage of running a batch in mode <see cref="CmdBatchRunType.LineByLine"/> that the batch runs line per line as separated processes. 
    /// The lines must be independent from each other.
    /// </remarks>
    /// <example>
    /// <include file='..\Samples\CmdBatchJobTask.xml' path='*'/>
    ///  </example>
    public sealed class CmdBatchJobTask : BaseJobTask
{
    /// <summary>
    /// Filename of .bat or .cmd file to run. Set either <see cref="Filename"/> or <see cref="BatchFileContent"/>. In doubt <see cref="BatchFileContent"/> wins!
    /// </summary>
    public string Filename { get; set; }

    /// <summary>
    /// Type of how to run a cmd batch file
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public CmdBatchRunType RunType { get; set; }


    /// <summary>
    /// Batch commands as string (like in a .bat or .cmd file). Commands in the string run always in mode <see cref="CmdBatchRunType.LineByLine"/>.  Set either <see cref="Filename"/> or <see cref="BatchFileContent"/>. In doubt <see cref="BatchFileContent"/> wins!
    /// </summary>
    public string BatchFileContent { get; set; }


    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        return new CmdBatchExecuterFactory();
    }
}