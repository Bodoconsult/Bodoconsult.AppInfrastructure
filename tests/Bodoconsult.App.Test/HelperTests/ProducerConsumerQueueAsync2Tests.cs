// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.Test.HelperTests;

[TestFixture]
public class ProducerConsumerQueueAsync2Tests
{
    private readonly Memory<byte> _data = new byte[] { 0x0, 0x1 }.AsMemory();
    private readonly Memory<byte> _data2 = new byte[] { 0x0, 0x1 }.AsMemory();
    private readonly Memory<byte> _data3 = new byte[] { 0x0, 0x1 }.AsMemory();
    private int _counter;
    private readonly List<Memory<byte>> _received = [];
    private bool _wasFired;

    private void Reset()
    {
        _counter = 0;
        _received.Clear();
        _wasFired = false;
    }

    private Task ConsumerTaskDelegate(Memory<byte> value)
    {
        _counter++;
        _received.Add(value);
        _wasFired = true;
        return Task.CompletedTask;
    }

    [Test]
    public void Ctor_DefaultSetup_PropsSetCorrectly()
    {
        // Arrange 
        Reset();

        // Act  
        var pc = new ProducerConsumerQueueAsync2<Memory<byte>>
        {
            ConsumerTaskDelegate = ConsumerTaskDelegate
        };

        // Assert
        Assert.That(pc, Is.Not.Null);
        Assert.That(pc.IsActivated, Is.False);
        Assert.That(_counter, Is.EqualTo(0));
    }

    [Test]
    public void StartConsumer_DefaultSetup_IsActivated()
    {
        // Arrange 
        Reset();

        var pc = new ProducerConsumerQueueAsync2<Memory<byte>>
        {
            ConsumerTaskDelegate = ConsumerTaskDelegate
        };

        // Act  
        pc.StartConsumer();

        // Assert
        Assert.That(pc.IsActivated, Is.True);
        pc.StopConsumer();
        Assert.That(pc.IsActivated, Is.False);
    }

    [Test]
    public void Enqueue_OneString_IsActivated()
    {
        // Arrange 
        Reset();

        var pc = new ProducerConsumerQueueAsync2<Memory<byte>>
        {
            ConsumerTaskDelegate = ConsumerTaskDelegate
        };
        pc.StartConsumer();

        // Act  
        pc.Enqueue(_data);

        // Assert
        Wait.Until(() => _counter > 0);
        Assert.That(_counter, Is.EqualTo(1));
        Assert.That(_received.Count, Is.EqualTo(1));
        Assert.That(_received.Contains(_data), Is.True);

        pc.StopConsumer();
        Assert.That(pc.IsActivated, Is.False);
    }

    [Test]
    public void Enqueue_ListOneString_IsActivated()
    {
        // Arrange 
        Reset();

        var pc = new ProducerConsumerQueueAsync2<Memory<byte>>
        {
            ConsumerTaskDelegate = ConsumerTaskDelegate
        };
        pc.StartConsumer();

        // Act  
        pc.Enqueue([_data]);

        // Assert
        Wait.Until(() => _counter > 0);
        Assert.That(_counter, Is.EqualTo(1));
        Assert.That(_received.Count, Is.EqualTo(1));
        Assert.That(_received.Contains(_data), Is.True);

        pc.StopConsumer();
        Assert.That(pc.IsActivated, Is.False);
    }


    [Test]
    public void Enqueue_MultipleStrings_IsActivated()
    {
        // Arrange 
        Reset();

        var pc = new ProducerConsumerQueueAsync2<Memory<byte>>
        {
            ConsumerTaskDelegate = ConsumerTaskDelegate
        };
        pc.StartConsumer();

        // Act  
        pc.Enqueue(_data);
        pc.Enqueue(_data2);
        pc.Enqueue(_data3);

        // Assert
        Wait.Until(() => _counter > 0);
        Assert.That(_counter, Is.EqualTo(3));
        Assert.That(_received.Count, Is.EqualTo(3));
        Assert.That(_received.Contains(_data), Is.True);
        Assert.That(_received.Contains(_data2), Is.True);
        Assert.That(_received.Contains(_data3), Is.True);

        pc.StopConsumer();
        Assert.That(pc.IsActivated, Is.False);
    }

    [Test]
    public void TestStartConsumerNoDelegateSet()
    {
        // Arrange 
        Reset();

        var queue = new ProducerConsumerQueueAsync2<Memory<byte>>();

        // Act and assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            queue.StartConsumer();
        });

        // Assert
        Assert.That(queue.InternalQueue, Is.Null);
        queue.Dispose();
    }

    [Test]
    public void TestEnqueueNotStartedYet()
    {
        // Arrange 
        Reset();

        var queue = new ProducerConsumerQueueAsync2<Memory<byte>>();

        // Act and assert
        Assert.DoesNotThrow(() =>
        {
            queue.Enqueue(_data);
        });

    }

    [Test]
    public void TestEnqueue()
    {
        // Arrange 
        Reset();

        var queue = new ProducerConsumerQueueAsync2<Memory<byte>>
        {
            ConsumerTaskDelegate = ConsumerTaskDelegate
        };
        queue.StartConsumer();

        // Act and assert
        Assert.DoesNotThrow(() =>
        {
            queue.Enqueue(_data);
        });

        // Assert
        Wait.Until(() => _wasFired, 300);
        Assert.That(_wasFired, Is.EqualTo(true));
        queue.Dispose();
    }

    [Test]
    public void TestStopConsumer()
    {
        // Arrange 
        Reset();

        var queue = new ProducerConsumerQueueAsync2<Memory<byte>>
        {
            ConsumerTaskDelegate = ConsumerTaskDelegate
        };
        queue.StartConsumer();

        Assert.DoesNotThrow(() =>
        {
            queue.Enqueue(_data);
        });

        // Act 
        queue.StopConsumer();

        // Assert
        Wait.Until(() => _wasFired, 100);
        Assert.That(_wasFired, Is.EqualTo(true));
        Assert.That(queue.InternalQueue, Is.Null);
        queue.Dispose();
    }
}