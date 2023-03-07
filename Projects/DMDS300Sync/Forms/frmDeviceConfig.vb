Public Class frmDeviceConfig

    Private m_Item As ANVIZS300DeviceConfig = Nothing

    <System.ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public Property Item As ANVIZS300DeviceConfig
        Get
            Return Me.m_Item
        End Get
        Set(value As ANVIZS300DeviceConfig)
            If (Me.m_Item Is value) Then Return
            Me.m_Item = value
            Me.Refill
        End Set
    End Property

    Public Sub Refill()
        Me.txtNome.Text = Me.m_Item.Nome
        Me.txtDeviceID.Value = Me.m_Item.DeviceID
        Me.txtAddress.Text = Me.m_Item.Indirizzo
        Me.txtSyncTimeInterval.Value = Me.m_Item.SincronizzaOgni
        Me.txtUserMappings.Text = Me.m_Item.Mapping
    End Sub

    Public Sub Save()
        Me.m_Item.Nome = Me.txtNome.Text
        Me.m_Item.DeviceID = Me.txtDeviceID.Value
        Me.m_Item.Indirizzo = Me.txtAddress.Text
        Me.m_Item.SincronizzaOgni = Me.txtSyncTimeInterval.Value
        Me.m_Item.Mapping = Me.txtUserMappings.Text
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim frm As New frmUtenti
        frm.Device = Me.Item.GetDevice
        frm.ShowDialog()
    End Sub



    Private Sub btnSync_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim col As CCollection(Of DMD.Office.MarcaturaIngressoUscita) = Me.Item.ScaricaNuoveMarcature
            Me.Log("Ho scaricato " & col.Count & " nuove marcature")
        Catch ex As Exception
            Me.Log("Errore nello scaricare le nuove marcature: " & ex.Message)
        End Try
    End Sub

    Public Sub Log(ByVal text As String)
        Sistema.ApplicationContext.Log(text)
    End Sub

    Private Sub btnDelMarcature_Click(sender As Object, e As EventArgs) Handles btnDelMarcature.Click
        Try
            Dim dev As S300.S300Device = Me.Item.GetDevice
            If (Not dev.IsConnected) Then dev.Start()
            dev.DeleteFirstNClockings(1000)
            Me.Log("Ho eliminato le prime 1000 marcature dal dispositivo " & dev.DeviceID & " : " & dev.GetDeviceTime().ToString)
            dev.Stop()
        Catch ex As Exception
            Me.Log("Errore nell'eliminazione delle marcature dal dispositivo: " & ex.Message & vbNewLine)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Item.SincronizzaOrologio()
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Me.Save()
        Me.Close()
    End Sub
End Class