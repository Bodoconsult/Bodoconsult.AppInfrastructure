// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

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

using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.GrpcBackgroundService.AppStarter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// https://github.com/grpc/grpc-dotnet/blob/master/examples/Worker/Server/Program.cs

namespace Bodoconsult.App.GrpcBackgroundService.App;

/// <summary>
/// Base class for <see cref="IAppBuilder"/> implementations running a background service using GRPC
/// </summary>
public class BaseGrpcBackgroundServiceAppBuilder : BaseAppBuilder
{
    /// <summary>
    /// Current app builder instance
    /// </summary>
    protected readonly WebApplicationBuilder Builder;

    /// <summary>
    /// Current GRPC server instance
    /// </summary>
    protected WebApplication GrpcServer;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appGlobals">Current app globals</param>
    /// <param name="args">Current app args from command line</param>
    public BaseGrpcBackgroundServiceAppBuilder(IAppGlobals appGlobals, string[] args) : base(appGlobals)
    {
        // Prepare the service builder instance
        Builder = WebApplication.CreateBuilder(args);
        AppGlobals.DiContainer = new DiContainer(Builder.Services);
    }

    /// <summary>
    /// Configure the host app builder
    /// </summary>
    /// <param name="configureAction">Configure action expecting a <see cref="HostApplicationBuilder"/> instance</param>
    public void ConfigureHostBuilder(Action<WebApplicationBuilder> configureAction)
    {
        if (configureAction == null)
        {
            return;
        }

        configureAction.Invoke(Builder);
    }

    /// <summary>
    /// Enable GRPC detailled errors
    /// </summary>
    public bool EnableDetailedErrors { get; set; }

    /// <summary>
    /// Register DI container services for GRPC
    /// </summary>
    public virtual void RegisterGrpcDiServices()
    {
        Builder.Services.AddGrpc(x => x.EnableDetailedErrors = EnableDetailedErrors);
    }

    /// <summary>
    /// Register DI container services
    /// </summary>
    public override void RegisterDiServices()
    {
        DiContainerServiceProviderPackage.AddServices(AppGlobals.DiContainer);

        AppGlobals.DiContainer.AddSingleton((IAppBuilder)this);
    }

    /// <summary>
    /// Configure the Kestrel server used for GRPC. Access the WebApplicationBuilder via <see cref="Builder"/> property
    /// </summary>
    public virtual void ConfigureGrpc()
    {
        // Do nothing
    }

    /// <summary>
    /// Register protobuf service required for the application
    /// </summary>
    /// <exception cref="NotSupportedException">Always thrown if this method is not overwritten</exception>
    public virtual void RegisterProtoServices()
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Start the application
    /// </summary>
    public override void StartApplication()
    {
        Builder.Services.AddHostedService<GrpcBackgroundServiceAppStarter>();

        GrpcServer = Builder.Build();
        AppGlobals.DiContainer.LoadServiceProvider(GrpcServer.Services);

        DiContainerServiceProviderPackage.LateBindObjects(AppGlobals.DiContainer);

        RegisterProtoServices();

        StartApplicationService(null);

        GrpcServer.Run();
    }

    /// <summary>
    /// Stops the application
    /// </summary>
    public override void StopApplication()
    {
        AppGlobals.EventWaitHandle?.Reset();
        ApplicationServer?.StopApplication();
        GrpcServer?.StopAsync();
    }
}