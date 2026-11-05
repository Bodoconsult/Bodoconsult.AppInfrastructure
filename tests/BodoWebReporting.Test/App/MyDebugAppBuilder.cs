// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoWebReporting.Dal.Interfaces;
using BodoWebReporting.DiContainerProvider;

namespace BodoWebReporting.Test.App;

public class MyDebugAppBuilder : BaseDebugAppBuilder
{
    public MyDebugAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    {
    }

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

        var section = root.GetSection("BodoWebReporting");

        if (AppGlobals is not IBodoWebReportingAppGlobals globals)
        {
            throw new ArgumentException("AppGlobals is not IBodoWebReportingAppGlobals");
        }

        globals.TargetPath = DefaultAppStartProvider.ReadStringProperty(section, "TargetPath", globals.TargetPath) ?? string.Empty;
        globals.PreRunSql = DefaultAppStartProvider.ReadStringProperty(section, "PreRunSql", globals.PreRunSql) ?? string.Empty;
        globals.PostRunSql = DefaultAppStartProvider.ReadStringProperty(section, "PostRunSql", globals.PostRunSql) ?? string.Empty;
    }
}