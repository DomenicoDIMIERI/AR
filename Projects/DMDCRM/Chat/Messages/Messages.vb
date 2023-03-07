Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls
Imports DMD.Messenger
Imports DMD.Internals

Namespace Internals


    Public NotInheritable Class CMessagesClass
        Inherits CGeneralClass(Of CMessage)

        Friend Sub New()
            MyBase.New("Messenger", GetType(CMessagesCursor), 0)
        End Sub



        ''' <summary>
        ''' Invia un messaggio al destinatario specificato
        ''' </summary>
        ''' <param name="target">[in] Utente destinatario</param>
        ''' <param name="message">[in] Corpo del messaggio da inviare</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SendMessageToUser(ByVal message As String, ByVal target As CUser) As CMessage
            Dim msg As New CMessage
            msg.Source = Sistema.Users.CurrentUser
            msg.SourceDescription = Sistema.Users.CurrentUser.Nominativo
            msg.Time = Now
            msg.Target = target
            msg.Message = message
            'msg.SourceSession = WebSite.int.Session.ID
            msg.Stato = ObjectStatus.OBJECT_VALID
            msg.Save()
            Return msg
        End Function

        ''' <summary>
        ''' Invia un messaggio a tutti i membri attivi in una stanza
        ''' </summary>
        ''' <param name="room">[in] Stanza</param>
        ''' <param name="message">[in] Corpo del messaggio da inviare</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SendMessageToRoom(ByVal message As String, ByVal room As CChatRoom) As CCollection(Of CMessage)
            Dim ret As New CCollection(Of CMessage)
            Dim t As DateTime = DateUtils.Now
            For Each u As CChatRoomUser In room.GetMembers
                If (u.Stato = ObjectStatus.OBJECT_VALID) Then
                    Dim msg As New CMessage
                    msg.Source = Sistema.Users.CurrentUser
                    msg.SourceDescription = Sistema.Users.CurrentUser.Nominativo
                    msg.Time = t
                    msg.Target = u.User
                    msg.Message = message
                    'msg.SourceSession = WebSite.int.Session.ID
                    msg.Stanza = room
                    msg.Stato = ObjectStatus.OBJECT_VALID
                    msg.Save()
                    ret.Add(msg)
                End If
            Next

            Return ret
        End Function

        ''' <summary>
        ''' Invia un messaggio al destinatario specificato
        ''' </summary>
        ''' <param name="targetID">[in] Utente destinatario</param>
        ''' <param name="message">[in] Corpo del messaggio da inviare</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SendMessagToUsere(ByVal message As String, ByVal targetID As Integer) As CMessage
            Return Me.SendMessageToUser(message, Sistema.Users.GetItemById(targetID))
        End Function

        Public Function CountOnlineUsers() As Integer
            Return Formats.ToInteger(APPConn.ExecuteScalar("SELECT Count(*) FROM [Login] WHERE ([Visible]=True) And ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And [IsLogged]=True"))
        End Function


        ''' <summary>
        ''' Restituisce l'elenco dei messaggi inviati all'utente 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadPrevUserMessages(ByVal fromUserO As CUser, ByVal toUserO As CUser, ByVal fromID As Integer, ByVal nMax As Integer) As CCollection(Of CMessage)
            Dim cursor As CMessagesCursor = Nothing
#If Not DEBUG Then
            Try
#End If
            If (fromUserO Is Nothing) Then Throw New ArgumentNullException("fromUser")
            If (toUserO Is Nothing) Then Throw New ArgumentNullException("toUser")

            Dim ret As New CCollection(Of CMessage)
            Dim fromUser As Integer = GetID(fromUserO)
            Dim toUser As Integer = GetID(toUserO)

            cursor = New CMessagesCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.ID.Value = fromID
            cursor.ID.Operator = OP.OP_LT
            cursor.ID.SortOrder = SortEnum.SORT_DESC
            cursor.IgnoreRights = True
            cursor.IDStanza.Value = 0
            cursor.WhereClauses.Add("[SourceID] In (" & fromUser & ", " & toUser & ") AND [TargetID]  In (" & fromUser & ", " & toUser & ")")

            While Not cursor.EOF AndAlso (ret.Count < nMax)
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
#If Not DEBUG Then
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
#End If

        End Function

        ''' <summary>
        ''' Restituisce l'elenco dei messaggi inviati al gruppo
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadPrevRoomMessages(ByVal user As CUser, ByVal roomO As CChatRoom, ByVal fromID As Integer, ByVal nMax As Integer) As CCollection(Of CMessage)
            Dim cursor As CMessagesCursor = Nothing
#If Not DEBUG Then
            Try
#End If
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            If (roomO Is Nothing) Then Throw New ArgumentNullException("room")

            Dim ret As New CCollection(Of CMessage)
            Dim rID As Integer = GetID(roomO)

            cursor = New CMessagesCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.ID.Value = fromID
            cursor.ID.Operator = OP.OP_LT
            cursor.ID.SortOrder = SortEnum.SORT_DESC
            cursor.IgnoreRights = True
            cursor.IDStanza.Value = rID
            cursor.TargetID.Value = GetID(user)
            While Not cursor.EOF AndAlso (ret.Count < nMax)
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
#If Not DEBUG Then
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
#End If

        End Function

        ''' <summary>
        ''' Restituisce l'elenco dei messaggi inviati all'utente 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadUserMessages(ByVal fromUserO As CUser, ByVal toUserO As CUser, ByVal fromDate As Nullable(Of Date), ByVal toDate As Nullable(Of Date)) As CCollection(Of CMessage)
            Dim cursor As CMessagesCursor = Nothing
#If Not DEBUG Then
            Try
#End If
            If (fromUserO Is Nothing) Then Throw New ArgumentNullException("fromUser")
            If (toUserO Is Nothing) Then Throw New ArgumentNullException("toUser")
            Dim ret As New CCollection(Of CMessage)
            Dim msg As CMessage
            Dim fromUser As Integer = GetID(fromUserO)
            Dim toUser As Integer = GetID(toUserO)

            cursor = New CMessagesCursor
            cursor.WhereClauses.Add("[SourceID] In (" & fromUser & ", " & toUser & ") AND [TargetID]  In (" & fromUser & ", " & toUser & ")")
            'cursor.Time.SortOrder = SortEnum.SORT_ASC
            'cursor.ID.SortOrder = SortEnum.SORT_ASC
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            If (fromDate.HasValue) Then
                cursor.Time.Value = fromDate.Value
                cursor.Time.Operator = OP.OP_GE
                If (toDate.HasValue) Then
                    cursor.Time.Value1 = toDate.Value
                    cursor.Time.Operator = OP.OP_BETWEEN
                End If
            ElseIf toDate.HasValue Then
                cursor.Time.Value = toDate.Value
                cursor.Time.Operator = OP.OP_LE
            End If
            cursor.IDStanza.Value = 0
            cursor.IgnoreRights = True
            While Not cursor.EOF
                msg = cursor.Item
                ret.Add(msg)
                cursor.MoveNext()
            End While
            cursor.Reset1()

            If (ret.Count < 30) Then
                cursor.Time.Clear()
                'cursor.Time.SortOrder = SortEnum.SORT_DESC
                cursor.ID.SortOrder = SortEnum.SORT_DESC
                While (ret.Count < 30 AndAlso Not cursor.EOF)
                    msg = cursor.Item
                    If (ret.GetItemById(GetID(msg)) Is Nothing) Then ret.Add(msg)
                    cursor.MoveNext()
                End While
            End If
            cursor.Dispose() : cursor = Nothing
            Return ret
#If Not DEBUG Then
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
#End If
        End Function

        ''' <summary>
        ''' Restituisce l'elenco dei messaggi inviati al gruppo
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadRoomMessages(ByVal user As CUser, ByVal roomO As CChatRoom, ByVal fromDate As Nullable(Of Date), ByVal toDate As Nullable(Of Date)) As CCollection(Of CMessage)
            Dim cursor As CMessagesCursor = Nothing
#If Not DEBUG Then
            Try
#End If
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            If (roomO Is Nothing) Then Throw New ArgumentNullException("room")
            Dim ret As New CCollection(Of CMessage)
            Dim msg As CMessage
            Dim rid As Integer = GetID(roomO)

            cursor = New CMessagesCursor
            cursor.IDStanza.Value = rid
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.TargetID.Value = GetID(user)
            If (fromDate.HasValue) Then
                cursor.Time.Value = fromDate.Value
                cursor.Time.Operator = OP.OP_GE
                If (toDate.HasValue) Then
                    cursor.Time.Value1 = toDate.Value
                    cursor.Time.Operator = OP.OP_BETWEEN
                End If
            ElseIf toDate.HasValue Then
                cursor.Time.Value = toDate.Value
                cursor.Time.Operator = OP.OP_LE
            End If
            cursor.IgnoreRights = True
            While Not cursor.EOF
                msg = cursor.Item
                ret.Add(msg)
                cursor.MoveNext()
            End While
            cursor.Reset1()

            If (ret.Count < 30) Then
                cursor.Time.Clear()
                'cursor.Time.SortOrder = SortEnum.SORT_DESC
                cursor.ID.SortOrder = SortEnum.SORT_DESC
                While (ret.Count < 30 AndAlso Not cursor.EOF)
                    msg = cursor.Item
                    If (ret.GetItemById(GetID(msg)) Is Nothing) Then ret.Add(msg)
                    cursor.MoveNext()
                End While
            End If
            cursor.Dispose() : cursor = Nothing
            Return ret
#If Not DEBUG Then
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
#End If
        End Function

        Public Function GetNewerOrChangedUserMessages(ByVal newerThan As Integer, fromUserO As CUser, toUserO As CUser, fromDate As Date?, toDate As Date?) As CCollection(Of CMessage)
            Dim ret As New CCollection(Of CMessage)
            Dim cursor As New CMessagesCursor
#If Not DEBUG Then
            Try
#End If
            If (fromUserO Is Nothing) Then Throw New ArgumentNullException("fromUser")
            If (toUserO Is Nothing) Then Throw New ArgumentNullException("toUser")
            Dim fromUser As Integer = GetID(fromUserO)
            Dim toUser As Integer = GetID(toUserO)
            cursor.WhereClauses.Add("[SourceID] In (" & fromUser & ", " & toUser & ") AND [TargetID] In (" & fromUser & ", " & toUser & ")")
            cursor.Time.SortOrder = SortEnum.SORT_ASC
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDStanza.Value = 0
            If (fromDate.HasValue) Then
                cursor.Time.Value = fromDate.Value
                cursor.Time.Operator = OP.OP_GE
                If (toDate.HasValue) Then
                    cursor.Time.Value1 = toDate.Value
                    cursor.Time.Operator = OP.OP_BETWEEN
                End If
            ElseIf toDate.HasValue Then
                cursor.Time.Value = toDate.Value
                cursor.Time.Operator = OP.OP_LE
            End If
            cursor.IgnoreRights = True
            cursor.ID.Value = newerThan
            cursor.ID.Operator = OP.OP_GT
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return ret
#If Not DEBUG Then
            Catch ex As Exception
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
#End If

        End Function


        Public Function GetNewerOrChangedRoomMessages(ByVal newerThan As Integer, ByVal user As CUser, roomO As CChatRoom, fromDate As Date?, toDate As Date?) As CCollection(Of CMessage)
            Dim ret As New CCollection(Of CMessage)
            Dim cursor As New CMessagesCursor
#If Not DEBUG Then
            Try
#End If
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            If (roomO Is Nothing) Then Throw New ArgumentNullException("room")
            Dim rid As Integer = GetID(roomO)
            cursor.IDStanza.Value = rid
            cursor.Time.SortOrder = SortEnum.SORT_ASC
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.TargetID.Value = GetID(user)

            If (fromDate.HasValue) Then
                cursor.Time.Value = fromDate.Value
                cursor.Time.Operator = OP.OP_GE
                If (toDate.HasValue) Then
                    cursor.Time.Value1 = toDate.Value
                    cursor.Time.Operator = OP.OP_BETWEEN
                End If
            ElseIf toDate.HasValue Then
                cursor.Time.Value = toDate.Value
                cursor.Time.Operator = OP.OP_LE
            End If
            cursor.IgnoreRights = True
            cursor.ID.Value = newerThan
            cursor.ID.Operator = OP.OP_GT
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return ret
#If Not DEBUG Then
            Catch ex As Exception
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
#End If

        End Function


        ''' <summary>
        ''' Restituisce un oggetto CCollection di CChatUser contenente tutti gli utenti abilitati a ricevere messaggi
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUsersList() As CCollection(Of CChatUser)
            Dim ret As New CKeyCollection(Of CChatUser)
            Dim users As CCollection(Of CUser) = Sistema.Users.LoadAll
            Dim u As CUser
            Dim item As CChatUser

            For Each u In users
                If (u.UserStato = UserStatus.USER_ENABLED) Then
                    item = New CChatUser
                    item.uID = GetID(u)
                    item.UserName = u.UserName
                    item.IconURL = u.IconURL
                    item.DisplayName = u.Nominativo
                    item.IsOnline = u.IsLogged
                    If (u.LastLogin IsNot Nothing) Then item.UltimoAccesso = u.LastLogin.LogInTime
                    ret.Add("K" & GetID(u), item)
                End If

            Next

            Dim dbSQL As String = "SELECT [SourceID], Count(*) As [NonLetti] FROM [tbl_Messenger] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " And [StatoMessaggio] In (" & StatoMessaggio.NonConsegnato & ", " & StatoMessaggio.NonLetto & ") And [Stanza]<>'' And Not [Stanza] Is Null GROUP BY [SourceID]"
            Dim dbRis As System.Data.IDataReader = Messenger.Database.ExecuteReader(dbSQL)
            While dbRis.Read
                Dim sourceID As Integer = Formats.ToInteger(dbRis("SourceID"))
                item = ret.GetItemByKey("K" & sourceID)
                If (item IsNot Nothing) Then item.MessaggiNonLetti = Formats.ToInteger(dbRis("NonLetti"))
            End While
            dbRis.Dispose()

            Return New CCollection(Of CChatUser)(ret)
        End Function

        Function GetUnreadMessages(ByVal fromDate As Nullable(Of Date), ByVal toDate As Nullable(Of Date), ByVal user As CUser) As Object
            Dim cursor As CMessagesCursor = Nothing
#If Not DEBUG Then
            Try
#End If
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            Dim ret As New CCollection(Of CMessage)
            cursor = New CMessagesCursor
            cursor.TargetID.Value = GetID(user)
            cursor.StatoMessaggio.ValueIn({StatoMessaggio.NonConsegnato, StatoMessaggio.NonLetto})
            cursor.Time.SortOrder = SortEnum.SORT_DESC
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            If (fromDate.HasValue) Then
                cursor.Time.Value = fromDate.Value
                cursor.Time.Operator = OP.OP_GE
                If (toDate.HasValue) Then
                    cursor.Time.Value1 = toDate.Value
                    cursor.Time.Operator = OP.OP_BETWEEN
                End If
            ElseIf toDate.HasValue Then
                cursor.Time.Value = toDate.Value
                cursor.Time.Operator = OP.OP_LE
            End If
            cursor.IgnoreRights = True
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
#If Not DEBUG Then
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

#End If
        End Function


    End Class

End Namespace

Public NotInheritable Class Messenger

    Private Shared m_Messages As CMessagesClass = Nothing
    Private Shared m_Database As CDBConnection = Nothing

    Public Shared ReadOnly Property Messages As CMessagesClass
        Get
            If (m_Messages Is Nothing) Then m_Messages = New CMessagesClass
            Return m_Messages
        End Get
    End Property

    Public Shared Property Database As CDBConnection
        Get
            If (m_Database Is Nothing) Then Return CRM.Database
            Return m_Database
        End Get
        Set(value As CDBConnection)
            m_Database = value
        End Set
    End Property
End Class
