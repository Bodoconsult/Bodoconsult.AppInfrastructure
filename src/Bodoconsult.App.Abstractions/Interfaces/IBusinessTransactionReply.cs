// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Basic content of a business transaction reply
/// </summary>
public interface IBusinessTransactionReply: IClientNotification
{
    /// <summary>
    /// The current request data
    /// </summary>
    public IBusinessTransactionRequestData? RequestData { get; set; }

    /// <summary>
    /// Current error code. Default is 0 for no error happened
    /// </summary>
    int ErrorCode { get; set; }

    /// <summary>
    /// Current message provided by the business transaction
    /// </summary>
    string? Message { get; set; }

    /// <summary>
    /// Current error message provided by the business transaction
    /// </summary>

    string? ExceptionMessage { get; set; }

}