'Filename    : BTPairingDlg.vb
'Part of     : Phone Navigator VB.NET example
'Description : Dialog for pairing Bluetooth devices
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

Public Class FRM_BTPairing
    Inherits System.Windows.Forms.Form

   

    Private m_bDisposed As Boolean = False
    Private m_pProgressDlg As FRM_ProgressDlg

#Region " Windows Form Designer generated code "

    '===================================================================
    ' Constructor
    '
    '===================================================================
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

         
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If Not m_bDisposed Then
            MyBase.Dispose(disposing)
        End If
        m_bDisposed = True
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents BTN_Close As System.Windows.Forms.Button
    Friend WithEvents BTN_Search As System.Windows.Forms.Button
    Friend WithEvents BTN_Pair As System.Windows.Forms.Button
    Friend WithEvents BTN_Unpair As System.Windows.Forms.Button
    Friend WithEvents BTN_SetTrusted As System.Windows.Forms.Button
    Friend WithEvents BTN_SetUnTrusted As System.Windows.Forms.Button
    Friend WithEvents LVW_PhoneList As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(FRM_BTPairing))
        Me.LVW_PhoneList = New System.Windows.Forms.ListView
        Me.BTN_Close = New System.Windows.Forms.Button
        Me.BTN_Search = New System.Windows.Forms.Button
        Me.BTN_Pair = New System.Windows.Forms.Button
        Me.BTN_Unpair = New System.Windows.Forms.Button
        Me.BTN_SetTrusted = New System.Windows.Forms.Button
        Me.BTN_SetUnTrusted = New System.Windows.Forms.Button
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'LVW_PhoneList
        '
        Me.LVW_PhoneList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.LVW_PhoneList.FullRowSelect = True
        Me.LVW_PhoneList.Location = New System.Drawing.Point(8, 8)
        Me.LVW_PhoneList.Name = "LVW_PhoneList"
        Me.LVW_PhoneList.Size = New System.Drawing.Size(272, 136)
        Me.LVW_PhoneList.TabIndex = 0
        Me.LVW_PhoneList.View = System.Windows.Forms.View.Details
        '
        'BTN_Close
        '
        Me.BTN_Close.Location = New System.Drawing.Point(288, 8)
        Me.BTN_Close.Name = "BTN_Close"
        Me.BTN_Close.Size = New System.Drawing.Size(80, 24)
        Me.BTN_Close.TabIndex = 7
        Me.BTN_Close.Text = "Close"
        '
        'BTN_Search
        '
        Me.BTN_Search.Location = New System.Drawing.Point(8, 160)
        Me.BTN_Search.Name = "BTN_Search"
        Me.BTN_Search.Size = New System.Drawing.Size(88, 24)
        Me.BTN_Search.TabIndex = 8
        Me.BTN_Search.Text = "Refresh List"
        '
        'BTN_Pair
        '
        Me.BTN_Pair.Location = New System.Drawing.Point(104, 160)
        Me.BTN_Pair.Name = "BTN_Pair"
        Me.BTN_Pair.Size = New System.Drawing.Size(80, 24)
        Me.BTN_Pair.TabIndex = 9
        Me.BTN_Pair.Text = "Pair"
        '
        'BTN_Unpair
        '
        Me.BTN_Unpair.Location = New System.Drawing.Point(104, 192)
        Me.BTN_Unpair.Name = "BTN_Unpair"
        Me.BTN_Unpair.Size = New System.Drawing.Size(80, 24)
        Me.BTN_Unpair.TabIndex = 10
        Me.BTN_Unpair.Text = "Unpair"
        '
        'BTN_SetTrusted
        '
        Me.BTN_SetTrusted.Location = New System.Drawing.Point(192, 160)
        Me.BTN_SetTrusted.Name = "BTN_SetTrusted"
        Me.BTN_SetTrusted.Size = New System.Drawing.Size(88, 24)
        Me.BTN_SetTrusted.TabIndex = 11
        Me.BTN_SetTrusted.Text = "Set Trusted"
        '
        'BTN_SetUnTrusted
        '
        Me.BTN_SetUnTrusted.Location = New System.Drawing.Point(192, 192)
        Me.BTN_SetUnTrusted.Name = "BTN_SetUnTrusted"
        Me.BTN_SetUnTrusted.Size = New System.Drawing.Size(88, 24)
        Me.BTN_SetUnTrusted.TabIndex = 12
        Me.BTN_SetUnTrusted.Text = "Set Untrusted"
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Phone name"
        Me.ColumnHeader1.Width = 134
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Status"
        Me.ColumnHeader2.Width = 134
        '
        'FRM_BTPairing
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(378, 224)
        Me.Controls.Add(Me.BTN_SetUnTrusted)
        Me.Controls.Add(Me.BTN_SetTrusted)
        Me.Controls.Add(Me.BTN_Unpair)
        Me.Controls.Add(Me.BTN_Pair)
        Me.Controls.Add(Me.BTN_Search)
        Me.Controls.Add(Me.BTN_Close)
        Me.Controls.Add(Me.LVW_PhoneList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FRM_BTPairing"
        Me.Text = "Bluetooth Pairing"
        Me.ResumeLayout(False)

    End Sub

#End Region

   

    '===================================================================
    ' FRM_BTPairing_Load
    '
    ' Initialization of FRM_BTPairing form
    '
    '===================================================================
    Private Sub FRM_BTPairing_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        BTRefreshPhoneList(True)
    End Sub

    '===================================================================
    ' ClearPhoneList
    '
    ' Removes all items from the phone list, and frees memory
    ' allocated for each list item data
    '===================================================================
    Private Sub ClearPhoneList()
        LVW_PhoneList.Items.Clear()
    End Sub

    '===================================================================
    ' RefreshPhoneList
    '
    ' Refreshes the phone list to the list control. If bSearchDevices
    ' is TRUE, searches all BT devices using connectivity API, else
    ' only copies the device information from memory to the list control.
    '===================================================================
    Private Sub BTRefreshPhoneList(ByVal bSearchDevices As Boolean)
        Me.LVW_PhoneList.Items.Clear()
        Dim devices As DMD.Nokia.NokiaBTDevice() = DMD.Nokia.GetBluetoothDevices
        For Each dev As DMD.Nokia.NokiaBTDevice In devices
            Dim item As ListViewItem = Me.LVW_PhoneList.Items.Add(dev.Name)
            item.Tag = dev
            If item.SubItems.Count <= 1 Then
                item.SubItems.Add(dev.StatusEx)
            Else
                item.SubItems(1).Text = dev.StatusEx
            End If
        Next
        'If iRet = ECONA_CANCELLED Then
        '    LVW_PhoneList.Items.Add("Search cancelled")
        'ElseIf iRet = ECONA_FAILED_TIMEOUT Then
        '    LVW_PhoneList.Items.Add("Timeout reached")
        'ElseIf iRet <> CONA_OK Then
        '    ShowErrorMessage("CONASearchDevices failed.", iRet)
        '    LVW_PhoneList.Items.Add("Search failed")
        'End If

        ' update device status to the list
    End Sub

    Public Function GetSelectedDevice() As DMD.Nokia.NokiaBTDevice
        Dim iSelIndex As Integer = -1
        If LVW_PhoneList.SelectedIndices().Count() > 0 Then
            iSelIndex = Me.LVW_PhoneList.SelectedItems(0).Index()
        End If
        If (iSelIndex < 0) Then Return Nothing
        Return Me.LVW_PhoneList.Items(iSelIndex).Tag
    End Function

    '===================================================================
    ' ChangeTrustedState
    '
    ' Changes the device trusted state. iTrustedState may be
    ' CONAPI_PAIR_DEVICE, CONAPI_UNPAIR_DEVICE, CONAPI_SET_PCSUITE_TRUSTED
    ' or CONAPI_SET_PCSUITE_UNTRUSTED. strPassword is needed only
    ' with CONAPI_PAIR_DEVICE, with other operations it should be NULL.
    '===================================================================
    Private Sub ChangeTrustedState(ByVal iTrustedState As DMD.Nokia.DeviceBTStatus, ByVal strPassword As String)
        Dim dev As DMD.Nokia.NokiaBTDevice = Me.GetSelectedDevice
        ' get selected device from the list
        If dev IsNot Nothing Then
            dev.SetTrustedState(iTrustedState, strPassword)
            BTRefreshPhoneList(False)
        Else
            MsgBox("Please select a phone from the list.")
        End If
    End Sub

    Private Sub FRM_BTPairing_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
         
    End Sub

    '===================================================================
    ' BTN_Search_Click
    '
    ' User has clicked Refresh List button
    '===================================================================
    Private Sub BTN_Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Search.Click
        BTRefreshPhoneList(True)
    End Sub

    '===================================================================
    ' BTN_Pair_Click
    '
    ' User has clicked Pair button
    '===================================================================
    Private Sub BTN_Pair_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Pair.Click
        Dim PasswordDlg As New FRM_Password
        If PasswordDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            ChangeTrustedState(DMD.Nokia.DeviceBTStatus.Paired, PasswordDlg.TXB_Password.Text)
        End If
    End Sub

    '===================================================================
    ' BTN_Unpair_Click
    '
    ' User has clicked Unpair button
    '===================================================================
    Private Sub BTN_Unpair_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Unpair.Click
        ChangeTrustedState(DMD.Nokia.DeviceBTStatus.Unpaired, vbNullString)
    End Sub

    '===================================================================
    ' BTN_SetTrusted_Click
    '
    ' User has clicked Set Trusted button
    '===================================================================
    Private Sub BTN_SetTrusted_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_SetTrusted.Click
        ChangeTrustedState(DMD.Nokia.DeviceBTStatus.Trusted, vbNullString)
    End Sub

    '===================================================================
    ' BTN_SetUntrusted_Click
    '
    ' User has clicked Set Untrusted button
    '===================================================================
    Private Sub BTN_SetUnTrusted_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_SetUnTrusted.Click
        ChangeTrustedState(DMD.Nokia.DeviceBTStatus.NotTrusted, vbNullString)
    End Sub

    '===================================================================
    ' BTN_Close_Click
    '
    ' User has clicked Close button
    '===================================================================
    Private Sub BTN_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_Close.Click
        Me.Close()
    End Sub
End Class
