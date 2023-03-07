'Filename    : InstallerDialog.vb
'Part of     : Application installer VB.NET example
'Description : Main dialog of VBInstaller.NET example application
'Version     : 3.2
'
'This example is only to be used with PC Connectivity API version 3.2.
'Compability ("as is") with future versions is not quaranteed.
'
'Copyright (c) 2007 Nokia Corporation.
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

Option Strict Off
Option Explicit On

Imports System.Runtime.InteropServices

Friend Class InstallerDialog
    Inherits System.Windows.Forms.Form
#Region "Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()
        If m_vb6FormDefInstance Is Nothing Then
            If m_InitializingDefInstance Then
                m_vb6FormDefInstance = Me
            Else
                Try
                    'For the start-up form, the first instance created is the default instance.
                    If System.Reflection.Assembly.GetExecutingAssembly.EntryPoint.DeclaringType Is Me.GetType Then
                        m_vb6FormDefInstance = Me
                    End If
                Catch
                End Try
            End If
        End If
        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            Static fTerminateCalled As Boolean
            If Not fTerminateCalled Then
                fTerminateCalled = True
            End If
            If Not components Is Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents TextSis As System.Windows.Forms.TextBox
    Public WithEvents TextJad As System.Windows.Forms.TextBox
    Public WithEvents TextJar As System.Windows.Forms.TextBox
    Public WithEvents CommandJad As System.Windows.Forms.Button
    Public WithEvents CommandSis As System.Windows.Forms.Button
    Public WithEvents CommandJar As System.Windows.Forms.Button
    Public WithEvents CommandCancel As System.Windows.Forms.Button
    Public WithEvents CommandInstall As System.Windows.Forms.Button
    Public WithEvents ComboType As System.Windows.Forms.ComboBox
    Public WithEvents ComboPhone As System.Windows.Forms.ComboBox
    Public WithEvents LabelSis As System.Windows.Forms.Label
    Public WithEvents LabelJad As System.Windows.Forms.Label
    Public WithEvents LabelJar As System.Windows.Forms.Label
    Public WithEvents LabelType As System.Windows.Forms.Label
    Public WithEvents LabelPhone As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents labelWait As System.Windows.Forms.Label
    Public WithEvents TextNth As System.Windows.Forms.TextBox
    Public WithEvents CommandNth As System.Windows.Forms.Button
    Friend WithEvents CommandList As System.Windows.Forms.Button
    Public WithEvents TextNGage As System.Windows.Forms.TextBox
    Public WithEvents CommandNGage As System.Windows.Forms.Button
    Public WithEvents NGageLabel As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Public WithEvents LabelNth As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InstallerDialog))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.CommandList = New System.Windows.Forms.Button()
        Me.TextSis = New System.Windows.Forms.TextBox()
        Me.TextJad = New System.Windows.Forms.TextBox()
        Me.TextJar = New System.Windows.Forms.TextBox()
        Me.CommandJad = New System.Windows.Forms.Button()
        Me.CommandSis = New System.Windows.Forms.Button()
        Me.CommandJar = New System.Windows.Forms.Button()
        Me.CommandCancel = New System.Windows.Forms.Button()
        Me.CommandInstall = New System.Windows.Forms.Button()
        Me.ComboType = New System.Windows.Forms.ComboBox()
        Me.ComboPhone = New System.Windows.Forms.ComboBox()
        Me.LabelSis = New System.Windows.Forms.Label()
        Me.LabelJad = New System.Windows.Forms.Label()
        Me.LabelJar = New System.Windows.Forms.Label()
        Me.LabelType = New System.Windows.Forms.Label()
        Me.LabelPhone = New System.Windows.Forms.Label()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.labelWait = New System.Windows.Forms.Label()
        Me.TextNth = New System.Windows.Forms.TextBox()
        Me.CommandNth = New System.Windows.Forms.Button()
        Me.LabelNth = New System.Windows.Forms.Label()
        Me.TextNGage = New System.Windows.Forms.TextBox()
        Me.CommandNGage = New System.Windows.Forms.Button()
        Me.NGageLabel = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'CommandList
        '
        Me.CommandList.Location = New System.Drawing.Point(343, 8)
        Me.CommandList.Name = "CommandList"
        Me.CommandList.Size = New System.Drawing.Size(75, 25)
        Me.CommandList.TabIndex = 24
        Me.CommandList.Text = "List..."
        Me.ToolTip1.SetToolTip(Me.CommandList, "Lists installed applications")
        Me.CommandList.UseVisualStyleBackColor = True
        '
        'TextSis
        '
        Me.TextSis.AcceptsReturn = True
        Me.TextSis.BackColor = System.Drawing.SystemColors.Window
        Me.TextSis.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextSis.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSis.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextSis.Location = New System.Drawing.Point(96, 144)
        Me.TextSis.MaxLength = 0
        Me.TextSis.Name = "TextSis"
        Me.TextSis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextSis.Size = New System.Drawing.Size(313, 20)
        Me.TextSis.TabIndex = 13
        '
        'TextJad
        '
        Me.TextJad.AcceptsReturn = True
        Me.TextJad.BackColor = System.Drawing.SystemColors.Window
        Me.TextJad.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextJad.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextJad.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextJad.Location = New System.Drawing.Point(96, 112)
        Me.TextJad.MaxLength = 0
        Me.TextJad.Name = "TextJad"
        Me.TextJad.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextJad.Size = New System.Drawing.Size(313, 20)
        Me.TextJad.TabIndex = 11
        '
        'TextJar
        '
        Me.TextJar.AcceptsReturn = True
        Me.TextJar.BackColor = System.Drawing.SystemColors.Window
        Me.TextJar.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextJar.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextJar.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextJar.Location = New System.Drawing.Point(96, 80)
        Me.TextJar.MaxLength = 0
        Me.TextJar.Name = "TextJar"
        Me.TextJar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextJar.Size = New System.Drawing.Size(313, 20)
        Me.TextJar.TabIndex = 9
        '
        'CommandJad
        '
        Me.CommandJad.BackColor = System.Drawing.SystemColors.Control
        Me.CommandJad.Cursor = System.Windows.Forms.Cursors.Default
        Me.CommandJad.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CommandJad.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CommandJad.Location = New System.Drawing.Point(424, 112)
        Me.CommandJad.Name = "CommandJad"
        Me.CommandJad.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CommandJad.Size = New System.Drawing.Size(73, 25)
        Me.CommandJad.TabIndex = 8
        Me.CommandJad.Text = "Browse..."
        Me.CommandJad.UseVisualStyleBackColor = False
        '
        'CommandSis
        '
        Me.CommandSis.BackColor = System.Drawing.SystemColors.Control
        Me.CommandSis.Cursor = System.Windows.Forms.Cursors.Default
        Me.CommandSis.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CommandSis.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CommandSis.Location = New System.Drawing.Point(424, 144)
        Me.CommandSis.Name = "CommandSis"
        Me.CommandSis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CommandSis.Size = New System.Drawing.Size(73, 25)
        Me.CommandSis.TabIndex = 7
        Me.CommandSis.Text = "Browse..."
        Me.CommandSis.UseVisualStyleBackColor = False
        '
        'CommandJar
        '
        Me.CommandJar.BackColor = System.Drawing.SystemColors.Control
        Me.CommandJar.Cursor = System.Windows.Forms.Cursors.Default
        Me.CommandJar.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CommandJar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CommandJar.Location = New System.Drawing.Point(424, 80)
        Me.CommandJar.Name = "CommandJar"
        Me.CommandJar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CommandJar.Size = New System.Drawing.Size(73, 25)
        Me.CommandJar.TabIndex = 6
        Me.CommandJar.Text = "Browse..."
        Me.CommandJar.UseVisualStyleBackColor = False
        '
        'CommandCancel
        '
        Me.CommandCancel.BackColor = System.Drawing.SystemColors.Control
        Me.CommandCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.CommandCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CommandCancel.Enabled = False
        Me.CommandCancel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CommandCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CommandCancel.Location = New System.Drawing.Point(424, 40)
        Me.CommandCancel.Name = "CommandCancel"
        Me.CommandCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CommandCancel.Size = New System.Drawing.Size(73, 25)
        Me.CommandCancel.TabIndex = 5
        Me.CommandCancel.Text = "Cancel"
        Me.CommandCancel.UseVisualStyleBackColor = False
        '
        'CommandInstall
        '
        Me.CommandInstall.BackColor = System.Drawing.SystemColors.Control
        Me.CommandInstall.Cursor = System.Windows.Forms.Cursors.Default
        Me.CommandInstall.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CommandInstall.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CommandInstall.Location = New System.Drawing.Point(424, 8)
        Me.CommandInstall.Name = "CommandInstall"
        Me.CommandInstall.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CommandInstall.Size = New System.Drawing.Size(73, 25)
        Me.CommandInstall.TabIndex = 4
        Me.CommandInstall.Text = "Install"
        Me.CommandInstall.UseVisualStyleBackColor = False
        '
        'ComboType
        '
        Me.ComboType.BackColor = System.Drawing.SystemColors.Window
        Me.ComboType.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboType.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboType.Location = New System.Drawing.Point(96, 40)
        Me.ComboType.Name = "ComboType"
        Me.ComboType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboType.Size = New System.Drawing.Size(209, 22)
        Me.ComboType.TabIndex = 2
        '
        'ComboPhone
        '
        Me.ComboPhone.BackColor = System.Drawing.SystemColors.Window
        Me.ComboPhone.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboPhone.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboPhone.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboPhone.Location = New System.Drawing.Point(96, 8)
        Me.ComboPhone.Name = "ComboPhone"
        Me.ComboPhone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboPhone.Size = New System.Drawing.Size(209, 22)
        Me.ComboPhone.TabIndex = 0
        '
        'LabelSis
        '
        Me.LabelSis.BackColor = System.Drawing.SystemColors.Control
        Me.LabelSis.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelSis.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelSis.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelSis.Location = New System.Drawing.Point(8, 148)
        Me.LabelSis.Name = "LabelSis"
        Me.LabelSis.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelSis.Size = New System.Drawing.Size(81, 17)
        Me.LabelSis.TabIndex = 14
        Me.LabelSis.Text = "SIS File:"
        Me.LabelSis.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'LabelJad
        '
        Me.LabelJad.BackColor = System.Drawing.SystemColors.Control
        Me.LabelJad.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelJad.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelJad.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelJad.Location = New System.Drawing.Point(8, 116)
        Me.LabelJad.Name = "LabelJad"
        Me.LabelJad.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelJad.Size = New System.Drawing.Size(81, 17)
        Me.LabelJad.TabIndex = 12
        Me.LabelJad.Text = "JAD File:"
        Me.LabelJad.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'LabelJar
        '
        Me.LabelJar.BackColor = System.Drawing.SystemColors.Control
        Me.LabelJar.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelJar.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelJar.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelJar.Location = New System.Drawing.Point(8, 84)
        Me.LabelJar.Name = "LabelJar"
        Me.LabelJar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelJar.Size = New System.Drawing.Size(81, 17)
        Me.LabelJar.TabIndex = 10
        Me.LabelJar.Text = "JAR File:"
        Me.LabelJar.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'LabelType
        '
        Me.LabelType.BackColor = System.Drawing.SystemColors.Control
        Me.LabelType.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelType.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelType.Location = New System.Drawing.Point(1, 44)
        Me.LabelType.Name = "LabelType"
        Me.LabelType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelType.Size = New System.Drawing.Size(88, 17)
        Me.LabelType.TabIndex = 3
        Me.LabelType.Text = "Installation type:"
        Me.LabelType.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'LabelPhone
        '
        Me.LabelPhone.BackColor = System.Drawing.SystemColors.Control
        Me.LabelPhone.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelPhone.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelPhone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelPhone.Location = New System.Drawing.Point(48, 12)
        Me.LabelPhone.Name = "LabelPhone"
        Me.LabelPhone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelPhone.Size = New System.Drawing.Size(41, 17)
        Me.LabelPhone.TabIndex = 1
        Me.LabelPhone.Text = "Phone:"
        Me.LabelPhone.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(9, 245)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(488, 19)
        Me.ProgressBar1.TabIndex = 16
        Me.ProgressBar1.Visible = False
        '
        'labelWait
        '
        Me.labelWait.Location = New System.Drawing.Point(11, 239)
        Me.labelWait.Name = "labelWait"
        Me.labelWait.Size = New System.Drawing.Size(486, 22)
        Me.labelWait.TabIndex = 17
        Me.labelWait.Text = "Waiting for user actions on the device side"
        Me.labelWait.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.labelWait.Visible = False
        '
        'TextNth
        '
        Me.TextNth.AcceptsReturn = True
        Me.TextNth.BackColor = System.Drawing.SystemColors.Window
        Me.TextNth.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextNth.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNth.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextNth.Location = New System.Drawing.Point(96, 176)
        Me.TextNth.MaxLength = 0
        Me.TextNth.Name = "TextNth"
        Me.TextNth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextNth.Size = New System.Drawing.Size(313, 20)
        Me.TextNth.TabIndex = 19
        '
        'CommandNth
        '
        Me.CommandNth.BackColor = System.Drawing.SystemColors.Control
        Me.CommandNth.Cursor = System.Windows.Forms.Cursors.Default
        Me.CommandNth.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CommandNth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CommandNth.Location = New System.Drawing.Point(424, 176)
        Me.CommandNth.Name = "CommandNth"
        Me.CommandNth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CommandNth.Size = New System.Drawing.Size(73, 25)
        Me.CommandNth.TabIndex = 18
        Me.CommandNth.Text = "Browse..."
        Me.CommandNth.UseVisualStyleBackColor = False
        '
        'LabelNth
        '
        Me.LabelNth.BackColor = System.Drawing.SystemColors.Control
        Me.LabelNth.Cursor = System.Windows.Forms.Cursors.Default
        Me.LabelNth.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelNth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelNth.Location = New System.Drawing.Point(8, 180)
        Me.LabelNth.Name = "LabelNth"
        Me.LabelNth.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LabelNth.Size = New System.Drawing.Size(81, 17)
        Me.LabelNth.TabIndex = 20
        Me.LabelNth.Text = "NTH File:"
        Me.LabelNth.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'TextNGage
        '
        Me.TextNGage.AcceptsReturn = True
        Me.TextNGage.BackColor = System.Drawing.SystemColors.Window
        Me.TextNGage.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextNGage.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNGage.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextNGage.Location = New System.Drawing.Point(96, 206)
        Me.TextNGage.MaxLength = 0
        Me.TextNGage.Name = "TextNGage"
        Me.TextNGage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextNGage.Size = New System.Drawing.Size(313, 20)
        Me.TextNGage.TabIndex = 26
        '
        'CommandNGage
        '
        Me.CommandNGage.BackColor = System.Drawing.SystemColors.Control
        Me.CommandNGage.Cursor = System.Windows.Forms.Cursors.Default
        Me.CommandNGage.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CommandNGage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CommandNGage.Location = New System.Drawing.Point(424, 206)
        Me.CommandNGage.Name = "CommandNGage"
        Me.CommandNGage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CommandNGage.Size = New System.Drawing.Size(73, 25)
        Me.CommandNGage.TabIndex = 25
        Me.CommandNGage.Text = "Browse..."
        Me.CommandNGage.UseVisualStyleBackColor = False
        '
        'NGageLabel
        '
        Me.NGageLabel.BackColor = System.Drawing.SystemColors.Control
        Me.NGageLabel.Cursor = System.Windows.Forms.Cursors.Default
        Me.NGageLabel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NGageLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.NGageLabel.Location = New System.Drawing.Point(8, 210)
        Me.NGageLabel.Name = "NGageLabel"
        Me.NGageLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.NGageLabel.Size = New System.Drawing.Size(81, 17)
        Me.NGageLabel.TabIndex = 27
        Me.NGageLabel.Text = "N-Gage File:"
        Me.NGageLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Timer1
        '
        '
        'InstallerDialog
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.CommandCancel
        Me.ClientSize = New System.Drawing.Size(506, 272)
        Me.Controls.Add(Me.TextNGage)
        Me.Controls.Add(Me.CommandNGage)
        Me.Controls.Add(Me.NGageLabel)
        Me.Controls.Add(Me.CommandList)
        Me.Controls.Add(Me.TextNth)
        Me.Controls.Add(Me.CommandNth)
        Me.Controls.Add(Me.LabelNth)
        Me.Controls.Add(Me.labelWait)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.TextSis)
        Me.Controls.Add(Me.TextJad)
        Me.Controls.Add(Me.TextJar)
        Me.Controls.Add(Me.CommandJad)
        Me.Controls.Add(Me.CommandSis)
        Me.Controls.Add(Me.CommandJar)
        Me.Controls.Add(Me.CommandCancel)
        Me.Controls.Add(Me.CommandInstall)
        Me.Controls.Add(Me.ComboType)
        Me.Controls.Add(Me.ComboPhone)
        Me.Controls.Add(Me.LabelSis)
        Me.Controls.Add(Me.LabelJad)
        Me.Controls.Add(Me.LabelJar)
        Me.Controls.Add(Me.LabelType)
        Me.Controls.Add(Me.LabelPhone)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(4, 30)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "InstallerDialog"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Application Installer"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region
