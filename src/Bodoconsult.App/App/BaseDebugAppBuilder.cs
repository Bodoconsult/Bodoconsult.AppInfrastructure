// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App;

/// <summary>
/// Base class for <see cref="IAppBuilder"/> implementations to be used for setting up test projects
/// </summary>
public class BaseDebugAppBuilder : BaseAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Global app settings</param>
    public BaseDebugAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    {
    }

    ///// <summary>
    ///// Process the configuration from <see cref="IAppBuilder.ConfigFile"/>. Uses the <see cref="DefaultAppStartProvider"/>.
    ///// </summary>
    //public override void ProcessConfiguration()
    //{
    //    // Now prepare the app start
    //    AppStartProvider = new DefaultAppStartProvider
    //    {
    //        ConfigFile = ConfigFile
    //    };

    //    AppStartProvider.LoadConfigurationProvider();
    //    AppStartProvider.LoadAppStartParameter();

    //}

    /// <summary>
    /// Start the application
    /// </summary>
    public override void StartApplication()
    {
        // Do nothing
    }
}