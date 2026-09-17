// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;

namespace BodoFileTransfer.Business.Interfaces;

/// <summary>
/// <see cref="IAppGlobals"/> enhancements for BodoFileTransfer app
/// </summary>
public interface IBodoFileTransferAppGlobals : IAppGlobals
{
    /// <summary>
    /// Number schema for numbering of Belege
    /// </summary>
    string NumberSchema { get; set; } 

    /// <summary>
    /// Weeks back to check emails
    /// </summary>
    int EmailSentDateWeeksBack { get; set; } 

    /// <summary>
    /// File extensions for signature files
    /// </summary>
    string SignatureFileExtensions { get; set; } 
}