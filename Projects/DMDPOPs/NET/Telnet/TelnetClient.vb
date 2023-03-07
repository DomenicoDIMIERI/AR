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
Imports FinSeA.Net.GenericClient
Imports FinSeA.Net.GenericClient.Commands
Imports System.Threading

Namespace Net.Telnet

    Public Class TelnetClient
        Implements IDisposable

        Public Event DataReceived(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
        Public Event DataSent(ByVal sender As Object, ByVal e As DataReceivedEventArgs)

        Private Const BUFFERSIZE = 1024

        Private m_ManualReset As New ManualResetEvent(False)
        Private m_Host As String
        Private m_Port As Integer = 23
        Private m_TCPClient As New TcpClient
        Private m_Stream As NetworkStream
        Private m_Buffer() As Byte
        Private m_WaitForText As String = ""
        Private m_Response As String = ""
        Private m_Output As New System.Text.StringBuilder

        Public Sub New()
            ReDim m_Buffer(BUFFERSIZE - 1)
        End Sub

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
                If (Me.TCPClient.Connected) Then Throw New ConnectionException("Impossibile modificare l'host mentre si è connessi")
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
                If (Me.TCPClient.Connected) Then Throw New ConnectionException("Impossibile modificare la porta mentre si è connessi")
                Me.m_Port = value
            End Set
        End Property

        Public Sub Connect()
            Me.m_TCPClient.Connect(Me.m_Host, Me.m_Port)
            Me.m_Stream = Me.m_TCPClient.GetStream
        End Sub

        Public Sub Close()
            Me.m_Stream.Dispose()
            Me.m_Stream = Nothing
            Me.m_TCPClient.Close()
        End Sub

        Public Sub Print(ByVal text As String)
            If (text = "") Then Exit Sub
            text &= vbCrLf
            Dim buffer() As Byte = System.Text.Encoding.UTF7.GetBytes(text)
            Me.m_Stream.Write(buffer, 0, UBound(buffer) + 1)
            Me.m_Output.Append(text)
            Me.OnDataSent(New DataReceivedEventArgs(buffer))
        End Sub

        Public Sub WaitFor(ByVal text As String)
            Me.m_WaitForText = text
            Me.m_Response = ""
            Me.m_ManualReset.Reset()
            Me.m_Stream.BeginRead(Me.m_Buffer, 0, BUFFERSIZE, New AsyncCallback(AddressOf HandleReceive), Nothing)
            Me.m_ManualReset.WaitOne()
        End Sub

        ''' <summary>
        ''' Gets the single line response callback.
        ''' </summary>
        ''' <param name="ar">The ar.</param>
        Protected Sub HandleReceive(ByVal ar As IAsyncResult)
            Dim bytesReceived As Integer = Me.m_Stream.EndRead(ar)
            If (bytesReceived = 0) Then
                Me.m_ManualReset.Set()
                Exit Sub
            End If
            Dim bytes() As Byte
            ReDim bytes(bytesReceived)
            Array.Copy(Me.m_Buffer, bytes, bytesReceived)
            Dim message As String = System.Text.Encoding.UTF7.GetString(bytes)
            Me.m_Response = Me.m_Response & message
            Me.m_Output.Append(message)
            'Debug.Print(message & " " & vbTab & " " & Me.m_Output)
            Me.OnDataReceived(New DataReceivedEventArgs(bytes))
            If (Me.m_WaitForText = "") OrElse (InStr(Me.m_Response, Me.m_WaitForText) > 0) Then
                Me.m_ManualReset.Set()
            Else
                Me.m_Stream.BeginRead(Me.m_Buffer, 0, BUFFERSIZE, New AsyncCallback(AddressOf HandleReceive), Nothing)
            End If
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

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).

                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            If Me.m_Stream IsNot Nothing Then
                Me.m_Stream.Dispose()
                Me.m_Stream = Nothing
            End If
            If Me.m_TCPClient.Connected Then
                Me.m_TCPClient.Close()
            End If
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