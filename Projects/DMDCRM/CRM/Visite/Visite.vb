Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Imports DMD.Anagrafica
Imports DMD.CustomerCalls




Partial Public Class CustomerCalls

    Public NotInheritable Class CVisiteClass
        Inherits CGeneralClass(Of CVisita)

        Private m_inAttesaLock As New Object
        Private m_InAttesa As  CCollection(Of CVisita)

        Friend Sub New()
            MyBase.New("modVisite", GetType(CVisiteCursor))
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

        Public Function GetStatsEffettuate(ByVal filter As CRMFilter, Optional ByVal ignoreRights As Boolean = True) As CStatisticheOperazione
            Dim dbSQL As String = "SELECT Sum([OutDateNum]) As [Num] , Sum ([OutDateTotWait]) As [Attesa], Sum([OutDateTotLen]) As [TotLen], Min([OutDateMinLen]) As [MinLen], Max([OutDateMaxLen]) As [MaxLen], SUM([OutDateCost]) AS [TotCost] FROM [tbl_CRMStats] "
            Dim wherePart As String = ""
            If (filter.IDPuntoOperativo <> 0) Then wherePart = Strings.Combine(wherePart, "[IDPuntoOperativo] = " & DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ")
            If (filter.IDOperatore <> 0) Then wherePart = Strings.Combine(wherePart, "[IDOperatore] = " & DBUtils.DBNumber(filter.IDOperatore), " AND ")
            If (filter.DataInizio.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]>=" & DBUtils.DBDate(DateUtils.GetDatePart(filter.DataInizio.Value)), " AND ")
            If (filter.DataFine.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]<=" & DBUtils.DBDate(DateUtils.GetDatePart(filter.DataFine.Value)), " AND ")
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
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try
        End Function
 
        Public Function ContaEffettuate(ByVal filter As CRMFilter, Optional ByVal ignoreRights As Boolean = True) As Integer
            Dim dbSQL As String = "SELECT Sum([OutDateNum]) As [Num] , Sum ([OutDateTotWait]) As [Attesa], Sum([OutDateTotLen]) As [TotLen], Min([OutDateMinLen]) As [MinLen], Max([OutDateMaxLen]) As [MaxLen] FROM [tbl_CRMStats] "
            Dim wherePart As String = ""
            If (filter.IDPuntoOperativo <> 0) Then wherePart = Strings.Combine(wherePart, "[IDPuntoOperativo] = " & DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ")
            If (filter.IDOperatore <> 0) Then wherePart = Strings.Combine(wherePart, "[IDOperatore] = " & DBUtils.DBNumber(filter.IDOperatore), " AND ")
            If (filter.DataInizio.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]>=" & DBUtils.DBDate(DateUtils.GetDatePart(filter.DataInizio.Value)), " AND ")
            If (filter.DataFine.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]<=" & DBUtils.DBDate(DateUtils.GetDatePart(filter.DataFine.Value)), " AND ")
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
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try
        End Function

        Public Function GetStatsRicevute(ByVal filter As CRMFilter, Optional ByVal ignoreRights As Boolean = True) As CStatisticheOperazione
            Dim dbSQL As String = "SELECT Sum([InDateNum]) As [Num], Sum ([InDateTotWait]) As [Attesa], Sum([InDateTotLen]) As [TotLen], Min([InDateMinLen]) As [MinLen], Max([InDateMaxLen]) As [MaxLen], SUM([InDateCost]) As [TotCost] FROM [tbl_CRMStats] "
            Dim wherePart As String = ""
            If (filter.IDPuntoOperativo <> 0) Then wherePart = Strings.Combine(wherePart, "[IDPuntoOperativo] = " & DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ")
            If (filter.IDOperatore <> 0) Then wherePart = Strings.Combine(wherePart, "[IDOperatore] = " & DBUtils.DBNumber(filter.IDOperatore), " AND ")
            If (filter.DataInizio.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]>=" & DBUtils.DBDate(DateUtils.GetDatePart(filter.DataInizio.Value)), " AND ")
            If (filter.DataFine.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]<=" & DBUtils.DBDate(DateUtils.GetDatePart(filter.DataFine.Value)), " AND ")
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
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try
        End Function

        Public Function ContaRicevute(ByVal filter As CRMFilter, Optional ByVal ignoreRights As Boolean = True) As Integer
            Dim dbSQL As String = "SELECT Sum([InDateNum]) As [Num], Sum ([InDateTotWait]) As [Attesa], Sum([InDateTotLen]) As [TotLen], Min([InDateMinLen]) As [MinLen], Max([InDateMaxLen]) As [MaxLen] FROM [tbl_CRMStats] "
            Dim wherePart As String = ""
            If (filter.IDPuntoOperativo <> 0) Then wherePart = Strings.Combine(wherePart, "[IDPuntoOperativo] = " & DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ")
            If (filter.IDOperatore <> 0) Then wherePart = Strings.Combine(wherePart, "[IDOperatore] = " & DBUtils.DBNumber(filter.IDOperatore), " AND ")
            If (filter.DataInizio.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]>=" & DBUtils.DBDate(DateUtils.GetDatePart(filter.DataInizio.Value)), " AND ")
            If (filter.DataFine.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]<=" & DBUtils.DBDate(DateUtils.GetDatePart(filter.DataFine.Value)), " AND ")
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
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try
        End Function
 

        Public Function GetStats(ByVal filter As CRMFilter) As CRMStatsAggregation

            Dim ret As New CRMStatsAggregation
            filter = Sistema.Types.Clone(filter)
            filter.MostraTelefonate = False
            filter.MostraAppuntamenti = True

            'ret.Previste = Ricontatti.ContaPreviste(filter)
            'filter.DataInizio = DateUtils.ToDay
            'filter.DataFine = DateUtils.DateAdd(DateInterval.Second, 3600 * 24 - 1, filter.DataInizio.Value)

            ret.Effettuate = Me.GetStatsEffettuate(filter)
            ret.Ricevute = Me.GetStatsRicevute(filter)

            Return ret
        End Function


        Public Function GetOutVisitsStats(ByVal idUfficio As Integer, ByVal operatori() As Integer, ByVal inizio As Nullable(Of Date), ByVal fine As Nullable(Of Date), Optional ByVal ignoreRights As Boolean = True) As CStatisticheOperazione
            Dim dbSQL As String = "SELECT Sum([OutDateNum]) As [Num], Sum([OutDateTotWait]) As [Attesa], Sum([OutDateTotLen]) As [TotLen], Min([OutDateMinLen]) As [MinLen], Max([OutDateMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore] In ("
            For i As Integer = 0 To UBound(operatori)
                If (i > 0) Then dbSQL &= ", "
                dbSQL &= DBUtils.DBNumber(operatori(i))
            Next
            dbSQL &= ")"
            If (inizio.HasValue) Then dbSQL &= " AND [Data]>=" & DBUtils.DBDate(DateUtils.GetDatePart(inizio.Value))
            If (fine.HasValue) Then dbSQL &= " AND [Data]<=" & DBUtils.DBDate(DateUtils.GetDatePart(fine.Value))
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
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try
        End Function

        Public Function GetInVisitsStats(ByVal idUfficio As Integer, ByVal operatori() As Integer, ByVal inizio As Nullable(Of Date), ByVal fine As Nullable(Of Date), Optional ByVal ignoreRights As Boolean = True) As CStatisticheOperazione
            Dim dbSQL As String = "SELECT Sum([InDateNum]) As [Num], Sum([InDateTotWait]) As [Attesa], Sum([InDateTotLen]) As [TotLen], Min([InDateMinLen]) As [MinLen], Max([InDateMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore] In ("
            For i As Integer = 0 To UBound(operatori)
                If (i > 0) Then dbSQL &= ", "
                dbSQL &= DBUtils.DBNumber(operatori(i))
            Next
            dbSQL &= ")"
            If (inizio.HasValue) Then dbSQL &= " AND [Data]>=" & DBUtils.DBDate(DateUtils.GetDatePart(inizio.Value))
            If (fine.HasValue) Then dbSQL &= " AND [Data]<=" & DBUtils.DBDate(DateUtils.GetDatePart(fine.Value))
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
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try
        End Function

        Public Function GetUltimaVisitaInCorso(pID As Integer) As CVisita
            If (pID = 0) Then Return Nothing
            SyncLock Me.m_inAttesaLock
                For Each v As CVisita In Me.InAttesa
                    If v.IDPersona = pID Then Return v
                Next
            End SyncLock
            Return Nothing
        End Function

        Protected Function InternalList() As CCollection(Of CVisita)
            If (Me.m_InAttesa Is Nothing) Then
                Me.m_InAttesa = New CCollection(Of CVisita)
                Dim cursor As New CVisiteCursor
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

        Public ReadOnly Property InAttesa As CCollection(Of CVisita)
            Get
                SyncLock Me.m_inAttesaLock
                    Return New CCollection(Of CVisita)(Me.InternalList)
                End SyncLock
            End Get
        End Property

        Public Sub SetInAttesa(ByVal item As CVisita)
            SyncLock Me.m_inAttesaLock
                Dim oldItem As CVisita = Me.InternalList.GetItemById(GetID(item))
                If (oldItem IsNot Nothing) Then Me.InternalList.Remove(oldItem)
                Me.InternalList.Add(item)
            End SyncLock
        End Sub

        Public Sub SetFineAttesa(ByVal item As CVisita)
            SyncLock Me.m_inAttesaLock
                Dim oldItem As CVisita = Me.InternalList.GetItemById(GetID(item))
                If (oldItem IsNot Nothing) Then Me.InternalList.Remove(oldItem)
            End SyncLock
        End Sub

        Public Function GetVisiteRicevute(ByVal ufficio As CUfficio, ByVal operatore As CUser, dataInizio As Date?, dataFine As Date?) As CCollection(Of CVisita)
            Dim ret As New CCollection(Of CVisita)
            Dim cursor As New CVisiteCursor
            If (ufficio IsNot Nothing) Then cursor.IDPuntoOperativo.Value = GetID(ufficio)
            If (operatore IsNot Nothing) Then cursor.IDOperatore.Value = GetID(operatore)
            If (dataInizio.HasValue) Then
                If (dataFine.HasValue) Then
                    cursor.Data.Between(dataInizio.Value, dataFine.Value)
                Else
                    cursor.Data.Value = dataInizio.Value
                    cursor.Data.Operator = OP.OP_GE
                End If
            ElseIf (dataFine.HasValue) Then
                cursor.Data.Value = dataFine.Value
                cursor.Data.Operator = OP.OP_LE
            End If
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.Ricevuta.Value = True
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return ret
        End Function

        Public Function GetUltimaVisita(ByVal p As CPersona) As CVisita
            Dim cursor As New CVisiteCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDPersona.Value = GetID(p)
            cursor.Data.SortOrder = SortEnum.SORT_DESC
            cursor.IgnoreRights = True
            Dim ret As CVisita = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
        End Function

    End Class

    Private Shared m_Visite As CVisiteClass = Nothing

    Public Shared ReadOnly Property Visite As CVisiteClass
        Get
            If (m_Visite Is Nothing) Then m_Visite = New CVisiteClass
            Return m_Visite
        End Get
    End Property

End Class