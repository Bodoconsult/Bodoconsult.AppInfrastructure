// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoFtpTransfer.Business.Interfaces;
using BodoFtpTransfer.DiContainerProvider;
using System.Runtime.Versioning;

namespace BodoFtpTransfer;

[SupportedOSPlatform("windows10.0.17763")]
public class BodoFtpTransferAppBuilder : BaseAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Global app settings</param>
    public BodoFtpTransferAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    { }

    /// <summary>
    /// Load the <see cref="IAppBuilder.DiContainerServiceProviderPackage"/>
    /// </summary>
    public override void LoadDiContainerServiceProviderPackage()
    {
        var factory = new BodoFtpTransferProductionDiContainerServiceProviderPackageFactory(AppGlobals);
        DiContainerServiceProviderPackage = factory.CreateInstance();
    }

    /// <summary>
    /// Process the configuration from <see cref="IAppStartParameter.ConfigFile"/>
    /// </summary>
    public override void ProcessConfiguration()
    {
        // Load basic config
        base.ProcessConfiguration();

        ArgumentNullException.ThrowIfNull(AppStartProvider?.AppConfigurationProvider);

        // Now get the root configuration element
        var root = AppStartProvider.AppConfigurationProvider.Configuration;

        if (root is null)
        {
            return;
        }

        var section = root.GetSection("BodoFtpTransfer");

        if (AppGlobals is not IBodoFtpTransferAppGlobals globals)
        {
            throw new ArgumentException("AppGlobals is not IBodoFtpTransferAppGlobals");
        }

        globals.RemoteDirectory = DefaultAppStartProvider.ReadStringProperty(section, "RemoteDirectory", globals.RemoteDirectory) ?? string.Empty;
        globals.ExcludeFiles= DefaultAppStartProvider.ReadStringProperty(section, "ExcludeFiles", globals.ExcludeFiles) ?? string.Empty;
        globals.ExcludeDirs = DefaultAppStartProvider.ReadStringProperty(section, "ExcludeDirs", globals.ExcludeDirs) ?? string.Empty;
        globals.BaseDirectory = DefaultAppStartProvider.ReadStringProperty(section, "BaseDirectory", globals.BaseDirectory) ?? string.Empty;
    }
}