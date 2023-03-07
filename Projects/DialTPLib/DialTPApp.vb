Imports System.IO
Imports System.Xml.Serialization
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Drivers
Imports DMD.Internals
Imports DIALTPLib.Internals
Imports System.Deployment.Application

Public NotInheritable Class DialTPApp

    Public Shared Event ConfigurationChanged(ByVal sender As Object, ByVal e As System.EventArgs)


    Private Shared m_Database As CDBConnection
    Private Shared m_Config As DialTPConfig
    Private Shared m_Configs As DialTPConfigClass
    Private Shared m_ProxyProfiles As CProxyProfilesCass
    Public Shared ReadOnly PendingCommands As New CCollection(Of DialTPCommand)

    Shared Sub New()
        m_Database = Nothing
        m_Config = New DialTPConfig
        m_Configs = Nothing
        m_ProxyProfiles = Nothing
    End Sub

    Public Shared Property Database As CDBConnection
        Get
            If (m_Database Is Nothing) Then Return APPConn
            Return m_Database
        End Get
        Set(value As CDBConnection)
            m_Database = value
        End Set
    End Property

    Public Shared ReadOnly Property ProxyProfiles As CProxyProfilesCass
        Get
            If m_ProxyProfiles Is Nothing Then m_ProxyProfiles = New CProxyProfilesCass
            Return m_ProxyProfiles
        End Get
    End Property

    Public Shared ReadOnly Property Configs As DialTPConfigClass
        Get
            If (m_Configs Is Nothing) Then
                m_Configs = New DialTPConfigClass
                m_Configs.Initialize()
            End If
            Return m_Configs
        End Get
    End Property

    Public Shared ReadOnly Property CurrentConfig As DialTPConfig
        Get
            Return m_Config
        End Get
    End Property

    Private Shared m_IconPath As String = ""
    Private Shared m_Icon As System.Drawing.Icon = Nothing
    Public Shared IDUltimaTelefonata As Integer = 0

    Public Shared ReadOnly Property DefaultIcon As System.Drawing.Icon
        Get
            If (m_Icon Is Nothing) Then
                If (m_IconPath = "") Then Return My.Resources.DMD
                Try
                    If (LCase(Left(m_IconPath, 4)) = "http") Then
                        'System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        Dim path As String = System.IO.Path.Combine(DMD.Sistema.ApplicationContext.StartupFloder, "defico.ico")
                        If (System.IO.File.Exists(path)) Then System.IO.File.Delete(path)
                        My.Computer.Network.DownloadFile(m_IconPath, path)
                        m_Icon = New System.Drawing.Icon(path)
                    Else
                        m_Icon = New System.Drawing.Icon(m_IconPath)
                    End If
                Catch ex As Exception
                    m_IconPath = ""
                    If (m_IconPath = "") Then Return My.Resources.DMD
                End Try
            End If
            Return m_Icon
        End Get
    End Property

    Shared Sub SetConfiguration(config As DialTPConfig)
        If (config Is Nothing) Then Throw New ArgumentNullException
        m_Config = config
#If Not DEBUG Then
        Try
#End If
        Dim tmp As String = Strings.Trim(CStr(config.Attributi.GetItemByKey("IconURL")))
        If (tmp <> m_IconPath) Then
            If (m_Icon IsNot Nothing) Then m_Icon.Dispose() : m_Icon = Nothing
        End If
        m_IconPath = tmp
#If Not DEBUG Then
        Catch ex As Exception

        End Try
#End If

#If Not DEBUG Then
        Try
#End If
        ResetAsteriskServers()
#If Not DEBUG Then
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try
#End If

#If Not DEBUG Then
        Try
#End If
        ResetWatchFolders()
#If Not DEBUG Then
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try
#End If

#If Not DEBUG Then
        Try
#End If
        ResetHylafaxServer()
#If Not DEBUG Then
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try
#End If

#If Not DEBUG Then
        Try
#End If
        ResetProxy()
#If Not DEBUG Then
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try
#End If

#If Not DEBUG Then
        Try
#End If
        ResetUSBHandler()
#If Not DEBUG Then
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
        End Try
