// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using Bodoconsult.App.Abstractions.Benchmarking;
using Bodoconsult.App.Interfaces;

namespace Bodoconsult.App.BusinessTransactions.RequestData;

/// <summary>
/// Base class for <see cref="IBusinessTransactionListRequestData"/> implementations
/// </summary>
public abstract class BaseBusinessTransactionListRequestData : IBusinessTransactionListRequestData
{
    /// <summary>
    /// The ID of the requested business transaction
    /// </summary>
    public int TransactionId { get; set; }

    /// <summary>
    /// Unique GUID of the transaction
    /// </summary>
    public Guid TransactionGuid { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Benchmark object (see output in XXX_Benchmark.csv)
    /// Make sure to create it, addStep, and dispose it 
    /// </summary>
    public Bench Benchmark { get; set; }

    /// <summary>
    /// Request metadata: client GUID
    /// </summary>
    public Guid MetaDataClientGuid { get; set; }

    /// <summary>
    /// Request metadata: client name
    /// </summary>
    public string MetaDataClientName { get; set; }

    /// <summary>
    /// Request metadata: IP address the request is coming from
    /// </summary>
    public string MetaDataIpAddress { get; set; }

    /// <summary>
    /// Request metadata: user name in cleartext
    /// </summary>
    public string MetaDataUserName { get; set; }

    /// <summary>
    /// Request metadata: User ID. A value of int.MaxValue means the system user.
    /// </summary>
    public int MetaDataUserId { get; set; }

    /// <summary>
    /// Timeout to wait for sending the request
    /// </summary>
    public int SendTimeout { get; set; } = 5000;

    /// <summary>
    /// Timeout to wait for sending the request
    /// </summary>
    public int ExecutionTimeout { get; set; } = 5000;

    /// <summary>
    /// LineId of calling client
    /// </summary>
    public int MetaDataLineId { get; set; }

    /// <summary>
    /// Current page number (if applicable)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Current page size (if applicable)
    /// </summary>
    public int PageSize { get; set; } = int.MaxValue;
}