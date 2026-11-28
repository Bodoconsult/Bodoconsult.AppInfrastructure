// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Wpf.Charting.Models;
using Bodoconsult.App.Wpf.Charting.Services;

namespace Bodoconsult.App.Wpf.Charting.Test;

public class DataHelper
{
    public static void SetModel(ChartStyle model)
    {
        model.GridlinePattern = DrawSurfaceChart.GridlinePatternEnum.Solid;
        model.Elevation =  30;
        model.Azimuth = -37;
        model.Title = "Ertrag in Abhängigkeit von Nutzungsentgelt und Betriebskosten";
        model.IsColorBar = true;
        model.XLabel = "Nutzungsentgelte [%]";
        model.YLabel = "Betriebskosten [%]";
        model.ZLabel = "Ertrag [%]";
        model.NumberFormatX = "#,##0%";
    }
}