// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Abstractions.ShellTools;
using Bodoconsult.App.Backup;
using Bodoconsult.App.Windows.Backup;
using NUnit.Framework;

namespace Bodoconsult.App.Windows.Test.Backup;

[TestFixture]
internal class RobocopyBackupTargetFactoryTests
{
    [Test]
    public void Ctor_ValidSetup_PropsSetCorrectly()
    {        
        // Arrange 
        var spm = new FakeShellProcessManager();

        RobocopyBackupTargetFactory factory = null;

        // Act  
        Assert.DoesNotThrow(() =>
        { 
            factory = new RobocopyBackupTargetFactory(spm);
        });

        // Assert
        Assert.That(factory, Is.Not.Null);
    }

    [Test]
    public void CreateInstance_ValidSetup_InstanceCreated()
    {
        // Arrange 
        var spm = new FakeShellProcessManager();

        IBackupTargetSettings settings = new BackupTargetSettings();

        var factory = new RobocopyBackupTargetFactory(spm);

        // Act  

        var result = factory.CreateInstance(settings);

        // Assert
        Assert.That(result, Is.Not.Null);
    }
}