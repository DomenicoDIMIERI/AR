Imports System
Imports System.Diagnostics
Imports System.Runtime.InteropServices

Public Class Settings

    ''' <summary>
    ''' Restituisce o imposta il server da cui viene scaricata la configurazione online
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property ConfigServer As String
        Get
            Return GetRegValue("ConfigServer", "")
        End Get
        Set(value As String)
            SetRegValue("ConfigServer", value)
        End Set
    End Property

    Public Shared Property AutoStart() As Boolean
        Get
            Return My.Settings.autostart
        End Get
        Set(value As Boolean)
            My.Settings.autostart = value
            My.Settings.Save()
        End Set
    End Property

    Public Shared Property RegisterDialtp As Boolean
        Get
            Return My.Settings.registerprotocol
        End Get
        Set(value As Boolean)
            My.Settings.registerprotocol = value
            My.Settings.Save()
        End Set
    End Property

    Shared Property SMTPLimitOutSent As Integer
        Get
            Return My.Settings.SMTPLimitOutSent
        End Get
        Set(value As Integer)
            My.Settings.SMTPLimitOutSent = value
            My.Settings.Save()
        End Set
    End Property

    Shared Property SMTPPassword As String
        Get
            Return GetRegValue("SMTPPassword", My.Settings.SMTPPassword)
        End Get
        Set(value As String)
            SetRegValue("SMTPPassword", value)
        End Set
    End Property

    Shared Property SMTPUserName As String
        Get
            Return GetRegValue("SMTPUserName", My.Settings.SMTPUserName)
        End Get
        Set(value As String)
            SetRegValue("SMTPUserName", value)
        End Set
    End Property

    Shared Property SMTPServer As String
        Get
            Return GetRegValue("SMTPServer", My.Settings.SMTPServer)
        End Get
        Set(value As String)
            SetRegValue("SMTPServer", value)
        End Set
    End Property

    Shared Property SMTPPort As Integer
        Get
            Return GetRegValue("SMTPPort", My.Settings.SMTPPort)
        End Get
        Set(value As Integer)
            SetRegValue("SMTPPort", value)
        End Set
    End Property

    Shared Property SMTPDisplayName As String
        Get
            Return GetRegValue("SMTPDisplayName", My.Settings.SMTPDisplayName)
        End Get
        Set(value As String)
            SetRegValue("SMTPDisplayName", value)
        End Set
    End Property

    Shared Property SMTPSubject As String
        Get
            Return GetRegValue("SMTPSubject", My.Settings.SMTPSubject)
        End Get
        Set(value As String)
            SetRegValue("SMTPSubject", value)
        End Set
    End Property

    Shared Property SMTPSSL As Boolean
        Get
            Return GetRegValue("SMTPServer", My.Settings.SMTPSSL)
        End Get
        Set(value As Boolean)
            SetRegValue("SMTPServer", value)
        End Set
    End Property

    Shared Property SMTPNotifyTo As String
        Get
            Return GetRegValue("SMTPNotifyTo", My.Settings.SMTPNotifyTo)
        End Get
        Set(value As String)
            SetRegValue("SMTPNotifyTo", value)
        End Set
    End Property

    Shared Property LogEvery As Integer
        Get
            Return GetRegValue("LogEvery", My.Settings.LogEvery)
        End Get
        Set(value As Integer)
            SetRegValue("LogEvery", value)
        End Set
    End Property

    

    Shared Property LastPrefix As String
        Get
            Return My.Settings.LastPrefix
        End Get
        Set(value As String)
            My.Settings.LastPrefix = value
            My.Settings.Save()
        End Set
    End Property

    Shared Property LastFaxPrefix As String
        Get
            Return My.Settings.LastFaxPrefix
        End Get
        Set(value As String)
            My.Settings.LastFaxPrefix = value
            My.Settings.Save()
        End Set
    End Property

    Shared Property LastDialerName As String
        Get
            Return My.Settings.LastDialerName
        End Get
        Set(value As String)
            My.Settings.LastDialerName = value
            My.Settings.Save()
        End Set
    End Property




    Shared Property Flags As Integer
        Get
            Return GetRegValue("Flags", My.Settings.Flags)
        End Get
        Set(value As Integer)
            SetRegValue("Flags", value)
        End Set
    End Property

     

    'Shared Property FoldersToExclude As String
    '    Get
    '        Return GetRegValue("FoldersToExclude", DIALTPLib.My.Settings.FoldersToExclude)
    '    End Get
    '    Set(value As String)
    '        SetRegValue("FoldersToExclude", value)
    '    End Set
    'End Property

    'Shared Property FoldersToWatch As String
    '    Get
    '        Return GetRegValue("FoldersToWatch", DIALTPLib.My.Settings.FoldersToWatch)
    '    End Get
    '    Set(value As String)
    '        SetRegValue("FoldersToWatch", value)
    '    End Set
    'End Property

    'Shared Sub Save()
    '    My.Settings.Save()
    'End Sub

    Public Shared Property ServersList() As String
        Get
            Return GetRegValue("ServersList", My.Settings.ServersList)
        End Get
        Set(value As String)
            SetRegValue("ServersList", value)
        End Set
    End Property





    Shared Property AsteriskServers() As String
        Get
            Return My.Settings.AsteriskServers
        End Get
        Set(value As String)
            My.Settings.AsteriskServers = value
            My.Settings.Save()
        End Set
    End Property

    Shared Property APPPassword() As String
        Get
            Return GetRegValue("APPPassword", My.Settings.APPPassword)
        End Get
        Set(value As String)
            SetRegValue("APPPassword", value)
        End Set
    End Property

    Private Shared Function GetRegValue(Of T)(ByVal name As String, Optional ByVal dVal As T = Nothing) As T
        Return CType(My.Computer.Registry.CurrentUser.GetValue("Software\DIALTP\" & name, dVal), T)
    End Function

    Private Shared Sub SetRegValue(ByVal name As String, ByVal dVal As Object)
        My.Computer.Registry.CurrentUser.SetValue("Software\DIALTP\" & name, dVal)
    End Sub

    Public Shared Property ProxyAddress As String
        Get
            Return GetRegValue(Of String)("ProxyAddress", "")
        End Get
        Set(value As String)
            SetRegValue("ProxyAddress", Strings.Trim(value))
        End Set
    End Property

    Public Shared Property ProxyPort As Integer
        Get
            Return GetRegValue(Of Integer)("ProxyPort", 8080)
        End Get
        Set(value As Integer)
            SetRegValue("ProxyPort", value)
        End Set
    End Property

    Public Shared Property WaveInDevName As String
        Get
            Return GetRegValue(Of String)("WaveInDevName", "")
        End Get
        Set(value As String)
            SetRegValue("WaveInDevName", value)
        End Set
    End Property

    Public Shared Property WaveInDisabled As Boolean
        Get
            Return GetRegValue(Of Boolean)("WaveInDisabled", False)
        End Get
        Set(value As Boolean)
            SetRegValue("WaveInDisabled", value)
        End Set
    End Property

    Public Shared Property WaveOutDevName As String
        Get
            Return GetRegValue(Of String)("WaveOutDevName", "")
        End Get
        Set(value As String)
            SetRegValue("WaveOutDevName", value)
        End Set
    End Property

    Public Shared Property WaveCodec As Integer
        Get
            Return GetRegValue(Of Integer)("WaveCodec", 0)
        End Get
        Set(value As Integer)
            SetRegValue("WaveCodec", value)
        End Set
    End Property
End Class
