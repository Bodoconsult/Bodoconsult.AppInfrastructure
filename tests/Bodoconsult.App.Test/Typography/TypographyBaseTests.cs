// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Reflection;
using System.Text;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Abstractions.Typography;
using Newtonsoft.Json;

namespace Bodoconsult.App.Test.Typography;


[TestFixture]
public class TypographyBaseTests
{

    /// <summary>
    /// Value used for float is equal to 0 comparisons
    /// </summary>
    private const double CompareToZeroValue = 0.000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001;

    private const double Tolerance = 0.0001;

    private readonly string _exportFolderName;

    public TypographyBaseTests()
    {
        var directoryInfo = new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent;
        var dir = directoryInfo?.Parent?.Parent?.Parent;

        if (dir != null)
        {
            _exportFolderName = Path.Combine(dir.FullName, "SampleData");
        }
    }


    [Test]
    public void SetMargins_BaseSettings_Successful()
    {
        // Arrange
        var t = new TypographyBase();

        // Act
        t.SetMargins();

        // Assert
        Assert.That(t.TypeAreaRect.Size.Width - 175 < Tolerance);
        Assert.That(Math.Abs(t.MarginUnit - 0.875) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Left - 1.75) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Right - 1.75) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Top - 0.875) < Tolerance);
        Assert.That(Math.Abs(t.TypoMargins.Bottom - 1.75) < Tolerance);


        //var erg = t.GetPixelHeight(4);

        Assert.That(Math.Abs(t.GetWidth(4) - 11.5) < Tolerance);
        Assert.That(Math.Abs(t.GetHeight(4) - 7.11) < Tolerance);

        Assert.That(Math.Abs(t.GetPixelWidth(4) - (int)(11.5 * TypographicConstants.InchPerCentimeter * 96)) < Tolerance);
        Assert.That(Math.Abs(t.GetPixelHeight(4) - (int)(7.11 * TypographicConstants.InchPerCentimeter * 96)) < Tolerance);
    }

    [Test]
    public void ExportImport_Elegant_Successful()
    {
        var t = new ElegantTypographyPageHeader("Cambria", "Calibri", "Calibri")
        {
            MarginLeftFactor = 1,
            MarginRightFactor = 1,
            MarginTopFactor = 1,
            MarginBottomFactor = 1
        };


        t.SetMargins();

        var fileName = Path.Combine(_exportFolderName, "ElegantTypography.json");

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        ExportToJson(t, fileName);

        var typo2 = ImportFromJson<ElegantTypographyPageHeader>(fileName);
        typo2.SetMargins();


        Assert.That(File.Exists(fileName));
        Assert.That(typo2, Is.Not.Null);
        Assert.That(t.MarginUnit - typo2.MarginUnit < Tolerance);

    }

    [Test]
    public void ExportImport_ElegantBodoprivate_Successful()
    {
        var t = new ElegantTypographyPageHeader("Cambria", "Cambria", "Cambria")
        {
            LogoPath = @"C:\bodoconsult\Logos\logoStatera.jpg"
        };


        t.SetMargins();

        var fileName = Path.Combine(_exportFolderName, "ElegantTypographyBodoPrivate.json");

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        ExportToJson(t, fileName);

        var typo2 = ImportFromJson<ElegantTypographyPageHeader>(fileName);
        typo2.SetMargins();


        Assert.That(File.Exists(fileName));
        Assert.That(typo2, Is.Not.Null);
        Assert.That(t.MarginUnit - typo2.MarginUnit < Tolerance);

    }


    [Test]
    public void ExportImport_Compact_Successful()
    {
        var t = new CompactTypographyPageHeader("Cambria", "Calibri", "Calibri")
        {
            MarginLeftFactor = 1,
            MarginRightFactor = 1,
            MarginTopFactor = 1,
            MarginBottomFactor = 1
        };


        t.SetMargins();

        var fileName = Path.Combine(_exportFolderName, "CompactTypography.json");

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        ExportToJson(t, fileName);

        var typo2 = ImportFromJson<CompactTypographyPageHeader>(fileName);
        typo2.SetMargins();


        Assert.That(File.Exists(fileName));
        Assert.That(typo2, Is.Not.Null);
        Assert.That(t.MarginUnit - typo2.MarginUnit < Tolerance);

    }

    private static void ExportToJson(ITypography typography, string fileName)
    {

        var json = JsonConvert.SerializeObject(
            typography,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

        //json = Encrypt(json);

        var sw11 = new StreamWriter(fileName, false, Encoding.UTF8);
        sw11.WriteLine(json);
        sw11.Close();
    }


    private static T ImportFromJson<T>(string fileName) where T : ITypography
    {

        var json = File.ReadAllText(fileName, Encoding.UTF8);

        //json = Decrypt(json);

        var erg = (T)JsonConvert.DeserializeObject(json, typeof(T),
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

        return erg;
    }

    [Test]
    public void CalculateVerticalLines_CompactTypographyPageHeader_LinesCalculated()
    {
        // Arrange
        var typo = new CompactTypographyPageHeader("Cambria", "Calibri", "Calibri")
        {
            MarginLeftFactor = 1,
            MarginRightFactor = 1,
            MarginTopFactor = 1,
            MarginBottomFactor = 1
        };

        typo.SetMargins();

        //Act (SetMargins called in the init event)
        typo.CalculateVerticalLines();

        // Assert
        const double expectedValue = 1.75;

        Assert.That(Math.Abs(typo.TypoMargins.Left - expectedValue) < CompareToZeroValue);
        Assert.That(Math.Abs(typo.TypoMargins.Right - expectedValue) < CompareToZeroValue);
        Assert.That(Math.Abs(typo.TypoMargins.Top - expectedValue) < CompareToZeroValue);
        Assert.That(Math.Abs(typo.TypoMargins.Bottom -expectedValue) < CompareToZeroValue);
        Assert.That(Math.Abs(typo.GridColumnWidth - 2.5) < CompareToZeroValue);

        Assert.That(Math.Abs(typo.VerticalLines[0] - expectedValue) < CompareToZeroValue);
        Assert.That(Math.Abs(typo.VerticalLines[1] - expectedValue - typo.GridColumnWidth) < CompareToZeroValue);
        //Assert.That(Math.Abs(typo.VerticalLines[2] - 5.75) < CompareToZeroValue);
        //Assert.That(Math.Abs(typo.VerticalLines[3] - 7.75) < CompareToZeroValue);
        //Assert.That(Math.Abs(typo.VerticalLines[4] - 8.25) < CompareToZeroValue);
        //Assert.That(Math.Abs(typo.VerticalLines[5] - 10.25) < CompareToZeroValue);
        //Assert.That(Math.Abs(typo.VerticalLines[6] - 10.75) < CompareToZeroValue);
        //Assert.That(Math.Abs(typo.VerticalLines[7] - 12.75) < CompareToZeroValue);
        //Assert.That(Math.Abs(typo.VerticalLines[8] - 13.25) < CompareToZeroValue);
        //Assert.That(Math.Abs(typo.VerticalLines[9] - 15.25) < CompareToZeroValue);
        //Assert.That(Math.Abs(typo.VerticalLines[10] - 15.75) < CompareToZeroValue);
        //Assert.That(Math.Abs(typo.VerticalLines[11] - 17.75) < CompareToZeroValue);
    }

    private static void DefaultAsserts(ITypography typo)
    {

    }

}