#End If



        RaiseEvent ConfigurationChanged(Nothing, New System.EventArgs)
    End Sub

    Public Shared Sub ResetUSBHandler()
        If CStr(DialTPApp.CurrentConfig.Attributi.GetItemByKey("USBHandler")) = "true" Then
            If (Not USBDriveHandler.IsRunning) Then
                USBDriveHandler.Start()
            End If
        Else
            If (USBDriveHandler.IsRunning) Then
                USBDriveHandler.Stop()
            End If
        End If
    End Sub

    Private Shared Function UnescapeFolderName(ByVal folderName As String) As String
        folderName = Strings.Replace(folderName, "%%DESKTOP%%", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory))
        folderName = Strings.Replace(folderName, "%%COMMONDESKTOP%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory))
        folderName = Strings.Replace(folderName, "%%DOCUMENTS%%", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
        folderName = Strings.Replace(folderName, "%%COMMONDOCUMENTS%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments))
        folderName = Strings.Replace(folderName, "%%APPDATA%%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))
        folderName = Strings.Replace(folderName, "%%COMMONAPPDATA%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData))
        folderName = Strings.Replace(folderName, "%%SYSDIR%%", Environment.GetFolderPath(Environment.SpecialFolder.System))
        folderName = Strings.Replace(folderName, "%%SYSDIR86%%", Environment.GetFolderPath(Environment.SpecialFolder.SystemX86))
        folderName = Strings.Replace(folderName, "%%WINDOWSDIR%%", Environment.GetFolderPath(Environment.SpecialFolder.Windows))
        folderName = Strings.Replace(folderName, "%%PICTURES%%", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures))
        folderName = Strings.Replace(folderName, "%%COMMONPICTURES%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures))
        folderName = Strings.Replace(folderName, "%%MUSIC%%", Environment.GetFolderPath(Environment.SpecialFolder.MyMusic))
        folderName = Strings.Replace(folderName, "%%COMMONMUSIC%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic))
        folderName = Strings.Replace(folderName, "%%VIDEO%%", Environment.GetFolderPath(Environment.SpecialFolder.MyVideos))
        folderName = Strings.Replace(folderName, "%%COMMONVIDEO%%", Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos))
        Return Trim(folderName)
    End Function

    Private Shared Sub ResetWatchFolders()
        FolderWatch.StopWatching()
        For Each folderName As String In CurrentConfig.CartelleMonitorate
            folderName = UnescapeFolderName(folderName)
            If (folderName <> "") Then FolderWatch.AddFolder(folderName)
        Next
        For Each folderName As String In CurrentConfig.CartelleEscluse
            folderName = UnescapeFolderName(folderName)
            If (folderName <> "") Then FolderWatch.ExcludeFolder(folderName)
        Next
        FolderWatch.StartWatching()
    End Sub

    Private Shared Sub ResetAsteriskServers()

        ' DIALTPLib.AsteriskServers.StopListening()
        DIALTPLib.AsteriskServers.StartListening(New CCollection(Of AsteriskServer)(DialTPApp.CurrentConfig.AsteriskServers))

    End Sub

    Private Shared Sub ResetProxy()
        Dim c As ProxyConfig = DialTPApp.CurrentConfig.ProxyConfig
        If TestFlag(c.Flags, ProxyConfigFlags.Enabled) Then
            If c.ProxyIP <> ProxyService.ProxyIP OrElse
               c.ProxyPort <> ProxyService.ProxyPort Then
                If ProxyService.IsRunning Then ProxyService.Stop()
            Else
                If ProxyService.IsRunning Then Return
            End If
            ProxyService.ProxyIP = c.ProxyIP
            ProxyService.ProxyPort = c.ProxyPort
            If (c.ProfileName <> "") Then
                Dim serverName As String = DialTPApp.CurrentConfig.ServerName
                If (ProxyService.Profile Is Nothing OrElse ProxyService.Profile.Name <> c.ProfileName) Then
                    ProxyService.Profile = Remote.GetProxyProfile(serverName, c.ProfileName)
                End If
            Else
                ProxyService.Profile = Nothing
            End If
            ProxyService.Start()
        Else
            If ProxyService.IsRunning Then ProxyService.Stop()
        End If
    End Sub

    Private Shared Sub ResetHylafaxServer()
        Dim driver As HylaFaxDriver = Nothing
        For Each d As BaseFaxDriver In DMD.Sistema.FaxService.GetInstalledDrivers
            If (TypeOf (d) Is HylaFaxDriver) Then
                driver = CType(d, HylaFaxDriver)
                Exit For
            End If
        Next
        If (driver Is Nothing) Then Return
        driver.Disconnect()
        With DirectCast(driver.Config, HylafaxDriverConfiguration)
            .HostName = DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxServer
            .HostPort = DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxPort
            .UserName = DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxUserName
            .Password = DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxPassword
            .DialPrefix = DIALTPLib.DialTPApp.CurrentConfig.HylafaxConfig.HylafaxDialPrefix
        End With
        driver.Connect()

    End Sub

    Public Shared Function DequeueCommand(ByVal machine As String, ByVal user As String) As DialTPCommand
        SyncLock DIALTPLib.DialTPApp.PendingCommands
            Dim i As Integer = 0
            While (i < PendingCommands.Count)
                Dim tmp As DialTPCommand = PendingCommands(i)
                If (Strings.Compare(machine, tmp.IDPostazione) = 0 AndAlso Strings.Compare(user, tmp.IDUtente) = 0) Then
                    PendingCommands.RemoveAt(i)
                    Return tmp
                End If
                i += 1
            End While
            Return Nothing
        End SyncLock
    End Function


    Public Shared Function CheckForUpdates() As Boolean
        Dim info As UpdateCheckInfo = Nothing
        If (ApplicationDeployment.IsNetworkDeployed) Then
            Dim AD As ApplicationDeployment = ApplicationDeployment.CurrentDeployment
            info = AD.CheckForDetailedUpdate()
            Return info.UpdateAvailable
        Else
            Return False
        End If
    End Function

    Public Shared Function CheckForRequiredUpdates() As Boolean
        Dim info As UpdateCheckInfo = Nothing
        If (ApplicationDeployment.IsNetworkDeployed) Then
            Dim AD As ApplicationDeployment = ApplicationDeployment.CurrentDeployment
            info = AD.CheckForDetailedUpdate()
            Return info.UpdateAvailable AndAlso info.IsUpdateRequired
        Else
            Return False
        End If
    End Function

    Public Shared Sub InstallUpdateSyncWithInfo(ByVal forceUpdate As Boolean)
        Dim info As UpdateCheckInfo = Nothing

        If (ApplicationDeployment.IsNetworkDeployed) Then
            Dim AD As ApplicationDeployment = ApplicationDeployment.CurrentDeployment
            info = AD.CheckForDetailedUpdate()
            If (info.UpdateAvailable) Then
                AD.Update()
                System.Windows.Forms.Application.Restart()
            End If
        End If
    End Sub

End Class