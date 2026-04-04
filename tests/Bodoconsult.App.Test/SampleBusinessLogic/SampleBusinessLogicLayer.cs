// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BusinessTransactions.Replies;

namespace Bodoconsult.App.Test.SampleBusinessLogic;

internal class SampleBusinessLogicLayer
{

    public IBusinessTransactionReply EmptyRequest(IBusinessTransactionRequestData requestData)
    {

        return new DefaultBusinessTransactionReply
        {
            ErrorCode = 0,
            Message = "Testmessage on success"
        };
    }



}