Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Aggiunge gli studi di fattibilità
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StoricoHandlerStudiF
        Inherits StoricoHandlerBase

        Protected Overrides Sub AggiungiInternal(items As CCollection(Of StoricoAction), filter As CRMFindFilter)
            Dim cnt As Integer = 0
            Dim cursor1 As New CQSPDConsulenzaCursor


#If Not DEBUG Then
            Try
#End If
            cursor1.Stato.Value = ObjectStatus.OBJECT_VALID
            If filter.Dal.HasValue Then
                cursor1.DataConsulenza.Value = filter.Dal.Value
                cursor1.DataConsulenza.Operator = OP.OP_GT
                If filter.Al.HasValue Then
                    cursor1.DataConsulenza.Value1 = filter.Al.Value
                    cursor1.DataConsulenza.Operator = OP.OP_BETWEEN
                End If
            ElseIf filter.Al.HasValue Then
                cursor1.DataConsulenza.Value = filter.Al.Value
                cursor1.DataConsulenza.Operator = OP.OP_LE
            End If
            If filter.IDPersona <> 0 Then
                cursor1.IDCliente.Value = filter.IDPersona
            ElseIf filter.Nominativo <> "" Then
                cursor1.NomeCliente.Value = filter.Nominativo & "%"
                cursor1.NomeCliente.Operator = OP.OP_LIKE
            End If
            If filter.IDOperatore <> 0 Then cursor1.IDInseritoDa.Value = filter.IDOperatore
            If filter.IDPuntoOperativo <> 0 Then cursor1.IDPuntoOperativo.Value = filter.IDPuntoOperativo
            If filter.Scopo <> "" Then
                'cursor1..Value = filter.Scopo & "%"
                'cursor1.Motivo.Operator = OP.OP_LIKE
            End If
            If filter.StatoConversazione.HasValue Then
                Select Case filter.StatoConversazione
                    Case 0 'In Attesa
                        cursor1.IDInseritoDa.Value = 0
                    Case 1 'in corso
                        cursor1.IDInseritoDa.Value = 0
                    Case Else
                        cursor1.IDInseritoDa.Value = 0
                        cursor1.IDInseritoDa.Operator = OP.OP_NE
                End Select
            End If
            If filter.Numero <> "" Then
                'cursor1.num.Value = filter.Numero & "%"
                'cursor1.NomeFonte.Operator = OP.OP_LIKE
            End If
            If (filter.IDContesto.HasValue) Then
                'cursor1.id.Value = filter.TipoContesto
                'cursor1.ContextID.Value = filter.IDContesto
            End If
            cursor1.DataConsulenza.SortOrder = SortEnum.SORT_DESC
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



        Private Sub AddActivities(ByVal col As CCollection(Of StoricoAction), ByVal rich As CQSPDConsulenza)
            Dim action As StoricoAction

            action = New StoricoAction
            action.Data = Formats.ToDate(rich.DataConsulenza)
            action.IDOperatore = rich.IDInseritoDa
            If (rich.InseritoDa IsNot Nothing) Then action.NomeOperatore = rich.InseritoDa.Nominativo
            action.IDCliente = rich.IDCliente
            action.NomeCliente = rich.NomeCliente
            action.Note = "Studio di Fattibilità: <b>" & RPC.FormatID(GetID(rich)) & "</b>, Note: " & rich.Descrizione
            action.Scopo = "Studio di Fattibilità"
            action.NumeroOIndirizzo = RPC.FormatID(GetID(rich))
            action.Esito = EsitoChiamata.RIPOSTA
            action.DettaglioEsito = ""
            action.Durata = rich.Durata
            action.Attesa = 0
            action.Tag = rich
            action.ActionSubID = 0
            action.StatoConversazione = StatoConversazione.CONCLUSO
            col.Add(action)
        End Sub

        Protected Overrides Sub FillSupportedTypes(items As CKeyCollection(Of String))
            items.Add("CQSPDConsulenza", "Studo di fattibilità")
        End Sub


        Public Sub New()

        End Sub
    End Class


End Class