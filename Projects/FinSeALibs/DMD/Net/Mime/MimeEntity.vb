Imports System
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Text
Imports System.IO
Imports System.Net.Mime
Imports System.Net.Mail
Imports DMD.Net.Mail

namespace Net.Mime

    ''' <summary>
    ''' This class represents a Mime entity.
    ''' </summary>
    Public Class MimeEntity
        Implements IDisposable

        'Private Shared usC As New Globalization.CultureInfo("en-us", False)

        Private _encodedMessage As StringBuilder
        Private _children As System.Collections.Generic.List(Of MimeEntity)
        Private _contentType As ContentType
        Private _mediaSubType As String
        Private _mediaMainType As String
        Private _headers As NameValueCollection
        Private _mimeVersion As String
        Private _contentId As String


        Private _contentDescription As String
        Private _contentDisposition As ContentDisposition
        Private _transferEncoding As String
        Private _contentTransferEncoding As TransferEncoding
        Private _startBoundary As String
        Private _parent As MimeEntity
        Private _content As MemoryStream

        ''' <summary>
        ''' Initializes a new instance of the <see cref="MimeEntity"/> class.
        ''' </summary>
        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me._children = New System.Collections.Generic.List(Of MimeEntity)
            Me._headers = New NameValueCollection
            Me._contentType = MimeReader.GetContentType(String.Empty)
            Me._parent = Nothing
            Me._encodedMessage = New StringBuilder
        End Sub



        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub


        ''' <summary>
        ''' Initializes a new instance of the <see cref="MimeEntity"/> class.
        ''' </summary>
        ''' <param name="parent">The parent.</param>
        Public Sub New(ByVal parent As MimeEntity)
            Me.New()
            If (parent Is Nothing) Then Throw New ArgumentNullException("parent")
            Me._parent = parent
            Me._startBoundary = parent.StartBoundary
        End Sub

        ''' <summary>
        ''' Gets the encoded message.
        ''' </summary>
        ''' <value>The encoded message.</value>
        Public ReadOnly Property EncodedMessage As StringBuilder
            Get
                Return Me._encodedMessage
            End Get
        End Property



        ''' <summary>
        ''' Gets the children.
        ''' </summary>
        ''' <value>The children.</value>
        Public ReadOnly Property Children As System.Collections.Generic.List(Of MimeEntity)
            Get
                Return Me._children
            End Get
        End Property


        ''' <summary>
        ''' Gets the type of the content.
        ''' </summary>
        ''' <value>The type of the content.</value>
        Public ReadOnly Property ContentType As ContentType
            Get
                Return Me._contentType
            End Get
        End Property

        ''' <summary>
        ''' Gets the type of the media sub.
        ''' </summary>
        ''' <value>The type of the media sub.</value>
        Public ReadOnly Property MediaSubType As String
            Get
                Return Me._mediaSubType
            End Get
        End Property

        ''' <summary>
        ''' Gets the type of the media main.
        ''' </summary>
        ''' <value>The type of the media main.</value>
        Public ReadOnly Property MediaMainType As String
            Get
                Return Me._mediaMainType
            End Get
        End Property

        ''' <summary>
        ''' Gets the headers.
        ''' </summary>
        ''' <value>The headers.</value>
        Public ReadOnly Property Headers As NameValueCollection
            Get
                Return Me._headers
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the MIME version.
        ''' </summary>
        ''' <value>The MIME version.</value>
        Public Property MimeVersion As String
            Get
                Return Me._mimeVersion
            End Get
            Set(ByVal value As String)
                Me._mimeVersion = value
            End Set
        End Property


        ''' <summary>
        ''' Gets or sets the content id.
        ''' </summary>
        ''' <value>The content id.</value>
        Public Property ContentId As String
            Get
                Return Me._contentId
            End Get
            Set(value As String)
                Me._contentId = value
            End Set
        End Property


        ''' <summary>
        ''' Gets or sets the content description.
        ''' </summary>
        ''' <value>The content description.</value>
        Public Property ContentDescription As String
            Get
                Return Me._contentDescription
            End Get
            Set(value As String)
                Me._contentDescription = value
            End Set
        End Property


        ''' <summary>
        ''' Gets or sets the content disposition.
        ''' </summary>
        ''' <value>The content disposition.</value>
        Public Property ContentDisposition As ContentDisposition
            Get
                Return Me._contentDisposition
            End Get
            Set(value As ContentDisposition)
                Me._contentDisposition = value
            End Set
        End Property



        ''' <summary>
        ''' Gets or sets the transfer encoding.
        ''' </summary>
        ''' <value>The transfer encoding.</value>
        Public Property TransferEncoding As String
            Get
                Return Me._transferEncoding
            End Get
            Set(value As String)
                Me._transferEncoding = value
            End Set
        End Property


        ''' <summary>
        ''' Gets or sets the content transfer encoding.
        ''' </summary>
        ''' <value>The content transfer encoding.</value>
        Public Property ContentTransferEncoding As TransferEncoding
            Get
                Return Me._contentTransferEncoding
            End Get
            Set(value As TransferEncoding)
                Me._contentTransferEncoding = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a value indicating whether this instance has boundary.
        ''' </summary>
        ''' <value>
        ''' 	<c>true</c> if this instance has boundary; otherwise, <c>false</c>.
        ''' </value>
        Friend ReadOnly Property HasBoundary As Boolean
            Get
                Return (Not String.IsNullOrEmpty(_contentType.Boundary)) Or (Not String.IsNullOrEmpty(_startBoundary))
            End Get
        End Property


        ''' <summary>
        ''' Gets the start boundary.
        ''' </summary>
        ''' <value>The start boundary.</value>
        Public ReadOnly Property StartBoundary As String
            Get
                If (String.IsNullOrEmpty(_startBoundary) Or Not String.IsNullOrEmpty(_contentType.Boundary)) Then
                    Return String.Concat("--", _contentType.Boundary)
                End If
                Return Me._startBoundary
            End Get
        End Property

        ''' <summary>
        ''' Gets the end boundary.
        ''' </summary>
        ''' <value>The end boundary.</value>
        Public ReadOnly Property EndBoundary As String
            Get
                Return String.Concat(StartBoundary, "--")
            End Get
        End Property


        ''' <summary>
        ''' Gets or sets the parent.
        ''' </summary>
        ''' <value>The parent.</value>
        Public Property Parent As MimeEntity
            Get
                Return Me._parent
            End Get
            Set(value As MimeEntity)
                Me._parent = value
            End Set
        End Property


        ''' <summary>
        ''' Gets or sets the content.
        ''' </summary>
        ''' <value>The content.</value>
        Public Property Content As MemoryStream
            Get
                Return Me._content
            End Get
            Friend Set(value As MemoryStream)
                Me._content = value
            End Set
        End Property


        ''' <summary>
        ''' Sets the type of the content.
        ''' </summary>
        ''' <param name="contentType">Type of the content.</param>
        Friend Sub SetContentType(ByVal contentType As ContentType)
            Me._contentType = contentType
            Me._contentType.MediaType = MimeReader.GetMediaType(contentType.MediaType)
            Me._mediaMainType = MimeReader.GetMediaMainType(contentType.MediaType)
            Me._mediaSubType = MimeReader.GetMediaSubType(contentType.MediaType)
        End Sub

        ''' <summary>
        ''' Toes the mail message ex.
        ''' </summary>
        ''' <returns></returns>
        Public Function ToMailMessageEx() As MailMessageEx
            Return Me.ToMailMessageEx(Me)
        End Function

        ''' <summary>
        ''' Toes the mail message ex.
        ''' </summary>
        ''' <param name="entity">The entity.</param>
        ''' <returns></returns>
        Private Function ToMailMessageEx(ByVal entity As MimeEntity) As MailMessageEx
            If (entity Is Nothing) Then Throw New ArgumentNullException("entity")
            'parse standard headers and create base email.
            Dim message As MailMessageEx = MailMessageEx.CreateMailMessageFromEntity(entity)

            If (Not String.IsNullOrEmpty(entity.ContentType.Boundary)) Then
                message = MailMessageEx.CreateMailMessageFromEntity(entity)
                Me.BuildMultiPartMessage(entity, message)
                'parse multipart message into sub parts.
            ElseIf (String.Equals(entity.ContentType.MediaType, MediaTypes.MessageRfc822, StringComparison.InvariantCultureIgnoreCase)) Then
                'use the first child to create the multipart message.
                If (entity.Children.Count < 0) Then Throw New Protocols.POP3.Pop3Exception("Invalid child count on message/rfc822 entity.")
                'create the mail message from the first child because it will
                'contain all of the mail headers.  The entity in this state
                'only contains simple content type headers indicating, disposition, type and description.
                'This means we can't create the mail message from this type as there is no 
                'internet mail headers attached to this entity.
                message = MailMessageEx.CreateMailMessageFromEntity(entity.Children(0))
                Me.BuildMultiPartMessage(entity, message)
                'parse nested message.
            Else
                message = MailMessageEx.CreateMailMessageFromEntity(entity)
                Me.BuildSinglePartMessage(entity, message)
            End If  'Create single part message.

            Return message
        End Function

        ''' <summary>
        ''' Builds the single part message.
        ''' </summary>
        ''' <param name="entity">The entity.</param>
        ''' <param name="message">The message.</param>
        Private Sub BuildSinglePartMessage(ByVal entity As MimeEntity, ByVal message As MailMessageEx)
            Me.SetMessageBody(message, entity)
        End Sub


        ''' <summary>
        ''' Gets the body encoding.
        ''' </summary>
        Public Function GetEncoding() As System.Text.Encoding
            If (String.IsNullOrEmpty(Me.ContentType.CharSet)) Then
                Return System.Text.Encoding.ASCII
            Else
                Try
                    Return System.Text.Encoding.GetEncoding(Me.ContentType.CharSet)
                Catch e As ArgumentException
                    Return System.Text.Encoding.ASCII
                End Try
            End If
        End Function

        ''' <summary>
        ''' Builds the multi part message.
        ''' </summary>
        ''' <param name="entity">The entity.</param>
        ''' <param name="message">The message.</param>
        Private Sub BuildMultiPartMessage(ByVal entity As MimeEntity, ByVal message As MailMessageEx)
            For Each child As MimeEntity In entity.Children
                If (Me.IsMultipart(child)) Then
                    Me.BuildMultiPartMessage(child, message)
                    'if the message is mulitpart/alternative or multipart/mixed then the entity will have children needing parsed.
                ElseIf (Not IsAttachment(child) AndAlso Me.IsPlainTextOrHTML(child)) Then
                    message.AlternateViews.Add(Me.CreateAlternateView(child))
                    Me.SetMessageBody(message, child)
                    'add the alternative views.
                ElseIf (Me.IsChild(child)) Then
                    message.Children.Add(Me.ToMailMessageEx(child))
                    'create a child message and 
                ElseIf (IsAttachment(child)) Then
                    message.Attachments.Add(Me.CreateAttachment(child))
                Else
                    'Debug.Assert(False)
                    Debug.Print("Codice non riconosciuto")
                    Try
                        message.Attachments.Add(Me.CreateAttachment(child))
                    Catch ex As Exception
                        Sistema.Events.NotifyUnhandledException(ex)
                        Debug.Print("Non sono riuscito ad interpretare")
                    End Try
                End If
            Next
        End Sub

        Private Function IsChild(ByVal child As MimeEntity) As Boolean
            Return MimeReader.IsSameContentType(child, MediaTypes.MessageRfc822) AndAlso _
                   MimeReader.IsSameContentDisposition(child, DispositionTypeNames.Attachment)
        End Function

        Private Function IsMultipart(ByVal child As MimeEntity) As Boolean
            If (Strings.StrComp(child.ContentType.MediaType, MediaTypes.MultipartAlternative) = 0) Then Return True ', StringComparison.InvariantCultureIgnoreCase) 
            If (Strings.StrComp(child.ContentType.MediaType, MediaTypes.MultipartMixed) = 0) Then Return True ', StringComparison.InvariantCultureIgnoreCase)
            Return False
        End Function

        Private Function IsPlainTextOrHTML(ByVal child As MimeEntity) As Boolean
            If (Strings.StrComp(child.ContentType.MediaType, MediaTypes.TextPlain, CompareMethod.Text) = 0) Then Return True
            If (Strings.StrComp(child.ContentType.MediaType, MediaTypes.TextHtml, CompareMethod.Text) = 0) Then Return True
            Return False
        End Function

        Private Shared Function IsAttachment(ByVal child As MimeEntity) As Boolean
            If (child.ContentDisposition IsNot Nothing) Then
                Return (String.Equals(child.ContentDisposition.DispositionType, DispositionTypeNames.Attachment, StringComparison.InvariantCultureIgnoreCase))
            Else
                Select Case LCase(child.ContentType.MediaType)
                    Case "text/plain", "text", "text/html" : Return False
                    Case Else : Return True
                End Select
            End If
        End Function

        ''' <summary>
        ''' Sets the message body.
        ''' </summary>
        ''' <param name="message">The message.</param>
        ''' <param name="child">The child.</param>
        Private Sub SetMessageBody(ByVal message As MailMessageEx, ByVal child As MimeEntity)
            Dim encoding As System.Text.Encoding = child.GetEncoding()
            message.Body = Me.DecodeBytes(child.Content.ToArray(), encoding)
            message.BodyEncoding = encoding
            message.IsBodyHtml = String.Equals(MediaTypes.TextHtml, child.ContentType.MediaType, StringComparison.InvariantCultureIgnoreCase)
        End Sub

        ''' <summary>
        ''' Decodes the bytes.
        ''' </summary>
        ''' <param name="buffer">The buffer.</param>
        ''' <param name="encoding">The encoding.</param>
        ''' <returns></returns>
        Private Function DecodeBytes(ByVal buffer As Byte(), ByVal encoding As System.Text.Encoding) As String
            If (buffer Is Nothing) Then
                Return Nothing
            End If

            If (encoding Is Nothing) Then
                encoding = System.Text.Encoding.UTF7
            End If  'email defaults to 7bit.  

            Dim ret As String = encoding.GetString(buffer)
            If (ret.IndexOf("Ã") >= 0) Then
                Debug.Print("Opps")

                Dim infos() As System.Text.EncodingInfo = System.Text.Encoding.GetEncodings()
                For Each info In infos
                    Try
                        Debug.Print(info.CodePage & " - " & info.Name & " - " & info.GetEncoding.GetString(buffer))
                    Catch ex As Exception

                    End Try

                Next

            End If
            Return ret
        End Function

        ''' <summary>
        ''' Creates the alternate view.
        ''' </summary>
        ''' <param name="view">The view.</param>
        ''' <returns></returns>
        Private Function CreateAlternateView(ByVal view As MimeEntity) As AlternateViewEx
            Dim stream As New MemoryStream
            view.Content.Position = 0
            DMD.Sistema.FileSystem.CopyStream(view.Content, stream)

            Dim alternateView As New AlternateViewEx(stream, view.ContentType)
            alternateView.TransferEncoding = view.ContentTransferEncoding
            alternateView.ContentId = TrimBrackets(view.ContentId)
            Return alternateView
        End Function

        ''' <summary>
        ''' Trims the brackets.
        ''' </summary>
        ''' <param name="value">The value.</param>
        ''' <returns></returns>
        Public Shared Function TrimBrackets(ByVal value As String) As String
            If (value Is Nothing) Then
                Return value
            End If

            If (value.StartsWith("<") AndAlso value.EndsWith(">")) Then
                Return value.Trim("<", ">")
            End If

            Return value
        End Function

        Private Function RemoveStrangeChars(ByVal value As String) As String
            Dim ret As New System.Text.StringBuilder(value.Length + 1)
            For Each ch As Char In value
                Select Case ch
                    Case vbTab,
                         vbFormFeed,
                         vbCr,
                         vbLf
                    Case Else
                        ret.Append(ch)
                End Select
            Next
            Return ret.ToString
        End Function

        ''' <summary>
        ''' Creates the attachment.
        ''' </summary>
        ''' <param name="entity">The entity.</param>
        ''' <returns></returns>
        Private Function CreateAttachment(ByVal entity As MimeEntity) As AttachmentEx
            Dim content As MemoryStream = Nothing
