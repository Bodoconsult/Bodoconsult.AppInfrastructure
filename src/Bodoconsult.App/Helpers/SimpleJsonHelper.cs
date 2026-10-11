// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;
using System.Text.Json;

namespace Bodoconsult.App.Helpers;

/// <summary>
/// Helper class to handle simple JSON actions. Does not support deeper serialization of classes like Newtonsoft.Json
/// </summary>
public class SimpleJsonHelper
{
    /// <summary>
    /// Load an object from a JSON file
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static T? LoadJsonFile<T>(string fileName)
    {
        var json = File.ReadAllText(fileName);

        var job = JsonSerializer.Deserialize<T>(json);

        return job;
    }

    /// <summary>
    /// Load an object from a JSON string
    /// </summary>
    /// <param name="json">JSON string to deserialize</param>
    /// <returns>Deserialize entity</returns>
    public static T? LoadJson<T>(string json)
    {
        var job = JsonSerializer.Deserialize<T>(json);
        return job;
    }

    /// <summary>
    /// Serialize a object to JSON
    /// </summary>
    /// <param name="value">object to convert</param>
    /// <typeparam name="T">type of object to convert</typeparam>
    /// <returns></returns>
    public static string SerializeObject<T>(T value)
    {
        var json = JsonSerializer.Serialize(value);
        return json;
    }

    /// <summary>
    /// Save an object of type T as JSON to a file
    /// </summary>
    /// <param name="fileName">full path for the JSON file</param>
    /// <param name="data">object to serialize to JSON</param>
    /// <typeparam name="T">type of the object to serialize to JSON</typeparam>
    /// <returns>JSON string</returns>
    public static string SaveAsFile<T>(string fileName, T data)
    {
        var json = SerializeObject(data);
        File.WriteAllText(fileName, json, Encoding.UTF8);
        return json;
    }
}