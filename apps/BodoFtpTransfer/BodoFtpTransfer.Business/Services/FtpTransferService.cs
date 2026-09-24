using System;
using System.IO;
using System.Reflection;
using Bodoconsult.Web.Ftp.Models;
using log4net;

namespace BodoFtpTransfer.Business.Services
{
    // Status-Event
    public delegate void StatusHandler(string msg);

    public class FtpTransferService
    {
        private readonly string _batch;
        private readonly FtpBatchService _ftpBatch;

        private readonly ILog _logger = LogManager.GetLogger(nameof(FtpTransferService));

        public FtpTransferService(SshCredentials credentials, StatusHandler statusChange)
        {
            SetMessage("Starting FtpTransfer...");
            StatusChange = statusChange;
            _ftpBatch = new FtpBatchService(credentials, StatusChange);
            Modus = 0;

            var fileName = Assembly.GetExecutingAssembly().Location;
            if (fileName == null)
            {
                throw new Exception("No application directory found (1)");
            }

            var curDir = new FileInfo(fileName).DirectoryName;
            if (curDir != null) _batch = Path.Combine(curDir, "ftp.txt");
        }


        // Event Status-Änderung
        public StatusHandler StatusChange { get; }

        /// <summary>
        /// Übertragungsmodus
        /// 0 = übertragen
        /// 1 = alle Dateien und Verzeichnisse als bereits übertragen kennzeichnen
        /// </summary>
        public int Modus { get; set; }

        public string ExcludeDirs { get; set; }


        private string _excludedFiles;

        public string ExcludedFiles
        {
            get => _excludedFiles;
            set => _excludedFiles = value.ToLower();
        }

        private string _remoteDir;

        public string RemoteDirectory
        {
            get => _remoteDir;
            set => _remoteDir = value ?? "";
        }


        private int _baseLen;
        private string _baseDir;
        /// <summary>
        /// Zu übertragendes Verzeichnis
        /// </summary>
        public string BaseDirectory
        {
            get => _baseDir;
            set
            {
                _baseDir = value;
                _baseLen = value.Length;
            }
        }

        /// <summary>
        /// Batch-Datei für FTP-Transfer erzeugen
        /// </summary>
        public void CreateBatch()
        {
            SetMessage("Update database...");
            _ftpBatch.BaseDir = _baseDir;
            _ftpBatch.FileName = _batch;
            _ftpBatch.RemoteDirectory = _remoteDir;
            _ftpBatch.ExcludedFiles = _excludedFiles;
            _ftpBatch.ExcludeDirs = ExcludeDirs;
            _ftpBatch.Open();

            //// Ermittle remote vorhandene Dateien und Ordner
            _ftpBatch.LoadAllFilePaths();
            _ftpBatch.StartCommandCollecting();
            _ftpBatch.GetRemoteData(
                $"{_remoteDir}{_baseDir.Substring(_baseLen, _baseDir.Length - _baseLen).Replace(@"\", "/")}/");
            SetMessage("Save results to database...");
            _ftpBatch.RunCommandsFromCollection();

            //// Ermittle lokal vorhandene Dateien und Ordner
            _ftpBatch.LoadAllFilePaths();
            _ftpBatch.StartCommandCollecting();
            _ftpBatch.GetLocalData(new DirectoryInfo(_baseDir));
            SetMessage("Save results to database...");
            _ftpBatch.RunCommandsFromCollection();

            // Ermittle Hashcodes für Dateien, um Änderungen gegenüber letzter Übertragung zu erkennen
            _ftpBatch.GetHashCodes();

            _ftpBatch.SetzeHashcodesFürRemoteFiles();

            _ftpBatch.Open();

            // Neue Daten auf Webseite übertragen
            SetMessage("Transfer new or updated data to website...");
            _ftpBatch.CreateRemoteDirectory();
            _ftpBatch.CopyLocalFiles();

            // Veraltete Daten löschen
            SetMessage("Transfer old data from website...");
            _ftpBatch.RemoveRemoteFiles();
            _ftpBatch.RemoveRemoteDirectory();


            _ftpBatch.Quit();

            SetMessage("Transfer done!");
        }


        /// <summary>
        /// Status-Nachricht setzen
        /// </summary>
        /// <param name="msg">Anzuzeigende Nachricht</param>
        private void SetMessage(string msg)
        {
            _logger.Info(msg);
            if (StatusChange != null) StatusChange(msg);

        }

    }
}