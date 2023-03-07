Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports FinSeA
Imports FinSeA.Databases
 

    Public Class CStackObject
        Public Caller As Object 'Chiamante
        Public MethodName As String 'Nome della funzione
        Public EnterTime As Double 'Tempo di ingresso

        Public Sub New()
            Me.Caller = Nothing
            Me.MethodName = ""
            Me.EnterTime = 0
        End Sub

        Public Overrides Function ToString() As String
            Return TypeName(Me.Caller) & "." & Me.MethodName
        End Function

    End Class

