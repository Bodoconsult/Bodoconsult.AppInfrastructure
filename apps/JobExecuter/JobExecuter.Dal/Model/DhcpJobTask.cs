// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Runtime.InteropServices;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Creates a report of the current Windows domain's DHCP infrastructure
/// </summary>
/// <remarks>User running that job needs to be member of security group DHCP Administrators on the DHCP server</remarks>
/// <example>
/// <include file="..\Samples\DhcpJobTask.xml" path="*" />
///  </example>
public sealed class DhcpJobTask : DocuBaseJobTask
{
    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new Exception($"{nameof(DhcpJobTask)}: Works only on Windows OS.");
        }

        return new DhcpExecuterFactory();
    }
}