// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DataProtection.FileProtection;

namespace Bodoconsult.App.DataProtection;

/// <summary>
/// Implemenation of <see cref="IDataProtectionManager"/> doing nothing. NOT intended for production
/// </summary>
public class DoNothingDataProtectionManager : IDataProtectionManager
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public DoNothingDataProtectionManager()
    {
        ReadStringDelegate = ReadStringDelegateIntern;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Delegate to read a string input from console, UI, etc.
    /// </summary>
    /// <returns>Read string input</returns>
    public ReadStringDelegate ReadStringDelegate { get; set; }

    private string ReadStringDelegateIntern(string message)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Values to protect
    /// </summary>
    public IReadOnlyList<KeyValuePair<string, string?>> Values { get; } = [];

    /// <summary>
    /// Available keys
    /// </summary>
    public IReadOnlyList<string> Keys { get; } = [];

    /// <summary>
    /// Current instance of <see cref="IDataProtectionService"/> to use
    /// </summary>
    public IDataProtectionService DataProtectionService { get; } = new FakeDataProtectionService();

    /// <summary>
    /// Current instance of <see cref="IFileProtectionService"/> to use
    /// </summary>
    public IFileProtectionService FileProtectionService { get; } = new FakeFileProtectionService();

    /// <summary>
    /// Add a key required for the current app
    /// </summary>
    /// <param name="key">Key to be added</param>
    public void AddKey(string key)
    {
        // Do nothing
    }

    /// <summary>
    /// Protect a secret
    /// </summary>
    /// <param name="key">Unique key to use for the secret</param>
    /// <param name="secret">Secret to store</param>
    public string Protect(string key, string secret)
    {
        return secret;
    }

    /// <summary>
    /// Protect a secret
    /// </summary>
    /// <param name="key">Unique key to use for the secret</param>
    /// <param name="secret">Secret to store</param>
    /// <param name="doNotSave">Do NOT save the values to file</param>
    public string Protect(string key, string secret, bool doNotSave)
    {
        return secret;
    }

    /// <summary>
    /// Unprotect a secret by its key
    /// </summary>
    /// <param name="key">Key the secret was stored with</param>
    public string Unprotect(string key)
    {
        return key;
    }

    /// <summary>
    /// Save the values to storage
    /// </summary>
    public void SaveValues()
    {
        // Do nothing
    }

    /// <summary>
    /// Load the values from storage
    /// </summary>
    public void LoadValues()
    {
        // Do nothing
    }

    /// <summary>
    /// Load the values the first time from console, UI, etc.. Overrides existing secrets file.
    /// </summary>
    public void AskForInitialLoadValues()
    {
        // Do nothing
    }

    /// <summary>
    /// Protect a secret
    /// </summary>
    /// <param name="entity">Entity to protect</param>
    /// <param name="doNotSave">Do NOT save the values to file</param>
    public void Protect(object entity, bool doNotSave)
    {
        // Do nothing
    }

    /// <summary>
    /// Protect properties of an entity
    /// </summary>
    /// <param name="entity">Entity to protect</param>
    /// <remarks> At one property of the entity has to be marked with [DataProtectionSecretAttribute]</remarks>
    public void Protect(object entity)
    {
        // Do nothing
    }

    /// <summary>
    /// Unprotect properties of an entity
    /// </summary>
    /// <param name="entity">Entity to unprotect</param>
    /// <remarks> At one property of the entity has to be marked with [DataProtectionSecretAttribute]</remarks>
    public void Unprotect(object entity)
    {
        // Do nothing
    }
}