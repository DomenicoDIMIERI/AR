Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    Public Class ActionResponseQueues
        'Inherits System.Collections.Specialized.NameValueCollection  '(Of ActionResponseQueue)
        Inherits System.Collections.ArrayList

        Private m_Owner As AsteriskCallManager

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal owner As AsteriskCallManager)
            Me.New
            If (owner Is Nothing) Then Throw New ArgumentException("Owner")
            Me.m_Owner = owner
        End Sub

        Public ReadOnly Property Owner As AsteriskCallManager
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function GetItemByKey(ByVal key As String) As ActionResponseQueue
            SyncLock Me.SyncRoot
                For Each item As ActionResponseQueue In Me
                    If item.Action.ActionID = key Then Return item
                Next
                Return Nothing
            End SyncLock
        End Function

        'Protected Overrides Sub OnInsert(index As Integer, value As Object)
        '    If (Me.m_Owner IsNot Nothing) Then DirectCast(value, ActionResponseQueue).SetOwner(Me.m_Owner)
        '    MyBase.OnInsert(index, value)
        'End Sub


        'Default Public ReadOnly Property Item(ByVal index As Integer) As ActionResponseQueue
        '    Get
        '        Return MyBase.InnerList(index)
        '    End Get
        'End Property

        'Friend Sub Add(ByVal actionID As String, ByVal item As ActionResponseQueue)
        '    Me.InnerList.Add(item)
        'End Sub

    End Class

End Namespace