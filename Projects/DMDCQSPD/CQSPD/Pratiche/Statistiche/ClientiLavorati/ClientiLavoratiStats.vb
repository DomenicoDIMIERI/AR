Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CustomerCalls

Partial Public Class CQSPD

     
    <Serializable> _
    Public Class ClientiLavoratiStats
        Implements DMD.XML.IDMDXMLSerializable

        Public items As CCollection(Of ClientiLavoratiStatsItem)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.items = New CCollection(Of ClientiLavoratiStatsItem)
        End Sub

        Public Sub Apply(ByVal filter As ClientiLavoratiFilter)
            Dim col As New CKeyCollection(Of ClientiLavoratiStatsItem)
            'Dim t1, t2 As Double
            't1 = Timer
            'Me.AggiungiVisite(filter, col)
            't2 = Timer : Debug.Print("AggiungiVisite " & (t2 - t1)) : t1 = t2
            'Me.AggiungiRichieste(filter, col)
            't2 = Timer : Debug.Print("AggiungiRichieste " & (t2 - t1)) : t1 = t2
            'Me.AggiungiConsulenze(filter, col)
            't2 = Timer : Debug.Print("AggiungiConsulenze " & (t2 - t1)) : t1 = t2
            'Me.AggiungiPratiche(filter, col)
            't2 = Timer : Debug.Print("AggiungiPratiche " & (t2 - t1)) : t1 = t2
            ''For Each item As ClientiLavoratiStatsItem In items


            'Next
            'Me.items = New CCollection(Of ClientiLavoratiStatsItem)(col)

            If (filter Is Nothing) Then Throw New ArgumentNullException("filter")
            Dim cursor As New ClientiLavoratiStatsItemCursor
            If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
            If (filter.DataInizio.HasValue) Then
                cursor.DataInizioLavorazione.IncludeNulls = True
                cursor.DataInizioLavorazione.Value = filter.DataInizio.Value
                cursor.DataInizioLavorazione.Operator = OP.OP_GE
                If (filter.DataFine.HasValue) Then
                    cursor.DataInizioLavorazione.Value1 = filter.DataFine.Value
                    cursor.DataInizioLavorazione.Operator = OP.OP_BETWEEN
                End If
            End If
            While Not cursor.EOF
                Dim item As ClientiLavoratiStatsItem = cursor.Item
                Dim oItem As ClientiLavoratiStatsItem = col.GetItemByKey("K" & item.IDCliente)
                If (oItem IsNot Nothing) Then
                    oItem.MergeWith(item)
                Else
                    col.Add("K" & item.IDCliente, item)
                End If
                cursor.MoveNext()
            End While
            cursor.Dispose()

            Me.items = New CCollection(Of ClientiLavoratiStatsItem)(col)
        End Sub
