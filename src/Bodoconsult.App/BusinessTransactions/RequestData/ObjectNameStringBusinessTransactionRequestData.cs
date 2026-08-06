// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.BusinessTransactions.RequestData;

/// <summary>
/// Request data for string based transaction
/// </summary>
public class ObjectNameStringBusinessTransactionRequestData : BaseBusinessTransactionRequestData
{

    /// <summary>
    /// The object name provided by the request
    /// </summary>
    public string? ObjectName { get; set; }

    /// <summary>
    /// The string value provided by the request
    /// </summary>
    public string? Value { get; set; }

}