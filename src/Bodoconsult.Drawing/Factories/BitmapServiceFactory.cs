// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

using Bodoconsult.Drawing.Interfaces;
using System.Runtime.Versioning;

namespace Bodoconsult.Drawing.Factories;

/// <summary>
/// Factory for <see cref="BitmapService"/> instances
/// </summary>
[SupportedOSPlatform("windows")]
public class BitmapServiceFactory : IBitmapServiceFactory
{
    /// <summary>
    /// Create an instance of <see cref="BitmapService"/>
    /// </summary>
    /// <returns>An instance of <see cref="BitmapService"/></returns>
    public IBitmapService CreateInstance()
    {
        return new BitmapService();
    }
}