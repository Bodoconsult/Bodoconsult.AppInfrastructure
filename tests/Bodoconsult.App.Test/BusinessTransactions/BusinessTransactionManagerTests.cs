// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.BusinessTransactions;
using Bodoconsult.App.BusinessTransactions.RequestData;
using Bodoconsult.App.Test.Helpers;
using Bodoconsult.App.Test.SampleBusinessLogic;
using Bodoconsult.App.Test.TestData;

namespace Bodoconsult.App.Test.BusinessTransactions;

[TestFixture]
internal class BusinessTransactionManagerTests
{

    private readonly IAppLoggerProxy _logger = TestHelper.GetFakeAppLoggerProxy();

    [Test]
    public void Ctor_ValidDefaultSetup_PropsSetCorrectly()
    {
        // Arrange 
        var aes = TestHelper.CreateAppEventSourceFactory();

        // Act  
        var m = new BusinessTransactionManager(_logger, aes);

        // Assert
        Assert.That(m.CreateBusinessTransactionDelegates, Is.Not.Null);
        Assert.That(m.CreateBusinessTransactionDelegates.Count, Is.EqualTo(0));

    }


    [Test]
    public void AddProvider_ValidProvider_ProviderLoaded()
    {
        // Arrange 
        var aes = TestHelper.CreateAppEventSourceFactory();

        var m = new BusinessTransactionManager(_logger, aes);

        var dependency = new SampleBusinessLogicLayer();
        var p = new TestBusinessTransactionProvider(dependency);

        // Act  
        m.AddProvider(p);

        // Assert
        Assert.That(m.CreateBusinessTransactionDelegates.Count, Is.EqualTo(p.CreateBusinessTransactionDelegates.Count));
    }

    [Test]
    public void CheckForBusinessTransaction_ValidTransaction_Success()
    {
        // Arrange 
        var aes = TestHelper.CreateAppEventSourceFactory();

        const int transactionId = 1000;
        var m = new BusinessTransactionManager(_logger, aes);
        var dependency = new SampleBusinessLogicLayer();
        var p = new TestBusinessTransactionProvider(dependency);

        m.AddProvider(p);

        // Act  
        var t = m.CheckForBusinessTransaction(transactionId);

        // Assert
        Assert.That(t, Is.Not.Null);
        Assert.That(m.TransactionCache.Count, Is.EqualTo(1));

    }

    [Test]
    public void CheckForBusinessTransaction_ValidTransactionRepeated_Success()
    {
        // Arrange 
        var aes = TestHelper.CreateAppEventSourceFactory();

        const int transactionId = 1000;
        var m = new BusinessTransactionManager(_logger, aes);
        var dependency = new SampleBusinessLogicLayer();
        var p = new TestBusinessTransactionProvider(dependency);

        m.AddProvider(p);

        // Act  
        var t = m.CheckForBusinessTransaction(transactionId);
        var t2 = m.CheckForBusinessTransaction(transactionId);

        // Assert
        Assert.That(t, Is.Not.Null);
        Assert.That(m.TransactionCache.Count, Is.EqualTo(1));
        Assert.That(t2, Is.EqualTo(t));
    }

    [Test]
    public void CheckForBusinessTransaction_NoTransactionLoaded_NoSuccess()
    {
        // Arrange 
        var aes = TestHelper.CreateAppEventSourceFactory();

        const int transactionId = 1000;
        var m = new BusinessTransactionManager(_logger, aes);
        var p = new TestBusinessTransactionProviderNoDelegate();

        m.AddProvider(p);

        // Act and assert
        Assert.Throws<ArgumentException>(() =>
        {
            var t = m.CheckForBusinessTransaction(transactionId);
        });
    }

    [Test]
    public void RunBusinessTransaction_ValidBt_Success()
    {
        // Arrange 
        var aes = TestHelper.CreateAppEventSourceFactory();

        const int transactionId = 1000;
        var m = new BusinessTransactionManager(_logger, aes);
        var dependency = new SampleBusinessLogicLayer();
        var p = new TestBusinessTransactionProvider(dependency);

        m.AddProvider(p);

        IBusinessTransactionRequestData requestData = new EmptyBusinessTransactionRequestData();

        // Act  
        var t = m.RunBusinessTransaction(transactionId, requestData);

        // Assert
        Assert.That(t, Is.Not.Null);
        Assert.That(m.TransactionCache, Has.Count.EqualTo(1));
        Assert.That(t.RequestData, Is.EqualTo(requestData));
    }

}