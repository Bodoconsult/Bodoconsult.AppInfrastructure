// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Runtime.Versioning;
using Bodoconsult.App;
using Bodoconsult.App.Abstractions.Interfaces;
using BodoFtpTransfer.DiContainerProvider;

namespace BodoFtpTransfer;

[SupportedOSPlatform("windows10.0.17763.0")]
public class BodoFtpTransferAppBuilder : BaseAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Global app settings</param>
    public BodoFtpTransferAppBuilder(IAppGlobals appGlobals) : base(appGlobals)
    { }

    /// <summary>
    /// Load the <see cref="IAppBuilder.DiContainerServiceProviderPackage"/>
    /// </summary>
    public override void LoadDiContainerServiceProviderPackage()
    {
        var factory = new BodoFtpTransferProductionDiContainerServiceProviderPackageFactory(AppGlobals);
        DiContainerServiceProviderPackage = factory.CreateInstance();
    }
    
    ///// <summary>
    ///// Process the configuration from <see cref="IAppStartParameter.ConfigFile"/>
    ///// </summary>
    //public override void ProcessConfiguration()
    //{
    //    // Load basic config
    //    base.ProcessConfiguration();

    //    ArgumentNullException.ThrowIfNull(AppStartProvider?.AppConfigurationProvider);

    //    // Now get the root configuration element
    //    var root = AppStartProvider.AppConfigurationProvider.Configuration;

    //    if (root is null)
    //    {
    //        return;
    //    }

    //    var section = root.GetSection("CurrentMailAccount");

    //    // Get your derived IAppGlobals instance here to access added properties
    //    var ma = new SmtpMailAccount();

    //    // Now get the requested config elements out of the root config element
    //    ma.SmtpServer = DefaultAppStartProvider.ReadStringProperty(section, "SmtpServer", string.Empty);
    //    ma.SmtpAccountName = DefaultAppStartProvider.ReadStringProperty(section, "SmtpAccountName", string.Empty);
    //    ma.MailAddressSender = DefaultAppStartProvider.ReadStringProperty(section, "MailAddressSender", string.Empty);
    //    ma.SmtpPassword = DefaultAppStartProvider.ReadStringProperty(section, "SmtpPassword", string.Empty);
    //    ma.UseSecureConnection = DefaultAppStartProvider.ReadBoolProperty(section, "UseSecureConnection", true);

    //    if (AppGlobals is not IBodoWebMailerGlobals bodoWebMailerGlobals)
    //    {
    //        throw new ArgumentException("AppGlobals is not IBodoWebMailerGlobals");
    //    }

    //    bodoWebMailerGlobals.CurrentMailAccount = ma;
    //}
}