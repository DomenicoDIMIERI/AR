'Filename    : DeviceInfoDlg.vb
'Part of     : Phone Navigator VB.NET Application
'Description : Implements a dialog that represents all device specific information
'Version     : 3.2

'This example is only to be used with PC Connectivity API version 3.2.
'Compability ("as is") with future versions is not quaranteed.

'Copyright (c) 2005-2007 Nokia Corporation.

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

Public Class FRM_DeviceInfo
    Inherits System.Windows.Forms.Form

    Private m_Dev As DMD.Nokia.NokiaDevice

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
    Friend WithEvents groupBoxSync As System.Windows.Forms.GroupBox
    Friend WithEvents labelCIDS As System.Windows.Forms.Label
    Friend WithEvents labelSADM As System.Windows.Forms.Label
    Friend WithEvents labelSyncSupport As System.Windows.Forms.Label
    Friend WithEvents labelSADS As System.Windows.Forms.Label
    Friend WithEvents buttonClose As System.Windows.Forms.Button
    Friend WithEvents groupBoxGen As System.Windows.Forms.GroupBox
    Friend WithEvents labelLang As System.Windows.Forms.Label
    Friend WithEvents labelVersion As System.Windows.Forms.Label
    Friend WithEvents labelName As System.Windows.Forms.Label
    Friend WithEvents groupBoxType As System.Windows.Forms.GroupBox
    Friend WithEvents labelS80 As System.Windows.Forms.Label
    Friend WithEvents labelS603ed As System.Windows.Forms.Label
    Friend WithEvents labelS602ed As System.Windows.Forms.Label
    Friend WithEvents labelS40 As System.Windows.Forms.Label
    Friend WithEvents labelUnknown As System.Windows.Forms.Label
    Friend WithEvents groupBoxFS As System.Windows.Forms.GroupBox
    Friend WithEvents labelConversion As System.Windows.Forms.Label
    Friend WithEvents labelSIS As System.Windows.Forms.Label
    Friend WithEvents labelJava As System.Windows.Forms.Label
    Friend WithEvents labelFSSupport As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FRM_DeviceInfo))
        Me.groupBoxSync = New System.Windows.Forms.GroupBox
        Me.labelCIDS = New System.Windows.Forms.Label
        Me.labelSADM = New System.Windows.Forms.Label
        Me.labelSyncSupport = New System.Windows.Forms.Label
        Me.labelSADS = New System.Windows.Forms.Label
        Me.buttonClose = New System.Windows.Forms.Button
        Me.groupBoxGen = New System.Windows.Forms.GroupBox
        Me.labelLang = New System.Windows.Forms.Label
        Me.labelVersion = New System.Windows.Forms.Label
        Me.labelName = New System.Windows.Forms.Label
        Me.groupBoxType = New System.Windows.Forms.GroupBox
        Me.labelS80 = New System.Windows.Forms.Label
        Me.labelS603ed = New System.Windows.Forms.Label
        Me.labelS602ed = New System.Windows.Forms.Label
        Me.labelS40 = New System.Windows.Forms.Label
        Me.labelUnknown = New System.Windows.Forms.Label
        Me.groupBoxFS = New System.Windows.Forms.GroupBox
        Me.labelConversion = New System.Windows.Forms.Label
        Me.labelSIS = New System.Windows.Forms.Label
        Me.labelJava = New System.Windows.Forms.Label
        Me.labelFSSupport = New System.Windows.Forms.Label
        Me.groupBoxSync.SuspendLayout()
        Me.groupBoxGen.SuspendLayout()
        Me.groupBoxType.SuspendLayout()
        Me.groupBoxFS.SuspendLayout()
        Me.SuspendLayout()
        '
        'groupBoxSync
        '
        Me.groupBoxSync.Controls.Add(Me.labelCIDS)
        Me.groupBoxSync.Controls.Add(Me.labelSADM)
        Me.groupBoxSync.Controls.Add(Me.labelSyncSupport)
        Me.groupBoxSync.Controls.Add(Me.labelSADS)
        Me.groupBoxSync.Location = New System.Drawing.Point(352, 131)
        Me.groupBoxSync.Name = "groupBoxSync"
        Me.groupBoxSync.Size = New System.Drawing.Size(336, 119)
        Me.groupBoxSync.TabIndex = 12
        Me.groupBoxSync.TabStop = False
        Me.groupBoxSync.Text = "Syncronization Support"
        '
        'labelCIDS
        '
        Me.labelCIDS.Enabled = False
        Me.labelCIDS.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.labelCIDS.Location = New System.Drawing.Point(16, 73)
        Me.labelCIDS.Name = "labelCIDS"
        Me.labelCIDS.Size = New System.Drawing.Size(312, 16)
        Me.labelCIDS.TabIndex = 3
        Me.labelCIDS.Text = "Device supports Client Initiated (CI) Data Syncronization"
        '
        'labelSADM
        '
        Me.labelSADM.Enabled = False
        Me.labelSADM.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.labelSADM.Location = New System.Drawing.Point(16, 54)
        Me.labelSADM.Name = "labelSADM"
        Me.labelSADM.Size = New System.Drawing.Size(312, 16)
        Me.labelSADM.TabIndex = 2
        Me.labelSADM.Text = "Device supports Server Alerted (SA) Device Management"
        '
        'labelSyncSupport
        '
        Me.labelSyncSupport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.labelSyncSupport.Location = New System.Drawing.Point(16, 16)
        Me.labelSyncSupport.Name = "labelSyncSupport"
        Me.labelSyncSupport.Size = New System.Drawing.Size(312, 16)
        Me.labelSyncSupport.TabIndex = 0
        '
        'labelSADS
        '
        Me.labelSADS.Enabled = False
        Me.labelSADS.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.labelSADS.Location = New System.Drawing.Point(16, 35)
        Me.labelSADS.Name = "labelSADS"
        Me.labelSADS.Size = New System.Drawing.Size(312, 16)
        Me.labelSADS.TabIndex = 1
        Me.labelSADS.Text = "Device supports Server Alerted (SA) Data Syncronization"
        '
        'buttonClose
        '
        Me.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.buttonClose.Location = New System.Drawing.Point(616, 259)
        Me.buttonClose.Name = "buttonClose"
        Me.buttonClose.Size = New System.Drawing.Size(75, 23)
        Me.buttonClose.TabIndex = 13
        Me.buttonClose.Text = "Close"
        '
        'groupBoxGen
        '
        Me.groupBoxGen.Controls.Add(Me.labelLang)
        Me.groupBoxGen.Controls.Add(Me.labelVersion)
        Me.groupBoxGen.Controls.Add(Me.labelName)
        Me.groupBoxGen.Location = New System.Drawing.Point(8, 8)
        Me.groupBoxGen.Name = "groupBoxGen"
        Me.groupBoxGen.Size = New System.Drawing.Size(336, 119)
        Me.groupBoxGen.TabIndex = 9
        Me.groupBoxGen.TabStop = False
        Me.groupBoxGen.Text = "General"
        '
        'labelLang
        '
        Me.labelLang.Enabled = False
        Me.labelLang.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.labelLang.Location = New System.Drawing.Point(16, 54)
        Me.labelLang.Name = "labelLang"
        Me.labelLang.Size = New System.Drawing.Size(312, 18)
        Me.labelLang.TabIndex = 2
        Me.labelLang.Text = "Used Language: "
        '
        'labelVersion
        '
        Me.labelVersion.Enabled = False
        Me.labelVersion.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.labelVersion.Location = New System.Drawing.Point(16, 35)
        Me.labelVersion.Name = "labelVersion"
        Me.labelVersion.Size = New System.Drawing.Size(312, 16)
        Me.labelVersion.TabIndex = 1
        Me.labelVersion.Text = "Software Version: "
        '
        'labelName
        '
        Me.labelName.Enabled = False
        Me.labelName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.labelName.Location = New System.Drawing.Point(16, 16)
        Me.labelName.Name = "labelName"
        Me.labelName.Size = New System.Drawing.Size(312, 16)
        Me.labelName.TabIndex = 0
        Me.labelName.Text = "Device Type Name: "
        '
        'groupBoxType
        '
        Me.groupBoxType.Controls.Add(Me.labelS80)
        Me.groupBoxType.Controls.Add(Me.labelS603ed)
        Me.groupBoxType.Controls.Add(Me.labelS602ed)
        Me.groupBoxType.Controls.Add(Me.labelS40)
        Me.groupBoxType.Controls.Add(Me.labelUnknown)
        Me.groupBoxType.Location = New System.Drawing.Point(352, 8)
        Me.groupBoxType.Name = "groupBoxType"
        Me.groupBoxType.Size = New System.Drawing.Size(336, 119)
        Me.groupBoxType.TabIndex = 10
        Me.groupBoxType.TabStop = False
        Me.groupBoxType.Text = "Device Type"
        '
        'labelS80
        '
        Me.labelS80.Enabled = False
        Me.labelS80.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.labelS80.Location = New System.Drawing.Point(16, 92)
        Me.labelS80.Name = "labelS80"
        Me.labelS80.Size = New System.Drawing.Size(312, 16)
        Me.labelS80.TabIndex = 4
        Me.labelS80.Text = "Series 80"
        '
        'labelS603ed
        '
        Me.labelS603ed.Enabled = False
        Me.labelS603ed.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.labelS603ed.Location = New System.Drawing.Point(16, 73)
        Me.labelS603ed.Name = "labelS603ed"
        Me.labelS603ed.Size = New System.Drawing.Size(312, 16)
        Me.labelS603ed.TabIndex = 3
        Me.labelS603ed.Text = "Series 60 3rd edition"
        '
        'labelS602ed
        '
        Me.labelS602ed.Enabled = False
        Me.labelS602ed.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.labelS602ed.Location = New System.Drawing.Point(16, 54)
        Me.labelS602ed.Name = "labelS602ed"
        Me.labelS602ed.Size = New System.Drawing.Size(312, 16)
        Me.labelS602ed.TabIndex = 2
        Me.labelS602ed.Text = "Series 60 2nd edition"
        '
        'labelS40
        '
        Me.labelS40.Enabled = False
        Me.labelS40.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.labelS40.Location = New System.Drawing.Point(16, 35)
        Me.labelS40.Name = "labelS40"
        Me.labelS40.Size = New System.Drawing.Size(312, 16)
        Me.labelS40.TabIndex = 1
        Me.labelS40.Text = "Series 40"
        '
        'labelUnknown
        '
        Me.labelUnknown.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.labelUnknown.Location = New System.Drawing.Point(16, 16)
        Me.labelUnknown.Name = "labelUnknown"
        Me.labelUnknown.Size = New System.Drawing.Size(312, 16)
        Me.labelUnknown.TabIndex = 0
        Me.labelUnknown.Text = "Unknown device"
        '
        'groupBoxFS
        '
        Me.groupBoxFS.Controls.Add(Me.labelConversion)
        Me.groupBoxFS.Controls.Add(Me.labelSIS)
        Me.groupBoxFS.Controls.Add(Me.labelJava)
        Me.groupBoxFS.Controls.Add(Me.labelFSSupport)
        Me.groupBoxFS.Location = New System.Drawing.Point(8, 131)
        Me.groupBoxFS.Name = "groupBoxFS"
        Me.groupBoxFS.Size = New System.Drawing.Size(336, 119)
        Me.groupBoxFS.TabIndex = 11
        Me.groupBoxFS.TabStop = False
        Me.groupBoxFS.Text = "File System Support"
        '
        'labelConversion
        '
        Me.labelConversion.Enabled = False
        Me.labelConversion.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.labelConversion.Location = New System.Drawing.Point(16, 73)
        Me.labelConversion.Name = "labelConversion"
        Me.labelConversion.Size = New System.Drawing.Size(312, 16)
        Me.labelConversion.TabIndex = 4
        Me.labelConversion.Text = "Device supports file conversion"
        '
        'labelSIS
        '
        Me.labelSIS.Enabled = False
        Me.labelSIS.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.labelSIS.Location = New System.Drawing.Point(16, 54)
        Me.labelSIS.Name = "labelSIS"
        Me.labelSIS.Size = New System.Drawing.Size(312, 16)
        Me.labelSIS.TabIndex = 3
        Me.labelSIS.Text = "Device supports SIS applications' installation"
        '
        'labelJava
        '
        Me.labelJava.Enabled = False
        Me.labelJava.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.labelJava.Location = New System.Drawing.Point(16, 35)
        Me.labelJava.Name = "labelJava"
        Me.labelJava.Size = New System.Drawing.Size(312, 16)
        Me.labelJava.TabIndex = 2
        Me.labelJava.Text = "Device supports Java MIDlet installation"
        '
        'labelFSSupport
        '
        Me.labelFSSupport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.labelFSSupport.Location = New System.Drawing.Point(16, 16)
        Me.labelFSSupport.Name = "labelFSSupport"
        Me.labelFSSupport.Size = New System.Drawing.Size(312, 16)
        Me.labelFSSupport.TabIndex = 0
        '
        'FRM_DeviceInfo
        '
        Me.AcceptButton = Me.buttonClose
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.buttonClose
        Me.ClientSize = New System.Drawing.Size(696, 288)
        Me.Controls.Add(Me.groupBoxSync)
        Me.Controls.Add(Me.buttonClose)
        Me.Controls.Add(Me.groupBoxGen)
        Me.Controls.Add(Me.groupBoxType)
        Me.Controls.Add(Me.groupBoxFS)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FRM_DeviceInfo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Device Info"
        Me.groupBoxSync.ResumeLayout(False)
        Me.groupBoxGen.ResumeLayout(False)
        Me.groupBoxType.ResumeLayout(False)
        Me.groupBoxFS.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub SetDevice(ByVal dev As DMD.Nokia.NokiaDevice)
        Me.m_Dev = dev
        Me.Refill()
    End Sub

    Public Sub Refill()
        If Me.m_Dev Is Nothing Then Return

        ' File system support
        If Me.m_Dev.GetDeviceCAPS(DMD.Nokia.NokiaDeviceCaps.SupportsFileSystem) Then
            SetLabelText(labelFSSupport, "Device supports file system")
        Else
            SetLabelText(labelFSSupport, "Device does not support file system")
        End If

        SetLabelEnabled(labelJava, Me.m_Dev.GetDeviceCAPS(DMD.Nokia.NokiaDeviceCaps.SupportsJavaAppInstallation))
        SetLabelEnabled(labelSIS, Me.m_Dev.GetDeviceCAPS(DMD.Nokia.NokiaDeviceCaps.SupportsSISAppInstallation))
        SetLabelEnabled(labelConversion, Me.m_Dev.GetDeviceCAPS(DMD.Nokia.NokiaDeviceCaps.SupportsFileConversion))

        ' Syncronization support
        If Me.m_Dev.GetDeviceCAPS(DMD.Nokia.NokiaDeviceCaps.SupportsSynchronization) Then
            SetLabelText(labelSyncSupport, "Device supports syncronization")
        Else
            SetLabelText(labelSyncSupport, "Device does not support syncronization")
        End If

        SetLabelEnabled(labelSADS, Me.m_Dev.GetDeviceCAPS(DMD.Nokia.NokiaDeviceCaps.SupportsSADSSynchronization))
        SetLabelEnabled(labelSADM, Me.m_Dev.GetDeviceCAPS(DMD.Nokia.NokiaDeviceCaps.SupportsSADMSynchronization))
        SetLabelEnabled(labelCIDS, Me.m_Dev.GetDeviceCAPS(DMD.Nokia.NokiaDeviceCaps.SupportsCIDSSynchronization))

        ' Device type
        SetLabelEnabled(labelS40, Me.m_Dev.DeviceType = DMD.Nokia.DeviceType.Serie40)
        SetLabelEnabled(labelS602ed, Me.m_Dev.DeviceType = DMD.Nokia.DeviceType.Serie60_2ed)
        SetLabelEnabled(labelS603ed, Me.m_Dev.DeviceType = DMD.Nokia.DeviceType.Serie60_3ed)
        SetLabelEnabled(labelS80, Me.m_Dev.DeviceType = DMD.Nokia.DeviceType.Serie80)

        ' Old type, not shown any more
        'SetLabelEnabled(labelNokia7710, info.iType = CONAPI_NOKIA7710_DEVICE)
        SetLabelEnabled(labelUnknown, Me.m_Dev.DeviceType = 0)

        ' General info (device type name, software version, used language)
        SetLabelText(labelName, Me.m_Dev.SoftwareName)
        SetLabelText(labelVersion, Me.m_Dev.SoftwareVersion)
        SetLabelText(labelLang, Me.m_Dev.UsedLanguage)

    End Sub

    '===================================================================
    ' FRM_DeviceInfo_Load:
    ' Gets the device specific information from PCSAPI and
    ' sets the information on the dialog.
    '===================================================================
    Private Sub FRM_DeviceInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim iRet As Integer = 0
        'Dim pStructure As IntPtr = IntPtr.Zero
        '' Create a general device info struct
        'Dim info As CONAPI_DEVICE_GEN_INFO = New CONAPI_DEVICE_GEN_INFO

        'm_hDMHandle = MainForm.LBX_PhoneFiles.GetDMHandle()
        'm_strSerial = MainForm.LBX_PhoneFiles.GetCurrentSN()
        'If m_hDMHandle <> 0 Then
        '    ' Get general device info
        '    iRet = CONAGetDeviceInfo(m_hDMHandle, m_strSerial, CONAPI_DEVICE_GENERAL_INFO, pStructure)
        '    If iRet = CONA_OK Then
        '        ' Cast the pointer to CONAPI_DEVICE_GEN_INFO struct
        '        info = Marshal.PtrToStructure(pStructure, GetType(CONAPI_DEVICE_GEN_INFO))

        '        ' File system support
        '        If info.iFileSystemSupport = CONAPI_FS_NOT_SUPPORTED Then
        '            SetLabelText(labelFSSupport, "Device does not support file system")
        '        ElseIf (info.iFileSystemSupport And CONAPI_FS_SUPPORTED) <> 0 Then
        '            SetLabelText(labelFSSupport, "Device supports file system")
        '        End If
        '        SetLabelEnabled(labelJava, (info.iFileSystemSupport And CONAPI_FS_INSTALL_JAVA_APPLICATIONS) <> 0)
        '        SetLabelEnabled(labelSIS, (info.iFileSystemSupport And CONAPI_FS_INSTALL_SIS_APPLICATIONS) <> 0)
        '        SetLabelEnabled(labelConversion, (info.iFileSystemSupport And CONAPI_FS_FILE_CONVERSION) <> 0)

        '        ' Syncronization support
        '        If (info.iSyncSupport = CONAPI_SYNC_NOT_SUPPORTED) Then
        '            SetLabelText(labelSyncSupport, "Device does not support syncronization")
        '        Else
        '            SetLabelText(labelSyncSupport, "Device supports syncronization")
        '        End If
        '        SetLabelEnabled(labelSADS, (info.iSyncSupport And CONAPI_SYNC_SA_DS) <> 0)
        '        SetLabelEnabled(labelSADM, (info.iSyncSupport And CONAPI_SYNC_SA_DM) <> 0)
        '        SetLabelEnabled(labelCIDS, (info.iSyncSupport And CONAPI_SYNC_CI_DS) <> 0)

        '        ' Device type
        '        SetLabelEnabled(labelS40, info.iType = CONAPI_SERIES40_DEVICE)
        '        SetLabelEnabled(labelS602ed, info.iType = CONAPI_SERIES60_2ED_DEVICE)
        '        SetLabelEnabled(labelS603ed, info.iType = CONAPI_SERIES60_3ED_DEVICE)
        '        SetLabelEnabled(labelS80, info.iType = CONAPI_SERIES80_DEVICE)
        '        ' Old type, not shown any more
        '        'SetLabelEnabled(labelNokia7710, info.iType = CONAPI_NOKIA7710_DEVICE)
        '        SetLabelEnabled(labelUnknown, info.iType = CONAPI_UNKNOWN_DEVICE)

        '        ' General info (device type name, software version, used language)
        '        SetLabelText(labelName, info.pstrTypeName)
        '        SetLabelText(labelVersion, info.pstrSWVersion)
        '        SetLabelText(labelLang, info.pstrUsedLanguage)

        '        ' Release allocated resources
        '        iRet = CONAFreeDeviceInfoStructure(CONAPI_DEVICE_GENERAL_INFO, pStructure)
        '        If iRet <> CONA_OK Then
        '            ShowErrorMessage("CONAFreeDeviceInfoStructure failed!", iRet)
        '        End If
        '    Else
        '        ShowErrorMessage("CONAGetDeviceInfo failed:", iRet)
        '    End If
        'End If
    End Sub

    '===================================================================
    ' SetLabelEnabled:
    ' Sets a label enabled/disabled.
    '===================================================================
    Private Sub SetLabelEnabled(ByVal lbl As Label, ByVal val As Boolean)
        lbl.Enabled = val
    End Sub

    '===================================================================
    ' SetLabelText:
    ' Appends text to a label whether not null. Also enables/disables label.
    '===================================================================
    Private Sub SetLabelText(ByVal lbl As Label, ByVal txt As String)
        SetLabelEnabled(lbl, txt <> vbNullString)
        If txt <> vbNullString Then
            lbl.Text += txt
        End If
    End Sub

    '===================================================================
    ' buttonClose_Click:
    ' Close button has been pressed. Closes the dialog.
    '===================================================================
    Private Sub buttonClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonClose.Click
        Me.Close()
    End Sub
End Class
