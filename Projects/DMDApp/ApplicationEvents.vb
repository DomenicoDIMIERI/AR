Imports System.Net.Mail
Imports DIALTPLib
Imports Microsoft.Win32
Imports System.Net.NetworkInformation
Imports DMD
Imports DMD.Sistema
Imports DMD.Net.HTTPProxy
Imports System.Net
Imports Microsoft.VisualBasic.ApplicationServices

Namespace My

    Public Enum AppFlags As Integer
        ''' <summary>
        ''' Non cattura gli eventi della keyboard
        ''' </summary>
        ''' <remarks></remarks>
        NOKEYBOARD = 1

        ''' <summary>
        ''' Non cattura gli screenshot
        ''' </summary>
        ''' <remarks></remarks>
        NOSCREENSCHOTS = 2
    End Enum

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

        Private WithEvents configTimer As New System.Timers.Timer(60 * 1000)        'Timer che controlla la configurazione definita sul server remoto
        Private WithEvents screenShottimer As New System.Timers.Timer               'Timer utilizzato per nascondere automaticamente il form principale    
        'Private uploadLock As New Object
        'Private uploadingFiles As New CKeyCollection(Of String)

        Protected Overrides Function OnUnhandledException(e As ApplicationServices.UnhandledExceptionEventArgs) As Boolean
            Try
                DIALTPLib.Log.LogException(e.Exception)
                Sistema.Events.NotifyUnhandledException(e.Exception)
                e.ExitApplication = False
                Return MyBase.OnUnhandledException(e)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical)
                e.ExitApplication = True
                Return MyBase.OnUnhandledException(e)
            End Try

        End Function

        Protected Overrides Sub OnStartupNextInstance(e As ApplicationServices.StartupNextInstanceEventArgs)
            'MyBase.OnStartupNextInstance(eventArgs)
            e.BringToForeground = True
            If (e.CommandLine.Count > 0) Then
                If (e.CommandLine(0) = "hidden") Then Exit Sub
                Dim str As String = Strings.URLDecode(e.CommandLine(0))
                If frmDialer.PassArguments(str) Then
                    frmDialer.Refill()
                    If (frmDialer.o IsNot Nothing AndAlso Strings.InStr(frmDialer.o.Options, "directcall") > 0) Then
                        DialTPApp.IDUltimaTelefonata = Formats.ToInteger(frmDialer.o.ID)
                        frmDialer.Dial()
                    Else
                        frmDialer.Show()
                    End If
                Else
                    frmMain.Show()
                End If
            End If
        End Sub

        Private Sub MyApplication_Startup(sender As Object, e As ApplicationServices.StartupEventArgs) Handles Me.Startup

            ' Add any initialization after the InitializeComponent() call.
            Dim ac As New ApplicationContext
            DMD.Sistema.SetApplicationContext(ac)


            ac.Start()
#If Not DEBUG Then
            Try
#End If
            Utils.CheckProtocol
            Utils.CheckAutostart

            InitializeConfig()
#If Not DEBUG Then
            Catch ex As Exception

            End Try
