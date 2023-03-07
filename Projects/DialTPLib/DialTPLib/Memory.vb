Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Drawing
Imports System.Threading


Public Class Memory


    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
    Public Structure MEMORYSTATUSEX
        Public dwLength As Int32
        Public dwMemoryLoad As UInt32
        Public ullTotalPhys As UInt64
        Public ullAvailPhys As UInt64
        Public ullTotalPageFile As UInt64
        Public ullAvailPageFile As UInt64
        Public ullTotalVirtual As UInt64
        Public ullAvailVirtual As UInt64
        Public ullAvailExtendedVirtual As UInt64

        'Public Sub New()
        'this.dwLength = (uint)Marshal.SizeOf(typeof(NativeMethods.MEMORYSTATUSEX));
    End Structure


    '<return: MarshalAs(UnmanagedType.Bool)> _
    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Shared Function GlobalMemoryStatusEx(ByRef lpBuffer As MEMORYSTATUSEX) As <MarshalAs(UnmanagedType.Bool)> Boolean

    End Function

    'Then use like:

    'ulong installedMemory;
    '    Private memStatus As MEMORYSTATUSEX ' new MEMORYSTATUSEX();
    '        If (GlobalMemoryStatusEx(memStatus)) Then
    '{ 
    '   installedMemory = memStatus.ullTotalPhys;
    'end Function

    Public Shared Function GetMemoryStatus() As MEMORYSTATUSEX
        Dim ret As New MEMORYSTATUSEX
        ret.dwLength = Marshal.SizeOf(GetType(MEMORYSTATUSEX))
        GlobalMemoryStatusEx(ret)
        Return ret
    End Function

End Class

