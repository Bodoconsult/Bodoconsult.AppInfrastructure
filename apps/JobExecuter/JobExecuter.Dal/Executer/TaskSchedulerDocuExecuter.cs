// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.IO;
using System.Linq;
using Bodoconsult.Text.Model;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;
using Microsoft.Win32.TaskScheduler;

namespace JobExecuter.Dal.Executer;

/// <inheritdoc />
/// <summary>
/// Executer for saving Windows task scheduler task´s as XML and their documentation
/// See https://github.com/dahall/TaskScheduler/wiki/Examples
/// </summary>
internal class TaskSchedulerDocuExecuter : BaseExecuter
{
    private readonly TaskSchedulerDocuJobTask _jobTask;
    private readonly StructuredText _result = new();
    private string[] _filters = [];
    private bool _isFilter;

    /// <summary>
    /// Run job task
    /// </summary>
    /// <returns>true in case of success</returns>
    public override bool Run()
    {
        var result = true;

        //try
        //{
        _isFilter = false;

        if (!string.IsNullOrEmpty(_jobTask.Filter))
        {
            _isFilter = true;
            _filters = _jobTask.Filter.Split(';');
        }

        _result.AddHeader1("Windows Task Scheduler: task documentation");

        using (var ts = new TaskService())
        {
            EnumFolderTasks(ts.RootFolder, ts.RootFolder.Name);
        }

        if (!string.IsNullOrEmpty(_jobTask.HtmlTargetFileName))
        {
            //FileHelper.CopyTemplate("printer.css", new FileInfo(_jobTask.HtmlTargetPath).DirectoryName);

            //var f = new HtmlTextFormatter
            //{
            //    StructuredText = _result,
            //    Template = FileHelper.GetTemplate("HtmlFile")
            //};
            //var result = f.GetFormattedText();

            //File.WriteAllText(_jobTask.HtmlTargetPath, result, System.Text.Encoding.UTF8);
            var result1 = FileHelper.SaveAsHtml(_jobTask.HtmlTargetFileName, _result, "Windows Task Scheduler: task documentation");

#if DEBUG
                SetStatus(result1);
#endif

        }

        if (!string.IsNullOrEmpty(_jobTask.JsonTargetFileName))
        {
            var json = JsonHelper.SerializeAsFile(_jobTask.JsonTargetFileName, _result);

#if DEBUG
                SetStatus(json);
#endif
        }
        //}
        //catch (Exception ex)
        //{
        //    result = false;
        //    const string msg = "Windows Task Scheduler documentation failed!";
        //    Errors.Add(new Exception(msg, ex));
        //    SetStatus(msg);
        //}

        return result;

    }



    private void EnumFolderTasks(TaskFolder fld, string folderName)
    {
        foreach (var task in fld.Tasks)
        {
            //SetStatus("Working on {0}", task.Name);
            ActOnTask(task, folderName);
        }

        foreach (var sfld in fld.SubFolders)
        {
            EnumFolderTasks(sfld, $"{folderName}\\{sfld.Name}");
        }
                
    }

    private void ActOnTask(Task t, string folderName)
    {
        if (_isFilter)
        {
            var result = _filters.Any(prefix => t.Name.StartsWith(prefix));
            if (!result) return;
        }


        SetStatus("Task {0} in progress...", t.Name);

        SetStatus("Export task Xml to folder...");
        ExportTaskAsXml(t);


        // Documentation
        SetStatus("Start documentation for task...");
        _result.AddHeader2("Task '{0}'", null, t.Name);
        _result.AddDefinitionListLine("Folder", folderName);
        _result.AddDefinitionListLine("Enabled:", t.Enabled.ToString());

        var td = t.Definition;

        _result.AddParagraph(td.Data);

        _result.AddDefinitionListLine("Logon user:", td.Principal.UserId);
        _result.AddDefinitionListLine("Logon type:", td.Principal.LogonType.ToString());

        GetTaskTriggers(td);

        GetTaskActions(td);

        _result.AddHeader3("XML of task (importable into task scheduler)");
        _result.AddXml(t.Xml);

        SetStatus("Task '{0}' done!", t.Name);
    }
    /// <summary>
    /// Export Xml to folder
    /// </summary>
    /// <param name="t">task to export</param>
    private void ExportTaskAsXml(Task t)
    {
        if (string.IsNullOrEmpty(_jobTask.XmlTargetPath)) return;

        try
        {
            if (!Directory.Exists(_jobTask.XmlTargetPath)) Directory.CreateDirectory(_jobTask.XmlTargetPath);
            var fileName = Path.Combine(_jobTask.XmlTargetPath, $"{t.Name}.xml");
            File.WriteAllText(fileName, t.Xml, System.Text.Encoding.Unicode);
        }
        catch (Exception ex)
        {
            Errors.Add(new Exception($"TaskSchedulerDocu:ExportTaskAsXml:{t.Name}", ex));
        }
    }

