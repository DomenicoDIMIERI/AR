Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Sistema


Public Class BadPasswordException
    Inherits UserLoginException

    Private m_BadPassword As String

    Public Sub New()
        Me.New(vbNullString, "Password non corrispondente", "")
    End Sub

    Public Sub New(ByVal userName As String)
        Me.New(userName, "[" & userName & "] Password non corrispondente", "")
    End Sub

    Public Sub New(ByVal userName As String, ByVal message As String, ByVal pwd As String)
        MyBase.New(userName, message)
        Me.m_BadPassword = pwd
    End Sub

    Public ReadOnly Property BadPassword As String
        Get
            Return Me.m_BadPassword
        End Get
    End Property

End Class
