// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Interfaces;

namespace Bodoconsult.App.BusinessTransactions.Replies;

/// <summary>
/// Default business transaction reply
/// </summary>
public class DefaultBusinessTransactionListReply : IBusinessTransactionListReply
{
    /// <summary>
    /// The current request data
    /// </summary>
    public IBusinessTransactionRequestData? RequestData { get; set; }

    /// <summary>
    /// Current error code. Default is 0 for no error happened
    /// </summary>
    public int ErrorCode { get; set; }

    /// <summary>
    /// Current message provided by the business transaction
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Exception message
    /// </summary>
    public string? ExceptionMessage { get; set; }

    /// <summary>
    /// Current number of pages (if applicable)
    /// </summary>
    public int PageCount { get; set; }

    /// <summary>
    /// Current number of rows provided by this call (if applicable)
    /// </summary>
    public int RowCount { get; set; }

    /// <summary>
    /// The notification object to send via GRPC etc to the client
    /// </summary>
    public object? NotificationObjectToSend { get; set; }
}