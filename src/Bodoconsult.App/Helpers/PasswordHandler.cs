// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Collections;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Bodoconsult.App.Helpers;

/// <summary>
/// Encrypts und decrypts passwords for console application start parameters
/// </summary>
public class PasswordHandler
{
    /// <summary>
    /// Key 1 used for symmetric data encryption 
    /// </summary>
    public static string Key1 { get; set; } = "abc";

    /// <summary>
    /// Key 2 used for symmetric data encryption 
    /// </summary>
    public static string Key2 { get; set; } = "def";

    /// <summary>
    /// Key 3 used for symmetric data encryption 
    /// </summary>
    public static string Key3 { get; set; } = "ghi";

    /// <summary>
    /// Salt
    /// </summary>
    public static byte[] Salt { get; set; } = [0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x63, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76];

    /// <summary>
    /// Number of iterations
    /// </summary>
    public static int Iterations { get; set; } = 255;

    /// <summary>
    /// Encrypt as string with 
    /// </summary>
    /// <param name="originalString">Original string</param>
    /// <returns>Encrypted string</returns>
    public static string Encrypt(string originalString)
    {
        if (string.IsNullOrEmpty(originalString))
        {
            throw new ArgumentNullException
                // ReSharper disable NotResolvedInText
                ("The string which needs to be encrypted can not be null.");
            // ReSharper restore NotResolvedInText
        }

        return EncryptInternal(originalString, Key1);
    }

    /// <summary>
    /// Encrypt as string with 
    /// </summary>
    /// <param name="originalString">Original string</param>
    /// <param name="key">Key to use for encryption</param>
    /// <returns>Encrypted string</returns>
    private static string EncryptInternal(string originalString, string key)
    {
        var clearBytes = Encoding.Unicode.GetBytes(originalString);

        using var encryptor = Aes.Create();
        var pdb = new Rfc2898DeriveBytes(key, Salt, Iterations, HashAlgorithmName.SHA256);
        encryptor.Key = pdb.GetBytes(32);
        encryptor.IV = pdb.GetBytes(16);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        {
            cs.Write(clearBytes, 0, clearBytes.Length);
            cs.Close();
        }
        return Convert.ToBase64String(ms.ToArray());
    }

    /// <summary>
    /// Encrypt as string with 
    /// </summary>
    /// <param name="originalString">Original string</param>
    /// <returns>Encrypted string</returns>
    public static string Encrypt2(string originalString)
    {
        if (string.IsNullOrEmpty(originalString))
        {
            throw new ArgumentNullException
                // ReSharper disable NotResolvedInText
                ("The string which needs to be encrypted can not be null.");
            // ReSharper restore NotResolvedInText
        }
        return EncryptInternal(originalString, Key2);
    }

    /// <summary>
    /// Encrypt as string with 
    /// </summary>
    /// <param name="originalString">Original string</param>
    /// <returns>Encrypted string</returns>
    public static string Encrypt3(string originalString)
    {
        if (string.IsNullOrEmpty(originalString))
        {
            throw new ArgumentNullException
                // ReSharper disable NotResolvedInText
                ("The string which needs to be encrypted can not be null.");
            // ReSharper restore NotResolvedInText
        }
        return EncryptInternal(originalString, Key3);
    }

    /// <summary>
    /// Decrypt as string
    /// </summary>
    /// <param name="cryptedString">Crypted string</param>
    /// <returns>Original string</returns>
    public static string? Decrypt(string cryptedString)
    {
        return string.IsNullOrEmpty(cryptedString) ? null : DecryptInternal(cryptedString, Key1);
    }

    /// <summary>
    /// Decrypt as string
    /// </summary>
    /// <param name="cryptedString">Crypted string</param>
    /// <returns>Original string</returns>
    public static string? Decrypt2(string cryptedString)
    {
        return string.IsNullOrEmpty(cryptedString) ? null : DecryptInternal(cryptedString, Key2);
    }

