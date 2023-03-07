Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    Public Class ParkedCalls
        Inherits Action

        Public Sub New()
            MyBase.New("ParkedCalls")
        End Sub

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New ParkedCallsResponse(Me)
        End Function


    End Class

End Namespace