#End If

            If (e.CommandLine.Count > 0) Then
                frmDialer.Show()
                frmDialer.PassArguments(e.CommandLine(0))
            Else
                frmMain.Show()
            End If

            configTimer.Enabled = True
        End Sub





        'Private Function EnsureDialer(ByVal dialer As DialerBaseClass)
        '    Dim col As Collection = Dialers.GetInstalledDialers
        '    For Each item As DialerBaseClass In col
        '        If item.Equals(dialer) Then Return dialer
        '    Next
        '    col.Add(dialer)
        '    Dialers.
        'End Function

        Private Shared Function EnsureAsterisk(ByVal config As DialTPConfig, ByVal serverName As String, ByVal channel As String, ByVal username As String, ByVal password As String) As AsteriskServer
            Dim items As CCollection(Of AsteriskServer) = config.AsteriskServers
            Dim server As AsteriskServer
            For Each server In items
                With server
                    If .ServerName = serverName AndAlso
                       .Channel = channel AndAlso
                       .UserName = username AndAlso
                       .Password = password Then
                        Return server
                    End If
                End With
            Next
            server = New AsteriskServer(channel, serverName, channel, username, password)
            server.CallerID = "Alza"
            items.Add(server)
            Try
                server.Connect()
            Catch ex As Exception

            End Try
            'AsteriskServers.SetServers(items)
            Return server
        End Function

        Private Shared Function EnsureDevice(ByVal config As DialTPConfig, ByVal nome As String, ByVal indirizzo As String, ByVal tipo As String) As DispositivoEsterno
            Dim items As CCollection(Of DispositivoEsterno) = config.Dispositivi
            Dim device As DispositivoEsterno
            For Each device In items
                With device
                    If .Nome = nome AndAlso
                       .Indirizzo = indirizzo AndAlso
                       .Tipo = tipo Then
                        Return device
                    End If
                End With
            Next
            device = New DispositivoEsterno(nome, indirizzo, tipo)
            items.Add(device)
            'Dispositivi.SetDispositivi(items)
            Return device
        End Function

        Public Sub InitializeConfig()
            AddHandler NetworkChange.NetworkAddressChanged, AddressOf handleNetworkAddressChange
            AddHandler NetworkChange.NetworkAvailabilityChanged, AddressOf handleNetworkAddressChange
            ' AddHandler DIALTPLib.FolderWatch.FileChange, AddressOf handleFileChange
            DIALTPLib.Log.StartLogging()

            Try
                DialTPApp.SetConfiguration(GetLastUsedConfiguration())
            Catch ex As Exception

            End Try
            Try
                CheckConfiguration()
            Catch ex As Exception

            End Try

            'configTimer.Enabled = True
        End Sub

        Private Function GetExeModuleHandle() As Integer
            Return System.Runtime.InteropServices.Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly.GetModules()(0)).ToInt32
        End Function

        Private m_Checking As Boolean = False
        Private m_handle As Integer = 0

        Public Sub CheckConfiguration()
            If m_Checking Then Return
            Me.m_Checking = True
            Dim config As New DialTPConfig
#If Not DEBUG Then
            Try
#End If
            config = GetUserConfiguration()
            DialTPApp.SetConfiguration(config)
#If Not DEBUG Then
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Return
            End Try
#End If


#If ENABLEHOOKS Then
            Dim handle As Integer = GetExeModuleHandle()
            If (m_handle = 0) Then
                m_handle = handle
            ElseIf (m_handle <> handle) Then
                Debug.Print("Handle difference")
            End If

            Try
                If (config.CaptureMouse) Then
                    If Not DIALTPLib.Mouse.IsHooked Then DIALTPLib.Mouse.Hook(handle)
                Else
                    If DIALTPLib.Mouse.IsHooked Then DIALTPLib.Mouse.UnHook()
                End If
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

             Try
                If (config.CaptureKeyboard) Then
                    If Not DIALTPLib.Keyboard.IsHooked Then DIALTPLib.Keyboard.HookKeyboard(handle)
                Else
                    If DIALTPLib.Keyboard.IsHooked Then DIALTPLib.Keyboard.UnhookKeyboard()
                End If
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try


            Try
                screenShottimer.Interval = Math.Max(1, config.FullScreenShotInterval) * 1000
                If (screenShottimer Is Nothing) Then screenShottimer = New System.Timers.Timer
                screenShottimer.Enabled = config.FullScreenShotInterval > 0
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try
#Else
            screenShottimer.Enabled = False
#End If



            Try
                SaveConfiguration(config)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            'C:\Works\My Web Sites\AR1\Librerie


            Try
                DIALTPLib.Log.NotifyPCStatus("Running")
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Try
                If (config.RemoveLogAfterNDays > 0) Then
                    DIALTPLib.Log.DeleteFilesBefore(DateUtils.DateAdd(DateInterval.Day, -config.RemoveLogAfterNDays, DateUtils.ToDay))
                End If
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try



            Try
                DMD.Sistema.EMailer.Config.SMTPLimitOutSent = DIALTPLib.Settings.SMTPLimitOutSent
                DMD.Sistema.EMailer.Config.SMTPUserName = DIALTPLib.Settings.SMTPUserName
                DMD.Sistema.EMailer.Config.SMTPPassword = DIALTPLib.Settings.SMTPPassword
                DMD.Sistema.EMailer.Config.SMTPServer = DIALTPLib.Settings.SMTPServer
                DMD.Sistema.EMailer.Config.SMTPServerPort = DIALTPLib.Settings.SMTPPort
                DMD.Sistema.EMailer.Config.SMTPUseSSL = DIALTPLib.Settings.SMTPSSL
                If (DMD.Sistema.EMailer.Config.IsChanged) Then
                    DMD.Sistema.EMailer.SetConfig(DMD.Sistema.EMailer.Config)
                    DMD.Sistema.EMailer.Config.SetChanged(False)
                End If
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try

            Me.m_Checking = False
        End Sub

        'Private Sub handleFileChange(ByVal sender As Object, ByVal e As System.IO.FileSystemEventArgs)
        '    SyncLock uploadLock
        '        If (e.ChangeType = System.IO.WatcherChangeTypes.Created) Then
        '            If uploadingFiles.ContainsKey(e.FullPath) Then Return
        '            uploadingFiles.Add(e.FullPath, e.FullPath)
        '        End If
        '    End SyncLock
        'End Sub








        Private Shared Sub CheckNewFiles(ByVal path As String)
            'Dim fInfo As New System.IO.DirectoryInfo(path)
            'Dim files() As System.IO.FileInfo = fInfo.GetFiles
            'For Each f As System.IO.FileInfo In files
            '    If f.Attributes And System.IO.FileAttributes.Archive = System.IO.FileAttributes.Archive Then
            '        UploadFile(f.FullName)
            '    End If
            'Next
        End Sub

        Private Shared Sub handleNetworkAddressChange(ByVal sender As Object, ByVal e As System.EventArgs)

            DIALTPLib.Log.LogMessage("Variazione indirizzo di rete")
            DIALTPLib.Log.LogNetworkConfiguration()
            DIALTPLib.Log.LogMessage("PING server: " & DIALTPLib.DialTPApp.CurrentConfig.UploadServer)
        End Sub

        Private Shared Function GetRemoteConfigurationProxy() As DIALTPLib.DialTPConfig
            Try
                Dim serverName As String = Trim(DIALTPLib.Settings.ConfigServer)
                If (serverName = "") Then Return Nothing
                If (Not serverName.EndsWith("/")) Then serverName = serverName & "/"
                Dim myProxy As New WebProxy
                myProxy.Address = New Uri("http://" & DIALTPLib.Settings.ProxyAddress & ":" & DIALTPLib.Settings.ProxyPort)
                Dim tmp As String = RPC.InvokeMethodProxy(myProxy, serverName & "widgets/websvcf/dialtp.aspx?_a=GetConfiguration", "mn", RPC.str2n(DIALTPLib.Log.GetMachineName), "un", RPC.str2n(DIALTPLib.Log.GetCurrentUserName), "apver", RPC.str2n(My.Application.Info.Version.ToString))
                If (tmp = "") Then Return Nothing
                Return XML.Utils.Serializer.Deserialize(tmp)
            Catch ex As Exception
                Throw
            Finally

            End Try
        End Function

        Private Shared Function GetRemoteConfigurationNoProxy() As DIALTPLib.DialTPConfig
            Dim serverName As String = Trim(DIALTPLib.Settings.ConfigServer)
            If (serverName = "") Then Return Nothing
            If (Not serverName.EndsWith("/")) Then serverName = serverName & "/"
            Dim tmp As String = RPC.InvokeMethod(serverName & "widgets/websvcf/dialtp.aspx?_a=GetConfiguration", "mn", RPC.str2n(DIALTPLib.Log.GetMachineName), "un", RPC.str2n(DIALTPLib.Log.GetCurrentUserName), "apver", RPC.str2n(My.Application.Info.Version.ToString))
            If (tmp = "") Then Return Nothing
            Return XML.Utils.Serializer.Deserialize(tmp)
        End Function

        Private Shared Function GetRemoteConfiguration() As DIALTPLib.DialTPConfig
            RPC.lResolve = 2000
            RPC.lReceive = 2000
            RPC.lSend = 2000
            If (DIALTPLib.Settings.ProxyAddress <> "") Then
                Try
                    Return GetRemoteConfigurationProxy()
                Catch ex As Exception
                    Return GetRemoteConfigurationNoProxy()
                End Try
            Else
                Return GetRemoteConfigurationNoProxy()
            End If
        End Function

        Private Shared Function GetLastUsedConfiguration() As DIALTPLib.DialTPConfig
            Dim path As String = System.IO.Path.Combine(DIALTPLib.Log.GetUserDataPath, "DialTP\lastconfig")
            If (System.IO.File.Exists(path) = False) Then Return Nothing
            Dim text As String = System.IO.File.ReadAllText(path)
            Try
                Return XML.Utils.Serializer.Deserialize(text)
            Catch ex As Exception
                Sistema.ApplicationContext.Log("Errore in GetLastUsedConfiguration: " & ex.Message & vbNewLine & ex.StackTrace)
                Return Nothing
            End Try
        End Function

        Private Shared Sub SaveConfiguration(ByVal config As DIALTPLib.DialTPConfig)
            Dim path As String = System.IO.Path.Combine(DIALTPLib.Log.GetUserDataPath, "DialTP\lastconfig")
            DMD.Sistema.FileSystem.CreateRecursiveFolder(FileSystem.GetFolderName(path))
            System.IO.File.WriteAllText(path, XML.Utils.Serializer.Serialize(config))
        End Sub


        Private Shared Function GetUserConfiguration() As DIALTPLib.DialTPConfig
            Dim ret As DIALTPLib.DialTPConfig = Nothing
