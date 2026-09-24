using System.Data;

namespace BodoFtpTransferCore.Business.Test.MetaDataSamples
{
    public static class DataHelper
    {
        /// <summary>
        /// Mapping datareader to entity class Files
        /// </summary>
        public static FtpFiles MapFromDbToFiles(IDataReader reader)
        {

            var dto = new FtpFiles();
            dto.ID = reader.GetInt64(0);
            dto.Path = reader[1].ToString();
            dto.HashCode = reader[2].ToString();
            dto.PathRemote = reader[3].ToString();
            dto.Source = reader[4].ToString();
            dto.HashCodeNew = reader[5].ToString();
            dto.Type = reader[6].ToString();
            dto.Size = reader.GetInt64(7);
            dto.SizeRemote = reader.GetInt64(8);

            return dto;

        }
    }
}