'Filename    : PhoneListBox.vb
'Part of     : Phone Navigator VB.NET example
'Description : Implementation of phone navigator's phone list box
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

Imports System
Imports System.Runtime.InteropServices
Imports System.Diagnostics
Imports System.Windows.Forms
Imports System.IO
Imports System.ComponentModel
Imports DMD.Nokia
Imports DMD.Nokia.APIS

'===================================================================
' PhoneListBox
Public Class PhoneListBox
    Inherits System.Windows.Forms.ListBox

    'Public Const PHONELIST_STATE_PHONELIST As Short = 1
    'Public Const PHONELIST_STATE_PHONECONTENT As Short = 2

    Const g_strPhoneRoot As String = "\\"   ' "\\" means phone root

    Private m_CurrentDevice As NokiaDevice
    Private m_CurrentFolder As NokiaFolderInfo
    
    '===================================================================
    ' Constructor
    '
    '===================================================================
    Public Sub New()

    End Sub

    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property CurrentDevice As NokiaDevice
        Get
            Return Me.m_CurrentDevice
        End Get
        Set(value As NokiaDevice)
            Me.m_CurrentDevice = value
            Me.SetCurrentFolder(Nothing)
        End Set
    End Property

   

    ''===================================================================
    '' PhoneItemDblClicked
    '' 
    '' Called, when user has doubleclicked phone item
    '' If item is [..], shows parent folder
    '' If item is [foldername], shows subfolder
    '' 
    ''===================================================================
    'Sub PhoneItemDblClicked()
    '    Dim strSelectedTxt, strCurrentFolder As String
    '    Dim index As Integer = SelectedIndex
    '    If index <> -1 Then
    '        strSelectedTxt = Items(SelectedIndex).ToString()
    '        If strSelectedTxt = "[..]" Then
    '            ' go up in folder tree
    '            strCurrentFolder = GetCurrentFolderName()
    '            If strCurrentFolder = g_strPhoneRoot Then
    '                ' root folder --> show phone list
    '                ResetContent()
    '                m_wState = PHONELIST_STATE_PHONELIST
    '                ListAllPhones()
    '            Else
    '                ' getting parent folder of strCurrentFolder:
    '                If strCurrentFolder <> g_strPhoneRoot Then
    '                    strCurrentFolder = strCurrentFolder.Substring(0, strCurrentFolder.LastIndexOf("\"))
    '                    If strCurrentFolder = "\" Then
    '                        ' move to root folder
    '                        strCurrentFolder = g_strPhoneRoot
    '                    End If
    '                End If
    '                ShowPhoneFolder(strCurrentFolder)
    '            End If
    '        ElseIf strSelectedTxt.Substring(0, 1) = "[" Then
    '            ' selected item is folder
    '            ShowPhoneFolder(Me.GetCurrentFolderName())
    '        Else
    '            ' selected item is file
    '            Trace.WriteLine("PhoneListBox::PhoneItemDblClicked(): Double clicked file %s --> ignoring...\n", strSelectedTxt)
    '        End If
    '    End If
    'End Sub

    '===================================================================
    ' ShowFolders
    '
    ' Adds all found folders to listbox by using CONAFindNextFolder.
    ' 
    '===================================================================
    Sub ShowFolders(ByVal f As NokiaFolderInfo)
        If (f Is Nothing) Then
            Dim dev As NokiaDevice = Me.GetCurrentDevice
            If dev IsNot Nothing Then
                For Each media As NokiaMediaInfo In dev.FileSystem.InstalledMedia
                    ' Copy data from buffer
                    Dim index As Integer = Me.Items.Add(media)
                    ' Setting folder name as itemdata
                Next
            End If
        Else
            Me.Items.Add("[..]")
            For Each sf As NokiaFolderInfo In f.GetAllFolders()
                ' Copy data from buffer
                Dim index As Integer = Me.Items.Add(sf)
                ' Setting folder name as itemdata
            Next
        End If
    End Sub

    '===================================================================
    ' ShowFiles
    '
    ' Adds all found files to listbox by using CONAFindNextFile.
    ' 
    '===================================================================
    Sub ShowFiles(ByVal f As NokiaFolderInfo)
        If f Is Nothing Then Exit Sub

        For Each File As NokiaFileInfo In f.GetAllFiles
            Dim index As Integer = Me.Items.Add(File)
        Next
    End Sub

    '===================================================================
    ' ShowPhoneFolder
    '
    ' Adds all files and folders to listbox by using
    ' functions CONAFindBegin and CONAFindEnd.
    ' 
    '===================================================================
    Public Sub SetCurrentFolder(ByVal folder As NokiaFolderInfo)
        Dim iFindOptions As Integer = 0
        If FileBrowser.CheckBoxUseCache.Checked Then
            iFindOptions = iFindOptions Or CONA_FIND_USE_CACHE
        End If

        Dim device As NokiaDevice = Me.GetCurrentDevice
        
        ResetContent()
        ShowFolders(folder)
        ShowFiles(folder)

        Me.m_CurrentFolder = folder

        '' Allocate memory for buffer
        'Dim mFN As String = device.FriendlyName
        'FileBrowser.LBL_PhoneFiles.Text = mFN & ":" & folder.ToString
        If folder Is Nothing Then

        End If


    End Sub

    '===================================================================
    ' GetCurrentSN
    '
    ' Returns serial number of currently selected phone
    ' 
    '===================================================================
    Function GetCurrentSN() As String
        Dim device As NokiaDevice = Me.GetCurrentDevice
        If (device Is Nothing) Then Return ""
        Return device.SerialNumber
    End Function

    '===================================================================
    ' GetCurrentFriendlyName
    '
    ' Returns friendly name of currently selected phone
    ' 
    '===================================================================
    Function GetCurrentFriendlyName() As String
        Dim device As NokiaDevice = Me.GetCurrentDevice
        If (device Is Nothing) Then Return ""
        Return device.FriendlyName
    End Function

    
    Private Function GetFreeMemoryString(ByVal device As NokiaDevice) As String
        Dim ret As String = ""
        ' Create FS connection
        ' refreshing memory values
        'ret = CONARefreshDeviceMemoryValues(hFS)
        '   If ret <> CONA_OK Then ShowErrorMessage("CONARefreshDeviceMemoryValues", ret)

        '    GetFreeMemoryString &= Long2MediaString(iMedia)
        For Each mem As CMemoryType In device.FileSystem.MemoryTypes
            ret &= "; " & mem.MediaType & ": "
            ' Getting memory of connected device
            If mem.FreeMemory <> -1 Then
                ret &= "Free " & String.Format("{0:0.00}", CDbl(mem.FreeMemory) / 1024 / 1024) & " MB"
            End If
            If mem.TotalMemory <> -1 AndAlso mem.FreeMemory <> 1 Then
                ret &= ", used " & String.Format("{0:0.00}", CDbl(mem.UsedMemory) / 1024 / 1024)
                ret &= "/" & String.Format("{0:0.0}", CDbl(mem.TotalMemory) / 1024 / 1024) & " MB"
            End If
        Next
        Return ret
    End Function

    ''===================================================================
    '' ListAllPhones
    ''
    '' Adds all connected phones to list box
    '' 
    ''===================================================================
    'Private Sub ListAllPhones()

    '    FileBrowser.Timer1.Enabled = False
    '    ResetContent()

    '    Me.Text = ""
    '    ' Querying count of connected devices
    '    Me.ShowPhoneFolder("\")

    '    FileBrowser.LBL_PhoneFiles.Text = ""
    '    FileBrowser.bRefreshPhoneListBox = False
    '    FileBrowser.Timer1.Enabled = True
    'End Sub

    '===================================================================
    ' ResetContent
    '
    ' Clear contents of list box
    ' 
    '===================================================================
    Public Sub ResetContent()
        Me.Items.Clear()
    End Sub

       

    '===================================================================
    ' PhoneListBox_DoubleClick
    '
    ' Called when user doubleclicks list item
    ' 
    '===================================================================
    Private Sub PhoneListBox_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.DoubleClick
        Try
            If Me.SelectedIndex < 0 Then Return
            Dim o As Object = Me.Items(Me.SelectedIndex)
            If (TypeOf (o) Is NokiaFolderInfo) Then
                Me.SetCurrentFolder(DirectCast(o, NokiaFolderInfo))
            ElseIf (TypeOf (o) Is NokiaFileInfo) Then

            ElseIf (TypeOf (o) Is String AndAlso CStr(o) = "[..]") Then
                Me.SetCurrentFolder(Me.GetCurrentFolder.ParentFolder)
            End If

            'ElseIf m_wState = PHONELIST_STATE_PHONECONTENT Then
            '    PhoneItemDblClicked()
            'End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    

     
    Public Function GetCurrentDevice() As NokiaDevice
        Return Me.m_CurrentDevice
    End Function

    Function GetCurrentFolderName() As String
        Dim f As NokiaFolderInfo = Me.GetCurrentFolder
        If (f Is Nothing) Then Return ""
        Return f.FullPath
    End Function

    Public Function GetCurrentFolder() As NokiaFolderInfo
        Return Me.m_CurrentFolder
    End Function

    Function GetCurrentFileName() As String
        Dim f As NokiaFileInfo = Me.GetCurrentFile
        If (f Is Nothing) Then Return ""
        Return f.FullPath
    End Function

    Public Function GetCurrentFile() As NokiaFileInfo
        If (Me.SelectedIndex < 0) Then Return Nothing
        Dim o As Object = Me.Items(Me.SelectedIndex)
        If (TypeOf (o) Is NokiaFileInfo) Then
            Return o
        Else
            Return Nothing
        End If
    End Function

    Public Sub Refill()
        Me.Items.Clear()
        If (Me.m_CurrentFolder Is Nothing) Then Return
        Me.m_CurrentFolder.Refresh()
        Me.SetCurrentFolder(Me.m_CurrentFolder)
    End Sub

End Class
