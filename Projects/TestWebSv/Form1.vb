Public Class Form1

    
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim w As New FinSeAWebSvc.WebSvcSoapClient
        Dim a As FinSeA.CQSPD.CRapportino = CType(w.Prova1, FinSeA.CQSPD.CRapportino)

        MsgBox(a.Impiego.NomeAzienda)

    End Sub
End Class
