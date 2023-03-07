#Const DEBUGEMALE = True

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD
Imports DMD.XML
Imports DMD.Internals
Imports DMD.Internals.CIndexingService

Partial Class Office

    <Serializable>
    Public Class MailApplication
        Inherits DBObjectPO

        Private m_UserID As Integer
        Private m_User As CUser
        Private m_UserName As String
        Private m_Parameters As CKeyCollection
        Private m_Flags As Integer
        Private m_Rules As MailApplicationRules
        Private m_RootID As Integer
        Private m_Root As MailRootFolder
        Private m_Accounts As MailApplicationAccounts
        Private ReadOnly receiveLock As New Object

        Public Sub New()
            Me.m_UserID = 0
            Me.m_User = Nothing
            Me.m_UserName = ""
            Me.m_Parameters = Nothing
            Me.m_Flags = 0
            Me.m_Rules = Nothing
            Me.m_RootID = 0
            Me.m_Root = Nothing
            Me.m_Accounts = Nothing
        End Sub

        Public ReadOnly Property Accounts As MailApplicationAccounts
            Get
                If (Me.m_Accounts Is Nothing) Then Me.m_Accounts = New MailApplicationAccounts(Me)
                Return Me.m_Accounts
            End Get
        End Property



        Public Property UserID As Integer
            Get
                Return GetID(Me.m_User, Me.m_UserID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.UserID
                If (oldValue = value) Then Return
                Me.m_UserID = value
                Me.m_User = Nothing
                Me.DoChanged("UserID", value, oldValue)
            End Set
        End Property

        Public Property User As CUser
            Get
                If (Me.m_User Is Nothing) Then Me.m_User = Sistema.Users.GetItemById(Me.m_UserID)
                Return Me.m_User
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.User
                If (oldValue Is value) Then Return
                Me.m_User = value
                Me.m_UserID = GetID(value)
                Me.m_UserName = ""
                If (value IsNot Nothing) Then Me.m_UserName = value.Nominativo
                Me.DoChanged("User", value, oldValue)
            End Set
        End Property

        Public Property UserName As String
            Get
                Return Me.m_UserName
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_UserName
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_UserName = value
                Me.DoChanged("UserName", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Parameters As CKeyCollection
            Get
                If (Me.m_Parameters Is Nothing) Then Me.m_Parameters = New CKeyCollection
                Return Me.m_Parameters
            End Get
        End Property

        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Rules As MailApplicationRules
            Get
                If (Me.m_Rules Is Nothing) Then Me.m_Rules = New MailApplicationRules(Me)
                Return Me.m_Rules
            End Get
        End Property

        Public Property RootID As Integer
            Get
                Return GetID(Me.m_Root, Me.m_RootID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.RootID
                If (oldValue = value) Then Return
                Me.m_RootID = value
                Me.m_Root = Nothing
                Me.DoChanged("Root", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Root As MailRootFolder
            Get
                If (Me.m_Root Is Nothing) Then
                    If (Me.m_RootID <> 0) Then
                        Dim cursor As New MailFolderCursor
                        cursor.ID.Value = Me.m_RootID
                        cursor.IgnoreRights = True
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                        Dim dbRis As System.Data.IDataReader = Office.Mails.Database.ExecuteReader(cursor.GetSQL)
                        If dbRis.Read Then
                            Me.m_Root = New MailRootFolder
                            Me.m_Root.SetApplication(Me)
                            Me.m_Root.SetUtente(Me.User)
                            Office.Mails.Database.Load(Me.m_Root, dbRis)
                        End If
                        dbRis.Dispose()
                        dbRis = Nothing
                    End If
                    If (Me.m_Root Is Nothing) Then
                        Me.m_Root = New MailRootFolder
                        Me.m_Root.Stato = ObjectStatus.OBJECT_VALID
                        Me.m_Root.Name = Me.UserName
                        Me.m_Root.SetApplication(Me)
                        Me.m_Root.Utente = Me.User
                        Me.m_Root.Save()
                    End If
                End If
                Me.m_Root.SetApplication(Me)
                Me.m_Root.SetUtente(Me.User)
                Return Me.m_Root
            End Get
        End Property

        Public Function GetFolderById(ByVal id As Integer) As MailFolder
            If (id = 0) Then Return Nothing
            If (id = Me.RootID) Then Return Me.Root
            Dim ret As MailFolder = Me.Root.Childs.GetItemById(id)
            If (ret Is Nothing) Then ret = Me.GetFolderById(Me.Root, id)
            Return ret
        End Function

        Private Function GetFolderById(ByVal owner As MailFolder, ByVal id As Integer) As MailFolder
            Dim ret As MailFolder = Nothing

            For Each f As MailFolder In owner.Childs
                ret = f.Childs.GetItemById(id)
                If (ret IsNot Nothing) Then Exit For
            Next

            Return ret
        End Function

        Public Overrides Function GetModule() As CModule
            Return Office.Mails.Applications.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Mails.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_MailApps"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_UserID = reader.Read("UserID", Me.m_UserID)
            Me.m_UserName = reader.Read("UserName", Me.m_UserName)
            Try
                Me.m_Parameters = XML.Utils.Serializer.Deserialize(reader.Read("Params", ""))
            Catch ex As Exception
                Me.m_Parameters = Nothing
            End Try
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)

            Try
                Me.m_Rules = XML.Utils.Serializer.Deserialize(reader.Read("Rules", ""))
            Catch ex As Exception
                Me.m_Rules = Nothing
            End Try

            Me.m_RootID = reader.Read("RootID", Me.m_RootID)


            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("UserID", Me.UserID)
            writer.Write("UserName", Me.m_UserName)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("RootID", Me.RootID)
            writer.Write("Params", XML.Utils.Serializer.Serialize(Me.Parameters))
            writer.Write("Rules", XML.Utils.Serializer.Serialize(Me.Rules))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XMLWriter)
            writer.WriteAttribute("UserID", Me.UserID)
            writer.WriteAttribute("UserName", Me.m_UserName)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("RootID", Me.RootID)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Params", Me.Parameters)
            writer.WriteTag("Rules", Me.Rules)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "UserID" : Me.m_UserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UserName" : Me.m_UserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "RootID" : Me.m_RootID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Rules" : Me.m_Rules = fieldValue : Me.m_Rules.SetApplication(Me)
                Case "Params" : Me.m_Parameters = fieldValue
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Overrides Function ToString() As String
            Return Me.UserName
        End Function

        Private Function GetDelDateStr(ByVal d As Date) As String
            Return Right("0000" & Year(d), 4) & Right("00" & Month(d), 2) & Right("00" & Day(d), 2) & Right("00" & Hour(d), 2) & Right("00" & Minute(d), 2) & Right("00" & Second(d), 2)
        End Function

        Private Function GetTargetName(ByVal message As DMD.Net.Mail.MailMessageEx, Optional ByVal account As CEmailAccount = Nothing) As String
            Dim path As String = Global.System.IO.Path.Combine(ApplicationContext.SystemDataFolder, "Mail")
            If (account IsNot Nothing) Then path = Global.System.IO.Path.Combine(path, account.AccountName)
            Dim strID As String = Me.GetDelDateStr(message.DeliveryDate) & "_" & message.MessageId
            strID = DMD.Sistema.FileSystem.RemoveSpecialChars(strID)
            Return Global.System.IO.Path.Combine(path, strID & "\message.xml")
        End Function

        Public Function IsNewMessage(ByVal message As DMD.Net.Mail.MailMessageEx, ByVal account As MailAccount) As Boolean
            Dim cursor As MailMessageCursor = Nothing
            Dim ret As Boolean = False

#If Not (DEBUGEMALE And DEBUG) Then
            Try
#End If
            cursor = New MailMessageCursor
            cursor.IgnoreRights = True
            Dim d1 As Date = message.DeliveryDate
            Dim d2 As Date = message.DeliveryDate

            If (d1 <> Nothing) Then d1 = DateUtils.DateAdd(DateInterval.Second, -5, d1)
            d2 = DateUtils.DateAdd(DateInterval.Second, 5, d2)

            cursor.DeliveryDate.Between(d1, d2)
                'cursor.DeliveryDate.Value = message.DeliveryDate
                cursor.From.Value = Trim(message.From.Address)
                If (Trim(message.From.Address) = "") Then cursor.From.IncludeNulls = True
                cursor.ApplicationID.Value = GetID(Me)
                If (account IsNot Nothing) Then cursor.AccountID.Value = GetID(account)
                cursor.MessageID.Value = Trim(message.MessageId)
                If (Trim(message.MessageId) = "") Then cursor.MessageID.IncludeNulls = True
                ' cursor.Subject.Value = message.Subject
                ' If (message.Subject = "") Then cursor.Subject.IncludeNulls = True
                cursor.Stato.Value = ObjectStatus.OBJECT_TEMP
                cursor.Stato.Operator = OP.OP_NE
            ret = cursor.EOF
#If Not (DEBUGEMALE And DEBUG) Then
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
#End If
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
#If Not (DEBUGEMALE And DEBUG) Then
            End Try
#End If

#If DEBUG Then
            If (ret = True) Then
                Dim ret1 As Boolean = Me.IsNewMessageS(message, account)
                If (ret1 = False) Then
                    Debug.Print("Attenzione")
                End If
            End If
            Return ret
#Else
            Return ret
#End If


        End Function

#If DEBUG Then

        Private Function IsNewMessageS(ByVal message As DMD.Net.Mail.MailMessageEx, ByVal account As MailAccount) As Boolean
            Dim cursor As MailMessageCursor = Nothing
            Dim ret As Boolean = False
            Try
                cursor = New MailMessageCursor
                cursor.IgnoreRights = True
                'Dim d1 As Date = DateUtils.DateAdd(DateInterval.Second, -2, message.DeliveryDate)
                'Dim d2 As Date = DateUtils.DateAdd(DateInterval.Second, 2, message.DeliveryDate)
                'cursor.DeliveryDate.Between(d1, d2)
                cursor.DeliveryDate.Value = message.DeliveryDate
                cursor.From.Value = Trim(message.From.Address)
                If (Trim(message.From.Address) = "") Then cursor.From.IncludeNulls = True
                cursor.ApplicationID.Value = GetID(Me)
                If (account IsNot Nothing) Then cursor.AccountID.Value = GetID(account)
                cursor.MessageID.Value = Trim(message.MessageId)
                If (Trim(message.MessageId) = "") Then cursor.MessageID.IncludeNulls = True
                'cursor.Subject.Value = message.Subject
                'If (message.Subject = "") Then cursor.Subject.IncludeNulls = True
                cursor.Stato.Value = ObjectStatus.OBJECT_TEMP
                cursor.Stato.Operator = OP.OP_NE
                ret = cursor.EOF
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

            Return ret
        End Function

#End If

        Public Function SaveMessage(ByVal message As DMD.Net.Mail.MailMessageEx, ByVal account As MailAccount) As MailMessage
            Dim mail As New MailMessage
            mail.SetApplication(Me)
            mail.Account = account
            If (account.DefaultFolder Is Nothing) OrElse (account.DefaultFolder.Stato <> ObjectStatus.OBJECT_VALID) Then
                mail.Folder = Me.Root.Inbox
            Else
                mail.Folder = account.DefaultFolder
            End If
            mail.FromMessage(message)
            mail.Stato = ObjectStatus.OBJECT_VALID
            mail.ForceUser(Me.User)
            mail.Save(True)
            Return mail
        End Function


        Private Function DownloadPOP3(ByVal account As MailAccount, ByVal maxItems As Integer) As CCollection(Of MailMessage)
            Dim client As DMD.Net.Mail.Pop3Client = Nothing
            Dim message As DMD.Net.Mail.MailMessageEx
            Dim list As System.Collections.Generic.List(Of DMD.Net.Mail.Protocols.POP3.Pop3ListItem)
            Dim item As DMD.Net.Mail.Protocols.POP3.Pop3ListItem
            Dim oraInizio As Date = DateUtils.Now

#If Not (DEBUGEMALE And DEBUG) Then
            Try
#End If
            Dim ret As New CCollection(Of MailMessage)
            Dim messagesToDispose As New CCollection(Of DMD.Net.Mail.MailMessageEx)

            client = New DMD.Net.Mail.Pop3Client(account.UserName, account.Password, account.ServerName, account.ServerPort, account.UseSSL)
            client.CustomCerfificateValidation = True 'Not account.val.SMTPValidateCertificate
            client.Connect()
            client.Authenticate()
            client.Stat()

            'Recuperiamo la data dell'ultimo messaggio scaricato tramite questo account
            'Scarichiamo i messaggi successivi
            list = client.List

            For i As Integer = list.Count - 1 To 0 Step -1
                    If (maxItems > 0 AndAlso ret.Count >= maxItems) Then Exit For

                    item = list(i)
                message = client.Top(item.MessageId, 10)

                If (Me.IsNewMessage(message, account)) Then
                    message = client.RetrMailMessageEx(item)

                    Dim mail As MailMessage = Me.SaveMessage(message, account)

                    ret.Add(mail)
                End If

                If (account.DelServerAfterNDays AndAlso DateUtils.DateDiff(DateInterval.Day, message.DeliveryDate, DateUtils.ToDay) > account.DelServerAfterDays) Then
                    client.Dele(item)
                End If
                messagesToDispose.Add(message)


            Next

            'For Each message In messagesToDelete
            '    client.Dele(message)
            'Next

            client.Quit()
            client.Disconnect()
            client = Nothing

            For Each m As DMD.Net.Mail.MailMessageEx In messagesToDispose
                m.Dispose()
            Next

#If Not (DEBUGEMALE And DEBUG) Then
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
#End If
            If (client IsNot Nothing) Then
                Try
                    client.Quit()
                    client.Disconnect()
                    client.Dispose()
                Catch ex As Exception
                    Try
                        Sistema.Events.NotifyUnhandledException(ex)
                    Catch ex1 As Exception

                    End Try
                End Try
                client = Nothing
            End If
#If Not (DEBUGEMALE And DEBUG) Then
            End Try
#Else
            Return ret
#End If

        End Function

        Public Function DownloadEMails(Optional ByVal maxItems As Integer = 0) As CCollection(Of MailMessage)
            SyncLock Me.receiveLock
                Dim ret As New CCollection(Of MailMessage)
                If (maxItems > 0) Then
                    For Each account As MailAccount In Me.Accounts
                        Dim tmp As CCollection(Of MailMessage) = Me.DownloadEMails(account, maxItems)
                        ret.AddRange(tmp)
                        maxItems -= tmp.Count
                        If (maxItems <= 0) Then Exit For
                    Next
                Else
                    For Each account As MailAccount In Me.Accounts
                        Dim tmp As CCollection(Of MailMessage) = Me.DownloadEMails(account)
                        ret.AddRange(tmp)
                    Next
                End If

                Return ret
            End SyncLock
        End Function

        'Private downloadingAccounts() As String = {}

        Public Function DownloadEMails(ByVal account As MailAccount, Optional ByVal maxItems As Integer = 0) As CCollection(Of MailMessage)
            SyncLock Me.receiveLock
                If (account Is Nothing) Then Throw New ArgumentNullException("account")
                '           If (Arrays.BinarySearch(Me.downloadingAccounts, account.AccountName) >= 0) Then Throw New Exception("Già si sta scaricando la posta da " & account.ID)

                Select Case LCase(account.Protocol)
                    Case "pop3" : Return Me.DownloadPOP3(account, maxItems)
                    Case Else
                        Throw New NotSupportedException("Protocol: " & account.Protocol)
                End Select
            End SyncLock
        End Function

        Public Function GetMessageById(ByVal id As Integer) As MailMessage
            Dim m As MailMessage = Office.Mails.GetItemById(id)
            If (m IsNot Nothing) Then m.SetApplication(Me)
            Return m
        End Function

        Public Function FindMessages(ByVal text As String, ByVal maxItems As Integer) As CCollection(Of MailMessage)
            Dim cursor As MailMessageCursor = Nothing
            Try
                Dim ret As New CCollection(Of MailMessage)

                Dim items As CCollection(Of CResult) = Office.Mails.Index.Find(text, Nothing) ' filter.nMax)
                Dim tmp As New System.Collections.ArrayList
                For Each res As CResult In items
                    tmp.Add(res.OwnerID)
                Next
                Dim arr() As Integer = tmp.ToArray(GetType(Integer))
                If (arr.Length > 0) Then
                    cursor = New MailMessageCursor
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.ID.ValueIn(arr)
                    cursor.ApplicationID.Value = GetID(Me)
                    While (Not cursor.EOF) AndAlso (maxItems <= 0 OrElse ret.Count < maxItems)
                        Dim mail As MailMessage = cursor.Item
                        mail.SetApplication(Me)
                        ret.Add(mail)
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

        Public Function ImportMessageFromFile(ByVal fileName As String, ByVal folder As MailFolder, Optional ByVal account As MailAccount = Nothing, Optional ByVal skipExisting As Boolean = True) As MailMessage
            fileName = Trim(fileName) : If (fileName = vbNullString) Then Throw New ArgumentNullException("fileName")
            If (folder Is Nothing) Then Throw New ArgumentNullException("folder")

            Dim stream As System.IO.FileStream = Nothing
            Dim m As DMD.Net.Mail.MailMessageEx = Nothing
            Dim ret As New MailMessage
#If Not DEBUG Then
            Try
#End If
            stream = New System.IO.FileStream(fileName, System.IO.FileMode.Open)
            m = DMD.Net.Mail.MailMessageEx.FromStream(stream)
            ret.SetApplication(Me)
            ret.FromMessage(m)
            If Not skipExisting OrElse (Me.IsNewMessage(m, account)) Then
                ret.Account = account
                ret.Folder = folder
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
            End If

#If Not DEBUG Then
             return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
#End If
            If (m IsNot Nothing) Then Sistema.EMailer.DisposeMessage(m) : m = Nothing
            If (stream IsNot Nothing) Then stream.Dispose() : stream = Nothing
#If Not DEBUG Then
            End Try
#Else
            Return ret
#End If
        End Function

        Public Sub SendMessage(ByVal msg As MailMessage)
            Dim client As SmtpClient = Nothing
            Dim m As DMD.Net.Mail.MailMessageEx = Nothing
#If Not DEBUG Then
            Try
#End If
            If (msg Is Nothing) Then Throw New ArgumentNullException("msg")
            If (msg.Account Is Nothing) Then Throw New ArgumentNullException("account non definito")
            Dim sendAcc As MailAccount = Nothing
            For Each acc As MailAccount In Me.Accounts
                If GetID(msg.Account) = GetID(acc) Then
                    sendAcc = acc
                    Exit For
                End If
            Next
            If (sendAcc Is Nothing) Then Throw New Exception("account non valido")

            Dim addr As String = msg.Account.eMailAddress
            Dim userName As String = msg.Account.SMTPUserName
            Dim userPass As String = msg.Account.SMTPPassword
            If (userName = "") Then
                userName = msg.Account.UserName
                userPass = msg.Account.Password
            End If
            Dim dispName As String = msg.Account.DisplayName
            Dim svrName As String = msg.Account.SMTPServerName
            Dim svrPort As Integer = msg.Account.SMTPPort
            Dim useCryp As SMTPTipoCrittografica = msg.Account.SMTPCrittografia

            client = New SmtpClient(svrName, svrPort)
            client.UseDefaultCredentials = (userName = "")
            client.EnableSsl = useCryp = SMTPTipoCrittografica.SSL
            client.Credentials = New System.Net.NetworkCredential(userName, userPass)


            If (msg.From.Address = "") Then msg.From = New MailAddress(addr, dispName)
            If (msg.Sender.Address = "") Then msg.Sender = New MailAddress(addr, dispName)
            msg.DeliveryDate = DateUtils.Now

            m = New DMD.Net.Mail.MailMessageEx
            m.From = New DMD.Net.Mail.MailAddressEx(msg.From.Address, msg.From.DisplayName)
            m.Sender = New DMD.Net.Mail.MailAddressEx(msg.Sender.Address, msg.Sender.DisplayName)
            m.Subject = msg.Subject
            m.IsBodyHtml = msg.IsBodyHtml
            m.Body = msg.Body
            For Each att As MailAttachment In msg.Attachments
                Dim fileName As String = System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, att.FileName)
                Dim a As New DMD.Net.Mail.AttachmentEx(fileName, New ContentType(att.ContentType))

                m.Attachments.Add(a)
            Next
            For Each madd As MailAddress In msg.To
                m.To.Add(New DMD.Net.Mail.MailAddressEx(madd.Address, madd.DisplayName))
            Next
            For Each madd As MailAddress In msg.Cc
                m.CC.Add(New DMD.Net.Mail.MailAddressEx(madd.Address, madd.DisplayName))
            Next
            For Each madd As MailAddress In msg.Bcc
                m.Bcc.Add(New DMD.Net.Mail.MailAddressEx(madd.Address, madd.DisplayName))
            Next

            client.Send(m)

            msg.Folder = Me.Root.Sent
            msg.Stato = ObjectStatus.OBJECT_VALID
            msg.Save()
#If Not DEBUG Then
            Catch ex As Exception
                Throw
            Finally
#End If
            If (m IsNot Nothing) Then
                For Each a As System.Net.Mail.Attachment In m.Attachments
                    a.Dispose()
                Next
                m.Dispose()
                m = Nothing
            End If
            If (client IsNot Nothing) Then client.Dispose() : client = Nothing
#If Not DEBUG Then
            End Try
#End If

        End Sub

        Protected Friend Sub UpdateFolder(ByVal folder As MailFolder)
            If (GetID(folder) = GetID(Me.Root)) Then
                Me.m_Root = folder
            Else
                Me.Root.UpdateFolder(folder)
            End If
        End Sub

    End Class

End Class