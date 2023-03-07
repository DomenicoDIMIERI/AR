Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls
Imports DMD.Anagrafica



Partial Public Class CustomerCalls

    ''' <summary>
    ''' Aggiunge la campagne ADV
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StoricoHandlerADV
        Inherits StoricoHandlerBase

        Protected Overrides Sub AggiungiInternal(items As CCollection(Of StoricoAction), filter As CRMFindFilter)
            Dim cnt As Integer = 0
            Dim cursor As New ADV.CRisultatoCampagnaCursor

#If Not Debug Then
            Try
#End If
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            If filter.Dal.HasValue Then
                cursor.DataEsecuzione.Value = filter.Dal.Value
                cursor.DataEsecuzione.Operator = OP.OP_GT
                If filter.Al.HasValue Then
                    cursor.DataEsecuzione.Value1 = filter.Al.Value
                    cursor.DataEsecuzione.Operator = OP.OP_BETWEEN
                End If
            ElseIf filter.Al.HasValue Then
                cursor.DataEsecuzione.Value = filter.Al.Value
                cursor.DataEsecuzione.Operator = OP.OP_LE
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
                cursor.IDDestinatario.Value = filter.IDPersona
            ElseIf filter.Nominativo <> "" Then
                cursor.NomeDestinatario.Value = filter.Nominativo & "%"
                cursor.NomeDestinatario.Operator = OP.OP_LIKE
            End If
            If filter.Numero <> "" Then
                cursor.IndirizzoDestinatario.Value = filter.Numero & "%"
                cursor.IndirizzoDestinatario.Operator = OP.OP_LIKE
            End If
            If filter.Scopo <> "" Then
                'cursor2.TipoRichiesta.Value = filter.Scopo & "%"
                'cursor2.TipoRichiesta.Operator = OP.OP_LIKE
            End If
            If filter.StatoConversazione.HasValue Then
                'Select Case filter.StatoConversazione
                '    Case 0 'In Attesa
                '        cursor.StatoMessaggio.ValueIn(New Object() {ADV.StatoMessaggioCampagna.InPreparazione, ADV.StatoMessaggioCampagna.ProntoPerLaSpedizione})
                '    Case 1 'in corso
                '        cursor.StatoMessaggio.ValueIn(New Object() {ADV.StatoMessaggioCampagna.Inviato})
                '    Case Else
                '        cursor.StatoMessaggio.ValueIn(New Object() {ADV.StatoMessaggioCampagna.Letto, ADV.StatoMessaggioCampagna.RifiutatoDalDestinatario, ADV.StatoMessaggioCampagna.RifiutatoDalVettore})
                'End Select
            End If
            If (filter.IDContesto.HasValue) Then
                'cursor2.ContextType.Value = filter.TipoContesto
                'cursor2.ContextID.Value = filter.IDContesto
            End If
            If (filter.Scopo <> "") Then
                cursor.NomeCampagna.Value = filter.Scopo & "%"
                cursor.NomeCampagna.Operator = OP.OP_LIKE
            End If

            cursor.DataEsecuzione.SortOrder = SortEnum.SORT_DESC
            cursor.IgnoreRights = filter.IgnoreRights
            'cursor2.StatoOperazione.Value = StatoRichiestaCERQ.RITIRATA
            While Not cursor.EOF AndAlso (Not filter.nMax.HasValue OrElse cnt < filter.nMax)
                cnt += 1
                'items.Add(cursor2.Item)
                Me.AddActivities(items, cursor.Item)
                cursor.MoveNext()
            End While
#If Not Debug Then
            Catch ex As Exception
                Throw
            Finally
#End If
            cursor.Dispose()
#If Not Debug Then
            End Try
#End If
        End Sub

        Private Function FormatTipoCampagna(ByVal t As ADV.TipoCampagnaPubblicitaria) As String
            Select Case t
                Case ADV.TipoCampagnaPubblicitaria.eMail : Return "e-Mail"
                Case ADV.TipoCampagnaPubblicitaria.Fax : Return "Fax"
                Case ADV.TipoCampagnaPubblicitaria.NonImpostato : Return "?"
                Case ADV.TipoCampagnaPubblicitaria.Quotidiani : Return "Quotidiani"
                Case ADV.TipoCampagnaPubblicitaria.SMS : Return "SMS"
                Case ADV.TipoCampagnaPubblicitaria.Web : Return "Web"
                Case Else : Return "??"
            End Select
        End Function

        Private Sub AddActivities(ByVal col As CCollection(Of StoricoAction), ByVal res As ADV.CRisultatoCampagna)
            Dim action As New StoricoAction

            action.Data = res.DataEsecuzione
            action.IDOperatore = res.CreatoDaId
            action.NomeOperatore = res.CreatoDa.Nominativo
            action.IDCliente = res.IDDestinatario
            action.NomeCliente = res.NomeDestinatario
            action.Note = "Campagna ADV: Programmato invio <b>" & Me.FormatTipoCampagna(res.TipoCampagna) & "</b> per la campagna <b>" & res.NomeCampagna
            action.Scopo = res.NomeCampagna
            action.NumeroOIndirizzo = res.IndirizzoDestinatario
            action.Esito = EsitoChiamata.RIPOSTA
            action.DettaglioEsito = "Invio Programmato"
            action.Durata = 0
            action.Attesa = 0
            action.Tag = res
            action.Ricevuta = False
            Select Case res.StatoMessaggio
                Case ADV.StatoMessaggioCampagna.InPreparazione, ADV.StatoMessaggioCampagna.ProntoPerLaSpedizione
                    action.StatoConversazione = StatoConversazione.INATTESA
                Case Else
                    action.StatoConversazione = StatoConversazione.CONCLUSO
            End Select

            col.Add(action)
        End Sub

        Protected Overrides Sub FillSupportedTypes(items As CKeyCollection(Of String))
            items.Add("CRisultatoCampagna", "Campagna ADV")
        End Sub

        Public Sub New()

        End Sub
    End Class


End Class