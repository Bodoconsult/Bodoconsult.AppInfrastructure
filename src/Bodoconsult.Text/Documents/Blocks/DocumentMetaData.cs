// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Text;
using Bodoconsult.App.Abstractions.Interfaces;

namespace Bodoconsult.Text.Documents;

/// <summary>
/// metadata for a document
/// </summary>
public class DocumentMetaData : Block, ITypoMetaData
{
    /// <summary>
    /// Default ctor
    /// </summary>
    public DocumentMetaData()
    {
        IsSingleton = true;
    }

    /// <summary>
    /// Copyright to print in charts and other items
    /// </summary>
    public string Copyright { get; set; }

    /// <summary>
    /// Name(s) of the author(s)
    /// </summary>
    public string Authors { get; set; }

    /// <summary>
    /// Company name
    /// </summary>
    public string Company { get; set; }

    /// <summary>
    /// Company website
    /// </summary>
    public string CompanyWebsite { get; set; }

    /// <summary>
    /// Path to logo to print in the page header
    /// </summary>
    public string LogoPath { get; set; }

    /// <summary>
    /// Width of the logo to print in the page header in cm
    /// </summary>
    public double LogoWidth { get; set; } = 2;

    /// <summary>
    /// Title of the document
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Document description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Keywords separated by comma
    /// </summary>
    public string Keywords { get; set; }

    /// <summary>
    /// Table of content (TOC) heading
    /// </summary>
    public string TocHeading { get; set; } = "Table of content";

    /// <summary>
    /// Table of figures (TOF) heading
    /// </summary>
    public string TofHeading { get; set; } = "Table of figures";

    /// <summary>
    /// Table of equations (TOE) heading
    /// </summary>
    public string ToeHeading { get; set; } = "Table of equations";

    /// <summary>
    /// Table of tables (TOT) heading
    /// </summary>
    public string TotHeading { get; set; } = "Table of tables";

    /// <summary>
    /// The word written before the page number in a page footer. Default: Page
    /// </summary>
    public string PageNumberPrefix { get; set; } = "Page";

    /// <summary>
    /// Prefix for equations
    /// </summary>
    public string EquationPrefix { get; set; } = "Equation";

    /// <summary>
    /// Prefix for citation sources
    /// </summary>
    public string CitationSourcePrefix { get; set; } = "Source: ";


    /// <summary>
    /// Prefix for tables
    /// </summary>
    public string TablePrefix { get; set; } = "Table";

    /// <summary>
    /// Prefix for figures
    /// </summary>
    public string FigurePrefix { get; set; } = "Figure";

    /// <summary>
    /// Should a TOC section added at the start of the document
    /// </summary>
    public bool IsTocRequired { get; set; }

    /// <summary>
    /// Should add table of figures be added at the start of the document
    /// </summary>
    public bool IsFiguresTableRequired { get; set; }

    /// <summary>
    /// Should add table of equations be added at the start of the document
    /// </summary>
    public bool IsEquationsTableRequired { get; set; }

    /// <summary>
    /// Should add table of tables be added at the start of the document
    /// </summary>
    public bool IsTablesTableRequired { get; set; }

    /// <summary>
    /// Footer text. Use &lt;&lt;page&gt;&gt; as placeholder for the page number
    /// </summary>
    public string FooterText { get; set; }

    /// <summary>
    /// Header text
    /// </summary>
    public string HeaderText { get; set; }

    /// <summary>
    /// Add the current element to a document defined in LDML (Logical document markup language)
    /// </summary>
    /// <param name="stringBuilder">StringBuilder instance to create the LDML in</param>
    /// <param name="indent">Current indent</param>
    public override void ToLdmlString(StringBuilder stringBuilder, string indent)
    {
        stringBuilder.AppendLine($"{indent}<DocumentMetaData{GetPropertiesAsAttributes()}/>");
    }

}