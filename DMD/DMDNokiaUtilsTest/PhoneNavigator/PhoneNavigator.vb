'Filename    : PhoneNavigator.vb
'Part of     : Phone Navigator VB.NET example
'Description : Main dialog of VBPhoneNavigator.NET example application
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

Imports System.Runtime.InteropServices

Public Class PhoneNavigator
    Inherits System.Windows.Forms.Form

    Private m_bCancelled As Boolean
    Private bRefreshPhoneListBox As Boolean = False
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Private m_bDisposed As Boolean = False



#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If Not m_bDisposed Then
            MyBase.Dispose(disposing)
        End If
        m_bDisposed = True
        Application.Exit()
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents LBX_PhoneFiles As PhoneListBox1
    Friend WithEvents BTN_Rename As System.Windows.Forms.Button
    Friend WithEvents LBL_PhoneFiles As System.Windows.Forms.Label
    Friend WithEvents BTN_DeviceInfo As System.Windows.Forms.Button
    Friend WithEvents BTN_BluetoothPairing As System.Windows.Forms.Button

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PhoneNavigator))
        Me.BTN_Rename = New System.Windows.Forms.Button()
        Me.LBL_PhoneFiles = New System.Windows.Forms.Label()
        Me.BTN_DeviceInfo = New System.Windows.Forms.Button()
        Me.BTN_BluetoothPairing = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.LBX_PhoneFiles = New DMDNokiaUtilsTest.PhoneListBox1()
        Me.SuspendLayout()
        '
        'BTN_Rename
        '
        Me.BTN_Rename.Location = New System.Drawing.Point(115, 276)
        Me.BTN_Rename.Name = "BTN_Rename"
        Me.BTN_Rename.Size = New System.Drawing.Size(105, 24)
        Me.BTN_Rename.TabIndex = 8
        Me.BTN_Rename.Text = "Rename"
        '
        'LBL_PhoneFiles
        '
        Me.LBL_PhoneFiles.Location = New System.Drawing.Point(5, 9)
        Me.LBL_PhoneFiles.Name = "LBL_PhoneFiles"
        Me.LBL_PhoneFiles.Size = New System.Drawing.Size(232, 16)
        Me.LBL_PhoneFiles.TabIndex = 11
        Me.LBL_PhoneFiles.Text = "Connected Devices:"
        '
        'BTN_DeviceInfo
        '
        Me.BTN_DeviceInfo.Location = New System.Drawing.Point(8, 276)
        Me.BTN_DeviceInfo.Name = "BTN_DeviceInfo"
        Me.BTN_DeviceInfo.Size = New System.Drawing.Size(101, 24)
        Me.BTN_DeviceInfo.TabIndex = 17
        Me.BTN_DeviceInfo.Text = "Device Info"
        '
        'BTN_BluetoothPairing
        '
        Me.BTN_BluetoothPairing.Location = New System.Drawing.Point(226, 276)
        Me.BTN_BluetoothPairing.Name = "BTN_BluetoothPairing"
        Me.BTN_BluetoothPairing.Size = New System.Drawing.Size(107, 24)
        Me.BTN_BluetoothPairing.TabIndex = 18
        Me.BTN_BluetoothPairing.Text = "Bluetooth Pairing"
        '
        'Timer1
        '
        '
        'LBX_PhoneFiles
        '
        Me.LBX_PhoneFiles.Location = New System.Drawing.Point(8, 28)
        Me.LBX_PhoneFiles.Name = "LBX_PhoneFiles"
        Me.LBX_PhoneFiles.Size = New System.Drawing.Size(325, 238)
        Me.LBX_PhoneFiles.TabIndex = 0
        '
        'PhoneNavigator
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(345, 314)
        Me.Controls.Add(Me.BTN_BluetoothPairing)
        Me.Controls.Add(Me.BTN_DeviceInfo)
        Me.Controls.Add(Me.LBL_PhoneFiles)
        Me.Controls.Add(Me.BTN_Rename)
        Me.Controls.Add(Me.LBX_PhoneFiles)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PhoneNavigator"
        Me.Text = "Phone Navigator"
        Me.ResumeLayout(False)

    End Sub

