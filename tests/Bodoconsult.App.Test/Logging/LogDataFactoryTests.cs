// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.App.Logging;

namespace Bodoconsult.App.Test.Logging;

[TestFixture]
internal class LogDataFactoryTests 
{


    [Test]
    public void TestCtor()
    {
        // Arrange 

        // Act  
        var factory = new LogDataFactory();

        // Assert
        Assert.That(factory.CurrentNumberOfInstancesInPool, Is.EqualTo(0));
    }

    [Test]
    public void TestAllocateBufferPool()
    {
        // Arrange 
        const int numberOfInstances = 50;
        var factory = new LogDataFactory();

        // Act  
        factory.AllocateBufferPool(numberOfInstances);

        // Assert
        Assert.That(factory.CurrentNumberOfInstancesInPool, Is.EqualTo(numberOfInstances));
    }

    [Test]
    public void TestDequeueInstance()
    {
        // Arrange 
        const int numberOfInstances = 50;
        var factory = new LogDataFactory();

        factory.AllocateBufferPool(numberOfInstances);

        // Act  
        var logData = factory.DequeueInstance();

        // Assert
        Assert.That(logData, Is.Not.Null);
        Assert.That(factory.CurrentNumberOfInstancesInPool, Is.EqualTo(numberOfInstances - 1));
    }

    [Test]
    public void TestEnqueueInstance()
    {
        // Arrange 
        const int numberOfInstances = 50;
        var factory = new LogDataFactory();

        factory.AllocateBufferPool(numberOfInstances);

        var logData = factory.DequeueInstance();

        // Act  
        factory.EnqueueInstance(logData);

        // Assert
        Assert.That(factory.CurrentNumberOfInstancesInPool, Is.EqualTo(numberOfInstances));
    }
}
