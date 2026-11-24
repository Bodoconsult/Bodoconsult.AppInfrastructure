// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Runtime.InteropServices;

namespace Bodoconsult.App.WinForms;

public static class WindowControl
{
    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

    [DllImport("user32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

    [DllImport("user32.dll")]
    public static extern IntPtr FindWindow(string className, string windowTitle);


    //private const int WmSetRedraw = 11;

    private const int WmSetRedraw = 11;
    private const int WM_USER = 0x400;

    private const int EM_GETEVENTMASK = (WM_USER + 59);

    private const int EM_SETEVENTMASK = (WM_USER + 69);


    public static void SuspendDrawing(Control parent)
    {
        //SendMessage(parent.Handle, WmSetRedraw, false, 0);

        var msgSuspendUpdate = Message.Create(parent.Handle, WmSetRedraw, IntPtr.Zero,
            IntPtr.Zero);

        var window = NativeWindow.FromHandle(parent.Handle);
        window.DefWndProc(ref msgSuspendUpdate);

    }

    public static void SuspendDrawing(IntPtr window)
    {
        SendMessage(window, WmSetRedraw, 0, 0);

        //var msgSuspendUpdate = Message.Create(window, WmSetRedraw, IntPtr.Zero,
        //IntPtr.Zero);

        //var w = NativeWindow.FromHandle(window);
        //w.DefWndProc(ref msgSuspendUpdate);

    }


    public static void ResumeDrawing(Control parent)
    {
        // Create a C "true" boolean as an IntPtr
        var wparam = new IntPtr(1);
        var msgResumeUpdate = Message.Create(parent.Handle, WmSetRedraw, wparam,
            IntPtr.Zero);

        var window = NativeWindow.FromHandle(parent.Handle);
        window.DefWndProc(ref msgResumeUpdate);

        parent.Invalidate();

    }

    public static void ResumeDrawing(IntPtr window)
    {

        SendMessage(window, WmSetRedraw, 1, 0);
        //// Create a C "true" boolean as an IntPtr
        //var wparam = new IntPtr(1);
        //var msgResumeUpdate = Message.Create(window, WmSetRedraw, wparam,
        //    IntPtr.Zero);

        //var w = NativeWindow.FromHandle(window);
        //w.DefWndProc(ref msgResumeUpdate);

    }


    public static IntPtr StopEvents(IntPtr window)
    {
        return (IntPtr)SendMessage(window, EM_GETEVENTMASK, 0, 0);
    }


    public static void StartEvents(IntPtr window, IntPtr eventMask)
    {
        SendMessage(window, EM_SETEVENTMASK, 0, (int)eventMask);
    }

    /// <summary>
    /// Returns a list of child windows
    /// </summary>
    /// <param name="parent">Parent of the windows to return</param>
    /// <returns>List of child windows</returns>
    public static List<IntPtr> GetChildWindows(IntPtr parent)
    {
        var result = new List<IntPtr>();
        var listHandle = GCHandle.Alloc(result);
        try
        {
            var childProc = new EnumWindowProc(EnumWindow);
            EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
        }
        finally
        {
            if (listHandle.IsAllocated)
                listHandle.Free();
        }
        return result;
    }

    /// <summary>
    /// Callback method to be used when enumerating windows.
    /// </summary>
    /// <param name="handle">Handle of the next window</param>
    /// <param name="pointer">Pointer to a GCHandle that holds a reference to the list to fill</param>
    /// <returns>True to continue the enumeration, false to bail</returns>
    private static bool EnumWindow(IntPtr handle, IntPtr pointer)
    {
        var gch = GCHandle.FromIntPtr(pointer);
        var list = gch.Target as List<IntPtr>;
        if (list == null)
        {
            throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
        }
        list.Add(handle);
        //  You can modify this to check to see if you want to cancel the operation, then return a null here
        return true;
    }

    /// <summary>
    /// Delegate for the EnumChildWindows method
    /// </summary>
    /// <param name="hWnd">Window handle</param>
    /// <param name="parameter">Caller-defined variable; we use it for a pointer to our list</param>
    /// <returns>True to continue enumerating, false to bail.</returns>
    public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

}