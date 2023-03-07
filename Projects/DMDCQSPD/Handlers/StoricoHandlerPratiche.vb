Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Aggiunge gli stati di lavorazione delle pratiche
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StoricoHandlerPratiche
        Inherits StoricoHandlerBase

        Private Function GetPratiche(ByVal filter As CRMFindFilter) As CKeyCollection(Of CRapportino)
            Dim pratiche As New CKeyCollection(Of CRapportino)
            Dim cursor1 As CRapportiniCursor = Nothing

            Try
                cursor1 = New CRapportiniCursor
                cursor1.Stato.Value = ObjectStatus.OBJECT_VALID
                'cursor1.StatoContatto.MacroStato = Nothing
                If (filter.IDPersona <> 0) Then cursor1.IDCliente.Value = filter.IDPersona
                If filter.IDPuntoOperativo <> 0 Then cursor1.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                'If filter.Scopo <> "" Then cursor1.StatoContatto.Note = filter.Scopo

                If (filter.Esito.HasValue) Then
                    Select Case filter.Esito
                        Case 0 'In Attesa

                        Case 1 'in corso

                        Case Else
                            cursor1.IDStatoAttuale.ValueIn({GetID(CQSPD.StatiPratica.StatoLiquidato), GetID(CQSPD.StatiPratica.StatoArchiviato), GetID(CQSPD.StatiPratica.StatoAnnullato)})
                    End Select
                End If


                If filter.Numero <> "" Then
                    ' cursor1.Luogo.Value = filter.Numero & "%"
                    ' cursor1.Luogo.Operator = OP.OP_LIKE
                    cursor1.NumeroEsterno.Value = filter.Numero
                End If
                If (filter.IDContesto.HasValue) Then
                    'cursor1.id.Value = filter.TipoContesto
                    'cursor1.ContextID.Value = filter.IDContesto
                End If
                'cursor1.DataDecorrenza.SortOrder = SortEnum.SORT_DESC
                cursor1.IgnoreRights = filter.IgnoreRights
                cursor1.StatoContatto.MacroStato = Nothing
                cursor1.StatoContatto.Inizio = filter.Dal
                cursor1.StatoContatto.Fine = filter.Al
                If (filter.IDOperatore <> 0) Then cursor1.StatoContatto.IDOperatore = filter.IDOperatore
                While Not cursor1.EOF
                    pratiche.Add("K" & GetID(cursor1.Item), cursor1.Item)
                    cursor1.MoveNext()
                End While
            Catch Ex As Exception
                Throw
            Finally
                If (cursor1 IsNot Nothing) Then cursor1.Dispose() : cursor1 = Nothing
            End Try

            Return pratiche
        End Function

        Private Function GetIDPRatiche(ByVal pratiche As CKeyCollection(Of CRapportino)) As Integer()
            Dim ret As Integer() = {}
            For Each p As CRapportino In pratiche
                If (Arrays.BinarySearch(ret, GetID(p)) < 0) Then
                    ret = Arrays.InsertSorted(ret, GetID(p))
                End If
            Next
            Return ret
        End Function

        Private Function GetStatiLav(ByVal pratiche As CKeyCollection(Of CRapportino), ByVal filter As CRMFindFilter)
            Dim statiLav As New CCollection(Of CStatoLavorazionePratica)

            Dim arr As Integer() = Me.GetIDPRatiche(pratiche)
            If (arr.Length = 0) Then Return statiLav

            Dim cursor2 As New CStatiLavorazionePraticaCursor
            cursor2.IDPratica.ValueIn(arr)
            cursor2.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor2.IgnoreRights = True
            If (filter.IDOperatore <> 0) Then cursor2.IDOperatore.Value = filter.IDOperatore
            If (filter.Dal.HasValue OrElse filter.Al.HasValue) Then cursor2.Data.Between(filter.Dal, filter.Al)
            While Not cursor2.EOF
                statiLav.Add(cursor2.Item)
                cursor2.MoveNext()
            End While
            cursor2.Dispose()

            statiLav.Sort()

            Return statiLav
        End Function

        Protected Overrides Sub AggiungiInternal(items As CCollection(Of StoricoAction), filter As CRMFindFilter)
            Dim pratiche As CKeyCollection(Of CRapportino) = Me.GetPratiche(filter)
            If (pratiche.Count = 0) Then Return

            Dim statiLav As CCollection(Of CStatoLavorazionePratica) = Me.GetStatiLav(pratiche, filter)

            For Each p As CRapportino In Pratiche
                Dim col As New CStatiLavorazionePraticaCollection
                For Each stLav In statiLav
                    If stLav.IDPratica = GetID(p) Then col.Add(stLav)
                Next
                p.SetStatiDiLavorazione(col)
                Me.AddActivities(items, p)
            Next

        End Sub

        Private Sub AddActivities(ByVal col As CCollection(Of StoricoAction), ByVal rich As CRapportino)
            Dim action As StoricoAction

            For Each stl As CStatoLavorazionePratica In rich.StatiDiLavorazione
                action = New StoricoAction
                action.Data = Formats.ToDate(stl.Data)
                action.IDOperatore = stl.IDOperatore
                action.NomeOperatore = stl.NomeOperatore
                action.IDCliente = rich.IDCliente
                action.NomeCliente = rich.NominativoCliente
                Dim nP As String = rich.NumeroEsterno
                If (nP = "") Then nP = rich.NumeroPratica
                action.Note = "Pratica N° <b>" & nP & "</b> -> <i>" & stl.DescrizioneStato & "</i> : " & stl.Note
                action.Scopo = stl.DescrizioneStato
                action.NumeroOIndirizzo = rich.NumeroEsterno
                action.Esito = EsitoChiamata.RIPOSTA
                action.DettaglioEsito = stl.DescrizioneStato
                action.Durata = 0
                action.Attesa = 0
                action.Tag = rich
                action.ActionSubID = GetID(stl)
                action.StatoConversazione = StatoConversazione.CONCLUSO
                col.Add(action)
            Next
        End Sub

        Protected Overrides Sub FillSupportedTypes(items As CKeyCollection(Of String))
            items.Add("CRapportino", "Pratica")
        End Sub


        Public Sub New()

        End Sub
    End Class


End Class