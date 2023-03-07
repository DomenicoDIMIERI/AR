Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Sockets
Imports System.Net.Security
Imports System.Security.Authentication
Imports FinSeA.Net.Mime

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' The Pop3Client class provides a wrapper for the Pop3 commands
    ''' that can be executed against a Pop3Server.  This class will 
    ''' execute and return results for the various commands that are 
    ''' executed.
    ''' </summary>
    Public NotInheritable Class Pop3Protocol
        Implements IDisposable

        Private Shared ReadOnly DefaultPort As Integer = 110

        Private _client As TcpClient
        Private _clientStream As Stream

        ''' <summary>
        ''' Traces the various command objects that executed during this objects
        ''' lifetime.
        ''' </summary>
        Public Event Trace As Action(Of String)

        Private Sub OnTrace(ByVal message As String)
            RaiseEvent Trace(message)
        End Sub

        Private _hostname As String

        ''' <summary>
        ''' Gets the hostname.
        ''' </summary>
        ''' <value>The hostname.</value>
        Public ReadOnly Property Hostname As String
            Get
                Return Me._hostname
            End Get
        End Property

        Private _port As Integer
        ''' <summary>
        ''' Gets the port.
        ''' </summary>
        ''' <value>The port.</value>
        Public ReadOnly Property Port As Integer
            Get
                Return Me._port
            End Get
        End Property

        Private _useSsl As Boolean
        ''' <summary>
        ''' Gets a value indicating whether [use SSL].
        ''' </summary>
        ''' <value><c>true</c> if [use SSL]; otherwise, <c>false</c>.</value>
        Public ReadOnly Property UseSsl As Boolean
            Get
                Return Me._useSsl
            End Get
        End Property

        Private _username As String
        ''' <summary>
        ''' Gets or sets the username.
        ''' </summary>
        ''' <value>The username.</value>
        Public Property Username As String
            Get
                Return Me._username
            End Get
            Set(value As String)
                Me._username = value
            End Set
        End Property

        Private _password As String
        ''' <summary>
        ''' Gets or sets the password.
        ''' </summary>
        ''' <value>The password.</value>
        Public Property Password As String
            Get
                Return Me._password
            End Get
            Set(value As String)
                Me._password = value
            End Set
        End Property


        Private _currentState As Pop3State
        ''' <summary>
        ''' Gets the state of the current.
        ''' </summary>
        ''' <value>The state of the current.</value>
        Public ReadOnly Property CurrentState As Pop3State
            Get
                Return Me._currentState
            End Get
        End Property

        ''' <summary>
        ''' Initializes a new instance of the <see cref="Pop3Protocol"/> class using the default POP3 port 110
        ''' without using SSL.
        ''' </summary>
        ''' <param name="hostname">The hostname.</param>
        ''' <param name="username">The username.</param>
        ''' <param name="password">The password.</param>
        Public Sub New(ByVal hostname As String, ByVal username As String, ByVal password As String)
            Me.New(hostname, DefaultPort, False, username, password)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="Pop3Client"/> class using the default POP3 port 110.
        ''' </summary>
        ''' <param name="hostname">The hostname.</param>
        ''' <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        ''' <param name="username">The username.</param>
        ''' <param name="password">The password.</param>
        Public Sub New(ByVal hostname As String, ByVal useSsl As Boolean, ByVal username As String, ByVal password As String)
            Me.New(hostname, DefaultPort, useSsl, username, password)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="Pop3Client"/> class.
        ''' </summary>
        ''' <param name="hostname">The hostname.</param>
        ''' <param name="port">The port.</param>
        ''' <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        ''' <param name="username">The username.</param>
        ''' <param name="password">The password.</param>
        Public Sub New(ByVal hostname As String, ByVal port As Integer, ByVal useSsl As Boolean, ByVal username As String, ByVal password As String)
            Me.New()
            If (String.IsNullOrEmpty(hostname)) Then Throw New ArgumentNullException("hostname")
            If (port < 0) Then Throw New ArgumentOutOfRangeException("port")
            If (String.IsNullOrEmpty(username)) Then Throw New ArgumentNullException("username")
            If (String.IsNullOrEmpty(password)) Then Throw New ArgumentNullException("password")
            Me._hostname = hostname
            Me._port = port
            Me._useSsl = useSsl
            Me._username = username
            Me._password = password
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="Pop3Client"/> class.
        ''' </summary>
        Private Sub New()
            Me._client = New TcpClient()
            Me._currentState = Pop3State.Unknown
        End Sub

        ''' <summary>
        ''' Checks the connection.
        ''' </summary>
        Private Sub EnsureConnection()
            If (Not Me._client.Connected) Then Throw New Pop3Exception("Pop3 client is not connected.")
        End Sub

        ''' <summary>
        ''' Resets the state.
        ''' </summary>
        ''' <param name="state">The state.</param>
        Private Sub SetState(ByVal state As Pop3State)
            Me._currentState = state
        End Sub

        ''' <summary>
        ''' Ensures the response.
        ''' </summary>
        ''' <param name="response">The response.</param>
        ''' <param name="error">The error.</param>
        Private Sub EnsureResponse(ByVal response As Pop3Response, ByVal [error] As String)
            If (response Is Nothing) Then Throw New Pop3Exception("Unable to get Response.  Response object null.")
            If (response.StatusIndicator) Then
                Return
            End If 'the command execution was successful.

            Dim errorMessage As String = String.Empty

            If (String.IsNullOrEmpty([error])) Then
                errorMessage = response.HostMessage
            Else
                errorMessage = String.Concat([error], ": ", [error])
            End If

            Throw New Pop3Exception(errorMessage)
        End Sub

        ''' <summary>
        ''' Ensures the response.
        ''' </summary>
        ''' <param name="response">The response.</param>
        Private Sub EnsureResponse(ByVal response As Pop3Response)
            EnsureResponse(response, String.Empty)
        End Sub

        ''' <summary>
        ''' Traces the command.
        ''' </summary>
        ''' <param name="command">The command.</param>
        Private Sub TraceCommand(Of TCommand As Pop3Command(Of TResponse), TResponse As Pop3Response)(ByVal command As TCommand)
            AddHandler command.Trace, AddressOf OnTrace
        End Sub

        ''' <summary>
        ''' Connects this instance and properly sets the 
        ''' client stream to Use Ssl if it is specified.
        ''' </summary>
        Private Sub Connect()
            If (Me._client Is Nothing) Then
                Me._client = New TcpClient()
            End If 'If a previous quit command was issued, the client would be disposed of.

            If (Me._client.Connected) Then
                Return
            End If 'if the connection already is established no need to reconnect.

            SetState(Pop3State.Unknown)
            Dim response As ConnectResponse
            Using command As New ConnectCommand(_client, _hostname, _port, _useSsl)
                TraceCommand(Of ConnectCommand, ConnectResponse)(command)
                response = command.Execute(CurrentState)
                EnsureResponse(response)
            End Using

            SetClientStream(response.NetworkStream)

            SetState(Pop3State.Authorization)
        End Sub

        ''' <summary>
        ''' Sets the client stream.  If UseSsl <c>true</c> then wrap 
        ''' the client's <c>NetworkStream</c> in an <c>SslStream</c>, if UseSsl <c>false</c> 
        ''' then set the client stream to the <c>NetworkStream</c>
        ''' </summary>
        Private Sub SetClientStream(ByVal networkStream As Stream)
            If (_clientStream IsNot Nothing) Then
                _clientStream.Dispose()
            End If
            _clientStream = networkStream
        End Sub

        ''' <summary>
        ''' Authenticates this instance.
        ''' </summary>
        ''' <remarks>A successful execution of this method will result in a Current State of Transaction.
        ''' Unsuccessful USER or PASS commands can be reattempted by resetting the Username or Password 
        ''' properties and re-execution of the methods.</remarks>
        ''' <exception cref="Pop3Exception">
        ''' If the Pop3Server is unable to be connected.
        ''' If the User command is unable to be successfully executed.
        ''' If the Pass command is unable to be successfully executed.
        ''' </exception>
        Public Sub Authenticate()
            Connect()
            'execute the user command.
            Using userCommand As New UserCommand(_clientStream, _username)
                ExecuteCommand(Of Pop3Response, UserCommand)(userCommand)
            End Using

            'execute the pass command.
            Using passCommand As New PassCommand(_clientStream, _password)
                ExecuteCommand(Of Pop3Response, PassCommand)(passCommand)
            End Using

            _currentState = Pop3State.Transaction
        End Sub

        ''' <summary>
        ''' Executes the POP3 DELE command.
        ''' </summary>
        ''' <param name="item">The item.</param>
        ''' ''' <exception cref="Pop3Exception">If the DELE command was unable to be executed successfully.</exception>
        Public Sub Dele(ByVal item As Pop3ListItem)
            If (item Is Nothing) Then Throw New ArgumentNullException("item")
            Using command As New DeleCommand(_clientStream, item.MessageId)
                ExecuteCommand(Of Pop3Response, DeleCommand)(command)
            End Using
        End Sub

        ''' <summary>
        ''' Executes the POP3 NOOP command.
        ''' </summary>
        ''' <exception cref="Pop3Exception">If the NOOP command was unable to be executed successfully.</exception>
        Public Sub Noop()
            Using command As New NoopCommand(_clientStream)
                ExecuteCommand(Of Pop3Response, NoopCommand)(command)
            End Using
        End Sub

        ''' <summary>
        ''' Executes the POP3 RSET command.
        ''' </summary>
        ''' <exception cref="Pop3Exception">If the RSET command was unable to be executed successfully.</exception>
        Public Sub Rset()
            Using command As New RsetCommand(_clientStream)
                ExecuteCommand(Of Pop3Response, RsetCommand)(command)
            End Using
        End Sub

        ''' <summary>
        ''' Executes the POP3 STAT command.
        ''' </summary>
        ''' <returns>A Stat object containing the results of STAT command.</returns>
        ''' <exception cref="Pop3Exception">If the STAT command was unable to be executed successfully.</exception>
        Public Function Stat() As Stat
            Dim response As StatResponse
            Using command As New StatCommand(_clientStream)
                response = ExecuteCommand(Of StatResponse, StatCommand)(command)
            End Using
            Return New Stat(response.MessageCount, response.Octets)
        End Function

        ''' <summary>
        ''' Executes the POP3 List command.
        ''' </summary>
        ''' <returns>A generic List of Pop3Items containing the results of the LIST command.</returns>
        ''' <exception cref="Pop3Exception">If the LIST command was unable to be executed successfully.</exception>
        Public Function List() As System.Collections.Generic.List(Of Pop3ListItem)
            Dim response As ListResponse
            Using command As New ListCommand(_clientStream)
                response = ExecuteCommand(Of ListResponse, ListCommand)(command)
            End Using
            Return response.Items
        End Function

        ''' <summary>
        ''' Lists the specified message.
        ''' </summary>
        ''' <param name="messageId">The message.</param>
        ''' <returns>A <c>Pop3ListItem</c> for the requested Pop3Item.</returns>
        ''' <exception cref="Pop3Exception">If the LIST command was unable to be executed successfully for the provided message id.</exception>
        Public Function List(ByVal messageId As Integer) As Pop3ListItem
            Dim response As ListResponse
            Using command As New ListCommand(_clientStream, messageId)
                response = ExecuteCommand(Of ListResponse, ListCommand)(command)
            End Using
            Return New Pop3ListItem(response.MessageNumber, response.Octets)
        End Function

        ''' <summary>
        ''' Retrs the specified message.
        ''' </summary>
        ''' <param name="item">The item.</param>
        ''' <returns>A MimeEntity for the requested Pop3 Mail Item.</returns>
        Public Function RetrMimeEntity(ByVal item As Pop3ListItem) As MimeEntity
            If (item Is Nothing) Then Throw New ArgumentNullException("item")
            If (item.MessageId < 1) Then Throw New ArgumentOutOfRangeException("item.MessageId")
            Dim response As RetrResponse
            Using command As New RetrCommand(_clientStream, item.MessageId)
                response = ExecuteCommand(Of RetrResponse, RetrCommand)(command)
            End Using
            Dim reader As New MimeReader(response.MessageLines)
            Return reader.CreateMimeEntity()
        End Function

        Public Function Top(ByVal messageId As Integer, ByVal lineCount As Integer) As MailMessageEx
            If (messageId < 1) Then Throw New ArgumentOutOfRangeException("messageId")
            If (lineCount < 0) Then Throw New ArgumentOutOfRangeException("lineCount")
            Dim response As RetrResponse
            Using command As New TopCommand(_clientStream, messageId, lineCount)
                response = ExecuteCommand(Of RetrResponse, TopCommand)(command)
            End Using
            Dim reader As New MimeReader(response.MessageLines)
            Dim entity As MimeEntity = reader.CreateMimeEntity()
            Dim message As MailMessageEx = entity.ToMailMessageEx()
            message.Octets = response.Octets
            message.MessageNumber = messageId
            Return entity.ToMailMessageEx()
        End Function

        ''' <summary>
        ''' Retrs the mail message ex.
        ''' </summary>
        ''' <param name="item">The item.</param>
        ''' <returns></returns>
        Public Function RetrMailMessageEx(ByVal item As Pop3ListItem) As MailMessageEx
            Dim message As MailMessageEx = RetrMimeEntity(item).ToMailMessageEx()
            message.Account = Me._username
            message.MessageNumber = item.MessageId
            Return message
        End Function


        ''' <summary>
        ''' Executes the Pop3 QUIT command.
        ''' </summary>
        ''' <exception cref="Pop3Exception">If the quit command returns a -ERR server message.</exception>
        Public Sub Quit()
            Using command As New QuitCommand(_clientStream)
                ExecuteCommand(Of Pop3Response, QuitCommand)(command)
                If (CurrentState.Equals(Pop3State.Transaction)) Then
                    SetState(Pop3State.Update)
                End If ' Messages could have been deleted, reflect the server state.

                Disconnect()

                'Quit command can only be called in Authorization or Transaction state, reset to Unknown.
                SetState(Pop3State.Unknown)
            End Using
        End Sub

        ''' <summary>
        ''' Provides a common way to execute all commands.  This method
        ''' validates the connection, traces the command and finally
        ''' validates the response message for a -ERR response.
        ''' </summary>
        ''' <param name="command">The command.</param>
        ''' <returns>The Pop3Response for the provided command</returns>
        ''' <exception cref="Pop3Exception">If the HostMessage does not start with '+OK'.</exception>
        ''' <exception cref="Pop3Exception">If the client is no longer connected.</exception>
        Private Function ExecuteCommand(Of TResponse As Pop3Response, TCommand As Pop3Command(Of TResponse))(ByVal command As TCommand) As TResponse
            EnsureConnection()
            TraceCommand(Of TCommand, TResponse)(command)
            Dim response As TResponse = command.Execute(CurrentState)
            EnsureResponse(response)
            Return response
        End Function

        ''' <summary>
        ''' Disconnects this instance.
        ''' </summary>
        Private Sub Disconnect()
            If (_clientStream IsNot Nothing) Then _clientStream.Close() 'release underlying socket.
            If (_client IsNot Nothing) Then _client.Close()
            _client = Nothing
        End Sub


#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.Disconnect()
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

End Namespace
