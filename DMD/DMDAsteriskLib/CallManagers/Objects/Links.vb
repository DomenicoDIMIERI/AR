Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers


    Public Class Links
        Inherits System.Collections.ArrayList '(Of Link)

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

        Public ReadOnly Property Owner As AsteriskCallManager
            Get
                Return Me.m_Owner
            End Get
        End Property

        'Protected Overrides Sub OnInsert(index As Integer, value As Object)
        '    If (Me.m_Owner IsNot Nothing) Then DirectCast(value, Link).SetOwner(Me.m_Owner)
        '    MyBase.OnInsert(index, value)
        'End Sub

        Protected Friend Sub Update(ByVal e As AsteriskEvent)
            SyncLock Me.SyncRoot
                'Dim link As Link
                If (TypeOf (e) Is Events.Link) Then
                    Dim e1 As Events.Link = e
                    'link = New Link
                    'link.Channel1 = Me.Owner.Channels(e1.Channel1)
                    'link.Channel2 = Me.Owner.Channels(e1.Channel2)
                    'Me.Add(link)
                    'Me.Owner.NotifyLink(link)
                ElseIf (TypeOf (e) Is Events.Unlink) Then
                    Dim e1 As Events.Unlink = e
                    Dim j As Integer = -1
                    For i As Integer = 0 To Me.Count - 1
                        If Me(i).Channel1.Channel = e1.Channel1 AndAlso Me(i).Channel2.Channel = e1.Channel2 Then
                            j = i
                            Exit For
                        End If
                    Next
                    If (j >= 0) Then Me.RemoveAt(j)
                End If
            End SyncLock
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace