// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Database.Interfaces;
using Bodoconsult.Database.SqlClient;
using Bodoconsult.Web.Mail.Helpers;
using BodoFileTransfer.Business.Helpers;
using BodoFileTransfer.Business.Interfaces;
using BodoFileTransfer.Business.Model;
using Microsoft.Data.SqlClient;

namespace BodoFileTransfer.Business.DataHandling;

/// <summary>
/// Get data from BodoFileTransfer database
/// </summary>
/// <inheritdoc />
public class DataHandler : IDataHandler
{
    private readonly string _errorMailAddressee;

    private readonly IConnManager _db;

    private readonly string _connectionString;

    private IList<Account> _allAccounts;

    private IList<O365Account> _allO365Accounts;

    private IList<O365SenderAccount> _allO365SenderAccounts;

    public DataHandler(IAppGlobals appGlobals)
    {
        _connectionString = appGlobals.AppStartParameter.DefaultConnectionString;
        _errorMailAddressee = appGlobals.AppStartParameter.ErrorMailAddress;
        _db = new SqlClientConnManager(_connectionString);
    }


    /// <summary>
    /// Gets all necessary accounts with their data
    /// </summary>
    /// <returns></returns>
    public IList<Account> GetAllAccounts()
    {
        if (_allAccounts != null)
        {
            return _allAccounts;
        }

        _allAccounts = new List<Account>();

        var dt = _db.GetDataTable("EXEC [dbo].[spInbox_GetAllInboxes]");
        foreach (DataRow r in dt.Rows)
        {

            var a = new Account
            {
                Id = Convert.ToInt32(r["A_ID"].ToString()),
                Keyword = r["A_Keyword"].ToString(),
                InboxPath = r["A_InboxPath"].ToString(),
                ArchivePath = r["A_ArchivePath"].ToString(),
                DocCounter = Convert.ToInt32(r["A_DocCounter"].ToString()),
                Name = r["A_Name"].ToString(),
                ManualTransfer = Convert.ToBoolean(r["A_ManualTransfer"].ToString()),
            };

            _allAccounts.Add(a);
        }

        return _allAccounts;
    }


