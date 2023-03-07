Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls
Imports DMD.Anagrafica



Partial Public Class Office

    ''' <summary>
    ''' Aggiunge le commissioni
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StoricoHandlerCommissioni
        Inherits StoricoHandlerBase

        Protected Overrides Sub AggiungiInternal(items As CCollection(Of StoricoAction), filter As CRMFindFilter)
            Dim cnt As Integer = 0
            Dim cursor1 As New CommissioneCursor


#If Not Debug Then
            Try
#End If
            cursor1.Stato.Value = ObjectStatus.OBJECT_VALID
            If filter.Dal.HasValue Then
                cursor1.OraRientro.Value = filter.Dal.Value
                cursor1.OraRientro.Operator = OP.OP_GT
                If filter.Al.HasValue Then
                    cursor1.OraRientro.Value1 = filter.Al.Value
                    cursor1.OraRientro.Operator = OP.OP_BETWEEN
                End If
            ElseIf filter.Al.HasValue Then
                cursor1.OraRientro.Value = filter.Al.Value
                cursor1.OraRientro.Operator = OP.OP_LE
            End If
            If filter.IDPersona <> 0 Then
                cursor1.IDPersonaIncontrata.Value = filter.IDPersona
            ElseIf filter.Nominativo <> "" Then
                cursor1.NomePersonaIncontrata.Value = filter.Nominativo & "%"
                cursor1.NomePersonaIncontrata.Operator = OP.OP_LIKE
            End If
            If filter.IDOperatore <> 0 Then cursor1.IDOperatore.Value = filter.IDOperatore
            If filter.IDPuntoOperativo <> 0 Then cursor1.IDPuntoOperativo.Value = filter.IDPuntoOperativo
            If filter.Scopo <> "" Then
                cursor1.Motivo.Value = filter.Scopo & "%"
                cursor1.Motivo.Operator = OP.OP_LIKE
            End If
            If filter.StatoConversazione.HasValue Then
                Select Case filter.StatoConversazione
                    Case 0 'In Attesa
                        cursor1.StatoCommissione.Value = StatoCommissione.NonIniziata
                    Case 1 'in corso
                        cursor1.StatoCommissione.ValueIn(New Object() {StatoCommissione.Iniziata, StatoCommissione.Rimandata})
                    Case Else
                        cursor1.StatoCommissione.ValueIn(New Object() {StatoCommissione.Annullata, StatoCommissione.Completata})
                End Select
            End If
            If filter.Numero <> "" Then
                'cursor.NumeroOIndirizzo.Operator = OP.OP_LIKE
                cursor1.NomeAzienda.Value = filter.Numero & "%"
                cursor1.NomeAzienda.Operator = OP.OP_LIKE
            End If
            If (filter.IDContesto.HasValue) Then
                cursor1.ContextType.Value = filter.TipoContesto
                cursor1.ContextID.Value = filter.IDContesto
            End If
            cursor1.OraRientro.SortOrder = SortEnum.SORT_DESC
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

        Private Sub AddActivities(ByVal col As CCollection(Of StoricoAction), ByVal commissione As Commissione)
            Dim action As New StoricoAction
            action.Data = commissione.AssegnataIl
            action.IDOperatore = commissione.IDAssegnataDa
            action.NomeOperatore = commissione.NomeAssegnataDa
            action.IDCliente = commissione.IDPersonaIncontrata
            action.NomeCliente = commissione.NomePersonaIncontrata
            action.Note = "Commissione Programmata. Motivo: <b>" & commissione.Motivo & "</b>"
            Select Case LCase(commissione.Presso)
                Case "residenza", "domicilio", "posto di lavoro"
                    action.Note &= " presso <b>" & commissione.Presso & "</b>" & vbNewLine
                Case Else
                    If (commissione.Azienda IsNot Nothing) Then
                        action.Note &= " presso <b>" & commissione.NomeAzienda & "</b>" & vbNewLine
                    Else
                        If (commissione.Luoghi.Count > 0) Then
                            action.Note &= " presso <b>" & commissione.Luoghi(0).ToString & "</b>" & vbNewLine
                        Else
                            action.Note &= " presso <b><i>Indirizzo non specificato</i></b>" & vbNewLine
                        End If
                    End If
            End Select
            action.Note &= " Assegnata a <b>" & commissione.NomeAssegnataA & "</b> per il <b>" & Formats.FormatUserDate(commissione.DataPrevista) & "</b>"

            action.Scopo = commissione.Motivo
            action.NumeroOIndirizzo = Strings.TrimTo(commissione.Luoghi.ToString, 255)
            action.Esito = EsitoChiamata.RIPOSTA
            action.DettaglioEsito = "Commissione Assegnata"
            action.Durata = 0
            action.Attesa = 0
            action.Tag = commissione
            action.ActionSubID = StatoCommissione.NonIniziata
            action.StatoConversazione = StatoConversazione.CONCLUSO

            col.Add(action)

            For Each u As CommissionePerUscita In commissione.Uscite
                If u.Stato = ObjectStatus.OBJECT_VALID AndAlso u.Uscita IsNot Nothing AndAlso u.Uscita.Stato = ObjectStatus.OBJECT_VALID Then
                    Dim uscita As Uscita = u.Uscita
                    action = New StoricoAction
                    action.Data = uscita.OraUscita
                    action.IDOperatore = uscita.IDOperatore
                    action.NomeOperatore = uscita.NomeOperatore
                    action.IDCliente = commissione.IDPersonaIncontrata
                    action.NomeCliente = commissione.NomePersonaIncontrata
                    action.Note = "Uscita Iniziata -> Commissione: <b>" & commissione.Motivo & "</b>" ' presso <b>" & commissione.NomeAzienda & "</b>" & vbNewLine
                    action.Scopo = commissione.Motivo
                    action.NumeroOIndirizzo = Strings.TrimTo(commissione.Luoghi.ToString, 255)
                    action.Esito = EsitoChiamata.RIPOSTA
                    action.DettaglioEsito = "Commissione Iniziata"
                    action.Durata = 0
                    action.Attesa = 0
                    action.Tag = commissione
                    action.ActionSubID = StatoCommissione.Iniziata
                    action.StatoConversazione = StatoConversazione.CONCLUSO
                    col.Add(action)

                    If (uscita.OraRientro.HasValue) Then
                        action = New StoricoAction
                        action.Data = uscita.OraRientro
                        action.IDOperatore = uscita.IDOperatore
                        action.NomeOperatore = uscita.NomeOperatore
                        action.IDCliente = commissione.IDPersonaIncontrata
                        action.NomeCliente = commissione.NomePersonaIncontrata
                        action.Note = "Uscita Terminata -> Commissione: <b>" & commissione.Motivo & "</b>" ' presso <b>" & commissione.NomeAzienda & "</b>" & vbNewLine
                        action.Scopo = commissione.Motivo
                        action.NumeroOIndirizzo = Strings.TrimTo(commissione.Luoghi.ToString, 255)
                        Select Case u.StatoCommissione
                            Case StatoCommissione.Annullata
                                action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                                action.DettaglioEsito = "Commissione Annullata: " & u.DescrizioneEsito
                                action.ActionSubID = StatoCommissione.Annullata
                                action.StatoConversazione = StatoConversazione.CONCLUSO

                            Case StatoCommissione.Completata
                                action.Esito = EsitoChiamata.RIPOSTA
                                action.DettaglioEsito = "Commissione Completata: " & u.DescrizioneEsito
                                action.ActionSubID = StatoCommissione.Completata
                                action.StatoConversazione = StatoConversazione.CONCLUSO

                            Case StatoCommissione.Rimandata
                                action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
                                action.DettaglioEsito = "Commissione Completata: " & u.DescrizioneEsito
                                action.ActionSubID = StatoCommissione.Rimandata
                                action.StatoConversazione = StatoConversazione.CONCLUSO
                        End Select
                        action.Durata = 0
                        action.Attesa = 0
                        action.Tag = commissione
                        col.Add(action)

                    End If
                End If
            Next

            'If (commissione.StatoCommissione >= StatoCommissione.Iniziata) Then
            '    action = New StoricoAction
            '    action.Data = commissione.OraUscita
            '    action.IDOperatore = commissione.IDOperatore
            '    action.NomeOperatore = commissione.NomeOperatore
            '    action.IDCliente = commissione.IDPersonaIncontrata
            '    action.NomeCliente = commissione.NomePersonaIncontrata
            '    action.Note = "Commissione Iniziata. Motivo: <b>" & commissione.Motivo & "</b> presso <b>" & commissione.NomeAzienda & "</b>" & vbNewLine
            '    action.Scopo = commissione.Motivo
            '    action.NumeroOIndirizzo = Strings.TrimTo(commissione.Luoghi.ToString, 255)
            '    action.Esito = EsitoChiamata.RIPOSTA
            '    action.DettaglioEsito = "Commissione Iniziata"
            '    action.Durata = 0
            '    action.Attesa = 0
            '    action.Tag = commissione
            '    action.ActionSubID = StatoCommissione.Iniziata
            '    action.StatoConversazione = StatoConversazione.CONCLUSO
            '    col.Add(action)
            'End If

            'Select Case (commissione.StatoCommissione)
            '    Case StatoCommissione.Completata
            '        action = New StoricoAction
            '        action.Data = commissione.OraRientro
            '        action.IDOperatore = commissione.IDOperatore
            '        action.NomeOperatore = commissione.NomeOperatore
            '        action.IDCliente = commissione.IDPersonaIncontrata
            '        action.NomeCliente = commissione.NomePersonaIncontrata
            '        action.Note = "Commissione Completata. Motivo: <b>" & commissione.Motivo & "</b> presso <b>" & commissione.NomeAzienda & "</b>"
            '        action.Scopo = commissione.Motivo
            '        action.NumeroOIndirizzo = Strings.TrimTo(commissione.Luoghi.ToString, 255)
            '        action.Esito = EsitoChiamata.RIPOSTA
            '        action.DettaglioEsito = "Commissione Completata"
            '        action.Durata = 0
            '        action.Attesa = 0
            '        action.Tag = commissione
            '        action.ActionSubID = StatoCommissione.Completata
            '        action.StatoConversazione = StatoConversazione.CONCLUSO
            '        col.Add(action)
            '    Case StatoCommissione.Annullata
            '        action = New StoricoAction
            '        action.Data = IIf(commissione.OraRientro.HasValue, commissione.OraRientro, IIf(commissione.OraUscita.HasValue, commissione.OraUscita, commissione.AssegnataIl))
            '        action.IDOperatore = commissione.IDOperatore
            '        action.NomeOperatore = commissione.NomeOperatore
            '        action.IDCliente = commissione.IDPersonaIncontrata
            '        action.NomeCliente = commissione.NomePersonaIncontrata
            '        action.Note = "Commissione Annullata. Motivo: <b>" & commissione.Motivo & "</b> presso <b>" & commissione.NomeAzienda & "</b>"
            '        action.Scopo = commissione.Motivo
            '        action.NumeroOIndirizzo = Strings.TrimTo(commissione.Luoghi.ToString, 255)
            '        action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
            '        action.DettaglioEsito = "Commissione Annullata"
            '        action.Durata = 0
            '        action.Attesa = 0
            '        action.Tag = commissione
            '        action.ActionSubID = StatoCommissione.Annullata
            '        action.StatoConversazione = StatoConversazione.CONCLUSO
            '        col.Add(action)
            '    Case StatoCommissione.Rimandata
            '        action = New StoricoAction
            '        action.Data = IIf(commissione.OraRientro.HasValue, commissione.OraRientro, IIf(commissione.OraUscita.HasValue, commissione.OraUscita, commissione.AssegnataIl))
            '        action.IDOperatore = commissione.IDOperatore
            '        action.NomeOperatore = commissione.NomeOperatore
            '        action.IDCliente = commissione.IDPersonaIncontrata
            '        action.NomeCliente = commissione.NomePersonaIncontrata
            '        action.Note = "Commissione Rimandata. Motivo: <b>" & commissione.Motivo & "</b> presso <b>" & commissione.NomeAzienda & "</b>"
            '        action.Scopo = commissione.Motivo
            '        action.NumeroOIndirizzo = Strings.TrimTo(commissione.Luoghi.ToString, 255)
            '        action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
            '        action.DettaglioEsito = "Commissione Rimandata"
            '        action.Durata = 0
            '        action.Attesa = 0
            '        action.Tag = commissione
            '        action.ActionSubID = StatoCommissione.Rimandata
            '        action.StatoConversazione = StatoConversazione.CONCLUSO
            '        col.Add(action)
            '    Case StatoCommissione.NonIniziata

            '    Case StatoCommissione.Iniziata
            '        action = New StoricoAction
            '        action.Data = IIf(commissione.OraUscita.HasValue, commissione.OraUscita, commissione.AssegnataIl)
            '        action.IDOperatore = commissione.IDOperatore
            '        action.NomeOperatore = commissione.NomeOperatore
            '        action.IDCliente = commissione.IDPersonaIncontrata
            '        action.NomeCliente = commissione.NomePersonaIncontrata
            '        action.Note = "Commissione In Corso. Motivo: <b>" & commissione.Motivo & "</b> presso <b>" & commissione.NomeAzienda & "</b>"
            '        action.Scopo = commissione.Motivo
            '        action.NumeroOIndirizzo = Strings.TrimTo(commissione.Luoghi.ToString, 255)
            '        action.Esito = EsitoChiamata.ALTRO
            '        action.DettaglioEsito = "Commissione In Corso"
            '        action.Durata = 0
            '        action.Attesa = 0
            '        action.Tag = commissione
            '        action.ActionSubID = StatoCommissione.Iniziata
            '        action.StatoConversazione = StatoConversazione.INCORSO
            '        col.Add(action)
            '    Case Else
            '        Debug.Assert(False)
            'End Select

        End Sub

        Protected Overrides Sub FillSupportedTypes(items As CKeyCollection(Of String))
            items.Add("Commissione", "Commissione")
        End Sub


        Public Sub New()

        End Sub
    End Class


End Class