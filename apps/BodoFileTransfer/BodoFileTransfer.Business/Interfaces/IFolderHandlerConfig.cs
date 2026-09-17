using System;

namespace BodoFileTransfer.Business.Interfaces;

/// <summary>
/// Public interface for folder config settings
/// </summary>
public interface IFolderHandlerConfig
{
    /// <summary>
    /// Number schema for document counter number formatting. Default: "000000"
    /// </summary>
    string NumberSchema { get; set; }

    /// <summary>
    /// Filter date for sent date: all mails received since
    /// </summary>
    DateTime SentDateFilter { get; set; }

    /// <summary>
    /// File extensions for signature files (Default: .ads;.cms)
    /// </summary>
    string SignatureFileExtensions { get; set; }
}