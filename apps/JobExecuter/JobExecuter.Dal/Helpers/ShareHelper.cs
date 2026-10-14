// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace JobExecuter.Dal.Helpers;

/// <summary>
/// Class with helper functionality for network shares
/// </summary>
public class ShareHelper
{

    #region Constants

    /// <summary>Maximum path length</summary>
    protected const int MaxPath = 260;
    /// <summary>No error</summary>
    protected const int NoError = 0;
    /// <summary>Access denied</summary>
    protected const int ErrorAccessDenied = 5;
    /// <summary>Access denied</summary>
    protected const int ErrorWrongLevel = 124;
    /// <summary>More data available</summary>
    protected const int ErrorMoreData = 234;
    /// <summary>Not connected</summary>
    protected const int ErrorNotConnected = 2250;
    /// <summary>Level 1</summary>
    protected const int UniversalNameInfoLevel = 1;
    /// <summary>Max extries (9x)</summary>
    protected const int MaxSi50Entries = 20;

    #endregion

    #region Structures

    /// <summary>Unc name</summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct UniversalNameInfo
    {
        /// <summary>
        /// Universal name
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)] public string lpUniversalName;
    }

    /// <summary>Share information, NT, level 2</summary>
    /// <remarks>
    /// Requires admin rights to work. 
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct ShareInfo2
    {
        /// <summary>
        /// Net name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string NetName;
        /// <summary>
        /// Share type
        /// </summary>
        public ShareType ShareType;
        /// <summary>
        /// Remark
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Remark;
        /// <summary>
        /// Permissions
        /// </summary>
        public int Permissions;
        /// <summary>
        /// Maximum number of users
        /// </summary>
        public int MaxUsers;
        /// <summary>
        /// Current users
        /// </summary>
        public int CurrentUsers;
        /// <summary>
        /// Local Path
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Path;

        /// <summary>
        /// Password
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Password;
    }

    /// <summary>Share information, NT, level 1</summary>
    /// <remarks>
    /// Fallback when no admin rights.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct ShareInfo1
    {
        /// <summary>
        /// Net name
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string NetName;
            
        /// <summary>
        /// Share type
        /// </summary>
        public ShareType ShareType;

        /// <summary>
        /// Remark
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string Remark;
    }

    /// <summary>Share information, Win9x</summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ShareInfo50
    {
        /// <summary>
        /// Net name
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string NetName;

        /// <summary>
        /// Share type byte
        /// </summary>
        public byte bShareType;

        /// <summary>
        /// Flags
        /// </summary>
        public ushort Flags;

        /// <summary>
        /// Remarks
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Remark;

        /// <summary>
        /// Local Path
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Path;

        /// <summary>
        /// Password read / write
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string PasswordRW;

        /// <summary>
        /// Password read-only
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string PasswordRO;

        /// <summary>
        /// Share type
        /// </summary>
        public ShareType ShareType => (ShareType)(bShareType & 0x7F);
    }

    /// <summary>Share information level 1, Win9x</summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ShareInfo1_9X
    {
        /// <summary>
        /// Net name
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string NetName;

        /// <summary>
        /// Padding
        /// </summary>
        public byte Padding;

        /// <summary>
        /// Share type byte
        /// </summary>
        public ushort bShareType;

        /// <summary>
        /// Remark
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Remark;

        /// <summary>
        /// Share type
        /// </summary>
        public ShareType ShareType => (ShareType)(bShareType & 0x7FFF);
    }

    #endregion


    #region Static methods

    #region Platform

    /// <summary>
    /// Is this an NT platform?
    /// </summary>
    protected static bool IsNt => PlatformID.Win32NT == Environment.OSVersion.Platform;

    /// <summary>
    /// Returns true if this is Windows 2000 or higher
    /// </summary>
    protected static bool IsW2KUp
    {
        get
        {
            var os = Environment.OSVersion;
            return PlatformID.Win32NT == os.Platform && os.Version.Major >= 5;
        }
    }

    #endregion

    /// <summary>
    /// Returns true if fileName is a valid local file-name of the form:
    /// X:\, where X is a drive letter from A-Z
    /// </summary>
    /// <param name="fileName">The filename to check</param>
    /// <returns></returns>
    public static bool IsValidFilePath(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) return false;

        var drive = char.ToUpper(fileName[0]);
        if ('A' > drive || drive > 'Z') return false;
        if (Path.VolumeSeparatorChar != fileName[1]) return false;
        return Path.DirectorySeparatorChar == fileName[2];
    }


    #region Functions

    /// <summary>Get a UNC name</summary>
    [DllImport("mpr", CharSet = CharSet.Auto)]
    protected static extern int WNetGetUniversalName(string lpLocalPath,
        int dwInfoLevel, ref UniversalNameInfo lpBuffer, ref int lpBufferSize);

    /// <summary>Get a UNC name</summary>
    [DllImport("mpr", CharSet = CharSet.Auto)]
    protected static extern int WNetGetUniversalName(string lpLocalPath,
        int dwInfoLevel, IntPtr lpBuffer, ref int lpBufferSize);

    /// <summary>Enumerate shares (NT)</summary>
    [DllImport("netapi32", CharSet = CharSet.Unicode)]
    protected static extern int NetShareEnum(string lpServerName, int dwLevel,
        out IntPtr lpBuffer, int dwPrefMaxLen, out int entriesRead,
        out int totalEntries, ref int hResume);

    /// <summary>Enumerate shares (9x)</summary>
    [DllImport("svrapi", CharSet = CharSet.Ansi)]
    protected static extern int NetShareEnum(
        [MarshalAs(UnmanagedType.LPTStr)] string lpServerName, int dwLevel,
        IntPtr lpBuffer, ushort cbBuffer, out ushort entriesRead,
        out ushort totalEntries);

    /// <summary>Free the buffer (NT)</summary>
    [DllImport("netapi32")]
    protected static extern int NetApiBufferFree(IntPtr lpBuffer);

    #region Enumerate shares

    /// <summary>
    /// Enumerates the shares on Windows NT
    /// </summary>
    /// <param name="server">The server name</param>
    /// <param name="shares">The ShareCollection</param>
    protected static void EnumerateSharesNt(string server, ShareCollection shares)
    {
        var level = 2;
        int entriesRead, totalEntries, nRet, hResume = 0;
        var pBuffer = IntPtr.Zero;

        try
        {
            nRet = NetShareEnum(server, level, out pBuffer, -1,
                out entriesRead, out totalEntries, ref hResume);

            if (ErrorAccessDenied == nRet)
            {
                //Need admin for level 2, drop to level 1
                level = 1;
                nRet = NetShareEnum(server, level, out pBuffer, -1,
                    out entriesRead, out totalEntries, ref hResume);
            }

            if (NoError == nRet && entriesRead > 0)
            {
                var t = 2 == level ? typeof(ShareInfo2) : typeof(ShareInfo1);
                var offset = Marshal.SizeOf(t);

                for (long i = 0, lpItem = pBuffer.ToInt64(); i < entriesRead; i++, lpItem += offset)
                {
                    var pItem = new IntPtr(lpItem);
                    if (1 == level)
                    {
                        var si = (ShareInfo1)Marshal.PtrToStructure(pItem, t);
                        shares.Add(si.NetName, string.Empty, si.ShareType, si.Remark);
                    }
                    else
                    {
                        var si = (ShareInfo2)Marshal.PtrToStructure(pItem, t);
                        shares.Add(si.NetName, si.Path, si.ShareType, si.Remark);
                    }
                }
            }

        }
        finally
        {
            // Clean up buffer allocated by system
            if (IntPtr.Zero != pBuffer)
                NetApiBufferFree(pBuffer);
        }
    }

    /// <summary>
    /// Enumerates the shares on Windows 9x
    /// </summary>
    /// <param name="server">The server name</param>
    /// <param name="shares">The ShareCollection</param>
    protected static void EnumerateShares9X(string server, ShareCollection shares)
    {
        var level = 50;
        var nRet = 0;
        ushort entriesRead, totalEntries;

        var t = typeof(ShareInfo50);
        var size = Marshal.SizeOf(t);
        var cbBuffer = (ushort)(MaxSi50Entries * size);
        //On Win9x, must allocate buffer before calling API
        var pBuffer = Marshal.AllocHGlobal(cbBuffer);

        try
        {
            nRet = NetShareEnum(server, level, pBuffer, cbBuffer,
                out entriesRead, out totalEntries);

            if (ErrorWrongLevel == nRet)
            {
                level = 1;
                t = typeof(ShareInfo1_9X);
                size = Marshal.SizeOf(t);

                nRet = NetShareEnum(server, level, pBuffer, cbBuffer,
                    out entriesRead, out totalEntries);
            }

            if (NoError == nRet || ErrorMoreData == nRet)
            {
                for (int i = 0, lpItem = pBuffer.ToInt32(); i < entriesRead; i++, lpItem += size)
                {
                    var pItem = new IntPtr(lpItem);

                    if (1 == level)
                    {
                        var si = (ShareInfo1_9X)Marshal.PtrToStructure(pItem, t);
                        shares.Add(si.NetName, string.Empty, si.ShareType, si.Remark);
                    }
                    else
                    {
                        var si = (ShareInfo50)Marshal.PtrToStructure(pItem, t);
                        shares.Add(si.NetName, si.Path, si.ShareType, si.Remark);
                    }
                }
            }
            else
                Console.WriteLine(nRet);

        }
        finally
        {
            //Clean up buffer
            Marshal.FreeHGlobal(pBuffer);
        }
    }

    /// <summary>
    /// Enumerates the shares
    /// </summary>
    /// <param name="server">The server name</param>
    /// <param name="shares">The ShareCollection</param>
    public static void EnumerateShares(string server, ShareCollection shares)
    {
        if (!string.IsNullOrEmpty(server) && !IsW2KUp)
        {
            server = server.ToUpper();

            // On NT4, 9x and Me, server has to start with "\\"
            if (!('\\' == server[0] && '\\' == server[1]))
                server = $@"\\{server}";
        }

        if (IsNt)
            EnumerateSharesNt(server, shares);
        else
            EnumerateShares9X(server, shares);
    }

    #endregion

    #endregion


    /// <summary>
    /// Returns the UNC path for a mapped drive or local share.
    /// </summary>
    /// <param name="fileName">The path to map</param>
    /// <returns>The UNC path (if available)</returns>
    public static string PathToUnc(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) return string.Empty;

        fileName = Path.GetFullPath(fileName);
        if (!IsValidFilePath(fileName)) return fileName;

        var nRet = 0;
        var rni = new UniversalNameInfo();
        var bufferSize = Marshal.SizeOf(rni);

        nRet = WNetGetUniversalName(
            fileName, UniversalNameInfoLevel,
            ref rni, ref bufferSize);

        if (ErrorMoreData == nRet)
        {
            var pBuffer = Marshal.AllocHGlobal(bufferSize); ;
            try
            {
                nRet = WNetGetUniversalName(
                    fileName, UniversalNameInfoLevel,
                    pBuffer, ref bufferSize);

                if (NoError == nRet)
                {
                    rni = (UniversalNameInfo)Marshal.PtrToStructure(pBuffer,
                        typeof(UniversalNameInfo));
                }
            }
            finally
            {
                Marshal.FreeHGlobal(pBuffer);
            }
        }

        switch (nRet)
        {
            case NoError:
                return rni.lpUniversalName;

            case ErrorNotConnected:
                // Local file-name
                var shi = LocalShares;
                if (shi == null) return null;
                var share = shi[fileName];

                if (share == null)
                {
                    for (var i = 0; i < shi.Count; i++)
                    {
                        var erg = shi[i];
                        if (!string.Equals(erg.Path.ToLower(), fileName.ToLower(), StringComparison.Ordinal)) continue;

                        share = erg;
                        break;
                    }
                }

                var path = share?.Path;
                if (string.IsNullOrEmpty(path))
                {
                    return null;

                }


                var index = path.Length;
                if (Path.DirectorySeparatorChar != path[path.Length - 1])
                    index++;

                if (index < fileName.Length)
                    fileName = fileName.Substring(index);
                else
                    fileName = string.Empty;

                fileName = Path.Combine(share.ToString(), fileName);

                return fileName;

            default:
                Console.WriteLine("Unknown return value: {0}", nRet);
                return string.Empty;
        }
    }

    /// <summary>
    /// Returns the local <see cref="Share"/> object with the best match
    /// to the specified path.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static Share PathToShare(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) return null;

        fileName = Path.GetFullPath(fileName);
        if (!IsValidFilePath(fileName)) return null;

        var shi = LocalShares;
        if (null == shi)
            return null;
        else
            return shi[fileName];
    }

    #endregion


    #region Local shares

    /// <summary>The local shares</summary>
    private static ShareCollection _local = null;

    /// <summary>
    /// Return the local shares
    /// </summary>
    public static ShareCollection LocalShares
    {
        get
        {
            if (null == _local) _local = new ShareCollection();

            return _local;
        }
    }

    /// <summary>
    /// Return the shares for a specified machine
    /// </summary>
    /// <param name="server"></param>
    /// <returns></returns>
    public static ShareCollection GetShares(string server)
    {
        return new ShareCollection(server);
    }

    #endregion
}