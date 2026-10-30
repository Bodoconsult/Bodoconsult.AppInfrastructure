// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App;
using Bodoconsult.App.Abstractions.Interfaces;
using System;
using OpenSourceConnectorBodoCms.Business.Interfaces;

namespace OpenSourceConnectorBodoCms.Test.App;

public class MyDebugAppBuilder : BaseDebugAppBuilder
{
    public MyDebugAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    {
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

        var section = root.GetSection("OpenSourceConnectorBodoCms");

        if (AppGlobals is not IOpenSourceConnectorBodoCmsAppGlobals globals)
        {
            throw new ArgumentException("AppGlobals is not IBodoFileTransferAppGlobals");
        }

        globals.TargetPath = DefaultAppStartProvider.ReadStringProperty(section, "TargetPath", globals.TargetPath) ?? string.Empty;
        globals.SourcePath = DefaultAppStartProvider.ReadStringProperty(section, "SourcePath", globals.SourcePath) ?? string.Empty;
        globals.FolderFilter = DefaultAppStartProvider.ReadStringProperty(section, "FolderFilter", globals.FolderFilter) ?? string.Empty;
        globals.FolderExcludeFilter = DefaultAppStartProvider.ReadStringProperty(section, "FolderExcludeFilter", globals.FolderExcludeFilter) ?? string.Empty;
    }
}