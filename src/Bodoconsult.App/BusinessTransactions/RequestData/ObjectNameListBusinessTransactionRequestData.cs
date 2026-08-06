// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.BusinessTransactions.RequestData;

/// <summary>
/// The request data for a business transaction request asking list of data for a certain object name
/// </summary>
public class ObjectNameListBusinessTransactionRequestData : BaseBusinessTransactionListRequestData
{
    /// <summary>
    /// The name of the requested object
    /// </summary>
    public string? Name { get; set; }

}