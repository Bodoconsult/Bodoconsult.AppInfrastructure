// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoWebMailer.DiContainerProvider;
using System;
using System.Runtime.Versioning;
using BodoWebMailer.Business.Interfaces;

namespace BodoWebMailer;

[SupportedOSPlatform("windows10.0.17763")]
public class BodoWebMailerAppBuilder: BaseAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Global app settings</param>
    public BodoWebMailerAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    { }

    /// <summary>
    /// Load the <see cref="IAppBuilder.DiContainerServiceProviderPackage"/>
    /// </summary>
    public override void LoadDiContainerServiceProviderPackage()
    {
        var factory = new BodoWebMailerProductionDiContainerServiceProviderPackageFactory(AppGlobals);
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

        var section = root.GetSection("BodoWebMailer");

        if (AppGlobals is not IBodoWebMailerAppGlobals globals)
        {
            throw new ArgumentException("AppGlobals is not IBodoWebMailerAppGlobals");
        }

        globals.UseSmtp = DefaultAppStartProvider.ReadBoolProperty(section, "UseSmtp", globals.UseSmtp);
    }
}