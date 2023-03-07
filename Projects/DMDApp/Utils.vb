Imports Microsoft.Win32

Module Utils

    Public Sub CheckProtocol()
        If DIALTPLib.Settings.RegisterDialtp Then
            RegisterProtocol()
        Else
            UnRegisterProtocol()
        End If
    End Sub


    Public Sub CheckAutostart()
        If (DIALTPLib.Settings.AutoStart) Then
            RegisterStartUp()
        Else
            UnRegisterStartUp()
        End If
    End Sub

    Public Function GetAppPath() As String
        Return DMD.Sistema.FileSystem.NormalizePath(My.Application.Info.DirectoryPath) & "DMDApp.exe"
    End Function



    Public Sub RegisterProtocol()
        Dim software As RegistryKey = GetOrCreateSubKey(My.Computer.Registry.CurrentUser, "Software")
        Dim Classes As RegistryKey = GetOrCreateSubKey(software, "Classes")
        Dim dialtp As RegistryKey = GetOrCreateSubKey(Classes, "dialtp")
        dialtp.SetValue("", "URL:Dial protocol")
        dialtp.SetValue("URL Protocol", "")
        Classes.Close()
        software.Close()

        Dim DefaultIcon As RegistryKey = GetOrCreateSubKey(dialtp, "DefaultIcon")
        DefaultIcon.SetValue("", "C:\Windows\System32\url.dll,0")
        Dim shell As RegistryKey = GetOrCreateSubKey(dialtp, "shell")
        Dim open As RegistryKey = GetOrCreateSubKey(shell, "open")
        Dim command As RegistryKey = GetOrCreateSubKey(open, "command")
        command.SetValue("", Chr(34) & GetAppPath() & """ ""%1""")

        shell.Close()
        open.Close()
        command.Close()
        dialtp.Close()

    End Sub


    Public Sub UnRegisterProtocol()
        Dim software As RegistryKey = GetOrCreateSubKey(My.Computer.Registry.CurrentUser, "Software")
        Dim Classes As RegistryKey = GetOrCreateSubKey(software, "Classes")
        Classes.DeleteSubKeyTree("dialtp", False)
        Classes.Close()
    End Sub

    Public Function IsProtocolRegistered() As Boolean
        Dim red As RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("Software\Classes\dialtp\shell\open\command", False)
        Dim ret As Boolean = False
        If (red IsNot Nothing) Then
            ret = red.GetValue("") = Chr(34) & GetAppPath() & """ ""%1"""
        End If
        red.Close()
        Return ret
    End Function

    Private Function OpenRegKey(ByVal u As RegistryKey, ByVal path As String, ByVal editable As Boolean) As RegistryKey
        Dim c As RegistryKey = u
        Dim n As RegistryKey = Nothing
        Dim items As String() = Split(path, "\")
        For i As Integer = 0 To UBound(items) - 1
            n = c.OpenSubKey(items(i))
            c.Close()
            c = n
        Next
        n = c.OpenSubKey(items(UBound(items)), editable)
        c.Close()
        Return n
    End Function


    Public Function GetOrCreateSubKey(ByVal p As RegistryKey, ByVal keyName As String) As RegistryKey
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


    Public Sub RegisterStartUp()
        'Dim run As RegistryKey = OpenRegKey(My.Computer.Registry.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
        Dim run As RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
        'Dim path As String = DMD.Sistema.FileSystem.NormalizePath(My.Application.Info.DirectoryPath) & "dialtp.exe"
        Dim path As String = GetAppPath() ' DMD.Sistema.FileSystem.NormalizePath(System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)) & "DMDApp.appref-ms"
        run.SetValue("DialTP", path)
        run.Close()
    End Sub

    Public Sub UnregisterStartUp()
        'Dim run As RegistryKey = OpenRegKey(My.Computer.Registry.CurrentUser, "SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
        Dim run As RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
        'Dim path As String = DMD.Sistema.FileSystem.NormalizePath(My.Application.Info.DirectoryPath) & "dialtp.exe"
        run.DeleteValue("DialTP", False)
        run.Close()
    End Sub

End Module
