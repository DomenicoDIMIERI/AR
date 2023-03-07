Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Databases

Partial Class Office

    Public Class EmailEventArg
        Inherits System.EventArgs

        Private m_Message As MailMessage

        Public Sub New()
        End Sub

        Public Sub New(ByVal msg As MailMessage)
            If (msg Is Nothing) Then Throw New ArgumentNullException("msg")
            Me.m_Message = msg
        End Sub

        Public ReadOnly Property Message As MailMessage
            Get
                Return Me.m_Message
            End Get
        End Property



    End Class

End Class