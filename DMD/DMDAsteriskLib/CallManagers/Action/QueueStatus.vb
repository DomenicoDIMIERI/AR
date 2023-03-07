Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    ''' <summary>
    ''' The "QueueStatus" request returns statistical information about calls delivered to the existing queues, as well as the corresponding service level.
    ''' The response consists of zero or more "Event: QueueParams" stanzas, one per queue. Each is followed by zero or more "Event: QueueMember" stanzas, one per agent assigned to that queue, and zero or more "Event: QueueEntry" stanzas, one for each call waiting in the queue. The whole lot ends with an "Event: QueueStatusComplete" stanza.
    ''' Parameters: ActionID, Queue, Member
    ''' The Queue parameter allows to select one queue to view its status, as with the Member parameter, that allow to select one queue member.
    ''' You can use the Member parameter with something like 'cnlohewfuy4r89734yc8yc48' to not receive any information about members, just info on the Queue.
    ''' If the parameters are not set all Queues and all Member will be shown.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QueueStatus
        Inherits AsyncAction

        Public Sub New()
            MyBase.New("QueueStatus")
        End Sub


        Protected Overrides Function GetCommandText() As String
            Dim ret As String = MyBase.GetCommandText()
            Return ret
        End Function

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New QueueStatusResponse(Me)
        End Function


    End Class

End Namespace