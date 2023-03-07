Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Drawing
Imports System.Threading
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
'Imports System.Runtime.Remoting.Channels.Ipc

Public Class Processi
    'Private Shared listener As DMD.Net.Messaging.XDListener = Nothing
    Private Shared lastProcessList As New DMD.CKeyCollection
    Private Shared lastReceivedMessageTime As Nullable(Of Date) = Nothing

    Public Shared Function GetDialtpRenewPID() As Integer
        Return CInt(My.Computer.Registry.CurrentUser.GetValue("HKEY_CURRENT_USER\Software\DMD\DIALTP\DialTPRenewPID", 0))
    End Function

    Public Shared Function GetDialtpPID() As Integer
        Return CInt(My.Computer.Registry.CurrentUser.GetValue("HKEY_CURRENT_USER\Software\DMD\DIALTP\DialTPPID", 0))
    End Function

    Public Shared Sub SetDialtpRenewPID(ByVal pid As Integer)
        My.Computer.Registry.CurrentUser.SetValue("HKEY_CURRENT_USER\Software\DMD\DIALTP\DialTPRenewPID", pid)
    End Sub

    Public Shared Sub SetDialtpPID(ByVal pid As Integer)
        My.Computer.Registry.CurrentUser.SetValue("HKEY_CURRENT_USER\Software\DMD\DIALTP\DialTPPID", pid)
    End Sub

    Public Shared Sub CheckService() 'As String
        Return

        '#If Not Debug Then
        '        Dim percorso As String

        '        SetDialtpPID(GetCurrentProcessId)

        '        If (listener Is Nothing) Then
        '            listener = New DMD.Net.Messaging.XDListener
        '            AddHandler listener.MessageReceived, AddressOf handleMessage
        '            listener.RegisterChannel("DIALTP.Renew")
        '        End If

        '        DMD.Net.Messaging.XDBroadcast.SendToChannel("DIALTP.Renew", "DIALTP")
        '        If (lastReceivedMessageTime.HasValue = False) OrElse Math.Abs((Now - lastReceivedMessageTime.Value).TotalMilliseconds) > 500 Then
        '            KillDialTPRenew()
        '            percorso = My.Application.Info.DirectoryPath & "\DIALTPRenew.exe"
        '            Shell(percorso)
        '        End If
        '#End If
    End Sub

    'Public Shared Sub handleMessage(ByVal sender As Object, ByVal e As DMD.Net.Messaging.XDMessageEventArgs)
    '    If TypeOf (e.DataGram.Message) Is String AndAlso CStr(e.DataGram.Message) <> "DIALTP" Then
    '        lastReceivedMessageTime = Now
    '        Debug.Print(Now & " " & CStr(e.DataGram.Message))
    '    End If
    'End Sub

    'Public Sub CheckProcesses()
    '    Dim percorso As String = ProgramFilesx86() & "\Fin.Se.A. Srl\DialTP\DialTP.exe"
    '    Dim t As Boolean = False
    '    For Each p As Process In Process.GetProcesses
    '        t = Strings.StrComp(p.MainModule.FileName, percorso, CompareMethod.Text) = 0
    '        If (t) Then Exit For
    '    Next
    '    If Not t Then
    '        Shell(percorso)
    '    End If
    'End Sub

    Private Shared Function GetPID(ByVal p As Process) As String
        Try
            Return p.Handle.ToString()
        Catch ex As Exception
            Return "ERR"
        End Try
    End Function

    Private Shared Function GetPName(ByVal p As Process) As String
        Try
            Return p.ProcessName
        Catch ex As Exception
            Return "ERR"
        End Try
    End Function

    Private Shared Function GetPPath(ByVal p As Process) As String
        Try
            Return p.MainModule.FileName
        Catch ex As Exception
            Return "ERR"
        End Try
    End Function

    Private Shared Function GetPWinTitle(ByVal p As Process) As String
        Try
            Return p.MainWindowTitle
        Catch ex As Exception
            Return "ERR"
        End Try
    End Function

    Private Shared Function GetPCPUTime(ByVal p As Process) As String
        Try
            Return p.TotalProcessorTime.ToString
        Catch ex As Exception
            Return "ERR"
        End Try
    End Function

    Private Shared Function GetP_NonpagedSystemMemorySize64(ByVal p As Process) As String
        Try
            Return CStr(p.NonpagedSystemMemorySize64)
        Catch ex As Exception
            Return "ERR"
        End Try
    End Function

    Private Shared Function GetP_PagedMemorySize64(ByVal p As Process) As String
        Try
            Return CStr(p.PagedMemorySize64)
        Catch ex As Exception
            Return "ERR"
        End Try
    End Function

    Private Shared Function GetP_PagedSystemMemorySize64(ByVal p As Process) As String
        Try
            Return CStr(p.PagedSystemMemorySize64)
        Catch ex As Exception
            Return "ERR"
        End Try
    End Function

    Private Shared Function GetP_PeakPagedMemorySize64(ByVal p As Process) As String
        Try
            Return CStr(p.PeakPagedMemorySize64)
        Catch ex As Exception
            Return "ERR"
        End Try
    End Function

    Private Shared Function GetP_PeakVirtualMemorySize64(ByVal p As Process) As String
        Try
            Return CStr(p.PeakVirtualMemorySize64)
        Catch ex As Exception
            Return "ERR"
        End Try
    End Function

    Private Shared Function GetP_WorkingSet64(ByVal p As Process) As String
        Try
            Return CStr(p.WorkingSet64)
        Catch ex As Exception
            Return "ERR"
        End Try
    End Function

    Private Shared Function GetP_PeakWorkingSet64(ByVal p As Process) As String
        Try
            Return CStr(p.PeakWorkingSet64)
        Catch ex As Exception
            Return "ERR"
        End Try
    End Function

    Public Shared Function CheckProcesses1() As String
        Return ""

        'Dim newProcess As New DMD.CCollection
        'Dim oldProcess As New DMD.CCollection

        'For Each p As Process In Process.GetProcesses
        '    If lastProcessList.ContainsKey("P" & p.Id) = False Then
        '        newProcess.Add(p)
        '    Else
        '        oldProcess.Add(p)
        '        lastProcessList.Remove(p)
        '    End If
        'Next

        'Dim html As String = ""
        'html &= "<table>"
        'html &= "<caption>PROCESSI TERMINATI</caption>"
        'html &= "<tr>"
        'html &= "<th>PID</th>"
        'html &= "<th>Nome</th>"
        'html &= "<th>Percorso</th>"
        'html &= "</tr>"
        'For Each p As Process In lastProcessList
        '    html &= "<tr>"
        '    html &= "<td>" & GetPID(p) & "</td>"
        '    html &= "<td>" & GetPName(p) & "</td>"
        '    html &= "<td>" & GetPPath(p) & "</td>"
        '    html &= "</tr>"
        'Next
        'html &= "</table>"

        'html &= "<br/>"
        'html &= "<hr/>"

        'html &= "<table>"
        'html &= "<caption>PROCESSI TERMINATI</caption>"
        'html &= "<tr>"
        'html &= "<th>PID</th>"
        'html &= "<th>Nome</th>"
        'html &= "<th>Percorso</th>"
        'html &= "<th>CPU</th>"
        'html &= "<th>Working Set</th>"
        'html &= "<th>Peak Working Set</th>"
        'html &= "<th>Paged</th>"
        'html &= "<th>Peak Paged</th>"
        'html &= "</tr>"
        'For Each p As Process In newProcess
        '    html &= "<tr>"
        '    html &= "<td>" & GetPID(p) & "</td>"
        '    html &= "<td>" & GetPName(p) & "</td>"
        '    html &= "<td>" & GetPPath(p) & "</td>"
        '    html &= "<td>" & GetPCPUTime(p) & "</td>"
        '    html &= "<td>" & GetP_WorkingSet64(p) & "</td>"
        '    html &= "<td>" & GetP_PeakWorkingSet64(p) & "</td>"
        '    html &= "<td>" & GetP_PagedMemorySize64(p) & "</td>"
        '    html &= "<td>" & GetP_PeakPagedMemorySize64(p) & "</td>"
        '    html &= "</tr>"
        'Next
        'html &= "</table>"

        'lastProcessList.Clear()
        'For Each p As Process In newProcess
        '    lastProcessList.Add("P" & p.Id, p)
        'Next

        'Return html
    End Function

    'Private Sub DebugProcess(ByVal p As Process)
    '    Dim str As String = p.ProcessName
    '    str &= vbTab
    '    str &= vbTab
    '    Try
    '        str &= p.Handle.ToString
    '    Catch ex As Exception
    '        str &= "." & vbTab
    '    End Try
    '    str &= vbTab
    '    Try
    '        str &= p.MainModule.FileName & vbTab & p.MainModule.ModuleName
    '    Catch ex As Exception
    '        str &= "." & vbTab & "."
    '    End Try
    '    str &= vbTab
    '    Try
    '        str &= p.MainWindowTitle
    '    Catch ex As Exception
    '        str &= "."
    '    End Try
    '    str &= vbTab

    '    Try
    '        str &= p.TotalProcessorTime.ToString
    '    Catch ex As Exception
    '        str &= "."
    '    End Try
    '    str &= vbTab


    '    DIALTP.Log.GetCurrSession.Append(str)
    'End Sub

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Shared Function GetCurrentProcessId() As Integer

    End Function

    Public Shared Sub KillDialTPRenew()
        Dim pid As Integer = GetDialtpRenewPID()
        For Each p As Process In Process.GetProcesses
            If p.Id = pid Then
                Try
                    Dim name As String = p.MainModule.ModuleName
                    If Strings.StrComp(name, "DIALTPRenew.exe") = 0 Then
                        p.Kill()
                    End If
                Catch ex As Exception

                End Try
            End If
        Next
    End Sub

End Class

