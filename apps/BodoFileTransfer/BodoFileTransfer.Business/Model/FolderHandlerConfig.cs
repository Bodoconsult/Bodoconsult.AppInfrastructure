// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using BodoFileTransfer.Business.Interfaces;
using System;
using Bodoconsult.App.Abstractions.Interfaces;

namespace BodoFileTransfer.Business.Model;

/// <summary>
/// Configuration for folder handling
/// </summary>
public class FolderHandlerConfig : IFolderHandlerConfig
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current appsettings</param>
    public FolderHandlerConfig(IAppGlobals appGlobals)
    {
        if (appGlobals is not IBodoFileTransferAppGlobals globals)
        {
            throw new ArgumentException("AppGlobals is not IBodoFileTransferAppGlobals");
        }

        SentDateFilter = DateTime.Now.AddDays(-Convert.ToInt32(globals.EmailSentDateWeeksBack) * 7);
        SignatureFileExtensions = globals.SignatureFileExtensions;
    }

    /// <summary>
    /// Number schema for document counter number formatting. Default: "000000"
    /// </summary>
    public string NumberSchema { get; set; } = "00000000";

    /// <summary>
    /// Filter date for sent date: all amils sent since
    /// </summary>
    public DateTime SentDateFilter { get; set; }

    /// <summary>
    /// File extensions for signature files (Default: .ads;.cms)
    /// </summary>
    public string SignatureFileExtensions { get; set; } 
}