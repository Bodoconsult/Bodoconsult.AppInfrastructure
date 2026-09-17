using System;

namespace BodoFileTransfer.Business.Model;

/// <summary>
/// Document file
/// </summary>
public class DocumentFile
{
    /// <summary>
    /// Unique ID of the file
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();


    /// <summary>
    /// Full path to the file
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Document title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// keyword for the type of the document, i.e. "Beleg", "Kontoauszug", ...
    /// </summary>
    public string Keyword { get; set; }

    /// <summary>
    /// ID of the account
    /// </summary>
    public int AccountId { get; set; }


    /// <summary>
    /// Last date of sending of the file
    /// </summary>
    public DateTime? SendDate { get; set; }


    /// <summary>
    /// Date the file was registered
    /// </summary>
    public DateTime DateCreated { get; set; } = DateTime.Now;

}