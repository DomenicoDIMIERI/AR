Imports System
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Net.Security
Imports System.Security.Authentication
Imports System.Security.Cryptography.X509Certificates
Imports DMD.Sistema

Namespace Net.HTTPProxy

    Public NotInheritable Class HTTPProxyServer

        ''' <summary>
        ''' Evento generato quando un client si connette al server proxy per richiedere una risorsa remota
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event NewRequest(ByVal sender As Object, ByVal e As ProxyRequestEventArgs)

        ''' <summary>
        ''' Evento gernato quando la risorsa richiesta dal client non è presente nella cache
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event CacheMiss(ByVal sender As Object, ByVal e As ProxyCacheMissEventArgs)

        ''' <summary>
        ''' Evento generato dal server proxy per le variazioni di stato 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ProxyLog(ByVal sender As Object, ByVal e As HTTPProxyLogEventArgs)


        Friend _outputLockObj As Object = New Object()

        Friend _certificate As X509Certificate2
        Friend m_ProxyAgent As String

        Private m_TimeOut As Integer
        Private addr As IPAddress
        Private port As Int32
        Private _listener As TcpListener
        Private _listenerThread As Thread
        Private _cacheMaintenanceThread As Thread
        Private m_ProxyCache As ProxyCache
        Private m_Proxy As WebProxy
        Private m_SSLTunneling As Boolean

        'Private _DumpHeaders As Boolean
        'Private _DumpPostData As Boolean
        'Private _DumpResponseData As Boolean


        Public Sub New()
            Me._outputLockObj = New Object()
            Me.m_ProxyAgent = "DMD"
            Me.m_TimeOut = 15000
            Me.addr = IPAddress.Loopback
            Me.port = 8081
            Me._listener = Nothing '
            Me.m_ProxyAgent = Nothing
            Me.m_SSLTunneling = True
        End Sub



        Public Property SSLTunneling As Boolean
            Get
                Return Me.m_SSLTunneling
            End Get
            Set(value As Boolean)
                Me.m_SSLTunneling = value
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il proxy che serve le richieste remote
        ''' </summary>
        ''' <returns></returns>
        Public Property Proxy As WebProxy
            Get
                Return Me.m_Proxy
            End Get
            Set(value As WebProxy)
                Me.m_Proxy = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'interfaccia di ascolto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ListeningIPInterface As IPAddress
            Get
                Return Me.addr
            End Get
            Set(value As IPAddress)
                If (Me.addr.Equals(value)) Then Exit Property
                If Me.IsRunning Then Throw New InvalidOperationException
                Me.addr = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la stringa da utilizzare come proxy agent
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProxyAgent As String
            Get
                Return Me.m_ProxyAgent
            End Get
            Set(value As String)
                Me.m_ProxyAgent = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il porto di ascolto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ListeningPort As Int32
            Get
                Return Me.port
            End Get
            Set(value As Int32)
                If Me.port = value Then Exit Property
                If Me.IsRunning Then Throw New InvalidOperationException
                Me.port = value
            End Set
        End Property

        '''' <summary>
        '''' Restituisce o imposta il percorso del certificato utilizzato per le connessioni SSL
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Property CertificateFilePath As String
        '    Get
        '        Return Me._certFilePath
        '    End Get
        '    Set(value As String)
        '        Me._certFilePath = Strings.Trim(value)
        '    End Set
        'End Property

        Public Property SSLCertificate As X509Certificate2
            Get
                Return Me._certificate
            End Get
            Set(value As X509Certificate2)
                If value Is Me._certificate Then Return
                If (Me.IsRunning) Then Throw New InvalidOperationException
                Me._certificate = value
            End Set
        End Property

        Public Function IsRunning() As Boolean
            SyncLock Me
                Return Me._listener IsNot Nothing
            End SyncLock
        End Function



        'Public Property DumpHeaders As Boolean

        'Public Property DumpPostData As Boolean

        'Public Property DumpResponseData As Boolean


        ''' <summary>
        ''' Restituisce la cache del proxy server
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ProxyCache As ProxyCache
            Get
                SyncLock Me
                    If (Me.m_ProxyCache Is Nothing) Then Me.m_ProxyCache = New ProxyCache(Me)
                    Return Me.m_ProxyCache
                End SyncLock

            End Get
        End Property


        ''' <summary>
        ''' Avvia l'ascolto del server Proxy
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Start()
            SyncLock Me
                If (Me._listener IsNot Nothing) Then Throw New InvalidOperationException("Già in ascolto")
                'Try
                '    Try
                '        Me._certificate = New X509Certificate2(Me._certFilePath) 'New X509Certificate2("c:\temp\DMDsrl.pfx", "DMD2013") ' 
                '    Catch ex As Exception
                '        Throw New Exception(String.Format("Could not create the certificate from file from {0}", Me._certFilePath), ex)
                '    End Try

                'Catch ex As Exception
                '    Me.OnProxyLog(New HTTPProxyLogEventArgs(ex.Message))
                'End Try
                If (Me._certificate Is Nothing) Then
                    Me.OnProxyLog(New HTTPProxyLogEventArgs("Certificato SSL non valido!"))
                End If


                Me._listener = New TcpListener(Me.addr, Me.port)
                Me._listener.Start()

                Me._listenerThread = New Thread(New ParameterizedThreadStart(AddressOf Listen))
                Me._listenerThread.Priority = ThreadPriority.BelowNormal


                Me._cacheMaintenanceThread = New Thread(New ThreadStart(AddressOf Me.ProxyCache.CacheMaintenance))
                Me._cacheMaintenanceThread.Priority = ThreadPriority.AboveNormal

                Me._listenerThread.Start(_listener)
                Me._cacheMaintenanceThread.Start()

            End SyncLock
        End Sub




        Private Sub Listen(ByVal obj As Object)
            Dim listener As TcpListener = obj
            Try
                While (True)
                    Dim client As TcpClient = listener.AcceptTcpClient()
                    Dim ret As Boolean
                    Dim wc As New WaitCallback(AddressOf Me.ProcessClient)
                    client.NoDelay = True
                    Do
                        ret = ThreadPool.QueueUserWorkItem(wc, client)
                        System.Threading.Thread.Sleep(100)
                    Loop While (Not ret)



                End While
            Catch e As ThreadAbortException
                Debug.Print("HttpProxyServer.Listen ->" & e.Message)
            Catch e As SocketException
                Debug.Print("HttpProxyServer.Listen ->" & e.Message)
            End Try
        End Sub


        ''' <summary>
        ''' Forma l'ascolto del server proxy
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub [Stop]()
            SyncLock Me
                Try
                    _listener.Stop()
                Catch ex As Exception
                    Debug.Print(ex.Message)
                End Try
                'wait for server to finish processing current connections...

                Try
                    _listenerThread.Abort()
                Catch ex As Exception
                    Debug.Print(ex.Message)
                End Try

                Try
                    _cacheMaintenanceThread.Abort()
                Catch ex As Exception
                    Debug.Print(ex.Message)
                End Try

                Try
                    _listenerThread.Join()
                Catch ex As Exception
                    Debug.Print(ex.Message)
                End Try
                Try
                    _cacheMaintenanceThread.Join()
                Catch ex As Exception
                    Debug.Print(ex.Message)
                End Try

                _listenerThread = Nothing
                _cacheMaintenanceThread = Nothing
                _listener = Nothing
            End SyncLock
        End Sub




        Private Sub ProcessClient(ByVal obj As Object)
            Me.DoHttpProcessing(obj)
        End Sub





        Private Sub DoHttpProcessing(ByVal client As TcpClient)

            'If (Server.DumpHeaders OrElse Server.DumpPostData OrElse Server.DumpResponseData) Then
            '    'make sure that things print out in order - NOTE: this is bad for performance
            '    Monitor.TryEnter(_outputLockObj, TimeSpan.FromMilliseconds(-1.0))
            'End If
            Dim req As ProxyRequest = Nothing
            Try
                'read the first line HTTP command
                req = New ProxyRequest
                req.m_Server = Me
                req.m_Client = client
                req.ParseRequest()

                'NewRequest
                Dim e As New ProxyRequestEventArgs(req)
                Me.OnNewRequest(e)

                If (e.Cancel) Then
                    req.ServeFromFile(System.IO.Path.Combine(Sistema.ApplicationContext.StartupFloder, "blockedcontent.html"))
                    Return
                End If

                Dim webReq As HttpWebRequest = req.Resolve()
                If req.CacheEntry Is Nothing Then Me.OnCacheMiss(New ProxyCacheMissEventArgs(req))

                If (req.CacheEntry Is Nothing) Then
                    If (req.isTunneling) Then
                        req.StartTunneling()
                    Else
                        If webReq IsNot Nothing Then req.DownloadRemoteResource(webReq)
                    End If
                Else
                    If webReq IsNot Nothing Then req.ServeFromCache(webReq)
                End If

            Catch ex As Exception
                Me.OnProxyLog(New HTTPProxyLogEventArgs(ex.Message)) ' & vbNewLine & ex.StackTrace))
            Finally

                'If (Server.DumpHeaders OrElse Server.DumpPostData OrElse Server.DumpResponseData) Then
                '    'release the lock
                '    Monitor.Exit(_outputLockObj)
                'End If
                If (req IsNot Nothing AndAlso Not req.isTunneling) Then req.Dispose()

            End Try

        End Sub

        Private Sub OnCacheMiss(ByVal e As ProxyCacheMissEventArgs)
            RaiseEvent CacheMiss(Nothing, e)
        End Sub

        Private Sub OnNewRequest(ByVal e As ProxyRequestEventArgs)
            RaiseEvent NewRequest(Nothing, e)
        End Sub

        Private Sub DumpHeaderCollectionToConsole(ByVal headers As WebHeaderCollection)
            For Each s As String In headers.AllKeys
                OnProxyLog(New HTTPProxyLogEventArgs(String.Format("{0}: {1}", s, headers(s))))
            Next
            OnProxyLog(New HTTPProxyLogEventArgs())
        End Sub

        Private Sub DumpHeaderCollectionToConsole(ByVal headers As List(Of HttpHeader))
            For Each header As HttpHeader In headers
                OnProxyLog(New HTTPProxyLogEventArgs(String.Format("{0}: {1}", header.Key, header.Value)))
            Next
            OnProxyLog(New HTTPProxyLogEventArgs())
        End Sub



        Protected Friend Sub OnProxyLog(ByVal e As HTTPProxyLogEventArgs)
            RaiseEvent ProxyLog(Nothing, e)
        End Sub

        Public Property TimeOut() As Integer
            Get
                Return Me.m_TimeOut
            End Get
            Set(value As Integer)
                Me.m_TimeOut = value
            End Set
        End Property

    End Class


End Namespace

