Imports DMD
Imports DMD.Sistema

Public NotInheritable Class Dialers
    'Private Shared m_InstalledDialers As Collection


   
    Public Shared Function GetInstalledDialers() As CCollection(Of DialerBaseClass)
        Dim m_InstalledDialers As New CCollection(Of DialerBaseClass)
        Dim types As System.Type() = GetType(Dialers).Assembly.GetTypes
        Dim d As DialerBaseClass
        For Each t As System.Type In types
            If (t.IsSubclassOf(GetType(DialerBaseClass))) Then
                d = DirectCast(Activator.CreateInstance(t), DialerBaseClass)
                If (d.IsInstalled) Then m_InstalledDialers.Add(d)
            End If
        Next
        For Each disp As DispositivoEsterno In DialTPApp.CurrentConfig.Dispositivi
            Select Case disp.Tipo
                Case "Cisco 7940", "Cisco 7960"
                    d = New CiscoDialer(disp.Indirizzo, disp.Nome)
                    m_InstalledDialers.Add(d)
            End Select
        Next
        For Each server As AsteriskServer In DialTPApp.CurrentConfig.AsteriskServers
            Try
                If (Not server.IsConnected) Then server.Connect()
                d = New AsteriskDialer(server)
                m_InstalledDialers.Add(d)
            Catch ex As Exception

            End Try
        Next
        Return m_InstalledDialers
    End Function

End Class