#If Not DEBUG Then
            Try
#End If
            If (entity Is Nothing) Then Throw New ArgumentNullException("entity")
            If (entity.Content Is Nothing) Then Throw New ArgumentNullException("entity")

            content = New MemoryStream()
            entity.Content.Position = 0
            DMD.Sistema.FileSystem.CopyStream(entity.Content, content)

            Dim attachment As New AttachmentEx(content, entity.ContentType)

                If (entity.ContentDisposition IsNot Nothing) Then
                    attachment.ContentDisposition.Parameters.Clear()
                    For Each key As String In entity.ContentDisposition.Parameters.Keys
                        Dim kVal As String = entity.ContentDisposition.Parameters(key)
                        Select Case LCase(key)
                            Case "creation-date", "modification-date", "read-date"
                            'attachment.ContentDisposition.Parameters.Add(key, kVal)
                        Case Else
                                attachment.ContentDisposition.Parameters.Add(key, kVal)
                        End Select
                    Next

                    attachment.ContentDisposition.CreationDate = entity.ContentDisposition.CreationDate
                attachment.ContentDisposition.DispositionType = entity.ContentDisposition.DispositionType
                attachment.ContentDisposition.FileName = entity.ContentDisposition.FileName
                attachment.ContentDisposition.Inline = entity.ContentDisposition.Inline
                attachment.ContentDisposition.ModificationDate = entity.ContentDisposition.ModificationDate
                attachment.ContentDisposition.ReadDate = entity.ContentDisposition.ReadDate
                attachment.ContentDisposition.Size = entity.ContentDisposition.Size
            End If

            If (Not String.IsNullOrEmpty(entity.ContentId)) Then
                attachment.ContentId = TrimBrackets(entity.ContentId)
            End If

            attachment.TransferEncoding = entity.ContentTransferEncoding

            Return attachment
