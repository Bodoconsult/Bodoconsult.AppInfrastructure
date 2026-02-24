// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.


using Bodoconsult.App.CentralServices;
using System.Globalization;

namespace Bodoconsult.App.Test.CentralServices;

[TestFixture]
internal class FakeAppDateTimeServiceTests
{
    private readonly FakeAppDateService _dateTimeService;

    public FakeAppDateTimeServiceTests()
    {
        _dateTimeService = new FakeAppDateService();
    }

    [Test]
    public void TestNowIsNow()
    {
        _dateTimeService.ResetOffset();
        Assert.That(_dateTimeService.Now.ToString(CultureInfo.InvariantCulture) == DateTime.Now.ToString(CultureInfo.InvariantCulture));
    }
    [Test]
    public void TestTodayIsToday()
    {
        _dateTimeService.ResetOffset();
        Assert.That(_dateTimeService.Today.ToString(CultureInfo.InvariantCulture) == DateTime.Today.ToString(CultureInfo.InvariantCulture));
    }

    [Test]
    public void TestGetSetOffset()
    {
        var offset = new TimeSpan(1, 0, 0);
        _dateTimeService.Offset = offset;
        Assert.That(_dateTimeService.Offset == offset);
    }

    [Test]
    public void TestOffsetNow()
    {
        var offset = new TimeSpan(1, 0, 0);
        _dateTimeService.Offset = offset;
        Assert.That(_dateTimeService.Now.ToString(CultureInfo.InvariantCulture), Is.EqualTo((DateTime.Now + offset).ToString(CultureInfo.InvariantCulture)));
    }

    [Test]
    public void TestOffsetToday()
    {
        var offset = new TimeSpan(1, 0, 0);
        _dateTimeService.Offset = offset;
        Assert.That(_dateTimeService.Today.ToString(CultureInfo.InvariantCulture), Is.EqualTo((DateTime.Today + offset).ToString(CultureInfo.InvariantCulture)));
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