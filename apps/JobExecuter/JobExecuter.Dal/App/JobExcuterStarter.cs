// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using JobExecuter.Dal.Helpers;
using System;
using System.IO;
using System.Text;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Interfaces;
using Bodoconsult.App.Windows.ConsoleTools;
using Bodoconsult.App.Windows.Helpers;
using JobExecuter.Dal.JobMailer;
using JobExecuter.Dal.Model;

namespace JobExecuter.Dal.App;

/// <summary>
/// Console starter for JobExecuter app
/// </summary>
public class JobExcuterStarter: BaseConsoleStarter
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="args">Command line args</param>
    public JobExcuterStarter(string[] args): base("JobExecuter", args)
    {
        DiskShadowHelper.TempDir = DataPath ?? Path.GetTempPath();
        MainAppRunnerDelegate = StartInternal;
    }

    /// <summary>
    /// Load plug ins
    /// </summary>
#pragma warning disable CA1822
    public void LoadPlugIns()
#pragma warning restore CA1822
    {
        // Load plug-ins first to them ready to for crypting etc.
        SetStatus("Loading plug-ins...");
        try
        {
            PlugInHelper.LoadPlugIns();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error:{e}");
        }
    }

    /// <summary>
    /// Do the requested start action
    /// </summary>
    public int StartInternal(IConsoleStarter consoleStarter)
    {
        if (consoleStarter.Args == null || consoleStarter.Args.Length == 0)
        {
            ShowHelp();
            return 0;
        }

        // Hash loaded? If no create one
        if (string.IsNullOrEmpty(consoleStarter.AppPasswordHash))
        {
            consoleStarter.UiHashAppPassword();
            return 0;
        }

        // Command line arg is a filename
        if (!consoleStarter.Args[0].StartsWith("/", StringComparison.OrdinalIgnoreCase) && !consoleStarter.Args[0].StartsWith("--", StringComparison.OrdinalIgnoreCase))
        {
            return RunJobTask(consoleStarter.Args[0]);
        }

        // Run command line args
        switch (consoleStarter.Args[0])
        {
            //case "/p": // Ask user for password, encrypt it and copy it to clipboard
            //    //ConsoleHelper.EncryptPassword();
            //    return 0;
            case "/c":  // Encrypt a JSON file and save it as CJSON
            case "--c":
                UiCryptJson(consoleStarter.Args[1]);
                return 0;
            //case "/s":
            //    CreateSalt();
            //    return 0;
            case "/j":
            case "--j":
                UiHashAppPassword();
                return 0;
            case "/m":
            case "--m":
                SendTestMail();
                return 0;
            case "/t":
            case "--t":
                CheckJson(consoleStarter.Args[1]);
                return 0;
            case "/?":
            case "--?":
                ShowHelp();
                return 0;
            default:
                ShowHelp();
                return 0;
        }
    }

    /// <summary>
    /// Run a job task
    /// </summary>
    /// <param name="cjsonFile">Filename of a CJSON file with the job task definition</param>
    /// <returns>Error code. If 0 job was executed without error</returns>
    public int RunJobTask(string cjsonFile)
    {
        var fi = new FileInfo(cjsonFile);

        if (!fi.Exists)
        {
            return 1;
        }

        if (!fi.Extension.Equals(".cjson", StringComparison.OrdinalIgnoreCase))
        {
            const string msg = "Job file is no CJSON file! The job will not be started!";
            Console.WriteLine(msg);
            Console.ReadLine();
            return 1;
        }

        var logFile = fi.FullName.Replace(fi.Extension, ".log", StringComparison.OrdinalIgnoreCase);
        SetStatus($"logFile: {logFile}");

        var jh = new JobHandler();
        jh.Mailers.Add(new ConsoleJobMailer());
        jh.Mailers.Add(new FileJobMailer
        {
            LogFileName = logFile
        });

        if (SmtpMailAccount != null && !string.IsNullOrEmpty(SmtpMailAccount.SmtpServer))
        {
            SetStatus("Loading SMTP mailer...");

            var smtp = new SmtpJobMailer(SmtpMailAccount);
            smtp.LoadDefaultHtmlTemplate();

            jh.Mailers.Add(smtp);
        }

        SetStatus("Loading job file...");
        var job = DecryptJson(cjsonFile);
        jh.LoadJob(job, cjsonFile);

        if (jh.CurrentJob.WriteToEventLog)
        {
            SetStatus("Add eventlog handling...");
            jh.Mailers.Add(new EventlogJobMailer());
        }

        SetStatus("Start processing job file...");
        jh.RunJob();

        return 0;
    }

    /// <summary>
    /// Crypt a JSON file (extension .json) to a crypted JSON file (.cjson)
    /// </summary>
    /// <param name="jsonFile">JSON file to crypt</param>
    public void UiCryptJson(string jsonFile)
    {
        if (!UiValidateAppPassword())
        {
            return;
        }

        CryptJson(jsonFile);

        ConsoleHelper.WaitUntilAnyKeyPressed("Crypting was successful!");
    }

    /// <summary>
    /// Crypt a JSON file (extension .json) to a crypted JSON file (.cjson)
    /// </summary>
    /// <param name="jsonFile">JSON file to crypt</param>
    public void CryptJson(string jsonFile)
    {
        if (!IsAppPasswordHashChecked)
        {
            return;
        }

        try
        {
            var cjsonFile = jsonFile.Replace(".json", ".cjson");

            const string key = "JSON";

            var job = JsonHelper.LoadJsonFile<Job>(jsonFile);

            SaveCryptedJson(job, key, cjsonFile);
        }
        catch (Exception ex)
        {
            ConsoleHelper.WaitUntilAnyKeyPressed($"Crypting was NOT successful: {ex}");
        }
    }

    /// <summary>
    /// Decrypt a JSON file (extension .json) to a crypted JSON file (.cjson)
    /// </summary>
    /// <param name="cjsonFile">JSON file to crypt</param>
    public Job DecryptJson(string cjsonFile)
    {
        try
        {
            var job = LoadCryptedJson<Job>("JSON", cjsonFile);
            return job;
        }
        catch (Exception e)
        {
            ConsoleHelper.WaitUntilAnyKeyPressed($"Decrypting job file was NOT successful: {e}");
            return null;
        }
    }

    /// <summary>
    /// Show a help for the program
    /// </summary>
    public static void ShowHelp()
    {
        var result = HelpHelper.GetHelp();
        Console.Write(result);
        ConsoleHelper.WaitUntilAnyKeyPressed();
    }

    /// <summary>
    /// Check if a JSON file is a valid JobExecuter job
    /// </summary>
    /// <param name="fileName">JSON file to check</param>
    public static void CheckJson(string fileName)
    {
        try
        {
            _ = SimpleJsonHelper.LoadJsonFile<Job>(fileName);
            ConsoleHelper.WaitUntilAnyKeyPressed("Testing JSON was successful. JSON structure is okay!");
        }
        catch (Exception e)
        {
            ConsoleHelper.WaitUntilAnyKeyPressed($"Error while testing JSON structure. JSON structure is NOT okay!\r\n{e}");
        }
    }

    /// <summary>
    /// Send an SMTP test mail
    /// </summary>
    public void SendTestMail()
    {
        try
        {
            var mail = ConsoleHelper.AskUserForStringValue("Insert email address to send the test mail to:");
            if (string.IsNullOrEmpty(mail))
            {
                return;
            }

            if (O365MailAccount != null)
            {
                var o365 = new O365JobMailer(O365MailAccount);
                o365.LoadDefaultHtmlTemplate();
                o365.SendDummyMail(mail);
            }
            else
            {
                var smtp = new SmtpJobMailer(SmtpMailAccount);
                smtp.LoadDefaultHtmlTemplate();
                smtp.SendDummyMail(mail);
            }

            ConsoleHelper.WaitUntilAnyKeyPressed("Test mail sent successfully!");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WaitUntilAnyKeyPressed($"Sending test mail not possible: {ex.Message}");
        }
    }

    /// <summary>
    /// Save an entity in a crypted JSON file
    /// </summary>
    /// <typeparam name="T">Type of the entity to store</typeparam>
    /// <param name="entityToStore">Entity to store</param>
    /// <param name="key">Key</param>
    /// <param name="fileName">Filename to save the crypted JSON string in</param>
    public void SaveCryptedJson<T>(T entityToStore, string key, string fileName)
    {
        var json = JsonHelper.SerializeObject(entityToStore);

        var cjson = DataProtectionService.Protect(key, json);

        File.WriteAllText(fileName, cjson, Encoding.UTF8);
    }

    /// <summary>
    /// Load a crypted json file
    /// </summary>
    /// <typeparam name="T">Type of the entity to deserialize from the JSON file</typeparam>
    /// <param name="key">Key</param>
    /// <param name="cjsonFile">Fillename of the crypted JSON file</param>
    /// <returns>Deserialized entity</returns>
    public T LoadCryptedJson<T>(string key, string cjsonFile)
    {
        var raw = File.ReadAllText(cjsonFile);

        var json = DataProtectionService.Unprotect(key, raw);

        var job = JsonHelper.LoadJson<T>(json);
        return job;
    }
}