// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Security.Cryptography;

namespace Bodoconsult.App.DataProtection;

/// <summary>
/// Encrypts und decrypts passwords i.e. for console application start parameters
/// </summary>
public static class PasswordHandler
{
    /// <summary>
    /// Key 1 used for symmetric data encryption 
    /// </summary>
    public static string Key1 { get; set; } = string.Empty;

    /// <summary>
    /// Key 2 used for symmetric data encryption 
    /// </summary>
    public static string Key2 { get; set; } = string.Empty;

    /// <summary>
    /// Key 3 used for symmetric data encryption 
    /// </summary>
    public static string Key3 { get; set; } = string.Empty;

    /// <summary>
    /// Requested length of the salt
    /// </summary>
    public static int SaltLength { get; set; } = 20;

    /// <summary>
    /// Salt
    /// </summary>
    public static byte[] Salt { get; set; } = [0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x63, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76];

    /// <summary>
    /// Number of iterations
    /// </summary>
    public static int Iterations { get; set; } = 255;

    /// <summary>
    /// Create three AES keys and saves it in <see cref="Key1"/>, <see cref="Key2"/>, <see cref="Key3"/>
    /// </summary>
    public static void CreateKeys()
    {
        var aes = Aes.Create();
        
        aes.GenerateKey();
        Key1 = Convert.ToBase64String(aes.Key);

        aes.GenerateKey();
        Key2 = Convert.ToBase64String(aes.Key);

        aes.GenerateKey();
        Key3 = Convert.ToBase64String(aes.Key);

        aes.GenerateIV();
        Salt = aes.IV;
    }

    /// <summary>
    /// Create AES salt and save it in <see cref="Salt"/>
    /// </summary>
    public static void CreateSalt()
    {

    }

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
    /// Encrypt as string with key 2
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
    /// Encrypt as string with key
    /// </summary>
    /// <param name="originalString">Original string</param>
    /// <param name="key">Key to use for encryption</param>
    /// <returns>Encrypted string</returns>
    private static string EncryptInternal(string originalString, string key)
    {
        var keyBytes = Convert.FromBase64String(key);
        byte[] encrypted;

        // Create an Aes object
        // with the specified key and IV.
        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = keyBytes;
            aesAlg.IV = Salt;

            // Create an encryptor to perform the stream transform.
            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(originalString);
                    }
                }

                encrypted = msEncrypt.ToArray();
            }
        }

        // Return the encrypted bytes from the memory stream.
        return Convert.ToBase64String(encrypted);
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
        var keyBytes = Convert.FromBase64String(key);
        var cipherBytes = Convert.FromBase64String(cryptedString.Replace(" ", "+"));

        using var aesAlg = Aes.Create();
        aesAlg.Key = keyBytes;
        aesAlg.IV = Salt;

        // Create a decryptor to perform the stream transform.
        var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        // Create the streams used for decryption.
        using var msDecrypt = new MemoryStream(cipherBytes);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
        // Read the decrypted bytes from the decrypting stream
        // and place them in a string.
        var plaintext = srDecrypt.ReadToEnd();

        return plaintext;
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
        var pbkdf2 = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, outputBytes);
        return pbkdf2;
    }


}
