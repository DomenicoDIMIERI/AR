Imports FinSeA
Imports FinSeA.FaxGateway

Partial Class [Default]
    Inherits System.Web.UI.Page

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        If FaxGateway.Database Is Nothing Then
            Dim db As New DBConnection
            db.Path = Server.MapPath("/App_Data/db/faxlog.mdb")
            db.Open()

            FaxGateway.Database = db


        End If

        Dim f As FaxDocument = FaxGateway.GetFax("12346")
        f.UpdateStatus()
        Response.Write(f.StatoInvio)
    End Sub

End Class
