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

using System.Text;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BusinessTransactions.RequestData;
using Bodoconsult.App.GrpcBackgroundService.Interfaces;
using Bodoconsult.App.Helpers;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Bodoconsult.App.GrpcBackgroundService.BusinessTransactions;

/// <summary>
/// Base impl for <see cref="IGrpcBusinessTransactionRequestMappingService"/> for mapping GRPC requests to internal business transaction requests
/// </summary>
public class BaseGrpcBusinessTransactionRequestMappingService : IGrpcBusinessTransactionRequestMappingService
{
    // ToDo: JW add unit tests for methods

    /// <summary>
    /// Delegate method to create business transaction request data
    /// </summary>
    /// <param name="request">Business tramsaction request</param>
    /// <returns>Internal request data object</returns>
    protected delegate IBusinessTransactionRequestData CreateBusinessTransactionRequestDataDelegate(BusinessTransactionRequest request);

    /// <summary>
    /// Dictionary with all loaded mappings
    /// </summary>
    protected readonly Dictionary<MessageDescriptor, CreateBusinessTransactionRequestDataDelegate> AllBusinessTransactionRequestDataDelegates = new();

    /// <summary>
    /// Current app logger instance
    /// </summary>
    protected readonly IAppLoggerProxy AppLogger;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appLogger">Current app logger</param>
    public BaseGrpcBusinessTransactionRequestMappingService(IAppLoggerProxy appLogger)
    {
        AppLogger = appLogger;

        AllBusinessTransactionRequestDataDelegates.Add(EmptyRequest.Descriptor, CreateEmptyRequest);
        AllBusinessTransactionRequestDataDelegates.Add(EmptyListRequest.Descriptor, CreateEmptyListRequest);
        AllBusinessTransactionRequestDataDelegates.Add(ObjectIdRequest.Descriptor, CreateObjectIdRequest);
        AllBusinessTransactionRequestDataDelegates.Add(ObjectIdStringRequest.Descriptor, CreateObjectIdStringRequest);
        AllBusinessTransactionRequestDataDelegates.Add(ObjectIdIntRequest.Descriptor, CreateObjectIdIntRequest);
        AllBusinessTransactionRequestDataDelegates.Add(ObjectIdListRequest.Descriptor, CreateObjectIdListRequest);
        AllBusinessTransactionRequestDataDelegates.Add(ObjectUidRequest.Descriptor, CreateObjectGuidRequest);
        AllBusinessTransactionRequestDataDelegates.Add(TwoObjectUidRequest.Descriptor, CreateTwoObjectUidRequest);
        AllBusinessTransactionRequestDataDelegates.Add(TwoObjectIdRequest.Descriptor, CreateTwoObjectIdRequest);
        AllBusinessTransactionRequestDataDelegates.Add(ObjectUidListRequest.Descriptor, CreateObjectUidListRequest);
        AllBusinessTransactionRequestDataDelegates.Add(ObjectNameRequest.Descriptor, CreateObjectNameRequest);
        AllBusinessTransactionRequestDataDelegates.Add(ObjectNameListRequest.Descriptor, CreateObjectListNameRequest);
        AllBusinessTransactionRequestDataDelegates.Add(ObjectNameStringRequest.Descriptor, CreateObjectNameStringRequest);
        AllBusinessTransactionRequestDataDelegates.Add(StringRequest.Descriptor, CreateStringRequest);

    }

    /// <summary>
    /// Map <see cref="TwoObjectIdRequest"/> to <see cref="TwoObjectIdBusinessTransactionRequestData"/>. Public for unit testing. Do not use directly
    /// </summary>
    /// <param name="request">GRPC request data</param>
    /// <returns>Internal request data</returns>
    public IBusinessTransactionRequestData CreateTwoObjectIdRequest(BusinessTransactionRequest request)
    {
        var o = request.RequestData.Unpack<TwoObjectIdRequest>();

        var r1 = new TwoObjectIdBusinessTransactionRequestData
        {
            ObjectId1 = o.ObjectId1,
            ObjectId2 = o.ObjectId2
        };

        return r1;
    }

