Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    Public Class QueueAdd
        Inherits AsyncAction

        Private m_Queue As String
        Private m_Interface As String
        Private m_Penalty As Integer
        Private m_Paused As Boolean
        Private m_MemberName As String
        Private m_StateInterface As String

        Public Sub New()
            MyBase.New("QueueAdd")
        End Sub

        Public Sub New(ByVal queue As String, ByVal [interface] As String, ByVal penalty As Integer, ByVal paused As Boolean, ByVal memberName As String, ByVal stateInterface As String)
            Me.New()
            Me.m_Queue = queue
            Me.m_Interface = [interface]
            Me.m_Penalty = penalty
            Me.m_Paused = paused
            Me.m_MemberName = memberName
            Me.m_StateInterface = stateInterface
        End Sub

        Public ReadOnly Property Queue As String
            Get
                Return Me.m_Queue
            End Get
        End Property

        Public ReadOnly Property [Interface] As String
            Get
                Return Me.m_Interface
            End Get
        End Property

        Public ReadOnly Property Penalty As Integer
            Get
                Return Me.m_Penalty
            End Get
        End Property

        Public ReadOnly Property Paused As Boolean
            Get
                Return Me.m_Paused
            End Get
        End Property

        Public ReadOnly Property MemberName As String
            Get
                Return Me.m_MemberName
            End Get
        End Property

        Public ReadOnly Property StateInterface As String
            Get
                Return Me.m_StateInterface
            End Get
        End Property

        Protected Overrides Function GetCommandText() As String
            Dim ret As String = MyBase.GetCommandText()
            ret &= "Queue: " & Me.Queue & vbCrLf
            ret &= "Interface: " & Me.Interface & vbCrLf
            ret &= "Penalty: " & Me.Penalty & vbCrLf
            ret &= "Paused: " & IIf(Me.Paused, "Yes", "No") & vbCrLf
            ret &= "MemberName: " & Me.MemberName & vbCrLf
            ret &= "StateInterface: " & Me.StateInterface & vbCrLf
            Return ret
        End Function

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New QueueAddResponse(Me)
        End Function


    End Class

End Namespace