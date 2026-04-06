// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BusinessTransactions;
using Bodoconsult.App.Interfaces;

namespace Bodoconsult.App.DependencyInjection;

/// <summary>
/// Loads all business transaction specific services to DI container. Intended mainly for production
/// </summary>
public class BusinessTransactionManagerDiContainerServiceProvider : IDiContainerServiceProvider
{
    /// <summary>
    /// Add DI container services to a DI container
    /// </summary>
    /// <param name="diContainer">Current DI container</param>
    public void AddServices(DiContainer diContainer)
    {
        diContainer.AddSingleton<IBusinessTransactionManager, BusinessTransactionManager>();
    }

    /// <summary>
    /// Late bind DI container references to avoid circular DI references
    /// </summary>
    /// <param name="diContainer"></param>
    public void LateBindObjects(DiContainer diContainer)
    {
        // Do nothing
    }
}