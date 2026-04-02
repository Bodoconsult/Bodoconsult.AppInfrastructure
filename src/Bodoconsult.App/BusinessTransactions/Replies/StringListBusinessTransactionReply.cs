// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.BusinessTransactions.Replies;

/// <summary>
/// A business transaction reply transporting a list string
/// </summary>
public class StringListBusinessTransactionReply : DefaultBusinessTransactionListReply
{
    /// <summary>
    /// String to transport with the reply
    /// </summary>
    public List<string> Content { get; set; } = [];

}