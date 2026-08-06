// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.DataProtection;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.DataProtection;

/// <summary>
/// Entity protection service protecting properties of an entity marked with [DataProtectionSecretAtrribute].
/// At one property of the entity has to be marked with [DataProtectionSecretAttribute]. 
/// Unprotecting works only on the same machine theprotecting was done
/// </summary>
public class EntityProtectionService : IEntityProtectionService
{
    private readonly IDataProtectionService _dataProtectionService;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataProtectionService">Current data protection service</param>
    public EntityProtectionService(IDataProtectionService dataProtectionService)
    {
        _dataProtectionService = dataProtectionService;
    }

    /// <summary>
    /// Protect properties of an entity
    /// </summary>
    /// <param name="entity">Entity to protect</param>
    public void Protect(object entity)
    {
        var type = entity.GetType();

        var props = type.GetProperties();

        string? key = null;

        foreach (var prop in props)
        {
            var attr = prop.GetCustomAttributes(typeof(DataProtectionKeyAttribute), false);
            if (attr.Length == 0)
            {
                continue;
            }

            key = prop.GetValue(entity)?.ToString();

            break;
        }

        if (key == null)
        {
            throw new ArgumentException("Key value of entity may not be null or empty");
        }

        foreach (var prop in props)
        {
            var attr = prop.GetCustomAttributes(typeof(DataProtectionSecretAttribute), false);
            if (attr.Length==0)
            {
                continue;   
            }

            var value = prop.GetValue(entity);
            if (value == null)
            {
                continue;
            }

            value = _dataProtectionService.Protect($"{key}.{prop.Name}", value.ToString() ?? string.Empty);

            prop.SetValue(entity, value);
        }
    }

    /// <summary>
    /// Unprotect properties of an entity
    /// </summary>
    /// <param name="entity">Entity to unprotect</param>
    public void Unprotect(object entity)
    {
        var type = entity.GetType();

        var props = type.GetProperties();

        string? key = null;

        foreach (var prop in props)
        {
            var attr = prop.GetCustomAttributes(typeof(DataProtectionKeyAttribute), false);
            if (attr.Length == 0)
            {
                continue;
            }

            key = prop.GetValue(entity)?.ToString();

            break;
        }

        if (key == null)
        {
            throw new ArgumentException("Key value of entity may not be null or empty");
        }

        foreach (var prop in props)
        {
            var attr = prop.GetCustomAttributes(typeof(DataProtectionSecretAttribute), false);
            if (attr.Length == 0)
            {
                continue;
            }

            var value = prop.GetValue(entity);
            if (value == null)
            {
                continue;
            }

            value = _dataProtectionService.Unprotect($"{key}.{prop.Name}", value.ToString() ?? string.Empty);

            prop.SetValue(entity, value);
        }
    }
}