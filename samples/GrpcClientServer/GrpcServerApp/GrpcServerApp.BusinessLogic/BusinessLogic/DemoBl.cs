// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BusinessTransactions.Replies;
using Bodoconsult.App.BusinessTransactions.RequestData;
using GrpcServerApp.BusinessLogic.Interfaces;

namespace GrpcServerApp.BusinessLogic.BusinessLogic;

public class DemoBl : IDemoBl
{

    public DefaultBusinessTransactionReply DoSomething(IBusinessTransactionRequestData request)
    {
        var reply = new DefaultBusinessTransactionReply();

        if (request is not EmptyBusinessTransactionRequestData)
        {
            reply.Message = "Wrong type of request: EmptyBusinessTransactionRequestData is expected!";
            reply.ErrorCode = 99;
            return reply;
        }

        try
        {
            // Do something
            reply.Message = "Doing something was successfully";
        }
        catch (Exception e)
        {
            reply.Message = "Doing something failed";
            reply.ErrorCode = 98;
            reply.ExceptionMessage = e.ToString();
        }

        return reply;
    }


    public ObjectIdBusinessTransactionReply DoSomethingElse(IBusinessTransactionRequestData request)
    {
        var reply = new ObjectIdBusinessTransactionReply();


        if (request is not ObjectIdBusinessTransactionRequestData oRequest)
        {
            reply.Message = "Wrong type of request: ObjectIdBusinessTransactionRequestData is expected!";
            reply.ErrorCode = 99;
            return reply;
        }

        try
        {
            reply.ObjectId = oRequest.ObjectId;
            reply.Message = $"Got ID {oRequest.ObjectId} successfully";
        }
        catch (Exception e)
        {
            reply.Message = $"Getting ID {oRequest.ObjectId} failed";
            reply.ErrorCode = 98;
            reply.ExceptionMessage = e.ToString();
        }

        return reply;
    }

}