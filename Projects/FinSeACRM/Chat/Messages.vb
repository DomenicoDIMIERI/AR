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
        Public Function SendMessage(ByVal message As String, ByVal target As CUser, ByVal stanza As String) As CMessage
            Dim msg As New CMessage
            msg.Source = Sistema.Users.CurrentUser
            msg.SourceDescription = Sistema.Users.CurrentUser.Nominativo
            msg.Time = Now
            msg.Target = target
            msg.Message = message
            msg.Stanza = stanza
            'msg.SourceSession = WebSite.int.Session.ID
            msg.Stato = ObjectStatus.OBJECT_VALID
            msg.Save()
            Return msg
        End Function

        ''' <summary>
        ''' Invia un messaggio al destinatario specificato
        ''' </summary>
        ''' <param name="targetID">[in] Utente destinatario</param>
        ''' <param name="message">[in] Corpo del messaggio da inviare</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SendMessage(ByVal message As String, ByVal targetID As Integer, ByVal stanza As String) As CMessage
            Return Me.SendMessage(message, Sistema.Users.GetItemById(targetID), stanza)
        End Function

        Public Function CountOnlineUsers() As Integer
            Return Formats.ToInteger(APPConn.ExecuteScalar("SELECT Count(*) FROM [Login] WHERE ([Visible]=True) And ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And [IsLogged]=True"))
        End Function

        ''' <summary>
        ''' Restituisce l'elenco dei messaggi inviati all'utente 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadUserMessages(ByVal fromUser As CUser, ByVal toUser As CUser, ByVal stanza As String, ByVal fromDate As Nullable(Of Date), ByVal toDate As Nullable(Of Date)) As CCollection(Of CMessage)
            Return Me.LoadUserMessages(GetID(fromUser), GetID(toUser), stanza, fromDate, toDate)
        End Function

        ''' <summary>
        ''' Restituisce l'elenco dei messaggi inviati all'utente 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadPrevMessages(ByVal fromUser As Integer, ByVal toUser As Integer, ByVal stanza As String, ByVal fromID As Integer, ByVal nMax As Integer) As CCollection(Of CMessage)
            Dim cursor As CMessagesCursor = Nothing
            Try
                Dim ret As New CCollection(Of CMessage)
                stanza = Strings.Trim(stanza)

                cursor = New CMessagesCursor
                cursor.Stanza.Value = stanza
                cursor.Stanza.IncludeNulls = (stanza = "")
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.ID.Value = fromID
                cursor.ID.Operator = OP.OP_LT
                cursor.ID.SortOrder = SortEnum.SORT_DESC
                cursor.IgnoreRights = True
                cursor.WhereClauses.Add("[SourceID] In (" & fromUser & ", " & toUser & ", NULL) Or [TargetID]  In (" & fromUser & ", " & toUser & ", NULL)")

                While Not cursor.EOF AndAlso (ret.Count < nMax)
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

        End Function

        ''' <summary>
        ''' Restituisce l'elenco dei messaggi inviati all'utente 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadUserMessages(ByVal fromUser As Integer, ByVal toUser As Integer, ByVal stanza As String, ByVal fromDate As Nullable(Of Date), ByVal toDate As Nullable(Of Date)) As CCollection(Of CMessage)
            Dim cursor As CMessagesCursor = Nothing

            Try
                Dim ret As New CCollection(Of CMessage)
                Dim msg As CMessage
                stanza = Strings.Trim(stanza)

                cursor = New CMessagesCursor
                cursor.WhereClauses.Add("[SourceID] In (" & fromUser & ", " & toUser & ", NULL) Or [TargetID]  In (" & fromUser & ", " & toUser & ", NULL)")
                'cursor.Time.SortOrder = SortEnum.SORT_ASC
                'cursor.ID.SortOrder = SortEnum.SORT_ASC
                cursor.Stanza.Value = stanza
                cursor.Stanza.IncludeNulls = (stanza = "")
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

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function


        Public Function GetNewerOrChangedUserMessages(ByVal newerThan As Integer, fromUser As Integer, toUser As Integer, stanza As String, fromDate As Date?, toDate As Date?) As CCollection(Of CMessage)
            Dim ret As New CCollection(Of CMessage)
            Dim cursor As New CMessagesCursor
            stanza = Strings.Trim(stanza)
            Try
                cursor.WhereClauses.Add("[SourceID] In (" & fromUser & ", " & toUser & ", NULL) Or [TargetID]  In (" & fromUser & ", " & toUser & ", NULL)")
                cursor.Time.SortOrder = SortEnum.SORT_ASC
                cursor.Stanza.Value = stanza
                cursor.Stanza.IncludeNulls = (stanza = "")
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
                cursor.ID.Value = newerThan
                cursor.ID.Operator = OP.OP_GT
                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            Catch ex As Exception
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
            Return ret
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
            Dim dbRis As System.Data.IDataReader = CRM.Database.ExecuteReader(dbSQL)
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
            Try
                Dim ret As New CCollection(Of CMessage)

                cursor = New CMessagesCursor
                cursor.TargetID.ValueIn({GetID(user), 0})
                'cursor.TargetID.IncludeNulls = True
                'cursor.ReadTime.Value = Nothing
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

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try


        End Function


    End Class

End Namespace

Public NotInheritable Class Messenger

    Private Shared m_Messages As CMessagesClass = Nothing

    Public Shared ReadOnly Property Messages As CMessagesClass
        Get
            If (m_Messages Is Nothing) Then m_Messages = New CMessagesClass
            Return m_Messages
        End Get
    End Property
End Class
