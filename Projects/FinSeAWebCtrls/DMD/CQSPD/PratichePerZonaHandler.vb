Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms

    Public Class PratichePerZonaHandler
        Inherits CQSPDBaseStatsHandler

        Public Sub New()
            Me.UseLocal = False
        End Sub


        Private Function GetComuniStr(ByVal z As CZona) As String
            Dim ret As New System.Text.StringBuilder
            For Each c As CComune In z.Comuni
                If ret.Length > 0 Then ret.Append(",")
                ret.Append(DBUtils.DBString(c.Nome & " (" & c.Provincia & ")"))
                ret.Append(",")
                ret.Append(DBUtils.DBString(c.Nome & " (" & c.Sigla & ")"))
            Next
            Return ret.ToString
        End Function



        Public Function GetStats(ByVal renderer As Object) As String
            If Not Me.Module.UserCanDoAction("list") Then Throw New PermissionDeniedException(Me.Module, "list")

            'Dim arr As New System.Collections.ArrayList
            Dim statoContatto As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_CONTATTO)
            'If (statoContatto IsNot Nothing) Then arr.Add(GetID(statoContatto))
            Dim statoLiquidata As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA)
            'If (statoLiquidata IsNot Nothing) Then arr.Add(GetID(statoLiquidata))
            Dim statoArchiviata As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA)
            'If (statoArchiviata IsNot Nothing) Then arr.Add(GetID(statoArchiviata))
            Dim statoAnnullata As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)
            'If (statoAnnullata IsNot Nothing) Then arr.Add(GetID(statoAnnullata))


            Dim cursor As CRapportiniCursor = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "cursor", "")))
            cursor.Flags.Value = PraticaFlags.HIDDEN
            cursor.Flags.Operator = OP.OP_NE
            cursor.IDStatoAttuale.Clear()



            Dim zcursor As New CZonaCursor
            Dim righe As New CRigaStatisticaPerStatoCollection

            zcursor.Nome.SortOrder = SortEnum.SORT_ASC
            zcursor.Stato.Value = ObjectStatus.OBJECT_VALID
            Dim zona As CZona
            Dim zone As New CKeyCollection(Of CZona)

            While Not zcursor.EOF
                zona = zcursor.Item
                zone.Add(zona.Nome, zona)
                Dim riga As CRigaStatisticaPerStato = righe.GetItemByKey(zona.Nome)
                If (riga Is Nothing) Then riga = righe.Add(zona.Nome)

                If (zona.Comuni.Count > 0) Then
                    cursor.WhereClauses.Clear()
                    cursor.WhereClauses.Add("UCase([ResidenteAComune] & ' (' & [ResidenteAProvincia] & ')' In (" & Me.GetComuniStr(zona) & "))")

                    Dim dbSQL As String
                    dbSQL = "SELECT [IDStatoAttuale], Count(*) As [Conteggio], Sum([MontanteLordo]) As [Montante] FROM ("
                    dbSQL &= cursor.GetSQL
                    dbSQL &= ") GROUP BY [IDStatoAttuale]"

                    If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                    Dim dbRis As System.Data.IDataReader = CQSPD.Database.ExecuteReader(dbSQL)
                    While dbRis.Read
                        Dim idStato As Integer = Formats.ToInteger(dbRis("IDStatoAttuale"))
                        If (idStato = GetID(statoArchiviata) OrElse idStato = GetID(statoLiquidata)) Then
                            With riga.Item(InfoStatoEnum.LIQUIDATO)
                                .Conteggio = Formats.ToInteger(dbRis("Conteggio"))
                                .Montante = Formats.ToDouble(dbRis("Montante"))
                            End With
                        ElseIf (idStato = GetID(statoAnnullata)) Then
                            With riga.Item(InfoStatoEnum.ANNULLATO)
                                .Conteggio = Formats.ToInteger(dbRis("Conteggio"))
                                .Montante = Formats.ToDouble(dbRis("Montante"))
                            End With
                        ElseIf (idStato = GetID(statoContatto)) Then
                            With riga.Item(InfoStatoEnum.CONTATTO)
                                .Conteggio = Formats.ToInteger(dbRis("Conteggio"))
                                .Montante = Formats.ToDouble(dbRis("Montante"))
                            End With
                        Else
                            With riga.Item(InfoStatoEnum.ALTRO)
                                .Conteggio += Formats.ToInteger(dbRis("Conteggio"))
                                .Montante += Formats.ToDouble(dbRis("Montante"))
                            End With
                        End If

                    End While
                    dbRis.Dispose()
                End If

                zcursor.MoveNext()
            End While
            zcursor.Dispose() : zcursor = Nothing

            Return XML.Utils.Serializer.Serialize(righe)
        End Function

    End Class

   


End Namespace