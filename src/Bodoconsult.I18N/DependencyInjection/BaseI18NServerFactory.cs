// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.I18N.DependencyInjection;

/// <summary>
/// Base class for a II18NServerFactory instancing a singleton I18NServer instance. Access this instance with property I18NInstance when overriding CreateInstance() method
/// </summary>
public class BaseI18NServerFactory : II18NServerFactory
{

    /// <summary>
    ///  Current <see cref="II18N"/> instance
    /// </summary>
    protected II18NServer I18NServerInstance;

    /// <summary>
    /// Default ctor
    /// </summary>
    public BaseI18NServerFactory()
    {
        I18NServerInstance = new I18NServer();
    }

    /// <summary>
    /// Creating a configured II18N instance. Access this II18NServer instance with property I18NServerInstance when overriding CreateInstance() method
    /// </summary>
    /// <returns>An II18N instance</returns>
    public virtual II18NServer CreateInstance()
    {
        throw new NotSupportedException("Overload this method to configure your I18N instance");
    }
}