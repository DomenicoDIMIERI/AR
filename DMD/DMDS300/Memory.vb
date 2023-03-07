'Imports Microsoft.VisualBasic.Compatibility.VB6;
Imports System
Imports System.Collections
Imports System.Data
Imports System.Diagnostics
Imports System.Runtime.InteropServices

Namespace S300

    Partial Public Class CKT_DLL

        'public  Declare Sub PCopyMemory Lib "kernel32"  Alias "RtlMoveMemory"(ByRef Destination As Any, ByVal Source As Integer, ByVal Length As Integer)
        <DllImport("kernel32", EntryPoint:="RtlMoveMemory", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=System.Runtime.InteropServices.CharSet.Ansi, SetLastError:=True)>
        Public Shared Sub PCopyMemory(ByRef Destination As Integer, ByVal Source As IntPtr, ByVal Length As Integer)

        End Sub

        <DllImport("kernel32", EntryPoint:="RtlMoveMemory", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=System.Runtime.InteropServices.CharSet.Ansi, SetLastError:=True)>
        Public Shared Sub PCopyMemory(ByRef Destination As Byte, ByVal Source As Integer, ByVal Length As Integer)

        End Sub

        <DllImport("kernel32", EntryPoint:="RtlMoveMemory", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=System.Runtime.InteropServices.CharSet.Ansi, SetLastError:=True)>
        Public Shared Sub PCopyMemory(ByRef Destination As CKT_DLL.PERSONINFO, ByVal Source As Integer, ByVal Length As Integer)

        End Sub

        <DllImport("kernel32", EntryPoint:="RtlMoveMemory", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=System.Runtime.InteropServices.CharSet.Ansi, SetLastError:=True)>
        Public Shared Sub PCopyMemory(ByRef Destination As CKT_DLL.CLOCKINGRECORD, ByVal Source As Integer, ByVal Length As Integer)

        End Sub

        <DllImport("kernel32", EntryPoint:="RtlMoveMemory", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=System.Runtime.InteropServices.CharSet.Ansi, SetLastError:=True)>
        Public Shared Sub PCopyMemory(ByRef Destination As CKT_DLL.NETINFO, ByVal Source As Integer, ByVal Length As Integer)

        End Sub

        <DllImport("kernel32", EntryPoint:="RtlMoveMemory", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=System.Runtime.InteropServices.CharSet.Ansi, SetLastError:=True)>
        Public Shared Sub PCopyMemory(ByRef Destination As CKT_DLL.RINGTIME, ByVal Source As Integer, ByVal Length As Integer)

        End Sub

        <DllImport("kernel32", EntryPoint:="RtlMoveMemory", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=System.Runtime.InteropServices.CharSet.Ansi, SetLastError:=True)>
        Public Shared Sub PCopyMemory(ByRef Destination As CKT_DLL.TIMESECT, ByVal Source As Integer, ByVal Length As Integer)

        End Sub

        <DllImport("kernel32", EntryPoint:="RtlMoveMemory", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=System.Runtime.InteropServices.CharSet.Ansi, SetLastError:=True)>
        Public Shared Sub PCopyMemory(ByVal Source As Integer, ByRef Destination As CKT_DLL.TIMESECT, ByVal Length As Integer)

        End Sub

        <DllImport("kernel32", EntryPoint:="RtlMoveMemory", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=System.Runtime.InteropServices.CharSet.Ansi, SetLastError:=True)>
        Public Shared Sub PPCopyMemory(ByVal Destination As Integer, ByVal Source As Integer, ByVal Length As Integer)

        End Sub

        <DllImport("kernel32", EntryPoint:="Sleep", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=System.Runtime.InteropServices.CharSet.Ansi, SetLastError:=True)>
        Public Shared Sub Sleep(ByVal dwMilliseconds As Integer)

        End Sub

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SYSTEMTIME
            Public wYear As Short
            Public wMonth As Short
            Public wDayOfWeek As Short
            Public wDay As Short
            Public wHour As Short
            Public wMinute As Short
            Public wSecond As Short
            Public wMilliseco1nds As Short
        End Structure


        <DllImport("kernel32", EntryPoint:="GetLocalTime", CallingConvention:=CallingConvention.StdCall, ExactSpelling:=True, CharSet:=System.Runtime.InteropServices.CharSet.Ansi, SetLastError:=True)>
        Public Shared Sub GetLocalTime(ByRef lpSystemTime As SYSTEMTIME)

        End Sub


    End Class


End Namespace
