// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Interface for services to save strings as complete file. This class is intended to save in-memory-configurations as JSON or XML to file after changes.
/// Writing to file happens in a single threaded manner.
/// </summary>
public interface IStringToFileService
{
    /// <summary>
    /// Full filepath for saving the content in
    /// </summary>
    string FilePath { get; set; }

    /// <summary>
    /// Encoding to use for string to byte array conversions. Default: UTF8
    /// </summary>
    Encoding Encoding { get; set; }

    /// <summary>
    /// Start the service
    /// </summary>
    void Start();

    /// <summary>
    ///  Stop the service
    /// </summary>
    void Stop();

    /// <summary>
    /// Writing content to file in asingle threaded manner
    /// </summary>
    /// <param name="item">Item to write as file content</param>
    void WriteToFile(string item);

    /// <summary>
    /// Getting the file content
    /// </summary>
    /// <returns>File content as string</returns>
    /// <exception cref="FileLoadException">Throws if the file to read is blocked for writing</exception>
    string GetFileContent();
}