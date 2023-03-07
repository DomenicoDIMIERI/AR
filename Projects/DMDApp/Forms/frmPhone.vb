Imports DMD
Imports DIALTPLib

Public Class frmPhone



    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        Me.PopulateList()
    End Sub

    Public Sub PopulateList()
        Dim items As CCollection(Of Interfono) = DIALTPLib.InterfonoService.Interfoni
        items.Sort()
        Me.lstInterni.Items.Clear()
        For Each i As Interfono In items
            If (i.IsAvailable) Then
                'Dim d As Date? = i.Log.EndDate
                'If (d.HasValue = False) Then d = i.Log.StartDate
                'If (d.HasValue) Then
                '    If (DMD.Sistema.DateUtils.DateDiff(DateInterval.Minute, d.Value, Now) <= 5) Then Me.lstInterni.Items.Add(i)
                'End If
                Me.lstInterni.Items.Add(i)
            End If
        Next
    End Sub

    Private m_SelItem As Interfono = Nothing

    Private Sub lstInterni_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstInterni.SelectedIndexChanged
        Try
            If (Me.m_SelItem IsNot Nothing AndAlso Me.m_SelItem.IsConnected) Then
                Me.m_SelItem.Disconnect()
            End If
        Catch ex As Exception
            DIALTPLib.Log.LogException(ex)
        End Try

        Me.m_SelItem = Me.lstInterni.SelectedItem

        If (Me.m_SelItem Is Nothing) Then
            Me.pnlInterno.Visible = False
        Else
            Me.m_SelItem.DisableMic = DIALTPLib.Settings.WaveInDisabled
            Me.lblInterno.Text = Me.m_SelItem.UserName
            Me.lblPostazione.Text = Me.m_SelItem.Address
            Me.lblUtente.Text = Me.m_SelItem.UserName
            If (Me.m_SelItem.IsConnected) Then
                Me.chkActive.Checked = True
                Me.chkActive.Image = My.Resources.MICR
            Else
                Me.chkActive.Checked = False
                Me.chkActive.Image = My.Resources.MICW
            End If

            Me.pnlInterno.Visible = True
        End If

    End Sub

    Private Sub lstInterni_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lstInterni.MouseDoubleClick

    End Sub

    Private Sub chkActive_CheckedChanged(sender As Object, e As EventArgs) Handles chkActive.CheckedChanged

#If Not DEBUG Then
        Try
#End If
        If (Me.chkActive.Checked) Then
                If (Not Me.m_SelItem.IsConnected) Then
                Me.m_SelItem.DisableMic = DIALTPLib.Settings.WaveInDisabled
                Me.m_SelItem.Connect()
                Me.chkActive.Image = My.Resources.MICR
                Me.chkActive.Text = "Aperto"
            End If
            Else
            If (Me.m_SelItem.IsConnected) Then
                Me.m_SelItem.Disconnect()
                Me.chkActive.Image = My.Resources.MICW
                Me.chkActive.Text = "Chiuso"

            End If
        End If


#If Not DEBUG Then
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
#End If
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        frmAudioConfig.Show()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        InterfonoService.Invalidate()
        Me.PopulateList()
    End Sub

    'Private Sub btnDisableMic_Click(sender As Object, e As EventArgs)
    '    If (Me.btnDisableMic.Checked) Then
    '        Me.btnDisableMic.Image = My.Resources.MICW
    '        Me.btnDisableMic.Text = "Microfono Disabilitato"
    '    Else
    '        Me.btnDisableMic.Image = My.Resources.MICR
    '        Me.btnDisableMic.Text = "Microfono Abilitato"
    '    End If

    '    If Me.m_SelItem IsNot Nothing Then
    '        Me.m_SelItem.DisableMic = Me.btnDisableMic.Checked
    '    End If
    'End Sub
End Class