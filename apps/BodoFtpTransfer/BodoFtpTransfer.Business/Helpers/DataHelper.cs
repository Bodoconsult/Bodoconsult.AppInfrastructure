// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Data;
using Bodoconsult.App.DataProtection.ConsoleTools;
using Bodoconsult.Web.Ftp.Models;
using BodoFtpTransfer.Business.Model;

namespace BodoFtpTransfer.Business.Helpers;

/// <summary>
///  Data helper class
/// </summary>
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
            Path = reader[1].ToString() ?? string.Empty,
            HashCode = reader[2].ToString() ?? string.Empty,
            PathRemote = reader[3].ToString() ?? string.Empty,
            Source = reader[4].ToString() ?? string.Empty,
            HashCodeNew = reader[5].ToString() ?? string.Empty,
            Type = reader[6].ToString() ?? string.Empty,
            Size = reader.GetInt64(7),
            SizeRemote = reader.GetInt64(8)
        };

        return dto;
    }

    /// <summary>
    /// Map <see cref="BasicCredentials"/> to <see cref="SshCredentials"/>
    /// </summary>
    /// <param name="credentials">Basic credentials</param>
    /// <returns>SSH credentials</returns>
    public static SshCredentials MapBasicCredentialsToSshCredentials(BasicCredentials credentials)
    {
        return new SshCredentials
        {
            Password = credentials.Password,
            Username = credentials.Username,
            Url = credentials.Url
        };
    }
}