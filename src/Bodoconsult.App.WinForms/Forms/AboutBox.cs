// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using Bodoconsult.App.WinForms.Helpers;
using System.Reflection;

namespace Bodoconsult.App.WinForms.Forms;

/// <summary>
/// About box
/// </summary>
public sealed partial class AboutBox : Form
{
    private static AboutBox _instance;
    private readonly Assembly _assembly = Assembly.GetEntryAssembly();


    private AboutBox()
    {
        InitializeComponent();

        //  Initialisieren Sie AboutBox, um die Produktinformationen aus den Assemblyinformationen anzuzeigen.
        //  Ändern Sie die Einstellungen für Assemblyinformationen für Ihre Anwendung durch eine der folgenden Vorgehensweisen:
        //  - Projekt->Eigenschaften->Anwendung->Assemblyinformationen
        //  - AssemblyInfo.cs            

        // Titel
        Text = $"{AssemblyTitle}";

        // Produkt       
        labelProductName.Text = AssemblyProduct;

        // Version
        labelVersion.Text = $"Version {AssemblyVersion}";


        // Coypright
        labelCopyright.Text = AssemblyCopyright;

        // Company
        labelCompanyName.Text = AssemblyCompany;

        // Beschreibung
        textBoxDescription.Text = AssemblyDescription;

        Refresh();
    }

    private AboutBox(Image logo)
    {
        InitializeComponent();

        //  Initialisieren Sie AboutBox, um die Produktinformationen aus den Assemblyinformationen anzuzeigen.
        //  Ändern Sie die Einstellungen für Assemblyinformationen für Ihre Anwendung durch eine der folgenden Vorgehensweisen:
        //  - Projekt->Eigenschaften->Anwendung->Assemblyinformationen
        //  - AssemblyInfo.cs

        if (logo != null)
        {
            logoPictureBox.Image = logo;
        }

        // Titel
        Text = $"{AssemblyTitle}";

        // Produkt       
        labelProductName.Text = AssemblyProduct;

        // Version
        labelVersion.Text = $"Version {AssemblyVersion}";


        // Coypright
        labelCopyright.Text = AssemblyCopyright;

        // Company
        labelCompanyName.Text = AssemblyCompany;

        // Beschreibung
        textBoxDescription.Text = AssemblyDescription;

        Refresh();
    }

    private AboutBox(Image logo, Color c)
    {
        InitializeComponent();

        BackColor = c;
        TransparencyKey = c;

        //  Initialisieren Sie AboutBox, um die Produktinformationen aus den Assemblyinformationen anzuzeigen.
        //  Ändern Sie die Einstellungen für Assemblyinformationen für Ihre Anwendung durch eine der folgenden Vorgehensweisen:
        //  - Projekt->Eigenschaften->Anwendung->Assemblyinformationen
        //  - AssemblyInfo.cs

        if (logo != null)
        {
            logoPictureBox.Image = logo;
        }

        // Titel
        Text = $"{AssemblyTitle}";

        // Produkt       
        labelProductName.Text = AssemblyProduct;

        // Version
        labelVersion.Text = $"Version {AssemblyVersion}";


        // Coypright
        labelCopyright.Text = AssemblyCopyright;

        // Company
        labelCompanyName.Text = AssemblyCompany;

        // Beschreibung
        textBoxDescription.Text = AssemblyDescription;

        Refresh();
    }


    /// <summary>Releases unmanaged resources and performs other cleanup operations before the <see cref="T:System.ComponentModel.Component" /> is reclaimed by garbage collection.</summary>
    ~AboutBox()
    {
        _instance = null;
    }

    #region Assemblyattributaccessoren

    /// <summary>
    /// Assembly title
    /// </summary>
    public string AssemblyTitle
    {
        get
        {
            var attributes = _assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

            if (attributes.Length <= 0)
            {
                return Path.GetFileNameWithoutExtension(_assembly.Location);
            }

            var titleAttribute = (AssemblyTitleAttribute)attributes[0];
            return !string.IsNullOrEmpty( titleAttribute.Title) ? titleAttribute.Title : Path.GetFileNameWithoutExtension(_assembly.Location);
        }
    }

