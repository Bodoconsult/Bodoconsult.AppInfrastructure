// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Bodoconsult.App.Abstractions.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Splat;

namespace Bodoconsult.App.ReactiveUI.DependecyResolvers;

// Source: https://github.com/reactiveui/splat/blob/main/src/

/// <summary>
/// Microsoft DI implementation for <see cref="IDependencyResolver"/>.
/// </summary>
/// <seealso cref="IDependencyResolver" />
public class MicrosoftDependencyResolver : IDependencyResolver, IAsyncDisposable
{
    private const string ImmutableExceptionMessage = "This container has already been built and cannot be modified.";
    private readonly object _syncLock = new();
    private IServiceCollection? _serviceCollection;
    private bool _isImmutable;
    private IServiceProvider? _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="MicrosoftDependencyResolver" /> class with an <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/>.</param>
    public MicrosoftDependencyResolver(IServiceCollection? services = null) => _serviceCollection = services ?? new ServiceCollection();

    /// <summary>
    /// Initializes a new instance of the <see cref="MicrosoftDependencyResolver" /> class with a configured service Provider.
    /// </summary>
    /// <param name="serviceProvider">A ready to use service provider.</param>
    public MicrosoftDependencyResolver(IServiceProvider? serviceProvider) =>
        UpdateContainer(serviceProvider);

    /// <summary>
    /// Gets the internal Microsoft container,
    /// or builds a new one if this instance was not initialized with one.
    /// </summary>
    public virtual IServiceProvider? ServiceProvider
    {
        get
        {
            lock (_syncLock)
            {
                _serviceProvider ??= _serviceCollection?.BuildServiceProvider();

                return _serviceProvider;
            }
        }
    }

    /// <summary>
    /// Updates this instance with a collection of configured services.
    /// </summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/>.</param>
    public void UpdateContainer(IServiceCollection? services)
    {
        ArgumentExceptionHelper.ThrowIfNull(services);

        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        lock (_syncLock)
        {
            if (_serviceProvider is not null)
            {
                DisposeServiceProvider(_serviceProvider);
                _serviceProvider = null;
            }

            _serviceCollection = services;
        }
    }

    /// <summary>
    /// Updates this instance with a configured service Provider.
    /// </summary>
    /// <param name="serviceProvider">A ready to use service provider.</param>
    public void UpdateContainer(IServiceProvider? serviceProvider)
    {
        ArgumentExceptionHelper.ThrowIfNull(serviceProvider);

        lock (_syncLock)
        {
            // can be null if constructor using IServiceCollection was used.
            // and no fetch of a service was called.
            if (_serviceProvider is not null)
            {
                DisposeServiceProvider(_serviceProvider);
            }

            _serviceProvider = serviceProvider;
            _serviceCollection = null;
            _isImmutable = true;
        }
    }

    /// <summary>
    /// Gets an instance of the given <paramref name="serviceType" />. Must return an empty
    /// collection if the service is not available (must not return <c>null</c> or throw).
    /// </summary>
    /// <param name="serviceType">The object type.</param>
    /// <returns>An instance of the requested <paramref name="serviceType" /> or null</returns>
    public virtual object? GetService(Type? serviceType) =>
        GetServices(serviceType).LastOrDefault();

    /// <summary>
    /// Gets an instance of the given <paramref name="serviceType" />. Must return an empty
    /// collection if the service is not available (must not return <c>null</c> or throw).
    /// </summary>
    /// <param name="serviceType">The object type.</param>
    /// <param name="contract">An optional value which will retrieve only objects registered with the same contract.</param>
    /// <returns>An instance of the requested <paramref name="serviceType" /> or null</returns>
    public virtual object? GetService(Type? serviceType, string? contract) =>
        GetServices(serviceType, contract).LastOrDefault();

