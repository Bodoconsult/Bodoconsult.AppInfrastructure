// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Globalization;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace EmergencyShutdown.Business.Helpers;

/// <summary>
/// Helper class to handle JSON in JobExecuter
/// </summary>
public class JsonHelper
{
    /// <summary>
    /// Load an object from a JSON file
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static T LoadJsonFile<T>(string fileName)
    {

        using var file = File.OpenText(fileName);
        var serializer = new JsonSerializer
        {
            TypeNameHandling = TypeNameHandling.All
        };

        var job = (T)serializer.Deserialize(file, typeof(T));

        return job;
    }

    /// <summary>
    /// Load an object from a JSON string
    /// </summary>
    /// <param name="json">JSON string to deserialize</param>
    /// <returns>Deserialize entity</returns>
    public static T LoadJson<T>(string json)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };
        var job = JsonConvert.DeserializeObject<T>(json, settings);

        return job;
    }

    /// <summary>
    /// Load a object from a JSON resource
    /// </summary>
    /// <param name="resourceName">Fully qualified resource name</param>
    /// <returns>object of type T or null</returns>
    public static T LoadJsonFromResource<T>(string resourceName)
    {
        T job;

        var ass = Assembly.GetExecutingAssembly();
        var str = ass.GetManifestResourceStream(resourceName);

        if (str == null) return default(T);

        using var file = new StreamReader(str);
        var serializer = new JsonSerializer
        {
            TypeNameHandling = TypeNameHandling.All
        };

        job = (T)serializer.Deserialize(file, typeof(T));

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
        var sb = new StringBuilder(256);
        var sw = new StringWriter(sb, CultureInfo.InvariantCulture);

        var jsonSerializer = new JsonSerializer
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Objects,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
        };

        jsonSerializer.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        using (var jsonWriter = new JsonTextWriter(sw))
        {
            jsonWriter.Formatting = Formatting.Indented;
            jsonWriter.IndentChar = '\t';
            jsonWriter.Indentation = 1;

            jsonSerializer.Serialize(jsonWriter, value, typeof(T));
        }

        return sw.ToString();
    }


    /// <summary>
    /// Serialize an object of type T as JSON and save it to a file
    /// </summary>
    /// <param name="fileName">full path for the JSON file</param>
    /// <param name="data">object to serialize to JSON</param>
    /// <typeparam name="T">type of the object to serialize to JSON</typeparam>
    /// <returns>JSON string</returns>
    public static string SerializeAsFile<T>(string fileName, T data)
    {
        var json = SerializeObject(data);

        File.WriteAllText(fileName, json, Encoding.UTF8);

        return json;
    }
}