using System.Drawing.Drawing2D;

namespace Bodoconsult.App.WinForms;

/// <summary>
/// Tools
/// </summary>
public static class Tools
{
    /// <summary>
    /// Checks if the connected screens fullfill the min requirements of the form.
    /// Form.MinimumSize Property has to be set for that.
    /// </summary>
    /// <param name="form">The form that should be checked</param>
    /// <returns></returns>
    public static bool IsScreenResolutionBigEnough(Form form)
    {
        if (form.MinimumSize.Width == 0 || form.MinimumSize.Height == 0)
        {
            throw new Exception("The MinimumSize Properties Width and Height of the given form have to be set or use overloaded function 'IsScreenResolutionBigEnough(int width, int height)' instead");
        }
        return IsScreenResolutionBigEnough(form.MinimumSize.Width, form.MinimumSize.Height);
    }

    /// <summary>
    /// Checks if the connected screens fullfill the min resolution requirements
    /// </summary>
    /// <param name="width">width of app</param>
    /// <param name="height">height of app</param>
    /// <returns>True if the screen resolution fullfills requirement</returns>
    public static bool IsScreenResolutionBigEnough(int width, int height)
    {
        // ToDo: Check screen resolution
        return true;

        //var oMs = new ManagementScope();

        ////get Fixed disk stats
        //var oQuery = new ObjectQuery("select * from Win32_VideoController");

        ////Execute the query  
        //var oSearcher = new ManagementObjectSearcher(oMs, oQuery);

        ////Get the results
        //var oReturnCollection = oSearcher.Get();

        ////loop through found drives and write out info
        //return !(from ManagementObject oReturn in oReturnCollection let sWidth = int.Parse(oReturn["CurrentHorizontalResolution"].ToString()) let sHeight = int.Parse(oReturn["CurrentVerticalResolution"].ToString()) where sWidth < width || sHeight < height select sWidth).Any();
    }


    /// <summary>
    /// Draw a rectangle with rounded corners
    /// </summary>
    /// <param name="graphics"> </param>
    /// <param name="x">X-Position of rectangle</param>
    /// <param name="y">Y-Position of rectangle</param>
    /// <param name="width">Width of rectangle</param>
    /// <param name="height">Height of rectangle</param>
    /// <param name="cornerRadius">Corner radius</param>
    /// <param name="fillBrush">Brush for filling or null.</param>
    /// <param name="linePen">Pen for lines or null</param>
    public static void DrawRoundedRectangle(Graphics graphics,
        int x, int y, int width, int height,
        int cornerRadius, Brush fillBrush, Pen linePen)
    {
        // Neuen GraphicsPath erzeugen ...
        var gp = new GraphicsPath();

        // ... und die Linien der Figur hinzufügen
        // Oben
        gp.AddLine(x + cornerRadius, y, x + width - cornerRadius, y);
        // Ecke rechts oben
        gp.AddArc(x + width - cornerRadius, y, cornerRadius,
            cornerRadius, 270, 90);
        // Rechts
        gp.AddLine(x + width, y + cornerRadius, x + width,
            y + height - cornerRadius);
        // Ecke rechts unten
        gp.AddArc(x + width - cornerRadius, y + height - cornerRadius,
            cornerRadius, cornerRadius, 0, 90);
        // Unten
        gp.AddLine(x + width - cornerRadius, y + height,
            x + cornerRadius, y + height);
        // Ecke links unten
        gp.AddArc(x, y + height - cornerRadius, cornerRadius,
            cornerRadius, 90, 90);
        // Links
        gp.AddLine(x, y + height - cornerRadius, x, y + cornerRadius);
        // Ecke links oben
        gp.AddArc(x, y, cornerRadius, cornerRadius, 180, 90);

        // Die Figur abschließen
        gp.CloseFigure();

        // Den Pfad mit dem übergebenen Pinsel füllen
        if (fillBrush != null)
        {
            graphics.FillPath(fillBrush, gp);
        }

        // Die Linien des Pfades zeichnen
        if (linePen != null)
        {
            graphics.DrawPath(linePen, gp);
        }
    }
}