    /// <summary>
    /// Map <see cref="TwoObjectUidRequest"/> to <see cref="TwoObjectGuidBusinessTransactionRequestData"/>. Public for unit testing. Do not use directly
    /// </summary>
    /// <param name="request">GRPC request data</param>
    /// <returns>Internal request data</returns>
    public IBusinessTransactionRequestData CreateTwoObjectUidRequest(BusinessTransactionRequest request)
    {
        var o = request.RequestData.Unpack<TwoObjectUidRequest>();

        var r1 = new TwoObjectGuidBusinessTransactionRequestData
        {
            ObjectGuid1 = string.IsNullOrEmpty(o.ObjectUid1) ? Guid.Empty : new Guid(o.ObjectUid1),
            ObjectGuid2 = string.IsNullOrEmpty(o.ObjectUid2) ? Guid.Empty : new Guid(o.ObjectUid2),
        };

        return r1;
    }

    /// <summary>
    /// Map <see cref="ObjectNameListRequest"/> to <see cref="ObjectNameListBusinessTransactionRequestData"/>. Public for unit testing. Do not use directly
    /// </summary>
    /// <param name="request">GRPC request data</param>
    /// <returns>Internal request data</returns>
    public IBusinessTransactionRequestData CreateObjectListNameRequest(BusinessTransactionRequest request)
    {
        var o = request.RequestData.Unpack<ObjectNameListRequest>();

        var r1 = new ObjectNameListBusinessTransactionRequestData
        {
            Name = o.Name,
            Page = o.Page == 0 ? 1 : o.Page,
            PageSize = o.PageSize == 0 ? int.MaxValue : o.PageSize,
        };

        return r1;
    }

    /// <summary>
    /// Map <see cref="ObjectUidListRequest"/> to <see cref="ObjectGuidListBusinessTransactionRequestData"/>. Public for unit testing. Do not use directly
    /// </summary>
    /// <param name="request">GRPC request data</param>
    /// <returns>Internal request data</returns>
    public IBusinessTransactionRequestData CreateObjectUidListRequest(BusinessTransactionRequest request)
    {
        var o = request.RequestData.Unpack<ObjectUidListRequest>();

        var ir = new ObjectGuidListBusinessTransactionRequestData
        {
            Page = o.Page == 0 ? 1 : o.Page,
            PageSize = o.PageSize == 0 ? int.MaxValue : o.PageSize,
            ObjectGuid = new Guid(o.ObjectUid)
        };

        return ir;
    }

    /// <summary>
    /// Map <see cref="StringRequest"/> to <see cref="StringBusinessTransactionRequestData"/>. Public for unit testing. Do not use directly
    /// </summary>
    /// <param name="request">GRPC request data</param>
    /// <returns>Internal request data</returns>
    IBusinessTransactionRequestData CreateStringRequest(BusinessTransactionRequest request)
    {
        var o = request.RequestData.Unpack<StringRequest>();

        var ir = new StringBusinessTransactionRequestData
        {
            Content = o.Message
        };
        return ir;
    }

    /// <summary>
    /// Map <see cref="ObjectNameStringRequest"/> to <see cref="ObjectNameStringBusinessTransactionRequestData"/>. Public for unit testing. Do not use directly
    /// </summary>
    /// <param name="request">GRPC request data</param>
    /// <returns>Internal request data</returns>
    public IBusinessTransactionRequestData CreateObjectNameStringRequest(BusinessTransactionRequest request)
    {
        var o = request.RequestData.Unpack<ObjectNameStringRequest>();

        var ir = new ObjectNameStringBusinessTransactionRequestData
        {
            ObjectName = o.Name,
            Value = o.Value
        };
        return ir;
    }

    /// <summary>
    /// Map <see cref="ObjectIdIntRequest"/> to <see cref="ObjectIdIntBusinessTransactionRequestData"/>. Public for unit testing. Do not use directly
    /// </summary>
    /// <param name="request">GRPC request data</param>
    /// <returns>Internal request data</returns>
    public IBusinessTransactionRequestData CreateObjectIdIntRequest(BusinessTransactionRequest request)
    {
        var o = request.RequestData.Unpack<ObjectIdIntRequest>();

        var ir = new ObjectIdIntBusinessTransactionRequestData
        {
            ObjectId = o.ObjectId,
            Value = o.Value
        };
        return ir;
    }

