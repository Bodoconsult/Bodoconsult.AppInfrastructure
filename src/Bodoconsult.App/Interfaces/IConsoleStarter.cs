// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.DataProtection.Hashing;

namespace Bodoconsult.App.Interfaces;

/// <summary>
/// A delegate for running the main app thread from the starter
/// </summary>
/// <param name="consoleStarter">Current console starter instance</param>
public delegate int MainAppRunnerDelegate(IConsoleStarter consoleStarter);

/// <summary>
/// Interface for console app starter implementations handling basic console related featires like app password, SMTP mail acccount or O365 MS Graph mail account handling 
/// </summary>
public interface IConsoleStarter
{
    /// <summary>
    /// Current <see cref="Bodoconsult.App.DataProtection.Hashing.Hasher"/> instance for hashing data
    /// </summary>
    Hasher Hasher { get; set; }

    /// <summary>
    /// Current data protection service instance to use for protecting secrets
    /// </summary>
    IDataProtectionService DataProtectionService { get; set; }

    /// <summary>
    /// A delegate for running the main app thread from the starter
    /// </summary>
    MainAppRunnerDelegate? MainAppRunnerDelegate { get; set; }

    /// <summary>
    /// Command line args
    /// </summary>
    string[]? Args { get; }

    /// <summary>
    /// Filename of the SMTP mail account storage file
    /// </summary>
    string SmtpMailAccountFileName { get; }

    /// <summary>
    /// Filename of the SMTP mail account storage import file. To be deleted after import
    /// </summary>
    string SmtpMailAccountImportFileName { get; }

    /// <summary>
    /// The current SMTP mail account or null if no SMTP mailing is used
    /// </summary>
    SmtpMailAccount? SmtpMailAccount { get; }

    /// <summary>
    /// Filename of the O365 MS Graph based mail account storage import file. To be deleted after import
    /// </summary>
    string O365MailAccountImportFileName { get; }

    /// <summary>
    /// Filename of the O365 MS Graph based mail account storage file
    /// </summary>
    string O365MailAccountFileName { get; }

    /// <summary>
    /// The current O365 mail account or null if no SMTP mailing is used
    /// </summary>
    O365MailAccount? O365MailAccount { get; }

    /// <summary>
    /// Filename of app password hash storage
    /// </summary>
    string HashFileName { get; }

    /// <summary>
    /// The hash value of the JSON password
    /// </summary>
    string? AppPasswordHash { get; }

    /// <summary>
    /// Save an entity in a crypted JSON file
    /// </summary>
    /// <typeparam name="T">Type of the entity to store</typeparam>
    /// <param name="entityToStore">Entity to store</param>
    /// <param name="key">Key</param>
    /// <param name="fileName">Filename to save the crypted JSON string in</param>
    void SaveCryptedSimpleJson<T>(T entityToStore, string key, string fileName);

    /// <summary>
    /// Load a crypted json file
    /// </summary>
    /// <typeparam name="T">Type of the entity to deserialize from the JSON file</typeparam>
    /// <param name="key">Key</param>
    /// <param name="cjsonFile">Fillename of the crypted JSON file</param>
    /// <returns>Deserialized entity</returns>
    T LoadCryptedSimpleJson<T>(string key, string cjsonFile);

    /// <summary>
    /// Load an existing SMTP mail config
    /// </summary>
    void LoadSmtpConfig();

    /// <summary>
    /// Save SMTP mail account data in a crypted file
    /// </summary>
    /// <param name="mailAccount">SMTP mail account data</param>
    void SaveSmtpMailAccount(SmtpMailAccount mailAccount);

    /// <summary>
    /// Load SMTP mail account data from a crypted file
    /// </summary>
    /// <returns>SMTP mail account data</returns>
    SmtpMailAccount LoadSmtpMailAccount();

    /// <summary>
    /// Load an existing O365 mail config
    /// </summary>
    void LoadO365Config();

    /// <summary>
    /// Save O365 mail account data in a crypted file
    /// </summary>
    /// <param name="mailAccount">O365 mail account data</param>
    void SaveO365MailAccount(O365MailAccount mailAccount);

    /// <summary>
    /// Load O365 mail account data from a crypted file
    /// </summary>
    /// <returns>O365 mail account data</returns>
    O365MailAccount LoadO365MailAccount();

    /// <summary>
    /// Ask the user for a JSON encryption password and hash it
    /// </summary>
    void UiHashJsonPassword();

    /// <summary>
    /// Hash the app password
    /// </summary>
    void HashAppPassword(string appPassword);

    /// <summary>
    /// Load the app password hash from file if existing
    /// </summary>
    void LoadAppPasswordHash();

    /// <summary>
    /// Ask the user for an app password 
    /// </summary>
    /// <returns>True if the password is correct else false</returns>
    bool UiValidateAppPassword();

    /// <summary>
    /// Validate an app password
    /// </summary>
    /// <param name="jsonPassword">App password to validate</param>
    /// <returns>True if the app password is correct else false</returns>
    bool ValidateAppPassword(string jsonPassword);

    /// <summary>
    /// Do the requested start action
    /// </summary>
    int Start();
}