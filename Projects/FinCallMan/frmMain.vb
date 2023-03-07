Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Text.RegularExpressions
Imports System.Text
Imports FinSeA.CallManagers.Responses
Imports FinSeA.CallManagers.Actions
Imports FinSeA.CallManagers

Public Class frmMain

    Private WithEvents m As AsteriskCallManager

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        Me.m = New AsteriskCallManager("admin", "elastix456", "192.168.70.254")
        m.Start()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnIAX2.Click
        Dim r As IAXpeersResponse = m.Execute(New IAXpeers)
    End Sub

    Private Sub m_LoggedIn(sender As Object, e As ManagerLoginEventArgs) Handles m.LoggedIn
        Me.btnConnect.Enabled = False
        Me.btnDisconnect.Enabled = True
        Me.btnIAX2.Enabled = True
        Me.btnMailCount.Enabled = True
        Me.btnPing.Enabled = True
        Me.btnOriginate.Enabled = True
    End Sub

    Private Sub m_LoggedOut(sender As Object, e As ManagerLogoutEventArgs) Handles m.LoggedOut
        Me.btnConnect.Enabled = True
        Me.btnDisconnect.Enabled = False
        Me.btnIAX2.Enabled = False
        Me.btnMailCount.Enabled = False
        Me.btnPing.Enabled = False
        Me.btnOriginate.Enabled = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnDisconnect.Click
        Me.m.Stop()
    End Sub

    Private Sub btnMailCount_Click(sender As Object, e As EventArgs) Handles btnMailCount.Click
        Dim a As New MailboxCount("211@default")
        Dim r As MailboxCountResponse = Me.m.Execute(a)
        Debug.Print(r.ToString)
    End Sub

    Private Sub btnPing_Click(sender As Object, e As EventArgs) Handles btnPing.Click
        Dim a As New Ping
        Dim r As PingResponse = Me.m.Execute(a)
        Debug.Print(r.ToString)
    End Sub

    Private Sub btnOriginate_Click(sender As Object, e As EventArgs) Handles btnOriginate.Click
        Dim a As New Originate
        a.Channel = "SIP/225"
        a.Context = "from-internal"
        a.Exten = InputBox("Inserisci il numero") ' "03470815531"
        a.Priority = 1
        a.CallerID = "225"
        a.Async = True

        Dim r As OriginateResponse = Me.m.Execute(a)
        Debug.Print(r.ToString)
    End Sub
End Class
