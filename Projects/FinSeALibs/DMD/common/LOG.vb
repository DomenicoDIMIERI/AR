Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports System.Net

Public NotInheritable Class LOG

    Public Shared Event OnWarn(ByVal message As String, ByVal e As System.Exception)
    Public Shared Event OnError(ByVal message As String, ByVal e As System.Exception)
    Public Shared Event OnInfo(ByVal message As String, ByVal e As System.Exception)

    Private Sub New()
    End Sub

    Public Shared Sub warn(ByVal message As String, ByVal e As System.Exception)
        RaiseEvent OnWarn(message, e)
    End Sub

    Public Shared Sub warn(ByVal message As String)
        RaiseEvent OnWarn(message, Nothing)
    End Sub

    Shared Sub info(ByVal message As String)
        RaiseEvent OnInfo(message, Nothing)
    End Sub

    Shared Sub [error](ByVal message As String, exception As System.Exception)
        RaiseEvent OnError(message, exception)
    End Sub

    Shared Sub [error](p1 As String)
        RaiseEvent OnError(p1, Nothing)
    End Sub

    Shared Sub debug(p1 As String)
        'Global.Microsoft.VisualBasic.deb.debug.Print(p1)
    End Sub

    Shared Function isDebugEnabled() As Boolean
        Throw New NotImplementedException
    End Function

End Class