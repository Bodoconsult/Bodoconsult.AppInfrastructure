// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.BusinessTransactions.RequestData;

/// <summary>
/// Request data for string based transaction
/// </summary>
public class StringBusinessTransactionRequestData : BaseBusinessTransactionRequestData
{
    /// <summary>
    /// The string content provided by the request
    /// </summary>
    public string? Content { get; set; }
}