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

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BusinessTransactions.Replies;
using Bodoconsult.App.GrpcBackgroundService.Interfaces;
using Bodoconsult.App.Helpers;
using Google.Protobuf.WellKnownTypes;

namespace Bodoconsult.App.GrpcBackgroundService.BusinessTransactions;

/// <summary>
/// Base class for <see cref="IGrpcBusinessTransactionReplyMappingService"/> implementations
/// </summary>
public class BaseGrpcBusinessTransactionReplyMappingService : IGrpcBusinessTransactionReplyMappingService
{

    // ToDo: JW add unit tests for methods

    /// <summary>
    /// Add reply mapping container content delegate
    /// </summary>
    /// <param name="reply">GRPC reply</param>
    /// <param name="internalReply">Business gtransaction reply</param>
    protected delegate void AddContainerContentDelegate(BusinessTransactionReply reply, IBusinessTransactionReply internalReply);

    /// <summary>
    /// Containers
    /// </summary>
    protected readonly Dictionary<string, AddContainerContentDelegate> Containers = new();

    /// <summary>
    /// Current app logger
    /// </summary>
    protected readonly IAppLoggerProxy AppLogger;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="appLogger">Current app logger</param>
    public BaseGrpcBusinessTransactionReplyMappingService(IAppLoggerProxy appLogger)
    {
        AppLogger = appLogger;

        Containers.Add(nameof(ObjectIdBusinessTransactionReply), CreateObjectIdReply);
        Containers.Add(nameof(ObjectUidBusinessTransactionReply), CreateObjectUidReply);
        Containers.Add(nameof(StringBusinessTransactionReply), CreateStringBusinessTransactionReply);
        Containers.Add(nameof(StringListBusinessTransactionReply), CreateStringListBusinessTransactionReply);
        Containers.Add(nameof(UidListBusinessTransactionReply), CreateUidListBusinessTransactionReply);
        Containers.Add(nameof(BoolBusinessTransactionReply), CreateBoolBusinessTransactionReply);
        Containers.Add(nameof(DateBusinessTransactionReply), CreateDateBusinessTransactionReply);
        Containers.Add(nameof(IntegerBusinessTransactionReply), CreateIntegerBusinessTransactionReply);
    }



    /// <summary>
    /// Map an internal reply instance <see cref="IBusinessTransactionReply"/> to a <see cref="BusinessTransactionReply"/> message
    /// </summary>
    /// <param name="internalReply">Internal business transaction reply</param>
    /// <returns>Protobuf business transaction reply</returns>
    public BusinessTransactionReply MapInternalReplyToGrpc(IBusinessTransactionReply internalReply)
    {
        if (internalReply.RequestData != null)
        {
            try
            {
                AppLogger.LogInformation(
                    $"BT {internalReply.RequestData.TransactionId} ({internalReply.RequestData.TransactionGuid}) reply: {ObjectHelper.GetObjectPropertiesAsString(internalReply)}");
            }
            catch //(Exception e)
            {
                AppLogger.LogDebug($"Object serialization for logging failed {internalReply.GetType().Name}");
            }
        }

        var result = new BusinessTransactionReply
        {
            TransactionId = internalReply.RequestData?.TransactionId ?? 0,
            TransactionUid = internalReply.RequestData?.TransactionGuid.ToString() ?? string.Empty,
            ErrorCode = internalReply.ErrorCode,
            ExceptionMessage = internalReply.ExceptionMessage ?? string.Empty,
            LogMessage = internalReply.Message ?? string.Empty,
        };

        //// Default reply: leave here
        //if (!internalReply.GetType().IsSubclassOf(typeof(DefaultBusinessTransactionReply)))
        //{
        //    return result;
        //}

        var typeName = internalReply.GetType().Name;

        var success = Containers.TryGetValue(typeName, out var containerDelegate);

        if (!success || containerDelegate==null)
        {
            return result;
        }

        containerDelegate.Invoke(result, internalReply);
        return result;
    }

    /// <summary>
    /// Map a UidListBusinessTransactionReply to a ObjectNamesReply message. Public for unit tests
    /// </summary>
    /// <param name="reply">Internal business transaction reply</param>
    /// <param name="internalReply">GRPC reply</param>
    public void CreateUidListBusinessTransactionReply(BusinessTransactionReply reply, IBusinessTransactionReply internalReply)
    {
        if (internalReply is not UidListBusinessTransactionReply sr)
        {
            return;
        }

        var ir = new ObjectNamesReply();

        foreach (var uid in sr.Uids)
        {
            ir.ObjectNames.Add(uid.ToString());
        }

        reply.ReplyData = Any.Pack(ir);

    }

