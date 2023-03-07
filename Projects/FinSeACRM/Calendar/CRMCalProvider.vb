Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls




    Public Class CRMCalProvider
        Inherits BaseCalendarActivitiesProvider

        Private Class FaxInfo
            Implements IComparable

            Public ID As Integer
            Public PO As Integer

            Public Sub New(ByVal c As FaxDocument)
                Me.ID = GetID(c)
                Me.PO = c.IDPuntoOperativo
            End Sub

            Public Sub New(ByVal id As Integer)
                Me.ID = id
                Me.PO = 0
            End Sub

            Private Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
                Dim tmp As FaxInfo = obj
                Return Arrays.Compare(Me.ID, tmp.ID)
            End Function
        End Class

        Private listLock As New Object
        Private m_FaxList As FaxInfo()

        Public Sub New()
            AddHandler CustomerCalls.CRM.ContattoModificato, AddressOf handleContatto
            AddHandler CustomerCalls.CRM.ContattoEliminato, AddressOf handleContatto
            AddHandler CustomerCalls.CRM.NuovoContatto, AddressOf handleContatto
            Me.m_FaxList = Nothing
        End Sub

        Private Function IsInFaxList(ByVal id As Integer) As Boolean
            Return Arrays.Len(Me.m_FaxList) > 0 AndAlso Arrays.BinarySearch(Me.m_FaxList, New FaxInfo(id)) >= 0
        End Function

        Private Sub AddToFaxList(ByVal c As FaxDocument)
            Dim i As Integer = -1
            Dim info As New FaxInfo(c)
            If (Arrays.Len(Me.m_FaxList) > 0) Then i = Arrays.BinarySearch(Me.m_FaxList, info)
            If (i >= 0) Then
                Me.m_FaxList(i) = info
            Else
                Me.m_FaxList = Arrays.Push(Me.m_FaxList, info)
                Array.Sort(Me.m_FaxList)
            End If
        End Sub

        Private Sub RemoveFromFaxList(ByVal id As Integer)
            Dim i As Integer = -1
            If (Arrays.Len(Me.m_FaxList) > 0) Then i = Arrays.BinarySearch(Me.m_FaxList, New FaxInfo(id))
            If (i >= 0) Then Me.m_FaxList = Arrays.RemoveAt(Me.m_FaxList, i)
        End Sub

        Private ReadOnly Property FaxList As CCollection(Of FaxInfo)
            Get
                SyncLock Me.listLock
                    If (Me.m_FaxList Is Nothing) Then
                        Dim cursor1 As New CustomerCalls.CCustomerCallsCursor
                        Dim tmp As New System.Collections.ArrayList
                        cursor1.Stato.Value = ObjectStatus.OBJECT_VALID
                        cursor1.IDPersona.Value = 0
                        cursor1.ClassName.Value = "FaxDocument"
                        cursor1.IgnoreRights = True
                        cursor1.ID.SortOrder = SortEnum.SORT_ASC
                        While Not cursor1.EOF
                            tmp.Add(New FaxInfo(cursor1.Item))
                            cursor1.MoveNext()
                        End While
                        cursor1.Dispose()
                        Me.m_FaxList = tmp.ToArray(GetType(FaxInfo))
                        'If (Me.m_FaxList IsNot Nothing) Then Array.Sort(Me.m_FaxList)
                    End If
                    Dim ret As New CCollection(Of FaxInfo)
                    If (Me.m_FaxList IsNot Nothing) Then ret.AddRange(Me.m_FaxList)
                    Return ret
                End SyncLock
            End Get
        End Property

        Private Sub handleContatto(ByVal e As ContattoEventArgs)
            Dim c As CContattoUtente = e.Contatto
            If (TypeOf (c) Is FaxDocument) Then
                SyncLock Me.listLock
                    If (c.Stato = ObjectStatus.OBJECT_VALID AndAlso c.IDPersona = 0) Then
                        Me.AddToFaxList(c)
                    Else
                        Me.RemoveFromFaxList(GetID(c))
                    End If
                End SyncLock
            End If
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

        Private Function GetAllowedFaxList() As CCollection(Of FaxInfo)
            Dim u As CUser = Sistema.Users.CurrentUser
            Dim ret As New CCollection(Of FaxInfo)
            For Each f As FaxInfo In Me.FaxList
                If (f.PO = 0) OrElse (u.Uffici.HasOffice(f.PO)) Then
                    ret.Add(f)
                End If
            Next
            Return ret
        End Function

        Public Overrides Function GetToDoList(user As CUser) As CCollection(Of ICalendarActivity)
            SyncLock Me.listLock
                Dim ret As New CCollection(Of ICalendarActivity)
                Dim act As CCalendarActivity

                If (CRM.CRMGroup.Members.Contains(Sistema.Users.CurrentUser)) Then
                    Dim items As CCollection(Of FaxInfo) = Me.GetAllowedFaxList
                    If items.Count > 0 Then
                        act = New CCalendarActivity
                        DirectCast(act, ICalendarActivity).SetProvider(Me)
                        act.Descrizione = "Ci sono " & items.Count & " fax da associare"
                        act.DataInizio = Calendar.Now
                        act.GiornataIntera = True
                        act.Categoria = "Normale"
                        act.Flags = CalendarActivityFlags.IsAction
                        act.IconURL = "/widgets/images/activities/faxdaassociare.png"
                        act.Note = "CRMShowFaxNonAssociati"
                        act.Stato = ObjectStatus.OBJECT_VALID
                        ret.Add(act)
                    End If
                End If
                Return ret
            End SyncLock
        End Function

        Public Overrides Sub SaveActivity(item As ICalendarActivity, Optional force As Boolean = False)

        End Sub

        Public Overrides ReadOnly Property UniqueName As String
            Get
                Return "CRMCALPROVIDER"
            End Get
        End Property
    End Class

End Class