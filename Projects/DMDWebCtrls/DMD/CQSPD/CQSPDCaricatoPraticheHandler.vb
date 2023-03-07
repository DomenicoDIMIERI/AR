Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms

    <Serializable> _
    Public Class CQSPDCPHSTATINFO
        Implements DMD.XML.IDMDXMLSerializable

        Public IDOperatore As Integer
        Public NomeOperatore As String
        Public Numero As Integer
        Public ML As Decimal
        Public Minimo As Double
        Public Massimo As Double
        Public Media As Double


        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IDOperatore" : Me.IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Numero" : Me.Numero = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ML" : Me.ML = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Minimo" : Me.Minimo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Massimo" : Me.Massimo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Media" : Me.Media = XML.Utils.Serializer.DeserializeDouble(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.NomeOperatore)
            writer.WriteAttribute("Numero", Me.Numero)
            writer.WriteAttribute("ML", Me.ML)
            writer.WriteAttribute("Minimo", Me.Minimo)
            writer.WriteAttribute("Massimo", Me.Massimo)
            writer.WriteAttribute("Media", Me.Media)
        End Sub
    End Class

    Public Class CQSPDCaricatoPraticheHandler
        Inherits CQSPDBaseStatsHandler

        Public Sub New()
            Me.UseLocal = False
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CRapportiniCursor
        End Function

        Private Function GetNomeCampo(ByVal tipop As String, ByVal tipov As String) As String
            Dim ret As String
            Select Case tipop
                Case "Provvigione Azienda" : ret = "[Spread]"
                Case "Provvigione Collaboratore" : ret = "[ProvvBroker]"
                Case "Provvigione Totale" : ret = "([Spread] + [ProvvBroker])"
                Case "UpFront" : ret = "[UpFront]"
                Case Else : ret = "[Spread]"
            End Select
            Select Case tipov
                Case "%" : ret = "IIF([MontanteLordo]>0, 100 * " & ret & " / [MontanteLordo], NULL)"
            End Select

            Return ret
        End Function

        Private Function GetWherePart(ByVal tipop As String) As String
            Dim ret As String
            Select Case tipop
                Case "Provvigione Azienda" : ret = ""
                Case "Provvigione Collaboratore" : ret = ""
                Case "Provvigione Totale" : ret = ""
                Case "UpFront" : ret = ""
                Case Else : ret = ""
            End Select
            Return ret
        End Function

        Private Function GetInnerHTMLCons(ByVal cursor As CRapportiniCursor, ByVal tipop As String, ByVal tipov As String) As CCollection(Of CQSPDCPHSTATINFO)
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                Dim nomeCampo As String = Me.GetNomeCampo(tipop, tipov)
                Dim dbSQL As String
                dbSQL = "SELECT [IDConsulente], Count(*) As [Num], Sum([MontanteLordo]) As [ML], Avg(" & nomeCampo & ") As [Media], Min(" & nomeCampo & ") As [Minimo], Max(" & nomeCampo & ") As [Massimo] FROM (" & cursor.GetSQL & ") WHERE ([StatoPratica]>=" & StatoPraticaEnum.STATO_LIQUIDATA & ") And ([StatoPratica]<=" & StatoPraticaEnum.STATO_ARCHIVIATA & ") " & Me.GetWherePart(tipop) & " GROUP BY [IDConsulente] ORDER BY Avg(" & nomeCampo & ") DESC"
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)

                Dim ret As New CCollection(Of CQSPDCPHSTATINFO)
                While dbRis.Read
                    Dim info As New CQSPDCPHSTATINFO
                    info.IDOperatore = Formats.ToInteger(dbRis("IDConsulente"))
                    Dim consulente As CConsulentePratica = DMD.CQSPD.Consulenti.GetItemById(info.IDOperatore)
                    If (consulente IsNot Nothing) Then info.NomeOperatore = consulente.Nome
                    info.Numero = Formats.ToInteger(dbRis("Num"))
                    info.ML = Formats.ToValuta(dbRis("ML"))
                    info.Minimo = Formats.ToDouble(dbRis("Minimo"))
                    info.Massimo = Formats.ToDouble(dbRis("Massimo"))
                    info.Media = Formats.ToDouble(dbRis("Media"))
                    ret.Add(info)
                End While



                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function


        Private Function GetInnerHTMLOpContatto(ByVal cursor As CRapportiniCursor, ByVal tipop As String, ByVal tipov As String) As CCollection(Of CQSPDCPHSTATINFO)
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                Dim ret As New CCollection(Of CQSPDCPHSTATINFO)
                Dim dbSQL As String
                Dim nomeCampo As String = Me.GetNomeCampo(tipop, tipov)
                dbSQL = "SELECT [STL_IDOP], Count(*) As [Num], Sum([MontanteLordo]) As [ML], Avg(" & nomeCampo & ") As [Media], Min(" & nomeCampo & ") As [Minimo], Max(" & nomeCampo & ") As [Massimo] FROM (" & cursor.GetSQL & ") WHERE ([StatoPratica]>=" & StatoPraticaEnum.STATO_LIQUIDATA & ") And ([StatoPratica]<=" & StatoPraticaEnum.STATO_ARCHIVIATA & ") " & Me.GetWherePart(tipop) & " GROUP BY [STL_IDOP] ORDER BY Avg(" & nomeCampo & ") DESC"
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    Dim info As New CQSPDCPHSTATINFO
                    info.IDOperatore = Formats.ToInteger(dbRis("STL_IDOP"))
                    Dim operatore As CUser = Sistema.Users.GetItemById(info.IDOperatore)
                    If (operatore IsNot Nothing) Then info.NomeOperatore = operatore.Nominativo
                    info.Numero = Formats.ToInteger(dbRis("Num"))
                    info.ML = Formats.ToValuta(dbRis("ML"))
                    info.Minimo = Formats.ToDouble(dbRis("Minimo"))
                    info.Massimo = Formats.ToDouble(dbRis("Massimo"))
                    info.Media = Formats.ToDouble(dbRis("Media"))
                    ret.Add(info)
                End While
                dbRis.Dispose() : dbRis = Nothing

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Private Function GetInnerHTMLOpRichDel(ByVal cursor As CRapportiniCursor, ByVal tipop As String, ByVal tipov As String) As CCollection(Of CQSPDCPHSTATINFO)
            Dim dbRis As System.Data.IDataReader = Nothing
            Try
                Dim ret As New CCollection(Of CQSPDCPHSTATINFO)
                Dim dbSQL As String
                'Dim i As Integer
                Dim nomeCampo As String = Me.GetNomeCampo(tipop, tipov)
                dbSQL = "SELECT [OperatoreLH] As [IDOperatore], Count(*) As [Num], Sum([MontanteLordo]) As [ML], Avg(" & nomeCampo & ") As [Media], Min(" & nomeCampo & ") As [Minimo], Max(" & nomeCampo & ") As [Massimo] FROM (" & cursor.GetSQL & ") WHERE ([StatoPratica]>=" & StatoPraticaEnum.STATO_LIQUIDATA & ") And ([StatoPratica]<=" & StatoPraticaEnum.STATO_ARCHIVIATA & ") " & Me.GetWherePart(tipop) & " GROUP BY [OperatoreLH] ORDER BY Avg(" & nomeCampo & ") DESC"
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    Dim info As New CQSPDCPHSTATINFO
                    info.IDOperatore = Formats.ToInteger(dbRis("IDOperatore"))
                    Dim operatore As CUser = Sistema.Users.GetItemById(info.IDOperatore)
                    If (operatore IsNot Nothing) Then info.NomeOperatore = operatore.Nominativo
                    info.Numero = Formats.ToInteger(dbRis("Num"))
                    info.ML = Formats.ToValuta(dbRis("ML"))
                    info.Minimo = Formats.ToDouble(dbRis("Minimo"))
                    info.Massimo = Formats.ToDouble(dbRis("Massimo"))
                    info.Media = Formats.ToDouble(dbRis("Media"))
                    ret.Add(info)
                End While
                dbRis.Dispose() : dbRis = Nothing

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Private Function GetInnerHTMLAmm(ByVal cursor As CRapportiniCursor, ByVal tipop As String, ByVal tipov As String) As CCollection(Of CQSPDCPHSTATINFO)
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                Dim ret As New CCollection(Of CQSPDCPHSTATINFO)
                Dim dbSQL As String
                Dim i As Integer
                Dim nomeCampo As String = Me.GetNomeCampo(tipop, tipov)
                Dim ammID As New System.Collections.ArrayList
                dbSQL = "SELECT [IDAmministrazione], Count(*) As [Num], Sum([MontanteLordo]) As [ML], Avg(" & nomeCampo & ") As [Media], Min(" & nomeCampo & ") As [Minimo], Max(" & nomeCampo & ") As [Massimo] FROM (" & cursor.GetSQL & ") WHERE ([StatoPratica]>=" & StatoPraticaEnum.STATO_LIQUIDATA & ") And ([StatoPratica]<=" & StatoPraticaEnum.STATO_ARCHIVIATA & ") " & Me.GetWherePart(tipop) & " GROUP BY [IDAmministrazione] ORDER BY Avg(" & nomeCampo & ") DESC"
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                i = 0
                While dbRis.Read
                    Dim info As New CQSPDCPHSTATINFO
                    info.IDOperatore = Formats.ToInteger(dbRis("IDAmministrazione"))
                    If (info.IDOperatore <> 0) Then ammID.Add(info.IDOperatore)
                    'Dim az As CAzienda = Anagrafica.Aziende.GetItemById(info.IDOperatore)
                    'If (az IsNot Nothing) Then info.NomeOperatore = az.RagioneSociale
                    info.Numero = Formats.ToInteger(dbRis("Num"))
                    info.ML = Formats.ToValuta(dbRis("ML"))
                    info.Minimo = Formats.ToDouble(dbRis("Minimo"))
                    info.Massimo = Formats.ToDouble(dbRis("Massimo"))
                    info.Media = Formats.ToDouble(dbRis("Media"))
                    ret.Add(info)
                End While
                dbRis.Dispose() : dbRis = Nothing

                If (ammID.Count > 0) Then
                    Dim buffer As New System.Text.StringBuilder
                    Dim id As Integer
                    For Each id In ammID
                        If buffer.Length > 0 Then buffer.Append(",")
                        buffer.Append(DBUtils.DBNumber(id))
                    Next
                    dbSQL = "SELECT [ID], [Cognome] FROM [tbl_Persone] WHERE [ID] In (" & buffer.ToString & ") AND [Stato]=" & ObjectStatus.OBJECT_VALID
                    dbRis = APPConn.ExecuteReader(dbSQL)
                    While dbRis.Read
                        id = Formats.ToInteger(dbRis("ID"))
                        For Each info As CQSPDCPHSTATINFO In ret
                            If info.IDOperatore = id Then
                                info.NomeOperatore = Formats.ToString(dbRis("Cognome"))
                                Exit For
                            End If
                        Next
                    End While
                    dbRis.Dispose() : dbRis = Nothing
                End If

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Private Function GetInnerHTMLOpPerfez(ByVal cursor As CRapportiniCursor, ByVal tipop As String, ByVal tipov As String) As CCollection(Of CQSPDCPHSTATINFO)
            Dim dbRis As System.Data.IDataReader = Nothing
            Try
                Dim dbSQL As String
                Dim nomeCampo As String = Me.GetNomeCampo(tipop, tipov)
                dbSQL = "SELECT [Operatore1], Count(*) As [Num], Sum([MontanteLordo]) As [ML], Avg(" & nomeCampo & ") As [Media], Min(" & nomeCampo & ") As [Minimo], Max(" & nomeCampo & ") As [Massimo] FROM (" & cursor.GetSQL & ") WHERE ([StatoPratica]>=" & StatoPraticaEnum.STATO_LIQUIDATA & ") And ([StatoPratica]<=" & StatoPraticaEnum.STATO_ARCHIVIATA & ") " & Me.GetWherePart(tipop) & " GROUP BY [Operatore1] ORDER BY Avg(" & nomeCampo & ") DESC"
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                Dim ret As New CCollection(Of CQSPDCPHSTATINFO)
                While dbRis.Read
                    Dim info As New CQSPDCPHSTATINFO
                    info.IDOperatore = Formats.ToInteger(dbRis("Operatore1"))
                    Dim operatore As CUser = Sistema.Users.GetItemById(info.IDOperatore)
                    If (operatore IsNot Nothing) Then info.NomeOperatore = operatore.Nominativo
                    info.Numero = Formats.ToInteger(dbRis("Num"))
                    info.ML = Formats.ToValuta(dbRis("ML"))
                    info.Minimo = Formats.ToDouble(dbRis("Minimo"))
                    info.Massimo = Formats.ToDouble(dbRis("Massimo"))
                    info.Media = Formats.ToDouble(dbRis("Media"))
                    ret.Add(info)
                End While
                dbRis.Dispose() : dbRis = Nothing
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Private Function GetInnerHTMLProdotto(ByVal cursor As CRapportiniCursor, ByVal tipop As String, ByVal tipov As String) As CCollection(Of CQSPDCPHSTATINFO)
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                Dim dbSQL As String
                Dim nomeCampo As String = Me.GetNomeCampo(tipop, tipov)
                dbSQL = "SELECT [Prodotto], Count(*) As [Num], Sum([MontanteLordo]) As [ML], Avg(" & nomeCampo & ") As [Media], Min(" & nomeCampo & ") As [Minimo], Max(" & nomeCampo & ") As [Massimo] FROM (" & cursor.GetSQL & ") WHERE ([StatoPratica]>=" & StatoPraticaEnum.STATO_LIQUIDATA & ") And ([StatoPratica]<=" & StatoPraticaEnum.STATO_ARCHIVIATA & ") " & Me.GetWherePart(tipop) & " GROUP BY [Prodotto] ORDER BY Avg(" & nomeCampo & ") DESC"
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                Dim ret As New CCollection(Of CQSPDCPHSTATINFO)
                While dbRis.Read
                    Dim info As New CQSPDCPHSTATINFO
                    info.IDOperatore = Formats.ToInteger(dbRis("Prodotto"))
                    Dim prodotto As CCQSPDProdotto = DMD.CQSPD.Prodotti.GetItemById(info.IDOperatore)
                    If (prodotto IsNot Nothing) Then info.NomeOperatore = prodotto.Descrizione & " (" & prodotto.NomeCessionario & ")"
                    info.Numero = Formats.ToInteger(dbRis("Num"))
                    info.ML = Formats.ToValuta(dbRis("ML"))
                    info.Minimo = Formats.ToDouble(dbRis("Minimo"))
                    info.Massimo = Formats.ToDouble(dbRis("Massimo"))
                    info.Media = Formats.ToDouble(dbRis("Media"))
                    ret.Add(info)
                End While
                dbRis.Dispose() : dbRis = Nothing
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Private Function GetInnerHTMLPO(ByVal cursor As CRapportiniCursor, ByVal tipop As String, ByVal tipov As String) As CCollection(Of CQSPDCPHSTATINFO)
            Dim dbRis As System.Data.IDataReader = Nothing
            Try
                Dim dbSQL As String
                Dim nomeCampo As String = Me.GetNomeCampo(tipop, tipov)
                dbSQL = "SELECT [IDPuntoOperativo], Count(*) As [Num], Sum([MontanteLordo]) As [ML], Avg(" & nomeCampo & ") As [Media], Min(" & nomeCampo & ") As [Minimo], Max(" & nomeCampo & ") As [Massimo] FROM (" & cursor.GetSQL & ") WHERE ([StatoPratica]>=" & StatoPraticaEnum.STATO_LIQUIDATA & ") And ([StatoPratica]<=" & StatoPraticaEnum.STATO_ARCHIVIATA & ") " & Me.GetWherePart(tipop) & " GROUP BY [IDPuntoOperativo] ORDER BY Avg(" & nomeCampo & ") DESC"
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                Dim ret As New CCollection(Of CQSPDCPHSTATINFO)
                While dbRis.Read
                    Dim info As New CQSPDCPHSTATINFO
                    info.IDOperatore = Formats.ToInteger(dbRis("IDPuntoOperativo"))
                    Dim oggetto As CUfficio = Anagrafica.Uffici.GetItemById(info.IDOperatore)
                    If (oggetto IsNot Nothing) Then info.NomeOperatore = oggetto.Nome
                    info.Numero = Formats.ToInteger(dbRis("Num"))
                    info.ML = Formats.ToValuta(dbRis("ML"))
                    info.Minimo = Formats.ToDouble(dbRis("Minimo"))
                    info.Massimo = Formats.ToDouble(dbRis("Massimo"))
                    info.Media = Formats.ToDouble(dbRis("Media"))
                    ret.Add(info)
                End While
                dbRis.Dispose() : dbRis = Nothing

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Private Function GetInnerHTMLGen(ByVal cursor As CRapportiniCursor, ByVal tipop As String, ByVal tipov As String) As CCollection(Of CQSPDCPHSTATINFO)
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                Dim dbSQL As String
                'Dim i As Integer
                Dim nomeCampo As String = Me.GetNomeCampo(tipop, tipov)
                dbSQL = "SELECT Count(*) As [Num], Sum([MontanteLordo]) As [ML], Avg(" & nomeCampo & ") As [Media], Min(" & nomeCampo & ") As [Minimo], Max(" & nomeCampo & ") As [Massimo] FROM (" & cursor.GetSQL & ") WHERE ([StatoPratica]>=" & StatoPraticaEnum.STATO_LIQUIDATA & ") And ([StatoPratica]<=" & StatoPraticaEnum.STATO_ARCHIVIATA & ") " & Me.GetWherePart(tipop) & " ORDER BY Avg(" & nomeCampo & ") DESC"
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                Dim ret As New CCollection(Of CQSPDCPHSTATINFO)
                While dbRis.Read
                    Dim info As New CQSPDCPHSTATINFO
                    info.Numero = Formats.ToInteger(dbRis("Num"))
                    info.ML = Formats.ToValuta(dbRis("ML"))
                    info.Minimo = Formats.ToDouble(dbRis("Minimo"))
                    info.Massimo = Formats.ToDouble(dbRis("Massimo"))
                    info.Media = Formats.ToDouble(dbRis("Media"))
                    ret.Add(info)
                End While
                dbRis.Dispose() : dbRis = Nothing
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Public Function GetStats(ByVal renderer As Object) As String
            If Not Me.Module.UserCanDoAction("list") Then Throw New PermissionDeniedException(Me.Module, "list")
            Dim tipos As String = RPC.n2str(GetParameter(renderer, "tipos", ""))
            Dim tipop As String = RPC.n2str(GetParameter(renderer, "tipop", ""))
            Dim tipov As String = RPC.n2str(GetParameter(renderer, "tipov", ""))
            Dim cursor As CRapportiniCursor = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "cursor", "")))
            cursor.Flags.Value = PraticaFlags.HIDDEN
            cursor.Flags.Operator = OP.OP_NE

            Dim ret As CCollection(Of CQSPDCPHSTATINFO)
            Select Case tipos
                Case "Per consulente" : ret = Me.GetInnerHTMLCons(cursor, tipop, tipov)
                Case "Per operatore di contatto" : ret = Me.GetInnerHTMLOpContatto(cursor, tipop, tipov)
                Case "Per operatore di richiesta delibera" : ret = Me.GetInnerHTMLOpRichDel(cursor, tipop, tipov)
                Case "Per operatore di perfezionamento" : ret = Me.GetInnerHTMLOpPerfez(cursor, tipop, tipov)
                Case "Per amministrazione" : ret = Me.GetInnerHTMLAmm(cursor, tipop, tipov)
                Case "Per prodotto" : ret = Me.GetInnerHTMLProdotto(cursor, tipop, tipov)
                Case "Per punto operativo" : ret = Me.GetInnerHTMLPO(cursor, tipop, tipov)
                Case Else : ret = Me.GetInnerHTMLGen(cursor, tipop, tipov)
            End Select

            Return XML.Utils.Serializer.Serialize(ret)
        End Function


    End Class


End Namespace