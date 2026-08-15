// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.SyncExecution;
using Bodoconsult.App.Benchmarking;
using Bodoconsult.App.Test.Helpers;

namespace Bodoconsult.App.Test.SyncExecution;

[TestFixture]
public class IntSyncProcessManagerTests
{
    private readonly AppBenchProxy _benchLogger = TestHelper.GetFakeAppBenchProxy();
    private readonly Func<DummyClass> _func = () => new DummyClass();

    [OneTimeTearDown]
    public void Cleanup()
    {
        _benchLogger.Dispose();
    }

    [Test]
    public void AddSyncProcess_ValidOrder_ProcessIsAddedToSyncQueue()
    {
        // Arrange 
        var op = new SyncProcessManager<int, DummyClass>(_func);

        const int processId = 99;

        // Act 
        var result = op.AddSyncProcess(processId, 1000);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CancellationTokenSource, Is.Not.Null);
            //Assert.That(result.TaskCompletionSource, Is.Null);
            Assert.That(result.ProcessId, Is.EqualTo(processId));
            Assert.That(op.IsSyncRunningOrderEmpty, Is.False);
        }
    }

    [Test]
    public void GetSyncProcessDataForProcess_ValidOrder_ReturnsData()
    {
        // Arrange 
        var op = new 
            SyncProcessManager<int, DummyClass>(_func);

        const int processId = 98;

        var dummyData = op.AddSyncProcess(processId, 1000);

        // Act 
        var result = op.GetSyncProcessDataForProcess(processId);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CancellationTokenSource, Is.Not.Null);
            //Assert.That(result.TaskCompletionSource, Is.Null);
            Assert.That(result.ProcessId, Is.EqualTo(processId));
            //Assert.That(op.IsSyncRunningOrderEmpty, Is.False);

            Assert.That(result, Is.EqualTo(dummyData));
        }
    }

    [Test]
    public void RemoveSyncProces_ValidOrder_ProcessIsRemovedFromSyncQueue()
    {
        // Arrange 
        var op = new SyncProcessManager<int, DummyClass>(_func);

        const int processId = 97;

        var result = op.AddSyncProcess(processId, 1000);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CancellationTokenSource, Is.Not.Null);
            //Assert.That(result.TaskCompletionSource, Is.Null);
            Assert.That(result.ProcessId, Is.EqualTo(processId));
            Assert.That(op.IsSyncRunningOrderEmpty, Is.False);
        }

        // Act 
        op.RemoveSyncProcess(processId);

        // Assert
        Assert.That(op.IsSyncRunningOrderEmpty, Is.True);
    }

    //[Test]
    //public void GetSyncProcessDataForProcess_ValidOrder_ReturnsData()
    //{
    //    // Arrange 
    //    var op = new SyncProcessManager<int, DummyClass>(_func);

    //    var processId = 98;

    //    var dummyData = op.AddSyncProcess(processId, 1000);

    //    // Act 
    //    var result = op.GetSyncProcessDataForProcess(processId);

    //    // Assert
    //    using (Assert.EnterMultipleScope())
    //    {
    //        Assert.That(result, Is.Not.Null);
    //        Assert.That(result.CancellationTokenSource, Is.Not.Null);
    //        Assert.That(result.TaskCompletionSource, Is.Null);
    //        Assert.That(result.ProcessId, Is.EqualTo(processId));
    //        //Assert.That(op.IsSyncRunningOrderEmpty, Is.False);

    //        Assert.That(result, Is.EqualTo(dummyData));
    //    }
    //}
}