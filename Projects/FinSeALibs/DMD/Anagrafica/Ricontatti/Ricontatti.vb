Imports DMD
Imports DMD.Sistema
Imports DMD.Databases


#Const usa_tbl_RicontattiQuick = False

Partial Public Class Anagrafica

 
    Public NotInheritable Class CRicontattiClass
        Inherits CGeneralClass(Of CRicontatto)

        ''' <summary>
        ''' Evento generato quando viene creato un ricontatto
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RicontattoCreated(ByVal sender As Object, ByVal e As RicontattoEventArgs)

        ''' <summary>
        ''' Evento generato quando viene eliminato un ricontatto
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RicontattoDeleted(ByVal sender As Object, ByVal e As RicontattoEventArgs)

        ''' <summary>
        ''' Evento generato quando viene modificato un ricontatto
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RicontattoModified(ByVal sender As Object, ByVal e As RicontattoEventArgs)


        Friend Sub New()
            MyBase.New("modRicontatti", GetType(CRicontattiCursor))
        End Sub



        Public Function FormatStatoRicontatto(ByVal value As StatoRicontatto) As String
            Dim ret As String
            Select Case value
                Case StatoRicontatto.NONPROGRAMMATO : ret = "NON PROGRAMMATO"
                Case StatoRicontatto.PROGRAMMATO : ret = "PROGRAMMATO"
                Case StatoRicontatto.EFFETTUATO : ret = "EFFETTUATO"
                Case StatoRicontatto.RIMANDATO : ret = "RIMANDATO"
                Case StatoRicontatto.ANNULLATO : ret = "ANNULLATO"
                Case Else : ret = ""
            End Select
            Return ret
        End Function

        Public Function GetProssimoRicontatto(ByVal persona As CPersona) As CRicontatto ', Optional ByVal nomeLista As String = vbNullString
            Return GetProssimoRicontatto(GetID(persona)) 'nomeLista
        End Function

        ''' <summary>
        ''' Restituisce il prossimo ricontatto a partire da adesso
        ''' </summary>
        ''' <param name="idPersona"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetProssimoRicontatto(ByVal idPersona As Integer) As CRicontatto ', Optional ByVal nomeLista As String = vbNullString
            Dim cursor As New CRicontattiCursor
            Dim ret As CRicontatto
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDPersona.Value = idPersona
            cursor.DataPrevista.Value = Now()
            cursor.DataPrevista.Operator = OP.OP_GE
            cursor.DataPrevista.SortOrder = SortEnum.SORT_ASC
            cursor.StatoRicontatto.Value = StatoRicontatto.PROGRAMMATO
            ret = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
        End Function

        Public Function GetUltimoRicontatto(ByVal persona As CPersona) As CRicontatto ', Optional ByVal nomeLista As String = vbNullString
            Return GetUltimoRicontatto(GetID(persona)) ', nomeLista
        End Function

        Public Function GetUltimoRicontatto(ByVal idPersona As Integer) As CRicontatto ', Optional ByVal nomeLista As String = vbNullString
            If (idPersona = 0) Then Return Nothing
            Dim cursor As New CRicontattiCursor
            Dim ret As CRicontatto
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDPersona.Value = idPersona
            cursor.DataPrevista.Value = Now()
            cursor.DataPrevista.Operator = OP.OP_LT
            cursor.DataPrevista.SortOrder = SortEnum.SORT_ASC
            cursor.StatoRicontatto.Value = StatoRicontatto.EFFETTUATO
            ret = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
        End Function

        Public Function GetRicontattoBySource(ByVal source As Object) As CRicontatto
            Return GetRicontattoBySource(TypeName(source), GetID(source))
        End Function

        Public Function GetRicontattoBySource(ByVal sourceName As String, ByVal param As String) As CRicontatto
            Dim cursor As CRicontattiCursor = Nothing
            Try
                sourceName = Trim(sourceName)
                param = Trim(param)
                If (sourceName = vbNullString AndAlso param = vbNullString) Then Return Nothing

                cursor = New CRicontattiCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.SourceName.Value = sourceName
                cursor.SourceParam.Value = param
                cursor.ID.SortOrder = SortEnum.SORT_DESC
                cursor.IgnoreRights = True
                Return cursor.Item
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function


        Public Function ProgrammaRicontatto( _
                                ByVal persona As CPersona, _
                                ByVal data As Date, _
                                ByVal motivo As String, _
                                ByVal sourceName As String, _
                                ByVal sourceParam As String, _
                                ByVal nomeLista As String, _
                                ByVal puntoOperativo As CUfficio, _
                                ByVal operatore As CUser _
                                ) As CRicontatto
            If (persona Is Nothing) Then Return Nothing
            Dim item As CRicontatto
            nomeLista = Trim(nomeLista)
            If (nomeLista = "") Then
                item = New CRicontatto
            Else
                item = New ListaRicontattoItem
                With DirectCast(item, ListaRicontattoItem)
                    .NomeLista = nomeLista
                End With
            End If

            item.DataPrevista = data
            'item.PuntoOperativo = persona.PuntoOperativo
            item.Note = motivo
            item.Persona = persona
            item.SourceName = sourceName
            item.SourceParam = sourceParam
            item.Stato = ObjectStatus.OBJECT_VALID
            item.StatoRicontatto = StatoRicontatto.PROGRAMMATO
            item.GiornataIntera = False
            item.PuntoOperativo = puntoOperativo
            item.AssegnatoA = operatore
            'item.Produttore = Anagrafica.Aziende.AziendaPrincipale
            item.Save()


            Return item
        End Function

        Public Function AnnullaRicontattoBySource(ByVal src As Object) As CRicontatto
            Return Me.AnnullaRicontattoBySource(TypeName(src), GetID(src))
        End Function

        Public Function AnnullaRicontattoBySource(ByVal source As String, ByVal param As String) As CRicontatto
            Dim ric As CRicontatto = GetRicontattoBySource(source, param)
            If Not (ric Is Nothing) Then
                ric.StatoRicontatto = StatoRicontatto.ANNULLATO
                ric.Save()
            End If
            Return ric
        End Function




        Public Function ContaRicontattiPrevistiPerData(ByVal filter As CRMFilter) As Integer
            Dim cursor As New CRicontattiCursor
            Try
                'd1 = Calendar.GetDatePart(fine)
                'd1 = DateAdd("s", (24 * 3600) - 1, d1)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                If (filter.DataFine.HasValue) Then cursor.DataRicontatto.Value = filter.DataFine
                cursor.DataRicontatto.Operator = OP.OP_LE
                cursor.StatoRicontatto.Value = StatoRicontatto.PROGRAMMATO
                If filter.IDPuntoOperativo <> 0 Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                If filter.IDOperatore <> 0 Then cursor.IDOperatore.Value = filter.IDOperatore
                Dim ret As Integer
                ret = cursor.Count
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Function GetActivePersons(ByVal filter As CRMFilter, ByVal cursor As CPersonaCursor) As CCollection(Of CActivePerson)
            Dim conn As CDBConnection = Databases.APPConn
            Dim ret As New CCollection(Of CActivePerson)

            If (conn.IsRemote) Then
                Dim strCursor As String = ""
                If (cursor IsNot Nothing) Then strCursor = XML.Utils.Serializer.Serialize(cursor)
                Dim tmp As String = RPC.InvokeMethod("/widgets/websvc/modCRM.aspx?_a=GetActivePersons", "filter", RPC.str2n(XML.Utils.Serializer.Serialize(filter)), "cursor", RPC.str2n(strCursor))
                If (tmp <> "") Then ret.AddRange(XML.Utils.Serializer.Deserialize(tmp))
            Else
                Dim reader As DBReader = Nothing

                Try
                    Dim dbSQL As New System.Text.StringBuilder
                    Dim wherePart As New System.Text.StringBuilder
                    Dim tmpSQL As New System.Text.StringBuilder
                    Dim p As CActivePerson
                    Dim r As CRicontatto
