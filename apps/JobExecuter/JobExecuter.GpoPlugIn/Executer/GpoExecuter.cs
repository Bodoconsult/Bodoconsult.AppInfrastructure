// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Runtime.InteropServices;
using Bodoconsult.Text.Interfaces;
using Bodoconsult.Text.Model;
using JobExecuter.Base;
using JobExecuter.Base.Interfaces;
using JobExecuter.GpoPlugIn.Model;

namespace JobExecuter.GpoPlugIn.Executer;
#pragma warning disable CA1416

/// <inheritdoc />
/// <summary>
/// Creates reports for all GPOs in a Windowss domain and backups the GPOs to the same directory
/// </summary>
internal class GpoExecuter : BaseExecuter
{

    // https://docs.microsoft.com/en-us/powershell/module/grouppolicy/?view=win10-ps

    // https://medium.com/river-yang/powershell-run-as-another-user-elevate-the-process-58b90fc4d11d

    // https://adamtheautomator.com/powershell-export-gpo/

    // Start-Process powershell.exe -Credential “Domain\SuperUser” -ArgumentList “Start-Process powershell_ise.exe -Verb runAs”

    private readonly GpoJobTask _jobTask;


    private readonly IStructuredText _result;

    /// <inheritdoc />
    public override bool Run()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new Exception($"{GetType().Name}: Works only on Windows OS.");
        }

        try
        {
            if (!Directory.Exists(_jobTask.TargetDirectory)) Directory.CreateDirectory(_jobTask.TargetDirectory);
        }
        catch
        {
            return false;
        }

        //try
        //{
        // Clear old backups
        ClearOldBackups();

        var domain = string.IsNullOrEmpty(_jobTask.Domain) ? Domain.GetCurrentDomain().Name : _jobTask.Domain;
        SetStatus($"Working on GPOs for domain {domain}");

        _result.AddHeader1($"GPOs for domain '{0}'");

        var gpoDomain = new GpDomain(domain);

        gpoDomain.GetAllGpos();

        gpoDomain.Backup(_jobTask.TargetDirectory);
        _result.AddParagraph($"Backup folder GPOs to folder {_jobTask.TargetDirectory}");

        //Debug.Print(gpoDomain.DomainController);

        foreach (var gpo in gpoDomain.Gpos)
        {

            if (gpo == null) continue;

            SetStatus($"Working on GPO '{gpo.DisplayName}'");

            var htmlFile = ReportGpo(gpo);

            Docu(gpo, htmlFile);
        }


            

        // Save a HTML
        SaveAsHtml?.Invoke(Path.Combine(_jobTask.TargetDirectory, "GPO.htm"), _result, "GPO documentation");

        //    //var guid = new Guid("A7DE85DE-1234-F34D-99AD-5AFEDF7D7B4A");

        //    //var gpoData = gpoDomain.GetGpo(guid);
        //    //var gpoXmlReport = gpoData.GenerateReport(ReportType.Xml).ToString();

        //    //using (XmlReader reader = XmlReader.Create(new StringReader(gpoXmlReport)))
        //    //{
        //    //    string field;
        //    //    while (reader.MoveToNextAttribute())
        //    //    {
        //    //        foreach (string attr in attributes)
        //    //        {
        //    //            // do something
        //    //        }
        //    //    }
        //    //}

        return true;
        //}
        //catch (Exception ex)
        //{
        //    var msg = "GpoExecuter:Run:";
        //    Errors.Add(new Exception(msg, ex));
        //    SetStatus(msg + ex.Message);
        //    return false;
        //}

        //return true;
    }

    private void Docu(Gpo gpo, string htmlDocuPath)
    {

        try
        {
            _result.AddHeader2($"GPO '{gpo.DisplayName}'");

            if (!string.IsNullOrEmpty(htmlDocuPath)) _result.AddParagraph($"[Report GPO {gpo.DisplayName}]({htmlDocuPath})");

            if (!string.IsNullOrEmpty(gpo.Description)) _result.AddParagraph(gpo.Description);


            _result.AddDefinitionListLine("Owner: ", gpo.Owner);


            var id = gpo.Id;
            if (!string.IsNullOrEmpty(id)) _result.AddDefinitionListLine("GUID:", id);



            _result.AddDefinitionListLine("Created:", $"{gpo.CreationTime:g}");
            _result.AddDefinitionListLine("Modification:", $" {gpo.ModificationTime:g}");
            _result.AddDefinitionListLine("Path:", gpo.Path);

            //_result.AddDefinitionListLine("Security info:", FormatSecurityInfo(gpo.GetSecurityInfo()));

            //var wmi = gpo.WmiFilter;

            //if (wmi != null)
            //{
            //    _result.AddDefinitionListLine("WmiFilter",
            //        $"Name: {wmi.Name}\r\nDescription: {wmi.Description}\r\nQuery list: {wmi.GetQueryList()}");
            //}
        }
        catch
        {
            // ignored
        }
    }

    //private static string FormatSecurityInfo(GPPermissionCollection secInfos)
    //{
    //    var s = new StringBuilder();

    //    foreach (var secInfo in secInfos)
    //    {
    //        var tr = secInfo.Trustee;
    //        var trustee = tr == null ? "" : tr.Name;

    //        s.AppendLine("Permission: " + secInfo.Permission + " Denied: " + secInfo.Denied + " Inheritable: " +
    //                 secInfo.Inheritable
    //                 + " Trustee: " + trustee);
    //    }

    //    return s.ToString();
    //}

    /// <summary>
    /// Clear old backups
    /// </summary>
    private void ClearOldBackups()
    {


        if (!_jobTask.ClearOldBackups) return;

        var dir = new DirectoryInfo(_jobTask.TargetDirectory);
        foreach (var folder in dir.GetDirectories())
        {
            try
            {
                folder.Delete(true);
            }
            catch
            {
                // ignored
            }
        }
    }

    ///// <summary>
    ///// Backup current GPO
    ///// </summary>
    ///// <param name="gpo">GPO to backup</param>
    //private void BackupGpo(Gpo gpo)
    //{



    //    var backup = gpo.Backup(_jobTask.TargetDirectory);

    //    _result.AddParagraph("Backup folder " + backup.BackupDirectory + "\\{" + backup.Id + "}");
    //}


    private string ReportGpo(Gpo gpo)
    {
        var fileName = $"{gpo.DisplayName.Replace(" ", "_")}.htm";
        gpo.GenerateReportToFile(Path.Combine(_jobTask.TargetDirectory, fileName));
        return fileName;
    }



    /// <inheritdoc />
    public GpoExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _result = new StructuredText();
        _jobTask = (GpoJobTask)jobTask;
    }
}