Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Aggiunge le commissioni
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StoricoHandlerRichiesteConteggiEstintivi
        Inherits StoricoHandlerBase

        Protected Overrides Sub AggiungiInternal(items As CCollection(Of StoricoAction), filter As CRMFindFilter)
            Dim cnt As Integer = 0
            Dim cursor1 As New CRichiestaConteggioCursor


#If Not Debug Then
            Try
#End If
            cursor1.Stato.Value = ObjectStatus.OBJECT_VALID
            If filter.Dal.HasValue Then
                cursor1.InviatoIl.Value = filter.Dal.Value
                cursor1.InviatoIl.Operator = OP.OP_GT
                If filter.Al.HasValue Then
                    cursor1.InviatoIl.Value1 = filter.Al.Value
                    cursor1.InviatoIl.Operator = OP.OP_BETWEEN
                End If
            ElseIf filter.Al.HasValue Then
                cursor1.InviatoIl.Value = filter.Al.Value
                cursor1.InviatoIl.Operator = OP.OP_LE
            End If
            If filter.IDPersona <> 0 Then
                cursor1.IDCliente.Value = filter.IDPersona
            ElseIf filter.Nominativo <> "" Then
                cursor1.NomeCliente.Value = filter.Nominativo & "%"
                cursor1.NomeCliente.Operator = OP.OP_LIKE
            End If
            If filter.IDOperatore <> 0 Then cursor1.PresaInCaricoDaID.Value = filter.IDOperatore
            If filter.IDPuntoOperativo <> 0 Then cursor1.IDPuntoOperativo.Value = filter.IDPuntoOperativo
            If filter.Scopo <> "" Then
                'cursor1..Value = filter.Scopo & "%"
                'cursor1.Motivo.Operator = OP.OP_LIKE
            End If
            If filter.StatoConversazione.HasValue Then
                Select Case filter.StatoConversazione
                    Case 0 'In Attesa
                        cursor1.PresaInCaricoDaID.Value = 0
                    Case 1 'in corso
                        cursor1.PresaInCaricoDaID.Value = 0
                    Case Else
                        cursor1.PresaInCaricoDaID.Value = 0
                        cursor1.PresaInCaricoDaID.Operator = OP.OP_NE
                End Select
            End If
            If filter.Numero <> "" Then
                cursor1.MezzoDiInvio.Value = filter.Numero & "%"
                cursor1.MezzoDiInvio.Operator = OP.OP_LIKE
            End If
            If (filter.IDContesto.HasValue) Then
                'cursor1.id.Value = filter.TipoContesto
                'cursor1.ContextID.Value = filter.IDContesto
            End If
            cursor1.InviatoIl.SortOrder = SortEnum.SORT_DESC
            cursor1.IgnoreRights = filter.IgnoreRights

            While Not cursor1.EOF AndAlso (Not filter.nMax.HasValue OrElse cnt < filter.nMax)
                cnt += 1
                Me.AddActivities(items, cursor1.Item)
                cursor1.MoveNext()
            End While
#If Not DEBUG Then
            Catch Ex As Exception
                Throw
            Finally
#End If
            cursor1.Dispose()
#If Not DEBUG Then
            End Try
