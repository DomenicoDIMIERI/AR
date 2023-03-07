Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    Public Class Ping
        Inherits Action

        Public Sub New()
            MyBase.New("Ping")
        End Sub

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New PingResponse(Me)
        End Function


    End Class

End Namespace