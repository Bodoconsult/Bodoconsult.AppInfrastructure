// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Runtime.InteropServices;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Factories;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Creates a report of the current Windows domain's Active Directory structure.
/// </summary>
/// <remarks>Work only for single domain networks. Domain forest are not supported. Delivers a report over the structure of the AD with Organizational Units (OU),  users, groups and computers.</remarks>
/// <example>
/// <include file="..\Samples\ActiveDirectoryJobTask.xml" path="*" />
///  </example>
public sealed class ActiveDirectoryJobTask : DocuBaseJobTask
{
    /// <inheritdoc />
    public override ITaskExecuterFactory GetFactory()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new Exception($"{nameof(ActiveDirectoryJobTask)}: Works only on Windows OS.");
        }

        return new ActiveDirectoryExecuterFactory();
    }
}