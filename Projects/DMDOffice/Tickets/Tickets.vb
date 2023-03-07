Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Internals
Imports DMD.Office

Namespace Internals


    Public NotInheritable Class CTicketsClass
        Inherits CGeneralClass(Of CTicket)

        Private m_GruppoResponsabili As CGroup = Nothing
        Private m_GruppoEsclusi As CGroup = Nothing
        Private m_Index As CIndexingService = Nothing

        Friend Sub New()
            MyBase.New("modTickets", GetType(CTicketCursor))
        End Sub

        Public ReadOnly Property Index As CIndexingService
            Get
                SyncLock Me
                    If (Me.m_Index Is Nothing) Then Me.m_Index = New CIndexingService(DMD.Office.Database)
                    Me.m_Index.Database = DMD.Office.Database
                    Me.m_Index.WordIndexFolder = System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, "tickets\wordindex\")
                    DMD.Sistema.FileSystem.CreateRecursiveFolder(Me.m_Index.WordIndexFolder)
                    Return Me.m_Index
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il gruppo dei responsabili cioè gli utenti a cui vengono notificate tutte le richieste di assistenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GruppoResponsabili As CGroup
            Get
                SyncLock Me
                    If (m_GruppoResponsabili Is Nothing) Then
                        m_GruppoResponsabili = Sistema.Groups.GetItemByName("Responsabili Supporto Tecnico")
                        If (m_GruppoResponsabili Is Nothing) Then
                            m_GruppoResponsabili = New CGroup("Responsabili Supporto Tecnico")
                            m_GruppoResponsabili.Description = "Gruppo a cui vengono notificate tutte le richieste di assistenza, qualsiasi sia la loro categoria."
                            m_GruppoResponsabili.Stato = ObjectStatus.OBJECT_VALID
                            m_GruppoResponsabili.Save()
                        End If
                    End If
                    Return m_GruppoResponsabili
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il gruppo degli utenti a cui non inviare alcun tipo di richiesta di assistenza.
        ''' Questo gruppo bypassa tutti gli altri 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GruppoEsclusi As CGroup
            Get
                SyncLock Me
                    If (m_GruppoEsclusi Is Nothing) Then
                        m_GruppoEsclusi = Sistema.Groups.GetItemByName("Esclusi Supporto Tecnico")
                        If (m_GruppoEsclusi Is Nothing) Then
                            m_GruppoEsclusi = New CGroup("Esclusi Supporto Tecnico")
                            m_GruppoEsclusi.Stato = ObjectStatus.OBJECT_VALID
                            m_GruppoEsclusi.Description = "Gruppo degli utenti per cui è bloccato l'invio delle notifiche delle richieste di assistenza"
                            m_GruppoEsclusi.Save()
                        End If
                    End If
                    Return m_GruppoEsclusi
                End SyncLock
            End Get
        End Property

        Protected Friend Shadows Sub doItemCreated(ByVal e As ItemEventArgs)
            MyBase.doItemCreated(e)
        End Sub

        Protected Friend Shadows Sub doItemDeleted(ByVal e As ItemEventArgs)
            MyBase.doItemDeleted(e)
        End Sub

        Protected Friend Shadows Sub doItemModified(ByVal e As ItemEventArgs)
            MyBase.doItemModified(e)
        End Sub

        Function GetActiveItems(ByVal user As CUser) As CCollection(Of CTicket)
            Dim ret As New CCollection(Of CTicket)
            If (GetID(user) = 0) Then Return ret

            Dim categories As CCollection(Of CTicketCategory) = TicketCategories.GetUserAllowedCategories(Sistema.Users.CurrentUser)
            Dim cursor As New CTicketCursor

            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.StatoSegnalazione.ValueIn({TicketStatus.APERTO, TicketStatus.INLAVORAZIONE, TicketStatus.INSERITO, TicketStatus.RIAPERTO, TicketStatus.SOSPESO})
            If (categories.Count = 0) Then
                cursor.IDApertoDa.Value = GetID(user)
            Else
                Dim buff As New System.Text.StringBuilder
                buff.Append("[ApertoDa]=" & GetID(user))
                For Each cat As CTicketCategory In categories
                    If (buff.Length > 0) Then buff.Append(" OR ")
                    If (cat.Sottocategoria = "") Then
                        buff.Append("([Categoria]=" & DBUtils.DBString(cat.Categoria) & ")")
                    Else
                        buff.Append("([Categoria]=" & DBUtils.DBString(cat.Categoria) & " AND [Sottocategoria]=" & DBUtils.DBString(cat.Sottocategoria) & ")")
                    End If
                Next

                cursor.WhereClauses.Add(buff.ToString)
                buff.Clear()
            End If

            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()

            Return ret
        End Function

    End Class


End Namespace

Partial Public Class Office
    Private Shared m_Tickets As CTicketsClass = Nothing

    Public Shared ReadOnly Property Tickets As CTicketsClass
        Get
            If (m_Tickets Is Nothing) Then m_Tickets = New CTicketsClass
            Return m_Tickets
        End Get
    End Property

End Class