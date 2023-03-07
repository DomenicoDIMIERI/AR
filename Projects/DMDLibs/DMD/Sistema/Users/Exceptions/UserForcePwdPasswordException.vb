Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Sistema


Public Class UserForcePwdPasswordException
    Inherits UserLoginException


    Public Sub New()
        Me.New(vbNullString, "Cambiamento password richiesto")
    End Sub

    Public Sub New(ByVal userName As String)
        Me.New(userName, "Cambiamento password richiesto")
    End Sub

    Public Sub New(ByVal userName As String, ByVal message As String)
        MyBase.New(userName, message)
    End Sub


End Class
