Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Net.Sockets
Imports System.Net.Security
Imports System.Security.Authentication

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' Performs the connect to a Pop3 server and returns a Pop3 
    ''' response indicating the attempt to connect results and the
    ''' network stream to use for all subsequent Pop3 Commands.
    ''' </summary>
    Friend NotInheritable Class ConnectCommand
        Inherits Pop3Command(Of ConnectResponse)

        Private _client As TcpClient
        Private m_Client As Pop3Client
        Private _hostname As String
        Private _port As Integer
        Private _useSsl As Boolean

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ConnectCommand"/> class.
        ''' </summary>
        ''' <remarks>
        ''' Even though a network stream is provided to the base constructor the stream
        ''' does not already exist so we have to send in a dummy stream until the actual
        ''' connect has taken place.  Then we'll reset network stream to the 
        ''' stream made available by the TcpClient.GetStream() to read the data returned
        ''' after a connect.
        ''' </remarks>
        ''' <param name="client">The client.</param>
        ''' <param name="hostname">The hostname.</param>
        ''' <param name="port">The port.</param>
        ''' <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        Public Sub New(ByVal client As Pop3Client, ByVal connection As TcpClient, ByVal hostname As String, ByVal port As Integer, ByVal useSsl As Boolean)
            MyBase.New(client, New System.IO.MemoryStream(), False, Pop3State.Unknown)
            If (client Is Nothing) Then Throw New ArgumentNullException("client")
            If (connection Is Nothing) Then Throw New ArgumentNullException("connection")
            If (String.IsNullOrEmpty(hostname)) Then Throw New ArgumentNullException("hostname")
            If (port < 1) Then Throw New ArgumentOutOfRangeException("port")
            Me._client = connection
            Me.m_Client = client
            Me._hostname = hostname
            Me._port = port
            Me._useSsl = useSsl
        End Sub

        ''' <summary>
        ''' Creates the connect request message.
        ''' </summary>
        ''' <returns>A byte[] containing connect request message.</returns>
        Protected Overrides Function CreateRequestMessage() As Byte()
            Return Nothing
        End Function

        ''' <summary>
        ''' Executes this instance.
        ''' </summary>
        ''' <returns></returns>
        Friend Overrides Function Execute(ByVal currentState As Pop3State) As ConnectResponse
            Me.EnsurePop3State(currentState)
            Try
                Me._client.Connect(_hostname, _port)
                Me.SetClientStream()
            Catch e As SocketException
                Throw New Pop3Exception(String.Format("Unable to connect to {0}:{1}.", _hostname, _port), e)
            End Try
            Return MyBase.Execute(currentState)
        End Function


        ''' <summary>
        ''' Sets the client stream.
        ''' </summary>
        Private Sub SetClientStream()
            If (Me._useSsl) Then
                Try
                    If (Me.Client.CustomCerfificateValidation) Then
                        Me.NetworkStream = New SslStream(_client.GetStream(), True, AddressOf Me.Client.certificateCallBack)  'make sure the inner stream stays available for the Pop3Client to make use of.
                    Else
                        Me.NetworkStream = New SslStream(_client.GetStream(), True)  'make sure the inner stream stays available for the Pop3Client to make use of.
                    End If
                    DirectCast(Me.NetworkStream, SslStream).AuthenticateAsClient(Me._hostname)
                Catch e As ArgumentException
                    Throw New Pop3Exception("Unable to create Ssl Stream for hostname: " + _hostname, e)
                Catch e As AuthenticationException
                    Throw New Pop3Exception("Unable to authenticate ssl stream for hostname: " + _hostname, e)
                Catch e As InvalidOperationException
                    Throw New Pop3Exception("There was a problem  attempting to authenticate this SSL stream for hostname: " + _hostname, e)
                End Try
                'wrap NetworkStream in an SSL stream
            Else
                Me.NetworkStream = _client.GetStream()
            End If

        End Sub

        ''' <summary>
        ''' Creates the response.
        ''' </summary>
        ''' <param name="buffer">The buffer.</param>
        ''' <returns>
        ''' The <c>Pop3Response</c> containing the results of the
        ''' Pop3 command execution.
        ''' </returns>
        Protected Overrides Function CreateResponse(ByVal buffer As Byte()) As ConnectResponse
            Dim response As Pop3Response = Pop3Response.CreateResponse(buffer)
            Return New ConnectResponse(response, Me.NetworkStream)
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class


End Namespace