Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS


Namespace Internals

    ''' <summary>
    ''' Rappresenta i Bookmarks
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CBookMarks
        Inherits CBookMarkFolder

        Private m_hBookmark As IntPtr
        Private m_pBookmarkBuffer As IntPtr
        
        Public Sub New()
            Me.m_hBookmark = IntPtr.Zero
            Me.m_pBookmarkBuffer = IntPtr.Zero
        End Sub

        Friend Sub New(ByVal device As NokiaDevice)
            MyBase.New(device)
        End Sub

        

        '===================================================================
        ' CloseBookmarkConnection
        '
        ' Close PIM connection to Bookmark folders
        '
        '===================================================================
        Private Function CloseBookmarkConnection() As Integer
            Dim iRet As Integer = CONA_OK
            If Not Me.m_hBookmark.Equals(IntPtr.Zero) Then
                ' Unregister CallBack
                Dim iResult As Integer = CARegisterNotifyCallback(Me.m_hBookmark, API_UNREGISTER, Me.Device.pCANotifyCallBack)

                ' Close PIM connection
                iRet = DACloseCA(m_hBookmark)
                If iRet <> CONA_OK Then ShowErrorMessage("DACloseCA", iRet)
                Me.m_hBookmark = IntPtr.Zero
            End If
            Return iRet
        End Function


        Protected Overrides Sub InitializeFolderData()
            ' Get Calendar folder info
            Me.folderInfo.iSize = Marshal.SizeOf(folderInfo)
            Dim bufItem As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_FOLDER_INFO)))
            Marshal.StructureToPtr(folderInfo, bufItem, True)
            Dim iRet As Integer = CAGetFolderInfo(Me.GetConnectionHandle, bufItem)
            If iRet <> CONA_OK Then ShowErrorMessage("DAOpenCA", iRet)

            folderInfo = CType(Marshal.PtrToStructure(bufItem, GetType(CA_FOLDER_INFO)), CA_FOLDER_INFO)
            iRet = Me.SubFolders.Count

            Dim iResult As Integer = CAFreeFolderInfoStructure(bufItem)
            If iResult <> CONA_OK Then ShowErrorMessage("CAFreeFolderInfoStructure", iResult)
            Marshal.FreeHGlobal(bufItem)
        End Sub



        ''===================================================================
        '' ShowNewBookmarkDlg
        ''
        '' Shows "New Text Message" dialog and writes SMS to device
        ''
        ''===================================================================
        'Private Sub ShowNewBookmarkDlg()
        '    ' Open "New Bookmark" dialog
        '    Dim dlg As New BookmarkDlg
        '    If dlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '        '
        '        CheckBookmarkConnection(GetCurrentDevice())
        '        '
        '        Dim dataBookmark As CA_DATA_BOOKMARK = New CA_DATA_BOOKMARK
        '        dataBookmark.iSize = Marshal.SizeOf(dataBookmark)

        '        dataBookmark.pstrTitle = dlg.TextTitle.Text
        '        dataBookmark.pstrBookMarkUrl = dlg.TextURL.Text
        '        dataBookmark.pstrUrlShortcut = dlg.TextShort.Text

        '        Dim bookmarksNode As System.Windows.Forms.TreeNode
        '        If TVW_Navigator.SelectedNode.ImageIndex = m_iIconBookmarkIndex Then
        '            bookmarksNode = TVW_Navigator.SelectedNode
        '        Else
        '            bookmarksNode = TVW_Navigator.SelectedNode.Parent
        '        End If
        '        Dim folderInfo As CA_FOLDER_INFO = MapCAFolderInfoToCFI(bookmarksNode.Tag)

        '        ' Write new Bookmark item to currently connected device
        '        Dim hOperHandle As Integer = 0
        '        Dim iRet As Integer = CABeginOperation(m_hCurrentConnection, 0, hOperHandle)
        '        If iRet <> CONA_OK Then ShowErrorMessage("CABeginOperation", iRet)
        '        CARegisterOperationCallback(hOperHandle, API_REGISTER, pCAOperationCallback)
        '        If iRet <> CONA_OK Then ShowErrorMessage("CARegisterOperationCallback", iRet)
        '        Dim itemUid As CA_ITEM_ID
        '        itemUid.iSize = Marshal.SizeOf(itemUid)
        '        itemUid.iFolderId = folderInfo.iFolderId
        '        itemUid.iFolderId = folderInfo.iFolderId
        '        itemUid.iStatus = 0
        '        itemUid.iTemporaryID = 0
        '        itemUid.iUidLen = 0
        '        itemUid.pbUid = IntPtr.Zero
        '        Dim buf As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_ITEM_ID)))
        '        Marshal.StructureToPtr(itemUid, buf, True)
        '        ' Allocate memory for buffer
        '        Dim buf2 As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_DATA_BOOKMARK)))
        '        Marshal.StructureToPtr(dataBookmark, buf2, True)
        '        iRet = CAWriteItem(hOperHandle, buf, 0, CA_DATA_FORMAT_STRUCT, buf2)
        '        If iRet <> CONA_OK Then ShowErrorMessage("DAWriteItem", iRet)
        '        ' Free memory allocated by DA API
        '        iRet = CAFreeItemData(m_hCurrentConnection, CA_DATA_FORMAT_STRUCT, buf2)
        '        If iRet <> CONA_OK Then ShowErrorMessage("DAFreeItemData", iRet)
        '        Marshal.FreeHGlobal(buf2)
        '        Marshal.FreeHGlobal(buf)
        '        iRet = CACommitOperations(hOperHandle, IntPtr.Zero)
        '        If iRet <> CONA_OK Then ShowErrorMessage("CACommitOperations", iRet)
        '        CARegisterOperationCallback(hOperHandle, API_UNREGISTER, pCAOperationCallback)
        '        If iRet <> CONA_OK Then ShowErrorMessage("CARegisterOperationCallback", iRet)
        '        iRet = CAEndOperation(hOperHandle)
        '        If iRet <> CONA_OK Then ShowErrorMessage("CAEndOperation", iRet)
        '        bookmarksNode.Nodes.Clear()
        '        GetBookmarks(GetCurrentDevice(), bookmarksNode)
        '    End If
        'End Sub

        Friend Overrides Function GetConnectionHandle() As IntPtr
            ' No PIM connection, open it
            If (Me.m_hBookmark.Equals(IntPtr.Zero)) Then
                Dim pstrSerialNumber As IntPtr = Marshal.StringToCoTaskMemUni(Me.Device.SerialNumber)
                Dim iMedia As Integer = API_MEDIA_ALL
                Dim iTarget As Integer = CA_TARGET_BOOKMARKS
                Dim iRet As Integer = DAOpenCA(pstrSerialNumber, iMedia, iTarget, Me.m_hBookmark)
                'If iRet <> CONA_OK And iRet <> ECONA_NOT_SUPPORTED_DEVICE Then ShowErrorMessage("DAOpenCA", iRet)
                If iRet <> CONA_OK Then
                    If iRet = ECONA_NOT_SUPPORTED_DEVICE Then
                        Throw New NotSupportedException("Phone does not support bookmarks")
                    Else
                        ShowErrorMessage("DAOpenCA", iRet)
                    End If
                End If
                Marshal.FreeCoTaskMem(pstrSerialNumber)

                ' Register CA notification callback function
                If Not Me.m_hBookmark.Equals(IntPtr.Zero) Then
                    Dim iResult As Integer = CARegisterNotifyCallback(m_hBookmark, API_REGISTER, Me.Device.pCANotifyCallBack)
                    If iResult <> CONA_OK Then ShowErrorMessage("CARegisterNotifyCallback", iResult)
                End If
            End If

            Return Me.m_hBookmark
        End Function



        Protected Overrides Sub InternalDelete()

        End Sub
 
    End Class

End Namespace