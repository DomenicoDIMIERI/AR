Imports System.IO
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports DMD.Office

Public NotInheritable Class InterfonoService
    Private Shared ReadOnly synchRoot As New Object
    Private Shared _listener As TcpListener
    Private Shared _listenerThread As Thread
    Private Shared m_Quit As Boolean
    Private Shared m_Interfoni As CCollection(Of Interfono) = Nothing

    Public Shared Sub StartServer(ByVal bindAddress As String, ByVal bindPort As Integer)
        SyncLock synchRoot
            m_Quit = False
            If (_listener IsNot Nothing) Then Throw New InvalidOperationException("Già in ascolto")

            'If (_certificate Is Nothing) Then
            '    Me.OnProxyLog(New HTTPProxyLogEventArgs("Certificato SSL non valido!"))
            'End If


            _listener = New TcpListener(IPAddress.Parse(bindAddress), bindPort)
            _listener.Start()

            _listenerThread = New Thread(New ParameterizedThreadStart(AddressOf Listen))
            _listenerThread.Priority = ThreadPriority.BelowNormal


            ' Me._cacheMaintenanceThread = New Thread(New ThreadStart(AddressOf Me.ProxyCache.CacheMaintenance))
            ' Me._cacheMaintenanceThread.Priority = ThreadPriority.AboveNormal

            _listenerThread.Start(_listener)
            '_cacheMaintenanceThread.Start()

        End SyncLock
    End Sub

    Public Shared Sub StopService()
        SyncLock synchRoot
            m_Quit = True
            _listener.Stop()

            _listenerThread.Join(2000)
            _listenerThread.Abort()
            _listener = Nothing
            _listenerThread = Nothing

            If (m_Interfoni IsNot Nothing) Then
                Dim col As New CCollection(Of Interfono)(m_Interfoni)
                For Each i As Interfono In m_Interfoni
                    Try
                        If (i.IsConnected) Then i.Disconnect()
                    Catch ex As Exception
                        DIALTPLib.Log.LogException(ex)
                    End Try
                Next
                m_Interfoni = Nothing
            End If

        End SyncLock
    End Sub

    Public Shared ReadOnly Property Interfoni As CCollection(Of Interfono)
        Get
            If (m_Interfoni Is Nothing) Then m_Interfoni = GetInterfoni()
            Return New CCollection(Of Interfono)(m_Interfoni)
        End Get
    End Property

    Public Shared Sub Invalidate()
        If (m_Interfoni Is Nothing) Then Return

        Dim oldItems As CCollection(Of Interfono) = Interfoni
        m_Interfoni = GetInterfoni()
        For Each i As Interfono In m_Interfoni
            For Each i1 As Interfono In oldItems
                If i.UniqueID = i1.UniqueID Then
                    i.Con = i1.Con
                    Exit For
                End If
            Next
        Next

    End Sub

    Private Shared Function GetInterfoni() As CCollection(Of Interfono)
        Dim ret As New CCollection(Of Interfono)
        Dim str As String = RPC.InvokeMethod(Remote.getServerName() & Remote.__DMDENTRYSVC & "/websvcf/dialtp.aspx?_a=GetDevices", "po", 0)
        Dim arr As New System.Text.StringBuilder
        Dim tmp As CCollection
        Dim dev As Dispositivo
        Dim interfono As Interfono

        If (str <> "") Then
            tmp = CType(XML.Utils.Serializer.Deserialize(str), CCollection)

            For Each dev In tmp
                If (arr.Length > 0) Then arr.Append(",")
                arr.Append(GetID(dev))
                interfono = New Interfono
                interfono.Dev = dev
                'interfono.Address = dev.
                ret.Add(interfono)
            Next
        End If

        If (arr.Length > 0) Then
            str = RPC.InvokeMethod(Remote.getServerName() & Remote.__DMDENTRYSVC & "/websvcf/dialtp.aspx?_a=GetDevicesLastLog", "ids", RPC.str2n(arr.ToString))
            tmp = CType(XML.Utils.Serializer.Deserialize(str), CCollection)
            For Each interfono In ret
                For Each log As DeviceLog In tmp
                    If log.IDDevice = GetID(interfono.Dev) Then
                        interfono.Log = log
                        interfono.UserName = log.NomeUtente
                        interfono.Address = log.IPAddress
                    End If
                Next
            Next
        End If

        Return ret
    End Function

    Private Shared Sub Listen(ByVal obj As Object)
        Dim listener As TcpListener = CType(obj, TcpListener)
        While (Not m_Quit)
            Try

                Dim client As TcpClient = listener.AcceptTcpClient()
                Dim ret As Boolean
                Dim wc As New WaitCallback(AddressOf ProcessClient)
                client.NoDelay = True
                Do
                    ret = ThreadPool.QueueUserWorkItem(wc, client)
                    System.Threading.Thread.Sleep(100)
                Loop While (Not ret) AndAlso (Not m_Quit)

            Catch e As ThreadAbortException

            Catch e As SocketException

            Catch e As Exception
                Throw
            End Try


        End While
    End Sub

    'Private Shared m_Connessioni As New CCollection(Of InterfonoConnection)

    Private Shared Sub ProcessClient(ByVal obj As Object)
        Dim client As TcpClient = CType(obj, TcpClient)
        Try
            Dim con As New InterfonoConnection(client)
            If con.BeginHandshake() Then
                ' m_Connessioni.Add(con)
                con.m_StreamThread = System.Threading.Thread.CurrentThread
                Try
                    con.Listener()
                Catch ex As Exception
                    DIALTPLib.Log.LogException(ex)
                End Try
                'm_Connessioni.Remove(con)
            End If
            client.Close()
        Catch ex As Exception
            DIALTPLib.Log.LogException(ex)
        End Try
    End Sub


End Class
