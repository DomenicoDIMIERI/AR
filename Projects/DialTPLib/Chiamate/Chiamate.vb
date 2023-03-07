Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.XML
Imports DIALTPLib
Imports DMD.CallManagers
Imports DMD.CallManagers.Events

Public NotInheritable Class Chiamate
    Private Shared lock As New Object
    Private Shared m_Attive As New CCollection(Of Chiamata)
    Private Shared m_DB As CDBConnection

    Private Sub New()
    End Sub

    Public Shared Property Database As CDBConnection
        Get
            If (m_DB Is Nothing) Then Return APPConn
            Return m_DB
        End Get
        Set(value As CDBConnection)
            m_DB = value
        End Set
    End Property

    Public Shared Function NewOutCall(server As AsteriskServer, e As DialEvent) As Chiamata
        Dim c As New Chiamata
        c.ServerIP = server.ServerName & ":" & server.ServerPort
        c.ServerName = server.ServerName
        c.SourceNumber = e.CallerIDNumber
        c.TargetNumber = e.ConnectedLineNum
        c.Direzione = 1
        c.Channel = e.Channel
        c.StatoChiamata = StatoChiamata.Dialling
        c.StartTime = e.Data
        c.Parameters.SetItemByKey("Destination", e.Destination)
        c.Parameters.SetItemByKey("DialString", e.DialString)
        c.Parameters.SetItemByKey("Uniqueid", e.UniqueID)

        c.Save()
        SyncLock lock
            m_Attive.Add(c)
        End SyncLock
        Return c
    End Function

    Public Shared Function NewInCall(server As AsteriskServer, e As DialEvent) As Chiamata
        Dim c As New Chiamata
        c.ServerIP = server.ServerName & ":" & server.ServerPort
        c.ServerName = server.ServerName
        c.TargetNumber = server.Channel
        c.SourceNumber = e.CallerIDNumber
        c.Channel = e.Channel
        c.Direzione = 0
        c.StatoChiamata = StatoChiamata.Ringing
        c.StartTime = e.Data
        c.Parameters.SetItemByKey("Destination", e.Destination)
        c.Parameters.SetItemByKey("DialString", e.DialString)
        c.Parameters.SetItemByKey("Uniqueid", e.UniqueID)
        c.Save()
        SyncLock lock
            m_Attive.Add(c)
        End SyncLock
        Return c
    End Function

    Public Shared Sub NotifyCallEnded(e As HangupEvent)
        Dim c As Chiamata = Nothing
        SyncLock lock
            For Each c1 In m_Attive
                If c1.Parameters.GetItemByKey("Destination") = e.Channel Then
                    c = c1
                    Exit For
                End If
            Next
            If (c IsNot Nothing) Then
                m_Attive.Remove(c)
            End If
        End SyncLock

        If (c IsNot Nothing) Then
            If (c.StatoChiamata = StatoChiamata.Speaking) Then
                c.StatoChiamata = StatoChiamata.Answered
            Else
                c.StatoChiamata = StatoChiamata.NotAnswered
            End If

            c.EndTime = e.Data
            c.Save()
        End If

    End Sub

    Public Shared Sub NotifyChannelState(e As AsteriskEvent)
        Dim channel As String = e.GetAttribute("Channel")
        If (channel = "") Then Return

        SyncLock lock
            For Each c1 In m_Attive
                Dim destination As String = CStr(c1.Parameters.GetItemByKey("Destination"))
                Dim state As String = e.GetAttribute("CHANNELSTATE")
                Dim stateEx As String = e.GetAttribute("CHANNELSTATEDESC")
                If destination = channel Then
                    Select Case state
                        Case "6" 'Risposto
                            c1.AnswerTime = e.Data
                            c1.StatoChiamata = StatoChiamata.Speaking
                            c1.Save()
                        Case "5" 'Ringing
                            c1.StatoChiamata = StatoChiamata.Ringing
                            c1.Save()
                        Case Else
                            Debug.Print(state & " -> " & stateEx)
                    End Select
                    Exit For
                End If
            Next
        End SyncLock


    End Sub
End Class
