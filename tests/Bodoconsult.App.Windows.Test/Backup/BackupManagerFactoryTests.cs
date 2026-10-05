// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.ShellTools;
using Bodoconsult.App.Backup;
using Bodoconsult.App.Windows.Backup;
using NUnit.Framework;

namespace Bodoconsult.App.Windows.Test.Backup;

[TestFixture]
internal class BackupManagerFactoryTests
{
    [Test]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {
        // Arrange 
        var spm = new FakeShellProcessManager();

        var targetFactory = new RobocopyBackupTargetFactory(spm);

        BackupManagerFactory factory = null;

        // Act  
        Assert.DoesNotThrow(() =>
        {
            factory = new BackupManagerFactory(targetFactory);
        });

        // Assert
        Assert.That(factory, Is.Not.Null);
    }

    [Test]
    public void CreateInstance_ValidSetup_InstanceCreated()
    {
        // Arrange 
        var spm = new FakeShellProcessManager();

        var targetFactory = new RobocopyBackupTargetFactory(spm);

        BackupManagerFactory factory = new BackupManagerFactory(targetFactory);

        var config = new BasicBackupConfig();

        // Act  

        var result = factory.CreateInstance(config);

        // Assert
        Assert.That(result, Is.Not.Null);
    }
}