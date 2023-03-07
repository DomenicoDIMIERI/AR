Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    ''' <summary>
    ''' The "Queues" request returns configuration and statistical information about the existing queues.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Queues
        Inherits Action
         
        Public Sub New()
            MyBase.New("Queues")
        End Sub
         

        Protected Overrides Function GetCommandText() As String
            Dim ret As String = MyBase.GetCommandText()
            Return ret
        End Function

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New QueuesResponse(Me)
        End Function


    End Class

End Namespace