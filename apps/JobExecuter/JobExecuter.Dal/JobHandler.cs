// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Bodoconsult.Text.Enums;
using Bodoconsult.Text.Interfaces;
using Bodoconsult.Text.Model;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Interfaces;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal;

/// <summary>
/// Class to handle a complete job
/// </summary>
public class JobHandler
{
    private int _counter;
    private string _jsonJobFile;
    private string _logPrefix;

    /// <summary>
    /// All <see cref="IJobMailer"/> objects to send error messages and summary to
    /// </summary>
    public IList<IJobMailer> Mailers { get; set; } = [];

    /// <summary>
    /// All error items that happened in the current job
    /// </summary>
    public IList<ErrorItem> ErrorItems { get; set; } = [];

    ///// <summary>
    ///// default ctor
    ///// </summary>
    //public JobHandler()
    //{
    //    _counter = 0;
    //}

    /// <summary>
    /// Current job to handle
    /// </summary>
    public IJob CurrentJob
    {
        get;
        set
        {
            field = value;
            ErrorItems.Clear();
        }
    }

    /// <summary>
    /// Run a single <see cref="IJobTask"/>
    /// </summary>
    /// <param name="jobTask"></param>
    /// <returns>true on success</returns>
    public bool RunTask(IJobTask jobTask)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var name = jobTask.GetType().Name;

        SendStatus($"{_logPrefix }:{DateTime.Now:yyyyMMdd HHmm}:JobTask{_counter}: type={name} name={jobTask.Name}\r\n",
            TextItemType.H2);

        var factory = jobTask.GetFactory();

        var executer = factory?.New(jobTask);
        if (executer == null)
        {
            return false;
        }

        executer.LogPrefix = _logPrefix;
        executer.StatusChanged += SendStatus;
        executer.MailSend += SendMail;
        executer.SaveAsHtml += FileHelper.SaveAsHtml;
        executer.SaveAsPdf += FileHelper.SaveAsPdf;
        executer.SaveAsJson += JsonHelper.SerializeAsFile;

