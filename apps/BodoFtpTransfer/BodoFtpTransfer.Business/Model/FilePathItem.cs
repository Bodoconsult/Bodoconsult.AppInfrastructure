namespace BodoFtpTransfer.Business.Model
{

    /// <summary>
    /// Item holding path info for a file
    /// </summary>
    public class FilePathItem
    {
        /// <summary>
        /// Path to the file
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Path to the remote file
        /// </summary>
        public string PathRemote { get; set; }


        /// <summary>
        /// Source
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Size of the file
        /// </summary>
        public long Size { get; set; }


        /// <summary>
        /// Size of the file
        /// </summary>
        public long SizeRemote { get; set; }
    }
}
