// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.App.Test.SyncExecution;

[TestFixture]
internal class SyncProcessDataTests
{
    private readonly Func<DummyClass> _func = () => new DummyClass();

    [Test]
    public void Ctor_ValidSetup_PropSetCorrectly()
    {
        // Arrange 
        var processId = Guid.NewGuid();

        // Act 
        var result = new SyncProcessData<Guid, DummyClass>(processId, 1000, _func);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CancellationTokenSource, Is.Not.Null);
            Assert.That(result.TaskCompletionSource, Is.Not.Null);
            Assert.That(result.ProcessId, Is.EqualTo(processId));
        }
    }

    [Test]
    public void CreateWaitingTask_Timeout1000_WaitingAsExpected()
    {
        // Arrange 
        const int timeout = 1000;
        var processId = Guid.NewGuid();

        var spd = new SyncProcessData<Guid, DummyClass>(processId, timeout,_func);

        var sw = new Stopwatch();
        sw.Start();

        // Act 
        spd.CreateWaitingTask().GetAwaiter().GetResult();

        sw.Stop();


        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(sw.ElapsedMilliseconds, Is.GreaterThan(timeout));
        }
    }

    [Test]
    public void CreateWaitingTask_Timeout2000_WaitingAsExpected()
    {
        // Arrange 
        const int timeout = 2000;
        var processId = Guid.NewGuid();

        var spd = new SyncProcessData<Guid, DummyClass>(processId, timeout, _func);

        var sw = new Stopwatch();
        sw.Start();

        // Act 
        spd.CreateWaitingTask().GetAwaiter().GetResult();

        sw.Stop();


        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(sw.ElapsedMilliseconds, Is.GreaterThan(timeout));
        }
    }

    [Test]
    public void CreateWaitingTask_Timeout5000_WaitingAsExpected()
    {
        // Arrange 
        const int timeout = 5000;
        var processId = Guid.NewGuid();

        var spd = new SyncProcessData<Guid, DummyClass>(processId, timeout, _func);

        var sw = new Stopwatch();
        sw.Start();

        // Act 
        spd.CreateWaitingTask().GetAwaiter().GetResult();

        sw.Stop();


        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(sw.ElapsedMilliseconds, Is.GreaterThan(timeout));
        }
    }
}