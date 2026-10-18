// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.Network.Windows.Dhcp.Options;
using Bodoconsult.Network.Windows.Helpers;
using Bodoconsult.Network.Windows.Interfaces;
using System.Runtime.InteropServices;

namespace Bodoconsult.Network.Windows.Dhcp;

/// <summary>
/// Write packet
/// </summary>
public class DhcpServerPacketWritable : DhcpServerPacket, IDhcpServerPacketRawWritable
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="pointer">Pointer</param>
    /// <param name="size">Size</param>
    public DhcpServerPacketWritable(IntPtr pointer, int size) : base(pointer, size)
    {
        IsBufferModified = false;
    }

    /// <summary>
    /// Is buffer modified
    /// </summary>
    public bool IsBufferModified { get; private set; }

    /// <summary>
    /// Write the bugffer
    /// </summary>
    /// <param name="packet">Packet</param>
    /// <param name="packetSize">Packet size</param>
    /// <exception cref="OutOfMemoryException"></exception>
    public void WriteBuffer(IntPtr packet, ref uint packetSize)
    {
        if (BufferInternal is null || !IsBufferModified)
        {
            return;
        }

        if (BufferInternal.Length > MaxBufferLength)
        {
            throw new OutOfMemoryException("Packet buffer is too large");
        }

        Marshal.Copy(BufferInternal, 0, packet, BufferInternal.Length);
        packetSize = (uint)BufferInternal.Length;
    }

    /// <inheritdoc />
    public bool TryAdjustBuffer(int index, int adjustmentAmount)
    {
        var buffer = BufferInternal ?? Buffer;
        var length = GetLength();

        if (adjustmentAmount == 0)
        {
            return true; // do nothing
        }

        if (adjustmentAmount < 0)
        {
            // shrink buffer
            var shrinkBytes = Math.Abs(adjustmentAmount);

            if (length - index < shrinkBytes)
                return false;

            // shrink buffer
            length -= shrinkBytes;
            Length = length;
            if (index < length)
                Array.Copy(buffer, index + shrinkBytes, buffer, index, length - index);
            Array.Clear(buffer, length, shrinkBytes);

            IsBufferModified = true;
        }
        else
        {
            // increase buffer
            if (!TryEnsureCapacity(length + adjustmentAmount))
            {
                return false;
            }

            if (index < length)
            {
                Array.Copy(buffer, index, buffer, index + adjustmentAmount, length - index);
            }
            Length = length + adjustmentAmount;
            Array.Clear(buffer, index, adjustmentAmount);

            IsBufferModified = true;
        }
        return true;
    }

    private bool TryEnsureCapacity(int minimumLength)
    {
        if (minimumLength > MaxBufferLength)
        {
            return false;
        }

        var buffer = BufferInternal ?? Buffer;

        if (buffer.Length >= minimumLength)
        {
            return true;
        }

        var length = GetLength();
        var destination = new byte[minimumLength];
        if (length > 0)
        {
            Array.Copy(buffer, 0, destination, 0, length);
        }
        buffer = destination;

        return true;
    }

    #region Packet Fields

    /// <inheritdoc />
    DhcpServerMessageTypes IDhcpServerPacketWritable.MessageType
    {
        get => MessageType;
        set => Buffer[OpOffset] = (byte)value;
    }

    /// <inheritdoc />
    DhcpServerHardwareType IDhcpServerPacketWritable.HardwareAddressType
    {
        get => HardwareAddressType;
        set => Buffer[HtypeOffset] = (byte)value;
    }

    /// <inheritdoc />
    byte IDhcpServerPacketWritable.HardwareAddressLength
    {
        get => HardwareAddressLength;
        set
        {
            if (value > 16)
                throw new ArgumentOutOfRangeException($"{nameof(HardwareAddressLength)} cannot be greater than 16");

            Buffer[HlenOffset] = value;
        }
    }

    byte IDhcpServerPacketWritable.GatewayHops
    {
        get => GatewayHops;
        set => Buffer[HopsOffset] = value;
    }

    int IDhcpServerPacketWritable.TransactionId
    {
        get => TransactionId;
        set => BitHelper.Write(Buffer, XidOffset, value);
    }

    ushort IDhcpServerPacketWritable.SecondsElapsed
    {
        get => SecondsElapsed;
        set => BitHelper.Write(Buffer, SecsOffset, value);
    }

    DhcpServerPacketFlags IDhcpServerPacketWritable.Flags
    {
        get => Flags;
        set => BitHelper.Write(Buffer, FlagsOffset, (ushort)value);
    }

    DhcpServerIpAddress IDhcpServerPacketWritable.ClientIpAddress
    {
        get => ClientIpAddress;
        set => BitHelper.Write(Buffer, CiaddrOffset, value);
    }

    DhcpServerIpAddress IDhcpServerPacketWritable.YourIpAddress
    {
        get => YourIpAddress;
        set => BitHelper.Write(Buffer, YiaddrOffset, value);
    }

    DhcpServerIpAddress IDhcpServerPacketWritable.NextServerIpAddress
    {
        get => NextServerIpAddress;
        set => BitHelper.Write(Buffer, SiaddrOffset, value);
    }

    DhcpServerIpAddress IDhcpServerPacketWritable.RelayAgentIpAddress
    {
        get => RelayAgentIpAddress;
        set => BitHelper.Write(Buffer, GiaddrOffset, value);
    }

    DhcpServerHardwareAddress IDhcpServerPacketWritable.ClientHardwareAddress
    {
        get => ClientHardwareAddress;
        set => value.ToNative(Buffer, ChaddrOffset, 16);
    }

    string IDhcpServerPacketWritable.ServerHostName
    {
        get => ServerHostName;
        set => BitHelper.WriteAscii(Buffer, SnameOffset, value, 64);
    }

    string IDhcpServerPacketWritable.FileName
    {
        get => FileName;
        set => BitHelper.WriteAscii(Buffer, FileOffset, value, 128);
    }

    int IDhcpServerPacketWritable.OptionsMagicCookie
    {
        get => OptionsMagicCookie;
        set => BitHelper.Write(Buffer, MagicCookieOffset, value);
    }

    #endregion

    #region TryAddOption

    /// <summary>
    /// Try to add an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was added else false</returns>
    public bool TryAddOption(DhcpServerOptionIds optionId, byte[] data)
    {
        if (!TryGetOptionIndex(DhcpServerOptionIds.End, out var endIndex))
        {
            endIndex = GetLength();
        }
        return TryWriteOption(optionId, data, endIndex, isUpdate: false);
    }

    /// <summary>
    /// Try to add an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was added else false</returns>

    public bool TryAddOption(DhcpServerOptionIds optionId, long data)
    {
        if (!TryGetOptionIndex(DhcpServerOptionIds.End, out var endIndex))
            endIndex = GetLength();
        return TryWriteOption(optionId, data, endIndex, isUpdate: false);
    }

    /// <summary>
    /// Try to add an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was added else false</returns>
    public bool TryAddOption(DhcpServerOptionIds optionId, int data)
    {
        if (!TryGetOptionIndex(DhcpServerOptionIds.End, out var endIndex))
            endIndex = GetLength();
        return TryWriteOption(optionId, data, endIndex, isUpdate: false);
    }

    /// <summary>
    /// Try to add an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was added else false</returns>
    public bool TryAddOption(DhcpServerOptionIds optionId, short data)
    {
        if (!TryGetOptionIndex(DhcpServerOptionIds.End, out var endIndex))
            endIndex = GetLength();
        return TryWriteOption(optionId, data, endIndex, isUpdate: false);
    }

    /// <summary>
    /// Try to add an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was added else false</returns>
    public bool TryAddOption(DhcpServerOptionIds optionId, byte data)
    {
        if (!TryGetOptionIndex(DhcpServerOptionIds.End, out var endIndex))
            endIndex = GetLength();
        return TryWriteOption(optionId, data, endIndex, isUpdate: false);
    }

    /// <summary>
    /// Try to add an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was added else false</returns>

    public bool TryAddOption(DhcpServerOptionIds optionId, string data)
    {
        if (!TryGetOptionIndex(DhcpServerOptionIds.End, out var endIndex))
            endIndex = GetLength();
        return TryWriteOption(optionId, data, endIndex, isUpdate: false);
    }

    /// <summary>
    /// Try to add an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was added else false</returns>
    public bool TryAddOption(DhcpServerOptionIds optionId, DhcpServerIpAddress data)
        => TryAddOption(optionId, (int)data.Native);

    #endregion

    #region TryAddOrUpdateOption

    /// <summary>
    /// Try to add or update an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was added or updated else false</returns>
    public bool TryAddOrUpdateOption(DhcpServerOptionIds optionId, byte[] data)
    {
        if (TryGetOptionIndex(optionId, out var index))
            return TryWriteOption(optionId, data, index, isUpdate: true);
        if (!TryGetOptionIndex(DhcpServerOptionIds.End, out var endIndex))
            endIndex = GetLength();
        return TryWriteOption(optionId, data, endIndex, isUpdate: false);
    }

    /// <summary>
    /// Try to add or update an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was added or updated else false</returns>
    public bool TryAddOrUpdateOption(DhcpServerOptionIds optionId, long data)
    {
        if (TryGetOptionIndex(optionId, out var index))
            return TryWriteOption(optionId, data, index, isUpdate: true);
        if (!TryGetOptionIndex(DhcpServerOptionIds.End, out var endIndex))
            endIndex = GetLength();
        return TryWriteOption(optionId, data, endIndex, isUpdate: false);
    }

    /// <summary>
    /// Try to add or update an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was added or updated else false</returns>
    public bool TryAddOrUpdateOption(DhcpServerOptionIds optionId, int data)
    {
        if (TryGetOptionIndex(optionId, out var index))
            return TryWriteOption(optionId, data, index, isUpdate: true);
        if (!TryGetOptionIndex(DhcpServerOptionIds.End, out var endIndex))
            endIndex = GetLength();
        return TryWriteOption(optionId, data, endIndex, isUpdate: false);
    }

    /// <summary>
    /// Try to add or update an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was added or updated else false</returns>
    public bool TryAddOrUpdateOption(DhcpServerOptionIds optionId, short data)
    {
        if (TryGetOptionIndex(optionId, out var index))
            return TryWriteOption(optionId, data, index, isUpdate: true);
        if (!TryGetOptionIndex(DhcpServerOptionIds.End, out var endIndex))
            endIndex = GetLength();
        return TryWriteOption(optionId, data, endIndex, isUpdate: false);
    }

    /// <summary>
    /// Try to add or update an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was added or updated else false</returns>
    public bool TryAddOrUpdateOption(DhcpServerOptionIds optionId, byte data)
    {
        if (TryGetOptionIndex(optionId, out var index))
        {
            return TryWriteOption(optionId, data, index, isUpdate: true);
        }

        if (!TryGetOptionIndex(DhcpServerOptionIds.End, out var endIndex))
        {
            endIndex = GetLength();
        }
        return TryWriteOption(optionId, data, endIndex, isUpdate: false);
    }

    /// <summary>
    /// Try to add or update an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was added or updated else false</returns>
    public bool TryAddOrUpdateOption(DhcpServerOptionIds optionId, string data)
    {
        if (TryGetOptionIndex(optionId, out var index))
        {
            return TryWriteOption(optionId, data, index, isUpdate: true);
        }

        if (!TryGetOptionIndex(DhcpServerOptionIds.End, out var endIndex))
        {
            endIndex = GetLength();
        }

        return TryWriteOption(optionId, data, endIndex, isUpdate: false);
    }

    /// <summary>
    /// Try to add or update an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was added or updated else false</returns>
    public bool TryAddOrUpdateOption(DhcpServerOptionIds optionId, DhcpServerIpAddress data)
        => TryAddOrUpdateOption(optionId, (int)data.Native);

    #endregion

    #region TryUpdateOption

    /// <summary>
    /// Try to update an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was updated else false</returns>
    public bool TryUpdateOption(DhcpServerOptionIds optionId, byte[] data)
    {
        return TryGetOptionIndex(optionId, out var index) && TryWriteOption(optionId, data, index, true);
    }

    /// <summary>
    /// Try to update an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was updated else false</returns>
    public bool TryUpdateOption(DhcpServerOptionIds optionId, long data)
    {
        return TryGetOptionIndex(optionId, out var index) && TryWriteOption(optionId, data, index, true);
    }

    /// <summary>
    /// Try to update an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was updated else false</returns>

    public bool TryUpdateOption(DhcpServerOptionIds optionId, int data)
    {
        return TryGetOptionIndex(optionId, out var index) && TryWriteOption(optionId, data, index, true);
    }

    /// <summary>
    /// Try to update an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was updated else false</returns>
    public bool TryUpdateOption(DhcpServerOptionIds optionId, short data)
    {
        return TryGetOptionIndex(optionId, out var index) && TryWriteOption(optionId, data, index, true);
    }

    /// <summary>
    /// Try to update an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was updated else false</returns>
    public bool TryUpdateOption(DhcpServerOptionIds optionId, byte data)
    {
        return TryGetOptionIndex(optionId, out var index) && TryWriteOption(optionId, data, index, true);
    }

    /// <summary>
    /// Try to update an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was updated else false</returns>
    public bool TryUpdateOption(DhcpServerOptionIds optionId, string data)
    {
        return TryGetOptionIndex(optionId, out var index) && TryWriteOption(optionId, data, index, true);
    }

    /// <summary>
    /// Try to update an option
    /// </summary>
    /// <param name="optionId">Option ID</param>
    /// <param name="data">Option data</param>
    /// <returns>True if the option was updated else false</returns>
    public bool TryUpdateOption(DhcpServerOptionIds optionId, DhcpServerIpAddress data)
        => TryUpdateOption(optionId, (int)data.Native);

    #endregion

    #region TryWriteOption

    private bool TryPrepWriteOption(DhcpServerOptionIds optionId, int dataLength, int index, bool isUpdate, out int dataIndex)
    {
        var buffer = BufferInternal ?? Buffer;
        dataIndex = -1;

        switch (optionId)
        {
            case DhcpServerOptionIds.Pad:
            case DhcpServerOptionIds.End:
                // 0-byte fixed length
                if (dataLength != 0)
                {
                    return false;
                }

                if (!isUpdate)
                {
                    if (!TryAdjustBuffer(index, 1))
                    {
                        return false;
                    }

                    buffer[index] = (byte)optionId;
                }

                dataIndex = ++index;
                return true;
            case DhcpServerOptionIds.SubnetMask:
            case DhcpServerOptionIds.TimeOffset:
                // 4-byte fixed length
                if (dataLength != 4)
                {
                    return false;
                }

                if (!isUpdate)
                {
                    if (!TryAdjustBuffer(index, 5))
                    {
                        return false;
                    }

                    buffer[index] = (byte)optionId;
                }

                dataIndex = ++index;
                IsBufferModified = true;
                return true;
            default:
                // variable length

                if (!isUpdate)
                {
                    if (dataLength > byte.MaxValue)
                    {
                        return false;
                    }

                    if (!TryAdjustBuffer(index, 2 + dataLength))
                    {
                        return false;
                    }

                    buffer[index++] = (byte)optionId;
                    buffer[index++] = (byte)dataLength;
                    dataIndex = index;
                    IsBufferModified = true;
                    return true;
                }

                var existingLength = buffer[++index];

                switch (dataLength)
                {
                    case 0:
                        return existingLength == 0 ||
                               // no update
                               TryAdjustBuffer(++index, -existingLength); // remove data
                    case > byte.MaxValue:
                        return false;
                }

                if (existingLength == dataLength)
                {
                    dataIndex = ++index;
                    IsBufferModified = true;
                    return true;
                }

                // update option length
                buffer[index++] = (byte)dataLength;

                if (!TryAdjustBuffer(index, dataLength - existingLength))
                {
                    return false;
                }

                dataIndex = index;
                return true;
        }
    }

    private bool TryWriteOption(DhcpServerOptionIds optionId, byte[] data, int index, bool isUpdate)
    {
        var dataLength = data?.Length ?? 0;

        if (!TryPrepWriteOption(optionId, dataLength, index, isUpdate, out var dataIndex))
        {
            return false;
        }

        if (dataLength <= 0)
        {
            return true;
        }

        ArgumentNullException.ThrowIfNull(data);
        Array.Copy(data, 0, BufferInternal ?? Buffer, dataIndex, dataLength);

        return true;
    }

    private bool TryWriteOption(DhcpServerOptionIds optionId, long data, int index, bool isUpdate)
    {
        if (!TryPrepWriteOption(optionId, BitHelper.Int64Size, index, isUpdate, out var dataIndex))
        {
            return false;
        }

        BitHelper.Write(BufferInternal ?? Buffer, dataIndex, data);
        return true;
    }

    private bool TryWriteOption(DhcpServerOptionIds optionId, int data, int index, bool isUpdate)
    {
        if (!TryPrepWriteOption(optionId, BitHelper.Int32Size, index, isUpdate, out var dataIndex))
        {
            return false;
        }

        BitHelper.Write(BufferInternal ?? Buffer, dataIndex, data);
        return true;
    }

    private bool TryWriteOption(DhcpServerOptionIds optionId, short data, int index, bool isUpdate)
    {
        if (!TryPrepWriteOption(optionId, BitHelper.Int16Size, index, isUpdate, out var dataIndex))
        {
            return false;
        }

        BitHelper.Write(BufferInternal ?? Buffer, dataIndex, data);
        return true;
    }

    private bool TryWriteOption(DhcpServerOptionIds optionId, byte data, int index, bool isUpdate)
    {
        if (!TryPrepWriteOption(optionId, BitHelper.ByteSize, index, isUpdate, out var dataIndex))
        {
            return false;
        }

        BitHelper.Write(BufferInternal ?? Buffer, dataIndex, data);
        return true;
    }

    private bool TryWriteOption(DhcpServerOptionIds optionId, string data, int index, bool isUpdate)
    {
        if (!TryPrepWriteOption(optionId, data.Length, index, isUpdate, out var dataIndex))
        {
            return false;
        }

        BitHelper.WriteAscii(BufferInternal ?? Buffer, dataIndex, data);
        return true;
    }

    #endregion

}