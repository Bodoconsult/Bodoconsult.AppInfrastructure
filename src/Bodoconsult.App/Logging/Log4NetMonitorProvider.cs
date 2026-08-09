// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Concurrent;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace Bodoconsult.App.Logging;

/// <summary>
/// Implementation of <see cref="ILoggerProvider"/> for Log4Net
/// </summary>
public class Log4NetMonitorProvider : ILoggerProvider
{
    private readonly string _log4NetConfigFile;

    private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new();
    private readonly string _monitorLogFilename;
    private readonly string _plainMonitorLogFilename;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="monitorLogFilename">Current monitor log filename</param>
    public Log4NetMonitorProvider(string monitorLogFilename)
    {
        var s = Environment.ProcessPath;
        ArgumentNullException.ThrowIfNull(s);

        var dir = new FileInfo(s).DirectoryName;
        ArgumentNullException.ThrowIfNull(dir);

        // ReSharper disable once AssignNullToNotNullAttribute
        s = Path.Combine(dir, "log4net.config");
        _log4NetConfigFile = s;

        _monitorLogFilename = monitorLogFilename;

        var fi = new FileInfo(_monitorLogFilename);
        _plainMonitorLogFilename = fi.Name.Replace(fi.Extension, string.Empty);
    }

    /// <summary>
    /// Ctor with a Log4Net file path to load
    /// </summary>
    /// <param name="monitorLogFilename">Current monitor log filename</param>
    /// <param name="log4NetConfigFile">Log4Net file path to</param>
    public Log4NetMonitorProvider(string monitorLogFilename, string log4NetConfigFile)
    {
        _log4NetConfigFile = log4NetConfigFile;
        _monitorLogFilename = monitorLogFilename;

        var fi = new FileInfo(_monitorLogFilename);
        _plainMonitorLogFilename = fi.Name.Replace(fi.Extension, string.Empty);
    }

    /// <summary>
    /// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
    /// </summary>
    /// <param name="categoryName">Category name</param>
    /// <returns>The instance of <see cref="T:Microsoft.Extensions.Logging.ILogger" /> that was created.</returns>
    public ILogger CreateLogger(string categoryName)
    {
        var impl = CreateLoggerImplementation(categoryName);

        impl.IsEnabled(LogLevel.Trace);

        return _loggers.GetOrAdd(categoryName, impl);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        //#pragma warning disable 
        try
        {
            _loggers.Clear();

        }
#pragma warning disable CA1031
        catch //(Exception e)
        {
            // ignored
        }
#pragma warning restore CA1031
    }

    private Log4NetLogger CreateLoggerImplementation(string name)
    {
        // Load the default config file
        var xml = Parselog4NetConfigFile(_log4NetConfigFile);

        ArgumentNullException.ThrowIfNull(xml);

        // Now create the logger
        var l = new Log4NetLogger(name, xml, _plainMonitorLogFilename);
        return l;
    }

    private XmlElement? Parselog4NetConfigFile(string filename)
    {
        var xml = new XmlDocument();
        xml.Load(filename);

        //XmlNode node = doc.SelectSingleNode("/MyXmlType/" + element);
        //if (node != null)
        //{
        //    node.InnerText = value;
        //}
        //else
        //{
        //    XmlNode root = doc.DocumentElement;
        //    XmlElement elem;
        //    elem = doc.CreateElement(element);
        //    elem.InnerText = value;
        //    root.AppendChild(elem);
        //}

        var log4NetNode = xml.LastChild;

        if (log4NetNode is null)
        {
            return null;
        }

        // Replace the tag content for filename
        foreach (XmlNode node in log4NetNode.ChildNodes)
        {
            if (node.Name == "root")
            {
                continue;
            }

            if (!node.HasChildNodes)
            {
                continue;
            }

            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.Name == "file")
                {
                    if (node2.Attributes?.Count > 0)
                    {
                        var attr = node2.Attributes["value"];
                        if (attr != null)
                        {
                            attr.Value = _monitorLogFilename;
                        }
                    }
                }
            }
        }

        //using (var s = File.OpenRead(filename))
        //{
        //    log4NetConfig.Load(s);
        //}

        return xml["log4net"];
    }
}