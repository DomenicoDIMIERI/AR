Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Drawing
Imports System.Threading
Imports System.Net.Mail
Imports Microsoft.Win32
Imports System.Management
Imports System.Net.NetworkInformation
Imports DShowNET
Imports DShowNET.Device
'Imports System.Device.Location

Public Class Machine

    Public Class MachineEvent
        Inherits System.EventArgs

        Private m_EventName As String

        Public Sub New()
            Me.m_EventName = ""
        End Sub

        Public Sub New(ByVal value As String)
            Me.New
            Me.m_EventName = value
        End Sub

        Public Property EventName As String
            Get
                Return Me.m_EventName
            End Get
            Set(value As String)
                Me.m_EventName = value
            End Set
        End Property


    End Class

    ''' <summary>
    ''' Evento generato quando viene rilevato un cambiamento dello stato del sistema
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Shared Event MachineStatusChanged(ByVal sender As Object, ByVal e As MachineEvent)

    ''' <summary>
    ''' Restituisce o imposta l'ora di avvio del sistema operativo
    ''' </summary>
    Private Shared m_BootTime As Date

    ''' <summary>
    ''' Restituisce o imposta l'ora in cui l'utente ha fatto il login
    ''' </summary>
    Private Shared m_LogInTime As Date

    ''' <summary>
    ''' Restituisce o imposta il numero di minuti di attività dell'utente (se il computer va in standby o se viene effettuato un cambio utente il tempo non viene aggiornato)
    ''' </summary>
    Private Shared m_ActiveTime As Long

    Private Shared m_CPUSerialNumber As String

    Private Shared m_BIOSVersion As String


    Private Shared m_Init As Boolean = False

    'Private Shared WithEvents watcher As GeoCoordinateWatcher
    'Private WithEvents watcher As GeoCoordinateWatcher
    'Public Sub GetLocationDataEvent()
    '    watcher = New System.Device.Location.GeoCoordinateWatcher()
    '    AddHandler watcher.PositionChanged, AddressOf watcher_PositionChanged
    '    watcher.Start()
    'End Sub

    'Private Sub watcher_PositionChanged(ByVal sender As Object, ByVal e As GeoPositionChangedEventArgs(Of GeoCoordinate))
    '    MsgBox(e.Position.Location.Latitude.ToString & ", " &
    '           e.Position.Location.Longitude.ToString)
    '    ' Stop receiving updates after the first one.
    '    watcher.Stop()
    'End Sub


    Shared Sub New()

        AddHandler SystemEvents.DisplaySettingsChanged, AddressOf handleDispChange
        AddHandler SystemEvents.SessionEnding, AddressOf handleSessionEnding

    End Sub

    Private Shared Sub CheckInit()
        If Not m_Init Then Init()
        m_Init = True


    End Sub

    Private Shared Sub Init()
        m_BootTime = Now.AddMilliseconds(-Environment.TickCount)


        Try
            m_CPUSerialNumber = getCPU_ID()
        Catch ex As Exception
            m_CPUSerialNumber = "PC" & Now.Ticks
        End Try

        Dim q As New SelectQuery("Win32_bios")
        Dim search As New ManagementObjectSearcher(q)
        Dim info As New ManagementObject

        For Each info In search.Get
            Try
                'm_SerialNumber = info("serialnumber").ToString
                m_BIOSVersion = info("version").ToString
            Catch ex As Exception

            End Try
            If (m_BIOSVersion <> "") Then Exit For
        Next

    End Sub

    Public Shared Function getCPU_ID() As String

        Dim cpuID As String = String.Empty
        Dim mc As ManagementClass = New ManagementClass("Win32_Processor")
        Dim moc As ManagementObjectCollection = mc.GetInstances()
        For Each mo As ManagementObject In moc
            If (cpuID = String.Empty) Then
                cpuID = mo.Properties("ProcessorId").Value.ToString()
            End If
        Next
        Return cpuID
    End Function

    ''' <summary>
    ''' Restituisce la data di avvio del sistema operativo
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property BootTime As Date
        Get
            CheckInit()
            Return m_BootTime
        End Get
    End Property

    ''' <summary>
    ''' Restituisce il numero seriale del processore
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetCPUSerialNumber() As String
        CheckInit()
        Return m_CPUSerialNumber
    End Function

    ''' <summary>
    ''' Restituisce l'indirizzo MAC della scheda di rete principale
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetMACAddress() As String
        Try
            Dim adapters As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
            Dim adapter As NetworkInterface
            Dim myMac As String = String.Empty

            For Each adapter In adapters
                If adapter.OperationalStatus = OperationalStatus.NotPresent Then Continue For

                Select Case adapter.NetworkInterfaceType
                'Exclude Tunnels, Loopbacks and PPP
                    Case NetworkInterfaceType.Tunnel, NetworkInterfaceType.Loopback, NetworkInterfaceType.Ppp
                    Case Else
                        Dim props As System.Net.NetworkInformation.IPInterfaceProperties = adapter.GetIPProperties
                        If props.GatewayAddresses.Count <= 0 Then Continue For
                        If Not adapter.GetPhysicalAddress.ToString = String.Empty And Not adapter.GetPhysicalAddress.ToString = "00000000000000E0" Then
                            myMac = adapter.GetPhysicalAddress.ToString
                            Exit For
                        End If

                End Select
            Next adapter

            Return myMac
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Restituisce l'indirizzo IP della scheda di rete principale
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetIPAddress() As String
        Try
            'Dim ret As String = String.Empty
            'Dim strHostName As String = System.Net.Dns.GetHostName()
            'Dim iphe As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(strHostName)

            'For Each ipheal As System.Net.IPAddress In iphe.AddressList
            '    If ipheal.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork Then
            '        ret = ipheal.ToString()
            '        Exit For
            '    End If
            'Next
            'Return ret
            Dim ret As System.Net.IPAddress = DMD.Net.Utils.GetLocalIP
            If (ret IsNot Nothing) Then
                Return ret.ToString
            Else
                Return "127.0.0.1"
            End If
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Restituisce il numero seriale composto da ID del processore, MAC
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetSerialNumber() As String
        Return GetCPUSerialNumber() & GetMACAddress()
    End Function

    Private Shared Sub handleDispChange(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    'during init of your application bind to this event  
    Private Shared Sub handleSessionEnding(ByVal sender As Object, ByVal e As SessionEndingEventArgs)
        Select Case e.Reason
            Case SessionEndReasons.Logoff
                RaiseEvent MachineStatusChanged(Nothing, New MachineEvent("LogOff"))
            Case SessionEndReasons.SystemShutdown
                RaiseEvent MachineStatusChanged(Nothing, New MachineEvent("ShutDown"))
        End Select

        If (Environment.HasShutdownStarted) Then
            'Tackle Shutdown


        Else
            'Tackle log off
        End If
    End Sub

    Public Shared Function GetOperatoratingSystemVersion() As String
        Return System.Environment.OSVersion.ToString()
    End Function


    Public Shared Function GetWindowsProductKey() As String
        'Try
        '    Dim strKey As String

        '    Dim bytDPID() As Byte = {}
        '    bytDPID = CType(My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\", "DigitalProductId", bytDPID), Byte())



        '    'Transfer only the needed bytes into our Key Array
        '    ' Key starts at byte 52 and is 15 bytes long.
        '    Dim bytKey(14) As Byte '0-14 = 15 bytes
        '    Array.Copy(bytDPID, 52, bytKey, 0, 15)
        '    'Our "Array" of valid CD-Key characters
        '    Dim strChar As String = "BCDFGHJKMPQRTVWXY2346789"
        '    'Finally, our decoded CD-Key to be returned
        '    'How Microsoft encodes this to begin with, I'd love to know...
        '    'but here's how we decode the byte array into a string
        '    'containing the CD-KEY.
        '    For j As Integer = 0 To 24
        '        Dim nCur As Short = 0
        '        For i As Integer = 14 To 0 Step -1
        '            nCur = CShort(nCur * 256 Xor bytKey(i))
        '            bytKey(i) = CByte(Int(nCur / 24))
        '            nCur = CShort(nCur Mod 24)
        '        Next
        '        strKey = strChar.Substring(nCur, 1) & strKey
        '    Next
        '    'Finally, insert the dashes into the string.
        '    For i As Integer = 4 To 1 Step -1
        '        strKey = strKey.Insert(i * 5, "-")
        '    Next

        '    Return strKey
        'Catch ex As Exception
        '    Debug.Print(ex.StackTrace)
        '    Return vbNullString
        'End Try
        Return GetProductKey("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\", "DigitalProductId")
    End Function

    Public Shared Function GetProductKey(ByVal KeyPath As String, ByVal ValueName As String) As String
        Dim arr() As Byte = {}
        arr = CType(My.Computer.Registry.GetValue(KeyPath, ValueName, arr), Byte())
        If arr Is Nothing OrElse arr.Length <= 0 Then Return vbNullString

        Dim tmp As String = ""

        For l As Integer = LBound(arr) To UBound(arr)
            tmp = tmp & " " & Hex(arr(l))
        Next

        Dim StartOffset As Integer = 52
        Dim EndOffset As Integer = 67
        Dim Digits(24) As String

        Digits(0) = "B" : Digits(1) = "C" : Digits(2) = "D" : Digits(3) = "F"
        Digits(4) = "G" : Digits(5) = "H" : Digits(6) = "J" : Digits(7) = "K"
        Digits(8) = "M" : Digits(9) = "P" : Digits(10) = "Q" : Digits(11) = "R"
        Digits(12) = "T" : Digits(13) = "V" : Digits(14) = "W" : Digits(15) = "X"
        Digits(16) = "Y" : Digits(17) = "2" : Digits(18) = "3" : Digits(19) = "4"
        Digits(20) = "6" : Digits(21) = "7" : Digits(22) = "8" : Digits(23) = "9"

        Dim dLen As Integer = 29
        Dim sLen As Integer = 15
        Dim HexDigitalPID(15) As String
        Dim Des(30) As String

        Dim tmp2 As String = ""

        For i = StartOffset To EndOffset
            HexDigitalPID(i - StartOffset) = CStr(arr(i))
            tmp2 = tmp2 & " " & Hex(HexDigitalPID(i - StartOffset))
        Next

        Dim KEYSTRING As String = ""

        For i As Integer = dLen - 1 To 0 Step -1
            If ((i + 1) Mod 6) = 0 Then
                Des(i) = "-"
                KEYSTRING = KEYSTRING & "-"
            Else
                Dim HN As Integer = 0
                For N As Integer = (sLen - 1) To 0 Step -1
                    Dim Value As Integer = CInt(CInt(HN * 2 ^ 8) Or CInt("&H" & HexDigitalPID(N)))
                    HexDigitalPID(N) = CStr(Value \ 24)
                    HN = (Value Mod 24)

                Next

                Des(i) = Digits(HN)
                KEYSTRING = KEYSTRING & Digits(HN)
            End If
        Next

        Return StrReverse(KEYSTRING)
    End Function

    Public Shared Function GetAudioInputDeviceNames() As String()
        Dim ret As String() = {}

        If (Not DsUtils.IsCorrectDirectXVersion()) Then
            Log.LogMessage("DirectX 8.1 NOT installed!")
            Return ret
        End If

        Dim capDevices As ArrayList = Nothing
        If (Not DsDev.GetDevicesOfCat(FilterCategory.AudioInputDevice, capDevices)) Then
            Log.LogMessage("No video capture devices found!")
            Return ret
        End If

        For Each dev As DsDevice In capDevices
            'DeviceSelector selector = New DeviceSelector(capDevices);
            'selector.ShowDialog(this);
            'dev = selector.SelectedDevice;
            ret = DMD.Sistema.Arrays.InsertSorted(Of String)(ret, dev.Name)
        Next

        Return ret
    End Function

End Class

