// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Collections.Generic;
using System.Linq;

namespace Bodoconsult.Text.Documents;

/// <summary>
/// Calculate TOC, TOF and TOE content for a LDML document
/// </summary>
public class LdmlCalculator
{
    private readonly DocumentMetaData _documentMetaData;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="document">Current document to calculate</param>
    public LdmlCalculator(Document document)
    {
        Document = document;

        _documentMetaData = (DocumentMetaData)Document.ChildBlocks.FirstOrDefault(x => x.GetType() == typeof(DocumentMetaData));

        if (_documentMetaData != null)
        {
            _documentMetaData = new DocumentMetaData();
        }
    }

    /// <summary>
    /// Current document to calculate
    /// </summary>
    public Document Document { get;  }

    /// <summary>
    /// All headings in the document to count
    /// </summary>
    public List<HeadingBase> Headings { get; } = [];

    /// <summary>
    /// All TocItems in the document to count
    /// </summary>
    public List<TocBase> TocItems { get; } = [];

    /// <summary>
    /// Heading counters
    /// </summary>
    public int[] HeadingCounters { get; } = new int[5];


    /// <summary>
    /// All figures in the document to count
    /// </summary>
    public List<Figure> Figures { get; } = [];

    /// <summary>
    /// All items of the TOF
    /// </summary>
    public List<Tof> TofItems { get; } = [];

    /// <summary>
    /// All items of the TOT
    /// </summary>
    public List<Tot> TotItems { get; } = [];

    /// <summary>
    /// Current figure counter
    /// </summary>
    public int FigureCounter { get; private set; }


    /// <summary>
    /// Current table counter
    /// </summary>
    public int TableCounter { get; private set; }

    /// <summary>
    /// All tables in the document to count
    /// </summary>
    public List<Table> Tables { get; } = [];

    /// <summary>
    /// All equations in the document to count
    /// </summary>
    public List<Equation> Equations { get; } = [];

    /// <summary>
    /// All items of the TOE
    /// </summary>
    public List<Toe> ToeItems { get; } = [];

    /// <summary>
    /// Current equation counter
    /// </summary>
    public int EquationCounter { get; private set; }

    /// <summary>
    /// Update column references for tables in all sections of the document
    /// </summary>
    public void UpdateAllTables()
    {
        var sections = Document.ChildBlocks.Where(x => x.GetType() == typeof(Section));

        foreach (var section in sections)
        {
            UpdateTableSection((Section)section);
        }
    }

    /// <summary>
    /// Update column references for tables in a section
    /// </summary>
    /// <param name="section">Current section</param>
    public static void  UpdateTableSection(Section section)
    {
        foreach (var table in section.ChildBlocks.Where(x => x.GetType() == typeof(Table)))
        {
            UpdateTable((Table)table);
        }
    }

    /// <summary>
    /// Update column references for a table
    /// </summary>
    /// <param name="table">Current table</param>
    public static void UpdateTable(Table table)
    {
        var cols = table.Columns;
        foreach (var row in table.Rows)
        {
            for (var index = 0; index < row.Cells.Count; index++)
            {
                var cell = row.Cells[index];
                cell.Column = cols[index];
            }
        }
    }


    /// <summary>
    /// Enumerate all items needed for TOC, TO´F and TOE
    /// </summary>
    public void EnumerateAllItems()
    {
        EnumerateAllItemsForToc();
        EnumerateAllItemsForToe();
        EnumerateAllItemsForTof();
        EnumerateAllItemsForTot();
    }

    /// <summary>
    /// Enumerate all items needed for TOC
    /// </summary>
    public void EnumerateAllItemsForToc()
    {
        foreach (var item in Document.ChildBlocks)
        {
            // Only Section instances are interesting
            if (item is not Section section)
            {
                continue;
            }

            // Current section is exluded from numbering
            if (section.DoNotIncludeInNumbering)
            {
                continue;
            }

            // Now find the equations
            FindHeadings(section);
        }

    }

    private void FindHeadings(Section section)
    {
        foreach (var block in section.ChildBlocks)
        {
            if (block is not HeadingBase equation)
            {
                continue;
            }

            if (equation is Heading1)
            {
                HeadingCounters[0]++;
                HeadingCounters[1]=0;
                HeadingCounters[2] = 0;
                HeadingCounters[3] = 0;
                HeadingCounters[4] = 0;
                equation.CurrentPrefix = $"{HeadingCounters[0]}. ";
            }

            if (equation is Heading2)
            {
                HeadingCounters[1]++;
                HeadingCounters[2] = 0;
                HeadingCounters[3] = 0;
                HeadingCounters[4] = 0;
                equation.CurrentPrefix = $"{HeadingCounters[0]}.{HeadingCounters[1]}. ";
            }

            if (equation is Heading3)
            {
                HeadingCounters[2]++;
                HeadingCounters[3] = 0;
                HeadingCounters[4] = 0;
                equation.CurrentPrefix = $"{HeadingCounters[0]}.{HeadingCounters[1]}.{HeadingCounters[2]}. ";
            }

            if (equation is Heading4)
            {
                HeadingCounters[3]++;
                HeadingCounters[4] = 0;
                equation.CurrentPrefix = $"{HeadingCounters[0]}.{HeadingCounters[1]}.{HeadingCounters[2]}.{HeadingCounters[3]}. ";
            }

            if (equation is Heading5)
            {
                HeadingCounters[4]++;
                equation.CurrentPrefix = $"{HeadingCounters[0]}.{HeadingCounters[1]}.{HeadingCounters[2]}.{HeadingCounters[3]}.{HeadingCounters[4]}. ";
            }

            equation.TagName = $"Heading{HeadingCounters[0]}_{HeadingCounters[1]}_{HeadingCounters[2]}_{HeadingCounters[3]}_{HeadingCounters[4]}";
                
            Headings.Add(equation);
        }
    }

    /// <summary>
    /// Enumerate all items needed for TOE
    /// </summary>
    public void EnumerateAllItemsForToe()
    {
        foreach (var item in Document.ChildBlocks)
        {
            // Only Section instances are interesting
            if (item is not Section section)
            {
                continue;
            }

            // Current section is exluded from numbering
            if (section.DoNotIncludeInNumbering)
            {
                continue;
            }

            // Now find the equations
            FindEquations(section);
        }
    }

    private void FindEquations(Section section)
    {
            
        foreach (var block in section.ChildBlocks)
        {
            if (block is not Equation equation)
            {
                continue;
            }

            EquationCounter++;

            equation.TagName = $"Equation{EquationCounter}";
            equation.CurrentPrefix = $"{_documentMetaData.EquationPrefix} {EquationCounter}: ";

            Equations.Add(equation);
        }
    }

    /// <summary>
    /// Enumerate all items needed for TOF
    /// </summary>
    public void EnumerateAllItemsForTof()
    {
        foreach (var item in Document.ChildBlocks)
        {
            // Only Section instances are interesting
            if (item is not Section section)
            {
                continue;
            }

            // Current section is exluded from numbering
            if (section.DoNotIncludeInNumbering)
            {
                continue;
            }

            // Now find the equations
            FindFigure(section);
        }

    }

    private void FindFigure(Section section)
    {

        foreach (var block in section.ChildBlocks)
        {
            if (block is not Figure figure)
            {
                continue;
            }

            FigureCounter++;

            figure.TagName = $"Figure{FigureCounter}";
            figure.CurrentPrefix = $"{_documentMetaData.FigurePrefix} {FigureCounter}: ";

            Figures.Add(figure);
        }
    }

    /// <summary>
    /// Enumerate all items needed for TOF
    /// </summary>
    public void EnumerateAllItemsForTot()
    {
        foreach (var item in Document.ChildBlocks)
        {
            // Only Section instances are interesting
            if (item is not Section section)
            {
                continue;
            }

            // Current section is exluded from numbering
            if (section.DoNotIncludeInNumbering)
            {
                continue;
            }

            // Now find the equations
            FindTable(section);
        }

    }

    private void FindTable(Section section)
    {

        foreach (var block in section.ChildBlocks)
        {
            if (block is not Table table)
            {
                continue;
            }

            TableCounter++;

            table.TagName = $"Table{TableCounter}";
            table.CurrentPrefix = $"{_documentMetaData.TablePrefix} {TableCounter}: ";

            Tables.Add(table);
        }
    }

    /// <summary>
    /// Prepare all items needed for TOC, TOE and TOF
    /// </summary>
    public void PrepareAllItems()
    {
        PrepareAllItemsForToc();
        PrepareAllItemsForToe();
        PrepareAllItemsForTof();
        PrepareAllItemsForTot();
    }

