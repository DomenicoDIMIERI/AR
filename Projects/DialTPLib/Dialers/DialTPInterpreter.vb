Imports System.IO

Public Class DialTPInterpreter
    Public Action As String
    Public Protocol As String
    Public Number As String
    Public DialPrefix As String
    Public Options As String
    Public ID As String

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Public Function Parse(ByVal argument As String) As Boolean
        Const dtp As String = "dialtp:"

        If Strings.Left(LCase(argument), Len(dtp)) = dtp Then argument = Trim(Mid(argument, Len(dtp) + 1))

        argument = System.Uri.UnescapeDataString(argument) 'Replace(argument, "%20", " ")

        'For Each argument As String In args
        If InStr(argument, " ") > 0 Then
            Dim subs() As String = Split(argument, " ")
            For Each a As String In subs
                Me.ProcessArgument(a)
            Next
            'Else
            '     ProcessArgument(argument, o)
        End If
        ' Next

        Return True
    End Function

    Private Sub ProcessArgument(ByVal arguemnt As String)
        Dim i As Integer
        Dim n, v As String
        i = InStr(arguemnt, "=")
        If (i > 0) Then
            n = Trim(Strings.Left(arguemnt, i - 1))
            v = Trim(Mid(arguemnt, i + 1))
        Else
            n = Trim(arguemnt)
            v = ""
        End If
        Select Case LCase(n)
            Case "action" : Me.Action = v
            Case "protocol" : Me.Protocol = v
            Case "number" : Me.Number = v
            Case "dialprefix" : Me.DialPrefix = v
            Case "id" : Me.ID = v
            Case "options" : Me.Options = v
            Case Else
                DMD.Sistema.ApplicationContext.Log("DialTPInterpreter: Argomento non riconosciuto: " & arguemnt & " (ignoro)")
        End Select

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class
