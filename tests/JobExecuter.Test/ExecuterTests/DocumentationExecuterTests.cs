// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.IO;
using Bodoconsult.Test.Helpers;
using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class DocumentationExecuterTests
{
    private readonly string _localPath = TestDataHelper.TempPath;

    [Test]
    public void TestDocu()
    {

        var docuTotalFile = Path.Combine(_localPath, "documentation.htm");
        if (File.Exists(docuTotalFile)) File.Delete(docuTotalFile);

        var job = new Job
        {
            Name = "Sample job for SqlServer backup of a single database",
            Description = "This job first saves a SQL database to a local path " +
                          "and then moves the backup file to a network location.",
            Timeout = 120
        };

        var docuSqlFile = Path.Combine(_localPath, "sql_docu.htm");
        var jsonSqlFile = Path.Combine(_localPath, "sql_docu.json");

        var sd = new SqlDocuSingleDatabaseJobTask
        {
            Name = "Create a documentation for a single database",
            Server = ".\\sqlexpress",
            DatabaseName = "MediaDb",
            HtmlTargetFileName = docuSqlFile,
            JsonTargetFileName = jsonSqlFile,
            Timeout = 600
        };

        job.JobTasks.Add(sd);

        var docuTsFile = Path.Combine(_localPath, "ts_docu.html");
        var jsonTsFile = Path.Combine(_localPath, "ts_docu.json");

        var ts = new TaskSchedulerDocuJobTask
        {
            Name = "Getting task scheduler data from local computer",
            XmlTargetPath = Path.Combine(_localPath, "Xml"),
            HtmlTargetFileName = docuTsFile,
            JsonTargetFileName = jsonTsFile
        };

        job.JobTasks.Add(ts);

        var jsonJobFile = Path.Combine(_localPath, "job.json");
        var docuJobFile = Path.Combine(_localPath, "job_docu.json");

        var jd = new JobDocuJobTask
        {
            Name = "Document the current JobExecuter job",
            Description = "If JobJsonFileName and DocuJsonFileName left empty, JobExecuter will insert current job file path and as docu file name the current job file path with the suffix _docu.json",
            JsonTargetFileName = docuJobFile,
            JobJsonFileName = jsonJobFile,
        };
        job.JobTasks.Add(jd);

        if (File.Exists(docuSqlFile)) File.Delete(docuSqlFile);
        if (File.Exists(jsonSqlFile)) File.Delete(jsonSqlFile);
        if (File.Exists(docuTsFile)) File.Delete(docuTsFile);
        if (File.Exists(jsonTsFile)) File.Delete(jsonTsFile);
        if (File.Exists(docuJobFile)) File.Delete(docuJobFile);

        var doc = new DocumentationJobTask
        {
            Title = "Gesamtdokumentation",

        };
        doc.HtmlTargetFiles.Add(docuTotalFile);

        doc.JsonFiles.Add(jsonTsFile);
        doc.JsonFiles.Add(jsonSqlFile);
        doc.JsonFiles.Add(docuJobFile);
        //doc.JsonFiles.Add(jsonSqlDocuFile);

        job.JobTasks.Add(doc);

        var jh = new JobHandler
        {
            CurrentJob = job
        };

        jh.Mailers.Add(new FakeJobMailer());

        // Create job json file
        jh.SaveJob(jsonJobFile);


        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(File.Exists(docuSqlFile));
        Assert.That(File.Exists(docuTotalFile));

        TestHelper.OpenFile(docuTotalFile);
    }

    [Test]
    public void TestDocuBodoconsult()
    {

        var docuTotalFile = Path.Combine(_localPath, "Bcg.htm");
        if (File.Exists(docuTotalFile)) File.Delete(docuTotalFile);

        var job = new Job
        {
            Name = "Sample job for SqlServer backup of a single database",
            Description = "This job first saves a SQL database to a local path " +
                          "and then moves the backup file to a network location.",
            Timeout = 120
        };

        var doc = new DocumentationJobTask
        {
            Title = "Gesamtdokumentation",

        };
        doc.HtmlTargetFiles.Add(docuTotalFile);

        doc.JsonFiles.Add("\\\\BCGS04AP\\software$\\Jobs\\CMS\\job_docu.json");
        doc.JsonFiles.Add("\\\\BCGS04AP\\software$\\Jobs\\CMS_Bildarchiv\\job_docu.json");
        doc.JsonFiles.Add("\\\\BCGS04AP\\software$\\Jobs\\Faktura_Archiv\\job_docu.json");
        doc.JsonFiles.Add("\\\\BCGS04AP\\software$\\Jobs\\Faktura_Buchungscheck\\job_docu.json");
        doc.JsonFiles.Add("\\\\BCGS04AP\\software$\\Jobs\\Faktura_Erinnerung\\job_docu.json");

        doc.JsonFiles.Add("\\\\BCGS04AP\\software$\\Jobs\\Faktura_Monatsabschluss\\job_docu.json");
        doc.JsonFiles.Add("\\\\BCGS04AP\\software$\\Jobs\\Faktura_Regelbuchungen\\job_docu.json");
        doc.JsonFiles.Add("\\\\BCGS04AP\\software$\\Jobs\\Faktura_Erinnerung\\job_docu.json");
        doc.JsonFiles.Add("\\\\BCGS04AP\\software$\\Jobs\\Network\\job_docu.json");
        doc.JsonFiles.Add("\\\\BCGS04AP\\software$\\Jobs\\VermoegenDB_Kurz\\job_docu.json");
        doc.JsonFiles.Add("\\\\BCGS04AP\\software$\\Jobs\\VermoegenDB_Simulation\\job_docu.json");
        doc.JsonFiles.Add("\\\\BCGS04AP\\software$\\Jobs\\VermoegenDB_Voll\\job_docu.json");

        job.JobTasks.Add(doc);


        var jh = new JobHandler
        {
            CurrentJob = job
        };

        jh.Mailers.Add(new FakeJobMailer());

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
        Assert.That(File.Exists(docuTotalFile));

        TestHelper.OpenFile(docuTotalFile);
    }
}