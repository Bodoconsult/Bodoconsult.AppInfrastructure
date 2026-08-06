// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.BusinessTransactions.Replies;

/// <summary>
/// A business transaction reply transporting an object
/// </summary>
public class ObjectBusinessTransactionReply : DefaultBusinessTransactionReply
{
    /// <summary>
    /// Object to transport with the reply
    /// </summary>
    public object? Object { get; set; }

}