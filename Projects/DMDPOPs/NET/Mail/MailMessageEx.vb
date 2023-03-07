Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports FinSeA.Net.Mime
Imports FinSeA.Databases

Namespace Net.Mail

    ''' <summary>
    ''' This class adds a few internet mail headers not already exposed by the 
    ''' System.Net.MailMessage.  It also provides support to encapsulate the
    ''' nested mail attachments in the Children collection.
    ''' </summary>
    Public Class MailMessageEx
        Inherits System.Net.Mail.MailMessage
        'Implements IDisposable

        Public Const EmailRegexPattern As String = "(['""]{1,}.+['""]{1,}\s+)?<?[\w\.\-]+@[^\.][\w\.\-]+\.[a-z]{2,}>?"

        Private m_Account As String     'Account utilizzato per scaricare il messaggio
        Private m_Folder As String
        'Private m_Message As System.Net.Mail.MailMessage    'Messaggio
        Private _octets As Long

        Public Sub New()
            'Me.m_Message = New 
            Me._children = New System.Collections.Generic.List(Of MailMessageEx)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="MailMessageEx"/> class.
        ''' </summary>
        Public Sub New(ByVal from As String, ByVal [to] As String)
            MyBase.New([from], [to])
            Me._children = New System.Collections.Generic.List(Of MailMessageEx)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="MailMessageEx"/> class.
        ''' </summary>
        Public Sub New(ByVal from As String, ByVal [to] As String, ByVal subject As String, ByVal body As String)
            MyBase.New([from], [to], subject, body)
            Me._children = New System.Collections.Generic.List(Of MailMessageEx)
        End Sub

        Public Sub New(ByVal from As System.Net.Mail.MailAddress, ByVal [to] As System.Net.Mail.MailAddress)
            MyBase.New([from], [to])
            Me._children = New System.Collections.Generic.List(Of MailMessageEx)
        End Sub

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

        Private _messageNumber As Integer

        ''' <summary>
        ''' Gets or sets the message number of the MailMessage on the POP3 server.
        ''' </summary>
        ''' <value>The message number.</value>
        Public Property MessageNumber As Integer
            Get
                Return Me._messageNumber
            End Get
            Friend Set(value As Integer)
                Me._messageNumber = value
            End Set
        End Property

        Private _children As System.Collections.Generic.List(Of MailMessageEx)
        ''' <summary>
        ''' Gets the children MailMessage attachments.
        ''' </summary>
        ''' <value>The children MailMessage attachments.</value>
        Public ReadOnly Property Children As System.Collections.Generic.List(Of MailMessageEx)
            Get
                Return Me._children
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
                Dim p As Integer = InStr(str, "(")
                If (p > 0) Then
                    Dim p1 As Integer = InStr(p, str, ")")
                    If (p1 > 0) Then
                        str = Left(str, p - 1) & Mid(str, p1 + 1)
                    End If
                End If
                Return Convert.ToDateTime(str)
            End Get
        End Property

        ''' <summary>
        ''' Gets the return address.
        ''' </summary>
        ''' <value>The return address.</value>
        Public ReadOnly Property ReturnAddress As MailAddress
            Get
                Dim replyTo As String = Me.GetHeader(MailHeaders.ReplyTo)
                If (String.IsNullOrEmpty(replyTo)) Then Return Nothing
                Return CreateMailAddress(replyTo)
            End Get
        End Property

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
                If (value.Equals(String.Empty)) Then
                    value = " "
                End If

                message.Headers.Add(key.ToLowerInvariant(), value)

                Select Case (key.ToLowerInvariant())
                    Case MailHeaders.Bcc
                        MailMessageEx.PopulateAddressList(value, message.Bcc)
                    Case MailHeaders.Cc
                        MailMessageEx.PopulateAddressList(value, message.Cc)
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
        Public Shared Function CreateMailAddress(ByVal address As String) As MailAddress
            Try
                Return New MailAddress(address.Trim(vbTab))
            Catch e As FormatException
                Throw New ArgumentException("Unable to create mail address from provided string: " & address, e)
            End Try
        End Function

        ''' <summary>
        ''' Populates the address list.
        ''' </summary>
        ''' <param name="addressList">The address list.</param>
        ''' <param name="recipients">The recipients.</param>
        Public Shared Sub PopulateAddressList(ByVal addressList As String, ByVal recipients As MailAddressCollection)
            For Each address As MailAddress In GetMailAddresses(addressList)
                recipients.Add(address)
            Next
        End Sub

        ''' <summary>
        ''' Gets the mail addresses.
        ''' </summary>
        ''' <param name="addressList">The address list.</param>
        ''' <returns></returns>
        Private Shared Function GetMailAddresses(ByVal addressList As String) As IEnumerable(Of MailAddress)
            Dim email As New Regex(EmailRegexPattern)
            Dim ret As New System.Collections.Generic.List(Of MailAddress)

            For Each match As Match In email.Matches(addressList)
                ret.Add(CreateMailAddress(match.Value))
            Next
#If DEBUG Then
            If (ret.Count > 1) Then
                Debug.Print(addressList)
            End If
#End If
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
        '    Return FinSeA.Databases.APPConn
        'End Function

        'Public Overrides Function GetModule() As Sistema.CModule
        '    Return FinSeA.Net.Mail.EMailMessages.Module
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
 
    End Class

End Namespace