        try
        {
            SendStatus($"{_logPrefix }:{DateTime.Now:yyyyMMdd HHmm}:JobTask{_counter}: Task started");

            var task = Task.Run(executer.Run);
            if (task.Wait(TimeSpan.FromSeconds(jobTask.Timeout)))
            {
                AddErrorsToErrorItems(executer, jobTask);
                stopwatch.Stop();

                var seconds = stopwatch.ElapsedTicks / (float)Stopwatch.Frequency;

                SendStatus(task.Result
                    ? $"{_logPrefix}:{DateTime.Now:yyyyMMdd HHmm}:JobTask{_counter}: done (Time: {seconds:#,##0.0}s)!\r\n"
                    : $"{_logPrefix}:{DateTime.Now:yyyyMMdd HHmm}:JobTask{_counter}: failed with {executer.Errors.Count} error(s) (Time: {seconds:#,##0.0}s)!\r\n");

                return task.Result;
            }

            executer.Kill();

            AddErrorsToErrorItems(executer, jobTask);

            var msg = string.Format($"{_logPrefix }:{DateTime.Now:yyyyMMdd HHmm}:JobTask{_counter}: type={name} name={jobTask.Name}: timeout after {1:#,##0.0}s\r\n");

            var ei = new ErrorItem
            {
                JobTask = jobTask,
                Exception = new Exception(msg)
            };

            ErrorItems.Add(ei);

            SendStatus(msg);
            return false;
        }
        catch (Exception ex)
        {
            AddErrorsToErrorItems(executer, jobTask);

            SendStatus($"{_logPrefix }:{DateTime.Now:yyyyMMdd HHmm}:JobTask{_counter}: type={name} name={jobTask.Name}: failed: {ex.Message}\r\n");
            var ei = new ErrorItem
            {
                JobTask = jobTask,
                Exception = ex
            };

            ErrorItems.Add(ei);

            return false;
        }
    }

    private void AddErrorsToErrorItems(ITaskExecuter executer, IJobTask jobTask)
    {
        foreach (var error in executer.Errors)
        {
            ErrorItems.Add(new ErrorItem
            {
                Exception = error,
                JobTask = jobTask
            });
        }
    }

    /// <summary>
    /// Send a mail to all mailers
    /// </summary>
    /// <param name="mailTo">receivers</param>
    /// <param name="subject">subject of the message</param>
    /// <param name="body">body of the message</param>
    /// <param name="priority">priority of the message</param>
    private void SendMail(string mailTo, string subject, IStructuredText body, bool priority)
    {
        foreach (var mailer in Mailers.Where(x => x.GetType() == typeof(SmtpJobMailer) ||
                                                  x.GetType() == typeof(FakeJobMailer)))
        {
            mailer.SendMail(mailTo, subject, body, priority);
        }
    }

    /// <summary>
    /// Send a mail to all mailers
    /// </summary>
    /// <param name="mailTo">receivers</param>
    /// <param name="subject">subject of the message</param>
    /// <param name="body">body of the message</param>
    private void SendMail(string mailTo, string subject, IStructuredText body)
    {
        foreach (var mailer in Mailers.Where(x => x.GetType() == typeof(SmtpJobMailer) ||
                                                  x.GetType() == typeof(FakeJobMailer)))
        {
            mailer.SendMail(mailTo, subject, body, false);
        }
    }

    /// <summary>
    /// Send a message to the windows eventlog
    /// </summary>
    /// <param name="mailTo">receivers</param>
    /// <param name="subject">subject of the message</param>
    /// <param name="body">body of the message</param>
    /// <param name="priority">priority of the message</param>
    private void SendEventlog(string mailTo, string subject, IStructuredText body, bool priority)
    {
        foreach (var mailer in Mailers.Where(x => x.GetType() == typeof(EventlogJobMailer) ||
                                                  x.GetType() == typeof(FakeJobMailer)))
        {
            mailer.SendMail(mailTo, subject, body, priority);
        }
    }

    /// <summary>
    /// Resolve all inner exceptions
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    private static string ResolveException(Exception ex)
    {
        var msg = ex.Message;
        if (ex.InnerException != null)
        {
            msg += $"\r\n{ResolveException(ex.InnerException)}";
        }
        else
        {
            msg += $"\r\n{ex.StackTrace}";
        }
        return msg;
    }

    /// <summary>
    /// Send status changed message
    /// </summary>
    /// <param name="message">message to send</param>
    private void SendStatus(string message)
    {
        SendStatus(message, TextItemType.P, null);
    }

    /// <summary>
    /// Send status changed message
    /// </summary>
    /// <param name="message">message to send</param>
    /// <param name="data">objects for formatting message</param>
    private void SendStatus(string message, params object[] data)
    {
        SendStatus(message, TextItemType.P, data);
    }

    /// <summary>
    /// Send status changed message
    /// </summary>
    /// <param name="message">true on success</param>
    /// <param name="textItemType">logical level of the text item</param>
    /// <param name="data">objects for formatting message</param>
    private void SendStatus(string message, TextItemType textItemType, params object[] data)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        if (message.EndsWith("\r\n"))
        {
            message = message[..^2];
        }

        if (message.EndsWith("\r\n"))
        {
            message = message[..^2];
        }

        if (message.EndsWith("\r\n"))
        {
            message = message[..^2];
        }

        var msg = data == null ? message : string.Format(message, data);

        var sr = new StructuredText();
        sr.AddText(msg, textItemType);

        SendStatus(sr);
    }

    private void SendStatus(IStructuredText sr)
    {
        foreach (
            var mailer in Mailers.Where(x => x.GetType() != typeof (SmtpJobMailer) && x.GetType() != typeof (EventlogJobMailer)))
        {
            mailer.SendMail(string.Empty, string.Empty, sr, false);
        }
    }

    /// <summary>
    /// Save current job as JSON to a file 
    /// </summary>
    /// <param name="fileName">Path for the JSON file to save</param>
    /// <returns>true on success</returns>
    public bool SaveJob(string fileName)
    {
        try
        {
            SendStatus("[JobHandler.SaveJob] Saving JSON to file {0}...", fileName);

            // serialize JSON directly to a file
            JsonHelper.SerializeAsFile(fileName, CurrentJob);

            SendStatus("[JobHandler.SaveJob] Saved JSON to file {0}.", fileName);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"[JobHandler.SaveJob] Saving JSON to {fileName} failed: ", ex);
        }
    }

    /// <summary>
    /// Load current job from JSON file 
    /// </summary>
    /// <param name="job">Job to run</param>
    /// <param name="jsonJobFile">Filename of the CJSON file</param>
    /// <returns>true on success</returns>
    public bool LoadJob(Job job, string jsonJobFile)
    {
        CurrentJob = job;
        CurrentJob.JobJsonFileName = jsonJobFile;
        _jsonJobFile = jsonJobFile;

        // Log the current machine
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ips = host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).Aggregate(string.Empty, (current, ip) =>
                $"{current}{ip} ");

            SendStatus($" Machine: {Environment.MachineName} (IP {ips}) User: {Environment.UserName}.");
        }
        catch
        {
            // ignored
        }

        // Log the start date
        var now = $"{DateTime.Now:yyyyMMdd HHmm}";

        var s = $"[JobExecuter.Start] Date: {now}";
        SendStatus(s);
       
        _logPrefix = $"[{CurrentJob.Name}:{now}]";

        SendStatus($"[JobExecuter.Start] job {CurrentJob.Name} loaded");

        return true;
    }

    /// <summary>
    /// Run the current job
    /// </summary>
    /// <returns>true if no job task caused an error</returns>
    public bool RunJob()
    {
        bool erg;

        if (_jsonJobFile != null && !new FileInfo(_jsonJobFile).Extension.Equals(".cjson", StringComparison.OrdinalIgnoreCase))
        {
            var msg = $"[JobExecuter.Start] Job file {_jsonJobFile} is no CJSON file! Due to security issues the job will not be started!";
            SendStatus(msg);

            return false;
        }

        if (!CheckJob())
        {
            SendStatus("[JobExecuter.Start] Job file not consistent! JobExecuter will not run the job;");
            return false;
        }

        try
        {
            SendStatus($"{_logPrefix }:{DateTime.Now:yyyyMMdd HHmm}: start processing the job", TextItemType.H1, CurrentJob.Name, DateTime.Now);

            var task = Task.Run(RunJobInternal);
            if (task.Wait(TimeSpan.FromSeconds(CurrentJob.Timeout)))
            {
                SendStatus($"{_logPrefix }:{DateTime.Now:yyyyMMdd HHmm}: {(task.Result ? "successful finished" : " failed")}");
                erg = task.Result;
            }
            else
            {
                var msg = $"{_logPrefix }:{DateTime.Now:yyyyMMdd HHmm}: timeout after {CurrentJob.Timeout}s";
                SendStatus(msg);

                var ei = new ErrorItem { Exception = new Exception(msg) };
                ErrorItems.Add(ei);

                erg = false;
            }
        }
        catch (Exception ex)
        {

            var ei = new ErrorItem { Exception = ex };
            ErrorItems.Add(ei);
            erg = false;
        }

        SendErrorsToEventlog();

        SendErrorMailsForTasks();

        SendErrorMail();

        SendSummary();

        return erg;
    }

    /// <summary>
    /// Make some plausibility checks for the job
    /// </summary>
    /// <returns></returns>
    private bool CheckJob()
    {
        if (CurrentJob == null)
        {
            return true;
        }

        //if (string.IsNullOrEmpty(_jsonJobFile)) return true;

        // Check if JobDocuJobTask is complete
        var erg = CurrentJob.JobTasks.Where(x => x is JobDocuJobTask).ToList();

        if (erg.Count==0)
        {
            return true;
        }

        foreach (var jobTask1 in erg)
        {
            var jobTask = (JobDocuJobTask) jobTask1;

            jobTask.CurrentJob ??= CurrentJob;

            if (string.IsNullOrEmpty(jobTask.JobJsonFileName))
            {
                jobTask.JobJsonFileName = _jsonJobFile;
            }

            if (!string.IsNullOrEmpty(jobTask.JsonTargetFileName))
            {
                continue;
            }

            var fi = new FileInfo(jobTask.JobJsonFileName);
            var docu = fi.FullName.Replace(fi.Extension, "_docu.json");

            jobTask.JsonTargetFileName = docu;
        }

        return true;
    }

    /// <summary>
    /// Write errors into Windows eventlog
    /// </summary>
    private void SendErrorsToEventlog()
    {
        try
        {
            if (!CurrentJob.JobTasks.Any())
            {
                return;
            }

            var tasks = CurrentJob.JobTasks;

            if (!tasks.Any())
            {
                return;
            }

            foreach (var task in tasks)
            {
                var errors = ErrorItems.Where(x => x.JobTask == task).ToList();

                if (errors.Count==0)
                {
                    continue;
                }

                var sr = new StructuredText();
                sr.AddHeader1($"Error in job {CurrentJob.Name} at task {task.Name}");

                foreach (var error in ErrorItems)
                {
                    sr.AddParagraph(ResolveException(error.Exception));
                }

                SendEventlog(task.ErrorMailReceiver, $"Error: {CurrentJob.Name}:{task.Name}", sr, true);
            }
        }
        catch (Exception ex)
        {
            SendStatus(ex.Message);
        }
    }

    /// <summary>
    /// Send error mails for single task errors
    /// </summary>
    private void SendErrorMailsForTasks()
    {
        try
        {
            if (!CurrentJob.JobTasks.Any())
            {
                return;
            }

            var tasks = CurrentJob.JobTasks.Where(x => !string.IsNullOrEmpty(x.ErrorMailReceiver)).ToList();

            if (tasks.Count == 0) return;

            foreach (var task in tasks)
            {
                var errors = ErrorItems.Where(x => x.JobTask == task).ToList();

                if (errors.Count == 0)
                {
                    continue;
                }

                var sr = new StructuredText();

                sr.AddHeader1($"Error in job {CurrentJob.Name} at task {task.Name}");

                var server = $"Source: {Environment.MachineName} User: {Environment.UserName}";

                sr.AddParagraph(server);

                foreach (var error in ErrorItems)
                {
                    sr.AddParagraph(ResolveException(error.Exception));
                }

                SendMail(task.ErrorMailReceiver, $"Error: {CurrentJob.Name}:{task.Name}", sr, true);
            }
        }
        catch (Exception ex)
        {
            SendStatus(ex.Message);
        }
    }

    /// <summary>
    /// Send an error mail to job error mail receivers
    /// </summary>
    private void SendErrorMail()
    {
        if (string.IsNullOrEmpty(CurrentJob.ErrorMailReceiver))
        {
            return;
        }

        if (ErrorItems.Count == 0)
        {
            return;
        }

        var sr = new StructuredText();
        sr.AddHeader1($"Job '{CurrentJob.Name}': Errors");

        var server = $"Source: {Environment.MachineName} User: {Environment.UserName}";

        sr.AddParagraph(server);

        GetErrorMessage(sr, TextItemType.H2);

        SendMail(CurrentJob.ErrorMailReceiver, $"Job '{CurrentJob.Name}': Errors", sr, true);
        SendStatus(sr);
    }
    /// <summary>
    /// Format all error messages
    /// </summary>
    /// <param name="sr">Error messages</param>
    /// <param name="headerLevel">Header level for the headline of the error part</param>
    private void GetErrorMessage(StructuredText sr, TextItemType headerLevel)
    {
        foreach (var ei in ErrorItems)
        {
            var ti = new TextItem
            {
                LogicalType = headerLevel,
                Content = $"Error at {(ei.JobTask != null ? $"task '{ei.JobTask.Name}'" : "job level")}"
            };

            sr.TextItems.Add(ti);

            ti = new TextItem
            {
                LogicalType = TextItemType.P,
                Content = ResolveException(ei.Exception)
            };

            sr.TextItems.Add(ti);
        }
    }

    /// <summary>
    /// Internal function to run job in a separate task
    /// </summary>
    /// <returns>true if success</returns>
    private bool RunJobInternal()
    {
        if (CurrentJob == null)
        {
            return false;
        }

        var erg = true;

        _counter = 0;

        foreach (var jt in CurrentJob.JobTasks)
        {
            _counter++;
            var z = RunTask(jt);
            if (!z) erg = false;
        }
        return erg;
    }

    /// <summary>
    /// Send a summary to mail receivers
    /// </summary>
    private void SendSummary()
    {
        if (string.IsNullOrEmpty(CurrentJob.SummaryMailReceiver))
        {
            return;
        }

        var sr = new StructuredText();
        sr.AddHeader1($"Summary of job {CurrentJob.Name}");

        var server = $"Source: {Environment.MachineName} User: {Environment.UserName}";

        sr.AddParagraph(server);

        sr.AddParagraph(CurrentJob.Description);

        if (ErrorItems.Count > 0)
        {
            sr.AddHeader2("Overview errors");

            sr.AddParagraph(
                $"Total number of errors: {ErrorItems.Count}\r\nNumber of job task errors: {ErrorItems.Count(x => x.JobTask != null)}\r\nNumber of job errors: {ErrorItems.Count(x => x.JobTask == null)}\r\n");

            GetErrorMessage(sr, TextItemType.H3);
        }

        SendMail(CurrentJob.SummaryMailReceiver, $"Summary: {CurrentJob.Name}", sr, false);

        SendStatus(sr);
    }

    /// <summary>
    /// Only for testing purposes: set JobJson file name (without loading the file)
    /// </summary>
    /// <param name="jobJson"></param>
    public void SetJobJsonFileName(string jobJson)
    {
        _jsonJobFile = jobJson;
    }
}