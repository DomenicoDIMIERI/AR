Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Net
Imports System.Net.Sockets
Imports System.IO
Imports System.Net.Security
Imports System.Security.Authentication
Imports System.Threading

Namespace Net.HTTPProxy

    Public Class ProxyRequest
        Inherits System.EventArgs
        Implements IDisposable

        Private Shared pending As New System.Collections.ArrayList
        Private Const BUFFER_SIZE As Integer = 8192

        Friend isDisposed As Boolean = False
        Friend isTunneling As Boolean = False
        Friend m_Server As HTTPProxyServer
        Friend m_Client As TcpClient
        Friend m_ClientStream As System.IO.Stream
        Private m_ClientStreamReader As StreamReader
        Private m_SslStream As SslStream
        Friend m_Method As String
        Friend m_RemoteURI As String
        Friend m_Version As Version
        Private m_CacheEntry As CacheEntry
        Friend m_StartDate As Date
        Friend m_LastClient As Date?
        Friend m_EndDate As Date?


        Public Sub New()
            Me.m_StartDate = Now
            Me.m_Server = Nothing
            Me.m_Client = Nothing
            Me.m_ClientStream = Nothing
            Me.m_ClientStreamReader = Nothing
            Me.m_SslStream = Nothing
            Me.m_Method = ""
            Me.m_RemoteURI = ""
            Me.m_Version = New Version(1, 0)
            Me.m_CacheEntry = Nothing
            SyncLock pending
                pending.Add(Me)
            End SyncLock
        End Sub


        ''' <summary>
        ''' Restituisce l'oggetto ProxyServer a cui il client ha inviato la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Server As HTTPProxyServer
            Get
                Return Me.m_Server
            End Get
        End Property


        ''' <summary>
        ''' Restituisce il metodo richiesto dal client
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Method As String
            Get
                Return Me.m_Method
            End Get
        End Property


        ''' <summary>
        ''' Restituisce la url che il client desidera scaricare
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RemoteUri As String
            Get
                Return Me.m_RemoteURI
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la versione della richiesta effettuata dal client
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Version As Version
            Get
                Return Me.m_Version
            End Get
        End Property

        ''' <summary>
        ''' Restituisce l'oggetto connessione aperto dal client
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Client As TcpClient
            Get
                Return Me.m_Client
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stream di comunicazione con il client da cui proviene la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ClientStream As System.IO.Stream
            Get
                Return Me.m_ClientStream
            End Get
            Set(value As System.IO.Stream)
                Me.m_ClientStream = value
            End Set
        End Property

        Public Property CacheEntry As CacheEntry
            Get
                Return Me.m_CacheEntry
            End Get
            Set(value As CacheEntry)
                Me.m_CacheEntry = value
            End Set
        End Property


        Protected Friend Overridable Function Resolve() As HttpWebRequest
            If (Me.Method.ToUpper = "CONNECT") Then
                If (Me.Server.SSLTunneling) Then
                    Me.isTunneling = True
                    Return Nothing
                Else
                    Me.Man()
                End If
            End If


            Select Case Me.Method.ToUpper
                Case "GET" : Return Me.ResolveGet()
                Case "POST" : Return Me.ResolvePost()
                Case "CONNECT" : Return Nothing 'ERRORE SSL: 
                Case Else ' Return Me.ResolveOther()
                    Return Nothing
            End Select
        End Function

        Protected Overridable Function ReadRequestHeaders(ByVal sr As StreamReader, ByVal webReq As HttpWebRequest) As Integer
            Dim contentLen As Integer = 0
            Dim header As String()
            Dim httpCmd As String

            httpCmd = Trim(sr.ReadLine())
            While (httpCmd <> vbNullString)
                header = httpCmd.Split(ProxyUtils.colonSpaceSplit, 2, StringSplitOptions.None)
                Select Case (header(0).ToLower())
                    Case "host"
                        webReq.Host = header(1)
                        'break;
                    Case "user-agent"
                        webReq.UserAgent = header(1)
                        'break;
                    Case "accept"
                        webReq.Accept = header(1)
                        'break; 
                    Case "referer"
                        webReq.Referer = header(1)
                        'break;
                    Case "cookie"
                        webReq.Headers("Cookie") = header(1)
                        'break;
                    Case "proxy-connection", "connection", "keep-alive"
                        Debug.Print(httpCmd)
                        'ignore these
                        'break;
                    Case "content-length"
                        Integer.TryParse(header(1), contentLen)
                        'break;
                    Case "content-type"
                        webReq.ContentType = header(1)
                        'break;
                    Case "if-modified-since"
                        Dim sb As String() = header(1).Trim().Split(ProxyUtils.semiSplit)
                        Dim d As DateTime
                        If (DateTime.TryParse(sb(0), d)) Then webReq.IfModifiedSince = d
                        'break;
                    Case "expect"
                        webReq.Expect = header(1)
                    Case Else
                        Try
                            webReq.Headers.Add(header(0), header(1))
                        Catch ex As Exception
                            Me.Server.OnProxyLog(New HTTPProxyLogEventArgs(String.Format("Could not add header {0}.  Exception message:{1}", header(0), ex.Message)))
                        End Try
                        'break;
                End Select
                httpCmd = sr.ReadLine()
            End While

            Return contentLen
        End Function

        Protected Overridable Function ResolveGet() As HttpWebRequest
            'construct the web request that we are going to issue on behalf of the client.
            'read the request headers from the client and copy them to our request


            Dim webReq As HttpWebRequest = HttpWebRequest.Create(Me.RemoteUri)
            Dim contentLen As Integer = Me.ReadRequestHeaders(Me.m_ClientStreamReader, webReq)
            webReq.Method = Me.Method
            'webReq.CachePolicy = System.Net.Cache.HttpRequestCacheLevel.Default
            'webReq.Timeout = Me.m_Server.TimeOut
            webReq.ProtocolVersion = Me.Version
            webReq.Proxy = Me.Server.Proxy
            webReq.KeepAlive = False
            webReq.AllowAutoRedirect = False
            webReq.AutomaticDecompression = DecompressionMethods.None

            'If (Server.DumpHeaders) Then
            'OnProxyLog(New HTTPProxyLogEventArgs(String.Format("{0} {1} HTTP/{2}", webReq.Method, webReq.RequestUri.AbsoluteUri, webReq.ProtocolVersion)))
            'DumpHeaderCollectionToConsole(webReq.Headers)
            'End If


            'imports the completed request, check our cache
            Me.m_CacheEntry = Me.Server.ProxyCache.GetData(webReq)

            Return webReq
        End Function

        Protected Overridable Function ResolvePost() As HttpWebRequest
            Dim webReq As HttpWebRequest = Nothing
            Dim postBuffer As Char() = Nothing
            Dim sw As StreamWriter = Nothing

            Try
                'construct the web request that we are going to issue on behalf of the client.
                webReq = HttpWebRequest.Create(Me.RemoteUri)
                webReq.Method = Me.Method
                webReq.ProtocolVersion = Me.Version
                webReq.Timeout = Me.m_Server.TimeOut

                'construct the web request that we are going to issue on behalf of the client.

                'read the request headers from the client and copy them to our request
                Dim contentLen As Integer = ReadRequestHeaders(Me.m_ClientStreamReader, webReq)

                webReq.Proxy = Me.Server.Proxy
                webReq.KeepAlive = False
                webReq.AllowAutoRedirect = False
                webReq.AutomaticDecompression = DecompressionMethods.None


                postBuffer = Sistema.Arrays.CreateInstance(Of Char)(contentLen)
                Dim bytesRead As Integer
                Dim totalBytesRead As Integer = 0

                sw = New StreamWriter(webReq.GetRequestStream(), System.Text.Encoding.ASCII)
                While (totalBytesRead < contentLen)
                    bytesRead = Me.m_ClientStreamReader.ReadBlock(postBuffer, 0, contentLen)
                    If (bytesRead > 0) Then sw.Write(postBuffer, 0, bytesRead)
                    totalBytesRead += bytesRead
                    'If (ProxyServer.Server.DumpPostData) Then ProxyServer.OnProxyLog(New HTTPProxyLogEventArgs(New String(postBuffer, 0, bytesRead)))
                End While

                sw.Close()
                'End If

                Return webReq
            Catch ex As Exception
                If (webReq IsNot Nothing) Then webReq.Abort()
                Throw
            Finally
                'If (sw IsNot Nothing) Then sw.Close() : sw = Nothing
                If (postBuffer IsNot Nothing) Then Erase postBuffer : postBuffer = Nothing
            End Try
        End Function

        Friend Sub ServeFromFile(ByVal fileName As String)
            'serve from cache
            Dim myResponseWriter As StreamWriter = New StreamWriter(Me.m_ClientStream, System.Text.Encoding.ASCII) 'outStream
            Try
                WriteResponseStatus("200", "Contenuto bloccato", myResponseWriter)
                WriteResponseHeaders(myResponseWriter, New List(Of HttpHeader))
                Dim bytes() As Byte = System.IO.File.ReadAllBytes(fileName)
                Me.m_ClientStream.Write(bytes, 0, bytes.Length) 'outStream.Write(CacheEntry.ResponseBytes, 0, CacheEntry.ResponseBytes.Length)
                myResponseWriter.Close()
            Catch ex As Exception
                'ProxyServer.OnProxyLog(New HTTPProxyLogEventArgs(ex.Message))
                Throw
            Finally
                myResponseWriter.Close()
            End Try

        End Sub

        Protected Friend Overridable Sub Man()
            'Browser wants to create a secure tunnel
            'instead = we are going to perform a man in the middle "attack"
            'the user's browser should warn them of the certification errors however.
            'Please note: THIS IS ONLY FOR TESTING PURPOSES - you are responsible for the use of this code

            Me.m_RemoteURI = "https: //" & Me.m_RemoteURI
            Dim t As Date = Now
            While (Not String.IsNullOrEmpty(Me.m_ClientStreamReader.ReadLine()))
                If (Now - t).TotalMilliseconds > 5000 Then
                    'Throw New TimeoutException("Cant' connect")
                End If
            End While

            Dim connectStreamWriter As StreamWriter = New StreamWriter(Me.ClientStream, System.Text.Encoding.ASCII)
            connectStreamWriter.WriteLine("HTTP/1.0 200 Connection established")
            connectStreamWriter.WriteLine(String.Format("Timestamp: {0}", DateTime.Now))
            If (Me.Server.ProxyAgent <> "") Then connectStreamWriter.WriteLine("Proxy-agent: " & Me.Server.ProxyAgent)
            connectStreamWriter.WriteLine()
            connectStreamWriter.Flush()

            Me.m_SslStream = New SslStream(Me.ClientStream, False)
            Try
                Me.m_SslStream.AuthenticateAsServer(Me.Server._certificate, False, SslProtocols.Tls Or SslProtocols.Ssl3 Or SslProtocols.Ssl2, True)
            Catch ex As Exception
                Me.m_SslStream.Close()
                Me.m_ClientStreamReader.Close()
                connectStreamWriter.Close()
                Me.ClientStream.Close()
                Me.Server.OnProxyLog(New HTTPProxyLogEventArgs("Problema con la connessione SSL server: " & ex.Message))
                Return
            End Try

            'HTTPS server created - we can now decrypt the client's traffic
            Me.ClientStream = Me.m_SslStream
            Me.m_ClientStreamReader = New StreamReader(Me.m_SslStream, System.Text.Encoding.ASCII)
            'outStream = SslStream

            'read the new http command.
            Dim httpCmd As String = Me.m_ClientStreamReader.ReadLine()
            If (String.IsNullOrEmpty(httpCmd)) Then
                Me.m_ClientStreamReader.Close()
                Me.ClientStream.Close()
                Me.m_SslStream.Close()
                Return
            End If

            Dim splitBuffer As String() = httpCmd.Split(ProxyUtils.spaceSplit, 3)
            Me.m_Method = splitBuffer(0)
            Me.m_RemoteURI = Me.m_RemoteURI & splitBuffer(1)

            connectStreamWriter.Close()
        End Sub

        'Private Function ResolveName(ByVal name As String) As IPAddress
        '    Dim ret As IPHostEntry = Dns.GetHostEntry(name)
        '    If (ret Is Nothing OrElse ret.AddressList.Count = 0) Then Return Nothing
        '    Return ret.AddressList(0)
        'End Function

        Friend Class StateAR
            Implements IDisposable

            Public owner As ProxyRequest
            Public s As ManualResetEvent
            Public th As System.Threading.Thread
            Public ar As IAsyncResult
            Public SourceStream As Stream
            Public TargetStream As Stream
            Public buffer() As Byte
            Public Stopping As Boolean = False

            Public Sub New(ByVal owner As ProxyRequest, ByVal source As Stream, ByVal target As Stream, ByVal s As ManualResetEvent)
                Me.s = s
                Me.owner = owner
                Me.SourceStream = source
                Me.TargetStream = target
                Me.buffer = Array.CreateInstance(GetType(Byte), 1024)
            End Sub

            'Public Function BeginRead(ByVal c As AsyncCallback) As IAsyncResult
            '    Me.ar = Me.Stream.BeginRead(Me.buffer, 0, Me.buffer.Length, c, Me)
            '    Return Me.ar
            'End Function

            'Public Function EndRead() As Integer
            '    Return Me.Stream.EndRead(Me.ar)
            'End Function

            Public Sub BeginRead()
                Dim wc As New WaitCallback(AddressOf Me.doJob)
                If (Not ThreadPool.QueueUserWorkItem(wc, Me)) Then
                    Throw New Exception
                End If

                'Me.th = New System.Threading.Thread(AddressOf doJob)
                'Me.th.Priority = ThreadPriority.Lowest
                'Me.th.Start()
            End Sub

            Private Sub doJob(ByVal o As Object)
                Try
                    Dim nRecv As Integer = Me.SourceStream.Read(Me.buffer, 0, Me.buffer.Length)
                    While (nRecv > 0 AndAlso Not Me.Stopping)
                        Me.TargetStream.Write(Me.buffer, 0, nRecv)
                        Me.TargetStream.Flush()
                        nRecv = Me.SourceStream.Read(Me.buffer, 0, Me.buffer.Length)
                        Me.owner.m_LastClient = Now
                    End While
                    Me.s.Set()
                    Return
                Catch iex As System.IO.IOException
                    Debug.Print(iex.Message & vbNewLine & iex.StackTrace)
                Catch ex As Exception
                    Debug.Print(ex.Message & vbNewLine & ex.StackTrace)
                End Try

                Try
                    If (Me.s IsNot Nothing) Then Me.s.Set()
                Catch ex As Exception
                    Debug.Print(ex.Message & vbNewLine & ex.StackTrace)
                End Try


            End Sub

            Public Sub [Stop]()
                Me.Stopping = True
                If (Me.th Is Nothing) Then Return
                Try
                    If Me.s IsNot Nothing Then Me.s.Set()
                Catch ex As Exception

                End Try
                Try
                    Me.th.Join(10000)
                Catch ex As Exception
                    Me.th.Abort()
                End Try
                Me.th = Nothing
            End Sub

#Region "IDisposable Support"
            Private disposedValue As Boolean ' Per rilevare chiamate ridondanti

            ' IDisposable
            Protected Overridable Sub Dispose(disposing As Boolean)
                If Not disposedValue Then
                    If disposing Then
                        If (Me.th IsNot Nothing) Then Me.th.Abort() : Me.th = Nothing
                        If Me.s IsNot Nothing Then Me.s.Set()
                        Me.owner = Nothing
                        Me.th = Nothing
                        Me.SourceStream = Nothing
                        Me.TargetStream = Nothing
                        Me.buffer = Nothing
                    End If

                    ' TODO: liberare risorse non gestite (oggetti non gestiti) ed eseguire sotto l'override di Finalize().
                    ' TODO: impostare campi di grandi dimensioni su Null.
                End If
                disposedValue = True
            End Sub

            ' TODO: eseguire l'override di Finalize() solo se Dispose(disposing As Boolean) include il codice per liberare risorse non gestite.
            'Protected Overrides Sub Finalize()
            '    ' Non modificare questo codice. Inserire sopra il codice di pulizia in Dispose(disposing As Boolean).
            '    Dispose(False)
            '    MyBase.Finalize()
            'End Sub

            ' Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Non modificare questo codice. Inserire sopra il codice di pulizia in Dispose(disposing As Boolean).
                Dispose(True)
                ' TODO: rimuovere il commento dalla riga seguente se è stato eseguito l'override di Finalize().
                ' GC.SuppressFinalize(Me)
            End Sub
