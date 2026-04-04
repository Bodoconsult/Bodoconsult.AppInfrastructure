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

namespace Bodoconsult.App.GrpcBackgroundService.Interfaces;

/// <summary>
/// Interface for mapping internal business transaction replies to GRPC replies
/// </summary>
public interface IGrpcBusinessTransactionReplyMappingService
{

    /// <summary>
    /// Map an internal reply instance <see cref="IBusinessTransactionReply"/> to a <see cref="BusinessTransactionReply"/> message
    /// </summary>
    /// <param name="internalReply">Internal business transaction reply</param>
    /// <returns>Protobuf business transaction reply</returns>
    BusinessTransactionReply MapInternalReplyToGrpc(IBusinessTransactionReply internalReply);

}