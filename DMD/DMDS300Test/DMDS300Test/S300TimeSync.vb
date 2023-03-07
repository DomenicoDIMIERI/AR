Imports DMD.S300

Public Class S300TimeSync

    Private m_Dev As S300Device
    Private m_Editing As Boolean

    Public Sub New()
        ' La chiamata è richiesta dalla finestra di progettazione.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().

    End Sub

    Public Property Device As S300Device
        Get
            Return Me.m_Dev
        End Get
        Set(value As S300Device)
            Me.m_Dev = value
            Me.Timer1.Enabled = Me.m_Dev IsNot Nothing
            If (Me.m_Dev IsNot Nothing) Then Me.txtOraDispositivo.Text = Me.m_Dev.GetDeviceTime
        End Set
    End Property

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.txtOraLocale.Text = formatData(Now)
        If (Me.m_Dev IsNot Nothing AndAlso Not Me.m_Editing) Then
            Me.txtOraDispositivo.Text = formatData(Me.m_Dev.GetDeviceTime)
        End If
    End Sub

    Private Function formatData(ByVal value As Date) As String
        Return value.ToString("dd/MM/yyyy HH:mm:ss")
    End Function

    Private Sub btnSet_Click(sender As Object, e As EventArgs) Handles btnSet.Click
        Try
            Me.m_Dev.SetDeviceTime(CDate(Me.txtOraDispositivo.Text))
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub txtOraDispositivo_TextChanged(sender As Object, e As EventArgs) Handles txtOraDispositivo.TextChanged

    End Sub

    Private Sub txtOraDispositivo_GotFocus(sender As Object, e As EventArgs) Handles txtOraDispositivo.GotFocus
        Me.m_Editing = True
    End Sub

    Private Sub txtOraDispositivo_LostFocus(sender As Object, e As EventArgs) Handles txtOraDispositivo.LostFocus
        Me.m_Editing = False
    End Sub

    Private Sub btnSync_Click(sender As Object, e As EventArgs) Handles btnSync.Click
        Try
            Me.m_Dev.SetDeviceTime(Now)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
End Class
