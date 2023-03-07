Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls
Imports DMD.Anagrafica
Imports DMD.CQSPD


Partial Public Class CQSPD

    Public Class CQSPDCalendarProviderStPrat
        Inherits BaseCalendarActivitiesProvider

        Private Class ItemInfo
            Implements IComparable

            Public ID As Integer
            Public PO As Integer
            Public Anno As Integer
            Public Mese As Integer
            Public StatoContatto As StatoOfferteFL

            Public Sub New(ByVal c As CRapportino)
                DMD.DMDObject.IncreaseCounter(Me)
                Me.ID = GetID(c)
                Me.PO = c.IDPuntoOperativo
                Dim param As String = Strings.Trim(c.StatoDiLavorazioneAttuale.Params)
                Try
                    Dim n() As String = Strings.Split(param, "|")
                    Me.Anno = CInt(n(0))
                    Me.Mese = CInt(n(1))
                Catch ex As Exception
                    Me.Anno = Calendar.ToDay.Year
                    Me.Mese = Calendar.ToDay.Month
                End Try
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


        Public Sub New()
            AddHandler CQSPD.Pratiche.ItemCreated, AddressOf handleItem
            AddHandler CQSPD.Pratiche.ItemDeleted, AddressOf handleItem
            AddHandler CQSPD.Pratiche.ItemModified, AddressOf handleItem
            Me.m_List = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            RemoveHandler CQSPD.Pratiche.ItemCreated, AddressOf handleItem
            RemoveHandler CQSPD.Pratiche.ItemDeleted, AddressOf handleItem
            RemoveHandler CQSPD.Pratiche.ItemModified, AddressOf handleItem
            MyBase.Finalize()
        End Sub

        Private Sub handleItem(ByVal sender As Object, ByVal e As ItemEventArgs)
            Dim c As CRapportino = e.Item
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

        Private Sub AddToList(ByVal c As CRapportino)
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

        Private Function Check(ByVal p As CRapportino) As Boolean
            Return (p.Stato = ObjectStatus.OBJECT_VALID) AndAlso (p.IDStatoAttuale = GetID(CQSPD.StatiPratica.StatoPreCaricamento))
        End Function

        Private ReadOnly Property List As CCollection(Of ItemInfo)
            Get
                SyncLock Me.listLock
                    If (Me.m_List Is Nothing) Then
                        Dim tmp As New System.Collections.ArrayList
                        tmp.AddRange(Me.GetPraticheInPrecaricamento)
                        Me.m_List = tmp.ToArray(GetType(ItemInfo))
                        'If (Me.m_List IsNot Nothing) Then Array.Sort(Me.m_List)
                    End If
                    Return New CCollection(Of ItemInfo)(Me.m_List)
                End SyncLock
            End Get
        End Property


        Private Function GetPraticheInPrecaricamento() As CCollection(Of ItemInfo)
            Dim cursor As CRapportiniCursor = Nothing

            Try
                Dim ret As New CCollection(Of ItemInfo)

                Dim di As Date = Calendar.DateAdd(DateInterval.Day, -CQSPD.Configuration.GiorniAnticipoRifin, Calendar.ToDay)
                Dim df As Date = Calendar.DateAdd(DateInterval.Day, CQSPD.Configuration.GiorniAnticipoRifin, Calendar.ToDay)
                'Inseriamo tutte le finestre di lavorazione precedenti
                cursor = New CRapportiniCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDStatoAttuale.Value = GetID(CQSPD.StatiPratica.StatoPreCaricamento)
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    ret.Add(New ItemInfo(cursor.Item))
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

            If Not (CQSPD.GruppoConsulenti.Members.Contains(Sistema.Users.CurrentUser) OrElse CQSPD.GruppoAutorizzatori.Members.Contains(Sistema.Users.CurrentUser) OrElse CQSPD.GruppoSupervisori.Members.Contains(Sistema.Users.CurrentUser) OrElse CQSPD.GruppoReferenti.Members.Contains(Sistema.Users.CurrentUser)) Then Return ret

            Dim y As Integer = Calendar.ToDay.Year
            Dim m As Integer = Calendar.ToDay.Month
            Dim cnt As Integer = Me.ContaPrecedenti(m, y)
            If (cnt > 0) Then
                act = New CCalendarActivity
                DirectCast(act, ICalendarActivity).SetProvider(Me)
                act.Descrizione = "<span class=""red"">Ci sono " & cnt & " pratic" & CStr(IIf(cnt > 1, "he", "a")) & " da caricare!</span>"
                act.DataInizio = Calendar.Now
                act.GiornataIntera = True
                act.Categoria = "Urgente"
                act.Priorita = ret.Count
                act.Flags = CalendarActivityFlags.IsAction
                act.IconURL = "/widgets/images/activities/clientirinnovaili.png"
                act.Note = "CQSPDCalendarProviderStPratR" & vbCr & y & "|" & m
                act.Stato = ObjectStatus.OBJECT_VALID
                ret.Add(act)
            End If

            For i As Integer = 0 To 11
                cnt = Me.ContaPerMese(m, y)
                If (cnt > 0) Then
                    act = New CCalendarActivity
                    DirectCast(act, ICalendarActivity).SetProvider(Me)
                    act.Descrizione = "Ci sono " & cnt & " pratic" & CStr(IIf(cnt > 1, "he deliberabili", "a deliberabile")) & " il " & Formats.Format(Calendar.MakeDate(y, m, 1), "MMMM yyyy")
                    act.DataInizio = Calendar.Now
                    act.GiornataIntera = True
                    act.Categoria = "Urgente"
                    act.Priorita = ret.Count
                    act.Flags = CalendarActivityFlags.IsAction
                    act.IconURL = "/widgets/images/activities/clientirinnovaili.png"
                    act.Note = "CQSPDCalendarProviderStPratR" & vbCr & y & "|" & m
                    act.Stato = ObjectStatus.OBJECT_VALID
                    ret.Add(act)
                End If
                m += 1
                If (m > 12) Then
                    m = 1
                    y += 1
                End If
            Next

            Return ret

        End Function

        Private Function ContaPerMese(ByVal mese As Integer, ByVal anno As Integer) As Integer
            Dim cnt As Integer = 0
            Dim u As CUser = Sistema.Users.CurrentUser
            Dim items As CCollection(Of ItemInfo) = Me.List
            For Each item As ItemInfo In items
                If (item.Mese = mese AndAlso item.Anno = anno) AndAlso (u.Uffici.HasOffice(item.PO)) Then
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

        Private Function ContaPrecedenti(m As Integer, y As Integer) As Integer
            Dim cnt As Integer = 0
            Dim u As CUser = Sistema.Users.CurrentUser
            Dim items As CCollection(Of ItemInfo) = Me.List
            For Each item As ItemInfo In items
                If (item.Anno < y OrElse (item.Anno = y AndAlso item.Mese < m)) AndAlso (u.Uffici.HasOffice(item.PO)) Then
                    cnt += 1
                End If
            Next
            Return cnt
        End Function

    End Class


End Class