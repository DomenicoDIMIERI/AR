Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    ''' <summary>
    ''' Rappresenta una collezione di bookmarks in una cartella
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CBookMarksCollection
        Inherits System.Collections.ReadOnlyCollectionBase

        Private m_Folder As CBookMarkFolder

        Public Sub New()
            Me.m_Folder = Nothing
        End Sub

        Public Sub New(ByVal folder As CBookMarkFolder)
            Me.New()
            Me.Load(folder)
        End Sub

        Public ReadOnly Property Device As Nokia.NokiaDevice
            Get
                Return Me.m_Folder.Device
            End Get
        End Property

        Public ReadOnly Property Folder As CBookMarkFolder
            Get
                Return Me.m_Folder
            End Get
        End Property

        Default Public Shadows ReadOnly Property Item(ByVal index As Integer) As CBookMarkItem
            Get
                Return DirectCast(MyBase.InnerList.Item(index), CBookMarkItem)
            End Get
        End Property

        Friend Sub Load(ByVal folder As CBookMarkFolder)
            If (folder Is Nothing) Then Throw New ArgumentNullException("folder")
            Me.m_Folder = folder

            ' Check PIM connection to calendar folder and open it if needed

            ' Set Bookmark folder target path
            'Dim folderInfo As CA_FOLDER_INFO = MapCAFolderInfoToCFI(parentItem.Tag)
            ' Read all the Bookmark item IDs from the connected device 
            Dim caIDList As CA_ID_LIST
            caIDList.iSize = Marshal.SizeOf(caIDList)
            Dim buf As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(caIDList))
            Marshal.StructureToPtr(caIDList, buf, True)
            Dim iRet As Integer = CAGetIDList(folder.GetConnectionHandle, folder.folderInfo.iFolderId, CA_OPTION_USE_CACHE, buf)
            If iRet <> CONA_OK Then ShowErrorMessage("CAGetIDList", iRet)

            caIDList = CType(Marshal.PtrToStructure(buf, GetType(CA_ID_LIST)), CA_ID_LIST)
            Dim hOperHandle As Integer = 0
            iRet = CABeginOperation(folder.GetConnectionHandle, 0, hOperHandle)
            If iRet <> CONA_OK Then ShowErrorMessage("CABeginOperation", iRet)
            Dim bErrorShown As Boolean = False
            Dim k As Integer
            For k = 0 To caIDList.iUIDCount - 1
                ' Read Bookmark item from the connected device
                Dim UID As CA_ITEM_ID = GetUidFromBuffer(k, caIDList.pUIDs)
                Dim item As New CBookMarkItem(Me.Folder)
                item.ReadBookMark(hOperHandle, UID)
                MyBase.InnerList.Add(item)
            Next
            iRet = CAEndOperation(hOperHandle)
            If iRet <> CONA_OK Then ShowErrorMessage("CAEndOperation", iRet)

            iRet = CAFreeIdListStructure(buf)
            If iRet <> CONA_OK Then ShowErrorMessage("CAFreeIdListStructure", iRet)
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

        Protected Friend Overridable Sub Remove(item As CBookMarkItem)
            Me.InnerList.Remove(item)
        End Sub



    End Class

End Namespace