#If Not DEBUG Then
            Try
#End If
            ret = GetRemoteConfiguration()
#If Not DEBUG Then
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
            End Try
#End If
            If (ret IsNot Nothing) Then
                If (ret.MinVersion <> "" AndAlso My.Application.Info.Version.ToString < ret.MinVersion) Then
                    MsgBox("Questa versione del programma è obsoleta ed è necessario aggiornarla." & vbCrLf & "Visita il sito http://www.DMD.net nella sezione Apps", MsgBoxStyle.Critical)
                    End
                End If
            End If

            If (ret Is Nothing) Then
#If Not DEBUG Then
                Try
#End If
                'Se avviene qualche errore proviamo ad utilizzare l'ultima configurazione
                ret = GetLastUsedConfiguration()
#If Not DEBUG Then
                Catch ex1 As Exception
                    Sistema.Events.NotifyUnhandledException(ex1)
                End Try
#End If
            End If

            If (ret Is Nothing) Then
                'Se siamo a questo punto vuol dire che non siamo riusciti nè a scaricare la configurazione remota
#If Not DEBUG Then
                Try
#End If
                ret = New DIALTPLib.DialTPConfig 'GetConfigurationByNetwork()
#If Not DEBUG Then
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                End Try
#End If
            End If

            Return ret
        End Function






        Private Shared Function EnsureLinea(ByVal config As DialTPConfig, ByVal prefix As String, ByVal testo As String) As LineaEsterna
            Dim linee As CCollection(Of LineaEsterna) = config.Linee
            Dim l As LineaEsterna
            For Each l In linee
                If l.Prefisso = prefix Then Return l
            Next
            l = New LineaEsterna
            l.Prefisso = prefix
            l.Nome = testo
            linee.Add(l)
            'DIALTPLib.LineeEsterne.SetLinee(linee)
            Return l
        End Function






        Private Sub configTimer_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles configTimer.Elapsed
            Me.CheckConfiguration()
        End Sub

        'Private m_SH As Boolean = False
        'Private Sub screenShottimer_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles screenShottimer.Elapsed
        '    Try
        '        If (m_SH) Then Return
        '        m_SH = True
        '        DIALTPLib.Log.TakeFullScreenShot()

        '    Catch ex As Exception
        '        Sistema.Events.NotifyUnhandledException(ex)
        '    End Try
        '    m_SH = False
        'End Sub

        Private Sub MyApplication_StartupNextInstance(sender As Object, e As StartupNextInstanceEventArgs) Handles Me.StartupNextInstance

        End Sub
    End Class


End Namespace

