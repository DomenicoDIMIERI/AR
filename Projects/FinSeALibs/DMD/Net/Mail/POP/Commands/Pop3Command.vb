Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This class represents a generic Pop3 command and 
    ''' encapsulates the major operations when executing a
    ''' Pop3 command against a Pop3 Server.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    Friend MustInherit Class Pop3Command(Of T As Pop3Response)
        Implements IDisposable


        Public Event Trace As Action(Of String)



        'Private Const READTIMEOUT As Integer = 10 * 1000
        Private Const BufferSize As Integer = 2048
        Private Const MultilineMessageTerminator As String = vbCr & vbLf & "." & vbCr & vbLf ' "\r\n.\r\n"
        Private Const MessageTerminator As String = "."

        Private _manualResetEvent As ManualResetEvent
        Private _buffer As Byte()
        Private _responseContents As MemoryStream
        Private _validExecuteState As Pop3State
        Private _networkStream As Stream
        Private m_Client As Pop3Client


        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me._manualResetEvent = New ManualResetEvent(False)
            ReDim Me._buffer(BufferSize - 1)
            Me._responseContents = New MemoryStream()
            Me._networkStream = Nothing
            Me._isMultiline = IsMultiline
            Me._validExecuteState = ValidExecuteState
            Me.m_Client = Nothing
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="Pop3Command"/> class.
        ''' </summary>
        ''' <param name="stream">The stream.</param>
        ''' <param name="isMultiline">if set to <c>true</c> [is multiline].</param>
        ''' <param name="validExecuteState">State of the valid execute.</param>
        Public Sub New(ByVal client As Pop3Client, ByVal stream As Stream, ByVal isMultiline As Boolean, ByVal validExecuteState As Pop3State)
            Me.New
            If (client Is Nothing) Then Throw New ArgumentNullException("client")
            If (stream Is Nothing) Then Throw New ArgumentNullException("stream")
            Me._manualResetEvent = New ManualResetEvent(False)
            ReDim Me._buffer(BufferSize - 1)
            Me._responseContents = New MemoryStream()
            Me._networkStream = stream
            Me._isMultiline = isMultiline
            Me._validExecuteState = validExecuteState
            Me.m_Client = client
        End Sub

        Public ReadOnly Property Client As Pop3Client
            Get
                Return Me.m_Client
            End Get
        End Property


        Public ReadOnly Property ValidExecuteState As Pop3State
            Get
                Return Me._validExecuteState
            End Get
        End Property

        Public Property NetworkStream As Stream
            Get
                Return Me._networkStream
            End Get
            Set(value As Stream)
                Me._networkStream = value
            End Set
        End Property


        Private _isMultiline As Boolean
        ''' <summary>
        ''' Sets a value indicating whether this instance is multiline.
        ''' </summary>
        ''' <value>
        ''' 	<c>true</c> if this instance is multiline; otherwise, <c>false</c>.
        ''' </value>
        Protected Property IsMultiline As Boolean
            Get
                Return Me._isMultiline
            End Get
            Set(value As Boolean)
                Me._isMultiline = value
            End Set
        End Property

       

        ''' <summary>
        ''' Abstract method intended for inheritors to 
        ''' build out the byte[] request message for 
        ''' the specific command.
        ''' </summary>
        ''' <returns>The byte[] containing the request message.</returns>
        Protected MustOverride Function CreateRequestMessage() As Byte()

        ''' <summary>
        ''' Sends the specified message.
        ''' </summary>
        ''' <param name="message">The message.</param>
        Private Sub Send(ByVal message As Byte())
            'EnsureConnection();
            Try
                Me._networkStream.Write(message, 0, message.Length)
            Catch e As SocketException
                Throw New Pop3Exception("Unable to send the request message: " & System.Text.Encoding.ASCII.GetString(message), e)
            End Try
        End Sub

        ''' <summary>
        ''' Executes this instance.
        ''' </summary>
        ''' <returns></returns>
        Friend Overridable Function Execute(ByVal currentState As Pop3State) As T
            Me.EnsurePop3State(currentState)
            Dim message As Byte() = Me.CreateRequestMessage()
            If (message IsNot Nothing) Then
                Me.Send(message)
            End If
            Dim response As T = Me.CreateResponse(Me.GetResponse())
            If (response Is Nothing) Then
                Return Nothing
            End If
            Me.OnTrace(response.HostMessage)
            Return response
        End Function

        ''' <summary>
        ''' Ensures the state of the POP3.
        ''' </summary>
        ''' <param name="currentState">State of the current.</param>
        Protected Sub EnsurePop3State(ByVal currentState As Pop3State)
            If (Not ((currentState And ValidExecuteState) = currentState)) Then
                Throw New Pop3Exception(String.Format("This command is being executed in an invalid execution state.  Current:{0}, Valid:{1}", currentState, ValidExecuteState))
            End If
        End Sub

        ''' <summary>
        ''' Creates the response.
        ''' </summary>
        ''' <param name="buffer">The buffer.</param>
        ''' <returns>The <c>Pop3Response</c> containing the results of the
        ''' Pop3 command execution.</returns>
        Protected Overridable Function CreateResponse(ByVal buffer As Byte()) As T
            Return Pop3Response.CreateResponse(buffer)
        End Function

        ''' <summary>
        ''' Gets the response.
        ''' </summary>
        ''' <returns></returns>
        Private Function GetResponse() As Byte()
            'EnsureConnection()
            If Me.Client.Async Then
                Dim callback As AsyncCallback
                If (Me._isMultiline) Then
                    callback = New AsyncCallback(AddressOf GetMultiLineResponseCallback)
                Else
                    callback = New AsyncCallback(AddressOf GetSingleLineResponseCallback)
                End If

                Try
                    AsyncReceive(callback)
                    _manualResetEvent.WaitOne(Me.Client.TimeOut * 1000)
                    Return _responseContents.ToArray()
                Catch e As SocketException
                    Throw New Pop3Exception("Unable to get response.", e)
                End Try
            Else
                Do
                    _networkStream.ReadTimeout = Me.Client.TimeOut * 1000
                    Dim bytesReceived As Integer = _networkStream.Read(_buffer, 0, _buffer.Length)
                    Dim message As String

                    If (bytesReceived > 0) Then
                        message = WriteReceivedBytesToBuffer(bytesReceived)
                        If (Me._isMultiline) Then
                            If (message.EndsWith(MultilineMessageTerminator)) Then 'if the POP3 server times out we'll get an error message, then we'll get a following callback w/ 0 bytes.
                                Exit Do
                            End If
                        Else
                            If (message.EndsWith(Pop3Commands.Crlf)) Then
                                Exit Do
                            End If
                        End If
                    Else
                        Exit Do
                    End If
                Loop
                Return _responseContents.ToArray()
            End If
        End Function

        ''' <summary>
        ''' Receives the specified callback.
        ''' </summary>
        ''' <param name="callback">The callback.</param>
        ''' <returns></returns>
        Private Function AsyncReceive(ByVal callback As AsyncCallback) As IAsyncResult
            Return _networkStream.BeginRead(_buffer, 0, _buffer.Length, callback, Nothing)
        End Function

        ''' <summary>
        ''' Writes the received bytes to buffer.
        ''' </summary>
        ''' <param name="bytesReceived">The bytes received.</param>
        ''' <returns></returns>
        Private Function WriteReceivedBytesToBuffer(ByVal bytesReceived As Integer) As String
            _responseContents.Write(_buffer, 0, bytesReceived)
            Dim contents As Byte() = _responseContents.ToArray()
            Return System.Text.Encoding.ASCII.GetString(contents, IIf(contents.Length > 5, contents.Length - 5, 0), 5)
        End Function

        ''' <summary>
        ''' Gets the single line response callback.
        ''' </summary>
        ''' <param name="ar">The ar.</param>
        Private Sub GetSingleLineResponseCallback(ByVal ar As IAsyncResult)
            If (_networkStream Is Nothing) Then Exit Sub
            
            Dim bytesReceived As Integer = _networkStream.EndRead(ar)
            Dim message As String = WriteReceivedBytesToBuffer(bytesReceived)
            If (message.EndsWith(Pop3Commands.Crlf)) Then
                _manualResetEvent.Set()
            Else
                AsyncReceive(New AsyncCallback(AddressOf GetSingleLineResponseCallback))
            End If
        End Sub

        ''' <summary>
        ''' Gets the multi line response callback.
        ''' </summary>
        ''' <param name="ar">The ar.</param>
        Private Sub GetMultiLineResponseCallback(ByVal ar As IAsyncResult)
            If (Me._networkStream Is Nothing OrElse Me._manualResetEvent IsNot Nothing) Then Exit Sub

            Dim bytesReceived As Integer = _networkStream.EndRead(ar)
            Dim message As String = WriteReceivedBytesToBuffer(bytesReceived)
            If (message.EndsWith(MultilineMessageTerminator) Or bytesReceived = 0) Then 'if the POP3 server times out we'll get an error message, then we'll get a following callback w/ 0 bytes.
                _manualResetEvent.Set()
            Else
                AsyncReceive(New AsyncCallback(AddressOf GetMultiLineResponseCallback))
            End If
        End Sub


        ''' <summary>
        ''' Gets the request message.
        ''' </summary>
        ''' <param name="args">The args.</param>
        ''' <returns>A byte[] request message to send to the host.</returns>
        Protected Function GetRequestMessage(ByVal ParamArray args() As String) As Byte()
            Dim message As String = String.Join(String.Empty, args)
            Me.OnTrace(message)
            Return System.Text.Encoding.ASCII.GetBytes(message)
        End Function

        ''' <summary>
        ''' Strips the POP3 host message.
        ''' </summary>
        ''' <param name="bytes">The bytes.</param>
        ''' <param name="header">The header.</param>
        ''' <returns>A <c>MemoryStream</c> without the Pop3 server message.</returns>
        Protected Function StripPop3HostMessage(ByVal bytes As Byte(), ByVal header As String) As MemoryStream
            Dim position As Integer = header.Length + 2
            Dim stream As New MemoryStream(bytes, position, bytes.Length - position)
            Return stream
        End Function

        ''' <summary>
        ''' Gets the response lines.
        ''' </summary>
        ''' <param name="stream">The stream.</param>
        ''' <returns>A string[] of Pop3 response lines.</returns>
        Protected Function GetResponseLines(ByVal stream As MemoryStream) As String()
            Dim reader As StreamReader = Nothing
            Try
                reader = New StreamReader(stream, System.Text.Encoding.ASCII)
                Dim lines As New System.Collections.Generic.List(Of String)
                Dim line As String

                Do
                    line = reader.ReadLine()
                    'pop3 protocol states if a line starts w/ a 
                    ''.' that line will be byte stuffed w/ a '.'
                    'if it is byte stuffed the remove the byte, 
                    'otherwise we have reached the end of the message.
                    If (line.StartsWith(MessageTerminator)) Then
                        If (line = MessageTerminator) Then
                            Exit Do
                        End If
                        line = line.Substring(1)
                    End If
                    lines.Add(line)
                Loop While (True)
                Return lines.ToArray()
            Catch e As IOException
                Throw New Pop3Exception("Unable to get response lines.", e)
            Finally
                If (reader IsNot Nothing) Then reader.Dispose()
            End Try
        End Function






        Public Sub Dispose() Implements IDisposable.Dispose
            If (Me._buffer IsNot Nothing) Then Erase Me._buffer : Me._buffer = Nothing
            If (Me._manualResetEvent IsNot Nothing) Then Me._manualResetEvent.Dispose() : Me._manualResetEvent = Nothing
            If (Me._networkStream IsNot Nothing) Then Me._networkStream = Nothing :             'Me._networkStream.Dispose()
            If (Me.m_Client IsNot Nothing) Then Me.m_Client = Nothing
            If (Me._responseContents IsNot Nothing) Then Me._responseContents.Dispose() : Me._responseContents = Nothing


        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Protected Sub OnTrace(ByVal message As String)
            RaiseEvent Trace(message)
        End Sub
    End Class

End Namespace