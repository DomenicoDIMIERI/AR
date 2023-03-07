Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.XML

Namespace Forms


    Public Class CQSPTTabelleFinanziarieModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CQSPD.TabelleFinanziarie.GetItemById(id)
        End Function

        Public Function GetTabellaXProdottoByID(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim item As CProdottoXTabellaFin = CQSPD.TabelleFinanziarie.GetTabellaXProdottoByID(id)
            If (item Is Nothing) Then Return ""
            Return XML.Utils.Serializer.Serialize(item, XMLSerializeMethod.Document)
        End Function

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CTabelleFinanziarieCursor
        End Function

        Public Function CreateElencoUnusedTblFin(ByVal renderer As Object) As String
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim idProdotto As Integer = RPC.n2int(GetParameter(renderer, "pid", "0"))
            Dim writer As New System.Text.StringBuilder

            If (idProdotto <> 0) Then
                Try
                    'dbRis = CQSPD.Database.ExecuteReader("SELECT * FROM [tbl_FIN_TblFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And Not [ID] In (SELECT DISTINCT [Tabella] FROM [tbl_FIN_ProdXTabFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And ([Prodotto]=" & idProdotto & ")) ORDER BY [Nome] ASC")
                    dbRis = CQSPD.Database.ExecuteReader("SELECT * FROM [tbl_FIN_TblFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And Not [ID] In (SELECT [Tabella] FROM [tbl_FIN_ProdXTabFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And ([Prodotto]=" & idProdotto & ") GROUP BY [Tabella]) ORDER BY [Nome] ASC")
                    While dbRis.Read
                        writer.Append("<option value=""")
                        writer.Append(Formats.ToInteger(dbRis("ID")))
                        writer.Append(""">")
                        writer.Append(Formats.ToString(dbRis("Nome")))
                        writer.Append(" (")
                        writer.Append(Formats.ToString(dbRis("NomeCessionario")))
                        writer.Append(")</option>")
                    End While
                Catch ex As Exception
                    Throw
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try
            End If

            Return writer.ToString
        End Function

        Public Function CreateElencoUsedTblFin(ByVal renderer As Object) As String
            'Dim items As CCollection = CQSPD.TabelleFinanziarie.GetUsed(idProdotto)
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim writer As New System.Text.StringBuilder
            Dim idProdotto As Integer = RPC.n2int(GetParameter(renderer, "pid", "0"))
            If (idProdotto <> 0) Then
                Try
                    'dbRis = CQSPD.Database.ExecuteReader("SELECT * FROM [tbl_FIN_TblFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And [ID] In (SELECT DISTINCT [Tabella] FROM [tbl_FIN_ProdXTabFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And ([Prodotto]=" & idProdotto & ")) ORDER BY [Nome] ASC")
                    dbRis = CQSPD.Database.ExecuteReader("SELECT * FROM [tbl_FIN_TblFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And [ID] In (SELECT [Tabella] FROM [tbl_FIN_ProdXTabFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And ([Prodotto]=" & idProdotto & ") GROUP BY [Tabella]) ORDER BY [Nome] ASC")
                    While dbRis.Read
                        writer.Append("<option value=""")
                        writer.Append(Formats.ToInteger(dbRis("ID")))
                        writer.Append(""">")
                        writer.Append(Formats.ToString(dbRis("Nome")))
                        writer.Append(" (")
                        writer.Append(Formats.ToString(dbRis("NomeCessionario")))
                        writer.Append(")</option>")
                    End While
                Catch ex As Exception
                    Throw
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try
            End If

            Return writer.ToString
        End Function

    End Class


End Namespace