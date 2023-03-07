Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls
Imports DMD.Anagrafica



Partial Public Class Office

    ''' <summary>
    ''' Aggiunge le richieste di 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StoricoHandlerRichiesteCERQ
        Inherits StoricoHandlerBase

        Protected Overrides Sub AggiungiInternal(items As CCollection(Of StoricoAction), filter As CRMFindFilter)
            Dim cnt As Integer = 0
            Dim cursor2 As New RichiestaCERQCursor
#If Not Debug Then
            Try
#End If
                cursor2.Stato.Value = ObjectStatus.OBJECT_VALID
                If filter.Dal.HasValue Then
                    cursor2.DataEffettiva.Value = filter.Dal.Value
                    cursor2.DataEffettiva.Operator = OP.OP_GT
                    If filter.Al.HasValue Then
                        cursor2.DataEffettiva.Value1 = filter.Al.Value
                        cursor2.DataEffettiva.Operator = OP.OP_BETWEEN
                    End If
                ElseIf filter.Al.HasValue Then
                    cursor2.DataEffettiva.Value = filter.Al.Value
                    cursor2.DataEffettiva.Operator = OP.OP_LE
                End If
                If filter.Contenuto <> "" Then
                    cursor2.Note.Value = filter.Contenuto & "%"
                    cursor2.Note.Operator = OP.OP_LIKE
                End If
                'If filter.DettaglioEsito Then
                'filter.etichetta
                If filter.IDOperatore <> 0 Then cursor2.IDOperatore.Value = filter.IDOperatore
                If filter.IDPuntoOperativo <> 0 Then cursor2.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                If (filter.IDPersona <> 0) Then
                    cursor2.IDCliente.Value = filter.IDPersona
                ElseIf filter.Nominativo <> "" Then
                    cursor2.NomeCliente.Value = filter.Nominativo & "%"
                    cursor2.NomeCliente.Operator = OP.OP_LIKE
                End If
                If filter.Numero <> "" Then
                    cursor2.RichiestaAIndirizzo.Value = filter.Numero & "%"
                    cursor2.RichiestaAIndirizzo.Operator = OP.OP_LIKE
                End If
                If filter.Scopo <> "" Then
                    cursor2.TipoRichiesta.Value = filter.Scopo & "%"
                    cursor2.TipoRichiesta.Operator = OP.OP_LIKE
                End If
                'If filter.StatoConversazione.HasValue Then
                '    Select Case filter.StatoConversazione
                '        Case 0 'In Attesa
                '            cursor2.StatoOperazione.Value = StatoRichiestaCERQ.DA_RICHIEDERE
                '        Case 1 'in corso
                '            cursor2.StatoOperazione.Value = StatoRichiestaCERQ.RICHIESTA
                '        Case Else
                '            cursor2.StatoOperazione.ValueIn(New Object() {StatoRichiestaCERQ.ANNULLATA, StatoRichiestaCERQ.RIFIUTATA, StatoRichiestaCERQ.RITIRATA})
                '    End Select
                'End If
                If (filter.IDContesto.HasValue) Then
                    cursor2.ContextType.Value = filter.TipoContesto
                    cursor2.ContextID.Value = filter.IDContesto
                End If
                cursor2.DataEffettiva.SortOrder = SortEnum.SORT_DESC
                cursor2.IgnoreRights = filter.IgnoreRights
            'cursor2.StatoOperazione.Value = StatoRichiestaCERQ.RITIRATA
            While Not cursor2.EOF AndAlso (Not filter.nMax.HasValue OrElse cnt < filter.nMax)
                cnt += 1
                    'items.Add(cursor2.Item)
                    Me.AddActivities(items, cursor2.Item)
                    cursor2.MoveNext()
                End While
#If Not Debug Then
            Catch ex As Exception
                Throw
            Finally
#End If
                cursor2.Dispose()
#If Not Debug Then
            End Try
