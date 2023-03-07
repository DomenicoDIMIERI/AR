Imports DMD
Imports DMD.S300

Public Class frmPersonInfo

    Private m_User As S300PersonInfo

    Public Property User As S300PersonInfo
        Get
            Return Me.m_User
        End Get
        Set(value As S300PersonInfo)
            Me.m_User = value
            Me.Refill
        End Set
    End Property


    Public Sub Refill()
        Me.txtID.Value = 0
        Me.txtNome.Text = ""
        Me.txtPassword.Text = ""
        If (Me.m_User Is Nothing) Then Return
        Me.txtID.Value = Me.m_User.PersonID
        Me.txtNome.Text = Me.m_User.Name
        Me.txtPassword.Text = Me.m_User.Password
        Me.txtCardNo.Value = Me.m_User.CardNo
        Me.cboUserType.SelectedIndex = CInt(Me.m_User.UserType)
        Me.txtGroup.Value = Me.m_User.Group
        Me.txtDepartment.Value = Me.m_User.Department
    End Sub

    Public Sub Apply()
        'Me.m_User.PersonID = Me.txtID.Value
        Me.m_User.Name = Me.txtNome.Text
        Me.m_User.Password = Me.txtPassword.Text
        Me.m_User.CardNo = Me.txtCardNo.Value
        Me.m_User.UserType = CType(Me.cboUserType.SelectedIndex, S300UserType)
        Me.m_User.Group = Me.txtGroup.Value
        Me.m_User.Department = Me.txtDepartment.Value
        Me.m_User.PersonID = Me.txtID.Value
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            Me.Apply
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
End Class