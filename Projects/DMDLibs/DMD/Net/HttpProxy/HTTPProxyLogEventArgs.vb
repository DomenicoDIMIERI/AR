Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Net

Namespace Net.HTTPProxy

    Public Class HTTPProxyLogEventArgs
        Inherits System.EventArgs

        Private m_Message As String

        Public Sub New()
        End Sub

        Public Sub New(ByVal message As String)
            Me.m_Message = message
        End Sub

        Public ReadOnly Property Message As String
            Get
                Return Me.m_Message
            End Get
        End Property

    End Class

End Namespace
