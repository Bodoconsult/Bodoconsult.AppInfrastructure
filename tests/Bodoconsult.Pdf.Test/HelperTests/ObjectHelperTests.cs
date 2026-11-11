// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.Pdf.Helpers;
using MigraDoc.DocumentObjectModel;
using NUnit.Framework;

namespace Bodoconsult.Pdf.Test.HelperTests;

[TestFixture]
public class ObjectHelperTests
{
    [Test]
    public void MapProperties_ValidObjects_PropertiesMapped()
    {
        var style1 = new Style("Test1", "Normal") {Font = {Size = 25}};

        var style2 = new Style("Test2", "Normal");

        Assert.That(style2.Font.Size, Is.Not.EqualTo(style1.Font.Size));

        ObjectHelper.MapProperties(style1, style2);
        ObjectHelper.MapProperties(style1.Font, style2.Font);

        Assert.That(style2.Font.Size, Is.EqualTo(style1.Font.Size));
    }
}