#End Region

        End Class

        Public Sub Terminate()
            Me.Dispose()
        End Sub

        Public ReadOnly Property InactiveTime As Integer
            Get
                If (Me.m_LastClient.HasValue) Then
                    Return (Now - Me.m_LastClient.Value).TotalMilliseconds
                Else
                    Return (Now - Me.m_StartDate).TotalMilliseconds
                End If
            End Get
        End Property

        Friend Shared Sub CheckPending(ByVal maxMilli As Integer)
            SyncLock pending
                Dim i As Integer = 0
                While (i < pending.Count)
                    Dim r As ProxyRequest = pending(i)
                    If (r.isDisposed) Then
                        pending.Remove(r)
                    ElseIf (r.InactiveTime > maxMilli) Then
                        pending.Remove(r)
                        r.Dispose()
                    Else
                        i += 1
                    End If
                End While
            End SyncLock
        End Sub


        'Private remoteStream As NetworkStream = Nothing
        'Private remoteServer As TcpClient = Nothing

        'Private Sub BeginReadClient()
        '    Me.arC = Me.ClientStream.BeginRead(Me.bufferC, 0, bufferC.Length, AddressOf onClientDataC, Me)
        'End Sub

        'Private Sub onClientDataC(ByVal ar As IAsyncResult)
        '    SyncLock Me
        '        Try
        '            Dim nRecv As Integer = Me.ClientStream.EndRead(Me.arC)
        '            If (nRecv > 0) Then Me.remoteStream.Write(Me.bufferC, 0, nRecv)
        '            Me.arC = Me.ClientStream.BeginRead(Me.bufferC, 0, bufferC.Length, AddressOf onClientDataC, Me)
        '        Catch ex As Exception
        '            'Catch ex As ObjectDisposedException
        '            Me.remoteStream.Close()
        '            Me.remoteclient.Close()
        '        End Try
        '    End SyncLock
        'End Sub

        'Private Sub BeginReadServer()
        '    Me.arS = Me.remoteStream.BeginRead(Me.bufferS, 0, bufferS.Length, AddressOf onServerDataS, Me)
        'End Sub

        'Private Sub onServerDataS(ByVal ar As IAsyncResult)
        '    SyncLock Me
        '        Try
        '            Dim nRecv As Integer = Me.remoteStream.EndRead(Me.arS)
        '            If (nRecv > 0) Then Me.m_ClientStream.Write(bufferS, 0, nRecv)
        '            Me.arS = Me.remoteStream.BeginRead(Me.bufferS, 0, bufferS.Length, AddressOf onServerDataS, Me)
        '        Catch ex As Exception
        '            'Catch ex As ObjectDisposedException
        '            Me.remoteStream.Close()
        '            Me.remoteclient.Close()
        '        End Try
        '    End SyncLock
        'End Sub



        'Private Sub BeginReadClient(ByVal buff As StateAR)
        '    buff.BeginRead(New AsyncCallback(AddressOf onClientDataC))
        'End Sub

        'Private Sub onClientDataC(ByVal ar As IAsyncResult)
        '    Dim buff As StateAR = CType(ar.AsyncState, StateAR)
        '    Dim nRecv As Integer = 0
        '    Try
        '        nRecv = buff.EndRead()
        '        While (nRecv > 0)
        '            Me.remoteStream.Write(buff.buffer, 0, nRecv)
        '            Me.remoteStream.Flush()
        '            'Me.BeginReadClient(buff)
        '            nRecv = buff.Stream.Read(buff.buffer, 0, buff.buffer.Length)
        '        End While
        '        If (Me.s IsNot Nothing) Then Me.s.Set()
        '    Catch ex As Exception
        '        Debug.Print(ex.Message & vbNewLine & ex.StackTrace)
        '        If (Me.s IsNot Nothing) Then Me.s.Set()
        '    End Try
        'End Sub

        'Private Sub BeginReadServer(ByVal buff As StateAR)
        '    buff.BeginRead(New AsyncCallback(AddressOf onServerDataC))
        'End Sub

        'Private Sub onServerDataC(ByVal ar As IAsyncResult)
        '    Dim buff As StateAR = CType(ar.AsyncState, StateAR)
        '    Dim nRecv As Integer = 0
        '    Try
        '        nRecv = buff.EndRead()
        '        While (nRecv > 0)
        '            Me.ClientStream.Write(buff.buffer, 0, nRecv)
        '            Me.ClientStream.Flush()
        '            'Me.BeginReadServer(buff)
        '            nRecv = buff.Stream.Read(buff.buffer, 0, buff.buffer.Length)
        '        End While
        '        If (Me.s IsNot Nothing) Then Me.s.Set()
        '    Catch ex As Exception
        '        Debug.Print(ex.Message & vbNewLine & ex.StackTrace)
        '        If (Me.s IsNot Nothing) Then Me.s.Set()
        '    End Try
        'End Sub

        Friend s As ManualResetEvent
        Friend buffC As StateAR
        Friend buffS As StateAR
        Friend remoteServer As TcpClient
        Friend remoteStream As Stream

        Public Sub StartTunneling()
            'leggiano gli headers
            Dim headers As New CKeyCollection
            Dim line As String = Me.m_ClientStreamReader.ReadLine
            Dim lineCount As Integer = 0
            While (line <> "")
                lineCount += 1
                If (lineCount > 100) Then
                    Debug.Print("Opps")
                End If
                Dim pColon As Integer = line.IndexOf(":")
                If (pColon >= 0 AndAlso pColon < line.Length - 1) Then
                    Dim headerName As String = line.Substring(0, pColon)
                    Dim headerValue As String = Trim(line.Substring(pColon + 1))
                    headers.Add(headerName, headerValue)
                End If
                line = Me.m_ClientStreamReader.ReadLine
            End While

            Dim name As String = ""
            Dim port As Integer = 443
            Dim p As Integer = Me.m_RemoteURI.LastIndexOf(":")
            If (p > 0) Then
                name = Me.m_RemoteURI.Substring(0, p)
                port = CInt(Me.m_RemoteURI.Substring(p + 1))
            Else
                name = Me.m_RemoteURI
            End If


            Dim connectStreamWriter As New StreamWriter(Me.ClientStream, System.Text.Encoding.ASCII)

            Try
                Me.remoteServer = New TcpClient
                Me.remoteServer.Connect(name, port)
                Me.remoteStream = remoteServer.GetStream
                ' Return
            Catch ex As Exception
                If Me.remoteStream IsNot Nothing Then Me.remoteStream.Close() : Me.remoteStream = Nothing
                If Me.remoteServer IsNot Nothing Then Me.remoteServer.Close() : Me.remoteServer = Nothing

                connectStreamWriter.WriteLine("HTTP/1.0 502 Bad Gateway")
                connectStreamWriter.WriteLine("Proxy-agent: " & Me.Server.ProxyAgent)
                connectStreamWriter.WriteLine("")
                connectStreamWriter.Flush()
                SyncLock pending
                    pending.Remove(Me)
                End SyncLock
                Me.Dispose()
                Return
            Finally

            End Try


            'SyncLock Me
            connectStreamWriter.WriteLine("HTTP/1.0 200 Connection established")
            connectStreamWriter.WriteLine(String.Format("Timestamp: {0}", DateTime.Now))
            If (Me.Server.ProxyAgent <> "") Then connectStreamWriter.WriteLine("Proxy-agent: " & Me.Server.ProxyAgent)
            connectStreamWriter.WriteLine()
            connectStreamWriter.Flush()

            Me.ClientStream.ReadTimeout = Me.Server.TimeOut
            Me.remoteStream.WriteTimeout = Me.Server.TimeOut

