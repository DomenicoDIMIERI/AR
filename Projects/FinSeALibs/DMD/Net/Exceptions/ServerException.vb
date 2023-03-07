Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Sockets
Imports System.Net.Security
Imports System.Security.Authentication
Imports DMD.Net.Mime

Namespace Net

    Public Class ServerException
        Inherits System.Exception

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal message As String, ByVal e As System.Exception)
            MyBase.New(message, e)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace
