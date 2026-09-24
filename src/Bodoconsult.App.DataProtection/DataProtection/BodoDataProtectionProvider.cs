// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Bodoconsult.App.Helpers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace Bodoconsult.App.DataProtection;

/// <summary>
/// Contains factory methods for creating an <see cref="IDataProtectionProvider"/> where keys are stored
/// at a particular location on the file system.
/// </summary>
/// <remarks>Use these methods when not using dependency injection to provide the service to the application.</remarks>
public static class BodoDataProtectionProvider
{
    /// <summary>
    /// Creates an <see cref="DataProtectionProvider"/> given a location at which to store keys.
    /// </summary>
    /// <param name="keyDirectory">The <see cref="DirectoryInfo"/> in which keys should be stored. This may
    /// represent a directory on a local disk or a UNC share.</param>
    /// <param name="appName">An identifier that uniquely discriminates this application from all other applications on the machine.</param>
    public static IDataProtectionProvider Create(DirectoryInfo keyDirectory, string appName)
    {
        ArgumentNullThrowHelper.ThrowIfNull(keyDirectory);

        return CreateProvider(keyDirectory, setupAction: builder => { builder.SetApplicationName(appName); }, certificate: null);
    }

    /// <summary>
    /// Creates an <see cref="DataProtectionProvider"/> given a location at which to store keys
    /// and a <see cref="X509Certificate2"/> used to encrypt the keys.
    /// </summary>
    /// <param name="keyDirectory">The <see cref="DirectoryInfo"/> in which keys should be stored. This may
    /// represent a directory on a local disk or a UNC share.</param>
    /// <param name="applicationName">An identifier that uniquely discriminates this application from all other
    /// applications on the machine.</param>
    /// <param name="certificate">The <see cref="X509Certificate2"/> to be used for encryption.</param>
    public static IDataProtectionProvider Create(
        DirectoryInfo keyDirectory,
        string applicationName,
        X509Certificate2 certificate)
    {
        ArgumentNullThrowHelper.ThrowIfNull(keyDirectory);
        ArgumentNullThrowHelper.ThrowIfNull(certificate);

        return CreateProvider(keyDirectory, setupAction: builder => { builder.SetApplicationName(applicationName); }, certificate: certificate);
    }

    /// <summary>
    /// Creates an <see cref="DataProtectionProvider"/> given a location at which to store keys, an
    /// optional configuration callback and a <see cref="X509Certificate2"/> used to encrypt the keys.
    /// </summary>
    /// <param name="keyDirectory">The <see cref="DirectoryInfo"/> in which keys should be stored. This may
    /// represent a directory on a local disk or a UNC share.</param>
    /// <param name="setupAction">An optional callback which provides further configuration of the data protection
    /// system. See <see cref="IDataProtectionBuilder"/> for more information.</param>
    /// <param name="certificate">The <see cref="X509Certificate2"/> to be used for encryption.</param>
    public static IDataProtectionProvider Create(
        DirectoryInfo? keyDirectory,
        Action<IDataProtectionBuilder> setupAction,
        X509Certificate2 certificate)
    {
        ArgumentNullException.ThrowIfNull(keyDirectory);
        ArgumentNullException.ThrowIfNull(setupAction);
        ArgumentNullException.ThrowIfNull(certificate);

        return CreateProvider(keyDirectory, setupAction, certificate);
    }

    internal static IDataProtectionProvider CreateProvider(
        DirectoryInfo? keyDirectory,
        Action<IDataProtectionBuilder> setupAction,
        X509Certificate2? certificate)
    {
        // build the service collection
        var serviceCollection = new ServiceCollection();
        var builder = serviceCollection.AddDataProtection();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            builder.ProtectKeysWithDpapi();
        }

        if (keyDirectory != null)
        {
            builder.PersistKeysToFileSystem(keyDirectory);
        }

        if (certificate != null)
        {
            builder.ProtectKeysWithCertificate(certificate);
        }

        setupAction(builder);

        var service = serviceCollection.BuildServiceProvider();

        // extract the provider instance from the service collection
        return service.GetRequiredService<IDataProtectionProvider>();
    }
}