// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Runtime.Versioning;
using Bodoconsult.App;
using Bodoconsult.App.Abstractions.Interfaces;
using OpenSourceConnectorBodoCms.DiContainerProvider;

namespace OpenSourceConnectorBodoCms;

[SupportedOSPlatform("windows10.0.17763")]
public class OpenSourceConnectorOpenSourceConnectorBodoCmsAppBuilder : BaseAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Global app settings</param>
    public OpenSourceConnectorOpenSourceConnectorBodoCmsAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    { }

    /// <summary>
    /// Load the <see cref="IAppBuilder.DiContainerServiceProviderPackage"/>
    /// </summary>
    public override void LoadDiContainerServiceProviderPackage()
    {
        var factory = new OpenSourceConnectorBodoCmsProductionDiContainerServiceProviderPackageFactory(AppGlobals);
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

        //var section = root.GetSection("OpenSourceConnectorBodoCms");

        //if (AppGlobals is not IOpenSourceConnectorBodoCmsAppGlobals globals)
        //{
        //    throw new ArgumentException("AppGlobals is not IOpenSourceConnectorBodoCmsAppGlobals");
        //}

        //globals.UseSmtp = DefaultAppStartProvider.ReadBoolProperty(section, "UseSmtp", globals.UseSmtp);
    }
}