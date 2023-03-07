Imports System.ComponentModel
Imports DMD.S300

Public Class frmUtenti
    Private m_IsLoading As Boolean = False
    Private WithEvents dev As DMD.S300.S300Device = Nothing
    Private seluser As S300PersonInfo = Nothing

    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Device As DMD.S300.S300Device
        Get
            Return Me.dev
        End Get
        Set(value As DMD.S300.S300Device)
            Me.dev = value
            Me.Refill()
        End Set
    End Property

    Public Sub Refill()
        If (Me.m_IsLoading) Then Return

        Me.lstUsers.Items.Clear()
        If Not Me.dev.IsConnected Then Me.dev.Start()
        For Each u As S300PersonInfo In Me.dev.Users
            Dim lItem As Windows.Forms.ListViewItem = Me.lstUsers.Items.Add(u.PersonID)
            lItem.SubItems.Add(u.Name)
            lItem.Tag = u
        Next
        seluser = Nothing
        Me.dev.Stop()
        Me.CheckUser()

        Me.m_IsLoading = False
    End Sub

    Private Sub lstUsers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstUsers.SelectedIndexChanged
        Me.CheckUser()
    End Sub

    Public Function GetSelectedUser() As S300PersonInfo
        If Me.dev Is Nothing Then Return Nothing

        If Me.lstUsers.SelectedItems.Count > 0 Then
            Return Me.lstUsers.SelectedItems(0).Tag
        Else
            Return Nothing
        End If
    End Function

    Private Sub CheckUser()
        Me.seluser = Me.GetSelectedUser
        If (Me.seluser Is Nothing) Then
            Me.txtUserID.Enabled = False
            Me.txtUserName.Enabled = False
            Me.btnLoadFP1.Enabled = False
            Me.btnLoadFP2.Enabled = False
            Me.btnSaveFP1.Enabled = False
            Me.btnSaveFP2.Enabled = False
            Me.txtUserID.Text = vbNullString
            Me.txtUserName.Text = vbNullString
        Else
            If Me.dev.IsConnected = False Then Me.dev.Start()
            Me.txtUserID.Enabled = False
            Me.txtUserName.Enabled = True
            Me.btnLoadFP1.Enabled = True
            Me.btnLoadFP2.Enabled = Me.seluser.FingerPrints.Count > 0
            Me.btnSaveFP1.Enabled = Me.seluser.FingerPrints.Count > 0
            Me.btnSaveFP2.Enabled = Me.seluser.FingerPrints.Count > 1
            Me.txtUserID.Text = Me.seluser.PersonID
            Me.txtUserName.Text = Me.seluser.Name
            Me.dev.Stop()
        End If
    End Sub

    Private Sub btnSaveFP1_Click(sender As Object, e As EventArgs) Handles btnSaveFP1.Click
        Dim sfn As System.Windows.Forms.SaveFileDialog = Nothing
        Try
            If Me.dev.IsConnected = False Then Me.dev.Start()
            Dim u As S300PersonInfo = Me.GetSelectedUser
            sfn = New System.Windows.Forms.SaveFileDialog
            sfn.Title = "Salve l'impronta 1"
            If sfn.ShowDialog = Windows.Forms.DialogResult.OK Then
                u.FingerPrints(0).SaveToFile(sfn.FileName)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        Finally
            If (sfn IsNot Nothing) Then sfn.Dispose() : sfn = Nothing
            Me.dev.Stop()

        End Try
    End Sub

    Private Sub btnSaveFP2_Click(sender As Object, e As EventArgs) Handles btnSaveFP2.Click
        Dim sfn As System.Windows.Forms.SaveFileDialog = Nothing
        Try
            If Me.dev.IsConnected = False Then Me.dev.Start()
            Dim u As S300PersonInfo = Me.GetSelectedUser
            sfn = New System.Windows.Forms.SaveFileDialog
            sfn.Title = "Salve l'impronta 2"
            If sfn.ShowDialog = Windows.Forms.DialogResult.OK Then
                u.FingerPrints(1).SaveToFile(sfn.FileName)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        Finally
            If (sfn IsNot Nothing) Then sfn.Dispose() : sfn = Nothing
            Me.dev.Stop()

        End Try
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        If Me.dev.IsConnected = False Then Me.dev.Start()
        Me.seluser = Me.dev.Users.Create()
        Me.txtUserID.Enabled = True
        Me.txtUserID.Text = vbNullString
        Me.txtUserName.Enabled = True
        Me.txtUserName.Text = vbNullString
        Me.btnSave.Enabled = True
        Me.dev.Stop()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            If Me.dev.IsConnected = False Then Me.dev.Start()
            Me.seluser.PersonID = Me.txtUserID.Text
            Me.seluser.Name = Me.txtUserName.Text

            Me.seluser.Save()
            Me.dev.Stop()
            Me.Refill()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnLoadFP1_Click(sender As Object, e As EventArgs) Handles btnLoadFP1.Click
        Dim sfn As System.Windows.Forms.OpenFileDialog = Nothing
        Try
            If Me.dev.IsConnected = False Then Me.dev.Start()
            Dim u As S300PersonInfo = Me.GetSelectedUser
            sfn = New System.Windows.Forms.OpenFileDialog
            sfn.Title = "Carica l'impronta 1"
            If sfn.ShowDialog = Windows.Forms.DialogResult.OK Then
                u.FingerPrints(0).LoadFromFile(sfn.FileName)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        Finally
            If (sfn IsNot Nothing) Then sfn.Dispose() : sfn = Nothing
            Me.dev.Stop()

        End Try
    End Sub

    Private Sub btnRefill_Click(sender As Object, e As EventArgs) Handles btnRefill.Click
        Me.Refill()
    End Sub
End Class