// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Runtime.InteropServices;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Creates a report for a Microsoft DNS server
/// </summary>
/// <remarks>User running that job needs to be member of security group DNS administrators on the DNS server</remarks>
/// <example>
/// <include file="..\Samples\DnsJobTask.xml" path="*" />
///  </example>
public sealed class DnsJobTask : DocuBaseJobTask
{

    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new Exception($"{GetType().Name}: Works only on Windows OS.");
        }

        return new DnsExecuterFactory();
    }

    /// <summary>
    /// FQDN of the DNS server
    /// </summary>
    public string Server { get; set; }
}