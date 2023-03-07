Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls
Imports DMD.Anagrafica



Partial Public Class Office

    ''' <summary>
    ''' Aggiunge le richieste di conteggi estintivi
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StoricoHandlerTickets
        Inherits StoricoHandlerBase

        Protected Overrides Sub AggiungiInternal(items As CCollection(Of StoricoAction), filter As CRMFindFilter)
            Dim cursor As New Office.CTicketCursor
            Dim cnt As Integer = 0

#If Not Debug Then
            Try
#End If
            If (filter.IDPersona <> 0) Then
                'cursor.IDPersona.Value = filter.IDPersona
                cursor.IDCliente.Value = filter.IDPersona
            Else
                If (filter.Nominativo <> "") Then
                    'cursor.NomePersona.Operator = OP.OP_LIKE
                    'cursor.NomePersona.Value = filter.Nominativo & "%"

                    cursor.NomeCliente.Value = filter.Nominativo & "%"
                    cursor.NomeCliente.Operator = OP.OP_LIKE
                End If
            End If
            If filter.IDOperatore <> 0 Then cursor.IDInCaricoA.Value = filter.IDOperatore
            If filter.IDPuntoOperativo <> 0 Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
            If filter.Contenuto <> "" Then
                cursor.Messaggio.Value = filter.Contenuto
                cursor.Messaggio.Operator = OP.OP_LIKE
            End If
            If filter.Etichetta <> "" Then
                'cursor..Value = filter.Etichetta & "%"
                'cursor.NomeIndirizzo.Operator = OP.OP_LIKE
            End If
            If filter.Numero <> "" Then
                Try
                    Dim id As Integer = CInt("&H" & filter.Numero)
                    cursor.ID.Value = id
                Catch ex As Exception
                    cursor.ID.Value = 0
                End Try
                'cursor.NumeroOIndirizzo.Operator = OP.OP_LIKE
            End If
            If filter.Dal.HasValue Then
                cursor.DataRichiesta.Value = filter.Dal.Value
                cursor.DataRichiesta.Operator = OP.OP_GT
                If filter.Al.HasValue Then
                    cursor.DataRichiesta.Value1 = filter.Al.Value
                    cursor.DataRichiesta.Operator = OP.OP_BETWEEN
                End If
            ElseIf filter.Al.HasValue Then
                cursor.DataRichiesta.Value = filter.Al.Value
                cursor.DataRichiesta.Operator = OP.OP_LE
            End If
            If filter.Scopo <> "" Then
                cursor.Categoria.Value = filter.Scopo & "%"
                cursor.Categoria.Operator = OP.OP_LIKE
            End If
            'If (filter.Esito.HasValue) Then cursor.StatoSegnalazione.Value = IIf( filter.Esito.Value, 
            'If (filter.IDContesto.HasValue) Then
            '    cursor.Contesto.Value = filter.TipoContesto
            '    cursor.IDContesto.Value = filter.IDContesto
            'End If
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            ' cursor.Data.SortOrder = SortEnum.SORT_DESC
            cursor.IgnoreRights = filter.IgnoreRights
            'If (filter.StatoConversazione.HasValue) Then cursor.StatoConversazione.Value = filter.StatoConversazione

            cursor.DataRichiesta.SortOrder = SortEnum.SORT_DESC
                While Not cursor.EOF
                    cnt += 1
                Dim item As CTicket = cursor.Item
                Me.AddActivities(items, item)
                cursor.MoveNext()
            End While
#If Not Debug Then
            Catch ex As Exception
                Throw
            Finally
#End If
            'cursor.Dispose()
            cursor.Dispose()
#If Not Debug Then
            End Try
