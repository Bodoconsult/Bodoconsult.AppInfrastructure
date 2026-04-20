// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BusinessTransactions.Replies;
using Bodoconsult.App.BusinessTransactions.RequestData;
using Bodoconsult.App.General;
using IGeneralAppManagementManager = Bodoconsult.App.Interfaces.IGeneralAppManagementManager;
using IGeneralAppManagementService = Bodoconsult.App.Interfaces.IGeneralAppManagementService;

namespace Bodoconsult.App;

/// <summary>
/// Implementation of general app services like database backup
/// </summary>
public class GeneralAppManagementManager : IGeneralAppManagementManager
{
    private readonly IGeneralAppManagementService _service;
    private readonly IAppLoggerProxy _appLogger;

    /// <summary>
    /// Default ctor
    /// </summary>
    public GeneralAppManagementManager(IGeneralAppManagementService service, IAppLoggerProxy appLogger)
    {
        _service = service;
        _appLogger = appLogger;
    }

    /// <summary>
    /// Create a dump file with all logs in ProgrammData\SmdTower\LogDump.zip
    /// </summary>
    /// <param name="request">Current request containing an int32 value from MiniDump.MiniDumpTypeEnum</param>/>
    /// <returns>Transaction reply</returns>
    public DefaultBusinessTransactionReply CreateLogDump(IBusinessTransactionRequestData request)
    {
        var reply = new DefaultBusinessTransactionReply
        {
            ErrorCode = 0,
            Message = "Log dump created successfully as file LogDump.zip"
        };

        try
        {
            // Create log dump now
            _service.CreateLogDump();
        }
        catch (Exception e)
        {
            _appLogger.LogError("Creating log dump failed", e);
            reply.ErrorCode = -1;
            reply.Message = "Creating log dump failed";
            reply.ExceptionMessage = e.ToString();
        }

        return reply;
    }

    /// <summary>
    /// Create a normal mini dump of the app in ProgrammData\SmdTower\MiniDump.dmp
    /// </summary>
    /// <param name="request">Current request containing an int32 value from MiniDump.MiniDumpTypeEnum</param>/>
    /// <returns>Transaction reply</returns>
    public DefaultBusinessTransactionReply CreateMiniDump(IBusinessTransactionRequestData request)
    {
        var reply = new DefaultBusinessTransactionReply
        {
            ErrorCode = 0,
            Message = "Mini dump created successfully as file MiniDump.dmp"
        };

        var dumpType = MiniDump.MiniDumpTypeEnum.MiniDumpNormal;

        if (request is ObjectIdBusinessTransactionRequestData oRequest)
        {
            dumpType = (MiniDump.MiniDumpTypeEnum)oRequest.ObjectId;
        }

        try
        {
            _service.CreateMiniDump(dumpType);
        }
        catch (Exception e)
        {
            _appLogger.LogError("Creating mini dump failed", e);
            reply.ErrorCode = -1;
            reply.Message = "Creating mini dump failed";
            reply.ExceptionMessage = e.ToString();
        }

        return reply;
    }
}