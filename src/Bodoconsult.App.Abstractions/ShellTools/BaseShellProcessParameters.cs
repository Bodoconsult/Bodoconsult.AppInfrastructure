// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;

namespace Bodoconsult.App.Abstractions.ShellTools;

/// <summary>
/// Base class for shell process parameters
/// </summary>
public abstract class BaseShellProcessParameters
{
    /// <summary>
    /// A delegate for "status message" method calls getting a string a input parameter
    /// </summary>
    public StatusMessageDelegate? StatusMessageDelegate { get; set; }

    /// <summary>
    /// Delegate called if an app exception has been raised and a message to the UI has to be sent before app terminates
    /// </summary>
    public HandleExceptionDelegate? HandleExceptionDelegate { get; set; }

    /// <summary>
    /// Working directory. Default: Environment.CurrentDirectory
    /// </summary>
    public string WorkingDirectory { get; set; } = Environment.CurrentDirectory;

    /// <summary>
    /// Do not check the exit code of the process for errors
    /// </summary>
    public bool DoNotCheckExitCode { get; set; }
}