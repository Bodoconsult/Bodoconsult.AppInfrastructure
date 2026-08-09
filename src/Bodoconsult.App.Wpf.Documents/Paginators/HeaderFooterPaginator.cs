// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using Bodoconsult.App.Abstractions.Interfaces;
using Bodoconsult.App.Wpf.Documents.Helpers;
using Bodoconsult.App.Wpf.Documents.Interfaces;
using Bodoconsult.App.Wpf.Helpers;
using Thickness = System.Windows.Thickness;

namespace Bodoconsult.App.Wpf.Documents.Paginators;

/// <summary>
/// Implements a paginator to add a header and a footer on the page. Tables can be split on pages and each table section may have a table header
/// </summary>
public class HeaderFooterPaginator : DocumentPaginator
{
    private readonly DocumentPaginator _paginator;
    private readonly IPageSettingsService _pageSettings;
    private PageNumberFormatEnum _pageNumberFormat;

    /// <summary>
    /// Current table
    /// </summary>
    private ContainerVisual _currentTable;

    /// <summary>
    /// Current table header
    /// </summary>
    private ContainerVisual _currentTableHeader;

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="document">document to paginate</param>
    /// <param name="pageSettings">Current typo settings service holding typographical info to use for printing</param>
    /// <param name="dispatcher">the dispatcher to use for the paginator</param>
    /// <param name="pageNumberFormat"></param>
    public HeaderFooterPaginator(FlowDocument document, IPageSettingsService pageSettings, Dispatcher dispatcher,
        PageNumberFormatEnum pageNumberFormat)
    {
        _pageSettings = pageSettings;
        _pageNumberFormat = pageNumberFormat;

        // Run the paginator on a copy of the flow document
        FlowDocument copy = null;

        dispatcher.Invoke(() => { copy = document; });

        // Configure the document as required
        copy.PageWidth = _pageSettings.ContentSize.Width;
        copy.PageHeight = _pageSettings.ContentSize.Height;
        copy.PagePadding = new Thickness(0);
        copy.ColumnWidth = double.MaxValue;

        // Now create the paginator
        _paginator = ((IDocumentPaginatorSource)copy).DocumentPaginator;
        _paginator.PageSize = _pageSettings.ContentSize;
    }

    /// <summary>
    /// Get a page by number as <see cref="DocumentPage"/>
    /// </summary>
    /// <param name="pageNumber">Requested page number (starting with 0)</param>
    /// <returns>A page of the document</returns>
    public override DocumentPage GetPage(int pageNumber)
    {
        // Use default _paginator to handle pagination
        var page = _paginator.GetPage(pageNumber);

        var originalPage = page.Visual;

        var dpi = VisualTreeHelper.GetDpi(originalPage).PixelsPerDip;

        // Save the content for the new page
        var contentVisual = new ContainerVisual
        {
            Transform = new TranslateTransform(
                _pageSettings.ContentOrigin.X,
                _pageSettings.ContentOrigin.Y
            )
        };
        contentVisual.Children.Add(originalPage);

        // Now start the final page visual
        var pageVisual = new ContainerVisual();
        pageVisual.Children.Add(contentVisual);

        // Create header for the new page
        if (_pageSettings.DrawHeaderDelegate != null)
        {
            pageVisual.Children.Add(WpfDocumentRendererHelper.CreateSectionVisual(_pageSettings.DrawHeaderDelegate, _pageSettings.HeaderRect, pageNumber, dpi, _pageNumberFormat));
        }

        // Create footer for the new page
        if (_pageSettings.DrawFooterDelegate != null)
        {
            pageVisual.Children.Add(WpfDocumentRendererHelper.CreateSectionVisual(_pageSettings.DrawFooterDelegate, _pageSettings.FooterRect, pageNumber, dpi, _pageNumberFormat));
        }

        // Check for repeating table headers
        if (!_pageSettings.RepeatTableHeaders)
        {
            return new DocumentPage(
                pageVisual,
                _pageSettings.PageSize,
                new Rect(new Point(), _pageSettings.PageSize),
                new Rect(_pageSettings.ContentOrigin, _pageSettings.ContentSize)
            );
        }

        // Find table header
        ContainerVisual newTable;
        if (_currentTableHeader is null)
        {
            _currentTable = null;
            _currentTableHeader = null;
        }
        else
        {
            if (IsPageStartingWithTable(originalPage, out newTable))
            {
                AddTableHeader(contentVisual, pageVisual);
            }
            else
            {
                _currentTableHeader = null;
                _currentTable = null;
            }
        }

        // Page is ending with a table
        if (IsPageEndingWithTable(originalPage, out newTable, out var newTableHeader))
        {
            // Change of the table header required?
            if (Equals(newTable, _currentTable))
            {
                return new DocumentPage(
                    pageVisual,
                    _pageSettings.PageSize,
                    new Rect(new Point(), _pageSettings.PageSize),
                    new Rect(_pageSettings.ContentOrigin, _pageSettings.ContentSize));
            }

            // New _table: load header to repeat on next page
            _currentTableHeader = newTableHeader;
            _currentTable = newTable;
        }
        else
        {
            // No table at the end of the page
            _currentTableHeader = null;
            _currentTable = null;
        }

        return new DocumentPage(
            pageVisual,
            _pageSettings.PageSize,
            new Rect(new Point(), _pageSettings.PageSize),
            new Rect(_pageSettings.ContentOrigin, _pageSettings.ContentSize)
        );
    }

    /// <summary>
    /// Add a table header to a table section and move the table section below the 
    /// </summary>
    /// <param name="contentVisual">Current content visual</param>
    /// <param name="pageVisual">Current page visual</param>
    private void AddTableHeader(ContainerVisual contentVisual, ContainerVisual pageVisual)
    {
        var headerArea = VisualTreeHelper.GetDescendantBounds(_currentTableHeader);

        // Move the header to the top of the page
        var tableHeaderVisual = new ContainerVisual
        {
            Transform = new TranslateTransform(
                _pageSettings.ContentOrigin.X,
                _pageSettings.ContentOrigin.Y - headerArea.Top
            )
        };

        // Move the content below the table header now
        var yScale = (_pageSettings.ContentSize.Height - headerArea.Height) /
                     _pageSettings.ContentSize.Height;
        var group = new TransformGroup();
        group.Children.Add(new ScaleTransform(1.0, yScale));
        group.Children.Add(new TranslateTransform(
            _pageSettings.ContentOrigin.X,
            _pageSettings.ContentOrigin.Y + headerArea.Height
        ));
        contentVisual.Transform = group;

        if (VisualTreeHelper.GetParent(_currentTableHeader) is ContainerVisual cp)
        {
            cp.Children.Remove(_currentTableHeader);
        }

        tableHeaderVisual.Children.Add(_currentTableHeader);
        pageVisual.Children.Add(tableHeaderVisual);
    }

    /// <summary>
    /// Checks if the page ends with a table
    /// </summary>
    /// <param name="element">Current element</param>
    /// <param name="tableVisual">Found table visual</param>
    /// <param name="tableHeaderVisual">Found table header visual</param>
    /// <returns>Trueif the page ends with a table</returns>
    private static bool IsPageEndingWithTable(DependencyObject element, out ContainerVisual tableVisual, out ContainerVisual tableHeaderVisual)
    {
        tableVisual = null;
        tableHeaderVisual = null;

        var name = element.GetType().Name;

        var children = VisualTreeHelper.GetChildrenCount(element) - 1;

        if (name == "PageVisual")
        {
            element = WpfHelper.FindLastContainerVisual(element);

            element = VisualTreeHelper.GetParent(element);

            if (element is null)
            {
                return false;
            }

            name = element.GetType().Name;

            children = VisualTreeHelper.GetChildrenCount(element) - 2;
        }

        //Debug.Print(name);

        if (name == "RowVisual")
        {
            tableVisual = (ContainerVisual)VisualTreeHelper.GetParent(element);

            if (tableVisual is null)
            {
                return false;
            }

            tableHeaderVisual = (ContainerVisual)VisualTreeHelper.GetChild(tableVisual, 0);
            return true;
        }

        if (children < 0)
        {
            return false;
        }

        var child = VisualTreeHelper.GetChild(element, children);
        return IsPageEndingWithTable(child, out tableVisual, out tableHeaderVisual);
    }



    /// <summary>
    /// Checks if the page starts with a table
    /// </summary>
    /// <param name="element">Current element</param>
    /// <param name="table">Visual containing a table</param>
    /// <returns>True if the page starts with a table else false</returns>
    private static bool IsPageStartingWithTable(DependencyObject element, out ContainerVisual table)
    {
        table = null;

        var name = element.GetType().Name;

        // Only if fresh page: find the next row
        if (name == "PageVisual")
        {
            element = VisualTreeHelper.GetChild(element, 0);

            element = VisualTreeHelper.GetChild(element, 1);

            name = element.GetType().Name;
        }

        // Check if the located element is a row
        if (name == "RowVisual")
        {
            table = (ContainerVisual)VisualTreeHelper.GetParent(element);
            return true;
        }

        // No more kids: leave here
        if (VisualTreeHelper.GetChildrenCount(element) <= 0)
        {
            return false;
        }

        // Search for more rows
        var child = VisualTreeHelper.GetChild(element, 0);
        return IsPageStartingWithTable(child, out table);
    }


    #region Overridden DocumentPaginator members

    /// <summary>
    /// Gets a value indicating whether <see cref="P:System.Windows.Documents.DocumentPaginator.PageCount"/> is the total number of pages. 
    /// </summary>
    /// <returns>
    /// true if pagination is complete and <see cref="P:System.Windows.Documents.DocumentPaginator.PageCount"/> is the total number of pages; otherwise, false, if pagination is in process and <see cref="P:System.Windows.Documents.DocumentPaginator.PageCount"/> is the number of pages currently formatted (not the total).This value may revert to false, after being true, if <see cref="P:System.Windows.Documents.DocumentPaginator.PageSize"/> or content changes; because those events would force a repagination.
    /// </returns>
    public override bool IsPageCountValid => _paginator.IsPageCountValid;

    /// <summary>
    /// Gets a count of the number of pages currently formatted
    /// </summary>
    /// <returns>
    /// A count of the number of pages that have been formatted.
    /// </returns>
    public override int PageCount => _paginator.PageCount;

    /// <summary>
    /// Gets or sets the suggested width and height of each page.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Windows.Size"/> representing the width and height of each page.
    /// </returns>
    public override Size PageSize
    {
        get => _paginator.PageSize;
        set => _paginator.PageSize = value;
    }

    /// <summary>
    /// Returns the element being paginated.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Windows.Documents.IDocumentPaginatorSource"/> representing the element being paginated.
    /// </returns>
    public override IDocumentPaginatorSource Source => _paginator.Source;

    #endregion





}