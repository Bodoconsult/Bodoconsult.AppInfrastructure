// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Delegates;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for implementing data protection manager
/// </summary>
public interface IDataProtectionManager: IDisposable
{
    /// <summary>
    /// Delegate to read a string input from console, UI, etc.
    /// </summary>
    /// <returns>Read string input</returns>
    ReadStringDelegate ReadStringDelegate { get; set; }

    /// <summary>
    /// Values to protect
    /// </summary>
    IReadOnlyList<KeyValuePair<string, string?>> Values { get; }

    /// <summary>
    /// Available keys
    /// </summary>
    IReadOnlyList<string> Keys { get; }

    /// <summary>
    /// Current instance of <see cref="IDataProtectionService"/> to use
    /// </summary>
    IDataProtectionService DataProtectionService { get; }

    /// <summary>
    /// Current instance of <see cref="IFileProtectionService"/> to use
    /// </summary>
    IFileProtectionService FileProtectionService { get; }

    /// <summary>
    /// Add a key required for the current app
    /// </summary>
    /// <param name="key">Key to be added</param>
    void AddKey(string key);

    /// <summary>
    /// Protect a secret
    /// </summary>
    /// <param name="key">Unique key to use for the secret</param>
    /// <param name="secret">Secret to store</param>
    void Protect(string key, string secret);

    /// <summary>
    /// Protect a secret
    /// </summary>
    /// <param name="key">Unique key to use for the secret</param>
    /// <param name="secret">Secret to store</param>
    /// <param name="doNotSave">Do NOT save the values to file</param>
    void Protect(string key, string secret, bool doNotSave);

    /// <summary>
    /// Unprotect a secret by its key
    /// </summary>
    /// <param name="key">Key the secret was stored with</param>
    string? Unprotect(string key);

    /// <summary>
    /// Save the values to storage
    /// </summary>
    void SaveValues();

    /// <summary>
    /// Load the values from storage
    /// </summary>
    void LoadValues();

    /// <summary>
    /// Load the values the first time from console, UI, etc.. Overrides existing secrets file.
    /// </summary>
    void AskForInitialLoadValues();
    
}