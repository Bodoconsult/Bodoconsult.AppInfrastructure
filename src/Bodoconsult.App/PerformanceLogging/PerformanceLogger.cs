// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Text;
using Bodoconsult.App.Abstractions.Interfaces;
using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;

namespace Bodoconsult.App.PerformanceLogging;

/// <summary>
/// Current implementation of <see cref="IPerformanceLogger"/> for performance logging
/// </summary>
/// <remarks>
/// https://medium.com/criteo-engineering/net-core-counters-internals-how-to-integrate-counters-in-your-monitoring-pipeline-5354cd61b42e
/// https://learn.microsoft.com/en-us/dotnet/core/diagnostics/diagnostics-client-library
///
/// </remarks>
public class PerformanceLogger : IPerformanceLogger
{

    private readonly float _numberOfProcessors;

    private readonly DiagnosticsClient _diagnosticsClient;

    private EventPipeSession? _session;

    private readonly CancellationTokenSource _cancellationTokenSource = new ();


    private double _cpuUsage;

    private double _timeInGc;

    private int _workingSet;

    int _gcHeapSize;

    //private int _gen0GcCount;

    //private int _gen1GcCount;

    //private int _gen2GcCount;

    private int _gen0GcSize;

    private int _gen1GcSize;

    private int _gen2GcSize;

    /// <summary>
    /// Default ctor. Use current process ID
    /// </summary>
    public PerformanceLogger()
    {
        _numberOfProcessors = Environment.ProcessorCount;
        _diagnosticsClient = new DiagnosticsClient(Process.GetCurrentProcess().Id);
    }

    /// <summary>
    /// Ctor to provide a
    /// </summary>
    /// <param name="processId"></param>
    public PerformanceLogger(int processId)
    {
        _numberOfProcessors = Environment.ProcessorCount;
        _diagnosticsClient = new DiagnosticsClient(processId);
    }

    /// <summary>
    /// Start the logger
    /// </summary>
    public void StartLogger()
    {
        var providers = new List<EventPipeProvider>
        {
            // Runtime Metrics
            new("System.Runtime", EventLevel.Verbose, 0,
                new Dictionary<string, string> { { "EventCounterIntervalSec", "1" } }),
            //new("Microsoft-Windows-DotNETRuntime",
            //    EventLevel.Informational, (long)ClrTraceEventParser.Keywords.GC)
        };

        var monitorTask = new Task(() =>
        {

            _session = _diagnosticsClient.StartEventPipeSession(providers, false, 10);


            //if (_resumeRuntime)
            //{
            //    try
            //    {
            //        _diagnosticsClient.ResumeRuntime();
            //    }
            //    catch (UnsupportedCommandException)
            //    {
            //        // Noop if the command is unknown since the target process is most likely a 3.1 app.
            //    }
            //}
            var source = new EventPipeEventSource(_session.EventStream);
            source.Dynamic.All += DynamicAllMonitor;
            source.Process();
            //}
            //catch (DiagnosticsClientException ex)
            //{
            //    Debug.Print($"Failed to start the counter session: {ex.ToString()}");
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine($"[ERROR] {ex.ToString()}");
            //}
            //finally
            //{
            //    _shouldExit.TrySetResult(ReturnCode.Ok);
            //}
        }, _cancellationTokenSource.Token);

        monitorTask.Start();
    }

    /// <summary>
    /// Process all the received evemt data
    /// </summary>
    /// <param name="obj"></param>
    private void DynamicAllMonitor(TraceEvent obj)
    {
            
        Debug.Print(obj.EventName);


        if (!obj.EventName.Equals("EventCounters"))
        {
            return;
        }

        var payloadVal = (IDictionary<string, object>)(obj.PayloadValue(0));
        var payloadFields = (IDictionary<string, object>)(payloadVal["Payload"]);
        var p = payloadFields["Name"].ToString();

        //Debug.Print(p);

        if (p!.Equals("cpu-usage"))
        {
            _cpuUsage = double.Parse(payloadFields["Mean"].ToString() ?? string.Empty);
        }

        else if (p!.Equals("working-set"))
        {
            _workingSet = int.Parse(payloadFields["Mean"].ToString() ?? string.Empty);
        }

        else if (p!.Equals("gc-heap-size"))
        {
            _gcHeapSize = int.Parse(payloadFields["Mean"].ToString() ?? string.Empty);
        }

        //else if (p!.Equals("gen-0-gc-count"))
        //{
        //    //gen0GcCount = int.Parse(payloadFields["Sum"].ToString() ?? string.Empty);
        //}

        //else if (p!.Equals("gen-1-gc-count"))
        //{
        //    gen1GcCount = int.Parse(payloadFields["Sum"].ToString() ?? string.Empty);
        //}

        //else if (p!.Equals("gen-2-gc-count"))
        //{
        //    gen2GcCount = int.Parse(payloadFields["Mean"].ToString() ?? string.Empty);
        //}

        else if (p!.Equals("time-in-gc"))
        {
            _timeInGc = double.Parse(payloadFields["Mean"].ToString() ?? string.Empty);
        }

        else if (p!.Equals("gen-0-size"))
        {
            _gen0GcSize = int.Parse(payloadFields["Mean"].ToString() ?? string.Empty);
        }

        else if (p!.Equals("gen-1-size"))
        {
            _gen1GcSize = int.Parse(payloadFields["Mean"].ToString() ?? string.Empty);
        }

        else if (p!.Equals("gen-2-size"))
        {
            _gen2GcSize = int.Parse(payloadFields["Mean"].ToString() ?? string.Empty);
        }
    }

    /// <summary>
    /// Stop the logger
    /// </summary>
    public void StopLogger()
    {
        try
        {
            _session?.Dispose();
            _cancellationTokenSource.Cancel();
        }
        catch (EndOfStreamException ex)
        {
            // If the app we're monitoring exits abruptly, this may throw in which case we just swallow the exception and exit gracefully.
            Debug.WriteLine($"[ERROR] {ex}");
        }
        // We may time out if the process ended before we sent StopTracing command. We can just exit in that case.
        catch (TimeoutException)
        {
        }
        // On Unix platforms, we may actually get a PNSE since the pipe is gone with the process, and Runtime Client Library
        // does not know how to distinguish a situation where there is no pipe to begin with, or where the process has exited
        // before dotnet-counters and got rid of a pipe that once existed.
        // Since we are catching this in StopMonitor() we know that the pipe once existed (otherwise the exception would've 
        // been thrown in StartMonitor directly)
        catch (PlatformNotSupportedException)
        {
        }
        // On non-abrupt exits, the socket may be already closed by the runtime and we won't be able to send a stop request through it. 
        catch (ServerNotAvailableException)
        {
        }
    }


    /// <summary>
    /// Get important counters as formatted string
    /// </summary>
    /// <returns>String with performance counter data</returns>
    public string GetCountersAsString()
    {
        var s = new StringBuilder();

        s.AppendLine($"processor time avg % = {_cpuUsage:P2}");
        s.AppendLine($"RAM bytes = {_workingSet}");
        s.AppendLine($"gc time in % = {_timeInGc:P2}");
        s.AppendLine($"gen 0 collections = {_gen0GcSize:N2}");
        s.AppendLine($"gen 1 collections = {_gen1GcSize:N2}");
        s.AppendLine($"gen 2 collections = {_gen2GcSize:N2}");

        return s.ToString();
    }
}