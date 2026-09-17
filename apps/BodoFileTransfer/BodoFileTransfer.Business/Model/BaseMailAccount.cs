// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using Bodoconsult.App.Zip;
using BodoFileTransfer.Business.Interfaces;

namespace BodoFileTransfer.Business.Model;

public abstract class BaseMailAccount: IMailAccount
{
    private string _subjectFilter;

    private string[] _subjectFilterExpressions;
    private string _fileExtensionFilter;

    /// <summary>
    /// Current datahandler to use
    /// </summary>
    public IDataHandler DataHandler { get; protected set; }

    /// <summary>
    /// Describing name of the property
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ID of the mail account
    /// </summary>
    public int Id { get; set; }


    /// <summary>
    /// Id of the BFT account
    /// </summary>
    public int AccountId { get; set; }

    /// <summary>
    /// The account's inbox path
    /// </summary>
    public string InboxPath { get; set; }

    /// <summary>
    /// File extensions for signature files (Default: .ads;.cms)
    /// </summary>
    public string SignatureFileExtensions { get; set; }

    /// <summary>
    /// Filter date for sent date: all mails received since
    /// </summary>
    public DateTime SentDateFilter { get; set; }

    /// <summary>
    /// Filter string for allowed file extensions. Example: .pdf,.docx for PDF and DOCX files
    /// </summary>
    public string FileExtensionFilter
    {
        get => _fileExtensionFilter;
        set => _fileExtensionFilter = value.ToLowerInvariant();
    }

    /// <summary>
    /// Filter data by mail subject
    /// </summary>
    public string SubjectFilter

    {
        get => _subjectFilter;
        set
        {
            _subjectFilter = value;
            _subjectFilterExpressions = value.ToLowerInvariant().Split(',');
        }
    }

    /// <summary>
    /// Save file to inbox
    /// </summary>
    /// <param name="originalfilename">Filename of the attachment</param>
    /// <param name="data">Data of the attachment</param>
    /// <returns>true on error</returns>
    public virtual bool FileHandling(string originalfilename, byte[] data)
    {
        try
        {
            if (string.IsNullOrEmpty(InboxPath))
            {
                return true;
            }

            var ext = new FileInfo(originalfilename).Extension.ToLower();

            switch (ext)
            {
                case ".zip":
                    return CheckZipData(data);
                default:
                    return !MoveFile(originalfilename, data);
            }
        }
        catch (Exception e)
        {
            Debug.Print(e.Message);
            return true;
        }
    }

    private bool CheckZipData(byte[] data)
    {
        var uh = new UnZipHandler(data);

        foreach (var fe in uh.Files)
        {

            var ext = new FileInfo(fe.FileName).Extension;

            var fileData = uh.GetFileData(fe.Path);

            if (SignatureFileExtensions.Contains(ext))
            {
                MoveSignatureFile(fe.FileName, fileData);
            }
            else
            {
                MoveFile(fe.FileName, fileData);
            }

        }

        return true;
    }

    private bool MoveFile(string originalfilename, byte[] data)
    {
        var fileExists = DataHandler.CheckIfFileExists(originalfilename, AccountId);

        if (fileExists)
        {
            return false;
        }

        var fileName = Path.Combine(InboxPath, originalfilename);

#if DEBUG
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
#endif

        if (File.Exists(fileName))
        {
            return false;
        }

        File.WriteAllBytes(fileName, data);

        Debug.Print($"Save file to {fileName}");
        return true;
    }

    private bool MoveSignatureFile(string originalfilename, byte[] data)
    {

        var fileName = Path.Combine(InboxPath, originalfilename);

#if DEBUG
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
#endif

        if (File.Exists(fileName))
        {
            return false;
        }

        File.WriteAllBytes(fileName, data);

        Debug.Print($"Save file to {fileName}");
        return true;
    }


    /// <summary>
    /// Check if a subject string fulfills subject filter expressions
    /// </summary>
    /// <param name="subject"></param>
    /// <returns>true if filter expressions are found in the subject</returns>
    public virtual bool CheckSubject(string subject)
    {
        if (string.IsNullOrEmpty(subject))
        {
            return false;
        }

        var value = subject.ToLowerInvariant();

        foreach (var expr in _subjectFilterExpressions)
        {
            if (value.Contains(expr))
            {
                return true;
            }
        }

        return false;
    }
}