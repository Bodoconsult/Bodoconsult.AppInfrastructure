namespace JobExecuterCore.Test
{
    [TestFixture]
    public class UnitTestSqlMailUserAccessAllDatabasesExecuter
    {

        [Test]
        public void TestSendMail()
        {

            var job = new Job
            {
                Name = "Sample job for SqlServer documentation of a all databases with exception of databases Master, Msdb, Model and TempDb",
                Description = "This job first saves SQL databases to a local path " +
                              "and then moves the backup file to a network location.",
                Timeout = 120
            };

            var sma = new SqlMailUserAccessAllDatabasesJobTask
            {
                Name = "Create and send an email with user access information for all databases on a SqlServer with exception of databases Master, Msdb, Model and TempDb",
                Server = ".\\sqlexpress2",
                MailTo = "robert.leisner@bodoconsult.de",
                Timeout = 60
            };

            job.JobTasks.Add(sma);

            var jh = new JobHandler
            {
                CurrentJob = job
            };


            var s = new SmtpMailer
            {
                SmtpServer = "smtp.1und1.de",
                From = "administrator@bodoconsult.de",
                Username = "test@bodoconsult.de",
                Password = "Test123!"
            };
            s.LoadDefaultHtmlTemplate();

            jh.Mailers.Add(new FakeMailer());
            jh.Mailers.Add(s);

            var result = jh.RunJob();

            Assert.IsTrue(result);
            Assert.IsTrue(jh.ErrorItems.Count == 0);
        }
    }
}