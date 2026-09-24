using Bodoconsult.Web.Ftp.Models;

namespace BodoFtpTransfer.Business.Model
{

    /// <summary>
    /// App settings class
    /// </summary>
    public class AppSettings
    {

        public SshCredentials Credentials { get; set; }

        public string ExcludeDirs { get; set; }

        public string ExcludedFiles { get; set; }

        /// <summary>
        /// Remote directory relative to Credentials.Url
        /// </summary>
        public string RemoteDirectory { get; set; }

        /// <summary>
        /// Zu übertragendes lokales Verzeichnis
        /// </summary>
        public string BaseDirectory { get; set; }

    }
}
