// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bodoconsult.App.Abstractions.DependencyInjection;

/// <summary>
/// Central class for managing DI container
/// </summary>
public class DiContainer
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public DiContainer()
    {
        ServiceCollection = new ServiceCollection();
    }

    /// <summary>
    /// Ctor with a injected <see cref="ServiceCollection"/> instance
    /// </summary>
    public DiContainer(IServiceCollection serviceCollection)
    {
        ServiceCollection = serviceCollection;
    }

    /// <summary>
    /// Current service collection
    /// </summary>
    public IServiceCollection ServiceCollection { get; }

    /// <summary>
    /// Current <see cref="IServiceProvider"/> instance or null if <see cref="BuildServiceProvider"/> was not called
    /// </summary>
    public IServiceProvider? ServiceProvider { get; private set; }

    /// <summary>
    /// Create the service provider. Must be called after <see cref="ServiceCollection"/> is fully configured.
    /// </summary>
    /// <returns><see cref="ServiceProvider"/> instance</returns>
    public IServiceProvider BuildServiceProvider()
    {
        ServiceProvider = ServiceCollection.BuildServiceProvider();
        return ServiceProvider;
    }

    /// <summary>
    /// Get an instance of a certain type
    /// </summary>
    /// <typeparam name="T">Requested type to resolve</typeparam>
    /// <returns>Object instance of the requested type</returns>
    public T Get<T>()
    {
        ArgumentNullException.ThrowIfNull(ServiceProvider);

        //if (ServiceCollection.Count < 2)
        //{
        //    return default;
        //}

        try
        {
            var t = ServiceProvider.GetService(typeof(T));
            return t is null ? throw new ArgumentNullException(): (T)t;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(typeof(T).Name, e);
        }
    }

    /// <summary>
    /// Add a service scoped. Scoped lifetime services (AddScoped) are created once per client request (connection).
    ///           When using a scoped service in a middleware, inject the service into the Invoke or InvokeAsync method.
    /// Don't inject via constructor injection because it forces the service to behave like a singleton. 
    /// </summary>
    /// <typeparam name="TInterface">Service interface</typeparam>
    /// <typeparam name="TInstanceType">Instance type implementing service interface</typeparam>
    public void AddScoped<TInterface, TInstanceType>() where TInterface : class where TInstanceType : class, TInterface
    {
        ServiceCollection.AddScoped<TInterface, TInstanceType>();
    }

    /// <summary>
    /// Add a service transient. Transient lifetime services are created each time they're requested from the service container. This lifetime works best for lightweight, stateless services.
    /// </summary>
    /// <typeparam name="TInterface">Service interface</typeparam>
    /// <typeparam name="TInstanceType">Instance type implementing service interface</typeparam>
    public void AddTransient<TInterface, TInstanceType>() where TInterface : class where TInstanceType : class, TInterface
    {
        ServiceCollection.AddTransient<TInterface, TInstanceType>();
    }

    /// <summary>
    /// Add a service as a singleton
    /// </summary>
    /// <typeparam name="TInterface">Service interface</typeparam>
    /// <typeparam name="TInstanceType">Instance type implementing service interface</typeparam>
    public void AddSingleton<TInterface, TInstanceType>() where TInterface : class where TInstanceType : class, TInterface
    {
        ServiceCollection.AddSingleton<TInterface, TInstanceType>();
    }

    /// <summary>
    /// Add a instance as singleton
    /// </summary>
    /// <typeparam name="TInterface">Service interface</typeparam>
    /// <param name="instance">Concrete object implementing service interface</param>
    public void AddSingletonInstance<TInterface>(TInterface instance) where TInterface : class
    {
        ServiceCollection.AddSingleton(instance);
    }

    /// <summary>
    /// Clear all (intended for testing purposes)
    /// </summary>
    public void ClearAll()
    {
        ServiceProvider = null;
        ServiceCollection.Clear();
    }

    /// <summary>
    /// Adds a singleton service of the type specified with an
    /// instance specified in <paramref name="instance"/> to the
    /// specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="instance">Instance to use as singleton</param>
    public void AddSingleton<TInterface>(TInterface instance) where TInterface : class
    {
        ServiceCollection.AddSingleton(instance);
    }

    /// <summary>
    /// Adds a singleton service of the type specified with an
    /// instance specified in <paramref name="instance"/> to the
    /// specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="instance">Instance to use as singleton</param>
    public void AddSingleton(Type instance)
    {
        ServiceCollection.AddSingleton(instance);
    }

    /// <summary>
    /// Load an existing <see cref="IServiceProvider"/> instance to work with
    /// </summary>
    /// <param name="hostServices">Current service provider</param>
    public void LoadServiceProvider(IServiceProvider hostServices)
    {
        ServiceProvider = hostServices;
    }

    /// <summary>
    /// Updates all currently loaded services implementing <see cref="IServiceRequiresAppSettingsUpdate"/>
    /// </summary>
    public List<string> UpdateServices()
    {
        ArgumentNullException.ThrowIfNull(ServiceProvider);

        var result = new List<string>();

        foreach (var x in ServiceCollection)
        {

            var type = x.ServiceType;

            if (!typeof(IServiceRequiresAppSettingsUpdate).IsAssignableFrom(type))
            {
                continue;
            }

            var o = ServiceProvider.GetService(type);
            if (o is null)
            {
                continue;
            }

            var instance = (IServiceRequiresAppSettingsUpdate)o;

            instance.UpdateService();

            result.Add(instance.GetType().Name);
        }

        return result;
    }

    /// <summary>
    /// Create a simple empty host application builder instance
    /// </summary>
    /// <returns>Host builder</returns>
    public static IHostBuilder CreateHost()
    {
        return Host.CreateDefaultBuilder();
    }
}