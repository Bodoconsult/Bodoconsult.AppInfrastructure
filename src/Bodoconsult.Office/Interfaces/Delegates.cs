// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.Office.Interfaces;

/// <summary>
/// Delegate for sending status message to UI
/// </summary>
/// <param name="message">Message to send</param>
public delegate void StatusHandlerDelegate(string message);

/// <summary>
/// Delegate for sending error message to UI
/// </summary>
/// <param name="ex">Exception to provide to UI</param>
/// <param name="message">Message to send</param>
public delegate void ErrorHandlerDelegate(Exception ex, string? message);