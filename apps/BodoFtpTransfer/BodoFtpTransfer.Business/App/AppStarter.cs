using BodoFtpTransfer.Business.Services;

namespace BodoFtpTransfer.Business.App
{
    public static class AppStarter
    {
        public static void Start(in int modus, StatusHandler statusHandler)
        {

            
            var appSettings = GlobalValues.CurrentAppSettings;

            var ftp = new FtpTransferService(appSettings.Credentials, statusHandler)
            {
                BaseDirectory = appSettings.BaseDirectory,
                ExcludedFiles = appSettings.ExcludedFiles,
                ExcludeDirs = appSettings.ExcludeDirs,
                RemoteDirectory = appSettings.RemoteDirectory,
                Modus = modus,
            };

            ftp.CreateBatch();

        }
    }
}