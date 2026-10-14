// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.Executer;

#pragma warning disable CA1416

/// <summary>
/// Executer for sending a mail with suer access information of a Sql database on Ms SqlServer
/// </summary>
internal class FolderComplianceExecuter : DocuBaseExecuter
{

    private readonly FolderComplianceJobTask _jobTask;

    /// <inheritdoc />
    public override bool Run()
    {

        var result = true;

        try
        {

            SetStatus("Start compliance reporting for folders...");

            foreach (var folder in _jobTask.FolderPaths)
            {

                SetStatus($"Start compliance reporting for folder: {folder.Path}");


                if (!Directory.Exists(folder.Path)) continue;

                var di = new DirectoryInfo(folder.Path);

                ReportFolder(di, folder.ShowPermissionsOfPredefinedAccounts);

                if (!folder.IncludeSubFolders) continue;

                foreach (var subDir in di.EnumerateDirectories())
                {
                    ReportFolder(subDir, folder.ShowPermissionsOfPredefinedAccounts, 2);
                }
            }

            Save("Compliance reporting for file system folders");

            SetStatus("Compliance reporting for folders done!");


        }
        catch (Exception ex)
        {
            var msg = "FolderComplianceExecuter:Run";
            result = false;
            Errors.Add(new Exception(msg, ex));
            SetStatus(msg);
        }

        return result;
    }

    private void ReportFolder(DirectoryInfo di, bool showPermissionsOfPredefinedAccounts = false, int level = 1)
    {

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new Exception("FolderComplianceExecuter.ReportFolder: Works only on Windows OS.");
        }

        var acl = di.GetAccessControl(AccessControlSections.All);
        var rules = acl.GetAccessRules(true, true, typeof(NTAccount));

        if (level == 1)
        {
            Result.AddHeader1($"Compliance report for folder '{di.FullName}'");
        }
        else
        {
            Result.AddHeader2($"Subfolder '{di.FullName}'");
        }

        // Folder is a shared folder?
        try
        {
            var share = ShareHelper.PathToUnc(di.FullName);

            if (!string.IsNullOrEmpty(share)) Result.AddParagraph($"UNC path: {share}");
        }
        catch
        {
            // ignored
        }


        //Go through the rules returned from the DirectorySecurity
        foreach (AuthorizationRule rule in rules)
        {

            var user = rule.IdentityReference.Value;
            var checkUser = user.ToLower();

            // Check users
            if (!showPermissionsOfPredefinedAccounts)
            {
                if (checkUser.StartsWith("nt authority\\")) continue;
                if (checkUser.StartsWith("builtin\\")) continue;
                if (checkUser.StartsWith("nt-autorität\\")) continue;
                if (checkUser.StartsWith("vordefiniert\\")) continue;
            }

            // Add user permissions
            var reg = PrintAce((FileSystemAccessRule)rule);
            Result.AddDefinitionListLine(user, reg);
        }
    }


    private static string PrintAce(FileSystemAccessRule rule)
    {
        var result = string.Empty;

        try
        {
            foreach (var right in Enum.GetNames(typeof(FileSystemRights)).OrderBy(x => x))
            {
                try
                {
                    var val = Convert.ToInt32(Enum.Parse(typeof(FileSystemRights), right));
                    // remove combined values
                    if (val == 0x1f01ff || val == 0x301bf || val == 0x20089 || val == 0x200a9 || val == 0x116) continue;
                    if (((int)rule.FileSystemRights & val) > 0) result += $"{right} ";
                }
                catch
                {
                    // ignored
                }
            }
        }
        catch
        {
            // ignored
        }


        return result;
    }


    /// <summary>
    /// ctor to initialize task with job data
    /// </summary>
    /// <param name="jobTask">job task to run</param>
    public FolderComplianceExecuter(IJobTask jobTask)
        : base(jobTask)
    {
        _jobTask = (FolderComplianceJobTask)jobTask;
    }
}