#If 0 Then

        Private Sub AggiungiVisite(ByVal filter As ClientiLavoratiFilter, ByVal items As CKeyCollection(Of ClientiLavoratiStatsItem))
            Dim visita As CVisita
            Dim dbSQL As String
            Dim dbRis As System.Data.IDataReader
            Dim item As ClientiLavoratiStatsItem
            
            dbSQL = "SELECT [T1].* FROM "
            dbSQL &= "("
            dbSQL &= "SELECT * FROM [tbl_Telefonate] "
            dbSQL &= " WHERE [ClassName]='CVisita' AND [Stato]=" & ObjectStatus.OBJECT_VALID
            If (filter.DataInizio.HasValue) Then dbSQL &= " AND [Data]>=" & DBUtils.DBDate(filter.DataInizio.Value)
            If (filter.DataFine.HasValue) Then dbSQL &= " AND [Data]<=" & DBUtils.DBDate(filter.DataFine.Value)
            dbSQL &= ") AS T1 "
            dbSQL &= " INNER JOIN "
            dbSQL &= "("
            dbSQL &= "SELECT * FROM [tbl_UltimaChiamata] "
            If (filter.IDPuntoOperativo <> 0) Then dbSQL &= " WHERE [IDPuntoOperativo]=" & filter.IDPuntoOperativo
            dbSQL &= ") AS T2"
            dbSQL &= " ON [T1].[ID]=[T2].[IDUltimaVisita]"

            'dbSQL = "SELECT * FROM [tbl_Telefonate] WHERE [ClassName]='CVisita' AND [Stato]=" & ObjectStatus.OBJECT_VALID

            dbRis = CRM.TelDB.ExecuteReader(dbSQL)
            While dbRis.Read
                visita = New CVisita
                CRM.TelDB.Load(visita, dbRis)
                If (visita.IDPerContoDi <> 0) Then
                    If TypeOf (visita.PerContoDi) Is CPersonaFisica Then
                        item = items.GetItemByKey("C" & visita.IDPerContoDi)
                        If (item Is Nothing) Then
                            item = New ClientiLavoratiStatsItem
                            item.Cliente = visita.PerContoDi
                            items.Add("C" & visita.IDPerContoDi, item)
                        End If
                        item.Visite.Add(visita)
                    End If
                ElseIf TypeOf (visita.Persona) Is CPersonaFisica Then
                    item = items.GetItemByKey("C" & visita.IDPersona)
                    If (item Is Nothing) Then
                        item = New ClientiLavoratiStatsItem
                        item.Cliente = visita.Persona
                        items.Add("C" & visita.IDPersona, item)
                    End If
                    item.Visite.Add(visita)
                End If
            End While
            dbRis.Dispose()
        End Sub

        Private Sub AggiungiRichieste(ByVal filter As ClientiLavoratiFilter, ByVal items As CKeyCollection(Of ClientiLavoratiStatsItem))
            Dim richiesta As CRichiestaFinanziamento
            Dim dbSQL As String
            Dim dbRis As System.Data.IDataReader
            Dim item As ClientiLavoratiStatsItem

            dbSQL = "SELECT * FROM [tbl_RichiesteFinanziamenti] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID
            If (filter.IDPuntoOperativo <> 0) Then dbSQL &= " AND [IDPuntoOperativo]=" & filter.IDPuntoOperativo
            If (filter.DataInizio.HasValue) Then dbSQL &= " AND [Data]>=" & DBUtils.DBDate(filter.DataInizio.Value)
            If (filter.DataFine.HasValue) Then dbSQL &= " AND [Data]<=" & DBUtils.DBDate(filter.DataFine.Value)

            dbRis = CQSPD.Database.ExecuteReader(dbSQL)
            While dbRis.Read
                richiesta = New CRichiestaFinanziamento
                CQSPD.Database.Load(richiesta, dbRis)
                item = items.GetItemByKey("C" & richiesta.IDCliente)
                If (item Is Nothing) Then
                    item = New ClientiLavoratiStatsItem
                    item.Cliente = richiesta.Cliente
                    items.Add("C" & richiesta.IDCliente, item)
                End If
                item.RichiesteFinanziamenti.Add(richiesta)
            End While
            dbRis.Dispose()
        End Sub

        Private Sub AggiungiConsulenze(ByVal filter As ClientiLavoratiFilter, ByVal items As CKeyCollection(Of ClientiLavoratiStatsItem))
            Dim dbSQL As String
            Dim dbRis As System.Data.IDataReader
            Dim item As ClientiLavoratiStatsItem
            Dim consulenza As CQSPDConsulenza

            dbSQL = "SELECT * FROM [tbl_CQSPDConsulenze] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID
            If (filter.IDPuntoOperativo <> 0) Then dbSQL &= " AND [IDPuntoOperativo]=" & filter.IDPuntoOperativo
            If (filter.DataInizio.HasValue) Then dbSQL &= " AND [DataConsulenza]>=" & DBUtils.DBDate(filter.DataInizio.Value)
            If (filter.DataFine.HasValue) Then dbSQL &= " AND [DataConsulenza]<=" & DBUtils.DBDate(filter.DataFine.Value)

            dbRis = CQSPD.Database.ExecuteReader(dbSQL)
            While dbRis.Read
                consulenza = New CQSPDConsulenza
                CQSPD.Database.Load(consulenza, dbRis)
                item = items.GetItemByKey("C" & consulenza.IDCliente)
                If (item Is Nothing) Then
                    item = New ClientiLavoratiStatsItem
                    item.Cliente = consulenza.Cliente
                    items.Add("C" & consulenza.IDCliente, item)
                End If
                item.Consulenze.Add(consulenza)
            End While
            dbRis.Dispose()
        End Sub

        Private Sub AggiungiPratiche(ByVal filter As ClientiLavoratiFilter, ByVal items As CKeyCollection(Of ClientiLavoratiStatsItem))
            Dim dbSQL As String
            Dim dbRis As System.Data.IDataReader
            Dim item As ClientiLavoratiStatsItem
            Dim pratica As CRapportino

            If (filter.DataInizio.HasValue OrElse filter.DataFine.HasValue) Then
                dbSQL = ""
                dbSQL &= "SELECT [tbl_Rapportini].* FROM [tbl_Rapportini] "
                dbSQL &= " INNER JOIN "
                dbSQL &= "(SELECT DISTINCT [IDPratica] FROM [tbl_PraticheSTL] WHERE "
                If (filter.DataInizio.HasValue) Then
                    dbSQL &= " [Data]>=" & DBUtils.DBDate(filter.DataInizio.Value)
                    If (filter.DataFine.HasValue) Then
                        dbSQL &= " AND [Data]<=" & DBUtils.DBDate(filter.DataFine.Value)
                    End If
                Else
                    dbSQL &= " [Data]<=" & DBUtils.DBDate(filter.DataFine.Value)
                End If

                dbSQL &= ") AS [T1] ON [tbl_Rapportini].[ID] = [T1].[IDPratica]"

                dbSQL &= " WHERE [Stato]=" & ObjectStatus.OBJECT_VALID
            Else
                dbSQL = "SELECT * FROM [tbl_Rapportini] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID
            End If
            If (filter.IDPuntoOperativo <> 0) Then dbSQL &= " AND [IDPuntoOperativo]=" & filter.IDPuntoOperativo

            dbRis = CQSPD.Database.ExecuteReader(dbSQL)
            While dbRis.Read
                pratica = New CRapportino
                CQSPD.Database.Load(pratica, dbRis)
                item = items.GetItemByKey("C" & pratica.IDCliente)
                If (item Is Nothing) Then
                    item = New ClientiLavoratiStatsItem
                    item.Cliente = pratica.Cliente
                    items.Add("C" & pratica.IDCliente, item)
                End If
                item.Pratiche.Add(pratica)
            End While
            dbRis.Dispose()
        End Sub

#End If


        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "items" : Me.items.Clear() : Me.items.AddRange(fieldValue)
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteTag("items", Me.items)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class




End Class
