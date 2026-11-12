Release notes for Bodoconsult.Pdf
==========================

With each release all the repository internal references are updated.

# 1.0.8

-	Updated PdfSharp and Migradoc packages

-	Refactoring for PdfCreator and PdfBuilder classes to make it work flexible espacially for tables

# 1.0.9

-   Added background image handling and watermarking to PdfBuilder and PdfCreator classes

# 1.0.10

NOT intended for public use

# 1.0.11

-   Better PDB handling

-   Had to change to AGPL-3.0-only license due to PdfSharp

-   Migration to Net10

# 1.0.12

-   Replaced IPdfBuilder.TitleTableOfContent with ITypoMetaData.TocHeading

-   Replaced IPdfBuilder.TitleTableOfTables with ITypoMetaData.TotHeading

-   Replaced IPdfBuilder.TitleTableOfEquations with ITypoMetaData.ToeHeading

-   Replaced IPdfBuilder.TitleTableOfFigures with ITypoMetaData.TofHeading

-   Replaced IPdfBuilder.PageNumberPrefix with ITypoMetaData.PageNumberPrefix

-   Renamed TypoColorExtensions class to PdfColorExtensions

-   Added PdfColorExtensions.FromHtml method

-   PdfBuilder and PdfCreator class allow saving the DDL markup for the PDF next to the PDF with extension .mdddl. This may be helpful finding bugs in PDF creation process. Use CreateDdlFile property set to true to activate DDL creation.

-   Bugfixing table handling for multipage tables