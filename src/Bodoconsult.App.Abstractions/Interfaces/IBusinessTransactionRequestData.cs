// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Benchmarking;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for defining minimum requirements for business transaction request data
/// </summary>
public interface IBusinessTransactionRequestData
{
    /// <summary>
    /// The ID of the requested business transaction
    /// </summary>
    int TransactionId { get; set; }

    /// <summary>
    /// Unique GUID of the current transaction
    /// </summary>
    Guid TransactionGuid { get; set; }

    /// <summary>
    /// Benchmark object (see output in XXX_Benchmark.csv)
    /// Make sure to create it, addStep, and dispose it 
    /// </summary>
    Bench Benchmark { get; set; }

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
    /// Request metadata: User ID
    /// </summary>
    public int MetaDataUserId { get; set; }

    /// <summary>
    /// Timeout to wait for sending the request
    /// </summary>
    int SendTimeout { get; set; }

    /// <summary>
    /// Timeout to wait for sending the request
    /// </summary>
    int ExecutionTimeout { get; set; }
}