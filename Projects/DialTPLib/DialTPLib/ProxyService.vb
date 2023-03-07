Imports System.ServiceProcess
Imports DIALTPLib.Log
Imports System.ComponentModel
Imports DMD.Net.HTTPProxy
Imports System.Security.Cryptography.X509Certificates
Imports System.Net

Public Class ProxyService
    Private Sub New()
    End Sub

    Public Shared Event NewRequest(ByVal sender As Object, ByVal e As DMD.Net.HTTPProxy.ProxyRequestEventArgs)
    Public Shared Event ProxyLog(ByVal sender As Object, ByVal e As DMD.Net.HTTPProxy.HTTPProxyLogEventArgs)
    Public Shared Event CacheMiss(ByVal sender As Object, ByVal e As DMD.Net.HTTPProxy.ProxyCacheMissEventArgs)

    Private Shared m_Server As HTTPProxyServer = Nothing
    Private Shared m_Profile As ProxyProfile = Nothing
    Private Shared m_ProxyIP As String = vbNullString
    Private Shared m_ProxyPort As Integer = 8080
    Private Shared m_ProxyTimeout As Integer = 30000
    Private Shared m_Certificate As X509Certificate2 = Nothing

    Public Shared Property Certificate As X509Certificate2
        Get
            Return m_Certificate
        End Get
        Set(value As X509Certificate2)
            If (m_Certificate Is value) Then Return
            If IsRunning() Then Throw New InvalidOperationException
            m_Certificate = value
        End Set
    End Property

    Public Shared Property Profile As ProxyProfile
        Get
            Return m_Profile
        End Get
        Set(value As ProxyProfile)
            m_Profile = value
        End Set
    End Property


    Public Shared Property ProxyIP As String
        Get
            Return m_ProxyIP
        End Get
        Set(value As String)
            Dim oldValue As String = m_ProxyIP
            If (oldValue = value) Then Return
            If IsRunning() Then Throw New InvalidOperationException
            m_ProxyIP = value
        End Set
    End Property

    Public Shared Property ProxyPort As Integer
        Get
            Return m_ProxyPort
        End Get
        Set(value As Integer)
            Dim oldValue As Integer = m_ProxyPort
            If (oldValue = value) Then Return
            If IsRunning() Then Throw New InvalidOperationException
            m_ProxyPort = value
        End Set
    End Property

    Public Property ProxyTimeout As Integer
        Get
            Return m_ProxyTimeout
        End Get
        Set(value As Integer)
            Dim oldValue As Integer = m_ProxyTimeout
            If (oldValue = value) Then Return
            If IsRunning() Then Throw New InvalidOperationException
            m_ProxyTimeout = value
        End Set
    End Property

    ''' <summary>
    ''' Avvia il servizio
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub Start()
        If IsRunning() Then Throw New InvalidOperationException

        m_Server = New HTTPProxyServer
        AddHandler m_Server.CacheMiss, AddressOf serverCacheMiss
        AddHandler m_Server.NewRequest, AddressOf handleNewProxyRequest
        AddHandler m_Server.ProxyLog, AddressOf handleProxyLog

        ' m_Server.CertificateFilePath = My.Application.Info.DirectoryPath & "\DMDsrl.crt" ' DIALTPLib.Settings.PROXYCERT
        ' If (ProxyIP <> "" AndAlso ProxyPort > 0) Then m_Server.Proxy = New WebProxy(ProxyIP, ProxyPort)
        m_Server.ListeningIPInterface = Net.IPAddress.Loopback
        m_Server.ListeningPort = 8080
        m_Server.SSLCertificate = m_Certificate
        m_Server.TimeOut = m_ProxyTimeout
        m_Server.Start()
    End Sub

    Private Shared Sub handleNewProxyRequest(ByVal sender As Object, ByVal e As ProxyRequestEventArgs)
        DIALTPLib.Log.LogProxy("Proxy Request: " & e.Request.RemoteUri)
        Dim p As ProxyProfile = m_Profile
        If (p IsNot Nothing) Then
            If (p.IsURLAllowed(e.Request.RemoteUri)) Then

            Else
                e.Cancel = True
            End If
        End If
        RaiseEvent NewRequest(sender, e)
    End Sub

    Private Shared Sub handleProxyLog(ByVal sender As Object, ByVal e As HTTPProxyLogEventArgs)
        ' DIALTPLib.Log.LogProxy("Proxy Log: " & e.Message)
        RaiseEvent ProxyLog(sender, e)
    End Sub


    Private Shared Sub serverCacheMiss(ByVal sender As Object, ByVal e As DMD.Net.HTTPProxy.ProxyCacheMissEventArgs)
        'DIALTPLib.Log.LogProxy("Proxy Cache Miss: " & e.Request.RemoteUri)
        RaiseEvent CacheMiss(sender, e)
    End Sub

    ''' <summary>
    ''' Ferma il servizio
    ''' </summary>
    ''' <remarks></remarks>
    Shared Sub [Stop]()
        If Not IsRunning() Then Throw New InvalidOperationException

        RemoveHandler m_Server.CacheMiss, AddressOf serverCacheMiss
        RemoveHandler m_Server.NewRequest, AddressOf handleNewProxyRequest
        RemoveHandler m_Server.ProxyLog, AddressOf handleProxyLog

        m_Server.Stop()
        m_Server = Nothing
    End Sub

    ''' <summary>
    ''' Riavvia il servizio
    ''' </summary>
    ''' <remarks></remarks>
    Shared Sub Restart()
        If IsRunning() Then [Stop]()
        Start()
    End Sub

    ''' <summary>
    ''' Restituisce vero se il servizio è stato avviato
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function IsRunning() As Boolean
        Return m_Server IsNot Nothing
    End Function



    Shared Function GetIP(ByVal addr As String) As System.Net.IPAddress
        Dim hostName = System.Net.Dns.GetHostName()
        Dim ip As System.Net.IPAddress = Nothing
        For Each hostAdr As System.Net.IPAddress In System.Net.Dns.GetHostEntry(hostName).AddressList()
            If Not (hostAdr.Equals(System.Net.IPAddress.Loopback)) AndAlso _
               Not (hostAdr.Equals(System.Net.IPAddress.Any)) AndAlso _
               Not (hostAdr.Equals(System.Net.IPAddress.Broadcast)) AndAlso _
               Not (hostAdr.Equals(System.Net.IPAddress.IPv6Any)) AndAlso _
               Not (hostAdr.IsIPv6LinkLocal) Then
                ip = hostAdr
                Exit For
            End If
        Next
        Return ip
    End Function

    Shared Function TestIP4(ByVal mask As Byte()) As Boolean
        Return TestIP4(New System.Net.IPAddress(mask))
    End Function

    ''' <summary>
    ''' Restitusice vero se il PC ha un indirizzo di rete che rispetta la maschera passata
    ''' </summary>
    ''' <param name="mask"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function TestIP4(ByVal mask As System.Net.IPAddress) As Boolean
        Dim b0 As Byte() = mask.GetAddressBytes
        Dim strHostName As String = System.Net.Dns.GetHostName()
        Dim t As Boolean = False
        For Each hostAdr As System.Net.IPAddress In System.Net.Dns.GetHostEntry(strHostName).AddressList()
            Dim b1 As Byte() = hostAdr.GetAddressBytes
            t = True
            For i As Integer = b0.Length - 1 To 0 Step -1
                If ((b0(i) <> 0) AndAlso (b1(i) <> b0(i))) Then
                    t = False
                    Exit For
                End If
            Next
            If (t) Then Exit For
        Next
        Return t
    End Function
End Class
