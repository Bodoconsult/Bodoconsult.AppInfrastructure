// Copyright (c) Bodoconsult EDV-Dienstleistungen. All rights reserved.

namespace Bodoconsult.Drawing.Interfaces;

/// <summary>
/// Interface for factories creating System.Drawing based bitmap services based on <see cref="IBitmapService"/>
/// </summary>
public interface IBitmapServiceFactory
{
    /// <summary>
    /// Create a System.Drawing based bitmap service instance based on <see cref="IBitmapService"/>
    /// </summary>
    /// <returns>System.Drawing based bitmap service instance based on <see cref="IBitmapService"/></returns>
    public IBitmapService CreateInstance();
}