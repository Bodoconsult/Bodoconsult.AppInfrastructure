// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.App.Abstractions.DependencyInjection;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.Charting.Factories;
using Bodoconsult.Drawing.SkiaSharp.Factories;
using Bodoconsult.Drawing.SkiaSharp.Interfaces;
using Bodoconsult.Drawing.SkiaSharp.Services;

namespace Bodoconsult.Charting.DependencyInjection
{
    /// <summary>
    /// <see cref="IDiContainerServiceProvider"/> implementation for using <see cref="ChartHandler"/> instances to create charts from data input like data tables
    /// </summary>
    public class ChartingDiContainerServiceProvider : IDiContainerServiceProvider
    {
        /// <summary>
        /// Add DI container services to a DI container
        /// </summary>
        /// <param name="diContainer">Current DI container</param>
        public void AddServices(DiContainer diContainer)
        {
            diContainer.AddSingleton<IBitmapServiceFactory, BitmapServiceFactory>();
            diContainer.AddSingleton<IChartHandlerFactory, ChartHandlerFactory>();
        }

        /// <summary>
        /// Late bind DI container references to avoid circular DI references
        /// </summary>
        /// <param name="diContainer">Current DI container</param>
        public void LateBindObjects(DiContainer diContainer)
        {
            // Do nothing
        }
    }
}
