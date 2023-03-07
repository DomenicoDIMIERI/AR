Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Office
Imports DMD.Anagrafica

Partial Public Class Office


    Public Class EstrattiContributiviCalProvider
        Inherits BaseCalendarActivitiesProvider

        Private Class ItemInfo
            Implements IComparable

            Public ID As Integer
            Public PO As Integer
            Public OP As Integer

            Public Sub New(ByVal c As EstrattoContributivo)
                DMD.DMDObject.IncreaseCounter(Me)
                Me.ID = GetID(c)
                Me.PO = c.IDPuntoOperativo
                Me.OP = c.IDAssegnatoA
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
        Private statiValidi() As StatoEstrattoContributivo = {StatoEstrattoContributivo.Richiesto, StatoEstrattoContributivo.Assegnato, StatoEstrattoContributivo.Sospeso}

        Public Sub New()
            AddHandler Office.EstrattiContributivi.ItemCreated, AddressOf handleItem
            AddHandler Office.EstrattiContributivi.ItemDeleted, AddressOf handleItem
            AddHandler Office.EstrattiContributivi.ItemModified, AddressOf handleItem
            Me.m_Items = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            RemoveHandler Office.EstrattiContributivi.ItemCreated, AddressOf handleItem
            RemoveHandler Office.EstrattiContributivi.ItemDeleted, AddressOf handleItem
            RemoveHandler Office.EstrattiContributivi.ItemModified, AddressOf handleItem
            MyBase.Finalize()
        End Sub


        Private Function IsInList(ByVal id As Integer) As Boolean
            Return Arrays.Len(Me.m_Items) > 0 AndAlso Arrays.BinarySearch(Me.m_Items, New ItemInfo(id)) >= 0
        End Function

        Private Sub AddToList(ByVal c As EstrattoContributivo)
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
            If (Arrays.Len(Me.InnerItems) > 0) Then i = Arrays.BinarySearch(Me.InnerItems, New ItemInfo(id))
            If (i >= 0) Then Me.m_Items = Arrays.RemoveAt(Me.m_Items, i)
        End Sub

        Private Function InnerItems() As ItemInfo()
            If (Me.m_Items Is Nothing) Then
                Dim cursor1 As New EstrattiContributiviCursor
                Dim tmp As New System.Collections.ArrayList
                cursor1.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor1.IgnoreRights = True
                cursor1.StatoRichiesta.ValueIn(statiValidi)
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

        Private Function IsVisible(ByVal c As EstrattoContributivo) As Boolean
            Return (c.Stato = ObjectStatus.OBJECT_VALID AndAlso Arrays.IndexOf(statiValidi, c.StatoRichiesta) >= 0)
        End Function

        Private Sub handleItem(ByVal sender As Object, ByVal e As ItemEventArgs)
            Dim c As EstrattoContributivo = e.Item
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

        Private Function Conta() As Integer
            'Dim u As CUser = Sistema.Users.CurrentUser
            'Dim cnt As Integer = 0
            'For Each f As ItemInfo In Me.Items
            '    If f.PO = 0 OrElse u.Uffici.HasOffice(f.PO) Then
            '        cnt += 1
            '    End If
            'Next
            'Return cnt
            Return Me.InnerItems.Count
        End Function

         
        Public Overrides Function GetToDoList(user As CUser) As CCollection(Of ICalendarActivity)
            SyncLock Me.listLock
                Dim ret As New CCollection(Of ICalendarActivity)
                Dim act As CCalendarActivity
                Dim cnt As Integer = Me.Conta
                If cnt > 0 AndAlso EstrattiContributivi.GruppoResponsabili.Members.Contains(Sistema.Users.CurrentUser) Then
                    act = New CCalendarActivity
                    DirectCast(act, ICalendarActivity).SetProvider(Me)
                    act.Descrizione = "Ci sono " & cnt & " richiesta di estratti contributivi da evadere"
                    act.DataInizio = Calendar.Now
                    act.GiornataIntera = True
                    act.Categoria = "Normale"
                    act.Flags = CalendarActivityFlags.IsAction
                    act.IconURL = "/widgets/images/activities/estratticontributivi.gif"
                    act.Note = "EstrattiContributiviCalToDoList"
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
                Return "ESTRCONTRCALPROV"
            End Get
        End Property
    End Class

End Class