    /// <summary>
    /// Assembly version
    /// </summary>
    public string AssemblyVersion => _assembly.GetName().Version?.ToString() ?? string.Empty;

    /// <summary>
    /// Assembly description
    /// </summary>
    public string AssemblyDescription
    {
        get
        {
            var attributes = _assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            return attributes.Length == 0 ? string.Empty : ((AssemblyDescriptionAttribute)attributes[0]).Description;
        }
    }

    /// <summary>
    /// Assembly product
    /// </summary>
    public string AssemblyProduct
    {
        get
        {
            var attributes = _assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            return attributes.Length == 0 ? string.Empty : ((AssemblyProductAttribute)attributes[0]).Product;
        }
    }

    /// <summary>
    /// Assembly copyright
    /// </summary>
    public string AssemblyCopyright
    {
        get
        {
            var attributes = _assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            return attributes.Length == 0 ? string.Empty : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }
    }

    /// <summary>
    /// Assembly company
    /// </summary>
    public string AssemblyCompany
    {
        get
        {
            var attributes = _assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            return attributes.Length == 0 ? string.Empty : ((AssemblyCompanyAttribute)attributes[0]).Company;
        }
    }
    #endregion

    private void OkButtonClick(object sender, EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// Start
    /// </summary>
    public void Start()
    {
        okButton.Enabled = false;
    }

    /// <summary>
    /// Start
    /// </summary>
    /// <param name="milliseconds">Timer interval in ms</param>
    public void Start(int milliseconds)
    {
        okButton.Enabled = false;
        timer1.Interval = milliseconds;
        timer1.Enabled = true;
    }


    /// <summary>
    /// Open a singleton instance
    /// </summary>
    /// <returns><see cref="AboutBox"/> instance</returns>
    public static AboutBox Instance()
    {
        return _instance ??= new AboutBox();
    }


    /// <summary>
    /// Open a singleton instance
    /// </summary>
    /// <returns><see cref="AboutBox"/> instance</returns>
    public static AboutBox Instance(Image logo)
    {
        return _instance ??= new AboutBox(logo);
    }

    /// <summary>
    /// Open a singleton instance
    /// </summary>
    /// <returns><see cref="AboutBox"/> instance</returns>
    public static AboutBox Instance(Image logo, Color  c)
    {
        return _instance ??= new AboutBox(logo, c);
    }


    private void AboutBoxFormClosed(object sender, FormClosedEventArgs e)
    {
        _instance = null;
    }

    /// <summary>
    /// Hide the About box
    /// </summary>
    public new void Hide()
    {
        _instance = null;
        base.Hide();
    }

    private void AboutBoxPaint(object sender, PaintEventArgs e)
    {
        try
        {
            WinFormsHelper.DrawRoundedRectangle(e.Graphics, 0, 0, Width, Height, 50, new SolidBrush(Color.DimGray), null);
            WinFormsHelper.DrawRoundedRectangle(e.Graphics, 2, 2, Width - 4, Height - 4, 48, new SolidBrush(Color.Pink), null);
            WinFormsHelper.DrawRoundedRectangle(e.Graphics, 4, 4, Width - 8, Height - 8, 46, new SolidBrush(Color.DarkGray), null);
        }
        catch
        {
            // ignored
        }
    }

    /// <summary>
    /// Überschreiben der Methode WndProc um die Nachricht WM_NCHITTEST abzufangen und zu simulieren, dass der Anwender auf der Titelleiste geklickt hat
    /// </summary>
    /// <param name="m"></param>
    protected override void WndProc(ref Message m)
    {
        try
        {
            const int wmNchittest = 0x0084;
            const int htcaption = 2;

            // Abfangen der Nachricht WM_NCHITTEST
            if (m.Msg == wmNchittest)
            {
                if (ClientRectangle.Contains(PointToClient(Cursor.Position)))
                {
                    // Wenn der Cursor sich im Clientbereich des Formulars befindet:
                    // Simulieren, dass der Cursor sich auf der Titelleiste befindet
                    m.Result = (IntPtr)htcaption;
                    return;
                }
            }

            base.WndProc(ref m);
        }
        catch
        {
            // ignored
        }
    }

    private void Timer1Tick(object sender, EventArgs e)
    {
        Close();
    }
}