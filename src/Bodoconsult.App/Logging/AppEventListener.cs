// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Text;
using System.Text.Json;
using Bodoconsult.App.Abstractions.Interfaces;
using Microsoft.Extensions.Logging.EventSource;

namespace Bodoconsult.App.Logging;

/// <summary>
/// Listener for fetching log messages from ILogger based logging
/// </summary>
public class AppEventListener : EventListener, IAppEventListener
{
    private EventSource? _eventSource;

    private EventLevel _eventLevel;

    /// <summary>
    /// Default ctor
    /// </summary>
    public AppEventListener()
    {
        EventLevel = EventLevel.Warning;
    }

    /// <summary>
    /// Ctor to a certain eventlevel
    /// </summary>
    /// <param name="eventLevel"></param>
    public AppEventListener(EventLevel eventLevel)
    {
        EventLevel = eventLevel;
    }

    /// <summary>
    /// Event level for the listener. Default: 
    /// </summary>
    public EventLevel EventLevel
    {
        get => _eventLevel;
        set
        {
            _eventLevel = value;

            if (_eventSource is null)
            {
                return;
            }

            MyEnableEvents();
        }
    }

    /// <summary>
    /// Stores the log messages for later use
    /// </summary>
    public ConcurrentQueue<string> Messages { get; } = new();


    /// <summary>Called for all existing event sources when the event listener is created and when a new event source is attached to the listener.</summary>
    /// <param name="eventSource">The event source.</param>
    protected override void OnEventSourceCreated(EventSource eventSource)
    {
        if (eventSource is null)
        {
            throw new ArgumentNullException(nameof(eventSource));
        }

        Debug.Print($"EventSource{eventSource.Name}");

        if (eventSource.Name != "Microsoft-Extensions-Logging")
        {
            return;
        }

        _eventSource = eventSource;

        MyEnableEvents();
    }

    /// <summary>Called whenever an event has been written by an event source for which the event listener has enabled events.</summary>
    /// <param name="eventData">The event arguments that describe the event.</param>
    protected override void OnEventWritten(EventWrittenEventArgs eventData)
    {

        if (eventData is null)
        {
            throw new ArgumentNullException(nameof(eventData));
        }

        // Look for the formatted message event, which has the following argument layout (as defined in the LoggingEventSource.
        // FormattedMessage(LogLevel Level, int FactoryID, string LoggerName, string EventId, string FormattedMessage);
        if (eventData.EventSource.Name != "Microsoft-Extensions-Logging")
        {
            return;
        }

        //// Look for the formatted message event, which has the following argument layout (as defined in the LoggingEventSource.
        //// FormattedMessage(LogLevel Level, int FactoryID, string LoggerName, string EventId, string FormattedMessage);

        switch (eventData.EventName)
        {
            case "FormattedMessage":
                //case "Message":
                //case "MessageJson":
                var msg = $"{GetPayloadString(eventData.Payload)}";

                if (!string.IsNullOrEmpty(msg))
                {
                    Messages.Enqueue(msg);
                }
                break;
        }

        //Debug.Print($"ES: {eventData.EventSource.Name}:{eventData.EventName}: {msg}");
    }

    /// <summary>
    /// Get payload as formatted string
    /// </summary>
    /// <param name="eventDataPayload"></param>
    /// <returns>A formatted string with not empty payload data</returns>
    private static string GetPayloadString(ReadOnlyCollection<object?>? eventDataPayload)
    {
        var result = new StringBuilder();

        const int skipElementsNumber = 4;

        if (eventDataPayload is null || eventDataPayload.Count < skipElementsNumber)
        {
            return string.Empty;
        }

        for (var i = skipElementsNumber; i < eventDataPayload.Count; i++)
        {
            var o = eventDataPayload[i];

            if (o is null)
            {
                continue;
            }

            var v = o is string ? o.ToString() : JsonSerializer.Serialize(o);


            if (string.IsNullOrEmpty(v))
            {
                continue;
            }


            result.Append($"{v} : ");

        }

        var s = result.ToString();

        //var pos = s.IndexOf("Default : ", StringComparison.OrdinalIgnoreCase);

        //if (pos>-1)
        //{
        //    return s[(pos + 1)..];
        //}

        return s[..^3];
    }


    private void MyEnableEvents()
    {
        // initialize a string, string dictionary of arguments to pass to the EventSource.
        // Turn on loggers matching App* to Information, everything else (*) is the default level (which is EventLevel.Error)
        //var args = new Dictionary<string, string>() { { "FilterSpecs", "App*:Information;*" } };
        // Set the default level (verbosity) to Error, and only ask for the formatted messages in this case.

        if (_eventSource is null)
        {
            return;
        }

        if (_eventLevel == EventLevel.Informational)
        {
            var args = new Dictionary<string, string?>
                {
                    { "FilterSpecs", "Microsoft.EntityFrameworkCore*:Warning;*" }
                };

            EnableEvents(_eventSource, EventLevel, LoggingEventSource.Keywords.FormattedMessage, args);
        }
        else
        {
            EnableEvents(_eventSource, EventLevel, LoggingEventSource.Keywords.FormattedMessage);
        }
    }
}