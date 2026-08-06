// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


namespace Bodoconsult.App.BusinessTransactions.RequestData;

/// <summary>
/// The request data for an business transaction request asking for a certain object name
/// </summary>
public class ObjectNameBusinessTransactionRequestData : BaseBusinessTransactionRequestData
{
    /// <summary>
    /// The name of the requested object
    /// </summary>
    public string? Name { get; set; }

}