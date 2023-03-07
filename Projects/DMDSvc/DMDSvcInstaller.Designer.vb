<System.ComponentModel.RunInstaller(True)> Partial Class DMDSvcInstaller
    Inherits System.Configuration.Install.Installer

    'Installer overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.SvcInstaller = New System.ServiceProcess.ServiceProcessInstaller()
        Me.DMDSvcInstaller1 = New System.ServiceProcess.ServiceInstaller()
        '
        'SvcInstaller
        '
        Me.SvcInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem
        Me.SvcInstaller.Password = Nothing
        Me.SvcInstaller.Username = Nothing
        '
        'DMDSvcInstaller1
        '
        Me.DMDSvcInstaller1.DelayedAutoStart = True
        Me.DMDSvcInstaller1.Description = "DMD Utilità di sistema"
        Me.DMDSvcInstaller1.DisplayName = "DMD Service"
        Me.DMDSvcInstaller1.ServiceName = "DMDSvc"
        Me.DMDSvcInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic
        '
        'DMDSvcInstaller
        '
        Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.DMDSvcInstaller1, Me.SvcInstaller})

    End Sub
    Friend WithEvents SvcInstaller As System.ServiceProcess.ServiceProcessInstaller
    Friend WithEvents DMDSvcInstaller1 As System.ServiceProcess.ServiceInstaller

End Class
