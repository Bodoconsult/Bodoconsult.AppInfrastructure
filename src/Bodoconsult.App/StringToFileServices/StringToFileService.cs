// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.StringToFileServices;

/// <summary>
/// Service to save strings as complete file. This class is intended to save in-memory-configurations as JSON or XML to file after changes.
/// Writing to file happens in a single threaded manner.
/// </summary>
public class StringToFileService : IStringToFileService
{
    private bool _isStarted;
    private readonly Lock _isStartedLock = new();

    private bool _isWriting;
    private readonly Lock _isWritingLock = new();

    private readonly ProducerConsumerQueue<string> _consumerQueue = new();

    /// <summary>
    /// Default ctor
    /// </summary>
    public StringToFileService()
    {
        _consumerQueue.ConsumerTaskDelegate = WriteToFileInternal;
    }

    /// <summary>
    /// Full filepath for saving the content in
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// Encoding to use for string to byte array conversions. Default: UTF8
    /// </summary>
    public Encoding Encoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// Start the service
    /// </summary>
    public void Start()
    {
        lock (_isStartedLock)
        {
            _isStarted = true; 
        }
       
        _consumerQueue.StartConsumer();
    }

    /// <summary>
    ///  Stop the service
    /// </summary>
    public void Stop()
    {
        lock (_isStartedLock)
        {
            _isStarted = false;
        }

        _consumerQueue.StopConsumer();
    }

    /// <summary>
    /// Writing content to file in asingle threaded manner
    /// </summary>
    /// <param name="item">Item to write as file content</param>
    public void WriteToFile(string item)
    {
        lock (_isWritingLock)
        {
            _isWriting = true;
        }

        lock (_isStartedLock)
        {
            if (_isStarted)
            {
                _consumerQueue.Enqueue(item);
            }
        }

        lock (_isWritingLock)
        {
            _isWriting = false;
        }
    }

    /// <summary>
    /// Getting the file content
    /// </summary>
    /// <returns>File content as string</returns>
    /// <exception cref="FileLoadException">Throws if the file to read is blocked for writing</exception>
    public string GetFileContent()
    {
        lock (_isWritingLock)
        {
            if (_isWriting)
            {
                throw new FileLoadException("File is not ready for reading");
            }

            return File.ReadAllText(FilePath, Encoding);
        }
    }

    /// <summary>
    /// Internal method for single threaded writing to file
    /// </summary>
    /// <param name="item">Item to write as file content</param>
    private void WriteToFileInternal(string item)
    {
        File.WriteAllText(FilePath, item, Encoding);
    }
}