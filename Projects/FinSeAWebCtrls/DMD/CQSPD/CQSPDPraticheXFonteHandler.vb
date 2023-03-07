Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms
     
    Public Class CQSPDPraticheXFonteHandler
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

                Dim statoContatto As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_CONTATTO)
                'If (statoContatto IsNot Nothing) Then arr.Add(GetID(statoContatto))
                Dim statoLiquidata As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_LIQUIDATA)
                'If (statoLiquidata IsNot Nothing) Then arr.Add(GetID(statoLiquidata))
                Dim statoArchiviata As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ARCHIVIATA)
                'If (statoArchiviata IsNot Nothing) Then arr.Add(GetID(statoArchiviata))
                Dim statoAnnullata As CStatoPratica = CQSPD.StatiPratica.GetItemByCompatibleID(StatoPraticaEnum.STATO_ANNULLATA)

                cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "cursor", "")))
                cursor.Flags.Value = PraticaFlags.HIDDEN
                cursor.Flags.Operator = OP.OP_NE
                ' Dim arr As New System.Collections.ArrayList
                'If (statoAnnullata IsNot Nothing) Then arr.Add(GetID(statoAnnullata))
                cursor.IDStatoAttuale.Clear()
                'Me.Cursor.IDStatoAttuale.ValuIn(arr.ToArray)

                dbRis = CQSPD.Database.ExecuteReader("SELECT [IDStatoAttuale], [TipoFonteContatto], [IDFonte], Count(*) As [Conteggio], Sum([MontanteLordo]) As [Montante] FROM (" & cursor.GetSQL & ") GROUP BY [TipoFonteContatto], [IDFonte], [IDStatoAttuale]")

                cursor.Dispose() : cursor = Nothing


                Dim col As New CKeyCollection(Of CRigaStatisticaPerStato)
                While dbRis.Read
                    Dim tipoFonte As String = Formats.ToString(dbRis("TipoFonteContatto"))
                    Dim idFonte As Integer = Formats.ToInteger(dbRis("IDFonte"))
                    Dim fonte As IFonte = Anagrafica.Fonti.GetItemById(tipoFonte, tipoFonte, idFonte)
                    Dim idStato As Integer = Formats.ToInteger(dbRis("IDStatoAttuale"))
                    Dim riga As CRigaStatisticaPerStato = col.GetItemByKey("R" & tipoFonte & "[" & idFonte & "]")
                    If (riga Is Nothing) Then
                        riga = New CRigaStatisticaPerStato
                        riga.Tag = tipoFonte
                        riga.Tag1 = idFonte
                        If (fonte IsNot Nothing) Then
                            riga.Descrizione = tipoFonte & " - " & fonte.Nome
                        Else
                            riga.Descrizione = tipoFonte & " (" & idFonte & ")"
                        End If
                        col.Add("R" & tipoFonte & "[" & idFonte & "]", riga)
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
                dbRis.Dispose() : dbRis = Nothing

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