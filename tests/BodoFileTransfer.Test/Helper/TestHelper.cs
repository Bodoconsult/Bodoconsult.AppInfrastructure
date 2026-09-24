// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Bodoconsult.App.Helpers;
using Bodoconsult.Web.Mail.Helpers;
using Bodoconsult.Web.Mail.Models;
using BodoFileTransfer.Business.DataHandling;
using BodoFileTransfer.Business.Interfaces;
using BodoFileTransfer.Business.Model;
using BodoFileTransfer.Test.App;

namespace BodoFileTransfer.Test.Helper;

internal class TestHelper
{
    private static PwdKeys pwdKeys;

    public static readonly string SecretsPath = "c:\\Daten\\Projekte\\_work\\Data\\";

    public static readonly string TestData;

    static TestHelper()
    {

        var path = new DirectoryInfo( new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName).Parent.Parent.Parent.FullName;

        TestData =Path.Combine(path, "TestData");

        //var fileName = Path.Combine(SecretsPath, "mail.json");

        //pwdKeys = JsonHelper.LoadJsonFile<PwdKeys>(fileName);

        //PasswordHandler.Key1 = pwdKeys.Key1;
        //PasswordHandler.Key2 = pwdKeys.Key2;
        //PasswordHandler.Key3 = pwdKeys.Key3;
        //PasswordHandler.Salt = pwdKeys.Salt;

        //Debug.Print(GetStringFromArrayCsharpStyle(PasswordHandler.Salt));
    }


    /// <summary>
    /// Encrypt a string
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string EncryptIt(string text)
    {
        return PasswordHandler.Encrypt(text);
    }


    public static DummyDataHandler GetDummyDataHandler()
    {
        var d = new DummyDataHandler(Globals.Instance.AppStartParameter.ErrorMailAddress);
        var a = CreateDummyAccount(d.Accounts, "M55048");
        CreateDummyAccount(d.Accounts, "M55021");

        d.ImapAccounts.Add(GetImapAccount(d, a));

        return d;
    }

    /// <summary>
    /// Create an dummy account with all required data
    /// </summary>
    /// <param name="accounts"></param>
    /// <param name="keyword"></param>
    /// <returns></returns>
    public static Account CreateDummyAccount(IList<Account> accounts, string keyword)
    {
        var a = new Account
        {
            Id = accounts.Count+1,
            Keyword = keyword,
            InboxPath = $@"d:\tmp\BodoFileTransfer\{keyword}\Inbox",
            ArchivePath = $@"d:\tmp\BodoFileTransfer\{keyword}\Archive",
            Name = "DummyAccount"
        };

        // Create dirs
        if (!Directory.Exists(a.InboxPath))
        {
            Directory.CreateDirectory(a.InboxPath);
        }

        if (!Directory.Exists(a.ArchivePath))
        {
            Directory.CreateDirectory(a.ArchivePath);
        }

        // Create dummy files
        var path = $"Test1{DateTime.Now:yyyyMMddhhmm}.txt";
        path = Path.Combine(a.InboxPath, path);
        File.WriteAllText(path, "Test1!");

        path = $"Test2{DateTime.Now:yyyyMMddhhmm}.txt";
        path = Path.Combine(a.InboxPath, path);
        File.WriteAllText(path, "Test2!");

        accounts.Add(a);

        return a;
    }


    public static ImapAccount GetTestImapSettings()
    {

        var fileName = Path.Combine(SecretsPath, "MailDownloader.json");

        var settings = JsonHelper.LoadJsonFile<ImapAccount>(fileName);


        var result = new ImapAccount(null)
        {
            FileExtensionFilter = settings.FileExtensionFilter,
            Host = settings.Host,
            Password = PasswordHandler.Decrypt(settings.Password),
            UserName = PasswordHandler.Decrypt(settings.UserName),
            Port = settings.Port,
            Name = settings.Name,
            SentDateFilter = settings.SentDateFilter,
            ServerDateFormat = settings.ServerDateFormat,
            SubjectFilter = settings.SubjectFilter,
            UseSsl = settings.UseSsl
        };

        return result;
    }


    public static ImapAccount GetImapAccount(IDataHandler dataHandler, Account account)
    {

        var imap = GetTestImapSettings();

        var settings = new ImapAccount(dataHandler)
        {
            Name = imap.Name,
            Host = imap.Host,
            Port = imap.Port,
            UserName = imap.UserName,
            Password = imap.Password,
            FileExtensionFilter = ".pdf;.zip",
            SubjectFilter = "Rechnung,Invoice",
            Id = 1,
            AccountId = 1,
            InboxPath = account.InboxPath
        };

        return settings;
    }

    public static void Status(string msg)
    {
        Debug.Print(msg);
    }

    public static FolderHandlerConfig GetDefaultConfig()
    {
        return new(Globals.Instance)
        {
            NumberSchema = "000000",
            SignatureFileExtensions = ".ads;.cms",
            SentDateFilter = DateTime.Now.AddDays(-70)
        };
    }

    /// <summary>
    /// Get a new document with a near-random name
    /// </summary>
    /// <returns>Document entity</returns>
    public static DocumentFile GetNewDocument()
    {
        var now = DateTime.Now.ToString("yyyyMMddhhmmssffff");

        return new DocumentFile
        {
            AccountId = 1,
            Keyword = "Beleg",
            Path = $@"D:\temp\test{now}.pdf",
            Title = $"Steuerbescheinigung 2021 {now}"
        };
    }

    public static string GetStringFromArray(byte[] ba)
    {
        var value = "";
        foreach (var b in ba)
        {
            if (b <= 33 || b >= 127)
            {
                value += $"[{b:X2}]";
            }
            else
            {
                value += Convert.ToChar(b);
            }
        }
        return value;
    }

    /// <summary>
    /// Get a string from a byte array in C# style (for copying it to unit tests)
    /// </summary>
    /// <param name="data">Byte array</param>
    /// <returns>Byte array as string</returns>
    public static string GetStringFromArrayCsharpStyle(byte[] data)
    {

        var result = new StringBuilder();

        result.AppendLine(GetStringFromArray(data));

        result.Append("{ ");

        foreach (var b in data)
        {
            result.Append($"0x{b:x}, ");
        }

        var s = result.ToString();

        return s.EndsWith(", ", StringComparison.OrdinalIgnoreCase) ?
            $"{s.Substring(0, s.Length - 2)} }}" :
            $"{s} }}";
    }

    public static O365SenderAccount GetTestO365Account()
    {
        //Debug.Print(PasswordHandler.Key1);
        //Debug.Print(PasswordHandler.Key2);
        //Debug.Print(PasswordHandler.Key3);

        var fileName = Path.Combine(SecretsPath, "O365Mailer.json");

        var account = JsonHelper.LoadJsonFile<O365MailAccount>(fileName);

        var o365 = new O365SenderAccount
        {
            Tenant = PasswordHandler.Decrypt(account.Tenant),
            Instance = PasswordHandler.Decrypt(account.Instance),
            ClientId = PasswordHandler.Decrypt(account.ClientId),
            ClientSecret = PasswordHandler.Decrypt(account.ClientSecret),
            Scope = PasswordHandler.Decrypt(account.Scope),
            UserName = PasswordHandler.Decrypt(account.UserName)
        };

        return o365;
    }
}