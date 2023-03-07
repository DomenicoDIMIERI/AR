Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica



Partial Public Class CustomerCalls

    ''' <summary>
    ''' Aggiunge telefonate, visite, ecc
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StoricoHandlerContatti
        Inherits StoricoHandlerBase

        Protected Overrides Sub AggiungiInternal(items As CCollection(Of StoricoAction), filter As CRMFindFilter)
            Dim cnt As Integer = 0
            Dim cursor As New CCustomerCallsCursor
            Dim dbRis As System.Data.IDataReader = Nothing

#If Not Debug Then
            Try
#End If
            If (filter.IDPersona <> 0) Then
                'cursor.IDPersona.Value = filter.IDPersona
                cursor.WhereClauses.Add("([IDPersona]=" & DBUtils.DBNumber(filter.IDPersona) & " OR [IDPerContoDi]=" & DBUtils.DBNumber(filter.IDPersona) & ")")
            Else
                If (filter.Nominativo <> "") Then
                    'cursor.NomePersona.Operator = OP.OP_LIKE
                    'cursor.NomePersona.Value = filter.Nominativo & "%"

                    cursor.WhereClauses.Add("([NomePersona] like '" & Strings.Replace(filter.Nominativo, "'", "''") & "%' OR [NomePerContoDi] like '" & Strings.Replace(filter.Nominativo, "'", "''") & "%')")
                End If
            End If
            If filter.IDOperatore <> 0 Then cursor.IDOperatore.Value = filter.IDOperatore
            If filter.IDPuntoOperativo <> 0 Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
            If filter.Contenuto <> "" Then
                ' cursor.Note.Value = filter.Contenuto
                'cursor.Note.Operator = OP.OP_LIKE
            End If
            If filter.Etichetta <> "" Then
                cursor.NomeIndirizzo.Value = filter.Etichetta & "%"
                cursor.NomeIndirizzo.Operator = OP.OP_LIKE
            End If
            If filter.Numero <> "" Then
                cursor.NumeroOIndirizzo.Value = filter.Numero & "%"
                cursor.NumeroOIndirizzo.Operator = OP.OP_LIKE
            End If
            If filter.TipoOggetto <> "" Then cursor.ClassName.Value = filter.TipoOggetto
            If filter.Dal.HasValue Then
                cursor.Data.Value = filter.Dal.Value
                cursor.Data.Operator = OP.OP_GT
                If filter.Al.HasValue Then
                    cursor.Data.Value1 = filter.Al.Value
                    cursor.Data.Operator = OP.OP_BETWEEN
                End If
            ElseIf filter.Al.HasValue Then
                cursor.Data.Value = filter.Al.Value
                cursor.Data.Operator = OP.OP_LE
            End If
            If TestFlag(filter.Flags, 1) Xor TestFlag(filter.Flags, 2) Then cursor.Ricevuta.Value = TestFlag(filter.Flags, 1)
            If filter.Scopo <> "" Then
                cursor.Scopo.Value = filter.Scopo & "%"
                cursor.Scopo.Operator = OP.OP_LIKE
            End If
            If filter.DettaglioEsito <> "" Then
                cursor.DettaglioEsito.Value = filter.DettaglioEsito & "%"
                cursor.DettaglioEsito.Operator = OP.OP_LIKE
            End If
            If (filter.Esito.HasValue) Then cursor.Esito.Value = filter.Esito.Value
            If (filter.IDContesto.HasValue) Then
                cursor.Contesto.Value = filter.TipoContesto
                cursor.IDContesto.Value = filter.IDContesto
            End If
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            ' cursor.Data.SortOrder = SortEnum.SORT_DESC
            cursor.IgnoreRights = filter.IgnoreRights
            If (filter.StatoConversazione.HasValue) Then cursor.StatoConversazione.Value = filter.StatoConversazione

            cursor.Data.SortOrder = SortEnum.SORT_DESC
            'If (filter.IDPersona = 0) AndAlso (cursor.Data.IsSet = False) Then
            '    cursor.Data.Value = Calendar.DateAdd(DateInterval.Month, -3, Now)
            '    cursor.Data.Operator = OP.OP_GE
            'End If

            Dim dbSQL As String = cursor.GetFullSQL
            Dim conn As CDBConnection = CRM.TelDB
            'If (filter.IDPersona = 0) AndAlso (cursor.Data.IsSet = False) Then
            '    dbSQL = Strings.Replace(dbSQL, "tbl_Telefonate", "tbl_TelefonateQuick")
            '    'conn = CRM.StatsDB
            'End If
            cursor.Dispose()

            dbRis = conn.ExecuteReader(dbSQL)
            While dbRis.Read AndAlso (Not filter.nMax.HasValue OrElse cnt < filter.nMax)
                cnt += 1
                Dim item As CContattoUtente = Types.CreateInstance(Formats.ToString(dbRis("ClassName")))
                CRM.TelDB.Load(item, dbRis)
                Me.AddActivities(items, item)
            End While
            'While Not cursor.EOF AndAlso cnt < filter.nMax
            '    cnt += 1
            '    'items.Add(cursor.Item)
            '    Me.AddActivities(items, cursor.Item)
            '    cursor.MoveNext()
            'End While
#If Not Debug Then
            Catch ex As Exception
                Throw
            Finally
#End If
                'cursor.Dispose()
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
#If Not Debug Then
            End Try
#End If
        End Sub

        Private Sub AddActivities(ByVal col As CCollection(Of StoricoAction), ByVal contatto As CContattoUtente)
            Dim action As New StoricoAction

            action.Data = contatto.Data
            action.IDOperatore = contatto.IDOperatore
            action.NomeOperatore = contatto.NomeOperatore
            action.IDCliente = contatto.IDPersona
            action.NomeCliente = contatto.NomePersona & CStr(IIf(contatto.IDPerContoDi <> 0, " per conto di " & contatto.NomePerContoDi, ""))
            action.Note = contatto.Note
            action.Scopo = contatto.Scopo
            action.NumeroOIndirizzo = contatto.NumeroOIndirizzo
            If (action.NumeroOIndirizzo = "" AndAlso TypeOf (contatto) Is CVisita) Then
                With DirectCast(contatto, CVisita).Luogo
                    action.NumeroOIndirizzo = Strings.Combine(.Nome, .ToString, vbNewLine)
                End With
            End If
            action.Esito = contatto.Esito
            action.DettaglioEsito = contatto.DettaglioEsito
            action.Durata = contatto.Durata
            action.Attesa = contatto.Attesa
            action.Tag = contatto
            action.Ricevuta = contatto.Ricevuta
            action.StatoConversazione = contatto.StatoConversazione
            action.Attachment = contatto.Attachment

            col.Add(action)
        End Sub

      
       
        Protected Overrides Sub FillSupportedTypes(items As CKeyCollection(Of String))
            items.Add("CTelefonata", "Telefonata")
            items.Add("CVisita", "Visita")
            items.Add("CAppunto", "Appunto")
            items.Add("SMSMessage", "SMS")
            items.Add("FaxDocument", "Fax")
            items.Add("CEMailMessage", "e-mail")
            items.Add("CTelegramma", "Telegramma")
        End Sub

    End Class


End Class