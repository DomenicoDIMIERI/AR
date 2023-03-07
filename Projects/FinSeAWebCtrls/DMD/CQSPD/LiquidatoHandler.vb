Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms
     
    Public Class LiquidatoHandler
        Inherits CQSPDBaseStatsHandler

        Private statoContatto As CStatoPratica
        Private statoRichiestaDelibera As CStatoPratica
        Private statoLiquidata As CStatoPratica
        Private statoArchiviata As CStatoPratica



        Public Sub New()
            Me.UseLocal = False
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return Nothing
        End Function

        Private Function GetArray(ByVal ufficiStr As String) As Integer()
            Dim ret As New System.Collections.ArrayList
            Dim tmp As String() = Split(ufficiStr, ",")
            For i As Integer = 0 To Arrays.Len(tmp) - 1
                Dim id As Integer = Formats.ToInteger(tmp(i))
                If (id <> 0) Then ret.Add(id)
            Next
            Return ret.ToArray(GetType(Integer))
        End Function

        Private Sub AggiungiEstinzioniRinnovabili(ByVal strAnni As String, ByVal strPO As String, ByVal items As CKeyCollection(Of StatLiquidatoItem))
            Dim dbRis As System.Data.IDataReader = Nothing
            Try
                Dim dbSQL As String = ""
                Dim item As StatLiquidatoItem = Nothing

                'Rinnovabile
                dbSQL &= "SELECT Count(*) AS [Cnt], Sum([Rata]*[Durata]) AS [Valore], [IDPuntoOperativo], Year([DataRinnovo]) AS [Anno], Month([DataRinnovo]) AS [Mese] "
                dbSQL &= "FROM [tbl_Estinzioni] "
                dbSQL &= "WHERE (([Stato]=1) AND (([Tipo]) In (1,2)) AND Not ([Rata] Is Null) AND Not ([Durata] Is Null) AND Not ([DataRinnovo]) Is Null) AND [Estingue]=FALSE AND [DataRinnovo]<=" & DBUtils.DBDate(Calendar.GetNextMonthFirstDay(Calendar.ToDay)) & " "
                If (strAnni <> "") Then dbSQL &= " AND Year([DataRinnovo]) In (" & strAnni & ") "
                If (strPO <> "") Then dbSQL &= " AND [IDPuntoOperativo] In (" & strPO & ") "
                dbSQL &= "GROUP BY [IDPuntoOperativo], Year([DataRinnovo]), Month([DataRinnovo])"
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    item = items.GetItemByKey("K" & Formats.ToInteger(dbRis("Anno")) & "_" & Formats.ToInteger(dbRis("Mese")))

                    If (item Is Nothing) Then
                        item = New StatLiquidatoItem
                        item.Anno = Formats.ToInteger(dbRis("Anno"))
                        item.Mese = Formats.ToInteger(dbRis("Mese"))
                        items.Add("K" & item.Anno & "_" & item.Mese, item)
                    End If
                    item.Anno = Formats.ToInteger(dbRis("Anno"))
                    item.Mese = Formats.ToInteger(dbRis("Mese"))
                    'item.RinnovabiliUpfront = Formats.ToValuta(dbRis("Upfront"))
                    'item.RinnovabiliRunning = Formats.ToValuta(dbRis("Running"))
                    item.RinnovabiliCnt = Formats.ToInteger(dbRis("Cnt"))
                    item.RinnovabiliSum = Formats.ToValuta(dbRis("Valore"))
                    'item.RinnovabiliSconto = Formats.ToValuta(dbRis("Sconto"))
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try

        End Sub

        Private Function GetStr(ByVal arr() As Integer) As String
            If (arr Is Nothing OrElse arr.Length <= 0) Then Return vbNullString

            Dim ret As New System.Text.StringBuilder
            For i As Integer = 0 To arr.Length - 1
                If (i > 0) Then ret.Append(",")
                ret.Append(arr(i))
            Next
            Return ret.ToString
        End Function

        Private Sub AggiungiLiquidato(ByVal strAnni As String, ByVal strPO As String, ByVal cursor As CRapportiniCursor, ByVal items As CKeyCollection(Of StatLiquidatoItem))
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                Dim dbSQL As String = ""
                Dim item As StatLiquidatoItem = Nothing

                'dbSQL = "SELECT Count(*) As [Cnt], Sum([T1].[MontanteLordo]) As [Valore], Sum([T1].[Running]) As [Running], Sum([T1].[UpFront]) As [UpFront], Month([T2].[Data]) As [Mese], Year([T2].[Data]) As [Anno] FROM ("
                dbSQL &= "SELECT Count(*) As [Cnt], " &
                               "SUM([T1].[MontanteLordo]) As [Valore], " &
                               "SUM([T1].[Running]) As [Running], " &
                               "SUM([T1].[UpFront]) As [UpFront], " &
                               "SUM(IIF([T1].[ProvvMax] Is Null Or [T1].[UpFront] Is Null, 0, [T1].[ProvvMax] - [T1].[UpFront])) As [Sconto], " &
                               "Month([T2].[Data]) As [Mese], " &
                               "Year([T2].[Data]) As [Anno] " &
                               " FROM ("
                dbSQL &= cursor.GetSQL & ") AS [T1] "
                dbSQL &= " INNER JOIN ("
                dbSQL &= "SELECT * FROM [tbl_PraticheSTL]  WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") AND Year([tbl_PraticheSTL].[Data]) In (" & strAnni & ") AND [tbl_PraticheSTL].[IDToStato] In (" & GetID(statoLiquidata) & ")"
                dbSQL &= ") AS T2 "
                dbSQL &= "ON [T1].[ID]=[T2].[IDPratica] "
                dbSQL &= "GROUP BY Year([T2].[Data]), Month([T2].[Data]) "


                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    item = items.GetItemByKey("K" & Formats.ToInteger(dbRis("Anno")) & "_" & Formats.ToInteger(dbRis("Mese")))

                    If (item Is Nothing) Then
                        item = New StatLiquidatoItem
                        item.Anno = Formats.ToInteger(dbRis("Anno"))
                        item.Mese = Formats.ToInteger(dbRis("Mese"))
                        items.Add("K" & item.Anno & "_" & item.Mese, item)
                    End If
                    item.LiquidatoUpfront = Formats.ToValuta(dbRis("Upfront"))
                    item.LiquidatoRunning = Formats.ToValuta(dbRis("Running"))
                    item.LiquidatoCnt = Formats.ToInteger(dbRis("Cnt"))
                    item.LiquidatoSum = Formats.ToValuta(dbRis("Valore"))
                    item.LiquidatoSconto = Formats.ToValuta(dbRis("Sconto"))
                End While

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Sub

        Private Sub AggiungiDataValuta(ByVal strAnni As String, ByVal strPO As String, ByVal cursor As CRapportiniCursor, ByVal items As CKeyCollection(Of StatLiquidatoItem))
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                Dim dbSQL As String = ""
                Dim item As StatLiquidatoItem = Nothing

                'dbSQL = "SELECT Count(*) As [Cnt], Sum([T1].[MontanteLordo]) As [Valore], Sum([T1].[Running]) As [Running], Sum([T1].[UpFront]) As [UpFront], Month([T2].[Data]) As [Mese], Year([T2].[Data]) As [Anno] FROM ("
                dbSQL &= "SELECT Count(*) As [Cnt], " &
                               "SUM([T1].[MontanteLordo]) As [Valore], " &
                               "SUM([T1].[Running]) As [Running], " &
                               "SUM([T1].[UpFront]) As [UpFront], " &
                               "SUM(IIF([T1].[ProvvMax] Is Null Or [T1].[UpFront] Is Null, 0, [T1].[ProvvMax] - [T1].[UpFront])) As [Sconto], " &
                               "Month([T1].[DataValuta]) As [Mese], " &
                               "Year([T1].[DataValuta]) As [Anno] " &
                               " FROM ("
                dbSQL &= cursor.GetSQL & ") AS [T1] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") AND Year([T1].[DataValuta]) In (" & strAnni & ") AND [T1].[IDStatoAttuale] In (" & GetID(statoLiquidata) & ", " & GetID(statoArchiviata) & ") "
                dbSQL &= "GROUP BY Year([T1].[DataValuta]), Month([T1].[DataValuta]) "


                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    item = items.GetItemByKey("K" & Formats.ToInteger(dbRis("Anno")) & "_" & Formats.ToInteger(dbRis("Mese")))

                    If (item Is Nothing) Then
                        item = New StatLiquidatoItem
                        item.Anno = Formats.ToInteger(dbRis("Anno"))
                        item.Mese = Formats.ToInteger(dbRis("Mese"))
                        items.Add("K" & item.Anno & "_" & item.Mese, item)
                    End If
                    item.LiquidatoUpfront = Formats.ToValuta(dbRis("Upfront"))
                    item.LiquidatoRunning = Formats.ToValuta(dbRis("Running"))
                    item.LiquidatoCnt = Formats.ToInteger(dbRis("Cnt"))
                    item.LiquidatoSum = Formats.ToValuta(dbRis("Valore"))
                    item.LiquidatoSconto = Formats.ToValuta(dbRis("Sconto"))
                End While

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Sub

        Private Sub AggiungiCaricato(ByVal strAnni As String, ByVal strPO As String, ByVal cursor As CRapportiniCursor, ByVal items As CKeyCollection(Of StatLiquidatoItem))
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                Dim dbSQL As String = ""
                Dim item As StatLiquidatoItem = Nothing

                'dbSQL = "SELECT Count(*) As [Cnt], Sum([T1].[MontanteLordo]) As [Valore], Sum([T1].[Running]) As [Running], Sum([T1].[UpFront]) As [UpFront], Month([T2].[Data]) As [Mese], Year([T2].[Data]) As [Anno] FROM ("
                dbSQL = "SELECT Count(*) As [Cnt], " &
                            "SUM([T1].[MontanteLordo]) As [Valore], " &
                            "SUM([T1].[Running]) As [Running], " &
                            "SUM([T1].[UpFront]) As [UpFront], " &
                            "SUM(IIF([T1].[ProvvMax] Is Null Or [T1].[UpFront] Is Null, 0, [T1].[ProvvMax] - [T1].[UpFront])) As [Sconto], " &
                            "Month([T2].[Data]) As [Mese], " &
                            "Year([T2].[Data]) As [Anno] " &
                            " FROM ("
                dbSQL &= cursor.GetSQL & ") AS [T1] "
                dbSQL &= " INNER JOIN ("
                dbSQL &= "SELECT * FROM [tbl_PraticheSTL]  WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") AND Year([tbl_PraticheSTL].[Data]) In (" & strAnni & ") AND [tbl_PraticheSTL].[IDToStato] In (" & GetID(statoContatto) & ")"
                dbSQL &= ") AS T2 "
                dbSQL &= "ON [T1].[ID]=[T2].[IDPratica] "
                dbSQL &= "GROUP BY Year([T2].[Data]), Month([T2].[Data]) "
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    item = items.GetItemByKey("K" & Formats.ToInteger(dbRis("Anno")) & "_" & Formats.ToInteger(dbRis("Mese")))

                    If (item Is Nothing) Then
                        item = New StatLiquidatoItem
                        item.Anno = Formats.ToInteger(dbRis("Anno"))
                        item.Mese = Formats.ToInteger(dbRis("Mese"))
                        items.Add("K" & item.Anno & "_" & item.Mese, item)
                    End If

                    item.CaricatoUpfront = Formats.ToValuta(dbRis("Upfront"))
                    item.CaricatoRunning = Formats.ToValuta(dbRis("Running"))
                    item.CaricatoCnt = Formats.ToInteger(dbRis("Cnt"))
                    item.CaricatoSum = Formats.ToValuta(dbRis("Valore"))
                    item.CaricatoSconto = Formats.ToValuta(dbRis("Sconto"))
                End While

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Sub

        Private Sub AggiungiRichiestaDelibera(ByVal strAnni As String, ByVal strPO As String, ByVal cursor As CRapportiniCursor, ByVal items As CKeyCollection(Of StatLiquidatoItem))
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                Dim dbSQL As String = ""
                Dim item As StatLiquidatoItem = Nothing

                'dbSQL = "SELECT Count(*) As [Cnt], Sum([T1].[MontanteLordo]) As [Valore], Sum([T1].[Running]) As [Running], Sum([T1].[UpFront]) As [UpFront], Month([T2].[Data]) As [Mese], Year([T2].[Data]) As [Anno] FROM ("
                dbSQL = "SELECT Count(*) As [Cnt], " &
                          "SUM([T1].[MontanteLordo]) As [Valore], " &
                          "SUM([T1].[Running]) As [Running], " &
                          "SUM([T1].[UpFront]) As [UpFront], " &
                          "SUM(IIF([T1].[ProvvMax] Is Null Or [T1].[UpFront] Is Null, 0, [T1].[ProvvMax] - [T1].[UpFront])) As [Sconto], " &
                          "Month([T2].[Data]) As [Mese], " &
                          "Year([T2].[Data]) As [Anno] " &
                          " FROM ("
                dbSQL &= cursor.GetSQL & ") AS [T1] "
                dbSQL &= " INNER JOIN ("
                dbSQL &= "SELECT * FROM [tbl_PraticheSTL]  WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") AND Year([tbl_PraticheSTL].[Data]) In (" & strAnni & ") AND [tbl_PraticheSTL].[IDToStato] In (" & GetID(statoRichiestaDelibera) & ")"
                dbSQL &= ") AS [T2] "
                dbSQL &= "ON [T1].[ID]=[T2].[IDPratica] "
                dbSQL &= "GROUP BY Year([T2].[Data]), Month([T2].[Data]) "
                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                While dbRis.Read
                    item = items.GetItemByKey("K" & Formats.ToInteger(dbRis("Anno")) & "_" & Formats.ToInteger(dbRis("Mese")))

                    If (item Is Nothing) Then
                        item = New StatLiquidatoItem
                        item.Anno = Formats.ToInteger(dbRis("Anno"))
                        item.Mese = Formats.ToInteger(dbRis("Mese"))
                        items.Add("K" & item.Anno & "_" & item.Mese, item)
                    End If

                    item.RichiestaDeliberaUpfront = Formats.ToValuta(dbRis("Upfront"))
                    item.RichiestaDeliberaRunning = Formats.ToValuta(dbRis("Running"))
                    item.RichiestaDeliberaCnt = Formats.ToInteger(dbRis("Cnt"))
                    item.RichiestaDeliberaSum = Formats.ToValuta(dbRis("Valore"))
                    item.RichiestaDeliberaSconto = Formats.ToValuta(dbRis("Sconto"))
                End While

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="renderer"></param>
        ''' <returns></returns>
        Public Function GetStats(ByVal renderer As Object) As String
            Dim arrPO() As Integer = Nothing
            Dim arrAnni() As Integer = Nothing
            Dim cursor As CRapportiniCursor = Nothing

            Try
                Dim items As New CKeyCollection(Of StatLiquidatoItem)
                Dim tipoG As String = RPC.n2str(GetParameter(renderer, "t", ""))
                arrPO = Me.GetArray(RPC.n2str(GetParameter(renderer, "po", "")))
                arrAnni = Me.GetArray(RPC.n2str(GetParameter(renderer, "an", "")))


                If (Arrays.Len(arrPO) <= 0) OrElse (Arrays.Len(arrAnni) <= 0) Then Return ""

                Me.statoContatto = CQSPD.StatiPratica.StatoContatto
                Me.statoRichiestaDelibera = CQSPD.StatiPratica.StatoRichiestaDelibera
                Me.statoLiquidata = CQSPD.StatiPratica.StatoLiquidato
                Me.statoArchiviata = CQSPD.StatiPratica.StatoArchiviato

                Dim strAnni As String = Me.GetStr(arrAnni)
                Dim strPO As String = Me.GetStr(arrPO)

                'Rinnovabile
                Me.AggiungiEstinzioniRinnovabili(strAnni, strPO, items)


                'Liquidato
                cursor = New CRapportiniCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Trasferita.Value = False
                cursor.Flags.Value = PraticaFlags.HIDDEN
                cursor.Flags.Operator = OP.OP_NE
                'cursor.StatoContatto.Inizio = Calendar.MakeDate(anno, 1, 1)
                'cursor.StatoContatto.IDToStato = GetID(statoContatto)
                'cursor.StatoContatto.MacroStato = Nothing
                cursor.IDPuntoOperativo.ValueIn(arrPO)
                'cursor.IDStatoAttuale.ValueIn({GetID(CQSPD.StatiPratica.StatoLiquidato), GetID(CQSPD.StatiPratica.StatoArchiviato)})

                Select Case tipoG
                    Case "DataValuta" : Me.AggiungiDataValuta(strAnni, strPO, cursor, items)
                    Case Else : Me.AggiungiLiquidato(strAnni, strPO, cursor, items)
                End Select


                'Caricato
                Me.AggiungiCaricato(strAnni, strPO, cursor, items)


                'Richiesta Delibera
                Me.AggiungiRichiestaDelibera(strAnni, strPO, cursor, items)

                cursor.Dispose() : cursor = Nothing

                items.Sort()

                Dim ret As New CKeyCollection
                ret.Add("items", items)
                ret.Add("obiettivi", Me.GetObiettivi(arrPO, Math.Min(arrAnni), Math.Max(arrAnni)))


                Return XML.Utils.Serializer.Serialize(ret)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                If (arrAnni IsNot Nothing) Then Erase arrAnni
                If (arrPO IsNot Nothing) Then Erase arrPO

            End Try
        End Function

        Private Function GetAnnoInizio() As Integer
            'Year(Now) - 1
            If CQSPD.Pratiche.Module.UserCanDoAction("seestats") Then
                Dim ret As Nullable(Of Date) = CQSPD.Database.ExecuteScalar("SELECT Min([Data]) FROM [tbl_PraticheSTL] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID)
                If (ret.HasValue) Then Return Year(ret.Value)
                Return Year(Now)
            Else
                Return Year(Now)
            End If
        End Function

        Private Function GetPercentage(ByVal val As Double, ByVal sum As Double) As Double
            If (sum = 0) Then Return Nothing
            Try
                Return val * 100 / sum
            Catch ex As Exception
                Return 0
            End Try
        End Function

        Private Function GetObiettivo(ByVal arrPO() As Integer, ByVal anno As Integer, ByVal mese As Integer) As CObiettivoPratica

            Dim ret As New CObiettivoPratica
            For i As Integer = 0 To UBound(arrPO)
                Dim po As CUfficio = Anagrafica.Uffici.GetItemById(arrPO(i))
                Dim items As CCollection(Of CObiettivoPratica) = CQSPD.Obiettivi.GetObiettiviAl(po, Calendar.MakeDate(anno, mese, 1))
                If (items.Count > 0) Then
                    Dim tmp As CObiettivoPratica = items(items.Count - 1) 'Restituiamo l'ultimo obiettivo
                    ret.MontanteLordoLiq = Math.SumNulls(ret.MontanteLordoLiq, tmp.MontanteLordoLiq)
                    ret.NumeroPraticheLiq = Math.SumNulls(ret.NumeroPraticheLiq, tmp.NumeroPraticheLiq)
                    ret.ValoreSpreadLiq = Math.SumNulls(ret.ValoreSpreadLiq, tmp.ValoreSpreadLiq)
                    ret.SpreadLiq = Math.AveNulls(ret.SpreadLiq, tmp.SpreadLiq)
                    ret.ValoreUpFront = Math.SumNulls(ret.ValoreUpFront, tmp.ValoreUpFront)
                    ret.UpFrontLiq = Math.AveNulls(ret.UpFrontLiq, tmp.UpFrontLiq)
                Else

                End If
            Next

            Return ret
        End Function

        Private Function GetObiettivi(ByVal arrPO() As Integer, ByVal annoInizio As Integer, ByVal annoFine As Integer) As CCollection(Of CObiettivoPratica)
            Dim cursor As CObiettivoPraticaCursor = Nothing

            Try
                Dim ret As New CCollection(Of CObiettivoPratica)
                cursor = New CObiettivoPraticaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDPuntoOperativo.ValueIn(arrPO)
                cursor.DataInizio.Between(Calendar.MakeDate(annoInizio), Calendar.MakeDate(annoFine, 12, 31, 23, 59, 59))
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While


                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function
    End Class
     
    Public Class StatLiquidatoItem
        Implements IComparable, DMD.XML.IDMDXMLSerializable

        Public Anno As Integer
        Public Mese As Integer

        Public RinnovabiliCnt As Integer
        Public RinnovabiliSum As Decimal
        Public RinnovabiliUpfront As Decimal
        Public RinnovabiliRunning As Decimal
        Public RinnovabiliSconto As Decimal

        Public CaricatoCnt As Integer
        Public CaricatoSum As Decimal
        Public CaricatoUpfront As Decimal
        Public CaricatoRunning As Decimal
        Public CaricatoSconto As Decimal

        Public RichiestaDeliberaCnt As Integer
        Public RichiestaDeliberaSum As Decimal
        Public RichiestaDeliberaUpfront As Decimal
        Public RichiestaDeliberaRunning As Decimal
        Public RichiestaDeliberaSconto As Decimal

        Public LiquidatoCnt As Integer
        Public LiquidatoSum As Decimal
        Public LiquidatoUpfront As Decimal
        Public LiquidatoRunning As Decimal
        Public LiquidatoSconto As Decimal

        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim o As StatLiquidatoItem = obj
            Dim ret As Integer = -Me.Anno + o.Anno
            If (ret = 0) Then ret = -Me.Mese + o.Mese
            Return ret
        End Function

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Anno" : Me.Anno = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Mese" : Me.Mese = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "RinnovabiliCnt" : Me.RinnovabiliCnt = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "RinnovabiliSum" : Me.RinnovabiliSum = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "RinnovabiliUpfront" : Me.RinnovabiliUpfront = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "RinnovabiliRunning" : Me.RinnovabiliRunning = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "RinnovabiliSconto" : Me.RinnovabiliSconto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "CaricatoCnt" : Me.CaricatoCnt = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "CaricatoSum" : Me.CaricatoSum = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "CaricatoUpfront" : Me.CaricatoUpfront = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "CaricatoRunning" : Me.CaricatoRunning = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "CaricatoSconto" : Me.CaricatoSconto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "RichiestaDeliberaCnt" : Me.RichiestaDeliberaCnt = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "RichiestaDeliberaSum" : Me.RichiestaDeliberaSum = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "RichiestaDeliberaUpfront" : Me.RichiestaDeliberaUpfront = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "RichiestaDeliberaRunning" : Me.RichiestaDeliberaRunning = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "RichiestaDeliberaSconto" : Me.RichiestaDeliberaSconto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "LiquidatoCnt" : Me.LiquidatoCnt = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "LiquidatoSum" : Me.LiquidatoSum = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "LiquidatoUpfront" : Me.LiquidatoUpfront = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "LiquidatoRunning" : Me.LiquidatoRunning = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "LiquidatoSconto" : Me.LiquidatoSconto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Anno", Me.Anno)
            writer.WriteAttribute("Mese", Me.Mese)
            writer.WriteAttribute("RinnovabiliCnt", Me.RinnovabiliCnt)
            writer.WriteAttribute("RinnovabiliSum", Me.RinnovabiliSum)
            writer.WriteAttribute("RinnovabiliUpfront", Me.RinnovabiliUpfront)
            writer.WriteAttribute("RinnovabiliRunning", Me.RinnovabiliRunning)
            writer.WriteAttribute("RinnovabiliSconto", Me.RinnovabiliSconto)
            writer.WriteAttribute("CaricatoCnt", Me.CaricatoCnt)
            writer.WriteAttribute("CaricatoSum", Me.CaricatoSum)
            writer.WriteAttribute("CaricatoUpfront", Me.CaricatoUpfront)
            writer.WriteAttribute("CaricatoRunning", Me.CaricatoRunning)
            writer.WriteAttribute("CaricatoSconto", Me.CaricatoSconto)
            writer.WriteAttribute("RichiestaDeliberaCnt", Me.RichiestaDeliberaCnt)
            writer.WriteAttribute("RichiestaDeliberaSum", Me.RichiestaDeliberaSum)
            writer.WriteAttribute("RichiestaDeliberaUpfront", Me.RichiestaDeliberaUpfront)
            writer.WriteAttribute("RichiestaDeliberaRunning", Me.RichiestaDeliberaRunning)
            writer.WriteAttribute("RichiestaDeliberaSconto", Me.RichiestaDeliberaSconto)
            writer.WriteAttribute("LiquidatoCnt", Me.LiquidatoCnt)
            writer.WriteAttribute("LiquidatoSum", Me.LiquidatoSum)
            writer.WriteAttribute("LiquidatoUpfront", Me.LiquidatoUpfront)
            writer.WriteAttribute("LiquidatoRunning", Me.LiquidatoRunning)
            writer.WriteAttribute("LiquidatoSconto", Me.LiquidatoSconto)
        End Sub
    End Class

End Namespace