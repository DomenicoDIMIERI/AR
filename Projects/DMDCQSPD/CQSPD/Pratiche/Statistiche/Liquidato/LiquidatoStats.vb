Imports DMD
Imports DMD.Databases

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class CQSPD

    ''' <summary>
    ''' Rappresenta le statistiche sul liquidato
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LiquidatoStats
        Implements DMD.XML.IDMDXMLSerializable

        Public items As CKeyCollection(Of LiquidatoStatsItem)
        Public charts As CKeyCollection(Of String)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Overridable Sub Apply(ByVal filter As LiquidatoFilter)
            Me.items.Clear()
            If (filter.Anni.Count = 0 OrElse filter.PuntiOperativi.Count = 0) Then Exit Sub

            Dim items As New CKeyCollection(Of LiquidatoStatsItem)
            Dim arrPO() As Integer = filter.PuntiOperativi.ToArray
            Dim arrAnni() As Integer = filter.Anni.ToArray
            Dim lAnno As Integer = -1


            Dim strAnni As String = ""
            For i As Integer = 0 To UBound(arrAnni)
                If i > 0 Then strAnni &= ","
                strAnni &= CStr(arrAnni(i))
            Next

            Dim statoArchiviata As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA)
            Dim statoContatto As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_CONTATTO)
            Dim statoLiquidata As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA)
            Dim statoRichiestaDelibera As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_RICHIESTADELIBERA)
            Dim dbRis As System.Data.IDataReader
            Dim item As LiquidatoStatsItem
            Dim dbSQL As String

            'Liquidato
            Dim cursor As New CRapportiniCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.Trasferita.Value = False
            cursor.Flags.Value = PraticaFlags.HIDDEN
            cursor.Flags.Operator = OP.OP_NE
            'cursor.StatoContatto.Inizio = DateUtils.MakeDate(anno, 1, 1)
            'cursor.StatoContatto.IDToStato = GetID(statoContatto)
            'cursor.StatoContatto.MacroStato = Nothing
            cursor.IDPuntoOperativo.ValueIn(arrPO)

            'Dim strSql As String


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
            dbSQL &= "SELECT * FROM [tbl_PraticheSTL]  WHERE Year([tbl_PraticheSTL].[Data]) In (" & strAnni & ") AND [tbl_PraticheSTL].[IDToStato] In (" & GetID(statoLiquidata) & ")"
            dbSQL &= ") AS T2 "
            dbSQL &= "ON [T1].[ID]=[T2].[IDPratica] "
            dbSQL &= "GROUP BY Year([T2].[Data]), Month([T2].[Data]) "
            dbRis = CQSPD.Database.ExecuteReader(dbSQL)
            While dbRis.Read
                item = New LiquidatoStatsItem
                item.Anno = Formats.ToInteger(dbRis("Anno"))
                item.Mese = Formats.ToInteger(dbRis("Mese"))
                item.LiquidatoUpfront = Formats.ToValuta(dbRis("Upfront"))
                item.LiquidatoRunning = Formats.ToValuta(dbRis("Running"))
                item.LiquidatoCnt = Formats.ToInteger(dbRis("Cnt"))
                item.LiquidatoSum = Formats.ToValuta(dbRis("Valore"))
                item.LiquidatoSconto = Formats.ToValuta(dbRis("Sconto"))
                items.Add("K" & item.Anno & "_" & item.Mese, item)
            End While

            'Caricato
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
            dbSQL &= "SELECT * FROM [tbl_PraticheSTL]  WHERE Year([tbl_PraticheSTL].[Data]) In (" & strAnni & ") AND [tbl_PraticheSTL].[IDToStato] In (" & GetID(statoContatto) & ")"
            dbSQL &= ") AS T2 "
            dbSQL &= "ON [T1].[ID]=[T2].[IDPratica] "
            dbSQL &= "GROUP BY Year([T2].[Data]), Month([T2].[Data]) "
            dbRis = CQSPD.Database.ExecuteReader(dbSQL)
            While dbRis.Read
                item = items.GetItemByKey("K" & Formats.ToInteger(dbRis("Anno")) & "_" & Formats.ToInteger(dbRis("Mese")))

                If (item Is Nothing) Then
                    item = New LiquidatoStatsItem
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

            'Richiesta Delibera
            'dbSQL = "SELECT Count(*) As [Cnt], Sum([T1].[MontanteLordo]) As [Valore], Sum([T1].[Running]) As [Running], Sum([T1].[UpFront]) As [UpFront], Month([T2].[Data]) As [Mese], Year([T2].[Data]) As [Anno] FROM ("
            'dbSQL &= cursor.GetSQL & ") AS [T1] "
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
            dbSQL &= "SELECT * FROM [tbl_PraticheSTL]  WHERE Year([tbl_PraticheSTL].[Data]) In (" & strAnni & ") AND [tbl_PraticheSTL].[IDToStato] In (" & GetID(statoRichiestaDelibera) & ")"
            dbSQL &= ") AS [T2] "
            dbSQL &= "ON [T1].[ID]=[T2].[IDPratica] "
            dbSQL &= "GROUP BY Year([T2].[Data]), Month([T2].[Data]) "
            dbRis = CQSPD.Database.ExecuteReader(dbSQL)
            While dbRis.Read
                item = items.GetItemByKey("K" & Formats.ToInteger(dbRis("Anno")) & "_" & Formats.ToInteger(dbRis("Mese")))

                If (item Is Nothing) Then
                    item = New LiquidatoStatsItem
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

            Me.items.AddRange(items)
            Me.items.Sort()

        End Sub



        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "items"
                    Me.items.Clear()
                    Dim tmp As CKeyCollection = fieldValue
                    For Each k As String In tmp.Keys
                        Me.items.Add(k, tmp(k))
                    Next
                Case "charts"
                    Me.charts.Clear()
                    Dim tmp As CKeyCollection = fieldValue
                    For Each k As String In tmp.Keys
                        Me.charts.Add(k, tmp(k))
                    Next
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteTag("items", Me.items)
            writer.WriteTag("charts", Me.charts)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class