#If usa_tbl_RicontattiQuick Then
                    Dim tableName As String = "tbl_RicontattiQuick"
#Else
                    Dim tableName As String = "tbl_Ricontatti"
#End If
                    Dim t1 As Double = Timer
                    ret.Capacity = 1000

                    wherePart.Append("[Stato]=")
                    wherePart.Append(ObjectStatus.OBJECT_VALID)
                    wherePart.Append(" And [StatoRicontatto]=")
                    wherePart.Append(StatoRicontatto.PROGRAMMATO) '(([StatoRicontatto] Mod 2)=1)"

                    If (filter.IDOperatore <> 0) Then
                        wherePart.Append(" AND ([IDAssegnatoA]=0 Or [IDAssegnatoA]=")
                        wherePart.Append(DBUtils.DBNumber(filter.IDOperatore))
                        wherePart.Append(")")
                    End If

                    If (filter.IDPuntoOperativo <> 0) Then
                        wherePart.Append(" AND ([IDPuntoOperativo]=0 Or [IDPuntoOperativo]=")
                        wherePart.Append(DBUtils.DBNumber(filter.IDPuntoOperativo))
                        wherePart.Append(")")
                    End If

                    If (filter.NomeLista <> "") Then
                        wherePart.Append(" AND ([NomeLista]=")
                        wherePart.Append(DBUtils.DBString(filter.NomeLista))
                        wherePart.Append(")")
                        tableName = "tbl_ListeRicontattoItems"
                        conn = Anagrafica.ListeRicontatto.Database
                    End If

                    If (filter.Motivo <> "") Then
                        wherePart.Append(" AND ([Note] Like '%")
                        wherePart.Append(Replace(filter.Motivo, "'", "''"))
                        wherePart.Append("%')")
                    End If

                    'If filter.DataInizio.HasValue Then wherePart = Strings.Combine(wherePart, "[DataPrevista]>=" & DBUtils.DBDate(filter.DataInizio), " And ")
                    If filter.DataInizio.HasValue Then
                        wherePart.Append(" AND ([DataPrevistaStr]>='")
                        wherePart.Append(DBUtils.ToDBDateStr(filter.DataInizio))
                        wherePart.Append("')")
                    End If
                    'If filter.DataFine.HasValue Then wherePart = Strings.Combine(wherePart, "[DataPrevista]<=" & DBUtils.DBDate(filter.DataFine), " And ")
                    If filter.DataFine.HasValue Then
                        wherePart.Append(" AND ([DataPrevistaStr]<='")
                        wherePart.Append(DBUtils.ToDBDateStr(filter.DataFine))
                        wherePart.Append("')")
                    End If

                    If (filter.Categorie.Count > 0) Then
                        tmpSQL.Append(tableName)
                        tmpSQL.Append(".[Categoria] In (Null, ''")
                        For Each value As String In filter.Categorie
                            tmpSQL.Append(", ")
                            tmpSQL.Append(DBUtils.DBString(value))
                        Next
                        tmpSQL.Append(")")
                        wherePart.Append(" AND ")
                        wherePart.Append(tmpSQL)
                    End If
                    If (filter.TipiAppuntamento.Count > 0) Then
                        tmpSQL.Clear()
                        tmpSQL.Append(tableName)
                        tmpSQL.Append(".[TipoAppuntamento] In (Null, ''")
                        For Each value As String In filter.TipiAppuntamento
                            tmpSQL.Append(", ")
                            tmpSQL.Append(DBUtils.DBString(value))
                        Next
                        tmpSQL.Append(")")

                        wherePart.Append(" AND ")
                        wherePart.Append(tmpSQL)
                    End If

                    If Not Anagrafica.Ricontatti.Module.UserCanDoAction("list") Then
                        tmpSQL.Clear()
                        If (Anagrafica.Ricontatti.Module.UserCanDoAction("list_office")) Then
                            If (Anagrafica.Ricontatti.Module.UserCanDoAction("list_own")) Then
                                tmpSQL.Append("([CreatoDa]= ")
                                tmpSQL.Append(GetID(Users.CurrentUser))
                                tmpSQL.Append(")")
                                tmpSQL.Append(" OR ")
                            End If

                            tmpSQL.Append("([IDPuntoOperativo] In (0")
                            For i As Integer = 0 To Users.CurrentUser.Uffici.Count - 1
                                Dim u As CUfficio = Users.CurrentUser.Uffici(i)
                                tmpSQL.Append(",")
                                tmpSQL.Append(CStr(GetID(u)))
                            Next
                            tmpSQL.Append("))")
                        End If
                        If (tmpSQL.Length = 0) Then tmpSQL.Append("0<>0")
                        wherePart.Append(" AND ")
                        wherePart.Append("(")
                        wherePart.Append(tmpSQL)
                        wherePart.Append(")")
                    End If

                    Dim oggi As Date = Calendar.ToDay
                    Dim domani As Date = Calendar.ToMorrow

                    'Select Case filter.SortOrder
                    '    Case CRMFilterSortFlags.LEASTRECENT
                    '        dbSQL.Append("Select * FROM ")
                    '        dbSQL.Append(tableName)
                    '        dbSQL.Append(" WHERE ")
                    '        dbSQL.Append(wherePart)
                    '        dbSQL.Append(" ORDER BY [DataPrevistaStr] ASC, [NomePersona] ASC")
                    '    Case CRMFilterSortFlags.MOSTIMPORTANT
                    '        dbSQL.Append("Select * FROM ")
                    '        dbSQL.Append(tableName)
                    '        dbSQL.Append(" WHERE ")
                    '        dbSQL.Append(wherePart)
                    '        dbSQL.Append(" ORDER BY  [Priorita] ASC") ', [DataPrevista] DESC, [NomePersona] ASC"
                    '    Case CRMFilterSortFlags.MOSTRECENT
                    '        dbSQL.Append("SELECT * FROM ")
                    '        dbSQL.Append(tableName)
                    '        dbSQL.Append(" WHERE ")
                    '        dbSQL.Append(wherePart)
                    '        dbSQL.Append(" ORDER BY [DataPrevistaStr] DESC, [NomePersona] ASC")
                    '    Case CRMFilterSortFlags.NAME
                    '        dbSQL.Append("SELECT * FROM ")
                    '        dbSQL.Append(tableName)
                    '        dbSQL.Append(" WHERE ")
                    '        dbSQL.Append(wherePart)
                    '        dbSQL.Append(" ORDER BY [NomePersona] ASC")
                    '    Case Else
                    '        dbSQL.Append("SELECT * FROM ")
                    '        dbSQL.Append(tableName)
                    '        dbSQL.Append(" WHERE ")
                    '        dbSQL.Append(wherePart)
                    '        dbSQL.Append(" ORDER BY [DataPrevistaStr] DESC, [NomePersona] ASC")
                    'End Select

                    dbSQL.Append("SELECT * FROM ")
                    dbSQL.Append(tableName)
                    dbSQL.Append(" WHERE ")
                    dbSQL.Append(wherePart)

                    reader = New DBReader(conn.Tables(tableName), dbSQL.ToString)
                    ret.Sorted = False

                    t1 = Timer

                    Dim buffer As New System.Text.StringBuilder

                    Dim cnt As Integer = 0

                    Dim ricCol As New CCollection(Of CRicontatto)
                    While reader.Read ' AndAlso ((filter.nMax.HasValue = False) OrElse (cnt < 2 * filter.nMax.Value))
                        r = New CRicontatto
                        APPConn.Load(r, reader)
                        If (r.IDPersona <> 0) Then
                            ricCol.Add(r)
                            If (buffer.Length > 0) Then buffer.Append(",")
                            buffer.Append(DBUtils.DBNumber(r.IDPersona))
                            cnt += 1
                        End If
                    End While
                    reader.Dispose()
                    reader = Nothing

                    If (ricCol.Count > 0) Then
                        dbSQL.Clear()
                        If (filter.ResidenteA <> "") Then
                            cursor.ResidenteA_Citta.Value = Luoghi.GetComune(filter.ResidenteA)
                            cursor.ResidenteA_Provincia.Value = Luoghi.GetProvincia(filter.ResidenteA)
                        End If
                        dbSQL.Append("SELECT * FROM (" & cursor.GetSQL & ") WHERE [ID] In (")
                        dbSQL.Append(buffer)
                        dbSQL.Append(") And [Stato]=")
                        dbSQL.Append(ObjectStatus.OBJECT_VALID)
                        If TestFlag(filter.Flags, CRMFilterFlags.SOLOAZIENDE) Then
                            dbSQL.Append(" AND [TipoPersona]<>")
                            dbSQL.Append(Anagrafica.TipoPersona.PERSONA_FISICA)
                        End If

                        If (filter.TipiRapporto.Count > 0) Then
                            dbSQL.Append(" AND [IMP_TipoRapporto] In (")
                            For i As Integer = 0 To filter.TipiRapporto.Count - 1
                                If (i > 0) Then dbSQL.Append(",")
                                dbSQL.Append(DBUtils.DBString(filter.TipiRapporto(i)))
                            Next
                            dbSQL.Append(")")
                        End If

                        reader = New DBReader(APPConn.Tables("tbl_Persone"), dbSQL.ToString)
                        cnt = 0
                        Dim persone As New System.Collections.Hashtable
                        Dim persona As CPersona
                        While reader.Read
                            persona = Anagrafica.Persone.Instantiate(Formats.ToInteger(reader.GetValue("TipoPersona")))
                            APPConn.Load(persona, reader)
                            persone.Add("P" & GetID(persona), persona)
                            cnt += 1
                        End While
                        reader.Dispose()
                        reader = Nothing

                        cnt = 0
                        While (cnt < ricCol.Count) AndAlso ((filter.nMax.HasValue = False) OrElse (ret.Count < filter.nMax.Value))
                            r = ricCol(cnt)
                            If (persone.ContainsKey("P" & r.IDPersona)) Then
                                persona = persone("P" & r.IDPersona)

                                p = New CActivePerson
                                p.Data = r.DataPrevista
                                p.Notes = r.Note
                                p.Ricontatto = r
                                p.GiornataIntera = r.GiornataIntera
                                p.Promemoria = r.Promemoria
                                p.Categoria = r.Categoria
                                p.PersonID = GetID(persona)
                                p.Person = persona
                                p.IconURL = persona.IconURL

                                ret.Add(p)
                            End If
                            cnt += 1
                        End While

                    End If

                    ret.Comparer = filter
                    ret.Sort()


                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally
                    If (reader IsNot Nothing) Then reader.Dispose() : reader = Nothing
                End Try
            End If

            Return ret
        End Function


        Public Function ContaPreviste(ByVal filter As CRMFilter, ByVal cursor As CPersonaCursor) As Integer
            Return Me.GetActivePersons(filter, cursor).Count
        End Function



        Public Function ContaRicontattiPerData(ByVal op As CUser, ByVal data As Date) As Integer
            If (op Is Nothing) Then Throw New ArgumentNullException("op")
            Return Me.ContaRicontattiPerData(GetID(op), data)
        End Function

        Public Function ContaRicontattiPerData(ByVal oID As Integer, ByVal data As Date) As Integer
            If (oID = 0) Then Return 0

            Dim cursor As New CRicontattiCursor
            Try
                Dim di As Date = Calendar.GetDatePart(data)
                Dim df As Date = Calendar.DateAdd(DateInterval.Second, 3600 * 24 - 1, di)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDAssegnatoA.Value = oID
                cursor.DataPrevista.Value = di
                cursor.DataPrevista.Value1 = df
                cursor.DataPrevista.Operator = Databases.OP.OP_BETWEEN
                cursor.IgnoreRights = True

                Return cursor.Count
            Catch ex As Exception
                Throw
            Finally
                cursor.Dispose()
            End Try
        End Function

        Public Function GetRicontatto(ByVal p As CPersona) As CRicontatto ', ByVal nomeLista As String
            If (p Is Nothing) Then Throw New ArgumentNullException("p")
            Return GetRicontatto(GetID(p))
        End Function

        Public Function GetRicontatto(ByVal idPersona As Integer) As CRicontatto ', ByVal nomeLista As String
            Dim cursor As CRicontattiCursor = Nothing
            Try
                If (idPersona = 0) Then Return Nothing
                cursor = New CRicontattiCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.StatoRicontatto.Value = StatoRicontatto.PROGRAMMATO
                cursor.IDPersona.Value = idPersona
                cursor.DataPrevista.SortOrder = SortEnum.SORT_ASC
                cursor.WhereClauses.Add(Strings.JoinW("([NFlags] And ", CStr(RicontattoFlags.Reserved), ") <> 0"))

                Dim ret As CRicontatto = cursor.Item
                While Not cursor.EOF
                    If (cursor.Item.IDAssegnatoA = GetID(Users.CurrentUser)) Then
                        ret = cursor.Item
                    End If
                    cursor.MoveNext()
                End While

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            End Try

        End Function

        Public Function GetRicontatti(ByVal idPersona As Integer) As CCollection(Of CRicontatto) ', ByVal nomeLista As String
            Dim ret As New CCollection(Of CRicontatto)
            If (idPersona = 0) Then Return ret

            Dim cursor As New CRicontattiCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.StatoRicontatto.Value = StatoRicontatto.PROGRAMMATO
            cursor.IDPersona.Value = idPersona
            cursor.DataPrevista.SortOrder = SortEnum.SORT_ASC

            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return ret
        End Function

        Public Function GetRicontattoByOperatore(ByVal operatore As CUser, ByVal p As CPersona) As CRicontatto ', ByVal nomeLista As String
            If (operatore Is Nothing) Then Throw New ArgumentNullException("operatore")
            If (p Is Nothing) Then Throw New ArgumentNullException("p")
            If (GetID(p) = 0) Then Return Nothing
            Return GetRicontattoByOperatore(GetID(operatore), GetID(p)) ', nomeLista
        End Function

        Public Function GetRicontattoByOperatore(ByVal idoperatore As Integer, ByVal idPersona As Integer) As CRicontatto ', ByVal nomeLista As String
            If (idPersona = 0) Then Return Nothing
            Dim cursor As New CRicontattiCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.StatoRicontatto.Value = StatoRicontatto.PROGRAMMATO
            cursor.IDPersona.Value = idPersona
            cursor.DataPrevista.SortOrder = SortEnum.SORT_ASC
            cursor.IDAssegnatoA.Value = idoperatore

            Dim ret As CRicontatto = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return ret
        End Function

        Friend Sub doRicontattoCreated(ByVal e As RicontattoEventArgs)
            RaiseEvent RicontattoCreated(Me, e)
            Me.Module.DispatchEvent(New EventDescription("ricontatto_created", "Creato ricontatto per " & e.Ricontatto.NomePersona, e))
        End Sub

        Friend Sub doRicontattoDeleted(ByVal e As RicontattoEventArgs)
            RaiseEvent RicontattoDeleted(Me, e)
            Me.Module.DispatchEvent(New EventDescription("ricontatto_deleted", "Eliminato ricontatto per " & e.Ricontatto.NomePersona, e))
        End Sub

        Friend Sub doRicontattoModified(ByVal e As RicontattoEventArgs)
            RaiseEvent RicontattoModified(Me, e)
            Me.Module.DispatchEvent(New EventDescription("ricontatto_modified", "Modificato ricontatto per " & e.Ricontatto.NomePersona, e))
        End Sub

        Function GetRicontattiModificati(filter As CRMFilter) As CCollection(Of CRicontatto)
            Dim ret As New CCollection(Of CRicontatto)

            Dim cursor As New CRicontattiCursor()

            If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
            If (filter.IDOperatore <> 0) Then cursor.IDOperatore.Value = filter.IDOperatore
            If (filter.DataInizio.HasValue) Then
                If (filter.DataFine.HasValue) Then
                    cursor.DataPrevista.Between(filter.DataInizio, filter.DataFine)
                Else
                    cursor.DataPrevista.Value = filter.DataInizio
                    cursor.DataPrevista.Operator = OP.OP_GE
                End If
            ElseIf (filter.DataFine.HasValue) Then
                cursor.DataPrevista.Value = filter.DataFine
                cursor.DataPrevista.Operator = OP.OP_LE
            End If
            If (filter.Motivo <> "") Then
                cursor.Note().Value = Strings.JoinW("%", filter.Motivo, "%")
                cursor.Note.Operator = OP.OP_LIKE
            End If
            'filter.Flags = 0; //SystemUtils
            cursor.ModificatoIl.Value = Calendar.DateAdd("s", -60, filter.fromDate.Value)
            cursor.ModificatoIl.Operator = OP.OP_GT
            If (filter.nMax.HasValue) Then cursor.PageSize = filter.nMax.Value

            Dim i As Integer = 0
            While (Not cursor.EOF() AndAlso (filter.nMax.HasValue = False OrElse i < filter.nMax.Value))
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return ret
        End Function

    End Class

    Private Shared m_Ricontatti As CRicontattiClass = Nothing

    Public Shared ReadOnly Property Ricontatti As CRicontattiClass
        Get
            If (m_Ricontatti Is Nothing) Then m_Ricontatti = New CRicontattiClass
            Return m_Ricontatti
        End Get
    End Property

End Class