#If Not DEBUG Then
            Catch ex As Exception
                If (content IsNot Nothing) Then content.Dispose() : content = Nothing
                Throw
            End Try
#End If
        End Function


        'Private Function ParseUsaDate(ByVal value As String) As String
        '    Try
        '        Return Date.Parse(value, usC).ToString(usC)
        '    Catch ex As Exception
        '        Sistema.Events.NotifyUnhandledException(New FormatException("Data non valida: " & value, ex))
        '        Throw
        '    End Try
        'End Function

        Public Overridable Sub Dispose() Implements IDisposable.Dispose
            If (Me._encodedMessage IsNot Nothing) Then Me._encodedMessage.Clear() : Me._encodedMessage = Nothing
            If (Me._children IsNot Nothing) Then
                For Each c As MimeEntity In Me._children
                    c.Dispose()
                Next
                Me._children = Nothing
            End If
            Me._headers = Nothing
            Me._parent = Nothing
            If (Me._content IsNot Nothing) Then Me._content.Dispose() : Me._content = Nothing

            Me._contentType = Nothing
            Me._mediaSubType = vbNullString
            Me._mediaMainType = vbNullString
            Me._mimeVersion = vbNullString
            Me._contentId = vbNullString


            Me._contentDescription = vbNullString
            Me._contentDisposition = Nothing
            Me._transferEncoding = vbNullString
            Me._contentTransferEncoding = vbNullString
            Me._startBoundary = vbNullString

        End Sub


    End Class

End Namespace
