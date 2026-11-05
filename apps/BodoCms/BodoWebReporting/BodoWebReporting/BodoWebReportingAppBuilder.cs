// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Runtime.Versioning;
using Bodoconsult.App;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoWebReporting.DiContainerProvider;

namespace BodoWebReporting;

[SupportedOSPlatform("windows10.0.17763")]
public class BodoWebReportingAppBuilder: BaseAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Global app settings</param>
    public BodoWebReportingAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    { }

    /// <summary>
    /// Load the <see cref="IAppBuilder.DiContainerServiceProviderPackage"/>
    /// </summary>
    public override void LoadDiContainerServiceProviderPackage()
    {
        var factory = new BodoWebReportingProductionDiContainerServiceProviderPackageFactory(AppGlobals);
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

        //var section = root.GetSection("BodoWebReporting");

        //if (AppGlobals is not IBodoWebReportingAppGlobals globals)
        //{
        //    throw new ArgumentException("AppGlobals is not IBodoWebReportingAppGlobals");
        //}

        //globals.UseSmtp = DefaultAppStartProvider.ReadBoolProperty(section, "UseSmtp", globals.UseSmtp);
    }
}