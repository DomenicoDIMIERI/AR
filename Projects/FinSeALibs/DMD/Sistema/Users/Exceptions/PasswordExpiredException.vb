Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Sistema


Public Class PasswordExpiredException
    Inherits UserLoginException

    Private m_ExpireDate As DateTime?

    Public Sub New()
        Me.New(vbNullString, "Password scaduta", Nothing)
    End Sub

    Public Sub New(ByVal userName As String)
        Me.New(userName, "[" & userName & "] Password scaduta", Nothing)
    End Sub

    Public Sub New(ByVal userName As String, ByVal message As String, ByVal expireDate As Date?)
        MyBase.New(userName, message)
        Me.m_ExpireDate = expireDate
    End Sub

    Public ReadOnly Property ExpireDate As Date?
        Get
            Return Me.m_ExpireDate
        End Get
    End Property

End Class