#End If
        End Sub

      Private Sub AddActivities(ByVal col As CCollection(Of StoricoAction), ByVal richiesta As RichiestaCERQ)
            Dim action As New StoricoAction


            action.Data = richiesta.Data
            action.IDOperatore = richiesta.IDOperatore
            action.NomeOperatore = richiesta.NomeOperatore
            action.IDCliente = richiesta.IDCliente
            action.NomeCliente = richiesta.NomeCliente
            action.Note = "Richiesta Programmata. Motivo: <b>" & richiesta.TipoRichiesta & "</b> presso <b>" & richiesta.NomeAmministrazione & "</b>" & vbNewLine
            action.Scopo = richiesta.TipoRichiesta
            action.NumeroOIndirizzo = richiesta.RichiestaAMezzo & ": " & richiesta.RichiestaAIndirizzo
            action.Esito = EsitoChiamata.RIPOSTA
            action.DettaglioEsito = "Richiesta Programmata"
            action.Durata = 0
            action.Attesa = 0
            action.Tag = richiesta
            action.ActionSubID = StatoRichiestaCERQ.DA_RICHIEDERE
            action.StatoConversazione = StatoConversazione.CONCLUSO
            col.Add(action)

            If (richiesta.StatoOperazione >= StatoRichiestaCERQ.RICHIESTA) Then
                action = New StoricoAction
                action.Data = richiesta.DataEffettiva
                action.IDOperatore = richiesta.IDOperatore
                action.NomeOperatore = richiesta.NomeOperatore
                action.IDCliente = richiesta.IDCliente
                action.NomeCliente = richiesta.NomeCliente
                action.Note = "Richiesta Effettuata. Motivo: <b>" & richiesta.TipoRichiesta & "</b> presso <b>" & richiesta.NomeAmministrazione & "</b>" & vbNewLine
                action.Scopo = richiesta.TipoRichiesta
                action.NumeroOIndirizzo = richiesta.RichiestaAMezzo & ": " & richiesta.RichiestaAIndirizzo
                action.Esito = EsitoChiamata.RIPOSTA
                action.DettaglioEsito = "Richiesta Effettuata"
                action.Durata = 0
                action.Attesa = 0
                action.Tag = richiesta
                action.ActionSubID = StatoRichiestaCERQ.RICHIESTA
                action.StatoConversazione = StatoConversazione.CONCLUSO
                col.Add(action)
            End If

            Select Case (richiesta.StatoOperazione)
                Case StatoRichiestaCERQ.RITIRATA
                    action = New StoricoAction
                    action.Data = richiesta.DataEffettiva
                    action.IDOperatore = richiesta.IDOperatoreEffettivo
                    action.NomeOperatore = richiesta.NomeOperatoreEffettivo
                    action.IDCliente = richiesta.IDCliente
                    action.NomeCliente = richiesta.NomeCliente
                    action.Note = "Richiesta Ritirata. Motivo: <b>" & richiesta.TipoRichiesta & "</b> presso <b>" & richiesta.NomeAmministrazione & "</b>" & vbNewLine
                    action.Scopo = richiesta.TipoRichiesta
                    action.NumeroOIndirizzo = richiesta.RichiestaAMezzo & ": " & richiesta.RichiestaAIndirizzo
                    action.Esito = EsitoChiamata.RIPOSTA
                    action.DettaglioEsito = "Richiesta Ritirata"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = richiesta
                    action.ActionSubID = StatoRichiestaCERQ.RITIRATA
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)
                Case StatoRichiestaCERQ.ANNULLATA
                    action = New StoricoAction
                    action.Data = richiesta.DataEffettiva
                    action.IDOperatore = richiesta.IDOperatoreEffettivo
                    action.NomeOperatore = richiesta.NomeOperatoreEffettivo
                    action.IDCliente = richiesta.IDCliente
                    action.NomeCliente = richiesta.NomeCliente
                    action.Note = "Richiesta Annullata. Motivo: <b>" & richiesta.TipoRichiesta & "</b> presso <b>" & richiesta.NomeAmministrazione & "</b>" & vbNewLine
                    action.Scopo = richiesta.TipoRichiesta
                    action.NumeroOIndirizzo = richiesta.RichiestaAMezzo & ": " & richiesta.RichiestaAIndirizzo
                    action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    action.DettaglioEsito = "Richiesta Annullata"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = richiesta
                    action.ActionSubID = StatoRichiestaCERQ.ANNULLATA
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)
                Case StatoRichiestaCERQ.RIFIUTATA
                    action = New StoricoAction
                    action.Data = richiesta.DataEffettiva
                    action.IDOperatore = richiesta.IDOperatoreEffettivo
                    action.NomeOperatore = richiesta.NomeOperatoreEffettivo
                    action.IDCliente = richiesta.IDCliente
                    action.NomeCliente = richiesta.NomeCliente
                    action.Note = "Richiesta Rifiutata. Motivo: <b>" & richiesta.TipoRichiesta & "</b> presso <b>" & richiesta.NomeAmministrazione & "</b>" & vbNewLine
                    action.Scopo = richiesta.TipoRichiesta
                    action.NumeroOIndirizzo = richiesta.RichiestaAMezzo & ": " & richiesta.RichiestaAIndirizzo
                    action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    action.DettaglioEsito = "Richiesta Rifiutata"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = richiesta
                    action.ActionSubID = StatoRichiestaCERQ.RIFIUTATA
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)
                Case StatoRichiestaCERQ.DA_RICHIEDERE
                Case StatoRichiestaCERQ.RICHIESTA
                Case Else
                    Debug.Assert(False)
            End Select

        End Sub

         
        Protected Overrides Sub FillSupportedTypes(items As CKeyCollection(Of String))
            items.Add("RichiestaCERQ", "Richiesta")
        End Sub

    End Class


End Class