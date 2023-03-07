'Filename    : FileBrowser.vb
'Part of     : Phone Navigator VB.NET example
'Description : Main dialog of VBFileBrowser.NET example application
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
Imports DMD.Nokia

Public Class FileBrowser
    Inherits System.Windows.Forms.Form

    Public Const PHONELIST_STATE_PHONELIST As Short = 1
    Public Const PHONELIST_STATE_PHONECONTENT As Short = 2

    Private m_bCancelled As Boolean
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Private m_bDisposed As Boolean = False
    Friend WithEvents CheckBoxUseCache As System.Windows.Forms.CheckBox
    Friend WithEvents BTN_ItemInfo As System.Windows.Forms.Button
    Friend WithEvents cboDevice As System.Windows.Forms.ComboBox
    Public bRefreshPhoneListBox As Boolean = False

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
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents LBX_PCFiles As FileListBox
    Friend WithEvents LBX_PhoneFiles As PhoneListBox
    Friend WithEvents BTN_CopyPCToPhone As System.Windows.Forms.Button
    Friend WithEvents BTN_CopyPhoneToPC As System.Windows.Forms.Button
    Friend WithEvents BTN_MovePCToPhone As System.Windows.Forms.Button
    Friend WithEvents BTN_MovePhoneToPC As System.Windows.Forms.Button
    Friend WithEvents BTN_Close As System.Windows.Forms.Button
    Friend WithEvents BTN_Delete As System.Windows.Forms.Button
    Friend WithEvents BTN_Rename As System.Windows.Forms.Button
    Friend WithEvents BTN_Create As System.Windows.Forms.Button
    Friend WithEvents LBL_PCFiles As System.Windows.Forms.Label
    Friend WithEvents LBL_PhoneFiles As System.Windows.Forms.Label
    Friend WithEvents BTN_Cancel As System.Windows.Forms.Button
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FileBrowser))
        Me.BTN_CopyPCToPhone = New System.Windows.Forms.Button()
        Me.BTN_CopyPhoneToPC = New System.Windows.Forms.Button()
        Me.BTN_MovePCToPhone = New System.Windows.Forms.Button()
        Me.BTN_MovePhoneToPC = New System.Windows.Forms.Button()
        Me.BTN_Close = New System.Windows.Forms.Button()
        Me.BTN_Delete = New System.Windows.Forms.Button()
        Me.BTN_Rename = New System.Windows.Forms.Button()
        Me.BTN_Create = New System.Windows.Forms.Button()
        Me.LBL_PCFiles = New System.Windows.Forms.Label()
        Me.LBL_PhoneFiles = New System.Windows.Forms.Label()
        Me.BTN_Cancel = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.CheckBoxUseCache = New System.Windows.Forms.CheckBox()
        Me.BTN_ItemInfo = New System.Windows.Forms.Button()
        Me.LBX_PCFiles = New DMDNokiaUtilsTest.FileListBox()
        Me.LBX_PhoneFiles = New DMDNokiaUtilsTest.PhoneListBox()
        Me.cboDevice = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'BTN_CopyPCToPhone
        '
        Me.BTN_CopyPCToPhone.Location = New System.Drawing.Point(253, 50)
        Me.BTN_CopyPCToPhone.Name = "BTN_CopyPCToPhone"
        Me.BTN_CopyPCToPhone.Size = New System.Drawing.Size(80, 24)
        Me.BTN_CopyPCToPhone.TabIndex = 2
        Me.BTN_CopyPCToPhone.Text = "Copy >>"
        '
        'BTN_CopyPhoneToPC
        '
        Me.BTN_CopyPhoneToPC.Location = New System.Drawing.Point(253, 80)
        Me.BTN_CopyPhoneToPC.Name = "BTN_CopyPhoneToPC"
        Me.BTN_CopyPhoneToPC.Size = New System.Drawing.Size(80, 24)
        Me.BTN_CopyPhoneToPC.TabIndex = 3
        Me.BTN_CopyPhoneToPC.Text = "<< Copy"
        '
        'BTN_MovePCToPhone
        '
        Me.BTN_MovePCToPhone.Location = New System.Drawing.Point(253, 110)
        Me.BTN_MovePCToPhone.Name = "BTN_MovePCToPhone"
        Me.BTN_MovePCToPhone.Size = New System.Drawing.Size(80, 24)
        Me.BTN_MovePCToPhone.TabIndex = 4
        Me.BTN_MovePCToPhone.Text = "Move >>"
        '
        'BTN_MovePhoneToPC
        '
        Me.BTN_MovePhoneToPC.Location = New System.Drawing.Point(253, 140)
        Me.BTN_MovePhoneToPC.Name = "BTN_MovePhoneToPC"
        Me.BTN_MovePhoneToPC.Size = New System.Drawing.Size(80, 24)
        Me.BTN_MovePhoneToPC.TabIndex = 5
        Me.BTN_MovePhoneToPC.Text = "<< Move"
        '
        'BTN_Close
        '
        Me.BTN_Close.Location = New System.Drawing.Point(253, 250)
        Me.BTN_Close.Name = "BTN_Close"
        Me.BTN_Close.Size = New System.Drawing.Size(80, 24)
        Me.BTN_Close.TabIndex = 6
        Me.BTN_Close.Text = "Close"
        '
        'BTN_Delete
        '
        Me.BTN_Delete.Location = New System.Drawing.Point(512, 288)
        Me.BTN_Delete.Name = "BTN_Delete"
        Me.BTN_Delete.Size = New System.Drawing.Size(72, 24)
        Me.BTN_Delete.TabIndex = 7
        Me.BTN_Delete.Text = "Delete"
        '
        'BTN_Rename
        '
        Me.BTN_Rename.Location = New System.Drawing.Point(428, 288)
        Me.BTN_Rename.Name = "BTN_Rename"
        Me.BTN_Rename.Size = New System.Drawing.Size(72, 24)
        Me.BTN_Rename.TabIndex = 8
        Me.BTN_Rename.Text = "Rename"
        '
        'BTN_Create
        '
        Me.BTN_Create.Location = New System.Drawing.Point(344, 288)
        Me.BTN_Create.Name = "BTN_Create"
        Me.BTN_Create.Size = New System.Drawing.Size(72, 24)
        Me.BTN_Create.TabIndex = 9
        Me.BTN_Create.Text = "Create"
        '
        'LBL_PCFiles
        '
        Me.LBL_PCFiles.Location = New System.Drawing.Point(11, 31)
        Me.LBL_PCFiles.Name = "LBL_PCFiles"
        Me.LBL_PCFiles.Size = New System.Drawing.Size(229, 18)
        Me.LBL_PCFiles.TabIndex = 10
        Me.LBL_PCFiles.Text = "Local folders:"
        '
        'LBL_PhoneFiles
        '
        Me.LBL_PhoneFiles.Location = New System.Drawing.Point(236, 9)
        Me.LBL_PhoneFiles.Name = "LBL_PhoneFiles"
        Me.LBL_PhoneFiles.Size = New System.Drawing.Size(116, 24)
        Me.LBL_PhoneFiles.TabIndex = 11
        Me.LBL_PhoneFiles.Text = "Connected Devices:"
        '
        'BTN_Cancel
        '
        Me.BTN_Cancel.Location = New System.Drawing.Point(253, 219)
        Me.BTN_Cancel.Name = "BTN_Cancel"
        Me.BTN_Cancel.Size = New System.Drawing.Size(80, 24)
        Me.BTN_Cancel.TabIndex = 12
        Me.BTN_Cancel.Text = "Cancel"
        Me.BTN_Cancel.Visible = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(8, 288)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(656, 16)
        Me.ProgressBar1.TabIndex = 13
        Me.ProgressBar1.Visible = False
        '
        'Timer1
        '
        '
        'CheckBoxUseCache
        '
        Me.CheckBoxUseCache.AutoSize = True
        Me.CheckBoxUseCache.Location = New System.Drawing.Point(253, 292)
        Me.CheckBoxUseCache.Name = "CheckBoxUseCache"
        Me.CheckBoxUseCache.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBoxUseCache.Size = New System.Drawing.Size(78, 17)
        Me.CheckBoxUseCache.TabIndex = 14
        Me.CheckBoxUseCache.Text = "Use cache"
        Me.CheckBoxUseCache.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBoxUseCache.UseVisualStyleBackColor = True
        '
        'BTN_ItemInfo
        '
        Me.BTN_ItemInfo.Location = New System.Drawing.Point(597, 288)
        Me.BTN_ItemInfo.Name = "BTN_ItemInfo"
        Me.BTN_ItemInfo.Size = New System.Drawing.Size(72, 24)
        Me.BTN_ItemInfo.TabIndex = 15
        Me.BTN_ItemInfo.Text = "Item info"
        Me.BTN_ItemInfo.UseVisualStyleBackColor = True
        '
        'LBX_PCFiles
        '
        Me.LBX_PCFiles.Location = New System.Drawing.Point(8, 50)
        Me.LBX_PCFiles.Name = "LBX_PCFiles"
        Me.LBX_PCFiles.Size = New System.Drawing.Size(232, 225)
        Me.LBX_PCFiles.TabIndex = 0
        '
        'LBX_PhoneFiles
        '
        Me.LBX_PhoneFiles.Location = New System.Drawing.Point(344, 50)
        Me.LBX_PhoneFiles.Name = "LBX_PhoneFiles"
        Me.LBX_PhoneFiles.Size = New System.Drawing.Size(327, 225)
        Me.LBX_PhoneFiles.TabIndex = 0
        '
        'cboDevice
        '
        Me.cboDevice.FormattingEnabled = True
        Me.cboDevice.Location = New System.Drawing.Point(344, 6)
        Me.cboDevice.Name = "cboDevice"
        Me.cboDevice.Size = New System.Drawing.Size(327, 21)
        Me.cboDevice.TabIndex = 16
        '
        'FileBrowser
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(678, 318)
        Me.Controls.Add(Me.cboDevice)
        Me.Controls.Add(Me.BTN_ItemInfo)
        Me.Controls.Add(Me.CheckBoxUseCache)
        Me.Controls.Add(Me.BTN_Cancel)
        Me.Controls.Add(Me.LBL_PhoneFiles)
        Me.Controls.Add(Me.LBL_PCFiles)
        Me.Controls.Add(Me.BTN_Create)
        Me.Controls.Add(Me.BTN_Rename)
        Me.Controls.Add(Me.BTN_Delete)
        Me.Controls.Add(Me.BTN_Close)
        Me.Controls.Add(Me.BTN_MovePhoneToPC)
        Me.Controls.Add(Me.BTN_MovePCToPhone)
        Me.Controls.Add(Me.BTN_CopyPhoneToPC)
        Me.Controls.Add(Me.BTN_CopyPCToPhone)
        Me.Controls.Add(Me.LBX_PCFiles)
        Me.Controls.Add(Me.LBX_PhoneFiles)
        Me.Controls.Add(Me.ProgressBar1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FileBrowser"
        Me.Text = "File Browser"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    <STAThread()> _
    Shared Sub Main()
        ' Starts the application.
        Application.Run(New FileBrowser)
    End Sub
 

    '===================================================================
    ' StartProgress
    '
    ' Sets progress bar visible and hides/unhides some buttons
    '
    '===================================================================
    Public Sub StartProgress(ByVal title As String, ByVal line1 As String, ByVal line2 As String)
        Me.LBX_PhoneFiles.ResetContent()
        Me.LBX_PhoneFiles.Items.Add(title)
        Me.LBX_PhoneFiles.Items.Add(line1)
        Me.LBX_PhoneFiles.Items.Add(line2)
        Me.Cursor = Cursors.WaitCursor
        Me.BTN_Create.Visible = False
        Me.BTN_Delete.Visible = False
        Me.BTN_Rename.Visible = False
        Me.BTN_Close.Visible = False
        Me.BTN_ItemInfo.Visible = False
        Me.CheckBoxUseCache.Visible = False
        Me.BTN_Cancel.Visible = True
        Me.ProgressBar1.Minimum = 0
        Me.ProgressBar1.Maximum = 100
        Me.ProgressBar1.Value = 0
        Me.ProgressBar1.Visible = True
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
    ' StopProgress
    '
    ' Hides progress bar and hides/unhides some buttons
    '
    '===================================================================
    Public Sub StopProgress()
        Me.ProgressBar1.Visible = False
        Me.BTN_Create.Visible = True
        Me.BTN_Delete.Visible = True
        Me.BTN_Rename.Visible = True
        Me.BTN_Close.Visible = True
        Me.BTN_ItemInfo.Visible = True
        Me.CheckBoxUseCache.Visible = True
        Me.BTN_Cancel.Visible = False
        Me.Cursor = Cursors.Default
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
    ' FileBrowser_Load
    '
    ' Initialization of FileBrowser form
    '
    '===================================================================
    Private Sub FileBrowser_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Initializing PC file list:
        LBX_PCFiles.PopulateList("")
        LBL_PCFiles.Text = LBX_PCFiles.GetCurrentFolder()
        ' Initializing phone file list:
        Me.RefreshDevices()
        LBL_PhoneFiles.Text = LBX_PhoneFiles.GetCurrentFolderName()

        Timer1.Enabled = True
        Timer1.Start()
    End Sub

    '===================================================================
    ' BTN_CopyPCToPhone_Click
    '
    ' Copies selected pc file to selected phone folder
    '===================================================================
    Private Sub BTN_CopyPCToPhone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_CopyPCToPhone.Click
        If LBX_PCFiles.GetCurrentFile().Length() <= 0 Then
            MsgBox("Please select PC file to be copied.")
        Else
            Dim device As DMD.Nokia.NokiaDevice = Me.GetCurrentDevice
            Dim strPCFile As String = LBX_PCFiles.GetCurrentFile() ' file name without path
            Dim strPCFolder As String = LBX_PCFiles.GetCurrentFolder() ' e.g. 'c:\temp'
            Dim strPhoneFolder As String = LBX_PhoneFiles.GetCurrentFolderName()
            If strPhoneFolder.Length <= 0 Then strPhoneFolder = LBX_PhoneFiles.GetCurrentFolderName()
            Try
                device.FileSystem.CopyFilesToDevice(System.IO.Path.Combine(strPCFolder, strPCFile), strPhoneFolder)
                MsgBox("Copy completed succesfully!")
            Catch ex As OperationCanceledException
                MsgBox("Copy was cancelled.")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            ' show updated folder listing
            LBX_PhoneFiles.Refill()
        End If
    End Sub

    '===================================================================
    ' BTN_CopyPhoneToPC_Click
    '
    ' Copies selected phone file to selected pc folder
    '===================================================================
    Private Sub BTN_CopyPhoneToPC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_CopyPhoneToPC.Click
        Dim file As String = LBX_PhoneFiles.GetCurrentFileName()
        If file.Length() <= 0 Then
            MsgBox("Please select phone file to be copied.")
        Else
            Dim device As DMD.Nokia.NokiaDevice = Me.GetCurrentDevice
            Dim strPhoneFile As String = LBX_PhoneFiles.GetCurrentFileName()
            Dim strPhoneFolder As String = LBX_PhoneFiles.GetCurrentFolderName()
            Dim strPCFolder As String = LBX_PCFiles.GetCurrentFolder()
            Try
                device.FileSystem.CopyFilesFromDevice(System.IO.Path.Combine(strPhoneFolder, strPhoneFile), strPCFolder)
                MsgBox("Copy completed succesfully!")
            Catch ex As OperationCanceledException
                MsgBox("Copy was cancelled.")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            ' show updated folder listing
            LBX_PCFiles.PopulateList("")
            LBX_PhoneFiles.Refill()
        End If
    End Sub

    '===================================================================
    ' BTN_MovePCToPhone_Click
    '
    ' Moves selected pc file to selected phone folder
    '===================================================================
    Private Sub BTN_MovePCToPhone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_MovePCToPhone.Click
        If LBX_PCFiles.GetCurrentFile().Length() <= 0 Then
            ' No PC file selected
            MsgBox("Please select PC file to be moved.")
        Else
            Dim device As DMD.Nokia.NokiaDevice = Me.GetCurrentDevice
            Dim strPCFile As String = LBX_PCFiles.GetCurrentFile() ' file name without path
            Dim strPCFolder As String = LBX_PCFiles.GetCurrentFolder() ' e.g. 'c:\temp'
            Dim strPhoneFolder As String = LBX_PhoneFiles.GetCurrentFolderName()
            If strPhoneFolder.Length <= 0 Then strPhoneFolder = LBX_PhoneFiles.GetCurrentFolderName()
            Try
                device.FileSystem.MoveFilesToDevice(System.IO.Path.Combine(strPCFolder, strPCFile), strPhoneFolder)
                MsgBox("Copy completed succesfully!")
            Catch ex As OperationCanceledException
                MsgBox("Copy was cancelled.")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            ' show updated folder listing
            LBX_PCFiles.PopulateList("")
            LBX_PhoneFiles.Refill()
        End If
    End Sub

    '===================================================================
    ' BTN_MovePhoneToPC_Click
    '
    ' Moves selected phone file to selected pc folder
    '===================================================================
    Private Sub BTN_MovePhoneToPC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_MovePhoneToPC.Click
        Dim file As String = LBX_PhoneFiles.GetCurrentFileName()
        If file.Length() <= 0 Then
            MsgBox("Please select phone file to be moved.")
        Else
            Dim device As DMD.Nokia.NokiaDevice = Me.GetCurrentDevice
            Dim strPCFile As String = LBX_PCFiles.GetCurrentFile() ' file name without path
            Dim strPCFolder As String = LBX_PCFiles.GetCurrentFolder() ' e.g. 'c:\temp'
            Dim strPhoneFolder As String = LBX_PhoneFiles.GetCurrentFolderName()
            If strPhoneFolder.Length <= 0 Then strPhoneFolder = LBX_PhoneFiles.GetCurrentFolderName()
            Try
                device.FileSystem.MoveFilesFromDevice(System.IO.Path.Combine(strPCFolder, strPCFile), strPhoneFolder)
                MsgBox("Copy completed succesfully!")
            Catch ex As OperationCanceledException
                MsgBox("Copy was cancelled.")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            ' show updated folder listing
            LBX_PCFiles.PopulateList("")
            LBX_PhoneFiles.Refill()

        End If
    End Sub

    '===================================================================
    ' BTN_Create_Click
    '
    ' Creates new folder to selected phone folder
    '===================================================================
    Private Sub BTN_Create_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Create.Click
        Dim CreateDlg As FRM_Create = New FRM_Create
        CreateDlg.TXB_Name.Text = ""
        If CreateDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim device As DMD.Nokia.NokiaDevice = Me.GetCurrentDevice
            Dim strPhoneFolder As String = LBX_PhoneFiles.GetCurrentFolderName()
            Try
                device.FileSystem.CreateDeviceFolder(System.IO.Path.Combine(strPhoneFolder, CreateDlg.TXB_Name.Text))
                MsgBox("Create folder completed succesfully!")
            Catch ex As OperationCanceledException
                MsgBox("Create folder was cancelled.")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            ' show updated folder listing
            LBX_PhoneFiles.Refill()
        End If
    End Sub

    '===================================================================
    ' BTN_Rename_Click
    '
    ' Renames a folder or file from phone. If the selected item is
    ' a phone, renames friendly name of phone 
    '===================================================================
    Private Sub BTN_Rename_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Rename.Click
        Dim bIsFile As Boolean = True  ' selected item is file - not folder
        If LBX_PhoneFiles.SelectedIndex <> -1 Then
            Dim RenameDlg As FRM_Rename = New FRM_Rename
            RenameDlg.LBL_OldName.Text = LBX_PhoneFiles.GetCurrentFileName()
            If RenameDlg.LBL_OldName.Text.Length <= 0 Then
                bIsFile = False ' selected item is folder - not file
                RenameDlg.LBL_OldName.Text = LBX_PhoneFiles.GetCurrentFolderName()
            End If
            If RenameDlg.LBL_OldName.Text.Length > 0 Then
                If RenameDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    Dim device As DMD.Nokia.NokiaDevice = Me.GetCurrentDevice
                    Dim strPhoneFolder As String = LBX_PhoneFiles.GetCurrentFolderName()
                    Dim strNewName As String = RenameDlg.TXB_NewName.Text
                    Dim strOldName As String = RenameDlg.LBL_OldName.Text
                    strOldName = strOldName.Substring(strOldName.LastIndexOf("\") + 1)
                    Try
                        If bIsFile Then
                            device.FileSystem.RenameDeviceFile(System.IO.Path.Combine(strOldName, strPhoneFolder), System.IO.Path.Combine(strNewName, strPhoneFolder))
                        Else
                            device.FileSystem.RenameDeviceFolder(System.IO.Path.Combine(strOldName, strPhoneFolder), System.IO.Path.Combine(strNewName, strPhoneFolder))
                        End If
                        MsgBox("Rename completed succesfully!")
                        ' show updated folder listing
                        LBX_PhoneFiles.Refill()
                    Catch ex As Exception

                    End Try
                Else
                    MsgBox("No phone renaming")
                End If
            End If
        Else
            MsgBox("Please select file/folder to be renamed.")
        End If
    End Sub


    '===================================================================
    ' BTN_Delete_Click
    '
    ' Deletes selected phone file/folder
    '===================================================================
    Private Sub BTN_Delete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Delete.Click
        'If LBX_PhoneFiles.GetState() = PHONELIST_STATE_PHONECONTENT Then
        Dim device As DMD.Nokia.NokiaDevice = Me.GetCurrentDevice
        Dim strMessage As String
        Dim strSelectedtxt As String = ""
        Dim cursel As Integer = LBX_PhoneFiles.SelectedIndex
        If cursel <> -1 Then strSelectedtxt = LBX_PhoneFiles.Items(cursel).ToString()
        Dim strSelectedFile As String = LBX_PhoneFiles.GetCurrentFileName()
        If Len(strSelectedFile) > 0 Then
            Dim strPhoneFile As String = LBX_PhoneFiles.GetCurrentFileName()
            Dim strPhoneFolder As String = LBX_PhoneFiles.GetCurrentFolderName()
            strMessage = "Are you sure you want to permanently delete file '"
            strMessage &= strPhoneFile & "' from folder '" & strPhoneFolder & "'?"
            If MsgBox(strMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                Try
                    device.FileSystem.DeleteDeviceFile(System.IO.Path.Combine(strPhoneFile, strPhoneFolder))
                    MsgBox("Delete file completed succesfully!")
                Catch ex As OperationCanceledException
                    MsgBox("Delete file was cancelled.")
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
                ' show updated folder listing
                LBX_PhoneFiles.Refill()
            End If
        ElseIf Len(strSelectedtxt) > 0 And strSelectedtxt <> "[..]" Then
            Dim strPhoneFolder As String = LBX_PhoneFiles.GetCurrentFolderName()
            Dim iLastSeparator As Integer = strPhoneFolder.LastIndexOf("\")
            strPhoneFolder = strPhoneFolder.Remove(iLastSeparator, strPhoneFolder.Length - iLastSeparator)
            Dim strFolder As String = LBX_PhoneFiles.GetCurrentFolderName()
            strMessage = "Are you sure you want to permanently delete folder '"
            strMessage &= strFolder & "' from folder '" & strPhoneFolder & "'?"
            If MsgBox(strMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                Try
                    device.FileSystem.DeleteDeviceFolder(System.IO.Path.Combine(strPhoneFolder, strFolder))
                    MsgBox("Delete folder completed succesfully!")
                Catch ex As OperationCanceledException
                    MsgBox("Delete folder was cancelled.")
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
                ' show updated folder listing
                LBX_PhoneFiles.Refill()
            End If
        Else
            MsgBox("Select file/folder to be deleted.")
        End If
         
    End Sub

    '===================================================================
    ' BTN_Cancel_Click
    '
    ' User has clicked Cancel button to cancel current file operation
    '===================================================================
    Private Sub BTN_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Cancel.Click
        m_bCancelled = True
        Cursor = Cursors.Default
        BTN_Create.Visible = True
        BTN_Delete.Visible = True
        BTN_Rename.Visible = True
        BTN_Close.Visible = True
        BTN_Cancel.Visible = False
        ProgressBar1.Visible = False
    End Sub

    '===================================================================
    ' BTN_Close_Click
    '
    ' User has clicked Close button
    '===================================================================
    Private Sub BTN_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Close.Click
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If bRefreshPhoneListBox Then
            Me.RefreshDevices()
            bRefreshPhoneListBox = False
        End If
    End Sub

    Public Sub RefreshDevices()
        Me.cboDevice.Items.Clear()
        For Each device As DMD.Nokia.NokiaDevice In DMD.Nokia.Devices
            Me.cboDevice.Items.Add(device)
        Next
        LBX_PhoneFiles.CurrentDevice = Me.GetCurrentDevice
        LBL_PhoneFiles.Text = ""
    End Sub

    
    Private Function StrFormatByteSize(ByVal lSize As Long) As String
        Dim dlSize As Decimal
        If lSize < 1000 Then
            StrFormatByteSize = lSize.ToString() + " bytes"
        ElseIf lSize < 1000000 Then
            dlSize = lSize / 1000
            StrFormatByteSize = String.Format("{0:0.0}", dlSize) + "KB"
        ElseIf lSize < 1000000000 Then
            dlSize = lSize / 1000000
            StrFormatByteSize = String.Format("{0:0.0}", dlSize) + "MB"
        ElseIf lSize < 1000000000000 Then
            dlSize = lSize / 1000000000
            StrFormatByteSize = String.Format("{0:0.0}", dlSize) + "GB"
        Else
            dlSize = lSize / 1000000000000
            StrFormatByteSize = String.Format("{0:0.0}", dlSize) + "TB"
        End If
    End Function

    Public Function GetCurrentDevice() As NokiaDevice
        Dim dev As DMD.Nokia.NokiaDevice = Nothing
        If (Me.cboDevice.SelectedIndex >= 0) Then
            Return Me.cboDevice.SelectedItem
        Else
            Return Nothing
        End If
    End Function

    Private Sub BTN_ItemInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_ItemInfo.Click
        Dim device As NokiaDevice = Me.GetCurrentDevice



        Dim strSelectedFile As String
        strSelectedFile = LBX_PhoneFiles.GetCurrentFileName
        If strSelectedFile.Length > 0 Then
            Dim strPhoneFolder As String = LBX_PhoneFiles.GetCurrentFolderName
            Dim strPhoneFile As String = LBX_PhoneFiles.GetCurrentFileName
            Dim stFileInfo As NokiaFileInfo = device.FileSystem.GetFileInfo(System.IO.Path.Combine(strPhoneFolder, strPhoneFile))


            Dim dlgFileInfo As FRM_FolderInfo = New FRM_FolderInfo
            dlgFileInfo.Text = "File Info"
            Dim iItemCount As Integer = 0
            Dim listItem As ListViewItem = New ListViewItem()

            listItem.Text = "Name"
            listItem.SubItems.Add(stFileInfo.FileName)
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            listItem = New ListViewItem()
            listItem.Text = "File permission"
            listItem.SubItems.Add(stFileInfo.AttributesEx)
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            listItem = New ListViewItem()
            listItem.Text = "File modified time"
            listItem.SubItems.Add(stFileInfo.DateLastModified.ToString)
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            listItem = New ListViewItem()
            listItem.Text = "File size"
            listItem.SubItems.Add(stFileInfo.Size.ToString())
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            listItem = New ListViewItem()
            listItem.Text = "MIME type"
            listItem.SubItems.Add(stFileInfo.MIMEType)
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            dlgFileInfo.ShowDialog(Me)
        Else
            Dim strPhoneFolder As String = LBX_PhoneFiles.GetCurrentFolderName()
            Dim strFolder As String = LBX_PhoneFiles.SelectedItem

            'strPhoneFolder &= "\\"
            strFolder = strFolder.TrimStart("[")
            strFolder = strFolder.TrimEnd("]")

            Dim info As NokiaFolderInfo = device.FileSystem.GetFolderInfo(System.IO.Path.Combine(strPhoneFolder, strFolder))


            Dim dlgFileInfo As FRM_FolderInfo = New FRM_FolderInfo

            dlgFileInfo.Text = "Folder Info"
            Dim iItemCount As Integer = 0
            Dim listItem As ListViewItem = New ListViewItem()

            listItem.Text = "Name"
            listItem.SubItems.Add(info.FileName)
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            listItem = New ListViewItem()
            listItem.Text = "Absolute path"
            listItem.SubItems.Add(info.FullPath)
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            listItem = New ListViewItem()
            listItem.Text = "Type and permission"
            listItem.SubItems.Add(info.AttributesEx)
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            listItem = New ListViewItem()
            listItem.Text = "Folder time"
            listItem.SubItems.Add(info.DateLastModified.ToString)
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            listItem = New ListViewItem()
            listItem.Text = "Label"
            listItem.SubItems.Add(info.Label)
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            listItem = New ListViewItem()
            listItem.Text = "Memory type"
            listItem.SubItems.Add(info.MemoryType)
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            listItem = New ListViewItem()
            listItem.Text = "Identification ID"
            listItem.SubItems.Add(info.IdentificationID)
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            listItem = New ListViewItem()
            listItem.Text = "Free memory"
            listItem.SubItems.Add(StrFormatByteSize(info.FreeMemory))
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            listItem = New ListViewItem()
            listItem.Text = "Total memory"
            listItem.SubItems.Add(StrFormatByteSize(info.TotalMemory))
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            listItem = New ListViewItem()
            listItem.Text = "Used memory"
            listItem.SubItems.Add(StrFormatByteSize(info.UsedMemory))
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            listItem = New ListViewItem()
            listItem.Text = "Number of files"
            listItem.SubItems.Add(info.NumberOfFiles)
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            listItem = New ListViewItem()
            listItem.Text = "Number of folders"
            listItem.SubItems.Add(info.NumberOfFolders)
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            listItem = New ListViewItem()
            listItem.Text = "Size of folder content"
            listItem.SubItems.Add(StrFormatByteSize(info.SizeOfFolderContent))
            dlgFileInfo.ListView1.Items.Insert(iItemCount, listItem)
            iItemCount += 1

            dlgFileInfo.ShowDialog(Me)

        End If
    End Sub

    Private Sub cboDevice_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDevice.SelectedIndexChanged
        'If LBX_PhoneFiles.GetState = PHONELIST_STATE_PHONELIST Then
        LBX_PhoneFiles.CurrentDevice = Me.GetCurrentDevice
        'End If
    End Sub
End Class
