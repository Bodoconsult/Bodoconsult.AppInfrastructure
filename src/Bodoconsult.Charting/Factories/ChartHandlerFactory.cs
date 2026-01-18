// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.Charting.Base.Interfaces;
using Bodoconsult.Drawing.SkiaSharp.Interfaces;

namespace Bodoconsult.Charting.Factories;

/// <summary>
/// Public factory for <see cref="ChartHandler"/> instances
/// </summary>
public class ChartHandlerFactory: IChartHandlerFactory
{
    private readonly IBitmapServiceFactory _bitmapServiceFactory;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="bitmapServiceFactory">Current <see cref="IBitmapServiceFactory"/> instance</param>
    public ChartHandlerFactory(IBitmapServiceFactory bitmapServiceFactory)
    {
        _bitmapServiceFactory = bitmapServiceFactory;
    }

    /// <summary>
    /// Create an <see cref="ChartHandler"/> instance
    /// </summary>
    /// <param name="chartData">Current <see cref="IChartData"/> instance containing all data to use in the chart</param>
    /// <returns><see cref="IChartHandler"/> instance for creating the chart</returns>
    public IChartHandler CreateInstance(IChartData chartData)
    {
        return new ChartHandler(chartData, _bitmapServiceFactory);
    }
}