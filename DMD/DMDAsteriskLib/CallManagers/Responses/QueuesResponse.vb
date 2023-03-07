Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.Asterisk

Namespace CallManagers.Responses

    Public Class QueuesResponse
        Inherits ActionResponse


        Public Sub New()
        End Sub

        Public Sub New(ByVal action As Action)
            MyBase.New(action)
        End Sub

        Protected Overrides Sub ParseRow(row As RowEntry)
            Select Case UCase(row.Command)
                Case Else : MyBase.ParseRow(row)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Dim ret As String = MyBase.ToString()
            Return ret
        End Function



    End Class

End Namespace