// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

#region GRPC copyright notice and license

// Copyright 2019 The gRPC Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System.Net;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.GrpcBackgroundService;
using Bodoconsult.App.GrpcBackgroundService.App;
using GrpcServerApp.Grpc.DiContainerProvider;
using GrpcServerApp.Grpc.ProtobufServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace GrpcServerApp;

/// <summary>
/// A windows service called GrpcServerApp
/// </summary>
public class GrpcServerAppAppBuilder : BaseGrpcBackgroundServiceAppBuilder
{
    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Global app settings</param>
    /// <param name="args">Command line args</param>
    public GrpcServerAppAppBuilder(IAppGlobals appGlobals, string[] args) : base(appGlobals,  args)
    {
        ConfigureHostBuilder(ConfigureAction);
    }

    private static void ConfigureAction(WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.AddWindowsService(options =>
        {
            options.ServiceName = "GrpcServerApp";
        });

    }

    /// <summary>
    /// Load the <see cref="IAppBuilder.DiContainerServiceProviderPackage"/>
    /// </summary>
    public override void LoadDiContainerServiceProviderPackage()
    {
        var factory = new GrpcServerAppProductionDiContainerServiceProviderPackageFactory(AppGlobals);
        DiContainerServiceProviderPackage = factory.CreateInstance();
    }

    public override void ConfigureGrpc()
    {
        //// For production: use HTTP2 and load a certificate for HTTPS
        //Builder.WebHost.ConfigureKestrel(options =>
        //{
        //    options.Listen(IPAddress.Any, AppGlobals.AppStartParameter.Port, listenOptions =>
        //    {
        //        listenOptions.Protocols = HttpProtocols.Http2;
        //        listenOptions.UseHttps("<path to .pfx file>",
        //            "<certificate password>");
        //    });
        //});

        // Only for testing:
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        Builder.WebHost.ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, 50051, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2; ;
            });
        });
        Builder.WebHost.UseUrls($"http://localhost:{AppGlobals.AppStartParameter.Port}");
    }

    /// <summary>
    /// Register protobuf service required for the application
    /// </summary>
    public override void RegisterProtoServices()
    {

        if (GrpcServer is null)
        {
            throw new ArgumentNullException(nameof(GrpcServer));
        }

        // Configure the HTTP request pipeline.
        GrpcServer.MapGrpcService<BusinessTransactionServiceImpl>().RequireHost("*:50051");
        GrpcServer.MapGrpcService<ClientCommunicationServiceImpl>().RequireHost("*:50051");
        GrpcServer.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    }
}