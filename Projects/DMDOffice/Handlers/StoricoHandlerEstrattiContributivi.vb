Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls
Imports DMD.Anagrafica



Partial Public Class Office

    ''' <summary>
    ''' Aggiunge gli estratti contributivi
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StoricoHandlerEstrattiContributivi
        Inherits StoricoHandlerBase

        Protected Overrides Sub AggiungiInternal(items As CCollection(Of StoricoAction), filter As CRMFindFilter)
            Dim cnt As Integer = 0
            Dim cursor1 As New EstrattiContributiviCursor


#If Not Debug Then
            Try
#End If
            cursor1.Stato.Value = ObjectStatus.OBJECT_VALID
            If filter.Dal.HasValue Then
                cursor1.DataRichiesta.Value = filter.Dal.Value
                cursor1.DataRichiesta.Operator = OP.OP_GT
                If filter.Al.HasValue Then
                    cursor1.DataRichiesta.Value1 = filter.Al.Value
                    cursor1.DataRichiesta.Operator = OP.OP_BETWEEN
                End If
            ElseIf filter.Al.HasValue Then
                cursor1.DataRichiesta.Value = filter.Al.Value
                cursor1.DataRichiesta.Operator = OP.OP_LE
            End If
            If filter.IDPersona <> 0 Then
                cursor1.IDCliente.Value = filter.IDPersona
            ElseIf filter.Nominativo <> "" Then
                cursor1.NomeCliente.Value = filter.Nominativo & "%"
                cursor1.NomeCliente.Operator = OP.OP_LIKE
            End If
            If filter.IDOperatore <> 0 Then cursor1.IDRichiedente.Value = filter.IDOperatore
            If filter.IDPuntoOperativo <> 0 Then cursor1.IDPuntoOperativo.Value = filter.IDPuntoOperativo
            If filter.Scopo <> "" Then
                'cursor1..Value = filter.Scopo & "%"
                'cursor1.Motivo.Operator = OP.OP_LIKE
                cursor1.ID.Value = 0
            End If
            If filter.StatoConversazione.HasValue Then
                Select Case filter.StatoConversazione
                    Case 0 'In Attesa
                        cursor1.StatoRichiesta.ValueIn({StatoEstrattoContributivo.DaRichiedere, StatoEstrattoContributivo.Sospeso})
                    Case 1 'in corso
                        cursor1.StatoRichiesta.ValueIn({StatoEstrattoContributivo.Richiesto, StatoEstrattoContributivo.Sospeso})
                    Case Else
                        cursor1.StatoRichiesta.ValueIn({StatoEstrattoContributivo.Errore, StatoEstrattoContributivo.Evaso})
                End Select
            End If
            If filter.Numero <> "" Then
                'cursor.NumeroOIndirizzo.Operator = OP.OP_LIKE
                cursor1.ID.Value = 0

            End If
            If (filter.IDContesto.HasValue) Then
                cursor1.SourceType.Value = filter.TipoContesto
                cursor1.SourceID.Value = filter.IDContesto
            End If
            cursor1.DataRichiesta.SortOrder = SortEnum.SORT_DESC
            cursor1.IgnoreRights = filter.IgnoreRights

            While Not cursor1.EOF AndAlso (Not filter.nMax.HasValue OrElse cnt < filter.nMax)
                cnt += 1
                Me.AddActivities(items, cursor1.Item)
                cursor1.MoveNext()
            End While
#If Not Debug Then
            Catch Ex As Exception
                Throw
            Finally
#End If
            cursor1.Dispose()
#If Not Debug Then
            End Try
