Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls
Imports DMD.Anagrafica



Partial Public Class Office

    ''' <summary>
    ''' Aggiunge le email inviate o ricevute 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StoricoHandlerEMails
        Inherits StoricoHandlerBase

        Protected Overrides Sub AggiungiInternal(items As CCollection(Of StoricoAction), filter As CRMFindFilter)
            Dim cursor As New Office.MailMessageCursor
            Dim cnt As Integer = 0

#If Not DEBUG Then
            Try
#End If

            If filter.IDOperatore <> 0 Then cursor.ID.Value = 0
            If filter.IDPuntoOperativo <> 0 Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
            If filter.Contenuto <> "" Then
                cursor.ID.Value = 0
            End If
            If filter.Etichetta <> "" Then
                cursor.ID.Value = 0
            End If
            If filter.Numero <> "" Then
                'cursor.Address.Value = filter.Numero
                cursor.ID.Value = 0
            End If
            If filter.Dal.HasValue OrElse filter.Al.HasValue Then
                cursor.DeliveryDate.Between(filter.Dal, filter.Al)
            End If
            If filter.Scopo <> "" Then
                cursor.Subject.Value = filter.Scopo
                cursor.Subject.Operator = OP.OP_LIKE
            End If
            'If (filter.Esito.HasValue) Then cursor.StatoSegnalazione.Value = IIf( filter.Esito.Value, 
            'If (filter.IDContesto.HasValue) Then
            '    cursor.Contesto.Value = filter.TipoContesto
            '    cursor.IDContesto.Value = filter.IDContesto
            'End If
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            ' cursor.Data.SortOrder = SortEnum.SORT_DESC
            cursor.IgnoreRights = filter.IgnoreRights
            cursor.DeliveryDate.SortOrder = SortEnum.SORT_DESC

            If (filter.IDPersona <> 0) Then
                'cursor.IDPersona.Value = filter.IDPersona
                Dim p As CPersona = Anagrafica.Persone.GetItemById(filter.IDPersona)
                Dim arr() As String = {}
                If (p IsNot Nothing) Then
                    For Each c As CContatto In p.Recapiti
                        If (c.Valore <> "" AndAlso (c.Validated.HasValue = False OrElse c.Validated.Value = True)) Then
                            Select Case LCase(c.Tipo)
                                Case "pec", "e-mail"
                                    If (Arrays.BinarySearch(arr, LCase(c.Valore)) < 0) Then
                                        arr = Arrays.InsertSorted(arr, LCase(c.Valore))
                                    End If
                            End Select
                        End If
                    Next
                End If

                If (arr.Length = 0) Then
                    cursor.ID.Value = 0
                Else
                    'cursor.Address.ValueIn(arr)
                    cursor.ID.ValueIn(GetIDS(arr))
                End If
            Else
                If (filter.Nominativo <> "") Then
                    'cursor.NomePersona.Operator = OP.OP_LIKE
                    'cursor.NomePersona.Value = filter.Nominativo & "%"

                    'cursor.NomeCliente.Value = filter.Nominativo & "%"
                    'cursor.NomeCliente.Operator = OP.OP_LIKE
                    cursor.ID.Value = 0
                End If
            End If

            While Not cursor.EOF
                cnt += 1
                Dim item As MailMessage = cursor.Item
                Me.AddActivities(items, item)
                cursor.MoveNext()
            End While
#If Not DEBUG Then
            Catch ex As Exception
                Throw
            Finally
#End If
            'cursor.Dispose()
            cursor.Dispose()
#If Not DEBUG Then
            End Try
#End If
        End Sub

        Private Function GetIDS(ByVal arr() As String) As Integer()
            Dim ret As New System.Collections.ArrayList(arr.Length + 1)
            Dim cursor As New MailAddressCursor
            cursor.Address.ValueIn(arr)
            While Not cursor.EOF
                Dim a As MailAddress = cursor.Item
                If (a.MessageID <> 0) Then ret.Add(a.MessageID)
                cursor.MoveNext()
            End While
            cursor.Dispose()
            Return ret.ToArray(GetType(Integer))
        End Function

        Private Sub AddActivities(ByVal col As CCollection(Of StoricoAction), ByVal res As Office.MailMessage)
            Dim action As New StoricoAction
            Dim actionSubID As Integer = 0

            action.Data = res.DeliveryDate
            'action.IDOperatore = res.
            action.NomeOperatore = res.AccountName
            'action.IDCliente = res.IDCliente
            action.NomeCliente = "" 'res.To.ToString
            action.Note = res.Body
            action.Scopo = res.Subject
            action.NumeroOIndirizzo = res.To.ToString
            action.Durata = 0
            action.Attesa = 0
            action.Tag = res
            action.Ricevuta = False
            action.ActionSubID = actionSubID : actionSubID += 1
            action.StatoConversazione = StatoConversazione.CONCLUSO
            action.Esito = EsitoChiamata.RIPOSTA
            action.DettaglioEsito = ""
            col.Add(action)


        End Sub

        Protected Overrides Function IsSupportedTipoOggetto(filter As CRMFindFilter) As Boolean
            Return filter.TipoOggetto = "" OrElse filter.TipoOggetto = "MailMessage"
        End Function

        Protected Overrides Sub FillSupportedTypes(items As CKeyCollection(Of String))
            items.Add("MailMessage", "e-Mail (App)")
        End Sub

    End Class


End Class