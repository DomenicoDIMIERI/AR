Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions


    
    Public Class ListCommands
        Inherits Action

        Private m_ActionID As Integer

        Public Sub New()
            MyBase.New("IAXpeers")
        End Sub

        Public Sub New(ByVal actionID As Integer)
            Me.New()
            Me.m_ActionID = actionID
        End Sub

        Public ReadOnly Property ActionID As Integer
            Get
                Return Me.m_ActionID
            End Get
        End Property


        Protected Overrides Function GetCommandText() As String
            Return MyBase.GetCommandText() & "ActionID: " & Me.ActionID & vbCrLf
        End Function

         

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New ListCommandsResponse(Me)
        End Function

    End Class

End Namespace