Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    ''' <summary>
    ''' Rappresenta la collezione degli SMS memorizzati in una cartella
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CSMSMessagesCollection
        Inherits System.Collections.ReadOnlyCollectionBase

        Private m_Folder As CSMSFolder

        Public Sub New()
            Me.m_Folder = Nothing
        End Sub

        Public Sub New(ByVal folder As CSMSFolder)
            Me.New()
            Me.Load(folder, False)
        End Sub

        Public ReadOnly Property Device As Nokia.NokiaDevice
            Get
                Return Me.m_Folder.Device
            End Get
        End Property

        Public ReadOnly Property Folder As CSMSFolder
            Get
                Return Me.m_Folder
            End Get
        End Property

        Default Public Shadows ReadOnly Property Item(ByVal index As Integer) As CSMSMessage
            Get
                Return DirectCast(MyBase.InnerList.Item(index), CSMSMessage)
            End Get
        End Property

        Friend Sub Load(ByVal folder As CSMSFolder, ByVal useCache As Boolean)
            If (folder Is Nothing) Then Throw New ArgumentNullException("folder")
            Me.m_Folder = folder

            ' Check PIM connection to SMS folders and open it if needed
            'Dim iRet As Integer = Me.Device.SMS.CheckSMSConnection() 'strSerialNumber)
            'If iRet <> CONA_OK Then Throw New Exception("Errore imprevisto")

            ' Set SMS folder target path
            'Dim folderInfo As CA_FOLDER_INFO = Me.Folder.folderInfo ' MapCAFolderInfoToCFI(parentItem.Tag)
            If folder.folderInfo.iFolderId = 0 Then Return ' SMS root folder

            ' Read all the SMS item IDs from the connected device 
            Dim caIDList As CA_ID_LIST
            caIDList.iSize = Marshal.SizeOf(caIDList)

            'Dim buf As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(caIDList))
            'Marshal.StructureToPtr(caIDList, buf, True)
            Dim iRet As Integer
            If (useCache) Then
                iRet = CAGetIDList(folder.GetConnectionHandle, Me.Folder.folderInfo.iFolderId, CA_OPTION_USE_CACHE, caIDList)
            Else
                iRet = CAGetIDList(folder.GetConnectionHandle, Me.Folder.folderInfo.iFolderId, 0, caIDList)
            End If


            ' NOTE:
            ' Item exists but ECONA_NOT_FOUND is sometimes returned 
            ' If library / device is not ready yet (delayed read).
            ' 
            If iRet <> CONA_OK And iRet <> ECONA_NOT_FOUND Then ShowErrorMessage("CAGetIDList", iRet)
                'caIDList = Marshal.PtrToStructure(buf, GetType(CA_ID_LIST))

                Dim hOperHandle As Integer = 0
                iRet = CABeginOperation(folder.GetConnectionHandle, 0, hOperHandle)
                If iRet <> CONA_OK Then ShowErrorMessage("CABeginOperation", iRet)

                'Dim bErrorShown As Boolean = False
                For k As Integer = 0 To caIDList.iUIDCount - 1
                    ' Read SMS item from the connected device
                    Dim UID As CA_ITEM_ID = GetUidFromBuffer(k, caIDList.pUIDs)
                    Dim item As New CSMSMessage(Me.Folder)
                    item.ReadSMS(hOperHandle, UID)
                    MyBase.InnerList.Add(item)
                Next

                iRet = CAEndOperation(hOperHandle)
                If iRet <> CONA_OK Then
                    ShowErrorMessage("CAEndOperation", iRet)
                End If

                'iRet = CAFreeIdListStructure(buf)
                iRet = CAFreeIdListStructure(caIDList)
                If iRet <> CONA_OK Then
                    ShowErrorMessage("CAFreeIdListStructure", iRet)
                End If
            ' Marshal.FreeHGlobal(buf)

        End Sub

        Public Sub Refresh(Optional ByVal useCache As Boolean = False)
            SyncLock Me.Device
                Me.Load(Me.m_Folder, useCache)
            End SyncLock
        End Sub

        '===================================================================
        ' GetUidFromBuffer
        '
        ' Retrieves UID from unmanaged memory buffer
        '
        '===================================================================
        Private Function GetUidFromBuffer(ByVal iIndex As Integer, ByVal pUIds As IntPtr) As CA_ITEM_ID
            Dim iPtr As Int64 = pUIds.ToInt64 + (iIndex * Marshal.SizeOf(GetType(CA_ITEM_ID))) ' Calculate beginning of item 'iIndex'
            Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr) ' Convert integer to pointer
            Return CType(Marshal.PtrToStructure(ptr, GetType(CA_ITEM_ID)), CA_ITEM_ID) ' Copy data from buffer
        End Function

        ''===================================================================
        '' GetEmptyPIMDate
        '' 
        '' Gets empty CA_DATA_DATE
        '' 
        ''===================================================================
        'Private Sub GetEmptyPIMDate(ByRef pimDate As CA_DATA_DATE)
        '    pimDate.iSize = Marshal.SizeOf(pimDate)
        '    pimDate.bDay = 0
        '    pimDate.lBias = 0
        '    pimDate.bHour = 0
        '    pimDate.bMinute = 0
        '    pimDate.bMonth = 0
        '    pimDate.bSecond = 0
        '    pimDate.lTimeZoneBias = 0
        '    pimDate.wYear = UInt16.Parse(0)
        'End Sub

        Friend Overridable Sub Remove(item As CSMSMessage)
            Me.InnerList.Remove(item)
        End Sub



    End Class

End Namespace