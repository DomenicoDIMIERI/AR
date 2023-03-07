Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

 

Partial Public Class Sistema

    <Serializable> _
    Public Class CEmailAccounts
        Inherits CCollection(Of CEmailAccount)

        Public Sub New()
        End Sub


        Sub Load()
            Dim db As CDBConnection = APPConn
            Dim reader As DBReader
            Dim dbSQL As String = "SELECT * FROM [tbl_EmailAccounts] ORDER BY [ID] ASC"
            reader = New DBReader(db.Tables("tbl_EmailAccounts"), dbSQL)
            While reader.Read
                Dim account As New CEmailAccount
                db.Load(account, reader)
                Me.Add(account)
            End While
            reader.Dispose()
        End Sub

    End Class


End Class