    /// <summary>
    /// Map a DateBusinessTransactionReply to a DateReply message. Public for unit tests
    /// </summary>
    /// <param name="reply">Internal business transaction reply</param>
    /// <param name="internalReply">GRPC reply</param>
    public void CreateDateBusinessTransactionReply(BusinessTransactionReply reply, IBusinessTransactionReply internalReply)
    {
        if (internalReply is not DateBusinessTransactionReply sr)
        {
            return;
        }

        var ir = new LongReply
        {
            Value = sr.Date?.ToFileTimeUtc() ?? 0
        };

        reply.ReplyData = Any.Pack(ir);
    }

    /// <summary>
    /// Map a IntegerBusinessTransactionReply to a IntegerReply message. Public for unit tests
    /// </summary>
    /// <param name="reply">Internal business transaction reply</param>
    /// <param name="internalReply">GRPC reply</param>
    public void CreateIntegerBusinessTransactionReply(BusinessTransactionReply reply, IBusinessTransactionReply internalReply)
    {
        if (internalReply is not IntegerBusinessTransactionReply sr)
        {
            return;
        }

        var ir = new IntegerReply
        {
            Value = sr.Value
        };

        reply.ReplyData = Any.Pack(ir);
    }

    /// <summary>
    /// Map a BoolBusinessTransactionReply to a BoolReply message. Public for unit tests
    /// </summary>
    /// <param name="reply">Internal business transaction reply</param>
    /// <param name="internalReply">GRPC reply</param>
    public void CreateBoolBusinessTransactionReply(BusinessTransactionReply reply, IBusinessTransactionReply internalReply)
    {
        if (internalReply is not BoolBusinessTransactionReply sr)
        {
            return;
        }

        var ir = new BoolReply
        {
            Value = sr.Value
        };

        reply.ReplyData = Any.Pack(ir);
    }

    /// <summary>
    /// Map a StringBusinessTransactionReply to a StringReply message. Public for unit tests
    /// </summary>
    /// <param name="reply">Internal business transaction reply</param>
    /// <param name="internalReply">GRPC reply</param>
    public void CreateStringBusinessTransactionReply(BusinessTransactionReply reply, IBusinessTransactionReply internalReply)
    {
        if (internalReply is not StringBusinessTransactionReply lr)
        {
            return;
        }

        var ir = new StringReply
        {
            Message = lr.Content ?? string.Empty
        };

        reply.ReplyData = Any.Pack(ir);
    }

    /// <summary>
    /// Map a ObjectUidBusinessTransactionReply to a ObjectUidReply message. Public for unit tests
    /// </summary>
    /// <param name="reply">Internal business transaction reply</param>
    /// <param name="internalReply">GRPC reply</param>
    public void CreateStringListBusinessTransactionReply(BusinessTransactionReply reply, IBusinessTransactionReply internalReply)
    {

        // String list provided
        if (internalReply is not StringListBusinessTransactionReply sl)
        {
            return;
        }

        var ir = new ObjectNamesReply();

        foreach (var s in sl.Content)
        {
            ir.ObjectNames.Add(s ?? string.Empty);
        }

        reply.ReplyData = Any.Pack(ir);
    }

    /// <summary>
    /// Map a ObjectUidBusinessTransactionReply to a ObjectUidReply message. Public for unit tests
    /// </summary>
    /// <param name="reply">Internal business transaction reply</param>
    /// <param name="internalReply">GRPC reply</param>
    public void CreateObjectUidReply(BusinessTransactionReply reply, IBusinessTransactionReply internalReply)
    {
        // Object UID provided
        if (internalReply is not ObjectUidBusinessTransactionReply s)
        {
            return;
        }
        var ir = new ObjectUidReply
        {
            ObjectUid = s.ObjectUid.ToString()
        };

        reply.ReplyData = Any.Pack(ir);
    }

    /// <summary>
    /// Map a ObjectIdBusinessTransactionReply to a ObjectIdReply message. Public for unit tests
    /// </summary>
    /// <param name="reply">Internal business transaction reply</param>
    /// <param name="internalReply">GRPC reply</param>
    public void CreateObjectIdReply(BusinessTransactionReply reply, IBusinessTransactionReply internalReply)
    {
        // Object UID provided
        if (internalReply is not ObjectIdBusinessTransactionReply s)
        {
            return;
        }

        var ir = new ObjectIdReply
        {
            ObjectId = s.ObjectId
        };

        reply.ReplyData = Any.Pack(ir);
    }

}