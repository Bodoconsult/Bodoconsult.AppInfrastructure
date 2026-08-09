// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.
// Copyright https://social.msdn.microsoft.com/Forums/en-US/6c8d3529-a493-49b9-93d7-07a3a2d715dc/the-simplest-way-to-generate-minidump-for-mixed-managed-amp-unmanaged-stack?forum=clr
// See http://voneinem-windbg.blogspot.com/2007/03/creating-and-analyzing-minidumps-in-net.html for analyzing dump files

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Bodoconsult.App.General;

/// <summary>
/// Creates a mini dump of an app
/// </summary>
public static class MiniDump
{
    /// <summary>
    /// Type of mini dump to create
    /// </summary>
    public enum MiniDumpTypeEnum
    {
        /// <summary>
        /// MiniDumpNormal 
        /// </summary>
        MiniDumpNormal = 0x00000000,
        /// <summary>
        /// MiniDumpWithDataSegs
        /// </summary>
        MiniDumpWithDataSegs = 0x00000001,
        /// <summary>
        /// MiniDumpWithFullMemory
        /// </summary>
        MiniDumpWithFullMemory = 0x00000002,
        /// <summary>
        /// MiniDumpWithHandleData
        /// </summary>
        MiniDumpWithHandleData = 0x00000004,
        /// <summary>
        /// MiniDumpFilterMemory
        /// </summary>
        MiniDumpFilterMemory = 0x00000008,
        /// <summary>
        /// MiniDumpScanMemory
        /// </summary>
        MiniDumpScanMemory = 0x00000010,
        /// <summary>
        /// MiniDumpWithUnloadedModules
        /// </summary>
        MiniDumpWithUnloadedModules = 0x00000020,
        /// <summary>
        /// MiniDumpWithIndirectlyReferencedMemory
        /// </summary>
         MiniDumpWithIndirectlyReferencedMemory= 0x00000040,
        /// <summary>
        /// MiniDumpFilterModulePaths 
        /// </summary>
        MiniDumpFilterModulePaths = 0x00000080,
        /// <summary>
        /// MiniDumpWithProcessThreadData 
        /// </summary>
        MiniDumpWithProcessThreadData = 0x00000100,
        /// <summary>
        /// MiniDumpWithPrivateReadWriteMemory
        /// </summary>
        MiniDumpWithPrivateReadWriteMemory = 0x00000200,
        /// <summary>
        /// MiniDumpWithoutOptionalData
        /// </summary>
        MiniDumpWithoutOptionalData = 0x00000400,
        /// <summary>
        /// MiniDumpWithFullMemoryInfo
        /// </summary>
        MiniDumpWithFullMemoryInfo = 0x00000800,
        /// <summary>
        /// MiniDumpWithThreadInfo
        /// </summary>
        MiniDumpWithThreadInfo = 0x00001000,
        /// <summary>
        /// MiniDumpWithCodeSegs
        /// </summary>
        MiniDumpWithCodeSegs = 0x00002000
    }

    [DllImport( "dbghelp.dll" )]
    private static extern bool MiniDumpWriteDump (
        IntPtr hProcess,
        int processId,
        IntPtr hFile,
        MiniDumpTypeEnum dumpType,
        IntPtr exceptionParam,
        IntPtr userStreamParam,
        IntPtr callackParam );

    /// <summary>
    /// Create a mini dump file
    /// </summary>
    /// <param name="fileToDump">Full path to the min dump file</param>
    /// <param name="dumpType">Mini dump type. Default: MiniDumpTypeEnum.MiniDumpNormal </param>
    public static void MiniDumpToFile ( string fileToDump, MiniDumpTypeEnum dumpType = MiniDumpTypeEnum.MiniDumpNormal )
    {
        var fsToDump = File.Exists(fileToDump) ? File.Open( fileToDump, FileMode.Append ) : File.Create( fileToDump );

        if (fsToDump.SafeFileHandle is null)
        {
            return;
        }

        var thisProcess = Process.GetCurrentProcess( );
        MiniDumpWriteDump( thisProcess.Handle, thisProcess.Id,
            fsToDump.SafeFileHandle.DangerousGetHandle( ), dumpType,
            IntPtr.Zero, IntPtr.Zero, IntPtr.Zero );
        fsToDump.Close( );
    }

}