    /// <summary>
    /// Map <see cref="ObjectIdStringRequest"/> to <see cref="ObjectIdStringBusinessTransactionRequestData"/>. Public for unit testing. Do not use directly
    /// </summary>
    /// <param name="request">GRPC request data</param>
    /// <returns>Internal request data</returns>
    public IBusinessTransactionRequestData CreateObjectIdStringRequest(BusinessTransactionRequest request)
    {
        var o = request.RequestData.Unpack<ObjectIdStringRequest>();

        var ir = new ObjectIdStringBusinessTransactionRequestData
        {
            ObjectId = o.ObjectId,
            Value = o.Value
        };
        return ir;
    }

    /// <summary>
    /// Map <see cref="EmptyListRequest"/> to <see cref="EmptyListBusinessTransactionRequestData"/>. Public for unit testing. Do not use directly
    /// </summary>
    /// <param name="request">GRPC request data</param>
    /// <returns>Internal request data</returns>
    public IBusinessTransactionRequestData CreateEmptyListRequest(BusinessTransactionRequest request)
    {
        var o = request.RequestData.Unpack<EmptyListRequest>();

        var result = new EmptyListBusinessTransactionRequestData
        {
            Page = o.Page == 0 ? 1 : o.Page,
            PageSize = o.PageSize == 0 ? int.MaxValue : o.PageSize,
        };

        return result;
    }


    /// <summary>
    /// Map <see cref="ObjectNameRequest"/> to <see cref="ObjectNameBusinessTransactionRequestData"/>. Public for unit testing. Do not use directly
    /// </summary>
    /// <param name="request">GRPC request data</param>
    /// <returns>Internal request data</returns>
    public IBusinessTransactionRequestData CreateObjectNameRequest(BusinessTransactionRequest request)
    {
        var o = request.RequestData.Unpack<ObjectNameRequest>();

        var r1 = new ObjectNameBusinessTransactionRequestData
        {
            Name = o.Name
        };

        return r1;
    }



    /// <summary>
    /// Map <see cref="ObjectUidRequest"/> to <see cref="ObjectGuidBusinessTransactionRequestData"/>. Public for unit testing. Do not use directly
    /// </summary>
    /// <param name="request">GRPC request data</param>
    /// <returns>Internal request data</returns>
    private IBusinessTransactionRequestData CreateObjectGuidRequest(BusinessTransactionRequest request)
    {
        var o = request.RequestData.Unpack<ObjectUidRequest>();

        var guid = Guid.Empty;

        if (!string.IsNullOrEmpty(o.ObjectUid))
        {
            try
            {
                guid = new Guid(o.ObjectUid);
            }
            catch //(Exception e)
            {
                AppLogger.LogError($"Invalid GUID <<{o.ObjectUid}>> replaced with Guid.Empty");
            }
        }

        var r1 = new ObjectGuidBusinessTransactionRequestData
        {
            ObjectGuid = guid
        };

        return r1;
    }

    /// <summary>
    /// Map <see cref="ObjectIdRequest"/> to <see cref="ObjectIdBusinessTransactionRequestData"/>. Public for unit testing. Do not use directly
    /// </summary>
    /// <param name="request">GRPC request data</param>
    /// <returns>Internal request data</returns>
    public IBusinessTransactionRequestData CreateObjectIdRequest(BusinessTransactionRequest request)
    {
        var o = request.RequestData.Unpack<ObjectIdRequest>();

        var r1 = new ObjectIdBusinessTransactionRequestData
        {
            ObjectId = o.ObjectId
        };

        return r1;
    }

