Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    ''' <summary>
    ''' Rappresenta una cartella che contiene SMS
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class CBaseFolder
        Inherits CBaseItem
        Friend folderInfo As CA_FOLDER_INFO
        Friend m_SubFolders As CFoldersCollection
        Friend m_Name As String
        Friend m_Path As String

        Public Sub New()
            Me.m_Name = ""
        End Sub

        Friend Sub New(ByVal device As Nokia.NokiaDevice)
            Me.New()
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            Me.SetDevice(device)
        End Sub

        Friend Sub New(ByVal parent As CBaseFolder)
            Me.New()
            If (parent Is Nothing) Then Throw New ArgumentNullException("parent")
            Me.SetDevice(parent.Device)
            Me.SetParentFolder(parent)
        End Sub


        ''' <summary>
        ''' Restituisce l'elenco delle sottocartelle
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SubFolders As CFoldersCollection
            Get
                SyncLock Me.m_Device
                    If (Me.m_SubFolders Is Nothing) Then Me.m_SubFolders = Me.InitializeSubFolders()
                    Return Me.m_SubFolders
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce i lnome della cartella
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Name As String
            Get
                Return Me.m_Name
            End Get
        End Property

        Protected Friend Overridable Sub SetName(ByVal name As String)
            Me.m_Name = name
        End Sub

        Public ReadOnly Property Path As String
            Get
                Return Me.m_Path
            End Get
        End Property

        Protected Friend Overridable Sub SetPath(ByVal value As String)
            Me.m_Path = value
        End Sub

        Protected MustOverride Sub InitializeFolderData()

        Protected Overridable Function InitializeSubFolders() As CFoldersCollection
            Dim ret As New CFoldersCollection(Me)
            For i As Integer = 0 To folderInfo.iSubFolderCount - 1
                Dim subFolder As CBaseFolder = Me.InstantiateSubFolder()
                ' Calculate beginning of CONAPI_DEVICE structure of item 'i'
                Dim iPtr As Int64 = folderInfo.pSubFolders.ToInt64 + i * Marshal.SizeOf(GetType(CA_FOLDER_INFO))
                ' Convert integer to pointer
                Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
                ' Copy data from buffer
                subFolder.folderInfo = CType(Marshal.PtrToStructure(ptr, GetType(CA_FOLDER_INFO)), CA_FOLDER_INFO)
                'AddFolder(strRootFolder, subFolderInfo, itemY, iIconFolderIndex, iIconItemIndex)
                subFolder.m_Name = subFolder.folderInfo.pstrName
                subFolder.m_Path = subFolder.folderInfo.pstrPath

                ret.InternalAdd(subFolder)

                Dim tmp As Integer = subFolder.SubFolders.Count
            Next
            Return ret
        End Function

        Protected MustOverride Function InstantiateSubFolder() As CBaseFolder

        Protected Friend Overrides Sub SetDevice(value As NokiaDevice)
            MyBase.SetDevice(value)
            Me.InitializeFolderData()
        End Sub

        Friend MustOverride Function GetConnectionHandle() As IntPtr

        Friend Overridable Function CreateSubFolder(ByVal name As String) As CBaseFolder
            ' Creates folder to currently connected device
            Dim hOperHandle As Integer = 0
            Dim iRet As Integer = CABeginOperation(Me.GetConnectionHandle, 0, hOperHandle)
            If iRet <> CONA_OK Then
                ShowErrorMessage("CABeginOperation", iRet)
            End If
            Dim strFolder As String
            Dim buffer As IntPtr
            'If treeNode.ImageIndex = m_iIconSMSMessagesIndex Then
            ' Fill item id struct
            Dim itemId As CA_ITEM_ID
            itemId.iSize = Marshal.SizeOf(itemId)
            itemId.iFolderId = CA_MESSAGE_FOLDER_USER_FOLDERS
            buffer = Marshal.AllocHGlobal(Marshal.SizeOf(itemId))
            Marshal.StructureToPtr(itemId, buffer, True)
            ' Create path for user defined message folder
            strFolder = "predefuserfolders\" & name 'dlg.TextFolder.Text
            ' Find SMS messages root folder
            'Else
            'Dim UID As CA_FOLDER_INFO = MapCAFolderInfoToCFI(TVW_Navigator.SelectedNode.Tag)
            '' Browse folder up to current item root and
            '' build path for new subfolder (root UID used for operation) 
            'strFolder = dlg.TextFolder.Text
            'While Not (TreeNode.Parent Is Nothing)
            '    If Not (TreeNode.Parent.Tag Is Nothing) Then
            '        UID = MapCAFolderInfoToCFI(TreeNode.Tag)
            '        If UID.pstrName <> "\" Then
            '            strFolder = UID.pstrName + "\" + strFolder
            '        End If
            '        TreeNode = TreeNode.Parent
            '    End If
            'End While

            'buffer = Marshal.AllocHGlobal(Marshal.SizeOf(UID))
            'Marshal.StructureToPtr(UID, buffer, True)
            'End If

            iRet = CACreateFolder(hOperHandle, buffer, strFolder)
            If iRet <> CONA_OK Then
                ShowErrorMessage("CACreateFolder", iRet)
            End If
            iRet = CACommitOperations(hOperHandle, IntPtr.Zero)
            If iRet <> CONA_OK Then
                ShowErrorMessage("CACommitOperations", iRet)
            End If
            iRet = CAEndOperation(hOperHandle)
            If iRet <> CONA_OK Then
                ShowErrorMessage("CAEndOperation", iRet)
            End If
            Marshal.FreeHGlobal(buffer)

            ' Fill connected devices, target folders and PIM items in tree view
            Throw New NotImplementedException

        End Function

        Protected Overrides Sub InternalDelete()
            'If MsgBox("Are you sure you want to delete selected item?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Confirm Item Delete") = MsgBoxResult.Yes Then
            Dim hOperHandle As Integer = 0
            Dim iRet As Integer = CABeginOperation(Me.GetConnectionHandle, 0, hOperHandle)
            If iRet <> CONA_OK Then ShowErrorMessage("CABeginOperation", iRet)

            ' Deletes PIM item from currently connected device
            Dim CFI As CA_FOLDER_INFO = Me.folderInfo
            Dim buffer As IntPtr = IntPtr.Zero


            buffer = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_FOLDER_INFO)))
            Marshal.StructureToPtr(CFI, buffer, True)
            iRet = CADeleteFolder(hOperHandle, buffer)


            If iRet <> CONA_OK Then ShowErrorMessage("DADeleteItem", iRet)

            Marshal.FreeHGlobal(buffer)

            iRet = CACommitOperations(hOperHandle, IntPtr.Zero)
            If iRet <> CONA_OK Then ShowErrorMessage("CACommitOperations", iRet)

            iRet = CAEndOperation(hOperHandle)
            If iRet <> CONA_OK Then ShowErrorMessage("CAEndOperation", iRet)

            'FreeUIDMappingMemory(UID)
        End Sub

        Protected Friend Overridable Sub NotifyDeleted(ByVal item As CBaseItem)
            If (TypeOf (item) Is CBaseFolder AndAlso Me.m_SubFolders IsNot Nothing) Then
                Me.m_SubFolders.Remove(item)
            End If
        End Sub

    End Class

End Namespace