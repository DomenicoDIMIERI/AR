Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema


    Public Enum UserStatus As Integer
        ' USER_TEMP = 0         'Utente non valido
        USER_ENABLED = 1      'Utente abilitato
        USER_DISABLED = 2     'Utente disabilitato
        USER_DELETED = 3      'Utente eliminato
        USER_NEW = 4          'Utente in attesa di essere confermato
        USER_SUSPENDED = 5    'Utente sospeso temporaneamente
    End Enum

    <Flags>
    Public Enum UserFlags As Integer
        None = 0
        Hidden = 1
        ForceChangePassword = 2
    End Enum

    <Serializable> _
    Public NotInheritable Class CUser
        Inherits DBObjectPO
        Implements ISupportsSingleNotes, IComparable, IFonte, ISettingsOwner

        Public Event PasswordChanged(ByVal sender As Object, ByVal e As UserEventArgs)

        Private m_UserName As String 'UserName
        Private m_Nominativo As String '[TEXT] Nominativo
        Private m_IconURL As String
        Private m_email As String 'e-mail dell'utente
        'Private m_Password		'[Password
        Private m_ExpireDate As Nullable(Of Date) '[DATE] Data di scadenza dell'account
        Private m_ExpireLogin As Nullable(Of Date) '[date] Data di scadenza dell'account
        Private m_ExpireDays As Integer '[int]  Numero di giorni di cui prolungare la data di scadenza dell'account
        'Private m_IsLogged As Boolean '[BOOL] Vero se l'utente è loggato
        Private m_LastLogin As CLoginHistory  '[CLoginHistory] ultimo login 
        Private m_Settings As CUserSettings '[CUserSettings] Collezione di parametri aggiuntivi
        Private m_HomePage As String '[TEXT] Indirizzo della pagina iniziale per l'utente

        Private m_PersonaID As Integer '[INT]  ID della persona collegata all'utenza   
        Private m_Persona As CPersona '[CPersona] Persona o azienda collegata all'utenza
        Private m_Flags As Integer '[BOOL] Vero se l'utente è visibile
        Private m_Note As String 'Note
        Private m_Uffici As CUserUffici
        Private m_Groups As CUserGroups
        Private m_UserStato As UserStatus
        Private m_ClientCertificate As String   'Percorso del certificato utente
        Private m_Authorizations As CUserAuthorizationCollection
        Private m_Modules As CModuleXUserCollection
        Private m_PasswordExpire As Date?           'Data in cui la password scade

        Public Sub New()
            Me.m_UserName = ""
            Me.m_Nominativo = ""
            Me.m_ExpireDate = Nothing
            Me.m_ExpireLogin = Nothing
            Me.m_ExpireDays = 0
            ' Me.m_IsLogged = False
            Me.m_HomePage = ""
            'Me.m_Visible = True
            Me.m_Flags = UserFlags.None
            Me.m_PersonaID = 0
            Me.m_LastLogin = Nothing
            Me.m_Settings = Nothing
            Me.m_Persona = Nothing
            Me.m_Uffici = Nothing
            Me.m_UserStato = UserStatus.USER_NEW
            Me.m_email = ""
            Me.m_Note = ""
            Me.m_IconURL = "/widgets/images/default.GIF"
            Me.m_ClientCertificate = ""
            Me.m_PasswordExpire = Nothing
        End Sub

        Public Sub New(ByVal userName As String)
            Me.New()
            Me.SetUserName(userName)
            Me.m_Nominativo = userName
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Sistema.Users.Module
        End Function

        ''' <summary>
        ''' Restituisce o imposta la data oltre la quale é necessario resettare la password
        ''' </summary>
        ''' <returns></returns>
        Public Property PasswordExpire As Date?
            Get
                Return Me.m_PasswordExpire
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_PasswordExpire
                If (Calendar.Compare(value, oldValue) = 0) Then Return
                Me.m_PasswordExpire = value
                Me.DoChanged("PasswordExpire", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la collezione delle azioni consentite o negate esplicitamente all'utente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Authorizations As CUserAuthorizationCollection
            Get
                If (Me.m_Authorizations Is Nothing) Then Me.m_Authorizations = New CUserAuthorizationCollection(Me)
                Return Me.m_Authorizations
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione dei moduli esplicitamente autorizzati o negati per l'utente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Modules As CModuleXUserCollection
            Get
                If (Me.m_Modules Is Nothing) Then Me.m_Modules = New CModuleXUserCollection(Me)
                Return Me.m_Modules
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il percorso del certificato che identifica l'utente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ClientCertificate As String
            Get
                Return Me.m_ClientCertificate
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_ClientCertificate
                If (oldValue = value) Then Exit Property
                Me.m_ClientCertificate = value
                Me.DoChanged("ClientCertificate", value, oldValue)
            End Set
        End Property


        Public Property UserStato As UserStatus
            Get
                Return Me.m_UserStato
            End Get
            Set(value As UserStatus)
                Dim oldValue As UserStatus = Me.m_UserStato
                If (oldValue = value) Then Exit Property
                Me.m_UserStato = value
                Me.DoChanged("UserStato", value, oldValue)
            End Set
        End Property

        Public Property IconURL As String
            Get
                Return Me.m_IconURL
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IconURL
                If (oldValue = value) Then Exit Property
                Me.m_IconURL = value
                Me.DoChanged("IconURL", value, oldValue)
            End Set
        End Property

        Public Function CheckPassword(ByVal value As String) As Boolean
            Dim db As CDBConnection = Me.GetConnection
            If (db.IsRemote) Then
                Dim ret As String = RPC.InvokeMethod("/widgets/websvc/?_m=Utente&_a=CheckPassword", "uid", RPC.int2n(GetID(Me)), "pwd", RPC.str2n(value))
                Return (ret = "0")
            Else
                Dim dbRis As System.Data.IDataReader
                Dim ret As Boolean
                ret = False
                dbRis = APPConn.ExecuteReader("SELECT [UsrPwd] FROM [Login] WHERE [ID]=" & Me.ID)
                If dbRis.Read Then ret = (Formats.ToString(dbRis("UsrPwd")) = value)
                dbRis.Dispose()
                Return ret
            End If
        End Function

        Public ReadOnly Property Uffici As CUserUffici
            Get
                SyncLock Me
                    If (Me.m_Uffici Is Nothing) Then Me.m_Uffici = New CUserUffici(Me)
                    Return Me.m_Uffici
                End SyncLock
            End Get
        End Property

        Protected Overrides Sub OnCreate(e As DMD.SystemEvent)
            'Me.m_UserName = Sistema.Users.GetFirstAvailableUserName
            MyBase.OnCreate(e)
            Users.OnUserCreated(New UserEventArgs(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            Users.OnUserDeleted(New UserEventArgs(Me))
        End Sub


        ''' <summary>
        ''' Restituisce il nome dell'utente corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UserName As String
            Get
                Return Me.m_UserName
            End Get
        End Property
        Public Sub SetUserName(ByVal value As String)
            Me.m_UserName = Trim("" & value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome visualizzato per l'utente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nominativo As String
            Get
                Return Me.m_Nominativo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nominativo
                If (oldValue = value) Then Exit Property
                Me.m_Nominativo = value
                Me.DoChanged("Nominativo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di scadenza della password
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ExpireDate As Nullable(Of Date)
            Get
                Return Me.m_ExpireDate
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_ExpireDate
                If (oldValue = value) Then Exit Property
                Me.m_ExpireDate = value
                Me.DoChanged("ExpireDate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di scadenza dell'account
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ExpireLogin As Nullable(Of Date)
            Get
                Return Me.m_ExpireLogin
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_ExpireLogin
                If (oldValue = value) Then Exit Property
                Me.m_ExpireLogin = value
                Me.DoChanged("ExpireLogin", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di giorni di cui prolungare la data di scadenza dell'account
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ExpireDays As Integer
            Get
                Return Me.m_ExpireDays
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_ExpireDays
                If (oldValue = value) Then Exit Property
                Me.m_ExpireDays = value
                Me.DoChanged("ExpireDays", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'e-mail principale dell'account
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property eMail As String
            Get
                Return Me.m_email
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_email
                If (oldValue = value) Then Exit Property
                Me.m_email = value
                Me.DoChanged("eMail", value, oldValue)
            End Set
        End Property

        Public Property Note As String Implements ISupportsSingleNotes.Notes
            Get
                Return Me.m_Note
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Note
                If (oldValue = value) Then Exit Property
                Me.m_Note = value
                Me.DoChanged("Note", value, oldValue)
            End Set
        End Property

        Public Property HomePage As String
            Get
                Return Me.m_HomePage
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_HomePage
                If (oldValue = value) Then Exit Property
                Me.m_HomePage = value
                Me.DoChanged("HomePage", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se l'utente è visibile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Visible As Boolean
            Get
                Return Not TestFlag(Me.m_Flags, UserFlags.Hidden) ' Me.m_Visible
            End Get
            Set(value As Boolean)
                If (Me.Visible = value) Then Exit Property
                'Me.m_Visible = value
                Me.m_Flags = SetFlag(Me.m_Flags, UserFlags.Hidden, Not value)
                Me.DoChanged("Visible", value, Not value)
            End Set
        End Property

        Public Property Flags As UserFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As UserFlags)
                Dim oldValue As UserFlags = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la collezione delle impostazioni utente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Settings As CUserSettings
            Get
                SyncLock Me
                    If (Me.m_Settings Is Nothing) Then Me.m_Settings = New CUserSettings(Me)
                    Return Me.m_Settings
                End SyncLock
            End Get
        End Property

        Private ReadOnly Property _Settings As CSettings Implements ISettingsOwner.Settings
            Get
                Return Me.Settings
            End Get
        End Property

        Public Property PersonaID As Integer
            Get
                Return GetID(Me.m_Persona, Me.m_PersonaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.PersonaID
                If (oldValue = value) Then Exit Property
                Me.m_PersonaID = value
                Me.m_Persona = Nothing
                Me.DoChanged("PersonaID", value, oldValue)
            End Set
        End Property

        Public Property Persona As CPersona
            Get
                If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_PersonaID)
                Return Me.m_Persona
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.Persona
                If (oldValue = value) Then Exit Property
                Me.m_Persona = value
                Me.m_PersonaID = GetID(value)
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_UserName
        End Function

        ' Rinomina l'utente corrente
        ' Valori restituiti:
        ' 	0 - Nessun errore
        '   1 - Nuovo nome utente non valido
        '   2 - Il nome utente esiste già 
        '   3 - Errore durante il salvataggio
        ' 255 - Errore generico
        Public Function Rename(ByVal newUserName As String) As Integer
            Dim db As CDBConnection = Me.GetConnection
            If (db.IsRemote) Then
                Dim ret As String = RPC.InvokeMethod("/widgets/websvc/?_m=Utente&_a=Rename", "uid", RPC.int2n(GetID(Me)), "nun", RPC.str2n(newUserName))
                If (ret = "0") Then
                    Me.SetUserName(newUserName)
                    Return 0
                Else
                    Return 255
                End If
            Else
                Dim item As CUser
                Dim ret As Integer
                newUserName = Trim("" & newUserName)
                If LCase(UserName) = LCase(newUserName) Then
                    ret = 0
                Else
                    ret = 255
                    If Len(newUserName) < Sistema.Users.MINUSERNAMELEN Then
                        ret = 1
                    Else
                        item = Sistema.Users.GetItemByName(newUserName)
                        If item.Stato <> 0 And item.Stato <> 2 Then
                            ret = 2
                        Else
                            Me.SetUserName(newUserName)
                            If Me.SaveToDatabase(APPConn.GetConnection, False) = False Then
                                ret = 3
                            End If
                        End If
                    End If
                End If

                Return ret
            End If
        End Function

        Private Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(DirectCast(obj, CUser))
        End Function

        Public Function CompareTo(ByVal obj As CUser) As Integer
            Dim ret As Integer
            ret = Strings.Compare(Me.Nominativo, obj.Nominativo, CompareMethod.Text)
            If (ret = 0) Then ret = Strings.Compare(Me.UserName, obj.UserName, CompareMethod.Text)
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce l'accesso della sessione corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CurrentLogin As CLoginHistory
            Get
                Return ApplicationContext.CurrentLogin
            End Get
        End Property

        ''' <summary>
        ''' Restituisce le informazioni sull'accesso più recente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property LastLogin As CLoginHistory
            Get
                If (Me.m_LastLogin IsNot Nothing) Then Return Me.m_LastLogin
                Dim db As CDBConnection = Me.GetConnection
                If (db.IsRemote) Then
                    Dim tmp As String = RPC.InvokeMethod("/widgets/websvc/?_m=Utenti&_a=GetLastLogin", "uid", RPC.int2n(Me.ID()))
                    If (Not tmp.Equals("")) Then Me.m_LastLogin = XML.Utils.Serializer.Deserialize(tmp)
                Else
                    Me.m_LastLogin = New CLoginHistory
                    Dim dbSQL As String = "SELECT * FROM [tbl_LoginHistory] WHERE ([User]=" & Me.ID & ") And [LogOutTime]=(SELECT Max([LogOutTime]) AS Espr1 FROM [tbl_LoginHistory] WHERE (((tbl_LoginHistory.[User])=" & Me.ID & ")))"
                    Dim reader As New DBReader(LOGConn.Tables("tbl_LoginHistory"), dbSQL)
                    If reader.Read Then
                        LOGConn.Load(Me.m_LastLogin, reader)
                    End If
                    Me.m_LastLogin.SetUser(Me)
                    reader.Dispose()
                End If
                Return Me.m_LastLogin
            End Get
        End Property

        Private Function validatePassword(ByVal value As String) As Boolean
            If (Len(value) < Sistema.Settings.PWDMINLEN) Then Return False
            Dim reg As New System.Text.RegularExpressions.Regex(Sistema.Settings.PWDPATTERN)
            Return reg.IsMatch(value)
        End Function

        ''' <summary>
        ''' Modifica la password dell'utente
        ''' </summary>
        ''' <param name="oldPassword"></param>
        ''' <param name="newPassword"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ChangePassword(ByVal oldPassword As String, ByVal newPassword As String) As Integer
            Dim ret As Integer = 0

            If Me.GetConnection.IsRemote Then
                Dim tmp As String = RPC.InvokeMethod("/websvc/?_m=Utenti&_a=ChangeUserPassword", "uid", RPC.int2n(GetID(Me)), "op", RPC.str2n(oldPassword), "np", RPC.str2n(newPassword))
                If (tmp <> "") Then Throw New Exception(tmp)
            Else
                Dim dbRis As System.Data.IDataReader
                Dim dbSQL As String
                Dim userPassword As String

                ret = 255
                If Not Me.validatePassword(newPassword) Then
                    ret = 2
                Else
                    ret = 255
                    dbSQL = "SELECT [UsrPwd] FROM [Login] WHERE [ID]=" & Me.ID & ";"
                    dbRis = APPConn.ExecuteReader(dbSQL)
                    If (dbRis.Read) Then
                        userPassword = Formats.ToString(dbRis("UsrPwd"))
                        If (userPassword <> oldPassword) Then
                            ret = 1
                        Else
                            Me.m_ExpireDate = DateAdd("d", DEFAULT_PASSWORD_INTERVAL, Now)
                            dbSQL = "UPDATE [Login] SET [UsrPwd] = '" & Replace(newPassword, "'", "''") & "', [ExpireDate]=" & DBUtils.DBDate(m_ExpireDate) & " WHERE [ID]=" & Me.ID & ";"
                            APPConn.ExecuteCommand(dbSQL)
                            ret = 0
                        End If
                    End If

                    dbRis.Dispose()
                    dbRis = Nothing
                End If

            End If

            Dim e As New UserEventArgs(Me)
            Me.OnPasswordChanged(e)
            Sistema.Users.notifyPasswordChanged(e)
            Me.GetModule.DispatchEvent(New EventDescription("password_changed", "Password modificata per l'utente: " & Me.UserName, e))

            Return ret
        End Function

        Protected Sub OnPasswordChanged(ByVal e As UserEventArgs)
            RaiseEvent PasswordChanged(Me, e)
        End Sub

        ''' <summary>
        ''' Forza la password dell'utente
        ''' </summary>
        ''' <param name="newPassword"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ForcePassword(ByVal newPassword As String) As Integer
            Dim ret As Integer = 0

            If Me.GetConnection.IsRemote Then
                Dim tmp As String = RPC.InvokeMethod("/websvc/?_m=Utenti&_a=ForceUserPassword", "uid", RPC.int2n(GetID(Me)), "np", RPC.str2n(newPassword))
                If (tmp <> "") Then Throw New Exception(tmp)
            Else
                    Dim dbRis As System.Data.IDataReader
                Dim dbSQL As String

                ret = 255
                'If Not Me.validatePassword(newPassword) < Sistema.Settings.PWDMINLEN Then
                '    ret = 2
                'Else
                ret = 255

                    dbSQL = "SELECT [UsrPwd] FROM [Login] WHERE [ID]=" & Me.ID & ";"
                    dbRis = APPConn.ExecuteReader(dbSQL)
                    If (dbRis.Read) Then
                        Me.m_ExpireDate = DateAdd("d", DEFAULT_PASSWORD_INTERVAL, Now)
                        dbSQL = "UPDATE [Login] SET [UsrPwd] = '" & Replace(newPassword, "'", "''") & "', [ExpireDate]=" & DBUtils.DBDate(m_ExpireDate) & " WHERE [ID]=" & Me.ID & ";"
                        APPConn.ExecuteCommand(dbSQL)
                        ret = 0
                    Else
                        ret = 3
                    End If

                    dbRis.Dispose()
                    dbRis = Nothing

                'End If



            End If

            Dim e As New UserEventArgs(Me)
            Me.OnPasswordChanged(e)
            Sistema.Users.notifyPasswordChanged(e)
            Me.GetModule.DispatchEvent(New EventDescription("password_changed", "Password forzata per l'utente: " & Me.UserName, e))

            Return ret
        End Function

        ''' <summary>
        ''' Restituisce un array contenente tutti i gruppi a cui appartiene l'utente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Groups As CUserGroups
            Get
                If (Me.m_Groups Is Nothing) Then
                    Me.m_Groups = New CUserGroups
                    Me.m_Groups.Load(Me)
                End If
                Return Me.m_Groups
            End Get
        End Property

        Public Function IsAdministrator() As Boolean
            Return Me.IsInGroup("Administrators")
        End Function

        Public Function IsInGroup(ByVal groupName As String) As Boolean
            Return Me.Groups.Contains(groupName)
        End Function



        ''' <summary>
        ''' Restituisce vero se esiste un utente loggato nella sessione corrente
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsLogged() As Boolean
            Return ApplicationContext.IsUserLogged(Me)
        End Function

        ''' <summary>
        ''' Restituisce vero se l'account è scaduto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsExpired() As Boolean
            If Me.ExpireLogin.HasValue Then
                Return (Now >= DateAdd("d", Me.ExpireDays, Me.ExpireLogin.Value))
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Restituisce vero se esiste un utente loggato nella sessione corrente
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsImpersonating() As Boolean
            Return False
        End Function

        Friend Function LogIn(ByVal password As String, ByVal parameters As CKeyCollection) As CLoginHistory
            Dim db As CDBConnection = Me.GetConnection
            If (db.IsRemote) Then
                Dim ret As String = RPC.InvokeMethod("/websvc/websvc/?_m=Utenti&_a=LogIn", "usr", RPC.str2n(Me.UserName), "pwd", RPC.str2n(password))
                If (Strings.Left(ret, 2) = "00") Then
                    Me.m_LastLogin = XML.Utils.Serializer.Deserialize(Strings.Mid(ret, 3))
                Else
                    Throw New Exception(Strings.Mid(ret, 3))
                End If
            Else
                If (LCase(Me.UserName) = "system") Or (LCase(Me.UserName) = "guest") Then Throw New InvalidConstraintException("L'utente [" & Me.UserName & "] è un utente di sistema")
                Dim dbSQL As String = "UPDATE [Login] SET [IsLogged]=TRUE WHERE [ID]=" & GetID(Me) & ";"

                db.ExecuteCommand(dbSQL)
                'Me.m_IsLogged = True
                Me.m_LastLogin = New CLoginHistory(Me, parameters)
                Me.m_LastLogin.Save()
            End If

            Sistema.ApplicationContext.CurrentUser = Me
            Sistema.ApplicationContext.CurrentLogin = Me.m_LastLogin

            Return Me.m_LastLogin
        End Function



        ''' <summary>
        ''' Effettua la disconnessione dell'utente corrente
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LogOut(ByVal method As LogOutMethods) As Boolean
            Dim dbConn As CDBConnection = Me.GetConnection
            If (dbConn.IsRemote) Then
                Dim ret As String = RPC.InvokeMethod("/websvc/websvc/?_m=Sistema&_a=CurrentUserLogOut")
                'If (ret != "") throw new a 
            Else
                Dim currLogin As CLoginHistory = ApplicationContext.CurrentLogin
                If (currLogin IsNot Nothing) Then
                    currLogin.SetLogoutMethod(method)
                    currLogin.SetLogOutTime(Calendar.Now)
                    currLogin.Save(True)
                End If
                ApplicationContext.CurrentLogin = Nothing
                ApplicationContext.CurrentUser = Users.KnownUsers.GuestUser
            End If

            Sistema.Users.OnUserLoggedOut(New UserLogoutEventArgs(Me, method))
            Return True
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "Login"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_UserName = reader.Read("UserName", Me.m_UserName)
            Me.m_ExpireDate = reader.Read("ExpireDate", Me.m_ExpireDate)
            Me.m_ExpireLogin = reader.Read("ExpireLogin", Me.m_ExpireLogin)
            Me.m_ExpireDays = reader.Read("ExpireDays", Me.m_ExpireDays)
            Me.m_Nominativo = reader.Read("Nominativo", Me.m_Nominativo)
            Me.m_email = reader.Read("email", Me.m_email)
            Me.m_Note = reader.Read("Notes", Me.m_Note)
            Me.m_PersonaID = reader.Read("Persona", Me.m_PersonaID)
            Me.m_HomePage = reader.Read("HomePage", Me.m_HomePage)
            'Me.m_Visible = reader.Read("Visible", Me.m_Visible)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_UserStato = reader.Read("UserStato", Me.m_UserStato)
            Me.m_IconURL = reader.Read("IconURL", Me.m_IconURL)
            Me.m_ClientCertificate = reader.Read("ClientCert", Me.m_ClientCertificate)
            Me.m_PasswordExpire = reader.Read("PasswordExpire", Me.m_PasswordExpire)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("UserName", Me.m_UserName)
            writer.Write("ExpireDate", Me.m_ExpireDate)
            writer.Write("ExpireLogin", Me.m_ExpireLogin)
            writer.Write("ExpireDays", Me.m_ExpireDays)
            writer.Write("Nominativo", Me.m_Nominativo)
            writer.Write("email", Me.m_email)
            writer.Write("Notes", Me.m_Note)
            writer.Write("Persona", GetID(Me.m_Persona, Me.m_PersonaID))
            writer.Write("HomePage", Me.m_HomePage)
            'writer.Write("Visible", Me.m_Visible)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("UserStato", Me.m_UserStato)
            writer.Write("IconURL", Me.m_IconURL)
            writer.Write("ClientCert", Me.m_ClientCertificate)
            writer.Write("PasswordExpire", Me.m_PasswordExpire)
            Return MyBase.SaveToRecordset(writer)
        End Function

        '-------------------------------------
        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("UserName", Me.m_UserName)
            writer.WriteAttribute("Nominativo", Me.m_Nominativo)
            writer.WriteAttribute("email", Me.m_email)
            writer.WriteAttribute("ExpireDate", Me.m_ExpireDate)
            writer.WriteAttribute("ExpireLogin", Me.m_ExpireLogin)
            writer.WriteAttribute("ExpireDays", Me.m_ExpireDays)
            writer.WriteAttribute("HomePage", Me.m_HomePage)
            writer.WriteAttribute("PersonaID", Me.PersonaID)
            'writer.WriteAttribute("Visible", Me.m_Visible)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("UserStato", Me.m_UserStato)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            writer.WriteAttribute("ClientCert", Me.m_ClientCertificate)
            writer.WriteAttribute("PasswordExpire", Me.m_PasswordExpire)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Note", Me.m_Note)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "UserName" : Me.m_UserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Nominativo" : Me.m_Nominativo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "email" : Me.m_email = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ExpireDate" : Me.m_ExpireDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ExpireLogin" : Me.m_ExpireLogin = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ExpireDays" : Me.m_ExpireDays = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "HomePage" : Me.m_HomePage = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PersonaID" : Me.m_PersonaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                'Case "Visible" : Me.m_Visible = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "UserStato" : Me.m_UserStato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PasswordExpire" : Me.m_PasswordExpire = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IsLogged"
                Case "ClientCert" : Me.m_ClientCertificate = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Private ReadOnly Property _NomeFonte As String Implements IFonte.Nome
            Get
                Return Me.m_Nominativo
            End Get
        End Property

        Private ReadOnly Property _FonteIconURL As String Implements IFonte.IconURL
            Get
                Return Me.m_IconURL
            End Get
        End Property

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Sistema.Users.UpdateCached(Me)
        End Sub

        Public Function IsValid() As Boolean
            Return Me.Stato = ObjectStatus.OBJECT_VALID AndAlso Me.UserStato = UserStatus.USER_ENABLED
        End Function

        Public Overrides Sub InitializeFrom(value As Object)
            MyBase.InitializeFrom(value)
            Me.m_Uffici = Nothing
            Me.m_Authorizations = Nothing
            Me.m_Settings = Nothing
            Me.m_Groups = Nothing
        End Sub

    End Class


End Class
