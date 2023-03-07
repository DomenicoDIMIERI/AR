Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Sockets
Imports System.Net.Security
Imports System.Security.Authentication
Imports DMD.Net.Mime
Imports DMD.Net.GenericClient
Imports DMD.Net.Mail.Protocols.POP3

Namespace Net.Mail

    ''' <summary>
    ''' The Pop3Client class provides a wrapper for the Pop3 commands
    ''' that can be executed against a Pop3Server.  This class will 
    ''' execute and return results for the various commands that are 
    ''' executed.
    ''' </summary>
    Public NotInheritable Class Pop3Client
        Inherits DMD.Net.Mail.GenericMailClient

        Public Const DefaultPort As Integer = 110

        Private _client As TcpClient
        Private _clientStream As Stream
        Private m_Async As Boolean
        Private m_CustomCerfificateValidation As Boolean
        Private m_TimeOut As Integer

        ''' <summary>
        ''' Initializes a new instance  
        ''' </summary>
        Public Sub New()
            Me._client = New TcpClient()
            Me._clientStream = Nothing
            Me.m_Async = False
            Me.m_CustomCerfificateValidation = False
            Me.m_TimeOut = 120
        End Sub

        Public Sub New(ByVal userName As String, ByVal password As String, ByVal server As String, Optional ByVal port As Integer = DefaultPort, Optional ByVal useSSL As Boolean = False)
            Me.New()
            Me.SetUserName(userName)
            Me.SetPassword(password)
            Me.SetServer(server)
            Me.SetPort(port)
            Me.SetUseSSL(useSSL)
        End Sub

        Public Property CustomCerfificateValidation As Boolean
            Get
                Return Me.m_CustomCerfificateValidation
            End Get
            Set(value As Boolean)
                Me.m_CustomCerfificateValidation = value
            End Set
        End Property

        Public Property Async As Boolean
            Get
                Return Me.m_Async
            End Get
            Set(value As Boolean)
                Me.m_Async = value
            End Set
        End Property

        Public Property TimeOut As Integer
            Get
                Return Me.m_TimeOut
            End Get
            Set(value As Integer)
                Me.m_TimeOut = value
            End Set
        End Property

        ''' <summary>
        ''' Checks the connection.
        ''' </summary>
        Private Sub EnsureConnection()
            If (Not Me._client.Connected) Then Throw New Pop3Exception("Pop3 client is not connected.")
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

        Public Overrides Sub Connect()
            If (Me._client Is Nothing) Then Me._client = New TcpClient() 'If a previous quit command was issued, the client would be disposed of.
            If (Me._client.Connected) Then Return
            Me.SetState(Pop3State.Unknown)
            Dim response As ConnectResponse
            Using command As New ConnectCommand(Me, Me._client, Me.ServerName, Me.ServerPort, Me.UseSSL)
                Me.TraceCommand(Of ConnectCommand, ConnectResponse)(command)
                response = command.Execute(Me.CurrentState)
                Me.EnsureResponse(response)
            End Using
            Me.SetClientStream(response.NetworkStream)
            Me.SetState(Pop3State.Authorization)
        End Sub

        ''' <summary>
        ''' Sets the client stream.  If UseSsl <c>true</c> then wrap 
        ''' the client's <c>NetworkStream</c> in an <c>SslStream</c>, if UseSsl <c>false</c> 
        ''' then set the client stream to the <c>NetworkStream</c>
        ''' </summary>
        Protected Sub SetClientStream(ByVal networkStream As Stream)
            If (Me._clientStream IsNot Nothing) Then Me._clientStream.Dispose()
            Me._clientStream = networkStream
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
        Public Overrides Sub Authenticate()
            Me.Connect()
            'execute the user command.
            Using userCommand As New UserCommand(Me, Me._clientStream, Me.UserName)
                Me.ExecuteCommand(Of Pop3Response, UserCommand)(userCommand)
            End Using

            'execute the pass command.
            Using passCommand As New PassCommand(Me, _clientStream, Me.Password)
                Me.ExecuteCommand(Of Pop3Response, PassCommand)(passCommand)
            End Using

            Me.SetState(ConnectionState.Transaction)
        End Sub

        ''' <summary>
        ''' Executes the POP3 DELE command.
        ''' </summary>
        ''' <param name="item">The item.</param>
        ''' ''' <exception cref="Pop3Exception">If the DELE command was unable to be executed successfully.</exception>
        Public Overloads Sub Dele(ByVal item As Pop3ListItem)
            If (item Is Nothing) Then Throw New ArgumentNullException("item")
            Using command As New DeleCommand(Me, _clientStream, item.MessageId)
                ExecuteCommand(Of Pop3Response, DeleCommand)(command)
            End Using
        End Sub

        Public Overrides Sub Dele(ByVal msg As MailMessageEx)
            Using command As New DeleCommand(Me, Me._clientStream, msg.MessageNumber)
                ExecuteCommand(Of Pop3Response, DeleCommand)(command)
            End Using
        End Sub

        ''' <summary>
        ''' Executes the POP3 NOOP command.
        ''' </summary>
        ''' <exception cref="Pop3Exception">If the NOOP command was unable to be executed successfully.</exception>
        Public Overrides Sub Noop()
            Using command As New NoopCommand(Me, Me._clientStream)
                ExecuteCommand(Of Pop3Response, NoopCommand)(command)
            End Using
        End Sub

        ''' <summary>
        ''' Executes the POP3 RSET command.
        ''' </summary>
        ''' <exception cref="Pop3Exception">If the RSET command was unable to be executed successfully.</exception>
        Public Overrides Sub Rset()
            Using command As New RsetCommand(Me, Me._clientStream)
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
            Using command As New StatCommand(Me, Me._clientStream)
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
            Using command As New ListCommand(Me, _clientStream)
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
            Using command As New ListCommand(Me, _clientStream, messageId)
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
            Using command As New RetrCommand(Me, Me._clientStream, item.MessageId)
                response = Me.ExecuteCommand(Of RetrResponse, RetrCommand)(command)
            End Using
            Dim reader As New MimeReader(response.MessageLines)
            Return reader.CreateMimeEntity()
        End Function

        Public Overrides Function Top(messageId As Integer, lineCount As Integer) As MailMessageEx
            If (messageId < 1) Then Throw New ArgumentOutOfRangeException("messageId")
            If (lineCount < 1) Then Throw New ArgumentOutOfRangeException("lineCount")

            Dim response As RetrResponse

            Using command As New TopCommand(Me, _clientStream, messageId, lineCount)
                response = ExecuteCommand(Of RetrResponse, TopCommand)(command)
            End Using

            Dim reader As New MimeReader(response.MessageLines)
            Dim entity As MimeEntity = reader.CreateMimeEntity()

            Dim message As MailMessageEx = entity.ToMailMessageEx()
            message.Octets = response.Octets
            message.MessageNumber = messageId

            entity.Dispose()

            Return message
        End Function

        ''' <summary>
        ''' Retrs the mail message ex.
        ''' </summary>
        ''' <param name="item">The item.</param>
        ''' <returns></returns>
        Public Function RetrMailMessageEx(ByVal item As Pop3ListItem) As MailMessageEx

            Dim entity As MimeEntity = Me.RetrMimeEntity(item)

            Dim message As MailMessageEx = entity.ToMailMessageEx()
            message.Account = Me.UserName
            message.MessageNumber = item.MessageId

            entity.Dispose()

            Return message
        End Function


        ''' <summary>
        ''' Executes the Pop3 QUIT command.
        ''' </summary>
        ''' <exception cref="Pop3Exception">If the quit command returns a -ERR server message.</exception>
        Public Overrides Sub Quit()
            Using command As New QuitCommand(Me, _clientStream)
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
        Public Overrides Sub Disconnect()
            If (Me._clientStream IsNot Nothing) Then Me._clientStream.Close() 'release underlying socket.
            If (Me._client IsNot Nothing) Then Me._client.Close()
            Me._clientStream = Nothing
            Me._client = Nothing
        End Sub

        Public Overrides Function GetMessagesList() As CCollection(Of MailMessageEx)
            Dim ret As New CCollection(Of MailMessageEx)
            Me.Stat()
            For Each item As DMD.Net.Mail.Protocols.POP3.Pop3ListItem In Me.List()
                Dim message As DMD.Net.Mail.MailMessageEx = Me.RetrMailMessageEx(item)
                ret.Add(message)

            Next
            Return ret
        End Function



        Public Overrides ReadOnly Property Name As String
            Get
                Return "POP3"
            End Get
        End Property


        Protected Friend Function certificateCallBack(sender As Object, certificate As System.Security.Cryptography.X509Certificates.X509Certificate, chain As System.Security.Cryptography.X509Certificates.X509Chain, sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
            Return True
        End Function

        Public Overrides Sub Dispose()
            If (Me._client IsNot Nothing) Then Me._client.Close() : Me._client = Nothing
            If (Me._clientStream IsNot Nothing) Then Me._clientStream.Dispose() : Me._clientStream = Nothing
            MyBase.Dispose()
        End Sub
    End Class

End Namespace
