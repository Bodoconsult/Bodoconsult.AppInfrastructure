// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.BufferPool;

namespace Bodoconsult.App.Test.BufferPool;

[TestFixture]
internal class BufferPoolResetableTests
{
    private const int NumberOfItems = 1000;

    [Test]
    public void Allocate_ValidSetup_QueueFilled()
    {
        // Arrange 
        var myPool = new BufferPoolResetable<TestDataResetable>(() => new TestDataResetable());

        // Act  
        myPool.Allocate(NumberOfItems);

        // Assert
        Assert.That(myPool.LengthOfQueue, Is.EqualTo(NumberOfItems));
    }

    [Test]
    public void Dequeue_ValidSetup_InstanceDequeued()
    {
        // Arrange 
        var myPool = new BufferPoolResetable<TestDataResetable>(() => new TestDataResetable());
        myPool.Allocate(NumberOfItems);

        // Act  
        var buffer = myPool.Dequeue();

        // Assert
        Assert.That(buffer, Is.Not.Null);
        Assert.That(myPool.LengthOfQueue, Is.EqualTo(NumberOfItems - 1));
    }


    [Test]
    public void Enqueue_ValidSetup_InstanceEnqueued()
    {
        // Arrange 
        var myPool = new BufferPoolResetable<TestDataResetable>(() => new TestDataResetable());
        myPool.Allocate(1000);

        var buffer = myPool.Dequeue();

        // Act  
        myPool.Enqueue(buffer);

        // Assert
        Assert.That(myPool.LengthOfQueue, Is.EqualTo(NumberOfItems));
    }
}