// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Backup;
using Bodoconsult.App.Helpers;
using Bodoconsult.Test.Helpers;
using Bodoconsult.Test.MetaData.Handler;
using Bodoconsult.Test.MetaData.Model;
using Bodoconsult.Text.Helpers;
using Bodoconsult.Text.Interfaces;
using Bodoconsult.Text.Model;
using JobExecuter.Base.Interfaces;
using JobExecuter.Dal;
using JobExecuter.Dal.Enums;
using JobExecuter.Dal.Helpers;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace JobExecuter.Test.DiverseTests;

[TestFixture]
public class CreateSamplesAndDocumentationTests
{
    private MetaDataHandler mh;

    private const string SourcePath = @"d:\tmp\FolderBackup1";
    private const string TargetPath = @"d:\tmp\FolderBackup2";

    private static string localPath;
    private static string targetPath;
    private const string fileName = "testbackup.bak";
    private const string psFilename = "PowerShellScript.ps1.bak";
    private const string fileName2 = "??server??_??database??.bak";
    private const string sql = "exec spTest 1\r\nGO\r\nexec spTest 2\r\nGO\r\nexec spTest 3";

    //private static string dirJe;
    private static string dirJeDal;
    private static string dirNameJeSamples;
    private static string dirNameJeDalSamples;

    private static string dirNameJeDocu;

    private const string json2 = @"d:\tmp\test.jsonSample.json";
    private const string json1 = @"d_:\tmp\test.jsonSample.json";

    private readonly Assembly _currentAssembly;
    private readonly string _appPath;

