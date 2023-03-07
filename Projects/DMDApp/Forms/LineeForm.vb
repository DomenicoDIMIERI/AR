Imports DMD
Imports DMD.Sistema
Imports DIALTPLib

Public Class LineeForm

    Private m_Linee As CCollection(Of LineaEsterna)
    Private m_SelItem As LineaEsterna
    Private m_Updating As Boolean = False

    Private Sub LineeForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Me.m_Linee = New CCollection(Of LineaEsterna)(DialTPApp.CurrentConfig.Linee)
            Me.lstLinee.Items.Clear()
            For Each d As LineaEsterna In Me.m_Linee
                Me.lstLinee.Items.Add(d)
            Next
            Me.CheckEnabled()
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            Dim item As New LineaEsterna
            item.Nome = "Nuova linea"
            Me.m_Linee.Add(item)
            Dim i As Integer = Me.lstLinee.Items.Add(item)
            Me.lstLinee.SelectedIndex = i
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            Dim item As LineaEsterna = Me.lstLinee.Items(Me.lstLinee.SelectedIndex)
            Me.m_Linee.Remove(item)
            Me.lstLinee.Items.Remove(item)
            Me.CheckEnabled()
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub


    Private Sub lstLinee_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstLinee.SelectedIndexChanged
        Try
            Me.CheckEnabled()
        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub CheckEnabled()
        If (Me.m_SelItem IsNot Nothing) Then
            Me.m_SelItem.Nome = Me.txtNome.Text
            Me.m_SelItem.Prefisso = Me.txtPrefisso.Text
            Me.m_SelItem.Server = Me.txtServerName.Text
        End If

        If (Me.lstLinee.SelectedIndex >= 0) Then
            Me.m_SelItem = Me.lstLinee.Items(Me.lstLinee.SelectedIndex)
        Else
            Me.m_SelItem = Nothing
        End If

        If (Me.m_SelItem IsNot Nothing) Then
            Me.txtPrefisso.Text = Me.m_SelItem.Prefisso
            Me.txtNome.Text = Me.m_SelItem.Nome
            Me.txtServerName.Text = Me.m_SelItem.Server
            Me.txtNome.Enabled = True
            Me.txtPrefisso.Enabled = True
            Me.txtServerName.Enabled = True
            Me.btnDelete.Enabled = True
        Else
            Me.txtNome.Enabled = False
            Me.txtPrefisso.Enabled = False
            Me.btnDelete.Enabled = False
            Me.txtServerName.Enabled = False
        End If
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            Me.CheckEnabled()
            Me.m_Linee.Sort()
            DialTPApp.CurrentConfig.Linee.Clear()
            For Each l As LineaEsterna In Me.m_Linee
                DialTPApp.CurrentConfig.Linee.Add(l)
            Next
            DialTPApp.SetConfiguration(DialTPApp.CurrentConfig)
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub


End Class