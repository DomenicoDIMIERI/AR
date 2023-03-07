Imports System.IO
Imports System.Xml.Serialization
Imports DMD
Imports DMD.Sistema
Imports DMD.CallManagers

Public NotInheritable Class AsteriskServers



    Public Shared Event Connected(ByVal sender As Object, ByVal e As AsteriskEventArgs)
    Public Shared Event Disconnected(ByVal sender As Object, ByVal e As AsteriskEventArgs)
    Public Shared Event ManagerEvent(ByVal sender As Object, ByVal e As AsteriskEvent)

    Private Shared m_Servers As New CCollection(Of AsteriskServer)

    Private Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Shared Sub New()
    End Sub


    Public Shared Sub StartListening(ByVal servers As CCollection(Of AsteriskServer))
        Dim unchanged As New CCollection(Of AsteriskServer)

        'Rimuoviamo i servers presenti in entrambi le configurazioni
        Dim i As Integer = 0
        While (i < m_Servers.Count)
            Dim curr As AsteriskServer = m_Servers(i)
            Dim found As Boolean = False
            Dim j As Integer = 0
            While (j < servers.Count)
                Dim other As AsteriskServer = servers(j)
                If (curr.Equals(other)) Then
                    unchanged.Add(curr)
                    servers.RemoveAt(j)
                    other.SetManager(curr.GetManager)
                    found = True
                    Exit While
                Else
                    j += 1
                End If
            End While
            If (found) Then
                m_Servers.RemoveAt(i)
            Else
                i += 1
            End If
        End While

        'Disconnettiamo le configurazioni non valide
        For Each server As AsteriskServer In m_Servers
            server.Disconnect()
            RemoveHandler server.Connected, AddressOf handleAsteriskConnect
            RemoveHandler server.Disconnected, AddressOf handleAsteriskDisconnect
            RemoveHandler server.ManagerEvent, AddressOf handleAsteriskEvent
        Next

        'Connettiamo le nuove configurazioni
        For Each server As AsteriskServer In servers
            AddHandler server.Connected, AddressOf handleAsteriskConnect
            AddHandler server.Disconnected, AddressOf handleAsteriskDisconnect
            AddHandler server.ManagerEvent, AddressOf handleAsteriskEvent
            server.Connect()
        Next

        m_Servers = New CCollection(Of AsteriskServer)
        m_Servers.AddRange(unchanged)
        m_Servers.AddRange(servers)
    End Sub

    Public Shared Sub StopListening()
        For Each server As AsteriskServer In m_Servers
            server.Disconnect()
            RemoveHandler server.Connected, AddressOf handleAsteriskConnect
            RemoveHandler server.Disconnected, AddressOf handleAsteriskDisconnect
            RemoveHandler server.ManagerEvent, AddressOf handleAsteriskEvent
        Next
        m_Servers.Clear()
    End Sub


    Private Shared Sub handleAsteriskEvent(sender As Object, e As AsteriskEvent)
        RaiseEvent ManagerEvent(sender, e)
    End Sub

    Private Shared Sub handleAsteriskDisconnect(sender As Object, e As AsteriskEventArgs)
        RaiseEvent Disconnected(sender, e)
    End Sub

    Private Shared Sub handleAsteriskConnect(sender As Object, e As AsteriskEventArgs)
        RaiseEvent Connected(sender, e)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class
