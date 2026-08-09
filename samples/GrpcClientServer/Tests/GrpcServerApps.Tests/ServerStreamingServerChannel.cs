// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.GrpcBackgroundService;
using Bodoconsult.App.Helpers;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServerApp.Grpc.App;

namespace GrpcServerApps.Tests;

public class ServerStreamingServerChannel
{
    private readonly GrpcChannel _channel;
    private ClientCommunicationService.ClientCommunicationServiceClient _clientCommunicationService;
    private AsyncServerStreamingCall<ClientNotificationMessage> _asyncServerStreamingCall;
    private int _counter;

    public ServerStreamingServerChannel(GrpcChannel channel)
    {
        _channel = channel;
    }


    public CancellationTokenSource CancellationTokenSource { get; } = new();

    private readonly object _counterLock = new();

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

        // Start the communication: send an identifier for client with the request
        var request = new StartCommunicationMessage
        {
            ClientId = Guid.NewGuid().ToString()
        };

        _asyncServerStreamingCall =
            _clientCommunicationService.StartServerStream(request,
                cancellationToken: CancellationTokenSource.Token);
        AsyncHelper.FireAndForget(() => _ = ReceiveServerMessages());

        while (!CancellationTokenSource.IsCancellationRequested)
        {
            Task.Delay(50).Wait();
        }

        _asyncServerStreamingCall?.Dispose();
    }


    private async Task ReceiveServerMessages()
    {
        var logger = Globals.Instance.Logger;

        try
        {

            while (true)
            {
                if (_asyncServerStreamingCall is null)
                {
                    Debug.Print("Waiting for server stream");
                    await Task.Delay(50);
                    continue;
                }

                Debug.Print("Waiting for server messages");
                logger.LogInformation("Waiting for server messages");

                await foreach (var notification in _asyncServerStreamingCall.ResponseStream.ReadAllAsync(
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
}