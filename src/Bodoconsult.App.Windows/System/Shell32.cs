// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Bodoconsult.App.Windows.System;

/// <summary>
/// Shell32 WIN API access class
/// </summary>
[SupportedOSPlatform("windows")]
public class Shell32
{
    /// <summary>
    /// Maximum path length
    /// </summary>
    public const int MaxPath = 256;
    /// <summary>
    /// BifReturnonlyfsdirs 
    /// </summary>
    public const uint BifReturnOnlyFsDirs = 1;
    /// <summary>
    /// BifDontGoBelowDomain
    /// </summary>
    public const uint BifDontGoBelowDomain = 2;
    /// <summary>
    /// BifStatustext
    /// </summary>
    public const uint BifStatusText = 4;
    /// <summary>
    /// BifReturnFsAncestors
    /// </summary>
    public const uint BifReturnFsAncestors = 8;
    /// <summary>
    /// BifEditbox
    /// </summary>
    public const uint BifEditbox = 16;
    /// <summary>
    /// BifValidate
    /// </summary>
    public const uint BifValidate = 32;
    /// <summary>
    /// BifNewDialogStyle
    /// </summary>
    public const uint BifNewDialogStyle = 64;
    /// <summary>
    /// BifUseNewUi
    /// </summary>
    public const uint BifUseNewUi = 80;
    /// <summary>
    /// BifBrowseincludeurls
    /// </summary>
    public const uint BifBrowseincludeurls = 128;
    /// <summary>
    /// BifBrowseforcomputer
    /// </summary>
    public const uint BifBrowseforcomputer = 4096;
    /// <summary>
    /// BifBrowseforprinter
    /// </summary>
    public const uint BifBrowseforprinter = 8192;
    /// <summary>
    /// BifBrowseincludefiles
    /// </summary>
    public const uint BifBrowseincludefiles = 16384;
    /// <summary>
    /// BifShareable 
    /// </summary>
    public const uint BifShareable = 32768;
    /// <summary>
    /// ShgfiIcon
    /// </summary>
    public const uint ShgfiIcon = 256;
    /// <summary>
    /// ShgfiDisplayname
    /// </summary>
    public const uint ShgfiDisplayname = 512;
    /// <summary>
    /// ShgfiTypename
    /// </summary>
    public const uint ShgfiTypename = 1024;
    /// <summary>
    /// ShgfiAttributes
    /// </summary>
    public const uint ShgfiAttributes = 2048;
    /// <summary>
    /// ShgfiIconlocation
    /// </summary>
    public const uint ShgfiIconlocation = 4096;
    /// <summary>
    /// ShgfiExetype
    /// </summary>
    public const uint ShgfiExetype = 8192;
    /// <summary>
    /// ShgfiSysiconindex
    /// </summary>
    public const uint ShgfiSysiconindex = 16384;
    /// <summary>
    /// ShgfiLinkoverlay
    /// </summary>
    public const uint ShgfiLinkoverlay = 32768;
    /// <summary>
    /// ShgfiSelected
    /// </summary>
    public const uint ShgfiSelected = 65536;
    /// <summary>
    /// ShgfiAttrSpecified
    /// </summary>
    public const uint ShgfiAttrSpecified = 131072;
    /// <summary>
    /// ShgfiLargeicon
    /// </summary>
    public const uint ShgfiLargeicon = 0;
    /// <summary>
    /// ShgfiSmallicon
    /// </summary>
    public const uint ShgfiSmallicon = 1;
    /// <summary>
    /// ShgfiOpenicon
    /// </summary>
    public const uint ShgfiOpenicon = 2;
    /// <summary>
    /// ShgfiShelliconsize
    /// </summary>
    public const uint ShgfiShelliconsize = 4;
    /// <summary>
    /// ShgfiPidl
    /// </summary>
    public const uint ShgfiPidl = 8;
    /// <summary>
    /// ShgfiUsefileattributes
    /// </summary>
    public const uint ShgfiUsefileattributes = 16;
    /// <summary>
    /// ShgfiAddoverlays
    /// </summary>
    public const uint ShgfiAddoverlays = 32;
    /// <summary>
    /// ShgfiOverlayindex 
    /// </summary>
    public const uint ShgfiOverlayindex = 64;
    /// <summary>
    /// FileAttributeDirectory
    /// </summary>
    public const uint FileAttributeDirectory = 16;
    /// <summary>
    /// FileAttributeNormal
    /// </summary>
    public const uint FileAttributeNormal = 128;

    /// <summary>
    /// Retrieves information about an object in the file system, such as a file, folder, directory, or drive root.
    /// See https://learn.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shgetfileinfoa for more details
    /// </summary>
    /// <param name="pszPath">File path</param>
    /// <param name="dwFileAttributes">File attributes</param>
    /// <param name="psfi">File info struct to fill with info</param>
    /// <param name="cbFileInfo">Size of psfi parameter in bytes</param>
    /// <param name="uFlags">The flags that specify the file information to retrieve. This parameter can be a combination of the following values.</param>
    /// <returns></returns>

    [DllImport("Shell32.dll")]
    public static extern IntPtr SHGetFileInfo(
        string? pszPath,
        uint dwFileAttributes,
        ref ShFileInfo psfi,
        uint cbFileInfo,
        uint uFlags);

    /// <summary>
    /// ShItemId
    /// </summary>
    public struct ShItemId
    {
        /// <summary>
        /// Cb
        /// </summary>
        public ushort Cb;

        /// <summary>
        /// AbId
        /// </summary>
        [MarshalAs(UnmanagedType.LPArray)]
        public byte[] AbId;
    }

    /// <summary>
    /// ItemIdList
    /// </summary>
    public struct ItemIdList
    {
        /// <summary>
        /// Mkid
        /// </summary>
        public ShItemId Mkid;
    }

    /// <summary>
    /// BrowseInfo
    /// </summary>
    public struct BrowseInfo
    {
        /// <summary>
        /// HwndOwner
        /// </summary>
        public IntPtr HwndOwner;
        /// <summary>
        /// PidlRoot
        /// </summary>
        public IntPtr PidlRoot;
        /// <summary>
        /// PszDisplayName
        /// </summary>
        public IntPtr PszDisplayName;
        /// <summary>
        /// LpszTitle
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string LpszTitle;
        /// <summary>
        /// UlFlags
        /// </summary>
        public uint UlFlags;
        /// <summary>
        /// Lpfn
        /// </summary>
        public IntPtr Lpfn;
        /// <summary>
        /// LParam
        /// </summary>
        public int LParam;
        /// <summary>
        /// IImage
        /// </summary>
        public IntPtr IImage;
    }

    /// <summary>
    /// Struct for file info.
    /// See https://learn.microsoft.com/en-us/windows/win32/api/shellapi/ns-shellapi-shfileinfoa for details
    /// </summary>
    public struct ShFileInfo
    {
        /// <summary>
        /// NameSize
        /// </summary>
        public const int NameSize = 80;

        /// <summary>
        /// A handle to the icon that represents the file
        /// </summary>
        public IntPtr HIcon;

        /// <summary>
        /// The index of the icon image within the system image list
        /// </summary>
        public int IIcon;

        /// <summary>
        /// An array of values that indicates the attributes of the file object
        /// </summary>
        public uint DwAttributes;

        /// <summary>
        /// A string that contains the name of the file as it appears in the Windows Shell, or the path and file name of the file that contains the icon representing the file
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string SzDisplayName;

        /// <summary>
        /// A string that describes the type of file
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string SzTypeName;
    }
}