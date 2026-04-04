// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BusinessTransactions.Replies;
using Bodoconsult.App.ExceptionManagement;

namespace Bodoconsult.App.Interfaces;

/// <summary>
/// Interface for a builder providing central exception reply management
/// </summary>
public interface IExceptionReplyBuilder
{
    /// <summary>
    /// Default error code
    /// </summary>
    int DefaultErrorCode { get; set; }

    /// <summary>
    /// Exception replies currently loaded for usage in the app
    /// </summary>
    Dictionary<string, ExceptionReplyData> ExceptionReplies { get; }

    /// <summary>
    /// Create a reply for an exception that has happened
    /// </summary>
    /// <param name="exception">Exception that has been thrown</param>
    /// <returns>A newly created <see cref="DefaultBusinessTransactionReply"/> instance</returns>
    IBusinessTransactionReply CreateReply(Exception exception);

    /// <summary>
    /// Add an <see cref="IExceptionReplyProvider"/> instance
    /// </summary>
    /// <param name="exceptionReplyProvider">Provider for exception replies to add</param>
    void AddProvider(IExceptionReplyProvider exceptionReplyProvider);
}