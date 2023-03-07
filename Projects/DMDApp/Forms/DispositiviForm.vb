Imports DMD
Imports DMD.Sistema
Imports DIALTPLib

Public Class DispositiviForm

    Private m_Dispositivi As CCollection(Of DispositivoEsterno)
    Private m_SelItem As DispositivoEsterno
    Private m_Updating As Boolean = False

    Public Sub New()

        ' La chiamata è richiesta dalla finestra di progettazione.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().

    End Sub

    Private Sub DispositiviForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.m_Dispositivi = New CCollection(Of DispositivoEsterno)(DialTPApp.CurrentConfig.Dispositivi)
        Me.lstDispositivi.Items.Clear()
        For Each d As DispositivoEsterno In Me.m_Dispositivi
            Me.lstDispositivi.Items.Add(d)
        Next
        Me.CheckEnabled()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim item As New DispositivoEsterno
        item.Nome = "Nuovo dispositivo"
        Me.m_Dispositivi.Add(item)

        Dim i As Integer = Me.lstDispositivi.Items.Add(item)
        Me.lstDispositivi.SelectedIndex = i
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim item As DispositivoEsterno = Me.lstDispositivi.Items(Me.lstDispositivi.SelectedIndex)
        Me.m_Dispositivi.Remove(item)
        Me.lstDispositivi.Items.Remove(item)
        Me.CheckEnabled()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub


    Private Sub lstDispositivi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstDispositivi.SelectedIndexChanged
        Me.CheckEnabled()
    End Sub

    Private Sub CheckEnabled()
        If (Me.m_SelItem IsNot Nothing) Then
            Me.m_SelItem.Nome = Me.txtNome.Text
            Me.m_SelItem.Tipo = Me.cboTipo.Text
            Me.m_SelItem.Indirizzo = Me.txtIP.Text
        End If

        If (Me.lstDispositivi.SelectedIndex >= 0) Then
            Me.m_SelItem = Me.lstDispositivi.Items(Me.lstDispositivi.SelectedIndex)
        Else
            Me.m_SelItem = Nothing
        End If

        If (Me.m_SelItem IsNot Nothing) Then
            Me.txtIP.Text = Me.m_SelItem.Indirizzo
            Me.txtNome.Text = Me.m_SelItem.Nome
            Me.cboTipo.Text = Me.m_SelItem.Tipo
            Me.txtIP.Enabled = True
            Me.txtNome.Enabled = True
            Me.cboTipo.Enabled = True
            'Me.cboTipo.Focus()
            Me.btnDelete.Enabled = True
        Else
            Me.txtIP.Enabled = False
            Me.txtNome.Enabled = False
            Me.cboTipo.Enabled = False
            Me.btnDelete.Enabled = False
        End If
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            Me.CheckEnabled()
            DialTPApp.CurrentConfig.Dispositivi.Clear()
            Me.m_Dispositivi.Sort()
            For Each d As DispositivoEsterno In Me.m_Dispositivi
                DialTPApp.CurrentConfig.Dispositivi.Add(d)
            Next
            DialTPApp.SetConfiguration(DialTPApp.CurrentConfig)
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub



End Class