namespace BodoWebReporting.Dal.Model;

/// <summary>
/// Application settings
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Database connection
    /// </summary>
    public string DatabaseConnection { get; set; }

    /// <summary>
    /// List of receiver email addresses separated by semicolon for error mails
    /// </summary>
    public string ErrorMailReceiver { get; set; }

    /// <summary>
    /// Target path for reporting items
    /// </summary>
    public string TargetPath { get; set; }

    /// <summary>
    /// Run SQL statement(s) at app start
    /// </summary>
    public string PreRunSql { get; set; }

    /// <summary>
    /// Run SQL statement(s) at app's runtime end
    /// </summary>
    public string PostRunSql { get; set; }
}