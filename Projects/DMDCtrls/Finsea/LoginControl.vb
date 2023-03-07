Imports System.ComponentModel
Imports FinSeA
Imports FinSeA.Sistema

Public Class LoginControl

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Protected Overrides Sub DestroyHandle()
        If DesignMode = False Then
            RemoveHandler Users.UserLoggedIn, AddressOf UserLoggedIn_Handler
            RemoveHandler Users.UserLoggedOut, AddressOf UserLoggedOut_Handler
        End If
        MyBase.DestroyHandle()
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        If DesignMode = False Then
            AddHandler Users.UserLoggedIn, AddressOf UserLoggedIn_Handler
            AddHandler Users.UserLoggedOut, AddressOf UserLoggedOut_Handler
            Me.CheckUser()
        End If
    End Sub

    Private Sub UserLoggedIn_Handler(ByVal e As UserLoginEventArgs)
        Me.CheckUser()
    End Sub

    Private Sub UserLoggedOut_Handler(ByVal e As UserLogoutEventArgs)
        Me.CheckUser()
    End Sub

    Public Sub CheckUser()
        If DesignMode = True Then
            Me.pnlLoggedIn.Enabled = False
            Me.pnlLoggedIn.Visible = True
            Me.pnlNotLogged.Visible = False
            Me.lblUserName.Text = ""
        ElseIf Users.CurrentUser Is Nothing OrElse Users.CurrentUser.IsLogged = False Then
            Me.pnlLoggedIn.Enabled = False
            Me.pnlLoggedIn.Visible = False
            Me.pnlNotLogged.Visible = True
            Me.lblUserName.Text = ""
        Else
            Me.pnlLoggedIn.Enabled = True
            Me.pnlLoggedIn.Visible = Users.CurrentUser.IsLogged
            Me.pnlNotLogged.Visible = Not Me.pnlLoggedIn.Visible
            Me.lblUserName.Text = CurrentUser.Nominativo
        End If
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        FinSeA.Sistema.Users.LogIn(Me.txtUsername.Text, Me.txtPassword.Text)
    End Sub

    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public ReadOnly Property CurrentUser As FinSeA.Sistema.CUser
        Get
            Return FinSeA.Sistema.Users.CurrentUser
        End Get
    End Property

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        FinSeA.Sistema.Users.LogOut(FinSeA.Sistema.Users.CurrentUser)
    End Sub
End Class
