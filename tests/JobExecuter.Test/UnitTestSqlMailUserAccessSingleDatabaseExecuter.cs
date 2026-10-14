using NUnit.Framework;
using JobExecuterCore.Dal.Model;

namespace JobExecuterCore.Test
{
    [TestFixture]
    public class UnitTestSqlMailUserAccessSingleDatabaseExecuter
    {


        [Test]
        public void TestSendMail()
        {



            var job = new Job
            {
                Name = "Sample job for SqlServer backup of a single database",
                Description = "This job first saves a SQL database to a local path " +
                              "and then moves the backup file to a network location.",
                Timeout = 120
            };



            var sms = new SqlMailUserAccessSingleDatabaseJobTask
            {
                Name = "Create and send an email with user access information for a single database",
                Server = ".\\sqlexpress2",
                DatabaseName = "MediaDb",
                MailTo = "robert.leisner@bodoconsult.de",
                Timeout = 60
            };

            job.JobTasks.Add(sms);

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