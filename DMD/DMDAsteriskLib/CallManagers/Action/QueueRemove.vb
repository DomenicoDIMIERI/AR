Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    Public Class QueueRemove
        Inherits AsyncAction

        Private m_Queue As String
        Private m_Interface As String
        
        Public Sub New()
            MyBase.New("QueueRemove")
        End Sub

        ''' <summary>
        ''' QueueRemove
        ''' </summary>
        ''' <param name="queue">Existing queue to remove member from</param>
        ''' <param name="interface">Member interface (sip/1000, zap/1-1, etc)?</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal queue As String, ByVal [interface] As String)
            Me.New()
            Me.m_Queue = queue
            Me.m_Interface = [interface]
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
         
        Protected Overrides Function GetCommandText() As String
            Dim ret As String = MyBase.GetCommandText()
            ret &= "Queue: " & Me.Queue & vbCrLf
            ret &= "Interface: " & Me.Interface & vbCrLf
            Return ret
        End Function

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New QueueRemoveResponse(Me)
        End Function


    End Class

End Namespace