    /// <summary>
    /// Add the triggers of a task
    /// </summary>
    /// <param name="td">task definition</param>
    private void GetTaskTriggers(TaskDefinition td)
    {
        _result.AddHeader3("Task triggers");

        foreach (var trigger in td.Triggers)
        {

            _result.AddHeader4("{1}: '{0}'", null, trigger.Id, trigger.GetType().Name);

            switch (trigger.TriggerType)
            {
                case TaskTriggerType.Event:
                    var eTrigger = (EventTrigger)trigger;

                    if (!string.IsNullOrEmpty(eTrigger.Subscription)) _result.AddParagraph("Subscription: {0}", eTrigger.Subscription);

                    string source;
                    string log;
                    int? eventId;
                    eTrigger.GetBasic(out log, out source, out eventId);

                    _result.AddParagraph("Log: {0}, Source: {1}, EventId; {2}", null, log, source, eventId);

                    break;
                case TaskTriggerType.Time:
                    //var tTrigger = (TimeTrigger)trigger;
                    //_result.AddHeader4("TimeTrigger: '{0}'", tTrigger.Id);

                    //if (newVer) tTrigger.ExecutionTimeLimit = TimeSpan.FromSeconds(15);
                    //if (newVer) tTrigger.Id = "Time test";
                    //tTrigger.Repetition.Duration = TimeSpan.FromMinutes(20);
                    //tTrigger.Repetition.Interval = TimeSpan.FromMinutes(15);
                    //tTrigger.Repetition.StopAtDurationEnd = true;
                    break;
                case TaskTriggerType.Daily:
                    var dTrigger = (DailyTrigger)trigger;
                    _result.AddDefinitionListLine("DaysInterval: {0}", dTrigger.DaysInterval.ToString());
                    _result.AddDefinitionListLine("Duration:", $"{dTrigger.Repetition.Duration.Seconds}s");
                    _result.AddDefinitionListLine("Interval: ", $"{dTrigger.Repetition.Interval.Seconds}s");
                    break;
                case TaskTriggerType.Weekly:
                    var wTrigger = (WeeklyTrigger)trigger;
                    _result.AddDefinitionListLine("DaysOfWeek:", wTrigger.DaysOfWeek.ToString());
                    _result.AddDefinitionListLine("WeeksInterval:", wTrigger.WeeksInterval.ToString());
                    break;
                case TaskTriggerType.Monthly:
                    var mTrigger = (MonthlyTrigger)trigger;
                    _result.AddDefinitionListLine("DaysOfMonth:", mTrigger.DaysOfMonth.ToString());
                    _result.AddDefinitionListLine("MonthsOfYear:", mTrigger.MonthsOfYear.ToString());
                    _result.AddDefinitionListLine("RunOnLastDayOfMonth:", mTrigger.RunOnLastDayOfMonth.ToString());
                    break;
                case TaskTriggerType.MonthlyDOW:
                    var mdTrigger = (MonthlyDOWTrigger)trigger;
                    _result.AddDefinitionListLine("DaysOfWeek:", mdTrigger.DaysOfWeek.ToString());
                    _result.AddDefinitionListLine("MonthsOfYear:", mdTrigger.MonthsOfYear.ToString());
                    _result.AddDefinitionListLine("RunOnLastWeekOfMonth:", mdTrigger.RunOnLastWeekOfMonth.ToString());
                    _result.AddDefinitionListLine("WeeksOfMonth:", mdTrigger.WeeksOfMonth.ToString());
                    break;
                case TaskTriggerType.Idle:
                    //var iTrigger = (IdleTrigger)trigger;
                    //_result.AddHeader4("IdleTrigger: '{0}'", iTrigger.Id);
                    break;
                case TaskTriggerType.Registration:
                    var rTrigger = (RegistrationTrigger)trigger;
                    _result.AddDefinitionListLine("Delay:", $"{rTrigger.Delay.Seconds}s");
                    break;
                case TaskTriggerType.Boot:
                    var bTrigger = (BootTrigger)trigger;
                    _result.AddDefinitionListLine("Delay:", $"{bTrigger.Delay.Seconds}s");
                    break;
                case TaskTriggerType.Logon:
                    var lTrigger = (LogonTrigger)trigger;
                    _result.AddDefinitionListLine("Delay:", $"{lTrigger.Delay.Seconds}s");
                    _result.AddDefinitionListLine("UserId:", lTrigger.UserId);
                    break;
                case TaskTriggerType.SessionStateChange:
                    var sscTrigger = (SessionStateChangeTrigger)trigger;
                    _result.AddDefinitionListLine("StateChange:", string.Format(sscTrigger.StateChange.ToString()));
                    _result.AddDefinitionListLine("UserId:", sscTrigger.UserId);
                    _result.AddDefinitionListLine("Delay:", $"{sscTrigger.Delay.Seconds}s");
                    break;
                case TaskTriggerType.Custom:
                    //var cTrigger = (CustomTrigger)trigger;
                    //_result.AddHeader4("CustomTrigger: {0}", cTrigger.Id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _result.AddDefinitionListLine("StartBoundary:", $"{trigger.StartBoundary:yyyyMMdd HHmm}");
            _result.AddDefinitionListLine("EndBoundary:", $"{trigger.EndBoundary:yyyyMMdd HHmm}");
            _result.AddDefinitionListLine("ExecutionTimeLimit: ", $"{trigger.ExecutionTimeLimit.Seconds}s");

        }
    }
    /// <summary>
    /// Add the actions of a task
    /// </summary>
    /// <param name="td"></param>
    private void GetTaskActions(TaskDefinition td)
    {
        _result.AddHeader3("Task actions");

        foreach (var action in td.Actions)
        {
            switch (action.ActionType)
            {
                case TaskActionType.Execute:
                    var exec = (ExecAction)action;
                    _result.AddHeader4("ExecAction: {0}", exec.Id);
                    _result.AddDefinitionListLine("Path:", exec.Path);
                    if (!string.IsNullOrEmpty(exec.Arguments)) _result.AddDefinitionListLine("Args:", exec.Arguments);
                    break;
                case TaskActionType.ComHandler:
                    var com = (ComHandlerAction)action;
                    _result.AddHeader4("ComHandlerAction: {0}", com.Id);
                    _result.AddDefinitionListLine("Class:", $"{com.ClassName} [{com.ClassId}]");
                    //_result.AddParagraph("Args: {0}", com.Data);
                    break;
                case TaskActionType.SendEmail:
                    var se = (EmailAction)action;
                    _result.AddHeader4("EmailAction: {0}", se.Id);
                    _result.AddDefinitionListLine("Mail to:", se.To);
                    _result.AddDefinitionListLine("Subject:", se.Subject);
                    _result.AddDefinitionListLine("Body:", se.Body);
                    break;
                case TaskActionType.ShowMessage:
                    var sma = (ShowMessageAction)action;
                    _result.AddHeader4("ShowMessageAction: {0}", sma.Id);
                    _result.AddDefinitionListLine("Title:", sma.Title);
                    _result.AddDefinitionListLine("MessageBody:", sma.MessageBody);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (action.ActionType == TaskActionType.ComHandler)
            {

            }
        }
    }


    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public TaskSchedulerDocuExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (TaskSchedulerDocuJobTask)jobTask;
    }
}