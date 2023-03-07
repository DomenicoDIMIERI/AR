Imports DMD.S300.CKT_DLL
Imports DMD.Internals
Imports System.Runtime.InteropServices


Namespace S300

    Public Enum ClearClockingRecordTypes As Integer
        All = 0
        [New] = 1
        Count = 2
    End Enum

    Public Class S300Device

        ''' <summary>
        ''' Evento generato quando il dispositivo si connette correttamente al PC
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Event Connected(ByVal sender As Object, ByVal e As S300EventArgs)

        ''' <summary>
        ''' Evento generato quando il dispositivo si disconnette dal PC
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Event Disconnected(ByVal sender As Object, ByVal e As S300EventArgs)

        ''' <summary>
        ''' Evento generato quando viene modificata la configurazione di rete del dispositivo
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Event NetworkConfigurationChanged(ByVal sender As Object, ByVal e As S300NetConfigEventArgs)

        Private m_IDNumber As Integer
        Private m_Address As String
        Private m_Connected As Boolean
        Private m_Users As S300UsersCollection

        Public Sub New()
            Me.m_IDNumber = 0
            Me.m_Address = ""
            Me.m_Connected = False
            Me.m_Users = Nothing
        End Sub

        Friend Sub New(ByVal deviceID As Integer, ByVal address As String)
            Me.New
            address = Trim(address)
            If (deviceID = 0) Then Throw New ArgumentException("deviceID non valido")
            If (address = "") Then Throw New ArgumentNullException("address non può essere NULL")
            Me.m_IDNumber = deviceID
            Me.m_Address = address
        End Sub

        ''' <summary>
        ''' Inizializza il collegamento ad un dispositivo collegato su una porta seriale
        ''' </summary>
        Protected Sub StartSerialCommunication()
            Dim comPort As Integer = CInt(Mid(Me.m_Address, 4))
            Dim ret As Integer = CKT_DLL.CKT_RegisterSno(Me.m_IDNumber, comPort) 'If from com
            Me.m_Connected = (ret = 1)
            If Not Me.m_Connected Then Throw New System.Exception("Errore: " & System.Runtime.InteropServices.Marshal.GetLastWin32Error.ToString)
        End Sub

        ''' <summary>
        ''' Inizializza il collegamento ad un dispositivo collegato in rete
        ''' </summary>
        Protected Sub StartNetworkCommunication()
            Dim ret As Integer = CKT_DLL.CKT_RegisterNet(Me.m_IDNumber, Me.m_Address) 'If from net
            Me.m_Connected = (ret = 1)
            If Not Me.m_Connected Then Throw New System.Exception("Errore: " & System.Runtime.InteropServices.Marshal.GetLastWin32Error.ToString)
        End Sub

        ''' <summary>
        ''' Inizializza il collegamento ad un dispositivo
        ''' </summary>
        Public Overridable Sub Start()
            If (Me.IsConnected) Then Throw New InvalidOperationException("Dispositivo già connesso")
            If UCase(Me.m_Address).StartsWith("COM") Then
                Me.StartSerialCommunication()
            Else
                Me.StartNetworkCommunication()
            End If
            Dim e As New S300EventArgs(Me)
            S300Devices.NotifyConnected(e)
            RaiseEvent Connected(Me, e)
        End Sub

        ''' <summary>
        ''' Interrompe il collegamento con il dispositivo remoto
        ''' </summary>
        Public Overridable Sub [Stop]()
            If (Not Me.IsConnected) Then Throw New InvalidOperationException("Dispositivo non connesso")
            CKT_DLL.CKT_UnregisterSnoNet(Me.m_IDNumber)
            Me.m_Connected = False
            Dim e As New S300EventArgs(Me)
            S300Devices.NotifyDisconnected(e)
            RaiseEvent Disconnected(Me, e)
        End Sub

        ''' <summary>
        ''' Restituisce l'ID della periferica
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property DeviceID As Integer
            Get
                Return Me.m_IDNumber
            End Get
        End Property

        ''' <summary>
        ''' Restituisce l'indirizzo della periferica
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Address As String
            Get
                Return Me.m_Address
            End Get
        End Property

        ''' <summary>
        ''' Restituisce vero se il dispositivo è correttamente connesso al pc
        ''' </summary>
        ''' <returns></returns>
        Public Function IsConnected() As Boolean
            Return Me.m_Connected
        End Function

        ''' <summary>
        ''' Restituisce la configurazione di rete del dispositivo
        ''' </summary>
        ''' <returns></returns>
        Public Function GetNetworkConfiguration() As NETINFO
            If Not Me.IsConnected Then Throw New InvalidOperationException("Dispositivo non connesso")
            Dim devnetinfo As CKT_DLL.NETINFO = New CKT_DLL.NETINFO()
            Dim ret As Integer = CKT_DLL.CKT_GetDeviceNetInfo(Me.m_IDNumber, devnetinfo)
            If (ret = 0) Then Throw New Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error)
            'msg = "IP: " & devnetinfo.IP(0) & "." & devnetinfo.IP(1) & "." & devnetinfo.IP(2) & "." & devnetinfo.IP(3) & vbLf
            '    msg = msg & "Mask: " & devnetinfo.Mask(0) & "." & devnetinfo.Mask(1) & "." & devnetinfo.Mask(2) & "." & devnetinfo.Mask(3) & vbLf
            '    msg = msg & "Gate: " & devnetinfo.Gateway(0) & "." & devnetinfo.Gateway(1) & "." & devnetinfo.Gateway(2) & "." & devnetinfo.Gateway(3) & vbLf
            '    msg = msg & "Server: " & devnetinfo.ServerIP(0) & "." & devnetinfo.ServerIP(1) & "." & devnetinfo.ServerIP(2) & "." & devnetinfo.ServerIP(3) & vbLf
            '    msg = msg & "MAC: " & devnetinfo.MAC(0) & "." & devnetinfo.MAC(1) & "." & devnetinfo.MAC(2) & "." & devnetinfo.MAC(3) & "." & devnetinfo.MAC(4) & "." & devnetinfo.MAC(5) & vbLf
            '    MessageBox.Show(msg)
            'End If
            Return devnetinfo
        End Function

        ''' <summary>
        ''' Imposta i parametri di rete del dispositivo
        ''' </summary>
        ''' <param name="config"></param>
        Public Sub SetNetworkConfiguration(ByVal config As NETINFO)
            If Not Me.IsConnected Then Throw New InvalidOperationException("Dispositivo non connesso")

            Dim ret As Integer

            ret = CKT_DLL.CKT_SetDeviceIPAddr(Me.m_IDNumber, config.IP)
            If (ret = 0) Then Throw New Exception("Errore in CKT_SetDeviceIPAddr: " & Marshal.GetLastWin32Error.ToString)

            ret = CKT_DLL.CKT_SetDeviceMask(Me.m_IDNumber, config.Mask)
            If (ret = 0) Then Throw New Exception("Errore in CKT_SetDeviceMask: " & Marshal.GetLastWin32Error.ToString)

            ret = CKT_DLL.CKT_SetDeviceGateway(Me.m_IDNumber, config.Gateway)
            If (ret = 0) Then Throw New Exception("Errore in CKT_SetDeviceGateway: " & Marshal.GetLastWin32Error.ToString)

            ret = CKT_DLL.CKT_SetDeviceServerIPAddr(Me.m_IDNumber, config.ServerIP)
            If (ret = 0) Then Throw New Exception("Errore in CKT_SetDeviceServerIPAddr: " & Marshal.GetLastWin32Error.ToString)

            ret = CKT_DLL.CKT_SetDeviceMAC(Me.m_IDNumber, config.MAC)
            If (ret = 0) Then Throw New Exception("Errore in CKT_SetDeviceMAC: " & Marshal.GetLastWin32Error.ToString)

            config = Me.GetNetworkConfiguration

            Me.OnNetworkConfigurationChanged(New S300NetConfigEventArgs(Me, config))
        End Sub

        Protected Overridable Sub OnNetworkConfigurationChanged(ByVal e As S300NetConfigEventArgs)
            S300Devices.NotifyNetworkConfigurationChanged(e)
            RaiseEvent NetworkConfigurationChanged(Me, e)
        End Sub

        ''' <summary>
        ''' Restituisce la data e l'ora di sistema sul dispositivo remoto
        ''' </summary>
        ''' <returns></returns>
        Public Function GetDeviceTime() As Date
            If Not Me.IsConnected Then Throw New InvalidOperationException("Dispositivo non connesso")

            Dim devclock As CKT_DLL.DATETIMEINFO = New CKT_DLL.DATETIMEINFO()
            Dim ret As Integer = CKT_DLL.CKT_GetDeviceClock(Me.m_IDNumber, devclock)
            If (ret = 0) Then Throw New Exception("Errore in CKT_GetDeviceClock: " & Marshal.GetLastWin32Error)

            Return New Date(devclock.Year_Renamed, devclock.Month_Renamed, devclock.Day_Renamed, devclock.Hour_Renamed, devclock.Minute_Renamed, devclock.Second_Renamed)
        End Function

        ''' <summary>
        ''' Imposta la data e l'ora di sistema sul dispositivo remoto
        ''' </summary>
        ''' <param name="[date]"></param>
        Public Sub SetDeviceTime(ByVal [date] As Date)
            If Not Me.IsConnected Then Throw New InvalidOperationException("Dispositivo non connesso")

            Dim ret As Integer = CKT_DLL.CKT_SetDeviceDate(Me.m_IDNumber, [date].Year, [date].Month, [date].Day)
            If (ret = 0) Then Throw New Exception("Errore in CKT_SetDeviceDate: " & Marshal.GetLastWin32Error)

            Sleep((300))
            ret = CKT_DLL.CKT_SetDeviceTime(Me.m_IDNumber, [date].Hour, [date].Minute, [date].Second)
            If (ret = 0) Then Throw New Exception("Errore in CKT_SetDeviceDate: " & Marshal.GetLastWin32Error)

        End Sub

        ''' <summary>
        ''' Restituisce la versione del software installato sul dispositivo remoto
        ''' </summary>
        ''' <returns></returns>
        Public Function GetDeviceSoftwareVersion() As Version
            If Not Me.IsConnected Then Throw New InvalidOperationException("Dispositivo non connesso")
            Dim devnfo As CKT_DLL.DEVICEINFO = New CKT_DLL.DEVICEINFO()
            Dim ret As Integer = CKT_DLL.CKT_GetDeviceInfo(Me.m_IDNumber, devnfo)
            If (ret = 0) Then Throw New Exception("Errore in CKT_GetDeviceInfo: " & Marshal.GetLastWin32Error)

            Return New Version(devnfo.MajorVersion, devnfo.MinorVersion)
        End Function

        ''' <summary>
        ''' Restituisce la versione del software installato sul dispositivo remoto
        ''' </summary>
        ''' <returns></returns>
        Public Function GetDeviceID() As Integer
            Me.EnsureConnected()
            Dim devnfo As CKT_DLL.DEVICEINFO = New CKT_DLL.DEVICEINFO()
            Dim ret As Integer = CKT_DLL.CKT_GetDeviceInfo(Me.m_IDNumber, devnfo)
            If (ret = 0) Then Throw New Exception("Errore in CKT_GetDeviceInfo: " & Marshal.GetLastWin32Error)

            Return devnfo.ID
        End Function

        Friend Sub EnsureConnected()
            If Not Me.IsConnected Then Throw New InvalidOperationException("Dispositivo non connesso")
        End Sub

        ''' <summary>
        ''' Restituisce i contatori relative alle strutture dati definite sul dispositivo
        ''' </summary>
        ''' <returns></returns>
        Public Function GetCounts() As S300CountsInfo
            Me.EnsureConnected()
            Dim pc, fc, cc As Integer
            Dim ret As Integer = CKT_DLL.CKT_GetCounts(Me.m_IDNumber, pc, fc, cc)
            If (ret = 0) Then Throw New Exception("Errore in CKT_GetCounts: " & Marshal.GetLastWin32Error)

            Dim info As New S300CountsInfo
            info.PersonsCount = pc
            info.FingerPrintsCount = fc
            info.ClockingsCounts = cc
            Return info
        End Function

        ''' <summary>
        ''' Restituisce la configurazione del dispositivo
        ''' </summary>
        ''' <returns></returns>
        Public Function GetConfiguration() As S300Config
            Me.EnsureConnected()
            Dim config As New S300Config

            Dim devnfo As CKT_DLL.DEVICEINFO = New CKT_DLL.DEVICEINFO()
            Dim ret As Integer = CKT_DLL.CKT_GetDeviceInfo(Me.m_IDNumber, devnfo)
            If (ret = 0) Then Throw New Exception("Errore in CKT_GetDeviceInfo: " & Marshal.GetLastWin32Error)

            config.RingAllow = devnfo.RingAllow <> 0
            config.RealtimeMode = devnfo.RealTimeAllow <> 0
            config.AutoUpdateFingerprint = devnfo.AutoUpdateAllow <> 0
            config.SpeakerVolume = devnfo.SpeakerVolume
            config.LockDelayTime = devnfo.LockDelayTime
            config.AdminPassword = Me.ToAnsiString(devnfo.AdminPassword)
            config.FixedWiegandAreaCode = devnfo.FixWGHead
            config.WiegandOption = devnfo.WGOption
            config.MinDelayInOut = devnfo.KQRepeatTime

            Return config
        End Function

        Private Function ToAnsiString(ByVal arr As Byte()) As String
            Dim ret As String = System.Text.Encoding.ASCII.GetString(arr)
            Dim p As Integer = InStr(ret, vbNullChar)
            If (p > 0) Then ret = Left(ret, p - 1)
            Return ret
        End Function

        ''' <summary>
        ''' Imposta la configurazione
        ''' </summary>
        ''' <param name="value"></param>
        Public Sub SetConfiguration(ByVal value As S300Config)
            Me.EnsureConnected()
            Dim ret As Integer
            ret = CKT_DLL.CKT_SetRingAllow(Me.m_IDNumber, IIf(value.RingAllow, 1, 0))
            If (ret = 0) Then Throw New Exception("Errore in CKT_SetRingAllow: " & Marshal.GetLastWin32Error)

            ret = CKT_DLL.CKT_SetRealtimeMode(Me.m_IDNumber, IIf(value.RealtimeMode, 1, 0))
            If (ret = 0) Then Throw New Exception("Errore in CKT_SetRealtimeMode: " & Marshal.GetLastWin32Error)

            ret = CKT_DLL.CKT_SetAutoUpdate(Me.m_IDNumber, IIf(value.AutoUpdateFingerprint, 1, 0))
            If (ret = 0) Then Throw New Exception("Errore in CKT_SetAutoUpdate: " & Marshal.GetLastWin32Error)

            ret = CKT_DLL.CKT_SetSpeakerVolume(Me.m_IDNumber, value.SpeakerVolume)
            If (ret = 0) Then Throw New Exception("Errore in CKT_SetSpeakerVolume: " & Marshal.GetLastWin32Error)

            ret = CKT_DLL.CKT_SetDoor(Me.m_IDNumber, value.LockDelayTime)
            If (ret = 0) Then Throw New Exception("Errore in CKT_SetDoor: " & Marshal.GetLastWin32Error)

            ret = CKT_DLL.CKT_SetFixWGHead(Me.m_IDNumber, value.FixedWiegandAreaCode)
            If (ret = 0) Then Throw New Exception("Errore in CKT_SetFixWGHead: " & Marshal.GetLastWin32Error)

            ret = CKT_DLL.CKT_SetWG(Me.m_IDNumber, value.WiegandOption)
            If (ret = 0) Then Throw New Exception("Errore in CKT_SetWG: " & Marshal.GetLastWin32Error)

            ret = CKT_DLL.CKT_SetRepeatKQ(Me.m_IDNumber, value.MinDelayInOut)
            If (ret = 0) Then Throw New Exception("Errore in CKT_SetRepeatKQ: " & Marshal.GetLastWin32Error)

            ret = CKT_DLL.CKT_SetDeviceAdminPassword(Me.m_IDNumber, value.AdminPassword)
            If (ret = 0) Then Throw New Exception("Errore in CKT_SetDeviceAdminPassword: " & Marshal.GetLastWin32Error)

        End Sub

        ''' <summary>
        ''' Forza l'apertura del relè
        ''' </summary>
        Public Sub ForceOpenLock()
            Me.EnsureConnected()
            Dim ret As Integer = CKT_DLL.CKT_ForceOpenLock(Me.m_IDNumber)
            If (ret = 0) Then Throw New Exception("Errore in CKT_ForceOpenLock: " & Marshal.GetLastWin32Error)
        End Sub

        ''' <summary>
        ''' Restituisce l'elenco degli utenti definiti sul dispositivo
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Users As S300UsersCollection
            Get
                Me.EnsureConnected()
                If (Me.m_Users Is Nothing) Then Me.m_Users = New S300UsersCollection(Me)
                Return Me.m_Users
            End Get
        End Property

        ''' <summary>
        ''' Scarica le mercature dal dispositivo
        ''' </summary>
        ''' <returns></returns>
        Public Function GetAllClockings() As S300Clocking()
            Me.EnsureConnected()

            Dim arr As S300Clocking() = {}
            Dim cnt As Integer = 0

            Dim pLongRun As IntPtr = IntPtr.Zero
            'If CKT_GetClockingNewRecordEx(IDNumber, pLongRun) Then 'IF GET NewRecord
            If (CKT_DLL.CKT_GetClockingRecordEx(Me.DeviceID, pLongRun) <> 0) Then ' If GET Record
                While (True)
                    Dim RecordCount As Integer = 0
                    Dim RetCount As Integer = 0
                    Dim pClockings As IntPtr = IntPtr.Zero
                    Dim Ret As Integer = CKT_DLL.CKT_GetClockingRecordProgress(pLongRun, RecordCount, RetCount, pClockings)
                    If (RecordCount > 0) Then
                        ReDim Preserve arr(RecordCount - 1)
                        'ProgressBar1.Maximum = RecordCount
                    End If
                    If (Ret = 0) Then
                        Debug.Print("Error cond 1")
                        Exit While
                    End If

                    If (Ret <> 0) Then
                        Dim ptemp As IntPtr = pClockings

                        For i As Integer = 1 To RetCount '(i = 1; i <= RetCount; i++)
                            Dim clocking As CKT_DLL.CLOCKINGRECORD = New CKT_DLL.CLOCKINGRECORD()
                            clocking.Time = Array.CreateInstance(GetType(Byte), 20) ' New Byte[20]

                            PCopyMemory(clocking, pClockings, CKT_DLL.CLOCKINGRECORDSIZE)
                            pClockings = pClockings + CKT_DLL.CLOCKINGRECORDSIZE

                            arr(cnt) = New S300Clocking(Me, clocking)
                            cnt += 1



                        Next

                        If (ptemp <> 0) Then CKT_DLL.CKT_FreeMemory(ptemp)
                    End If

                    If (Ret = 1) Then
                        Debug.Print("Error cond 2")
                        Exit While
                    End If
                End While
            Else
                Debug.Print("Error cond 3")
            End If

            If (cnt > 0) Then
                ReDim Preserve arr(cnt - 1)
            Else
                arr = {}
            End If

            Return arr
        End Function

        ''' <summary>
        ''' Scarica le mercature dal dispositivo
        ''' </summary>
        ''' <returns></returns>
        Public Function GetNewClockings() As S300Clocking()
            Me.EnsureConnected()

            Dim arr As S300Clocking() = {}
            Dim cnt As Integer = 0

            Dim pLongRun As IntPtr = IntPtr.Zero
            'If CKT_GetClockingNewRecordEx(IDNumber, pLongRun) Then 'IF GET NewRecord
            If (CKT_DLL.CKT_GetClockingNewRecordEx(Me.DeviceID, pLongRun) <> 0) Then ' If GET Record
                While (True)
                    Dim RecordCount As Integer = 0
                    Dim RetCount As Integer = 0
                    Dim pClockings As IntPtr = IntPtr.Zero
                    Dim Ret As Integer = CKT_DLL.CKT_GetClockingRecordProgress(pLongRun, RecordCount, RetCount, pClockings)
                    If (RecordCount > 0) Then
                        ReDim Preserve arr(RecordCount - 1)
                        'ProgressBar1.Maximum = RecordCount
                    End If
                    If (Ret = 0) Then
                        Exit While
                    End If

                    If (Ret <> 0) Then
                        Dim ptemp As IntPtr = pClockings

                        For i As Integer = 1 To RetCount '(i = 1; i <= RetCount; i++)
                            Dim clocking As CKT_DLL.CLOCKINGRECORD = New CKT_DLL.CLOCKINGRECORD()
                            clocking.Time = Array.CreateInstance(GetType(Byte), 20) ' New Byte[20]

                            PCopyMemory(clocking, pClockings, CKT_DLL.CLOCKINGRECORDSIZE)
                            pClockings = pClockings + CKT_DLL.CLOCKINGRECORDSIZE

                            arr(cnt) = New S300Clocking(Me, clocking)
                            cnt += 1



                        Next

                        If (ptemp <> 0) Then CKT_DLL.CKT_FreeMemory(ptemp)
                    End If

                    If (Ret = 1) Then
                        Exit While
                    End If
                End While
            End If

            Return arr
        End Function

        ''' <summary>
        ''' Elimina tutte le marcature
        ''' </summary>
        Public Sub DeleteAllClockings()
            Me.EnsureConnected()
            Dim ret As Integer = CKT_DLL.CKT_ClearClockingRecord(Me.DeviceID, ClearClockingRecordTypes.All, 0)
            If ret = 0 Then Throw New S300Exception
            '    MessageBox.Show("CKT_ClearClockingRecord OK!")
            'Else
            '    MessageBox.Show("Í¨Ñ¶Ê§°Ü")
            'End If
        End Sub

        ''' <summary>
        ''' Elimina le prime N marcature
        ''' </summary>
        ''' <param name="n"></param>
        Public Sub DeleteFirstNClockings(ByVal n As Integer)
            Me.EnsureConnected()
            Dim ret As Integer = CKT_DLL.CKT_ClearClockingRecord(Me.DeviceID, ClearClockingRecordTypes.Count, n)
            If ret = 0 Then Throw New S300Exception
            '    MessageBox.Show("CKT_ClearClockingRecord OK!")
            'Else
            '    MessageBox.Show("Í¨Ñ¶Ê§°Ü")
            'End If
        End Sub

        ''' <summary>
        ''' Resetta il dispositivo riportandolo alle impostazioni di fabbrica
        ''' </summary>
        Public Sub FactoryReset()
            Me.EnsureConnected()
            Dim ret As Integer = CKT_DLL.CKT_ResetDevice(Me.DeviceID)
            If (ret <> 0) Then Throw New S300Exception()
            '            MessageBox.Show("CKT_ResetDevice OK!")
            'End If
        End Sub

        ''' <summary>
        ''' Restituisce un array di strutture contenenti gli orari per la sirena
        ''' </summary>
        ''' <returns></returns>
        Public Function GetRingTimes() As RINGTIME()
            Me.EnsureConnected()
            Dim tarr As CKT_DLL.RINGTIME() = System.Array.CreateInstance(GetType(CKT_DLL.RINGTIME), 50)
            Dim ret As Integer = CKT_DLL.CKT_GetHitRingInfo(Me.DeviceID, tarr)
            If (ret = 0) Then Throw New S300Exception

            Dim arr As New System.Collections.ArrayList
            For i As Integer = 0 To tarr.Length - 1
                If tarr(i).week <> 0 Then arr.Add(tarr(i))
            Next

            Return arr.ToArray(GetType(RINGTIME))
        End Function

        ''' <summary>
        ''' Imposta gli orari 
        ''' </summary>
        ''' <param name="arr"></param>
        Public Sub SetRingTimes(ByVal arr As RINGTIME())
            Me.EnsureConnected()
            For i As Integer = 0 To arr.Length - 1
                Dim ret As Integer = CKT_DLL.CKT_SetHitRingInfo(Me.DeviceID, i, arr(i))
                If ret = 0 Then Throw New S300Exception
            Next


        End Sub
    End Class


End Namespace
