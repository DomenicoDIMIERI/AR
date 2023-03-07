Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    ''' <summary>
    ''' Rappresenta lo stazione per gli SMS
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CSMS
        Inherits CSMSFolder

        Private m_hSMS As IntPtr = IntPtr.Zero
        Private pCANotifyCallBack As CANotifyCallbackDelegate

        Private m_InBox As CSMSFolder
        Private m_OutBox As CSMSFolder
        Private m_Sent As CSMSFolder
        Private m_Archive As CSMSFolder
        Private m_Drafts As CSMSFolder
        Private m_Templates As CSMSFolder

        Public Sub New()
            Me.pCANotifyCallBack = AddressOf CANotifyCallBack
            Me.m_InBox = Nothing
            Me.m_OutBox = Nothing
            Me.m_Sent = Nothing
            Me.m_Archive = Nothing
            Me.m_Drafts = Nothing
            Me.m_Templates = Nothing
        End Sub

        Public Sub New(ByVal device As Nokia.NokiaDevice)
            Me.New()
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            Me.SetDevice(device)
        End Sub

        Private Function GetFolderById(ByVal id As SMSFolderTypes) As CSMSFolder
            If (id = SMSFolderTypes.Root) Then Return Me
            For Each f As CSMSFolder In Me.SubFolders
                If (f.FolderType = id) Then Return f
            Next
            Return Nothing
        End Function

        Public ReadOnly Property InBox As CSMSFolder
            Get
                If (Me.m_InBox Is Nothing) Then Me.m_InBox = Me.GetFolderById(SMSFolderTypes.InBox)
                Return Me.m_InBox
            End Get
        End Property

        Public ReadOnly Property OutBox As CSMSFolder
            Get
                If (Me.m_OutBox Is Nothing) Then Me.m_OutBox = Me.GetFolderById(SMSFolderTypes.OutBox)
                Return Me.m_OutBox
            End Get
        End Property

        Public ReadOnly Property Sent As CSMSFolder
            Get
                If (Me.m_Sent Is Nothing) Then Me.m_Sent = Me.GetFolderById(SMSFolderTypes.Sent)
                Return Me.m_Sent
            End Get
        End Property

        Public ReadOnly Property Archive As CSMSFolder
            Get
                If (Me.m_Archive Is Nothing) Then Me.m_Archive = Me.GetFolderById(SMSFolderTypes.Archive)
                Return Me.m_Archive
            End Get
        End Property

        Public ReadOnly Property Drafts As CSMSFolder
            Get
                If (Me.m_Drafts Is Nothing) Then Me.m_Drafts = Me.GetFolderById(SMSFolderTypes.Drafts)
                Return Me.m_Drafts
            End Get
        End Property

        Public ReadOnly Property Templates As CSMSFolder
            Get
                If (Me.m_Templates Is Nothing) Then Me.m_Templates = Me.GetFolderById(SMSFolderTypes.Templates)
                Return Me.m_Templates
            End Get
        End Property

      

        Protected Overrides Sub InitializeFolderData()
            ' Check PIM connection to SMS folders and open it if needed
            
            ' Get SMS folder info
            Me.folderInfo = New CA_FOLDER_INFO
            folderInfo.iSize = Marshal.SizeOf(folderInfo)
            'Dim bufItem As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_FOLDER_INFO)))
            'Marshal.StructureToPtr(folderInfo, bufItem, True)
            'Dim iRet As Integer = CAGetFolderInfo(m_hSMS, bufItem)
            Dim iRet As Integer = CAGetFolderInfo(Me.GetConnectionHandle, Me.folderInfo)
            If iRet <> CONA_OK Then ShowErrorMessage("CAGetFolderInfo", iRet)
            iRet = Me.SubFolders.Count

            iRet = CAFreeFolderInfoStructure(Me.folderInfo)
            If iRet <> CONA_OK Then ShowErrorMessage("CAFreeFolderInfoStructure", iRet)
            'Marshal.FreeHGlobal(bufItem)
        End Sub

        '===================================================================
        ' CloseSMSConnection
        '
        ' Close PIM connection to SMS folders
        '
        '===================================================================
        Private Function CloseSMSConnection() As Integer
            Dim iRet As Integer = CONA_OK
            If Not Me.m_hSMS.Equals(IntPtr.Zero) Then
                ' Unregister CallBack
                Dim iResult As Integer = CARegisterNotifyCallback(Me.m_hSMS, API_UNREGISTER, pCANotifyCallBack)
                If iResult <> CONA_OK Then ShowErrorMessage("CONARegisterNotifyCallback", iResult)

                ' Close PIM connection
                iRet = DACloseCA(Me.m_hSMS)
                If iRet <> CONA_OK Then ShowErrorMessage("DACloseCA", iRet)
                Me.m_hSMS = IntPtr.Zero
            End If
            Return iRet
        End Function

        Friend Overrides Function GetConnectionHandle() As IntPtr
            If Me.m_hSMS.Equals(IntPtr.Zero) Then
                ' No PIM connection, open it
                Dim pstrSerialNumber As IntPtr = Marshal.StringToCoTaskMemUni(Me.Device.SerialNumber)
                Dim iMedia As Integer = API_MEDIA_ALL
                Dim iTarget As Integer = CA_TARGET_SMS_MESSAGES
                Dim iRet As Integer = DAOpenCA(pstrSerialNumber, iMedia, iTarget, Me.m_hSMS)
                If iRet <> CONA_OK And iRet <> ECONA_NOT_SUPPORTED_DEVICE Then ShowErrorMessage("DAOpenCA", iRet)
                Marshal.FreeCoTaskMem(pstrSerialNumber)
                System.Threading.Thread.Sleep(100)
                ' Register CA notification callback function
                If Not Me.m_hSMS.Equals(IntPtr.Zero) Then
                    Dim iResult As Integer = CARegisterNotifyCallback(Me.m_hSMS, API_REGISTER, pCANotifyCallBack)
                    If iResult <> CONA_OK Then ShowErrorMessage("CARegisterNotifyCallback", iResult)
                End If
            End If

            Return Me.m_hSMS
        End Function

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


        '===================================================================
        ' GetSMSAddressBuffer
        '
        ' Fills SMS address in CA_DATA_ADDRESS struct and copies it to
        ' unmanaged memory buffer
        '
        '===================================================================
        Private Function GetSMSAddressBuffer(ByVal strNumber As String) As IntPtr
            Dim dataAddress As CA_DATA_ADDRESS
            dataAddress = New CA_DATA_ADDRESS
            dataAddress.iSize = Marshal.SizeOf(dataAddress)
            dataAddress.iAddressInfo = CA_MSG_ADDRESS_TYPE_NUMBER
            dataAddress.pstrAddress = strNumber
            ' Allocate memory for buffer
            Dim bufDataAddress As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(dataAddress))
            Marshal.StructureToPtr(dataAddress, bufDataAddress, True)
            Return bufDataAddress
        End Function

        Protected Overrides Function InitializeSubFolders() As CFoldersCollection
            Return MyBase.InitializeSubFolders()
        End Function
         
    End Class

End Namespace