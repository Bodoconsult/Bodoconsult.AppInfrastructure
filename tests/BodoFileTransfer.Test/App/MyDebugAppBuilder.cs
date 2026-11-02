// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoFileTransfer.Business.Helpers;
using BodoFileTransfer.Business.Interfaces;
using System;

namespace BodoFileTransfer.Test.App;

public class MyDebugAppBuilder : BaseDebugAppBuilder
{
    public MyDebugAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    {
        AppHelper.LoadPasswordData();
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

        if (AppGlobals is not IBodoFileTransferAppGlobals globals)
        {
            throw new ArgumentException("AppGlobals is not IBodoFileTransferAppGlobals");
        }

        globals.EmailSentDateWeeksBack = DefaultAppStartProvider.ReadIntProperty(section, "EmailSentDateWeeksBack", globals.EmailSentDateWeeksBack);
        globals.NumberSchema = DefaultAppStartProvider.ReadStringProperty(section, "NumberSchema", globals.NumberSchema);
        globals.SignatureFileExtensions = DefaultAppStartProvider.ReadStringProperty(section, "SignatureFileExtensions", globals.SignatureFileExtensions);
    }
}