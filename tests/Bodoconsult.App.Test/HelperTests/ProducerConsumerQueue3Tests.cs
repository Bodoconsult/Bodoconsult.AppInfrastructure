// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.Test.HelperTests;

[TestFixture]
public class ProducerConsumerQueue3Tests
{

    private int _counter;
    private readonly List<string> _received = [];
    private bool _wasFired;

    private void Reset()
    {
        _counter = 0;
        _received.Clear();
        _wasFired = false;
    }

    private void ConsumerTaskDelegate(string value)
    {
        _counter++;
        _received.Add(value);
        _wasFired = true;
    }

    [Test]
    public void Ctor_DefaultSetup_PropsSetCorrectly()
    {
        // Arrange 
        Reset();

        // Act  
        var pc = new ProducerConsumerQueue3<string>
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

        var pc = new ProducerConsumerQueue3<string>
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

        const string s1 = "Blubb";

        var pc = new ProducerConsumerQueue3<string>
        {
            ConsumerTaskDelegate = ConsumerTaskDelegate
        };
        pc.StartConsumer();

        // Act  
        pc.Enqueue(s1);

        // Assert
        Wait.Until(() => _counter > 0);
        Assert.That(_counter, Is.EqualTo(1));
        Assert.That(_received.Count, Is.EqualTo(1));
        Assert.That(_received.Contains(s1), Is.True);

        pc.StopConsumer();
        Assert.That(pc.IsActivated, Is.False);
    }

    [Test]
    public void Enqueue_ListOneString_IsActivated()
    {
        // Arrange 
        Reset();

        const string s1 = "Blubb";

        var pc = new ProducerConsumerQueue3<string>
        {
            ConsumerTaskDelegate = ConsumerTaskDelegate
        };
        pc.StartConsumer();

        // Act  
        pc.Enqueue([s1]);

        // Assert
        Wait.Until(() => _counter > 0);
        Assert.That(_counter, Is.EqualTo(1));
        Assert.That(_received.Count, Is.EqualTo(1));
        Assert.That(_received.Contains(s1), Is.True);

        pc.StopConsumer();
        Assert.That(pc.IsActivated, Is.False);
    }


    [Test]
    public void Enqueue_MultipleStrings_IsActivated()
    {
        // Arrange 
        Reset();

        const string s1 = "Blubb";
        const string s2 = "Blabb";
        const string s3 = "Blobb";

        var pc = new ProducerConsumerQueue3<string>
        {
            ConsumerTaskDelegate = ConsumerTaskDelegate
        };
        pc.StartConsumer();

        // Act  
        pc.Enqueue(s1);
        pc.Enqueue(s2);
        pc.Enqueue(s3);

        // Assert
        Wait.Until(() => _counter > 0);
        Assert.That(_counter, Is.EqualTo(3));
        Assert.That(_received.Count, Is.EqualTo(3));
        Assert.That(_received.Contains(s1), Is.True);
        Assert.That(_received.Contains(s2), Is.True);
        Assert.That(_received.Contains(s3), Is.True);

        pc.StopConsumer();
        Assert.That(pc.IsActivated, Is.False);
    }

    [Test]
    public void TestStartConsumerNoDelegateSet()
    {
        // Arrange 
        Reset();

        var queue = new ProducerConsumerQueue3<string>();

        // Act and assert
        Assert.Throws<ArgumentNullException>(queue.StartConsumer);

        // Assert
        queue.Dispose();
    }

    [Test]
    public void TestEnqueueNotStartedYet()
    {
        // Arrange 
        Reset();

        var queue = new ProducerConsumerQueue3<string>();

        // Act and assert
        Assert.DoesNotThrow(() =>
        {
            queue.Enqueue("Test");
        });

    }

    [Test]
    public void TestEnqueue()
    {
        // Arrange 
        Reset();

        var queue = new ProducerConsumerQueue3<string>
        {
            ConsumerTaskDelegate = ConsumerTaskDelegate
        };
        queue.StartConsumer();

        // Act and assert
        Assert.DoesNotThrow(() =>
        {
            queue.Enqueue("Test");
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

        var queue = new ProducerConsumerQueue3<string>
        {
            ConsumerTaskDelegate = ConsumerTaskDelegate
        };
        queue.StartConsumer();

        queue.Enqueue("Test");

        // Act 
        queue.StopConsumer();

        // Assert
        Wait.Until(() => _wasFired, 3000);
        //Assert.That(_wasFired, Is.EqualTo(true));
        //queue.Dispose();
    }
}