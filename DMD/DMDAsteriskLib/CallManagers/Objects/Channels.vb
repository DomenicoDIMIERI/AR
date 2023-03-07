Namespace CallManagers

    Public Class Channels
        Inherits System.Collections.ArrayList ' DMD.CSyncKeyCollection(Of Channel)

        Private m_Owner As AsteriskCallManager

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Owner = Nothing
        End Sub

        Public Sub New(ByVal owner As AsteriskCallManager)
            Me.New
            Me.m_Owner = owner

        End Sub

        Public Overrides ReadOnly Property IsSynchronized As Boolean
            Get
                Return True
            End Get
        End Property

        'Protected Overrides Sub OnInsert(index As Integer, value As Object)
        '    If (Me.m_Owner IsNot Nothing) Then DirectCast(value, IDObject).SetOwner(Me.m_Owner)
        '    MyBase.OnInsert(index, value)
        'End Sub

        Friend Sub SetUpChannel(ByVal e As Events.Newchannel)
            Me.Add(New Channel(e))
        End Sub

        Private Function ContainsKey(ByVal key As String) As Boolean
            Return Me.IndexOf(key) >= 0
        End Function

        Default Public Shadows ReadOnly Property Item(ByVal index As Integer) As Channel
            Get
                Return MyBase.Item(index)
            End Get
        End Property

        Private Function IndexOfKey(ByVal key As String) As Boolean
            SyncLock Me.SyncRoot
                For i As Integer = 0 To Me.Count - 1
                    If Me(i).Channel = key Then Return i
                Next
                Return -1
            End SyncLock
        End Function

        Public Shadows Sub Add(ByVal value As Channel)
            If (Me.m_Owner IsNot Nothing) Then DirectCast(value, IDObject).SetOwner(Me.m_Owner)
            MyBase.Add(value)
        End Sub

        Friend Sub HangUpChannel(ByVal e As Events.HangupEvent)
            SyncLock Me.SyncRoot
                If Me.ContainsKey(e.Channel) = False Then Exit Sub
                Me(e.Channel).Hangup()
                Me.RemoveByKey(e.Channel)
            End SyncLock
        End Sub

        Public Sub RemoveByKey(ByVal key As String)
            SyncLock Me.SyncRoot
                Dim i As Integer = Me.IndexOfKey(key)
                If (i < 0) Then Throw New ArgumentOutOfRangeException
                Me.RemoveAt(i)
            End SyncLock
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace