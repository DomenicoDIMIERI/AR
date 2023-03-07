Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    Public Class AgentCalled
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("AgentCalled")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property AgentCalled As String
            Get
                Return Me.GetAttribute("AgentCalled")
            End Get
        End Property

        Public ReadOnly Property ChannelCalling As String
            Get
                Return Me.GetAttribute("ChannelCalling")
            End Get
        End Property

        Public ReadOnly Property CallerID As String
            Get
                Return Me.GetAttribute("CallerID")
            End Get
        End Property

        Public ReadOnly Property Context As String
            Get
                Return Me.GetAttribute("Context")
            End Get
        End Property

        Public ReadOnly Property Extension As String
            Get
                Return Me.GetAttribute("Extension")
            End Get
        End Property

        Public ReadOnly Property Priority As String
            Get
                Return Me.GetAttribute("Priority")
            End Get
        End Property
          
    End Class

End Namespace