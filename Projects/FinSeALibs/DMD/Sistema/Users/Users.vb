Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Sistema
Imports DMD.Internals

Namespace Internals





    Public NotInheritable Class CUsersClass
        Inherits CGeneralClass(Of CUser)

        'Private m_Lock As New Object

        Public Event UserLoggedIn(ByVal e As UserLoginEventArgs)
        Public Event UserLoggedOut(ByVal e As UserLogoutEventArgs)
        Public Event UserCreated(ByVal e As UserEventArgs)
        Public Event UserDeleted(ByVal e As UserEventArgs)
        Public Event UserPasswordChanged(ByVal e As UserEventArgs)

        ''' <summary>
        ''' Evento generato quando viene tentato un accesso non autorizzato
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event LoginError(ByVal e As UserLoginException)


        Public ReadOnly MINUSERNAMELEN As Integer = 3


        'Private m_Users As CKeyCollection(Of CUser)


        Friend Sub New()
            MyBase.New("Utenti", GetType(CUserCursor), -1)
        End Sub

        Protected Friend Sub notifyPasswordChanged(ByVal e As UserEventArgs)
            RaiseEvent UserPasswordChanged(e)
        End Sub

        Public Sub Trap()
            If (Sistema.ApplicationContext.CurrentUser IsNot Nothing AndAlso Sistema.ApplicationContext.CurrentUser.UserName = "SYSTEM") Then Exit Sub
            Sistema.ApplicationContext.CurrentUser = Me.KnownUsers.SystemUser
        End Sub

        Public Sub UnTrap()
            If (Sistema.ApplicationContext.CurrentUser Is Nothing OrElse Sistema.ApplicationContext.CurrentUser.UserName <> "SYSTEM") Then Exit Sub
            Sistema.ApplicationContext.CurrentUser = Nothing
        End Sub

        'Private Shared Function LoadUsers() As CKeyCollection(Of CUser)
        '    Dim ret As New CKeyCollection(Of CUser)
        '    'Dim cursor As New CUserCursor
        '    'cursor.Stato.Value = ObjectStatus.OBJECT_VALID
        '    'cursor.Nominativo.SortOrder = SortEnum.SORT_ASC
        '    'cursor.IgnoreRights = True
        '    'While Not cursor.EOF
        '    '    ret.Add(cursor.Item.UserName, cursor.Item)
        '    '    cursor.MoveNext()
        '    'End While
        '    'cursor.Reset()
        '    Dim dbSQL As String = "SELECT * FROM [Login] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " ORDER BY [Nominativo] ASC"
        '    Dim dbCmd As System.Data.IDbCommand = APPConn.GetConnection.CreateCommand()
        '    dbCmd.CommandText = dbSQL
        '    Dim dbRis As System.Data.IDataReader = dbCmd.ExecuteReader
        '    While dbRis.Read
        '        Dim user As New CUser
        '        APPConn.Load(user, dbRis)
        '        ret.Add(user.UserName, user)
        '    End While
        '    dbRis.Dispose()
        '    dbCmd.Dispose()
        '    Return ret
        'End Function

        'Public Shared ReadOnly Property Items As CKeyCollection(Of CUser)
        '    Get
        '        If m_Users Is Nothing Then
        '            m_Users = LoadUsers()
        '            'm_Users.Sorted = True
        '        End If
        '        Return m_Users
        '    End Get
        'End Property

        Public Function CountLoggedUsers() As Integer
            Dim cnt As Integer = 0
            For Each user As CUser In Sistema.Users.LoadAll
                If user.IsLogged Then cnt += 1
            Next
            Return cnt
        End Function



        Public Sub DisconnectAll()
            For Each user As CUser In Sistema.Users.LoadAll
                user.LogOut(LogOutMethods.LOGOUT_TIMEOUT)
            Next
        End Sub





        Public Function CreateUser(ByVal userName As String) As CUser
            Dim item As New CUser
            Call item.SetUserName(userName)
            item.Save()
            Sistema.Users.UpdateCached(item)
            Return item
        End Function

        Public Function GetFirstAvailableUserName() As String
            Return GetAvailableUserName("Utente")
        End Function

        Public Function GetAvailableUserName(ByVal baseName As String) As String
            baseName = Trim(baseName)
            Dim nome As String = baseName
            Dim i As Integer
            Dim t As Boolean = (Me.GetItemByName(nome) Is Nothing)
            i = 1
            While (t)
                nome = baseName & " (" & i & ")"
                t = (Me.GetItemByName(nome) Is Nothing)
                i = i + 1
            End While
            Return nome
        End Function

        Public Function GetValidUserName(ByVal value As String) As String
            Const chars As String = "/-#'""@?,;:"
            Dim ret As String
            Dim i As Integer
            ret = LCase(Trim("" & value))
            For i = 1 To Len(chars)
                ret = Replace(ret, Mid(chars, i, 1), "")
            Next
            i = InStr(ret, " ")
            If i > 0 Then ret = Left(ret, i - 1) & "." & Mid(ret, i + 1)
            ret = Replace(ret, " ", "")
            Return ret
        End Function

        Public Function TranslateLoginErrorCode(ByVal code As Integer) As String
            Dim ret As String
            Select Case code
                Case 0 : ret = "Nessun errore"
                Case -1 : ret = "Nome utente non valido"
                Case -2 : ret = "Password non valida"
                Case -3 : ret = "Nome utente non esiste"
                Case -4 : ret = "Password non corrispondente al nome utente"
                Case -5 : ret = "L'utente è stato disabilitato"
                Case -6 : ret = "L'utente è stato eliminato"
                Case -7 : ret = "L'utente è in attesa di essere attivato"
                Case -8 : ret = "L'utente non ha ancora confermato la sua iscrizione al sito"
                Case -9 : ret = "L'utente è stato sospeso"
                Case Else : ret = "Errore generico sconosciuto"
            End Select
            Return ret
        End Function


        Friend Sub OnLoginError(ByVal e As UserLoginException)
            If (TypeOf (e) Is BadPasswordException) Then
                Me.Module.DispatchEvent(New EventDescription("LogIn_Error", "Tentativo di accesso non autorizzato: " & e.UserName, New String() {e.Message, DirectCast(e, BadPasswordException).BadPassword}))
            Else
                Me.Module.DispatchEvent(New EventDescription("LogIn_Error", "Tentativo di accesso non autorizzato: " & e.UserName, e.Message))
            End If
            RaiseEvent LoginError(e)
        End Sub

        Friend Sub OnUserCreated(ByVal e As UserEventArgs)
            Me.Module.DispatchEvent(New EventDescription("Delete", "[" & e.User.UserName & "] è stato creato", e))
            RaiseEvent UserCreated(e)
        End Sub

        Friend Sub OnUserDeleted(ByVal e As UserEventArgs)
            Me.Module.DispatchEvent(New EventDescription("Delete", "[" & e.User.UserName & "] è stato eliminato", e))
            RaiseEvent UserDeleted(e)
        End Sub

        Friend Sub OnUserLoggedIn(ByVal e As UserLoginEventArgs)
            Me.Module.DispatchEvent(New EventDescription("LogIn", "[" & e.User.UserName & "] ha effettuato l'accesso", e))
            RaiseEvent UserLoggedIn(e)
        End Sub

        Friend Sub OnUserLoggedOut(ByVal e As UserLogoutEventArgs)
            Me.Module.DispatchEvent(New EventDescription("LogOut", "[" & e.User.UserName & "] ha effettuato il logout", e))
            RaiseEvent UserLoggedOut(e)
        End Sub


        ' Effettua il LogIn dell'utente corrente e restituisce un codice
        ' numerico che indica il tipo di errore:
        '      0  	Utente loggato correttamente
        '      -1	Nome utente non valido
        '	   -2	Password non valida
        '      -3	Nome utente non esiste
        '      -4 	Password non valida per il nome utente
        '      -5   L'utente è stato disabilitato
        '	   -6   L'utente è stato eliminato
        '      -7   L'utente è in attesa di essere attivato
        '      -8   L'utente non ha ancora confermato la sua iscrizione al sito
        '      -9   L'utente è stato sospeso
        '      -10  L'account utente è scaduto 
        '      -255 Errore generico sconosciuto
        Public Function LogIn(ByVal userName As String, ByVal password As String, ByVal parameters As CKeyCollection) As CUser
            Dim user As CUser
            Dim e As System.Exception

            userName = Trim(userName)
            user = Me.GetItemByName(userName)
            If user Is Nothing OrElse (user.Stato <> ObjectStatus.OBJECT_VALID) Then
                e = New UserNotFoundException(userName)
                Users.OnLoginError(e)
                Throw e
            End If
            If Not user.CheckPassword(password) Then
                e = New BadPasswordException(userName, "Password non valida", password)
                Users.OnLoginError(e)
                Throw e
            End If

            Select Case user.UserStato
                Case UserStatus.USER_DISABLED,
                     UserStatus.USER_DELETED,
                     UserStatus.USER_NEW,
                     UserStatus.USER_SUSPENDED
                    e = New UserNotEnabledException(userName, user.UserStato)
                    Users.OnLoginError(e)
                    Throw e
                Case Else
                    If user.IsExpired() Then
                        e = New UserExpiredException(userName)
                        Sistema.Users.OnLoginError(e)
                        Throw e
                    ElseIf (Testflag(user.Flags, UserFlags.ForceChangePassword)) Then
                        e = New UserForcePwdPasswordException(userName)
                        Sistema.Users.OnLoginError(e)
                        Throw e
                    ElseIf (user.PasswordExpire.HasValue AndAlso Calendar.Compare(user.PasswordExpire, Calendar.Now) < 0) Then
                        e = New PasswordExpiredException(userName)
                        Sistema.Users.OnLoginError(e)
                        Throw e
                    Else
                        user.LogIn(password, parameters)
                        OnUserLoggedIn(New UserLoginEventArgs(user))
                    End If
            End Select

            Return user
        End Function

        Public Sub LogOut(ByVal user As CUser, Optional ByVal logoutMethod As LogOutMethods = LogOutMethods.LOGOUT_LOGOUT)
            If (GetID(user) = 0) Then Exit Sub
            If (GetID(user) <> GetID(Users.CurrentUser)) And Not (Users.CurrentUser Is KnownUsers.SystemUser) Then
                'Events.DispatchEvent(Sistema.Users.Module, "LogOut_Error", user.UserName, "L'utente [" & Users.CurrentUser.UserName & "] non è autorizzato a disconnettere [" & user.UserName & "]")
                Throw New InvalidOperationException
            End If
            user.LogOut(logoutMethod)
        End Sub

        ''' <summary>
        ''' Restituisce un oggetto CCurrentUser che rappresenta l'utente corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CurrentUser() As CUser
            Get
                If DMD.Sistema.ApplicationContext Is Nothing Then Return Nothing
                Return DMD.Sistema.ApplicationContext.CurrentUser
            End Get
            Set(value As CUser)
                DMD.Sistema.ApplicationContext.CurrentUser = value
            End Set
        End Property



        Public Function GetItemByName(ByVal userName As String) As CUser
            userName = Trim(userName)
            If (userName = "") Then Return Nothing
            Dim items As CCollection(Of CUser) = Me.LoadAll
            For Each u As CUser In items
                If (Strings.Compare(u.UserName, userName, CompareMethod.Text) = 0) Then Return u
            Next
            Return Nothing
        End Function

        Public Function GetItemDisplayByName(ByVal value As String) As CUser
            value = LCase(Trim(value))
            If (value = "") Then Return Nothing
            Dim items As CCollection(Of CUser) = Me.LoadAll
            For Each u As CUser In items
                If Strings.Compare(u.Nominativo, value) = 0 Then Return u
            Next
            Return Nothing
        End Function

        ''' <summary>
        ''' Restituisce un oggetto CLoginHistory in base al suo ID
        ''' </summary>
        ''' <param name="ID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetLoginHistoryById(ByVal ID As Integer) As CLoginHistory
            Dim ret As CLoginHistory
            Dim cursor As New CLoginHistoryCursor
            Try
                cursor.PageSize = 1
                cursor.IgnoreRights = True
                cursor.ID.Value = ID
                ret = cursor.Item
            Catch ex As Exception
                Throw
            Finally
                cursor.Dispose()
            End Try
            Return ret
        End Function

        Public Function GetStatusText(ByVal value As UserStatus) As String
            Dim ret As String
            Select Case value
                'Case UserStatus.USER_TEMP : ret = "Temporaneo"
                Case UserStatus.USER_ENABLED : ret = "Abilitato"
                Case UserStatus.USER_DISABLED : ret = "Disabilitato"
                Case UserStatus.USER_DELETED : ret = "Eliminato"
                Case UserStatus.USER_NEW : ret = "Da Confermare"
                Case UserStatus.USER_SUSPENDED : ret = "Sospeso"
                Case Else : ret = "Unknown"
            End Select
            Return ret
        End Function

        Public Function GetLogoutMethodText(ByVal value As LogOutMethods) As String
            Dim ret As String
            Select Case value
                'Case LOGOUT_UNKNOWN : ret = "Unknown"
                Case LogOutMethods.LOGOUT_LOGOUT : ret = "Logout"
                Case LogOutMethods.LOGOUT_TIMEOUT : ret = "Timeout"
                Case LogOutMethods.LOGOUT_REMOTEDISCONNECT : ret = "Remote Disconnect"
                Case Else : ret = "Unknown"
            End Select
            Return ret
        End Function

        Public Sub RefreshSessionTimes()
            Dim db As CDBConnection = Me.GetConnection
            If (db.IsRemote) Then
                RPC.InvokeMethod("/widgets/websvc/Utenti.aspx?_a=RefreshSessionTimes")
            Else
                Dim dbSQL As String
                Dim uID As Integer = GetID(Sistema.Users.CurrentUser)
                dbSQL = "UPDATE [Login]  SET [IsLogged]=TRUE WHERE [ID]=" & uID & ";"
                APPConn.ExecuteCommand(dbSQL)
                dbSQL = "UPDATE [tbl_LoginHistory] SET [LogOutTime]=" & DBUtils.DBDate(Now) & " WHERE [ID]=" & uID & ";"
                LOGConn.ExecuteCommand(dbSQL)
            End If

        End Sub



        ''' <summary>
        ''' Ricarica le variabili di sistema
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Reset()
            KnownUsers.Reset()
        End Sub

        Public Function GetUtentiAsCollection() As CCollection(Of CUser)
            Return Me.LoadAll
        End Function

        Private m_KnownUsers As CKnownUsersClass = Nothing

        Public ReadOnly Property KnownUsers As CKnownUsersClass
            Get
                SyncLock Me
                    If (Me.m_KnownUsers Is Nothing) Then Me.m_KnownUsers = New CKnownUsersClass
                    Return Me.m_KnownUsers
                End SyncLock
            End Get
        End Property



    End Class
End Namespace

Partial Public Class Sistema





    Private Shared m_Users As CUsersClass = Nothing

    Public Shared ReadOnly Property Users As CUsersClass
        Get
            SyncLock Sistema.lock
                If (m_Users Is Nothing) Then m_Users = New CUsersClass
                Return m_Users
            End SyncLock
        End Get
    End Property

End Class
