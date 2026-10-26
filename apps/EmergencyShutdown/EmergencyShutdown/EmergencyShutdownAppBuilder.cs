// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App;
using Bodoconsult.App.Abstractions.Interfaces;
using EmergencyShutdown.DiContainerProvider;
using System.Runtime.Versioning;

namespace EmergencyShutdown;

[SupportedOSPlatform("windows10.0.17763")]
public class EmergencyShutdownAppBuilder: BaseAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Global app settings</param>
    public EmergencyShutdownAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    { }

    /// <summary>
    /// Load the <see cref="IAppBuilder.DiContainerServiceProviderPackage"/>
    /// </summary>
    public override void LoadDiContainerServiceProviderPackage()
    {
        var factory = new EmergencyShutdownProductionDiContainerServiceProviderPackageFactory(AppGlobals);
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

        //var section = root.GetSection("EmergencyShutdown");

        //if (AppGlobals is not IEmergencyShutdownAppGlobals globals)
        //{
        //    throw new ArgumentException("AppGlobals is not IEmergencyShutdownAppGlobals");
        //}

        //globals.UseSmtp = DefaultAppStartProvider.ReadBoolProperty(section, "UseSmtp", globals.UseSmtp);
    }
}