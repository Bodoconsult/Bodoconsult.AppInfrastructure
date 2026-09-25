// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using BodoFtpTransfer.Business.App;

namespace BodoFtpTransfer;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {

            //XmlDocument log4netConfig = new XmlDocument();
            //log4netConfig.Load(File.OpenRead("log4net.config"));

            //var repo = LogManager.CreateRepository(
            //    Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            //XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

            //Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);



            var modus = 0;
            if (args.Length > 0)
            {
                modus = Convert.ToInt16(args[0]);
            }

            GlobalValues.LoadAppSettings();

            AppStarter.Start(modus);

            //            GlobalValues.LoadAppSettings();
            //            var appSettings = GlobalValues.CurrentAppSettings;

            //            var ftp = new FtpTransfer(appSettings.Credentials);
            //                ftp.StatusChange += SetMessage;



            //                //ftp.Url = Properties.Settings.Default.FTPUrl;
            //                //ftp.User = Properties.Settings.Default.FTPUsername;
            //                //ftp.Password = Properties.Settings.Default.FTPPassword;
            //                ftp.BaseDirectory = appSettings.BaseDirectory;
            //                ftp.ExcludedFiles = appSettings.ExcludedFiles;
            //                ftp.ExcludeDirs = appSettings.ExcludeDirs;
            //                ftp.RemoteDirectory = appSettings.RemoteDirectory;
            //                ftp.CreateBatch();
            //                //_FTP.Execute();
            //                //Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
#if DEBUG
            Console.ReadLine();
#endif
        }
    }

    /// <summary>
    /// Status-Nachricht setzen
    /// </summary>
    /// <param name="msg">Anzuzeigende Nachricht</param>
    private static void SetMessage(string msg)
    {
        Console.WriteLine(msg);
    }
}