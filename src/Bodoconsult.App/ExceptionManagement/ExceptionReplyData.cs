// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

namespace Bodoconsult.App.ExceptionManagement;

/// <summary>
/// Default data for exception reply generation
/// </summary>
public class ExceptionReplyData
{
    /// <summary>
    /// Error code to use
    /// </summary>
    public int ErrorCode { get; set; }

    /// <summary>
    /// Message to log for the error
    /// </summary>
    public string? Message { get; set; }


    /// <summary>
    /// Additional error message with information like stack trace
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Deliver always an empty error message
    /// </summary>
    public bool EmptyErrorMessage { get; set; }

    /// <summary>
    /// Iterate inner exceptions if existing. Default: true
    /// </summary>
    public bool IterateExceptions { get; set; } = true;

}