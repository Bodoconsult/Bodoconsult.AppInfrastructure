// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using BodoFileTransfer.Business.Enums;

namespace BodoFileTransfer.Business.Model;

/// <summary>
/// Trace log entry
/// </summary>
public class TraceEntry
{

    /// <summary>
    /// Id of the trace entry
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();


    /// <summary>
    /// File ID or Guid.Empty for application events
    /// </summary>
    public Guid FileId { get; set; } = Guid.Empty;

    /// <summary>
    /// Trace message code
    /// </summary>
    public TraceMessageCode MessageCode { get; set; } = TraceMessageCode.ApplicationEvent;

    /// <summary>
    /// Trace message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Date of trace writing
    /// </summary>
    public DateTime Date { get; set; } = DateTime.Now;

}