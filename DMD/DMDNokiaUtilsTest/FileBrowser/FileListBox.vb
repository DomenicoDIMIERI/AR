'Filename    : FileListBox.vb
'Part of     : Phone Navigator VB.NET example
'Description : Implementation of phone navigator's file list box
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



'===================================================================
' FileListBox
Public Class FileListBox
    Inherits System.Windows.Forms.ListBox
    Private m_strCurrentPath As String

    '===================================================================
    ' Constructor
    '
    '===================================================================
    Public Sub New()
        m_strCurrentPath = ""
    End Sub

    '===================================================================
    ' GetCurrentFolder
    '
    ' Returns currently selected folder.
    ' If no folder is selected, returns folder, the content of which is
    ' currently shown.
    ' 
    '===================================================================
    Public Function GetCurrentFolder() As String
        Return m_strCurrentPath
    End Function

    '===================================================================
    ' GetCurrentFile
    '
    ' Returns currently selected file.
    ' If no file is selected, returns empty string.
    ' 
    '===================================================================
    Function GetCurrentFile() As String
        GetCurrentFile = ""
        Dim index As Integer = SelectedIndex
        If index <> -1 Then
            ' there is a selected item
            Dim strSelectedTxt As String
            strSelectedTxt = Items(index).ToString()
            If strSelectedTxt.Substring(0, 1) <> "[" Then
                ' selected item is a file
                GetCurrentFile = strSelectedTxt
            End If
        End If
    End Function

    '===================================================================
    ' PopulateList
    '
    ' Adds folders, files and drives of current folder to listbox
    ' 
    '===================================================================
    Public Sub PopulateList(ByVal strNewDirectory As String)
        Me.Items.Clear()
        Try
            Dim strPath As String

            If m_strCurrentPath.Length = 0 Then
                strPath = ("c:\")
            ElseIf strNewDirectory = ".." Then
                ' Parent folder
                strPath = m_strCurrentPath
                Dim iLastSeparator As Integer = strPath.LastIndexOf("\")
                strPath = strPath.Remove(iLastSeparator, strPath.Length - iLastSeparator)
                If strPath.Length < 3 And Not strPath.EndsWith("\") Then
                    strPath = strPath + "\"
                End If
            Else
                If m_strCurrentPath.EndsWith("\") Then
                    strPath = m_strCurrentPath + strNewDirectory
                Else
                    strPath = m_strCurrentPath + "\" + strNewDirectory
                End If
            End If

            Dim dirInfo As DirectoryInfo = New DirectoryInfo(strPath)

            If strPath.Length > 3 Then Me.Items.Add("[..]")

            ' List directories
            Dim dirInfos() As DirectoryInfo = dirInfo.GetDirectories()
            Dim di As DirectoryInfo
            For Each di In dirInfos
                Dim dirName As String = "[" + di.ToString() + "]"
                Me.Items.Add(dirName)
            Next

            ' List files
            Dim fileInfos() As FileInfo = dirInfo.GetFiles()
            Dim fi As FileInfo
            For Each fi In fileInfos
                Me.Items.Add(fi.ToString())
            Next

            ' List drives if the current level is at the root
            If strPath.Length >= 0 And strPath.Length <= 3 Then
                Dim strArrDrives() As String = System.IO.Directory.GetLogicalDrives()
                Dim drive As String
                For Each drive In strArrDrives
                    Dim driveName As String = "[-" + drive.Substring(0, 1) + "-]"
                    Me.Items.Add(driveName)
                Next
            End If

            m_strCurrentPath = strPath

        Catch
            ' If reading directory failed offer all logical drives
            Dim strArrDrives() As String = System.IO.Directory.GetLogicalDrives()
            Dim drive As String
            For Each drive In strArrDrives
                Dim driveName As String = "[-" + drive.Substring(0, 1) + "-]"
                Me.Items.Add(driveName)
            Next
        End Try
    End Sub

    '===================================================================
    ' OnDoubleClick
    '
    ' User has doubleclicked in the listbox.
    ' 
    '===================================================================
    Protected Overrides Sub OnDoubleClick(ByVal e As EventArgs)
        Dim iItemIdx As Integer = Me.SelectedIndex
        If iItemIdx >= 0 Then
            Dim strItemText As String = Me.Items(iItemIdx).ToString()
            If strItemText.Substring(0, 1) = "["c Then
                If strItemText.Substring(1, 1) = "-"c Then
                    ' drive selected
                    m_strCurrentPath = strItemText.Substring(2, 1) + ":\"
                    PopulateList("")
                Else
                    Dim strNewDirectory As String = strItemText
                    strNewDirectory = strNewDirectory.Remove(0, 1)
                    strNewDirectory = strNewDirectory.Remove(strNewDirectory.Length - 1, 1)
                    PopulateList(strNewDirectory)
                End If
            End If
        End If
        FileBrowser.LBL_PCFiles.Text = m_strCurrentPath
        MyBase.OnDoubleClick(e)
    End Sub
End Class
