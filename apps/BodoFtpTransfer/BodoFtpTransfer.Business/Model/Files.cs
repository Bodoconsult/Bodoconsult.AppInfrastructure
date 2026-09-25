// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.ComponentModel.DataAnnotations;

namespace BodoFtpTransfer.Business.Model;

public class FtpFiles
{
    /// <summary>
    /// ID 
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Path  Maximum length: -1 chars
    /// </summary>
    [MaxLength(-1, ErrorMessage = "Maximum number of characters that can be entered for Path is -1!")]
    public string Path { get; set; }

    /// <summary>
    /// HashCode  Maximum length: -1 chars
    /// </summary>
    [MaxLength(-1, ErrorMessage = "Maximum number of characters that can be entered for HashCode is -1!")]
    public string HashCode { get; set; }

    /// <summary>
    /// PathRemote  Maximum length: -1 chars
    /// </summary>
    [MaxLength(-1, ErrorMessage = "Maximum number of characters that can be entered for PathRemote is -1!")]
    public string PathRemote { get; set; }

    /// <summary>
    /// Source  Maximum length: -1 chars
    /// </summary>
    [MaxLength(-1, ErrorMessage = "Maximum number of characters that can be entered for Source is -1!")]
    public string Source { get; set; }

    /// <summary>
    /// HashCodeNew  Maximum length: -1 chars
    /// </summary>
    [MaxLength(-1, ErrorMessage = "Maximum number of characters that can be entered for HashCodeNew is -1!")]
    public string HashCodeNew { get; set; }

    /// <summary>
    /// Type  Maximum length: -1 chars
    /// </summary>
    [MaxLength(-1, ErrorMessage = "Maximum number of characters that can be entered for Type is -1!")]
    public string Type { get; set; }

    /// <summary>
    /// Size 
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// SizeRemote 
    /// </summary>
    public long SizeRemote { get; set; }
}