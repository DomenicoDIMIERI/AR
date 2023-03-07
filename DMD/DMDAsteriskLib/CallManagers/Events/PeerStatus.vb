Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    ''' <summary>
    ''' Rappresenta un evento generico quando una oggetto cambia stato
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PeerStatus
        Inherits AsteriskEvent
         
        Public Sub New()
            MyBase.New("PeerStatus")
        End Sub

        Public Sub New(ByVal peer As String, ByVal peerStatus As String)
            MyBase.New("PeerStatus")
            Me.SetAttribute("Peer", peer)
            Me.SetAttribute("PeerStatus", peerStatus)
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        ''' <summary>
        ''' Restituisce l'oggetto che ha cambiato stato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Peer As String
            Get
                Return Me.GetAttribute("Peer")
            End Get
        End Property

        ''' <summary>
        ''' Restituisce lo stato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PeerStatus As String
            Get
                Return Me.GetAttribute("PeerStatus")
            End Get
        End Property

     
        Public ReadOnly Property Cause As String
            Get
                Return Me.GetAttribute("Cause")
            End Get
        End Property

        Public ReadOnly Property Time As String
            Get
                Return Me.GetAttribute("Time")
            End Get
        End Property
         
    End Class

End Namespace