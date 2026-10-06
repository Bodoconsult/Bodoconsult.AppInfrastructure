// ReSharper disable InconsistentNaming
namespace Bodoconsult.Database
{
    /// <summary>
    /// DB conenction error codes
    /// </summary>
    public enum DbConnErrorCode : int
    {
        /// <summary>
        /// Error
        /// </summary>
        ERC_ERROR = 10,
        /// <summary>
        /// Not implemented
        /// </summary>
        ERC_NOTIMPLEMENTED = 11,
        /// <summary>
        /// Unsupported provider
        /// </summary>
        ERC_UNSUPPORTEDPROVIDER = 12,
        /// <summary>
        /// UDL not found
        /// </summary>
        ERC_UDLNOTFOUND = 13,
        /// <summary>
        /// Async not possible
        /// </summary>
        ERC_ASYNCNOTPOSSIBLE = 14
    }
}