#End Region

    <STAThread()> _
    Shared Sub Main()
        ' Starts the application.
        Application.Run(New PhoneNavigator)
    End Sub

    '===================================================================
    ' RefreshPhoneListBox
    '
    ' Refresh phone list to list box
    '
    '===================================================================
    Public Sub RefreshPhoneListBox()
        bRefreshPhoneListBox = True
    End Sub

    '===================================================================
    ' SetCancelled
    '
    ' Sets m_bCancelled value
    '
    '===================================================================
    Public Sub SetCancelled(ByVal bCancelled As Boolean)
        m_bCancelled = bCancelled
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
        m_bCancelled = False
    End Function

    '===================================================================
    ' PhoneNavigator_Load
    '
    ' Initialization of PhoneNavigator form
    '
    '===================================================================
    Private Sub PhoneNavigator_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Initializing phone file list:
        Me.LBX_PhoneFiles.Refill()
        Timer1.Enabled = True
        Timer1.Start()
        'LBX_PhoneFiles.ListAllPhones()
        bRefreshPhoneListBox = True
    End Sub


    '===================================================================
    ' BTN_BluetoothPairing_Click
    '
    ' Opens dialog for pairing Bluetooth devices
    '===================================================================
    Private Sub BTN_BluetoothPairing_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_BluetoothPairing.Click
        Dim BtPairingDlg As New FRM_BTPairing
        BtPairingDlg.ShowDialog(Me)
    End Sub


    '===================================================================
    ' BTN_Rename_Click
    '
    ' Renames a folder or file from phone. If the selected item is
    ' a phone, renames friendly name of phone 
    '===================================================================
    Private Sub BTN_Rename_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Rename.Click
        If LBX_PhoneFiles.SelectedIndex <> -1 Then
            Dim RenameDlg As FRM_Rename = New FRM_Rename
            'Rename(phone) 's friendly name
            RenameDlg.LBL_OldName.Text = Me.LBX_PhoneFiles.GetCurrentDevice.FriendlyName
            If RenameDlg.LBL_OldName.Text.Length > 0 Then
                ' Open dialog to get new name
                If RenameDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    Dim strNewName As String = RenameDlg.TXB_NewName.Text

                    bRefreshPhoneListBox = True

                End If
            End If
        Else
            MsgBox("Please select phone to be renamed.")
        End If
    End Sub

    '===================================================================
    ' BTN_DeviceInfo_Click
    '
    ' Show device info
    '===================================================================
    Private Sub BTN_DeviceInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_DeviceInfo.Click
        ' currently selected device
        If (Me.LBX_PhoneFiles.GetCurrentDevice Is Nothing) Then
            MsgBox("Please select a phone")
            Return
        End If
        Dim infoDlg As FRM_DeviceInfo = New FRM_DeviceInfo
        infoDlg.ShowDialog()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If bRefreshPhoneListBox Then
            LBX_PhoneFiles.Refill()
            bRefreshPhoneListBox = False
        End If
    End Sub

    Private Sub LBX_PhoneFiles_DoubleClick(sender As Object, e As EventArgs) Handles LBX_PhoneFiles.DoubleClick
        ' currently selected device
        If (Me.LBX_PhoneFiles.GetCurrentDevice Is Nothing) Then
            MsgBox("Please select a phone")
            Return
        End If
        Dim infoDlg As FRM_DeviceInfo = New FRM_DeviceInfo
        infoDlg.SetDevice(Me.LBX_PhoneFiles.GetCurrentDevice)
        infoDlg.ShowDialog()
    End Sub

    Private Sub LBX_PhoneFiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LBX_PhoneFiles.SelectedIndexChanged

    End Sub
End Class
