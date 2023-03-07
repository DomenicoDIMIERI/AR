Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Databases
Imports System.Globalization
Imports System.IO

Namespace Net.Mail

    ''' <summary>
    ''' This class adds a few internet mail headers not already exposed by the 
    ''' System.Net.MailMessage.  It also provides support to encapsulate the
    ''' nested mail attachments in the Children collection.
    ''' </summary>
    <Serializable>
    Public Class MailMessageEx
        Inherits System.Net.Mail.MailMessage
        Implements DMD.XML.IDMDXMLSerializable

        Public Const EmailRegexPattern As String = "(['""]{1,}.+['""]{1,}\s+)?<?[\w\.\-]+@[^\.][\w\.\-]+\.[a-z]{2,}>?"

        Private m_Account As String     'Account utilizzato per scaricare il messaggio
        Private m_Folder As String
        Private _octets As Long
        Private m_AlternateViews As AlternateViewCollectionEx = Nothing
        Private m_To As MailAddressCollectionEx = Nothing
        Private m_Attachments As AttachmentCollectionEx = Nothing
        Private m_CC As MailAddressCollectionEx = Nothing
        Private m_BCC As MailAddressCollectionEx = Nothing
        Private _messageNumber As Integer
        Private _children As System.Collections.Generic.List(Of MailMessageEx)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Account = ""
            Me.m_Folder = ""
            Me._octets = 0
            Me.m_AlternateViews = Nothing
            Me.m_To = Nothing
            Me.m_Attachments = Nothing
            Me.m_CC = Nothing
            Me.m_BCC = Nothing
            Me._messageNumber = 0
            Me._children = Nothing
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="MailMessageEx"/> class.
        ''' </summary>
        Public Sub New(ByVal from As String, ByVal [to] As String)
            MyBase.New([from], [to])
            DMD.DMDObject.IncreaseCounter(Me)
            Me._children = Nothing
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="MailMessageEx"/> class.
        ''' </summary>
        Public Sub New(ByVal from As String, ByVal [to] As String, ByVal subject As String, ByVal body As String)
            MyBase.New([from], [to], subject, body)
            DMD.DMDObject.IncreaseCounter(Me)
            Me._children = Nothing
        End Sub

        Public Sub New(ByVal from As System.Net.Mail.MailAddress, ByVal [to] As System.Net.Mail.MailAddress)
            MyBase.New([from], [to])
            DMD.DMDObject.IncreaseCounter(Me)
            Me._children = Nothing
        End Sub

        Public Sub New(ByVal m As MailMessage)
            Me.New
            Me.HeadersEncoding = m.HeadersEncoding
            For Each k As String In m.Headers.Keys
                Me.Headers.Set(k, m.Headers.Get(k))
            Next
            For Each a As AlternateView In m.AlternateViews
                Me.AlternateViews.Add(New AlternateViewEx(a))
            Next
            For Each a As Attachment In m.Attachments
                Me.Attachments.Add(New AttachmentEx(a))
            Next
            For Each a As MailAddress In m.Bcc
                Me.Bcc.Add(New MailAddressEx(a))
            Next
            Me.IsBodyHtml = m.IsBodyHtml
            Me.Body = m.Body
            Me.BodyEncoding = m.BodyEncoding
            For Each a As MailAddress In m.CC
                Me.CC.Add(New MailAddressEx(a))
            Next
            Me.DeliveryNotificationOptions = m.DeliveryNotificationOptions
            Me.From = New MailAddressEx(m.From)
            Me.Priority = m.Priority
            Me.ReplyTo = New MailAddressEx(m.ReplyTo)
            For Each a As MailAddress In m.ReplyToList
                Me.ReplyToList.Add(New MailAddressEx(a))
            Next
            Me.Sender = New MailAddressEx(m.Sender)
            Me.Subject = m.Subject
            Me.SubjectEncoding = m.SubjectEncoding
            For Each a As MailAddress In m.To
                Me.To.Add(New MailAddressEx(a))
            Next
        End Sub


        Public Shadows ReadOnly Property Attachments As AttachmentCollectionEx
            Get
                If (Me.m_Attachments Is Nothing) Then
                    Me.m_Attachments = New AttachmentCollectionEx(Me)
                End If
                Return Me.m_Attachments
            End Get
        End Property

        Friend Function GetBaseAttachments() As System.Net.Mail.AttachmentCollection
            Return MyBase.Attachments
        End Function


        Public Shadows ReadOnly Property AlternateViews As AlternateViewCollectionEx
            Get
                If (Me.m_AlternateViews Is Nothing) Then
                    Me.m_AlternateViews = New AlternateViewCollectionEx(Me)
                End If
                Return Me.m_AlternateViews
            End Get
        End Property

        Friend Function GetBaseAlternateViews() As System.Net.Mail.AlternateViewCollection
            Return MyBase.AlternateViews
        End Function

        Public Shadows ReadOnly Property [To] As MailAddressCollectionEx
            Get
                If (Me.m_To Is Nothing) Then Me.m_To = New MailAddressCollectionEx(MyBase.To)
                Return Me.m_To
            End Get
        End Property

        Public Shadows ReadOnly Property CC As MailAddressCollectionEx
            Get
                If (Me.m_CC Is Nothing) Then Me.m_CC = New MailAddressCollectionEx(MyBase.CC)
                Return Me.m_CC
            End Get
        End Property

        Public Shadows ReadOnly Property Bcc As MailAddressCollectionEx
            Get
                If (Me.m_BCC Is Nothing) Then Me.m_BCC = New MailAddressCollectionEx(MyBase.Bcc)
                Return Me.m_BCC
            End Get
        End Property

        Public Shadows Property From As MailAddressEx
            Get
                Dim v As MailAddress = MyBase.From
                If v IsNot Nothing AndAlso Not (TypeOf (v) Is MailAddressEx) Then
                    v = New MailAddressEx(v)
                    MyBase.From = v
                End If
                Return MyBase.From
            End Get
            Set(value As MailAddressEx)
                MyBase.From = value
            End Set
        End Property

        Public Shadows Property Sender As MailAddressEx
            Get
                Dim v As MailAddress = MyBase.Sender
                If v IsNot Nothing AndAlso Not (TypeOf (v) Is MailAddressEx) Then
                    v = New MailAddressEx(v)
                    MyBase.Sender = v
                End If
                Return MyBase.Sender
            End Get
            Set(value As MailAddressEx)
                MyBase.Sender = value
            End Set
        End Property

        Public Shadows Property ReplyTo As MailAddressEx
            Get
                Dim v As MailAddress = MyBase.ReplyTo
                If v IsNot Nothing AndAlso Not (TypeOf (v) Is MailAddressEx) Then
                    v = New MailAddressEx(v)
                    MyBase.ReplyTo = v
                End If
                Return MyBase.ReplyTo
            End Get
            Set(value As MailAddressEx)
                MyBase.ReplyTo = value
            End Set
        End Property



        ''' <summary>
        ''' Restituisce o imposta il nome dell'account utilizzate per scaricare il messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Account As String
            Get
                Return Me.m_Account
            End Get
            Friend Set(value As String)
                Me.m_Account = Trim(value)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o imposta il percorso in cui il messaggio è stato archiviato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Folder As String
            Get
                Return Me.m_Folder
            End Get
            Set(value As String)
                Me.m_Folder = Trim(value)
            End Set
        End Property

        Public Property Octets As Long
            Get
                Return Me._octets
            End Get
            Set(value As Long)
                Me._octets = value
            End Set
        End Property


        ''' <summary>
        ''' Gets or sets the message number of the MailMessage on the POP3 server.
        ''' </summary>
        ''' <value>The message number.</value>
        Protected Friend Property MessageNumber As Integer
            Get
                Return Me._messageNumber
            End Get
            Friend Set(value As Integer)
                If (Me._messageNumber = value) Then Exit Property
                Me._messageNumber = value
                If (value = 0) Then
                    Debug.Print("o")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the children MailMessage attachments.
        ''' </summary>
        ''' <value>The children MailMessage attachments.</value>
        Public ReadOnly Property Children As System.Collections.Generic.List(Of MailMessageEx)
            Get
                SyncLock Me
                    If (Me._children Is Nothing) Then Me._children = New System.Collections.Generic.List(Of MailMessageEx)
                    Return Me._children
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Gets the delivery date.
        ''' </summary>
        ''' <value>The delivery date.</value>
        Public ReadOnly Property DeliveryDate As DateTime
            Get
                Dim str As String = Me.GetHeader(MailHeaders.Date)
                If (String.IsNullOrEmpty(str)) Then Return DateTime.MinValue

                Try
                    Dim p As Integer = InStr(str, "(")
                    If (p > 0) Then
                        Dim p1 As Integer = InStr(p, str, ")")
                        If (p1 > 0) Then
                            str = Left(str, p - 1) & Mid(str, p1 + 1)
                        End If
                    End If
                    Return DMD.Sistema.Formats.USA.ParseDate(str)
                Catch ex As Exception
                    Return Date.MinValue
                End Try
            End Get
        End Property

        ' ''' <summary>
        ' ''' Gets the return address.
        ' ''' </summary>
        ' ''' <value>The return address.</value>
        'Public ReadOnly Property ReturnAddress As MailAddress
        '    Get
        '        Dim replyTo As String = Me.GetHeader(MailHeaders.ReplyTo)
        '        If (String.IsNullOrEmpty(replyTo)) Then Return Nothing
        '        Return CreateMailAddress(replyTo)
        '    End Get
        'End Property

        ''' <summary>
        ''' Gets the routing.
        ''' </summary>
        ''' <value>The routing.</value>
        Public ReadOnly Property Routing As String
            Get
                Return Me.GetHeader(MailHeaders.Received)
            End Get
        End Property

        ''' <summary>
        ''' Gets the message id.
        ''' </summary>
        ''' <value>The message id.</value>
        Public ReadOnly Property MessageId As String
            Get
                Return Me.GetHeader(MailHeaders.MessageId)
            End Get
        End Property

        Public ReadOnly Property ReplyToMessageId As String
            Get
                Return Me.GetHeader(MailHeaders.InReplyTo, True)
            End Get
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