    public CreateSamplesAndDocumentationTests()
    {
        _currentAssembly = Assembly.GetExecutingAssembly();
        _appPath = new FileInfo(_currentAssembly.Location).DirectoryName;

        localPath = TestDataHelper.LocalPath;
        targetPath = Path.Combine(localPath,  "network");

        var dirSolution1 = Path.Combine(TestDataHelper.SolutionPath,  "apps\\JobExecuter");
        //dirJe = Path.Combine(dirSolution1, @"JobExecuter\");
        dirJeDal = Path.Combine(dirSolution1, @"JobExecuter.Dal\");
        dirNameJeSamples = Path.Combine(dirSolution1, @"JobExecuter\Samples\");
        dirNameJeDalSamples = Path.Combine(dirSolution1, @"JobExecuter.Dal\Samples\");
        dirNameJeDocu = Path.Combine(TestDataHelper.SolutionPath, @"doc\JobExecuter");
    }

    [Test]
    public void CreateSamplesAndDocumentation_ValidJob_SamplesAndDocuAreCreated()
    {
        mh = new MetaDataHandler();
        mh.LoadReferencedAssemblies(_currentAssembly);

        // Init job
        var job = DefineJob();

        // init job handler
        var jh = new JobHandler
        {
            CurrentJob = job
        };

        var count = jh.CurrentJob.JobTasks.Count;

        var jsonFile = Path.Combine(dirNameJeSamples, "Sample.json");

        jh.Mailers.Add(new FakeJobMailer());
        jh.SaveJob(jsonFile);
        jh.CurrentJob = null;
        jh.LoadJob(job, jsonFile);

        jsonFile = Path.Combine(dirNameJeDalSamples, "Sample.json");
        jh.SaveJob(jsonFile);

        ArgumentNullException.ThrowIfNull(jh.CurrentJob);

        Assert.That(count == jh.CurrentJob.JobTasks.Count);
        Assert.That(File.Exists(jsonFile));

        CheckJobTasks(job);

        CreateSamplesAndHelpForJobTasks(job);

        CreateHtmlDocu();

        CreateAppHelp();

        CreateIncludes();
    }

    private static void CheckJobTasks(IJob job)
    {
        foreach (var task in job.JobTasks.Where(x => string.IsNullOrEmpty(x.Name)))
        {
            task.Name = $"Sample for {task.GetType().Name} job task";
        }
    }

    /// <summary>
    /// Create include files to use in BodoCms for web page content
    /// </summary>
    private void CreateIncludes()
    {
        // Create include JobExecuter1.inc with a definition list for JobTask-Classes
        var sr = new StringBuilder();

        sr.AppendLine("<dl>");
        foreach (var item in mh.TypeMetaDatas.OrderBy(x => x.Name))
        {
            var s = GetBreakableName(item.Name);
            sr.AppendFormat("<dt>{0}</dt><dd>{1}</dd>\r\n", s, item.Summary?.Trim());
        }

        sr.AppendLine("</dl>");

        File.WriteAllText(TestDataHelper.IncludePath, sr.ToString(), Encoding.UTF8);
    }

    private static Job DefineJob()
    {
        var job = new Job
        {
            Name = "Sample job with all kinds of job tasks",
            Description = "This job shows typical settings for all kinds of job taks",
            Timeout = 240,
            ErrorMailReceiver = "errorreceiver1@test.xxx;errorreceiver2@test.xxx",
            SummaryMailReceiver = "summaryreceiver1@test.xxx;summaryreceiver2@test.xxx",
            WriteToEventLog = true
        };

        var ads = new ActiveDirectoryJobTask
        {
            Name = "Report Active Directory",
            Description =
                "Report the current domain's Active Directory structure with organisation units (OU), users, groups and computers.",
            HtmlTargetFileName = Path.Combine(localPath, "ActiveDirectory.htm")
        };

        job.JobTasks.Add(ads);

        var backup = new BackupBatchJobTask
        {
            Name = "Backup file system",
            Description = "Take a backup of a filesystem folder using diskshadowing if needed and keeping old backups for a certain amount of time depending on the backup mode",
            ShadowingSourceDrive = "C:",
            ShadowingTargetDrive = "H:",
            GetAliveFolder = Path.GetTempPath(),
        };

        backup.BackupTargets.Add(new BackupTargetSettings
        {
            BackupMode = BackupModeEnum.Simple,
            Count = 5,
            Logfile = Path.Combine(localPath, "logfile.log"),
            Source = SourcePath,
            Target = TargetPath,
            Thread = 1
        });

        job.JobTasks.Add(backup);

        var dhcp = new DhcpJobTask
        {
            Name = "Report Dhcp infrastructure",
            Description =
                "Report the current domain's DHCP infrastructure",
            HtmlTargetFileName = Path.Combine(localPath, "DHCP.htm")
        };

        job.JobTasks.Add(dhcp);

        var dns = new DnsJobTask
        {
            Name = "Report a DNS server's structure",
            Description =
                "Report DNS server's structure",
            HtmlTargetFileName = Path.Combine(localPath, "DNS.htm"),
            Server = "BCGS02DC.BCG-AD.DE"
        };

        job.JobTasks.Add(dns);

        var jt = new SqlBackupSingleDatabaseJobTask
        {
            Name = "Backup database to local file only",
            Server = ".\\sqlexpress",
            DatabaseName = "MediaDb",
            LocalTarget = localPath,
            FileName = fileName,
            Timeout = 60,
            RemoveLocalBackup = false
        };

        job.JobTasks.Add(jt);

        jt = new SqlBackupSingleDatabaseJobTask
        {
            Name = "Backup database to a network path",
            Server = ".\\sqlexpress",
            DatabaseName = "MediaDb",
            LocalTarget = localPath,
            NetworkTarget = targetPath,
            FileName = fileName,
            Timeout = 60,
            RemoveLocalBackup = true
        };

        job.JobTasks.Add(jt);


        var jtl = new SqlBackupLogSingleDatabaseJobTask
        {
            Name = "Backup database to a network path",
            Server = ".\\sqlexpress",
            DatabaseName = "MediaDb",
            LocalTarget = localPath,
            NetworkTarget = targetPath,
            FileName = fileName,
            Timeout = 60,
            RemoveLocalBackup = true
        };

        job.JobTasks.Add(jtl);

        var ja = new SqlBackupAllDatabasesJobTask
        {
            Name = "Local backup of all databases on a SqlServer instance",
            Server = ".\\sqlexpress",
            LocalTarget = localPath,
            FileName = fileName2,
            Timeout = 60,
            RemoveLocalBackup = false
        };

        job.JobTasks.Add(ja);


        ja = new SqlBackupAllDatabasesJobTask
        {
            Name = "Network share backup of all databases on a SqlServer instance",
            Server = ".\\sqlexpress",
            LocalTarget = localPath,
            NetworkTarget = targetPath,
            FileName = fileName,
            Timeout = 60,
            RemoveLocalBackup = false
        };

        job.JobTasks.Add(ja);

        var smjt = new SqlMaintenanceJobTask
        {
            Name = "Runs a maintenance job on a SqlServer instance",
            MailTo = "test@test123.de",
            HtmlTargetFileName = Path.Combine(localPath, "Maintenance.htm"),
            Timeout = 600,
            Server = ".\\sqlexpress"
        };


        job.JobTasks.Add(smjt);


        var fc = new FolderCleanJobTask
        {
            Name = "Clean a folder from files older than 7 days",
            FolderPath = Path.GetTempPath(),
            Days = 7,
            Timeout = 60
        };

        job.JobTasks.Add(fc);

        var br = new SqlBatchJobTask
        {
            Name = "Run a SQL batch file",
            Server = ".\\sqlexpress",
            DatabaseName = "MediaDb",
            FileName = fileName,
            Timeout = 60
        };

        job.JobTasks.Add(br);

        br = new SqlBatchJobTask
        {
            Name = "Run SQL batch directly from string",
            Server = ".\\sqlexpress",
            DatabaseName = "MediaDb",
            BatchContent = sql,
            Timeout = 60
        };

        job.JobTasks.Add(br);

        var fb = new FolderBackupJobTask
        {
            Name = "Backup a folder to a local or network share target",
            SourcePath = SourcePath,
            TargetPath = TargetPath,
            BackupType = FolderBackupType.CopyOnlyNewerFiles,
            Timeout = 60
        };

        job.JobTasks.Add(fb);


        var cb = new CmdBatchJobTask
        {
            Name = "Run a batch (*.bat or *.cmd) with cmd.exe",
            RunType = CmdBatchRunType.WholeFile,
            Filename = fileName,
            Timeout = 110
        };

        job.JobTasks.Add(cb);


        cb = new CmdBatchJobTask
        {
            Name = "Run a batch (*.bat or *.cmd) with cmd.exe",
            RunType = CmdBatchRunType.LineByLine,
            Filename = fileName,
            Timeout = 110
        };

        job.JobTasks.Add(cb);

        cb = new CmdBatchJobTask
        {
            Name = "Run a batch commands from a string",
            RunType = CmdBatchRunType.LineByLine,
            Timeout = 110,
            BatchFileContent = "REm dfgsfd\r\ncopy d:\\tmp\\FolderBackup1\\*.* d:\\tmp\\FolderBackup2\\*.* /y\r\n" +
                               "REM fghdfg\r\ncopy d:\\tmp\\FolderBackup2\\*.* d:\\tmp\\FolderBackup1\\*.* /y\r\n" +
                               "REM fghfsd\r\n"
        };

        job.JobTasks.Add(cb);

        var pc = new PingCheckJobTask
        {
            Name = "Sample task for ping check",
            IpAdresses = "192.168.12.15;192.168.12.20;192.168.12.1;8.8.8.8",
            Timeout = 60
        };

        job.JobTasks.Add(pc);


        var ds = new DiskSizeJobTask();
        ds.Drives.Add(new Drive { Name = "C:", MinimumPercentageOfFreeSpace = 0.3 });
        //ds.Drives.Add(new Drive { Name = "D:", MinimumPercentageOfFreeSpace = 0.4 });
        ds.Drives.Add(new Drive { Name = @"\\bododata\data$", MinimumPercentageOfFreeSpace = 0.7 });
        ds.Drives.Add(new Drive { Name = "C:", MinimumPercentageOfFreeSpace = 0.3, IpAddress = "192.168.12.219" });
        ds.Drives.Add(new Drive { Name = "C:", MinimumPercentageOfFreeSpace = 0.3, IpAddress = "192.168.12.15" });

        job.JobTasks.Add(ds);


        var ra = new RunAppJobTask
        {
            Name = "Sample for running a app without parmeters",
            Filename = "robocopy.exe"
        };

        job.JobTasks.Add(ra);

        ra = new RunAppJobTask
        {
            Name = "Sample for running a app with parameters",
            Filename = "robocopy.exe",
            Arguments = "/?"
        };

        job.JobTasks.Add(ra);

        var ps = new PsBatchJobTask
        {
            Name = "Running a PowerShell batch file with PowerShell V01",
            Filename = psFilename,
            PowerShellVersion = PowerShellVersion.V01
        };

        job.JobTasks.Add(ps);

        ps = new PsBatchJobTask
        {
            Name = "Running a PowerShell batch file with PowerShell V03",
            Filename = psFilename,
            PowerShellVersion = PowerShellVersion.V03
        };

        job.JobTasks.Add(ps);

        var ts = new TaskSchedulerDocuJobTask
        {
            Name = "Getting all task scheduler tasks from local computer",
            XmlTargetPath = TargetPath,
            HtmlTargetFileName = Path.Combine(TargetPath, "docu.html"),
            JsonTargetFileName = Path.Combine(TargetPath, "docu.json")
        };

        job.JobTasks.Add(ts);

        ts = new TaskSchedulerDocuJobTask
        {
            Name = "Getting task scheduler data from local computer with use of filter expressions",
            Description = "Filters only tasks starting with Adobe or Microsoft",
            XmlTargetPath = TargetPath,
            Filter = "Adobe;Microsoft",
            HtmlTargetFileName = Path.Combine(TargetPath, "docu.html"),
            JsonTargetFileName = Path.Combine(TargetPath, "docu.json")
        };

        job.JobTasks.Add(ts);

        var htmlSqlDocuFile = Path.Combine(localPath, "database_docu.htm");
        var jsonSqlDocuFile = Path.Combine(localPath, "database_docu.json");

        var sd = new SqlDocuSingleDatabaseJobTask
        {
            Name = "Create a documentation for a single database",
            Server = ".\\sqlexpress",
            DatabaseName = "MediaDb",
            HtmlTargetFileName = htmlSqlDocuFile,
            JsonTargetFileName = jsonSqlDocuFile,
            Timeout = 60
        };

        job.JobTasks.Add(sd);

        var docuFile = Path.Combine(localPath, $"{fileName}.htm");
        var jsonFile = Path.Combine(localPath, $"{fileName}.json");

        var ad = new SqlDocuAllDatabasesJobTask
        {
            Name = "Create a documentation for all databases on a SqlServer with exception of databases Master, Msdb, Model and TempDb",
            Server = ".\\sqlexpress",
            HtmlTargetFileName = docuFile,
            JsonTargetFileName = jsonFile,
            Timeout = 60
        };

        job.JobTasks.Add(ad);

        var sms = new SqlComplianceSingleDatabaseJobTask
        {
            Name = "Create compliance information for a single database",
            Server = ".\\sqlexpress",
            DatabaseName = "DatabaseXY",
            MailTo = "info@bodoconsult.de",
            HtmlTargetFileName = Path.Combine(localPath, "ServerXY_DatabaseXY_ComplianceReport.htm"),
            Timeout = 60
        };

        var sma = new SqlComplianceAllDatabasesJobTask
        {
            Name = "Create compliance information for all databases on a SqlServer with exception of databases Master, Msdb, Model and TempDb",
            Server = ".\\sqlexpress",
            MailTo = "info@bodoconsult.de",
            HtmlTargetFileName = Path.Combine(localPath, "ServerXY_DatabaseComplianceReport.htm"),
            Timeout = 60
        };

        job.JobTasks.Add(sma);

        job.JobTasks.Add(sms);


        var fct = new FolderComplianceJobTask
        {
            Name = "Create a compliance report for file system folders",
            MailTo = "test@test123.de",


            HtmlTargetFileName = Path.Combine(localPath, "Compliance2.htm"),
            Timeout = 60
        };

        fct.FolderPaths.Add(new ComplianceFolder
        {
            Path = @"c:\daten\projekte",
            ShowPermissionsOfPredefinedAccounts = true,
            IncludeSubFolders = true
        });

        job.JobTasks.Add(fct);


        var jd = new JobDocuJobTask
        {
            Name = "Document the current JobExecuter job",
            Description =
                "If JobJsonFileName and DocuJsonFileName left empty, JobExecuter will insert current job file path and as docu file name the current job file path with the suffix _docu.json",
            JsonTargetFileName = "",
            JobJsonFileName = ""
        };
        job.JobTasks.Add(jd);

        var jobDocuJson = Path.Combine(TargetPath, "job.json");

        jd = new JobDocuJobTask
        {
            Name = "Document the a JobExecuter job",
            Description = "Set JobJsonFileName and DocuJsonFileName to a spezific path if needed",
            JsonTargetFileName = jobDocuJson,
            JobJsonFileName = Path.Combine(TargetPath, "job.json")
        };
        job.JobTasks.Add(jd);


        var doc = new DocumentationJobTask
        {
            Title = "Full documentation",
            Name = "Full documentation"
        };
        doc.HtmlTargetFiles.Add(Path.Combine(TargetPath, "documentation.htm"));

        doc.JsonFiles.Add(json1);
        doc.JsonFiles.Add(json2);
        //doc.JsonFiles.Add(jobDocuJson);
        //doc.JsonFiles.Add(jsonSqlDocuFile);

        job.JobTasks.Add(doc);
        return job;
    }

    private void CreateAppHelp()
    {
        var path = Path.Combine(dirJeDal, "Resources");
        var txtHelpFile = Path.Combine(path, "HelpFile.txt");
        var jsonHelpFile = Path.Combine(path, "HelpFile.json");

        var readMeHtm = Path.Combine(dirNameJeSamples, "Readme.htm");
        var readMeTxt = Path.Combine(dirNameJeSamples, "Readme.txt");

        var htmlHelpFile = Path.Combine(dirNameJeSamples, "JobExecuter_Help.htm");
        var markdownHelpFile = Path.Combine(dirNameJeDocu, "documentation.md");

        var rt = new StructuredText();
        rt.AppendUtf8PlainTextFile(readMeTxt);
        FileHelper.SaveAsHtml(readMeHtm, rt, "");

        var sr = new StructuredText();

        // Load basic help
        sr.AppendUtf8PlainTextFile(txtHelpFile);

        // Load overview over classes
        CreateOverviewClasses(sr);

        // Load docu for classes
        CreateDocuClasses(sr);

        // add help to Readme.txt
        sr.AppendUtf8PlainTextFile( readMeTxt);

        AddLegalNotes(sr);

        SimpleJsonHelper.SerializeAsFile(jsonHelpFile, sr);

        FileHelper.SaveAsHtml(htmlHelpFile, sr, "JobExecuter documentation");
        FileHelper.SaveAsMarkdown(markdownHelpFile, sr, "JobExecuter documentation");

        TestDataHelper.SaveAsHtmlPage(sr);

        TestHelper.OpenFile(htmlHelpFile);
    }

    private static void AddLegalNotes(StructuredText sr)
    {
        var licPath = Path.Combine(dirJeDal, "Licences.txt");

        sr.AddHeader1("Legal notes");
        sr.AddParagraph($"Date this documentation was created: {DateTime.Now:g}");

        sr.AddHeader2("Registered and unregistered trademarks");
        sr.AddParagraph("All other registered and unregistered trademarks are the property of their respective owners.");

        // add Licence information
        sr.AddHeader2("Licence information");
        sr.AppendUtf8PlainTextFile(licPath, true);
    }

    private void CreateOverviewClasses(StructuredText sr)
    {

        sr.AddHeader1("Overview JobTask classes");
        foreach (var item in mh.TypeMetaDatas.OrderBy(x => x.Name))
        {
            var s = GetBreakableName(item.Name);
            sr.AddDefinitionListLine(s, item.Summary);
        }
    }

    private static string GetBreakableName(string name)
    {
        var i = name.IndexOf("JobTask", StringComparison.Ordinal);

        if (i == -1)
        {
            return name;
        }

        // Add "&shy;": did not work in 

        return $"{name[..i]}JobTask";
    }


    private string GetFormattedString(string data)
    {
        return data;
        //data = data.Replace("<para>", "??pa??")
        //    .Replace("</para>", "??pe??").Replace("\t", "&nbsp;&nbsp;&nbsp;");

        //if (data.Contains("??pa??"))
        //{
        //    return data.Replace("??pa??", "<p>")
        //        .Replace("??pe??", "</p>")
        //        .Replace("??pe??\r\n", "??pe??");
        //}
        //else
        //{
        //    return "??pa??" + data.Replace("\r\n", "??pe????pa??") + "??pe??";
        //}

    }



    private void CreateDocuClasses(IStructuredText sr)
    {

        sr.AddHeader1("Documentation JobTask classes");
        foreach (var item in mh.TypeMetaDatas.OrderBy(x => x.Name))
        {
            sr.AddHeader2(item.Name);

            var summary = GetFormattedString(item.Summary);
            sr.AddParagraph(summary);

            if (!string.IsNullOrEmpty(item.Remarks))
            {
                var remarks = GetFormattedString(item.Remarks);
                sr.AddParagraph(remarks);
            }

            if (!string.IsNullOrEmpty(item.Code))
            {
                var code = GetFormattedString(item.Code);
                sr.AddCode(code);
            }


            var data = item.TypeItemMetaDatas.Where(x => x.Type == 'P').OrderBy(x => x.Name).ToList();
            if (data.Count != 0)
            {
                sr.AddHeader3("Properties");

                foreach (var pItem in data)
                {
                    sr.AddDefinitionListLine(pItem.Name, pItem.Summary);
                }
            }

            data = item.TypeItemMetaDatas.Where(x => x.Type == 'M').OrderBy(x => x.Name).ToList();
            if (data.Count != 0)
            {
                sr.AddHeader3("Methods");

                foreach (var pItem in data)
                {
                    sr.AddDefinitionListLine(pItem.Name, pItem.Summary);
                }
            }

            data = item.TypeItemMetaDatas.Where(x => x.Type == 'E').OrderBy(x => x.Name).ToList();
            if (data.Count != 0)
            {
                sr.AddHeader3("Events");

                foreach (var pItem in data)
                {
                    sr.AddDefinitionListLine(pItem.Name, pItem.Summary);
                }
            }

            if (string.IsNullOrEmpty(item.JsonContentFilename))
            {
                continue;
            }

            sr.AddCode(FileHelper.GetTextFile(item.JsonContentFilename), "prettyprint linenums lang-json");
        }
    }

    private void CreateHtmlDocu()
    {

        // Copy stylesheet
        var cssSource = Path.Combine(dirJeDal, @"Templates\printer.css");
        var cssTarget = Path.Combine(dirNameJeSamples, "printer.css");

        if (File.Exists(cssTarget))
        {
            File.Delete(cssTarget);
        }
        File.Copy(cssSource, cssTarget);

        // build XML
        var xml = Path.Combine(_appPath, "JobExecuter.Dal.xml");
        var html = Path.Combine(dirNameJeSamples, "documentation.html");

        //var xmlString = File.ReadAllText(xml, Encoding.UTF8);

        var myXslTrans = new XslCompiledTransform();

        var path = GetTemplate("XmlDocumentation");

        myXslTrans.Load(path);
        myXslTrans.Transform(xml, html);




        //var myXPathDoc = new XPathDocument(xml);

        //var myXslTrans = new XslCompiledTransform();

        //myXslTrans.Load(GetTemplate("XmlDocumentation"));

        //var myWriter = new XmlTextWriter(html, null);

        //myXslTrans.Transform(myXPathDoc, null, myWriter);

    }

    /// <summary>
    /// Get template file as string
    /// </summary>
    /// <param name="templateName">Full path to template file</param>
    /// <returns>template content</returns>
    private static string GetTemplate(string templateName)
    {
        try
        {
            // ReSharper disable once AssignNullToNotNullAttribute

            var dir = Assembly.GetExecutingAssembly().Location;

            if (string.IsNullOrEmpty(dir))
            {
                return string.Empty;
            }

            var directoryName = new FileInfo(dir).DirectoryName;
            if (directoryName == null)
            {
                return string.Empty;
            }

            return Path.Combine(directoryName, $"Templates\\{templateName}.xslt");

        }
        catch
        {
            return "";
        }
    }

    private void CreateSamplesAndHelpForJobTasks(Job dataJob)
    {

        var types = dataJob.JobTasks.GroupBy(x => x.GetType());

        foreach (var typeGroup in types)
        {
            var item = mh.AddType(typeGroup.Key);

            CreateSampleJobForJobTask(dataJob, typeGroup, item);
        }


        //foreach (var type in mh.TypeMetaDatas)
        //{
        //    Debug.Print("{0}  {1}", type.Name, type.FullName);

        //    Debug.Print("{0}\r\n{1}", type.Summary, type.Example);
        //}

    }

    private static void CreateSampleJobForJobTask(IJob dataJob, IGrouping<Type, IJobTask> type, TypeMetaData item)
    {
        var job = new Job
        {
            Name = $"Sample job for {type.Key.Name}",
            Description = $"This job contains one or more sample JobTasks for class {type.Key.FullName}",
            Timeout = 500,
            ErrorMailReceiver = dataJob.ErrorMailReceiver,
            SummaryMailReceiver = dataJob.SummaryMailReceiver,
            WriteToEventLog = true
        };

        foreach (var jobTask in type)
        {
            job.JobTasks.Add(jobTask);
        }

        var jsonFile = Path.Combine(dirNameJeSamples, $"{type.Key.Name.Replace(".", "_")}.json");

        var jh = new JobHandler
        {
            CurrentJob = job
        };

        var count = jh.CurrentJob.JobTasks.Count;
        jh.SaveJob(jsonFile);
        item.JsonContentFilename = jsonFile;

        jh.CurrentJob = null;
        jh.LoadJob(job, jsonFile);

        jsonFile = Path.Combine(dirNameJeDalSamples, $"{type.Key.Name.Replace(".", "_")}.xml");
        if (File.Exists(jsonFile)) File.Delete(jsonFile);

        var json = JsonHelper.SerializeObject(jh.CurrentJob).Replace("  ", "\t");

        Debug.Print(json);

        var xml = $"<?xml version=\"1.0\" encoding=\"UTF-8\"?><sample>{System.Security.SecurityElement.Escape(json)}</sample>";

        var xdoc = new XmlDocument();
        xdoc.LoadXml(xml);
        xdoc.Save(jsonFile);

        ArgumentNullException.ThrowIfNull(jh.CurrentJob);

        Assert.That(count == jh.CurrentJob.JobTasks.Count);
        Assert.That(File.Exists(jsonFile));
    }


    //[Test]
    //public void Test_Test()
    //{
    //    var json = @"\\bododata\software\Jobs\VermoegenDB_Kurz\VermoegenDB_Kurz.json";

    //    var jh = new JobHandler();
    //    jh.Mailers.Add(new FakeMailer());
    //    jh.LoadJob(json);

    //    Assert.That(jh.CurrentJob != null);
    //}

}