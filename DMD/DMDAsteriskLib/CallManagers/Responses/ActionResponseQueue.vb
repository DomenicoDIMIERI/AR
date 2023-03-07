Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    Public Class ActionResponseQueue
        Private m_Owner As AsteriskCallManager
        Private m_Action As AsyncAction
        Private m_Items As System.Collections.ArrayList ' CCollection(Of AsteriskEvent)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal action As AsyncAction)
            Me.New
            If (action Is Nothing) Then Throw New ArgumentException("Action")
            Me.m_Action = action
        End Sub

        Public ReadOnly Property Action As AsyncAction
            Get
                Return Me.m_Action
            End Get
        End Property

        Public ReadOnly Property Items As System.Collections.ArrayList ' CCollection(Of AsteriskEvent)
            Get
                If (Me.m_Items Is Nothing) Then Me.m_Items = New System.Collections.ArrayList ' CCollection(Of AsteriskEvent)
                Return Me.m_Items
            End Get
        End Property

        Public ReadOnly Property Owner As AsteriskCallManager
            Get
                Return Me.m_Owner
            End Get
        End Property

        Protected Friend Overridable Sub SetOwner(ByVal owner As AsteriskCallManager)
            Me.m_Owner = owner
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace