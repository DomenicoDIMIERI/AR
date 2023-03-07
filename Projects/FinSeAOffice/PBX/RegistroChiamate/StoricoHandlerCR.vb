Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls
Imports DMD.Anagrafica



Partial Public Class Office

    ''' <summary>
    ''' Handler delle chiamate registrate
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StoricoHandlerCR
        Inherits StoricoHandlerBase

        Public Sub New()

        End Sub

        Protected Overrides Sub AggiungiInternal(items As CCollection(Of StoricoAction), filter As CRMFindFilter)
            Dim cnt As Integer = 0
            Dim cursor As New ChiamataRegistrataCursor

#If Not DEBUG Then
            Try
#End If
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            If filter.Dal.HasValue Then
                cursor.DataInizio.Value = filter.Dal.Value
                cursor.DataInizio.Operator = OP.OP_GT
                If filter.Al.HasValue Then
                    cursor.DataInizio.Value1 = filter.Al.Value
                    cursor.DataInizio.Operator = OP.OP_BETWEEN
                End If
            ElseIf filter.Al.HasValue Then
                cursor.DataInizio.Value = filter.Al.Value
                cursor.DataInizio.Operator = OP.OP_LE
            End If
            If filter.Contenuto <> "" Then
                'cursor..Value = filter.Contenuto & "%"
                'cursor.Note.Operator = OP.OP_LIKE
            End If
            'If filter.DettaglioEsito Then
            'filter.etichetta
            'If filter.IDOperatore <> 0 Then cursor.IDOperator.Value = filter.IDOperatore
            'If filter.IDPuntoOperativo <> 0 Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
            If (filter.IDPersona <> 0) Then
                cursor.IDChiamato.Value = filter.IDPersona
            ElseIf filter.Nominativo <> "" Then
                cursor.NomeChiamato.Value = filter.Nominativo & "%"
                cursor.NomeChiamato.Operator = OP.OP_LIKE
            End If
            If filter.Numero <> "" Then
                cursor.ANumero.Value = filter.Numero & "%"
                cursor.ANumero.Operator = OP.OP_LIKE
            End If
            If filter.StatoConversazione.HasValue Then
                Select Case filter.StatoConversazione
                    Case 0 'In Attesa
                        cursor.StatoChiamata.ValueIn(New Object() {StatoChiamataRegistrata.Sconosciuto, StatoChiamataRegistrata.Composizione})
                    Case 1 'in corso
                        cursor.StatoChiamata.ValueIn(New Object() {StatoChiamataRegistrata.InCorso})
                    Case Else
                        cursor.StatoChiamata.ValueIn(New Object() {StatoChiamataRegistrata.AgganciatoChiamante, StatoChiamataRegistrata.AgganciatoChiamato, StatoChiamataRegistrata.Errore, StatoChiamataRegistrata.NonRisposto, StatoChiamataRegistrata.Rifiutata})
                End Select
            End If
            If (filter.IDContesto.HasValue) Then
                'cursor2.ContextType.Value = filter.TipoContesto
                'cursor2.ContextID.Value = filter.IDContesto
            End If
            If (filter.Scopo <> "") Then

            End If

            cursor.DataInizio.SortOrder = SortEnum.SORT_DESC
            cursor.IgnoreRights = filter.IgnoreRights
            'cursor2.StatoOperazione.Value = StatoRichiestaCERQ.RITIRATA
            While Not cursor.EOF AndAlso (Not filter.nMax.HasValue OrElse cnt < filter.nMax)
                cnt += 1
                'items.Add(cursor2.Item)
                Me.AddActivities(items, cursor.Item)
                cursor.MoveNext()
            End While
#If Not DEBUG Then
            Catch ex As Exception
                Throw
            Finally
#End If
            cursor.Dispose()
#If Not DEBUG Then
            End Try
#End If
        End Sub



        Private Sub AddActivities(ByVal col As CCollection(Of StoricoAction), ByVal c As ChiamataRegistrata)
            Dim action As New StoricoAction

            action.Data = c.DataInizio
            action.IDOperatore = GetID(Sistema.Users.KnownUsers.SystemUser)
            action.NomeOperatore = Sistema.Users.KnownUsers.SystemUser.Nominativo
            action.IDCliente = c.IDChiamato
            action.NomeCliente = c.NomeChiamato
            action.Note = "Chiamata da: " & Formats.FormatNumber(c.DaNumero) & " a " & Formats.FormatNumber(c.ANumero) & vbCrLf &
                          "Esito: " & [Enum].GetName(GetType(EsitoChiamataRegistrata), c.EsitoChiamata) &
                          "Inizio: " & Formats.FormatUserDateTime(c.DataInizio)
            If (c.DataRisposta.HasValue) Then action.Note &= vbCrLf & "Risposta: " & Formats.FormatUserDateTime(c.DataRisposta)
            If (c.DataRisposta.HasValue AndAlso c.DataInizio.HasValue) Then
                action.Note &= " (Attesa: " & Formats.FormatDurata((c.DataRisposta.Value - c.DataInizio.Value).TotalSeconds) & ")"
                action.Attesa = 0
            End If
            If (c.DataFine.HasValue) Then
                action.Note &= vbCrLf & "Fine: " & Formats.FormatUserDateTime(c.DataFine)
                If (c.DataRisposta.HasValue) Then
                    action.Durata = (c.DataFine.Value - c.DataInizio.Value).TotalSeconds
                    action.Attesa = (c.DataRisposta.Value - c.DataInizio.Value).TotalSeconds
                    action.Note &= " (Durata conversazione: " & Formats.FormatDurata((c.DataFine.Value - c.DataRisposta.Value).TotalSeconds) & ")"

                ElseIf (c.DataInizio.HasValue) Then
                    action.Durata = (c.DataFine.Value - c.DataInizio.Value).TotalSeconds
                    action.Attesa = action.Durata
                    action.Note &= " (Attesa: " & Formats.FormatDurata(action.Durata) & ")"
                End If

            End If
            action.Scopo = ""
            action.NumeroOIndirizzo = c.ANumero
            Select Case c.EsitoChiamata
                Case EsitoChiamataRegistrata.NonRisposto
                    action.Esito = EsitoChiamata.RIPOSTA
                Case EsitoChiamataRegistrata.NonRisposto
                    action.Esito = EsitoChiamata.NESSUNA_RISPOSTA
            End Select

            action.DettaglioEsito = c.EsitoChiamataEx
            action.Tag = c
            action.Ricevuta = False
            Select Case c.StatoChiamata
                Case StatoChiamataRegistrata.Sconosciuto, StatoChiamataRegistrata.InCorso, StatoChiamataRegistrata.Composizione
                    action.StatoConversazione = StatoConversazione.INATTESA
                Case Else
                    action.StatoConversazione = StatoConversazione.CONCLUSO
            End Select

            col.Add(action)
        End Sub

        Protected Overrides Sub FillSupportedTypes(items As CKeyCollection(Of String))
            items.Add("ChiamataRegistrata", "Chiamata Registrata")
        End Sub


    End Class


End Class