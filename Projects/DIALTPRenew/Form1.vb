Imports System.Runtime.InteropServices

Public Class Form1
    Private listener As FinSeA.Net.Messaging.XDListener = Nothing
    Private lastReceivedMessageTime As Nullable(Of Date) = Nothing

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.Visible = False

        SetDialtpRenewPID(GetCurrentProcessId)

        'Dim t As Boolean = False
        'For Each p As Process In Process.GetProcesses
        '    Try
        '        t = Strings.StrComp(p.MainModule.FileName, percorso, CompareMethod.Text) = 0
        '        If (t) Then Exit For
        '    Catch ex As Exception

        '    End Try
        'Next
        'If Not t Then
        '    Shell(percorso)
        'End If
        If (listener Is Nothing) Then
            listener = New FinSeA.Net.Messaging.XDListener
            AddHandler listener.MessageReceived, AddressOf handleMessage
            listener.RegisterChannel("DIALTP.Renew")
        End If

        FinSeA.Net.Messaging.XDBroadcast.SendToChannel("DIALTP.Renew", "DIALTPRenew")
        If (lastReceivedMessageTime.HasValue = False) OrElse Math.Abs((Now - lastReceivedMessageTime.Value).TotalMilliseconds) > 500 Then
            KillDialTP()
            Dim percorso As String = My.Application.Info.DirectoryPath & "\DIALTP.exe"
            Shell(percorso)
        End If

    End Sub

    Public Sub handleMessage(ByVal sender As Object, ByVal e As FinSeA.Net.Messaging.XDMessageEventArgs)
        If (TypeOf (e.DataGram.Message) Is String AndAlso CStr(e.DataGram.Message) = "DIALTP") Then
            lastReceivedMessageTime = Now
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Visible = False
    End Sub

    Public Function GetDialtpRenewPID() As Integer
        Return CInt(My.Computer.Registry.CurrentUser.GetValue("HKEY_CURRENT_USER\Software\FinSeA\DIALTP\DialTPRenewPID", 0))
    End Function

    Public Function GetDialtpPID() As Integer
        Return CInt(My.Computer.Registry.CurrentUser.GetValue("HKEY_CURRENT_USER\Software\FinSeA\DIALTP\DialTPPID", 0))
    End Function

    Public Sub SetDialtpRenewPID(ByVal pid As Integer)
        My.Computer.Registry.CurrentUser.SetValue("HKEY_CURRENT_USER\Software\FinSeA\DIALTP\DialTPRenewPID", pid)
    End Sub

    Public Sub SetDialtpPID(ByVal pid As Integer)
        My.Computer.Registry.CurrentUser.SetValue("HKEY_CURRENT_USER\Software\FinSeA\DIALTP\DialTPPID", pid)
    End Sub

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Shared Function GetCurrentProcessId() As Integer

    End Function

    Public Sub KillDialTP()
        Dim pid As Integer = GetDialtpPID()
        For Each p As Process In Process.GetProcesses
            Try
                Dim name As String = p.MainModule.ModuleName
                If Strings.StrComp(name, "DIALTP.exe") = 0 Then
                    p.Kill()
                End If
            Catch ex As Exception

            End Try
        Next
    End Sub
End Class
