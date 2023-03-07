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
Imports FinSeA.Net.GenericClient.Commands

Namespace Net.GenericClient

    <Flags()>
    Public Enum ConnectionState

        ''' <summary>
        ''' Undefined.
        ''' </summary>
        Unknown = 0
        ''' <summary>
        ''' Connected to Pop3 server, but not authorized.
        ''' May issue any of the following commands QUIT, USER, PASS
        ''' </summary>
        Authorization = 1
        ''' <summary>
        ''' Authorized to Pop3 server, can issue any of the following commands;
        ''' STAT, LIST, RETR, DELE, RSET
        ''' </summary>
        Transaction = 2
        ''' <summary>
        ''' Quit command was sent to server indicating deleted
        ''' messages should be removed.
        ''' </summary>
        Update = 4
    End Enum


    ''' <summary>
    ''' Client su cui si basano i client Telnet, POP3, IMAP, ecc
    ''' </summary>
    Public MustInherit Class GenericClient

        Private m_TCPClient As TcpClient
        Private m_ClientStream As Stream
        Private m_CurrentState As ConnectionState

        ''' <summary>
        ''' Evento generato quando il client riceve dei dati (in risposta ad un qualsiasi comando o tramite una comunicazione iniziata direttamente dal server)
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event DataReceived(ByVal sender As Object, ByVal e As DataReceivedEventArgs)

        ''' <summary>
        ''' Traces the various command objects that executed during this objects lifetime.
        ''' </summary>
        Public Event Trace(ByVal sender As Object, ByVal message As String)

        ''' <summary>
        ''' Initializes a new instance  
        ''' </summary>
        Public Sub New()
            Me.m_TCPClient = New TcpClient()
            Me.m_ClientStream = Nothing
            Me.m_CurrentState = ConnectionState.Unknown
        End Sub

        ''' <summary>
        ''' Restituisce il client TCP sottostante
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property TCPClient As TcpClient
            Get
                Return Me.m_TCPClient
            End Get
        End Property

        ''' <summary>
        ''' Restituisce lo stream di comunicazione con il server
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ClientStream As Stream
            Get
                Return Me.m_ClientStream
            End Get
        End Property
        ''' <summary>
        ''' Sets the client stream.  If UseSsl <c>true</c> then wrap 
        ''' the client's <c>NetworkStream</c> in an <c>SslStream</c>, if UseSsl <c>false</c> 
        ''' then set the client stream to the <c>NetworkStream</c>
        ''' </summary>
        Protected Sub SetClientStream(ByVal networkStream As Stream)
            If (Me.m_ClientStream IsNot Nothing) Then Me.m_ClientStream.Dispose()
            Me.m_ClientStream = networkStream
        End Sub

        ''' <summary>
        ''' Restituisce lo stato dell'oggetto
        ''' </summary>
        ''' <value>The state of the current.</value>
        Public ReadOnly Property CurrentState As ConnectionState
            Get
                Return Me.m_CurrentState
            End Get
        End Property

        Protected Overridable Sub SetState(ByVal state As ConnectionState)
            Me.m_CurrentState = state
        End Sub

        Protected Overridable Sub OnTrace(ByVal message As String)
            RaiseEvent Trace(Me, message)
        End Sub


        ''' <summary>
        ''' Checks the connection.
        ''' </summary>
        Protected Sub EnsureConnection()
            If (Not Me.m_TCPClient.Connected) Then Throw New NotConnectedException
        End Sub


        ''' <summary>
        ''' Ensures the response.
        ''' </summary>
        ''' <param name="response">The response.</param>
        ''' <param name="error">The error.</param>
        Protected Sub EnsureResponse(ByVal response As ServerResponse, ByVal [error] As String)
            If (response Is Nothing) Then Throw New BadServerResponseException("Unable to get Response.  Response object null.")
            If (response.IsOk) Then Return 'the command execution was successful.
            Throw New ServerException()
        End Sub

        ''' <summary>
        ''' Ensures the response.
        ''' </summary>
        ''' <param name="response">The response.</param>
        Protected Sub EnsureResponse(ByVal response As ServerResponse)
            Me.EnsureResponse(response, String.Empty)
        End Sub

        ''' <summary>
        ''' Traces the command.
        ''' </summary>
        ''' <param name="command">The command.</param>
        Protected Sub TraceCommand(ByVal command As ServerCommand)
            AddHandler command.Trace, AddressOf OnTrace
        End Sub

        ''' <summary>
        ''' Prepara gli stream
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub SetUpConnection()
            If (Me.m_TCPClient Is Nothing) Then Me.m_TCPClient = New TcpClient() 'If a previous quit command was issued, the client would be disposed of.
            If (Me.m_TCPClient.Connected) Then Return
            Me.SetState(ConnectionState.Unknown)

        End Sub


        Public Overridable Sub Connect()
            Me.SetUpConnection()
        End Sub


        Protected Overridable Function ExecuteCommand(ByVal command As ServerCommand) As ServerResponse
            Me.EnsureConnection()
            command.NetworkStream = Me.m_ClientStream
            Me.TraceCommand(command)
            Dim response As ServerResponse = command.Execute(Me)
            Me.EnsureResponse(response)
            Return response
        End Function

        ''' <summary>
        ''' Disconnects this instance.
        ''' </summary>
        Public Overridable Sub Disconnect()
            If (Me.m_ClientStream IsNot Nothing) Then Me.m_ClientStream.Close() 'release underlying socket.
            If (Me.m_TCPClient IsNot Nothing) Then Me.m_TCPClient.Close()
            Me.m_ClientStream = Nothing
            Me.m_TCPClient = Nothing
        End Sub

        ''' <summary>
        ''' Genera l'evento DataReceived
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Friend Overridable Sub OnDataReceived(ByVal e As DataReceivedEventArgs)
            RaiseEvent DataReceived(Me, e)
        End Sub

    End Class

End Namespace
