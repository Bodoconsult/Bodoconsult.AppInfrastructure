// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DependencyInjection;

namespace EfConsoleApp1.Model.DiContainerProvider;

/// <summary>
/// Load all database layer services to DI container. Intended mainly for testing DB layer not for production
/// </summary>
public class SqlServerExampleDbDiContainerServiceProviderPackage : BaseDiContainerServiceProviderPackage
{

    /// <summary>
    /// Default ctor
    /// </summary>
    public SqlServerExampleDbDiContainerServiceProviderPackage(IAppGlobals appGlobals): base(appGlobals)
    {
        var provider = new SqlServerExampleDbDiContainerProvider(((IAppGlobalsWithDatabase)AppGlobals).ContextConfig);
        ServiceProviders.Add(provider);
    }

}