// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface to handle credentials safely
/// </summary>
public interface ICredentialManager
{
    /// <summary>
    /// Load credential by unique target name
    /// </summary>
    /// <param name="targetName"></param>
    /// <returns>Credential</returns>
    ICredentials? Load(string targetName);

    /// <summary>
    /// Save a credential
    /// </summary>
    /// <param name="credential">Credential to save</param>
    void Save(ICredentials credential);

    /// <summary>
    /// Delete credentials
    /// </summary>
    /// <param name="targetName">UUnique target name</param>
    /// <returns>true if the credential was deleted else false</returns>
    bool Delete(string targetName);

}