Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms

    Public Class ReportPratichePerComuneHandler
        Inherits CQSPDBaseStatsHandler

        Public Sub New()
            Me.UseLocal = False
        End Sub



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

            Dim dbRis As System.Data.IDataReader = CQSPD.Database.ExecuteReader("SELECT UCase([ResidenteAComune] & ' (' & [ResidenteAProvincia] & ')') As [Residenza], [IDStatoAttuale], Count(*) As [Conteggio], Sum([MontanteLordo]) As [Montante] FROM (" & cursor.GetSQL & ") GROUP By UCase([ResidenteAComune] & ' (' & [ResidenteAProvincia] & ')'), [IDStatoAttuale] ")

            cursor.Dispose()

            Dim col As New CRigaStatisticaPerStatoCollection
            While dbRis.Read
                Dim key As String = Formats.ToString(dbRis("Residenza"))
                Dim idStato As Integer = Formats.ToInteger(dbRis("IDStatoAttuale"))
                Dim riga As CRigaStatisticaPerStato = col.GetItemByKey(key)
                If (riga Is Nothing) Then riga = col.Add(key)
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
            dbRis = Nothing

            Return XML.Utils.Serializer.Serialize(col)
        End Function

    End Class


End Namespace