#Region "Static Metghods"

        Private Shared ReadOnly AddressDelimiters As Char() = New Char() {",", ";"}

        ''' <summary>
        ''' Creates the mail message from entity.
        ''' </summary>
        ''' <param name="entity">The entity.</param>
        ''' <returns></returns>
        Public Shared Function CreateMailMessageFromEntity(ByVal entity As MimeEntity) As MailMessageEx
            Dim message As New MailMessageEx()
            Dim value As String

            For Each key As String In entity.Headers.AllKeys
                value = entity.Headers(key)


                Try
                    value = MailHeaders.Decode(value)
                Catch ex As Exception
                    Sistema.ApplicationContext.Log("Decoding Mail Message Header: " & key & " - " & CStr(value))
                    Sistema.Events.NotifyUnhandledException(ex)
                End Try

                If (value.Equals(String.Empty)) Then value = " "
                message.Headers.Add(key.ToLowerInvariant(), value)

                If value.IndexOf("=?UTF-8") > 0 Then
                    Debug.Print("Problema decodifica " & key & " - " & value)
                End If

                Select Case (key.ToLowerInvariant())
                    Case MailHeaders.Bcc
                        MailMessageEx.PopulateAddressList(value, message.Bcc)
                    Case MailHeaders.Cc
                        MailMessageEx.PopulateAddressList(value, message.CC)
                    Case MailHeaders.From
                        message.From = MailMessageEx.CreateMailAddress(value)
                    Case MailHeaders.ReplyTo
                        message.ReplyTo = MailMessageEx.CreateMailAddress(value)
                    Case MailHeaders.Subject
                        message.Subject = value
                    Case MailHeaders.To
                        MailMessageEx.PopulateAddressList(value, message.To)
                    Case Else
                        'Throw New NotSupportedException
                End Select
            Next

            Return message
        End Function

        ''' <summary>
        ''' Creates the mail address.
        ''' </summary>
        ''' <param name="address">The address.</param>
        ''' <returns></returns>
        Public Shared Function CreateMailAddress(ByVal address As String) As MailAddressEx
            Try
                address = Trim(address).Trim(vbTab)
                If (InStr(address, Chr(34)) > 0 AndAlso Not address.StartsWith(Chr(34))) Then address = Chr(34) & address
                If (address = "") Then Return Nothing
                Return New MailAddressEx(address)
            Catch e As FormatException
                Throw New ArgumentException("Unable to create mail address from provided string: " & address, e)
            End Try
        End Function

        ''' <summary>
        ''' Populates the address list.
        ''' </summary>
        ''' <param name="addressList">The address list.</param>
        ''' <param name="recipients">The recipients.</param>
        Public Shared Sub PopulateAddressList(ByVal addressList As String, ByVal recipients As MailAddressCollectionEx)
            For Each address As MailAddressEx In GetMailAddresses(addressList)
                recipients.Add(address)
            Next
        End Sub

        ''' <summary>
        ''' Gets the mail addresses.
        ''' </summary>
        ''' <param name="addressList">The address list.</param>
        ''' <returns></returns>
        Public Shared Function GetMailAddresses(ByVal addressList As String) As IEnumerable
            Dim email As New Regex(EmailRegexPattern)
            Dim ret As New System.Collections.ArrayList

            For Each match As Match In email.Matches(addressList)
                Try
                    Dim items As New System.Collections.ArrayList
                    Dim stato As Integer = 0
                    Dim name As String = ""
                    Dim address As String = ""
                    For i As Integer = 1 To Len(match.Value)
                        Dim ch As String = Mid(match.Value, i, 1)
                        Select Case stato
                            Case 0 'Normale
                                If (ch = Chr(34)) Then
                                    stato = 1 'Siamo dentro una stringa
                                ElseIf (ch = "<") Then
                                    stato = 2 'Siamo dentro ad un indirizzo
                                    address = ""
                                ElseIf (ch = ",") OrElse (ch = ";") Then

                                Else
                                    name &= ch
                                End If
                            Case 1 'Siamo dentro una stringa
                                If (ch = Chr(34)) Then
                                    stato = 0 'Torniamo in stato normale
                                Else
                                    name &= ch
                                End If
                            Case 2 'Siamo dentro un indirizzo
                                If (ch = ">") Then
                                    address = Trim(address)
                                    name = Trim(name)
                                    If (address = "") Then
                                        If (Sistema.EMailer.IsValidAddress(name)) Then
                                            ret.Add(New MailAddressEx(name))
                                        End If
                                    Else
                                        If (Sistema.EMailer.IsValidAddress(address)) Then
                                            If (name <> "") Then
                                                ret.Add(New MailAddressEx(address, name))
                                            Else
                                                ret.Add(New MailAddressEx(address))
                                            End If
                                        End If
                                    End If
                                    name = ""
                                    address = ""
                                    Exit For
                                Else
                                    address &= ch
                                End If
                        End Select
                    Next

                    address = Trim(address)
                    name = Trim(name)
                    If (address = "") Then
                        If (Sistema.EMailer.IsValidAddress(name)) Then
                            ret.Add(New MailAddressEx(name))
                        End If
                    Else
                        If (Sistema.EMailer.IsValidAddress(address)) Then
                            If (name <> "") Then
                                ret.Add(New MailAddressEx(address, name))
                            Else
                                ret.Add(New MailAddressEx(address))
                            End If
                        End If
                    End If


                    '= Split(Replace(match.Value, ";", ","), ",")
                    'For Each s As String In items
                    ' If (Trim(s) <> "") Then ret.Add(CreateMailAddress(s))
                    'Next
                Catch ex As Exception
                    Debug.Print(ex.Message)
                End Try

            Next
            Return ret
            '/*
            'string[] addresses = addressList.Split(AddressDelimiters);
            'foreach (string address in addresses)
            '{
            '    yield return CreateMailAddress(address);
            '}*/
        End Function

