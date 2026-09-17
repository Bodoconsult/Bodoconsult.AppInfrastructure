// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;

namespace BodoFileTransfer.Business.Interfaces;

public interface IMailAccount
{
    /// <summary>
    /// Current datahandler to use
    /// </summary>
    IDataHandler DataHandler { get; }

    /// <summary>
    /// Describing name of the property
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// ID of the mail account
    /// </summary>
    int Id { get; set; }

    /// <summary>
    /// Id of the BFT account
    /// </summary>
    int AccountId { get; set; }

    /// <summary>
    /// The account's inbox path
    /// </summary>
    string InboxPath { get; set; }

    /// <summary>
    /// File extensions for signature files (Default: .ads;.cms)
    /// </summary>
    string SignatureFileExtensions { get; set; }

    /// <summary>
    /// Filter date for sent date: all mails received since
    /// </summary>
    DateTime SentDateFilter { get; set; }

    /// <summary>
    /// Filter string for allowed file extensions. Example: .pdf,.docx for PDF and DOCX files
    /// </summary>
    string FileExtensionFilter { get; set; }

    /// <summary>
    /// Filter data by mail subject
    /// </summary>
    string SubjectFilter { get; set; }

    /// <summary>
    /// Save file to inbox
    /// </summary>
    /// <param name="originalfilename">Filename of the attachment</param>
    /// <param name="data">Data of the attachment</param>
    /// <returns>true on error</returns>
    bool FileHandling(string originalfilename, byte[] data);

    /// <summary>
    /// Check if a subject string fulfills subject filter expressions
    /// </summary>
    /// <param name="subject"></param>
    /// <returns>true if filter expressions are found in the subject</returns>
    bool CheckSubject(string subject);
}