#If 1 Then
            Me.s = New ManualResetEvent(False)
            Me.buffC = New StateAR(Me, Me.ClientStream, Me.remoteStream, s)
            Me.buffS = New StateAR(Me, remoteStream, Me.ClientStream, s)

            buffC.BeginRead()
            buffS.BeginRead()

            s.WaitOne()

            If (buffC IsNot Nothing) Then
                buffC.s = Nothing
                buffC.Dispose() : Me.buffC = Nothing
            End If
            If (buffS IsNot Nothing) Then
                buffS.s = Nothing
                buffS.Dispose() : Me.buffS = Nothing
            End If
            If (s IsNot Nothing) Then Me.s.Dispose() : Me.s = Nothing



#Else
            Dim buff() As Byte = Array.CreateInstance(GetType(Byte), 2048)
            Dim nRead As Integer

            Try
                nRead = Me.ClientStream.Read(buff, 0, buff.Length)
                While (nRead > 0)
                    remoteStream.Write(buff, 0, nRead)
                    nRead = Me.ClientStream.Read(buff, 0, buff.Length)
                End While
                remoteStream.Flush()

                nRead = remoteStream.Read(buff, 0, buff.Length)
                While (nRead > 0)
                    Me.ClientStream.Write(buff, 0, nRead)
                    nRead = remoteStream.Read(buff, 0, buff.Length)
                End While

                Me.ClientStream.Flush()
            Catch ex As Exception
                Debug.Print(ex.Message)
            End Try
