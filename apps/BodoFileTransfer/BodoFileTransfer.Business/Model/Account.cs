// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace BodoFileTransfer.Business.Model;

/// <summary>
/// Represents a account folder
/// </summary>
public class Account
{

    /// <summary>
    /// ID of the account
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Keyword like Mandantennummer, customer number, etc...
    /// </summary>
    public string Keyword { get; set; }

    /// <summary>
    /// Inbox path for the account
    /// </summary>
    public string InboxPath { get; set; }

    /// <summary>
    /// Archive path for the account
    /// </summary>
    public string ArchivePath { get; set; }

    /// <summary>
    /// Current document counter
    /// </summary>
    public int DocCounter { get; set; }


    /// <summary>
    /// Account name
    /// </summary>
    public string Name { get; set; }


    /// <summary>
    /// Mail transfer only manually 
    /// </summary>
    public bool ManualTransfer { get; set; }

}