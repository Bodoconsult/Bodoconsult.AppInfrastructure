//// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

//using System;
//using System.Threading;
//using Bodoconsult.App.DataProtection;
//using Bodoconsult.App.Windows.System;

//namespace JobExecuter.Dal.Helpers;

///// <summary>
///// Class with console helper functions
///// </summary>
//public class ConsoleHelper
//{
//    /// <summary>
//    /// Number of iterations for hashing    
//    /// </summary>
//    public static int Iterations { get; set; }

//    /// <summary>
//    /// Salt for hashing
//    /// </summary>
//    public static string Salt { get; set; }

//    /// <summary>
//    /// Number of hash bytes to use for hashing
//    /// </summary>
//    public static int HashBytes { get; set; }


       
//    /// <summary>
//    /// Ask user for a password, encrypt it and copy it to the clipboard
//    /// </summary>
//    public static bool EncryptPassword()
//    {
     
//        //Console.WriteLine("Insert password for user (encrypted password will be copied to clipboard):");
//        //var s = PasswordHandler.ReadPassword();

//        //s = PasswordHandler.Encrypt(s);

//        //Clipboard.SetText(s);

//        return true;
//    }


//    ///// <summary>
//    ///// Ask user for a password, hashs it and copy it to the clipboard
//    ///// </summary>
//    //public static string HashPassword()
//    //{
//    //    try
//    //    {
//    //        Console.WriteLine("Insert password for user (encrypted password will be copied to clipboard):");
//    //        var s = PasswordHandler.ReadPassword();

//    //        s = PasswordHandler.CreateHash(s, Salt, HashBytes, Iterations);

//    //        Clipboard.SetText(s);
//    //        Console.WriteLine("Insert password for user (encrypted password will be copied to clipboard):");
//    //        return s;
//    //    }
//    //    catch (Exception e)
//    //    {
//    //        Console.WriteLine(e);
//    //        return null;
//    //    }
    
//    //}


//    ///// <summary>
//    ///// Create a salt and copy it to the clipboard
//    ///// </summary>
//    //public static bool CreateSalt(int saltBytes)
//    //{
//    //    Console.WriteLine("Creating salt. Please wait...");
//    //    var s = PasswordHandler.CreateSalt(saltBytes);
//    //    Salt = s;
//    //    Clipboard.SetText(s);
//    //    Console.WriteLine("Salt copied to clipboard!");
//    //    Thread.Sleep(2000);

//    //    return true;
//    //}


//    ///// <summary>
//    ///// Ask user for a password, hashs it and validate it against a hashed password
//    ///// </summary>
//    //public static bool ValidatePassword(string hashedPassword)
//    //{
//    //    Console.WriteLine("Insert your password:");
//    //    var s = PasswordHandler.ReadPassword();

//    //    var result = PasswordHandler.ValidateHash(s, Salt, hashedPassword, Iterations);

//    //    //// Set new value
//    //    //s = "";

//    //    return result;
//    //}


//    /// <summary>
//    /// Decrypts a password
//    /// </summary>
//    /// <param name="encryptedPassword">password to decrypt</param>
//    /// <returns></returns>
//    public static string DecryptPassword(string encryptedPassword)
//    {
//        return PasswordHandler.Decrypt(encryptedPassword);
//    }
//}