#End Region

        'Protected Overrides Function GetConnection() As Databases.CDBConnection
        '    Return DMD.Databases.APPConn
        'End Function

        'Public Overrides Function GetModule() As Sistema.CModule
        '    Return DMD.Net.Mail.EMailMessages.Module
        'End Function

        'Protected Overrides Function GetTableName() As String
        '    Return "tbl_eMailMessages"
        'End Function

        'Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
        '    'message.Headers.Add(key.ToLowerInvariant(), value)
        '    reader.Read("Folder", Me.m_Folder)
        '    reader.Read("MailAccount", Me.m_Account)
        '    reader.Read("MsgNum", Me._messageNumber)
        '    'Me.MessageId = Formats.ToString(dbRis("MessageId")))
        '    MailMessageEx.PopulateAddressList(reader.GetValue("Bcc", vbNullString), Me.m_Message.Bcc)
        '    MailMessageEx.PopulateAddressList(reader.GetValue("Cc", vbNullString), Me.m_Message.CC)
        '    Me.m_Message.From = MailMessageEx.CreateMailAddress(reader.GetValue("From", vbNullString))
        '    Me.m_Message.ReplyTo = MailMessageEx.CreateMailAddress(reader.GetValue("ReplayTo", vbNullString))
        '    reader.Read("Subject", Me.m_Message.Subject)
        '    MailMessageEx.PopulateAddressList(reader.GetValue("To", vbNullString), Me.m_Message.To)
        '    reader.Read("Body", Me.m_Message.Body)
        '    reader.Read("BodyHtml", Me.m_Message.IsBodyHtml)
        '    Me.m_Message.BodyEncoding = System.Text.Encoding.GetEncoding(reader.GetValue("BodyEncoding", vbNullString))
        '    Me.m_Message.SubjectEncoding = System.Text.Encoding.GetEncoding(reader.GetValue("SubjectEncoding", vbNullString))
        '    Return MyBase.LoadFromRecordset(reader)
        'End Function

        'Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
        '    'message.Headers.Add(key.ToLowerInvariant(), value)
        '    writer.Write("Folder", Me.m_Folder)
        '    writer.Write("MailAccount", Me.m_Account)
        '    writer.Write("MsgNum", Me._messageNumber)
        '    writer.Write("MessageId", Me.MessageId)
        '    writer.Write("Bcc", Me.m_Message.Bcc.ToString)
        '    writer.Write("Cc", Me.m_Message.CC.ToString)
        '    writer.Write("From", Me.m_Message.From.ToString)
        '    writer.Write("ReplayTo", Me.m_Message.ReplyTo.ToString)
        '    writer.Write("Subject", Me.m_Message.Subject)
        '    writer.Write("To", Me.m_Message.To.ToString)
        '    writer.Write("Body", Me.m_Message.Body)
        '    writer.Write("BodyHtml", Me.m_Message.IsBodyHtml)
        '    writer.Write("BodyEncoding", Me.m_Message.BodyEncoding.EncodingName)
        '    writer.Write("SubjectEncoding", Me.m_Message.SubjectEncoding.EncodingName)
        '    writer.Write("DeliveryDate", Me.DeliveryDate)
        '    Return MyBase.SaveToRecordset(writer)
        'End Function

        'Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
        '    MyBase.XMLSerialize(writer)
        '    writer.WriteTag("Folder", Me.m_Folder)
        '    writer.WriteTag("MailAccount", Me.m_Account)
        '    writer.WriteTag("MsgNum", Me._messageNumber)
        '    writer.WriteTag("MessageId", Me.MessageId)
        '    writer.WriteTag("Bcc", Me.m_Message.Bcc.ToString)
        '    writer.WriteTag("Cc", Me.m_Message.CC.ToString)
        '    writer.WriteTag("From", Me.m_Message.From.ToString)
        '    writer.WriteTag("ReplayTo", Me.m_Message.ReplyTo.ToString)
        '    writer.WriteTag("Subject", Me.m_Message.Subject)
        '    writer.WriteTag("To", Me.m_Message.To.ToString)
        '    writer.WriteTag("Body", Me.m_Message.Body)
        '    writer.WriteTag("BodyHtml", Me.m_Message.IsBodyHtml)
        '    writer.WriteTag("BodyEncoding", Me.m_Message.BodyEncoding.EncodingName)
        '    writer.WriteTag("SubjectEncoding", Me.m_Message.SubjectEncoding.EncodingName)
        '    writer.WriteTag("DeliveryDate", Me.DeliveryDate)
        'End Sub

        'Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
        '    Select Case fieldName
        '        Case "Folder" : XML.Read(Me.m_Folder, fieldValue)
        '        Case "MailAccount" : XML.Read(Me.m_Account, fieldValue)
        '        Case "MsgNum" : XML.Read(Me._messageNumber, fieldValue)
        '        Case "MessageId" : XML.Read(Me.MessageId, fieldValue)
        '        Case "Bcc" : XML.Read(Me.m_Message.Bcc.ToString, fieldValue)
        '        Case "Cc" : XML.Read(Me.m_Message.CC.ToString, fieldValue)
        '        Case "From" : XML.Read(Me.m_Message.From.ToString, fieldValue)
        '        Case "ReplayTo" : XML.Read(Me.m_Message.ReplyTo.ToString, fieldValue)
        '        Case "Subject" : XML.Read(Me.m_Message.Subject, fieldValue)
        '        Case "To" : XML.Read(Me.m_Message.To.ToString, fieldValue)
        '        Case "Body" : XML.Read(Me.m_Message.Body, fieldValue)
        '        Case "BodyHtml" : XML.Read(Me.m_Message.IsBodyHtml, fieldValue)
        '        Case "BodyEncoding" : XML.Read(Me.m_Message.BodyEncoding.EncodingName, fieldValue)
        '        Case "SubjectEncoding" : XML.Read(Me.m_Message.SubjectEncoding.EncodingName, fieldValue)
        '        Case "DeliveryDate" : XML.Read(Me.DeliveryDate, fieldValue)
        '        Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
        '    End Select

        'End Sub

        Public Overrides Function ToString() As String
            Return Me.ToString
        End Function

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "From" : Me.From = New MailAddressEx(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "ReplyTo" : Me.ReplyTo = New MailAddressEx(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "To"
                    Me.To.Clear()
                    If (TypeOf (fieldValue) Is IEnumerable) Then
                        Me.To.AddRange(fieldValue)
                    ElseIf (TypeOf (fieldValue) Is MailAddressEx) Then
                        Me.To.Add(DirectCast(fieldValue, MailAddressEx))
                    End If
                Case "CC"
                    Me.CC.Clear()
                    If (TypeOf (fieldValue) Is IEnumerable) Then
                        Me.CC.AddRange(fieldValue)
                    ElseIf (TypeOf (fieldValue) Is MailAddressEx) Then
                        Me.CC.Add(DirectCast(fieldValue, MailAddressEx))
                    End If
                Case "Bcc"
                    Me.Bcc.Clear()
                    If (TypeOf (fieldValue) Is IEnumerable) Then
                        Me.Bcc.AddRange(fieldValue)
                    ElseIf (TypeOf (fieldValue) Is MailAddressEx) Then
                        Me.Bcc.Add(DirectCast(fieldValue, MailAddressEx))
                    End If
                Case "Attachments"
                    Me.Attachments.Clear()
                    If (TypeOf (fieldValue) Is IEnumerable) Then
                        Me.Attachments.AddRange(fieldValue)
                    ElseIf (TypeOf (fieldValue) Is AttachmentEx) Then
                        Me.Attachments.Add(fieldValue)
                    End If
                Case "AlternateViews"
                    Me.AlternateViews.Clear()
                    If (TypeOf (fieldValue) Is IEnumerable) Then
                        Me.AlternateViews.AddRange(fieldValue)
                    ElseIf (TypeOf (fieldValue) Is AttachmentEx) Then
                        Me.AlternateViews.Add(fieldValue)
                    End If
            End Select
        End Sub

        Private Function MakeString(ByVal a As MailAddressEx) As String
            If (a Is Nothing) Then Return ""
            Return a.ToString
        End Function

        Private Function MakeString(ByVal a As System.Text.Encoding) As String
            If (a Is Nothing) Then Return ""
            Return a.WebName
        End Function

        Private Function MakeString(ByVal a As System.Net.Mime.ContentType) As String
            If (a Is Nothing) Then Return ""
            Return a.ToString
        End Function

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("From", Me.MakeString(Me.From))
            writer.WriteAttribute("ReplyTo", Me.MakeString(Me.ReplyTo))
            writer.WriteAttribute("BodyEncoding", Me.MakeString(Me.BodyEncoding))
            writer.WriteAttribute("ContentDescription", Me.ContentDescription)
            'writer.WriteAttribute("ContentDisposition", Me.ContentDisposition)
            writer.WriteAttribute("ContentId", Me.ContentId)
            writer.WriteAttribute("ContentType", Me.MakeString(Me.ContentType))
            writer.WriteAttribute("DeliveryDate", Me.DeliveryDate)
            writer.WriteAttribute("DeliveryNotificationOptions", Me.DeliveryNotificationOptions)
            writer.WriteAttribute("IsBodyHtml", Me.IsBodyHtml)
            writer.WriteAttribute("MessageId", Me.MessageId)
            writer.WriteAttribute("MessageNumber", Me.MessageNumber)
            writer.WriteAttribute("MimeVersion", Me.MimeVersion)
            writer.WriteAttribute("Priority", Me.Priority)
            writer.WriteAttribute("ReplyToMessageId", Me.ReplyToMessageId)
            writer.WriteAttribute("Sender", Me.MakeString(Me.Sender))
            writer.WriteAttribute("Subject", Me.Subject)
            writer.WriteAttribute("SubjectEncoding", Me.MakeString(Me.SubjectEncoding))

            writer.WriteTag("To", Me.To)
            writer.WriteTag("CC", Me.CC)
            writer.WriteTag("Bcc", Me.Bcc)
            writer.WriteTag("Body", Me.Body)
            writer.WriteTag("Attachments", Me.Attachments)
            writer.WriteTag("AlternateViews", Me.AlternateViews)
            writer.WriteTag("Headers", Me.Headers.ToString)
        End Sub


        Protected Overrides Sub Dispose(disposing As Boolean)

            If (Me._children IsNot Nothing) Then
                For Each m As MailMessage In Me._children
                    m.Dispose()
                Next
                Me._children = Nothing
            End If

            If (Me.m_AlternateViews IsNot Nothing) Then
                For Each a As System.Net.Mail.AlternateView In Me.m_AlternateViews
                    For Each l As LinkedResource In a.LinkedResources
                        l.Dispose()
                    Next
                    a.Dispose()
                Next
                Me.m_AlternateViews = Nothing
            End If

            If (Me.m_Attachments IsNot Nothing) Then
                For Each a As System.Net.Mail.Attachment In Me.m_Attachments
                    a.Dispose()
                Next
                Me.m_Attachments = Nothing
            End If

            Me.m_Account = Nothing
            Me.m_BCC = Nothing
            Me.m_CC = Nothing
            Me.m_Folder = Nothing
            Me.m_To = Nothing


            MyBase.Dispose(disposing)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Private Const MultilineMessageTerminator As String = vbCr & vbLf & "." & vbCr & vbLf ' "\r\n.\r\n"
        Private Const MessageTerminator As String = "."

        Public Shared Function FromStream(ByVal stream As System.IO.Stream) As MailMessageEx
            Dim reader As StreamReader = Nothing
            Dim msg As DMD.Net.Mail.MailMessageEx = Nothing
            Dim entity As MimeEntity = Nothing
#If Not DEBUG Then
            Try
#End If
            reader = New StreamReader(stream)

            Dim lines As New System.Collections.Generic.List(Of String)
            Dim line As String

            While (Not reader.EndOfStream)
                line = reader.ReadLine()
                'pop3 protocol states if a line starts w/ a 
                ''.' that line will be byte stuffed w/ a '.'
                'if it is byte stuffed the remove the byte, 
                'otherwise we have reached the end of the message.
                If (line = vbNullString) Then line = ""

                If (line.StartsWith(MessageTerminator)) Then
                    If (line = MessageTerminator) Then Exit While
                    line = line.Substring(1)
                End If

                lines.Add(line)
            End While

            Dim items() As String = lines.ToArray()
            Dim mReader As New MimeReader(items)

            entity = mReader.CreateMimeEntity()
                msg = entity.ToMailMessageEx
#If Not DEBUG Then
                Return msg
            Catch e As Exception
                Throw
            Finally
#End If
            If (entity IsNot Nothing) Then entity.Dispose() : entity = Nothing
            If (reader IsNot Nothing) Then reader.Dispose()
#If Not DEBUG Then
            End Try
#Else
            Return msg
#End If
        End Function
    End Class

End Namespace
