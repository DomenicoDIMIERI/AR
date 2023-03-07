'Filename    : BTPasswordDlg.vb
'Part of     : Phone Navigator VB.NET example
'Description : Dialog for asking passkey for Bluetooth devices
'Version     : 3.2
'
'This example is only to be used with PC Connectivity API version 3.2.
'Compability ("as is") with future versions is not quaranteed.
'
'Copyright (c) 2005-2207 Nokia Corporation.
'
'This material, including but not limited to documentation and any related
'computer programs, is protected by intellectual property rights of Nokia
'Corporation and/or its licensors.
'All rights are reserved. Reproducing, modifying, translating, or
'distributing any or all of this material requires the prior written consent
'of Nokia Corporation. Nokia Corporation retains the right to make changes
'to this material at any time without notice. A copyright license is hereby
'granted to download and print a copy of this material for personal use only.
'No other license to any other intellectual property rights is granted. The
'material is provided "as is" without warranty of any kind, either express or
'implied, including without limitation, any warranty of non-infringement,
'merchantability and fitness for a particular purpose. In no event shall
'Nokia Corporation be liable for any direct, indirect, special, incidental,
'or consequential loss or damages, including but not limited to, lost profits
'or revenue,loss of use, cost of substitute program, or loss of data or
'equipment arising out of the use or inability to use the material, even if
'Nokia Corporation has been advised of the likelihood of such damages occurring.

Public Class FRM_Password
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents BTN_Cancel As System.Windows.Forms.Button
    Friend WithEvents BTN_OK As System.Windows.Forms.Button
    Friend WithEvents LBL_Password As System.Windows.Forms.Label
    Friend WithEvents TXB_Password As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FRM_Password))
        Me.LBL_Password = New System.Windows.Forms.Label
        Me.TXB_Password = New System.Windows.Forms.TextBox
        Me.BTN_Cancel = New System.Windows.Forms.Button
        Me.BTN_OK = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'LBL_Password
        '
        Me.LBL_Password.Location = New System.Drawing.Point(8, 64)
        Me.LBL_Password.Name = "LBL_Password"
        Me.LBL_Password.Size = New System.Drawing.Size(128, 16)
        Me.LBL_Password.TabIndex = 11
        Me.LBL_Password.Text = "Bluetooth passkey:"
        '
        'TXB_Password
        '
        Me.TXB_Password.Location = New System.Drawing.Point(8, 88)
        Me.TXB_Password.Name = "TXB_Password"
        Me.TXB_Password.Size = New System.Drawing.Size(296, 20)
        Me.TXB_Password.TabIndex = 8
        Me.TXB_Password.Text = "123"
        '
        'BTN_Cancel
        '
        Me.BTN_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BTN_Cancel.Location = New System.Drawing.Point(232, 48)
        Me.BTN_Cancel.Name = "BTN_Cancel"
        Me.BTN_Cancel.Size = New System.Drawing.Size(72, 32)
        Me.BTN_Cancel.TabIndex = 10
        Me.BTN_Cancel.Text = "Cancel"
        '
        'BTN_OK
        '
        Me.BTN_OK.Location = New System.Drawing.Point(232, 8)
        Me.BTN_OK.Name = "BTN_OK"
        Me.BTN_OK.Size = New System.Drawing.Size(72, 32)
        Me.BTN_OK.TabIndex = 9
        Me.BTN_OK.Text = "OK"
        '
        'FRM_Password
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(312, 118)
        Me.Controls.Add(Me.LBL_Password)
        Me.Controls.Add(Me.TXB_Password)
        Me.Controls.Add(Me.BTN_Cancel)
        Me.Controls.Add(Me.BTN_OK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FRM_Password"
        Me.Text = "Bluetooth Passkey"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub BTN_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_OK.Click
        If TXB_Password.Text.Length() <= 0 Then
            MsgBox("Empty passkey not allowed")
        Else
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub BTN_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Cancel.Click
        Me.Close()
    End Sub

    Private Sub TXB_Password_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TXB_Password.TextChanged
        If TXB_Password.Text.Length() > 0 And Not IsNumeric(TXB_Password.Text) Then
            MsgBox("Passkey can contain only numbers")
            Dim i As Integer
            Dim strOut As String = ""
            For i = 0 To TXB_Password.Text.Length - 1
                If IsNumeric(TXB_Password.Text.Substring(i, 1)) Then
                    strOut &= TXB_Password.Text.Substring(i, 1)
                End If
            Next
            TXB_Password.Text = strOut
        End If
    End Sub
End Class
