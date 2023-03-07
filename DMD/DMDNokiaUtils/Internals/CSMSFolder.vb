Imports System.Runtime.InteropServices
Imports DMD.Internals
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals



    ''' <summary>
    ''' Rappresenta una cartella che contiene SMS
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CSMSFolder
        Inherits CBaseFolder

        Private m_Messages As CSMSMessagesCollection

        Public Sub New()
            Me.m_Messages = Nothing
        End Sub

        Friend Sub New(ByVal parent As CSMSFolder)
            Me.New()
            Me.SetDevice(parent.Device)
            Me.SetParentFolder(parent)
        End Sub

        Friend Sub New(ByVal device As Nokia.NokiaDevice)
            MyBase.New(device)
        End Sub

        ''' <summary>
        ''' Restituisce il tipo di cartella associata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FolderType As SMSFolderTypes
            Get
                Return CType(Me.folderInfo.iFolderId, SMSFolderTypes)
            End Get
        End Property

        ''' <summary>
        ''' Restituisce i messaggi memorizzati nella cartella
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Messages As CSMSMessagesCollection
            Get
                SyncLock Me.m_Device
                    If (Me.m_Messages Is Nothing) Then Me.m_Messages = New CSMSMessagesCollection(Me)
                    Return Me.m_Messages
                End SyncLock
            End Get
        End Property

        Protected Overrides Sub InitializeFolderData()
            '' Check PIM connection to SMS folders and open it if needed
            'Dim iRet As Integer = Me.Device.SMS.CheckSMSConnection()
            'If iRet <> CONA_OK Then Throw New Exception
            '' Get SMS folder info
            'Dim folderInfo As New CA_FOLDER_INFO
            'folderInfo.iSize = Marshal.SizeOf(folderInfo)
            'Dim bufItem As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_FOLDER_INFO)))
            'Marshal.StructureToPtr(folderInfo, bufItem, True)
            'iRet = CAGetFolderInfo(Me.Device.SMS.m_hSMS, bufItem)
            'If iRet = CONA_OK Then
            '    folderInfo = Marshal.PtrToStructure(bufItem, GetType(CA_FOLDER_INFO))
            '    'AddFolder("SMS Messages", folderInfo, parentItem, m_iIconSMSMessagesIndex, m_iIconSMSIndex)
            'Else
            '    ShowErrorMessage("CAGetFolderInfo", iRet)
            'End If

            'Me.m_Name = folderInfo.pstrName
            'Me.m_Path = folderInfo.pstrPath

            'Dim iResult As Integer = CAFreeFolderInfoStructure(bufItem)
            'If iResult <> CONA_OK Then ShowErrorMessage("CAFreeFolderInfoStructure", iResult)
            'Marshal.FreeHGlobal(bufItem)

        End Sub



        ''===================================================================
        '' FreeUIDMappingMemory
        ''
        ''   Free's memory allocated by MapCAItemIDToUID call.
        ''
        ''===================================================================
        'Private Sub FreeUIDMappingMemory(ByVal UID As CA_ITEM_ID)
        '    Marshal.FreeHGlobal(UID.pbUid)
        '    UID.pbUid = IntPtr.Zero
        'End Sub



        Protected Overrides Function InstantiateSubFolder() As CBaseFolder
            Return New CSMSFolder(Me)
        End Function

        Friend Overrides Function GetConnectionHandle() As IntPtr
            Return Me.Device.SMS.GetConnectionHandle
        End Function

        Protected Friend Overrides Sub NotifyDeleted(item As CBaseItem)
            If (TypeOf (item) Is CSMSMessage) Then
                If Me.m_Messages IsNot Nothing Then Me.m_Messages.Remove(DirectCast(item, CSMSMessage))
            Else
                MyBase.NotifyDeleted(item)
            End If
        End Sub
    End Class

End Namespace