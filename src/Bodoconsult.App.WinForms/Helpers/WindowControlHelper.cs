// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System.Runtime.InteropServices;

namespace Bodoconsult.App.WinForms.Helpers;

/// <summary>
/// Helper class for controlling windows
/// </summary>
public static class WindowControlHelper
{
    //private const int WmSetRedraw = 11;
    private const int WmSetRedraw = 11;
    private const int WmUser = 0x400;
    private const int EmGetEventMask = WmUser + 59;
    private const int EmSetEventMask = WmUser + 69;

    /// <summary>
    /// Send a message to a window
    /// </summary>
    /// <param name="hWnd">Window pointer</param>
    /// <param name="wMsg">Message to send</param>
    /// <param name="wParam">W parameter</param>
    /// <param name="lParam">L parameter</param>
    /// <returns>Return code</returns>
    [DllImport("user32.dll")]
    private static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

    /// <summary>
    /// Enum the child windows of a window
    /// </summary>
    /// <param name="window">Window pointer</param>
    /// <param name="callback">Callback proc</param>
    /// <param name="i">Parameter pointer given to callback proc</param>
    /// <returns>Not used</returns>
    [DllImport("user32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool EnumChildWindows(IntPtr window, EnumWindowProcDelegate callback, IntPtr i);

    /// <summary>
    /// Retrieves a handle to the top-level window whose class name or window title match the specified strings
    /// </summary>
    /// <param name="className">Class name or null</param>
    /// <param name="windowTitle">Window title or null</param>
    /// <returns>Window pointer</returns>
    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string className, string windowTitle);

    /// <summary>
    /// Suspend the drawing of a window
    /// </summary>
    /// <param name="parent">Control in the window the drawing should be suspended</param>
    public static void SuspendDrawing(Control parent)
    {
        //SendMessage(parent.Handle, WmSetRedraw, false, 0);

        var msgSuspendUpdate = Message.Create(parent.Handle, WmSetRedraw, IntPtr.Zero, IntPtr.Zero);

        var window = NativeWindow.FromHandle(parent.Handle);

        window?.DefWndProc(ref msgSuspendUpdate);
    }

    /// <summary>
    /// Suspend the drawing of a window
    /// </summary>
    /// <param name="window">Window the drawing should be suspended</param>
    public static void SuspendDrawing(IntPtr window)
    {
        SendMessage(window, WmSetRedraw, 0, 0);

        //var msgSuspendUpdate = Message.Create(window, WmSetRedraw, IntPtr.Zero,
        //IntPtr.Zero);

        //var w = NativeWindow.FromHandle(window);
        //w.DefWndProc(ref msgSuspendUpdate);
    }

    /// <summary>
    /// Resume the drawing of a window
    /// </summary>
    /// <param name="parent">Control in the window the drawing should be resumed</param>
    public static void ResumeDrawing(Control parent)
    {
        // Create a C "true" boolean as an IntPtr
        var wparam = new IntPtr(1);
        var msgResumeUpdate = Message.Create(parent.Handle, WmSetRedraw, wparam,
            IntPtr.Zero);

        var window = NativeWindow.FromHandle(parent.Handle);

        if (window is null)
        {
            parent.Invalidate();
            return;
        }

        window.DefWndProc(ref msgResumeUpdate);
        parent.Invalidate();
    }

    /// <summary>
    /// Resume the drawing of a window
    /// </summary>
    /// <param name="window">The window the drawing should be resumed</param>
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

    /// <summary>
    /// Stop event handling
    /// </summary>
    /// <param name="window">Window pointer</param>
    /// <returns></returns>
    public static IntPtr StopEvents(IntPtr window)
    {
        return SendMessage(window, EmGetEventMask, 0, 0);
    }

    /// <summary>
    /// Start event handling
    /// </summary>
    /// <param name="window">Window pointer</param>
    /// <param name="eventMask">Event mask</param>
    public static void StartEvents(IntPtr window, IntPtr eventMask)
    {
        SendMessage(window, EmSetEventMask, 0, eventMask.ToInt32());
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
            var childProc = new EnumWindowProcDelegate(EnumWindow);
            EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
        }
        finally
        {
            if (listHandle.IsAllocated)
            {
                listHandle.Free();
            }
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
        if (gch.Target is not List<IntPtr> list)
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
    public delegate bool EnumWindowProcDelegate(IntPtr hWnd, IntPtr parameter);
}