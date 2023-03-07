Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mail

namespace Net.Mime

    ''' <summary>
    ''' This class is responsible for parsing a string array of lines
    ''' containing a MIME message.
    ''' </summary>
    Public Class MimeReader

        Private Shared ReadOnly HeaderWhitespaceChars As Char() = New Char() {" ", vbTab}
        Private _entity As MimeEntity
        Private _lines As Queue(Of String)



        ''' <summary>
        ''' Initializes a new instance of the <see cref="MimeReader"/> class.
        ''' </summary>
        Private Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me._entity = New MimeEntity()
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)

        End Sub


        ''' <summary>
        ''' Initializes a new instance of the <see cref="MimeReader"/> class.
        ''' </summary>
        ''' <param name="entity">The entity.</param>
        ''' <param name="lines">The lines.</param>
        Private Sub New(ByVal entity As MimeEntity, ByVal lines As Queue(Of String))
            Me.New()
            If (entity Is Nothing) Then Throw New ArgumentNullException("entity")
            If (lines Is Nothing) Then Throw New ArgumentNullException("lines")
            Me._lines = lines
            Me._entity = New MimeEntity(entity)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="MimeReader"/> class.
        ''' </summary>
        ''' <param name="lines">The lines.</param>
        Public Sub New(ByVal lines As String())
            Me.New()
            If (lines Is Nothing) Then Throw New ArgumentNullException("lines")
            Me._lines = New Queue(Of String)(lines)
        End Sub


        ''' <summary>
        ''' Gets the lines.
        ''' </summary>
        ''' <value>The lines.</value>
        Public ReadOnly Property Lines As Queue(Of String)
            Get
                Return Me._lines
            End Get
        End Property

        ''' <summary>
        ''' Parse headers into _entity.Headers NameValueCollection.
        ''' </summary>
        Private Function ParseHeaders() As Integer
            Dim lastHeader As String = String.Empty
            Dim line As String = String.Empty
            ' the first empty line is the end of the headers.
            While (_lines.Count > 0 AndAlso Not String.IsNullOrEmpty(_lines.Peek()))
                line = _lines.Dequeue()
                'if a header line starts with a space or tab then it is a continuation of the
                'previous line.
                If (line.StartsWith(" ") Or line.StartsWith(Convert.ToString(vbTab))) Then
                    _entity.Headers(lastHeader) = String.Concat(_entity.Headers(lastHeader), line)
                    Continue While
                End If

                Dim separatorIndex As Integer = line.IndexOf(":")

                If (separatorIndex < 0) Then
                    System.Diagnostics.Debug.WriteLine("Invalid header:{0}", line)
                    Continue While
                End If  'This is an invalid header field.  Ignore this line.

                Dim headerName As String = line.Substring(0, separatorIndex)
                Dim headerValue As String = line.Substring(separatorIndex + 1).Trim(HeaderWhitespaceChars)

                _entity.Headers.Add(headerName.ToLower(), headerValue)
                lastHeader = headerName
            End While

            If (_lines.Count > 0) Then
                _lines.Dequeue()
            End If 'remove closing header CRLF.

            Return _entity.Headers.Count
        End Function

        ''' <summary>
        ''' Processes mime specific headers.
        ''' </summary>
        Private Sub ProcessHeaders()
            For Each key As String In Me._entity.Headers.AllKeys
                Select Case (key)
                    Case "content-description"
                        Me._entity.ContentDescription = _entity.Headers(key)
                    Case "content-disposition"
                        Me._entity.ContentDisposition = ParseContentDisposition(_entity.Headers(key))
                    Case "content-id"
                        Me._entity.ContentId = _entity.Headers(key)
                    Case "content-transfer-encoding"
                        Me._entity.TransferEncoding = _entity.Headers(key)
                        Me._entity.ContentTransferEncoding = MimeReader.GetTransferEncoding(_entity.Headers(key))
                    Case "content-type"
                        Me._entity.SetContentType(MimeReader.GetContentType(_entity.Headers(key)))
                    Case "mime-version"
                        Me._entity.MimeVersion = _entity.Headers(key)
                End Select
            Next
        End Sub

        Public Shared Function ParseContentDisposition(ByVal str As String) As ContentDisposition
            Try
                Return New ContentDisposition(str)
            Catch ex As Exception
                Sistema.ApplicationContext.Log("Parsing Content Disposition: " & str & vbCrLf & ex.Message & vbCrLf)
                str = Replace(str, ";" & vbTab, vbTab)
                str = Replace(str, ";", vbTab)

                Dim items() As String = Split(str, vbTab)
                Dim ret As New ContentDisposition(Trim(items(0)))
                For i As Integer = 1 To UBound(items)
                    Dim item As String = Trim(items(i))
                    Dim j As Integer
                    If (LCase(Left(item, Len("filename")) = "filename")) Then
                        j = Len("filename") + 1
                        While (j <= Len(item)) AndAlso (InStr(":=", Mid(item, j, 1)) <= 0)
                            j += 1
                        End While
                        If (j < Len(item)) Then
                            item = Trim(Mid(item, j + 1))
                            If Mid(item, 1, 1) = Chr(34) Then
                                ret.FileName = Mid(item, 2, Len(item) - 2)
                            Else
                                ret.FileName = item
                            End If

                        End If
                    Else
                        Debug.Print(item)
                    End If
                Next
                Return ret
            End Try
        End Function

        ''' <summary>
        ''' Creates the MIME entity.
        ''' </summary>
        ''' <returns>A mime entity containing 0 or more children representing the mime message.</returns>
        Public Function CreateMimeEntity() As MimeEntity
            Me.ParseHeaders()
            Me.ProcessHeaders()
            Me.ParseBody()
            Me.SetDecodedContentStream()
            Return Me._entity
        End Function


        ''' <summary>
        ''' Sets the decoded content stream by decoding the EncodedMessage 
        ''' and writing it to the entity content stream.
        ''' </summary>
        Private Sub SetDecodedContentStream()
            Try
                Select Case (Me._entity.ContentTransferEncoding)
                    Case System.Net.Mime.TransferEncoding.Base64
                        Me._entity.Content = New MemoryStream(Convert.FromBase64String(_entity.EncodedMessage.ToString()), False)
                    Case System.Net.Mime.TransferEncoding.QuotedPrintable
                        Dim tmp As String = "" & _entity.EncodedMessage.ToString()
                        tmp = Strings.Replace(tmp, "=" & vbCrLf, "")
                        If (tmp = "") Then
                            Me._entity.Content = New MemoryStream()
                        Else
                            Me._entity.Content = New MemoryStream(GetBytes(QuotedPrintableEncodingq.Decode(tmp)), False)
                        End If
                        'Me._entity.Content = New MemoryStream(GetBytes(MailHeaders.Decode(_entity.EncodedMessage.ToString())), False)
                        'Case System.Net.Mime.TransferEncoding.SevenBit
                    Case Else
                        Me._entity.Content = New MemoryStream(GetBytes(_entity.EncodedMessage.ToString()), False)
                End Select
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            End Try
            '#If DEBUG Then
            '            Dim txt As String = System.Text.Encoding.Default.GetString(Me._entity.Content.ToArray)
            '            If (InStr(txt, "=") > 0) Then
            '                Debug.Print(txt)
            '            End If

            '#End If
        End Sub

        ''' <summary>
        ''' Gets a byte[] of content for the provided string.
        ''' </summary>
        ''' <param name="content">Content.</param>
        ''' <returns>A byte[] containing content.</returns>
        Private Function GetBytes(ByVal content As String) As Byte()
            'Using stream As New MemoryStream()
            '    Using writer As New StreamWriter(stream)
            '        writer.Write(content)
            '    End Using
            '    Return stream.ToArray()
            'End Using
            Return System.Text.Encoding.ASCII.GetBytes(content)
        End Function

        Public Shared Function IsSameContentType(ByVal _entity As MimeEntity, ByVal content As String) As Boolean
            Return ((_entity.ContentType Is Nothing) AndAlso (content = "")) OrElse _
                   ((_entity.ContentType IsNot Nothing) AndAlso (String.Equals(_entity.ContentType.MediaType, content, StringComparison.InvariantCultureIgnoreCase)))
        End Function

        Public Shared Function IsSameContentDisposition(ByVal _entity As MimeEntity, ByVal content As String) As Boolean
            Return ((_entity.ContentDisposition Is Nothing) AndAlso (content = "")) OrElse _
                   ((_entity.ContentDisposition IsNot Nothing) AndAlso (String.Equals(_entity.ContentDisposition.DispositionType, content, StringComparison.InvariantCultureIgnoreCase)))
        End Function

            ''' <summary>
            ''' Parses the body.
            ''' </summary>
        Private Sub ParseBody()
            If (Me._entity.HasBoundary) Then
                While (Me._lines.Count > 0 AndAlso Not String.Equals(Me._lines.Peek(), Me._entity.EndBoundary))
                    'Check to verify the current line is not the same as the parent starting boundary.  
                    'If it is the same as the parent starting boundary this indicates existence of a 
                    'new child entity. Return and process the next child.
                    If (Me._entity.Parent IsNot Nothing AndAlso String.Equals(Me._entity.Parent.StartBoundary, Me._lines.Peek())) Then
                        Return
                    End If

                    If (_entity.ContentType Is Nothing) Then
                        Debug.Print("Content Type Is Null")
                    End If

                    If (_entity.ContentDisposition Is Nothing) Then
                        Debug.Print("Content Disposition Is Null")
                    End If

                    If (String.Equals(Me._lines.Peek(), Me._entity.StartBoundary)) Then
                        Me.AddChildEntity(Me._entity, Me._lines)
                        'Parse a new child mime part.
                    ElseIf ( _
                             IsSameContentType(_entity, MediaTypes.MessageRfc822) AndAlso _
                             IsSameContentDisposition(_entity, DispositionTypeNames.Attachment) _
                            ) Then
                        'If the content type is message/rfc822 the stop condition to parse headers has already been encountered.
                        'But, a content type of message/rfc822 would have the message headers immediately following the mime
                        'headers so we need to parse the headers for the attached message now.  This is done by creating
                        'a new child entity.
                        Me.AddChildEntity(Me._entity, Me._lines)
                        Exit While
                    Else
                        Me._entity.EncodedMessage.Append(String.Concat(Me._lines.Dequeue(), vbCrLf))
                    End If 'Append the message content.
                End While
                'Parse a multipart message.
            Else
                While (Me._lines.Count > 0)
                    Me._entity.EncodedMessage.Append(String.Concat(Me._lines.Dequeue(), vbCrLf))
                End While
            End If 'Parse a single part message.
        End Sub

        ''' <summary>
        ''' Adds the child entity.
        ''' </summary>
        ''' <param name="entity">The entity.</param>
        Private Sub AddChildEntity(ByVal entity As MimeEntity, ByVal lines As Queue(Of String))
            '/*if (entity == null)
            '{
            '    return;
            '}

            'if (lines == null)
            '{
            '    return;
            '}*/

            Dim reader As New MimeReader(entity, lines)
            entity.Children.Add(reader.CreateMimeEntity())
        End Sub

        ''' <summary>
        ''' Gets the type of the content.
        ''' </summary>
        ''' <param name="contentType">Type of the content.</param>
        ''' <returns></returns>
        Public Shared Function GetContentType(ByVal contentType As String) As ContentType
            Dim ret As ContentType = Nothing
            contentType = Trim(contentType)
            If (String.IsNullOrEmpty(contentType)) Then
                ret = New ContentType("text/plain; charset=us-ascii")
            Else
                'Try
                '    ret = New ContentType(cleanContentType(contentType))
                'Catch ex As Exception
                contentType = Replace(contentType, ";" & vbTab, vbTab)
                contentType = Replace(contentType, ";", vbTab)

                Dim items() As String = Split(contentType, vbTab)
                ret = New ContentType(Trim(items(0)))
                For i As Integer = 1 To UBound(items)
                    Dim item As String = Trim(items(i))
                    If (item <> "") Then
                        Dim j As Integer
                        If (LCase(Left(item, Len("charset"))) = "charset") Then
                            j = Len("charset") + 1
                            While (j <= Len(item)) AndAlso (InStr(":=", Mid(item, j, 1)) <= 0)
                                j += 1
                            End While
                            If (j < Len(item)) Then
                                item = Trim(Mid(item, j + 1))
                                If Mid(item, 1, 1) = Chr(34) Then
                                    ret.CharSet = Mid(item, 2, Len(item) - 2)
                                Else
                                    ret.CharSet = Trim(item)
                                End If
                            End If
                            Debug.Assert(ret.CharSet <> "")
                        ElseIf (LCase(Left(item, Len("boundary"))) = "boundary") Then
                            j = Len("boundary") + 1
                            While (j <= Len(item)) AndAlso (InStr(":=", Mid(item, j, 1)) <= 0)
                                j += 1
                            End While
                            If (j < Len(item)) Then
                                item = Trim(Mid(item, j + 1))
                                If Mid(item, 1, 1) = Chr(34) Then
                                    ret.Boundary = Mid(item, 2, Len(item) - 2)
                                Else
                                    ret.Boundary = Trim(item)
                                End If
                            End If
                            Debug.Assert(ret.Boundary <> "")
                        ElseIf (LCase(Left(item, Len("name"))) = "name") Then
                            j = Len("name") + 1
                            While (j <= Len(item)) AndAlso (InStr(":=", Mid(item, j, 1)) <= 0)
                                j += 1
                            End While
                            If (j < Len(item)) Then
                                item = Trim(Mid(item, j + 1))
                                If Mid(item, 1, 1) = Chr(34) Then
                                    ret.Name = Mid(item, 2, Len(item) - 2)
                                Else
                                    ret.Name = Trim(item)
                                End If
                            End If
                            Debug.Assert(ret.Name <> "")
                            'ElseIf (LCase(Left(item, Len("Content-Transfer-Encoding"))) = "content-transfer-encoding") Then
                            '    j = Len("content-transfer-encoding") + 1
                            '    While (j <= Len(item)) AndAlso (InStr(":=", Mid(item, j, 1)) <= 0)
                            '        j += 1
                            '    End While
                            '    item = Trim(Mid(item, j + 1))
                            '    If Mid(item, 1, 1) = Chr(34) Then
                            '        ret.Parameters.Add("Content-Transfer-Encoding", Mid(item, 2, Len(item) - 2))
                            '    Else
                            '        ret.Parameters.Add("Content-Transfer-Encoding", Trim(item))
                            '    End If
                        Else
                            Debug.Print(item)
                            'report-type=delivery-status
                            j = 1
                            While (j <= Len(item)) AndAlso (InStr(":=", Mid(item, j, 1)) <= 0)
                                j += 1
                            End While
                            Dim name As String = Trim(Left(item, j - 1))
                            item = Trim(Mid(item, j + 1))
                            If Mid(item, 1, 1) = Chr(34) Then
                                ret.Parameters.Add(name, Mid(item, 2, Len(item) - 2))
                            Else
                                ret.Parameters.Add(name, Trim(item))
                            End If


                        End If
                    End If
                Next
                'End Try
            End If

            Return ret
        End Function

        Private Shared Function cleanContentType(ByVal contentType As String) As String
            Dim cleanedContentType As New StringBuilder()
            'contentType = Replace(Trim(contentType), Chr(34), "")
            If (contentType.Contains(";")) Then 'It contains paramenter
                cleanedContentType.Append(contentType.Split(";")(0)).Append("; ")
                Dim items() As String = contentType.Split(";")
                If (items.Length > 1) Then
                    For i = 1 To items.Length - 1
                        If (items(i) <> "") Then cleanedContentType.Append(items(i).Replace(":", "=")).Append("; ")
                    Next
                End If
            Else
                Return contentType
            End If
            Return cleanedContentType.ToString()
        End Function

        ''' <summary>
        ''' Gets the type of the media.
        ''' </summary>
        ''' <param name="mediaType">Type of the media.</param>
        ''' <returns></returns>
        Public Shared Function GetMediaType(ByVal mediaType As String) As String
            If (String.IsNullOrEmpty(mediaType)) Then
                Return "text/plain"
            End If
            Return mediaType.Trim()
        End Function

        ''' <summary>
        ''' Gets the type of the media main.
        ''' </summary>
        ''' <param name="mediaType">Type of the media.</param>
        ''' <returns></returns>
        Public Shared Function GetMediaMainType(ByVal mediaType As String) As String
            Dim separatorIndex As Integer = mediaType.IndexOf("/")
            If (separatorIndex < 0) Then
                Return mediaType
            Else
                Return mediaType.Substring(0, separatorIndex)
            End If
        End Function

        ''' <summary>
        ''' Gets the type of the media sub.
        ''' </summary>
        ''' <param name="mediaType">Type of the media.</param>
        ''' <returns></returns>
        Public Shared Function GetMediaSubType(ByVal mediaType As String) As String
            Dim separatorIndex As Integer = mediaType.IndexOf("/")
            If (separatorIndex < 0) Then
                If (mediaType.Equals("text")) Then
                    Return "plain"
                End If
                Return String.Empty
            Else
                If (mediaType.Length > separatorIndex) Then
                    Return mediaType.Substring(separatorIndex + 1)
                Else
                    Dim mainType As String = GetMediaMainType(mediaType)
                    If (mainType.Equals("text")) Then
                        Return "plain"
                    End If
                    Return String.Empty
                End If
            End If
        End Function

        ''' <summary>
        ''' Gets the transfer encoding.
        ''' </summary>
        ''' <param name="transferEncoding">The transfer encoding.</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' The transfer encoding determination follows the same rules as 
        ''' Peter Huber's article w/ the exception of not throwing exceptions 
        ''' when binary is provided as a transferEncoding.  Instead it is left
        ''' to the calling code to check for binary.
        ''' </remarks>
        Public Shared Function GetTransferEncoding(ByVal transferEncoding As String) As TransferEncoding
            Select Case (transferEncoding.Trim().ToLowerInvariant())
                Case "7bit", "8bit"
                    Return System.Net.Mime.TransferEncoding.SevenBit
                Case "quoted-printable"
                    Return System.Net.Mime.TransferEncoding.QuotedPrintable
                Case "base64"
                    Return System.Net.Mime.TransferEncoding.Base64
                    'Case "binary"
                Case Else
                    Return System.Net.Mime.TransferEncoding.Unknown
            End Select
        End Function

    End Class

End Namespace