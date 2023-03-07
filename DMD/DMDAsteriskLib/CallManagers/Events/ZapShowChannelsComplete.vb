Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    Public Class ZapShowChannelsComplete
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("ZapShowChannelsComplete")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub
          

    End Class

End Namespace