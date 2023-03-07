Option Strict Off
Option Explicit On

Imports System.Runtime.InteropServices
Imports DMD.Internals
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Partial Public Class Nokia

    Public Enum NokiaFindFileFlags As Integer
        None = 0
        UseCache = CONA_FIND_USE_CACHE
    End Enum

    Public Class NokiaFindFileCursor
        Private m_Options As NokiaFileAttributes
        Private m_Device As NokiaDevice
        Private m_hFind As Integer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal device As NokiaDevice, ByVal folderName As String, ByVal options As NokiaFindFileFlags)
            Me.New()
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            Me.m_Device = device
            Me.m_Options = options

            Dim iResult As Integer = CONAFindBegin(device.FileSystem.GetHandle, Me.m_Options, Me.m_hFind, folderName)
            If iResult <> CONA_OK Then ShowErrorMessage("CONAFindBegin(): CONAFindEnd failed!", iResult)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class
