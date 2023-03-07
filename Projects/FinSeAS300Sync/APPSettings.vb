Imports System
Imports System.Diagnostics
Imports System.Runtime.InteropServices

Public Class APPSettings


    Private Const REGBASE As String = "Software\FSES300App\"

    Public Shared Property DeviceID As Integer
        Get
            Return GetRegValue("DeviceID", My.Settings.DeviceID)
        End Get
        Set(value As Integer)
            SetRegValue("DeviceID", value)
        End Set
    End Property

    Public Shared Property Address As String
        Get
            Return GetRegValue("Address", My.Settings.Address)
        End Get
        Set(value As String)
            SetRegValue("Address", value)
        End Set
    End Property

    Public Shared Property UserMapping As String
        Get
            Return GetRegValue("UserMapping", My.Settings.UserMapping)
        End Get
        Set(value As String)
            SetRegValue("UserMapping", value)
        End Set
    End Property

    Public Shared Property AutoCloseAfterNSeconds() As Integer
        Get
            Return GetRegValue("AutoCloseAfterNSeconds", 10)
        End Get
        Set(value As Integer)
            SetRegValue("AutoCloseAfterNSeconds", value)
        End Set
    End Property

    Public Shared Property AutoSyncTime As Integer
        Get
            Return GetRegValue("AutoSyncTime", My.Settings.AutoSyncTime)
        End Get
        Set(value As Integer)
            SetRegValue("AutoSyncTime", value)
        End Set
    End Property

    Public Shared Property UploadServer As String
        Get
            Return GetRegValue("UploadServer", My.Settings.UploadServer)
        End Get
        Set(value As String)
            SetRegValue("UploadServer", value)
        End Set
    End Property

    Public Shared Property AutoUploadTime As Integer
        Get
            Return GetRegValue("AutoUploadTime", My.Settings.AutoUploadTime)
        End Get
        Set(value As Integer)
            SetRegValue("AutoUploadTime", value)
        End Set
    End Property

    Public Shared Property LastSyncTime As Date
        Get
            Return GetRegValue("LastSyncTime", My.Settings.LastSyncTime)
        End Get
        Set(value As Date)
            SetRegValue("LastSyncTime", value)
        End Set
    End Property

    Public Shared Property LastUploadTime As Date
        Get
            Return GetRegValue("LastUploadTime", My.Settings.LastUploadTime)
        End Get
        Set(value As Date)
            SetRegValue("LastUploadTime", value)
        End Set
    End Property

    Public Shared Property LastCheckTime As Date
        Get
            Return GetRegValue("LastCheckTime", My.Settings.LastCheckTime)
        End Get
        Set(value As Date)
            SetRegValue("LastCheckTime", value)
        End Set
    End Property

    Public Shared Property CheckTimes As String
        Get
            Return GetRegValue("CheckTimes", My.Settings.CheckTimes)
        End Get
        Set(value As String)
            SetRegValue("CheckTimes", value)
        End Set
    End Property


    Private Shared Function GetRegValue(Of T)(ByVal name As String, Optional ByVal dVal As T = Nothing) As T
        Return CType(My.Computer.Registry.CurrentUser.GetValue(REGBASE & name, dVal), T)
    End Function

    Private Shared Sub SetRegValue(ByVal name As String, ByVal dVal As Object)
        My.Computer.Registry.CurrentUser.SetValue(REGBASE & name, dVal)
    End Sub



End Class
