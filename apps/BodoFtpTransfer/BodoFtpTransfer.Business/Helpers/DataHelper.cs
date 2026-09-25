// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Data;
using BodoFtpTransfer.Business.Model;

namespace BodoFtpTransfer.Business.Helpers;

public static class DataHelper
{
    /// <summary>
    /// Mapping datareader to entity class Files
    /// </summary>
    public static FtpFiles MapFromDbToFiles(IDataReader reader)
    {

        var dto = new FtpFiles
        {
            Id = reader.GetInt64(0),
            Path = reader[1].ToString(),
            HashCode = reader[2].ToString(),
            PathRemote = reader[3].ToString(),
            Source = reader[4].ToString(),
            HashCodeNew = reader[5].ToString(),
            Type = reader[6].ToString(),
            Size = reader.GetInt64(7),
            SizeRemote = reader.GetInt64(8)
        };

        return dto;
    }
}