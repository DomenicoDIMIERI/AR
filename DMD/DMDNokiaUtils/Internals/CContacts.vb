Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    ''' <summary>
    ''' Cartella contatti
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CContacts
        Inherits CContactsFolder

        Private m_hContacts As IntPtr
        Private pCANotifyCallBack As CANotifyCallbackDelegate
        
        Public Sub New()
            Me.m_hContacts = IntPtr.Zero
            Me.pCANotifyCallBack = AddressOf CANotifyCallBack
        End Sub

        Public Sub New(ByVal device As Nokia.NokiaDevice)
            Me.New()
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            Me.SetDevice(device)
        End Sub


        Protected Friend Overrides Sub SetDevice(value As NokiaDevice)
            MyBase.SetDevice(value)
            Me.InitializeFolderData()
        End Sub





        Protected Overrides Sub InitializeFolderData()
            ' Get contacts folder info
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
        ' CloseContactsConnection
        '
        ' Close PIM connection to contacts folder
        '
        '===================================================================
        Private Function CloseContactsConnection() As Integer
            Dim iRet As Integer = CONA_OK
            If Not Me.m_hContacts.Equals(IntPtr.Zero) Then
                ' Unregister CallBack
                Dim iResult As Integer = CARegisterNotifyCallback(m_hContacts, API_UNREGISTER, pCANotifyCallBack)

                ' Close PIM connection
                iRet = DACloseCA(m_hContacts)
                If iRet <> CONA_OK Then ShowErrorMessage("DACloseCA", iRet)
                Me.m_hContacts = IntPtr.Zero
            End If
            Return iRet
        End Function



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

        Protected Overrides Function InstantiateSubFolder() As CBaseFolder
            Throw New NotSupportedException
        End Function

        Protected Overrides Sub InternalDelete()
            Throw New NotSupportedException
        End Sub

        Friend Overrides Function GetConnectionHandle() As IntPtr
            If Me.m_hContacts.Equals(IntPtr.Zero) Then
                ' No PIM connection, open it
                Dim pstrSerialNumber As IntPtr = Marshal.StringToCoTaskMemUni(Me.m_Device.SerialNumber) ' CONAAllocString(strSerialNumber)
                Dim iMedia As Integer = API_MEDIA_ALL
                Dim iTarget As Integer = CA_TARGET_CONTACTS
                Dim iRet As Integer = DAOpenCA(pstrSerialNumber, iMedia, iTarget, Me.m_hContacts)
                If iRet <> CONA_OK And iRet <> ECONA_NOT_SUPPORTED_DEVICE Then ShowErrorMessage("DAOpenCA", iRet)
                Marshal.FreeCoTaskMem(pstrSerialNumber)
                ' Register CA notification callback function
                If Not Me.m_hContacts.Equals(IntPtr.Zero) Then
                    Dim iResult As Integer = CARegisterNotifyCallback(Me.m_hContacts, API_REGISTER, pCANotifyCallBack)
                    If iResult <> CONA_OK Then ShowErrorMessage("CARegisterNotifyCallback", iResult)
                End If
            End If

            Return Me.m_hContacts
        End Function
    End Class

End Namespace