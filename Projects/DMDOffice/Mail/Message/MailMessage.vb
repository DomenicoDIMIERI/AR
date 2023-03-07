Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Net.Mail
Imports DMD
Imports System.IO
Imports System.Collections.Specialized

Partial Class Office

    <Flags>
    Public Enum MailFlags As Integer
        NotSet = 0

        ''' <summary>
        ''' Il messaggio non è ancora stato letto
        ''' </summary>
        ''' <remarks></remarks>
        Unread = 1

        ''' <summary>
        ''' Se vero indica che la mail contiene degli allegati
        ''' </summary>
        Attachments = 2
    End Enum

    <Serializable>
    Public Class MailMessage
        Inherits DBObjectPO
        Implements IIndexable

        'Public Const EmailRegexPattern As String = "(['""]{1,}.+['""]{1,}\s+)?<?[\w\.\-]+@[^\.][\w\.\-]+\.[a-z]{2,}>?"

        Private m_ApplicationID As Integer
        Private m_Application As MailApplication

        Private m_FolderID As Integer
        Private m_Folder As MailFolder

        Private m_AccountID As Integer
        Private m_AccountName As String     'Account utilizzato per scaricare il messaggio
        Private m_Account As MailAccount

        'Private m_Message1 As DMD.Net.Mail.MailMessageEx   'Messaggio
        Friend m_Attachements As MailAttachmentCollection
        Private m_Flags As MailFlags
        Private m_Categoria As String
        Private m_Headers As CKeyCollection(Of String)
        Private m_From As MailAddress
        Private m_Sender As MailAddress
        Private m_DeliveredTo As MailAddress
        Private m_ReplyTo As MailAddress
        Private m_To As MailAddressCollection
        Private m_Cc As MailAddressCollection
        Private m_Bcc As MailAddressCollection
        Private m_Subject As String
        Private m_Body As String

        Private m_DeliveryDate As Date?         'Data di consegna
        Private m_DownloadDate As Date?         'Data di download
        Private m_ReadDate As Date?             'Data di lettura

        Private m_MessageId As String
        Private m_ReplyToMessageId As String = ""


        Private m_IsBodyHtml As Boolean
        Private m_DeliveryNotificationOptions As System.Net.Mail.DeliveryNotificationOptions
        Private m_Priority As System.Net.Mail.MailPriority

        Friend m_OriginalAddresses As System.Collections.ArrayList

        Public Sub New()
            Me.m_Application = Nothing
            Me.m_ApplicationID = 0
            Me.m_FolderID = 0
            Me.m_Folder = Nothing
            Me.m_AccountID = 0
            Me.m_AccountName = ""
            Me.m_Account = Nothing
            Me.m_Categoria = ""
            Me.m_Attachements = Nothing
            Me.m_Flags = MailFlags.NotSet
            Me.m_Headers = Nothing
            Me.m_From = Nothing ' New MailAddress(Me, "from")
            Me.m_DeliveredTo = Nothing ' New MailAddress(Me, "delivered-to")
            Me.m_Sender = Nothing ' New MailAddress(Me, "sender")
            Me.m_ReplyTo = Nothing ' New MailAddress(Me, "reply-to")
            Me.m_To = Nothing
            Me.m_Cc = Nothing
            Me.m_Bcc = Nothing
            Me.m_Subject = ""
            Me.m_Body = vbNullString
            Me.m_DeliveryDate = Nothing
            Me.m_MessageId = ""
            Me.m_IsBodyHtml = True
            Me.m_DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.None
            Me.m_Priority = MailPriority.Normal
            'Me.m_Object = Nothing
            Me.m_DownloadDate = Nothing
            Me.m_ReadDate = Nothing
            Me.m_OriginalAddresses = Nothing
        End Sub

        Private Function getDeliveredTo(ByVal stri As String) As String
            Dim p As Integer = stri.IndexOf(",")
            If (p > 0) Then
                Return stri.Substring(0, p)
            Else
                Return stri
            End If
        End Function

        Protected Friend Sub FromMessage(ByVal msg As DMD.Net.Mail.MailMessageEx)
            Me.Stato = ObjectStatus.OBJECT_TEMP
            'Me.Save()

            Me.m_Headers = New CKeyCollection(Of String)
            Dim keys() As String = msg.Headers.AllKeys.Clone
            Dim deliveredTo As String = ""
            For Each k As String In keys
                Me.m_Headers.Add(k, msg.Headers.Get(k))
                If (LCase(k) = "delivered-to") Then
                    deliveredTo = Me.getDeliveredTo(msg.Headers.Get(k))
                End If
            Next

            Me.Attachments.Clear()
            For Each at As System.Net.Mail.Attachment In msg.Attachments
                Dim at1 As New MailAttachment
                at1.SetApplication(Me.Application)
                at1.SetMessage(Me)
                at1.From(at)
                at1.Stato = ObjectStatus.OBJECT_VALID
                Me.Attachments.Add(at1)
            Next

            Me.m_OriginalAddresses = New System.Collections.ArrayList

            Me.m_Flags = MailFlags.Unread
            Me.From = Me.ParseAddress(msg.From, "from")
            Me.ReplyTo = Me.ParseAddress(msg.ReplyTo, "reply-to")
            Me.DeliveredTo = Me.ParseAddress(deliveredTo, "delivered-to")
            'Me.Sender  = Me.ParseAddress(deliveredTo, "delivered-to")

            Me.m_To = Me.ParseAddressCollection(msg.To, "to")
            Me.m_Cc = Me.ParseAddressCollection(msg.CC, "cc")
            Me.m_Bcc = Me.ParseAddressCollection(msg.Bcc, "bcc")
            Me.m_Subject = msg.Subject
            Me.m_Body = msg.Body
            Me.m_DeliveryDate = msg.DeliveryDate
            Me.m_MessageId = msg.MessageId
            Me.m_IsBodyHtml = msg.IsBodyHtml
            Me.m_DeliveryNotificationOptions = msg.DeliveryNotificationOptions
            Me.m_Priority = msg.Priority
            Me.m_DownloadDate = DateUtils.Now
            Me.m_ReadDate = Nothing
            SetFlag(MailFlags.Attachments, (msg.Attachments.Count > 0))
            Me.SetChanged(True)
        End Sub



        Private Function ParseAddress(ByVal addr As DMD.Net.Mail.MailAddressEx, ByVal fieldName As String) As MailAddress
            Dim ret As New MailAddress
            ret.FieldName = fieldName
            If (addr IsNot Nothing) Then
                ret.Address = addr.Address
                ret.DisplayName = addr.DisplayName
            End If
            ret.SetApplication(Me.Application)
            ret.SetMessage(Me)
            Return ret
        End Function

        Private Function ParseAddress(ByVal addr As String, ByVal fieldName As String) As MailAddress
            Dim ret As New MailAddress
            ret.FieldName = fieldName
            ret.Address = addr
            ret.DisplayName = ""
            ret.SetApplication(Me.Application)
            ret.SetMessage(Me)
            Return ret
        End Function

        Private Function ParseAddressCollection(ByVal col As MailAddressCollectionEx, ByVal fieldName As String) As MailAddressCollection
            Dim ret As New MailAddressCollection()
            ret.SetMessage(Me)
            ret.SetFieldName(fieldName)
            For Each a As MailAddressEx In col
                Dim ad As MailAddress = Me.ParseAddress(a, fieldName)
                If (ad IsNot Nothing) Then ret.Add(ad)
            Next
            Return ret
        End Function

        Public Property ApplicationID As Integer
            Get
                Return GetID(Me.m_Application, Me.m_ApplicationID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ApplicationID
                If (oldValue = value) Then Return
                Me.m_Application = Nothing
                Me.m_ApplicationID = value
                Me.DoChanged("ApplicationID", value, oldValue)
            End Set
        End Property

        Public Property Application As MailApplication
            Get
                If (Me.m_Application Is Nothing) Then Me.m_Application = Office.Mails.Applications.GetItemById(Me.m_ApplicationID)
                Return Me.m_Application
            End Get
            Set(value As MailApplication)
                Dim oldValue As MailApplication = Me.Application
                If (oldValue Is value) Then Return
                Me.m_Application = value
                Me.m_ApplicationID = GetID(value)
                Me.DoChanged("Application", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetApplication(ByVal value As MailApplication)
            Me.m_Application = value
            Me.m_ApplicationID = GetID(value)
        End Sub

        ''' <summary>
        ''' Restitusice o imposta il percorso in cui il messaggio è stato archiviato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Folder As MailFolder
            Get
                If (Me.m_Folder Is Nothing AndAlso Me.Application IsNot Nothing) Then Me.m_Folder = Me.Application.GetFolderById(Me.m_FolderID)
                If (Me.m_Folder Is Nothing) Then Me.m_Folder = Office.Mails.Folders.GetItemById(Me.m_FolderID)
                Return Me.m_Folder
            End Get
            Set(value As MailFolder)
                Dim oldValue As MailFolder = Me.m_Folder
                If (oldValue Is value) Then Exit Property
                Me.m_Folder = value
                Me.m_FolderID = GetID(value)
                Me.DoChanged("Folder", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetFolder(ByVal value As MailFolder)
            Me.m_Folder = value
            Me.m_FolderID = GetID(value)
        End Sub

        Public Property FolderID As Integer
            Get
                Return GetID(Me.m_Folder, Me.m_FolderID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.FolderID
                If (oldValue = value) Then Exit Property
                Me.m_FolderID = value
                Me.m_Folder = Nothing
                Me.DoChanged("FolderID", value, oldValue)
            End Set
        End Property

        Public Property AccountID As Integer
            Get
                Return GetID(Me.m_Account, Me.m_AccountID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.AccountID
                If (oldValue = value) Then Return
                Me.m_AccountID = value
                Me.m_Account = Nothing
                Me.DoChanged("AccountID", value, oldValue)
            End Set
        End Property

        Public Property Account As MailAccount
            Get
                If (Me.m_Account Is Nothing AndAlso Me.Application IsNot Nothing) Then Me.m_Account = Me.m_Application.Accounts.GetItemById(Me.m_AccountID)
                If (Me.m_Account Is Nothing) Then Me.m_Account = Office.Mails.Accounts.GetItemById(Me.m_AccountID)
                Return Me.m_Account
            End Get
            Friend Set(value As MailAccount)
                Dim oldValue As MailAccount = Me.m_Account
                If (oldValue Is value) Then Exit Property
                Me.m_Account = value
                Me.m_AccountID = GetID(value)
                Me.m_AccountName = ""
                If (value IsNot Nothing) Then Me.m_AccountName = value.AccountName
                Me.DoChanged("Account", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetAccount(ByVal value As MailAccount)
            Me.m_Account = value
            Me.m_AccountID = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome dell'account utilizzate per scaricare il messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AccountName As String
            Get
                If (Me.m_Account IsNot Nothing) Then Return Me.m_Account.AccountName
                Return Me.m_AccountName
            End Get
            Friend Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_AccountName
                If (oldValue = value) Then Exit Property
                Me.m_AccountName = value
                Me.m_Account = Nothing
                Me.DoChanged("Account", value, oldValue)
            End Set
        End Property






        ''' <summary>
        ''' Gets the delivery date.
        ''' </summary>
        ''' <value>The delivery date.</value>
        Public Property DeliveryDate As Nullable(Of Date)
            Get
                Return Me.m_DeliveryDate
            End Get
            Friend Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DeliveryDate
                If (oldValue = value) Then Exit Property
                Me.m_DeliveryDate = value
                Me.DoChanged("DeliveryDate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui il messaggio è stato scaricato dal server
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DownloadDate As Date?
            Get
                Return Me.m_DownloadDate
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DownloadDate
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DownloadDate = value
                Me.DoChanged("DownloadDate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui il messaggio è stato marcato come letto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ReadDate As Date?
            Get
                Return Me.m_ReadDate
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_ReadDate
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_ReadDate = value
                Me.DoChanged("ReadDate", value, oldValue)
            End Set
        End Property



        ''' <summary>
        ''' Gets the message id.
        ''' </summary>
        ''' <value>The message id.</value>
        Public Property MessageId As String
            Get
                Return Me.m_MessageId
            End Get
            Friend Set(value As String)
                Dim oldValue As String = Me.m_MessageId
                If (oldValue = value) Then Exit Property
                Me.m_MessageId = value
                Me.DoChanged("MessageId", value, oldValue)
            End Set
        End Property

        Public Property ReplyToMessageId As String
            Get
                Return Me.m_ReplyToMessageId
            End Get
            Friend Set(value As String)
                Dim oldValue As String = Me.m_ReplyToMessageId
                If (oldValue = value) Then Exit Property
                Me.m_ReplyToMessageId = value
                Me.DoChanged("ReplyToMessageId", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Gets the MIME version.
        ''' </summary>
        ''' <value>The MIME version.</value>
        Public ReadOnly Property MimeVersion As String
            Get
                Return Me.GetHeader(MimeHeaders.MimeVersion)
            End Get
        End Property

        ''' <summary>
        ''' Gets the content id.
        ''' </summary>
        ''' <value>The content id.</value>
        Public ReadOnly Property ContentId As String
            Get
                Return Me.GetHeader(MimeHeaders.ContentId)
            End Get
        End Property

        ''' <summary>
        ''' Gets the content description.
        ''' </summary>
        ''' <value>The content description.</value>
        Public ReadOnly Property ContentDescription As String
            Get
                Return Me.GetHeader(MimeHeaders.ContentDescription)
            End Get
        End Property

        ''' <summary>
        ''' Gets the content disposition.
        ''' </summary>
        ''' <value>The content disposition.</value>
        Public ReadOnly Property ContentDisposition As ContentDisposition
            Get
                Dim contentDisposition1 As String = Me.GetHeader(MimeHeaders.ContentDisposition)
                If (String.IsNullOrEmpty(contentDisposition1)) Then Return Nothing
                Return New ContentDisposition(contentDisposition1)
            End Get
        End Property

        ''' <summary>
        ''' Gets the type of the content.
        ''' </summary>
        ''' <value>The type of the content.</value>
        Public ReadOnly Property ContentType As ContentType
            Get
                Dim contentType1 As String = GetHeader(MimeHeaders.ContentType)
                If (String.IsNullOrEmpty(contentType1)) Then Return Nothing
                Return MimeReader.GetContentType(contentType1)
            End Get
        End Property

        ''' <summary>
        ''' Gets the header.
        ''' </summary>
        ''' <param name="header">The header.</param>
        ''' <returns></returns>
        Private Function GetHeader(ByVal header As String) As String
            Return Me.GetHeader(header, False)
        End Function

        Private Function GetHeader(ByVal header As String, ByVal stripBrackets As Boolean) As String
            If (stripBrackets) Then Return MimeEntity.TrimBrackets(Me.Headers(header))
            Return Me.Headers(header)
        End Function



        ''' <summary>
        ''' Restituisce la collezione degli allegati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Attachments As MailAttachmentCollection
            Get
                If (Me.m_Attachements Is Nothing) Then
                    Me.m_Attachements = New MailAttachmentCollection(Me) ', Me.m_Message.Attachments)
                    Me.m_Attachements.Load()
                End If
                Return Me.m_Attachements
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il corpo del messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Body As String
            Get
                If (Me.m_Body = vbNullString) Then
                    Try
                        Dim path As String = "mail\app\" & Me.ApplicationID
                        path = System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, path)
                        'FileSystem.CreateRecursiveFolder(path)
                        path = path & "\msg" & Me.ID & ".dat"
                        Me.m_Body = System.IO.File.ReadAllText(path)
                    Catch ex As Exception
                        Sistema.Events.NotifyUnhandledException(ex)
                    End Try
                End If
                Return Me.m_Body
            End Get
            Set(value As String)
                Dim oldValue As String = Me.Body
                If (oldValue = value) Then Exit Property
                Me.m_Body = value
                Me.DoChanged("Body", value, oldValue)
            End Set
        End Property



        ''' <summary>
        ''' Restituisce la collezione degli indirizzi destinatari in Copia Carbone
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Cc As MailAddressCollection
            Get
                If (Me.m_Cc Is Nothing) Then
                    Me.checkAddressies()
                    Me.m_Cc = New MailAddressCollection(Me, "cc")
                End If
                Return Me.m_Cc
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione degli indirizzi destinatari in Copia Carbone Nascosta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Bcc As MailAddressCollection
            Get
                If (Me.m_Bcc Is Nothing) Then
                    Me.checkAddressies()
                    Me.m_Bcc = New MailAddressCollection(Me, "bcc")
                End If
                Return Me.m_Bcc
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore che indica se richiedere o meno la conferma di consegna
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DeliveryNotificationOptions As System.Net.Mail.DeliveryNotificationOptions
            Get
                Return Me.m_DeliveryNotificationOptions
            End Get
            Set(value As System.Net.Mail.DeliveryNotificationOptions)
                Dim oldValue As System.Net.Mail.DeliveryNotificationOptions = Me.m_DeliveryNotificationOptions
                If (oldValue = value) Then Exit Property
                Me.m_DeliveryNotificationOptions = value
                Me.DoChanged("DeliveryNotificationOptions", value, oldValue)
            End Set
        End Property

        Private Sub checkAddressies()
            If (GetID(Me) <> 0) AndAlso (Me.m_OriginalAddresses Is Nothing) Then
                Me.SetOriginalAddressList(Me.GetOriginalAdressies)
            End If
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo del mittente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property From As MailAddress
            Get
                If (Me.m_From Is Nothing) Then Me.checkAddressies()
                If (Me.m_From Is Nothing) Then Me.m_From = New MailAddress(Me, "from")
                Return Me.m_From
            End Get
            Set(value As MailAddress)
                Dim oldValue As MailAddress = Me.From
                If (value Is Nothing) Then value = New MailAddress
                If (oldValue.Equals(value)) Then Exit Property
                Me.m_From.Address = value.Address
                Me.m_From.DisplayName = value.DisplayName
                Me.DoChanged("From", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Headers As CKeyCollection(Of String)
            Get
                If (Me.m_Headers Is Nothing) Then Me.m_Headers = New CKeyCollection(Of String)
                Return Me.m_Headers
            End Get
        End Property

        Public Property IsBodyHtml As Boolean
            Get
                Return Me.m_IsBodyHtml
            End Get
            Set(value As Boolean)
                If (Me.IsBodyHtml = value) Then Exit Property
                Me.m_IsBodyHtml = value
                Me.DoChanged("IsBodyHtml", value, Not value)
            End Set
        End Property

        Public Property Priority As System.Net.Mail.MailPriority
            Get
                Return Me.m_Priority
            End Get
            Set(value As System.Net.Mail.MailPriority)
                Dim oldValue As MailPriority = Me.Priority
                If (oldValue = value) Then Exit Property
                Me.m_Priority = value
                Me.DoChanged("Priority", value, oldValue)
            End Set
        End Property

        Public Property ReplyTo As MailAddress
            Get
                If (Me.m_ReplyTo Is Nothing) Then Me.checkAddressies()
                If (Me.m_ReplyTo Is Nothing) Then Me.m_ReplyTo = New MailAddress(Me, "reply-to")
                Return Me.m_ReplyTo
            End Get
            Set(value As MailAddress)
                Dim oldValue As MailAddress = Me.ReplyTo
                If (value Is Nothing) Then value = New MailAddress
                If (oldValue.Equals(value)) Then Exit Property
                Me.m_ReplyTo.Address = value.Address
                Me.m_ReplyTo.DisplayName = value.DisplayName
                Me.DoChanged("ReplyTo", value, oldValue)
            End Set
        End Property

        Public Property DeliveredTo As MailAddress
            Get
                If (Me.m_DeliveredTo Is Nothing) Then Me.checkAddressies()
                If (Me.m_DeliveredTo Is Nothing) Then Me.m_DeliveredTo = New MailAddress(Me, "delivered-to")
                Return Me.m_DeliveredTo
            End Get
            Set(value As MailAddress)
                Dim oldValue As MailAddress = Me.DeliveredTo
                If (value Is Nothing) Then value = New MailAddress
                If (oldValue.Equals(value)) Then Exit Property
                Me.m_DeliveredTo.Address = value.Address
                Me.m_DeliveredTo.DisplayName = value.DisplayName
                Me.DoChanged("DeliveredTo", value, oldValue)
            End Set
        End Property

        Public Property Sender As MailAddress
            Get
                If (Me.m_Sender Is Nothing) Then Me.checkAddressies()
                If (Me.m_Sender Is Nothing) Then Me.m_Sender = New MailAddress(Me, "sender")
                Return Me.m_Sender
            End Get
            Set(value As MailAddress)
                Dim oldValue As MailAddress = Me.Sender
                If (value Is Nothing) Then value = New MailAddress
                If (oldValue.Equals(value)) Then Exit Property
                Me.m_Sender.Address = value.Address
                Me.m_Sender.DisplayName = value.DisplayName
                Me.DoChanged("Sender", value, oldValue)
            End Set
        End Property

        Public Property Subject As String
            Get
                Return Me.m_Subject
            End Get
            Set(value As String)
                Dim oldValue As String = Me.Subject
                If (oldValue = value) Then Exit Property
                Me.m_Subject = value
                Me.DoChanged("Subject", value, oldValue)
            End Set
        End Property



        Public ReadOnly Property [To] As MailAddressCollection
            Get
                If (Me.m_To Is Nothing) Then
                    Me.checkAddressies()
                    Me.m_To = New MailAddressCollection(Me, "to")
                End If
                Return Me.m_To
            End Get
        End Property


        Private Function CreateHeaders(ByVal str As String) As CKeyCollection(Of String)
            If (str = "") Then
                Return New CKeyCollection(Of String)
            Else
                Dim tmp As CKeyCollection = XML.Utils.Serializer.Deserialize(str)
                Dim ret As New CKeyCollection(Of String)
                For Each k As String In tmp.Keys
                    ret.Add(k, Strings.CStr(tmp(k)))
                Next
                Return ret

            End If
        End Function

        Private Function GetEncoding(ByVal str As String) As System.Text.Encoding
            str = Trim(str)
            If (str = "") Then Return Nothing
            Return System.Text.Encoding.GetEncoding(str)
        End Function

        Private Function GetStr(ByVal addr As MailAddressEx) As String
            If (addr Is Nothing) Then Return ""
            Return addr.ToString
        End Function

        Private Function GetStr(ByVal addrs As MailAddressCollectionEx) As String
            If (addrs Is Nothing) Then Return ""
            Return addrs.ToString
        End Function


        Private Function GetStr(ByVal enc As System.Text.Encoding) As String
            If (enc Is Nothing) Then Return ""
            Return enc.WebName
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_EmailMessages"
        End Function



        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_ApplicationID = reader.Read("ApplicationID", Me.m_ApplicationID)
            Me.m_AccountID = reader.Read("AccountID", Me.m_AccountID)
            Me.m_FolderID = reader.Read("FolderID", Me.m_FolderID)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Headers = Me.CreateHeaders(reader.GetValue("Headers", ""))
            Me.m_Subject = reader.Read("Subject", Me.m_Subject)
            Me.m_DeliveryDate = reader.Read("DeliveryDate", Me.m_DeliveryDate)
            Me.m_DownloadDate = reader.Read("DownloadDate", Me.m_DownloadDate)
            Me.m_ReadDate = reader.Read("ReadDate", Me.m_ReadDate)
            Me.m_MessageId = reader.Read("MessageId", Me.m_MessageId)
            Me.m_ReplyToMessageId = reader.Read("ReplyToMessageId", Me.m_ReplyToMessageId)
            Me.m_IsBodyHtml = reader.Read("BodyHtml", Me.m_IsBodyHtml)
            Me.m_DeliveryNotificationOptions = reader.Read("DeliveryNotificationOptions", Me.m_DeliveryNotificationOptions)
            Me.m_Priority = reader.Read("Priority", Me.m_Priority)
            Me.m_Body = vbNullString
            Return MyBase.LoadFromRecordset(reader)
        End Function



        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            If (Me.m_Attachements IsNot Nothing) Then
                SetFlag(MailFlags.Attachments, (Me.m_Attachements.Count > 0))
            End If
            writer.Write("ApplicationID", Me.ApplicationID)
            writer.Write("AccountID", Me.AccountID)
            writer.Write("FolderID", Me.FolderID)
            writer.Write("Flags", Me.m_Flags)
            If (Me.m_Headers IsNot Nothing) Then writer.Write("Headers", XML.Utils.Serializer.Serialize(Me.Headers, XML.XMLSerializeMethod.Document))
            writer.Write("Subject", Me.m_Subject)
            writer.Write("DeliveryDate", Me.m_DeliveryDate)
            writer.Write("DownloadDate", Me.m_DownloadDate)
            writer.Write("ReadDate", Me.m_ReadDate)
            writer.Write("MessageId", Me.m_MessageId)
            writer.Write("ReplyToMessageId", Me.m_ReplyToMessageId)
            writer.Write("BodyHtml", Me.m_IsBodyHtml)
            writer.Write("DeliveryNotificationOptions", Me.m_DeliveryNotificationOptions)
            writer.Write("Priority", Me.m_Priority)
            writer.Write("From", Me.From.Address)
            writer.Write("DeliveredTo", Me.DeliveredTo.Address)
            'writer.Write("To", XML.Utils.Serializer.Serialize(Me.To))
            'writer.Write("Cc", XML.Utils.Serializer.Serialize(Me.Cc))
            'writer.Write("Bcc", XML.Utils.Serializer.Serialize(Me.Bcc))


            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            'SetFlag(MailFlags.Attachments, (Me.Attachments.Count > 0))
            writer.WriteAttribute("ApplicationID", Me.ApplicationID)
            writer.WriteAttribute("AccountID", Me.AccountID)
            writer.WriteAttribute("FolderID", Me.FolderID)
            writer.WriteAttribute("DownloadDate", Me.m_DownloadDate)
            writer.WriteAttribute("ReadDate", Me.m_ReadDate)
            writer.WriteAttribute("DeliveryDate", Me.m_DeliveryDate)
            writer.WriteAttribute("MessageId", Me.m_MessageId)
            writer.WriteAttribute("ReplyToMessageId", Me.m_ReplyToMessageId)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("BodyHtml", Me.m_IsBodyHtml)
            writer.WriteAttribute("DeliveryNotificationOptions", Me.m_DeliveryNotificationOptions)
            writer.WriteAttribute("Priority", Me.m_Priority)
            writer.WriteAttribute("Subject", Me.m_Subject)

            MyBase.XMLSerialize(writer)

            Me.checkAddressies()
            'If (writer.Settings.GetValueBool("OnlyHeaders", False) = False) Then
            '    writer.WriteTag("Headers", Me.Headers)
            'End If
            'writer.WriteTag("Attachments", Me.Attachments())
            writer.WriteTag("Addressies", Me.GetCurrentAddressList)
            ' writer.WriteTag("Body", Me.Body)
        End Sub

        Private Function ParseXMLRetAddress(ByVal fieldValue As Object) As MailAddressEx
            Dim str As String = Strings.Trim(XML.Utils.Serializer.DeserializeString(fieldValue))
            If (str = "") Then Return Nothing
            Return New MailAddressEx(str)
        End Function

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "ApplicationID" : Me.m_ApplicationID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "AccountID" : Me.m_AccountID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "FolderID" : Me.m_FolderID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DownloadDate" : Me.m_DownloadDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ReadDate" : Me.m_ReadDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DeliveryDate" : Me.m_DeliveryDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "MessageId" : Me.m_MessageId = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ReplyToMessageId" : Me.m_ReplyToMessageId = XML.Utils.Serializer.DeserializeString(fieldValue) 'Me.ReplyToMessageId)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "BodyHtml" : Me.m_IsBodyHtml = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "DeliveryNotificationOptions" : Me.m_DeliveryNotificationOptions = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Priority" : Me.m_Priority = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Subject" : Me.m_Subject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Attachments" : Me.m_Attachements = fieldValue : Me.m_Attachements.SetOwner(Me)

                Case "Headers"
                    fieldValue = XML.Utils.Serializer.ToObject(fieldValue)
                    If (fieldValue IsNot Nothing) Then
                        Me.m_Headers = New CKeyCollection(Of String)
                        Dim tmp As CKeyCollection = fieldValue
                        For Each k As String In tmp.Keys
                            Me.m_Headers.Add(k, tmp(k))
                        Next
                    End If

                Case "Addressies" : Me.SetCurrentAddressList(fieldValue)
                Case "Body" : Me.m_Body = XML.Utils.Serializer.DeserializeString(fieldValue)

                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Overrides Function ToString() As String
            Return Formats.FormatUserDateTime(Me.m_DeliveryDate) & ", to: " & Me.m_To.ToString & ", Subject: " & Me.m_Subject
        End Function

        Public Property Flags As MailFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As MailFlags)
                Dim oldValue As MailFlags = Me.m_Flags
                If (value = oldValue) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Function TestFlag(ByVal flag As MailFlags) As Boolean
            Return DMD.Sistema.TestFlag(Me.m_Flags, flag)
        End Function

        Public Sub SetFlag(ByVal flag As MailFlags, ByVal value As Boolean)
            If (Me.TestFlag(flag) = value) Then Exit Sub
            Dim oldValue As MailFlags = Me.m_Flags
            Me.m_Flags = DMD.Sistema.SetFlag(Me.m_Flags, flag, value)
            Me.DoChanged("Flags", value, oldValue)
        End Sub

        Private Function GetFileName() As String
            SyncLock Sistema.ApplicationContext
                Dim ret As String = "mail\app\" & Me.ApplicationID & "\msg" & Me.ID & ".dat"
                Dim fileName As String = System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, ret)
                System.IO.File.WriteAllText(fileName, "")
                Return fileName
            End SyncLock
        End Function



        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)

            If (ret) Then
                Dim path As String = "mail\app\" & Me.ApplicationID
                path = System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, path)
                FileSystem.CreateRecursiveFolder(path)
                path = path & "\msg" & Me.ID & ".dat"
