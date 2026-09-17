// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Runtime.Versioning;
using Bodoconsult.App;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoFileTransfer.Business.Interfaces;
using BodoFileTransfer.DiContainerProvider;

namespace BodoFileTransfer;

[SupportedOSPlatform("windows10.0.17763.0")]
public class BodoFileTransferAppBuilder : BaseAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Global app settings</param>
    public BodoFileTransferAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    { }

    /// <summary>
    /// Load the <see cref="IAppBuilder.DiContainerServiceProviderPackage"/>
    /// </summary>
    public override void LoadDiContainerServiceProviderPackage()
    {
        var factory = new BodoFileTransferProductionDiContainerServiceProviderPackageFactory(AppGlobals);
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

        var section = root.GetSection("BodoFileTransfer");

        if (AppGlobals is not IBodoFileTransferAppGlobals globals)
        {
            throw new ArgumentException("AppGlobals is not IBodoFileTransferAppGlobals");
        }

        globals.EmailSentDateWeeksBack = DefaultAppStartProvider.ReadIntProperty(section, "EmailSentDateWeeksBack", globals.EmailSentDateWeeksBack);
        globals.NumberSchema = DefaultAppStartProvider.ReadStringProperty(section, "NumberSchema", globals.NumberSchema);
        globals.SignatureFileExtensions = DefaultAppStartProvider.ReadStringProperty(section, "SignatureFileExtensions", globals.SignatureFileExtensions);
    }
}