using System;
using System.Data;
using BodoFileTransfer.Business.Enums;
using BodoFileTransfer.Business.Model;

namespace BodoFileTransfer.Business.Helpers;

/// <summary>
/// Helper class for database data manipulation
/// </summary>
public static class DataHelper
{
    /// <summary>
    /// Map a data row to a DocumentFile entity
    /// </summary>
    /// <param name="row">DataRow object with DocumentFile data</param>
    /// <returns>DocumentFile entiy</returns>
    public static DocumentFile MapDataRowToDocumentFile(DataRow row)
    {

        var doc = new DocumentFile
        {
            AccountId = Convert.ToInt32(row["F_AccountId"].ToString()),
            Keyword = row["F_Keyword"].ToString(),
            Path = row["F_Path"].ToString(),
            Title = row["F_Title"].ToString()
        };

        var d = row["F_ID"].ToString();

        if (!string.IsNullOrEmpty(d))
        {
            doc.Id = new Guid(d);
        }

        d = row["F_DateCreated"].ToString();

        if (!string.IsNullOrEmpty(d))
        {
            doc.DateCreated = Convert.ToDateTime(d);
        }

        d = row["F_DateCreated"].ToString();

        if (!string.IsNullOrEmpty(d))
        {
            doc.SendDate = Convert.ToDateTime(d);
        }

        return doc;
    }


    /// <summary>
    /// Map a data row to a TraceEntry entity
    /// </summary>
    /// <param name="row">DataRow object with TraceEntry data</param>
    /// <returns>TraceEntry entity</returns>
    public static TraceEntry MapDataRowToTraceEntry(DataRow row)
    {

        var doc = new TraceEntry
        {
            MessageCode = (TraceMessageCode)Convert.ToInt32(row["T_Code"].ToString()),
            Message = row["T_Message"].ToString(),
        };

        var d = row["T_ID"].ToString();

        if (!string.IsNullOrEmpty(d))
        {
            doc.Id = new Guid(d);
        }

        d = row["T_F_ID"].ToString();

        if (!string.IsNullOrEmpty(d))
        {
            doc.FileId = new Guid(d);
        }

        d = row["T_Date"].ToString();

        if (!string.IsNullOrEmpty(d))
        {
            doc.Date = Convert.ToDateTime(d);
        }

        return doc;
    }

    /// <summary>
    /// Map a datarow with account mail data to an entity
    /// </summary>
    /// <param name="row">DataRow with account mail data</param>
    /// <returns>Account mail data entity</returns>
    public static AccountMailData MapDataRowToAccountMailData(DataRow row)
    {
        var amd = new AccountMailData
        {
            AccountName = row["A_name"].ToString(),
            Keyword = row["A_Keyword"].ToString(),
            Password = row["Password"].ToString(),
            MailAddress = row["MailAddress"].ToString(),
            MailType = (SendEmailsType) Convert.ToInt32( row["TT_Typ"].ToString()),
            Office365AccountId = Convert.ToInt32(row["TT_O365Sender_ID"].ToString())
        };

        return amd;
    }
}