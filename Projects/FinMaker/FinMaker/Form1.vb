Public Class Form1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim dbConn As New System.Data.Odbc.OdbcConnection("DSN=FINMAKER;uid=admin;pwd=")
        dbConn.Open()
        Dim dbCmd As New System.Data.Odbc.OdbcCommand("select * from Pratiche.fp7", dbConn)
        Dim dbRis As System.Data.Odbc.OdbcDataReader = dbCmd.ExecuteReader
        While dbRis.Read
            Debug.Print("F " & dbRis.GetName(0) & " = " & dbRis(0))
        End While
        dbRis.Close()
        dbCmd.Dispose()
        dbConn.Dispose()
    End Sub
End Class