#End If
        End Sub

        Private Sub AddActivities(ByVal col As CCollection(Of StoricoAction), ByVal res As Office.CTicket)
            Dim action As New StoricoAction
            Dim actionSubID As Integer = 0

            action.Data = res.DataRichiesta
            action.IDOperatore = res.IDApertoDa
            action.NomeOperatore = res.NomeApertoDa
            action.IDCliente = res.IDCliente
            action.NomeCliente = res.NomeCliente
            action.Note = "Ticket Aperto: " & res.Messaggio
            action.Scopo = res.Categoria & " / " & res.Sottocategoria
            action.NumeroOIndirizzo = res.NumberEx
            action.Durata = 0
            action.Attesa = 0
            action.Tag = res
            action.Ricevuta = False
            action.ActionSubID = actionSubID : actionSubID += 1
            Select Case res.StatoSegnalazione
                Case TicketStatus.RISOLTO, TicketStatus.NONRISOLVIBILE
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                Case TicketStatus.INSERITO, TicketStatus.APERTO
                    action.StatoConversazione = StatoConversazione.INATTESA
                Case Else
                    action.StatoConversazione = StatoConversazione.INCORSO
            End Select
            Select Case res.StatoSegnalazione
                Case TicketStatus.RISOLTO
                    action.Esito = EsitoChiamata.RIPOSTA
                    action.DettaglioEsito = "Risolto"
                Case TicketStatus.NONRISOLVIBILE
                    action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    action.DettaglioEsito = "Non Risolvibile"
                Case TicketStatus.APERTO
                    action.Esito = EsitoChiamata.ALTRO
                    action.DettaglioEsito = "In Attesa"
                Case Else
                    action.Esito = EsitoChiamata.ALTRO
                    action.DettaglioEsito = "In Lavorazione"
            End Select
            col.Add(action)

            For Each m As CTicketAnsware In res.Messages
                action = New StoricoAction
                action.Data = m.Data
                action.IDOperatore = m.IDOperatore
                action.NomeOperatore = m.NomeOperatore
                action.IDCliente = res.IDCliente
                action.NomeCliente = res.NomeCliente
                action.Note = "Ticket Aggiornato: " & m.Messaggio
                action.Scopo = res.Categoria & " / " & res.Sottocategoria
                action.NumeroOIndirizzo = res.NumberEx
                action.Esito = EsitoChiamata.RIPOSTA
                action.Durata = 0
                action.Attesa = 0
                action.Tag = res
                action.Ricevuta = False
                action.ActionSubID = actionSubID : actionSubID += 1
                Select Case res.StatoSegnalazione
                    Case TicketStatus.RISOLTO, TicketStatus.NONRISOLVIBILE
                        action.StatoConversazione = StatoConversazione.CONCLUSO
                    Case TicketStatus.INSERITO, TicketStatus.APERTO
                        action.StatoConversazione = StatoConversazione.INATTESA
                    Case Else
                        action.StatoConversazione = StatoConversazione.INCORSO
                End Select
                Select Case m.StatoTicket
                    Case TicketStatus.RISOLTO
                        action.Esito = EsitoChiamata.RIPOSTA
                        action.DettaglioEsito = "Risolto"
                    Case TicketStatus.NONRISOLVIBILE
                        action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                        action.DettaglioEsito = "Non Risolvibile"
                    Case TicketStatus.APERTO
                        action.Esito = EsitoChiamata.ALTRO
                        action.DettaglioEsito = "In Attesa"
                    Case Else
                        action.Esito = EsitoChiamata.ALTRO
                        action.DettaglioEsito = "In Lavorazione"
                End Select

                col.Add(action)
            Next

            'Select Case res.StatoSegnalazione
            '    Case TicketStatus.RISOLTO, TicketStatus.NONRISOLVIBILE
            '        action = New StoricoAction
            '        action.Data = res.DataPresaInCarico
            '        action.IDOperatore = res.IDInCaricoA
            '        action.NomeOperatore = res.NomeInCaricoA
            '        action.IDCliente = res.IDCliente
            '        action.NomeCliente = res.NomeCliente
            '        action.Note = "Ticket Assegnato: " & res.Messaggio
            '        action.Scopo = res.Categoria & " / " & res.Sottocategoria
            '        action.NumeroOIndirizzo = res.NumberEx
            '        action.Esito = EsitoChiamata.RIPOSTA
            '        action.DettaglioEsito = "Preso in carico"
            '        action.Durata = 0
            '        action.Attesa = 0
            '        action.Tag = res
            '        action.StatoConversazione = StatoConversazione.CONCLUSO
            '        action.Ricevuta = False
            '        action.ActionSubID = actionSubID : actionSubID += 1
            '        col.Add(action)

            '        action = New StoricoAction
            '        action.Data = res.GetDataUltimoAggiornamento
            '        action.IDOperatore = res.IDInCaricoA
            '        action.NomeOperatore = res.NomeInCaricoA
            '        action.IDCliente = res.IDCliente
            '        action.NomeCliente = res.NomeCliente
            '        action.Note = "Ticket " & CStr(IIf(res.StatoSegnalazione = TicketStatus.RISOLTO, "Risolto", "Non Risolvibile")) & ": " & res.Messaggio
            '        action.Scopo = res.Categoria & " / " & res.Sottocategoria
            '        action.NumeroOIndirizzo = res.NumberEx
            '        action.Esito = IIf(res.StatoSegnalazione = TicketStatus.RISOLTO, EsitoChiamata.RIPOSTA, EsitoChiamata.NESSUNA_RISPOSTA)
            '        action.DettaglioEsito = action.Note
            '        action.StatoConversazione = StatoConversazione.CONCLUSO
            '        action.Durata = 0
            '        action.Attesa = 0
            '        action.Tag = res
            '        action.Ricevuta = False
            '        action.ActionSubID = actionSubID : actionSubID += 1
            '        col.Add(action)

            '    Case TicketStatus.INSERITO, TicketStatus.APERTO

            '    Case Else
            '        action = New StoricoAction
            '        action.Data = res.DataPresaInCarico
            '        action.IDOperatore = res.IDInCaricoA
            '        action.NomeOperatore = res.NomeInCaricoA
            '        action.IDCliente = res.IDCliente
            '        action.NomeCliente = res.NomeCliente
            '        action.Note = "Ticket Assegnato: " & res.Messaggio
            '        action.Scopo = res.Categoria & " / " & res.Sottocategoria
            '        action.NumeroOIndirizzo = res.NumberEx
            '        action.Esito = EsitoChiamata.RIPOSTA
            '        action.DettaglioEsito = "Ticket Preso in carico"
            '        action.Durata = 0
            '        action.Attesa = 0
            '        action.Tag = res
            '        action.StatoConversazione = StatoConversazione.INCORSO
            '        action.Ricevuta = False
            '        action.ActionSubID = actionSubID : actionSubID += 1
            '        col.Add(action)
            'End Select


        End Sub

        Protected Overrides Function IsSupportedTipoOggetto(filter As CRMFindFilter) As Boolean
            Return filter.TipoOggetto = "" OrElse filter.TipoOggetto = "CTicket"
        End Function

        Protected Overrides Sub FillSupportedTypes(items As CKeyCollection(Of String))
            items.Add("CTicket", "Ticket")
        End Sub

    End Class


End Class