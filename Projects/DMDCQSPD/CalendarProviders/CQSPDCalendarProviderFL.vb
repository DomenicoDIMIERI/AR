Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls
Imports DMD.Anagrafica
Imports DMD.CQSPD


Partial Public Class CQSPD

    Public Class CQSPDCalendarProviderFL
        Inherits BaseCalendarActivitiesProvider

        Private Class ItemInfo
            Implements IComparable

            Public ID As Integer
            Public PO As Integer
            Public StatoFinestra As StatoFinestraLavorazione
            Public StatoContatto As StatoOfferteFL
            Public IDBustaPaga As Integer
            Public IDRichiestaCS As Integer
            Public StatoCS As StatoOfferteFL
            Public IDStudioF As Integer
            Public StatoStudioF As StatoOfferteFL
            Public StatoPratica As StatoOfferteFL

            Public Sub New(ByVal c As FinestraLavorazione)
                DMD.DMDObject.IncreaseCounter(Me)
                Me.ID = GetID(c)
                Me.PO = c.IDPuntoOperativo
                Me.StatoFinestra = c.StatoFinestra
                Me.StatoContatto = c.StatoContatto
                Me.IDBustaPaga = c.IDBustaPaga
                Me.IDRichiestaCS = c.IDRichiestaCertificato
                Me.StatoCS = c.StatoRichiestaCertificato
                Me.IDStudioF = c.IDStudioDiFattibilita
                Me.StatoStudioF = c.StatoStudioFattibilita
                Me.StatoPratica = c.GetMaxStatoPratica
            End Sub

            Public Sub New(ByVal id As Integer)
                DMD.DMDObject.IncreaseCounter(Me)
                Me.ID = id
                Me.PO = 0
            End Sub

            Public Function CompareTo(ByVal obj As ItemInfo) As Integer
                Return Arrays.Compare(Me.ID, obj.ID)
            End Function

            Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
                Return Me.CompareTo(obj)
            End Function

            Protected Overrides Sub Finalize()
                MyBase.Finalize()
                DMD.DMDObject.DecreaseCounter(Me)
            End Sub
        End Class

        Private listLock As New Object
        Private m_List As ItemInfo()
        Private statiValidi As String() = {"Attesa Busta Paga", "Busta Paga Ricevuta", "Proporre Delega", "Rinnovabile"}


        Public Sub New()
            AddHandler CQSPD.FinestreDiLavorazione.ItemCreated, AddressOf handleItem
            AddHandler CQSPD.FinestreDiLavorazione.ItemDeleted, AddressOf handleItem
            AddHandler CQSPD.FinestreDiLavorazione.ItemModified, AddressOf handleItem
            Me.m_List = Nothing

        End Sub

        Protected Overrides Sub Finalize()
            RemoveHandler CQSPD.FinestreDiLavorazione.ItemCreated, AddressOf handleItem
            RemoveHandler CQSPD.FinestreDiLavorazione.ItemDeleted, AddressOf handleItem
            RemoveHandler CQSPD.FinestreDiLavorazione.ItemModified, AddressOf handleItem
            MyBase.Finalize()
        End Sub

        Private Sub handleItem(ByVal sender As Object, ByVal e As ItemEventArgs)
            Dim c As FinestraLavorazione = e.Item
            SyncLock Me.listLock
                If Me.Check(c) Then
                    Me.AddToList(c)
                Else
                    Me.RemoveFromList(GetID(c))
                End If
            End SyncLock
        End Sub


        Private Function IsInList(ByVal id As Integer) As Boolean
            Return Arrays.Len(Me.m_List) > 0 AndAlso Arrays.BinarySearch(Me.m_List, New ItemInfo(id)) >= 0
        End Function

        Private Sub AddToList(ByVal c As FinestraLavorazione)
            Dim i As Integer = -1
            Dim info As New ItemInfo(c)
            If (Arrays.Len(Me.m_List) > 0) Then i = Arrays.BinarySearch(Me.m_List, info)
            If (i >= 0) Then
                Me.m_List(i) = info
            Else
                Me.m_List = Arrays.Push(Me.m_List, info)
                Array.Sort(Me.m_List)
            End If
        End Sub

        Private Sub RemoveFromList(ByVal id As Integer)
            Dim i As Integer = -1
            If (Arrays.Len(Me.m_List) > 0) Then i = Arrays.BinarySearch(Me.m_List, New ItemInfo(id))
            If (i >= 0) Then Me.m_List = Arrays.RemoveAt(Me.m_List, i)
        End Sub

        Private Function Check(ByVal p As FinestraLavorazione) As Boolean
            Return (p.Stato = ObjectStatus.OBJECT_VALID) AndAlso (p.StatoFinestra = StatoFinestraLavorazione.Aperta) AndAlso (p.IDBustaPaga <> 0) AndAlso (p.IDStudioDiFattibilita = 0)
        End Function

        Private ReadOnly Property List As CCollection(Of ItemInfo)
            Get
                SyncLock Me.listLock
                    If (Me.m_List Is Nothing) Then
                        Dim tmp As New System.Collections.ArrayList
                        tmp.AddRange(Me.GetRinnovabili)
                        tmp.AddRange(Me.GetInLavorazione)
                        Me.m_List = tmp.ToArray(GetType(ItemInfo))
                        'If (Me.m_List IsNot Nothing) Then Array.Sort(Me.m_List)
                    Else
                        Me.m_List = {}
                    End If

                    Return New CCollection(Of ItemInfo)(Me.m_List)
                End SyncLock
            End Get
        End Property

        Private Function GetInLavorazione() As CCollection(Of ItemInfo)
            Dim ret As New CCollection(Of ItemInfo)
            Dim cursor As New FinestraLavorazioneCursor
            cursor = New FinestraLavorazioneCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.StatoFinestra.Value = StatoFinestraLavorazione.Aperta
            cursor.IgnoreRights = True
            While Not cursor.EOF
                ret.Add(New ItemInfo(cursor.Item))
                cursor.MoveNext()
            End While
            cursor.Dispose()

            Return ret
        End Function

        Private Function GetRinnovabili() As CCollection(Of ItemInfo)
            Dim ret As New CCollection(Of ItemInfo)

            Dim di As Date = DateUtils.DateAdd(DateInterval.Day, -CQSPD.Configuration.GiorniAnticipoRifin, DateUtils.ToDay)
            Dim df As Date = DateUtils.DateAdd(DateInterval.Day, CQSPD.Configuration.GiorniAnticipoRifin, DateUtils.ToDay)
            'Inseriamo tutte le finestre di lavorazione precedenti
            Dim cursor As New FinestraLavorazioneCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.StatoFinestra.Value = StatoFinestraLavorazione.NonAperta
            cursor.Flags.Value = FinestraLavorazioneFlags.Rinnovo
            cursor.Flags.Operator = OP.OP_ALLBITAND
            cursor.DataInizioLavorabilita.Between(di, df) ' = DateUtils.DateAdd(DateInterval.Day, 30, DateUtils.ToDay)  'Between(di.Value, df.Value)
            cursor.IgnoreRights = True
            'cursor.DataInizioLavorabilita.Operator = OP.OP_LE

            While Not cursor.EOF
                ret.Add(New ItemInfo(cursor.Item))
                cursor.MoveNext()
            End While
            cursor.Dispose()

            Return ret
        End Function

           
        Public Overrides Function GetActivePersons(nomeLista As String, fromDate As Date?, toDate As Date?, Optional ufficio As Integer = 0, Optional operatore As Integer = 0) As CCollection(Of CActivePerson)
            Return New CCollection(Of CActivePerson)
        End Function

        Public Overrides Function GetPendingActivities() As CCollection(Of ICalendarActivity)
            Return New CCollection(Of ICalendarActivity)
        End Function

        Public Overrides Function GetScadenze(fromDate As Date?, toDate As Date?) As CCollection(Of ICalendarActivity)
            Return New CCollection(Of ICalendarActivity)
        End Function

        Public Overrides Function GetToDoList(user As CUser) As CCollection(Of ICalendarActivity)
            Dim ret As New CCollection(Of ICalendarActivity)
            Dim act As CCalendarActivity

            If (CRM.CRMGroup.Members.Contains(Sistema.Users.CurrentUser)) Then
                Dim cnt As Integer = Me.ContaRinnovabili

                If (cnt > 0) Then
                    act = New CCalendarActivity
                    DirectCast(act, ICalendarActivity).SetProvider(Me)
                    act.Descrizione = "Ci sono " & cnt & " clienti che possono rinnovare"
                    act.DataInizio = DateUtils.Now
                    act.GiornataIntera = True
                    act.Categoria = "Urgente"
                    act.Priorita = ret.Count
                    act.Flags = CalendarActivityFlags.IsAction
                    act.IconURL = "/widgets/images/activities/clientirinnovaili.png"
                    act.Note = "CQSPDCalendarProviderFLRinnovabili"
                    act.Stato = ObjectStatus.OBJECT_VALID
                    ret.Add(act)
                End If

                cnt = Me.ContaClientiInteressati
                If (cnt > 0) Then
                    act = New CCalendarActivity
                    DirectCast(act, ICalendarActivity).SetProvider(Me)
                    act.Descrizione = "Ci sono " & cnt & " clienti interessati che devono inviare la busta paga"
                    act.DataInizio = DateUtils.Now
                    act.GiornataIntera = True
                    act.Categoria = "Urgente"
                    act.Priorita = ret.Count
                    act.Flags = CalendarActivityFlags.IsAction
                    act.IconURL = "/widgets/images/activities/bustapagaricevere.png"
                    act.Note = "CQSPDCalendarProviderFLBustePagaRic"
                    act.Stato = ObjectStatus.OBJECT_VALID
                    ret.Add(act)
                End If


                cnt = Me.ContaCSDaRichiedere
                If (cnt > 0) Then
                    act = New CCalendarActivity
                    DirectCast(act, ICalendarActivity).SetProvider(Me)
                    act.Descrizione = "Ci sono " & cnt & " Certificati di Stipendio/CUD da richiedere "
                    act.DataInizio = DateUtils.Now
                    act.GiornataIntera = True
                    act.Categoria = "Urgente"
                    act.Priorita = ret.Count
                    act.Flags = CalendarActivityFlags.IsAction
                    act.IconURL = "/widgets/images/activities/bustapagavalutare.png"
                    act.Note = "CQSPDCalendarProviderFLCSRichiedere"
                    act.Stato = ObjectStatus.OBJECT_VALID
                    ret.Add(act)
                End If

                cnt = Me.ContaStudiDaEffettuare
                If (cnt > 0) Then
                    act = New CCalendarActivity
                    DirectCast(act, ICalendarActivity).SetProvider(Me)
                    act.Descrizione = "Ci sono " & cnt & " studi di fattibilità da effettuare"
                    act.DataInizio = DateUtils.Now
                    act.GiornataIntera = True
                    act.Categoria = "Urgente"
                    act.Priorita = ret.Count
                    act.Flags = CalendarActivityFlags.IsAction
                    act.IconURL = "/widgets/images/activities/bustapagavalutare.png"
                    act.Note = "CQSPDCalendarProviderFLSFFare"
                    act.Stato = ObjectStatus.OBJECT_VALID
                    ret.Add(act)
                End If

                cnt = Me.ContaSFInCorso
                If (cnt > 0) Then
                    act = New CCalendarActivity
                    DirectCast(act, ICalendarActivity).SetProvider(Me)
                    act.Descrizione = "Ci sono " & cnt & " studi di fattibilità in sospeso"
                    act.DataInizio = DateUtils.Now
                    act.GiornataIntera = True
                    act.Categoria = "Urgente"
                    act.Priorita = ret.Count
                    act.Flags = CalendarActivityFlags.IsAction
                    act.IconURL = "/widgets/images/activities/sfinsospeso.png"
                    act.Note = "CQSPDCalendarProviderFLSFSospeso"
                    act.Stato = ObjectStatus.OBJECT_VALID
                    ret.Add(act)
                End If

                cnt = Me.ContaPraticheDaCaricare
                If (cnt > 0) Then
                    act = New CCalendarActivity
                    DirectCast(act, ICalendarActivity).SetProvider(Me)
                    act.Descrizione = "Ci sono " & cnt & " pratiche da caricare"
                    act.DataInizio = DateUtils.Now
                    act.GiornataIntera = True
                    act.Categoria = "Urgente"
                    act.Priorita = ret.Count
                    act.Flags = CalendarActivityFlags.IsAction
                    act.IconURL = "/widgets/images/activities/pratichecaricare.png"
                    act.Note = "CQSPDCalendarProviderFLPraticheCAR"
                    act.Stato = ObjectStatus.OBJECT_VALID
                    ret.Add(act)
                End If
            End If

            Return ret

        End Function

        Private Function ContaSFInCorso() As Integer
            Dim cnt As Integer = 0
            Dim u As CUser = Sistema.Users.CurrentUser
            Dim items As CCollection(Of ItemInfo) = Me.List
            For Each item As ItemInfo In items
                If (item.StatoFinestra = StatoFinestraLavorazione.Aperta AndAlso _
                    item.StatoStudioF <> 0 AndAlso (item.StatoStudioF = StatoOfferteFL.InLavorazione OrElse item.StatoStudioF = StatoOfferteFL.Sconosciuto) AndAlso _
                    item.StatoPratica = StatoOfferteFL.Sconosciuto AndAlso _
                    u.Uffici.HasOffice(item.PO)) Then
                    cnt += 1
                End If
            Next
            Return cnt
        End Function

        Private Function ContaRinnovabili() As Integer
            Dim cnt As Integer = 0
            Dim u As CUser = Sistema.Users.CurrentUser
            Dim items As CCollection(Of ItemInfo) = Me.List
            For Each item As ItemInfo In items
                If (item.StatoFinestra = StatoFinestraLavorazione.NonAperta AndAlso u.Uffici.HasOffice(item.PO)) Then
                    cnt += 1
                End If
            Next
            Return cnt
        End Function

        Private Function ContaPraticheDaCaricare() As Integer
            Dim cnt As Integer = 0
            Dim u As CUser = Sistema.Users.CurrentUser
            Dim items As CCollection(Of ItemInfo) = Me.List
            For Each item As ItemInfo In items
                If (item.IDStudioF <> 0 AndAlso item.StatoStudioF = StatoOfferteFL.Liquidata) AndAlso (item.StatoPratica = StatoOfferteFL.Sconosciuto) AndAlso (item.StatoFinestra = StatoFinestraLavorazione.Aperta) AndAlso u.Uffici.HasOffice(item.PO) Then
                    cnt += 1
                End If
            Next
            Return cnt
        End Function

        Private Function ContaBustePagaDaValutare() As Integer
            Dim cnt As Integer = 0
            Dim u As CUser = Sistema.Users.CurrentUser
            Dim items As CCollection(Of ItemInfo) = Me.List
            For Each item As ItemInfo In items
                If (item.IDBustaPaga <> 0) AndAlso (item.IDStudioF = 0) AndAlso (item.StatoFinestra = StatoFinestraLavorazione.Aperta) AndAlso u.Uffici.HasOffice(item.PO) Then
                    cnt += 1
                End If
            Next
            Return cnt
        End Function

        Private Function ContaClientiInteressati() As Integer
            Dim cnt As Integer = 0
            Dim u As CUser = Sistema.Users.CurrentUser
            Dim items As CCollection(Of ItemInfo) = Me.List
            For Each item As ItemInfo In items
                If (item.IDBustaPaga = 0) AndAlso (item.StatoContatto = StatoOfferteFL.Liquidata) AndAlso (item.StatoFinestra = StatoFinestraLavorazione.Aperta) AndAlso u.Uffici.HasOffice(item.PO) Then
                    cnt += 1
                End If
            Next
            Return cnt
        End Function


        Private Function ContaCSDaRichiedere() As Integer
            Dim cnt As Integer = 0
            Dim u As CUser = Sistema.Users.CurrentUser
            Dim items As CCollection(Of ItemInfo) = Me.List
            For Each item As ItemInfo In items
                If (item.IDBustaPaga <> 0) AndAlso (item.IDRichiestaCS = 0) AndAlso (item.IDStudioF = 0) AndAlso (item.StatoFinestra = StatoFinestraLavorazione.Aperta) AndAlso u.Uffici.HasOffice(item.PO) Then
                    cnt += 1
                End If
            Next
            Return cnt
        End Function

        Private Function ContaStudiDaEffettuare() As Integer
            Dim cnt As Integer = 0
            Dim u As CUser = Sistema.Users.CurrentUser
            Dim items As CCollection(Of ItemInfo) = Me.List
            For Each item As ItemInfo In items
                If (item.IDRichiestaCS <> 0) AndAlso (item.IDStudioF = 0) AndAlso (item.StatoFinestra = StatoFinestraLavorazione.Aperta) AndAlso u.Uffici.HasOffice(item.PO) Then
                    cnt += 1
                End If
            Next
            Return cnt
        End Function

        Private Function ContaBustePagaDaRicevere() As Integer
            Dim cnt As Integer = 0
            Dim u As CUser = Sistema.Users.CurrentUser
            Dim items As CCollection(Of ItemInfo) = Me.List
            For Each item As ItemInfo In items
                If (item.IDBustaPaga = 0) AndAlso (item.StatoFinestra = StatoFinestraLavorazione.Aperta) Then
                    cnt += 1
                End If
            Next
            Return cnt
        End Function



        Public Overrides Sub SaveActivity(item As ICalendarActivity, Optional force As Boolean = False)

        End Sub

        Public Overrides Sub DeleteActivity(item As ICalendarActivity, Optional force As Boolean = False)

        End Sub



        Public Overrides ReadOnly Property UniqueName As String
            Get
                Return "CQSPDCALPROVFL"
            End Get
        End Property
    End Class


End Class