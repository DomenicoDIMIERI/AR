Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica



Partial Public Class CustomerCalls


    Public NotInheritable Class CEMailMessageslass
        Inherits CGeneralClass(Of CEMailMessage)

        Private m_inAttesaLock As New Object
        Private m_InAttesa As CCollection(Of CEMailMessage)

        Friend Sub New()
            MyBase.New("modCustomerCalls", GetType(EMailMessagesCursor))
            Me.m_InAttesa = Nothing
        End Sub

        Public Function GetStats(ByVal filter As CRMFilter) As CRMStatsAggregation
            Dim ret As New CRMStatsAggregation
            filter = filter.Clone
            filter.MostraAppuntamenti = False
            filter.MostraTelefonate = True

            'ret.Previste = Ricontatti.ContaPreviste(filter)
            filter = Sistema.Types.Clone(filter)

            ' ret.Effettuate = .GetStatsEffettuate(filter)
            'ret.Ricevute = .GetStatsRicevute(filter)

            Return ret
        End Function

        Public Function GetStatsEffettuate(ByVal filter As CRMFilter, Optional ByVal ignoreRights As Boolean = True) As CStatisticheOperazione
            'Dim dbSQL As String = "SELECT Sum([OutCallNum]) As [Num], Sum([OutCallTotWait]) As [Attesa], Sum([OutCallTotLen]) As [TotLen], Min([OutCallMinLen]) As [MinLen], Max([OutCallMaxLen]) As [MaxLen], SUM([OutCallCost]) AS [TotCost] FROM [tbl_CRMStats]  "
            'Dim wherePart As String = ""
            'If (filter.IDPuntoOperativo <> 0) Then wherePart = Strings.Combine(wherePart, "[IDPuntoOperativo] = " & DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ")
            'If (filter.IDOperatore <> 0) Then wherePart = Strings.Combine(wherePart, "[IDOperatore] = " & DBUtils.DBNumber(filter.IDOperatore), " AND ")
            'If (filter.DataInizio.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]>=" & DBUtils.DBDate(DateUtils.GetDatePart(filter.DataInizio.Value)), " AND ")
            'If (filter.DataFine.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]<=" & DBUtils.DBDate(DateUtils.GetDatePart(filter.DataFine.Value)), " AND ")
            'If (wherePart <> "") Then dbSQL &= " WHERE " & wherePart
            'Dim dbRis As System.Data.IDataReader = Nothing
            Dim ret As New CStatisticheOperazione
            Try
                'dbRis = CRM.StatsDB.ExecuteReader(dbSQL)
                'If (dbRis.Read) Then
                '    ret.Numero = Formats.ToInteger(dbRis("Num"))
                '    ret.MinLen = Formats.ToDouble(dbRis("MinLen"))
                '    ret.MaxLen = Formats.ToDouble(dbRis("MaxLen"))
                '    ret.TotalLen = Formats.ToDouble(dbRis("TotLen"))
                '    ret.TotalWait = Formats.ToDouble(dbRis("Attesa"))
                '    ret.TotalCost = Formats.ToDouble(dbRis("TotCost"))
                '    'ret.MinWait = Formats.ToDouble(dbRis("MinWait"))
                '    'ret.MaxWait = Formats.ToDouble(dbRis("MaxWait"))
                'End If
                Return ret
            Catch ex As Exception
                Throw
            Finally
                'If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try
        End Function


        Public Function GetStatsRicevute(ByVal filter As CRMFilter, Optional ByVal ignoreRights As Boolean = True) As CStatisticheOperazione
            'Dim dbSQL As String = "SELECT Sum([InCallNum]) As [Num], Sum([InCallTotWait]) As [Attesa], Sum([InCallTotLen]) As [TotLen], Min([InCallMinLen]) As [MinLen], Max([InCallMaxLen]) As [MaxLen], SUM([InCallCost]) AS [TotCost] FROM [tbl_CRMStats] "
            'Dim wherePart As String = ""
            'If (filter.IDPuntoOperativo <> 0) Then wherePart = Strings.Combine(wherePart, "[IDPuntoOperativo] = " & DBUtils.DBNumber(filter.IDPuntoOperativo), " AND ")
            'If (filter.IDOperatore <> 0) Then wherePart = Strings.Combine(wherePart, "[IDOperatore] = " & DBUtils.DBNumber(filter.IDOperatore), " AND ")
            'If (filter.DataInizio.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]>=" & DBUtils.DBDate(DateUtils.GetDatePart(filter.DataInizio.Value)), " AND ")
            'If (filter.DataFine.HasValue) Then wherePart = Strings.Combine(wherePart, "[Data]<=" & DBUtils.DBDate(DateUtils.GetDatePart(filter.DataFine.Value)), " AND ")
            'If (wherePart <> "") Then dbSQL &= " WHERE " & wherePart
            'Dim dbRis As System.Data.IDataReader = Nothing
            Dim ret As New CStatisticheOperazione
            Try
                'dbRis = CRM.StatsDB.ExecuteReader(dbSQL)
                'If (dbRis.Read) Then
                '    ret.Numero = Formats.ToInteger(dbRis("Num"))
                '    ret.MinLen = Formats.ToDouble(dbRis("MinLen"))
                '    ret.MaxLen = Formats.ToDouble(dbRis("MaxLen"))
                '    ret.TotalLen = Formats.ToDouble(dbRis("TotLen"))
                '    ret.TotalWait = Formats.ToDouble(dbRis("Attesa"))
                '    ret.TotalCost = Formats.ToDouble(dbRis("TotCost"))
                'End If
                Return ret
            Catch ex As Exception
                Throw
            Finally
                'If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try
        End Function

        Private Function InnerInAttesa() As CCollection(Of CEMailMessage)
            If (Me.m_InAttesa Is Nothing) Then
                Me.m_InAttesa = New CCollection(Of CEMailMessage)
                Dim cursor As New EMailMessagesCursor
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

        Public ReadOnly Property InAttesa As CCollection(Of CEMailMessage)
            Get
                SyncLock Me.m_inAttesaLock
                    Return New CCollection(Of CEMailMessage)(Me.InnerInAttesa)
                End SyncLock
            End Get
        End Property

        Public Sub SetInAttesa(ByVal item As CEMailMessage)
            SyncLock Me.m_inAttesaLock
                Dim oldItem As CEMailMessage = Me.InnerInAttesa.GetItemById(GetID(item))
                If (oldItem IsNot Nothing) Then Me.InnerInAttesa.Remove(oldItem)
                Me.InnerInAttesa.Add(item)
            End SyncLock
        End Sub

        Public Sub SetFineAttesa(ByVal item As CEMailMessage)
            SyncLock Me.m_inAttesaLock
                Dim oldItem As CEMailMessage = Me.InnerInAttesa.GetItemById(GetID(item))
                If (oldItem IsNot Nothing) Then Me.InnerInAttesa.Remove(oldItem)
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

    End Class

    Private Shared m_EMailMessages As CEMailMessageslass = Nothing

    Public Shared ReadOnly Property EMailMessages As CEMailMessageslass
        Get
            If (m_EMailMessages Is Nothing) Then m_EMailMessages = New CEMailMessageslass
            Return m_EMailMessages
        End Get
    End Property


End Class