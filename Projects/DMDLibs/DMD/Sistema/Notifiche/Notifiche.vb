Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Gestione delle notifiche di sistema
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CNotificheClass
        Inherits CGeneralClass(Of Notifica)

        Private m_Database As CDBConnection = Nothing

        ''' <summary>
        ''' Evento generato quando ci sono nuove notifiche per l'utente corrente
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event NuoveNotifiche(ByVal sender As Object, ByVal e As System.EventArgs)



        Friend Sub New()
            MyBase.New("modSYSNotifiche", GetType(NotificaCursor))
        End Sub


        Public Property Database As CDBConnection
            Get
                If (Me.m_Database Is Nothing) Then Return APPConn
                Return Me.m_Database
            End Get
            Set(value As CDBConnection)
                Me.m_Database = value
            End Set
        End Property

        ''' <summary>
        ''' Annulla tutte le notifiche pendenti impostate dall'oggetto specificato
        ''' </summary>
        ''' <param name="toDate">[in] Data fino alla quale annullare le notifiche pendenti. Se NULL vengono annullate tutte le date</param>
        ''' <param name="source">[in] Oggetto che ha generato le notifiche da annullare. Se NULL vengono annullate tutte le notifiche</param>
        ''' <remarks></remarks>
        Public Sub CancelPendingAlertsBySource(ByVal toDate As Nullable(Of Date), ByVal source As Object, ByVal categoria As String)
            CancelPendingAlertsBySource(Sistema.Users.CurrentUser, toDate, source, categoria)
        End Sub



        ''' <summary>
        ''' Annulla tutte le notifiche pendenti impostate dall'oggetto specificato
        ''' </summary>
        ''' <param name="user">[in] Utente per cui annullare le notifiche</param>
        ''' <param name="toDate">[in] Data fino a cui annullare le notifiche (se NULL annulla tutte le notifiche)</param>
        ''' <param name="source">[in] Oggetto che ha generato le notifiche. Se NULL annulla tutte le notifiche</param>
        ''' <remarks></remarks>
        Public Sub CancelPendingAlertsBySource(ByVal user As CUser, ByVal toDate As Nullable(Of Date), ByVal source As Object, ByVal categoria As String)
            Dim cursor As New NotificaCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.StatoNotifica.Value = StatoNotifica.CONSEGNATA
            cursor.StatoNotifica.Operator = OP.OP_LE
            If (categoria <> "") Then cursor.Categoria.Value = categoria
            cursor.IgnoreRights = True
            If (toDate.HasValue) Then
                cursor.Data.Value = DateUtils.DateAdd(DateInterval.Day, 1, DateUtils.GetDatePart(toDate))
                cursor.Data.Operator = OP.OP_LT
            End If
            If (user IsNot Nothing) Then cursor.TargetID.Value = GetID(user)
            If (source IsNot Nothing) Then
                cursor.SourceName.Value = TypeName(source)
                cursor.SourceID.Value = GetID(source)
            End If
            While Not cursor.EOF
                cursor.Item.StatoNotifica = StatoNotifica.ANNULLATA
                cursor.Item.Save()
                cursor.MoveNext()
            End While
            cursor.Dispose()
        End Sub

        ''' <summary>
        ''' Programma un promemoria per l'utente corrente
        ''' </summary>
        ''' <param name="descrione"></param>
        ''' <param name="data"></param>
        ''' <param name="source"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ProgramAlert(ByVal descrione As String, ByVal data As Date, ByVal source As Object, ByVal categoria As String) As Notifica
            Return ProgramAlert(Users.CurrentUser, descrione, data, source, categoria)
        End Function

        ''' <summary>
        ''' Programma un promemoria per l'utente specificato
        ''' </summary>
        ''' <param name="user">[in] Utente per cui programmare il promemoria</param>
        ''' <param name="descrione"></param>
        ''' <param name="data"></param>
        ''' <param name="source"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ProgramAlert(ByVal user As CUser, ByVal descrione As String, ByVal data As Date, ByVal source As Object, ByVal categoria As String) As Notifica
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            If (source Is Nothing) Then Throw New ArgumentNullException("source")
            Dim ret As New Notifica

            If (GetID(user) = GetID(Sistema.Users.KnownUsers.GuestUser)) Then Throw New ArgumentException("Impossibile programmare una notifica per l'utente Guest")
            If (GetID(user) = GetID(Sistema.Users.KnownUsers.SystemUser)) Then Throw New ArgumentException("Impossibile programmare una notifica per l'utente SYSTEM")

            'ret.PuntoOperativo = po
            ret.Target = user
            ret.Descrizione = descrione
            ret.Data = data
            ret.Categoria = categoria
            ret.SourceName = TypeName(source)
            ret.SourceID = GetID(source)
            ret.StatoNotifica = StatoNotifica.NON_CONSEGNATA
            ret.Stato = ObjectStatus.OBJECT_VALID
            ret.Save()
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce la collezione di tutte le notifiche "attive" cioè la cui data è al più oggi e
        ''' </summary>
        ''' <param name="source">[in] Oggetto che ha generato le notifiche (se NULL vengono restituite tutte le notifiche)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPendingAlertsBySource(ByVal source As Object) As CCollection(Of Notifica)
            Return GetPendingAlertsBySource(Users.CurrentUser, source)
        End Function

        ''' <summary>
        ''' Restituisce la collezione di tutte le notifiche "attive" cioè la cui data è al più oggi e
        ''' </summary>
        ''' <param name="source">[in] Oggetto che ha generato le notifiche (se NULL vengono restituite tutte le notifiche)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPendingAlertsBySource(ByVal source As Object, ByVal contesto As String) As CCollection(Of Notifica)
            Return GetPendingAlertsBySource(Users.CurrentUser, source, contesto)
        End Function

        ''' <summary>
        ''' Restituisce la collezione di tutte le notifiche "attive" cioè la cui data è al più oggi e
        ''' </summary>
        ''' <param name="userID">[in] Utente di cui recuperare le notifiche</param>
        ''' <param name="source">[in] Oggetto che ha generato le notifiche (se NULL vengono restituite tutte le notitiche)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPendingAlertsBySource(ByVal userID As Integer, ByVal source As Object) As CCollection(Of Notifica)
            Dim cursor As NotificaCursor = Nothing
            Dim ret As New CCollection(Of Notifica)

            If (userID = 0 OrElse userID = GetID(Sistema.Users.KnownUsers.GuestUser)) Then Return ret

#If Not Debug Then
            Try
#End If
            cursor = New NotificaCursor

            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.StatoNotifica.Value = StatoNotifica.CONSEGNATA
            cursor.StatoNotifica.Operator = OP.OP_LE
            cursor.Data.Value = DateUtils.ToMorrow
            cursor.Data.Operator = OP.OP_LT
            cursor.TargetID.Value = userID
            cursor.IgnoreRights = True
            If (source IsNot Nothing) Then
                cursor.SourceName.Value = TypeName(source)
                cursor.SourceID.Value = GetID(source)
            End If
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While

#If Not Debug Then
            Catch ex As Exception
                Throw
            Finally
#End If
            If (cursor IsNot Nothing) Then cursor.Dispose()
#If Not Debug Then
            End Try
#End If

            Return ret
        End Function

        Public Function GetNotificheNonConsegnate(ByVal userID As Integer, ByVal source As Object) As CCollection(Of Notifica)
            Dim ret As New CCollection(Of Notifica)
            If (userID = 0 OrElse userID = GetID(Sistema.Users.KnownUsers.GuestUser)) Then Return ret

            Dim cursor As New NotificaCursor
            Try
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.StatoNotifica.Value = StatoNotifica.NON_CONSEGNATA
                cursor.TargetID.Value = userID
                cursor.IgnoreRights = True
                If (source IsNot Nothing) Then
                    cursor.SourceName.Value = TypeName(source)
                    cursor.SourceID.Value = GetID(source)
                End If
                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While

                Return ret
            Catch ex As Exception
                Throw
            Finally
                cursor.Dispose()
            End Try
        End Function

        Public Function GetNotificheNonConsegnate(ByVal source As String) As CCollection(Of Notifica)
            Return Me.GetNotificheNonConsegnate(GetID(Users.CurrentUser), source)
        End Function


        ''' <summary>
        ''' Restituisce la collezione di tutte le notifiche "attive" cioè la cui data è al più oggi e
        ''' </summary>
        ''' <param name="user">[in] Utente di cui recuperare le notifiche</param>
        ''' <param name="source">[in] Oggetto che ha generato le notifiche (se NULL vengono restituite tutte le notitiche)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPendingAlertsBySource(ByVal user As CUser, ByVal source As Object) As CCollection(Of Notifica)
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            Return GetPendingAlertsBySource(GetID(user), source)
        End Function

        Public Function CountPendingAlertsBySource(ByVal user As CUser, ByVal source As Object) As Integer
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            Return CountPendingAlertsBySource(GetID(user), source)
        End Function

        ''' <summary>
        ''' Restituisce il numero delle notifiche "attive" cioè la cui data è al più oggi e
        ''' </summary>
        ''' <param name="source">[in] Oggetto che ha generato le notifiche (se NULL vengono restituite tutte le notitiche)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CountPendingAlertsBySource(ByVal source As Object) As Integer
            Return CountPendingAlertsBySource(Sistema.Users.CurrentUser, source)
        End Function

        ''' <summary>
        ''' Restituisce il numero delle notifiche "attive" cioè la cui data è al più oggi e
        ''' </summary>
        ''' <param name="source">[in] Oggetto che ha generato le notifiche (se NULL vengono restituite tutte le notitiche)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CountPendingAlertsBySource(ByVal userID As Integer, ByVal source As Object) As Integer
            If (userID = 0 OrElse userID = GetID(Sistema.Users.KnownUsers.GuestUser)) Then Return 0

            Dim cursor As New NotificaCursor
            Try
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.StatoNotifica.Value = StatoNotifica.CONSEGNATA
                cursor.StatoNotifica.Operator = OP.OP_LE
                cursor.Data.Value = DateUtils.ToMorrow
                cursor.Data.Operator = OP.OP_LT
                cursor.TargetID.Value = userID
                cursor.IgnoreRights = True
                If (source IsNot Nothing) Then
                    cursor.SourceName.Value = TypeName(source)
                    cursor.SourceID.Value = GetID(source)
                End If
                Return cursor.Count
            Catch ex As Exception
                Throw
            Finally
                cursor.Dispose()
            End Try
        End Function

        ''' <summary>
        ''' Restituisce la collezione di tutte le notifiche "attive" cioè la cui data è al più oggi e
        ''' </summary>
        ''' <param name="user">[in] Utente di cui recuperare le notifiche</param>
        ''' <param name="source">[in] Oggetto che ha generato le notifiche (se NULL vengono restituite tutte le notitiche)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPendingAlertsBySource(ByVal user As CUser, ByVal source As Object, ByVal contesto As String) As CCollection(Of Notifica)
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            Dim ret As New CCollection(Of Notifica)
            If (GetID(user) = 0 OrElse GetID(user) = GetID(Sistema.Users.KnownUsers.GuestUser)) Then Return ret
            contesto = Trim(contesto)

            Dim cursor As New NotificaCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.StatoNotifica.Value = StatoNotifica.CONSEGNATA
            cursor.StatoNotifica.Operator = OP.OP_LE
            cursor.Data.Value = DateUtils.ToMorrow
            cursor.Data.Operator = OP.OP_LT
            cursor.TargetID.Value = GetID(user)
            cursor.Context.Value = contesto
            cursor.IgnoreRights = True

            If (source IsNot Nothing) Then
                cursor.SourceName.Value = TypeName(source)
                cursor.SourceID.Value = GetID(source)
            End If
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()

            ''Notifiche inviate agli utenti dei punti operativi a cui appartiene l'utente o a tutti gli utenti
            'cursor.TargetID.Value = 0
            'Dim str As New System.Text.StringBuilder
            'str.Append("0")
            'For Each u As CUfficio In user.Uffici
            '    str.Append(",")
            '    str.Append(GetID(u))
            'Next
            'cursor.WhereClauses.Add("[IDPuntoOperativo] In (" & str.ToString & ")")
            'While Not cursor.EOF
            '    ret.Add(cursor.Item)
            '    cursor.MoveNext()
            'End While
            'cursor.Dispose()


            Return ret
        End Function

        ''' <summary>
        ''' Restituisce la collezione di tutte le notifiche "attive" e non programmate per l'utente
        ''' </summary>
        ''' <param name="source">[in] Oggetto che ha generato le notifiche (se NULL vengono restituite tutte le notitiche)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAlertsBySource(ByVal source As Object, ByVal contesto As String) As CCollection(Of Notifica)
            Dim ret As New CCollection(Of Notifica)
            contesto = Trim(contesto)

            Dim cursor As New NotificaCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.Context.Value = contesto

            If (source IsNot Nothing) Then
                cursor.SourceName.Value = TypeName(source)
                cursor.SourceID.Value = GetID(source)
            End If
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()

            ''Notifiche inviate agli utenti dei punti operativi a cui appartiene l'utente o a tutti gli utenti
            'cursor.TargetID.Value = 0
            'Dim str As New System.Text.StringBuilder
            'str.Append("0")
            'For Each u As CUfficio In user.Uffici
            '    str.Append(",")
            '    str.Append(GetID(u))
            'Next
            'cursor.WhereClauses.Add("[IDPuntoOperativo] In (" & str.ToString & ")")
            'While Not cursor.EOF
            '    ret.Add(cursor.Item)
            '    cursor.MoveNext()
            'End While
            'cursor.Dispose()


            Return ret
        End Function


    End Class

    Private Shared m_Notifiche As CNotificheClass = Nothing

    Public Shared ReadOnly Property Notifiche As CNotificheClass
        Get
            If (m_Notifiche Is Nothing) Then m_Notifiche = New CNotificheClass
            Return m_Notifiche
        End Get
    End Property

End Class