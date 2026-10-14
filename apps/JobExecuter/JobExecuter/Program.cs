// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using JobExecuter.Dal.App;

namespace JobExecuter;

/// <summary>
/// Main application class of JobExecuter
/// </summary>
internal class Program
{
    /// <summary>
    /// Main routine of the JobExecuter  application
    /// </summary>
    /// <param name="args">
    /// <para>Diverse parameters described below</para>
    /// <para>JobExecuter.exe %JSON filename%  Runs JobExecuter with the job saved in the file %JSON filename%</para>
    /// 
    /// 
    /// </param>
    [STAThread]
    internal static int Main(string[] args)
    {
        //CryptJson(@"\\bododata\software$\Jobs\Test\Test.cjson");

        // /c D:\Tmp\job.json

        GlobalValues.LoadAppSettings();

        var appStarter = new JobExcuterStarter(args);
        appStarter.LoadSmtpConfig();
        appStarter.LoadPlugIns();
        appStarter.LoadAppPasswordHash();
        return appStarter.Start();
    }
}