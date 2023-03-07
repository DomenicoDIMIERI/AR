Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Databases
Imports DMD

Partial Class Office

    Public Class MailAccountCursor
        Inherits DBObjectCursorPO(Of MailAccount)

        Private m_AccountName As New CCursorFieldObj(Of String)("AccountName")
        Private m_DefaultFolderID As New CCursorField(Of Integer)("DefaultFolderID")
        Private m_UserName As New CCursorFieldObj(Of String)("UserName")
        Private m_Password As New CCursorFieldObj(Of String)("Password")
        Private m_ServerName As New CCursorFieldObj(Of String)("ServerName")
        Private m_ServerPort As New CCursorField(Of Integer)("ServerPort")
        Private m_eMailAddress As New CCursorFieldObj(Of String)("eMailAddress")
        Private m_Protocol As New CCursorFieldObj(Of String)("Protocol")
        Private m_UseSSL As New CCursorField(Of Boolean)("UseSSL")
        Private m_SMTPServerName As New CCursorFieldObj(Of String)("SMTPServerName")
        Private m_SMTPPort As New CCursorField(Of Integer)("SMTPPort")
        Private m_ReplayTo As New CCursorFieldObj(Of String)("ReplayTo")
        Private m_DisplayName As New CCursorFieldObj(Of String)("DisplayName")
        Private m_SMTPUserName As New CCursorFieldObj(Of String)("SMTPUserName")
        Private m_SMTPPassword As New CCursorFieldObj(Of String)("SMTPPassword")
        Private m_PopBeforeSMPT As New CCursorField(Of Boolean)("PopBeforeSMTP")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_DelServerAfterDays As New CCursorField(Of Integer)("DelAfterDays")
        Private m_TimeOut As New CCursorField(Of Integer)("TimeOut")
        Private m_SMTPCrittografia As New CCursorField(Of SMTPTipoCrittografica)("SMTPCrittografia")
        Private m_LastSync As New CCursorField(Of Date)("LastSync")
        Private m_FirmaPerNuoviMessaggi As New CCursorFieldObj(Of String)("FirmaPerNuoviMessaggi")
        Private m_FirmaPerRisposte As New CCursorFieldObj(Of String)("FirmaPerRisposte")
        Private m_ApplicationID As New CCursorField(Of Integer)("ApplicationID")

        Public Sub New()
        End Sub

        Public Overrides Function GetTableName() As String
            Return "tbl_EmailAccounts"
        End Function

        Protected Overrides Function GetModule() As Sistema.CModule
            Return Office.Mails.Accounts.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Mails.Database
        End Function

        Public ReadOnly Property ApplicationID As CCursorField(Of Integer)
            Get
                Return Me.m_ApplicationID
            End Get
        End Property

        Public ReadOnly Property FirmaPerNuoviMessaggi As CCursorFieldObj(Of String)
            Get
                Return Me.m_FirmaPerNuoviMessaggi
            End Get
        End Property

        Public ReadOnly Property FirmaPerRisposte As CCursorFieldObj(Of String)
            Get
                Return Me.m_FirmaPerRisposte
            End Get
        End Property


        Public ReadOnly Property LastSync As CCursorField(Of Date)
            Get
                Return Me.m_LastSync
            End Get
        End Property

        Public ReadOnly Property SMTPCrittografia As CCursorField(Of SMTPTipoCrittografica)
            Get
                Return Me.m_SMTPCrittografia
            End Get
        End Property

        Public ReadOnly Property TimeOut As CCursorField(Of Integer)
            Get
                Return Me.m_TimeOut
            End Get
        End Property

        Public ReadOnly Property AccountName As CCursorFieldObj(Of String)
            Get
                Return Me.m_AccountName
            End Get
        End Property

        Public ReadOnly Property DefaultFolderID As CCursorField(Of Integer)
            Get
                Return Me.m_DefaultFolderID
            End Get
        End Property

        Public ReadOnly Property UserName As CCursorFieldObj(Of String)
            Get
                Return Me.m_UserName
            End Get
        End Property

        Public ReadOnly Property Password As CCursorFieldObj(Of String)
            Get
                Return Me.m_Password
            End Get
        End Property

        Public ReadOnly Property ServerName As CCursorFieldObj(Of String)
            Get
                Return Me.m_ServerName
            End Get
        End Property

        Public ReadOnly Property ServerPort As CCursorField(Of Integer)
            Get
                Return Me.m_ServerPort
            End Get
        End Property

        Public ReadOnly Property eMailAddress As CCursorFieldObj(Of String)
            Get
                Return Me.m_eMailAddress
            End Get
        End Property

        Public ReadOnly Property Protocol As CCursorFieldObj(Of String)
            Get
                Return Me.m_Protocol
            End Get
        End Property

        Public ReadOnly Property UseSSL As CCursorField(Of Boolean)
            Get
                Return Me.m_UseSSL
            End Get
        End Property

        Public ReadOnly Property SMTPServerName As CCursorFieldObj(Of String)
            Get
                Return Me.m_SMTPServerName
            End Get
        End Property

        Public ReadOnly Property SMTPPort As CCursorField(Of Integer)
            Get
                Return Me.m_SMTPPort
            End Get
        End Property

        Public ReadOnly Property ReplayTo As CCursorFieldObj(Of String)
            Get
                Return Me.m_ReplayTo
            End Get
        End Property

        Public ReadOnly Property DisplayName As CCursorFieldObj(Of String)
            Get
                Return Me.m_DisplayName
            End Get
        End Property

        Public ReadOnly Property SMTPUserName As CCursorFieldObj(Of String)
            Get
                Return Me.m_SMTPUserName
            End Get
        End Property

        Public ReadOnly Property SMTPPassword As CCursorFieldObj(Of String)
            Get
                Return Me.m_SMTPPassword
            End Get
        End Property

        Public ReadOnly Property PopBeforeSMPT As CCursorField(Of Boolean)
            Get
                Return Me.m_PopBeforeSMPT
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property DelServerAfterDays As CCursorField(Of Integer)
            Get
                Return Me.m_DelServerAfterDays
            End Get
        End Property


    End Class

End Class
