// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BusinessTransactions.Replies;

namespace Bodoconsult.App.Interfaces;

/// <summary>
/// Interface for general app features like database backup
/// </summary>
public interface IGeneralAppManagementManager
{
    /// <summary>
    /// Create a dump file with all logs in ProgrammData\AppFolder\LogDump.zip
    /// </summary>
    /// <param name="request">Current request containing an int32 value from MiniDump.MiniDumpTypeEnum</param>/>
    /// <returns>Transaction reply</returns>
    DefaultBusinessTransactionReply CreateLogDump(IBusinessTransactionRequestData request);

    /// <summary>
    /// Create a normal mini dump of the app in ProgrammData\AppFolder\MiniDump.dmp
    /// </summary>
    /// <param name="request">Current request containing an int32 value from MiniDump.MiniDumpTypeEnum</param>/>
    /// <returns>Transaction reply</returns>
    DefaultBusinessTransactionReply CreateMiniDump(IBusinessTransactionRequestData request);

}