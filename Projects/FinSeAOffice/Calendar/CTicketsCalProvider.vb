Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Office
Imports DMD.Anagrafica

Partial Public Class Office


    Public Class CTicketsCalProvider
        Inherits BaseCalendarActivitiesProvider

        Private Class ItemInfo
            Implements IComparable

            Public ID As Integer
            Public PO As Integer
            Public Categoria As String
            Public Sottocategoria As String

            Public Sub New(ByVal c As CTicket)
                DMD.DMDObject.IncreaseCounter(Me)
                Me.ID = GetID(c)
                Me.PO = c.IDPuntoOperativo
                Me.Categoria = c.Categoria
                Me.Sottocategoria = c.Sottocategoria
            End Sub

            Public Sub New(ByVal id As Integer)
                DMD.DMDObject.IncreaseCounter(Me)
                Me.ID = id
                Me.PO = 0
            End Sub



            Private Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
                Dim tmp As ItemInfo = obj
                Return Arrays.Compare(Me.ID, tmp.ID)
            End Function

            Protected Overrides Sub Finalize()
                MyBase.Finalize()
                DMD.DMDObject.DecreaseCounter(Me)
            End Sub
        End Class

        Private listLock As New Object
        Private m_Items As ItemInfo()
        Private ReadOnly statiValidi As TicketStatus() = {TicketStatus.APERTO, TicketStatus.INLAVORAZIONE, TicketStatus.RIAPERTO, TicketStatus.SOSPESO}

        Public Sub New()
            AddHandler Office.Tickets.ItemCreated, AddressOf handleItem
            AddHandler Office.Tickets.ItemDeleted, AddressOf handleItem
            AddHandler Office.Tickets.ItemModified, AddressOf handleItem
            Me.m_Items = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            RemoveHandler Office.Tickets.ItemCreated, AddressOf handleItem
            RemoveHandler Office.Tickets.ItemDeleted, AddressOf handleItem
            RemoveHandler Office.Tickets.ItemModified, AddressOf handleItem
            MyBase.Finalize()
        End Sub

        Private Function IsInFaxList(ByVal id As Integer) As Boolean
            Return Arrays.Len(Me.m_Items) > 0 AndAlso Arrays.BinarySearch(Me.m_Items, New ItemInfo(id)) >= 0
        End Function

        Private Sub AddToList(ByVal c As CTicket)
            Dim i As Integer = -1
            Dim info As New ItemInfo(c)
            If (Arrays.Len(Me.InnerItems) > 0) Then i = Arrays.BinarySearch(Me.m_Items, info)
            If (i > 0) Then
                Me.m_Items(i) = info
            Else
                Me.m_Items = Arrays.Push(Me.m_Items, info)
                Array.Sort(Me.m_Items)
            End If
        End Sub

        Private Sub RemoveFromList(ByVal id As Integer)
            Dim i As Integer = -1
            If (Arrays.Len(Me.InnerItems) > 0) Then i = Arrays.BinarySearch(Me.m_Items, New ItemInfo(id))
            If (i >= 0) Then Me.m_Items = Arrays.RemoveAt(Me.m_Items, i)
        End Sub

        Private Function InnerItems() As ItemInfo()
            If (Me.m_Items Is Nothing) Then
                Dim cursor1 As New CTicketCursor
                Try
                    Dim tmp As New System.Collections.ArrayList
                    cursor1.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor1.IgnoreRights = True
                    cursor1.StatoSegnalazione.ValueIn(statiValidi)
                    While Not cursor1.EOF
                        tmp.Add(New ItemInfo(cursor1.Item))
                        cursor1.MoveNext()
                    End While
                    cursor1.Dispose()
                    If (tmp.Count > 0) Then
                        Me.m_Items = tmp.ToArray(GetType(ItemInfo))
                    Else
                        Me.m_Items = {}
                    End If
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally
                    If (cursor1 IsNot Nothing) Then cursor1.Dispose() : cursor1 = Nothing
                End Try
            End If
            Return Me.m_Items
        End Function

        Private ReadOnly Property Items As CCollection(Of ItemInfo)
            Get
                SyncLock Me.listLock
                    Return New CCollection(Of ItemInfo)(Me.InnerItems)
                End SyncLock
            End Get
        End Property

        Private Function IsVisible(ByVal c As CTicket) As Boolean
            Return (c.Stato = ObjectStatus.OBJECT_VALID AndAlso Arrays.IndexOf(statiValidi, c.StatoSegnalazione) >= 0)
        End Function

        Private Sub handleItem(ByVal sender As Object, ByVal e As ItemEventArgs)
            Dim c As CTicket = e.Item
            SyncLock Me.listLock
                If Me.IsVisible(c) Then
                    Me.AddToList(c)
                Else
                    Me.RemoveFromList(GetID(c))
                End If
            End SyncLock
        End Sub


        Public Overrides Sub DeleteActivity(item As ICalendarActivity, Optional force As Boolean = False)

        End Sub

        Public Overrides Function GetActivePersons(nomeLista As String, fromDate As Date?, toDate As Date?, Optional ufficio As Integer = 0, Optional operatore As Integer = 0) As CCollection(Of CActivePerson)
            Return New CCollection(Of CActivePerson)
        End Function

        Public Overrides Function GetPendingActivities() As CCollection(Of ICalendarActivity)
            Return New CCollection(Of ICalendarActivity)
        End Function

        Public Overrides Function GetScadenze(fromDate As Date?, toDate As Date?) As CCollection(Of ICalendarActivity)
            Return New CCollection(Of ICalendarActivity)
        End Function

        Private Function GetAllowedList() As CCollection(Of ItemInfo)
            Dim u As CUser = Sistema.Users.CurrentUser
            Dim ret As New CCollection(Of ItemInfo)
            For Each f As ItemInfo In Me.Items
                Dim c As CTicketCategory = Office.TicketCategories.GetItemByName(f.Categoria, f.Sottocategoria)
                If (c IsNot Nothing) Then
                    If Tickets.Module.UserCanDoAction("list") Then
                        ret.Add(f)
                    Else
                        If (c.NotifyUsers.Contains(u)) Then
                            ret.Add(f)
                        End If
                    End If
                End If
            Next
            Return ret
        End Function

        Public Overrides Function GetToDoList(user As CUser) As CCollection(Of ICalendarActivity)
            SyncLock Me.listLock
                Dim ret As New CCollection(Of ICalendarActivity)
                Dim act As CCalendarActivity
                Dim items As CCollection(Of ItemInfo) = Me.GetAllowedList
                If items.Count > 0 Then
                    act = New CCalendarActivity
                    DirectCast(act, ICalendarActivity).SetProvider(Me)
                    act.Descrizione = Strings.JoinW("Ci sono ", items.Count, " richieste di assistenza")
                    act.DataInizio = Calendar.Now
                    act.GiornataIntera = True
                    act.Categoria = "Normale"
                    act.Flags = CalendarActivityFlags.IsAction
                    act.IconURL = "/widgets/images/activities/tip.gif"
                    act.Note = "CTicketsShowToDo"
                    act.Stato = ObjectStatus.OBJECT_VALID
                    ret.Add(act)
                End If
                Return ret
            End SyncLock
        End Function

        Public Overrides Sub SaveActivity(item As ICalendarActivity, Optional force As Boolean = False)

        End Sub



        Public Overrides ReadOnly Property UniqueName As String
            Get
                Return "OTICKTCALPROVIDER"
            End Get
        End Property
    End Class

End Class