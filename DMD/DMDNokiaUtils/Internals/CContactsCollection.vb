Imports System.Runtime.InteropServices

Imports DMD.Nokia
Imports DMD.Nokia.APIS
Imports DMD.Internals

Namespace Internals

    ''' <summary>
    ''' Rappresenta la collezione dei contatti memorizzati sul dispositivo Nokia
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CContactsCollection
        Inherits System.Collections.ReadOnlyCollectionBase

        Private m_Folder As CContactsFolder
        Private m_pContactBuffer As IntPtr = IntPtr.Zero


        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Friend Sub New(ByVal folder As CContactsFolder)
            Me.New()
            Me.Load(folder)
        End Sub

        Public ReadOnly Property Device As NokiaDevice
            Get
                If (Me.m_Folder Is Nothing) Then Return Nothing
                Return Me.m_Folder.Device
            End Get
        End Property

        Friend Sub Load(ByVal folder As CContactsFolder)
            If (folder Is Nothing) Then Throw New ArgumentNullException("folder")

            Me.m_Folder = folder

            ' Check PIM connection to contacts folder and open it if needed
            ' Read all the contact item IDs from the connected device 
            Dim caIDList As CA_ID_LIST
            caIDList.iSize = Marshal.SizeOf(caIDList)
            Dim buf As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(caIDList))
            Marshal.StructureToPtr(caIDList, buf, True)
            Dim iRet As Integer = CAGetIDList(folder.GetConnectionHandle, 0, CA_OPTION_USE_CACHE, buf) 'CAGetIDList(m_hContacts, folderInfo.iFolderId, 0, buf)
            If iRet <> CONA_OK Then ShowErrorMessage("CAGetIDList", iRet)
            caIDList = CType(Marshal.PtrToStructure(buf, GetType(CA_ID_LIST)), CA_ID_LIST)
            Dim hOperHandle As Integer = 0
            iRet = CABeginOperation(folder.GetConnectionHandle, 0, hOperHandle)
            If iRet <> CONA_OK Then ShowErrorMessage("CABeginOperation", iRet)
            Dim bErrorShown As Boolean = False
            Dim k As Integer
            For k = 0 To caIDList.iUIDCount - 1
                ' Read contact item from the connected device
                Dim UID As CA_ITEM_ID = GetUidFromBuffer(k, caIDList.pUIDs)
                Dim contact As New CContactItem(Me.m_Folder)
                contact.ReadContact(hOperHandle, UID)
                MyBase.InnerList.Add(contact)
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

        '===================================================================
        ' FreeContactData
        '
        ' Frees allocated memory of Contact
        '
        '===================================================================
        Private Sub FreeContactData()
            ' Free memory allocated by DA API
            Dim iRet As Integer = CAFreeItemData(Me.m_Folder.GetConnectionHandle, CA_DATA_FORMAT_STRUCT, Me.m_pContactBuffer)
            If iRet <> CONA_OK Then ShowErrorMessage("DAFreeItemData", iRet)
            Marshal.FreeHGlobal(m_pContactBuffer)
            Me.m_pContactBuffer = IntPtr.Zero
        End Sub

        '===================================================================
        ' GetContactPIMBuffer
        '
        ' Fills contact name etc. in CA_DATA_ITEM struct and copies it to
        ' unmanaged memory buffer
        '
        '===================================================================
        Private Function GetContactPIMBuffer(ByVal iCount As Integer, ByVal strFirstName As String, ByVal strLastName As String, ByVal strJob As String, ByVal strCompany As String, ByVal dtBirthDay As DateTime) As IntPtr
            Dim dataItem() As CA_DATA_ITEM
            ReDim dataItem(iCount)
            Dim iIndex As Integer = 0
            If strFirstName.Length > 0 Then
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CONTACT_PI
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_FN
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strFirstName)
                iIndex += 1
            End If
            If strLastName.Length > 0 Then
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CONTACT_PI
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_LN
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strLastName)
                iIndex += 1
            End If
            If strJob.Length > 0 Then
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CONTACT_PI
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_JOB_TITLE
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strJob)
                iIndex += 1
            End If
            If strCompany.Length > 0 Then
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CONTACT_PI
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_JOB_TITLE
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strCompany)
                iIndex += 1
            End If
            If iIndex < iCount Then
                Dim dataDate As CA_DATA_DATE
                Dim datePtr As IntPtr = IntPtr.Zero
                GetEmptyPIMDate(dataDate)
                dataDate.wYear = CUShort(dtBirthDay.Year)
                dataDate.bMonth = CByte(dtBirthDay.Month)
                dataDate.bDay = CByte(dtBirthDay.Day)
                dataDate.iSize = Marshal.SizeOf(dataDate)
                datePtr = Marshal.AllocHGlobal(dataDate.iSize)
                Marshal.StructureToPtr(dataDate, datePtr, True)
                ' 
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CONTACT_PI
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_BIRTHDAY
                dataItem(iIndex).pCustomData = datePtr
                iIndex += 1
            End If
            ' Allocate memory for buffer
            Dim iSize As Integer = iCount * Marshal.SizeOf(GetType(CA_DATA_ITEM))
            Dim bufDataItem As IntPtr = Marshal.AllocHGlobal(iSize)
            For iIndex = 0 To iCount - 1
                ' Calculate beginning of CA_DATA_ITEM structure of item 'iIndex'
                Dim iPtr As Int64 = bufDataItem.ToInt64 + iIndex * Marshal.SizeOf(GetType(CA_DATA_ITEM))
                ' Convert integer to pointer
                Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
                Marshal.StructureToPtr(dataItem(iIndex), ptr, True)
            Next
            Return bufDataItem
        End Function

        '===================================================================
        ' GetContactNumberBuffer
        '
        ' Fills phone numbers in CA_DATA_ITEM struct and copies it to
        ' unmanaged memory buffer
        '
        '===================================================================
        Private Function GetContactNumberBuffer(ByVal iCount As Integer, ByVal strGeneral As String, ByVal strMobile As String, ByVal strHome As String, ByVal strWork As String, ByVal strFax As String) As IntPtr
            Dim dataItem() As CA_DATA_ITEM
            ReDim dataItem(iCount)
            Dim iIndex As Integer = 0
            If strGeneral.Length > 0 Then
                ' Mobile number
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CONTACT_NUMBER
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_TEL
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strGeneral)
                iIndex += 1
            End If
            If strMobile.Length > 0 Then
                ' Mobile number
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CONTACT_NUMBER
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_MOBILE
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strMobile)
                iIndex += 1
            End If
            If strHome.Length > 0 Then
                ' Home number
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CONTACT_NUMBER
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_TEL_HOME
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strHome)
                iIndex += 1
            End If
            If (strWork.Length > 0) Then
                ' Work number
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CONTACT_NUMBER
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_TEL_WORK
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strWork)
                iIndex += 1
            End If
            If (strFax.Length > 0) Then
                ' Work number
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CONTACT_NUMBER
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_FAX
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strFax)
                iIndex += 1
            End If
            ' Allocate memory for buffer
            Dim iSize As Integer = iCount * Marshal.SizeOf(GetType(CA_DATA_ITEM))
            Dim bufDataItem As IntPtr = Marshal.AllocHGlobal(iSize)
            For iIndex = 0 To iCount - 1
                ' Calculate beginning of CA_DATA_ITEM structure of item 'iIndex'
                Dim iPtr As Int64 = bufDataItem.ToInt64 + iIndex * Marshal.SizeOf(GetType(CA_DATA_ITEM))
                ' Convert integer to pointer
                Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
                Marshal.StructureToPtr(dataItem(iIndex), ptr, True)
            Next
            Return bufDataItem
        End Function

        '===================================================================
        ' GetContactAddressBuffer
        '
        ' Fills addresses in CA_DATA_ITEM struct and copies it to
        ' unmanaged memory buffer
        '
        '===================================================================
        Private Function GetContactAddressBuffer(ByVal iCount As Integer, ByVal strEmail As String, ByVal strWeb As String, _
                ByVal bHasAddress As Boolean, ByVal strPOBox As String, ByVal strPCode As String, ByVal strStreet As String, _
                ByVal strCity As String, ByVal strState As String, ByVal strCountry As String, ByVal strEData As String) As IntPtr
            Dim dataItem() As CA_DATA_ITEM
            ReDim dataItem(iCount)
            Dim iIndex As Integer = 0
            If strEmail.Length > 0 Then
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CONTACT_ADDRESS
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_EMAIL
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strEmail)
                iIndex += 1
            End If
            If strWeb.Length > 0 Then
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CONTACT_ADDRESS
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_WEB
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strWeb)
                iIndex += 1
            End If
            If bHasAddress Then
                Dim dataAddress As CA_DATA_POSTAL_ADDRESS = New CA_DATA_POSTAL_ADDRESS
                dataAddress.iSize = Marshal.SizeOf(dataAddress)
                dataAddress.pstrPOBox = strPOBox
                dataAddress.pstrPostalCode = strPCode
                dataAddress.pstrStreet = strStreet
                dataAddress.pstrCity = strCity
                dataAddress.pstrState = strState
                dataAddress.pstrCountry = strCountry
                dataAddress.pstrExtendedData = strEData

                Dim aDataPtr As IntPtr = Marshal.AllocHGlobal(dataAddress.iSize)
                Marshal.StructureToPtr(dataAddress, aDataPtr, True)
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CONTACT_ADDRESS
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_POSTAL
                dataItem(iIndex).pCustomData = aDataPtr
                iIndex += 1
            End If
            ' Allocate memory for buffer
            Dim iSize As Integer = iCount * Marshal.SizeOf(GetType(CA_DATA_ITEM))
            Dim bufDataItem As IntPtr = Marshal.AllocHGlobal(iSize)
            For iIndex = 0 To iCount - 1
                ' Calculate beginning of CA_DATA_ITEM structure of item 'iIndex'
                Dim iPtr As Int64 = bufDataItem.ToInt64 + iIndex * Marshal.SizeOf(GetType(CA_DATA_ITEM))
                ' Convert integer to pointer
                Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
                Marshal.StructureToPtr(dataItem(iIndex), ptr, True)
            Next
            Return bufDataItem
        End Function

        '===================================================================
        ' GetContactGeneralBuffer
        '
        ' Fills general info in CA_DATA_ITEM struct and copies it to
        ' unmanaged memory buffer
        '
        '===================================================================
        Private Function GetContactGeneralBuffer(ByVal iCount As Integer, ByVal strNote As String) As IntPtr
            Dim dataItem() As CA_DATA_ITEM
            ReDim dataItem(iCount)
            Dim iIndex As Integer = 0
            If strNote.Length > 0 Then
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CONTACT_GENERAL
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_NOTE
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strNote)
                iIndex += 1
            End If
            ' Allocate memory for buffer
            Dim iSize As Integer = iCount * Marshal.SizeOf(GetType(CA_DATA_ITEM))
            Dim bufDataItem As IntPtr = Marshal.AllocHGlobal(iSize)
            For iIndex = 0 To iCount - 1
                ' Calculate beginning of CA_DATA_ITEM structure of item 'iIndex'
                Dim iPtr As Int64 = bufDataItem.ToInt64 + iIndex * Marshal.SizeOf(GetType(CA_DATA_ITEM))
                ' Convert integer to pointer
                Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
                Marshal.StructureToPtr(dataItem(iIndex), ptr, True)
            Next
            Return bufDataItem
        End Function

        '===================================================================
        ' FreeContactWriteBuffers
        '
        ' Frees global memory allocated for Conatc write operation
        '
        '===================================================================
        Private Sub FreeContactWriteBuffers(ByRef dataContact As CA_DATA_CONTACT)

            Dim iCount As Integer = dataContact.bPICount
            Dim iPtr As IntPtr = dataContact.pPIFields
            Dim i As Integer = 0
            Dim dataItem As CA_DATA_ITEM = New CA_DATA_ITEM()
            If iPtr <> IntPtr.Zero Then
                For i = 0 To iCount - 1
                    dataItem = CType(Marshal.PtrToStructure(iPtr, GetType(CA_DATA_ITEM)), CA_DATA_ITEM)
                    If dataItem.iFieldSubType = CA_FIELD_SUB_TYPE_BIRTHDAY Then
                        Marshal.FreeHGlobal(dataItem.pCustomData)
                    Else
                        Marshal.FreeCoTaskMem(dataItem.pCustomData)
                    End If
                    iPtr = IntPtr.op_Explicit(iPtr.ToInt64() + Marshal.SizeOf(dataItem))
                Next
                Marshal.FreeHGlobal(dataContact.pPIFields)
            End If

            iCount = dataContact.bNumberCount
            iPtr = dataContact.pNumberFields
            If iPtr <> IntPtr.Zero Then
                For i = 0 To iCount - 1
                    dataItem = CType(Marshal.PtrToStructure(iPtr, GetType(CA_DATA_ITEM)), CA_DATA_ITEM)
                    Marshal.FreeCoTaskMem(dataItem.pCustomData)
                    iPtr = IntPtr.op_Explicit(iPtr.ToInt64() + Marshal.SizeOf(dataItem))
                Next
                Marshal.FreeHGlobal(dataContact.pNumberFields)
            End If

            iCount = dataContact.bAddressCount
            iPtr = dataContact.pAddressFields
            If iPtr <> IntPtr.Zero Then
                For i = 0 To iCount - 1
                    dataItem = CType(Marshal.PtrToStructure(iPtr, GetType(CA_DATA_ITEM)), CA_DATA_ITEM)
                    If dataItem.iFieldSubType = CA_FIELD_SUB_TYPE_POSTAL Then
                        Marshal.FreeHGlobal(dataItem.pCustomData)
                    Else
                        Marshal.FreeCoTaskMem(dataItem.pCustomData)
                        iPtr = IntPtr.op_Explicit(iPtr.ToInt64() + Marshal.SizeOf(dataItem))
                    End If
                Next
                Marshal.FreeHGlobal(dataContact.pAddressFields)
            End If

            iCount = dataContact.bGeneralCount
            iPtr = dataContact.pGeneralFields
            If iPtr <> IntPtr.Zero Then
                For i = 0 To iCount - 1
                    dataItem = CType(Marshal.PtrToStructure(iPtr, GetType(CA_DATA_ITEM)), CA_DATA_ITEM)
                    Marshal.FreeCoTaskMem(dataItem.pCustomData)
                    iPtr = IntPtr.op_Explicit(iPtr.ToInt64() + Marshal.SizeOf(dataItem))
                Next
                Marshal.FreeHGlobal(dataContact.pGeneralFields)
            End If
        End Sub

        '===================================================================
        ' GetEmptyPIMDate
        ' 
        ' Gets empty CA_DATA_DATE
        ' 
        '===================================================================
        Private Sub GetEmptyPIMDate(ByRef pimDate As CA_DATA_DATE)
            pimDate.iSize = Marshal.SizeOf(pimDate)
            pimDate.bDay = 0
            pimDate.lBias = 0
            pimDate.bHour = 0
            pimDate.bMinute = 0
            pimDate.bMonth = 0
            pimDate.bSecond = 0
            pimDate.lTimeZoneBias = 0
            pimDate.wYear = 0
        End Sub

        Protected Friend Overridable Sub Remove(item As CContactItem)
            Me.InnerList.Remove(item)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace