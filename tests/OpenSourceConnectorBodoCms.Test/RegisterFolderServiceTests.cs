// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System;
using System.IO;
using NUnit.Framework;
using OpenSourceConnectorBodoCms.Business;
using OpenSourceConnectorBodoCms.Business.App;
using OpenSourceConnectorBodoCms.Business.Services;
using System.Linq;
using System.Reflection;

namespace OpenSourceConnectorBodoCms.Test;

[TestFixture]
internal class RegisterFolderServiceTests
{
    private RegisterFolderService _service;

    [SetUp]
    public void Setup()
    {
        _service = new RegisterFolderService(Globals.Instance);
    }

    [Test]
    public void GetPackageFromFileName_ValidFileName_ReturnsPackageName()
    {
        // Arrange
        const string packageName = "package.abc.def";
        var fi = new FileInfo(@$"C:\temp\{packageName}.1.0.10.nupkg");

        // Act
        var result = RegisterFolderService.GetPackageFromFileName(fi);

        // assert
        Assert.IsNotNull(result);
        Assert.That(result.Length, Is.Not.Zero);
        Assert.That(result, Is.EqualTo(packageName));
    }

    [Test]
    public void GetPackagesFromFolder_ValidFolderName_ReturnsPackageName()
    {
        // Arrange
        ArgumentNullException.ThrowIfNull(Globals.Instance.AppStartParameter.AppPath);
        var di = new DirectoryInfo(Globals.Instance.AppStartParameter.AppPath).Parent?.Parent?.Parent?.Parent?.Parent;

        ArgumentNullException.ThrowIfNull(di);
        var dir = Path.Combine(di.FullName, "packages");

        // Act
        var result = RegisterFolderService.GetPackagesFromFolder(dir);

        // assert
        Assert.IsNotNull(result);
        Assert.That(result.Count, Is.Not.Zero);
    }

    [Test]
    public void GetAllProjectFolders_ValidSetup_FoldersFound()
    {
        // Arrange

        // Act
        _service.GetAllProjectFolders();

        // assert
        Assert.IsNotNull(_service.OpenSourceFolders);
        Assert.IsTrue(_service.OpenSourceFolders.Any());
        Assert.IsTrue(_service.OpenSourceFolders.Count > 0);
    }

    [Test]
    public void GetRepositoryUrl_ValidConfig_RepositoryUrlFound()
    {
        // Arrange
        var config = "[core]\r\n" +
                     "repositoryformatversion = 0\r\n" +
                     "filemode = false\r\n" +
                     "bare = false\r\n" +
                     "logallrefupdates = true\r\n" +
                     "symlinks = false\r\n" +
                     "ignorecase = true\r\n" +
                     "    [remote \"origin\"]\r\n" +
                     "\turl = https://github.com/RobertLeisner/Bodoconsult.Core.Typography.git\r\n" +
                     "fetch = +refs / heads/*:refs/remotes/origin/*\r\n" +
                     "[branch \"master\"]\r\n" +
                     "remote = origin\r\n" +
                     "merge = refs/heads/master";

        // Act
        var result = _service.GetRepositoryUrl(config);

        // assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length>0);
    }
}