#End If


            Try
                If (Me.remoteStream IsNot Nothing) Then
                    remoteStream.Flush()
                    remoteStream.Close()
                    remoteStream = Nothing
                End If
            Catch ex As Exception
                Debug.Print(ex.Message)
            End Try

            Try
                If Me.remoteServer IsNot Nothing Then
                    remoteServer.Close()
                    remoteServer = Nothing
                End If
            Catch ex As Exception
                Debug.Print(ex.Message)
            End Try

            'Try
            '    Me.ClientStream.Flush()
            '    Me.ClientStream.Close()
            '    Me.ClientStream = Nothing
            'Catch ex As Exception
            '    Debug.Print(ex.Message)
            'End Try
            'DMD.Sistema.FileSystem.CopyStream(Me.ClientStream, Me.remoteStream)
            'Me.remoteStream.Flush()
            SyncLock pending
                pending.Remove(Me)
            End SyncLock
            Me.Dispose()
        End Sub

        Public ReadOnly Property ExeTime As Long
            Get
                If (Me.m_EndDate.HasValue) Then Return (Me.m_EndDate.Value - Me.m_StartDate).TotalMilliseconds
                Return (Now - Me.m_StartDate).TotalMilliseconds
            End Get
        End Property

        Protected Friend Overridable Sub DownloadRemoteResource(ByVal webReq As HttpWebRequest)
            Dim response As HttpWebResponse = Nothing
            Dim buffer As Byte() = Nothing
            Dim bytesRead As Integer
            Dim responseHeaders As List(Of HttpHeader) = Nothing
            Dim myResponseWriter As StreamWriter = Nothing
            Dim responseStream As Stream = Nothing
            Dim expires As DateTime? = Nothing
            Dim entry As CacheEntry = Nothing
            Dim canCache As Boolean = False

            'Console.WriteLine(String.Format("ThreadID: {2} Requesting {0} on behalf of client {1}", webReq.RequestUri, client.Client.RemoteEndPoint.ToString(), Thread.CurrentThread.ManagedThreadId));
            'webReq.Timeout  = Me.m_Server.TimeOut

            Try
                response = webReq.GetResponse()
            Catch webEx As WebException
                ' response = webEx.Response 'as HttpWebResponse
                myResponseWriter = New StreamWriter(Me.ClientStream, System.Text.Encoding.ASCII) 'outStream
                myResponseWriter.WriteLine("HTTP/1.0 500 " & webEx.Message)
                myResponseWriter.WriteLine("Proxy-agent: " & Me.Server.ProxyAgent)
                myResponseWriter.WriteLine("")
                myResponseWriter.Flush()
                myResponseWriter.Close()
                SyncLock pending
                    pending.Remove(Me)
                End SyncLock
                Me.Dispose()
                Return
            End Try

            If (response IsNot Nothing) Then
                Try
                    responseHeaders = Me.ProcessResponse(response)
                    Debug.Assert(Me.ClientStream IsNot Nothing)
                    myResponseWriter = New StreamWriter(Me.ClientStream, System.Text.Encoding.ASCII) 'outStream
                    responseStream = response.GetResponseStream()
                    Debug.Assert(responseStream IsNot Nothing)
                    'send the response status and response headers
                    WriteResponseStatus(response.StatusCode, response.StatusDescription, myResponseWriter)
                    WriteResponseHeaders(myResponseWriter, responseHeaders)

                    canCache = (Me.m_SslStream Is Nothing AndAlso Me.Server.ProxyCache.CanCache(response.Headers, expires))
                    If (canCache) Then
                        entry = Me.Server.ProxyCache.MakeEntry(webReq, response, responseHeaders, expires)
                        If (response.ContentLength > 0) Then
                            entry.Stream = New MemoryStream(entry.ContentLength)
                        Else
                            entry.Stream = New MemoryStream()
                        End If
                    End If


                    If (response.ContentLength > 0) Then
                        buffer = DMD.Sistema.Arrays.CreateInstance(Of Byte)(response.ContentLength)
                    Else
                        buffer = DMD.Sistema.Arrays.CreateInstance(Of Byte)(BUFFER_SIZE)
                    End If



                    bytesRead = responseStream.Read(buffer, 0, buffer.Length)
                    While (bytesRead > 0)
                        If (entry IsNot Nothing) Then entry.Stream.Write(buffer, 0, bytesRead)
                        Me.m_ClientStream.Write(buffer, 0, bytesRead) 'outStream.Write(buffer, 0, bytesRead)
                        'If (Server.DumpResponseData) Then OnProxyLog(New HTTPProxyLogEventArgs(UTF8Encoding.UTF8.GetString(buffer, 0, bytesRead)))
                        bytesRead = responseStream.Read(buffer, 0, buffer.Length)
                    End While

                    If (entry IsNot Nothing) Then entry.Stream.Flush()
                    Me.ClientStream.Flush() 'outStream.Flush()

                    If (canCache) Then
                        Me.Server.ProxyCache.AddData(entry)
                    End If

                    '#If Not DEBUG Then
                Catch ex As Exception
                    ' ProxyServer.OnProxyLog(New HTTPProxyLogEventArgs(ex.Message))
                    ' Throw
                    Debug.Print(ex.Message & vbNewLine & ex.StackTrace)
                Finally
                    '#End If
                    'If (cacheStream IsNot Nothing) Then
                    '    cacheStream.Flush()
                    '    cacheStream.Close()
                    'End If
                    'cacheStream = Nothing

                    If (responseStream IsNot Nothing) Then responseStream.Close() : responseStream = Nothing
                    If (response IsNot Nothing) Then response.Close() : response = Nothing
                    If (myResponseWriter IsNot Nothing) Then myResponseWriter.Close() : myResponseWriter = Nothing

                    If (buffer IsNot Nothing) Then Erase buffer : buffer = Nothing
                    '#If Not DEBUG Then
                End Try
                '#End If
            Else

            End If
        End Sub



        ''' <summary>
        ''' Interpreta la richiesta fatta dal client
        ''' </summary>
        ''' <remarks></remarks>
        Friend Sub ParseRequest()
            If (Me.m_Server Is Nothing) Then Throw New ArgumentNullException("server")
            If (Me.m_Client Is Nothing) Then Throw New ArgumentNullException("client")

            Dim clientStream As Stream = Client.GetStream()
            'Dim outStream As Stream = clientStream 'use this stream for writing out - may change if we use ssl
            Dim clientStreamReader As StreamReader = New StreamReader(clientStream, System.Text.Encoding.ASCII)

            Dim httpCmd As String = clientStreamReader.ReadLine()

            If (String.IsNullOrEmpty(httpCmd)) Then
                clientStreamReader.Close()
                clientStream.Close()
                Return
            End If
            'clientStreamReader.Close()

            'break up the line into three components
            Dim splitBuffer As String() = httpCmd.Split(ProxyUtils.spaceSplit, 3)

            Dim method As String = splitBuffer(0)
            Dim remoteUri As String = splitBuffer(1)
            Dim version As Version = New Version(1, 0) 'parseVersion(splitBuffer(2)) 

            'e.m_Server = Server
            'e.m_Client = Client
            Me.m_ClientStream = clientStream
            Me.m_ClientStreamReader = clientStreamReader
            Me.m_Method = method
            Me.m_RemoteURI = remoteUri
            Me.m_Version = version
        End Sub


        Private Shared Function parseVersion(ByVal str As String) As Version
            Dim major As Integer = 1
            Dim minor As Integer = 0
            str = Strings.Trim(str)
            If (str <> "") Then
                Dim p As Integer
                p = str.IndexOf("/")
                If (p >= 0) Then str = str.Substring(p + 1)
                p = str.IndexOf(".")
                Try
                    If (p > 0) Then
                        major = CInt(str.Substring(0, p))
                        minor = CInt(str.Substring(p + 1))
                    End If
                Catch ex As Exception
                    Debug.Print("parseVersion Error (" & str & ")" & vbCrLf & ex.Message)
                    major = 1
                    minor = 0
                End Try
            End If
            Return New Version(major, minor)
        End Function


        Protected Overridable Function ProcessResponse(ByVal response As HttpWebResponse) As List(Of HttpHeader)
            Dim value As String = vbNullString
            Dim header As String = vbNullString
            Dim returnHeaders As List(Of HttpHeader) = New List(Of HttpHeader)
            For Each s As String In response.Headers.Keys
                If (s.ToLower() = "set-cookie") Then
                    header = s
                    value = response.Headers(s)
                Else
                    returnHeaders.Add(New HttpHeader(s, response.Headers(s)))
                End If
            Next

            If (Not DMD.Sistema.Strings.IsNullOrWhiteSpace(value)) Then
                response.Headers.Remove(header)
                Dim cookies As String() = ProxyUtils.cookieSplitRegEx.Split(value)
                For Each cookie As String In cookies
                    returnHeaders.Add(New HttpHeader("Set-Cookie", cookie))
                Next
            End If
            returnHeaders.Add(New HttpHeader("X-Proxied-By", "FSEProxy"))
            Return returnHeaders
        End Function

        Protected Overridable Sub WriteResponseStatus(ByVal code As HttpStatusCode, ByVal description As String, ByVal myResponseWriter As StreamWriter)
            Dim s As String = String.Format("HTTP/1.0 {0} {1}", CInt(code), description)
            myResponseWriter.WriteLine(s)
            ' If (ProxyServer.Server.DumpHeaders) Then ProxyServer.OnProxyLog(New HTTPProxyLogEventArgs(s))
        End Sub

        Protected Overridable Sub WriteResponseHeaders(ByVal myResponseWriter As StreamWriter, ByVal headers As List(Of HttpHeader))
            If (headers IsNot Nothing) Then
                For Each header As HttpHeader In headers
                    myResponseWriter.WriteLine(String.Format("{0}: {1}", header.Key, header.Value))
                Next
            End If
            myResponseWriter.WriteLine()
            myResponseWriter.Flush()

            'If (ProxyServer.Server.DumpHeaders) Then ProxyServer.DumpHeaderCollectionToConsole(headers)
        End Sub

        Protected Friend Overridable Sub ServeFromCache(webReq As HttpWebRequest)
            'serve from cache
            Dim myResponseWriter As StreamWriter = New StreamWriter(Me.m_ClientStream, System.Text.Encoding.ASCII) 'outStream
            Try
                Me.CacheEntry.LastUsed = Now
                WriteResponseStatus(Me.CacheEntry.StatusCode, Me.CacheEntry.StatusDescription, myResponseWriter)
                WriteResponseHeaders(myResponseWriter, Me.CacheEntry.Headers)
                If (Me.CacheEntry.Stream IsNot Nothing) Then
                    Me.CacheEntry.Stream.Position = 0
                    DMD.Sistema.FileSystem.CopyStream(Me.CacheEntry.Stream, Me.m_ClientStream)
                    'Me.m_ClientStream.Write(Me.CacheEntry.ResponseBytes, 0, Me.CacheEntry.ResponseBytes.Length) 'outStream.Write(CacheEntry.ResponseBytes, 0, CacheEntry.ResponseBytes.Length)
                End If
                Me.m_ClientStream.Flush()
                myResponseWriter.Close()

            Catch ex As Exception
                'ProxyServer.OnProxyLog(New HTTPProxyLogEventArgs(ex.Message))
                Throw
            Finally
                myResponseWriter.Close()
            End Try
        End Sub


