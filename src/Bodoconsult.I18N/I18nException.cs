// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


namespace Bodoconsult.I18N;

/// <summary>
/// I18N exception
/// </summary>
public class I18NException : Exception
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="message">Message</param>
    /// <param name="innerException">Inner exception</param>
    public I18NException(string message, Exception? innerException = null) : base(message, innerException)
    { }
}