#Region "Upgrade Support "
    Private Shared m_vb6FormDefInstance As InstallerDialog
    Private Shared m_InitializingDefInstance As Boolean
    Public Shared Property DefInstance() As InstallerDialog
        Get
            If m_vb6FormDefInstance Is Nothing OrElse m_vb6FormDefInstance.IsDisposed Then
                m_InitializingDefInstance = True
                m_vb6FormDefInstance = New InstallerDialog
                m_InitializingDefInstance = False
            End If
            DefInstance = m_vb6FormDefInstance
        End Get
        Set(ByVal Value As InstallerDialog)
            m_vb6FormDefInstance = Value
        End Set
    End Property
#End Region

    ' Definitions for installation types, NOTE order is significant
    Const INSTALL_TYPE_JAVA As Integer = 0
    Const INSTALL_TYPE_NGAGE As Integer = 1
    Const INSTALL_TYPE_SYMBIAN As Integer = 2
    Const INSTALL_TYPE_THEMES As Integer = 3
    Const strJavaItem As String = "Java"
    Const strSymbianItem As String = "Symbian"
    Const strThemesItem As String = "Themes"
    Const strNGageItem As String = "N-Gage"
    Dim bPhoneSupportsSisX As Boolean = False
    Public bRefreshPhonecombo As Boolean = False

    Public bCancelled As Boolean
    Dim iInstallationType As Integer

    <STAThread()>
    Shared Sub Main()
        ' Starts the application.
        Application.Run(New InstallerDialog)
    End Sub

    '===================================================================
    ' RefreshPhoneList
    '
    ' Refresh phone list to combo
    '
    '===================================================================
    Public Sub RefreshPhoneList()
        Me.ComboPhone.Items.Clear()
        If DMD.Nokia.Devices.Count > 0 Then
            For Each dev As DMD.Nokia.NokiaDevice In DMD.Nokia.Devices
                Me.ComboPhone.Items.Add(dev)
            Next
            Me.ComboPhone.Enabled = True
            Me.ComboPhone.SelectedIndex = 0
        Else
            Me.ComboPhone.Text = "No phones connected"
            Me.ComboPhone.Enabled = False
            Me.ComboPhone.SelectedIndex = -1
        End If
        Timer1.Enabled = True
    End Sub

    '===================================================================
    ' TypeSelectionChanged
    '
    ' User has changed installation type from combobox,
    ' so make some UI changes
    '
    '===================================================================
    Private Sub TypeSelectionChanged()
        TextJar.Text = ""
        TextJad.Text = ""
        TextSis.Text = ""
        TextNth.Text = ""
        TextJad.Enabled = False
        TextJar.Enabled = False
        TextSis.Enabled = False
        TextNth.Enabled = False
        TextNGage.Enabled = False
        CommandJar.Enabled = False
        CommandJad.Enabled = False
        CommandSis.Enabled = False
        CommandNth.Enabled = False
        CommandNGage.Enabled = False
        Dim strItem As String = ComboType.SelectedItem.ToString
        If strItem = strJavaItem Then
            iInstallationType = INSTALL_TYPE_JAVA
        ElseIf strItem = strSymbianItem Then
            iInstallationType = INSTALL_TYPE_SYMBIAN
        ElseIf strItem = strThemesItem Then
            iInstallationType = INSTALL_TYPE_THEMES
        ElseIf strItem = strNGageItem Then
            iInstallationType = INSTALL_TYPE_NGAGE
        End If
        If iInstallationType = INSTALL_TYPE_JAVA Then
            TextJad.Enabled = True
            TextJar.Enabled = True
            CommandJar.Enabled = True
            CommandJad.Enabled = True
        ElseIf iInstallationType = INSTALL_TYPE_NGAGE Then
            TextNGage.Enabled = True
            CommandNGage.Enabled = True
        ElseIf iInstallationType = INSTALL_TYPE_SYMBIAN Then
            TextSis.Enabled = True
            CommandSis.Enabled = True
        ElseIf iInstallationType = INSTALL_TYPE_THEMES Then
            TextNth.Enabled = True
            CommandNth.Enabled = True
        End If
    End Sub

    '===================================================================
    ' SetProgress
    '
    ' Sets progress bar state
    '
    '===================================================================
    Public Sub SetProgress(ByVal iState As Integer)
        ProgressBar1.Value = iState
    End Sub

    '===================================================================
    ' IsCancelled
    '
    ' Returns true if user has clicked Cancel button
    '
    '===================================================================
    Public Function IsCancelled() As Boolean
        Application.DoEvents()
        IsCancelled = bCancelled
        bCancelled = False
    End Function

     

    '===================================================================
    ' InstallerDialog_Load
    '
    ' Initialization of InstallerDialog form
    '
    '===================================================================
    Private Sub InstallerDialog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Dim col1 As MSComctlLib.ColumnHeader
        ' Dim col2 As MSComctlLib.ColumnHeader
        ' Dim col3 As MSComctlLib.ColumnHeader

        bCancelled = False
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = 100
        ProgressBar1.Value = 0
          
        ' Get connected devices list
        Timer1.Enabled = True
        Timer1.Start()
        'Refresh phonelist
        bRefreshPhonecombo = True
    End Sub

    '===================================================================
    ' ComboType_SelectedIndexChanged
    '
    ' User has changed installation type from combobox
    '
    '===================================================================
    Private Sub ComboType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboType.SelectedIndexChanged
        TypeSelectionChanged()
    End Sub

    '=========================================================
    ' CommandJar_Click 
    '
    ' Opens file open dialog for selecting a file
    '
    '=========================================================
    Private Sub CommandJar_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CommandJar.Click
        Dim openFileDialog As OpenFileDialog
        openFileDialog = New OpenFileDialog
        openFileDialog.Filter = "Jar files (*.jar)|*.jar"
        openFileDialog.FilterIndex = 0
        openFileDialog.RestoreDirectory = True
        If openFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextJar.Text = openFileDialog.FileName
        End If
    End Sub

    '=========================================================
    ' CommandJad_Click 
    '
    ' Opens file open dialog for selecting a file
    '
    '=========================================================
    Private Sub CommandJad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CommandJad.Click
        Dim openFileDialog As OpenFileDialog
        openFileDialog = New OpenFileDialog
        openFileDialog.Filter = "Jad files (*.jad)|*.jad"
        openFileDialog.FilterIndex = 0
        openFileDialog.RestoreDirectory = True
        If openFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextJad.Text = openFileDialog.FileName
        End If
    End Sub

    '=========================================================
    ' CommandSis_Click 
    '
    ' Opens file open dialog for selecting a file
    '
    '=========================================================
    Private Sub CommandSis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CommandSis.Click
        Dim openFileDialog As OpenFileDialog
        openFileDialog = New OpenFileDialog
        If bPhoneSupportsSisX Then
            openFileDialog.Filter = "Sis files (*.sis)|*.sis|Sisx files (*.sisx)|*.sisx"
        Else
            openFileDialog.Filter = "Sis files (*.sis)|*.sis"
        End If
        openFileDialog.FilterIndex = 0
        openFileDialog.RestoreDirectory = True
        If openFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextSis.Text = openFileDialog.FileName
        End If
    End Sub

    '=========================================================
    ' CommandNGage_Click 
    '
    ' Opens file open dialog for selecting a file
    '
    '=========================================================
    Private Sub CommandNGage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CommandNGage.Click
        Dim openFileDialog As OpenFileDialog
        openFileDialog = New OpenFileDialog
        openFileDialog.Filter = "N-Gage files (*.n-gage)|*.n-gage"
        openFileDialog.FilterIndex = 0
        openFileDialog.RestoreDirectory = True
        If openFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            TextNGage.Text = openFileDialog.FileName
        End If
    End Sub

    '=========================================================
    ' CommandNth_Click 
    '
    ' Opens file open dialog for selecting a file
    '
    '=========================================================
    Private Sub CommandNth_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CommandNth.Click
        Dim openFileDialog As OpenFileDialog
        openFileDialog = New OpenFileDialog
        openFileDialog.Filter = "Nth files (*.nth)|*.nth"
        openFileDialog.FilterIndex = 0
        openFileDialog.RestoreDirectory = True
        If openFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            TextNth.Text = openFileDialog.FileName
        End If
    End Sub

    '=========================================================
    ' CommandInstall_Click
    '
    ' Install required files to device
    '
    '=========================================================
    Private Sub CommandInstall_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CommandInstall.Click
        Dim dev As DMD.Nokia.NokiaDevice = Me.GetCurrentDevice
        If dev Is Nothing Then Exit Sub

        CommandInstall.Enabled = False
        CommandCancel.Enabled = True
        labelWait.Visible = False
        ProgressBar1.Visible = True
        Cursor = Cursors.WaitCursor

        If iInstallationType = INSTALL_TYPE_JAVA Then
            ' Installing Java application
            dev.InstalledApplications.InstallJavaApplication(TextJar.Text, TextJad.Text)
        ElseIf iInstallationType = INSTALL_TYPE_NGAGE Then
            ' Installing N-gage application 
            dev.InstalledApplications.InstallNGageApplication(TextNGage.Text)
        ElseIf iInstallationType = INSTALL_TYPE_SYMBIAN Then
            ' Installing Symbian application
            dev.InstalledApplications.InstallSymbianApplication(TextSis.Text)
        ElseIf iInstallationType = INSTALL_TYPE_THEMES Then
            ' Installing theme
            dev.InstalledApplications.InstallTheme(TextNth.Text)
        End If
        Me.Cursor = Cursors.Default
        CommandInstall.Enabled = True
        CommandCancel.Enabled = False
        labelWait.Visible = False
        ProgressBar1.Value = 0
        ProgressBar1.Visible = False
    End Sub

    '===================================================================
    ' CommandCancel_Click
    '
    ' User has clicked Cancel button to cancel application installation
    '===================================================================
    Private Sub CommandCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CommandCancel.Click
        bCancelled = True
        Cursor = Cursors.Default
        CommandInstall.Enabled = True
        CommandCancel.Enabled = False
        ProgressBar1.Visible = False
    End Sub

    Public Function GetCurrentDevice() As DMD.Nokia.NokiaDevice
        If Me.ComboPhone.SelectedIndex < 0 Then Return Nothing
        Return Me.ComboPhone.SelectedItem
    End Function

    '===================================================================
    ' CommandList_Click
    '
    ' User has clicked List button to list installed applications 
    '===================================================================
    Private Sub CommandList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CommandList.Click
        Dim dev As DMD.Nokia.NokiaDevice = Me.GetCurrentDevice
        ' Find out phone selected
        If dev Is Nothing Then
            ' No phone selected, inform user
            MessageBox.Show(Me, "No phone selected", "List phone applications")
            Exit Sub
        End If

        ' Phone is selected
        Dim listDlg As New ListDialog
        listDlg.SetDevice(dev)
         
        ' Clean up
        Me.Cursor = Cursors.Default
        CommandInstall.Enabled = True
        CommandList.Enabled = True
        CommandCancel.Enabled = False
        labelWait.Visible = False
        ProgressBar1.Value = 0
        ProgressBar1.Visible = False
        ' If no errors, show application list dialog

        listDlg.ShowDialog(Me)
        
    End Sub
    '===================================================================
    ' ComboPhone_SelectedIndexChanged
    '
    ' User has changed selected phone
    '===================================================================
    Private Sub ComboPhone_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboPhone.SelectedIndexChanged
        ComboType.Items.Clear()

        ' Find out phone selected
        Dim dev As DMD.Nokia.NokiaDevice = Me.GetCurrentDevice
        If dev Is Nothing Then Exit Sub

        ' Check if application listing is supported
        CommandList.Enabled = dev.GetDeviceCAPS(DMD.Nokia.NokiaDeviceCaps.CanListInstalledApplications)
        If (dev.GetDeviceCAPS(DMD.Nokia.NokiaDeviceCaps.SupportsJavaAppInstallation)) Then ComboType.Items.Add(strJavaItem)
        If (dev.GetDeviceCAPS(DMD.Nokia.NokiaDeviceCaps.SupportsSISAppInstallation)) Then ComboType.Items.Add(strSymbianItem)
        If (dev.GetDeviceCAPS(DMD.Nokia.NokiaDeviceCaps.SupportsTheme)) Then ComboType.Items.Add(strThemesItem)
        bPhoneSupportsSisX = (dev.GetDeviceCAPS(DMD.Nokia.NokiaDeviceCaps.SupportsSISXAppInstallation))
        ComboType.Items.Add(strNGageItem)
        ComboType.SelectedIndex = 0
        TypeSelectionChanged()
    End Sub

    ' Timer for updating phonelist from callback function
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If bRefreshPhonecombo Then
            RefreshPhoneList()
            bRefreshPhonecombo = False
        End If
    End Sub

    Private Sub labelWait_Click(sender As Object, e As EventArgs) Handles labelWait.Click

    End Sub
End Class