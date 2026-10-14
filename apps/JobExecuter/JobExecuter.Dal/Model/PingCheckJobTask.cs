// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Check successful ping to certain network addresses
/// </summary>
/// <example>
/// <include file='..\Samples\PingCheckJobTask.xml' path='*'/>
///  </example>
public sealed class PingCheckJobTask : BaseJobTask
{

    /// <summary>
    /// default ctor
    /// </summary>
    public PingCheckJobTask()
    {
        PingTimeout = 15000;
    }

    /// <summary>
    /// IP-addresses separated by semicolon
    /// </summary>
    public string IpAdresses { get; set; }

    /// <summary>
    /// Timeout in milliseconds for Ping. Default: 15.000 
    /// </summary>
    public int PingTimeout { get; set; }


    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        return new PingCheckExecuterFactory();
    }

}