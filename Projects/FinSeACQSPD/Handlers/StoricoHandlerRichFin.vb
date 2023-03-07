Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Aggiunge le richieste di finanziamento
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StoricoHandlerRichFin
        Inherits StoricoHandlerBase

        Protected Overrides Sub AggiungiInternal(items As CCollection(Of StoricoAction), filter As CRMFindFilter)
            Dim cnt As Integer = 0
            Dim cursor1 As New CRichiesteFinanziamentoCursor


#If Not DEBUG Then
            Try
#End If
            cursor1.Stato.Value = ObjectStatus.OBJECT_VALID
            If filter.Dal.HasValue Then
                cursor1.Data.Value = filter.Dal.Value
                cursor1.Data.Operator = OP.OP_GT
                If filter.Al.HasValue Then
                    cursor1.Data.Value1 = filter.Al.Value
                    cursor1.Data.Operator = OP.OP_BETWEEN
                End If
            ElseIf filter.Al.HasValue Then
                cursor1.Data.Value = filter.Al.Value
                cursor1.Data.Operator = OP.OP_LE
            End If
            If filter.IDPersona <> 0 Then
                cursor1.IDCliente.Value = filter.IDPersona
            ElseIf filter.Nominativo <> "" Then
                cursor1.NomeCliente.Value = filter.Nominativo & "%"
                cursor1.NomeCliente.Operator = OP.OP_LIKE
            End If
            If filter.IDOperatore <> 0 Then cursor1.CreatoDa.Value = filter.IDOperatore
            If filter.IDPuntoOperativo <> 0 Then cursor1.IDPuntoOperativo.Value = filter.IDPuntoOperativo
            If filter.Scopo <> "" Then
                'cursor1..Value = filter.Scopo & "%"
                'cursor1.Motivo.Operator = OP.OP_LIKE
            End If
            If filter.StatoConversazione.HasValue Then
                Select Case filter.StatoConversazione
                    Case 0 'In Attesa
                        cursor1.CreatoDa.Value = 0
                    Case 1 'in corso
                        cursor1.CreatoDa.Value = 0
                    Case Else
                        cursor1.CreatoDa.Value = 0
                        cursor1.CreatoDa.Operator = OP.OP_NE
                End Select
            End If
            If filter.Numero <> "" Then
                cursor1.NomeFonte.Value = filter.Numero & "%"
                cursor1.NomeFonte.Operator = OP.OP_LIKE
            End If
            If (filter.IDContesto.HasValue) Then
                'cursor1.id.Value = filter.TipoContesto
                'cursor1.ContextID.Value = filter.IDContesto
            End If
            cursor1.Data.SortOrder = SortEnum.SORT_DESC
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

        Private Function formatImporto(ByVal rich As CRichiestaFinanziamento) As String
            Select Case rich.TipoRichiesta
                Case TipoRichiestaFinanziamento.ALMENO : Return "Almeno " & Formats.FormatValuta(rich.ImportoRichiesto)
                Case TipoRichiestaFinanziamento.MASSIMO_POSSIBILE : Return "Massimo possibile"
                Case TipoRichiestaFinanziamento.NONSPECIFICATO : Return "Non specificato"
                Case TipoRichiestaFinanziamento.TRA : Return "Tra " & Formats.FormatValuta(rich.ImportoRichiesto) & " e " & Formats.FormatValuta(rich.ImportoRichiesto1)
                Case TipoRichiestaFinanziamento.UGUALEA : Return Formats.FormatValuta(rich.ImportoRichiesto)
                Case Else
                    Return "?"
            End Select
        End Function

        Private Sub AddActivities(ByVal col As CCollection(Of StoricoAction), ByVal rich As CRichiestaFinanziamento)
            Dim action As StoricoAction
            If (rich.ImportoRichiesto.HasValue) Then
                action = New StoricoAction
                action.Data = Formats.ToDate(rich.Data)
                action.IDOperatore = rich.CreatoDaId
                If (rich.CreatoDa IsNot Nothing) Then action.NomeOperatore = rich.CreatoDa.Nominativo
                action.IDCliente = rich.IDCliente
                action.NomeCliente = rich.NomeCliente
                action.Note = "Importo Richiesto: <b>" & Me.formatImporto(rich) & "</b>, Scopo: " & rich.Scopo & ", Note: " & rich.Note
                action.Scopo = rich.Scopo
                action.NumeroOIndirizzo = ""
                action.Esito = EsitoChiamata.RIPOSTA
                action.DettaglioEsito = ""
                action.Durata = rich.Durata
                action.Attesa = 0
                action.Tag = rich
                action.ActionSubID = 0
                action.StatoConversazione = StatoConversazione.CONCLUSO
                col.Add(action)
            End If

        End Sub

        Protected Overrides Sub FillSupportedTypes(items As CKeyCollection(Of String))
            items.Add("CRichiestaFinanziamento", "Richiesta di Finanziamento")
        End Sub


        Public Sub New()

        End Sub
    End Class


End Class