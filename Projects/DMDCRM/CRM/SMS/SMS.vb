Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Imports DMD.Anagrafica
Imports DMD.CustomerCalls




Partial Public Class CustomerCalls

    Public NotInheritable Class CSMSClass
        Inherits CGeneralClass(Of SMSMessage)

        Private m_inAttesaLock As New Object
        Private m_InAttesa As CCollection(Of SMSMessage)

        Friend Sub New()
            MyBase.New("modSMS", GetType(SMSMessageCursor))
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

        Public Function GetStatsSMSInviati(ByVal filter As CRMFilter, Optional ByVal ignoreRights As Boolean = True) As CStatisticheOperazione
            Dim dbSQL As String = "SELECT Sum([OutSMSNum]) As [Num] , Sum([OutSMSTotLen]) As [TotLen], Min([OutSMSMinLen]) As [MinLen], Max([OutSMSMaxLen]) As [MaxLen], SUM([OutSMSCost]) As [TotCost] FROM [tbl_CRMStats]  "
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
                    ret.TotalCost = Formats.ToDouble(dbRis("TotCost"))
                End If
                Return ret
            Catch ex As Exception
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try
        End Function

        Public Function ContaSMSInviati(ByVal filter As CRMFilter, Optional ByVal ignoreRights As Boolean = True) As Integer
            Dim dbSQL As String = "SELECT Sum([OutSMSNum]) As [Num] , Sum([OutSMSTotLen]) As [TotLen], Min([OutSMSMinLen]) As [MinLen], Max([OutSMSMaxLen]) As [MaxLen] FROM [tbl_CRMStats]  "
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
                End If
                Return ret.Numero
            Catch ex As Exception
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try
        End Function

        Public Function GetStatsSMSRicevuti(ByVal filter As CRMFilter, Optional ByVal ignoreRights As Boolean = True) As CStatisticheOperazione
            Dim dbSQL As String = "SELECT Sum([InSMSNum]) As [Num], Sum([InSMSTotLen]) As [TotLen], Min([InSMSMinLen]) As [MinLen], Max([InSMSMaxLen]) As [MaxLen], SUM([InSMSCost]) As [TotCost] FROM [tbl_CRMStats]  "
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
                    ret.TotalCost = Formats.ToDouble(dbRis("TotCost"))
                End If
                Return ret
            Catch ex As Exception
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try
        End Function

        Public Function ContaSMSRicevuti(ByVal filter As CRMFilter, Optional ByVal ignoreRights As Boolean = True) As Integer
            Dim dbSQL As String = "SELECT Sum([InSMSNum]) As [Num], Sum([InSMSTotLen]) As [TotLen], Min([InSMSMinLen]) As [MinLen], Max([InSMSMaxLen]) As [MaxLen] FROM [tbl_CRMStats]  "
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

            ret.Effettuate = Me.GetStatsSMSInviati(filter)
            ret.Ricevute = Me.GetStatsSMSRicevuti(filter)

            Return ret
        End Function


        Public Function GetOutSMSStats(ByVal idUfficio As Integer, ByVal operatori() As Integer, ByVal inizio As Nullable(Of Date), ByVal fine As Nullable(Of Date), Optional ByVal ignoreRights As Boolean = True) As CStatisticheOperazione
            Dim dbSQL As String = "SELECT Sum([OutSMSNum]) As [Num], Sum([OutSMSTotLen]) As [TotLen], Min([OutSMSMinLen]) As [MinLen], Max([OutSMSMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore] In ("
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
                dbRis = CRM.TelDB.ExecuteReader(dbSQL)
                If (dbRis.Read) Then
                    ret.Numero = Formats.ToInteger(dbRis("Num"))
                    ret.MinLen = Formats.ToDouble(dbRis("MinLen"))
                    ret.MaxLen = Formats.ToDouble(dbRis("MaxLen"))
                    ret.TotalLen = Formats.ToDouble(dbRis("TotLen"))
                End If
                Return ret
            Catch ex As Exception
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try
        End Function

        Public Function GetInSMSStats(ByVal idUfficio As Integer, ByVal operatori() As Integer, ByVal inizio As Nullable(Of Date), ByVal fine As Nullable(Of Date), Optional ByVal ignoreRights As Boolean = True) As CStatisticheOperazione
            Dim dbSQL As String = "SELECT Sum([InSMSNum]) As [Num], Sum([InSMSTotLen]) As [TotLen], Min([InSMSMinLen]) As [MinLen], Max([InSMSMaxLen]) As [MaxLen] FROM [tbl_CRMStats] WHERE [IDOperatore] In ("
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
                dbRis = CRM.TelDB.ExecuteReader(dbSQL)
                If (dbRis.Read) Then
                    ret.Numero = Formats.ToInteger(dbRis("Num"))
                    ret.MinLen = Formats.ToDouble(dbRis("MinLen"))
                    ret.MaxLen = Formats.ToDouble(dbRis("MaxLen"))
                    ret.TotalLen = Formats.ToDouble(dbRis("TotLen"))
                End If
                Return ret
            Catch ex As Exception
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try
        End Function

        Public Function GetUltimoSMSInCorso(pID As Integer) As SMSMessage
            If (pID = 0) Then Return Nothing
            SyncLock Me.m_inAttesaLock
                For Each v As SMSMessage In Me.InAttesa
                    If v.IDPersona = pID Then Return v
                Next
            End SyncLock
            Return Nothing
        End Function

        Public ReadOnly Property InAttesa As CCollection(Of SMSMessage)
            Get
                SyncLock Me.m_inAttesaLock
                    If (Me.m_InAttesa Is Nothing) Then
                        Me.m_InAttesa = New CCollection(Of SMSMessage)
                        Dim cursor As New SMSMessageCursor
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
                End SyncLock
            End Get
        End Property

        Public Sub SetInAttesa(ByVal item As SMSMessage)
            SyncLock Me.m_inAttesaLock
                Dim oldItem As SMSMessage = Me.InAttesa.GetItemById(GetID(item))
                If (oldItem IsNot Nothing) Then Me.InAttesa.Remove(oldItem)
                Me.m_InAttesa.Add(item)
            End SyncLock
        End Sub

        Public Sub SetFineAttesa(ByVal item As SMSMessage)
            SyncLock Me.m_inAttesaLock
                Dim oldItem As SMSMessage = Me.InAttesa.GetItemById(GetID(item))
                If (oldItem IsNot Nothing) Then Me.InAttesa.Remove(oldItem)
            End SyncLock
        End Sub

        Public Function GetSMSRicevute(ByVal ufficio As CUfficio, ByVal operatore As CUser, dataInizio As Date?, dataFine As Date?) As CCollection(Of SMSMessage)
            Dim ret As New CCollection(Of SMSMessage)
            Dim cursor As New SMSMessageCursor
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

        Public Function GetUltimoSMS(ByVal p As CPersona) As SMSMessage
            Dim cursor As New SMSMessageCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDPersona.Value = GetID(p)
            cursor.Data.SortOrder = SortEnum.SORT_DESC
            cursor.IgnoreRights = True
            Dim ret As SMSMessage = cursor.Item
            cursor.Dispose()
            Return ret
        End Function

    End Class

    Private Shared m_SMS As CSMSClass = Nothing

    Public Shared ReadOnly Property SMS As CSMSClass
        Get
            If (m_SMS Is Nothing) Then m_SMS = New CSMSClass
            Return m_SMS
        End Get
    End Property

End Class