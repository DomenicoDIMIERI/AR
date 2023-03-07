Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Sockets
Imports System.Net.Security
Imports System.Security.Authentication
Imports FinSeA.Net.Mime

Namespace Net

    Public Class BadServerResponseException
        Inherits System.Exception

        Public Sub New()
        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(ByVal message As String, ByVal e As System.Exception)
            MyBase.New(message, e)
        End Sub
    End Class

End Namespace
