// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DataProtection.Hashing;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Interfaces;

namespace Bodoconsult.App.Windows.ConsoleTools;

/// <summary>
/// Base class for console app starters
/// </summary>
public abstract class BaseConsoleStarter : IConsoleStarter
{
    private const string KeyNameWebCredentials = "WEB";
    private const string KeyNameSmtp = "SMTP";
    private const string KeyNameO365 = "O365";

    /// <summary>
    /// The current app of the app
    /// </summary>
    protected readonly string? AppPath;

    /// <summary>
    /// The current path in C:\ProgrammData for the data of the app
    /// </summary>
    protected readonly string? DataPath;

    /// <summary>
    /// Is the app password already checked
    /// </summary>
    protected bool IsAppPasswordHashChecked;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appName">App name</param>
    /// <param name="args">Args the app was started with</param>
    protected BaseConsoleStarter(string appName, string[] args)
    {
        DataProtectionService = DataProtection.DataProtectionService.CreateInstance(appName, DataProtectionScope.LocalMachine);
        Args = args;

        AppPath = Environment.ProcessPath;
        ArgumentNullException.ThrowIfNull(AppPath);

        AppPath = new FileInfo(AppPath).DirectoryName;
        ArgumentNullException.ThrowIfNull(AppPath);

        DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), appName);

        if (!Directory.Exists(DataPath))
        {
            Directory.CreateDirectory(DataPath);
        }

        HashFileName = Path.Combine(DataPath, "data0.dat");

        SmtpMailAccountFileName = Path.Combine(AppPath, "data1.dat");
        SmtpMailAccountImportFileName = Path.Combine(AppPath, "smtp.json");

        O365MailAccountFileName = Path.Combine(AppPath, "data2.dat");
        O365MailAccountImportFileName = Path.Combine(AppPath, "0365.json");

        WebCredentialsFileName = Path.Combine(AppPath, "data3.dat");
        WebCredentialsImportFileName = Path.Combine(AppPath, "web.json");
    }

    /// <summary>
    /// Current <see cref="Bodoconsult.App.DataProtection.Hashing.Hasher"/> instance for hashing data
    /// </summary>
    public Hasher Hasher { get; set; } = new();

    /// <summary>
    /// Current data protection service instance to use for protecting secrets
    /// </summary>
    public IDataProtectionService DataProtectionService { get; set; }

    /// <summary>
    /// A delegate for running the main app thread from the starter
    /// </summary>
    public MainAppRunnerDelegate? MainAppRunnerDelegate { get; set; }

    /// <summary>
    /// Command line args
    /// </summary>
    public string[]? Args { get; }

    /// <summary>
    /// Filename of the SMTP mail account storage file
    /// </summary>
    public string SmtpMailAccountFileName { get; }

    /// <summary>
    /// Filename of the SMTP mail account storage import file. To be deleted after import
    /// </summary>
    public string SmtpMailAccountImportFileName { get; }

    /// <summary>
    /// The current SMTP mail account or null if no SMTP mailing is used
    /// </summary>
    public SmtpMailAccount? SmtpMailAccount { get; private set; }

    /// <summary>
    /// Filename of the O365 MS Graph based mail account storage import file. To be deleted after import
    /// </summary>
    public string O365MailAccountImportFileName { get; }

    /// <summary>
    /// Filename of the O365 MS Graph based mail account storage file
    /// </summary>
    public string O365MailAccountFileName { get; }

    /// <summary>
    /// The current O365 mail account or null if no SMTP mailing is used
    /// </summary>
    public O365MailAccount? O365MailAccount { get; private set; }

    /// <summary>
    /// Filename of app password hash storage
    /// </summary>
    public string HashFileName { get; }

    /// <summary>
    /// The hash value of the JSON password
    /// </summary>
    public string? AppPasswordHash { get; private set; }

    /// <summary>
    /// Show a status message
    /// </summary>
    /// <param name="message">Message to show in the console</param>
    protected static void SetStatus(string message)
    {
        Console.WriteLine(message);
    }

    #region Basic JSON handling

    /// <summary>
    /// Save a simple entity in a crypted JSON file
    /// </summary>
    /// <typeparam name="T">Type of the entity to store</typeparam>
    /// <param name="entityToStore">Entity to store</param>
    /// <param name="key">Key</param>
    /// <param name="fileName">Filename to save the crypted JSON string in</param>
    public void SaveCryptedSimpleJson<T>(T entityToStore, string key, string fileName)
    {
        var json = SimpleJsonHelper.Serialize(entityToStore);

        var cjson = DataProtectionService.Protect(key, json);

        File.WriteAllText(fileName, cjson, Encoding.UTF8);
    }

    /// <summary>
    /// Load a crypted simple json file
    /// </summary>
    /// <typeparam name="T">Type of the entity to deserialize from the JSON file</typeparam>
    /// <param name="key">Key</param>
    /// <param name="cjsonFile">Fillename of the crypted JSON file</param>
    /// <returns>Deserialized entity</returns>
    public T LoadCryptedSimpleJson<T>(string key, string cjsonFile)
    {
        var raw = File.ReadAllText(cjsonFile);

        var json = DataProtectionService.Unprotect(key, raw);

        var job = SimpleJsonHelper.LoadJsonString<T>(json);

        ArgumentNullException.ThrowIfNull(job);

        return job;
    }

    #endregion

    #region SMTP account handling

    /// <summary>
    /// Load an existing SMTP mail config
    /// </summary>
    public void LoadSmtpConfig()
    {
        if (File.Exists(SmtpMailAccountFileName))
        {
            LoadSmtpMailAccount();

            ArgumentNullException.ThrowIfNull(SmtpMailAccount);
            return;
        }

        if (!File.Exists(SmtpMailAccountImportFileName))
        {
            return;
        }
        var ma = SimpleJsonHelper.LoadJsonFile<SmtpMailAccount>(SmtpMailAccountImportFileName);

        ArgumentNullException.ThrowIfNull(ma);

        SaveSmtpMailAccount(ma);

        LoadSmtpMailAccount();

        FileSystemHelper.DeleteFile(SmtpMailAccountImportFileName);
    }

    /// <summary>
    /// Save SMTP mail account data in a crypted file
    /// </summary>
    /// <param name="mailAccount">SMTP mail account data</param>
    public void SaveSmtpMailAccount(SmtpMailAccount mailAccount)
    {
        SaveCryptedSimpleJson(mailAccount, KeyNameSmtp, SmtpMailAccountFileName);
    }

    /// <summary>
    /// Load SMTP mail account data from a crypted file
    /// </summary>
    /// <returns>SMTP mail account data</returns>
    public SmtpMailAccount LoadSmtpMailAccount()
    {
        SmtpMailAccount = LoadCryptedSimpleJson<SmtpMailAccount>(KeyNameSmtp, SmtpMailAccountFileName);
        return SmtpMailAccount;
    }

    #endregion

    #region O365 mail account handling

    /// <summary>
    /// Load an existing O365 mail config
    /// </summary>
    public void LoadO365Config()
    {
        if (File.Exists(O365MailAccountFileName))
        {
            LoadO365MailAccount();

            ArgumentNullException.ThrowIfNull(O365MailAccount);
            return;
        }

        if (!File.Exists(O365MailAccountImportFileName))
        {
            return;
        }
        var ma = SimpleJsonHelper.LoadJsonFile<O365MailAccount>(O365MailAccountImportFileName);

        ArgumentNullException.ThrowIfNull(ma);

        SaveO365MailAccount(ma);

        LoadO365MailAccount();

        FileSystemHelper.DeleteFile(O365MailAccountImportFileName);
    }

    /// <summary>
    /// Save O365 mail account data in a crypted file
    /// </summary>
    /// <param name="mailAccount">O365 mail account data</param>
    public void SaveO365MailAccount(O365MailAccount mailAccount)
    {
        SaveCryptedSimpleJson(mailAccount, KeyNameO365, O365MailAccountFileName);
    }

    /// <summary>
    /// Load O365 mail account data from a crypted file
    /// </summary>
    /// <returns>O365 mail account data</returns>
    public O365MailAccount LoadO365MailAccount()
    {
        O365MailAccount = LoadCryptedSimpleJson<O365MailAccount>(KeyNameO365, O365MailAccountFileName);
        return O365MailAccount;
    }

    /// <summary>
    /// Filename of the webcredentials storage file
    /// </summary>
    public string WebCredentialsFileName { get; }

    /// <summary>
    /// Filename of the WebCredentials storage import file. To be deleted after import
    /// </summary>
    public string WebCredentialsImportFileName { get; }

    /// <summary>
    /// The current WebCredentials or null if no web access is used
    /// </summary>
    public WebCredentials? WebCredentials { get; private set; }

    /// <summary>
    /// Load an existing WebCredentials config
    /// </summary>
    public void LoadWebCredentialsConfig()
    {
        if (File.Exists(WebCredentialsFileName))
        {
            LoadWebCredentials();

            ArgumentNullException.ThrowIfNull(WebCredentials);
            return;
        }

        if (!File.Exists(WebCredentialsImportFileName))
        {
            return;
        }
        var ma = SimpleJsonHelper.LoadJsonFile<WebCredentials>(WebCredentialsImportFileName);

        ArgumentNullException.ThrowIfNull(ma);

        SaveWebCredentials(ma);

        LoadWebCredentials();

        FileSystemHelper.DeleteFile(WebCredentialsImportFileName);
    }

    /// <summary>
    /// Save WebCredentials data in a crypted file
    /// </summary>
    /// <param name="webCredentials">WebCredentials data</param>
    public void SaveWebCredentials(WebCredentials webCredentials)
    {
        SaveCryptedSimpleJson(webCredentials, KeyNameWebCredentials, WebCredentialsFileName);
    }

    /// <summary>
    /// Load WebCredentials data from a crypted file
    /// </summary>
    /// <returns>WebCredentials data</returns>
    public WebCredentials LoadWebCredentials()
    {
        WebCredentials = LoadCryptedSimpleJson<WebCredentials>(KeyNameWebCredentials, WebCredentialsFileName);
        return WebCredentials;
    }

    #endregion

    #region App password

    /// <summary>
    /// Ask the user for an app password and hash it
    /// </summary>
    public void UiHashAppPassword()
    {
        var pwd = UiAskTheUserForAppPassword();

        if (string.IsNullOrEmpty(pwd))
        {
            return;
        }

        HashAppPassword(pwd);

        ConsoleHelper.WaitUntilAnyKeyPressed("Password was crypted!");

        //Clipboard.SetText(JsonPasswordHash);
        //Console.WriteLine("Encrypted password was copied to clipboard!");
    }

    /// <summary>
    /// Hash the app password
    /// </summary>
    public void HashAppPassword(string appPassword)
    {
        if (string.IsNullOrEmpty(appPassword) || File.Exists(HashFileName))
        {
            return;
        }

        AppPasswordHash = Hasher.ComputeHash(appPassword);

        var crypt = DataProtectionService.Protect("HASH", AppPasswordHash);

        File.WriteAllText(HashFileName, crypt);
    }

    /// <summary>
    /// Load the app password hash from file if existing
    /// </summary>
    public void LoadAppPasswordHash()
    {
        if (!File.Exists(HashFileName))
        {
            return;
        }

        try
        {
            var crypt = File.ReadAllText(HashFileName);

            AppPasswordHash = DataProtectionService.Unprotect("HASH", crypt);
        }
        catch (Exception e)
        {
            ConsoleHelper.WaitUntilAnyKeyPressed($"Loading pashword hash failed:\r\n{e}");
        }
    }

    /// <summary>
    /// Ask the user for an app password 
    /// </summary>
    /// <returns>True if the password is correct else false</returns>
    public bool UiValidateAppPassword()
    {
        var pwd = UiAskTheUserForAppPassword();
        var valid = !string.IsNullOrEmpty(pwd) && ValidateAppPassword(pwd);
        return valid;
    }

    /// <summary>
    /// Validate an app password
    /// </summary>
    /// <param name="jsonPassword">App password to validate</param>
    /// <returns>True if the app password is correct else false</returns>
    public bool ValidateAppPassword(string jsonPassword)
    {
        if (AppPasswordHash is null)
        {
            return false;
        }
        IsAppPasswordHashChecked = Hasher.ValidateHash(jsonPassword, AppPasswordHash);
        return IsAppPasswordHashChecked;
    }

    /// <summary>
    /// Do the requested start action. The default implementation simply runs the <see cref="MainAppRunnerDelegate "/>  
    /// </summary>
    public int Start()
    {
        return MainAppRunnerDelegate?.Invoke(this) ?? 0;
    }

    /// <summary>
    /// Ask the user to insert an app password
    /// </summary>
    /// <returns></returns>
    public static string UiAskTheUserForAppPassword()
    {
        var pwd = ConsoleHelper.ReadSecret("Please insert password for app:");
        return pwd;
    }

    #endregion

    #region Custom entites to encrypt

    /// <summary>
    /// Load an entity from encrypted storage. Uses  <see cref="SimpleJsonHelper"/> not supporting interfaces. Override this method if you need to serialize and deserialize interfaces
    /// </summary>
    /// <param name="key">Key to use</param>
    /// <param name="fileName">Target filename</param>
    /// <param name="importFileName">Name of the import file</param>
    public virtual T? LoadCredentials<T>(string key, string fileName, string importFileName)
    {
        if (File.Exists(fileName))
        {
            var crypted = File.ReadAllText(fileName);

            var json = DataProtectionService.Unprotect(key, crypted);

            return SimpleJsonHelper.LoadJsonString<T>(json);
        }
        else
        {
            if (!File.Exists(importFileName))
            {
                return default;
            }

            var json = File.ReadAllText(importFileName);

            var result = SimpleJsonHelper.LoadJsonString<T>(json);

            SaveCredentials(result, key, fileName);

            FileSystemHelper.DeleteFile(importFileName);

            return result;
        }
    }

    /// <summary>
    /// Save an entity to encrypted storage. Uses  <see cref="SimpleJsonHelper"/> not supporting interfaces. Override this method if you need to serialize and deserialize interfaces
    /// </summary>
    /// <param name="entity">Entity to save</param>
    /// <param name="key">Key to use</param>
    /// <param name="fileName">Target filename</param>
    public virtual void SaveCredentials<T>(T entity, string key, string fileName)
    {
        var json = SimpleJsonHelper.Serialize(entity);

        var crypted = DataProtectionService.Protect(key, json);

        File.WriteAllText(fileName, crypted);
    }

    #endregion

}