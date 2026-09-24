// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for entity protection service protecting properties of an entity marked with [DataProtectionSecretAtrribute].
/// At one property of the entity has to be marked with [DataProtectionSecretAttribute]. 
/// Unprotecting works only on the same machine the protecting was done
/// </summary>
public interface IEntityProtectionService
{
    /// <summary>
    /// Protect properties of an entity
    /// </summary>
    /// <param name="entity">Entity to protect</param>
    void Protect(object entity);

    /// <summary>
    /// Unprotect properties of an entity
    /// </summary>
    /// <param name="entity">Entity to unprotect</param>
    void Unprotect(object entity);
}