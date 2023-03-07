'Filename    : PIMNavigator.vb
'Part of     : PCSAPI VB.NET examples
'Description : Implementation of PIM Navigator main dialog
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

Imports DMD.Nokia
Imports DMD.Internals


Public Class PIMNavigator
    Inherits System.Windows.Forms.Form

    Delegate Sub InsertNotificationDelegate(ByVal strNotification As String)

    Private Const m_iIconPhoneIndex As Integer = 0
    Private Const m_iIconNoPhoneIndex As Integer = 1
    Private Const m_iIconContactsIndex As Integer = 2
    Private Const m_iIconContactIndex As Integer = 3
    Private Const m_iIconSMSMessagesIndex As Integer = 4
    Private Const m_iIconSMSIndex As Integer = 5
    Private Const m_iIconMMSMessagesIndex As Integer = 6
    Private Const m_iIconMMSIndex As Integer = 7
    Private Const m_iIconCalendarIndex As Integer = 8
    Private Const m_iIconCalendarItemIndex As Integer = 9
    Private Const m_iIconBookmarkIndex As Integer = 10
    Private Const m_iIconBookmarkItemIndex As Integer = 11

    Private m_strFile As String
    Friend WithEvents BTN_NewFolder As System.Windows.Forms.Button
    Private bRefresh As Boolean = False
    Friend WithEvents ButtonNotifications As System.Windows.Forms.Button
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents DeviceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FileBrowserToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents InstallerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PhoneNavigatorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    'Private NotificationsDialog As NotificationsDlg

    '===================================================================
    '
    ' Structure to map CA_ITEM_ID "permanently" into managed memory 
    ' 
    '===================================================================
    Private Structure CAItemID
        Dim iFolderId As Integer
        Dim iTemporaryID As Integer
        Dim abUID() As Byte
        Dim iStatus As Integer
    End Structure




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
            Static fTerminateCalled As Boolean
            If Not fTerminateCalled Then
                fTerminateCalled = True
            End If
            If Not components Is Nothing Then
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
    Friend WithEvents TVW_Navigator As System.Windows.Forms.TreeView
    Friend WithEvents LVW_ItemList As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents BTN_Close As System.Windows.Forms.Button
    Friend WithEvents BTN_New As System.Windows.Forms.Button
    Friend WithEvents BTN_Delete As System.Windows.Forms.Button
    Friend WithEvents BTN_Refresh As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents BTN_Save As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PIMNavigator))
        Me.TVW_Navigator = New System.Windows.Forms.TreeView()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.LVW_ItemList = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.BTN_Close = New System.Windows.Forms.Button()
        Me.BTN_New = New System.Windows.Forms.Button()
        Me.BTN_Delete = New System.Windows.Forms.Button()
        Me.BTN_Refresh = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.BTN_Save = New System.Windows.Forms.Button()
        Me.BTN_NewFolder = New System.Windows.Forms.Button()
        Me.ButtonNotifications = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.DeviceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FileBrowserToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InstallerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PhoneNavigatorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TVW_Navigator
        '
        Me.TVW_Navigator.Dock = System.Windows.Forms.DockStyle.Left
        Me.TVW_Navigator.ImageIndex = 0
        Me.TVW_Navigator.ImageList = Me.ImageList1
        Me.TVW_Navigator.Location = New System.Drawing.Point(0, 0)
        Me.TVW_Navigator.Name = "TVW_Navigator"
        Me.TVW_Navigator.SelectedImageIndex = 0
        Me.TVW_Navigator.Size = New System.Drawing.Size(264, 393)
        Me.TVW_Navigator.TabIndex = 0
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "")
        Me.ImageList1.Images.SetKeyName(2, "")
        Me.ImageList1.Images.SetKeyName(3, "")
        Me.ImageList1.Images.SetKeyName(4, "")
        Me.ImageList1.Images.SetKeyName(5, "")
        Me.ImageList1.Images.SetKeyName(6, "")
        Me.ImageList1.Images.SetKeyName(7, "")
        Me.ImageList1.Images.SetKeyName(8, "")
        Me.ImageList1.Images.SetKeyName(9, "")
        Me.ImageList1.Images.SetKeyName(10, "Bookmarks.ico")
        Me.ImageList1.Images.SetKeyName(11, "Bookmark.ico")
        '
        'LVW_ItemList
        '
        Me.LVW_ItemList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.LVW_ItemList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LVW_ItemList.Location = New System.Drawing.Point(264, 0)
        Me.LVW_ItemList.Name = "LVW_ItemList"
        Me.LVW_ItemList.Size = New System.Drawing.Size(572, 393)
        Me.LVW_ItemList.TabIndex = 1
        Me.LVW_ItemList.UseCompatibleStateImageBehavior = False
        Me.LVW_ItemList.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Field"
        Me.ColumnHeader1.Width = 123
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Data"
        Me.ColumnHeader2.Width = 168
        '
        'BTN_Close
        '
        Me.BTN_Close.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BTN_Close.Location = New System.Drawing.Point(1011, 5)
        Me.BTN_Close.Name = "BTN_Close"
        Me.BTN_Close.Size = New System.Drawing.Size(74, 24)
        Me.BTN_Close.TabIndex = 2
        Me.BTN_Close.Text = "Close"
        '
        'BTN_New
        '
        Me.BTN_New.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BTN_New.Enabled = False
        Me.BTN_New.Location = New System.Drawing.Point(519, 5)
        Me.BTN_New.Name = "BTN_New"
        Me.BTN_New.Size = New System.Drawing.Size(74, 24)
        Me.BTN_New.TabIndex = 3
        Me.BTN_New.Text = "New"
        '
        'BTN_Delete
        '
        Me.BTN_Delete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BTN_Delete.Enabled = False
        Me.BTN_Delete.Location = New System.Drawing.Point(683, 5)
        Me.BTN_Delete.Name = "BTN_Delete"
        Me.BTN_Delete.Size = New System.Drawing.Size(74, 24)
        Me.BTN_Delete.TabIndex = 4
        Me.BTN_Delete.Text = "Delete"
        '
        'BTN_Refresh
        '
        Me.BTN_Refresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BTN_Refresh.Location = New System.Drawing.Point(929, 5)
        Me.BTN_Refresh.Name = "BTN_Refresh"
        Me.BTN_Refresh.Size = New System.Drawing.Size(74, 24)
        Me.BTN_Refresh.TabIndex = 5
        Me.BTN_Refresh.Text = "Refresh"
        '
        'Timer1
        '
        '
        'BTN_Save
        '
        Me.BTN_Save.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BTN_Save.Enabled = False
        Me.BTN_Save.Location = New System.Drawing.Point(765, 5)
        Me.BTN_Save.Name = "BTN_Save"
        Me.BTN_Save.Size = New System.Drawing.Size(74, 24)
        Me.BTN_Save.TabIndex = 6
        Me.BTN_Save.Text = "Save to File"
        '
        'BTN_NewFolder
        '
        Me.BTN_NewFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BTN_NewFolder.Enabled = False
        Me.BTN_NewFolder.Location = New System.Drawing.Point(601, 5)
        Me.BTN_NewFolder.Name = "BTN_NewFolder"
        Me.BTN_NewFolder.Size = New System.Drawing.Size(74, 24)
        Me.BTN_NewFolder.TabIndex = 7
        Me.BTN_NewFolder.Text = "New folder"
        '
        'ButtonNotifications
        '
        Me.ButtonNotifications.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonNotifications.Location = New System.Drawing.Point(847, 5)
        Me.ButtonNotifications.Name = "ButtonNotifications"
        Me.ButtonNotifications.Size = New System.Drawing.Size(74, 24)
        Me.ButtonNotifications.TabIndex = 8
        Me.ButtonNotifications.Text = "Notifications"
        '
        'PictureBox1
        '
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Right
        Me.PictureBox1.Location = New System.Drawing.Point(836, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(250, 393)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 9
        Me.PictureBox1.TabStop = False
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeviceToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1086, 24)
        Me.MenuStrip1.TabIndex = 10
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'DeviceToolStripMenuItem
        '
        Me.DeviceToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileBrowserToolStripMenuItem, Me.InstallerToolStripMenuItem, Me.PhoneNavigatorToolStripMenuItem})
        Me.DeviceToolStripMenuItem.Name = "DeviceToolStripMenuItem"
        Me.DeviceToolStripMenuItem.Size = New System.Drawing.Size(54, 20)
        Me.DeviceToolStripMenuItem.Text = "Device"
        '
        'FileBrowserToolStripMenuItem
        '
        Me.FileBrowserToolStripMenuItem.Name = "FileBrowserToolStripMenuItem"
        Me.FileBrowserToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.FileBrowserToolStripMenuItem.Text = "&File Browser"
        '
        'InstallerToolStripMenuItem
        '
        Me.InstallerToolStripMenuItem.Name = "InstallerToolStripMenuItem"
        Me.InstallerToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.InstallerToolStripMenuItem.Text = "Installer"
        '
        'PhoneNavigatorToolStripMenuItem
        '
        Me.PhoneNavigatorToolStripMenuItem.Name = "PhoneNavigatorToolStripMenuItem"
        Me.PhoneNavigatorToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.PhoneNavigatorToolStripMenuItem.Text = "&Phone Navigator"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.BTN_New)
        Me.Panel1.Controls.Add(Me.BTN_Close)
        Me.Panel1.Controls.Add(Me.ButtonNotifications)
        Me.Panel1.Controls.Add(Me.BTN_Delete)
        Me.Panel1.Controls.Add(Me.BTN_NewFolder)
        Me.Panel1.Controls.Add(Me.BTN_Refresh)
        Me.Panel1.Controls.Add(Me.BTN_Save)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 417)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1086, 35)
        Me.Panel1.TabIndex = 11
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.LVW_ItemList)
        Me.Panel2.Controls.Add(Me.PictureBox1)
        Me.Panel2.Controls.Add(Me.TVW_Navigator)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 24)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1086, 393)
        Me.Panel2.TabIndex = 12
        '
        'PIMNavigator
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(1086, 452)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "PIMNavigator"
        Me.Text = "PIMNavigator"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    <STAThread()> _
    Shared Sub Main()
        ' Starts the application.
        Application.Run(New PIMNavigator)
    End Sub



    '===================================================================
    ' AddFolder
    '
    ' Adds folder to tree view and calls recursively itself for subfolders.
    '
    '===================================================================
    Private Sub AddFolder(ByVal strRootFolder As String, ByVal folderInfo As CBaseFolder, ByVal parentItem As System.Windows.Forms.TreeNode, ByVal iIconFolderIndex As Integer, ByVal iIconItemIndex As Integer)
        ' Insert folder item in tree view
        Dim strFolderName As String
        If folderInfo.Name = "" OrElse folderInfo.Name = "\" Then
            strFolderName = strRootFolder
        Else
            strFolderName = folderInfo.Name
        End If

        ' Insert target folder item in tree view
        Dim itemY As New System.Windows.Forms.TreeNode
        itemY.Text = strFolderName
        itemY.ImageIndex = iIconFolderIndex
        itemY.SelectedImageIndex = iIconFolderIndex
        itemY.Tag = folderInfo
        parentItem.Nodes.Add(itemY)

        ' Add dummy item to get '+' showed.
        Dim itemZ As New System.Windows.Forms.TreeNode
        itemZ.Text = ""
        itemZ.ImageIndex = iIconItemIndex
        itemZ.SelectedImageIndex = iIconItemIndex
        itemZ.Tag = Nothing
        itemY.Nodes.Add(itemZ)
        Dim i As Integer

        For i = 0 To folderInfo.SubFolders.Count - 1
            ' Recursive call for adding subfolders.
            Dim subFolderInfo As CBaseFolder = folderInfo.SubFolders(i)

            AddFolder(strRootFolder, subFolderInfo, itemY, iIconFolderIndex, iIconItemIndex)
        Next

    End Sub

    '===================================================================
    ' GetContactsFolder
    '
    ' Gets Contacts folder info and creates folder in tree view
    '
    '===================================================================
    Private Sub GetContactsFolder(ByVal parentItem As System.Windows.Forms.TreeNode)
        Dim device As NokiaDevice = parentItem.Tag
        AddFolder("Contacts", device.Contacts, parentItem, m_iIconContactsIndex, m_iIconContactIndex)
    End Sub

    '===================================================================
    ' GetSMSFolders
    '
    ' Gets SMS folder info and creates folders in tree view
    '
    '===================================================================
    Private Sub GetSMSFolders(ByVal parentItem As System.Windows.Forms.TreeNode)
        Dim device As NokiaDevice = parentItem.Tag
        AddFolder("SMS Messages", device.SMS, parentItem, m_iIconSMSMessagesIndex, m_iIconSMSIndex)
    End Sub

    '===================================================================
    ' GetMMSFolders
    '
    ' Gets SMS folder info and creates folders in tree view
    '
    '===================================================================
    Private Sub GetMMSFolders(ByVal parentItem As System.Windows.Forms.TreeNode)
        Dim device As NokiaDevice = parentItem.Tag
        AddFolder("MMS Messages", device.MMS, parentItem, m_iIconMMSMessagesIndex, m_iIconMMSIndex)
    End Sub

    '===================================================================
    ' GetCalendarFolder
    '
    ' Gets Calendar folder info and creates folder in tree view
    '
    '===================================================================
    Private Sub GetCalendarFolder(ByVal parentItem As System.Windows.Forms.TreeNode)
        Dim device As NokiaDevice = parentItem.Tag
        AddFolder("Calendar", device.Calendar, parentItem, m_iIconCalendarIndex, m_iIconCalendarItemIndex)
    End Sub

    '===================================================================
    ' GetBookmarkFolder
    '
    ' Gets Bookmarks folder info and creates folder in tree view
    '
    '===================================================================
    Private Sub GetBookmarkFolder(ByVal parentItem As System.Windows.Forms.TreeNode)
        Dim device As NokiaDevice = parentItem.Tag
        ' Check PIM connection to Calendar folders and open it if needed
        AddFolder("Bookmarks", device.BookMarks, parentItem, m_iIconBookmarkIndex, m_iIconBookmarkItemIndex)
    End Sub


    '===================================================================
    ' GetContactDetails
    '
    ' Read selected contact from phone and show details in list view.
    '
    '===================================================================
    Private Sub GetContactDetails(ByVal dataContact As CContactItem)
        ' Read contact item data from device
        ' Personal information
        Dim itemX As System.Windows.Forms.ListViewItem
        If (dataContact.BirthDate.HasValue) Then
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = "BirthDate"
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(dataContact.BirthDate.Value.ToString)
        End If
        If (dataContact.ContactName <> vbNullString) Then
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = "ContactName"
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(dataContact.ContactName)
        End If
        If (dataContact.Suffix <> vbNullString) Then
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = "Suffix"
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(dataContact.Suffix)
        End If
        If (dataContact.FirstName <> vbNullString) Then
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = "FistName"
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(dataContact.FirstName)
        End If
        If (dataContact.MiddleName <> vbNullString) Then
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = "MiddleName"
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(dataContact.MiddleName)
        End If
        If (dataContact.LastName <> vbNullString) Then
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = "LastName"
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(dataContact.LastName)
        End If
        If (dataContact.Title <> vbNullString) Then
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = "Title"
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(dataContact.Title)
        End If
        If (dataContact.NickName <> vbNullString) Then
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = "NickName"
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(dataContact.NickName)
        End If
        If (dataContact.FormalName <> vbNullString) Then
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = "FormalName"
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(dataContact.FormalName)
        End If

        If (dataContact.Company <> vbNullString) Then
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = "Company"
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(dataContact.Company)
        End If
        If (dataContact.JobTitle <> vbNullString) Then
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = "JobTitle"
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(dataContact.JobTitle)
        End If

        If (dataContact.NamePronunciation <> vbNullString) Then
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = "NamePronunciation"
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(dataContact.NamePronunciation)
        End If
        If (dataContact.FamilyNamePronunciation <> vbNullString) Then
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = "FamilyNamePronunciation"
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(dataContact.FamilyNamePronunciation)
        End If
        If (dataContact.CompanyNamePronunciation <> vbNullString) Then
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = "CompanyNamePronunciation"
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(dataContact.CompanyNamePronunciation)
        End If

        If (dataContact.Picture IsNot Nothing) Then
            PictureBox1.Image = dataContact.Picture
            Dim size1 As System.Drawing.Size = Me.Size
            Dim size2 As System.Drawing.Size = PictureBox1.Size
            size1.Width = size1.Width + size2.Width + 15
            Me.Size = size1
        End If


        '' Numbers
        For Each number As ItemData In dataContact.Numbers
            ' Add item to list view
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = number.Tipo ' PIMFieldType2String(itemData)
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(number.Valore)
        Next


        ' Addresses
        For Each postal As CPostalAddress In dataContact.Addresses
            ' Add item to list view
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = postal.Tipo ' PIMFieldType2String(itemData)
            LVW_ItemList.Items.Add(itemX)
            '    If itemData.iFieldSubType = CA_FIELD_SUB_TYPE_POSTAL Or itemData.iFieldSubType = CA_FIELD_SUB_TYPE_POSTAL_BUSINESS Or itemData.iFieldSubType = CA_FIELD_SUB_TYPE_POSTAL_PRIVATE Then
            If postal.POBox <> vbNullString Then
                Dim item1 As New System.Windows.Forms.ListViewItem
                item1.Text = "    PO Box"
                item1.SubItems.Add(postal.POBox)
                LVW_ItemList.Items.Add(item1)
            End If
            If postal.Street <> vbNullString Then
                Dim item1 As New System.Windows.Forms.ListViewItem
                item1.Text = "    Street"
                item1.SubItems.Add(postal.Street)
                LVW_ItemList.Items.Add(item1)
            End If
            If postal.PostalCode <> vbNullString Then
                Dim item1 As New System.Windows.Forms.ListViewItem
                item1.Text = "    Postal code"
                item1.SubItems.Add(postal.PostalCode)
                LVW_ItemList.Items.Add(item1)
            End If
            If postal.City <> vbNullString Then
                Dim item1 As New System.Windows.Forms.ListViewItem
                item1.Text = "    City"
                item1.SubItems.Add(postal.City)
                LVW_ItemList.Items.Add(item1)
            End If
            If Not postal.State Is Nothing Then
                Dim item1 As New System.Windows.Forms.ListViewItem
                item1.Text = "    State"
                item1.SubItems.Add(postal.State)
                LVW_ItemList.Items.Add(item1)
            End If
            If Not postal.Country Is Nothing Then
                Dim item1 As New System.Windows.Forms.ListViewItem
                item1.Text = "    Country"
                item1.SubItems.Add(postal.Country)
                LVW_ItemList.Items.Add(item1)
            End If
            If Not postal.ExtendedData Is Nothing Then
                Dim item1 As New System.Windows.Forms.ListViewItem
                item1.Text = "    Extended address information"
                item1.SubItems.Add(postal.ExtendedData)
                LVW_ItemList.Items.Add(item1)
            End If
        Next

        ' General information
        For Each valore As ItemData In dataContact.GeneralInformations
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = valore.Tipo
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(valore.Valore)
        Next

    End Sub



    '===================================================================
    ' GetCalendarDetails
    '
    ' Read selected calendar item from phone and show details in list view.
    '
    '===================================================================
    Private Sub GetCalendarDetails(ByVal dataCalendar As CCalendarItem)
        ' Calendar item type
        Dim itemX As New System.Windows.Forms.ListViewItem
        itemX.Text = "Item type"
        LVW_ItemList.Items.Add(itemX)
        itemX.SubItems.Add(dataCalendar.ItemTypeEx) ' CalendarItemType2String(dataCalendar.iInfoField))
        ' Get note start date and format it
        If dataCalendar.StartDate.HasValue Then
            Dim item1 As New System.Windows.Forms.ListViewItem
            item1.Text = "Start date"
            LVW_ItemList.Items.Add(item1)
            item1.SubItems.Add(dataCalendar.StartDate.Value.ToString)
        End If
        ' Get note end date and format it
        If dataCalendar.EndDate.HasValue Then
            Dim item2 As New System.Windows.Forms.ListViewItem
            item2.Text = "End date"
            LVW_ItemList.Items.Add(item2)
            item2.SubItems.Add(dataCalendar.EndDate.Value.ToString)
        End If
        ' Get note alarm time and format it
        If dataCalendar.AlarmTime.HasValue Then
            Dim item3 As New System.Windows.Forms.ListViewItem
            item3.Text = "Alarm date"
            LVW_ItemList.Items.Add(item3)
            item3.SubItems.Add(dataCalendar.AlarmTime.Value.ToString)
        End If
        ' Show recurrence
        Dim item4 As New System.Windows.Forms.ListViewItem
        item4.Text = "Recurrence"
        LVW_ItemList.Items.Add(item4)
        item4.SubItems.Add(dataCalendar.RecurrenceTypeEx) ' PIMCalendarRecurrence2String(dataCalendar.iRecurrence))
        ' Get recurrence end date and format it
        If dataCalendar.RecurrenceDate.HasValue Then
            Dim item5 As New System.Windows.Forms.ListViewItem
            item5.Text = "Recurrence end date"
            LVW_ItemList.Items.Add(item5)
            item5.SubItems.Add(dataCalendar.RecurrenceDate.Value.ToString)
        End If

        '' Get SubItems
        If dataCalendar.Description <> vbNullString Then
            Dim item6 As New System.Windows.Forms.ListViewItem
            item6.Text = "Description"
            LVW_ItemList.Items.Add(item6)
            item6.SubItems.Add(dataCalendar.Description)
        End If
        If (dataCalendar.Location <> vbNullString) Then
            Dim item7 As New System.Windows.Forms.ListViewItem
            item7.Text = "Location"
            LVW_ItemList.Items.Add(item7)
            item7.SubItems.Add(dataCalendar.Location)
        End If
        If (dataCalendar.Detail <> vbNullString) Then
            Dim item8 As New System.Windows.Forms.ListViewItem
            item8.Text = "Detail"
            LVW_ItemList.Items.Add(item8)
            item8.SubItems.Add(dataCalendar.Detail)
        End If
        If (dataCalendar.Number <> vbNullString) Then
            Dim item9 As New System.Windows.Forms.ListViewItem
            item9.Text = "Number"
            LVW_ItemList.Items.Add(item9)
            item9.SubItems.Add(dataCalendar.Number)
        End If
        If dataCalendar.Year.HasValue Then
            Dim item10 As New System.Windows.Forms.ListViewItem
            item10.Text = "Year"
            LVW_ItemList.Items.Add(item10)
            item10.SubItems.Add(dataCalendar.Year.Value.ToString())
        End If
        If (dataCalendar.Priority.HasValue) Then
            Dim item11 As New System.Windows.Forms.ListViewItem
            item11.Text = "Priority"
            LVW_ItemList.Items.Add(item11)
            item11.SubItems.Add(dataCalendar.PriorityEx)
        End If
        If (dataCalendar.ToDoStatusEx <> "") Then
            Dim item12 As New System.Windows.Forms.ListViewItem
            item12.Text = "Status"
            LVW_ItemList.Items.Add(item12)
            item12.SubItems.Add(dataCalendar.ToDoStatusEx)
        End If

    End Sub

    '===================================================================
    ' GetBookmarkDetails
    '
    ' Read selected Bookmark item from phone and show details in list view.
    '
    '===================================================================
    Private Sub GetBookmarkDetails(ByVal dataBookmark As CBookMarkItem)
        ' Bookmark data
        Dim itemA As New System.Windows.Forms.ListViewItem
        itemA.Text = "Title"
        LVW_ItemList.Items.Add(itemA)
        itemA.SubItems.Add(dataBookmark.Title)
        Dim itemB As New System.Windows.Forms.ListViewItem
        itemB.Text = "URL"
        LVW_ItemList.Items.Add(itemB)
        itemB.SubItems.Add(dataBookmark.BookMarkUrl)
        Dim itemC As New System.Windows.Forms.ListViewItem
        itemC.Text = "URL Shortcut"
        LVW_ItemList.Items.Add(itemC)
        itemC.SubItems.Add(dataBookmark.UrlShortCut)
    End Sub



    '===================================================================
    ' GetSMSDetails
    '
    ' Read selected SMS from phone and show details in list view.
    '
    '===================================================================
    Private Sub GetSMSDetails(ByVal dataMsg As CSMSMessage)
        Dim itemX As System.Windows.Forms.ListViewItem

        ' Addresses
        For Each address As String In dataMsg.Addresses
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = "Address"
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(address)
        Next

        itemX = New System.Windows.Forms.ListViewItem
        itemX.Text = "Message"
        LVW_ItemList.Items.Add(itemX)
        itemX.SubItems.Add(dataMsg.Message)
        itemX = New System.Windows.Forms.ListViewItem
        itemX.Text = "Date"
        LVW_ItemList.Items.Add(itemX)
        itemX.SubItems.Add(dataMsg.Date.ToString)

        Dim itemY As New System.Windows.Forms.ListViewItem
        itemY.Text = "Status"
        LVW_ItemList.Items.Add(itemY)
        itemY.SubItems.Add(dataMsg.StatusEx)

        'If dataMsg.
    End Sub


    '===================================================================
    ' GetMMSDetails
    '
    ' Read selected MMS from phone and show details in list view.
    '
    '===================================================================
    Private Sub GetMMSDetails(ByVal dataMsg As CMMSMessage)
        Dim itemX As System.Windows.Forms.ListViewItem

        ' Addresses
        For Each address As String In dataMsg.Addresses
            ' Add item to list view
            itemX = New System.Windows.Forms.ListViewItem
            itemX.Text = "Address"
            LVW_ItemList.Items.Add(itemX)
            itemX.SubItems.Add(address)
        Next

        ' Get message date and format it
        ' Add item to list view
        itemX = New System.Windows.Forms.ListViewItem
        itemX.Text = "Date"
        LVW_ItemList.Items.Add(itemX)
        itemX.SubItems.Add(dataMsg.Data.ToString)

        ' Message status
        Dim itemY As New System.Windows.Forms.ListViewItem
        itemY.Text = "Status"
        LVW_ItemList.Items.Add(itemY)
        itemY.SubItems.Add(dataMsg.StatusEx)


    End Sub

    '===================================================================
    ' RefreshTreeView
    '
    ' Refresh phone list to combo
    '
    '===================================================================

    Public Sub RefreshTreeView()
        ' Clear all previous data
        Timer1.Enabled = False
        TVW_Navigator.Enabled = False

        ClearListView()
        TVW_Navigator.Nodes.Clear()
        TVW_Navigator.Enabled = False
        Cursor = System.Windows.Forms.Cursors.WaitCursor

        ' Add each device to the tree view
        For Each device As NokiaDevice In DMD.Nokia.Devices
            ' Insert phone item in tree view
            Dim itemX As New System.Windows.Forms.TreeNode
            itemX.Text = device.FriendlyName
            itemX.ImageIndex = m_iIconPhoneIndex
            itemX.SelectedImageIndex = m_iIconPhoneIndex
            itemX.Tag = device

            TVW_Navigator.Nodes.Add(itemX)

            GetContactsFolder(itemX)
            GetSMSFolders(itemX)
            GetMMSFolders(itemX)
            GetCalendarFolder(itemX)
            GetBookmarkFolder(itemX)
        Next

        If DMD.Nokia.Devices.Count = 0 Then
            Dim itemX As New System.Windows.Forms.TreeNode
            itemX.Text = "No phones connected"
            itemX.ImageIndex = m_iIconNoPhoneIndex
            itemX.SelectedImageIndex = m_iIconNoPhoneIndex
            TVW_Navigator.Nodes.Add(itemX)
        Else
            TVW_Navigator.Enabled = True
        End If
        Cursor = System.Windows.Forms.Cursors.Default
        Timer1.Enabled = True
        TVW_Navigator.Enabled = True
    End Sub

    '===================================================================
    ' GetCurrentDevice
    '
    ' Get's device serial number for currently selected TreeNode
    '
    '===================================================================
    Private Function GetCurrentDevice() As NokiaDevice
        Return Me.GetCurrentDevice(TVW_Navigator.SelectedNode)
    End Function

    '===================================================================
    ' GetCurrentDevice
    '
    ' Get's device serial number for currently given TreeNode
    '
    '===================================================================
    Private Function GetCurrentDevice(ByVal gItem As System.Windows.Forms.TreeNode) As NokiaDevice
        Dim item As System.Windows.Forms.TreeNode = gItem
        While Not (item Is Nothing) AndAlso Not (TypeOf (item.Tag) Is NokiaDevice)
            item = item.Parent
        End While
        Return Nothing
    End Function

    '===================================================================
    ' ClearListView
    '
    ' Clear previous details
    '
    '===================================================================
    Sub ClearListView()
        If PictureBox1.Image IsNot Nothing Then
            Dim size1 As System.Drawing.Size = Me.Size
            Dim size2 As System.Drawing.Size = PictureBox1.Size
            PictureBox1.Image.Dispose()
            PictureBox1.Image = Nothing
            size1.Width = size1.Width - size2.Width - 15
            Me.Size = size1
            My.Computer.FileSystem.DeleteFile(m_strFile)
        End If
        LVW_ItemList.Items.Clear()
    End Sub

    '===================================================================
    ' TVW_Navigator_AfterSelect
    '
    ' User has selected item in tree view. If item is contact or SMS
    ' message, show details in list view.
    '
    '===================================================================
    Private Sub TVW_Navigator_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TVW_Navigator.AfterSelect
        ClearListView()
        ' Disable "New" and "Delete" buttons
        BTN_New.Enabled = False
        BTN_Delete.Enabled = False
        BTN_Save.Enabled = False
        BTN_NewFolder.Enabled = False

        Dim item As System.Windows.Forms.TreeNode = e.Node
        If (TypeOf (item.Tag) Is CContacts) Then
            BTN_New.Enabled = True
        ElseIf (TypeOf (item.Tag) Is CCalendar) Then
            ' Check PIM connection to Calendar folders and open it if needed
            ' Calendar folder selected
            BTN_New.Enabled = True
        ElseIf (TypeOf (item.Tag) Is CBookMarks) Then
            ' Check PIM connection to Bookmark folders and open it if needed
            ' Bookmark folder selected
            BTN_New.Enabled = True
            BTN_NewFolder.Enabled = True
            BTN_Delete.Enabled = True
        ElseIf (TypeOf (item.Tag) Is CSMSFolder) Then
            ' Check PIM connection to SMS folders and open it if needed
            ' SMS folder selected
            If Not TypeOf (item.Tag) Is CSMS Then
                BTN_New.Enabled = True
            End If
            BTN_NewFolder.Enabled = True
        ElseIf (TypeOf (item.Tag) Is CMMSFolder) Then
            ' Check PIM connection to MMS folders and open it if needed
            ' MMS folder selected
            BTN_New.Enabled = False
        ElseIf (TypeOf (item.Tag) Is CContactItem) Then
            ' Contact item is selected
            ' Check PIM connection to contacts folder and open it if needed
            GetContactDetails(TVW_Navigator.SelectedNode.Tag)
            BTN_Delete.Enabled = True
            BTN_Save.Enabled = True
        ElseIf (TypeOf (item.Tag) Is CCalendarItem) Then
            ' Calendar item is selected
            GetCalendarDetails(TVW_Navigator.SelectedNode.Tag)
            ' PIM item is selected, delete button can be enabled
            BTN_Delete.Enabled = True
            BTN_Save.Enabled = True
            BTN_New.Enabled = True
        ElseIf (TypeOf (item.Tag) Is CBookMarkItem) Then
            ' Calendar item is selected
            GetBookmarkDetails(TVW_Navigator.SelectedNode.Tag)
            ' PIM item is selected, delete button can be enabled
            BTN_Delete.Enabled = True
            BTN_Save.Enabled = True
            BTN_New.Enabled = True
        ElseIf (TypeOf (item.Tag) Is CSMSMessage) Then
            ' SMS item is selected
            GetSMSDetails(TVW_Navigator.SelectedNode.Tag)
            ' PIM item is selected, delete button can be enabled
            BTN_Delete.Enabled = True
            BTN_Save.Enabled = True
        ElseIf (TypeOf (item.Tag) Is CMMSMessage) Then
            ' MMS item is selected
            GetMMSDetails(TVW_Navigator.SelectedNode.Tag)
            ' PIM item is selected, delete button can be enabled
            BTN_Delete.Enabled = True
            BTN_Save.Enabled = True
        End If
        Cursor = System.Windows.Forms.Cursors.Default
    End Sub


    '===================================================================
    ' ShowNewContactDlg
    '
    ' Shows "New Contact" dialog and writes contact to device
    '
    '===================================================================
    Private Sub ShowNewContactDlg()
        '' Open "New Contact" dialog
        'Dim dlg As New ContactDlg

        '' Set birthday value to next week, if changed save value
        'Dim dtInitialDate As DateTime = DateTime.Now.AddDays(7)
        'dlg.DTP_Birthday.Value = dtInitialDate

        'If dlg.ShowDialog() <> Windows.Forms.DialogResult.OK Then
        '    Exit Sub
        'End If

        '' User has filled contact information in dialog and clicked OK
        'CheckContactsConnection(GetCurrentDevice())
        'Dim dataContact As CA_DATA_CONTACT
        'dataContact.iSize = Marshal.SizeOf(dataContact)

        '' Contact name
        'dataContact.bPICount = 0
        'If dlg.TXT_FirstName.Text.Length > 0 Then dataContact.bPICount += 1
        'If dlg.TXT_LastName.Text.Length > 0 Then dataContact.bPICount += 1
        'If dlg.TXT_Job.Text.Length > 0 Then dataContact.bPICount += 1
        'If dlg.TXT_Company.Text.Length > 0 Then dataContact.bPICount += 1
        'If dlg.DTP_Birthday.Value <> dtInitialDate Then dataContact.bPICount += 1
        'If dataContact.bPICount > 0 Then
        '    dataContact.pPIFields = GetContactPIMBuffer(dataContact.bPICount, dlg.TXT_FirstName.Text, dlg.TXT_LastName.Text, dlg.TXT_Job.Text, dlg.TXT_Company.Text, dlg.DTP_Birthday.Value)
        'End If

        '' Phone numbers
        'Dim iCount As Integer = 0
        'If dlg.TXT_General.Text.Length > 0 Then iCount += 1
        'If dlg.TXT_Mobile.Text.Length > 0 Then iCount += 1
        'If dlg.TXT_Home.Text.Length > 0 Then iCount += 1
        'If dlg.TXT_Work.Text.Length > 0 Then iCount += 1
        'If dlg.TXT_Fax.Text.Length > 0 Then iCount += 1
        'If iCount > 0 Then
        '    dataContact.bNumberCount = iCount
        '    dataContact.pNumberFields = GetContactNumberBuffer(iCount, dlg.TXT_General.Text, dlg.TXT_Mobile.Text, dlg.TXT_Home.Text, dlg.TXT_Work.Text, dlg.TXT_Fax.Text)
        'End If

        '' Address
        'Dim bHasAddress As Boolean = False
        'Dim iACount As Integer = 0
        'If dlg.strPOBox.Length > 0 Or dlg.strPostalCode.Length > 0 Or dlg.strStreet.Length > 0 Or _
        '   dlg.strCity.Length > 0 Or dlg.strState.Length > 0 Or dlg.strCountry.Length > 0 Or _
        '   dlg.strExtentedData.Length > 0 Then
        '    bHasAddress = True
        '    iACount += 1
        'End If
        'If dlg.TXT_Email.Text.Length > 0 Then
        '    iACount += 1
        'End If
        'If dlg.TXT_Web.Text.Length > 0 Then
        '    iACount += 1
        'End If
        'If iACount > 0 Then
        '    dataContact.bAddressCount = iACount
        '    dataContact.pAddressFields = GetContactAddressBuffer(iACount, dlg.TXT_Email.Text, dlg.TXT_Web.Text, bHasAddress, dlg.strPOBox, dlg.strPostalCode, dlg.strStreet, dlg.strCity, dlg.strState, dlg.strCountry, dlg.strExtentedData)
        'End If

        '' Just one item, but let's make room for future extensions
        'Dim iGCount As Integer = 0
        'If dlg.TXT_Note.Text.Length > 0 Then
        '    iGCount += 1
        'End If
        'If iGCount > 0 Then
        '    dataContact.bGeneralCount = iGCount
        '    dataContact.pGeneralFields = GetContactGeneralBuffer(iGCount, dlg.TXT_Note.Text)
        'End If

        'Dim contactsNode As System.Windows.Forms.TreeNode
        'If TVW_Navigator.SelectedNode.ImageIndex = m_iIconContactsIndex Then
        '    contactsNode = TVW_Navigator.SelectedNode
        'Else
        '    contactsNode = TVW_Navigator.SelectedNode.Parent
        'End If
        'Dim folderInfo As CA_FOLDER_INFO = MapCAFolderInfoToCFI(contactsNode.Tag)

        '' Write new contact item to currently connected device
        'Dim hOperHandle As Integer = 0
        'Dim iRet As Integer = CABeginOperation(m_hCurrentConnection, 0, hOperHandle)
        'If iRet <> CONA_OK Then ShowErrorMessage("CABeginOperation", iRet)
        '' Register CA operation notification callback function
        'CARegisterOperationCallback(hOperHandle, API_REGISTER, pCAOperationCallback)
        'If iRet <> CONA_OK Then ShowErrorMessage("CARegisterOperationCallback", iRet)
        'Dim itemUid As CA_ITEM_ID
        'itemUid.iSize = Marshal.SizeOf(itemUid)
        'itemUid.iFolderId = folderInfo.iFolderId
        'itemUid.iStatus = 0
        'itemUid.iTemporaryID = 0
        'itemUid.iUidLen = 0
        'itemUid.pbUid = IntPtr.Zero
        'Dim buf As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_ITEM_ID)))
        'Marshal.StructureToPtr(itemUid, buf, True)
        '' Allocate memory for buffer
        'Dim buf2 As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_DATA_CONTACT)))
        'Marshal.StructureToPtr(dataContact, buf2, True)
        'iRet = CAWriteItem(hOperHandle, buf, 0, CA_DATA_FORMAT_STRUCT, buf2)
        'If iRet <> CONA_OK Then ShowErrorMessage("DAWriteItem", iRet)
        'Try
        '    Marshal.FreeHGlobal(dataContact.pPIFields)
        '    Marshal.FreeHGlobal(dataContact.pNumberFields)
        '    Marshal.FreeHGlobal(dataContact.pAddressFields)
        '    Marshal.FreeHGlobal(dataContact.pGeneralFields)
        '    Marshal.FreeHGlobal(buf2)
        '    Marshal.FreeHGlobal(buf)
        'Catch
        'End Try
        'iRet = CACommitOperations(hOperHandle, IntPtr.Zero)
        'If iRet <> CONA_OK Then ShowErrorMessage("CACommitOperations", iRet)
        'CARegisterOperationCallback(hOperHandle, API_UNREGISTER, pCAOperationCallback)
        'If iRet <> CONA_OK Then ShowErrorMessage("CARegisterOperationCallback", iRet)
        'iRet = CAEndOperation(hOperHandle)
        'If iRet <> CONA_OK Then ShowErrorMessage("CAEndOperation", iRet)
        'contactsNode.Nodes.Clear()
        'GetContacts(GetCurrentDevice(), contactsNode)
    End Sub



    '===================================================================
    ' ShowNewSMSDlg
    '
    ' Shows "New Text Message" dialog and writes SMS to device
    '
    '===================================================================
    Private Sub ShowNewSMSDlg()
        '' Open "New Text Message" dialog
        'Dim dlg As New SmsMessageDlg
        'If dlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '    '
        '    CheckSMSConnection(GetCurrentDevice())
        '    Dim dataMsg As CA_DATA_MSG
        '    dataMsg.iSize = Marshal.SizeOf(dataMsg)
        '    CA_SET_DATA_FORMAT(dataMsg.iInfoField, CA_DATA_FORMAT_UNICODE)
        '    CA_SET_DATA_CODING(dataMsg.iInfoField, CA_DATA_CODING_UNICODE)
        '    CA_SET_MESSAGE_STATUS(dataMsg.iInfoField, CA_MESSAGE_STATUS_DRAFT)
        '    CA_SET_MESSAGE_TYPE(dataMsg.iInfoField, CA_SMS_SUBMIT)
        '    ' Phone number
        '    If (dlg.TXT_Number.Text.Length > 0) Then
        '        dataMsg.bAddressCount = 1
        '        dataMsg.pAddress = GetSMSAddressBuffer(dlg.TXT_Number.Text)
        '    End If
        '    ' Message body text
        '    Dim iLength As Integer = dlg.TXT_Message.Text.Length
        '    If (iLength > 0) Then
        '        dataMsg.iDataLength = iLength * 2
        '        dataMsg.pbData = Marshal.StringToCoTaskMemUni(dlg.TXT_Message.Text)
        '    End If
        '    ' Set message date
        '    GetCurrentPIMDate(dataMsg.messageDate)
        '    Dim smsNode As System.Windows.Forms.TreeNode
        '    If TVW_Navigator.SelectedNode.ImageIndex = m_iIconSMSMessagesIndex Then
        '        smsNode = TVW_Navigator.SelectedNode
        '    Else
        '        smsNode = TVW_Navigator.SelectedNode.Parent
        '    End If
        '    Dim folderInfo As CA_FOLDER_INFO = MapCAFolderInfoToCFI(smsNode.Tag)
        '    ' Write new SMS item to currently connected device
        '    Dim hOperHandle As Integer = 0
        '    Dim iRet As Integer = CABeginOperation(m_hCurrentConnection, 0, hOperHandle)
        '    If iRet <> CONA_OK Then ShowErrorMessage("CABeginOperation", iRet)
        '    CARegisterOperationCallback(hOperHandle, API_REGISTER, pCAOperationCallback)
        '    If iRet <> CONA_OK Then ShowErrorMessage("CARegisterOperationCallback", iRet)
        '    Dim itemUid As CA_ITEM_ID
        '    itemUid.iSize = Marshal.SizeOf(itemUid)
        '    itemUid.iFolderId = folderInfo.iFolderId
        '    itemUid.iFolderId = folderInfo.iFolderId
        '    itemUid.iStatus = 0
        '    itemUid.iTemporaryID = 0
        '    itemUid.iUidLen = 0
        '    itemUid.pbUid = IntPtr.Zero
        '    Dim buf As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_ITEM_ID)))
        '    Marshal.StructureToPtr(itemUid, buf, True)
        '    ' Allocate memory for buffer
        '    Dim buf2 As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_DATA_MSG)))
        '    Marshal.StructureToPtr(dataMsg, buf2, True)
        '    iRet = CAWriteItem(hOperHandle, buf, 0, CA_DATA_FORMAT_STRUCT, buf2)
        '    If iRet <> CONA_OK Then ShowErrorMessage("DAWriteItem", iRet)
        '    Marshal.FreeHGlobal(buf2)
        '    Marshal.FreeHGlobal(buf)
        '    Marshal.FreeCoTaskMem(dataMsg.pbData)
        '    Marshal.FreeCoTaskMem(dataMsg.pAddress)

        '    iRet = CACommitOperations(hOperHandle, IntPtr.Zero)
        '    If iRet <> CONA_OK Then ShowErrorMessage("CACommitOperations", iRet)
        '    CARegisterOperationCallback(hOperHandle, API_UNREGISTER, pCAOperationCallback)
        '    If iRet <> CONA_OK Then ShowErrorMessage("CARegisterOperationCallback", iRet)
        '    iRet = CAEndOperation(hOperHandle)
        '    If iRet <> CONA_OK Then ShowErrorMessage("CAEndOperation", iRet)
        '    smsNode.Nodes.Clear()
        '    GetSMSMessages(GetCurrentDevice(), smsNode)
        'End If
    End Sub


    '===================================================================
    ' ShowNewCalendarDlg
    '
    ' Shows "New Calendar item" dialog and writes Calendar entry to device
    '
    '===================================================================
    Private Sub ShowNewCalendarDlg()
        '' Open "New Text Message" dialog
        'Dim dlg As New CalendarItemDlg
        'If dlg.ShowDialog() <> Windows.Forms.DialogResult.OK Then
        '    Exit Sub
        'End If

        'CheckCalendarConnection(GetCurrentDevice())
        'Dim dataCalendar As CA_DATA_CALENDAR
        'dataCalendar.iSize = Marshal.SizeOf(dataCalendar)
        'dataCalendar.iInfoField = CA_CALENDAR_ITEM_MEETING + dlg.ComboBoxType.SelectedIndex

        'ConvertToPIMDate(dlg.DTPickerNoteBeginDate.Value, dataCalendar.noteStartDate)
        'ConvertToPIMDate(dlg.DTPickerNoteEndDate.Value, dataCalendar.noteEndDate)
        'dataCalendar.iAlarmState = CA_CALENDAR_ALARM_NOT_SET + dlg.ComboAlarm.SelectedIndex
        'If (dataCalendar.iAlarmState = CA_CALENDAR_ALARM_NOT_SET) Then
        '    GetEmptyPIMDate(dataCalendar.noteAlarmTime)
        'Else
        '    ConvertToPIMDate(dlg.DTPickerAlarmDate.Value, dataCalendar.noteAlarmTime)
        'End If
        'dataCalendar.iRecurrence = CA_CALENDAR_RECURRENCE_NONE + dlg.ComboRecurrence.SelectedIndex
        'If (dataCalendar.iRecurrence = CA_CALENDAR_RECURRENCE_NONE) Then
        '    GetEmptyPIMDate(dataCalendar.recurrenceEndDate)
        'Else
        '    CA_SET_RECURRENCE_INTERVAL(dataCalendar.iRecurrence, 1)
        '    If dlg.CheckBoxRecEnd.Checked Then
        '        ConvertToPIMDate(dlg.DTPickerAlarmDate.Value, dataCalendar.recurrenceEndDate)
        '    Else
        '        GetEmptyPIMDate(dataCalendar.recurrenceEndDate)
        '    End If
        'End If

        'If dataCalendar.iInfoField = CA_CALENDAR_ITEM_MEETING Then
        '    Dim iCount As Integer = 0
        '    If dlg.TextBoxNote.Text.Length > 0 Then
        '        iCount += 1
        '    End If
        '    If dlg.TextBoxLocation.Text.Length > 0 Then
        '        iCount += 1
        '    End If
        '    dataCalendar.bItemCount = iCount
        '    dataCalendar.pDataItems = GetMeetingDescriptionLocationBuffer(dlg.TextBoxNote.Text, dlg.TextBoxLocation.Text, iCount)
        'ElseIf dataCalendar.iInfoField = CA_CALENDAR_ITEM_CALL Then
        '    Dim iCount As Integer = 0
        '    If dlg.TextBoxNote.Text.Length > 0 Then
        '        iCount += 1
        '    End If
        '    If dlg.TextBoxLocation.Text.Length > 0 Then
        '        iCount += 1
        '    End If
        '    dataCalendar.bItemCount = iCount
        '    dataCalendar.pDataItems = GetCallNameNumberBuffer(dlg.TextBoxNote.Text, dlg.TextBoxLocation.Text, iCount)
        'ElseIf dataCalendar.iInfoField = CA_CALENDAR_ITEM_BIRTHDAY Then
        '    Dim iCount As Integer = 0
        '    If dlg.TextBoxNote.Text.Length > 0 Then
        '        iCount += 1
        '    End If
        '    If dlg.TextBoxLocation.Text.Length > 0 Then
        '        iCount += 1
        '    End If
        '    dataCalendar.bItemCount = iCount
        '    dataCalendar.pDataItems = GetBirthDayNameYearBuffer(dlg.TextBoxNote.Text, dlg.TextBoxLocation.Text, iCount)
        'ElseIf dataCalendar.iInfoField = CA_CALENDAR_ITEM_MEMO Then
        '    Dim iCount As Integer = 0
        '    If dlg.TextBoxNote.Text.Length > 0 Then
        '        iCount += 1
        '    End If
        '    dataCalendar.bItemCount = iCount
        '    dataCalendar.pDataItems = GetMemoBuffer(dlg.TextBoxNote.Text, iCount)
        'ElseIf dataCalendar.iInfoField = CA_CALENDAR_ITEM_REMINDER Then
        '    Dim iCount As Integer = 0
        '    If dlg.TextBoxNote.Text.Length > 0 Then
        '        iCount += 1
        '    End If
        '    dataCalendar.bItemCount = iCount
        '    ' reuse getmemobuffer function
        '    dataCalendar.pDataItems = GetMemoBuffer(dlg.TextBoxNote.Text, iCount)
        'ElseIf dataCalendar.iInfoField = CA_CALENDAR_ITEM_NOTE Then
        '    Dim iCount As Integer = 0
        '    If dlg.TextBoxNote.Text.Length > 0 Then
        '        iCount += 1
        '    End If
        '    dataCalendar.bItemCount = iCount
        '    ' reuse getmemobuffer function
        '    dataCalendar.pDataItems = GetMemoBuffer(dlg.TextBoxNote.Text, iCount)
        'ElseIf dataCalendar.iInfoField = CA_CALENDAR_ITEM_TODO Then
        '    Dim iTodoPriority As Integer = dlg.ComboTodoPrior.SelectedIndex + CA_CALENDAR_TODO_PRIORITY_HIGH
        '    Dim iTodoAction As Integer = dlg.ComboTodoAction.SelectedIndex + CA_CALENDAR_TODO_STATUS_NEEDS_ACTION
        '    Dim iCount As Integer = 0
        '    If iTodoPriority > 0 Then
        '        iCount += 1
        '    End If
        '    If iTodoAction > 0 Then
        '        iCount += 1
        '    End If
        '    If dlg.TextBoxNote.Text.Length > 0 Then
        '        iCount += 1
        '    End If
        '    dataCalendar.bItemCount = iCount
        '    dataCalendar.pDataItems = GetTodoBuffer(dlg.TextBoxNote.Text, iTodoPriority, iTodoAction, iCount)
        'End If

        'Dim calendarNode As System.Windows.Forms.TreeNode
        'If TVW_Navigator.SelectedNode.ImageIndex = m_iIconCalendarIndex Then
        '    calendarNode = TVW_Navigator.SelectedNode
        'Else
        '    calendarNode = TVW_Navigator.SelectedNode.Parent
        'End If
        'Dim folderInfo As CA_FOLDER_INFO = MapCAFolderInfoToCFI(calendarNode.Tag)

        '' Write new Calendar item to currently connected device
        'Dim hOperHandle As Integer = 0
        'Dim iRet As Integer = CABeginOperation(m_hCurrentConnection, 0, hOperHandle)
        'If iRet <> CONA_OK Then ShowErrorMessage("CABeginOperation", iRet)
        'CARegisterOperationCallback(hOperHandle, API_REGISTER, pCAOperationCallback)
        'If iRet <> CONA_OK Then ShowErrorMessage("CARegisterOperationCallback", iRet)
        'Dim itemUid As CA_ITEM_ID
        'itemUid.iSize = Marshal.SizeOf(itemUid)
        'itemUid.iFolderId = folderInfo.iFolderId
        'itemUid.iStatus = 0
        'itemUid.iTemporaryID = 0
        'itemUid.iUidLen = 0
        'itemUid.pbUid = IntPtr.Zero
        'Dim buf As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_ITEM_ID)))
        'Marshal.StructureToPtr(itemUid, buf, True)
        '' Allocate memory for buffer
        'Dim buf2 As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_DATA_CALENDAR)))
        'Marshal.StructureToPtr(dataCalendar, buf2, True)
        'iRet = CAWriteItem(hOperHandle, buf, 0, CA_DATA_FORMAT_STRUCT, buf2)
        'If iRet <> CONA_OK Then ShowErrorMessage("DAWriteItem", iRet)
        'Marshal.FreeHGlobal(buf2)
        'Marshal.FreeHGlobal(buf)

        ''If iRet <> CONA_OK Then ShowErrorMessage("DAFreeItemData", iRet)
        'FreeCalendarDataAllocations(dataCalendar)

        'iRet = CACommitOperations(hOperHandle, IntPtr.Zero)
        'If iRet <> CONA_OK Then ShowErrorMessage("CACommitOperations", iRet)
        'CARegisterOperationCallback(hOperHandle, API_UNREGISTER, pCAOperationCallback)
        'If iRet <> CONA_OK Then ShowErrorMessage("CARegisterOperationCallback", iRet)
        'iRet = CAEndOperation(hOperHandle)
        'If iRet <> CONA_OK Then ShowErrorMessage("CAEndOperation", iRet)
        'calendarNode.Nodes.Clear()
        'GetCalendar(GetCurrentDevice(), calendarNode)
    End Sub

    '===================================================================
    ' ShowNewBookmarkDlg
    '
    ' Shows "New Text Message" dialog and writes SMS to device
    '
    '===================================================================
    Private Sub ShowNewBookmarkDlg()
        '' Open "New Bookmark" dialog
        'Dim dlg As New BookmarkDlg
        'If dlg.ShowDialog() = Windows.Forms.DialogResult.OK Then

        '    Dim dataBookmark As CA_DATA_BOOKMARK = New CA_DATA_BOOKMARK
        '    dataBookmark.iSize = Marshal.SizeOf(dataBookmark)

        '    dataBookmark.pstrTitle = dlg.TextTitle.Text
        '    dataBookmark.pstrBookMarkUrl = dlg.TextURL.Text
        '    dataBookmark.pstrUrlShortcut = dlg.TextShort.Text

        '    Dim bookmarksNode As System.Windows.Forms.TreeNode
        '    If TVW_Navigator.SelectedNode.ImageIndex = m_iIconBookmarkIndex Then
        '        bookmarksNode = TVW_Navigator.SelectedNode
        '    Else
        '        bookmarksNode = TVW_Navigator.SelectedNode.Parent
        '    End If
        '    Dim folderInfo As CA_FOLDER_INFO = MapCAFolderInfoToCFI(bookmarksNode.Tag)

        '    ' Write new Bookmark item to currently connected device
        '    Dim hOperHandle As Integer = 0
        '    Dim iRet As Integer = CABeginOperation(m_hCurrentConnection, 0, hOperHandle)
        '    If iRet <> CONA_OK Then ShowErrorMessage("CABeginOperation", iRet)
        '    CARegisterOperationCallback(hOperHandle, API_REGISTER, pCAOperationCallback)
        '    If iRet <> CONA_OK Then ShowErrorMessage("CARegisterOperationCallback", iRet)
        '    Dim itemUid As CA_ITEM_ID
        '    itemUid.iSize = Marshal.SizeOf(itemUid)
        '    itemUid.iFolderId = folderInfo.iFolderId
        '    itemUid.iFolderId = folderInfo.iFolderId
        '    itemUid.iStatus = 0
        '    itemUid.iTemporaryID = 0
        '    itemUid.iUidLen = 0
        '    itemUid.pbUid = IntPtr.Zero
        '    Dim buf As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_ITEM_ID)))
        '    Marshal.StructureToPtr(itemUid, buf, True)
        '    ' Allocate memory for buffer
        '    Dim buf2 As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_DATA_BOOKMARK)))
        '    Marshal.StructureToPtr(dataBookmark, buf2, True)
        '    iRet = CAWriteItem(hOperHandle, buf, 0, CA_DATA_FORMAT_STRUCT, buf2)
        '    If iRet <> CONA_OK Then ShowErrorMessage("DAWriteItem", iRet)
        '    ' Free memory allocated by DA API
        '    iRet = CAFreeItemData(m_hCurrentConnection, CA_DATA_FORMAT_STRUCT, buf2)
        '    If iRet <> CONA_OK Then ShowErrorMessage("DAFreeItemData", iRet)
        '    Marshal.FreeHGlobal(buf2)
        '    Marshal.FreeHGlobal(buf)
        '    iRet = CACommitOperations(hOperHandle, IntPtr.Zero)
        '    If iRet <> CONA_OK Then ShowErrorMessage("CACommitOperations", iRet)
        '    CARegisterOperationCallback(hOperHandle, API_UNREGISTER, pCAOperationCallback)
        '    If iRet <> CONA_OK Then ShowErrorMessage("CARegisterOperationCallback", iRet)
        '    iRet = CAEndOperation(hOperHandle)
        '    If iRet <> CONA_OK Then ShowErrorMessage("CAEndOperation", iRet)
        '    bookmarksNode.Nodes.Clear()
        '    GetBookmarks(GetCurrentDevice(), bookmarksNode)
        'End If
    End Sub



    '===================================================================
    ' PIMNavigator_Load
    '
    ' Initialization of PIM Navigator dialog
    '
    '===================================================================
    Private Sub PIMNavigator_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        bRefresh = True

        Timer1.Enabled = True
        Timer1.Start()

        AddHandler DMD.Nokia.DeviceAdded, AddressOf handleDeviceAdded
        AddHandler DMD.Nokia.DeviceRemoved, AddressOf handleDeviceRemoved

    End Sub

    Private Sub handleDeviceAdded(ByVal sender As Object, ByVal e As DeviceEventArgs)
        Debug.Print("Device added: " & e.Device.SerialNumber)
        Me.bRefresh = True
    End Sub

    Private Sub handleDeviceRemoved(ByVal sender As Object, ByVal e As DeviceEventArgs)
        Debug.Print("Device removed: " & e.Device.SerialNumber)
        Me.bRefresh = True
    End Sub

    '===================================================================
    ' BTN_New_Click
    '
    ' User has clicked "New" button
    '
    '===================================================================
    Private Sub BTN_New_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_New.Click
        'If m_hCurrentConnection = m_hContacts Then
        '    ShowNewContactDlg()
        'ElseIf m_hCurrentConnection = m_hBookmark Then
        '    ShowNewBookmarkDlg()
        'ElseIf m_hCurrentConnection = m_hCalendar Then
        '    ShowNewCalendarDlg()
        'Else
        '    ShowNewSMSDlg()
        'End If
    End Sub

    '===================================================================
    ' BTN_Delete_Click
    '
    ' User has clicked "Delete" button
    '
    '===================================================================
    Private Sub BTN_Delete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Delete.Click
        If MsgBox("Are you sure you want to delete selected item?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Confirm Item Delete") <> MsgBoxResult.Yes Then Return
        Try
            Dim bFolder As Boolean = False
            Dim buffer As IntPtr = IntPtr.Zero
            Dim node As TreeNode = TVW_Navigator.SelectedNode
            Dim item As CBaseItem = node.Tag
            item.Delete()
            node.Remove()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '===================================================================
    ' BTN_Refresh_Click
    '
    ' User has clicked "Refresh" button
    '
    '===================================================================
    Private Sub BTN_Refresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Refresh.Click
        Me.RefreshTreeView()
    End Sub

    '===================================================================
    ' BTN_Close_Click
    '
    ' User has clicked "Close" button
    '
    '===================================================================
    Private Sub BTN_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_Close.Click
        Close()
    End Sub

    '===================================================================
    ' Timer1_Tick
    '
    ' Handles timer events.
    ' Timer is used to start RefreshTreeView() asynchronously
    '
    '===================================================================
    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If (bRefresh) Then
            bRefresh = False
            ' Fill connected devices, target folders and PIM items in tree view
            RefreshTreeView()
        End If
    End Sub



    '===================================================================
    ' GetContacts
    '
    ' Reads contact items from phone and adds them into tree view.
    '
    '===================================================================
    Private Sub GetContacts(ByVal parentItem As System.Windows.Forms.TreeNode)
        Dim folderInfo As CContacts = parentItem.Tag
        For Each dataContact As CContactItem In folderInfo.Items
            ' Insert contact item in tree view
            Dim itemZ As New System.Windows.Forms.TreeNode
            itemZ.ImageIndex = m_iIconContactIndex
            itemZ.SelectedImageIndex = m_iIconContactIndex
            itemZ.Tag = dataContact
            itemZ.Text = dataContact.ToString
            parentItem.Nodes.Add(itemZ)
        Next
    End Sub

    '===================================================================
    ' GetSMSMessages
    '
    ' Reads SMS messages from phone and adds them into tree view.
    '
    '===================================================================
    Private Sub GetSMSMessages(ByVal parentItem As System.Windows.Forms.TreeNode)
        Dim folderInfo As CSMSFolder = parentItem.Tag
        For Each datasms As CSMSMessage In folderInfo.Messages
            ' Insert SMS message item in tree view
            Dim itemZ As New System.Windows.Forms.TreeNode
            itemZ.Text = datasms.Message
            itemZ.ImageIndex = m_iIconSMSIndex
            itemZ.SelectedImageIndex = m_iIconSMSIndex
            itemZ.Tag = datasms
            parentItem.Nodes.Add(itemZ)
        Next
    End Sub

    '===================================================================
    ' GetMMSMessages
    '
    ' Reads MMS messages from phone and adds them into tree view.
    '
    '===================================================================
    Private Sub GetMMSMessages(ByVal parentItem As System.Windows.Forms.TreeNode)
        ' Check PIM connection to MMS folders and open it if needed
        ' Set MMS folder target path
        Dim folderInfo As CMMSFolder = parentItem.Tag
        For Each dataMMS As CMMSMessage In folderInfo.Messages
            ' Insert MMS message item in tree view
            Dim itemZ As New System.Windows.Forms.TreeNode
            itemZ.Text = dataMMS.Message & ": " & dataMMS.Addresses.ToString
            itemZ.ImageIndex = m_iIconMMSIndex
            itemZ.SelectedImageIndex = m_iIconMMSIndex
            itemZ.Tag = dataMMS
            parentItem.Nodes.Add(itemZ)
        Next
    End Sub

    '===================================================================
    ' GetCalendar
    '
    ' Reads calendar items from phone and adds them into tree view.
    '
    '===================================================================
    Private Sub GetCalendar(ByVal parentItem As System.Windows.Forms.TreeNode)
        '' Check PIM connection to calendar folder and open it if needed
        '' Set calendar folder target path
        Dim folderInfo As CCalendarFolder = parentItem.Tag
        For Each dataCalendar As CCalendarItem In folderInfo.Items
            Dim strData As String = ""
            Dim dateMsg As Date? = dataCalendar.StartDate
            If dateMsg.HasValue Then strData = dateMsg.ToString & ": "
            strData &= dataCalendar.ItemTypeEx ' CalendarItemType2String(dataCalendar.iInfoField)
            Dim itemZ As New System.Windows.Forms.TreeNode
            itemZ.ImageIndex = m_iIconCalendarItemIndex
            itemZ.SelectedImageIndex = m_iIconCalendarItemIndex
            itemZ.Tag = dataCalendar
            itemZ.Text = strData
            parentItem.Nodes.Add(itemZ)
        Next
    End Sub

    '===================================================================
    ' GetBookmarks
    '
    ' Reads bookmark items from phone and adds them into tree view.
    '
    '===================================================================
    Private Sub GetBookmarks(ByVal parentItem As System.Windows.Forms.TreeNode)
        Dim folder As CBookMarkFolder = parentItem.Tag
        For Each dataBookmark As CBookMarkItem In folder.Items
            Dim strData As String = dataBookmark.Title

            strData &= ": "
            strData &= dataBookmark.BookMarkUrl

            Dim itemZ As New System.Windows.Forms.TreeNode
            itemZ.ImageIndex = m_iIconBookmarkItemIndex
            itemZ.SelectedImageIndex = m_iIconBookmarkItemIndex
            itemZ.Tag = dataBookmark
            itemZ.Text = strData
            parentItem.Nodes.Add(itemZ)
        Next
    End Sub

    '===================================================================
    ' BTN_Save_Click
    '
    ' User has clicked "Save to File" button
    '
    '===================================================================
    Private Sub BTN_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Save.Click
        'Dim strFilter As String
        'If TVW_Navigator.SelectedNode.ImageIndex = m_iIconContactIndex Then
        '    strFilter = "vCard files (*.vcf)|*.vcf||"
        'ElseIf TVW_Navigator.SelectedNode.ImageIndex = m_iIconSMSIndex Then
        '    strFilter = "vMessage files (*.vmg)|*.vmg||"
        'ElseIf TVW_Navigator.SelectedNode.ImageIndex = m_iIconBookmarkItemIndex Then
        '    strFilter = "vBookmark files (*.vbk)|*.vbk||"
        'ElseIf TVW_Navigator.SelectedNode.ImageIndex = m_iIconCalendarItemIndex Then
        '    strFilter = "vCalendar files (*.vcs)|*.vcs||"
        'ElseIf TVW_Navigator.SelectedNode.ImageIndex = m_iIconMMSIndex Then
        '    strFilter = "MMS files (*.mms)|*.mms||"
        'Else
        '    Return
        'End If
        'Dim fileDlg As New System.Windows.Forms.SaveFileDialog
        'fileDlg.Filter = strFilter
        'If fileDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
        '    Dim hOperHandle As Integer
        '    Dim iRet As Integer = CABeginOperation(m_hCurrentConnection, 0, hOperHandle)
        '    If iRet <> CONA_OK Then
        '        ShowErrorMessage("CABeginOperation", iRet)
        '    End If
        '    ' Read contact item data from device
        '    Dim UID As CA_ITEM_ID = MapCAItemIDToUID(TVW_Navigator.SelectedNode.Tag)
        '    Dim bufId As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(UID))
        '    Marshal.StructureToPtr(UID, bufId, True)
        '    Dim dataVersit As CA_DATA_VERSIT
        '    dataVersit.iSize = Marshal.SizeOf(dataVersit)
        '    Dim bufData As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(dataVersit))
        '    Marshal.StructureToPtr(dataVersit, bufData, True)
        '    iRet = CAReadItem(hOperHandle, bufId, CA_OPTION_USE_CACHE, CA_DATA_FORMAT_VERSIT, bufData)
        '    If iRet = CONA_OK Then
        '        dataVersit = Marshal.PtrToStructure(bufData, GetType(CA_DATA_VERSIT))
        '        Dim bVersitObject As Byte()
        '        ReDim bVersitObject(dataVersit.iDataLength)
        '        Marshal.Copy(dataVersit.pbVersitObject, bVersitObject, 0, dataVersit.iDataLength)
        '        Dim iFr As Short
        '        iFr = FreeFile()
        '        FileOpen(iFr, fileDlg.FileName, OpenMode.Binary)
        '        FilePut(iFr, bVersitObject)
        '        FileClose(iFr)
        '    Else
        '        ShowErrorMessage("CAReadItem", iRet)
        '    End If
        '    Marshal.FreeHGlobal(bufId)
        '    Marshal.FreeHGlobal(bufData)
        '    iRet = CAEndOperation(hOperHandle)
        '    If iRet <> CONA_OK Then
        '        ShowErrorMessage("CAEndOperation", iRet)
        '    End If
        '    FreeUIDMappingMemory(UID)
        'End If
    End Sub

    '===================================================================
    ' BTN_NewFolder_Click
    '
    ' User has clicked "New Folder" button
    '
    '===================================================================
    Private Sub BTN_NewFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_NewFolder.Click
        '' Open "New Folder" dialog
        'Dim dlg As New NewFolderDlg
        'If dlg.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then
        '    Exit Sub
        'End If

        '' Creates folder to currently connected device
        'Dim hOperHandle As Integer = 0
        'Dim iRet As Integer = CABeginOperation(m_hCurrentConnection, 0, hOperHandle)
        'If iRet <> CONA_OK Then
        '    ShowErrorMessage("CABeginOperation", iRet)
        'End If
        'Dim treeNode As Windows.Forms.TreeNode = TVW_Navigator.SelectedNode
        'Dim strFolder As String
        'Dim buffer As IntPtr
        'If treeNode.ImageIndex = m_iIconSMSMessagesIndex Then
        '    ' Fill item id struct
        '    Dim itemId As CA_ITEM_ID
        '    itemId.iSize = Marshal.SizeOf(itemId)
        '    itemId.iFolderId = CA_MESSAGE_FOLDER_USER_FOLDERS
        '    buffer = Marshal.AllocHGlobal(Marshal.SizeOf(itemId))
        '    Marshal.StructureToPtr(itemId, buffer, True)
        '    ' Create path for user defined message folder
        '    strFolder = "predefuserfolders\" + dlg.TextFolder.Text
        '    ' Find SMS messages root folder
        '    While treeNode.Parent.ImageIndex = m_iIconSMSMessagesIndex
        '        treeNode = treeNode.Parent
        '    End While
        'Else
        '    Dim UID As CA_FOLDER_INFO = MapCAFolderInfoToCFI(TVW_Navigator.SelectedNode.Tag)
        '    ' Browse folder up to current item root and
        '    ' build path for new subfolder (root UID used for operation) 
        '    strFolder = dlg.TextFolder.Text
        '    While Not (treeNode.Parent Is Nothing)
        '        If Not (treeNode.Parent.Tag Is Nothing) Then
        '            UID = MapCAFolderInfoToCFI(treeNode.Tag)
        '            If UID.pstrName <> "\" Then
        '                strFolder = UID.pstrName + "\" + strFolder
        '            End If
        '            treeNode = treeNode.Parent
        '        End If
        '    End While

        '    buffer = Marshal.AllocHGlobal(Marshal.SizeOf(UID))
        '    Marshal.StructureToPtr(UID, buffer, True)
        'End If

        'iRet = CACreateFolder(hOperHandle, buffer, strFolder)
        'If iRet <> CONA_OK Then
        '    ShowErrorMessage("CACreateFolder", iRet)
        'End If
        'iRet = CACommitOperations(hOperHandle, IntPtr.Zero)
        'If iRet <> CONA_OK Then
        '    ShowErrorMessage("CACommitOperations", iRet)
        'End If
        'iRet = CAEndOperation(hOperHandle)
        'If iRet <> CONA_OK Then
        '    ShowErrorMessage("CAEndOperation", iRet)
        'End If
        'Marshal.FreeHGlobal(buffer)

        '' Fill connected devices, target folders and PIM items in tree view
        'bRefresh = True
    End Sub

    '===================================================================
    ' ShowNotification
    '
    ' Asynchronously inserts notification text to Notifications dialog 
    '
    '===================================================================
    Public Sub ShowNotification(ByVal strNotification As String)
        'If NotificationsDialog Is Nothing Then
        '    Exit Sub
        'End If
        'If NotificationsDialog.IsDisposed Then
        '    Exit Sub
        'End If
        '' Insert text to Notifications dialog asynchronously, so that UI is not blocked
        'BeginInvoke(New InsertNotificationDelegate(AddressOf NotificationsDialog.InsertNotification), New Object() {strNotification})
    End Sub



    '===================================================================
    ' CAOperationCallback
    '
    ' Callback function for CA operation notifications
    '
    '===================================================================
    Public Function CAOperationCallback(ByVal hOperHandle As Integer, ByVal iOperation As Integer, ByVal iCurrent As Integer, ByVal iTotalAmount As Integer, ByVal iStatus As Integer, ByVal pItemID As IntPtr) As Integer
        Dim strStatus As String
        strStatus = String.Format("CAOperationCallback: Operation({0}), progress({0}), total({0}), status({0})", iOperation, iCurrent, iTotalAmount, iStatus)
        ShowNotification(strStatus)
        Return 1
    End Function

    '===================================================================
    ' TVW_Navigator_BeforeExpand
    '
    ' User has clicked '+' in tree view
    '
    '===================================================================
    Private Sub TVW_Navigator_BeforeExpand(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles TVW_Navigator.BeforeExpand
        Dim item As System.Windows.Forms.TreeNode = e.Node
        Dim childItem As System.Windows.Forms.TreeNode = item.FirstNode
        If Not childItem.Tag Is Nothing Then
            ' Already expanded
            Return
        End If
        ' Remove dummy item.
        childItem.Remove()

        If TypeOf (item.Tag) Is CContacts Then
            ' Check PIM connection to contacts folder and open it if needed
            GetContacts(item)
        ElseIf TypeOf (item.Tag) Is CCalendar Then
            ' Check PIM connection to Calendar folders and open it if needed
            GetCalendar(item)
        ElseIf TypeOf (item.Tag) Is CBookMarks Then
            ' Check PIM connection to Bookmark folders and open it if needed
            GetBookmarks(item)
        ElseIf TypeOf (item.Tag) Is CSMSFolder Then
            ' Check PIM connection to SMS folders and open it if needed
            GetSMSMessages(item)
        ElseIf TypeOf (item.Tag) Is CMMSFolder Then
            ' Check PIM connection to MMS folders and open it if needed
            GetMMSMessages(item)
        End If
    End Sub

    '===================================================================
    ' ButtonNotifications_Click
    '
    ' User has clicked "Notifications" button
    '
    '===================================================================
    Private Sub ButtonNotifications_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNotifications.Click
        'If NotificationsDialog Is Nothing Then
        '    NotificationsDialog = New NotificationsDlg
        'End If
        'If NotificationsDialog.IsDisposed Then
        '    NotificationsDialog = New NotificationsDlg
        'End If
        'NotificationsDialog.Show()
    End Sub

    Private Sub FileBrowserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FileBrowserToolStripMenuItem.Click
        FileBrowser.Show()
    End Sub

    Private Sub InstallerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InstallerToolStripMenuItem.Click
        InstallerDialog.Show()
    End Sub

    Private Sub PhoneNavigatorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PhoneNavigatorToolStripMenuItem.Click
        PhoneNavigator.Show()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Dim sfd As New SaveFileDialog
        If sfd.ShowDialog = Windows.Forms.DialogResult.OK Then
            Me.PictureBox1.Image.Save(sfd.FileName)
        End If
        sfd.Dispose()
    End Sub

    Private Sub TVW_Navigator_MouseDown(sender As Object, e As MouseEventArgs) Handles TVW_Navigator.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then


            Dim item As System.Windows.Forms.TreeNode = Me.TVW_Navigator.SelectedNode
            If (TypeOf (item.Tag) Is CContacts) Then
                'DirectCast(item.Tag, CContacts).Refresh()
            ElseIf (TypeOf (item.Tag) Is CCalendar) Then

            ElseIf (TypeOf (item.Tag) Is CBookMarks) Then

            ElseIf (TypeOf (item.Tag) Is CSMSFolder) Then
                DirectCast(item.Tag, CSMSFolder).Messages.Refresh()

            ElseIf (TypeOf (item.Tag) Is CMMSFolder) Then

            ElseIf (TypeOf (item.Tag) Is CContactItem) Then

            ElseIf (TypeOf (item.Tag) Is CCalendarItem) Then

            ElseIf (TypeOf (item.Tag) Is CBookMarkItem) Then

            ElseIf (TypeOf (item.Tag) Is CSMSMessage) Then

            ElseIf (TypeOf (item.Tag) Is CMMSMessage) Then

            End If

        End If
    End Sub
End Class
