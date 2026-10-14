
// ReSharper disable InconsistentNaming

using JobExecuter.Dal;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;
using JobExecuter.Test.Helpers;
using NUnit.Framework;

namespace JobExecuter.Test.ExecuterTests;

[TestFixture]
public class SqlBackupAllDatabasesExecuterTests
{
    private readonly string localPath = TestDataHelper.LocalPath;
    private readonly string targetPath = TestDataHelper.LocalPath;
        
    private const string fileName = "??server??_??database??.bak";

    [Test]
    public void TestBackup_OnlyLocal()
    {

        var job = new Job
        {
            Name = "Sample job for SqlServer backup of all databases",
            Description = "This job first saves a SQL database to a local path " +
                          "and then moves the backup file to a network location.",
            Timeout = 120
        };


        var jt = new SqlBackupAllDatabasesJobTask
        {
            Server = ".\\sqlexpress",
            LocalTarget = localPath,
            FileName = fileName,
            Timeout = 60
        };

        job.JobTasks.Add(jt);

        var jh = new JobHandler
        {
            CurrentJob = job
        };
            
        jh.Mailers.Add(new FakeJobMailer());

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
            
    }

    [Test]
    public void TestBackup_ToNetwork()
    {

        var job = new Job
        {
            Name = "Sample job for SqlServer backup of a all databases",
            Description = "This job first saves a SQL database to a local path " +
                          "and then moves the backup file to a network location.",
            Timeout = 120
        };


        var jt = new SqlBackupAllDatabasesJobTask
        {
            Server = ".\\sqlexpress",
            LocalTarget = localPath,
            NetworkTarget = targetPath,
            FileName = fileName,
            Timeout = 60
        };

        job.JobTasks.Add(jt);

        var jh = new JobHandler
        {
            CurrentJob = job
        };
            
        jh.Mailers.Add(new FakeJobMailer());

        var result = jh.RunJob();

        Assert.That(result);
        Assert.That(jh.ErrorItems.Count == 0);
            
    }

 
    //ToDo: test with placeholders in folder path


}