// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.Abstractions.Helpers;

namespace Bodoconsult.App.Test.HelperTests;

// EMU's (914400 EMUs is 1 inch, 12700 EMUs is 1pt).

[TestFixture]
internal class MeasurementHelperTests
{
    [Test]
    public void GetDxaFromInch_8Inch_ReturnsDxa()
    {
        // Arrange 
        const double input = 8.2677;    // 21cm

        // Act  
        var result = MeasurementHelper.GetDxaFromInch(input);

        // Assert
        Assert.That(result, Is.EqualTo(11905));
    }

    [Test]
    public void GetDxaFromCm_21cm_ReturnsDxa()
    {
        // Arrange 
        const int input = 21;

        // Act  
        var result = MeasurementHelper.GetDxaFromCm(input);

        // Assert
        Assert.That(result, Is.EqualTo(11905));
    }

    [Test]
    public void GetEmuFromCm_1cm_ReturnsEmu()
    {
        // Arrange 
        const int input = 1;

        // Act  
        var result = MeasurementHelper.GetEmuFromCm(input);

        // Assert
        Assert.That(result, Is.EqualTo(360000));
    }

    [Test]
    public void GetEmuFromPixels_600px_ReturnsEmu()
    {
        // Arrange 
        const int input = 600;

        // Act  
        var result = MeasurementHelper.GetEmuFromPx(input);

        // Assert
        Assert.That(result, Is.EqualTo(5715000));
    }

    [Test]
    public void GetPtFromCm_1cm_ReturnsPt()
    {
        // Arrange 
        const int input = 1;

        // Act  
        var result = MeasurementHelper.GetPtFromCm(input);

        // Assert
        Assert.That(result, Is.EqualTo(28.3));
    }

    [Test]
    public void GetCmFromPt_1pt_ReturnsCm()
    {
        // Arrange 
        const int input = 1;

        // Act  
        var result = MeasurementHelper.GetCmFromPt(input);

        // Assert
        Assert.That(result, Is.EqualTo(0.035));
    }

    [Test]
    public void GetDxaFromCm_1ptAsCm_ReturnsCm()
    {
        // Arrange 
        var input = MeasurementHelper.GetCmFromPt(1);

        // Act  
        var result = MeasurementHelper.GetDxaFromCm(input);

        // Assert
        Assert.That(result, Is.EqualTo(19));
    }
    
    [Test]
    public void GetPtFromMm_1mm_ReturnsPt()
    {
        // Arrange 
        const int input = 1;

        // Act  
        var result = MeasurementHelper.GetPtFromMm(input);

        // Assert
        Assert.That(result, Is.EqualTo(2.83));
    }

    [Test]
    public void GetTwipsFromCm_1cm_ReturnsTwips()
    {
        // Arrange 
        const int input = 1;

        // Act  
        var result = MeasurementHelper.GetTwipsFromCm(input);

        // Assert
        Assert.That(result, Is.EqualTo(569));
    }

}