Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    Public Class ZapShowChannels
        Inherits AsyncAction

        Public Sub New()
            MyBase.New("ZapShowChannels")
        End Sub
         
        Protected Overrides Function GetCommandText() As String
            Dim ret As String = MyBase.GetCommandText()
            Return ret
        End Function

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New ZapShowChannelsResponse(Me)
        End Function


    End Class

End Namespace