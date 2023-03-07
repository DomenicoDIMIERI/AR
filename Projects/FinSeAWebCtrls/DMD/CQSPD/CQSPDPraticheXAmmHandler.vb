Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms
     
    Public Class CQSPDPraticheXAmmHandler
        Inherits CQSPDBaseStatsHandler

        Public Sub New()
            Me.UseLocal = False
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CRapportiniCursor
        End Function

        Public Function GetStats(ByVal renderer As Object) As String
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim cursor As CRapportiniCursor = Nothing

            Try
                If Not Me.Module.UserCanDoAction("list") Then Throw New PermissionDeniedException(Me.Module, "list")

                Dim dbSQL As String
                Dim tipo As String = RPC.n2str(GetParameter(renderer, "tipo", ""))

                cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "cursor", "")))
                cursor.Flags.Value = PraticaFlags.HIDDEN
                cursor.Flags.Operator = OP.OP_NE
                ' Dim arr As New System.Collections.ArrayList
                Dim statoContatto As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_CONTATTO)
                'If (statoContatto IsNot Nothing) Then arr.Add(GetID(statoContatto))
                Dim statoLiquidata As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA)
                'If (statoLiquidata IsNot Nothing) Then arr.Add(GetID(statoLiquidata))
                Dim statoArchiviata As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA)
                'If (statoArchiviata IsNot Nothing) Then arr.Add(GetID(statoArchiviata))
                Dim statoAnnullata As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)
                'If (statoAnnullata IsNot Nothing) Then arr.Add(GetID(statoAnnullata))

                cursor.IDStatoAttuale.Clear()
                'Me.Cursor.IDStatoAttuale.ValuIn(arr.ToArray)

                Select Case tipo
                    Case "Ente Pagante"
                        dbSQL = "SELECT [IDStatoAttuale], Count(*) As [Conteggio], Sum([MontanteLordo]) As [Montante], [IDEntePagante] As [IDEnte], [NomeEntePagante] As [NomeEnte] FROM (" & cursor.GetSQL & ") GROUP BY [IDEntePagante], [NomeEntePagante], [IDStatoAttuale]"
                    Case Else
                        dbSQL = "SELECT [IDStatoAttuale], Count(*) As [Conteggio], Sum([MontanteLordo]) As [Montante], [IDAmministrazione] As [IDEnte], [Ente] As [NomeEnte] FROM (" & cursor.GetSQL & ") GROUP BY [IDAmministrazione], [Ente], [IDStatoAttuale]"
                End Select

                dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                Dim col As New CKeyCollection(Of CRigaStatisticaPerStato)
                Dim riga As CRigaStatisticaPerStato

                While dbRis.Read
                    Dim idEnte As Integer = Formats.ToInteger(dbRis("IDEnte"))
                    Dim nomeEnte As String = Trim(Formats.ToString(dbRis("NomeEnte")))
                    Dim idStato As Integer = Formats.ToInteger(dbRis("IDStatoAttuale"))
                    riga = col.GetItemByKey("R" & idEnte)
                    If (riga Is Nothing) Then
                        riga = New CRigaStatisticaPerStato
                        riga.Tag = idEnte
                        riga.Descrizione = IIf(nomeEnte <> "", nomeEnte, "[" & idEnte & "]")
                        col.Add("R" & idEnte, riga)
                    End If

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
                Return XML.Utils.Serializer.Serialize(col)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            End Try
        End Function

    End Class

  
End Namespace