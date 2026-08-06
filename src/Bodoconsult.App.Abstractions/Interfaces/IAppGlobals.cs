// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Licence MIT

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for global app settings with lifetime for the whole app lifetime
/// </summary>
public interface IAppGlobals: IDisposable
{
    /// <summary>
    /// This event is set if the application is started only as singleton
    /// </summary>
    public EventWaitHandle? EventWaitHandle { get; set; }

    /// <summary>
    /// App start parameter
    /// </summary>
    IAppStartParameter AppStartParameter { get; set; }

    /// <summary>
    /// Current logging config
    /// </summary>
    LoggingConfig LoggingConfig { get; set; }

    /// <summary>
    /// Current app logger. Use this instance only if no DI container is available. Nonetheless, use DiContainer.Get&lt;IAppLoggerProxy&gt; to fetch the default app logger from DI container. Don't forget to load it during DI setup!
    /// </summary>
    IAppLoggerProxy? Logger { get; set; }

    /// <summary>
    /// Current dependency injection (DI) container
    /// </summary>
    DiContainer DiContainer { get; set; }

    /// <summary>
    /// Delegate called if a fatale app exception has been raised and a message to the UI has to be sent before app terminates
    /// </summary>
    HandleFatalExceptionDelegate? HandleFatalExceptionDelegate  { get; set; }

    /// <summary>
    /// Current app storage connection check instance or null
    /// </summary>
    IAppStorageConnectionCheck? AppStorageConnectionCheck { get; set; }

    /// <summary>
    /// Current status message delegate
    /// </summary>
    public StatusMessageDelegate? StatusMessageDelegate { get; set; }

    /// <summary>
    /// Current license management delegate
    /// </summary>
    public LicenseMissingDelegate? LicenseMissingDelegate { get; set; }

    /// <summary>
    /// Delegate to handle I18N translations
    /// </summary>
    TranslateDelegate? TranslateDelegate { get; set; }

    /// <summary>
    /// Delegate to handle I18N translations with parameters to fill in translated text
    /// </summary>
    TranslateWithParamsDelegate? TranslateWithParamsDelegate { get; set; }

    /// <summary>
    /// Externally registered product name
    /// </summary>
    string? ProductName { get; set; }

    /// <summary>
    /// Externally registered version
    /// </summary>
    string? ProductVersion { get; set; }

    /// <summary>
    /// The current configuration loaded from appsettings.json
    /// </summary>
    IConfigurationRoot? ConfigurationRoot { get; set; }
}