    /// <summary>
    /// Prepare all items needed for TOC
    /// </summary>
    public void PrepareAllItemsForToc()
    {
        if (Headings.Count == 0)
        {
            return;
        }

        foreach (var heading in Headings)
        {
            TocBase tocItem = heading switch
            {
                Heading1 => new Toc1(),
                Heading2 => new Toc2(),
                Heading3 => new Toc3(),
                Heading4 => new Toc4(),
                Heading5 => new Toc5(),
                _ => null
            };

            if (tocItem is null)
            {
                continue;
            }

            tocItem.TagName = heading.TagName;

            tocItem.AddInline(new Span(heading.CurrentPrefix));
            foreach (var inline in heading.ChildInlines)
            {
                tocItem.AddInline(inline);
            }

            TocItems.Add(tocItem);
        }
    }

    /// <summary>
    /// Prepare all items needed for TOE
    /// </summary>
    public void PrepareAllItemsForToe()
    {
        if (Equations.Count == 0)
        {
            return;
        }

        foreach (var equation in Equations)
        {
            var toe = new Toe
            {
                TagName = equation.TagName
            };
            toe.AddInline(new Span(equation.CurrentPrefix));
            foreach (var inline in equation.ChildInlines)
            {
                toe.AddInline(inline);
            }

            ToeItems.Add(toe);
        }

    }

    /// <summary>
    /// Prepare all items needed for TOF
    /// </summary>
    public void PrepareAllItemsForTof()
    {

        if (Figures.Count == 0)
        {
            return;
        }

        foreach (var figure in Figures)
        {
            var tof = new Tof
            {
                TagName = figure.TagName
            };
            tof.AddInline(new Span(figure.CurrentPrefix));
            foreach (var inline in figure.ChildInlines)
            {
                tof.AddInline(inline);
            }

            TofItems.Add(tof);
        }

    }

    /// <summary>
    /// Prepare all items needed for TOF
    /// </summary>
    public void PrepareAllItemsForTot()
    {

        if (Tables.Count == 0)
        {
            return;
        }

        foreach (var table in Tables)
        {
            var tot = new Tot
            {
                TagName = table.TagName
            };
            tot.AddInline(new Span(table.CurrentPrefix));
            foreach (var inline in table.ChildInlines)
            {
                tot.AddInline(inline);
            }

            TotItems.Add(tot);
        }

    }

    /// <summary>
    /// Prepare all sections TOC, TOE and TOF
    /// </summary>
    public void PrepareAllSections()
    {
        PrepareTocSection();
        PrepareToeSection();
        PrepareTofSection();
        PrepareTotSection();
    }

    /// <summary>
    /// Prepare section TOF
    /// </summary>
    public void PrepareTofSection()
    {
        // No items 
        if (TofItems.Count == 0)
        {
            return;
        }

        // Get the section
        var section = Document.TofSection;

        // No section found: leave here
        if (section is null)
        {
            return;
        }

        // Add all TOF items to section
        foreach (var tofItem in TofItems)
        {
            section.AddBlock(tofItem);
        }
    }

    /// <summary>
    /// Prepare section TOT
    /// </summary>
    public void PrepareTotSection()
    {
        // No items 
        if (TotItems.Count == 0)
        {
            return;
        }

        // Get the section
        var section = Document.TotSection;

        // No section found: leave here
        if (section is null)
        {
            return;
        }

        // Add all TOT items to section
        foreach (var totItem in TotItems)
        {
            section.AddBlock(totItem);
        }
    }

    /// <summary>
    /// Prepare section TOE
    /// </summary>
    public void PrepareToeSection()
    {
        // No items 
        if (ToeItems.Count == 0)
        {
            return;
        }

        // Get the section
        var section = Document.ToeSection;

        // No section found: leave here
        if (section is null)
        {
            return;
        }

        // Add all TOE items to section
        foreach (var toeItem in ToeItems)
        {
            section.AddBlock(toeItem);
        }
    }

    /// <summary>
    /// Prepare section TOC
    /// </summary>
    public void PrepareTocSection()
    {
        // No items
        if (TocItems.Count == 0)
        {
            return;
        }

        // Get the section
        var section = Document.TocSection;

        // No section found: leave here
        if (section is null)
        {
            return;
        }

        // Add all TOC items to section
        foreach (var tocItem in TocItems)
        {
            section.AddBlock(tocItem);
        }

    }
}