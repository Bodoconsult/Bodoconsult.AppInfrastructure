// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

namespace Bodoconsult.App.Abstractions.Interfaces;

/// <summary>
/// Default page style
/// </summary>
public class DefaultPageStyle : BaseTypoPageStyle
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public DefaultPageStyle()
    {
        TypoPaperFormat.Landscape = false;
        SetMargins();
    }
}