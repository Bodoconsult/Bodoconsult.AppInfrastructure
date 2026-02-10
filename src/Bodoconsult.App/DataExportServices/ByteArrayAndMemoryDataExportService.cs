// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.DataExportServices;

/// <summary>
/// Data export service for byte arrays
/// </summary>
public class ByteArrayDataExportService : DataExportServiceBase<byte[]>
{
    /// <summary>
    /// Converts an object of type T into a ReadOnlyMemory&lt;byte&gt; instance
    /// </summary>
    /// <param name="data">Data object to serialize</param>
    /// <returns>Byte array</returns>
    /// <exception cref="NotSupportedException">Thrown if type T is NOT string, ReadOnlyMemory&lt;byte&gt; or byte[]</exception>
    public override ReadOnlyMemory<byte> ToMemory(byte[] data)
    {
        return data;
    }
}