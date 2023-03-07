Public Class frmDevicesList
    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click

        Me.Close()
    End Sub


    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        Me.Refill
    End Sub

    Public Sub Refill()
        Me.lstDevices.Items.Clear()
        Dim c As ANVIZS300DeviceConfig
        For Each c In ANVIZS300Devices.Items
            Me.lstDevices.Items.Add(c)
        Next
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Dim frm As New frmDeviceConfig
        Dim c As New ANVIZS300DeviceConfig
        frm.Item = c
        If (frm.ShowDialog = Windows.Forms.DialogResult.OK) Then
            ANVIZS300Devices.Items.Add(c)
            ANVIZS300Devices.Persist()
            Me.Refill()
        End If
    End Sub

    Private Sub lstDevices_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstDevices.SelectedIndexChanged
        Me.btnDelete.Enabled = Me.lstDevices.SelectedItems.Count > 0
        Me.btnEdit.Enabled = Me.lstDevices.SelectedItems.Count = 1
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim frm As New frmDeviceConfig
        Dim c As ANVIZS300DeviceConfig = Me.lstDevices.SelectedItem
        frm.Item = c
        If (frm.ShowDialog = Windows.Forms.DialogResult.OK) Then
            ANVIZS300Devices.Persist()
            Me.Refill()
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If MsgBox("Confermi l'eliminazione?", vbYesNo, "Attenzione") = vbYes Then
            Dim c As ANVIZS300DeviceConfig = Me.lstDevices.SelectedItem
            ANVIZS300Devices.Items.Remove(c)
            ANVIZS300Devices.Persist()
            Me.Refill
        End If
    End Sub
End Class