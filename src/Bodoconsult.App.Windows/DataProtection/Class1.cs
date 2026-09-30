//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

//using Bodoconsult.Core.Windows.System;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Versioning;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Bodoconsult.App.Windows.Crypto.DataProtection;
//using System.Collections;
//using System.Globalization;

//namespace Bodoconsult.App.Windows.Crypto;

///// <summary>
///// Class with console helper functions
///// </summary>
//[SupportedOSPlatform("windows")]
//public static class ConsoleHelper
//{
//    static ConsoleHelper()
//    {
//        DataProtectionService = new DataProtectionService();
//    }


//    public static DataProtectionService DataProtectionService { get; }


//    /// <summary>
//    /// Ask user for a password, encrypt it and copy it to the clipboard
//    /// </summary>
//    public static bool EncryptPassword()
//    {

//        Console.WriteLine("Insert password for user (encrypted password will be copied to clipboard):");
//        var s = ReadPassword();

//        s = DataProtectionService.ProtectString(s);

//        Clipboard.SetText(s);

//        return true;
//    }


//    /// <summary>
//    /// Ask user for a password, hashs it and copy it to the clipboard
//    /// </summary>
//    public static string HashPassword()
//    {
//        try
//        {
//            Console.WriteLine("Insert password for user (encrypted password will be copied to clipboard):");
//            var s = ReadPassword();

//            s = PasswordHandler.CreateHash(s, Salt, HashBytes, Iterations);

//            Clipboard.SetText(s);
//            Console.WriteLine("Insert password for user (encrypted password will be copied to clipboard):");
//            return s;
//        }
//        catch (Exception e)
//        {
//            Console.WriteLine(e);
//            return null;
//        }

//    }


//    /// <summary>
//    /// Create a salt and copy it to the clipboard
//    /// </summary>
//    public static bool CreateSalt(int saltBytes)
//    {
//        Console.WriteLine("Creating salt. Please wait...");
//        var s = PasswordHandler.CreateSalt(saltBytes);
//        Clipboard.SetText(s);
//        Console.WriteLine("Salt copied to clipboard!");
//        Thread.Sleep(2000);

//        return true;
//    }


//    /// <summary>
//    /// Ask user for a password, hashs it and validate it against a hashed password
//    /// </summary>
//    public static bool ValidatePassword(string hashedPassword)
//    {
//        Console.WriteLine("Insert your password:");
//        var s = ReadPassword();

//        var result = PasswordHandler.ValidateHash(s, Salt, hashedPassword, Iterations);

//        // Set new value
//        s = "";

//        return result;
//    }


//    ///// <summary>
//    ///// Decrypts a password
//    ///// </summary>
//    ///// <param name="encryptedPassword">password to decrypt</param>
//    ///// <returns>Clear text password</returns>
//    //public static string DecryptPassword(string encryptedPassword)
//    //{
//    //    return DataProtectionService.UnprotectString(encryptedPassword);
//    //}


//    /// <summary>
//    /// Read a password from console
//    /// </summary>
//    /// <returns>password string</returns>
//    public static string ReadPassword()
//    {
//        var passbits = new Queue();

//        for (var cki = Console.ReadKey(true); cki.Key != ConsoleKey.Enter; cki = Console.ReadKey(true))
//        {
//            if (cki.Key == ConsoleKey.Backspace)
//            {
//                if (passbits.Count <= 0) continue;
//                Console.SetCursorPosition((Console.CursorLeft - 1), Console.CursorTop);
//                Console.Write(" ");
//                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
//                passbits.Dequeue();
//            }
//            else
//            {
//                Console.Write("*");
//                passbits.Enqueue(cki.KeyChar.ToString(CultureInfo.InvariantCulture));
//            }
//        }
//        Console.WriteLine();

//        var pass = passbits.Cast<object>()
//            .Select(x => x.ToString())
//            .ToArray();

//        return string.Join(string.Empty, pass);
//    }
//}