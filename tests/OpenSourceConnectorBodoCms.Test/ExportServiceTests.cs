// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Linq;
using NUnit.Framework;
using OpenSourceConnectorBodoCms.Business.App;
using OpenSourceConnectorBodoCms.Business.Services;

namespace OpenSourceConnectorBodoCms.Test;

[TestFixture]
internal class ExportServiceTests
{
    private RegisterFolderService _registerService;
    private ExportService _service;

    private readonly FakeDatabaseService _dbService = new();

    [SetUp]
    public void Setup()
    {
        _registerService = new RegisterFolderService(Globals.Instance);
    }

    [Test]
    public void PrepareData_ValidSetup_PropsSetCorrectly()
    {
        // Arrange
        _registerService.GetAllProjectFolders();

        Assert.IsNotNull(_registerService.OpenSourceFolders);
        Assert.IsTrue(_registerService.OpenSourceFolders.Any());
        Assert.IsTrue(_registerService.OpenSourceFolders.Count > 0);

        _service = new ExportService(Globals.Instance, _dbService);

        // Act
        _service.PrepareData();

        // assert
        Assert.That(_service.Categories.Count, Is.Not.Zero);
        Assert.That(_service.Packages.Count, Is.Not.Zero);
        Assert.That(_service.CategoriePackages.Count, Is.Not.Zero);
    }

    [Test]
    public void CreateFolders_ValidSetup_FoldersCreated()
    {
        // Arrange
        _registerService.GetAllProjectFolders();
            
        Assert.IsNotNull(_registerService.OpenSourceFolders);
        Assert.IsTrue(_registerService.OpenSourceFolders.Any());
        Assert.IsTrue(_registerService.OpenSourceFolders.Count > 0);

        _service = new ExportService(Globals.Instance, _dbService);
        _service.PrepareData();

        // Act
        _service.CreateFolders();

        // assert

    }

    [Test]
    public void CreateHtml_ValidSetup_PackageHtmlCreated()
    {
        // Arrange
        _registerService.GetAllProjectFolders();

        Assert.IsNotNull(_registerService.OpenSourceFolders);
        Assert.IsTrue(_registerService.OpenSourceFolders.Any());
        Assert.IsTrue(_registerService.OpenSourceFolders.Count > 0);

        _service = new ExportService(Globals.Instance, _dbService);
        _service.PrepareData();
        _service.CreateFolders();

        // Act
        _service.CreateHtml();

        // assert

    }

    [Test]
    public void CreateStartPage_ValidSetup_StartPageCreated()
    {
        // Arrange
        _registerService.GetAllProjectFolders();

        Assert.IsNotNull(_registerService.OpenSourceFolders);
        Assert.IsTrue(_registerService.OpenSourceFolders.Any());
        Assert.IsTrue(_registerService.OpenSourceFolders.Count > 0);

        _service = new ExportService(Globals.Instance, _dbService);
        _service.PrepareData();
        _service.CreateFolders();
        _service.CreateHtml();

        // Act
        _service.CreateStartpage();

        // assert

    }
}