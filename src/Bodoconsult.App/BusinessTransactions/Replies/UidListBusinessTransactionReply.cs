// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.BusinessTransactions.Replies;

/// <summary>
/// A business transaction reply transporting a list of UIDs
/// </summary>
public class UidListBusinessTransactionReply : DefaultBusinessTransactionReply
{
    /// <summary>
    /// UID to transport with the reply
    /// </summary>
    public List<Guid> Uids { get; set; } = [];

}