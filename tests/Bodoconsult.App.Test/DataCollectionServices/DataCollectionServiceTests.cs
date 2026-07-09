// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.DataCollectionServices;
using Bodoconsult.App.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.Test.DataCollectionServices;

internal class DataCollectionServiceTests
{
    private int _numberOfDataForwarded;
    private CancellationTokenSource _cts;
    private int _numberOfDataAdded;

    [SetUp]
    public void SetUp()
    {
        _numberOfDataForwarded = 0;
        _cts = new CancellationTokenSource();
        _numberOfDataAdded = 0;
    }

    [Test]
    public void Ctor_DefaultSetup_PropsSetCorrectly()
    {
        // Arrange 

        // Act  
        ForwardCollectDataDelegate<TestData> forwardCollectDataDelegate = ForwardCollectDataDelegate;
        var dcs = new DataCollectionService<TestData>(forwardCollectDataDelegate);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(dcs.ForwardCollectDataDelegate, Is.Not.Null);
            Assert.That(dcs.Data, Is.Not.Null);
            Assert.That(dcs.Data.Count, Is.Zero);
        }
    }

    [Test]
    public void AddList_DefaultSetupInactive_NoDataCollected()
    {
        // Arrange 
        ForwardCollectDataDelegate<TestData> forwardCollectDataDelegate = ForwardCollectDataDelegate;
        var dcs = new DataCollectionService<TestData>(forwardCollectDataDelegate);

        // Act  
        dcs.Add([new TestData()]);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(dcs.Data.Count, Is.Zero);
        }
    }

    [Test]
    public void AddItem_DefaultSetupInactive_NoDataCollected()
    {
        // Arrange 
        ForwardCollectDataDelegate<TestData> forwardCollectDataDelegate = ForwardCollectDataDelegate;
        var dcs = new DataCollectionService<TestData>(forwardCollectDataDelegate);

        // Act  
        dcs.Add(new TestData());

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(dcs.Data.Count, Is.Zero);
        }
    }

    [Test]
    public void AddItem_DefaultSetupActive_DataCollected()
    {
        // Arrange 
        ForwardCollectDataDelegate<TestData> forwardCollectDataDelegate = ForwardCollectDataDelegate;
        var dcs = new DataCollectionService<TestData>(forwardCollectDataDelegate);
        dcs.CollectionInterval = 1500;

        // Act  
        dcs.Start();
            
        Task.Delay(dcs.CollectionInterval * 3).GetAwaiter().GetResult();

        dcs.Stop();

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(dcs.Data.Count, Is.Zero);

            Wait.Until(() => _numberOfDataForwarded > 0);

            Assert.That(_numberOfDataForwarded, Is.Not.Zero);
        }
    }

    [Test]
    public void AddItem_DefaultSetupActiveMultipleItems_DataCollected()
    {
        // Arrange 
        ForwardCollectDataDelegate<TestData> forwardCollectDataDelegate = ForwardCollectDataDelegate;
        var dcs = new DataCollectionService<TestData>(forwardCollectDataDelegate);
        dcs.CollectionInterval = 1500;

        AsyncHelper.FireAndForget(() =>
        {
            ReceiveMessages(dcs);
        });

        // Act  
        dcs.Start();

        Task.Delay(dcs.CollectionInterval * 3).GetAwaiter().GetResult();
        _cts.Cancel(false);

        dcs.Stop();

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(dcs.Data.Count, Is.Zero);

            Wait.Until(() => _numberOfDataForwarded > 1);

            Assert.That(_numberOfDataForwarded, Is.Not.Zero);
            Assert.That(_numberOfDataAdded, Is.Not.Zero);
        }
    }

    [Test]
    public void AddList_DefaultSetupActive_DataCollected()
    {
        // Arrange 
        ForwardCollectDataDelegate<TestData> forwardCollectDataDelegate = ForwardCollectDataDelegate;
        var dcs = new DataCollectionService<TestData>(forwardCollectDataDelegate);
        dcs.CollectionInterval = 1500;

        // Act  
        dcs.Start();
        dcs.Add([new TestData()]);

        Task.Delay(dcs.CollectionInterval * 3).GetAwaiter().GetResult();
        dcs.Stop();

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(dcs.Data.Count, Is.Zero);

            Wait.Until(() => _numberOfDataForwarded > 0);

            Assert.That(_numberOfDataForwarded, Is.Not.Zero);
        }
    }

    private void ForwardCollectDataDelegate(List<TestData> data)
    {
        _numberOfDataForwarded += data.Count;
    }


    private void ReceiveMessages(DataCollectionService<TestData> dcs)
    {
        while (!_cts.IsCancellationRequested)
        {
            dcs.Add(new TestData());
            Task.Delay(20).GetAwaiter().GetResult();
            _numberOfDataAdded++;
        }
    }
}