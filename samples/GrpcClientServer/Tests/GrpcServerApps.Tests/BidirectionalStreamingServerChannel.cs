// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BusinessTransactions.RequestData;
using Bodoconsult.App.GrpcBackgroundService;
using Bodoconsult.App.Helpers;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServerApp.Grpc.App;

namespace GrpcServerApps.Tests;

// https://www.noser.com/techblog/grpc-tutorial-teil-3-robustes-duplex-system/

public class BidirectionalStreamingServerChannel
{
    private readonly GrpcChannel _channel;
    private ClientCommunicationService.ClientCommunicationServiceClient _clientCommunicationService;
    private AsyncDuplexStreamingCall<ClientNotificationMessage, ClientNotificationMessage> _asyncDuplexStreamingCall;
    private int _counter;

    private readonly ProducerConsumerQueue<ClientNotificationMessage> _consumerQueue = new();

    private readonly List<BusinessTransactionRunner> _runningBusinessTransactions = [];

    public BidirectionalStreamingServerChannel(GrpcChannel channel)
    {
        _channel = channel;
        _consumerQueue.ConsumerTaskDelegate = ConsumerTaskDelegate;
        _consumerQueue.StartConsumer();
    }

    private void ConsumerTaskDelegate(ClientNotificationMessage notification)
    {
        _asyncDuplexStreamingCall.RequestStream.WriteAsync(notification).GetAwaiter().GetResult();
    }


    public CancellationTokenSource CancellationTokenSource { get; } = new();

    private readonly Lock _counterLock = new();

    public int Counter

    {
        get
        {
            lock (_counterLock)
            {
                return _counter;
            }
        }
        private set
        {
            lock (_counterLock)
            {
                _counter = value;
            }
        }
    }

    public void StartClientNotificationChannel()
    {
        _clientCommunicationService = new ClientCommunicationService.ClientCommunicationServiceClient(_channel);

        Counter = 0;

        _asyncDuplexStreamingCall = _clientCommunicationService.StartBidirectionalStream(cancellationToken: CancellationTokenSource.Token);


        // Start the communication: send an identifier for client with the request
        StartCommunication();

        var result = WaitForStartCommunicationMessageFromServer().GetAwaiter().GetResult();
        if (!result)
        {
            CancellationTokenSource.Cancel(true);
            _asyncDuplexStreamingCall?.Dispose();
        }

        // Start message receiving
        AsyncHelper.FireAndForget(() => _ = ReceiveServerMessages());

        // Start sending business transaction
        AsyncHelper.FireAndForget(StartBusinessTransactions);

        // Wait now until cancelation is requested
        while (!CancellationTokenSource.IsCancellationRequested)
        {
            Task.Delay(50).Wait();
        }

        _asyncDuplexStreamingCall?.Dispose();
    }

    private async Task<bool> WaitForStartCommunicationMessageFromServer()
    {
        while (true)
        {
            await foreach (var notification in _asyncDuplexStreamingCall.ResponseStream.ReadAllAsync(
                               cancellationToken: CancellationTokenSource.Token))
            {
                // Wait for StartCommunicationMessage
                if (notification.Dto.Is(StartCommunicationMessage.Descriptor))
                {
                    var noti = notification.Dto.Unpack<StartCommunicationMessage>();
                    Debug.Print($"Client received client ID back: {noti.ClientId}");
                    return true;
                }

                Debug.Print("Client received NO client ID back");
                return false;
            }

            if (CancellationTokenSource.Token.IsCancellationRequested)
            {
                return false;
            }
        }
    }

    private void StartCommunication()
    {
        var startRequest = new StartCommunicationMessage
        {
            ClientId = Guid.NewGuid().ToString()
        };

        var noti = new ClientNotificationMessage
        {
            Dto = Any.Pack(startRequest)
        };

        _asyncDuplexStreamingCall.RequestStream.WriteAsync(noti).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Do notify the server
    /// </summary>
    /// <param name="notification">Current notification to send to the server</param>
    public void DoNotifyServer(ClientNotificationMessage notification)
    {
        _consumerQueue.Enqueue(notification);
    }

    /// <summary>
    /// Run a business transaction
    /// </summary>
    /// <param name="requestData">Business transaction request data</param>
    public BusinessTransactionReply RunBusinessTransaction(IBusinessTransactionRequestData requestData)
    {
        var bt = new BusinessTransactionRunner(requestData)
        {
            DoNotifyServerDelegate = DoNotifyServer
        };

        _runningBusinessTransactions.Add(bt);

        bt.RunBusinessTransaction();

        return bt.Reply;
    }


    private async Task ReceiveServerMessages()
    {
        var logger = Globals.Instance.Logger;

        try
        {
            while (true)
            {
                if (_asyncDuplexStreamingCall is null)
                {
                    Debug.Print("Waiting for server stream");
                    await Task.Delay(50);
                    continue;
                }

                Debug.Print("Waiting for server messages");
                logger.LogInformation("Waiting for server messages");

                await foreach (var notification in _asyncDuplexStreamingCall.ResponseStream.ReadAllAsync(
                                   cancellationToken: CancellationTokenSource.Token))
                {
                    // Here you should handle your different types of notifications in a similar way then shown in GrpcBusinessTransactionRequestMappingService
                    if (notification.Dto.Is(SimpleClientNotificationMessage.Descriptor))
                    {
                        var noti = notification.Dto.Unpack<SimpleClientNotificationMessage>();
                        Debug.Print($"Client received: {noti.Message}");
                        logger.LogInformation($"Client received: {noti.Message}");
                        Counter++;
                    }

                    // Business transaction reply
                    if (notification.Dto.Is(BusinessTransactionReply.Descriptor))
                    {
                        var noti = notification.Dto.Unpack<BusinessTransactionReply>();
                        Debug.Print($"Client received reply for BT {noti.TransactionId} {noti.TransactionUid}");
                        logger.LogInformation($"Client received reply for BT {noti.TransactionId} {noti.TransactionUid}");

                        var runner = _runningBusinessTransactions.FirstOrDefault(x =>
                            x.TransactionId == noti.TransactionId && x.TransactionUid == noti.TransactionUid);

                        runner?.ReceiveReply(noti);

                        Counter++;
                    }

                    if (CancellationTokenSource.Token.IsCancellationRequested)
                    {
                        break;
                    }
                }

                if (CancellationTokenSource.Token.IsCancellationRequested)
                {
                    break;
                }

                await Task.Delay(50);
            }
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
        {
            Debug.Print("Stream cancelled.");
            //CancellationTokenSource.Cancel(true);
        }
        catch (Exception ex)
        {
            CancellationTokenSource.Cancel(true);
            Globals.Instance.Logger.LogError("Receiving failed", ex);
        }
    }

    /// <summary>
    /// Start some business transactions. In production this calls come from UI or any other consumer of business transactions
    /// </summary>
    public void StartBusinessTransactions()
    {

        while (true)
        {
            var request = new EmptyBusinessTransactionRequestData
            {
                TransactionId = 1
            };

            var reply = RunBusinessTransaction(request);

            Debug.Print($"BT {reply.TransactionId} {reply.TransactionUid}: {reply.LogMessage}");
            
            if (CancellationTokenSource.Token.IsCancellationRequested)
            {
                break;
            }
        }


    }


}

