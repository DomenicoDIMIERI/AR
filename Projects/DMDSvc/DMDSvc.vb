Imports DIALTPLib
Imports DIALTPLib.Keyboard
Imports Microsoft.Win32
Imports System.Management
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.Timers

Public Class DMDSvc

#Region "Interop"

    Public Enum ServiceState As Integer
        SERVICE_STOPPED = &H1
        SERVICE_START_PENDING = &H2
        SERVICE_STOP_PENDING = &H3
        SERVICE_RUNNING = &H4
        SERVICE_CONTINUE_PENDING = &H5
        SERVICE_PAUSE_PENDING = &H6
        SERVICE_PAUSED = &H7
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Public Structure ServiceStatus
        Public dwServiceType As Integer
        Public dwCurrentState As ServiceState
        Public dwControlsAccepted As Integer
        Public dwWin32ExitCode As Integer
        Public dwServiceSpecificExitCode As Integer
        Public dwCheckPoint As Integer
        Public dwWaitHint As Integer
    End Structure

    <DllImport("advapi32.dll", SetLastError:=True)>
    Private Shared Function SetServiceStatus(ByVal handle As IntPtr, ByRef serviceStatus As ServiceStatus) As Boolean

    End Function

#End Region

    Public Const NOTIFYINTERVALSECONDS As Integer = 5
    Private m_EventID As Integer = 0
    Private WithEvents m_Timer As System.Timers.Timer

    Public Sub New()
        Me.InitializeComponent()
        If (Not System.Diagnostics.EventLog.SourceExists("FinSeASvcEvtSrc")) Then
            System.Diagnostics.EventLog.CreateEventSource("FinSeASvcEvtSrc", "FinSeASvcEvtSrcLog")
        End If
        Me.EventLog1.Source = "FinSeASvcEvtSrc"
        Me.EventLog1.Log = "FinSeASvcEvtSrcLog"
        Me.m_Timer = Nothing
    End Sub

    Protected Overrides Sub OnStart(ByVal args() As String)
        '' Update the service state to Start Pending.  
        'Dim ServiceStatus As New ServiceStatus()
        'ServiceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING
        'ServiceStatus.dwWaitHint = 100000
        'SetServiceStatus(Me.ServiceHandle, ServiceStatus)
        Me.NotifySvcEvent("Avvio di FinSeASvc")
        Dim m As System.Reflection.Module = System.Reflection.Assembly.GetExecutingAssembly.GetModules()(0)
        DIALTPLib.Keyboard.HookKeyboard(Marshal.GetHINSTANCE(m))
        AddHandler DIALTPLib.Keyboard.KeyPressed, AddressOf handleKP
        'DMDSvcService.StartService(Marshal.GetHINSTANCE(m))

        Me.m_Timer = New System.Timers.Timer
        Me.m_Timer.Interval = NOTIFYINTERVALSECONDS * 1000
        Me.m_Timer.Enabled = True
        AddHandler DIALTPLib.Machine.MachineStatusChanged, AddressOf handleMachineEvent

        ' Update the service state to Running.  
        'ServiceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING
        'SetServiceStatus(Me.ServiceHandle, ServiceStatus)

        'Ripetere questa procedura per il metodo OnStop (facoltativo).

    End Sub

    Protected Overrides Sub OnStop()

        ' DIALTPLib.DIALTPService.StopService()
        Me.m_Timer.Enabled = False
        Me.m_Timer.Dispose()
        Me.m_Timer = Nothing

        RemoveHandler DIALTPLib.Machine.MachineStatusChanged, AddressOf handleMachineEvent

        Me.NotifySvcEvent("Arresto di FinSeASvc")
    End Sub


    Private Sub handleMachineEvent(ByVal sender As Object, ByVal e As DIALTPLib.Machine.MachineEvent)
        Me.NotifySvcEvent("Machine Status Changed")
        Try
            DIALTPLib.Log.NotifyPCStatus(e.EventName)
        Catch ex As Exception
            'Me.Log(ex.Message)
        End Try
    End Sub

    Protected Overrides Sub OnCustomCommand(command As Integer)
        Select Case command
            Case 100 : Me.Stop()
            Case Else : MyBase.OnCustomCommand(command)
        End Select

    End Sub

    Private Sub doTimerTick()
        Try
            DIALTPLib.Log.NotifyPCStatus("Running")
            Me.checkDIALTP()
        Catch ex As Exception
            Me.NotifySvcEvent("Errore:" & ex.Message & vbNewLine)
        End Try
    End Sub

    Private Function GetActiveUserName() As String
        Dim searcher As New ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem")
        Dim Collection As ManagementObjectCollection = searcher.Get()
        Dim username As String = CStr(Collection.Cast(Of ManagementBaseObject)().First()("UserName"))
        Return username
    End Function

    Private Function GetCurrentUserSSID() As String
        Dim ff As System.Security.Principal.WindowsIdentity
        ff = System.Security.Principal.WindowsIdentity.GetCurrent
        Return ff.User.Value
    End Function

    Private Function GetUserSSID(ByVal domain As String, ByVal userName As String) As String
        Dim f As New NTAccount(domain & "\" & userName)
        Dim s As SecurityIdentifier = f.Translate(GetType(SecurityIdentifier))
        Dim sidString As String = s.ToString()
        Return sidString
    End Function

    Private Sub checkDIALTP()
        Dim path As String = ""

        Dim loggedUser As String = Me.GetActiveUserName()
        Me.NotifySvcEvent("Current User: " & loggedUser)
        If (loggedUser = "") Then Return
        Dim n() As String = Split(loggedUser, "\")
        Dim userSSID As String = GetUserSSID(n(0), n(1))
        Me.NotifySvcEvent("Current User SSID: " & userSSID)

        Dim userReg As RegistryKey = My.Computer.Registry.Users.OpenSubKey(userSSID, False)
        If (userReg IsNot Nothing) Then
            Dim run As RegistryKey = userReg.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", False)
            If (run IsNot Nothing) Then
                path = run.GetValue("DialTP")
                run.Close()
            End If
            userReg.Close()
        End If

        If (path <> "") Then
            Me.NotifySvcEvent("Eseguio: " & path)
            Process.Start(path) '"file://" & Replace(path, "\", "/"))
        End If
    End Sub

    Protected Overrides Sub OnContinue()
        MyBase.OnContinue()
    End Sub

    Public Function NotifySvcEvent(ByVal message As String) As String
        Try
            System.IO.File.AppendAllText("C:\Temp\DMDSvc.log", Now.ToString("yyyy/MM/dd HH:mm:ss") & " -> " & message & vbNewLine)
            Me.m_EventID += 1
            Me.EventLog1.WriteEntry(message, EventLogEntryType.Information, Me.m_EventID)
            Return CStr(Me.m_EventID)
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Sub m_Timer_Elapsed(sender As Object, e As ElapsedEventArgs) Handles m_Timer.Elapsed
        Me.doTimerTick()
    End Sub

    Private Sub handleKP(ByVal sender As Object, ByVal e As KeyboardEventArgs)
        Me.NotifySvcEvent("Key Pressed: " & [Enum].GetName(GetType(VirtualKeys), e.Key))
    End Sub
End Class
