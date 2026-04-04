// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BusinessTransactions.Replies;

namespace GrpcServerApp.BusinessLogic.Interfaces;

public interface IDemoBl
{
    DefaultBusinessTransactionReply DoSomething(IBusinessTransactionRequestData request);

    ObjectIdBusinessTransactionReply DoSomethingElse(IBusinessTransactionRequestData request);
}