#End If
        End Sub

        Private Sub AddActivities(ByVal col As CCollection(Of StoricoAction), ByVal rich As CRichiestaConteggio)
            Dim action As New StoricoAction

            action.Data = rich.InviatoIl
            action.IDOperatore = rich.RicevutoDaID
            action.NomeOperatore = rich.RicevutoDaNome
            action.IDCliente = rich.IDCliente
            action.NomeCliente = rich.NomeCliente
            action.Note = "Segnalazione Stay: " & Formats.FormatValuta(rich.ImportoRata) & " x " & Formats.FormatInteger(rich.DurataMesi) & ", TAN: " & Formats.FormatPercentage(rich.TAN) & " %, TAEG: " & Formats.FormatPercentage(rich.TAEG) & ".<br/>Richiedente " & rich.NomeIstituto & ".<br/>Segnalata da " & rich.InviatoDaNome
            action.Scopo = "Segnalazione Stay"
            action.NumeroOIndirizzo = rich.MezzoDiInvio
            action.Esito = EsitoChiamata.RIPOSTA
            action.DettaglioEsito = "Segnalata"
            action.Durata = 0
            action.Attesa = 0
            action.Tag = rich
            action.ActionSubID = 0
            action.StatoConversazione = IIf(rich.PresaInCaricoDaID = 0, StatoConversazione.INCORSO, StatoConversazione.CONCLUSO)
            col.Add(action)

            'action = New StoricoAction
            'action.Data = rich.DataSegnalazione
            'action.IDOperatore = rich.RicevutoDaID
            'action.NomeOperatore = rich.RicevutoDaNome
            'action.IDCliente = rich.IDCliente
            'action.NomeCliente = rich.NomeCliente
            'action.Note = "Richiesta CE da parte di " & rich.NomeIstituto & " ricevuta da " & rich.RicevutoDaNome
            'action.Scopo = "Richiesta CE"
            'action.NumeroOIndirizzo = rich.MezzoDiInvio
            'action.Esito = EsitoChiamata.RIPOSTA
            'action.DettaglioEsito = "Ricevuta"
            'action.Durata = 0
            'action.Attesa = 0
            'action.Tag = rich
            'action.ActionSubID = 1
            'action.StatoConversazione = IIf(rich.PresaInCaricoDaID = 0, StatoConversazione.INCORSO, StatoConversazione.CONCLUSO)
            'col.Add(action)

            'If (rich.DataPresaInCarico.HasValue AndAlso rich.PresaInCaricoDaID <> 0) Then
            '    action = New StoricoAction
            '    action.Data = rich.DataPresaInCarico
            '    action.IDOperatore = rich.PresaInCaricoDaID
            '    action.NomeOperatore = rich.PresaInCaricoDaNome
            '    action.IDCliente = rich.IDCliente
            '    action.NomeCliente = rich.NomeCliente
            '    action.Note = "Richiesta CE da parte di " & rich.NomeIstituto & " presa in carico da " & rich.PresaInCaricoDaNome
            '    action.Scopo = "Richiesta CE"
            '    action.NumeroOIndirizzo = rich.MezzoDiInvio
            '    action.Esito = EsitoChiamata.RIPOSTA
            '    action.DettaglioEsito = "Presa in carico"
            '    action.Durata = 0
            '    action.Attesa = 0
            '    action.Tag = rich
            '    action.ActionSubID = 2
            '    action.StatoConversazione = IIf(rich.PresaInCaricoDaID = 0, StatoConversazione.INCORSO, StatoConversazione.CONCLUSO)
            '    col.Add(action)
            'End If

            'If (rich.DataEvasione.HasValue AndAlso rich.DataEvasione.Value <= Calendar.ToMorrow) Then
            '    action = New StoricoAction
            '    action.Data = rich.DataEvasione
            '    action.IDOperatore = 0 'rich.PresaInCaricoDaID
            '    action.NomeOperatore = "" ' rich.PresaInCaricoDaNome
            '    action.IDCliente = rich.IDCliente
            '    action.NomeCliente = rich.NomeCliente
            '    action.Note = "Richiesta CE da parte di " & rich.NomeIstituto & " evasa da " & rich.PresaInCaricoDaNome
            '    action.Scopo = "Richiesta CE"
            '    action.NumeroOIndirizzo = rich.MezzoDiInvio
            '    action.Esito = EsitoChiamata.RIPOSTA
            '    action.DettaglioEsito = "Evasa"
            '    action.Durata = 0
            '    action.Attesa = 0
            '    action.Tag = rich
            '    action.ActionSubID = 3
            '    action.StatoConversazione = IIf(rich.PresaInCaricoDaID = 0, StatoConversazione.INCORSO, StatoConversazione.CONCLUSO)
            '    col.Add(action)
            'End If

        End Sub

        Protected Overrides Sub FillSupportedTypes(items As CKeyCollection(Of String))
            items.Add("CRichiestaConteggio", "Richiesta Conteggio Estintivo")
        End Sub


        Public Sub New()

        End Sub
    End Class


End Class