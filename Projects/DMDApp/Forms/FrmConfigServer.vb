Public Class FrmConfigServer
    Inherits System.Windows.Forms.Form

    Public Sub New()

        ' La chiamata è richiesta dalla finestra di progettazione.
        InitializeComponent()

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().

    End Sub

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Friend WithEvents lblServerName As Label
    Friend WithEvents txtServerName As TextBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnOk As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtProxyAddress As TextBox
    Friend WithEvents txtProxyPort As TextBox
    Friend WithEvents Label2 As Label

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblServerName = New System.Windows.Forms.Label()
        Me.txtServerName = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtProxyAddress = New System.Windows.Forms.TextBox()
        Me.txtProxyPort = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblServerName
        '
        Me.lblServerName.AutoSize = True
        Me.lblServerName.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServerName.Location = New System.Drawing.Point(12, 18)
        Me.lblServerName.Name = "lblServerName"
        Me.lblServerName.Size = New System.Drawing.Size(55, 18)
        Me.lblServerName.TabIndex = 15
        Me.lblServerName.Text = "Server:"
        '
        'txtServerName
        '
        Me.txtServerName.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtServerName.Location = New System.Drawing.Point(73, 15)
        Me.txtServerName.Name = "txtServerName"
        Me.txtServerName.Size = New System.Drawing.Size(357, 24)
        Me.txtServerName.TabIndex = 14
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Controls.Add(Me.btnOk)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 96)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(444, 29)
        Me.Panel1.TabIndex = 13
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(366, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "&Annulla"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.Location = New System.Drawing.Point(291, 3)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 10
        Me.btnOk.Text = "&Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(13, 53)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 18)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Proxy:"
        '
        'txtProxyAddress
        '
        Me.txtProxyAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProxyAddress.Location = New System.Drawing.Point(74, 50)
        Me.txtProxyAddress.Name = "txtProxyAddress"
        Me.txtProxyAddress.Size = New System.Drawing.Size(121, 24)
        Me.txtProxyAddress.TabIndex = 16
        '
        'txtProxyPort
        '
        Me.txtProxyPort.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProxyPort.Location = New System.Drawing.Point(213, 50)
        Me.txtProxyPort.Name = "txtProxyPort"
        Me.txtProxyPort.Size = New System.Drawing.Size(70, 24)
        Me.txtProxyPort.TabIndex = 18
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(197, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(12, 18)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = ":"
        '
        'FrmConfigServer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(444, 125)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtProxyPort)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtProxyAddress)
        Me.Controls.Add(Me.lblServerName)
        Me.Controls.Add(Me.txtServerName)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmConfigServer"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Server di Configurazione"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles lblServerName.Click

    End Sub

    Private Sub FrmConfigServer_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Me.txtServerName.Text = DIALTPLib.Settings.ConfigServer
            Me.txtProxyAddress.Text = DIALTPLib.Settings.ProxyAddress
            Me.txtProxyPort.Text = CStr(DIALTPLib.Settings.ProxyPort)
        Catch ex As Exception
            MsgBox(ex.Message)
            DMD.Sistema.Events.NotifyUnhandledException(ex)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
            DMD.Sistema.Events.NotifyUnhandledException(ex)
        End Try
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Try
            DIALTPLib.Settings.ProxyAddress = Me.txtProxyAddress.Text
            DIALTPLib.Settings.ProxyPort = CInt(Me.txtProxyPort.Text)
            DIALTPLib.Settings.ConfigServer = Me.txtServerName.Text
            My.Application.CheckConfiguration()
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            DMD.Sistema.Events.NotifyUnhandledException(ex)
        End Try
    End Sub

    Private Sub txtServerName_TextChanged(sender As Object, e As EventArgs) Handles txtServerName.TextChanged
        'Me.btnOk.Enabled = Trim(Me.txtServerName.te)
    End Sub

    Private Sub txtProxyAddress_TextChanged(sender As Object, e As EventArgs) Handles txtProxyAddress.TextChanged

    End Sub
End Class