#If DEBUG Then
                If Len(Me.Body) = 0 Then
                    Debug.Print("oops")
                End If
#End If
                System.IO.File.WriteAllText(path, Me.Body)

                If (Me.m_Attachements IsNot Nothing) Then Me.m_Attachements.Save(True)

                Dim original As System.Collections.ArrayList = Me.GetOriginalAdressies
                Dim saved As CCollection = Me.SaveAddresses
                For Each a As MailAddress In saved
                    Dim i As Integer = 0
                    While (i < original.Count)
                        Dim o As MailAddress = original(i)
                        If (a Is o) OrElse GetID(a) = GetID(o) Then '(a.Address = o.Address AndAlso a.FieldName = o.FieldName) Then
                            original.RemoveAt(i)
                        Else
                            i += 1
                        End If
                    End While
                Next

                For Each a As MailAddress In original
                    a.Delete()
                Next


            End If
            Return ret

        End Function

        Public Overrides Sub Save(Optional force As Boolean = False)
            SyncLock Office.Mails.Index.lockObject
                MyBase.Save(force)

                'Salviamo gli indici
                If (Me.Stato = ObjectStatus.OBJECT_VALID) Then
                    Office.Mails.Index.Index(Me)
                Else
                    Office.Mails.Index.Unindex(Me)
                End If
            End SyncLock

        End Sub

        Private Function SaveAddresses() As CCollection
            Dim col As CCollection = Me.GetCurrentAddressList



            For Each m As MailAddress In col
                m.SetApplication(Me.Application)
                m.SetMessage(Me)
                m.Save(True)
            Next

            Return col
        End Function

        Friend Function GetCurrentAddressList() As CCollection
            Dim col As New CCollection
            If (Me.m_From IsNot Nothing) Then
                Me.m_From.FieldName = "from" : col.Add(Me.m_From)
            End If
            If (Me.m_ReplyTo IsNot Nothing) Then
                Me.m_ReplyTo.FieldName = "reply-to" : col.Add(Me.m_ReplyTo)
            End If
            If (Me.m_Sender IsNot Nothing) Then
                Me.m_Sender.FieldName = "sender" : col.Add(Me.m_Sender)
            End If
            If (Me.m_DeliveredTo IsNot Nothing) Then
                Me.m_DeliveredTo.FieldName = "delivered-to" : col.Add(Me.m_DeliveredTo)
            End If

            If (Me.m_To IsNot Nothing) Then
                For Each m As MailAddress In Me.m_To
                    m.FieldName = "to"
                    col.Add(m)
                Next
            End If

            If (Me.m_Cc IsNot Nothing) Then
                For Each m As MailAddress In Me.m_Cc
                    m.FieldName = "cc"
                    col.Add(m)
                Next
            End If

            If (Me.m_Bcc IsNot Nothing) Then
                For Each m As MailAddress In Me.m_Bcc
                    m.FieldName = "bcc"
                    col.Add(m)
                Next
            End If


            Return col
        End Function

        Friend Sub SetCurrentAddressList(ByVal value As CCollection)
            Me.m_From = Nothing
            Me.m_Sender = Nothing
            Me.m_DeliveredTo = Nothing
            Me.m_ReplyTo = Nothing

            Me.m_To = New MailAddressCollection
            Me.m_To.SetFieldName("to")
            Me.m_To.SetMessage(Me)

            Me.m_Cc = New MailAddressCollection
            Me.m_Cc.SetFieldName("cc")
            Me.m_Cc.SetMessage(Me)

            Me.m_Bcc = New MailAddressCollection
            Me.m_Bcc.SetFieldName("bcc")
            Me.m_Bcc.SetMessage(Me)


            For Each a As MailAddress In value
                a.SetApplication(Me.Application)
                a.SetMessage(Me)
                Select Case a.FieldName
                    Case "from" : Me.m_From = a
                    Case "sender" : Me.m_Sender = a
                    Case "delivered-to" : Me.m_DeliveredTo = a
                    Case "reply-to" : Me.m_ReplyTo = a
                    Case "to" : Me.m_To.Add(a)
                    Case "cc" : Me.m_Cc.Add(a)
                    Case "bcc" : Me.m_Bcc.Add(a)
                End Select
            Next

            If (Me.m_From Is Nothing) Then Me.m_From = New MailAddress(Me, "from")
            If (Me.m_Sender Is Nothing) Then Me.m_Sender = New MailAddress(Me, "sender")
            If (Me.m_DeliveredTo Is Nothing) Then Me.m_DeliveredTo = New MailAddress(Me, "delivered-to")
            If (Me.m_ReplyTo Is Nothing) Then Me.m_ReplyTo = New MailAddress(Me, "reply-to")

        End Sub

        Public Sub SetOriginalAddressList(ByVal value As System.Collections.ArrayList)
            Me.m_From = Nothing
            Me.m_Sender = Nothing
            Me.m_DeliveredTo = Nothing
            Me.m_ReplyTo = Nothing

            Me.m_To = Nothing
            Me.m_Cc = Nothing
            Me.m_Bcc = Nothing


            For Each a As MailAddress In value
                a.SetApplication(Me.Application)
                a.SetMessage(Me)
                Select Case a.FieldName
                    Case "from" : Me.m_From = a
                    Case "sender" : Me.m_Sender = a
                    Case "delivered-to" : Me.m_DeliveredTo = a
                    Case "reply-to" : Me.m_ReplyTo = a
                    Case "to"
                        If (Me.m_To Is Nothing) Then
                            Me.m_To = New MailAddressCollection
                            Me.m_To.SetFieldName("to")
                            Me.m_To.SetMessage(Me)
                        End If
                        Me.m_To.Add(a)
                    Case "cc"
                        If (Me.m_Cc Is Nothing) Then
                            Me.m_Cc = New MailAddressCollection
                            Me.m_Cc.SetFieldName("cc")
                            Me.m_Cc.SetMessage(Me)
                        End If
                        Me.m_Cc.Add(a)
                    Case "bcc"
                        If (Me.m_Bcc Is Nothing) Then
                            Me.m_Bcc = New MailAddressCollection
                            Me.m_Bcc.SetFieldName("bcc")
                            Me.m_Bcc.SetMessage(Me)
                        End If
                        Me.m_Bcc.Add(a)
                End Select
            Next

            Me.m_OriginalAddresses = value
        End Sub

        'Protected Function FindAddress(ByVal fieldName As String) as MailAddress
        '    Dim items As System.Collections.ArrayList = Me.GetOriginalAdressies
        '    For Each a as MailAddress In items
        '        If a.FieldName = fieldName Then Return a
        '    Next
        '    Return Nothing
        'End Function

        Protected Friend Function GetOriginalAdressies() As System.Collections.ArrayList
            If (Me.m_OriginalAddresses Is Nothing) Then
                Me.m_OriginalAddresses = New System.Collections.ArrayList

                Dim cursor As New MailAddressCursor
                cursor.IgnoreRights = True
                cursor.MessageID.Value = GetID(Me)
                cursor.ID.SortOrder = SortEnum.SORT_ASC
                While Not cursor.EOF
                    Dim m As MailAddress = cursor.Item
                    m.SetApplication(Me.Application)
                    m.SetMessage(Me)
                    Me.m_OriginalAddresses.Add(m)
                    cursor.MoveNext()
                End While
                cursor.Dispose()
            End If
            Return Me.m_OriginalAddresses
        End Function






        '' This code added by Visual Basic to correctly implement the disposable pattern.
        'Public Overridable Sub Dispose() Implements IDisposable.Dispose
        '    If (Me._children IsNot Nothing) Then
        '        For Each m As MailMessage In Me._children
        '            m.Dispose()
        '        Next
        '        Me._children = Nothing
        '    End If


        '    If (Me.m_Attachements IsNot Nothing) Then
        '        For Each a As MailAttachment In Me.m_Attachements
        '            a.Dispose()
        '        Next
        '        Me.m_Attachements = Nothing
        '    End If
        '    Me.m_Headers = Nothing
        '    Me.m_From = Nothing
        '    Me.m_To = Nothing
        '    Me.m_Cc = Nothing
        '    Me.m_Bcc = Nothing
        '    Me.m_ReturnAddress = Nothing
        '    Me.m_Sender = Nothing
        '    Me.m_ReplyTo = Nothing

        '    Me.m_AccountName = vbNullString
        '    Me.m_Account = Nothing
        '    Me.m_Folder = Nothing
        '    Me._children = Nothing
        '    Me.m_AlternateViews = Nothing
        '    Me.m_Attachements = Nothing
        '    Me.m_Headers = Nothing
        '    Me.m_From = Nothing
        '    Me.m_To = Nothing
        '    Me.m_Cc = Nothing
        '    Me.m_Bcc = Nothing
        '    Me.m_ReturnAddress = Nothing
        '    Me.m_Subject = vbNullString
        '    Me.m_Body = vbNullString
        '    Me.m_DeliveryDate = Nothing
        '    Me.m_Routing = vbNullString
        '    Me.m_MessageId = vbNullString
        '    Me.m_ReplyToMessageId = vbNullString
        '    Me.m_BodyEncoding = Nothing
        '    Me.m_SubjectEncoding = Nothing
        '    Me.m_Sender = Nothing
        '    Me.m_ReplyTo = Nothing
        '    Me.m_DownloadDate = Nothing
        '    Me.m_ReadDate = Nothing
        'End Sub


        'Friend Sub Process(ByVal message As Net.Mail.MailMessageEx)
        '    'Me.m_Object = message

        '    Me.Headers.Clear()

        '    For Each key As String In message.Headers.Keys
        '        Me.Headers.Add(key, message.Headers(key))
        '    Next

        '    Me.Attachments.Clear()
        '    For Each at As System.Net.Mail.Attachment In message.Attachments
        '        Dim at1 As New MailAttachment
        '        at1.From(at)
        '        at1.Stato = ObjectStatus.OBJECT_VALID
        '        Me.Attachments.Add(at1)
        '    Next

        '    Me.m_Flags = MailFlags.Unread
        '    Me.m_From = message.From
        '    Me.m_To = message.To
        '    Me.m_Cc = message.CC
        '    Me.m_Bcc = message.Bcc
        '    Me.m_ReturnAddress = message.ReplyTo
        '    Me.m_Subject = message.Subject
        '    Me.m_Body = message.Body
        '    Me.m_DeliveryDate = message.DeliveryDate
        '    Me.m_MessageId = message.MessageId
        '    Me.m_ReplyToMessageId = message.ReplyToMessageId
        '    Me.m_Sender = message.Sender
        '    Me.m_ReplyTo = message.ReplyTo
        '    Me.m_IsBodyHtml = message.IsBodyHtml
        '    Me.m_DeliveryNotificationOptions = message.DeliveryNotificationOptions
        '    Me.m_Priority = message.Priority
        'End Sub

        'Function GetObject() As System.Net.Mail.MailMessage
        '    Return Me.m_Object
        'End Function

        '''' <summary>
        '''' Sposta il messaggio nel cestino
        '''' </summary>
        '''' <remarks></remarks>
        'Sub TrashMe()
        '    Me.MoveTo(Me.Folders.TrashBin)
        'End Sub

        Sub MoveTo(ByVal mailFolder As MailFolder)
            Me.Folder = mailFolder
            Me.Save()
        End Sub

        Sub CopyTo(mailFolder As MailFolder)
            Dim tmp As MailMessage = Me.MemberwiseClone
            DBUtils.ResetID(tmp)
            tmp.Folder = mailFolder

            For Each at As MailAttachment In Me.Attachments
                Dim at1 As MailAttachment = at.Clone
                tmp.Attachments.Add(at1)
            Next

            tmp.Save()

        End Sub



        Protected Overridable Function GetIndexedWords() As String() Implements IIndexable.GetIndexedWords
            Dim words() As String = {}
            Dim str As String = UCase(Me.Subject & " " & Strings.RemoveHTMLTags(Me.Body))
            Dim word As String = ""
            Dim stato As Integer = 0
            For i As Integer = 1 To Len(str)
                Dim ch As String = Mid(str, i, 1)
                Select Case stato
                    Case 0
                        If Char.IsDigit(ch) OrElse Char.IsLetter(ch) Then
                            word &= Strings.OnlyCharsAndNumbers(ch)
                        ElseIf (ch = " " OrElse ch = vbCr OrElse ch = vbLf OrElse ch = vbTab) Then
                            If (Arrays.BinarySearch(words, word) < 0) Then
                                words = Arrays.InsertSorted(words, word)
                                word = ""
                            End If
                        Else
                        End If
                End Select
            Next
            Return words
        End Function

        Private Function GetKeyWords() As String() Implements IIndexable.GetKeyWords
            Return {}
        End Function

        Public Overrides Function GetModule() As CModule
            Return Office.Mails.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Mails.Database
        End Function

    End Class

End Class

