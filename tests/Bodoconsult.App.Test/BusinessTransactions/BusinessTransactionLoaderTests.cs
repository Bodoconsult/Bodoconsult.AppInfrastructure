// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BusinessTransactions;
using Bodoconsult.App.BusinessTransactions.RequestData;
using Bodoconsult.App.Test.Helpers;
using Bodoconsult.App.Test.SampleBusinessLogic;
using Bodoconsult.App.Test.TestData;

namespace Bodoconsult.App.Test.BusinessTransactions;

[TestFixture]
internal class BusinessTransactionLoaderTests
{


    [Test]
    public void LoadProviders_ValidSetup_Success()
    {
        // Arrange 
        var aes = TestHelper.CreateAppEventSourceFactory();
        var logger = TestHelper.GetFakeAppLoggerProxy();

        const int transactionId = 1000;
        var m = new BusinessTransactionManager(logger, aes);

        var dependency = new SampleBusinessLogicLayer();
        var p = new TestBusinessTransactionLoader(m, dependency);

        p.LoadProviders();

        IBusinessTransactionRequestData requestData = new EmptyBusinessTransactionRequestData();

        // Act  
        var t = m.RunBusinessTransaction(transactionId, requestData);

        // Assert
        Assert.That(t, Is.Not.Null);
        Assert.That(m.TransactionCache, Has.Count.EqualTo(1));
        Assert.That(t.RequestData, Is.EqualTo(requestData));

    }


}