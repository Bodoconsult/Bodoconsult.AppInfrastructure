// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;
using System.Text.Json;
using Bodoconsult.App.Abstractions.Delegates;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.DataProtection;

/// <summary>
/// Current implementation of <see cref="IDataProtectionManager"/>
/// </summary>
public class DataProtectionManager : IDataProtectionManager
{
    private readonly ProducerConsumerQueue<object> _saveValuesRequestQueue = new();
    private readonly List<KeyValuePair<string, string?>> _values = [];

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="dataProtectionService">Current data protection service</param>
    /// <param name="fileProtectionService">Current file protection service</param>
    /// <param name="filePath">File path to store the protected data in</param>
    public DataProtectionManager(IDataProtectionService dataProtectionService, IFileProtectionService fileProtectionService, string filePath)
    {
        DataProtectionService = dataProtectionService;
        FileProtectionService = fileProtectionService;
        FilePath = filePath;

        _saveValuesRequestQueue.ConsumerTaskDelegate = ConsumerTaskDelegate;
        _saveValuesRequestQueue.StartConsumer();
    }

    private void ConsumerTaskDelegate(object clientNotification)
    {
        SaveValues();
    }

    /// <summary>
    /// File path to save the values in. Public only for testing
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    /// Delegate to read a string input from console, UI, etc.
    /// </summary>
    /// <returns>Read string input</returns>
    public ReadStringDelegate ReadStringDelegate { get; set; } = _ => string.Empty;

    /// <summary>
    /// Current values to protect
    /// </summary>
    public List<KeyValuePair<string, string?>> Values => _values.ToList();

    /// <summary>
    /// Available keys
    /// </summary>
    public List<string> Keys => _values.Select(x=> x.Key).ToList();

    /// <summary>
    /// Current instance of <see cref="IDataProtectionService"/> to use
    /// </summary>
    public IDataProtectionService DataProtectionService { get; }

    /// <summary>
    /// Current file protection service implementation
    /// </summary>
    public IFileProtectionService FileProtectionService { get; }

    /// <summary>
    /// Save the values to storage
    /// </summary>
    public void SaveValues()
    {
        if (_values.Any(x => string.IsNullOrEmpty(x.Value)))
        {
            //throw new ArgumentException("Not all keys have a valid value set: saving values was denied!");
            return;
        }

        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }

        var json = JsonSerializer.Serialize(_values);

        var data = FileProtectionService.Protect(Encoding.UTF8.GetBytes(json)) ;

        File.WriteAllBytes(FilePath, data);
    }

    /// <summary>
    /// Load the values from storage
    /// </summary>
    public void LoadValues()
    {
        if (!File.Exists(FilePath))
        {
            return;
        }

        var data = File.ReadAllBytes(FilePath);

        var json = FileProtectionService.Unprotect(data);

        var result = JsonSerializer.Deserialize<List<KeyValuePair<string, string?>>>(json);

        ArgumentNullException.ThrowIfNull(result);

        ClearAll();
        _values.AddRange(result);
    }

    /// <summary>
    /// Load the values the first time from console, UI, etc.. Overrides existing secrets file.
    /// </summary>
    public void AskForInitialLoadValues()
    {
        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }

        foreach (var key in Keys)
        {
            var secret = ReadStringDelegate($"Please insert value for key '{key}': ");

            if (string.IsNullOrEmpty(secret))
            {
                return;
            }

            Protect(key, secret, true);
        }

        _saveValuesRequestQueue.Enqueue(new object());
    }

    /// <summary>
    /// Clear the loaded values
    /// </summary>
    public void ClearAll()
    {
        _values.Clear();
    }

    /// <summary>
    /// Add a key required for the current app
    /// </summary>
    /// <param name="key">Key to be added</param>
    public void AddKey(string key)
    {
        _values.Add(new KeyValuePair<string, string?>(key, null));
    }

    /// <summary>
    /// Protect a secret
    /// </summary>
    /// <param name="key">Unique key to use for the secret</param>
    /// <param name="secret">Secret to store</param>
    public void Protect(string key, string secret)
    {
        Protect(key, secret, false);
    }

    /// <summary>
    /// Protect a secret
    /// </summary>
    /// <param name="key">Unique key to use for the secret</param>
    /// <param name="secret">Secret to store</param>
    /// <param name="doNotSave">Do NOT save the values to file. Default: false</param>
    public void Protect(string key, string secret, bool doNotSave)
    {
        var kvp = _values.FirstOrDefault(x => x.Key.Equals(key));

        var cipher = DataProtectionService.Protect(key, secret);

        if (kvp is not { Key: null })
        {
            _values.RemoveAll(x => x.Key == kvp.Key);
        }

        kvp = new KeyValuePair<string, string?>(key, cipher);
        _values.Add(kvp);

        if (doNotSave)
        {
            return;
        }
        _saveValuesRequestQueue.Enqueue(new object());
    }

    /// <summary>
    /// Unprotect a secret by its key
    /// </summary>
    /// <param name="key">Key the secret was stored with</param>
    /// <returns>Secret or null if the key does not exist</returns>
    public string? Unprotect(string key)
    {
        var kvp = _values.FirstOrDefault(x => x.Key.Equals(key));

        if (kvp is { Key: null })
        {
            return null;
        }

        if (kvp.Value == null)
        {
            return null;
        }

        var secret = DataProtectionService.Unprotect(key, kvp.Value);
        return secret;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        _saveValuesRequestQueue.StopConsumer();
        _saveValuesRequestQueue.Dispose();
    }
}