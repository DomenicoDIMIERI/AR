Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    ''' <summary>
    ''' Rappresenta la collezione degli MMS memorizzati in una cartella
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CCalendarItemsCollection
        Inherits System.Collections.ReadOnlyCollectionBase

        Private m_Folder As CCalendarFolder

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Folder = Nothing
        End Sub

        Friend Sub New(ByVal folder As CCalendarFolder)
            Me.New()
            Me.Load(folder)
        End Sub

        Public ReadOnly Property Device As Nokia.NokiaDevice
            Get
                Return Me.m_Folder.Device
            End Get
        End Property

        Public ReadOnly Property Folder As CCalendarFolder
            Get
                Return Me.m_Folder
            End Get
        End Property

        Protected Friend Sub Load(ByVal folder As CCalendarFolder) 'ByVal strSerialNumber As String, ByVal parentItem As System.Windows.Forms.TreeNode)
            If (folder Is Nothing) Then Throw New ArgumentNullException("folder")
            Me.m_Folder = folder

            ' Set calendar folder target path
            Dim folderInfo As CA_FOLDER_INFO = folder.folderInfo ' MapCAFolderInfoToCFI(parentItem.Tag)
            ' Read all the calendar item IDs from the connected device 
            Dim caIDList As CA_ID_LIST
            caIDList.iSize = Marshal.SizeOf(caIDList)
            Dim buf As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(caIDList))
            Marshal.StructureToPtr(caIDList, buf, True)
            Dim iRet As Integer = CAGetIDList(folder.GetConnectionHandle, folderInfo.iFolderId, CA_OPTION_USE_CACHE, buf)
            If iRet <> CONA_OK Then ShowErrorMessage("CAGetIDList", iRet)
            caIDList = CType(Marshal.PtrToStructure(buf, GetType(CA_ID_LIST)), CA_ID_LIST)
            Dim hOperHandle As Integer = 0
            iRet = CABeginOperation(folder.GetConnectionHandle, 0, hOperHandle)
            If iRet <> CONA_OK Then ShowErrorMessage("CABeginOperation", iRet)
            Dim bErrorShown As Boolean = False
            Dim k As Integer
            Dim item As CCalendarItem
            For k = 0 To caIDList.iUIDCount - 1
                item = New CCalendarItem(Me.Folder)
                Dim UID As CA_ITEM_ID = GetUidFromBuffer(k, caIDList.pUIDs)
                'item.ItemType = UID.i
                item.ReadCalendarItem(hOperHandle, UID)
                MyBase.InnerList.Add(item)
            Next
            iRet = CAEndOperation(hOperHandle)
            If iRet <> CONA_OK Then
                ShowErrorMessage("CAEndOperation", iRet)
            End If
            iRet = CAFreeIdListStructure(buf)
            If iRet <> CONA_OK Then
                ShowErrorMessage("CAFreeIdListStructure", iRet)
            End If
            Marshal.FreeHGlobal(buf)


        End Sub


        '===================================================================
        ' GetUidFromBuffer
        '
        ' Retrieves UID from unmanaged memory buffer
        '
        '===================================================================
        Private Function GetUidFromBuffer(ByVal iIndex As Integer, ByVal pUIds As IntPtr) As CA_ITEM_ID
            ' Calculate beginning of item 'iIndex'
            Dim iPtr As Int64 = pUIds.ToInt64 + (iIndex * Marshal.SizeOf(GetType(CA_ITEM_ID)))
            ' Convert integer to pointer
            Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
            ' Copy data from buffer
            Return CType(Marshal.PtrToStructure(ptr, GetType(CA_ITEM_ID)), CA_ITEM_ID)
        End Function

        Protected Friend Overridable Sub Remove(item As CCalendarItem)
            Me.InnerList.Remove(item)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace