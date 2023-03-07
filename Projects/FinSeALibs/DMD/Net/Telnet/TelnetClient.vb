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
Imports DMD.Net.GenericClient.Commands
Imports System.Threading

Namespace Net.Telnet

    Public Class TelnetClient
        Implements IDisposable

        Public Event DataReceived(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
        Public Event DataSent(ByVal sender As Object, ByVal e As DataReceivedEventArgs)

        Private Const BUFFERSIZE = 1024

        Private m_ManualReset As ManualResetEvent
        Private m_Host As String
        Private m_Port As Integer
        Private m_TCPClient As TcpClient
        Private m_Stream As NetworkStream
        Private m_Buffer() As Byte
        Private m_WaitForText As String
        Private m_Response As String
        Private m_Output As CCollection(Of String)
        Private lockObject As New Object
        Private m_currentCommand As String
        Private m_Res As IAsyncResult

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Buffer = Array.CreateInstance(GetType(Byte), BUFFERSIZE)
            Me.m_ManualReset = Nothing
            Me.m_Host = ""
            Me.m_Port = 23
            Me.m_TCPClient = New TcpClient
            Me.m_Stream = Nothing
            Me.m_WaitForText = ""
            Me.m_Response = ""
            Me.m_Output = New CCollection(Of String)
            Me.m_currentCommand = ""
            Me.m_Res = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce un riferimento al client TCP sottostante utilizzato per le comunicazioni 
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
        ''' Restituisce o imposta il nome o l'indirizzo dell'host a cui connettersi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Host As String
            Get
                Return Me.m_Host
            End Get
            Set(value As String)
                If (Me.IsConnected) Then Throw New ConnectionException("Impossibile modificare l'host mentre si è connessi")
                Me.m_Host = Trim(value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la porta a cui connettersi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Port As Integer
            Get
                Return Me.m_Port
            End Get
            Set(value As Integer)
                If (Me.IsConnected) Then Throw New ConnectionException("Impossibile modificare la porta mentre si è connessi")
                Me.m_Port = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce vero se la connessione con il sistema remoto è attiva
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsConnected() As Boolean
            Return Me.m_TCPClient.Connected
        End Function

        ''' <summary>
        ''' Crea la connessione al sistema remoto e inizia le operazioni di IO
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Connect()
            If (Me.IsConnected) Then Throw New InvalidOperationException("Connessione già attiva")
            Me.m_ManualReset = New ManualResetEvent(False)
            Me.m_TCPClient.Connect(Me.m_Host, Me.m_Port)
            Me.m_Stream = Me.m_TCPClient.GetStream
            'System.Threading.Thread.Sleep(100)
            Me.m_Res = Me.m_Stream.BeginRead(Me.m_Buffer, 0, Me.m_Buffer.Length, New AsyncCallback(AddressOf HandleReceive), Nothing)
        End Sub

        ''' <summary>
        ''' Termina la connessione con il sistema remoto
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Close()
            If (Not Me.IsConnected) Then Throw New InvalidOperationException("Connessione non attiva")
            Me.m_ManualReset.Dispose()
            Me.m_Stream.Dispose()
            Me.m_Stream = Nothing
            Me.m_TCPClient.Close()
        End Sub

        Public Overrides Function ToString() As String
            SyncLock Me.lockObject
                Dim buffer As New System.Text.StringBuilder
                For Each line As String In Me.m_Output
                    buffer.Append(line & vbCrLf)
                Next
                Return buffer.ToString
            End SyncLock
        End Function

        ''' <summary>
        ''' Invia il testo specificato al server
        ''' </summary>
        ''' <param name="text"></param>
        ''' <remarks></remarks>
        Public Sub Print(ByVal text As String)
            If (text = "") Then Exit Sub
            Dim buffer() As Byte = System.Text.Encoding.UTF7.GetBytes(text)
            Me.m_Stream.Write(buffer, 0, UBound(buffer) + 1)
            SyncLock Me.lockObject
                Me.m_currentCommand &= text
            End SyncLock
            Me.OnDataSent(New DataReceivedEventArgs(buffer))
        End Sub

        ''' <summary>
        ''' Invia il testo specificato al server aggiungendo il CR + LF in coda
        ''' </summary>
        ''' <param name="text"></param>
        ''' <remarks></remarks>
        Public Sub Println(ByVal text As String)
            If (text = "") Then Exit Sub
            text &= vbCrLf
            Dim buffer() As Byte = System.Text.Encoding.UTF7.GetBytes(text)
            Me.m_Stream.Write(buffer, 0, UBound(buffer) + 1)
            SyncLock Me.lockObject
                Me.m_currentCommand &= text
                Me.m_Output.Add(Me.m_currentCommand)
                Me.m_currentCommand = ""
            End SyncLock
            Me.OnDataSent(New DataReceivedEventArgs(buffer))
        End Sub

        ''' <summary>
        ''' Analizza la risposta del server in attesa di ricevere la stringa specificata
        ''' </summary>
        ''' <param name="text"></param>
        ''' <remarks></remarks>
        Public Sub WaitFor(ByVal text As String)
            Me.m_WaitForText = text
            Me.m_Response = ""
            'Me.m_ManualReset.Reset()
            Me.m_ManualReset.WaitOne()
        End Sub

        Public ReadOnly Property LastResponse As String
            Get
                Return Me.m_Response
            End Get
        End Property

        ''' <summary>
        ''' Gets the single line response callback.
        ''' </summary>
        ''' <param name="ar">The ar.</param>
        Protected Sub HandleReceive(ByVal ar As IAsyncResult)
            If (Me.m_Stream Is Nothing) Then Exit Sub
            
            Dim bytesReceived As Integer
            Dim bytes() As Byte = Nothing
            Dim found As Boolean = False

            Try
                bytesReceived = Me.m_Stream.EndRead(ar)
            Catch ex As ObjectDisposedException
                Return
            End Try

            If (bytesReceived > 0) Then
                ReDim bytes(bytesReceived)

                Array.Copy(Me.m_Buffer, bytes, bytesReceived)

                Dim message As String = System.Text.Encoding.UTF7.GetString(bytes)

                SyncLock Me.lockObject
                    Me.m_Response = Me.m_Response & message

                    If (Me.m_WaitForText <> "") AndAlso (InStr(Me.m_Response, Me.m_WaitForText) > 0) Then
                        found = True
                    End If

                    Dim p As Integer
                    Do
                        p = InStr(Me.m_Response, vbCr)
                        If (p > 0) Then
                            Dim line As String = Left(Me.m_Response, p - 1)
                            Me.m_Output.Add(line)
                            Me.m_Response = Mid(Me.m_Response, p + 1)
                        End If
                    Loop While (p > 0)
                End SyncLock

                If (found) Then
                    Me.m_ManualReset.Set()
                End If
            End If

            Me.m_Stream.BeginRead(Me.m_Buffer, 0, Me.m_Buffer.Length, New AsyncCallback(AddressOf HandleReceive), Nothing)
        End Sub

        Protected Overridable Sub OnDataSent(ByVal e As DataReceivedEventArgs)
            RaiseEvent DataSent(Me, e)
        End Sub

        Protected Overridable Sub OnDataReceived(ByVal e As DataReceivedEventArgs)
            RaiseEvent DataReceived(Me, e)
        End Sub

        Public ReadOnly Property Output As String
            Get
                Return Me.m_Output.ToString
            End Get
        End Property


        'Protected Overrides Sub SetUpConnection()
        '    Me.TCPClient.Connect(Me.m_Host, Me.m_Port)
        '    Me.SetClientStream(Me.TCPClient.GetStream)
        'End Sub

        'Public Sub WaitFor(ByVal response As String)

        'End Sub

        ' ''' <summary>
        ' ''' Attende che il server invii l'intera risposta e ne restituisce il buffer 
        ' ''' </summary>
        ' ''' <returns></returns>
        'Protected Overridable Function GetResponse() As Byte()
        '    Me.m_response = Me.CreateResponse
        '    Try
        '        Me.Receive(New AsyncCallback(AddressOf HandleReceive))
        '        Me._manualResetEvent.WaitOne()
        '        Return Me._responseContents.ToArray()
        '    Catch e As SocketException
        '        Throw New ServerException("Unable to get response.", e)
        '    End Try
        'End Function

        'Private Sub Receive(ByVal cb As AsyncCallback)
        '    Me._networkStream.BeginRead(Me._buffer, 0, _buffer.Length, cb, Nothing)
        'End Sub

        ' ''' <summary>
        ' ''' Gets the single line response callback.
        ' ''' </summary>
        ' ''' <param name="ar">The ar.</param>
        'Protected Sub HandleReceive(ByVal ar As IAsyncResult)
        '    Dim bytesReceived As Integer = Me._networkStream.EndRead(ar)
        '    'Dim message As String = WriteReceivedBytesToBuffer(bytesReceived)
        '    Me.m_response.AppendBytes(Me._buffer, bytesReceived)
        '    If Me.m_response.IsComplete Then
        '        Me._manualResetEvent.Set()
        '    Else
        '        Me.Receive(New AsyncCallback(AddressOf HandleReceive))
        '    End If
        'End Sub

#If 0 Then

        use Net::Telnet;
use Time::HiRes;

# cisco phone host name
my $host='10.0.0.1';
# cisco phone password
my $password='cisco';
# mute on a dial 0/1
my $mute=0;

my $sleeptime=.2;
my $prompt='/> $/';

my $argc = @ARGV;
if ($argc!=1){
    print "Usage: call.pl <number>\n";
    exit;
}
my $number=@ARGV[0];

if($number!~/^[0-9*#]+$/) {
    print "Error: wrong characters in the numer\n";
    exit 2;
}
$telnet = new Net::Telnet ( Timeout=>3, Errmode=>'die');
# connecting
$telnet->open($host);
$telnet->waitfor('/Password :$/i'); 
$telnet->print($password); 
$telnet->waitfor($prompt);

$telnet->print('test open');
$telnet->waitfor($prompt);
$telnet->print('test key spkr');
$telnet->waitfor($prompt);Time::HiRes::sleep($sleeptime);
if($mute){
    $telnet->print('test key mute');
    $telnet->waitfor($prompt);Time::HiRes::sleep($sleeptime);
}
$telnet->print("test key ".$number."#");
$telnet->waitfor($prompt);Time::HiRes::sleep((length($number)+1)*$sleeptime);
$telnet->print('test close');
$telnet->waitfor($prompt);
$telnet->close($host);

#End If

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Overridable Sub Dispose() Implements IDisposable.Dispose
            If Me.m_Stream IsNot Nothing Then Me.m_Stream.Dispose() : Me.m_Stream = Nothing
            If Me.m_TCPClient.Connected Then Me.m_TCPClient.Close() : Me.m_TCPClient = Nothing
            If Me.m_ManualReset IsNot Nothing Then Me.m_ManualReset.Dispose() : Me.m_ManualReset = Nothing

            Me.m_Host = vbNullString
            If (Me.m_Buffer IsNot Nothing) Then Erase Me.m_Buffer : Me.m_Buffer = Nothing
            Me.m_WaitForText = vbNullString
            Me.m_Response = vbNullString
            Me.m_Output = Nothing
            Me.lockObject = Nothing
            Me.m_currentCommand = vbNullString
            Me.m_Res = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace