Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls
Imports DMD.Anagrafica


Partial Public Class CustomerCalls

    Public Class CRMCalendarProvider
        Inherits BaseCalendarActivitiesProvider

        Private Structure PersonaInfo
            Implements IComparable

            Public ID As Integer
            Public PO As Integer
            Public Stato As String

            Public Sub New(ByVal c As CPersonaFisica)
                Me.ID = GetID(c)
                Me.PO = c.IDPuntoOperativo
                Me.Stato = c.DettaglioEsito
            End Sub

            Public Sub New(ByVal id As Integer)
                Me.ID = id
                Me.PO = 0
            End Sub

            Public Function CompareTo(ByVal obj As PersonaInfo) As Integer
                Return Arrays.Compare(Me.ID, obj.ID)
            End Function

            Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
                Return Me.CompareTo(obj)
            End Function
        End Structure

        Private listLock As New Object
        Private m_List As PersonaInfo()
        ' Private m_statiValidi As String() = Nothing
        '{
        '    "Da Contattare",
        '    "Interessato",
        '    "Non Interessato",
        '    "Non Fattibile",
        '    "Irrintracciabile",
        '    "Attesa Busta Paga",
        '    "Busta Paga Ricevuta",
        '    "Sollecitare Appuntamento",
        '    "Appuntamento Fissato",
        '    "Certificato Stipendio/CUD da Richiedere",
        '    "Certificato Stipendio/CUD Richiesto",
        '    "Certificato Stipendio/CUD Ricevuto",
        '    "Da Valutare",
        '    "In Valutazione",
        '    "Proposta",
        '    "In Lavorazione",
        '    "Proporre Delega",
        '    "Rinnovabile"
        '}

        Private statiValidi As String() =
        {
            "Attesa Richiamata",
            "Da Contattare",
            "NON CONTATTARE",
            "Irrintracciabile",
            "Interessato",
            "Non Interessato",
            "Non Fattibile",
            "Studio di Fattibità",
            "Pratica",
            "Certificato di Stipendio/CUD",
            "Busta Paga"
        }

        Public Sub New()
            AddHandler Anagrafica.PersonaCreated, AddressOf handlePersona
            AddHandler Anagrafica.PersonaDeleted, AddressOf handlePersona
            AddHandler Anagrafica.PersonaModified, AddressOf handlePersona
            Me.m_List = Nothing
        End Sub


        Private Function IsInList(ByVal id As Integer) As Boolean
            Return Arrays.Len(Me.m_List) > 0 AndAlso Arrays.BinarySearch(Me.m_List, New PersonaInfo(id)) >= 0
        End Function

        Private Sub AddToList(ByVal c As CPersonaFisica)
            Dim i As Integer = -1
            Dim info As New PersonaInfo(c)
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
            If (Arrays.Len(Me.m_List) > 0) Then i = Arrays.BinarySearch(Me.m_List, New PersonaInfo(id))
            If (i >= 0) Then Me.m_List = Arrays.RemoveAt(Me.m_List, i)
        End Sub



        Private Function CheckStato(ByVal p As CPersonaFisica) As Boolean
            Return Arrays.IndexOf(statiValidi, p.DettaglioEsito) >= 0
        End Function

        Private Sub handlePersona(ByVal e As PersonaEventArgs)
            Dim c As CPersona = e.Persona
            If (TypeOf (c) Is CPersonaFisica) Then
                SyncLock Me.listLock
                    If (c.Stato = ObjectStatus.OBJECT_VALID) AndAlso Me.CheckStato(c) Then
                        Me.AddToList(c)
                    Else
                        Me.RemoveFromList(GetID(c))
                    End If
                End SyncLock
            End If
        End Sub

        Private ReadOnly Property List As CCollection(Of PersonaInfo)
            Get
                SyncLock Me.listLock
                    If (Me.m_List Is Nothing) Then
                        Dim tmp As New System.Collections.ArrayList
                        Dim cursor As New CPersonaCursor
                        cursor.TipoPersona.Value = TipoPersona.PERSONA_FISICA
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                        cursor.DettaglioEsito.ValueIn(statiValidi)
                        cursor.ID.SortOrder = SortEnum.SORT_ASC
                        cursor.IgnoreRights = True
                        While Not cursor.EOF
                            tmp.Add(New PersonaInfo(cursor.Item))
                            cursor.MoveNext()
                        End While
                        cursor.Dispose()
                        Me.m_List = tmp.ToArray(GetType(PersonaInfo))
                    End If

                    Dim ret As New CCollection(Of PersonaInfo)
                    If (Me.m_List IsNot Nothing) Then ret.AddRange(Me.m_List)
                    Return ret
                End SyncLock
            End Get
        End Property

        Private Function GetList(ByVal stato As String) As CCollection(Of PersonaInfo)
            Dim u As CUser = Sistema.Users.CurrentUser
            Dim ret As New CCollection(Of PersonaInfo)
            For Each p As PersonaInfo In Me.List
                If p.Stato = stato AndAlso (p.PO = 0 OrElse u.Uffici.HasOffice(p.PO)) Then
                    ret.Add(p)
                End If
            Next
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
            ' SyncLock Me.listLock
            Dim ret As New CCollection(Of ICalendarActivity)
            Dim act As CCalendarActivity
            Dim items As CCollection(Of PersonaInfo)

            For Each stato As String In Me.statiValidi
                items = Me.GetList(stato)
                If (items.Count > 0) Then
                    act = New CCalendarActivity
                    DirectCast(act, ICalendarActivity).SetProvider(Me)
                    act.Descrizione = "Ci sono " & items.Count & " clienti in stato: <b>" & stato & "</b>"
                    act.DataInizio = Calendar.ToDay
                    act.GiornataIntera = True
                    act.Categoria = "Normale"
                    act.Flags = CalendarActivityFlags.IsAction
                    act.IconURL = "/widgets/images/activities/bustapagaatesa.png"
                    act.Note = "CRMShowClientiInStato" & vbLf & stato
                    act.Stato = ObjectStatus.OBJECT_VALID
                    act.Priorita = ret.Count
                    ret.Add(act)
                End If
            Next
            ''If (CRM.CRMGroup.Members.Contains(Sistema.Users.CurrentUser)) Then
            'items = Me.GetList("Attesa Busta Paga")
            'If (items.Count > 0) Then
            '    act = New CCalendarActivity
            '    DirectCast(act, ICalendarActivity).SetProvider(Me)
            '    act.Descrizione = "Ci sono " & items.Count & " clienti che devono inviare la busta paga"
            '    act.DataInizio = Calendar.Now
            '    act.GiornataIntera = True
            '    act.Categoria = "Urgente"
            '    act.Flags = CalendarActivityFlags.IsAction
            '    act.IconURL = "/widgets/images/activities/bustapagaatesa.png"
            '    act.Note = "CQSPDShowClientiInStatoAttesaBustaPaga"
            '    act.Stato = ObjectStatus.OBJECT_VALID
            '    ret.Add(act)
            'End If
            ''End If

            ''Dim c As CConsulentePratica = CQSPD.Consulenti.GetItemByUser(user)
            ''If (c IsNot Nothing) Then
            'items = Me.GetList("Busta Paga Ricevuta")
            'If (items.Count > 0) Then
            '    act = New CCalendarActivity
            '    DirectCast(act, ICalendarActivity).SetProvider(Me)
            '    act.Descrizione = "Ci sono " & items.Count & " buste paga da valutare"
            '    act.DataInizio = Calendar.Now
            '    act.GiornataIntera = True
            '    act.Categoria = "Urgente"
            '    act.Flags = CalendarActivityFlags.IsAction
            '    act.IconURL = "/widgets/images/activities/bustapagaricevuta.png"
            '    act.Note = "CQSPDShowClientiInStatoBustaPagaRicevuta"
            '    act.Stato = ObjectStatus.OBJECT_VALID
            '    ret.Add(act)
            'End If

            ''-------------------
            'items = Me.GetList("Proporre Delega")

            'If (items.Count > 0) Then
            '    act = New CCalendarActivity
            '    DirectCast(act, ICalendarActivity).SetProvider(Me)
            '    act.Descrizione = "Ci sono " & items.Count & " clienti a cui proporre la delega"
            '    act.DataInizio = Calendar.Now
            '    act.GiornataIntera = True
            '    act.Categoria = "Urgente"
            '    act.Flags = CalendarActivityFlags.IsAction
            '    act.IconURL = "/widgets/images/activities/bustapagaricevuta.png"
            '    act.Note = "CQSPDShowClientiInStatoClientiDelega"
            '    act.Stato = ObjectStatus.OBJECT_VALID
            '    ret.Add(act)
            'End If

            ''-------------------
            'items = Me.GetList("Rinnovabile")
            'If (items.Count > 0) Then
            '    act = New CCalendarActivity
            '    DirectCast(act, ICalendarActivity).SetProvider(Me)
            '    act.Descrizione = "Ci sono " & items.Count & " clienti che possono rinnovare"
            '    act.DataInizio = Calendar.Now
            '    act.GiornataIntera = True
            '    act.Categoria = "Urgente"
            '    act.Flags = CalendarActivityFlags.IsAction
            '    act.IconURL = "/widgets/images/activities/bustapagaricevuta.png"
            '    act.Note = "CQSPDShowClientiInStatoClientiRinnovabili"
            '    act.Stato = ObjectStatus.OBJECT_VALID
            '    ret.Add(act)
            'End If
            '' End If



            Return ret
            ' End SyncLock
        End Function

        Public Overrides Sub SaveActivity(item As ICalendarActivity, Optional force As Boolean = False)

        End Sub

        Public Overrides Sub DeleteActivity(item As ICalendarActivity, Optional force As Boolean = False)

        End Sub


        Public Overrides ReadOnly Property UniqueName As String
            Get
                Return "CRMCALPROVBASE"
            End Get
        End Property
    End Class


End Class