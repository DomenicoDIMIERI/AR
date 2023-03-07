Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Sistema




Public Class UserLoginEventArgs
    Inherits UserLogEventArgs

    Public Sub New()
    End Sub

    Public Sub New(ByVal user As CUser)
        Me.New(user, "Login di [" & user.UserName & "]")
    End Sub

    Public Sub New(ByVal user As CUser, ByVal message As String)
        MyBase.New(user, message)
    End Sub

End Class
