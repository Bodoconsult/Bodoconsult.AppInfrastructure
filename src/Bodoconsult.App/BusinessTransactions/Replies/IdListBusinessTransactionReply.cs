// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.BusinessTransactions.Replies;

/// <summary>
/// A business transaction reply transporting a list of IDs to the client
/// </summary>
public class IdListBusinessTransactionReply : DefaultBusinessTransactionReply
{
    /// <summary>
    /// Client type assigend to the client logged int
    /// </summary>
    public List<int> IdList { get; set; } = [];
}