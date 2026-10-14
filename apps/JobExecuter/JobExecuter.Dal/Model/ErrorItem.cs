// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using JobExecuter.Base.Interfaces;

namespace JobExecuter.Dal.Model;

/// <summary>
/// Contains errors and the job tasks they happened
/// </summary>
public class ErrorItem
{
    /// <summary>
    /// The <see cref="IJobTask"/>, the exception that happened in
    /// </summary>
    public IJobTask JobTask { get; set; }

    /// <summary>
    /// The <see cref="Exception"/> that happened
    /// </summary>
    public Exception Exception { get; set; }

}