// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Runtime.Versioning;
using BodoCmsConsole.DiContainerProvider;
using Bodoconsult.App;
using Bodoconsult.App.Abstractions.Interfaces;

namespace BodoCmsConsole;

[SupportedOSPlatform("windows10.0.17763")]
public class BodoCmsAppBuilder: BaseAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Global app settings</param>
    public BodoCmsAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    { }

    /// <summary>
    /// Load the <see cref="IAppBuilder.DiContainerServiceProviderPackage"/>
    /// </summary>
    public override void LoadDiContainerServiceProviderPackage()
    {
        var factory = new BodoCmsProductionDiContainerServiceProviderPackageFactory(AppGlobals);
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

        //var section = root.GetSection("BodoCms");

        //if (AppGlobals is not IBodoCmsAppGlobals globals)
        //{
        //    throw new ArgumentException("AppGlobals is not IBodoCmsAppGlobals");
        //}

        //globals.UseSmtp = DefaultAppStartProvider.ReadBoolProperty(section, "UseSmtp", globals.UseSmtp);
    }
}