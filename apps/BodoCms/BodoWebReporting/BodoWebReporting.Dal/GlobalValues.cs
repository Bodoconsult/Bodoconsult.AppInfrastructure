using System.Reflection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Abstractions.Typography;
using BodoWebReporting.Dal.Helpers;
using BodoWebReporting.Dal.Model;

namespace BodoWebReporting.Dal;

public static class GlobalValues
{
    /// <summary>
    /// Current app settings
    /// </summary>
    public static AppSettings CurrentAppSettings { get; private set; }

    /// <summary>
    /// Get a elegant default typography
    /// </summary>
    /// <returns></returns>
    public static ITypography DefaultTypography()
    {
        var typography = new ElegantTypographyPageHeader("Calibri", "Calibri", "Calibri")
        {
            ChartStyle =
            {
                Width = 750,
                Height = 464,
                FontSize = 10,
                BackGradientStyle = GradientStyle.TopBottom,
                BackgroundColor = TypoColors.Transparent
            }
        };

        return typography;
    }

    /// <summary>
    /// Load app settings from app directory
    /// </summary>
    public static void LoadAppSettings()
    {

        var path = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;

        var fileName = Path.Combine(path, "appSettings.json");

        CurrentAppSettings = JsonHelper.LoadJsonFile<AppSettings>(fileName);
    }


}