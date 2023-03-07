Imports System
Imports System.Diagnostics
Imports System.Runtime.InteropServices

Public Class FaxSvrSettings

#Region "HylaFax"

    Public Shared Property HylaFaxServerName As String
        Get
            Return GetRegValue("HylaFaxServerName", "192.168.6.254")
        End Get
        Set(value As String)
            SetRegValue("HylaFaxServerName", value)
        End Set
    End Property

    Public Shared Property HylaFaxServerPort As Integer
        Get
            Return GetRegValue("HylaFaxServerPort", 4559)
        End Get
        Set(value As Integer)
            SetRegValue("HylaFaxServerPort", value)
        End Set
    End Property

    Public Shared Property HylaFaxUserName As String
        Get
            Return GetRegValue("HylaFaxUserName", "root")
        End Get
        Set(value As String)
            SetRegValue("HylaFaxUserName", value)
        End Set
    End Property

    Public Shared Property HylafaxPrefix As String
        Get
            Return GetRegValue("HylafaxPrefix", "9")
        End Get
        Set(value As String)
            SetRegValue("HylafaxPrefix", value)
        End Set
    End Property

    Public Shared Property HylaFaxPassword As String
        Get
            Return GetRegValue("HylaFaxPassword", "")
        End Get
        Set(value As String)
            SetRegValue("HylaFaxPassword", value)
        End Set
    End Property


#End Region

#Region "SMTP"

    Public Shared Property SMTPServer As String
        Get
            Return GetRegValue("SMTPServer", "smtp.DMD.net")
        End Get
        Set(value As String)
            SetRegValue("SMTPServer", value)
        End Set
    End Property

    Public Shared Property SMTPDisplayName As String
        Get
            Return GetRegValue("SMTPDisplayName", "")
        End Get
        Set(value As String)
            SetRegValue("SMTPDisplayName", value)
        End Set
    End Property

    Public Shared Property SMTPPassword As String
        Get
            Return GetRegValue("SMTPPassword", "fax.server1")
        End Get
        Set(value As String)
            SetRegValue("SMTPPassword", value)
        End Set
    End Property

    Public Shared Property SMTPPort As Integer
        Get
            Return GetRegValue("SMTPPort", 25)
        End Get
        Set(value As Integer)
            SetRegValue("SMTPPort", value)
        End Set
    End Property

    Public Shared Property SMTPSSL As Boolean
        Get
            Return GetRegValue("SMTPSSL", False)
        End Get
        Set(value As Boolean)
            SetRegValue("SMTPSSL", value)
        End Set
    End Property

    Public Shared Property SMTPSubject As String
        Get
            Return GetRegValue("SMTPSubject", "")
        End Get
        Set(value As String)
            SetRegValue("SMTPSubject", value)
        End Set
    End Property

    Public Shared Property SMTPUserName As String
        Get
            Return GetRegValue("SMTPUserName", "faxserver@DMD.net")
        End Get
        Set(value As String)
            SetRegValue("SMTPUserName", value)
        End Set
    End Property


#End Region

#Region "POP3"


    Public Shared Property POP3Server As String
        Get
            Return GetRegValue("POP3Server", "pop3.DMD.net")
        End Get
        Set(value As String)
            SetRegValue("POP3Server", value)
        End Set
    End Property

    Public Shared Property POP3Port As Integer
        Get
            Return GetRegValue("POP3Port", 110)
        End Get
        Set(value As Integer)
            SetRegValue("POP3Port", value)
        End Set
    End Property

    Public Shared Property POP3UserName As String
        Get
            Return GetRegValue("POP3UserName", "faxserver@DMD.net")
        End Get
        Set(value As String)
            SetRegValue("POP3UserName", value)
        End Set
    End Property

    Public Shared Property POP3Password As String
        Get
            Return GetRegValue("POP3Password", "fax.server1")
        End Get
        Set(value As String)
            SetRegValue("POP3Password", value)
        End Set
    End Property

    Public Shared Property POP3SSL As Boolean
        Get
            Return GetRegValue("POP3SSL", False)
        End Get
        Set(value As Boolean)
            SetRegValue("POP3SSL", value)
        End Set
    End Property

    Public Shared Property POP3CheckEvery As Integer
        Get
            Return GetRegValue("POP3CheckEvery", 1)
        End Get
        Set(value As Integer)
            SetRegValue("POP3CheckEvery", value)
        End Set
    End Property

#End Region



    Private Shared Function GetRegValue(Of T)(ByVal name As String, Optional ByVal dVal As T = Nothing) As T
        Return CType(My.Computer.Registry.CurrentUser.GetValue("Software\DIALTP\" & name, dVal), T)
    End Function

    Private Shared Sub SetRegValue(ByVal name As String, ByVal dVal As Object)
        My.Computer.Registry.CurrentUser.SetValue("Software\DIALTP\" & name, dVal)
    End Sub

End Class