    /// <summary>
    /// Gets all instances of the given <paramref name="serviceType" />. Must return an empty
    /// collection if the service is not available (must not return <c>null</c> or throw).
    /// </summary>
    /// <param name="serviceType">The object type.</param>
    /// <returns>A sequence of instances of the requested <paramref name="serviceType" />. The sequence
    /// should be empty (not <c>null</c>) if no objects of the given type are available.</returns>
    public virtual IEnumerable<object> GetServices(Type? serviceType)
    {
        if (ServiceProvider is null)
        {
            throw new InvalidOperationException("The ServiceProvider is null.");
        }

        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        // this is to deal with CS8613 that GetServices returns IEnumerable<object?>?
        var services = ServiceProvider.GetServices(serviceType)
            .Where(a => a is not null)
            .Select(a => a!);

        if (isNull)
        {
            services = services
                .Cast<NullServiceType>()
                .Select(nst => nst.Factory()!);
        }

        return services;
    }


    /// <summary>
    /// Gets all instances of the given <paramref name="serviceType" />. Must return an empty
    /// collection if the service is not available (must not return <c>null</c> or throw).
    /// </summary>
    /// <param name="serviceType">The object type.</param>
    /// <param name="contract">An optional value which will retrieve only objects registered with the same contract.</param>
    /// <returns>A sequence of instances of the requested <paramref name="serviceType" />. The sequence
    /// should be empty (not <c>null</c>) if no objects of the given type are available.</returns>
    public virtual IEnumerable<object> GetServices(Type? serviceType, string? contract)
    {
        if (ServiceProvider is null)
        {
            throw new InvalidOperationException("The ServiceProvider is null.");
        }

        var isNull = serviceType is null;
        serviceType ??= NullServiceType.CachedType;

        IEnumerable<object> services = [];

        if (ServiceProvider is IKeyedServiceProvider serviceProvider)
        {
            services = serviceProvider.GetKeyedServices(serviceType, contract)
                .Where(a => a is not null)
                .Select(a => a!);
        }

        if (isNull)
        {
            services = services
                .Cast<NullServiceType>()
                .Select(nst => nst.Factory()!);
        }

        return services;
    }