#End If
        End Sub

        Private Sub AddActivities(ByVal col As CCollection(Of StoricoAction), ByVal commissione As EstrattoContributivo)
            Dim action As New StoricoAction
            action.Data = commissione.DataRichiesta
            action.IDOperatore = commissione.IDRichiedente
            action.NomeOperatore = commissione.NomeRichiedente
            action.IDCliente = commissione.IDCliente
            action.NomeCliente = commissione.NomeCliente
            action.Note = "Richiesta Etratto Contributivo -> Registrata da " & commissione.NomeRichiedente & "</b>"
            action.Scopo = ""
            action.NumeroOIndirizzo = ""
            action.Esito = EsitoChiamata.RIPOSTA
            action.DettaglioEsito = "Richiesta Registrata"
            action.Durata = 0
            action.Attesa = 0
            action.Tag = commissione
            action.ActionSubID = StatoEstrattoContributivo.Richiesto
            action.StatoConversazione = StatoConversazione.CONCLUSO

            col.Add(action)

            If (commissione.StatoRichiesta >= StatoEstrattoContributivo.Assegnato) Then
                action = New StoricoAction
                action.Data = commissione.DataAssegnazione
                action.IDOperatore = commissione.IDAssegnatoA
                action.NomeOperatore = commissione.NomeAssegnatoA
                action.IDCliente = commissione.IDCliente
                action.NomeCliente = commissione.NomeCliente
                action.Note = "Richiesta Etratto Contributivo -> Presa in carico da " & commissione.NomeAssegnatoA & "</b>"
                action.Scopo = ""
                action.NumeroOIndirizzo = ""
                action.Esito = EsitoChiamata.RIPOSTA
                action.DettaglioEsito = "Richiesta Presa In Carico"
                action.Durata = 0
                action.Attesa = 0
                action.Tag = commissione
                action.ActionSubID = StatoEstrattoContributivo.Assegnato
                action.StatoConversazione = StatoConversazione.CONCLUSO
                col.Add(action)
            End If

            Select Case (commissione.StatoRichiesta)
                Case StatoEstrattoContributivo.Evaso

                    action = New StoricoAction
                    action.Data = commissione.DataCompletamento
                    action.IDOperatore = commissione.IDAssegnatoA
                    action.NomeOperatore = commissione.NomeAssegnatoA
                    action.IDCliente = commissione.IDCliente
                    action.NomeCliente = commissione.NomeCliente
                    action.Note = "Richiesta Etratto Contributivo -> Richiesta Evasa da " & commissione.NomeAssegnatoA & "</b>"
                    action.Scopo = ""
                    action.NumeroOIndirizzo = ""
                    action.Esito = EsitoChiamata.RIPOSTA
                    action.DettaglioEsito = "Richiesta Evasa"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = commissione
                    action.ActionSubID = StatoEstrattoContributivo.Evaso
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)

                Case StatoEstrattoContributivo.Errore
                    action = New StoricoAction
                    action.Data = commissione.DataCompletamento
                    action.IDOperatore = commissione.IDAssegnatoA
                    action.NomeOperatore = commissione.NomeAssegnatoA
                    action.IDCliente = commissione.IDCliente
                    action.NomeCliente = commissione.NomeCliente
                    action.Note = "Richiesta Etratto Contributivo -> Richiesta Rigettata da " & commissione.NomeAssegnatoA & "</b>"
                    action.Scopo = ""
                    action.NumeroOIndirizzo = ""
                    action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    action.DettaglioEsito = "Richiesta Presa In Carico"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = commissione
                    action.ActionSubID = StatoEstrattoContributivo.Errore
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)

                Case StatoEstrattoContributivo.Sospeso
                    action = New StoricoAction
                    action.Data = commissione.DataCompletamento
                    action.IDOperatore = commissione.IDAssegnatoA
                    action.NomeOperatore = commissione.NomeAssegnatoA
                    action.IDCliente = commissione.IDCliente
                    action.NomeCliente = commissione.NomeCliente
                    action.Note = "Richiesta Etratto Contributivo -> Richiesta Sospesa da " & commissione.NomeAssegnatoA & "</b>"
                    action.Scopo = ""
                    action.NumeroOIndirizzo = ""
                    action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    action.DettaglioEsito = "Richiesta Sospesa"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = commissione
                    action.ActionSubID = StatoEstrattoContributivo.Sospeso
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)
                 
                Case Else
                    ' Debug.Assert(False)
            End Select

        End Sub

        Protected Overrides Sub FillSupportedTypes(items As CKeyCollection(Of String))
            items.Add("EstrattoContributivo", "Estratto Contributivo")
        End Sub


        Public Sub New()

        End Sub
    End Class


End Class