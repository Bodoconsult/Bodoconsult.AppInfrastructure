// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using Bodoconsult.Network.Windows.Dns;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Executer for reporting DNS servers
/// </summary>
internal class DnsExecuter : DocuBaseExecuter
{

    private readonly DnsJobTask _jobTask;

    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {

        var result = true;

        try
        {

            SetStatus($"Create report for DNS server {_jobTask.Server}...");

            if (string.IsNullOrEmpty(_jobTask.Server))
            {
                var msg = "DnsExecuter:Run:No DNS server IP address supplied in the job task";
                result = false;
                Errors.Add(new Exception(msg));
                SetStatus(msg);
                return result;
            }

            var d = new DnsServer(_jobTask.Server);

            Result.AddHeader1($"Structure of DNS server '{_jobTask.Server}'");

            Result.AddHeader2("DNS domains");
            foreach (var domain in d.GetListOfDomains())
            {
                Result.AddHeader3($"{domain.Name} ({domain.ZoneType}{(domain.ReverseZone ? ", Reverse zone" : string.Empty)})");
                //and a list of all the records in the domain:-
                foreach (var record in d.GetRecordsForDomain(domain.Name))
                {
                    Result.AddParagraph(record.ToString());
                    //any domains we are primary for we could go and edit the record now!
                }
            }


            Save("DNS servers");

            SetStatus($"Report for DNS server {_jobTask.Server} created!");


        }
        catch (Exception ex)
        {
            var msg = "DnsExecuter:Run:Maybe not enough permissions to access DNS settings";
            result = false;
            Errors.Add(new Exception(msg, ex));
            SetStatus(msg);
        }

        return result;
    }


    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public DnsExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (DnsJobTask)jobTask;
    }
}