// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ClientNotifications;
using Bodoconsult.App.GrpcBackgroundService;
using Bodoconsult.App.GrpcBackgroundService.Interfaces;
using Bodoconsult.App.Helpers;
using Bodoconsult.App.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcServerApp.BusinessLogic.Notifications;
using GrpcServerApp.Grpc.App;

namespace GrpcServerApp.Grpc.ProtobufServices;

public class ClientCommunicationServiceImpl: ClientCommunicationService.ClientCommunicationServiceBase
{

    private readonly IClientManager _clientManager;
    private readonly IGrpcBusinessTransactionRequestMappingService _requestMappingService;
    private readonly IGrpcBusinessTransactionReplyMappingService _replyMappingService;
    private readonly IBusinessTransactionManager _businessTransactionManager;

    public ClientCommunicationServiceImpl(IClientManager clientManager, IGrpcBusinessTransactionRequestMappingService requestMappingService, IGrpcBusinessTransactionReplyMappingService replyMappingService, IBusinessTransactionManager businessTransactionManager)
    {
        _clientManager = clientManager;
        _replyMappingService = replyMappingService;
        _requestMappingService = requestMappingService;
        _businessTransactionManager = businessTransactionManager;
    }


    public override Task<Empty> CheckServer(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new Empty());
    }

    public override async Task ServerNotification(NotificationIntervalMessage request,
        IServerStreamWriter<ServerNotificationMessage> responseStream,
        ServerCallContext context)
    {
        var nextNotificationTime = DateTime.UtcNow.AddMilliseconds(request.IntervalMs);
        var cancellationToken = context.CancellationToken;
        do
        {
            if (DateTime.UtcNow > nextNotificationTime)
            {
                nextNotificationTime = DateTime.UtcNow.AddMilliseconds(request.IntervalMs);
                await responseStream.WriteAsync(
                    new ServerNotificationMessage
                    {
                        ServerTime = Timestamp.FromDateTime(DateTime.UtcNow)
                    }).ConfigureAwait(false);
            }
            await Task.Delay(10, cancellationToken).ConfigureAwait(false);
        } while (cancellationToken.IsCancellationRequested == false);
        cancellationToken.ThrowIfCancellationRequested();
    }


    //Server Streaming RPC [server side]
    public override async Task StartServerStream(StartCommunicationMessage request, IServerStreamWriter<ClientNotificationMessage> responseStream, ServerCallContext context)
    {

        var clientData = new FakeLoginData
        {
            Id = Guid.NewGuid()
        };

        var client = new GrpcClient(clientData, context.CancellationToken);
        client.IsConnected = true;

        _clientManager.AddClient(client);

        client.LoadStream(responseStream);

        //var data = new SimpleClientNotificationMessage
        //{
        //    Message = $"Notification 0"
        //};
        //var noti = new ClientNotificationMessage()
        //{
        //    SimpleClientNotificationMessage = data
        //};

        //Debug.Print($"Notification 0 sent: {data.Message}");

        //await responseStream.WriteAsync(noti);

        AsyncHelper.FireAndForget(() =>
        {
            SendDemoNotifications(client);
        });

        while (!context.CancellationToken.IsCancellationRequested)
        {
            await Task.Delay(500);
        }

        _clientManager.RemoveClient(client);
        client.Dispose();
    }

    public override async Task StartBidirectionalStream(IAsyncStreamReader<ClientNotificationMessage> requestStream,
        IServerStreamWriter<ClientNotificationMessage> responseStream, ServerCallContext context)
    {

        // Get first message
        var clientUid = Guid.Empty;

        await foreach (var notification in requestStream.ReadAllAsync(cancellationToken: context.CancellationToken))
        {
            // Wait for StartCommunicationMessage
            if (notification.Dto.Is(StartCommunicationMessage.Descriptor))
            {
                var noti = notification.Dto.Unpack<StartCommunicationMessage>();
                clientUid = new Guid(noti.ClientId);
                Debug.Print($"Server received client ID: {noti.ClientId}");
                break;
            }

            Debug.Print("Server received NO client ID");
            return;
        }


        var clientData = new FakeLoginData
        {
            Id = clientUid
        };

        var client = new GrpcClient(clientData, context.CancellationToken);
        client.IsConnected = true;

        _clientManager.AddClient(client);

        client.LoadStream(responseStream);

        AsyncHelper.FireAndForget(() =>
        {
            ReceiveMessages(requestStream, client, context, responseStream).GetAwaiter().GetResult();
        });

        // Inform client that server is ready
        StartCommunication(client, responseStream);

        Task.Delay(500).Wait();

        AsyncHelper.FireAndForget(() =>
        {
            SendDemoNotifications(client);
        });

        while (!context.CancellationToken.IsCancellationRequested)
        {
            await Task.Delay(500);
        }

        _clientManager.RemoveClient(client);
        client.Dispose();

    }

    private void StartCommunication(GrpcClient client, IServerStreamWriter<ClientNotificationMessage> responseStream)
    {
        var startRequest = new StartCommunicationMessage
        {
            ClientId = client.ClientData.Id.ToString()
        };

        var noti = new ClientNotificationMessage
        {
            Dto = Any.Pack(startRequest)
        };

        responseStream.WriteAsync(noti).GetAwaiter().GetResult();
    }

    private async Task ReceiveMessages(IAsyncStreamReader<ClientNotificationMessage> requestStream, GrpcClient client,
        ServerCallContext context, IServerStreamWriter<ClientNotificationMessage> responseStream)
    {
        try
        {
            while (true)
            {
                if (client.ContextCancellationToken.IsCancellationRequested)
                {
                    return;
                }

                await foreach (var notification in requestStream.ReadAllAsync(cancellationToken: client.ContextCancellationToken))
                {
                    // Here you should handle your different types of notifications in a similar way then shown in GrpcBusinessTransactionRequestMappingService
                    if (notification.Dto.Is(SimpleClientNotificationMessage.Descriptor))
                    {
                        var noti = notification.Dto.Unpack<SimpleClientNotificationMessage>();
                        Debug.Print($"Server received: {noti.Message}");
                    }

                    // Handle business transaction here in an async manner. Do not wait for answer. Client must wait for it.
                    if (notification.Dto.Is(BusinessTransactionRequest.Descriptor))
                    {
                        var noti = notification.Dto.Unpack<BusinessTransactionRequest>();
                        Debug.Print($"Server received BT {noti.TransactionId} {noti.TransactionUid}");

                        var internalRequest = _requestMappingService.MapToBusinessTransactionRequestData(noti, context);

                        AsyncHelper.FireAndForget(() =>
                        {
                            RunBusinessTransaction(internalRequest, client);
                        });
                    }

                    if (client.ContextCancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                }

                if (client.ContextCancellationToken.IsCancellationRequested)
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
            Globals.Instance.Logger.LogError("Receiving failed", ex);
        }
    }

    private void RunBusinessTransaction(IBusinessTransactionRequestData internalRequest, GrpcClient client)
    {
        var internalReply = _businessTransactionManager.RunBusinessTransaction(internalRequest.TransactionId, internalRequest);

        var dto = _replyMappingService.MapInternalReplyToGrpc(internalReply);

        var message = new ClientNotificationMessage
            { Dto = Any.Pack(dto) };



        internalReply.NotificationObjectToSend = message;

        client.DoNotifyClient(internalReply);

    }

    private void SendDemoNotifications(GrpcClient client)
    {
        var i = 0;
        while (!client.ContextCancellationToken.IsCancellationRequested && i<10)
        {
            var data = new SimpleClientNotification
            {
                Message = $"Notification {i}"
            };

            Debug.Print($"Notification {i} sent: {data.Message}");
            _clientManager.DoNotifyAllClients(data);

            i++;
            Task.Delay(500).Wait();
        }
    }
}