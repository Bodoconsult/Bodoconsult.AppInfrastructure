// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.Pdf.Stylesets;

namespace Bodoconsult.Pdf.Interfaces;

/// <summary>
/// Interface for creating instances of <see cref="IPdfBuilder"/> for creating a PDF file
/// </summary>
public interface IPdfBuilderFactory
{
    /// <summary>
    /// Create an instance of <see cref="IPdfBuilder"/> for creating a PDF file
    /// </summary>
    /// <param name="styleSet">Current styleset to use for the new PDF file</param>
    /// <returns>Instance of <see cref="IPdfBuilder"/> for creating a PDF file</returns>
    public IPdfBuilder CreateInstance(IStyleSet styleSet);
}