// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Globalization;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.CentralServices;

namespace Bodoconsult.App.Test.CentralServices;

[TestFixture]
internal class DateTimeServiceTests
{
    private readonly IAppDateService _dateTimeService;

    public DateTimeServiceTests()
    {
        _dateTimeService = new AppDateService();
    }

    [Test]
    public void TestNowIsNow()
    {
        Assert.That(_dateTimeService.Now.ToString(CultureInfo.InvariantCulture) == DateTime.Now.ToString(CultureInfo.InvariantCulture));
    }

    [Test]
    public void TestTodayIsToday()
    {
        Assert.That(_dateTimeService.Today.ToString(CultureInfo.InvariantCulture) == DateTime.Today.ToString(CultureInfo.InvariantCulture));
    }


    [Test]
    public void TestGetValidAccessDateNullValue()
    {
        // Arrange 
        DateTime? value = null;

        // Act  
        var result = _dateTimeService.GetValidAccessDate(value);

        // Assert
        Assert.That(result, Is.GreaterThan(DateTime.Now.AddSeconds(-10)));

    }

    [Test]
    public void TestGetValidAccessDateInvalidDate()
    {
        // Arrange 
        DateTime? value = new DateTime(1800, 1, 1);

        // Act  
        var result = _dateTimeService.GetValidAccessDate(value);

        // Assert
        Assert.That(result, Is.EqualTo(new DateTime(1900, 1, 1)));

    }

}