Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Net

Namespace Net.HTTPProxy

    Public Class ProxyRequestEventArgs
        Inherits System.EventArgs

        Private m_Request As ProxyRequest
        Private m_Cancel As Boolean

        Public Sub New()
            Me.m_Request = Nothing
            Me.m_Cancel = False
        End Sub

        Public Sub New(ByVal req As ProxyRequest)
            If (req Is Nothing) Then Throw New ArgumentNullException("req")
            Me.m_Request = req
        End Sub

        ''' <summary>
        ''' Restituisce la richiesta 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Request As ProxyRequest
            Get
                Return Me.m_Request
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o impostau n valore booleano che indica se annullare la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cancel As Boolean
            Get
                Return Me.m_Cancel
            End Get
            Set(value As Boolean)
                Me.m_Cancel = value
            End Set
        End Property

    End Class

End Namespace
