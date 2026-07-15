// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Buffers;
using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace Bodoconsult.App.Helpers;

/// <summary>
/// Tools for array handling
/// </summary>
public static class ArrayHelper
{
    /// <summary>
    /// Compare two integer arrays
    /// </summary>
    /// <param name="array1">Array 1</param>
    /// <param name="array2">Array 2</param>
    /// <returns>True if both arrays have the same values, else false.</returns>
    public static bool ArrayCompare(int[,,] array1, int[,,] array2)
    {

        var stackLen = array1.GetLength(0);
        var rowLen = array1.GetLength(1);
        var subindexLen = array1.GetLength(2);

        if (stackLen != array2.GetLength(0))
        {
            return false;
        }

        if (rowLen != array2.GetLength(1))
        {
            return false;
        }

        if (subindexLen != array2.GetLength(2))
        {
            return false;
        }

        for (var stack = 0; stack < stackLen; stack++)
        {
            for (var row = 0; row < rowLen; row++)
            {
                for (var subindex = 0; subindex < subindexLen; subindex++)
                {
                    if (array1[stack, row, subindex] != array2[stack, row, subindex])
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Get a string from a byte array
    /// </summary>
    /// <param name="data">Byte array as <see cref="ReadOnlySequence{T}"/></param>
    /// <returns>Byte array as string</returns>
    public static string GetStringFromArray(ref ReadOnlySequence<byte> data)
    {
        var value = new StringBuilder();
        foreach (var b in data.ToArray())
        {
            if (b <= 33 || b >= 127)
            {
                value.Append($"[{b:X2}]");
            }
            else
            {
                value.Append(Convert.ToChar(b));
            }
        }
        return value.ToString();
    }

    /// <summary>
    /// Get a string from a byte array
    /// </summary>
    /// <param name="data">Byte array</param>
    /// <returns>Byte array as string</returns>
    public static string GetStringFromArray(byte[] data)
    {
        var value = new StringBuilder();
        foreach (var b in data)
        {
            if (b <= 33 || b >= 127)
            {
                value.Append($"[{b:X2}]");
            }
            else
            {
                value.Append(Convert.ToChar(b));
            }
        }
        return value.ToString();
    }


    /// <summary>
    /// Get a string from a byte array
    /// </summary>
    /// <param name="data">Byte array</param>
    /// <returns>Byte array as string</returns>
    public static string GetStringFromArray(ReadOnlyMemory<byte> data)
    {
        var value = new StringBuilder();
        byte b;

        for (var i = 0; i < data.Length; i++)
        {
            b = data.Slice(i, 1).Span[0];
            if (b <= 33 || b >= 127)
            {
                value.Append($"[{b:X2}]");
            }
            else
            {
                value.Append(Convert.ToChar(b));
            }
        }
        return value.ToString();
    }

    ///// <summary>
    ///// Get a string from a byte array
    ///// </summary>
    ///// <param name="data">Byte array</param>
    ///// <returns>Byte array as string</returns>
    //public static string GetStringFromArray(ReadOnlySequence<byte> data)
    //{
    //    var value = new StringBuilder();
    //    byte b;

    //    for (var i = 0; i < data.Length; i++)
    //    {
    //        b = data.Slice(i, 1).FirstSpan[0];
    //        if (b <= 33 || b >= 127)
    //        {
    //            value.Append($"[{b:X2}]");
    //        }
    //        else
    //        {
    //            value.Append(Convert.ToChar(b));
    //        }
    //    }
    //    return value.ToString();
    //}

    /// <summary>
    /// Get a string from a byte array
    /// </summary>
    /// <param name="data">Byte array</param>
    /// <returns>Byte array as string</returns>
    public static string GetStringFromArray(Memory<byte> data)
    {
        var value = new StringBuilder();
        byte b;

        for (var i = 0; i < data.Length; i++)
        {
            b = data.Slice(i, 1).Span[0];
            if (b <= 33 || b >= 127)
            {
                value.Append($"[{b:X2}]");
            }
            else
            {
                value.Append(Convert.ToChar(b));
            }
        }
        return value.ToString();
    }

    ///// <summary>
    ///// Get a string from a byte array
    ///// </summary>
    ///// <param name="data">Byte array</param>
    ///// <returns>Byte array as string</returns>
    //public static string GetStringFromArray(ref Span<byte> data)
    //{
    //    var value = new StringBuilder();
    //    byte b;

    //    for (var i = 0; i < data.Length; i++)
    //    {
    //        b = data.Slice(i, 1)[0];
    //        if (b <= 33 || b >= 127)
    //        {
    //            value.Append($"[{b:X2}]");
    //        }
    //        else
    //        {
    //            value.Append(Convert.ToChar(b));
    //        }
    //    }
    //    return value.ToString();
    //}

    /// <summary>
    /// Get a string from a byte array in C# style (for copying it to unit tests)
    /// </summary>
    /// <param name="data">Byte array</param>
    /// <returns>Byte array as string</returns>
    public static string GetStringFromArrayCsharpStyle(byte[] data)
    {

        var result = new StringBuilder();

        result.AppendLine(GetStringFromArray(data));

        result.Append("{ ");

        foreach (var b in data)
        {
            result.Append($"0x{b:x}, ");
        }

        var s = result.ToString();

        return s.EndsWith(", ", StringComparison.OrdinalIgnoreCase) ?
            $"{s[..^2]} }}" :
            $"{s} }}";
    }

    /// <summary>
    /// Get a string with array data in C# style
    /// </summary>
    /// <param name="data">Array data</param>
    /// <param name="showClearText">Show the array as clear text too</param>
    /// <returns>String with array data</returns>
    public static string GetStringFromArrayCsharpStyle(ReadOnlyMemory<byte> data, bool showClearText = true)
    {
        var result = new StringBuilder();

        // Show the array as clear text
        if (showClearText)
        {
            result.Append(GetStringFromArray(data));
        }

        // Now show the array in C# style
        result.Append("  { ");

        for (var i = 0; i < data.Length; i++)
        {
            result.Append($"0x{data.Slice(i, 1).Span[0]:x}, ");
        }

        var s = result.ToString();

        return s.EndsWith(", ", StringComparison.OrdinalIgnoreCase) ?
            $"{s[..^2]} }}" :
            $"{s} }}";
    }


    //public static string GetStringFromArrayCsharpStyle(ref Span<byte> data)
    //{

    //    var result = new StringBuilder();

    //    result.AppendLine(GetStringFromArray(ref data));

    //    result.Append("{ ");

    //    for (var i = 0; i < data.Length; i++)
    //    {
    //        result.Append($"0x{data.Slice(i, 1)[0]:x}, ");
    //    }

    //    var s = result.ToString();

    //    return s.EndsWith(", ", StringComparison.OrdinalIgnoreCase) ?
    //        $"{s[..^2]} }}" :
    //        $"{s} }}";
    //}

    //public static string GetStringFromArrayCsharpStyle(ReadOnlySequence<byte> data)
    //{

    //    var result = new StringBuilder();

    //    result.AppendLine(GetStringFromArray(data));

    //    result.Append("{ ");

    //    for (var i = 0; i < data.Length; i++)
    //    {
    //        result.Append($"0x{data.Slice(i, 1).FirstSpan[0]:x}, ");
    //    }

    //    var s = result.ToString();

    //    return s.EndsWith(", ", StringComparison.OrdinalIgnoreCase) ?
    //        $"{s[..^2]} }}" :
    //        $"{s} }}";
    //}


    /// <summary>
    /// Get a string from a byte array in C# style (for copying it to unit tests)
    /// </summary>
    /// <param name="data">Byte array as readonly span</param>
    /// <returns>Byte array as string</returns>
    public static string GetStringFromArrayCsharpStyle(ref ReadOnlySequence<byte> data)
    {

        var result = new StringBuilder();

        result.Append("{ ");

        for (var i = 0; i < data.Length; i++)
        {
            var b = data.Slice(i, 1).FirstSpan[0];
            result.Append($"0x{b:x}, ");
        }

        var s = result.ToString();

        return s.EndsWith(", ", StringComparison.OrdinalIgnoreCase) ?
            $"{s[..^2]} }}" :
            $"{s} }}";

    }

    /// <summary>
    /// Get a byte array from a string
    /// </summary>
    /// <param name="arrayString">Array as string like 0x00x10x20x2d without delimiter between the byte data</param>
    /// <returns>Byte array</returns>
    /// <exception cref="ArgumentException">At least one value is not parseable to byte datatype</exception>
    public static byte[] GetBytes(string arrayString)
    {
        var s = arrayString
            .Replace(" ", string.Empty, StringComparison.InvariantCultureIgnoreCase)
            .Replace("{", string.Empty, StringComparison.InvariantCultureIgnoreCase)
            .Replace("}", string.Empty, StringComparison.InvariantCultureIgnoreCase)
            .Replace("0x", ",", StringComparison.InvariantCultureIgnoreCase)
            .Split([','], StringSplitOptions.RemoveEmptyEntries);

        var ar = new byte[s.Length];

        var e = string.Empty;

        try
        {
            for (var index = 0; index < s.Length; index++)
            {
                e = $"{s[index]}";
                var d = Convert.ToByte(e, 16);
                ar[index] = d;
            }
        }
        catch
        {
            throw new ArgumentException($"{e} not parseable to byte");
        }

        return ar;
    }

    /// <summary>
    /// Get a byte array from a string
    /// </summary>
    /// <param name="arrayString">Array as string like 0x0, 0x1, 0x2, 0x2d without delimiter between the byte data</param>
    /// <param name="delimiter">Delimiter char like comma or semicolon</param>
    /// <returns>Byte array</returns>
    /// <exception cref="ArgumentException">At least one value is not parseable to byte datatype</exception>
    public static byte[] GetBytes(string arrayString, char delimiter)
    {
        var s = arrayString
            .Replace(" ", string.Empty, StringComparison.InvariantCultureIgnoreCase)
            .Replace("{", string.Empty, StringComparison.InvariantCultureIgnoreCase)
            .Replace("}", string.Empty, StringComparison.InvariantCultureIgnoreCase)
            .Replace("0x", string.Empty, StringComparison.InvariantCultureIgnoreCase)
            .Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

        var ar = new byte[s.Length];

        var e = string.Empty;

        try
        {
            for (var index = 0; index < s.Length; index++)
            {
                e = s[index];
                var d = Convert.ToByte(e, 16);
                ar[index] = d;
            }
        }
        catch
        {
            throw new ArgumentException($"{e} not parseable to byte");
        }

        return ar;
    }

    /// <summary>
    /// Compress byte data
    /// </summary>
    /// <param name="data">Decompressed byte data to compress</param>
    /// <returns>Compressed byte data</returns>
    public static byte[] Compress(byte[] data)
    {
        using var memoryStream = new MemoryStream();

        using (var deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress))
        {
            deflateStream.Write(data, 0, data.Length);
        }

        var compressArray = memoryStream.ToArray();

        return compressArray;
    }

    /// <summary>
    /// Decompress an byte array
    /// </summary>
    /// <param name="data">Compressed byte data</param>
    /// <returns>Decompressed byte data</returns>
    public static byte[] Decompress(byte[] data)
    {
        using var decompressedStream = new MemoryStream();

        using (var compressStream = new MemoryStream(data))
        {
            using (var deflateStream = new DeflateStream(compressStream, CompressionMode.Decompress))
            {
                deflateStream.CopyTo(decompressedStream);
            }
        }
        var decompressedArray = decompressedStream.ToArray();

        return decompressedArray;
    }
}