using System.Collections.Generic;
using BodoFileTransferCore.Business.Model;

namespace BodoFileTransferCore.Business.DataHandling
{

    /// <summary>
    /// Interface for raw data handling
    /// </summary>
    public interface IDataHandler
    {
        /// <summary>
        /// Gets all necessary accounts with their data
        /// </summary>
        /// <returns></returns>
        IList<Account> GetAllAccounts();

        /// <summary>
        /// Adds 1 to the account's document counter
        /// </summary>
        /// <param name="accountId"></param>
        void UpdateDocCounter(int accountId);


        /// <summary>
        /// Send a email to the error mail receiver
        /// </summary>
        /// <param name="subject">Subject for the mail to send</param>
        /// <param name="message">Message for the mail to send</param>
        void SendMail(string subject, string message);



        /// <summary>
        /// Add a file to the files table for transfer
        /// </summary>
        /// <param name="path">full path of the file</param>
        /// <param name="title">title of the file</param>
        /// <param name="keyword">keyword for the type of the document, i.e. "Beleg", "Kontoauszug", ...</param>
        /// <param name="accountId">ID of the account</param>
        void AddFile(string path, string title, string keyword, int accountId);



        /// <summary>
        /// Transfer files to send via SMTP
        /// </summary>
        /// <returns>true if error</returns>
        bool TransferSmtpMails();


        /// <summary>
        /// Transfer SmtpMails for one account
        /// </summary>
        /// <param name="accountId">ID of the account</param>
        /// <returns></returns>
        bool TransferSmtpMailForAccount(int accountId);


        /// <summary>
        /// Get all IMAP accounts to process
        /// </summary>
        /// <returns></returns>
        IList<ImapAccount> GetAllImapAccounts();

        /// <summary>
        /// Check in the database if a file with same name has been stored already
        /// </summary>
        /// <param name="originalFileName">file name to search (without directory path)</param>
        /// <param name="accountId">ID of the BFT account</param>
        /// <returns>true if file already exists</returns>
        bool CheckIfFileExists(string originalFileName, int accountId);
    }
}