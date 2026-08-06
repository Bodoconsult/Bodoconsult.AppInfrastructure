// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.BusinessTransactions.Replies;

/// <summary>
/// A business transaction reply transporting a string
/// </summary>
public class StringBusinessTransactionReply : DefaultBusinessTransactionReply
{
    /// <summary>
    /// String to transport with the reply
    /// </summary>
    public string? Content { get; set; }

}