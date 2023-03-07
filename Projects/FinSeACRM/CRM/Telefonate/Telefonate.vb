Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.CustomerCalls
Imports DMD.Internals

Namespace Internals

    Public NotInheritable Class CTelefonateClass
        Inherits CGeneralClass(Of CTelefonata)

        Private m_inAttesaLock As New Object
        Private m_InAttesa As CCollection(Of CTelefonata)

        Friend Sub New()
            MyBase.New("modCustomerCalls", GetType(CTelefonateCursor))
            Me.m_InAttesa = Nothing
        End Sub

        Public Function GetStats(ByVal filter As CRMFilter) As CRMStatsAggregation
            Dim ret As New CRMStatsAggregation
            filter = filter.Clone
            filter.MostraAppuntamenti = False
            filter.MostraTelefonate = True

            'ret.Previste = Ricontatti.ContaPreviste(filter)
            filter = Sistema.Types.Clone(filter)

            ret.Effettuate = Telefonate.GetStatsEffettuate(filter)
            ret.Ricevute = Telefonate.GetStatsRicevute(filter)

            Return ret
        End Function

        Public Function GetStatsEffettuate(ByVal filter As CRMFilter, Optional ByVal ignoreRights As Boolean = True) As CStatisticheOperazione
            Dim dbSQL As String = "SELECT Sum([OutCallNum]) As [Num], Sum([OutCallTotWait]) As [Attesa], Sum([OutCallTotLen]) As [TotLen], Min([OutCallMinLen]) As [MinLen], Max([OutCallMaxLen]) As [MaxLen], SUM([OutCallCost]) AS [TotCost] FROM [tbl_CRMStats]  "
            Dim wherePart As String = ""
            If (filter.IDPuntoOperativo <> 0) Then wherePart = Strings.Combine(wherePart, "[IDPuntoOperativo] = " & DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ")
            If (filter.IDOperatore <> 0) Then wherePart = Strings.Combine(wherePart, "[IDOperatore] = " & DBUtils.DBNumber(filter.IDOperatore), " AND ")
            If (filter.DataInizio.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]>=" & DBUtils.DBDate(Calendar.GetDatePart(filter.DataInizio.Value)), " AND ")
            If (filter.DataFine.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]<=" & DBUtils.DBDate(Calendar.GetDatePart(filter.DataFine.Value)), " AND ")
            If (wherePart <> "") Then dbSQL &= " WHERE " & wherePart
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim ret As New CStatisticheOperazione
            Try
                dbRis = CRM.StatsDB.ExecuteReader(dbSQL)
                If (dbRis.Read) Then
                    ret.Numero = Formats.ToInteger(dbRis("Num"))
                    ret.MinLen = Formats.ToDouble(dbRis("MinLen"))
                    ret.MaxLen = Formats.ToDouble(dbRis("MaxLen"))
                    ret.TotalLen = Formats.ToDouble(dbRis("TotLen"))
                    ret.TotalWait = Formats.ToDouble(dbRis("Attesa"))
                    ret.TotalCost = Formats.ToDouble(dbRis("TotCost"))
                    'ret.MinWait = Formats.ToDouble(dbRis("MinWait"))
                    'ret.MaxWait = Formats.ToDouble(dbRis("MaxWait"))
                End If
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Public Function ContaEffettuate(ByVal filter As CRMFilter, Optional ByVal ignoreRights As Boolean = True) As Integer
            Return Me.GetStatsEffettuate(filter, ignoreRights).Numero
        End Function


        Public Function GetStatsRicevute(ByVal filter As CRMFilter, Optional ByVal ignoreRights As Boolean = True) As CStatisticheOperazione
            Dim dbSQL As String = "SELECT Sum([InCallNum]) As [Num], Sum([InCallTotWait]) As [Attesa], Sum([InCallTotLen]) As [TotLen], Min([InCallMinLen]) As [MinLen], Max([InCallMaxLen]) As [MaxLen], SUM([InCallCost]) AS [TotCost] FROM [tbl_CRMStats] "
            Dim wherePart As String = ""
            If (filter.IDPuntoOperativo <> 0) Then wherePart = Strings.Combine(wherePart, "[IDPuntoOperativo] = " & DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ")
            If (filter.IDOperatore <> 0) Then wherePart = Strings.Combine(wherePart, "[IDOperatore] = " & DBUtils.DBNumber(filter.IDOperatore), " AND ")
            If (filter.DataInizio.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]>=" & DBUtils.DBDate(Calendar.GetDatePart(filter.DataInizio.Value)), " AND ")
            If (filter.DataFine.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]<=" & DBUtils.DBDate(Calendar.GetDatePart(filter.DataFine.Value)), " AND ")
            If (wherePart <> "") Then dbSQL &= " WHERE " & wherePart
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim ret As New CStatisticheOperazione
            Try
                dbRis = CRM.StatsDB.ExecuteReader(dbSQL)
                If (dbRis.Read) Then
                    ret.Numero = Formats.ToInteger(dbRis("Num"))
                    ret.MinLen = Formats.ToDouble(dbRis("MinLen"))
                    ret.MaxLen = Formats.ToDouble(dbRis("MaxLen"))
                    ret.TotalLen = Formats.ToDouble(dbRis("TotLen"))
                    ret.TotalWait = Formats.ToDouble(dbRis("Attesa"))
                    ret.TotalCost = Formats.ToDouble(dbRis("TotCost"))
                End If
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function
        Public Function ContaRicevute(ByVal filter As CRMFilter, Optional ByVal ignoreRights As Boolean = True) As Integer
            Dim dbSQL As String = "SELECT Sum([InCallNum]) As [Num], Sum([InCallTotWait]) As [Attesa], Sum([InCallTotLen]) As [TotLen], Min([InCallMinLen]) As [MinLen], Max([InCallMaxLen]) As [MaxLen] FROM [tbl_CRMStats] "
            Dim wherePart As String = ""
            If (filter.IDPuntoOperativo <> 0) Then wherePart = Strings.Combine(wherePart, "[IDPuntoOperativo] = " & DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ")
            If (filter.IDOperatore <> 0) Then wherePart = Strings.Combine(wherePart, "[IDOperatore] = " & DBUtils.DBNumber(filter.IDOperatore), " AND ")
            If (filter.DataInizio.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]>=" & DBUtils.DBDate(Calendar.GetDatePart(filter.DataInizio.Value)), " AND ")
            If (filter.DataFine.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]<=" & DBUtils.DBDate(Calendar.GetDatePart(filter.DataFine.Value)), " AND ")
            If (wherePart <> "") Then dbSQL &= " WHERE " & wherePart
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim ret As New CStatisticheOperazione
            Try
                dbRis = CRM.StatsDB.ExecuteReader(dbSQL)
                If (dbRis.Read) Then
                    ret.Numero = Formats.ToInteger(dbRis("Num"))
                    ret.MinLen = Formats.ToDouble(dbRis("MinLen"))
                    ret.MaxLen = Formats.ToDouble(dbRis("MaxLen"))
                    ret.TotalLen = Formats.ToDouble(dbRis("TotLen"))
                    ret.TotalWait = Formats.ToDouble(dbRis("Attesa"))
                End If
                Return ret.Numero
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Public Function ContaTelefonateEffettuatePerData(ByVal idUfficio As Integer, ByVal idOperatore As Integer, ByVal inizio As Nullable(Of Date), ByVal fine As Nullable(Of Date), Optional ByVal ignoreRights As Boolean = True) As CStatisticheOperazione
            Dim dbSQL As String = "SELECT Sum([OutCallNum]) As [Num], Sum([OutCallTotWait]) As [Attesa], Sum([OutCallTotLen]) As [TotLen], Min([OutCallMinLen]) As [MinLen], Max([OutCallMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore]=" & DBUtils.DBNumber(idOperatore)
            If (inizio.HasValue) Then dbSQL &= " AND [Data]>=" & DBUtils.DBDate(Calendar.GetDatePart(inizio.Value))
            If (fine.HasValue) Then dbSQL &= " AND [Data]<=" & DBUtils.DBDate(Calendar.GetDatePart(fine.Value))
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim ret As New CStatisticheOperazione
            Try
                dbRis = CRM.TelDB.ExecuteReader(dbSQL)
                If (dbRis.Read) Then
                    ret.Numero = Formats.ToInteger(dbRis("Num"))
                    ret.MinLen = Formats.ToDouble(dbRis("MinLen"))
                    ret.MaxLen = Formats.ToDouble(dbRis("MaxLen"))
                    ret.TotalLen = Formats.ToDouble(dbRis("TotLen"))
                    ret.TotalWait = Formats.ToDouble(dbRis("Attesa"))
                End If
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Public Function GetOutCallsStats(ByVal idUfficio As Integer, ByVal operatori As Integer(), ByVal inizio As Nullable(Of Date), ByVal fine As Nullable(Of Date), Optional ByVal ignoreRights As Boolean = True) As CStatisticheOperazione
            Dim dbSQL As String = "SELECT Sum([OutCallNum]) As [Num], Sum([OutCallTotWait]) As [Attesa], Sum([OutCallTotLen]) As [TotLen], Min([OutCallMinLen]) As [MinLen], Max([OutCallMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore] In ("
            For i As Integer = 0 To UBound(operatori)
                If (i > 0) Then dbSQL &= ","
                dbSQL &= DBUtils.DBNumber(operatori(i))
            Next
            dbSQL &= ")"
            If (inizio.HasValue) Then dbSQL &= " AND [Data]>=" & DBUtils.DBDate(Calendar.GetDatePart(inizio.Value))
            If (fine.HasValue) Then dbSQL &= " AND [Data]<=" & DBUtils.DBDate(Calendar.GetDatePart(fine.Value))
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim ret As New CStatisticheOperazione
            Try
                dbRis = CRM.StatsDB.ExecuteReader(dbSQL)
                If (dbRis.Read) Then
                    ret.Numero = Formats.ToInteger(dbRis("Num"))
                    ret.MinLen = Formats.ToDouble(dbRis("MinLen"))
                    ret.MaxLen = Formats.ToDouble(dbRis("MaxLen"))
                    ret.TotalLen = Formats.ToDouble(dbRis("TotLen"))
                    ret.TotalWait = Formats.ToDouble(dbRis("Attesa"))
                End If
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Public Function ContaTelefonateRicevutePerData(ByVal idUfficio As Integer, ByVal idOperatore As Integer, ByVal inizio As Nullable(Of Date), ByVal fine As Nullable(Of Date), Optional ByVal ignoreRights As Boolean = True) As CStatisticheOperazione
            'If (idOperatore <> 0) Then
            '    Return Me.ContaTelefonateRicevutePerData(idUfficio, {idOperatore}, inizio, fine, ignoreRights)
            'Else
            '    Return Me.ContaTelefonateRicevutePerData(idUfficio, {}, inizio, fine, ignoreRights)
            'End If
            Dim dbSQL As String = "SELECT Sum([InCallNum]) As [Num], Sum([InCallTotWait]) As [Attesa], Sum([InCallTotLen]) As [TotLen], Min([InCallMinLen]) As [MinLen], Max([InCallMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore]=" & DBUtils.DBNumber(idOperatore)
            If (inizio.HasValue) Then dbSQL &= " AND [Data]>=" & DBUtils.DBDate(Calendar.GetDatePart(inizio.Value))
            If (fine.HasValue) Then dbSQL &= " AND [Data]<=" & DBUtils.DBDate(Calendar.GetDatePart(fine.Value))
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim ret As New CStatisticheOperazione
            Try
                dbRis = CRM.StatsDB.ExecuteReader(dbSQL)
                If (dbRis.Read) Then
                    ret.Numero = Formats.ToInteger(dbRis("Num"))
                    ret.MinLen = Formats.ToDouble(dbRis("MinLen"))
                    ret.MaxLen = Formats.ToDouble(dbRis("MaxLen"))
                    ret.TotalLen = Formats.ToDouble(dbRis("TotLen"))
                    ret.TotalWait = Formats.ToDouble(dbRis("Attesa"))
                End If
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Public Function GetInCallStats(ByVal idUfficio As Integer, ByVal operatori() As Integer, ByVal inizio As Nullable(Of Date), ByVal fine As Nullable(Of Date), Optional ByVal ignoreRights As Boolean = True) As CStatisticheOperazione
            Dim dbSQL As String = "SELECT Sum([InCallNum]) As [Num], Sum([InCallTotWait]) As [Attesa], Sum([InCallTotLen]) As [TotLen], Min([InCallMinLen]) As [MinLen], Max([InCallMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore] In ("
            For i As Integer = 0 To UBound(operatori)
                If (i > 0) Then dbSQL &= ","
                dbSQL &= DBUtils.DBNumber(operatori(i))
            Next
            dbSQL &= ")"
            If (inizio.HasValue) Then dbSQL &= " AND [Data]>=" & DBUtils.DBDate(Calendar.GetDatePart(inizio.Value))
            If (fine.HasValue) Then dbSQL &= " AND [Data]<=" & DBUtils.DBDate(Calendar.GetDatePart(fine.Value))
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim ret As New CStatisticheOperazione
            Try
                dbRis = CRM.StatsDB.ExecuteReader(dbSQL)
                If (dbRis.Read) Then
                    ret.Numero = Formats.ToInteger(dbRis("Num"))
                    ret.MinLen = Formats.ToDouble(dbRis("MinLen"))
                    ret.MaxLen = Formats.ToDouble(dbRis("MaxLen"))
                    ret.TotalLen = Formats.ToDouble(dbRis("TotLen"))
                    ret.TotalWait = Formats.ToDouble(dbRis("Attesa"))
                End If
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

        Public Function ContaPersoneContattate(ByVal idUfficio As Integer, ByVal operatori() As Integer, ByVal inizio As Nullable(Of Date), ByVal fine As Nullable(Of Date), Optional ByVal ignoreRights As Boolean = True) As Integer
            Dim d1, d2 As Nullable(Of Date)
            ' Dim ret As Integer
            d1 = Calendar.GetDatePart(inizio)
            If Types.IsNull(fine) Then
                d2 = DateAdd("s", (24 * 3600) - 1, d1)
            Else
                d2 = Calendar.GetDatePart(fine)
                d2 = DateAdd("s", (24 * 3600) - 1, d2)
            End If
            'Dim wherePart As String = "[tbl_Persone].[Stato]=" & ObjectStatus.OBJECT_VALID & " AND [tbl_Telefonate].[Stato]=" & ObjectStatus.OBJECT_VALID & " AND [tbl_Telefonate].[Ricevuta]=" & DBUtils.DBBool(False)
            Dim wherePart As String = ""
            wherePart = "[Stato]=" & ObjectStatus.OBJECT_VALID
            If (d1.HasValue) Then wherePart = Strings.Combine(wherePart, "[tbl_Telefonate].[Data]>=" & DBUtils.DBDate(d1.Value), " AND ")
            If (d2.HasValue) Then wherePart = Strings.Combine(wherePart, "[tbl_Telefonate].[Data]<=" & DBUtils.DBDate(d2.Value), " AND ")
            If idUfficio <> 0 Then wherePart = Strings.Combine(wherePart, "[tbl_Telefonate].[IDPuntoOperativo]=" & idUfficio, " AND ")
            If (Arrays.Len(operatori) > 0) Then wherePart = Strings.Combine(wherePart, "[tbl_Telefonate].[IDOperatore] In (" & DBUtils.MakeArrayStr(operatori) & ")", " AND ")

            Return Formats.ToInteger(CRM.TelDB.ExecuteScalar("SELECT Count(*) FROM (SELECT [IDPersona] FROM [tbl_Telefonate] WHERE " & wherePart & " GROUP BY [IDPersona])"))
        End Function

        Public Function GetIDPersoneContattate(ByVal idUfficio As Integer, ByVal operatori() As Integer, ByVal inizio As Nullable(Of Date), ByVal fine As Nullable(Of Date), Optional ByVal ignoreRights As Boolean = True) As Integer()
            Dim d1, d2 As Nullable(Of Date)
            ' Dim ret As Integer
            d1 = Calendar.GetDatePart(inizio)
            If Types.IsNull(fine) Then
                d2 = DateAdd("s", (24 * 3600) - 1, d1)
            Else
                d2 = Calendar.GetDatePart(fine)
                d2 = DateAdd("s", (24 * 3600) - 1, d2)
            End If
            Dim wherePart As String = "" '"[tbl_Persone].[Stato]=" & ObjectStatus.OBJECT_VALID & " AND [tbl_Telefonate].[Stato]=" & ObjectStatus.OBJECT_VALID & " AND [tbl_Telefonate].[Ricevuta]=" & DBUtils.DBBool(False)
            wherePart = "[Stato]=" & ObjectStatus.OBJECT_VALID
            If (d1.HasValue) Then wherePart = Strings.Combine(wherePart, "[tbl_Telefonate].[Data]>=" & DBUtils.DBDate(d1.Value), " AND ")
            If (d2.HasValue) Then wherePart = Strings.Combine(wherePart, "[tbl_Telefonate].[Data]<=" & DBUtils.DBDate(d2.Value), " AND ")
            If idUfficio <> 0 Then wherePart = Strings.Combine(wherePart, "[tbl_Telefonate].[IDPuntoOperativo]=" & idUfficio, " AND ")
            If (Arrays.Len(operatori) > 0) Then wherePart = Strings.Combine(wherePart, "[tbl_Telefonate].[IDOperatore] In (" & DBUtils.MakeArrayStr(operatori) & ")", " AND ")

            Dim ret() As Integer = Nothing
            Dim cnt As Integer = Formats.ToInteger(CRM.TelDB.ExecuteScalar("SELECT Count(*) FROM (SELECT [IDPersona] FROM [tbl_Telefonate] WHERE " & wherePart) & " GROUP BY [IDPersona])")
            If (cnt > 0) Then
                ReDim ret(cnt - 1)
                Dim dbRis As System.Data.IDataReader = Nothing
                dbRis = CRM.TelDB.ExecuteReader("SELECT [IDPersona] FROM [tbl_Telefonate] WHERE " & wherePart & " GROUP BY [IDPersona]")
                Dim i As Integer = 0
                While Not dbRis.Read
                    ret(i) = Formats.ToInteger(dbRis("IDPersona"))
                End While
                dbRis.Dispose()
            End If

            Return ret
        End Function



        Private Function FormatImpiego(ByVal impiego As CImpiegato) As String
            Dim ret As String = ""
            If (impiego.Posizione <> "") Then ret = impiego.Posizione
            If (impiego.TipoRapporto <> "") Then ret = Strings.Combine(ret, "(" & impiego.TipoRapporto & ")", " ")
            If (impiego.NomeAzienda <> "") Then ret = Strings.Combine(ret, impiego.NomeAzienda, " presso ")
            If (impiego.DataAssunzione.HasValue) Then ret = Strings.Combine(ret, Formats.FormatUserDate(impiego.DataAssunzione), " dal ")
            Return ret
        End Function





        Function GetUltimaTelefonataInCorso(pID As Integer) As CTelefonata
            If (pID = 0) Then Return Nothing
            SyncLock Me.m_inAttesaLock
                For Each v As CTelefonata In Me.InAttesa
                    If v.IDPersona = pID Then Return v
                Next
            End SyncLock
            Return Nothing

        End Function

        Function GetIDPersoneContattate(idUfficio As Integer, p2 As Object, inizio As Date?, fine As Date?, ir As Boolean) As Integer
            Throw New NotImplementedException
        End Function

        Protected Function InternalList() As CCollection(Of CTelefonata)
            If (Me.m_InAttesa Is Nothing) Then
                Me.m_InAttesa = New CCollection(Of CTelefonata)
                Dim cursor As New CTelefonateCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDAzienda.Value = GetID(Anagrafica.Aziende.AziendaPrincipale)
                cursor.Data.SortOrder = SortEnum.SORT_DESC
                cursor.StatoConversazione.Value = StatoConversazione.CONCLUSO
                cursor.StatoConversazione.Operator = OP.OP_NE
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    Me.m_InAttesa.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                cursor.Dispose()
            End If
            Return Me.m_InAttesa
        End Function

        Public ReadOnly Property InAttesa As CCollection(Of CTelefonata)
            Get
                SyncLock Me.m_inAttesaLock
                    Return New CCollection(Of CTelefonata)(Me.InternalList)
                End SyncLock
            End Get
        End Property

        Public Sub SetInAttesa(ByVal item As CTelefonata)
            SyncLock Me.m_inAttesaLock
                Dim oldItem As CTelefonata = Me.InternalList.GetItemById(GetID(item))
                If (oldItem IsNot Nothing) Then Me.InternalList.Remove(oldItem)
                Me.InternalList.Add(item)
            End SyncLock
        End Sub

        Public Sub SetFineAttesa(ByVal item As CTelefonata)
            SyncLock Me.m_inAttesaLock
                Dim oldItem As CTelefonata = Me.InternalList.GetItemById(GetID(item))
                If (oldItem IsNot Nothing) Then Me.InternalList.Remove(oldItem)
            End SyncLock
        End Sub

        Protected Friend Shadows Sub doItemCreated(ByVal e As ItemEventArgs)
            MyBase.doItemCreated(e)
        End Sub

        Protected Friend Shadows Sub doItemDeleted(ByVal e As ItemEventArgs)
            MyBase.doItemDeleted(e)
        End Sub

        Protected Friend Shadows Sub doItemModified(ByVal e As ItemEventArgs)
            MyBase.doItemModified(e)
        End Sub

        Private Class ByDateComparer
            Implements IComparer

            Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
                Dim s1 As CPersonStats = x
                Dim s2 As CPersonStats = y
                Return Calendar.Compare(s1.DataUltimoContatto, s2.DataUltimoContatto)
            End Function
        End Class

        Public Function GetSuggeriti(filter As CRMFilter) As CCollection(Of CActivePerson)
            Dim cursor As CPersonStatsCursor = Nothing
            Dim pcursor As CPersonaFisicaCursor = Nothing
            Dim personeID() As Integer = {}

            Try
                If (filter Is Nothing) Then Throw New ArgumentNullException("filter")

                Dim ret As New CCollection(Of CActivePerson)
                Dim tmp As New CKeyCollection(Of CPersonStats)
                Dim persona As CPersonaFisica
                Dim stats As CPersonStats

                cursor = New CPersonStatsCursor
                'If (filter.IDOperatore <> 0) Then cursor.id
                'If (filter.IDPuntoOperativo <> 0) Then cursor.IDPuntoOperativo.Value = filter.IDPuntoOperativo
                cursor.IDPuntoOperativo.Value = GetID(Sistema.Users.CurrentUser.LastLogin.Ufficio)
                cursor.DataUltimoContattoOk.Value = Calendar.DateAdd(DateInterval.Month, -8, Calendar.ToDay)
                cursor.DataUltimoContattoOk.Operator = OP.OP_LT
                cursor.DataUltimoContattoOk.SortOrder = SortEnum.SORT_DESC
                cursor.IgnoreRights = True

                'cursor.Flags.Value = (PersonFlags.PersonaFisica Or PersonFlags.Ricontattabile) And Not PersonFlags.Deceduto

                While Not cursor.EOF
                    stats = cursor.Item
                    If (stats.IDPersona <> 0) Then
                        tmp.Add("K" & stats.IDPersona, stats)
                        personeID = Arrays.Append(personeID, stats.IDPersona)
                    End If
                    cursor.MoveNext()
                End While
                cursor.Dispose() : cursor = Nothing

                If (tmp.Count > 0) Then
                    pcursor = New CPersonaFisicaCursor
                    pcursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    pcursor.Deceduto.Value = False
                    pcursor.ID.ValueIn(personeID)
                    While Not pcursor.EOF
                        persona = pcursor.Item
                        stats = tmp.GetItemByKey("K" & GetID(persona))
                        stats.Persona = persona
                        pcursor.MoveNext()
                    End While
                End If

                tmp.Comparer = New ByDateComparer
                tmp.Sort()

                For Each stats In tmp
                    If (filter.nMax.HasValue AndAlso ret.Count > filter.nMax.Value) Then Exit For

                    persona = stats.Persona
                    Dim include As Boolean = Not persona.Deceduto
                    If (include) Then
                        If (filter.TipiRapporto.Count > 0) Then
                            include = False
                            For Each tr As String In filter.TipiRapporto
                                If persona.ImpiegoPrincipale.TipoRapporto = tr Then
                                    include = True
                                    Exit For
                                End If
                            Next
                        End If
                    End If

                    If (include) Then
                        Dim a As New CActivePerson()
                        a.Person = persona
                        a.GiornataIntera = True
                        a.IconURL = persona.IconURL
                        a.MoreInfo.Add("DettaglioEsito", persona.DettaglioEsito)
                        a.MoreInfo.Add("DataUltimoContatto", stats.DataUltimoContatto)
                        ret.Add(a)
                    End If
                Next

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                If (pcursor IsNot Nothing) Then pcursor.Dispose() : pcursor = Nothing
                If (personeID IsNot Nothing) Then Erase personeID
            End Try
        End Function

    End Class

End Namespace

Partial Public Class CustomerCalls

    Private Shared m_Telefonate As CTelefonateClass = Nothing

    Public Shared ReadOnly Property Telefonate As CTelefonateClass
        Get
            If (m_Telefonate Is Nothing) Then m_Telefonate = New CTelefonateClass
            Return m_Telefonate
        End Get
    End Property


End Class