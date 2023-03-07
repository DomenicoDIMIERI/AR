'Filename    : BTProgressDlg.vb
'Part of     : Phone Navigator VB.NET example
'Description : Dialog for showing progress when searching for Bluetooth devices
'Version     : 3.2
'
'This example is only to be used with PC Connectivity API version 3.2.
'Compability ("as is") with future versions is not quaranteed.
'
'Copyright (c) 2005-2007 Nokia Corporation.
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

Public Class FRM_ProgressDlg
    Inherits System.Windows.Forms.Form

    Private m_bCancelled As Boolean = False

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
    Friend WithEvents LBL_Searching As System.Windows.Forms.Label
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FRM_ProgressDlg))
        Me.BTN_Cancel = New System.Windows.Forms.Button
        Me.LBL_Searching = New System.Windows.Forms.Label
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
        Me.SuspendLayout()
        '
        'BTN_Cancel
        '
        Me.BTN_Cancel.Location = New System.Drawing.Point(296, 8)
        Me.BTN_Cancel.Name = "BTN_Cancel"
        Me.BTN_Cancel.Size = New System.Drawing.Size(72, 24)
        Me.BTN_Cancel.TabIndex = 13
        Me.BTN_Cancel.Text = "Cancel"
        '
        'LBL_Searching
        '
        Me.LBL_Searching.Location = New System.Drawing.Point(8, 16)
        Me.LBL_Searching.Name = "LBL_Searching"
        Me.LBL_Searching.Size = New System.Drawing.Size(224, 16)
        Me.LBL_Searching.TabIndex = 14
        Me.LBL_Searching.Text = "Searching bluetooth devices..."
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(8, 40)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(360, 8)
        Me.ProgressBar1.TabIndex = 15
        '
        'FRM_ProgressDlg
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(378, 56)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.LBL_Searching)
        Me.Controls.Add(Me.BTN_Cancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FRM_ProgressDlg"
        Me.Text = "Searching Devices"
        Me.ResumeLayout(False)

    End Sub

#End Region

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
        IsCancelled = m_bCancelled
        ' reset search cancel flag.
        m_bCancelled = False
    End Function

    '===================================================================
    ' BTN_Cancel_Click
    '
    ' User has clicked Cancel button to cancel current operation
    '===================================================================
    Private Sub BTN_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_Cancel.Click
        m_bCancelled = True
    End Sub
End Class
