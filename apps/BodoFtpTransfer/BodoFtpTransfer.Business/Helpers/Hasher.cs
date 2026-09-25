// Daten mit Hashing-Verfahren verschlüsseln

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
// ReSharper disable InconsistentNaming

namespace BodoFtpTransfer.Business.Helpers;

/// <summary>
/// Klasse für verschiedene Hash-Algorithmen
/// </summary>
public class Hasher
{
    /* Eigenschaft zur Speicherung des Hash-Objekts */
    private readonly HashAlgorithm _hashObject;

    /// <summary>
    /// Konstruktor
    /// </summary>
    /// <param name="algorithm">Angabe des zu verwendenden Hash-Algorithmus</param>
    public Hasher(HashAlgorithmEnum algorithm)
    {
        switch (algorithm)
        {
            case HashAlgorithmEnum.MD5:
                _hashObject = MD5.Create();
                break;

            //case HashAlgorithmEnum.RIPEMD160:
            //    this.hashObject = new RIPEMD160Managed();
            //    break;

            case HashAlgorithmEnum.SHA1:
                _hashObject =SHA1.Create();
                break;

            case HashAlgorithmEnum.SHA256:
                _hashObject = SHA256.Create();
                break;

            case HashAlgorithmEnum.SHA384:
                _hashObject = SHA384.Create();
                break;

            case HashAlgorithmEnum.SHA512:
                _hashObject = SHA512.Create();
                break;

            case HashAlgorithmEnum.HMACMD5:
                throw new NotSupportedException("Not supported anymore");
                //_hashObject = HMACMD5.Create();
                //break;

            //case HashAlgorithmEnum.HMACRIPEMD160:
            //    this.hashObject = new HMACRIPEMD160();
            //    break;

            case HashAlgorithmEnum.HMACSHA1:
                throw new NotSupportedException("Not supported anymore");
                //_hashObject = HMACSHA1.Create();
                //break;

            case HashAlgorithmEnum.HMACSHA256:
                throw new NotSupportedException("Not supported anymore");
                //_hashObject = SHA256.Create();
                //break;

            case HashAlgorithmEnum.HMACSHA384:
                throw new NotSupportedException("Not supported anymore");
                //_hashObject = HMACSHA384.Create();
                //break;

            case HashAlgorithmEnum.HMACSHA512:
                throw new NotSupportedException("Not supported anymore");
                //_hashObject = HMACSHA512.Create();
                //break;

                //case HashAlgorithmEnum.MACTripleDES:
                //    this.hashObject = new MACTripleDES();
                //    break;
        }
    }

    /// <summary>
    /// Verwaltet den Schlüssel für Algorithmen, die einen solchen benötigen
    /// </summary>
    public string Key
    {
        get
        {
            // Überprüfen, ob der Algorithmus einen Schlüssel erlaubt,
            // und Speichern des Schlüssel
            if (_hashObject is KeyedHashAlgorithm kha)
            // Schlüssel in einen String umwandeln und zurückgeben
            {
                return Encoding.GetEncoding("ISO-8859-1").GetString(kha.Key);
            }

            throw new NotSupportedException("Der aktuell verwendete " +
                                            "Hash-Algorithmus unterstützt keine Schlüssel");
        }

        set
        {
            // Auf Unicode-Zeichen größer 0x00FF überprüfen
            for (var i = 0; i < value.Length; i++)
            {
                if ((int)value[i] > 255)
                {
                    throw new CryptographicException(
                        $"Der übergebene Schlüssel enthält mindestens ein Unicode-Zeichen, das größer ist als 0x00FF (255): {value[i]} ({(int)value[i]}). Unterstützt werden lediglich 8-Bit-Unicode-Zeichen");
                }
            }

            // Überprüfen, ob der Algorithmus einen Schlüssel erlaubt,
            // und Speichern des Schlüssel
            if (_hashObject is KeyedHashAlgorithm kha)
            // Schlüssel in einen String umwandeln und zurückgeben
            {
                kha.Key = Encoding.GetEncoding("ISO-8859-1").GetBytes(value);
            }
            else
            {
                throw new NotSupportedException("Der aktuell verwendete " +
                                                "Hash-Algorithmus unterstützt keine Schlüssel");
            }
        }
    }

    ///// <summary>
    ///// Erzeugt einen Hash aus einem Byte-Array
    ///// </summary>
    ///// <param name="data">Das Array mit den Daten</param>
    ///// <returns>Gibt einen String zurück, der den Hash repräsentiert</returns>
    //public string ComputeHash(byte[] data)
    //{
    //    return Encoding.GetEncoding("ISO-8859-1").GetString(
    //       _hashObject.ComputeHash(data));
    //}

    private static string ByteArrayToString(byte[] arrInput)
    {
        int i;
        var sOutput = new StringBuilder(arrInput.Length);
        for (i = 0; i < arrInput.Length - 1; i++)
        {
            sOutput.Append(arrInput[i].ToString("X2"));
        }
        return sOutput.ToString();
    }


    ///// <summary>
    ///// Erzeugt einen Hash aus einem Byte-Array
    ///// </summary>
    ///// <param name="data">Das Array mit den Daten</param>
    ///// <param name="offset">Offset, ab dem das Array gelesen werden soll</param>
    ///// <param name="count">Anzahl der zu lesenden Bytes</param>
    ///// <returns>Gibt einen String zurück, der den Hash repräsentiert</returns>
    //public string ComputeHash(byte[] data, int offset, int count)
    //{
    //    return Encoding.GetEncoding("ISO-8859-1").GetString(
    //       _hashObject.ComputeHash(data, offset, count));
    //}

    ///// <summary>
    ///// Erzeugt einen Hash aus den Daten eines Stream
    ///// </summary>
    ///// <param name="inputStream">Der Stream mit den Daten</param>
    ///// <returns>Gibt einen String zurück, der den Hash repräsentiert</returns>
    //public string ComputeHash(Stream inputStream)
    //{
    //    return Encoding.GetEncoding("ISO-8859-1").GetString(
    //       _hashObject.ComputeHash(inputStream));
    //}

    public string ComputeHashHex(Stream inputStream)
    {
        return ByteArrayToString(_hashObject.ComputeHash(inputStream));
    }

    ///// <summary>
    ///// Erzeugt einen Hash für einen String
    ///// </summary>
    ///// <param name="inputString">Der String</param>
    ///// <returns>Gibt einen String zurück, der den Hash repräsentiert</returns>
    //public string ComputeHash(string inputString)
    //{
    //    // Byte-Array aus dem String erzeugen und damit den Hashcode erzeugen
    //    var buffer = Encoding.Unicode.GetBytes(inputString);
    //    return Encoding.GetEncoding("ISO-8859-1").GetString(
    //       _hashObject.ComputeHash(buffer));
    //}

    //public string ComputeHashHex(string inputString)
    //{
    //    // Byte-Array aus dem String erzeugen und damit den Hashcode erzeugen
    //    var buffer = Encoding.Unicode.GetBytes(inputString);
    //    return ByteArrayToString(_hashObject.ComputeHash(buffer));
    //}
}