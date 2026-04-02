// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.ClientNotifications;
using Bodoconsult.App.GrpcBackgroundService;
using Google.Protobuf.WellKnownTypes;
using GrpcServerApp.BusinessLogic.Notifications;

namespace GrpcServerApp.Grpc.MappingServices;

/// <summary>
/// Converts internal notifications to GRPC messages
/// </summary>
public class GrpcClientMessagingService : BaseClientMessagingService
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public GrpcClientMessagingService()
    {
        ConversionRules.Add(nameof(SimpleClientNotification), GetSimpleClientNotificationMessageDtoMessage);
    }

    /// <summary>
    /// Convert SimpleClientNotification into a SimpleClientNotificationMessage proto message. Public for unit tests
    /// </summary>
    /// <param name="notification"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public object GetSimpleClientNotificationMessageDtoMessage(IClientNotification notification)
    {
        if (notification is not SimpleClientNotification noti)
        {
            throw new ArgumentException($"{nameof(notification)} does NOT have the expected type of {nameof(SimpleClientNotification)}");
        }

        var data = new SimpleClientNotificationMessage
        {
            Message = noti.Message
        };

        var message = new ClientNotificationMessage
        {
            Dto = Any.Pack(data)
        };

        return message;
    }
}