Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls
Imports DMD.Anagrafica



Partial Public Class Office

    ''' <summary>
    ''' Aggiunge gli oggetti da spedire
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StoricoHandlerOggettiSpedire
        Inherits StoricoHandlerBase

        Protected Overrides Sub AggiungiInternal(items As CCollection(Of StoricoAction), filter As CRMFindFilter)
            Dim cnt As Integer = 0
            Dim cursor1 As New OggettiDaSpedireCursor


#If Not Debug Then
            Try
#End If
            cursor1.Stato.Value = ObjectStatus.OBJECT_VALID
            If filter.Dal.HasValue Then
                cursor1.DataInizioSpedizione.Value = filter.Dal.Value
                cursor1.DataInizioSpedizione.Operator = OP.OP_GT
                If filter.Al.HasValue Then
                    cursor1.DataInizioSpedizione.Value1 = filter.Al.Value
                    cursor1.DataInizioSpedizione.Operator = OP.OP_BETWEEN
                End If
            ElseIf filter.Al.HasValue Then
                cursor1.DataInizioSpedizione.Value = filter.Al.Value
                cursor1.DataInizioSpedizione.Operator = OP.OP_LE
            End If

            If filter.IDPersona <> 0 Then
                cursor1.IDCliente.Value = filter.IDPersona
            ElseIf filter.Nominativo <> "" Then
                cursor1.NomeCliente.Value = filter.Nominativo & "%"
                cursor1.NomeCliente.Operator = OP.OP_LIKE
            End If
            If filter.IDOperatore <> 0 Then cursor1.IDRichiestaDa.Value = filter.IDOperatore
            If filter.IDPuntoOperativo <> 0 Then cursor1.IDPuntoOperativo.Value = filter.IDPuntoOperativo
            If filter.Scopo <> "" Then
                cursor1.CategoriaContenuto.Value = filter.Scopo & "%"
                cursor1.CategoriaContenuto.Operator = OP.OP_LIKE
            End If
            If filter.StatoConversazione.HasValue Then
                Select Case filter.StatoConversazione
                    Case 0 'In Attesa
                        cursor1.StatoOggetto.ValueIn({StatoOggettoDaSpedire.InPreparazione, StatoOggettoDaSpedire.ProntoPerLaSpedizione})
                    Case 1 'in corso
                        cursor1.StatoOggetto.ValueIn({StatoOggettoDaSpedire.Spedito})
                    Case Else
                        cursor1.StatoOggetto.ValueIn({StatoOggettoDaSpedire.ConsegnaFallitaNonTrovato, StatoOggettoDaSpedire.ConsegnaFallitaIndirizzoErrato, StatoOggettoDaSpedire.ConsegnaFallitaNonTrovato, StatoOggettoDaSpedire.ConsegnaFallitaRifiutoDestinatario, StatoOggettoDaSpedire.Consegnato, StatoOggettoDaSpedire.SpedizioneAnnullata, StatoOggettoDaSpedire.SpedizioneBocciata, StatoOggettoDaSpedire.SpedizioneRifiutata})
                End Select
            End If
            If filter.Numero <> "" Then
                'cursor.NumeroOIndirizzo.Operator = OP.OP_LIKE
                cursor1.INDDEST_ToponimoViaECivico.Value = filter.Numero & "%"
            End If
            cursor1.DataInizioSpedizione.SortOrder = SortEnum.SORT_DESC
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

        Private Sub AddActivities(ByVal col As CCollection(Of StoricoAction), ByVal item As OggettoDaSpedire)
            Dim action As New StoricoAction
            action.Data = item.DataRichiesta
            action.IDOperatore = item.IDRichiestaDa
            action.NomeOperatore = item.NomeRichiestaDa
            action.IDCliente = item.IDCliente
            action.NomeCliente = item.NomeCliente
            action.Note = "Programmata Spedizione di <b>" & item.CategoriaContenuto & "</b> a <b>" & item.NomeDestinatario & "</a> presso " & item.IndirizzoDestinatario.ToString
            action.Scopo = item.CategoriaContenuto
            action.NumeroOIndirizzo = Strings.TrimTo(item.IndirizzoDestinatario.ToString, 255)
            action.Esito = EsitoChiamata.RIPOSTA
            action.DettaglioEsito = "Spedizione Programmata"
            action.Durata = 0
            action.Attesa = 0
            action.Tag = item
            action.ActionSubID = StatoOggettoDaSpedire.InPreparazione
            action.StatoConversazione = StatoConversazione.CONCLUSO
            col.Add(action)

            If (item.StatoOggetto > StatoOggettoDaSpedire.ProntoPerLaSpedizione) Then
                action = New StoricoAction
                action.Data = item.DataPresaInCarico
                action.IDOperatore = item.IDPresaInCaricoDa
                action.NomeOperatore = item.NomePresaInCaricoDa
                action.IDCliente = item.IDCliente
                action.NomeCliente = item.NomeCliente
                action.Note = "Presa in carico della Spedizione di <b>" & item.CategoriaContenuto & "</b> a <b>" & item.NomeDestinatario & "</a> presso " & item.IndirizzoDestinatario.ToString
                action.Scopo = item.CategoriaContenuto
                action.NumeroOIndirizzo = Strings.TrimTo(item.IndirizzoDestinatario.ToString, 255)
                action.Esito = EsitoChiamata.RIPOSTA
                action.DettaglioEsito = "Spedizione Presa in Carico"
                action.Durata = 0
                action.Attesa = 0
                action.Tag = item
                action.ActionSubID = StatoOggettoDaSpedire.ProntoPerLaSpedizione
                action.StatoConversazione = StatoConversazione.CONCLUSO
                col.Add(action)
            End If

           


            Select Case (item.StatoOggetto)
                Case StatoOggettoDaSpedire.Spedito
                    action = New StoricoAction
                    action.Data = item.DataInizioSpedizione
                    action.IDOperatore = item.IDPresaInCaricoDa
                    action.NomeOperatore = item.NomePresaInCaricoDa
                    action.IDCliente = item.IDCliente
                    action.NomeCliente = item.NomeCliente
                    action.Note = "Inizio della Spedizione di <b>" & item.CategoriaContenuto & "</b> a <b>" & item.NomeDestinatario & "</a> presso " & item.IndirizzoDestinatario.ToString
                    action.Scopo = item.CategoriaContenuto
                    action.NumeroOIndirizzo = Strings.TrimTo(item.IndirizzoDestinatario.ToString, 255)
                    action.Esito = EsitoChiamata.RIPOSTA
                    action.DettaglioEsito = "Inizio Spedizione"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = item
                    action.ActionSubID = StatoOggettoDaSpedire.Spedito
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)

                Case StatoOggettoDaSpedire.SpedizioneAnnullata
                    action = New StoricoAction
                    action.Data = item.DataConferma
                    action.IDOperatore = item.IDConfermatoDa
                    action.NomeOperatore = item.NomeConfermatoDa
                    action.IDCliente = item.IDCliente
                    action.NomeCliente = item.NomeCliente
                    action.Note = "Annullamento della Spedizione di <b>" & item.CategoriaContenuto & "</b> a <b>" & item.NomeDestinatario & "</a> presso " & item.IndirizzoDestinatario.ToString
                    action.Scopo = item.CategoriaContenuto
                    action.NumeroOIndirizzo = Strings.TrimTo(item.IndirizzoDestinatario.ToString, 255)
                    action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    action.DettaglioEsito = "Spediaizone Annullata"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = item
                    action.ActionSubID = StatoOggettoDaSpedire.SpedizioneAnnullata
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)

                Case StatoOggettoDaSpedire.SpedizioneBocciata
                    action = New StoricoAction
                    action.Data = item.DataConferma
                    action.IDOperatore = item.IDConfermatoDa
                    action.NomeOperatore = item.NomeConfermatoDa
                    action.IDCliente = item.IDCliente
                    action.NomeCliente = item.NomeCliente
                    action.Note = "Bocciatura della Spedizione di <b>" & item.CategoriaContenuto & "</b> a <b>" & item.NomeDestinatario & "</a> presso " & item.IndirizzoDestinatario.ToString
                    action.Scopo = item.CategoriaContenuto
                    action.NumeroOIndirizzo = Strings.TrimTo(item.IndirizzoDestinatario.ToString, 255)
                    action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    action.DettaglioEsito = "Spedizione Bocciata"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = item
                    action.ActionSubID = StatoOggettoDaSpedire.SpedizioneBocciata
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)

                Case StatoOggettoDaSpedire.SpedizioneRifiutata
                    action = New StoricoAction
                    action.Data = item.DataConferma
                    action.IDOperatore = item.IDConfermatoDa
                    action.NomeOperatore = item.NomeConfermatoDa
                    action.IDCliente = item.IDCliente
                    action.NomeCliente = item.NomeCliente
                    action.Note = "Rifiuto del corriere per la Spedizione di <b>" & item.CategoriaContenuto & "</b> a <b>" & item.NomeDestinatario & "</a> presso " & item.IndirizzoDestinatario.ToString
                    action.Scopo = item.CategoriaContenuto
                    action.NumeroOIndirizzo = Strings.TrimTo(item.IndirizzoDestinatario.ToString, 255)
                    action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    action.DettaglioEsito = "Spedizione Rifiutata dal Corriere"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = item
                    action.ActionSubID = StatoOggettoDaSpedire.SpedizioneRifiutata
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)

                Case StatoOggettoDaSpedire.Consegnato
                    action = New StoricoAction
                    action.Data = item.DataInizioSpedizione
                    action.IDOperatore = item.IDPresaInCaricoDa
                    action.NomeOperatore = item.NomePresaInCaricoDa
                    action.IDCliente = item.IDCliente
                    action.NomeCliente = item.NomeCliente
                    action.Note = "Inizio della Spedizione di <b>" & item.CategoriaContenuto & "</b> a <b>" & item.NomeDestinatario & "</a> presso " & item.IndirizzoDestinatario.ToString
                    action.Scopo = item.CategoriaContenuto
                    action.NumeroOIndirizzo = Strings.TrimTo(item.IndirizzoDestinatario.ToString, 255)
                    action.Esito = EsitoChiamata.RIPOSTA
                    action.DettaglioEsito = "Inizio Spedizione"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = item
                    action.ActionSubID = StatoOggettoDaSpedire.Spedito
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)

                    action = New StoricoAction
                    action.Data = item.DataConferma
                    action.IDOperatore = item.IDConfermatoDa
                    action.NomeOperatore = item.NomeConfermatoDa
                    action.IDCliente = item.IDCliente
                    action.NomeCliente = item.NomeCliente
                    action.Note = "<span class=""green"">Consegna riuscita</span> di <b>" & item.CategoriaContenuto & "</b> a <b>" & item.NomeDestinatario & "</a> presso " & item.IndirizzoDestinatario.ToString
                    action.Scopo = item.CategoriaContenuto
                    action.NumeroOIndirizzo = Strings.TrimTo(item.IndirizzoDestinatario.ToString, 255)
                    action.Esito = EsitoChiamata.RIPOSTA
                    action.DettaglioEsito = "Consegna Riuscita"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = item
                    action.ActionSubID = StatoOggettoDaSpedire.Consegnato
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)

                Case StatoOggettoDaSpedire.ConsegnaFallita
                    action = New StoricoAction
                    action.Data = item.DataInizioSpedizione
                    action.IDOperatore = item.IDPresaInCaricoDa
                    action.NomeOperatore = item.NomePresaInCaricoDa
                    action.IDCliente = item.IDCliente
                    action.NomeCliente = item.NomeCliente
                    action.Note = "Inizio della Spedizione di <b>" & item.CategoriaContenuto & "</b> a <b>" & item.NomeDestinatario & "</a> presso " & item.IndirizzoDestinatario.ToString
                    action.Scopo = item.CategoriaContenuto
                    action.NumeroOIndirizzo = Strings.TrimTo(item.IndirizzoDestinatario.ToString, 255)
                    action.Esito = EsitoChiamata.RIPOSTA
                    action.DettaglioEsito = "Inizio Spedizione"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = item
                    action.ActionSubID = StatoOggettoDaSpedire.Spedito
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)

                    action = New StoricoAction
                    action.Data = item.DataConferma
                    action.IDOperatore = item.IDConfermatoDa
                    action.NomeOperatore = item.NomeConfermatoDa
                    action.IDCliente = item.IDCliente
                    action.NomeCliente = item.NomeCliente
                    action.Note = "<span class=""red"">Consegna fallita</span> di <b>" & item.CategoriaContenuto & "</b> a <b>" & item.NomeDestinatario & "</a> presso " & item.IndirizzoDestinatario.ToString
                    action.Scopo = item.CategoriaContenuto
                    action.NumeroOIndirizzo = Strings.TrimTo(item.IndirizzoDestinatario.ToString, 255)
                    action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    action.DettaglioEsito = "Consegna Fallita"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = item
                    action.ActionSubID = StatoOggettoDaSpedire.ConsegnaFallita
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)

                Case StatoOggettoDaSpedire.ConsegnaFallitaIndirizzoErrato
                    action = New StoricoAction
                    action.Data = item.DataInizioSpedizione
                    action.IDOperatore = item.IDPresaInCaricoDa
                    action.NomeOperatore = item.NomePresaInCaricoDa
                    action.IDCliente = item.IDCliente
                    action.NomeCliente = item.NomeCliente
                    action.Note = "Inizio della Spedizione di <b>" & item.CategoriaContenuto & "</b> a <b>" & item.NomeDestinatario & "</a> presso " & item.IndirizzoDestinatario.ToString
                    action.Scopo = item.CategoriaContenuto
                    action.NumeroOIndirizzo = Strings.TrimTo(item.IndirizzoDestinatario.ToString, 255)
                    action.Esito = EsitoChiamata.RIPOSTA
                    action.DettaglioEsito = "Inizio Spedizione"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = item
                    action.ActionSubID = StatoOggettoDaSpedire.Spedito
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)

                    action = New StoricoAction
                    action.Data = item.DataConferma
                    action.IDOperatore = item.IDConfermatoDa
                    action.NomeOperatore = item.NomeConfermatoDa
                    action.IDCliente = item.IDCliente
                    action.NomeCliente = item.NomeCliente
                    action.Note = "<span class=""red"">Consegna fallita per indirizzo del destinatario errato</span> di <b>" & item.CategoriaContenuto & "</b> a <b>" & item.NomeDestinatario & "</a> presso " & item.IndirizzoDestinatario.ToString
                    action.Scopo = item.CategoriaContenuto
                    action.NumeroOIndirizzo = Strings.TrimTo(item.IndirizzoDestinatario.ToString, 255)
                    action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    action.DettaglioEsito = "Consegna Fallita per Indirizzo del destinatario errato"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = item
                    action.ActionSubID = StatoOggettoDaSpedire.ConsegnaFallitaIndirizzoErrato
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)

                Case StatoOggettoDaSpedire.ConsegnaFallitaNonTrovato
                    action = New StoricoAction
                    action.Data = item.DataInizioSpedizione
                    action.IDOperatore = item.IDPresaInCaricoDa
                    action.NomeOperatore = item.NomePresaInCaricoDa
                    action.IDCliente = item.IDCliente
                    action.NomeCliente = item.NomeCliente
                    action.Note = "Inizio della Spedizione di <b>" & item.CategoriaContenuto & "</b> a <b>" & item.NomeDestinatario & "</a> presso " & item.IndirizzoDestinatario.ToString
                    action.Scopo = item.CategoriaContenuto
                    action.NumeroOIndirizzo = Strings.TrimTo(item.IndirizzoDestinatario.ToString, 255)
                    action.Esito = EsitoChiamata.RIPOSTA
                    action.DettaglioEsito = "Inizio Spedizione"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = item
                    action.ActionSubID = StatoOggettoDaSpedire.Spedito
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)

                    action = New StoricoAction
                    action.Data = item.DataConferma
                    action.IDOperatore = item.IDConfermatoDa
                    action.NomeOperatore = item.NomeConfermatoDa
                    action.IDCliente = item.IDCliente
                    action.NomeCliente = item.NomeCliente
                    action.Note = "<span class=""red"">Consegna fallita perché il destinatario non è stato trovato</span> di <b>" & item.CategoriaContenuto & "</b> a <b>" & item.NomeDestinatario & "</a> presso " & item.IndirizzoDestinatario.ToString
                    action.Scopo = item.CategoriaContenuto
                    action.NumeroOIndirizzo = Strings.TrimTo(item.IndirizzoDestinatario.ToString, 255)
                    action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    action.DettaglioEsito = "Consegna Fallita perché il destinatario non è stato trovato"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = item
                    action.ActionSubID = StatoOggettoDaSpedire.ConsegnaFallitaNonTrovato
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)

                Case StatoOggettoDaSpedire.ConsegnaFallitaRifiutoDestinatario
                    action = New StoricoAction
                    action.Data = item.DataInizioSpedizione
                    action.IDOperatore = item.IDPresaInCaricoDa
                    action.NomeOperatore = item.NomePresaInCaricoDa
                    action.IDCliente = item.IDCliente
                    action.NomeCliente = item.NomeCliente
                    action.Note = "Inizio della Spedizione di <b>" & item.CategoriaContenuto & "</b> a <b>" & item.NomeDestinatario & "</a> presso " & item.IndirizzoDestinatario.ToString
                    action.Scopo = item.CategoriaContenuto
                    action.NumeroOIndirizzo = Strings.TrimTo(item.IndirizzoDestinatario.ToString, 255)
                    action.Esito = EsitoChiamata.RIPOSTA
                    action.DettaglioEsito = "Inizio Spedizione"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = item
                    action.ActionSubID = StatoOggettoDaSpedire.Spedito
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)

                    action = New StoricoAction
                    action.Data = item.DataConferma
                    action.IDOperatore = item.IDConfermatoDa
                    action.NomeOperatore = item.NomeConfermatoDa
                    action.IDCliente = item.IDCliente
                    action.NomeCliente = item.NomeCliente
                    action.Note = "<span class=""red"">Consegna fallita perché rifiutata dal destinatario</span> di <b>" & item.CategoriaContenuto & "</b> a <b>" & item.NomeDestinatario & "</a> presso " & item.IndirizzoDestinatario.ToString
                    action.Scopo = item.CategoriaContenuto
                    action.NumeroOIndirizzo = Strings.TrimTo(item.IndirizzoDestinatario.ToString, 255)
                    action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                    action.DettaglioEsito = "Consegna fallita perché rifiutata dal destinatario"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = item
                    action.ActionSubID = StatoOggettoDaSpedire.ConsegnaFallitaRifiutoDestinatario
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)

                Case Else
                    'Debug.Assert(False)
            End Select

        End Sub

        Protected Overrides Sub FillSupportedTypes(items As CKeyCollection(Of String))
            items.Add("OggettoDaSpedire", "Spedizione Oggetto")
        End Sub


        Public Sub New()

        End Sub
    End Class


End Class