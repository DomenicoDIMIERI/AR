Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers


    Public Class AsyncAction
        Inherits Action
        Private m_ActionID As String
        
        Public Sub New()
            Me.m_ActionID = ""
        End Sub

        Public Sub New(ByVal actionName As String, ByVal actionID As String)
            MyBase.New(actionName)
            Me.m_ActionID = ActionID
        End Sub

        Public Sub New(ByVal actionName As String)
            MyBase.New(actionName)
            Me.m_ActionID = ""
        End Sub
         
        Public ReadOnly Property ActionID As String
            Get
                Return Me.m_ActionID
            End Get
        End Property
        Protected Friend Sub SetActionID(ByVal id As String)
            Me.m_ActionID = id
        End Sub
         
        Protected Overrides Function GetCommandText() As String
            Dim ret As String = MyBase.GetCommandText
            If (Me.m_ActionID <> "") Then ret &= "Actionid: " & Me.m_ActionID & vbCrLf
            Return ret
        End Function

        Public Overrides Function IsAsync() As Boolean
            Return True
        End Function

        Protected Friend Overridable Sub NotifyEvent(ByVal e As AsteriskEvent)

        End Sub

    End Class

End Namespace