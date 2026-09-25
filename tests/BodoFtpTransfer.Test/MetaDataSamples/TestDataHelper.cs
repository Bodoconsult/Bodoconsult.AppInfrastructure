namespace BodoFtpTransferCore.Business.Test.MetaDataSamples
{
    public static class TestDataHelper
    {
        /// <summary>
        /// Get a filled new object from DTO class Files object
        /// </summary>
        public static FtpFiles NewFiles()
        {

            var item = new FtpFiles
            {
                ID = 1,
                Path = "T",
                HashCode = "T",
                PathRemote = "T",
                Source = "T",
                HashCodeNew = "T",
                Type = "T",
                Size = 1,
                SizeRemote = 1,
            };

            return item;

        }
    }
}