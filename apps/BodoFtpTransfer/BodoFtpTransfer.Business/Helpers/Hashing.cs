// Daten mit Hashing-Verfahren verschlüsseln

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BodoFtpTransfer.Business.Helpers
{
    /// <summary>
    /// Aufzählung für die unterstützten Hashing-Algorithmen
    /// </summary>
    public enum HashAlgorithmEnum
    {
        /// <summary>
        /// MD5-Verfahren, das einen Hash mit einer Länge von 128 Bit erzeugt
        /// </summary>
        MD5,

        /// <summary>
        /// RIPEMD-Verfahren, das einen Hash mit einer Länge von 160 Bit erzeugt
        /// </summary>
        /// <remarks>
        /// Entwickelt als Ersatz für MD4 und MD5
        /// </remarks>
        RIPEMD160,

        /// <summary>
        /// SHA-Verfahren, das einen Hash mit einer Länge von 160 Bit erzeugt
        /// </summary>
        SHA1,

        /// <summary>
        /// SHA-Verfahren, das einen Hash mit einer Länge von 256 Bit erzeugt
        /// </summary>
        SHA256,

        /// <summary>
        /// SHA-Verfahren, das einen Hash mit einer Länge von 384 Bit erzeugt
        /// </summary>
        SHA384,

        /// <summary>
        /// SHA-Verfahren, das einen Hash mit einer Länge von 512 Bit erzeugt
        /// </summary>
        SHA512,

        /// <summary>
        /// Hash-based Message Authentication Code (HMAC) über das RIPEMD160-Verfahren
        /// </summary>
        /// <remarks>
        /// HMAC verwendet einen privaten Schlüssel, der mit den Daten vermischt wird.
        /// Das Ergebnis wird über das Hashing-Verfahren in einem Hash umgewandelt,
        /// der Hashcode wird wieder mit dem privaten Schlüssel vermengt und die
        /// Hashfunktion wird ein zweites Mal auf diese Datenmenge angewendet.
        /// HMACMD5 verwendet das MD5-Verfahren, das einen Hash der Länge 128 Bit erzeugt.
        /// </remarks>
        HMACMD5,

        /// <summary>
        /// Hash-based Message Authentication Code (HMAC) über das RIPEMD160-Verfahren
        /// </summary>
        /// <remarks>
        /// HMAC verwendet einen privaten Schlüssel, der mit den Daten vermischt wird.
        /// Das Ergebnis wird über das Hashing-Verfahren in einem Hash umgewandelt,
        /// der Hashcode wird wieder mit dem privaten Schlüssel vermengt und die
        /// Hashfunktion wird ein zweites Mal auf diese Datenmenge angewendet.
        /// HMACRIPEMD160 verwendet das RIPEMD160-Verfahren, das einen Hash der Länge 160 Bit erzeugt.
        /// </remarks>
        HMACRIPEMD160,

        /// <summary>
        /// Hash-based Message Authentication Code (HMAC) über das SHA1-Verfahren
        /// </summary>
        /// <remarks>
        /// HMAC verwendet einen privaten Schlüssel, der mit den Daten vermischt wird.
        /// Das Ergebnis wird über das Hashing-Verfahren in einem Hash umgewandelt,
        /// der Hashcode wird wieder mit dem privaten Schlüssel vermengt und die
        /// Hashfunktion wird ein zweites Mal auf diese Datenmenge angewendet.
        /// HMACSHA1 verwendet das SHA1-Verfahren, das einen Hash der Länge 160 Bit erzeugt.
        /// </remarks>
        HMACSHA1,

        /// <summary>
        /// Hash-based Message Authentication Code (HMAC) über das SHA256-Verfahren
        /// </summary>
        /// <remarks>
        /// HMAC verwendet einen privaten Schlüssel, der mit den Daten vermischt wird.
        /// Das Ergebnis wird über das Hashing-Verfahren in einem Hash umgewandelt,
        /// der Hashcode wird wieder mit dem privaten Schlüssel vermengt und die
        /// Hashfunktion wird ein zweites Mal auf diese Datenmenge angewendet.
        /// HMACSHA256 verwendet das SHA1-Verfahren, das einen Hash der Länge 256 Bit erzeugt.
        /// </remarks>
        HMACSHA256,

        /// <summary>
        /// Hash-based Message Authentication Code (HMAC) über das SHA384-Verfahren
        /// </summary>
        /// <remarks>
        /// HMAC verwendet einen privaten Schlüssel, der mit den Daten vermischt wird.
        /// Das Ergebnis wird über das Hashing-Verfahren in einem Hash umgewandelt,
        /// der Hashcode wird wieder mit dem privaten Schlüssel vermengt und die
        /// Hashfunktion wird ein zweites Mal auf diese Datenmenge angewendet.
        /// HMACSHA384 verwendet das SHA1-Verfahren, das einen Hash der Länge 384 Bit erzeugt.
        /// </remarks>
        HMACSHA384,

        /// <summary>
        /// Hash-based Message Authentication Code (HMAC) über das SHA512-Verfahren
        /// </summary>
        /// <remarks>
        /// HMAC verwendet einen privaten Schlüssel, der mit den Daten vermischt wird.
        /// Das Ergebnis wird über das Hashing-Verfahren in einem Hash umgewandelt,
        /// der Hashcode wird wieder mit dem privaten Schlüssel vermengt und die
        /// Hashfunktion wird ein zweites Mal auf diese Datenmenge angewendet.
        /// HMACSHA512 verwendet das SHA1-Verfahren, das einen Hash der Länge 512 Bit erzeugt.
        /// </remarks>
        HMACSHA512,

        /// <summary>
        /// Message Authentication Code (MAC) über das TripleDES-Verfahren
        /// </summary>
        /// <remarks>
        /// MACTripleDES setzt das TripleDES-Verfahren ein und erzeugt eunen Hash von 
        /// 64 Bit Länge
        /// </remarks>
        MACTripleDES
    }

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
                    _hashObject = new MD5CryptoServiceProvider();
                    break;

                //case HashAlgorithmEnum.RIPEMD160:
                //    this.hashObject = new RIPEMD160Managed();
                //    break;

                case HashAlgorithmEnum.SHA1:
                    _hashObject = new SHA1Managed();
                    break;

                case HashAlgorithmEnum.SHA256:
                    _hashObject = new SHA256Managed();
                    break;

                case HashAlgorithmEnum.SHA384:
                    _hashObject = new SHA384Managed();
                    break;

                case HashAlgorithmEnum.SHA512:
                    _hashObject = new SHA512Managed();
                    break;

                case HashAlgorithmEnum.HMACMD5:
                    _hashObject = new HMACMD5();
                    break;

                //case HashAlgorithmEnum.HMACRIPEMD160:
                //    this.hashObject = new HMACRIPEMD160();
                //    break;

                case HashAlgorithmEnum.HMACSHA1:
                    _hashObject = new HMACSHA1();
                    break;

                case HashAlgorithmEnum.HMACSHA256:
                    _hashObject = new HMACSHA256();
                    break;

                case HashAlgorithmEnum.HMACSHA384:
                    _hashObject = new HMACSHA384();
                    break;

                case HashAlgorithmEnum.HMACSHA512:
                    _hashObject = new HMACSHA512();
                    break;

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
                            $"Der übergebene Schlüssel enthält mindestens ein Unicode-Zeichen, das größer ist als 0x00FF (255): {value[i]} ({(int) value[i]}). Unterstützt werden lediglich 8-Bit-Unicode-Zeichen");
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

}