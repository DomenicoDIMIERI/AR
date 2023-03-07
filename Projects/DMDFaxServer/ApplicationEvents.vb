Imports Microsoft.Win32

Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

        Protected Overrides Function OnStartup(eventArgs As ApplicationServices.StartupEventArgs) As Boolean

            ' Add any initialization after the InitializeComponent() call.
            Dim ac As New MyApplicationContext
            DMD.Sistema.SetApplicationContext(ac)
            ac.Start()

            RegisterStartUp()

            Return MyBase.OnStartup(eventArgs)
        End Function

        Protected Overrides Sub OnStartupNextInstance(eventArgs As ApplicationServices.StartupNextInstanceEventArgs)
            eventArgs.BringToForeground = True
            MyBase.OnStartupNextInstance(eventArgs)
        End Sub

        Public Shared Sub RegisterStartUp()
            Dim run As RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
            'Dim path As String = DMD.Sistema.FileSystem.NormalizePath(My.Application.Info.DirectoryPath) & "dialtp.exe"
            Dim path As String = DMD.Sistema.FileSystem.NormalizePath(System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)) & "DMDFaxServer.appref-ms"
            run.SetValue("FINFAX", path)
        End Sub

    End Class


End Namespace

