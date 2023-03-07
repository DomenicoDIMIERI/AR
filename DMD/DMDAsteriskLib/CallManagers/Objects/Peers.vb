Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers


    Public Class Peers
        Inherits System.Collections.ArrayList 'CSyncKeyCollection(Of Peer)

        Private m_Owner As AsteriskCallManager

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Overrides ReadOnly Property IsSynchronized As Boolean
            Get
                Return True
            End Get
        End Property

        Public Sub New(ByVal owner As AsteriskCallManager)
            Me.New
            Me.m_Owner = owner
        End Sub

        'Protected Overrides Sub OnInsert(index As Integer, value As Object)
        '    If (Me.m_Owner IsNot Nothing) Then DirectCast(value, Peer).SetOwner(Me.m_Owner)
        '    MyBase.OnInsert(index, value)
        'End Sub

        Public Shadows Sub Add(ByVal value As Peer)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, Peer).SetOwner(Me.m_Owner)
            MyBase.Add(value)
        End Sub

        Public Function GetItemByKey(ByVal name As String) As Peer
            SyncLock Me.SyncRoot
                For Each item As Peer In Me
                    If item.Name = name Then Return item
                Next
                Return Nothing
            End SyncLock
        End Function

        Protected Friend Sub UpdatePeerStatus(ByVal e As Events.PeerStatus)
            SyncLock Me.SyncRoot
                Dim peer As Peer = Me.GetItemByKey(e.Peer)
                If (peer Is Nothing) Then
                    peer = New Peer
                    peer.Name = e.Peer
                    Me.Add(peer) 'e.Peer) ', peer)
                End If
                peer.LastUpdated = Now
                peer.Status = e.PeerStatus
            End SyncLock
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace