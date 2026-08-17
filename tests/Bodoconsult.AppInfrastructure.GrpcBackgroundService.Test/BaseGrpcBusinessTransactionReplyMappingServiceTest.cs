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
using Bodoconsult.App.GrpcBackgroundService.BusinessTransactions;
using Bodoconsult.App.Test.Helpers;

namespace Bodoconsult.App.GrpcBackgroundService.Test;

[TestFixture]
//[NonParallelizable]
//[SingleThreaded]
public class BaseGrpcBusinessTransactionReplyMappingServiceTests
{

    private readonly IAppLoggerProxy _logger = TestHelper.GetFakeAppLoggerProxy();


    ///// <summary>
    ///// Default ctor for all tests
    ///// </summary>
    //public BaseBusinessTransactionReplyMappingServiceTests()
    //{

    //}


    [Test]
    public void MapInternalReply_DefaultBusinessTransactionReply_ToGrpcBusinessTransactionReply()
    {
        // Arrange 
        var rms = new BaseGrpcBusinessTransactionReplyMappingService(_logger);

        var internalReply = new DefaultBusinessTransactionReply
        {
            ErrorCode = 99,
            Message = "Testmessage",
            ExceptionMessage = "ExceptionMessage"
        };

        // Act  
        var result = rms.MapInternalReplyToGrpc(internalReply);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ErrorCode, Is.EqualTo(internalReply.ErrorCode));
        Assert.That(result.LogMessage, Is.EqualTo(internalReply.Message));
        Assert.That(result.ExceptionMessage, Is.EqualTo(internalReply.ExceptionMessage));
    }

    [Test]
    public void MapInternalReply_BoolBusinessTransactionReply_ToGrpcBusinessTransactionReply()
    {
        // Arrange 
        var rms = new BaseGrpcBusinessTransactionReplyMappingService(_logger);

        var internalReply = new BoolBusinessTransactionReply
        {
            ErrorCode = 99,
            Message = "Testmessage",
            ExceptionMessage = "ExceptionMessage",
            Value = true
        };

        // Act  
        var result = rms.MapInternalReplyToGrpc(internalReply);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ErrorCode, Is.EqualTo(internalReply.ErrorCode));
        Assert.That(result.LogMessage, Is.EqualTo(internalReply.Message));
        Assert.That(result.ExceptionMessage, Is.EqualTo(internalReply.ExceptionMessage));

        var success = result.ReplyData.TryUnpack<BoolReply>(out var o);

        Assert.That(success, Is.EqualTo(true));
        Assert.That(o.Value, Is.EqualTo(internalReply.Value));
    }

    [Test]
    public void MapInternalReply_DateBusinessTransactionReply_ValidDate_ToGrpcBusinessTransactionReply()
    {
        // Arrange 
        var rms = new BaseGrpcBusinessTransactionReplyMappingService(_logger);

        var date = DateTime.Now;

        var internalReply = new DateBusinessTransactionReply
        {
            ErrorCode = 99,
            Message = "Testmessage",
            ExceptionMessage = "ExceptionMessage",
            Date = date
        };

        // Act  
        var result = rms.MapInternalReplyToGrpc(internalReply);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ErrorCode, Is.EqualTo(internalReply.ErrorCode));
        Assert.That(result.LogMessage, Is.EqualTo(internalReply.Message));
        Assert.That(result.ExceptionMessage, Is.EqualTo(internalReply.ExceptionMessage));

        var success = result.ReplyData.TryUnpack<LongReply>(out var o);

        Assert.That(success, Is.EqualTo(true));
        Assert.That(o.Value, Is.EqualTo(date.ToFileTimeUtc()));
    }

    [Test]
    public void MapInternalReply_DateBusinessTransactionReply_NullDate_ToGrpcBusinessTransactionReply()
    {
        // Arrange 
        var rms = new BaseGrpcBusinessTransactionReplyMappingService(_logger);

        var internalReply = new DateBusinessTransactionReply
        {
            ErrorCode = 99,
            Message = "Testmessage",
            ExceptionMessage = "ExceptionMessage",
            Date = null
        };

        // Act  
        var result = rms.MapInternalReplyToGrpc(internalReply);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ErrorCode, Is.EqualTo(internalReply.ErrorCode));
        Assert.That(result.LogMessage, Is.EqualTo(internalReply.Message));
        Assert.That(result.ExceptionMessage, Is.EqualTo(internalReply.ExceptionMessage));

        var success = result.ReplyData.TryUnpack<LongReply>(out var o);

        Assert.That(success, Is.EqualTo(true));
        Assert.That(o.Value, Is.EqualTo(0));
    }


    [Test]
    public void MapInternalReply_DefaultBusinessTransactionReplyEmptyMessages_ToGrpcBusinessTransactionReply()
    {
        // Arrange 
        var rms = new BaseGrpcBusinessTransactionReplyMappingService(_logger);

        var internalReply = new DefaultBusinessTransactionReply
        {
            ErrorCode = 99,
            Message = null,
            ExceptionMessage = null
        };

        // Act  
        var result = rms.MapInternalReplyToGrpc(internalReply);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ErrorCode, Is.EqualTo(internalReply.ErrorCode));
        Assert.That(result.LogMessage, Is.EqualTo(string.Empty));
        Assert.That(result.ExceptionMessage, Is.EqualTo(string.Empty));
    }

    [Test]
    public void MapInternalReply_StringBusinessTransactionReply_ToGrpcBusinessTransactionReply()
    {
        // Arrange 
        var rms = new BaseGrpcBusinessTransactionReplyMappingService(_logger);

        var internalReply = new StringBusinessTransactionReply
        {
            ErrorCode = 99,
            Message = "Testmessage",
            ExceptionMessage = "ExceptionMessage",
            Content = "Balla"
        };

        // Act  
        var result = rms.MapInternalReplyToGrpc(internalReply);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ErrorCode, Is.EqualTo(internalReply.ErrorCode));
        Assert.That(result.LogMessage, Is.EqualTo(internalReply.Message));
        Assert.That(result.ExceptionMessage, Is.EqualTo(internalReply.ExceptionMessage));

        var success = result.ReplyData.TryUnpack<StringReply>(out var o);

        Assert.That(success, Is.EqualTo(true));
        Assert.That(o.Message, Is.EqualTo(internalReply.Content));
    }


    [Test]
    public void MapInternalReply_StringListBusinessTransactionReply_ToGrpcBusinessTransactionReply()
    {
        // Arrange 
        var rms = new BaseGrpcBusinessTransactionReplyMappingService(_logger);

        var internalReply = new StringListBusinessTransactionReply
        {
            ErrorCode = 99,
            Message = "Testmessage",
            ExceptionMessage = "ExceptionMessage",
        };

        internalReply.Content.Add("Blubb");

        // Act  
        var result = rms.MapInternalReplyToGrpc(internalReply);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ErrorCode, Is.EqualTo(internalReply.ErrorCode));
        Assert.That(result.LogMessage, Is.EqualTo(internalReply.Message));
        Assert.That(result.ExceptionMessage, Is.EqualTo(internalReply.ExceptionMessage));

        var success = result.ReplyData.TryUnpack<ObjectNamesReply>(out var o);

        Assert.That(success, Is.EqualTo(true));
        Assert.That(o.ObjectNames.Count, Is.EqualTo(1));
        Assert.That(o.ObjectNames[0], Is.EqualTo(internalReply.Content[0]));
    }

    [Test]
    public void MapInternalReply_StringListBusinessTransactionReplyEmptyStrings_ToGrpcBusinessTransactionReply()
    {
        // Arrange 
        var rms = new BaseGrpcBusinessTransactionReplyMappingService(_logger);
        var value = string.Empty;

        var internalReply = new StringListBusinessTransactionReply
        {
            ErrorCode = 99,
            Message = "Testmessage",
            ExceptionMessage = "ExceptionMessage",
        };

        internalReply.Content.Add(value);

        // Act  
        var result = rms.MapInternalReplyToGrpc(internalReply);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ErrorCode, Is.EqualTo(internalReply.ErrorCode));
        Assert.That(result.LogMessage, Is.EqualTo(internalReply.Message));
        Assert.That(result.ExceptionMessage, Is.EqualTo(internalReply.ExceptionMessage));

        var success = result.ReplyData.TryUnpack<ObjectNamesReply>(out var o);

        Assert.That(success, Is.EqualTo(true));
        Assert.That(o.ObjectNames.Count, Is.EqualTo(1));
        Assert.That(o.ObjectNames[0], Is.EqualTo(value));
    }

    [Test]
    public void MapInternalReply_StringBusinessTransactionReplyEmptyMessages_ToGrpcBusinessTransactionReply()
    {
        // Arrange 
        var rms = new BaseGrpcBusinessTransactionReplyMappingService(_logger);

        var internalReply = new StringBusinessTransactionReply
        {
            ErrorCode = 99,
            Message = null,
            ExceptionMessage = null,
            Content = null
        };

        // Act  
        var result = rms.MapInternalReplyToGrpc(internalReply);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ErrorCode, Is.EqualTo(internalReply.ErrorCode));
        Assert.That(result.LogMessage, Is.EqualTo(string.Empty));
        Assert.That(result.ExceptionMessage, Is.EqualTo(string.Empty));

        var success = result.ReplyData.TryUnpack<StringReply>(out var o);

        Assert.That(success, Is.EqualTo(true));
        Assert.That(o.Message, Is.EqualTo(string.Empty));
    }

    [Test]
    public void MapInternalReply_ObjectUidBusinessTransactionReply_ToGrpcBusinessTransactionReply()
    {
        // Arrange 
        var rms = new BaseGrpcBusinessTransactionReplyMappingService(_logger);

        var internalReply = new ObjectUidBusinessTransactionReply
        {
            ErrorCode = 99,
            Message = null,
            ExceptionMessage = null,
            ObjectUid = Guid.NewGuid()
        };

        // Act  
        var result = rms.MapInternalReplyToGrpc(internalReply);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ErrorCode, Is.EqualTo(internalReply.ErrorCode));
        Assert.That(result.LogMessage, Is.EqualTo(string.Empty));
        Assert.That(result.ExceptionMessage, Is.EqualTo(string.Empty));

        var success = result.ReplyData.TryUnpack<ObjectUidReply>(out var o);

        Assert.That(success, Is.EqualTo(true));
        Assert.That(o.ObjectUid, Is.EqualTo(internalReply.ObjectUid.ToString()));
    }

    [Test]
    public void MapInternalReply_ObjectIdBusinessTransactionReply_ToGrpcBusinessTransactionReply()
    {
        // Arrange 
        var rms = new BaseGrpcBusinessTransactionReplyMappingService(_logger);

        var internalReply = new ObjectIdBusinessTransactionReply
        {
            ErrorCode = 99,
            Message = null,
            ExceptionMessage = null,
            ObjectId = 747
        };

        // Act  
        var result = rms.MapInternalReplyToGrpc(internalReply);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ErrorCode, Is.EqualTo(internalReply.ErrorCode));
        Assert.That(result.LogMessage, Is.EqualTo(string.Empty));
        Assert.That(result.ExceptionMessage, Is.EqualTo(string.Empty));

        var success = result.ReplyData.TryUnpack<ObjectIdReply>(out var o);

        Assert.That(success, Is.EqualTo(true));
        Assert.That(o.ObjectId, Is.EqualTo(internalReply.ObjectId));
    }

    [Test]
    public void MapInternalReply_UidListBusinessTransactionReply_ToGrpcBusinessTransactionReply()
    {
        // Arrange 
        var rms = new BaseGrpcBusinessTransactionReplyMappingService(_logger);

        var internalReply = new UidListBusinessTransactionReply
        {
            ErrorCode = 99,
            Message = null,
            ExceptionMessage = null,

        };

        var guid = Guid.NewGuid();

        internalReply.Uids.Add(guid);

        // Act  
        var result = rms.MapInternalReplyToGrpc(internalReply);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ErrorCode, Is.EqualTo(internalReply.ErrorCode));
        Assert.That(result.LogMessage, Is.EqualTo(string.Empty));
        Assert.That(result.ExceptionMessage, Is.EqualTo(string.Empty));

        var success = result.ReplyData.TryUnpack<ObjectNamesReply>(out var o);

        Assert.That(success, Is.EqualTo(true));
        Assert.That(o.ObjectNames.Count, Is.EqualTo(1));
        Assert.That(o.ObjectNames.Contains(guid.ToString()));

    }

}