    /// <summary>
    /// Register a function with the resolver which will generate an object
    /// for the specified service type.
    /// Optionally a contract can be registered which will indicate
    /// that registration will only work with that contract.
    /// Most implementations will use a stack based approach to allow for multiple items to be registered.
    /// </summary>
    /// <param name="factory">The factory function which generates our object.</param>
    /// <param name="serviceType">The type which is used for the registration.</param>
    public virtual void Register(Func<object?> factory, Type? serviceType)
    {
        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        var isNull = serviceType is null;

        serviceType ??= NullServiceType.CachedType;

        lock (_syncLock)
        {
            _serviceCollection?.AddTransient(serviceType, _ =>
                isNull
                    ? new NullServiceType(factory)
                    : factory()!);

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <summary>
    /// Register a function with the resolver which will generate an object
    /// for the specified service type.
    /// Optionally a contract can be registered which will indicate
    /// that registration will only work with that contract.
    /// Most implementations will use a stack based approach to allow for multiple items to be registered.
    /// </summary>
    /// <param name="factory">The factory function which generates our object.</param>
    /// <param name="serviceType">The type which is used for the registration.</param>
    /// <param name="contract">An optional contract value which will indicates to only generate the value if this contract is specified.</param>
    public virtual void Register(Func<object?> factory, Type? serviceType, string? contract)
    {
        if (contract is null)
        {
            Register(factory, serviceType);
            return;
        }

        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        var isNull = serviceType is null;

        serviceType ??= NullServiceType.CachedType;

        lock (_syncLock)
        {
            _serviceCollection?.AddKeyedTransient(serviceType, contract, (_, _) =>
                isNull
                    ? new NullServiceType(factory)
                    : factory()!);

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <summary>
    /// Unregister a service type
    /// </summary>
    /// <param name="serviceType">Type of service to unregister</param>
    /// <exception cref="InvalidOperationException">Thrown if service collection is immutable</exception>
    public virtual void UnregisterCurrent(Type? serviceType)
    {
        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        serviceType ??= NullServiceType.CachedType;

        lock (_syncLock)
        {
            var sd = _serviceCollection?.LastOrDefault(s => !s.IsKeyedService && s.ServiceType == serviceType);
            if (sd is not null)
            {
                _ = _serviceCollection?.Remove(sd);
            }

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <summary>
    /// Unregisters the current item based on the specified type and contract.
    /// </summary>
    /// <param name="serviceType">The service type to unregister.</param>
    /// <param name="contract">The optional contract value, which will only remove the value associated with the contract.</param>
    public virtual void UnregisterCurrent(Type? serviceType, string? contract)
    {
        if (contract is null)
        {
            UnregisterCurrent(serviceType);
            return;
        }

        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        serviceType ??= NullServiceType.CachedType;

        lock (_syncLock)
        {
            var sd = _serviceCollection?.LastOrDefault(sd => MatchesKeyedContract(serviceType, contract, sd));
            if (sd is not null)
            {
                _ = _serviceCollection?.Remove(sd);
            }

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <summary>
    /// Unregisters all the values associated with the specified type and contract - or -
    /// If the container has already been built, removes the specified contract (scope) entirely,
    /// ignoring the <paramref name="serviceType"/> argument.
    /// </summary>
    /// <param name="serviceType">The service type to unregister.</param>
    public virtual void UnregisterAll(Type? serviceType)
    {
        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        serviceType ??= NullServiceType.CachedType;

        lock (_syncLock)
        {
            if (_serviceCollection is null)
            {
                // required so that it gets rebuilt if not injected externally.
                DisposeServiceProvider(_serviceProvider);
                _serviceProvider = null;
                return;
            }

            var sds = _serviceCollection.Where(s => !s.IsKeyedService && s.ServiceType == serviceType);

            foreach (var sd in sds.ToList())
            {
                _ = _serviceCollection.Remove(sd);
            }

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <summary>
    /// Unregisters all the values associated with the specified type and contract - or -
    /// If the container has already been built, removes the specified contract (scope) entirely,
    /// ignoring the <paramref name="serviceType"/> argument.
    /// </summary>
    /// <param name="serviceType">The service type to unregister.</param>
    /// <param name="contract">A value which will remove only objects registered with the same contract.</param>
    public virtual void UnregisterAll(Type? serviceType, string? contract)
    {
        if (contract is null)
        {
            UnregisterAll(serviceType);
            return;
        }

        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        serviceType ??= NullServiceType.CachedType;

        lock (_syncLock)
        {
            if (_serviceCollection is null)
            {
                // required so that it gets rebuilt if not injected externally.
                DisposeServiceProvider(_serviceProvider);
                _serviceProvider = null;
                return;
            }

            var sds = _serviceCollection.Where(sd => MatchesKeyedContract(serviceType, contract, sd));

            foreach (var sd in sds.ToList())
            {
                _ = _serviceCollection.Remove(sd);
            }

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <summary>
    /// <para>
    /// Register a callback to be called when a new service matching the type
    /// and contract is registered.
    /// </para>
    /// <para>
    /// When registered, the callback is also called for each currently matching
    /// service.
    /// </para>
    /// </summary>
    /// <returns>When disposed removes the callback.</returns>
    /// <param name="serviceType">The type which is used for the registration.</param>
    /// <param name="callback">The callback which will be called when the specified service type and contract are registered.</param>
    public virtual IDisposable ServiceRegistrationCallback(Type serviceType, Action<IDisposable> callback) =>
        throw new NotImplementedException("ServiceRegistrationCallback without contract is not implemented in the Microsoft dependency resolver.");


    /// <summary>
    /// <para>
    /// Register a callback to be called when a new service matching the type
    /// and contract is registered.
    /// </para>
    /// <para>
    /// When registered, the callback is also called for each currently matching
    /// service.
    /// </para>
    /// </summary>
    /// <returns>When disposed removes the callback.</returns>
    /// <param name="serviceType">The type which is used for the registration.</param>
    /// <param name="contract">An optional contract value which will indicates to only generate the value if this contract is specified.</param>
    /// <param name="callback">The callback which will be called when the specified service type and contract are registered.</param>
    public virtual IDisposable ServiceRegistrationCallback(Type serviceType, string? contract, Action<IDisposable> callback) =>
        throw new NotImplementedException("ServiceRegistrationCallback is not implemented in the Microsoft dependency resolver.");

    /// <summary>
    /// Check to see if a resolver has a registration for a type.
    /// </summary>
    /// <param name="serviceType">The type to check for registration.</param>
    /// <returns>Whether there is a registration for the type.</returns>
    public virtual bool HasRegistration(Type? serviceType)
    {
        serviceType ??= NullServiceType.CachedType;

        if (!_isImmutable)
        {
            return _serviceCollection?.Any(sd => !sd.IsKeyedService && sd.ServiceType == serviceType) == true;
        }

        var service = _serviceProvider?.GetService(serviceType);
        return service is not null;
    }

    /// <summary>
    /// Check to see if a resolver has a registration for a type.
    /// </summary>
    /// <param name="serviceType">The type to check for registration.</param>
    /// <returns>Whether there is a registration for the type.</returns>
    /// <param name="contract">An optional contract value which will indicates to only generate the value if this contract is specified.</param>
    public virtual bool HasRegistration(Type? serviceType, string? contract)
    {
        // Contract semantics: a null contract means "use the non-keyed registration path".
        // This matches Register(..., Type?, string? contract) which delegates to Register(..., Type?)
        // when contract is null, and matches expected IDependencyResolver behavior in Splat tests.
        if (contract is null)
        {
            return HasRegistration(serviceType);
        }

        serviceType ??= NullServiceType.CachedType;

        if (!_isImmutable)
        {
            // Only keyed services match when a non-null contract is specified.
            return _serviceCollection?.Any(sd => MatchesKeyedContract(serviceType, contract, sd)) == true;
        }

        // Immutable provider path: only keyed services match when a non-null contract is specified.
        return _serviceProvider is IKeyedServiceProvider keyedServiceProvider
               && keyedServiceProvider.GetKeyedService(serviceType, contract) is not null;
    }

    /// <summary>
    /// Gets an instance of the given <typeparamref name="T" />. Must return an empty
    /// collection if the service is not available (must not return <c>null</c> or throw).
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <returns>An instance of the requested <typeparamref name="T" /> or null</returns>
    public T? GetService<T>() => (T?)GetService(typeof(T));

    /// <summary>
    /// Gets an instance of the given <typeparamref name="T" />. Must return an empty
    /// collection if the service is not available (must not return <c>null</c> or throw).
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="contract">An optional contract value which will indicates to only generate the value if this contract is specified.</param>
    /// <returns>An instance of the requested <typeparamref name="T" /> or null</returns>
    public T? GetService<T>(string contract) =>
        (T?)GetService(typeof(T), contract);

    /// <summary>
    /// Gets all instances of the given <typeparamref name="T" />. Must return an empty
    /// collection if the service is not available (must not return <c>null</c> or throw).
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <returns>A sequence of instances of the requested <typeparamref name="T" />. The sequence
    /// should be empty (not <c>null</c>) if no objects of the given type are available.</returns>
    public IEnumerable<T> GetServices<T>() => GetServices(typeof(T)).Cast<T>();

    /// <summary>
    /// Gets all instances of the given <typeparamref name="T" />. Must return an empty
    /// collection if the service is not available (must not return <c>null</c> or throw).
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="contract">An optional value which will retrieve only objects registered with the same contract.</param>
    /// <returns>A sequence of instances of the requested <typeparamref name="T" />. The sequence
    /// should be empty (not <c>null</c>) if no objects of the given type are available.</returns>
    public IEnumerable<T> GetServices<T>(string contract) =>
        GetServices(typeof(T), contract).Cast<T>();

    /// <summary>
    /// Check to see if a resolver has a registration for a type.
    /// </summary>
    /// <typeparam name="T">The type to check for registration.</typeparam>
    /// <returns>Whether there is a registration for the type.</returns>
    public bool HasRegistration<T>() => HasRegistration(typeof(T));

    /// <summary>
    /// Check to see if a resolver has a registration for a type.
    /// </summary>
    /// <typeparam name="T">The type to check for registration.</typeparam>
    /// <returns>Whether there is a registration for the type.</returns>
    /// <param name="contract">An optional contract value which will indicates to only generate the value if this contract is specified.</param>
    public bool HasRegistration<T>(string contract) =>
        HasRegistration(typeof(T), contract);

    /// <summary>
    /// Register a function with the resolver which will generate an object
    /// for the specified service type.
    /// Optionally a contract can be registered which will indicate
    /// that registration will only work with that contract.
    /// Most implementations will use a stack based approach to allow for multiple items to be registered.
    /// </summary>
    /// <param name="factory">The factory function which generates our object.</param>
    /// <typeparam name="T">The type which is used for the registration.</typeparam>
    public void Register<T>(Func<T?> factory) =>
        Register(() => factory(), typeof(T));

    /// <summary>
    /// Register a function with the resolver which will generate an object
    /// for the specified service type.
    /// Optionally a contract can be registered which will indicate
    /// that registration will only work with that contract.
    /// Most implementations will use a stack based approach to allow for multiple items to be registered.
    /// </summary>
    /// <param name="factory">The factory function which generates our object.</param>
    /// <typeparam name="T">The type which is used for the registration.</typeparam>
    /// <param name="contract">An optional contract value which will indicates to only generate the value if this contract is specified.</param>
    public void Register<T>(Func<T?> factory, string contract) =>
        Register(() => factory(), typeof(T), contract);

    /// <summary>
    /// Unregisters the current item based on the specified type and contract.
    /// </summary>
    /// <typeparam name="T">The service type to unregister.</typeparam>
    public void UnregisterCurrent<T>() => UnregisterCurrent(typeof(T));

    /// <summary>
    /// Unregisters the current item based on the specified type and contract.
    /// </summary>
    /// <typeparam name="T">The service type to unregister.</typeparam>
    /// <param name="contract">The optional contract value, which will only remove the value associated with the contract.</param>
    public void UnregisterCurrent<T>(string contract) =>
        UnregisterCurrent(typeof(T), contract);

    /// <summary>
    /// Unregisters all the values associated with the specified type and contract.
    /// </summary>
    /// <typeparam name="T">The service type to unregister.</typeparam>
    public void UnregisterAll<T>() => UnregisterAll(typeof(T));

    /// <summary>
    /// Unregisters all the values associated with the specified type and contract.
    /// </summary>
    /// <typeparam name="T">The service type to unregister.</typeparam>
    /// <param name="contract">The optional contract value, which will only remove the value associated with the contract.</param>
    public void UnregisterAll<T>(string contract) =>
        UnregisterAll(typeof(T), contract);


    //public IDisposable ServiceRegistrationCallback<T>(Action<IDisposable> callback) =>
    //    ServiceRegistrationCallback(typeof(T), callback);

    //public IDisposable ServiceRegistrationCallback<T>(string contract, Action<IDisposable> callback) =>
    //    ServiceRegistrationCallback(typeof(T), contract, callback);


    /// <summary>
    /// Register a function with the resolver which will generate an object
    /// for the specified service type.
    /// Optionally a contract can be registered which will indicate
    /// that registration will only work with that contract.
    /// Most implementations will use a stack based approach to allow for multiple items to be registered.
    /// </summary>
    public void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, new()
    {
        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        lock (_syncLock)
        {
            _serviceCollection?.AddTransient<TService, TImplementation>();

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <summary>
    /// Register a function with the resolver which will generate an object
    /// for the specified service type.
    /// Optionally a contract can be registered which will indicate
    /// that registration will only work with that contract.
    /// Most implementations will use a stack based approach to allow for multiple items to be registered.
    /// </summary>
    /// <param name="contract">The optional contract value, which will only remove the value associated with the contract.</param>
    public void Register<TService, TImplementation>(string? contract)
        where TService : class
        where TImplementation : class, TService, new()
    {
        if (contract is null)
        {
            Register<TService, TImplementation>();
            return;
        }

        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        lock (_syncLock)
        {
            _serviceCollection?.AddKeyedTransient<TService, TImplementation>(contract);

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <summary>
    /// Register a constant with the resolver which will generate an object
    /// for the specified service type.
    /// Optionally a contract can be registered which will indicate
    /// that registration will only work with that contract.
    /// Most implementations will use a stack based approach to allow for multiple items to be registered.
    /// </summary>
    public void RegisterConstant<T>(T value)
        where T : class
    {
        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        lock (_syncLock)
        {
            _serviceCollection?.AddSingleton(value);

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <summary>
    /// Register a constant with the resolver which will generate an object
    /// for the specified service type.
    /// Optionally a contract can be registered which will indicate
    /// that registration will only work with that contract.
    /// Most implementations will use a stack based approach to allow for multiple items to be registered.
    /// </summary>
    /// <param name="value">Constant value</param>
    /// <param name="contract">The optional contract value, which will only remove the value associated with the contract.</param>
    public void RegisterConstant<T>(T value, string? contract)
        where T : class
    {
        if (contract is null)
        {
            RegisterConstant(value);
            return;
        }

        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        lock (_syncLock)
        {
            _serviceCollection?.AddKeyedSingleton(contract, value);

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <summary>
    /// Register a service as lazy singleton
    /// </summary>
    /// <typeparam name="T">Type of service</typeparam>
    /// <param name="valueFactory">Value factory</param>
    /// <exception cref="InvalidOperationException">If service collection is immutable</exception>
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T> valueFactory)
        where T : class
    {
        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        var lazy = new Lazy<T>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);

        lock (_syncLock)
        {
            _serviceCollection?.AddSingleton<T>(_ => lazy.Value);

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <summary>
    /// Register a service as lazy singleton
    /// </summary>
    /// <typeparam name="T">Type of service</typeparam>
    /// <param name="valueFactory">Value factory</param>
    /// <param name="contract">The optional contract value, which will only remove the value associated with the contract.</param>
    /// <exception cref="InvalidOperationException">If service collection is immutable</exception>
    public void RegisterLazySingleton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(Func<T> valueFactory, string? contract)
        where T : class
    {
        if (contract is null)
        {
            RegisterLazySingleton(valueFactory);
            return;
        }

        if (_isImmutable)
        {
            throw new InvalidOperationException(ImmutableExceptionMessage);
        }

        var lazy = new Lazy<T>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);

        lock (_syncLock)
        {
            _serviceCollection?.AddKeyedSingleton<T>(contract, (_, _) => lazy.Value);

            // required so that it gets rebuilt if not injected externally.
            DisposeServiceProvider(_serviceProvider);
            _serviceProvider = null;
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or
    /// resetting unmanaged resources asynchronously.</summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        if (_serviceProvider is IAsyncDisposable d)
        {
            await d.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the instance.
    /// </summary>
    /// <param name="disposing">Whether or not the instance is disposing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            DisposeServiceProvider(_serviceProvider);
        }
    }

    private static void DisposeServiceProvider(IServiceProvider? sp)
    {
        if (sp is IDisposable d)
        {
            d.Dispose();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool MatchesKeyedContract(Type? serviceType, string contract, ServiceDescriptor sd) =>
        sd.ServiceType == serviceType
        && sd is { IsKeyedService: true, ServiceKey: string serviceKey }
        && serviceKey == contract;
}