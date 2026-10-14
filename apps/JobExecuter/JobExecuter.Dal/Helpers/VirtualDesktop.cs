// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH.  All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace JobExecuter.Dal.Helpers;

/*

 * VirtualDesktop.cs - A desktop creation/deletion/switching class for Windows.
  * Copyright (c) 2013, 2014, 2016 Eyal Kalderon
 * https://nullpwd.wordpress.com/2013/05/25/writing-a-virtual-desktop-system-in-cs-with-winapi/
 * The MIT License (MIT)
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.

 */

/// <summary>
/// Provides a virtual desktop
/// </summary>
public class VirtualDesktop : IDisposable
{

    /// <summary>
    /// These security descriptors below are required to let us manipulate the desktop objects.
    /// </summary>
    internal enum DESKTOP_ACCESS_MASK : uint
    {
        DESKTOP_NONE = 0,
        DESKTOP_READOBJECTS = 0x0001,
        DESKTOP_CREATEWINDOW = 0x0002,
        DESKTOP_CREATEMENU = 0x0004,
        DESKTOP_HOOKCONTROL = 0x0008,
        DESKTOP_JOURNALRECORD = 0x0010,
        DESKTOP_JOURNALPLAYBACK = 0x0020,
        DESKTOP_ENUMERATE = 0x0040,
        DESKTOP_WRITEOBJECTS = 0x0080,
        DESKTOP_SWITCHDESKTOP = 0x0100,

        EVERYTHING = (DESKTOP_READOBJECTS | DESKTOP_CREATEWINDOW | DESKTOP_CREATEMENU |
                      DESKTOP_HOOKCONTROL | DESKTOP_JOURNALRECORD | DESKTOP_JOURNALPLAYBACK |
                      DESKTOP_ENUMERATE | DESKTOP_WRITEOBJECTS | DESKTOP_SWITCHDESKTOP),
    }

    #region Variables

    /// <summary>
    /// This will point to the current desktop we are using.
    /// </summary>
    public IntPtr DesktopPtr;
        
    /// <summary>
    /// This will hold the name for the desktop object we created.
    /// </summary>
    private string _sMyDesk; 
        
    /// <summary>
    /// // This will remember the very first desktop we spawned on.
    /// </summary>
    IntPtr _hOrigDesktop; 

    #endregion

    #region DLL Definitions

    /// <summary>
    /// Close the desktop
    /// </summary>
    /// <param name="handle"></param>
    /// <returns></returns>
    [DllImport("user32.dll", EntryPoint = "CloseDesktop", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CloseDesktop(IntPtr handle);


    /// <summary>
    /// Create a desktop session
    /// </summary>
    /// <param name="lpszDesktop"></param>
    /// <param name="lpszDevice"></param>
    /// <param name="pDevmode"></param>
    /// <param name="dwFlags"></param>
    /// <param name="dwDesiredAccess"></param>
    /// <param name="lpsa"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    private static extern IntPtr CreateDesktop(string lpszDesktop, IntPtr lpszDevice, IntPtr pDevmode,
        int dwFlags, long dwDesiredAccess, IntPtr lpsa);

    /// <summary>
    /// Get the ID of the current thread
    /// </summary>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    public static extern int GetCurrentThreadId();

    /// <summary>
    /// Get the desktop thread as pointer
    /// </summary>
    /// <param name="dwThreadId"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern IntPtr GetThreadDesktop(int dwThreadId);

    /// <summary>
    /// Set a thread as desktop
    /// </summary>
    /// <param name="hDesktop">Pointer to the desktop</param>
    /// <returns>True on success else false</returns>
    [DllImport("user32.dll")]
    public static extern bool SetThreadDesktop(IntPtr hDesktop);

    /// <summary>
    /// Swicth to another desktop
    /// </summary>
    /// <param name="hDesktop">Pointer of the desktop to switch to</param>
    /// <returns>True on success else false</returns>
    [DllImport("user32.dll")]
    private static extern bool SwitchDesktop(IntPtr hDesktop);

    #endregion

    #region Destructors

      
    /// <summary>
    /// Switch to the desktop we were on before.
    /// </summary>
    public void Dispose()
    {
        SwitchToOrginal();
        ((IDisposable)this).Dispose();
    }

    /// <summary>
    /// Delete our custom one.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            CloseDesktop(DesktopPtr);
        }
    }


    /// <summary>
    /// ... flush!
    /// </summary>
    void IDisposable.Dispose()
    {
        Dispose(true);

        // This takes the already destroyed desktop off the finalization queue so the GC
        // doesn’t call the finalization code twice.
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get the pointer for the desktop thread
    /// </summary>
    /// <returns></returns>
    public IntPtr GetCurrentDesktopPtr()
    {
        return GetThreadDesktop(GetCurrentThreadId());
    }

    /// <summary>
    /// Launch a desktop session
    /// </summary>
    /// <returns>Pointer to the desktop session</returns>
    private IntPtr LaunchDesktop()
    {
        return CreateDesktop(_sMyDesk, IntPtr.Zero, IntPtr.Zero,
            0, (long)DESKTOP_ACCESS_MASK.EVERYTHING, IntPtr.Zero);
    }

    /// <summary>
    /// Show the desktop
    /// </summary>
    public void ShowDesktop()
    {
        SetThreadDesktop(DesktopPtr);
        SwitchDesktop(DesktopPtr);
    }

    /// <summary>
    /// Switch to the orginial desktop
    /// </summary>
    public void SwitchToOrginal()
    {
        SwitchDesktop(_hOrigDesktop);
        SetThreadDesktop(_hOrigDesktop);
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Ctor
    /// </summary>
    public VirtualDesktop()
    {
        _sMyDesk = "";
    }

    /// <summary>
    /// Ctor with desktop name
    /// </summary>
    /// <param name="sDesktopName"></param>
    public VirtualDesktop(string sDesktopName)
    {
        _hOrigDesktop = GetCurrentDesktopPtr();
        _sMyDesk = sDesktopName;
        DesktopPtr = LaunchDesktop();
    }

    #endregion
}