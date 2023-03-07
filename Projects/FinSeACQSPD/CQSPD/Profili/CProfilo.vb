Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD

Partial Public Class CQSPD

    <Serializable> _
    Public Class CProfilo
        Inherits DBObject
        Implements ISupportInitializeFrom, IComparable

        Private m_CessionarioID As Integer
        Private m_Cessionario As CCQSPDCessionarioClass
        Private m_NomeCessionario As String
        Private m_Nome As String
        Private m_ProfiloVisibile As String
        Private m_Profilo As String
        Private m_Path As String
        Private m_Visibile As Boolean
        Private m_IconPath As String
        Private m_UserName As String
        Private m_Password As String
        Private m_ParentID As Integer
        Private m_Parent As CProfilo
        Private m_EreditaProdotti As Boolean
        Private m_ProvvigioneFissa As Double
        Private m_ConsentiProvvigione As Boolean
        Private m_DataInizio As Nullable(Of Date)
        Private m_DataFine As Nullable(Of Date)
        Private m_ProdottiXProfiloRelations As CProdottiXProfiloRelations
        Private m_UsersAuth As CProfiloXUserAllowNegateCollection
        Private m_GroupAuth As CProfiloXGroupAllowNegateCollection
         

        Public Sub New()
        End Sub

        Public Overrides Function GetModule() As CModule
            Return DMD.CQSPD.Profili.Module
        End Function

        ''' <summary>
        ''' Restituisce un oggetto che consente di accedere e modificare la relazione diretta (eredita, includi, escludi) e gli spread per ogni singolo prodotto.
        ''' Se il prodotto non è definito in questa collezione e se la proprietà EreditaProdotti è impostata su True esso verrà ereditato direttamente dal preventivatore genitore (se esiste).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ProdottiXProfiloRelations As CProdottiXProfiloRelations
            Get
                If (Me.m_ProdottiXProfiloRelations Is Nothing) Then Me.m_ProdottiXProfiloRelations = New CProdottiXProfiloRelations(Me)
                Return Me.m_ProdottiXProfiloRelations
            End Get
        End Property

        ''' <summary>
        ''' Restituisce l'insieme delle associazioni (consenti/nega) del profilo corrente per gli specifici utenti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UserAuth As CProfiloXUserAllowNegateCollection
            Get
                If (Me.m_UsersAuth Is Nothing) Then Me.m_UsersAuth = New CProfiloXUserAllowNegateCollection(Me)
                Return Me.m_UsersAuth
            End Get
        End Property

        ''' <summary>
        ''' Restituisce l'insieme delle associazioni (consenti/nega) del profilo corrente per i gruppi utente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GroupAuth As CProfiloXGroupAllowNegateCollection
            Get
                If (Me.m_GroupAuth Is Nothing) Then Me.m_GroupAuth = New CProfiloXGroupAllowNegateCollection(Me)
                Return Me.m_GroupAuth
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di inizio validità del profilo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizio As Nullable(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataInizio
                If (oldValue = value) Then Exit Property
                Me.m_DataInizio = value
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di fine validità del profilo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataFine As Nullable(Of Date)
            Get
                Return Me.m_DataFine
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataFine
                If (oldValue = value) Then Exit Property
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce vero se il profilo è valido alla data corrente
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsValid() As Boolean
            Return Me.IsValid(Calendar.Now)
        End Function

        ''' <summary>
        ''' Restituisce vero se il profilo è valido alla data corrente
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsValid(ByVal allaData As Date) As Boolean
            Return Calendar.CheckBetween(allaData, Me.m_DataInizio, Me.m_DataFine)
        End Function

        ''' <summary>
        ''' Restituisce il valore della provvigione fissa imposta al collaboratore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProvvigioneFissa As Double
            Get
                Return Me.m_ProvvigioneFissa
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_ProvvigioneFissa
                If (oldValue = value) Then Exit Property
                Me.m_ProvvigioneFissa = value
                Me.DoChanged("ProvvigioneFissa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore che indica se consentire al collaboratore di modificare la provvigione.
        ''' Se questo valore è impostato su False allora il collaboratore potrà caricare solo la ProvvigioneFissa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsentiProvvigione As Boolean
            Get
                Return Me.m_ConsentiProvvigione
            End Get
            Set(value As Boolean)
                If (Me.m_ConsentiProvvigione = value) Then Exit Property
                Me.m_ConsentiProvvigione = value
                Me.DoChanged("ConsentiProvvigione", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del cessionario a cui appartiene il profilo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeCessionario As String
            Get
                Return Me.m_NomeCessionario
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeCessionario
                If (oldValue = value) Then Exit Property
                Me.m_NomeCessionario = value
                Me.DoChanged("NomeCessionario", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del cessionario a cui appartiene il profilo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCessionario As Integer
            Get
                Return GetID(Me.m_Cessionario, Me.m_CessionarioID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCessionario
                If oldValue = value Then Exit Property
                Me.m_CessionarioID = value
                Me.m_Cessionario = Nothing
                Me.DoChanged("IDCessionario", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto Cessionario a cui appartiene il profilo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cessionario As CCQSPDCessionarioClass
            Get
                If (Me.m_Cessionario Is Nothing) Then Me.m_Cessionario = DMD.CQSPD.Cessionari.GetItemById(Me.m_CessionarioID)
                Return Me.m_Cessionario
            End Get
            Set(value As CCQSPDCessionarioClass)
                Dim oldValue As CCQSPDCessionarioClass = Me.Cessionario
                If (oldValue = value) Then Exit Property
                Me.m_Cessionario = value
                Me.m_CessionarioID = GetID(value)
                If Not (value Is Nothing) Then Me.m_NomeCessionario = value.Nome
                Me.DoChanged("Cessionario", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il profilo è visibile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Visibile As Boolean
            Get
                Return Me.m_Visibile
            End Get
            Set(value As Boolean)
                If (Me.m_Visibile = value) Then Exit Property
                Me.m_Visibile = value
                Me.DoChanged("Visibile", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del profilo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che viene visualizzata nell'elenco dei profili
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Profilo As String
            Get
                Return Me.m_Profilo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Profilo
                If (oldValue = value) Then Exit Property
                Me.m_Profilo = value
                Me.DoChanged("Profilo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del genitore di questo profilo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ParentID As Integer
            Get
                Return GetID(Me.m_Parent, Me.m_ParentID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ParentID
                If oldValue = value Then Exit Property
                Me.m_ParentID = value
                Me.m_Parent = Nothing
                Me.DoChanged("ParentID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il profilo può ereditare (ricorsivamente) i prodotti dal genitore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EreditaProdotti As Boolean
            Get
                Return Me.m_EreditaProdotti
            End Get
            Set(value As Boolean)
                If Me.m_EreditaProdotti = value Then Exit Property
                Me.m_EreditaProdotti = value
                Me.DoChanged("EreditaProdotti", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il profilo genitore di questo profilo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Parent As CProfilo
            Get
                If (Me.m_Parent Is Nothing) Then Me.m_Parent = DMD.CQSPD.Profili.GetItemById(Me.m_ParentID)
                Return Me.m_Parent
            End Get
            Set(value As CProfilo)
                Dim oldValue As CProfilo = Me.Parent
                If (oldValue = value) Then Exit Property
                Me.m_Parent = value
                Me.m_ParentID = GetID(value)
                Me.DoChanged("Parent", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che viene visualizzata in caso l'utente non disponga dei diritti
        ''' per visualizzare il nome del profilo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProfiloVisibile As String
            Get
                Return Me.m_ProfiloVisibile
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_ProfiloVisibile
                If (oldValue = value) Then Exit Property
                Me.m_ProfiloVisibile = value
                Me.DoChanged("ProfiloVisibile", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il percorso della backdoor per effettuare l'accesso ai profili esterni
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Path As String
            Get
                Return Me.m_Path
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Path
                If (oldValue = value) Then Exit Property
                Me.m_Path = value
                Me.DoChanged("Path", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce vero se il profilo non è esterno
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function IsOffline() As Boolean
            Return ("" & Me.m_Path = "")
        End Function

        ''' <summary>
        ''' Restituisce o imposta la URL o il percorso dell'immagine che identifica il profilo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IconPath As String
            Get
                Return Me.m_IconPath
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IconPath
                If (oldValue = value) Then Exit Property
                Me.m_IconPath = value
                Me.DoChanged("IconPath", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la username utilizzata per accedere al profilo esterno
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserName As String
            Get
                Return Me.m_UserName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_UserName
                If (oldValue = value) Then Exit Property
                Me.m_UserName = value
                Me.DoChanged("UserName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la password utilizzata per accedere al profilo esterno
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Password As String
            Get
                Return Me.m_Password
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Password
                If (oldValue = value) Then Exit Property
                Me.m_Password = value
                Me.DoChanged("Password")
            End Set
        End Property


        'Public Function DefaultLink() As String
        '    If Me.IsOffline() Then
        '        Return "/finanziaria/preventivo/?_a=preventivo&pid=" & GetID(Me)
        '    Else
        '        'tmp = Path & "?tk=" & ASPSecurity.FindTokenOrCreate("preventivatore=" & ID, ID)
        '        Return "/download.asp?tk=" & ASPSecurity.FindTokenOrCreate("preventivatore=" & GetID(Me), Me.Path & "?pid=" & GetID(Me))
        '    End If
        'End Function

        'Public Function DefaultTarget() As String
        '    If Me.IsOffline() Then
        '        Return "_self"
        '    Else
        '        Return "_blank"
        '    End If
        'End Function


        Public Function CompareTo(ByVal item As CProfilo) As Integer
            Return Strings.Compare(Me.Profilo, item.Profilo, CompareMethod.Text)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Function GetProdottiValidi() As CCollection(Of CCQSPDProdotto)
            Dim items As CCollection(Of CCQSPDProdotto)
            Dim ret As New CCollection(Of CCQSPDProdotto)
            items = Me.ProdottiXProfiloRelations.GetProdotti
            For i As Integer = 0 To items.Count - 1
                Dim prodotto As CCQSPDProdotto = items(i)
                If prodotto.IsValid Then ret.Add(prodotto)
            Next
            Return ret
        End Function

         
        ''' <summary>
        ''' Crea o modifica l'associazione (consenti/nega) tra il profilo corrente e l'utente
        ''' </summary>
        ''' <param name="user"></param>
        ''' <param name="allow"></param>
        ''' <param name="negate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetUserAllowNegate(ByVal user As CUser, ByVal allow As Boolean, ByVal negate As Boolean) As CProfiloXUserAllowNegate
            Dim item As CProfiloXUserAllowNegate = Nothing
            Dim i As Integer = 0
            While (i < Me.UserAuth.Count) AndAlso (item Is Nothing)
                If Me.UserAuth(i).UserID = GetID(user) Then
                    item = Me.UserAuth(i)
                Else
                    i += 1
                End If
            End While
            If (allow = negate) Then
                If (item IsNot Nothing) Then
                    item.Delete()
                    Me.UserAuth.Remove(item)
                End If
            Else
                If (item Is Nothing) Then
                    item = New CProfiloXUserAllowNegate
                    Me.UserAuth.Add(item)
                End If
                item.Item = Me
                item.User = user
                item.Allow = allow
                item.Negate = negate
                item.Save()
            End If

            Return item
        End Function

        ''' <summary>
        ''' Crea o modifica l'associazione (consenti/nega) tra il profilo corrente ed il gruppo
        ''' </summary>
        ''' <param name="group"></param>
        ''' <param name="allow"></param>
        ''' <param name="negate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SetGroupAllowNegate(ByVal group As CGroup, ByVal allow As Boolean, ByVal negate As Boolean) As CProfiloXGroupAllowNegate
            Dim item As CProfiloXGroupAllowNegate = Nothing
            Dim i As Integer = 0
            While (i < Me.GroupAuth.Count) AndAlso (item Is Nothing)
                If Me.GroupAuth(i).GroupID = GetID(group) Then
                    item = Me.GroupAuth(i)
                Else
                    i += 1
                End If
            End While
            If (allow = negate) Then
                If (item IsNot Nothing) Then
                    item.Delete()
                    Me.GroupAuth.Remove(item)
                End If
            Else
                If (item Is Nothing) Then
                    item = New CProfiloXGroupAllowNegate
                    Me.GroupAuth.Add(item)
                End If
                item.Item = Me
                item.Group = group
                item.Allow = allow
                item.Negate = negate
                item.Save()
            End If

            Return item
        End Function


        ''' <summary>
        ''' Restituisce vero se l'utente è abilitato all'azione corrente anche considerando i gruppi a cui appartiene l'utente stesso
        ''' </summary>
        ''' <param name="user"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsVisibleToUser(ByVal user As CUser) As Boolean
            Dim a = False, n As Boolean = False
            Dim i As Integer = 0
            Dim ua As CProfiloXUserAllowNegate
            Dim ga As CProfiloXGroupAllowNegate

            While (Not n AndAlso i < Me.UserAuth.Count)
                ua = Me.UserAuth(i)
                If (ua.UserID = GetID(user)) Then
                    a = ua.Allow
                    n = ua.Negate
                End If
                i += 1
            End While

            i = 0
            While (Not n AndAlso i < Me.GroupAuth.Count)
                ga = Me.GroupAuth(i)
                For Each gp As CGroup In user.Groups
                    If (ga.GroupID = GetID(gp)) Then
                        a = ga.Allow
                        n = ga.Negate
                        Exit For
                    End If
                Next
                i += 1
            End While

            Return a And Not n
        End Function

        Public Function IsVisibleToUser(ByVal userID As Integer) As Boolean
            Return Me.IsVisibleToUser(Sistema.Users.GetItemById(userID))
        End Function

       
        ''' <summary>
        ''' Restituisce una collezione di tutti i tipi contratto disponibili per il profilo
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTipiContrattoDisponibili() As CCollection(Of CTipoContratto)
            Dim items As New System.Collections.Hashtable
            For Each p As CCQSPDProdotto In Me.GetProdottiValidi
                If (p.IdTipoContratto <> "" AndAlso items.ContainsKey(p.IdTipoContratto)) Then items.Add(p.IdTipoContratto, p.IdTipoContratto)
            Next
            Dim ret As New CCollection(Of CTipoContratto)
            For Each tc As String In items.Keys
                Dim item As CTipoContratto = CQSPD.TipiContratto.GetItemByIdTipoContratto(tc)
                If (item IsNot Nothing) Then ret.Add(item)
            Next
            ret.Sort()
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce la collezione dei tipi rapporto disponibili in funzione del tipo contratto selezionato
        ''' </summary>
        ''' <param name="tipoContratto"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTipiRapportoDisponibili(ByVal tipoContratto As String) As CKeyCollection(Of CTipoRapporto)
            Dim sc As New CStringComparer
            Dim ret As New CKeyCollection(Of CTipoRapporto)
            tipoContratto = LCase(Trim(tipoContratto))
            Dim prodotti As CCollection(Of CCQSPDProdotto) = Me.GetProdottiValidi
            For i As Integer = 0 To prodotti.Count - 1
                Dim p As CCQSPDProdotto = prodotti(i)
                If LCase(p.IdTipoContratto) = tipoContratto AndAlso p.Visibile = True Then
                    Debug.Assert(p.Stato = ObjectStatus.OBJECT_VALID)
                End If
                Dim tr As CTipoRapporto = Anagrafica.TipiRapporto.GetItemByName(p.IdTipoRapporto)
                If (tr IsNot Nothing) Then
                    If Not ret.ContainsKey(p.IdTipoRapporto) Then ret.Add(p.IdTipoRapporto, tr)
                End If
            Next
            ret.Sort()

            Return ret
        End Function

        ' ''' <summary>
        ' ''' Restituisce la collezione dei gruppi a cui è assegnato il profilo
        ' ''' </summary>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function GetAllowedGroups() As CCollection(Of ItemAllowNegate(Of CGroup))
        '    Dim dbSQL As String = "SELECT * FROM [tbl_PreventivatoriXGroup] WHERE [tbl_PreventivatoriXGroup].[Preventivatore]=" & GetID(Me)
        '    Dim dbRis As System.Data.IDataReader = Nothing
        '    Try
        '        Dim ret As New CCollection(Of ItemAllowNegate(Of CGroup))
        '        dbRis = CQSPD.Database.ExecuteReader(dbSQL)
        '        While dbRis.Read
        '            Dim item As New ItemAllowNegate(Of CGroup)
        '            item.GroupAllow = Formats.ToBool(dbRis("Allow"))
        '            item.GroupNegate = Formats.ToBool(dbRis("Negate"))
        '            item.Item = Sistema.Groups.GetItemById(dbRis("Gruppo"))
        '        End While
        '        dbRis.Dispose()
        '        Return ret
        '    Catch ex As Exception
        '        Throw
        '    Finally
        '        If (dbRis IsNot Nothing) Then dbRis.Dispose()
        '    End Try
        'End Function

        'Public Function GetAllowedUsers() As CCollection(Of ItemAllowNegate(Of CUser))
        '    Dim dbSQL As String = "SELECT * FROM [tbl_PreventivatoriXUser] WHERE [tbl_PreventivatoriXUser].[Preventivatore]=" & GetID(Me)
        '    Dim dbRis As System.Data.IDataReader = Nothing
        '    Try
        '        Dim ret As New CCollection(Of ItemAllowNegate(Of CUser))
        '        dbRis = CQSPD.Database.ExecuteReader(dbSQL)
        '        While dbRis.Read
        '            Dim item As New ItemAllowNegate(Of CUser)
        '            item.UserAllow = Formats.ToBool(dbRis("Allow"))
        '            item.UserNegate = Formats.ToBool(dbRis("Negate"))
        '            item.Item = Sistema.Users.GetItemById(dbRis("Utente"))
        '        End While
        '        dbRis.Dispose()
        '        Return ret
        '    Catch ex As Exception
        '        Throw
        '    Finally
        '        If (dbRis IsNot Nothing) Then dbRis.Dispose()
        '    End Try

        '    'dbSQL = dbSQL & "SELECT * FROM ("
        '    'dbSQL = dbSQL & "SELECT [Login].[Stato], [Login].[ID] As [Utente], [Login].[UserName], [Login].[Nominativo], [tbl_PreventivatoriXUser].[Allow], [tbl_PreventivatoriXUser].[Negate] FROM [Login] INNER JOIN [tbl_PreventivatoriXUser] ON [Login].[ID]=[tbl_PreventivatoriXUser].[Utente] WHERE [tbl_PreventivatoriXUser].[Preventivatore]=" & Item.ID & " "
        '    'dbSQL = dbSQL & "UNION "
        '    'dbSQL = dbSQL & "SELECT [Login].[Stato], [ID] As [Utente], [UserName], [Nominativo], False As [Allow], False As [Negate] FROM [Login] WHERE [ID] Not In (SELECT [Utente] FROM [tbl_PreventivatoriXUser] WHERE [Preventivatore]=" & Item.ID & ") "
        '    'dbSQL = dbSQL & ") WHERE ([Login].[Stato]<>0) And ([Login].[Stato]<>" & ObjectStatus.OBJECT_DELETED & ") ORDER BY [Nominativo] ASC"
        '    'dbRis = APPConn.ExecuteReader(dbSQL)
        'End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Preventivatori"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_NomeCessionario = reader.Read("NomeCessionario", Me.m_NomeCessionario)
            Me.m_CessionarioID = reader.Read("Cessionario", Me.m_CessionarioID)
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Profilo = reader.Read("Profilo", Me.m_Profilo)
            Me.m_Path = reader.Read("path", Me.m_Path)
            Me.m_ProfiloVisibile = reader.Read("ProfiloVisibile", Me.m_ProfiloVisibile)
            Me.m_IconPath = reader.Read("IconPath", Me.m_IconPath)
            Me.m_UserName = reader.Read("UserName", Me.m_UserName)
            Me.m_Password = reader.Read("Password", Me.m_Password)
            Me.m_ParentID = reader.Read("Padre", Me.m_ParentID)
            Me.m_Visibile = reader.Read("Visibile", Me.m_Visibile)
            Me.m_EreditaProdotti = reader.Read("EreditaProdotti", Me.m_EreditaProdotti)
            Me.m_ProvvigioneFissa = reader.Read("ProvvigioneFissa", Me.m_ProvvigioneFissa)
            Me.m_ConsentiProvvigione = reader.Read("ConsentiProvvigione", Me.m_ConsentiProvvigione)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("ClassName", TypeName(Me))
            writer.Write("NomeCessionario", Me.m_NomeCessionario)
            writer.Write("Cessionario", Me.IDCessionario)
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Profilo", Me.m_Profilo)
            writer.Write("Visibile", Me.m_Visibile)
            writer.Write("path", Me.m_Path)
            writer.Write("ProfiloVisibile", Me.m_ProfiloVisibile)
            writer.Write("IconPath", Me.m_IconPath)
            writer.Write("UserName", Me.m_UserName)
            writer.Write("Password", Me.m_Password)
            writer.Write("Padre", Me.ParentID)
            writer.Write("EreditaProdotti", Me.m_EreditaProdotti)
            writer.Write("ProvvigioneFissa", Me.m_ProvvigioneFissa)
            writer.Write("ConsentiProvvigione", Me.m_ConsentiProvvigione)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome & " - " & Me.m_ProfiloVisibile
        End Function

        'Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
        '    Dim o As CProfilo = CQSPD.Profili.GetItemById(GetID(Me))
        '    Dim oldCessionario As CCQSPDCessionarioClass = Nothing
        '    If (o IsNot Nothing) Then oldCessionario = o.Cessionario

        '    Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
        '    If (ret) Then
        '        If Not (oldCessionario Is Me.Cessionario) Then
        '            If (oldCessionario IsNot Nothing) Then
        '                oldCessionario.Pr()
        '            End If
        '        End If
        '    End If

        '    Return ret
        'End Function

        'Public Sub InitFrom(ByVal value As CPreventivatoreFsbPrev)
        '    With value
        '        Me.m_Cessionario = .m_Cessionario
        '        Me.m_CessionarioID = .m_CessionarioID
        '        Me.m_ConsentiProvvigione = .m_ConsentiProvvigione
        '        Me.m_DataFine = .m_DataFine
        '        Me.m_DataInizio = .m_DataInizio
        '        Me.m_EreditaProdotti = .m_EreditaProdotti
        '        Me.m_IconPath = .m_IconPath
        '        Me.m_Nome = .m_Nome
        '        Me.m_NomeCessionario = .m_NomeCessionario
        '        Me.m_Parent = .m_Parent
        '        Me.m_ParentID = .m_ParentID
        '        Me.m_Password = .m_Password
        '        Me.m_Path = .m_Path
        '        Me.m_Profilo = .m_Profilo
        '        Me.m_ProfiloVisibile = .m_ProfiloVisibile
        '        Me.m_ProvvigioneFissa = .m_ProvvigioneFissa
        '        Me.m_UserName = .m_UserName
        '        Me.m_Visibile = .m_Visibile
        '    End With
        'End Sub

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("m_CessionarioID", Me.IDCessionario)
            writer.WriteAttribute("m_NomeCessionario", Me.m_NomeCessionario)
            writer.WriteAttribute("m_Nome", Me.m_Nome)
            writer.WriteAttribute("m_ProfiloVisibile", Me.m_ProfiloVisibile)
            writer.WriteAttribute("m_Profilo", Me.m_Profilo)
            writer.WriteAttribute("m_Path", Me.m_Path)
            writer.WriteAttribute("m_Visibile", Me.m_Visibile)
            writer.WriteAttribute("m_IconPath", Me.m_IconPath)
            writer.WriteAttribute("m_UserName", Me.m_UserName)
            writer.WriteAttribute("m_Password", Me.m_Password)
            writer.WriteAttribute("m_ParentID", Me.ParentID)
            writer.WriteAttribute("m_EreditaProdotti", Me.m_EreditaProdotti)
            writer.WriteAttribute("m_ProvvigioneFissa", Me.m_ProvvigioneFissa)
            writer.WriteAttribute("m_ConsentiProvvigione", Me.m_ConsentiProvvigione)
            writer.WriteAttribute("m_DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("m_DataFine", Me.m_DataFine)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "m_CessionarioID" : Me.m_CessionarioID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_NomeCessionario" : Me.m_NomeCessionario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_ProfiloVisibile" : Me.m_ProfiloVisibile = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_Profilo" : Me.m_Profilo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_Path" : Me.m_Path = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_Visibile" : Me.m_Visibile = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "m_IconPath" : Me.m_IconPath = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_UserName" : Me.m_UserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_Password" : Me.m_Password = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_ParentID" : Me.m_ParentID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_EreditaProdotti" : Me.m_EreditaProdotti = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "m_ProvvigioneFissa" : Me.m_ProvvigioneFissa = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "m_ConsentiProvvigione" : Me.m_ConsentiProvvigione = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "m_DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "m_DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.Profili.UpdateCached(Me)
        End Sub


    End Class


End Class