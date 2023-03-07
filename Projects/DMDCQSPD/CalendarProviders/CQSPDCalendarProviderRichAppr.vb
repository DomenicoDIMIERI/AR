Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls
Imports DMD.Anagrafica
Imports DMD.CQSPD


Partial Public Class CQSPD

    Public Class CQSPDCalendarProviderRichAppr
        Inherits BaseCalendarActivitiesProvider

        Private Class ItemInfo
            Implements IComparable

            Public ID As Integer
            Public PO As Integer
            Public IDRichAppr As Integer
            Public Privilaged As Boolean

            Public Sub New(ByVal c As CRichiestaApprovazione)
                DMD.DMDObject.IncreaseCounter(Me)
                Me.ID = GetID(c)
                Me.PO = c.IDPuntoOperativo
                Me.IDRichAppr = GetID(c)
                If (c.MotivoRichiesta Is Nothing) Then
                    Me.Privilaged = True
                Else
                    Me.Privilaged = c.MotivoRichiesta.Privilegiato
                End If
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
            AddHandler CQSPD.RichiesteApprovazione.ItemCreated, AddressOf handleItem
            AddHandler CQSPD.RichiesteApprovazione.ItemDeleted, AddressOf handleItem
            AddHandler CQSPD.RichiesteApprovazione.ItemModified, AddressOf handleItem
            Me.m_List = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            RemoveHandler CQSPD.RichiesteApprovazione.ItemCreated, AddressOf handleItem
            RemoveHandler CQSPD.RichiesteApprovazione.ItemDeleted, AddressOf handleItem
            RemoveHandler CQSPD.RichiesteApprovazione.ItemModified, AddressOf handleItem

            MyBase.Finalize()
        End Sub

        Private Sub handleItem(ByVal sender As Object, ByVal e As ItemEventArgs)
            Dim c As CRichiestaApprovazione = e.Item
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

        Private Sub AddToList(ByVal c As CRichiestaApprovazione)
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

        Private Function Check(ByVal p As CRichiestaApprovazione) As Boolean
            If (p.Stato <> ObjectStatus.OBJECT_VALID) Then Return False
            Return p.StatoRichiesta = StatoRichiestaApprovazione.PRESAINCARICO Or p.StatoRichiesta = StatoRichiestaApprovazione.ATTESA
        End Function

        Private ReadOnly Property List As CCollection(Of ItemInfo)
            Get
                SyncLock Me.listLock
                    If (Me.m_List Is Nothing) Then
                        Dim cursor As New CRichiestaApprovazioneCursor
                        Dim tmp As New System.Collections.ArrayList
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                        cursor.StatoRichiesta.ValueIn({StatoRichiestaApprovazione.ATTESA, StatoRichiestaApprovazione.PRESAINCARICO})
                        cursor.IgnoreRights = True
                        While Not cursor.EOF
                            tmp.Add(New ItemInfo(cursor.Item))
                            cursor.MoveNext()
                        End While
                        cursor.Dispose()

                        Me.m_List = tmp.ToArray(GetType(ItemInfo))
                        'If (Me.m_List IsNot Nothing) Then Array.Sort(Me.m_List)
                    End If

                    Return New CCollection(Of ItemInfo)(Me.m_List)
                End SyncLock
            End Get
        End Property

        


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
            Dim cnt As Integer

            If (CQSPD.GruppoAutorizzatori.Members.Contains(Sistema.Users.CurrentUser)) Then
                cnt = Me.ContaTutto
            ElseIf (CQSPD.GruppoSupervisori.Members.Contains(Sistema.Users.CurrentUser)) Then
                cnt = Me.ContaNonProvilegiati
            End If

            If (cnt > 0) Then
                act = New CCalendarActivity
                DirectCast(act, ICalendarActivity).SetProvider(Me)
                act.Descrizione = "Ci sono " & cnt & " richieste di autorizzazione"
                act.DataInizio = DateUtils.Now
                act.GiornataIntera = True
                act.Categoria = "Urgente"
                act.Flags = CalendarActivityFlags.IsAction
                act.IconURL = "/widgets/images/activities/richiesteautorizzazione.png"
                act.Note = "CQSPDCalendarProviderRICHAUT"
                act.Stato = ObjectStatus.OBJECT_VALID
                ret.Add(act)
            End If

            Return ret
        End Function

        Private Function ContaTutto() As Integer
            Return Me.List.Count
        End Function

        Private Function ContaNonProvilegiati() As Integer
            Dim cnt As Integer = 0
            For Each item As ItemInfo In Me.List
                If item.Privilaged = True Then cnt += 1
            Next
            Return cnt
        End Function
        

        Public Overrides Sub SaveActivity(item As ICalendarActivity, Optional force As Boolean = False)

        End Sub

        Public Overrides Sub DeleteActivity(item As ICalendarActivity, Optional force As Boolean = False)

        End Sub


        Public Overrides ReadOnly Property UniqueName As String
            Get
                Return "CQSPDCALPROVRICHA"
            End Get
        End Property
    End Class


End Class