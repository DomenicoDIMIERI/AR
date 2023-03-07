Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    Public Class ZapShowChannels
        Inherits AsteriskEvent


        Public Sub New()
            MyBase.New("ZapShowChannels")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Channel As String
            Get
                Return Me.GetAttribute("Channel")
            End Get
        End Property

        Public ReadOnly Property Signalling As String
            Get
                Return Me.GetAttribute("Signalling")
            End Get
        End Property

        Public ReadOnly Property Context As String
            Get
                Return Me.GetAttribute("Context")
            End Get
        End Property

        Public ReadOnly Property DND As String
            Get
                Return Me.GetAttribute("DND")
            End Get
        End Property

        Public ReadOnly Property Alarm As String
            Get
                Return Me.GetAttribute("Alarm")
            End Get
        End Property
         
    End Class

End Namespace