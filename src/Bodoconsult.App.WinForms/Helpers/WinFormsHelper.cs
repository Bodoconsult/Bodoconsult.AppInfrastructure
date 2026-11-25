// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using Bodoconsult.App.Helpers;

namespace Bodoconsult.App.WinForms.Helpers;

// https://formats.kaitai.io/windows_lnk_file/csharp.html

/// <summary>
/// WinForms related tools
/// </summary>
public static class WinFormsHelper
{
    private static string _lastDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    private static string _lastFile;

    /// <summary>
    /// Checks if the connected screens fullfill the min requirements of the form.
    /// Form.MinimumSize Property has to be set for that.
    /// </summary>
    /// <param name="form">The form that should be checked</param>
    /// <returns>True if the screen resolution fullfills requirements</returns>
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
    /// <param name="width">Required minimum width of app</param>
    /// <param name="height">Required minimum height of app</param>
    /// <returns>True if the screen resolution fullfills requirements</returns>
    public static bool IsScreenResolutionBigEnough(int width, int height)
    {
        // Check the resolution of all screens
        foreach (var screen in Screen.AllScreens)
        {
            if (screen.WorkingArea.Height < height || screen.WorkingArea.Width < width)
            {
                // Too small
                return false;
            }
        }

        // OK
        return true;
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
    /// <param name="linePen">Pen for borders or null</param>
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

    //public static void CreateShortCut(string target, string destination, string description)
    //{
    //    new ShellShortcut(target)
    //    {
    //        Path = destination,
    //        Description = description
    //    }.Save();
    //}

    /// <summary>
    /// Text saying 'save file'
    /// </summary>
    public static string SaveFileAsText { get; set; } = "Save file";

    /// <summary>
    /// Text saying 'open file'
    /// </summary>
    public static string OpenFileText { get; set; } = "Open file";

    /// <summary>
    /// Show a save as file dialog
    /// </summary>
    /// <param name="file">Filename proposal</param>
    /// <param name="filter">Filter</param>
    /// <returns>Filename of the file saved finally or null</returns>
    public static string SaveFileAs(string file, string filter)
    {
        var saveFileDialog = new SaveFileDialog();
        if (_lastDir != null)
        {
            saveFileDialog.InitialDirectory = _lastDir;
        }
        if (filter != null)
        {
            saveFileDialog.Filter = filter;
        }

        if (file != null)
        {
            saveFileDialog.FileName = file;
        }

        saveFileDialog.Title = SaveFileAsText;
        return saveFileDialog.ShowDialog() == DialogResult.Cancel ? null : saveFileDialog.FileName;
    }

    /// <summary>
    /// Show an open a file dialog
    /// </summary>
    /// <param name="extensionMask">File selection mask</param>
    /// <param name="fileName">Proposal for the filename</param>
    /// <returns>Selected filename to open</returns>
    public static string OpenFile(string extensionMask, string fileName)
    {
        try
        {
            string str1;
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = _lastFile;
                str1 = _lastDir;
            }
            else
            {
                str1 = new FileInfo(fileName).DirectoryName;
            }

            var openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = str1 ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = fileName
            };

            var openFileDialog2 = openFileDialog1;
            openFileDialog2.Title = OpenFileText;
            openFileDialog2.Filter = extensionMask;
            if (openFileDialog2.ShowDialog() == DialogResult.Cancel)
            {
                return null;
            }

            fileName = openFileDialog2.FileName;
            if (openFileDialog2.FileName == "")
            {
                return null;
            }
            _lastDir = new FileInfo(fileName).DirectoryName;
            _lastFile = fileName;
            return openFileDialog2.FileName;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Run a command in the shell
    /// </summary>
    /// <param name="shellCmd">Shell command</param>
    public static void RunInShell(string shellCmd)
    {
        if (string.IsNullOrEmpty(shellCmd))
        {
            return;
        }

        var startInfo = new ProcessStartInfo("cmd.exe ", $"/C {shellCmd}")
        {
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Hidden
        };
        try
        {
            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error running command {shellCmd}: {ex.Message}");
        }
    }

    /// <summary>
    /// Run a command in the shell visible
    /// </summary>
    /// <param name="shellCmd">Shell command</param>
    public static void RunInShellVisible(string shellCmd)
    {
        if (string.IsNullOrEmpty(shellCmd))
        {
            return;
        }

        var startInfo = new ProcessStartInfo("cmd.exe ", $"/C {shellCmd}")
        {
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Normal
        };
        RunInternal(startInfo);
    }

    /// <summary>
    /// Run a command in the shell visible
    /// </summary>
    /// <param name="shellCmd">Shell command</param>
    /// <param name="workingDir">Requested working directory</param>
    public static void RunInShellVisible(string shellCmd, string workingDir)
    {
        if (string.IsNullOrEmpty(shellCmd))
        {
            return;
        }

        var startInfo = new ProcessStartInfo("cmd.exe ", $"/C {shellCmd}")
        {
            WorkingDirectory = workingDir,
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Normal
        };
        RunInternal(startInfo);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="shellCmd"></param>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <exception cref="Exception"></exception>
    public static void RunInShell(string shellCmd, string userName, SecureString password)
    {
        if (string.IsNullOrEmpty(shellCmd))
        {
            return;
        }

        var startInfo = new ProcessStartInfo("cmd.exe ", $"/C {shellCmd}")
        {
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            LoadUserProfile = false,
            UserName = userName,
            Password = password
        };
        try
        {
            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error running command {shellCmd}: {ex.Message}");
        }
    }

    /// <summary>
    /// Run an EXE file in the shell but do not wait for it
    /// </summary>
    /// <param name="exe">EXE file</param>
    /// <param name="args">Arguments</param>
    public static void Shell(string exe, string args)
    {
        var startInfo = new ProcessStartInfo(exe, args)
        {
            WindowStyle = ProcessWindowStyle.Maximized
        };
        try
        {
            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error running command {exe}: {ex.Message}");
        }
    }

    /// <summary>
    /// Run an EXE file in the shell and wait for it
    /// </summary>
    /// <param name="exe">EXE file</param>
    /// <param name="args">Arguments</param>
    public static void ShellAndWait(string exe, string args)
    {
        var startInfo = new ProcessStartInfo(exe, args)
        {
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Maximized
        };
        RunInternal(startInfo);
    }

    /// <summary>
    /// Run an EXE file in the shell and wait for it
    /// </summary>
    /// <param name="exe">EXE file</param>
    /// <param name="args">Arguments</param>
    /// <param name="userName">Username</param>
    /// <param name="password">Password</param>
    public static void ShellAndWait(string exe, string args, string userName, SecureString password)
    {
        var startInfo = new ProcessStartInfo(exe, args)
        {
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Maximized,
            LoadUserProfile = false,
            UserName = userName,
            Password = password
        };
        RunInternal(startInfo);
    }

    /// <summary>
    /// Run an EXE file calling the verb print
    /// </summary>
    /// <param name="exe">EXE file</param>
    /// <param name="args">Arguments</param>
    public static void ShellAndPrint(string exe, string args)
    {
        var startInfo = new ProcessStartInfo(exe, args)
        {
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            Verb = "print"
        };
        RunInternal(startInfo);
    }

    /// <summary>
    /// Run an EXE file hidden and wait for it
    /// </summary>
    /// <param name="exe">EXE file</param>
    /// <param name="args">Arguments</param>
    public static void ShellAndWaitHidden(string exe, string args)
    {
        var startInfo = new ProcessStartInfo(exe, args)
        {
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Hidden
        };
        RunInternal(startInfo);
    }

    private static void RunInternal(ProcessStartInfo startInfo)
    {
        try
        {
            var process = Process.Start(startInfo);

            if (process is null)
            {
                return;
            }

            process.WaitForExit();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error running command {startInfo.FileName} {startInfo.Arguments}: {ex}");
        }
    }

    /// <summary>
    ///The name of the default printer
    /// </summary>
    public static string DefaultPrinter => new PrintDocument().PrinterSettings.PrinterName;

    /// <summary>
    /// Get all printer paper sources
    /// </summary>
    /// <param name="printerName">Name of the printer</param>
    /// <returns>Collection of printer paper sources</returns>
    public static PrinterSettings.PaperSourceCollection PrinterSources(string printerName)
    {
        return new PrintDocument
        {
            PrinterSettings = {
        PrinterName = printerName
      }
        }.PrinterSettings.PaperSources;
    }

    /// <summary>
    /// Get paper size collection for a printer
    /// </summary>
    /// <param name="printerName">Name of the printer</param>
    /// <returns>Collection of paper sizes</returns>
    public static PrinterSettings.PaperSizeCollection PrinterPapers(string printerName)
    {
        return new PrintDocument
        {
            PrinterSettings = {
                PrinterName = printerName
            }
        }.PrinterSettings.PaperSizes;
    }

    /// <summary>
    /// The language of the current thread
    /// </summary>
    public static string CurrentThreadLanguage
    {
        get
        {
            var language = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag[..2];
            return language;
        }
    }

    /// <summary>
    /// Select a folder
    /// </summary>
    /// <returns>Select folder path or null</returns>
    public static string SelectFolder()
    {
        var lastDir = _lastDir;

        var folderBrowserDialog = new FolderBrowserDialog
        {
            Description = SelectFolderText,
            RootFolder = Environment.SpecialFolder.MyComputer,
            SelectedPath = lastDir
        };

        if (folderBrowserDialog.ShowDialog() == DialogResult.Cancel)
        {
            return null;
        }
        if (string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
        {
            return folderBrowserDialog.SelectedPath;
        }

        _lastDir = folderBrowserDialog.SelectedPath;
        return folderBrowserDialog.SelectedPath;
    }

    /// <summary>
    /// Text saying 'select folder'
    /// </summary>
    public static string SelectFolderText { get; set; } = "Select folder";

    /// <summary>
    /// Select a folder
    /// </summary>
    /// <param name="dirPath">Start directory path</param>
    /// <returns>Select folder path or null</returns>
    public static string SelectFolder(string dirPath)
    {
        var folderBrowserDialog = new FolderBrowserDialog
        {
            Description = SelectFolderText,
            RootFolder = Environment.SpecialFolder.MyComputer,
            SelectedPath = dirPath
        };
        if (folderBrowserDialog.ShowDialog() == DialogResult.Cancel)
        {
            return null;
        }
        if (string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
        {
            return folderBrowserDialog.SelectedPath;
        }

        _lastDir = folderBrowserDialog.SelectedPath;
        return folderBrowserDialog.SelectedPath;
    }

    /// <summary>
    /// Set default folder
    /// </summary>
    /// <param name="dirPath">Directory path</param>
    public static void SetDefaultFolder(string dirPath)
    {
        _lastDir = dirPath;
    }

    /// <summary>
    /// Open a folder in operating system
    /// </summary>
    /// <param name="dirPath">Path to the folder to open</param>
    public static void OpenFolder(string dirPath) => FileSystemHelper.Run($"explorer.exe /n /e \"{dirPath}\"");

    /// <summary>
    /// Check if a file is in use
    /// </summary>
    /// <param name="filePath">Full file path</param>
    /// <returns>True if the file is already in use else false</returns>
    public static bool FileInUse(string filePath)
    {
        try
        {
            var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
            stream.Close();
            stream.Dispose();
        }
        catch
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Load a cursor from a file
    /// </summary>
    /// <param name="fileName">Filepath to load the cursor from</param>
    /// <returns>Pounter to the cursor</returns>
    [DllImport("user32.dll", EntryPoint = "LoadCursorFromFileW", CharSet = CharSet.Unicode)]
    private static extern IntPtr LoadCursorFromFile(string fileName);

    /// <summary>
    /// Load a cursor from a resource
    /// </summary>
    /// <param name="cursorResourceName">Name of the cursor resource</param>
    /// <returns>Cursor instance</returns>
    public static Cursor CreateCursorFromFile(string cursorResourceName)
    {
        var embeddedResourceStream = GetEmbeddedResourceStream(cursorResourceName);
        var buffer = new byte[embeddedResourceStream.Length];
        embeddedResourceStream.ReadExactly(buffer, 0, buffer.Length);
        embeddedResourceStream.Close();
        var randomFileName = Path.GetRandomFileName();
        var fileInfo = new FileInfo(randomFileName);
        var fileStream = fileInfo.Create();
        fileStream.Write(buffer, 0, buffer.Length);
        fileStream.Close();
        var cursorFromFile = new Cursor(LoadCursorFromFile(randomFileName));
        fileInfo.Delete();
        return cursorFromFile;
    }

    /// <summary>
    /// Get an embedded resource from the calling assembly
    /// </summary>
    /// <param name="resourceName">Resource name</param>
    /// <returns>Embedded image ressource</returns>
    public static Image GetImageFromEmbeddedResourceExecutingAssembly(string resourceName)
    {
        var manifestResourceStream = Assembly.GetCallingAssembly().GetManifestResourceStream(resourceName);
        return manifestResourceStream == null ? null : (Image)new Bitmap(manifestResourceStream);
    }

    /// <summary>
    /// Create a thumbnail from an image
    /// </summary>
    /// <param name="originalImage">Original image</param>
    /// <param name="imageHeight">Image height</param>
    /// <returns>Thumbnail image</returns>
    public static Image CreateThumbnail(Image originalImage, int imageHeight)
    {
        var num = originalImage.Width / (float)originalImage.Height;
        var thumbWidth = (int)(imageHeight * (double)num);
        return originalImage.GetThumbnailImage(thumbWidth, imageHeight, ThumbnailCallback, IntPtr.Zero);
    }

    private static bool ThumbnailCallback() => true;

    /// <summary>
    /// Get a rectangle fitting to an image
    /// </summary>
    /// <param name="image">Image to scale</param>
    /// <param name="targetArea">Target rectangle</param>
    /// <param name="strechToFit">Stretch to fit</param>
    /// <returns>Rectangle fitting to the image</returns>
    public static Rectangle ScaleToFit(Image image, Rectangle targetArea, bool strechToFit)
    {
        Rectangle fit;
        if (image.Width < targetArea.Width && image.Height < targetArea.Height)
        {
            if (strechToFit)
            {
                var num = Math.Min(targetArea.Width / (float)image.Width, targetArea.Height / (float)image.Height);
                fit = new Rectangle(targetArea.X, targetArea.Y, (int)(image.Width * (double)num), (int)(image.Height * (double)num));
                if (fit.Width < targetArea.Width)
                    fit.X += (targetArea.Width - fit.Width) / 2;
                if (fit.Height < targetArea.Height)
                    fit.Y += (targetArea.Height - fit.Height) / 2;
            }
            else
            {
                fit = new Rectangle(targetArea.Location, image.Size);
                fit.X += (targetArea.Width - fit.Width) / 2;
                fit.Y += (targetArea.Height - fit.Height) / 2;
            }
        }
        else
        {
            fit = new Rectangle(targetArea.Location, targetArea.Size);
            if (image.Width * fit.Height > image.Height * fit.Width)
            {
                fit.Height = fit.Width * image.Height / image.Width;
                fit.Y += (targetArea.Height - fit.Height) / 2;
            }
            else
            {
                fit.Width = fit.Height * image.Width / image.Height;
                fit.X += (targetArea.Width - fit.Width) / 2;
            }
        }
        return fit;
    }

    internal static Image GetImageFromEmbeddedResourceCallingAssembly(string resourceName)
    {
        return new Bitmap(GetEmbeddedResourceStream(resourceName));
    }

    private static Stream GetEmbeddedResourceStream(string resourceName)
    {
        return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
    }
}