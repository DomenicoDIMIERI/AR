Imports DMD
Imports DMD.Sistema


Public Class frmDynAR
    Private m_Loading As Boolean

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.PingAll()
    End Sub

    Private Function Ping(ByVal server As String, ByVal ufficio As String) As String
        Try
            server = Strings.Trim(server)
            ufficio = Strings.Trim(ufficio)
            If (server = "") Then Return "NS"
            If (ufficio = "") Then Return "NU"
            Return RPC.InvokeMethod(server & "/widgets/websvcf/?_a=DynDNS", "u", ufficio)
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Private Sub txtPING1_TextChanged(sender As Object, e As EventArgs) Handles txtPING1.TextChanged, txtPING2.TextChanged, txtPING3.TextChanged, txtPING4.TextChanged, txtUfficio1.TextChanged, txtUfficio2.TextChanged, txtUfficio3.TextChanged, txtUfficio4.TextChanged
        Try
            If (m_Loading) Then Return
            My.Settings.Server1 = Strings.Trim(Me.txtPING1.Text)
            My.Settings.Ufficio1 = Strings.Trim(Me.txtUfficio1.Text)
            My.Settings.Server2 = Strings.Trim(Me.txtPING2.Text)
            My.Settings.Ufficio2 = Strings.Trim(Me.txtUfficio2.Text)
            My.Settings.Server3 = Strings.Trim(Me.txtPING3.Text)
            My.Settings.Ufficio3 = Strings.Trim(Me.txtUfficio3.Text)
            My.Settings.Server4 = Strings.Trim(Me.txtPING4.Text)
            My.Settings.Ufficio4 = Strings.Trim(Me.txtUfficio4.Text)
            My.Settings.Save()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub frmDynAR_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Me.m_Loading = True
            Me.txtPING1.Text = My.Settings.Server1
            Me.txtUfficio1.Text = My.Settings.Ufficio1
            Me.txtPING2.Text = My.Settings.Server2
            Me.txtUfficio2.Text = My.Settings.Ufficio2
            Me.txtPING3.Text = My.Settings.Server3
            Me.txtUfficio3.Text = My.Settings.Ufficio3
            Me.txtPING4.Text = My.Settings.Server4
            Me.txtUfficio4.Text = My.Settings.Ufficio4
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Me.m_Loading = False
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Me.PingAll()
    End Sub

    Public Sub PingAll()
        Me.lblError1.Text = Me.Ping(Me.txtPING1.Text, Me.txtUfficio1.Text)
        Me.lblError2.Text = Me.Ping(Me.txtPING2.Text, Me.txtUfficio2.Text)
        Me.lblError3.Text = Me.Ping(Me.txtPING3.Text, Me.txtUfficio3.Text)
        Me.lblError4.Text = Me.Ping(Me.txtPING4.Text, Me.txtUfficio4.Text)
    End Sub
End Class
