// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Runtime.Versioning;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Windows.System;
using Bodoconsult.Web.Mail.Helpers;

namespace BodoWebMailer;

/// <summary>
/// Class with console helper functions
/// </summary>
[SupportedOSPlatform("windows10.0")]
public class ConsoleHelper
{
    /// <summary>
    /// Ask user for a token, encrypt it and copy it to the clipboard
    /// </summary>
    public static bool EncryptToken()
    {
     
        Console.WriteLine("Insert token for user (encrypted token will be copied to clipboard):");
        var s = PasswordHandler.ReadPassword();

        s = PasswordHandler.Encrypt(s);

        Clipboard.SetText(s);

        return true;
    }
}