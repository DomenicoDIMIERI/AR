Imports DMD.S300.CKT_DLL


Namespace S300

    Public NotInheritable Class S300Devices


        ''' <summary>
        ''' Evento generato quando il dispositivo si connette correttamente al PC
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Shared Event DeviceConnected(ByVal sender As Object, ByVal e As S300EventArgs)

        ''' <summary>
        ''' Evento generato quando il dispositivo si disconnette dal PC
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Shared Event DeviceDisconnected(ByVal sender As Object, ByVal e As S300EventArgs)

        ''' <summary>
        ''' Evento generato quando viene modificata la configurazione di rete del dispositivo
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Shared Event DeviceNetworkConfigurationChanged(ByVal sender As Object, ByVal e As S300NetConfigEventArgs)


        Private Shared m_Devices As New System.Collections.ArrayList

        Private Sub New()

        End Sub

        ''' <summary>
        ''' Inizializza la libreria
        ''' </summary>
        Public Shared Sub Initialize()

        End Sub

        ''' <summary>
        ''' Finalizza la libreria (nessun metodo di questa libreria deve essere chiamato dopo Terminate)
        ''' </summary>
        Public Shared Sub Terminate()
            CKT_DLL.CKT_Disconnect()
        End Sub

        ''' <summary>
        ''' Restituisce un array contenente l'ID di tutti i dispositivi connessi
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetConnectedDevicesIDs() As Integer()
            Dim SnoPtr As IntPtr = IntPtr.Zero
            Dim count As Integer = CKT_DLL.CKT_ReportConnections(SnoPtr)
            Dim ret As Integer() = {}
            If (count > 0) Then
                ReDim ret(count - 1)
                For i = 0 To count - 1 '(i = 1; i <= count; i++)
                    Dim Sno As Integer = 0
                    PCopyMemory(Sno, New IntPtr(SnoPtr.ToInt32 + i * 4), 4)
                    ret(i) = Sno
                Next

                CKT_DLL.CKT_FreeMemory(SnoPtr)
            End If

            Return ret
        End Function

        ''' <summary>
        ''' Metodo richiamato quando un dispositivo si connette
        ''' </summary>
        ''' <param name="e"></param>
        Friend Shared Sub NotifyConnected(ByVal e As S300EventArgs)
            RaiseEvent DeviceDisconnected(e.Device, e)
        End Sub

        ''' <summary>
        ''' Metodo richiamato quando un dispositivo si disconnette
        ''' </summary>
        ''' <param name="e"></param>
        Friend Shared Sub NotifyDisconnected(ByVal e As S300EventArgs)
            RaiseEvent DeviceDisconnected(e.Device, e)
        End Sub

        ''' <summary>
        ''' Metodo richiamato quando viene modificata la configurazione di rete di un dispositivo
        ''' </summary>
        ''' <param name="e"></param>
        Friend Shared Sub NotifyNetworkConfigurationChanged(ByVal e As S300NetConfigEventArgs)
            RaiseEvent DeviceNetworkConfigurationChanged(e.Device, e)
        End Sub

        ''' <summary>
        ''' Registra la periferica nel sistema
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="address"></param>
        ''' <returns></returns>
        Public Shared Function RegisterDevice(ByVal id As Integer, ByVal address As String) As S300Device
            If (id = 0) Then Return Nothing
            address = Trim(address)
            If (address = "") Then Return Nothing

            Dim dev As S300Device = Nothing
            For Each dev In m_Devices
                If dev.DeviceID = id AndAlso String.Compare(dev.Address, address, True) = 0 Then
                    Return dev
                End If
            Next
            dev = New S300Device(id, address)
            m_Devices.Add(dev)
            Return dev
        End Function

        ''' <summary>
        ''' Elimina la registrazione della periferica nel sistema
        ''' </summary>
        ''' <param name="device"></param>
        Public Shared Sub UnregisterDevice(ByVal device As S300Device)
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            If (device.IsConnected) Then Throw New InvalidOperationException("La periferica deve essere disconnessa prima di chiamare il metodo UnregisterDevice")
            m_Devices.Remove(device)
        End Sub


    End Class


End Namespace
