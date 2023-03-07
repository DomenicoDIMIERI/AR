Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    Public Class ChannelReload
        Inherits AsteriskEvent

       
        Public Sub New()
            MyBase.New("ChannelReload")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Channel As String
            Get
                Return Me.GetAttribute("Channel")
            End Get
        End Property

        Public ReadOnly Property ReloadReason As String
            Get
                Return Me.GetAttribute("ReloadReason")
            End Get
        End Property

        Public ReadOnly Property RegistryCount As String
            Get
                Return Me.GetAttribute("Registry_Count")
            End Get
        End Property

        Public ReadOnly Property PeerCount As String
            Get
                Return Me.GetAttribute("Peer_Count")
            End Get
        End Property

        Public ReadOnly Property UserCount As String
            Get
                Return Me.GetAttribute("User_Count")
            End Get
        End Property
         
    End Class

End Namespace