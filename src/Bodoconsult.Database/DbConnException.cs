namespace Bodoconsult.Database;

/// <summary>
/// Database connection exception
/// </summary>
public class DbConnException : System.Exception
{
    /// <summary>
    /// Error code
    /// </summary>
    public DbConnErrorCode Erc { get; }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="e">Error code</param>
    public DbConnException(string message, DbConnErrorCode e)
        : base(message)
    {
        Erc = e;
    }
}