// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System.Diagnostics;
using System.IO;
using System.Reflection;
using Bodoconsult.App.Helpers;
using Bodoconsult.Web.Mail.Helpers;
using Bodoconsult.Web.Mail.Models;
using NUnit.Framework;

namespace Bodoconsult.Web.Mail.Test.Helpers;

public static class TestHelper
{
    private static string _testDataPath;

    private static readonly string SecretsPath = "c:\\Daten\\Projekte\\_work\\Data\\";

    public static string TempPath = @"c:\temp\";


    static TestHelper()
    {
        var fileName = Path.Combine(SecretsPath, "mail.json");

        //pwdKeys = JsonHelper.LoadJsonFile<PwdKeys>(fileName);

        //PasswordHandler.Key1 = pwdKeys.Key1;
        //PasswordHandler.Key2 = pwdKeys.Key2;
        //PasswordHandler.Key3 = pwdKeys.Key3;
        //PasswordHandler.Salt = pwdKeys.Salt;
    }


    public static string TestDataPath
    {
        get
        {
            if (!string.IsNullOrEmpty(_testDataPath))
            {
                return _testDataPath;
            }

            var path = new DirectoryInfo(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName).Parent.Parent.Parent.FullName;

            _testDataPath = Path.Combine(path, "TestData");

            if (!Directory.Exists(_testDataPath))
            {
                Directory.CreateDirectory(_testDataPath);
            }

            return _testDataPath;
        }
    }

    /// <summary>
    /// Start an app by file name
    /// </summary>
    /// <param name="fileName"></param>
    public static void StartFile(string fileName)
    {
        if (!Debugger.IsAttached)
        {
            return;
        }

        if (string.IsNullOrEmpty(fileName))
        {
            return;
        }

        Assert.That(File.Exists(fileName));

        var p = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                UseShellExecute = true, 
                FileName = fileName
            }
        };

        p.Start();
    }

    /// <summary>
    /// Get a test mail account. Adjust path to your current situation
    /// </summary>
    /// <returns></returns>
    public static SmtpMailAccount GetTestMailAccount()
    {
        var fileName = Path.Combine(SecretsPath, "BodoWebMailer.json");
        var account = JsonHelper.LoadJsonFile<SmtpMailAccount>(fileName);
        return account;
    }

    /// <summary>
    /// Get a test mail receiver. Adjust path to your current situation
    /// </summary>
    /// <returns></returns>
    public static string GetTestReceiver()
    {
        var fileName = Path.Combine(SecretsPath, "TestMailReceiver.txt");
        var account = File.ReadAllText(fileName);
        return account;
    }

    public static O365MailAccount GetTestO365Account()
    {
        var fileName = Path.Combine(SecretsPath, "O365Mailer1.json");

        var account = JsonHelper.LoadJsonFile<O365MailAccount>(fileName);

        //Debug.Print(PasswordHandler.Key1);
        //Debug.Print(PasswordHandler.Key2);
        //Debug.Print(PasswordHandler.Key3);

        var o365 = new O365MailAccount
        {
            Tenant = PasswordHandler.Decrypt(account.Tenant),
            Instance = PasswordHandler.Decrypt(account.Instance),
            ClientId = PasswordHandler.Decrypt(account.ClientId),
            ClientSecret = PasswordHandler.Decrypt(account.ClientSecret),
            Scope = PasswordHandler.Decrypt(account.Scope),
            UserName = PasswordHandler.Decrypt(account.UserName)
        };

        //o365.UserName = "noreply@bodoconsult.de";

        return o365;
    }
}