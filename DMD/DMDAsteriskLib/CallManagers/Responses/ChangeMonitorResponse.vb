Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Responses

    Public Class ChangeMonitorResponse
        Inherits ActionResponse

        Public Sub New()
        End Sub

        Public Sub New(ByVal action As Action)
            MyBase.New(action)
        End Sub

    End Class

End Namespace