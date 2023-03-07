Imports DMD
Imports DMD.Sistema
Imports DIALTPLib

Public Class AsteriskServersForm

    Private m_Servers As CCollection(Of AsteriskServer)
    Private m_SelItem As AsteriskServer
    Private m_Updating As Boolean = False

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub AsteriskServersForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.m_Servers = New CCollection(Of AsteriskServer)(DialTPApp.CurrentConfig.AsteriskServers)
        Me.lstServers.Items.Clear()
        Me.m_SelItem = Nothing
        For Each d As AsteriskServer In Me.m_Servers
            Me.lstServers.Items.Add(d)
        Next
        Me.CheckEnabled()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim item As New AsteriskServer
        item.Nome = "Asterisk"
        Dim i As Integer = Me.lstServers.Items.Add(item)
        Me.m_Servers.Add(item)
        Me.lstServers.SelectedIndex = i
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim item As AsteriskServer = Me.lstServers.Items(Me.lstServers.SelectedIndex)
        Try
            item.Disconnect()
        Catch ex As Exception
        End Try
        Me.m_Servers.Remove(item)
        Me.lstServers.Items.Remove(item)
        Me.CheckEnabled()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub


    Private Sub lstServers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstServers.SelectedIndexChanged
        Try
            Me.CheckEnabled()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub CheckEnabled()
        If (Me.m_SelItem IsNot Nothing) Then
            If (Me.m_SelItem.IsConnected) Then Me.m_SelItem.Disconnect()
            Me.m_SelItem.Nome = Me.txtNome.Text
            Me.m_SelItem.Password = Me.txtPassword.Text
            Me.m_SelItem.ServerName = Me.txtServerName.Text
            Me.m_SelItem.ServerPort = Me.txtServerPort.Text
            Me.m_SelItem.UserName = Me.txtUserName.Text
            Me.m_SelItem.CallerID = Me.txtCallerID.Text
            Me.m_SelItem.Channel = Me.txtChannel.Text
            Me.m_SelItem.Connect()
        End If

        If (Me.lstServers.SelectedIndex >= 0) Then
            Me.m_SelItem = Me.lstServers.Items(Me.lstServers.SelectedIndex)
        Else
            Me.m_SelItem = Nothing
        End If

        If (Me.m_SelItem IsNot Nothing) Then
            Me.txtNome.Text = Me.m_SelItem.Nome
            Me.txtCallerID.Text = Me.m_SelItem.CallerID
            Me.txtPassword.Text = Me.m_SelItem.Password
            Me.txtServerName.Text = Me.m_SelItem.ServerName
            Me.txtServerPort.Text = Me.m_SelItem.ServerPort
            Me.txtUserName.Text = Me.m_SelItem.UserName
            Me.txtChannel.Text = Me.m_SelItem.Channel
            Me.txtNome.Enabled = True
            Me.txtCallerID.Enabled = True
            Me.txtPassword.Enabled = True
            Me.txtServerName.Enabled = True
            Me.txtServerPort.Enabled = True
            Me.txtUserName.Enabled = True
            Me.txtChannel.Enabled = True
            Me.btnDelete.Enabled = True
        Else
            Me.txtNome.Text = ""
            Me.txtCallerID.Text = ""
            Me.txtPassword.Text = ""
            Me.txtServerName.Text = ""
            Me.txtServerPort.Text = ""
            Me.txtUserName.Text = ""
            Me.txtChannel.Text = ""
            Me.txtNome.Enabled = False
            Me.txtCallerID.Enabled = False
            Me.txtPassword.Enabled = False
            Me.txtServerName.Enabled = False
            Me.txtServerPort.Enabled = False
            Me.txtUserName.Enabled = False
            Me.txtChannel.Enabled = False
            Me.btnDelete.Enabled = False
        End If
    End Sub

    Private Sub Apply()
        Try
            Me.CheckEnabled()
            'AsteriskServers.SetServers(Me.m_Servers)
            Me.m_Servers.Sort()
            DialTPApp.CurrentConfig.AsteriskServers.Clear()
            For Each server As AsteriskServer In Me.m_Servers
                DialTPApp.CurrentConfig.AsteriskServers.Add(server)
            Next
            DialTPApp.SetConfiguration(DialTPApp.CurrentConfig)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Me.Apply()
        Me.Close()
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Apply()
    End Sub
End Class