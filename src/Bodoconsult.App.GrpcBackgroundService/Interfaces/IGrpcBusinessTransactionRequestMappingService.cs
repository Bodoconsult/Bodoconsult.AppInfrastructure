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

using Bodoconsult.App.Abstractions.Interfaces;
using Google.Protobuf;
using Grpc.Core;

namespace Bodoconsult.App.GrpcBackgroundService.Interfaces;

/// <summary>
/// Interface for mapping GRPC requests to internal business transaction requests
/// </summary>
public interface IGrpcBusinessTransactionRequestMappingService
{
    /// <summary>
    /// Map a GRPC request to an internal business transaction request
    /// </summary>
    /// <param name="request">Current GRPC request</param>
    /// <param name="context">Request context with metadata</param>
    /// <returns>Internal business transaction request</returns>
    IBusinessTransactionRequestData MapToBusinessTransactionRequestData(BusinessTransactionRequest request,
        ServerCallContext context);


    /// <summary>
    /// Helper method to provoke moving to transactions. Should be replaced with 
    /// </summary>
    /// <param name="grpcRequest">Current not business transaction based GRPC request</param>
    /// <param name="context">Request context with metadata</param>
    /// <returns>Internal business transaction request</returns>
    IBusinessTransactionRequestData MapGrpcRequestToBusinessTransactionRequestData<T>(IMessage<T> grpcRequest,
        ServerCallContext context) where T : IMessage<T>;

}