    /// <summary>
    /// Decrypt as string
    /// </summary>
    /// <param name="cryptedString">Crypted string</param>
    /// <returns>Original string</returns>
    public static string? Decrypt3(string cryptedString)
    {
        return string.IsNullOrEmpty(cryptedString) ? null : DecryptInternal(cryptedString, Key3);
    }

    /// <summary>
    /// Decrypt as string
    /// </summary>
    /// <param name="cryptedString">Crypted string</param>
    /// <param name="key">Key to use for decryption</param>
    /// <returns>Original string</returns>
    private static string DecryptInternal(string cryptedString, string key)
    {
        var cipherBytes = Convert.FromBase64String(cryptedString.Replace(" ", "+"));
        
        using var encryptor = Aes.Create();
        
        var pdb = new Rfc2898DeriveBytes(key, Salt, Iterations, HashAlgorithmName.SHA256);
        encryptor.Key = pdb.GetBytes(32);
        encryptor.IV = pdb.GetBytes(16);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        {
            cs.Write(cipherBytes, 0, cipherBytes.Length);
            cs.Close();
        }
        cryptedString = Encoding.Unicode.GetString(ms.ToArray());

        return cryptedString;
    }

    /// <summary>
    /// Compare two hash values a and b: if a equals b return true else false
    /// </summary>
    /// <param name="a">hash value a</param>
    /// <param name="b">hash value b</param>
    /// <returns></returns>
    private static bool SlowEquals(byte[] a, byte[] b)
    {
        var diff = (uint)a.Length ^ (uint)b.Length;
        for (var i = 0; i < a.Length && i < b.Length; i++)
        {
            diff |= (uint)(a[i] ^ b[i]);
        }
        return diff == 0;
    }

    private static byte[] Pbkdf2(string password, byte[] salt, int iterations, int outputBytes)
    {
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(outputBytes);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="salt"></param>
    /// <param name="hashBytes"></param>
    /// <param name="pbkdf2Iterations"></param>
    /// <returns>Base64 encoded string</returns>
    public static string CreateHash(string value, string salt, int hashBytes, int pbkdf2Iterations)
    {
        var saltBytes = Convert.FromBase64String(salt);

        // Hash the value and encode the parameters
        var hash = Pbkdf2(value, saltBytes, pbkdf2Iterations, hashBytes);

        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Create a salt for hashing
    /// </summary>
    /// <param name="saltBytes"></param>
    /// <returns></returns>
    public static string CreateSalt(int saltBytes)
    {
        // Generate a random salt
        var salt = new byte[saltBytes];

        var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return Convert.ToBase64String(salt);
    }

    /// <summary>
    /// Hashes a value and compares with another hashed value
    /// </summary>
    /// <param name="pureVal"></param>
    /// <param name="saltVal">Salt</param>
    /// <param name="hashVal">Hashed value to compare with</param>
    /// <param name="pbkdf2Iterations"></param>
    /// <returns></returns>
    public static bool ValidateHash(string pureVal, string saltVal, string hashVal, int pbkdf2Iterations)
    {
        try
        {
            var salt = Convert.FromBase64String(saltVal);
            var hash = Convert.FromBase64String(hashVal);

            var testHash = Pbkdf2(pureVal, salt, pbkdf2Iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Read a password from console
    /// </summary>
    /// <returns>password string</returns>
    public static string ReadPassword()
    {
        var passbits = new Queue();

        for (var cki = Console.ReadKey(true); cki.Key != ConsoleKey.Enter; cki = Console.ReadKey(true))
        {
            if (cki.Key == ConsoleKey.Backspace)
            {
                if (passbits.Count <= 0)
                {
                    continue;
                }
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                Console.Write(" ");
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                passbits.Dequeue();
            }
            else
            {
                Console.Write("*");
                passbits.Enqueue(cki.KeyChar.ToString(CultureInfo.InvariantCulture));
            }
        }
        Console.WriteLine();

        var pass = passbits.Cast<object>()
            .Select(x => x.ToString())
            .ToArray();

        return string.Join(string.Empty, pass);
    }
}