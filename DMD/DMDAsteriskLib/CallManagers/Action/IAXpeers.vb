Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions


    ''' <summary>
    ''' Show the IAX Peers on the server and their status
    ''' </summary>
    ''' <remarks>Will not work for built-in variables like LANGUAGE !</remarks>
    Public Class IAXpeers
        Inherits Action


        Public Sub New()
            MyBase.New("IAXpeers")
        End Sub

       
        Protected Overrides Function GetCommandText() As String
            Return MyBase.GetCommandText()
        End Function

         

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New IAXpeersResponse(Me)
        End Function

    End Class

End Namespace