    /// <summary>
    /// Adds 1 to the account's document counter
    /// </summary>
    /// <param name="accountId"></param>
    public void UpdateDocCounter(int accountId)
    {
        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "dbo.[spAccount_UpdateDocCounter]"
        };
        lo.Parameters.Add("@A_ID", SqlDbType.Int).Value = accountId;
        _db.Exec(lo);
    }

    /// <summary>
    /// Send a email to the error mail receiver
    /// </summary>
    /// <param name="subject">Subject for the mail to send</param>
    /// <param name="message">Message for the mail to send</param>
    public void SendMail(string subject, string message)
    {
        try
        {
            var lo = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "dbo.[spMAIL_New]"
            };
            lo.Parameters.Add("@To", SqlDbType.VarChar).Value = _errorMailAddressee;

            lo.Parameters.Add("@Subject", SqlDbType.VarChar).Value = $"BodoFileTransfer:{subject}";

            lo.Parameters.Add("@Msg", SqlDbType.VarChar).Value = message;


            _db.Exec(lo);
        }
        catch (Exception e)
        {
            throw new Exception("DataHandler:SendMail", e);
        }
    }


    /// <summary>
    /// Send a email to the error mail receiver
    /// </summary>
    /// <param name="mailTo">Mail to address</param>
    /// <param name="subject">Subject for the mail to send</param>
    /// <param name="message">Message for the mail to send</param>
    /// <param name="attachments">Attachment paths for the mail to send</param>
    /// <param name="zip">ZIP the attachments</param>
    /// <param name="zipPassword">Password for the ZIP file with the attachments</param>
    public void SendMail(string mailTo, string subject, string message, string attachments, bool zip=false, string zipPassword=null)
    {
        try
        {
            var lo = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "dbo.[spMAIL_New]"
            };
            lo.Parameters.Add("@To", SqlDbType.VarChar).Value = mailTo;

            lo.Parameters.Add("@Subject", SqlDbType.VarChar).Value = $"BodoFileTransfer:{subject}";

            lo.Parameters.Add("@Msg", SqlDbType.VarChar).Value = message;

            lo.Parameters.Add("@Att", SqlDbType.VarChar).Value = attachments;

            lo.Parameters.Add("@Zip", SqlDbType.Bit).Value = zip;

            lo.Parameters.Add("@ZipPassword", SqlDbType.VarChar).Value = zipPassword;

            _db.Exec(lo);
        }
        catch (Exception e)
        {
            throw new Exception("DataHandler:SendMail", e);
        }
    }


    /// <summary>
    /// Add a file to the files table for transfer
    /// </summary>
    /// <param name="file">Document file</param>
    public void AddFile(DocumentFile file)
    {
        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "[dbo].[spFiles_New]",
        };

        lo.Parameters.Add("@FileID", SqlDbType.UniqueIdentifier)
            .Value = file.Id;

        lo.Parameters.Add("@Path", SqlDbType.VarChar)
            .Value = file.Path;

        lo.Parameters.Add("@Title", SqlDbType.VarChar)
            .Value = file.Title;

        lo.Parameters.Add("@Keyword", SqlDbType.VarChar)
            .Value = file.Keyword;

        lo.Parameters.Add("@AccountId", SqlDbType.Int)
            .Value = file.AccountId;

        lo.Parameters.Add("@DateCreated", SqlDbType.DateTime)
            .Value = file.DateCreated;

        var conn = new SqlConnection(_connectionString);
        conn.InfoMessage += ConnOnInfoMessage;
        conn.Open();

        lo.Connection = conn;

        lo.ExecuteNonQuery();

        conn.Close();
        conn.Dispose();

        //_db.Exec(lo);
    }

    private void ConnOnInfoMessage(object sender, SqlInfoMessageEventArgs e)
    {
        Debug.Print(e.Message);
    }


    /// <summary>
    /// Add a file to the files archive table for archiving
    /// </summary>
    public void AddFileToArchive(DocumentFile file)
    {
        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "dbo.[spFilesArchive_New]"
        };

        lo.Parameters.Add("@FileID", SqlDbType.UniqueIdentifier).Value = file.Id;

        lo.Parameters.Add("@Path", SqlDbType.VarChar).Value = file.Path;

        lo.Parameters.Add("@Title", SqlDbType.VarChar).Value = file.Title;

        lo.Parameters.Add("@Keyword", SqlDbType.VarChar).Value = file.Keyword;

        lo.Parameters.Add("@AccountId", SqlDbType.Int).Value = file.AccountId;

        lo.Parameters.Add("@DateCreated", SqlDbType.DateTime).Value = file.DateCreated;

        lo.Parameters.Add("@SendDate", SqlDbType.DateTime).Value = DateTime.Now;

        _db.Exec(lo);
    }

    /// <summary>
    /// Delete a file from the files table
    /// </summary>
    public void DeleteFile(Guid fileId)
    {
        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "[dbo].[spFiles_Delete]"
        };

        lo.Parameters.Add("@FileID", SqlDbType.UniqueIdentifier).Value = fileId;

        _db.Exec(lo);
    }


    ///// <summary>
    ///// Transfer files to send via SMTP
    ///// </summary>
    ///// <returns>true if error</returns>
    //public bool TransferSmtpMails()
    //{
    //    try
    //    {
    //        _db.Exec("EXEC [dbo].[spMail_SendMailsForTransferTargets]");
    //        return false;
    //    }
    //    catch (Exception e)
    //    {
    //        SendMail("BodoFileTransfer: Error", "TransferSmtpMails: " + e.Message);
    //        return true;
    //    }
    //}


    ///// <summary>
    ///// Transfer SmtpMails for one account
    ///// </summary>
    ///// <param name="accountId">ID of the account</param>
    ///// <returns></returns>
    //public bool TransferSmtpMailForAccount(int accountId)
    //{
    //    try
    //    {

    //        var lo = new SqlCommand
    //        {
    //            CommandType = CommandType.StoredProcedure,
    //            CommandText = "[dbo].[spMail_SendFilesForAccount]"
    //        };
    //        lo.Parameters.Add("@AccountId", SqlDbType.Int);
    //        lo.Parameters[0].Value = accountId;
    //        _db.Exec(lo);
    //        return false;
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Print(e.Message);
    //        return true;
    //    }

    //}


    /// <summary>
    /// Get all IMAP accounts to process
    /// </summary>
    /// <returns></returns>
    public IList<ImapAccount> GetAllImapAccounts()
    {
        var result = new List<ImapAccount>();

        var dt = _db.GetDataTable("EXEC dbo.spImapMail_GetAllAccounts");
        foreach (DataRow r in dt.Rows)
        {

            var a = new ImapAccount(this)
            {
                Id = Convert.ToInt32(r["IM_ID"].ToString()),
                Host = r["IM_Host"].ToString(),
                Port = Convert.ToInt32(r["IM_Port"].ToString()),
                UserName = PasswordHandler.Decrypt(r["IM_UserName"].ToString()),
                Password = PasswordHandler.Decrypt(r["IM_Password"].ToString()),
                InboxPath = r["A_InboxPath"].ToString(),
                AccountId = Convert.ToInt32(r["A_ID"].ToString()),
                FileExtensionFilter = r["IM_FileExtensionFilter"].ToString(),
                SubjectFilter = r["IM_SubjectFilter"].ToString(),
                UseSsl = Convert.ToBoolean(r["IM_UseSsl"].ToString())
            };

            result.Add(a);
        }

        return result;
    }


    /// <summary>
    /// Check in the database if a file with same name has been stored already
    /// </summary>
    /// <param name="originalFileName">file name to search (without directory path)</param>
    /// <param name="accountId">ID of the BFT account</param>
    /// <returns>true if file already exists</returns>
    public bool CheckIfFileExists(string originalFileName, int accountId)
    {
        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "dbo.spFiles_CheckIfExisting"
        };
        lo.Parameters.Add("@Path", SqlDbType.VarChar).Value = originalFileName;

        lo.Parameters.Add("@AccountId", SqlDbType.Int).Value = accountId;

        var result = _db.ExecWithResult(lo);

        return Convert.ToBoolean(result);
    }

    /// <summary>
    /// Add a trace entry
    /// </summary>
    /// <param name="entry">Trace log entry</param>
    public void AddTrace(TraceEntry entry)
    {
        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "dbo.spTraceAdd"
        };

        lo.Parameters.Add("@T_ID", SqlDbType.UniqueIdentifier).Value = entry.Id;

        lo.Parameters.Add("@F_ID", SqlDbType.UniqueIdentifier).Value = entry.FileId;

        lo.Parameters.Add("@Code", SqlDbType.Int).Value = entry.MessageCode;

        lo.Parameters.Add("@Message", SqlDbType.NVarChar).Value = entry.Message;

        lo.Parameters.Add("@Date", SqlDbType.DateTime).Value = entry.Date;

        _db.Exec(lo);

    }


    /// <summary>
    /// Get document files for an account
    /// </summary>
    /// <param name="accountId">Acccount ID</param>
    /// <returns>List with document files</returns>
    public IList<DocumentFile> GetFilesForAnAccount(int accountId)
    {
        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "[dbo].[spFiles_GetFilesForAccount]"
        };
        lo.Parameters.Add("@AccountId", SqlDbType.Int).Value = accountId;


        var dt = _db.GetDataTable(lo);

        var result = new List<DocumentFile>();

        foreach (DataRow row in dt.Rows)
        {
            var doc = DataHelper.MapDataRowToDocumentFile(row);
            result.Add(doc);
        }

        return result;
    }

    /// <summary>
    /// Get all traces for a file
    /// </summary>
    /// <param name="fileId">Current file ID</param>
    /// <returns>List of all traces stored for the file</returns>
    public IList<TraceEntry> GetTracesForFile(Guid fileId)
    {

        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "[dbo].[spTrace_GetTracesForFile]"
        };

        lo.Parameters.Add("@FileId", SqlDbType.UniqueIdentifier).Value = fileId;

        var dt = _db.GetDataTable(lo);

        var result = new List<TraceEntry>();

        foreach (DataRow row in dt.Rows)
        {
            var doc = DataHelper.MapDataRowToTraceEntry(row);
            result.Add(doc);
        }

        return result;
    }

    /// <summary>
    /// Get the number of document files
    /// </summary>
    /// <returns>number of document files</returns>
    public int GetFilesCount()
    {
        var s = _db.ExecWithResult("SELECT COUNT(*) FROM dbo.t_Files;");
        return Convert.ToInt32(s);
    }

    /// <summary>
    /// Get the number of document files in the archive
    /// </summary>
    /// <returns>number of document files in the archive</returns>
    public int GetFilesArchiveCount()
    {
        var s = _db.ExecWithResult("SELECT COUNT(*) FROM dbo.t_FilesArchive;");
        return Convert.ToInt32(s);
    }

    /// <summary>
    /// Delete a file from archive
    /// </summary>
    /// <param name="fileId">ID of the document entity to delete</param>
    public void DeleteFileArchive(Guid fileId)
    {
        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "[dbo].[spFilesArchive_Delete]"
        };

        lo.Parameters.Add("@F_ID", SqlDbType.UniqueIdentifier).Value = fileId;

        _db.Exec(lo);
    }

    /// <summary>
    /// Delete a file by its name
    /// </summary>
    /// <param name="fileName">Full filename</param>
    public void DeleteFile(string fileName)
    {
        var sql = $"DELETE FROM t_Files WHERE F_path='{fileName}'; DELETE FROM t_FilesArchive WHERE F_path='{fileName}'; ";
        _db.Exec(sql);
    }

    /// <summary>
    /// Get a single document file
    /// </summary>
    /// <param name="fileId">ID of the document entity</param>
    /// <returns>Document entity</returns>
    public DocumentFile GetFile(Guid fileId)
    {
        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "[dbo].[spFiles_GetFile]"
        };
        lo.Parameters.Add("@FileId", SqlDbType.UniqueIdentifier).Value = fileId;


        var dt = _db.GetDataTable(lo);

        var row = dt.Rows[0];
        var doc = DataHelper.MapDataRowToDocumentFile(row);
        return doc;
    }

    /// <summary>
    /// Get a single document file from archive
    /// </summary>
    /// <param name="fileId">ID of the document entity</param>
    /// <returns>Document entity</returns>
    public DocumentFile GetFileArchive(Guid fileId)
    {
        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "[dbo].[spFilesArchive_GetFile]"
        };

        lo.Parameters.Add("@FileId", SqlDbType.UniqueIdentifier).Value = fileId;


        var dt = _db.GetDataTable(lo);

        var row = dt.Rows[0];
        var doc = DataHelper.MapDataRowToDocumentFile(row);
        return doc;
    }

    /// <summary>
    /// Get the required mail data for an account
    /// </summary>
    /// <param name="accountId">Account ID</param>
    /// <returns>Mail data</returns>
    public AccountMailData GetAccountMailData(int accountId)
    {
        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "[dbo].[spAccount_GetMailData]"
        };
        lo.Parameters.Add("@AccountId", SqlDbType.Int).Value = accountId;

        var dt = _db.GetDataTable(lo);

        var row = dt.Rows[0];
        var doc = DataHelper.MapDataRowToAccountMailData(row);
        return doc;
    }

    /// <summary>
    /// Get all accounts requiring a mail transfer
    /// </summary>
    /// <returns>List with account IDs</returns>
    public IList<int> GetMailTransferAccounts()
    {
        var result = new List<int>();

        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "[dbo].[spAccount_GetTransferAccounts]"
        };

        var dt = _db.GetDataTable(lo);

        foreach (DataRow row in dt.Rows)
        {

            var id = Convert.ToInt32(row["A_ID"]);

            result.Add(id);
        }

        return result;
    }

    public IList<DocumentFile> GetFilesArchiveForAnAccount(int accountId)
    {
        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "[dbo].[spFilesArchive_GetFilesForAccount]"
        };
        lo.Parameters.Add("@AccountId", SqlDbType.Int).Value = accountId;


        var dt = _db.GetDataTable(lo);

        var result = new List<DocumentFile>();

        foreach (DataRow row in dt.Rows)
        {
            var doc = DataHelper.MapDataRowToDocumentFile(row);
            result.Add(doc);
        }

        return result;
    }

    public IList<DocumentFile> GetFilesForAnAccountByDate(int accountId, DateTime @from, DateTime until)
    {
        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "[dbo].[spFiles_GetFilesForAccountByDate]"
        };
        lo.Parameters.Add("@AccountId", SqlDbType.Int).Value = accountId;

        lo.Parameters.Add("@DV", SqlDbType.DateTime).Value = from;

        lo.Parameters.Add("@DB", SqlDbType.DateTime).Value = until;


        var dt = _db.GetDataTable(lo);

        var result = new List<DocumentFile>();

        foreach (DataRow row in dt.Rows)
        {
            var doc = DataHelper.MapDataRowToDocumentFile(row);
            result.Add(doc);
        }

        return result;
    }

    public IList<DocumentFile> GetFilesArchiveForAnAccountByDate(int accountId, DateTime @from, DateTime until)
    {
        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "[dbo].[spFilesArchive_GetFilesForAccountByDate]"
        };
        lo.Parameters.Add("@AccountId", SqlDbType.Int).Value = accountId;

        lo.Parameters.Add("@DV", SqlDbType.DateTime).Value = from;

        lo.Parameters.Add("@DB", SqlDbType.DateTime).Value = until;


        var dt = _db.GetDataTable(lo);

        var result = new List<DocumentFile>();

        foreach (DataRow row in dt.Rows)
        {
            var doc = DataHelper.MapDataRowToDocumentFile(row);
            result.Add(doc);
        }

        return result;
    }

    /// <summary>
    /// Get all O365 accounts from database
    /// </summary>
    /// <returns>List with all O365 accounts</returns>
    public IList<O365Account> GetAllO365Accounts()
    {

        if (_allO365Accounts != null)
        {
            return _allO365Accounts;
        }
            
        _allO365Accounts = new List<O365Account>();

        var dt = _db.GetDataTable("EXEC dbo.spO365Mail_GetAllAccounts");
        foreach (DataRow r in dt.Rows)
        {

            var a = new O365Account(this)
            {
                Id = Convert.ToInt32(r["O_ID"].ToString()),
                Instance = PasswordHandler.Decrypt(r["O_Instance"].ToString()),
                Tenant = PasswordHandler.Decrypt(r["O_Tenant"].ToString()),
                ClientId = PasswordHandler.Decrypt(r["O_ClientId"].ToString()),
                ClientSecret = PasswordHandler.Decrypt(r["O_ClientSecret"].ToString()),
                Scope = PasswordHandler.Decrypt(r["O_Scope"].ToString()),
                InboxPath = r["A_InboxPath"].ToString(),
                AccountId = Convert.ToInt32(r["A_ID"].ToString()),
                FileExtensionFilter = r["O_FileExtensionFilter"].ToString(),
                SubjectFilter = r["O_SubjectFilter"].ToString(),
                UserName = PasswordHandler.Decrypt(r["O_UserName"].ToString()),
                Name = r["O_Name"].ToString()
            };

            _allO365Accounts.Add(a);
        }

        return _allO365Accounts;
    }


    /// <summary>
    /// Get all O365 accounts from database used to sent mails
    /// </summary>
    /// <returns>List with all O365 accounts used to sent mails</returns>
    public IList<O365SenderAccount> GetAllO365SenderAccounts()
    {

        if (_allO365SenderAccounts != null)
        {
            return _allO365SenderAccounts;
        }

        _allO365SenderAccounts = new List<O365SenderAccount>();

        var dt = _db.GetDataTable("EXEC dbo.spO365MailSender_GetAllAccounts");
        foreach (DataRow r in dt.Rows)
        {

            var a = new O365SenderAccount
            {
                Id = Convert.ToInt32(r["OS_ID"].ToString()),
                Instance = PasswordHandler.Decrypt(r["OS_Instance"].ToString()),
                Tenant = PasswordHandler.Decrypt(r["OS_Tenant"].ToString()),
                ClientId = PasswordHandler.Decrypt(r["OS_ClientId"].ToString()),
                ClientSecret = PasswordHandler.Decrypt(r["OS_ClientSecret"].ToString()),
                Scope = PasswordHandler.Decrypt(r["OS_Scope"].ToString()),
                UserName = PasswordHandler.Decrypt(r["OS_UserName"].ToString()),
                Name = r["OS_Name"].ToString(),
                SizeOfAttachments = Convert.ToInt32(r["OS_SizeOfAttachments"].ToString()),
                NumberOfAttachments = Convert.ToInt32(r["OS_NumberOfAttachments"].ToString())
            };

            _allO365SenderAccounts.Add(a);
        }

        return _allO365SenderAccounts;
    }

    /// <summary>
    /// Get all files for delivery
    /// </summary>
    /// <returns>List with documents</returns>
    public IList<DocumentFile> GetFilesForAnAccountForDelivery(int accountId)
    {
        var lo = new SqlCommand
        {
            CommandType = CommandType.StoredProcedure,
            CommandText = "[dbo].[spFilesArchive_GetFilesForAccountForDelivery]"
        };
        lo.Parameters.Add("@AccountId", SqlDbType.Int).Value = accountId;

        var dt = _db.GetDataTable(lo);

        var result = new List<DocumentFile>();

        foreach (DataRow row in dt.Rows)
        {
            var doc = DataHelper.MapDataRowToDocumentFile(row);
            result.Add(doc);
        }

        return result;
    }
}