// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BusinessTransactions.RequestData;
using Bodoconsult.App.GrpcBackgroundService;
using Bodoconsult.App.Helpers;
using Google.Protobuf.WellKnownTypes;

namespace GrpcServerApps.Tests;

/// <summary>
/// Delegate for notifying the server
/// </summary>
/// <param name="notification">Current notification to send to the server</param>
public delegate void DoNotifyServerDelegate(ClientNotificationMessage notification);

public class BusinessTransactionRunner
{

    private CancellationTokenSource _cancellationTokenSource;
    private TaskCompletionSource<bool> _taskCompletionSource;

    public BusinessTransactionRunner( IBusinessTransactionRequestData requestData)
    {
        TransactionId = requestData.TransactionId;
        RequestData = requestData;
    }

    public DoNotifyServerDelegate DoNotifyServerDelegate { get; set; }

    /// <summary>
    /// ID of the called transaction
    /// </summary>
    public int TransactionId { get; }

    /// <summary>
    /// Uniqueidentifier to identity the reply for the business transaction
    /// </summary>
    public string TransactionUid { get; } = Guid.NewGuid().ToString();


    /// <summary>
    /// Request data to send with business transaction request
    /// </summary>
    public IBusinessTransactionRequestData RequestData { get; }


    /// <summary>
    /// The received reply
    /// </summary>
    public BusinessTransactionReply Reply { get; private set; }

    /// <summary>
    /// Timeout in ms
    /// </summary>
    public int Timeout { get; set; } = 5000;


    /// <summary>
    /// Run the business transaction until the reply is received or a timeout has happened
    /// </summary>
    public void RunBusinessTransaction()
    {
        var btRequest = new BusinessTransactionRequest
        {
            TransactionId = TransactionId,
            TransactionUid = TransactionUid
        };


        // Map interanl request to GRPC request DTO. Should be done in a separate mapper class in production
        if (RequestData != null)
        {
            if (RequestData is EmptyBusinessTransactionRequestData)
            {
                var requestData = new EmptyRequest();
                btRequest.RequestData = Any.Pack(requestData);
            }

            if (RequestData is ObjectIdBusinessTransactionRequestData idRequest)
            {
                var requestData = new ObjectIdRequest
                {
                    ObjectId = idRequest.ObjectId
                };
                btRequest.RequestData = Any.Pack(requestData);
            }
        }

        var noti = new ClientNotificationMessage
        {
            Dto = Any.Pack(btRequest)
        };

        // Timeout settings
        _cancellationTokenSource = new CancellationTokenSource(Timeout);
        _cancellationTokenSource.Token.Register(() =>
        {
            if (_taskCompletionSource is not
                {
                    Task:
                    {
                        IsCompleted: false, IsCanceled: false, IsFaulted: false, IsCompletedSuccessfully: false
                    }
                })
            {
                return;
            }

            _taskCompletionSource?.SetResult(false);

        });

        // Run the transaction
        AsyncHelper.FireAndForget(() =>
        {
            DoNotifyServerDelegate.Invoke(noti);
        });

        // Wait now
        var result = AsyncHelper.CreateWaitingTask(out _taskCompletionSource);

        if (!result)
        {
            Reply = new BusinessTransactionReply
            {
                TransactionId = TransactionId,
                TransactionUid = TransactionUid,
                ErrorCode = -99,
                LogMessage = "Business transaction ran into timeout"
            };
        }
    }

    /// <summary>
    /// Receive the reply and stop waiting for reply
    /// </summary>
    /// <param name="reply">Received reply</param>
    public void ReceiveReply(BusinessTransactionReply reply)
    {
        Reply = reply;
        _taskCompletionSource?.SetResult(true);
        Debug.Print($"BT {reply.TransactionId} {reply.TransactionUid} has finished");
    }

}