#Region "IDisposable Support"


        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            Me.isDisposed = True
            If (Me.s IsNot Nothing) Then Me.s.Dispose() : Me.s = Nothing
            If (Me.buffC IsNot Nothing) Then Me.buffC.Stop() : Me.buffC = Nothing
            If (Me.buffS IsNot Nothing) Then Me.buffS.Stop() : Me.buffS = Nothing
            If (Me.remoteStream IsNot Nothing) Then Me.remoteStream.Dispose() : Me.remoteStream = Nothing
            If (Me.remoteServer IsNot Nothing) Then Me.remoteServer.Close() : Me.remoteServer = Nothing
            If (Me.m_ClientStreamReader IsNot Nothing) Then Me.m_ClientStreamReader.Close() : Me.m_ClientStreamReader = Nothing
            If (Me.m_ClientStream IsNot Nothing) Then
                Me.m_ClientStream.Flush()
                Me.m_ClientStream.Close()
                Me.m_ClientStream = Nothing
            End If
            If (Me.m_SslStream IsNot Nothing) Then Me.m_SslStream.Close() : Me.m_SslStream = Nothing
            If (Me.m_Client IsNot Nothing) Then Me.m_Client.Close() : Me.m_Client = Nothing

            'GC.SuppressFinalize(Me)
            Me.m_CacheEntry = Nothing
            Me.m_Server = Nothing
        End Sub
#End Region

        Protected Overridable Function ResolveOther() As HttpWebRequest
            Throw New NotImplementedException
        End Function

    End Class

End Namespace

