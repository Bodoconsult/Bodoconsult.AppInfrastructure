// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.ComponentModel;
using System.Drawing.Printing;
using System.Globalization;
using Bodoconsult.App.Abstractions.Delegates;

// ReSharper disable LocalizableElement

namespace Bodoconsult.App.WinForms.Reporting;

/// <summary>
/// Print preview form code behind
/// </summary>
public sealed partial class PrintPreviewForm : Form
{

    /// <summary>
    /// Current document
    /// </summary>
    public PrintDocument Document;

    /// <summary>
    /// The number of pages in the preview
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    //[DefaultValue(0)]
    public int NumberOfPages { get; set; }

    /// <summary>
    /// Default ctor
    /// </summary>
    /// <param name="translateDelegate">Delegate for translation</param>
    public PrintPreviewForm(TranslateDelegate translateDelegate)
    {

        InitializeComponent();

        if (translateDelegate is null)
        {
            Text = "Print preview";
            buttonPrint.Text = "Print";
            buttonExit.Text = "Exit";
            Duplex.Text = "Duplex print";
            return;
        }

        Text = translateDelegate.Invoke("Bodoconsult.App.WinForms.Reporting.PrintPreviewForm.Title");
        buttonPrint.Text = translateDelegate.Invoke("Bodoconsult.App.WinForms.Reporting.PrintPreviewForm.PrintButton");
        buttonExit.Text = translateDelegate.Invoke("Bodoconsult.App.WinForms.Reporting.PrintPreviewForm.ExitButton"); 
        Duplex.Text = translateDelegate.Invoke("Bodoconsult.App.WinForms.Reporting.PrintPreviewForm.DuplexPrint");
    }

    /// <summary>
    /// Load a document to print
    /// </summary>
    /// <param name="name"></param>
    /// <param name="document"></param>
    /// <param name="documentName"></param>
    /// <param name="numberOfPages"></param>
    /// <param name="screen"></param>
    public void LoadDocument(string name, PrintDocument document, string documentName,
        int numberOfPages, Screen screen)
    {
        Document = document ?? throw new ArgumentNullException(nameof(document));
        NumberOfPages = numberOfPages;
        InitializeComponent();

        StartPosition = FormStartPosition.Manual;

        Location = screen.WorkingArea.Location;

        // set it fullscreen
        Size = new Size(screen.WorkingArea.Width, screen.WorkingArea.Height);

        WindowState = FormWindowState.Maximized;

        printPreviewControl1.Document = document;
        printPreviewControl1.Name = name;
        printPreviewControl1.Document.DocumentName = documentName;
        printDialog1.Document = document;

        Duplex.Checked = true;
        Duplex.Enabled = printDialog1.PrinterSettings.CanDuplex;

        textBoxCurrentZoom.Text = Math.Round(printPreviewControl1.Zoom * 100, 0).ToString(CultureInfo.InvariantCulture);
        textBoxPage.Text = (printPreviewControl1.StartPage + 1).ToString();
    }

    /// <summary>
    /// Change the zoom
    /// </summary>
    public void ChangeZoom()
    {
        try
        {
            printPreviewControl1.Zoom = Convert.ToDouble(Convert.ToUInt16(textBoxCurrentZoom.Text)/100);
        }
        catch
        {
            // ignored
        }

        textBoxCurrentZoom.Text = Math.Round(printPreviewControl1.Zoom * 100, 0).ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Change the page
    /// </summary>
    public void PageChange()
    {
        try
        {
            printPreviewControl1.StartPage = Convert.ToByte(Convert.ToByte(textBoxPage.Text) - 1);
        }
        catch
        {
            textBoxPage.Text = (printPreviewControl1.StartPage + 1).ToString();
        }
    }

    private void buttonExit_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void buttonPrint_Click(object sender, EventArgs e)
    {
        try
        {
            if (printDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (Duplex.Checked && printDialog1.PrinterSettings.CanDuplex)
            {
                printDialog1.PrinterSettings.Duplex = System.Drawing.Printing.Duplex.Vertical;
            }

            Document.Print();
        }
        catch
        {
            // ignored
        }

        Close();
    }

    private void textBoxCurrentZoom_Leave(object sender, EventArgs e)
    {
        ChangeZoom();
    }

    private void buttonPrevPage_Click(object sender, EventArgs e)
    {
        if (printPreviewControl1.StartPage <= 0)
        {
            return;
        }

        printPreviewControl1.StartPage--;
        textBoxPage.Text = (printPreviewControl1.StartPage + 1).ToString();
    }

    private void textBoxPage_Leave(object sender, EventArgs e)
    {
        PageChange();
    }

    private void buttonNextPage_Click(object sender, EventArgs e)
    {
        if (printPreviewControl1.StartPage - 1 >= NumberOfPages)
        {
            return;
        }

        printPreviewControl1.StartPage++;
        textBoxPage.Text = (printPreviewControl1.StartPage + 1).ToString();
    }

    private void PrintPreviewForm_Resize(object sender, EventArgs e)
    {
        try
        {
            printPreviewControl1.Width = Width - 96;
            printPreviewControl1.Height = Height - printPreviewControl1.Top - 40;
            buttonExit.Left = printPreviewControl1.Width + printPreviewControl1.Left - buttonExit.Width;
        }
        catch
        {
            // ignored
        }
    }
}