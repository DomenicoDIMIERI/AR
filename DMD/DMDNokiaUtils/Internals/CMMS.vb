Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS


Namespace Internals

    ''' <summary>
    ''' Rappresenta lo stazione per gli SMS
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CMMS
        Inherits CMMSFolder

        Private m_hMMS As IntPtr = IntPtr.Zero
        Private m_pMMSBuffer As IntPtr = IntPtr.Zero
        Private pCANotifyCallBack As CANotifyCallbackDelegate
        Private m_InBox As CMMSFolder
        Private m_OutBox As CMMSFolder
        Private m_Sent As CMMSFolder
        Private m_Archive As CMMSFolder
        Private m_Drafts As CMMSFolder
        Private m_Templates As CMMSFolder

        Public Sub New()
            Me.pCANotifyCallBack = AddressOf CANotifyCallBack
        End Sub

        Friend Sub New(ByVal device As Nokia.NokiaDevice)
            Me.New()
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            Me.SetDevice(device)
        End Sub

        Private Function GetFolderById(ByVal id As SMSFolderTypes) As CMMSFolder
            If (id = SMSFolderTypes.Root) Then Return Me
            For Each f As CMMSFolder In Me.SubFolders
                If (f.FolderType = id) Then Return f
            Next
            Return Nothing
        End Function

        Public ReadOnly Property InBox As CMMSFolder
            Get
                If (Me.m_InBox Is Nothing) Then Me.m_InBox = Me.GetFolderById(SMSFolderTypes.InBox)
                Return Me.m_InBox
            End Get
        End Property

        Public ReadOnly Property OutBox As CMMSFolder
            Get
                If (Me.m_OutBox Is Nothing) Then Me.m_OutBox = Me.GetFolderById(SMSFolderTypes.OutBox)
                Return Me.m_OutBox
            End Get
        End Property

        Public ReadOnly Property Sent As CMMSFolder
            Get
                If (Me.m_Sent Is Nothing) Then Me.m_Sent = Me.GetFolderById(SMSFolderTypes.Sent)
                Return Me.m_Sent
            End Get
        End Property

        Public ReadOnly Property Archive As CMMSFolder
            Get
                If (Me.m_Archive Is Nothing) Then Me.m_Archive = Me.GetFolderById(SMSFolderTypes.Archive)
                Return Me.m_Archive
            End Get
        End Property

        Public ReadOnly Property Drafts As CMMSFolder
            Get
                If (Me.m_Drafts Is Nothing) Then Me.m_Drafts = Me.GetFolderById(SMSFolderTypes.Drafts)
                Return Me.m_Drafts
            End Get
        End Property

        Public ReadOnly Property Templates As CMMSFolder
            Get
                If (Me.m_Templates Is Nothing) Then Me.m_Templates = Me.GetFolderById(SMSFolderTypes.Templates)
                Return Me.m_Templates
            End Get
        End Property

        '===================================================================
        ' CloseMMSConnection
        '
        ' Close PIM connection to MMS folders
        '
        '===================================================================
        Private Function CloseMMSConnection() As Integer
            Dim iRet As Integer = CONA_OK
            If Not Me.m_hMMS.Equals(IntPtr.Zero) Then
                ' Unregister CallBack
                Dim iResult As Integer = CARegisterNotifyCallback(Me.m_hMMS, API_UNREGISTER, pCANotifyCallBack)
                ' Close PIM connection
                iRet = DACloseCA(Me.m_hMMS)
                If iRet <> CONA_OK Then ShowErrorMessage("DACloseCA", iRet)
                Me.m_hMMS = IntPtr.Zero
            End If
            Return iRet
        End Function

        Friend Overrides Function GetConnectionHandle() As IntPtr
            If Me.m_hMMS.Equals(IntPtr.Zero) Then
                ' No PIM connection, open it
                Dim pstrSerialNumber As IntPtr = Marshal.StringToCoTaskMemUni(Me.Device.SerialNumber) '(strSerialNumber)
                Dim iMedia As Integer = API_MEDIA_ALL
                Dim iTarget As Integer = CA_TARGET_MMS_MESSAGES
                Dim iRet As Integer = DAOpenCA(pstrSerialNumber, iMedia, iTarget, Me.m_hMMS)
                If iRet <> CONA_OK And iRet <> ECONA_NOT_SUPPORTED_DEVICE Then ShowErrorMessage("DAOpenCA", iRet)
                Marshal.FreeCoTaskMem(pstrSerialNumber)
                ' Register CA notification callback function
                If Not Me.m_hMMS.Equals(IntPtr.Zero) Then
                    Dim iResult As Integer = CARegisterNotifyCallback(Me.m_hMMS, API_REGISTER, pCANotifyCallBack)
                    If iResult <> CONA_OK Then ShowErrorMessage("CARegisterNotifyCallback", iResult)
                End If
            End If
            Return Me.m_hMMS
        End Function

        ''===================================================================
        '' GetMMSFolders
        ''
        '' Gets SMS folder info and creates folders in tree view
        ''
        ''===================================================================
        'Private Sub GetMMSFolders(ByVal strSerialNumber As String, ByVal parentItem As System.Windows.Forms.TreeNode)
        '    ' Check PIM connection to MMS folders and open it if needed
        '    Dim iRet As Integer = CheckMMSConnection(strSerialNumber)
        '    If iRet = CONA_OK Then
        '        ' Get MMS folder info
        '        Dim folderInfo As CA_FOLDER_INFO
        '        folderInfo = New CA_FOLDER_INFO
        '        folderInfo.iSize = Marshal.SizeOf(folderInfo)
        '        Dim bufItem As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_FOLDER_INFO)))
        '        Marshal.StructureToPtr(folderInfo, bufItem, True)
        '        iRet = CAGetFolderInfo(m_hMMS, bufItem)
        '        If iRet = CONA_OK Then
        '            folderInfo = Marshal.PtrToStructure(bufItem, GetType(CA_FOLDER_INFO))
        '            'AddFolder("MMS Messages", folderInfo, parentItem, m_iIconMMSMessagesIndex, m_iIconMMSIndex)
        '        Else
        '            ShowErrorMessage("CAGetFolderInfo", iRet)
        '        End If
        '        Dim iResult As Integer = CAFreeFolderInfoStructure(bufItem)
        '        If iResult <> CONA_OK Then ShowErrorMessage("CAFreeFolderInfoStructure", iResult)
        '        Marshal.FreeHGlobal(bufItem)
        '    End If
        'End Sub

        Protected Overrides Sub InitializeFolderData()
            ' Check PIM connection to MMS folders and open it if needed
            ' Get MMS folder info
            folderInfo.iSize = Marshal.SizeOf(folderInfo)
            Dim bufItem As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_FOLDER_INFO)))
            Marshal.StructureToPtr(folderInfo, bufItem, True)
            Dim iRet As Integer = CAGetFolderInfo(Me.GetConnectionHandle, bufItem)
            If iRet <> CONA_OK Then ShowErrorMessage("CAGetFolderInfo", iRet)
            folderInfo = CType(Marshal.PtrToStructure(bufItem, GetType(CA_FOLDER_INFO)), CA_FOLDER_INFO)
            iRet = Me.SubFolders.Count
            Dim iResult As Integer = CAFreeFolderInfoStructure(bufItem)
            If iResult <> CONA_OK Then ShowErrorMessage("CAFreeFolderInfoStructure", iResult)
            Marshal.FreeHGlobal(bufItem)
        End Sub

        '===================================================================
        ' CANotifyCallBack
        '
        ' Callback function for CA notifications
        '
        '===================================================================
        Private Function CANotifyCallBack(ByVal hCAHandle As Integer, ByVal iReason As Integer, ByVal iParam As Integer, ByVal pItemID As IntPtr) As Integer
            CANotifyCallBack = CONA_OK
            If iReason = CA_REASON_ENUMERATING Then
                'ShowNotification("CANotifyCallBack: CA_REASON_ENUMERATING")
            ElseIf iReason = CA_REASON_ITEM_ADDED Then
                'ShowNotification("CANotifyCallBack: CA_REASON_ITEM_ADDED")
            ElseIf iReason = CA_REASON_ITEM_DELETED Then
                'ShowNotification("CANotifyCallBack: CA_REASON_ITEM_DELETED")
            ElseIf iReason = CA_REASON_ITEM_UPDATED Then
                'ShowNotification("CANotifyCallBack: CA_REASON_ITEM_UPDATED")
            ElseIf iReason = CA_REASON_ITEM_MOVED Then
                'ShowNotification("CANotifyCallBack: CA_REASON_ITEM_MOVED")
            ElseIf iReason = CA_REASON_ITEM_REPLACED Then
                'ShowNotification("CANotifyCallBack: CA_REASON_ITEM_REPLACED")
            ElseIf iReason = CA_REASON_CONNECTION_LOST Then
                'ShowNotification("CANotifyCallBack: CA_REASON_CONNECTION_LOST")
            ElseIf iReason = CA_REASON_MSG_DELIVERY Then
                'ShowNotification("CANotifyCallBack: CA_REASON_MSG_DELIVERY")
            End If
        End Function

        Protected Overrides Sub InternalDelete()
            MyBase.InternalDelete()
        End Sub
    End Class

End Namespace