    /// <summary>
    /// Map <see cref="ObjectIdListRequest"/> to <see cref="ObjectIdListBusinessTransactionRequestData"/>. Public for unit testing. Do not use directly
    /// </summary>
    /// <param name="request">GRPC request data</param>
    /// <returns>Internal request data</returns>
    public IBusinessTransactionRequestData CreateObjectIdListRequest(BusinessTransactionRequest request)
    {
        var o = request.RequestData.Unpack<ObjectIdListRequest>();

        var r1 = new ObjectIdListBusinessTransactionRequestData
        {
            ObjectId = o.ObjectId,
            Page = o.Page == 0 ? 1 : o.Page,
            PageSize = o.PageSize == 0 ? int.MaxValue : o.PageSize,
        };

        return r1;
    }

    /// <summary>
    /// Create an empty request. Public for unit testing. Do not use directly
    /// </summary>
    /// <param name="request">Current GRPC request</param>
    /// <returns>Internal business transaction request</returns>
    public IBusinessTransactionRequestData CreateEmptyRequest(BusinessTransactionRequest request)
    {
        var r1 = new EmptyBusinessTransactionRequestData();

        return r1;
    }


    /// <summary>
    /// Map a GRPC request to an internal business transaction request
    /// </summary>
    /// <param name="request">Current GRPC request</param>
    /// <param name="context">Request context with metadata</param>
    /// <returns>Internal business transaction request</returns>
    public IBusinessTransactionRequestData MapToBusinessTransactionRequestData(BusinessTransactionRequest request,
        ServerCallContext context)
    {

        // Request data is required always!
        if (request?.RequestData == null)
        {
            return null;
        }

        // Now search the correct mapper and run it
        try
        {
            foreach (var kvp in AllBusinessTransactionRequestDataDelegates)
            {
                if (!request.RequestData.Is(kvp.Key))
                {
                    continue;
                }

                var s = new StringBuilder();
                s.Append($"BT {request.TransactionId}");

                // Create the internal request
                var internalRequest = kvp.Value.Invoke(request);

                if (internalRequest == null)
                {
                    s.Append($" mapping for request data {kvp.Key} returns null");
                    AppLogger.LogInformation(s.ToString());
                    return null;
                }

                // Store transaction iD to request
                internalRequest.TransactionId = request.TransactionId;

                // Client delivers transaction UID
                if (!string.IsNullOrEmpty(request.TransactionUid))
                {
                    internalRequest.TransactionGuid = new Guid(request.TransactionUid);
                }

                // Now add metadata to the request
                GetMetaDataFromRequestHeader(context, internalRequest);

                s.Append($" ({internalRequest.TransactionGuid})");

                try
                {
                    s.Append($" requested: {ObjectHelper.GetObjectPropertiesAsString(internalRequest)}");
                }
                catch //(Exception e)
                {
                    s.Append($"Object serialization for logging failed {internalRequest.GetType().Name}");
                }

                AppLogger.LogInformation(s.ToString());

                return internalRequest;

            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


        return null;
    }

    /// <summary>
    /// Helper method to provoke moving to transactions. Should be replaced with 
    /// </summary>
    /// <param name="grpcRequest">Current not business transaction based GRPC request</param>
    /// <param name="context">Request context with metadata</param>
    /// <returns>Internal business transaction request</returns>
    public IBusinessTransactionRequestData MapGrpcRequestToBusinessTransactionRequestData<T>(IMessage<T> grpcRequest,
        ServerCallContext context) where T : IMessage<T>
    {

        var request = new BusinessTransactionRequest
        {
            RequestData = Any.Pack(grpcRequest)
        };

        var result = MapToBusinessTransactionRequestData(request, context);

        return result;
    }

    /// <summary>
    /// Get the metadata from a Grpc request header
    /// </summary>
    /// <param name="context">The current server request context</param>
    /// <param name="requestMetaData"></param>
    public virtual void GetMetaDataFromRequestHeader(ServerCallContext context, IBusinessTransactionRequestData requestMetaData)
    {

        if (context == null)
        {
            return;
        }

        try
        {
            // User ID: search in the header for UserId
            var userId = context.RequestHeaders.FirstOrDefault(ent => ent.Key.ToLower().Equals("UserId"));
            requestMetaData.MetaDataUserId = userId != null ? Convert.ToInt32(userId.Value) : 0;

            // Add other tags
        }
        catch (Exception e)
        {
            AppLogger.LogError("Exception getting request header", e);
        }
    }
}