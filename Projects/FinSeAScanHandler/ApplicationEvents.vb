Imports System.Net.Mail
Imports DIALTPLib
Imports Microsoft.Win32
Imports System.Net.NetworkInformation
Imports DMD
Imports DMD.Sistema
Imports DMD.Net.HTTPProxy

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


        Protected Overrides Function OnUnhandledException(e As ApplicationServices.UnhandledExceptionEventArgs) As Boolean
            Try
                Sistema.Events.NotifyUnhandledException(e.Exception)
            Catch ex As Exception

            End Try
            e.ExitApplication = False
            Return MyBase.OnUnhandledException(e)
        End Function

        Protected Overrides Sub OnStartupNextInstance(e As ApplicationServices.StartupNextInstanceEventArgs)
            'MyBase.OnStartupNextInstance(eventArgs)
            e.BringToForeground = True
        End Sub

        Private Sub MyApplication_Startup(sender As Object, e As ApplicationServices.StartupEventArgs) Handles Me.Startup
            ' Add any initialization after the InitializeComponent() call.
            Dim ac As New ApplicationContext
            DMD.Sistema.SetApplicationContext(ac)
            ac.Start()



        End Sub









        Private Function GetExeModuleHandle() As Integer
            Return System.Runtime.InteropServices.Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly.GetModules()(0)).ToInt32
        End Function


        Private Shared Function GetOrCreateSubKey(ByVal p As RegistryKey, ByVal keyName As String) As RegistryKey
            Dim ret As RegistryKey = Nothing
            Try
                ret = p.OpenSubKey(keyName, True)
            Catch ex As Exception

            End Try
            If (ret Is Nothing) Then
                ret = p.CreateSubKey(keyName)
            End If
            Return ret
        End Function






    End Class


End Namespace

