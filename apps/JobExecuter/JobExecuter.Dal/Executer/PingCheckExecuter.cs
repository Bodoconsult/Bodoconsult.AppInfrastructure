// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Net.NetworkInformation;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Executer for checking successful ping to certain network addresses
/// </summary>
internal class PingCheckExecuter : BaseExecuter
{

    private readonly PingCheckJobTask _jobTask;


    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {

        var ipAddresses = _jobTask.IpAdresses.Split([';'], StringSplitOptions.RemoveEmptyEntries);

        var result = true;

        foreach (var ipAddress in ipAddresses)
        {
            if (!CheckPing(ipAddress)) result = false;
        }

        return result;
    }

    /// <summary>
    /// Check ping for a IP address
    /// </summary>
    /// <param name="ipAddress"></param>
    private bool CheckPing(string ipAddress)
    {
        var pingSender = new Ping();

        var reply = pingSender.Send(ipAddress, _jobTask.PingTimeout);

        if (reply == null)
        {
            var msg = $"Ping {ipAddress.PadRight(15)}: No reply";
            Errors.Add(new Exception(msg));
            SetStatus(msg);
            return false;
        }

        if (reply.Status == IPStatus.Success)
        {
            var msg = $"Ping {ipAddress.PadRight(15)}: {reply.Status}";
            SetStatus(msg);
            return true;
        }
        else
        {
            var msg = $"Ping {ipAddress.PadRight(15)}: {reply.Status}";
            Errors.Add(new Exception(msg));
            SetStatus(msg);
            return false;
        }
    }


    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public PingCheckExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (PingCheckJobTask)jobTask;
    }
}