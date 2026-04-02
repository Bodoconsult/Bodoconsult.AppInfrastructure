// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Delegates;
using Bodoconsult.App.Interfaces;

namespace Bodoconsult.App.BusinessTransactions;

/// <summary>
/// Represents a business transaction that can be called from a client the current app
/// </summary>
public class BusinessTransaction
{
    /// <summary>
    /// A unique transaction ID. Transactions are called by this ID by the client
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the transaction
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Error code the transaction should return. Default: 0 means no error 
    /// </summary>
    public int ErrorCode { get; set; } = 0;

    /// <summary>
    /// The error message the transaction should return
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// The business logic method that should be called for the current transaction
    /// </summary>
    public RunBusinessTransactionDelegate RunBusinessTransactionDelegate { get; set; }

    /// <summary>
    /// Do not add an event counter for this transaction
    /// </summary>
    public bool NoEventCounter { get; set; } = false;

    /// <summary>
    /// Allowed type names of <see cref="IBusinessTransactionRequestData"/>
    /// </summary>
    public List<string> AllowedRequestDataTypes { get; } = [];
}