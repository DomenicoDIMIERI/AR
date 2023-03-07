Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports DMD.Net.Mime
Imports DMD.Databases
Imports DMD.Net.Mail
Imports DMD.Sistema
Imports DMD.Internals
Imports DMD.Office

Namespace Internals


    ''' <summary>
    ''' Rappresenta la base per il gestore della posta elettronica
    ''' </summary>
    Public Class CMailsClass
        Inherits CGeneralClass(Of MailMessage)

        ''' <summary>
        ''' Evento generato quando viene terminato il download di una nuova email
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event EmailReceived(ByVal sender As Object, ByVal e As EmailEventArg)

        ''' <summary>
        ''' Evento generato quando viene terminato l'invio di una email
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event EmailSent(ByVal sender As Object, ByVal e As EmailEventArg)

        ''' <summary>
        ''' Evento generato quando si verifica un errore in fase di download dei messaggi
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event DownloadException(ByVal sender As Object, ByVal e As DownloadErrorEventArgs)

        Private m_Applications As CMailApplications
        Private m_Accounts As CMailAccounts
        Private m_Folders As CFoldersClass
        Private m_Attachments As CMailAttachments
        Private m_Database As CDBConnection
        Private m_Rules As MailApplicationRules
        Private m_Index As CIndexingService

        Public Sub New()
            MyBase.New("modOfficeEMails", GetType(MailMessageCursor), 0)
            Me.m_Applications = Nothing
            Me.m_Accounts = Nothing
            Me.m_Database = Nothing
            Me.m_Attachments = Nothing
            Me.m_Folders = Nothing
            Me.m_Rules = Nothing
            Me.m_Index = Nothing
        End Sub

        'Protected Overrides Sub Finalize()
        '    Me.m_Accounts = Nothing
        '    Me.m_Database = Nothing
        '    Me.m_Folders = Nothing
        '    Me.m_Rules = Nothing
        '    Me.m_Index = Nothing
        '    MyBase.Finalize()
        'End Sub

        Public ReadOnly Property Index As CIndexingService
            Get
                SyncLock Me
                    If (Me.m_Index Is Nothing) Then
                        Me.m_Index = New CIndexingService(Me.Database)
                        Me.m_Index.WordIndexFolder = System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, "mails\wordindex\")
                        DMD.Sistema.FileSystem.CreateRecursiveFolder(Me.m_Index.WordIndexFolder)
                    End If
                    Return Me.m_Index
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property Applications As CMailApplications
            Get
                SyncLock Me
                    If (Me.m_Applications Is Nothing) Then Me.m_Applications = New CMailApplications
                    Return Me.m_Applications
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property Attachments As CMailAttachments
            Get
                SyncLock Me
                    If (Me.m_Attachments Is Nothing) Then Me.m_Attachments = New CMailAttachments
                    Return Me.m_Attachments
                End SyncLock
            End Get
        End Property



        Public ReadOnly Property Rules As MailApplicationRules
            Get
                SyncLock Me
                    If (Me.m_Rules Is Nothing) Then Me.m_Rules = New MailApplicationRules
                    Return Me.m_Rules
                End SyncLock
            End Get
        End Property


        Protected Function SplitWords(ByVal text As String) As String()
            Dim words() As String = {}
            Dim str As String = UCase(text)
            Dim word As String = ""
            Dim stato As Integer = 0
            For i As Integer = 1 To Len(str)
                Dim ch As String = Mid(str, i, 1)
                Select Case stato
                    Case 0
                        If Char.IsDigit(ch) OrElse Char.IsLetter(ch) Then
                            word &= Strings.OnlyCharsAndNumbers(ch)
                        ElseIf (Arrays.BinarySearch(words, word) < 0) Then
                            words = Arrays.InsertSorted(words, word)
                            word = ""
                        End If
                End Select
            Next
            Return words
        End Function

        Public Function FindMessages(ByVal what As Object, ByVal where As FindMessageWhereEnum, Optional ByVal findExact As Boolean = True) As CCollection(Of MailMessage)
            Dim ret As New CCollection(Of MailMessage)
            Dim flags As FindMessageWhereEnum() = [Enum].GetValues(GetType(FindMessageWhereEnum))
            Dim addedids() As Integer = {}
            Dim cursor As MailMessageCursor

            For Each flag As FindMessageWhereEnum In flags
                Select Case flag
                    Case FindMessageWhereEnum.Body
                        Dim res As CCollection(Of CIndexingService.CResult) = Me.Index.Find(what, -1)
                        If (res.Count > 0) Then
                            Dim addingids = {}
                            For Each r As CIndexingService.CResult In res
                                If Arrays.BinarySearch(addedids, r.OwnerID) < 0 Then
                                    addingids = Arrays.Push(addingids, r.OwnerID)
                                End If
                            Next
                            cursor = New MailMessageCursor()
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                            cursor.ID.ValueIn(addingids)
                            While Not cursor.EOF
                                ret.Add(cursor.Item)
                                addedids = Arrays.InsertSorted(addedids, GetID(cursor.Item))
                                cursor.MoveNext()
                            End While
                            cursor.Dispose()
                        End If
                    Case FindMessageWhereEnum.CCField

                    Case FindMessageWhereEnum.CCNField
                    Case FindMessageWhereEnum.FromField
                    Case FindMessageWhereEnum.SendDate
                    Case FindMessageWhereEnum.Subject
                    Case FindMessageWhereEnum.SubjectOrBody
                    Case FindMessageWhereEnum.ToField
                    Case FindMessageWhereEnum.ToOrCCOrCCn

                End Select
            Next

            Return ret
        End Function




        Protected Overridable Sub OnEmailReceived(ByVal e As EmailEventArg)
            RaiseEvent EmailReceived(Me, e)
        End Sub

        Protected Overridable Sub OnEmailSent(ByVal e As EmailEventArg)
            RaiseEvent EmailSent(Me, e)
        End Sub

        ''' <summary>
        ''' Restituisec o imposta la struttura principale delle cartelle
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Folders As CFoldersClass
            Get
                SyncLock Me
                    If (Me.m_Folders Is Nothing) Then Me.m_Folders = New CFoldersClass
                    Return Me.m_Folders
                End SyncLock
            End Get
        End Property


        Public Property Database As CDBConnection
            Get
                If (Me.m_Database Is Nothing) Then Return DMD.Office.Database
                Return Me.m_Database
            End Get
            Set(value As CDBConnection)
                Me.m_Database = value
            End Set
        End Property

        'Protected Overridable Function OpenDatabase() As CDBConnection
        '    Dim ret As New COleDBConnection

        '    'Assicuriamoci che la cartella esista
        '    DMD.Sistema.FileSystem.CreateRecursiveFolder(Me.WorkingDir)

        '    ret.Path = FileSystem.CombinePath(Me.WorkingDir, "db.mdb")

        '    'Assicuriamoci che il file esista
        '    If (FileSystem.FileExists(ret.Path) = False) Then FileSystem.CopyFile(Sistema.ApplicationContext.MapPath("/templates/maildb.mdb"), ret.Path)

        '    'Apriamo il database
        '    ret.OpenDB()



        '    Return ret
        'End Function

        Private Sub CheckAccountsTable()
            'Accounts
            Dim tbl As DMD.Databases.CDBTable = Me.Database.Tables.GetItemByKey("tbl_EmailAccounts")
            Dim col As DMD.Databases.CDBEntityField = tbl.Fields.GetItemByKey("TimeOut")
            If (col Is Nothing) Then
                col = tbl.Fields.Add("TimeOut", GetType(Int32))
                col.AllowDBNull = True
                col.Create()
            End If
            col = tbl.Fields.GetItemByKey("LastSync")
            If (col Is Nothing) Then
                col = tbl.Fields.Add("LastSync", GetType(Date))
                col.AllowDBNull = True
                col.Create()
            End If
            col = tbl.Fields.GetItemByKey("SMTPCrittografia")
            If (col Is Nothing) Then
                col = tbl.Fields.Add("SMTPCrittografia", GetType(Int32))
                col.AllowDBNull = True
                col.Create()
            End If
            col = tbl.Fields.GetItemByKey("FirmaPerNuoviMessaggi")
            If (col Is Nothing) Then
                col = tbl.Fields.Add("FirmaPerNuoviMessaggi", GetType(String))
                col.Create()
            End If
            col = tbl.Fields.GetItemByKey("FirmaPerRisposte")
            If (col Is Nothing) Then
                col = tbl.Fields.Add("FirmaPerRisposte", GetType(String))
                col.AllowDBNull = True
                col.Create()
            End If

            'Messaggi
            tbl = Me.Database.Tables.GetItemByKey("tbl_EmailMessages")
            col = tbl.Fields.GetItemByKey("DownloadDate")
            If (col Is Nothing) Then
                col = tbl.Fields.Add("DownloadDate", GetType(Date))
                col.AllowDBNull = True
                col.Create()
                Me.Database.ExecuteCommand("CREATE INDEX [idxMessagesDWNLDT] ON [tbl_EmailMessages] ([DownloadDate])")
            End If
            col = tbl.Fields.GetItemByKey("ReadDate")
            If (col Is Nothing) Then
                col = tbl.Fields.Add("ReadDate", GetType(Date))
                col.AllowDBNull = True
                col.Create()
                Me.Database.ExecuteCommand("CREATE INDEX [idxMessagesRDTDT] ON [tbl_EmailMessages] ([ReadDate])")
            End If

            'AlternateView
            tbl = Me.Database.Tables.GetItemByKey("tbl_eMailAlternateViews")
            col = tbl.Fields.GetItemByKey("BaseUri")
            If (col Is Nothing) Then
                col = tbl.Fields.Add("BaseUri", GetType(String))
                col.MaxLength = 255
                col.AllowDBNull = True
                col.Create()
            End If

            col = tbl.Fields.GetItemByKey("TransferEncoding")
            If (col Is Nothing) Then
                col = tbl.Fields.Add("TransferEncoding", GetType(String))
                col.MaxLength = 255
                col.AllowDBNull = True
                col.Create()
            End If

            'Folders
            tbl = Me.Database.Tables.GetItemByKey("tbl_eMailFolders")
            col = tbl.Fields.GetItemByKey("TotalMessages")
            If (col Is Nothing) Then
                col = tbl.Fields.Add("TotalMessages", GetType(Integer))
                col.AllowDBNull = True
                col.Create()
            End If

            col = tbl.Fields.GetItemByKey("TotalUnread")
            If (col Is Nothing) Then
                col = tbl.Fields.Add("TotalUnread", GetType(Integer))
                col.AllowDBNull = True
                col.Create()
            End If

            col = tbl.Fields.GetItemByKey("Flags")
            If (col Is Nothing) Then
                col = tbl.Fields.Add("Flags", GetType(Integer))
                col.AllowDBNull = True
                col.Create()
            End If

            col = tbl.Fields.GetItemByKey("Attributi")
            If (col Is Nothing) Then
                col = tbl.Fields.Add("Attributi", GetType(String))
                col.MaxLength = 0
                col.AllowDBNull = True
                col.Create()
            End If


            tbl = Me.Database.Tables.GetItemByKey("tbl_Index")
            If (tbl Is Nothing) Then
                tbl = Me.Database.Tables.Add("tbl_Index")
                col = tbl.Fields.Add("ID", GetType(Integer))
                col.AutoIncrement = True

                col = tbl.Fields.Add("ObjectType", GetType(String))
                col.MaxLength = 255

                col = tbl.Fields.Add("ObjectID", GetType(Integer))

                col = tbl.Fields.Add("Word", GetType(String))
                col.MaxLength = 255

                col = tbl.Fields.Add("Rank", GetType(Integer))

                tbl.Create()

                Me.Database.ExecuteCommand("ALTER TABLE [tbl_Index] ADD PRIMARY KEY ([ID])")
                Me.Database.ExecuteCommand("CREATE INDEX [idxIndexObjType] ON [tbl_Index] ([ObjectType])")
                Me.Database.ExecuteCommand("CREATE INDEX [idxIndexObjID] ON [tbl_Index] ([ObjectID])")
                Me.Database.ExecuteCommand("CREATE INDEX [idxIndexWord] ON [tbl_Index] ([Word])")
            End If


            tbl = Me.Database.Tables.GetItemByKey("tbl_WordStats")
            If (tbl Is Nothing) Then
                tbl = Me.Database.Tables.Add("tbl_WordStats")
                col = tbl.Fields.Add("ID", GetType(Integer))
                col.AutoIncrement = True

                col = tbl.Fields.Add("Word", GetType(String))
                col.MaxLength = 255

                col = tbl.Fields.Add("Frequenza", GetType(Integer))

                col = tbl.Fields.Add("Indice", GetType(Object))

                tbl.Create()

                Me.Database.ExecuteCommand("ALTER TABLE [tbl_WordStats] ADD PRIMARY KEY ([ID])")
                Me.Database.ExecuteCommand("CREATE INDEX [idxWordStatsWord] ON [tbl_WordStats] ([Word])")
                Me.Database.ExecuteCommand("CREATE INDEX [idxWordStatsFreq] ON [tbl_WordStats] ([Frequenza])")
            End If


        End Sub

        ''' <summary>
        ''' Restituisce la collezione degli accounts configurati per l'applicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Accounts As CMailAccounts
            Get
                SyncLock Me
                    If Me.m_Accounts Is Nothing Then Me.m_Accounts = New CMailAccounts
                    Return Me.m_Accounts
                End SyncLock
            End Get
        End Property

        Public Sub CheckEMails()
            'SyncLock Me
            '    If (Me.m_DownloadWorker Is Nothing) Then Me.m_DownloadWorker = New DownloadWorker(Me, -1)
            '    If (Me.m_DownloadWorker.IsBusy) Then Return
            '    Me.m_DownloadWorker.RunWorkerAsync()
            'End SyncLock
        End Sub

        'Public Function DownloadEmails() As CCollection(Of MailMessage)
        '    Dim ret As New CCollection(Of MailMessage)
        '    For Each a As MailAccount In Me.Accounts
        '        ret.AddRange(Me.DownloadEmails(a, -1))
        '    Next
        '    Return ret
        'End Function

        'Public Function DownloadEmails(ByVal nItems As Integer) As CCollection(Of MailMessage)
        '    Dim ret As New CCollection(Of MailMessage)
        '    If (nItems <= 0) Then Return ret
        '    For Each a As MailAccount In Me.Accounts
        '        ret.AddRange(Me.DownloadEmails(a, nItems - ret.Count))
        '    Next
        '    Return ret
        'End Function


        'Public Function DownloadEmails(ByVal account As MailAccount, ByVal nItems As Integer) As CCollection(Of MailMessage)
        '    Dim ret As New CCollection(Of MailMessage)
        '    If (nItems = 0) Then Return ret

        '    Select Case account.Protocol
        '        Case "POP3"
        '            Dim client As New DMD.Net.Mail.Pop3Client(account.UserName, account.Password, account.ServerName, account.ServerPort, account.UseSSL)
        '            Dim message As DMD.Net.Mail.MailMessageEx
        '            Dim msg As MailMessage
        '            Dim folder As MailFolder = Nothing
        '            client.TimeOut = IIf(account.TimeOut <= 100, 100, account.TimeOut)

        '            If (account.DefaultFolderName <> "") Then folder = Me.GetFolderByName(account.DefaultFolderName)
        '            If (folder Is Nothing) Then folder = Me.Folders.Inbox
        '            'Dim ret As New CCollection(Of MailMessage)
        '            Dim dataInizio As Date = DateUtils.Now
        '            client.Connect()
        '            client.Authenticate()
        '            client.Stat()

        '            'Recuperiamo la data dell'ultimo messaggio scaricato tramite questo account
        '            Dim ultimaData As Date? = account.LastStync

        '            'Scarichiamo i messaggi successivi
        '            Dim list As System.Collections.Generic.List(Of Protocols.POP3.Pop3ListItem) = client.List
        '            Dim toDelete As New CCollection

        '            Dim item As DMD.Net.Mail.Protocols.POP3.Pop3ListItem
        '            For i As Integer = list.Count - 1 To 0 Step -1
        '                item = list(i)
        '                message = client.Top(item.MessageId, 5)
        '                msg = New MailMessage(Me)
        '                msg.Account = account
        '                msg.Folder = folder
        '                msg.Process(message) ' = New MailMessage(Me, message, account.UserName, folder)
        '                'If (Not Me.IsDownloaded(msg)) Then
        '                'If (ultimaData.HasValue = True AndAlso msg.DeliveryDate >= ultimaData.Value) OrElse _
        '                '   ((ultimaData.HasValue = False OrElse (ultimaData.HasValue AndAlso ultimaData = msg.DeliveryDate)) AndAlso Not Me.IsDownloaded(msg)) Then
        '                If (Not Me.IsDownloaded(msg)) Then
        '                    message = client.RetrMailMessageEx(item)
        '                    msg.Process(message)
        '                    msg.Stato = ObjectStatus.OBJECT_VALID
        '                    msg.SetFlag(MailFlags.Unread, True)
        '                    msg.Save()
        '                    ret.Add(msg)
        '                    Me.OnEmailReceived(New EmailEventArg(msg))
        '                End If

        '                If account.DelServerAfterNDays Then
        '                    If msg.DeliveryDate.HasValue AndAlso DateUtils.DateDiff(DateInterval.Day, msg.DeliveryDate.Value, DateUtils.Now) >= account.DelServerAfterDays Then
        '                        toDelete.Add(item)
        '                    End If
        '                End If
        '                If (nItems > 0 AndAlso ret.Count >= nItems) Then Exit For
        '            Next

        '            For i As Integer = toDelete.Count - 1 To 0 Step -1
        '                item = toDelete(i)
        '                client.Dele(item)
        '            Next

        '            client.Disconnect()

        '            account.LastStync = dataInizio
        '            If (account.ID <> 0) Then account.Save()
        '        Case Else
        '            Throw New NotSupportedException("Protocollo e-mail non supportato: [" & account.Protocol & "]")
        '    End Select

        '    Return ret
        'End Function

        Public Function IsDownloaded(ByVal m As MailMessage) As Boolean
            Dim cursor As MailMessageCursor = Nothing
            Try
                cursor = New MailMessageCursor()
                cursor.IgnoreRights = True
                cursor.DeliveryDate.Value = m.DeliveryDate
                cursor.From.Value = _strtodb(m.From.ToString)
                cursor.To.Value = _strtodb(m.To.ToString)
                cursor.Cc.Value = _strtodb(m.Cc.ToString)
                cursor.Bcc.Value = _strtodb(m.Bcc.ToString)
                cursor.Subject.Value = m.Subject

                'cursor.WhereClauses.Add("Len([Body])=" & Len(m.Body))
                Dim ret As Boolean = Not cursor.EOF

                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Private Function _strtodb(ByVal value As String) As Object
            Return value
        End Function

        Public Sub SendReceive()
            'Me.DownloadEmails()
            Me.CheckEMails()
            '  Me.SendEmails()
        End Sub

        'Public Sub SendEmails()
        '    For Each a As MailAccount In Me.Accounts
        '        Me.SendEMails(a)
        '    Next
        'End Sub

        Public Sub SendEMails(ByVal account As MailAccount)

        End Sub



        Public ReadOnly Property Connection As DMD.Databases.CDBConnection
            Get
                Return APPConn
            End Get
        End Property

        'Private Function GetFolderById(ByVal parent As MailFolder, ByVal id As Integer) As MailFolder
        '    Dim f As MailFolder = parent.Childs.GetItemById(id)
        '    If (f Is Nothing) Then
        '        For Each c As MailFolder In parent.Childs
        '            f = GetFolderById(c, id)
        '            If (f IsNot Nothing) Then Exit For
        '        Next
        '    End If
        '    Return f
        'End Function

        'Public Function GetFolderById(ByVal id As Integer) As MailFolder
        '    Dim f As MailFolder = Me.Folders.GetItemById(id)
        '    If (f Is Nothing) Then
        '        For Each c As MailFolder In Me.Folders
        '            f = GetFolderById(c, id)
        '            If (f IsNot Nothing) Then Exit For
        '        Next
        '    End If
        '    Return f
        'End Function

        'Public Function GetFolderByName(ByVal path As String) As MailFolder
        '    Return Me.Folders.GetItemByName(path)
        'End Function


        'Public Function GetMessageById(ByVal id As Integer) As MailMessage
        '    Dim cursor As MailMessageCursor = Nothing
        '    Try
        '        cursor = New MailMessageCursor(Me)
        '        cursor.PageSize = 1
        '        cursor.IgnoreRights = True
        '        cursor.ID.Value = id
        '        Dim ret As MailMessage = cursor.Item
        '        cursor.Dispose()
        '        Return ret
        '    Catch ex As Exception
        '        Sistema.Events.NotifyUnhandledException(ex)
        '        Throw
        '    Finally
        '        If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
        '    End Try
        'End Function


        'Public Overrides Function GetTableName() As String
        '    Return "tbl_eMailApps"
        'End Function

        'Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
        '    Me.m_Name = reader.Read("Name", Me.m_Name)
        '    Me.m_DisplayName = reader.Read("DisplayName", Me.m_DisplayName)
        '    Me.m_WorkingDir = reader.Read("WorkingDir", Me.m_WorkingDir)
        '    Me.m_OwnerID = reader.Read("OwnerID", Me.m_OwnerID)
        '    Me.m_OwnerName = reader.Read("OwnerName", Me.m_OwnerName)
        '    Try
        '        Me.m_Rules = XML.Utils.Serializer.Deserialize(reader.Read("Rules", ""))
        '        Me.m_Rules.SetApplication(Me)
        '    Catch ex As Exception
        '        Me.m_Rules = Nothing
        '    End Try
        '    Return MyBase.LoadFromRecordset(reader)
        'End Function

        'Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
        '    writer.Write("Name", Me.m_Name)
        '    writer.Write("DisplayName", Me.m_DisplayName)
        '    writer.Write("WorkingDir", Me.m_WorkingDir)
        '    writer.Write("OwnerID", Me.OwnerID)
        '    writer.Write("OwnerName", Me.m_OwnerName)
        '    writer.Write("Rules", XML.Utils.Serializer.Serialize(Me.Rules))
        '    Return MyBase.SaveToRecordset(writer)
        'End Function

        'Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
        '    Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
        '    If (ret AndAlso m_Accounts IsNot Nothing) Then m_Accounts.Save(force)
        '    'If (ret AndAlso m_Settings IsNot Nothing) Then dbConn.Save(m_Settings)
        '    If (ret AndAlso m_Folders IsNot Nothing) Then m_Folders.Save(force)
        '    Return ret
        'End Function

        'Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
        '    writer.WriteAttribute("Name", Me.m_Name)
        '    writer.WriteAttribute("DisplayName", Me.m_DisplayName)
        '    writer.WriteAttribute("WorkingDir", Me.m_WorkingDir)
        '    writer.WriteAttribute("OwnerID", Me.OwnerID)
        '    writer.WriteAttribute("OwnerName", Me.m_OwnerName)
        '    MyBase.XMLSerialize(writer)
        '    writer.WriteTag("Rules", Me.Rules)
        'End Sub

        'Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
        '    Select Case fieldName
        '        Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
        '        Case "DisplayName" : Me.m_DisplayName = XML.Utils.Serializer.DeserializeString(fieldValue)
        '        Case "WorkingDir" : Me.m_WorkingDir = XML.Utils.Serializer.DeserializeString(fieldValue)
        '        Case "OwnerID" : Me.m_OwnerID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
        '        Case "OwnerName" : Me.m_OwnerName = XML.Utils.Serializer.DeserializeString(fieldValue)
        '        Case "Rules" : Me.m_Rules = CType(fieldValue, MailApplicationRules) : Me.m_Rules.SetApplication(Me)
        '        Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
        '    End Select
        'End Sub

        ''' <summary>
        ''' Informa l'applicazione che deve ricaricare accounts e folders a causa di cambiamenti esterni
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub NotifyChanges()
            Me.m_Accounts = Nothing
            Me.m_Folders = Nothing

        End Sub



        'Sub Create()
        '    Dim path As String = FileSystem.CombinePath(Me.WorkingDir, "db.mdb")
        '    If (FileSystem.FileExists(path)) Then Throw New InvalidOperationException("Impossibile sovrascrivere il database esistente")
        '    DMD.Sistema.FileSystem.CreateRecursiveFolder(Me.WorkingDir)
        '    FileSystem.CopyFile(Sistema.ApplicationContext.MapPath("/templates/maildb.mdb"), path)
        'End Sub


        Friend Sub doEmailReceived(ByVal e As EmailEventArg)
            Me.Index.Index(e.Message)
            Me.OnEmailReceived(e)
        End Sub

        Friend Sub doDownloadError(ByVal e As DownloadErrorEventArgs)
            Me.OnDownloadError(e)
        End Sub

        Protected Overridable Sub OnDownloadError(ByVal e As DownloadErrorEventArgs)
            RaiseEvent DownloadException(Me, e)
        End Sub


    End Class

End Namespace

Partial Class Office

    <Flags>
    Public Enum FindMessageWhereEnum As Integer
        FromField = 1
        ToField = 2
        CCField = 4
        CCNField = 8
        Subject = 16
        Body = 32
        ToOrCCOrCCn = 2 Or 4 Or 8
        SubjectOrBody = 16 Or 32
        SendDate = 64
    End Enum

    Private Shared m_Mails As CMailsClass = Nothing

    Public Shared ReadOnly Property Mails As CMailsClass
        Get
            If (m_Mails Is Nothing) Then m_Mails = New CMailsClass
            Return m_Mails
        End Get
    End Property

End Class