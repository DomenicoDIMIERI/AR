'==============================================================================
'* Data Access API 3.2 
'*
'Filename    : DAContentAccess.vb
'Description : 
'Version     : 3.2
'
'Copyright (c) 2006,2007 Nokia Corporation.
'This software, including but not limited to documentation and any related 
'computer programs ("Software"), is protected by intellectual property rights 
'of Nokia Corporation and/or its licensors. All rights are reserved. By using 
'the Software you agree to the terms and conditions hereunder. If you do not 
'agree you must cease using the software immediately.
'Reproducing, disclosing, modifying, translating, or distributing any or all 
'of the Software requires the prior written consent of Nokia Corporation. 
'Nokia Corporation retains the right to make changes to the Software at any 
'time without notice.
'
'A copyright license is hereby granted to use of the Software to make, publish, 
'distribute, sub-license and/or sell new Software utilizing this Software. 
'The Software may not constitute the primary value of any new software utilizing 
'this software. No other license to any other intellectual property rights of 
'Nokia or a third party is granted. 
'
'THIS SOFTWARE IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS 
'OR IMPLIED, INCLUDING WITHOUT LIMITATION, ANY WARRANTY OF NON-INFRINGEMENT, 
'MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. IN NO EVENT SHALL
'NOKIA CORPORATION BE LIABLE FOR ANY DIRECT, INDIRECT, SPECIAL, INCIDENTAL, 
'OR CONSEQUENTIAL LOSS OR DAMAGES, INCLUDING BUT NOT LIMITED TO, LOST PROFITS 
'OR REVENUE, LOSS OF USE, COST OF SUBSTITUTE PROGRAM, OR LOSS OF DATA OR EQUIPMENT 
'ARISING OUT OF THE USE OR INABILITY TO USE THE MATERIAL, EVEN IF 
'NOKIA CORPORATION HAS BEEN ADVISED OF THE LIKELIHOOD OF SUCH DAMAGES OCCURRING. 
'==============================================================================

Option Strict Off
Option Explicit On 

Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Partial Public Class Nokia

    Partial Public Class APIS


        'Public NotInheritable Class DAContentAccess
        '    Private Sub New()

        '    End Sub

        '=========================================================
        ' Current Content Access API version 
        '
        Public Const CAAPI_VERSION_30 As Integer = 30
        Public Const CAAPI_VERSION_31 As Integer = 31
        Public Const CAAPI_VERSION_32 As Integer = 32

        '=========================================================

        '=========================================================
        ' CAAPI_Initialize
        '
        ' Description:
        ' CAAPI_Initialize initializes the API. This must be called once and before any other API call!
        ' It's not allowed to call this function like this 
        ' CAAPI_Initialize(CAGetAPIVersion(), NULL);
        ' You must call it like this
        ' CAAPI_Initialize(CA_API_VERSION_32, NULL);
        '
        ' Parameters:
        '	dwAPIVersion	[in] CAAPI version requested.
        '	pdwParam		[in] Reserved for future use. Must be NULL.
        '
        ' Return values:
        '
        <DllImport("DAAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CAAPI_Initialize(ByVal dwAPIVersion As Integer, ByVal pdwParam As Integer) As Integer

        End Function

        '=========================================================
        ' CAAPI_Terminate
        '
        ' Description:
        '	CAAPI_Terminate terminates the API. This must be called once and as the last API call!
        '
        ' Parameters:
        '	pdwParam		[in] Reserved for future use. Must be NULL.
        '
        ' Return values:
        '
        <DllImport("DAAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CAAPI_Terminate(ByVal pdwParam As Integer) As Integer

        End Function


        '=========================================================
        ' CAGetAPIVersion
        '
        ' Description:
        '	Returns current version of this API. 	
        '
        ' Parameters:
        '	-
        '
        ' Return values:
        ' DWORD- function returns the Content Access API version number. 

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CAGetAPIVersion() As Integer

        End Function

        '=========================================================
        ' DAOpenCA
        '
        ' Description:
        ' Open content access connection for specified content type
        '  
        ' Parameters:
        ' pstrSN		[in] Serial number (IMEI) of the device to be connected	
        ' pdwMedia		[in,out] Media used for connecting 		
        ' dwTarget		[in] Target content to be opened, 
        '				see "Available PIM connection targets" in DADefinitions.h
        ' phCAHandle	[out] Handle to opened CA connection
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_NOT_INITIALIZED
        ' ECONA_DEVICE_NOT_FOUND
        ' ECONA_INVALID_POINTER	
        ' ECONA_INVALID_PARAMETER
        ' ECONA_DEVICE_NOT_FOUND
        ' ECONA_UNKNOWN_ERROR

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function DAOpenCA(ByVal pstrSN As IntPtr, ByRef iMedia As Integer, ByVal iTarget As Integer, ByRef hCAHandle As IntPtr) As Integer

        End Function

        '=========================================================

        '=========================================================
        ' DACloseCA
        '
        ' Description:
        ' Closes content access connection. If operations have been 
        ' started but not closed for connection specified by connection 
        ' handle, those will be closed also. 
        '  
        ' Parameters:
        ' hCAHandle	[in]	Handle to content access connection.
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_NOT_INITIALIZED
        ' ECONA_INVALID_HANDLE
        ' ECONA_UNKNOWN_ERROR

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function DACloseCA(ByVal hCAHandle As IntPtr) As Integer

        End Function
        '=========================================================

        '=========================================================
        ' CAGetFolderInfo
        '
        ' Description:
        ' Get's content specific item paths from the device. For some content
        ' types there only exists root path which will be returned as '\\' in output
        ' parameters
        '
        ' Parameters:
        ' hCAHandle		[in]	Handle to opened CA connection	
        ' pFolderInfo		[out]	Content specific target paths, 
        '							"\\" is the root for target CA connection.
        '							Some connection types only have root ("\\") folder defined.
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_NOT_INITIALIZED
        ' ECONA_INVALID_HANDLE
        ' ECONA_INVALID_POINTER
        ' ECONA_CONNECTION_BUSY	
        ' ECONA_CONNECTION_LOST
        ' ECONA_UNKNOWN_ERROR

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAGetFolderInfo(ByVal hCAHandle As IntPtr, ByVal pFolderInfo As IntPtr) As Integer

        End Function

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAGetFolderInfo(ByVal hCAHandle As IntPtr, ByRef pFolderInfo As CA_FOLDER_INFO) As Integer

        End Function

        '=========================================================
        ' CAFreeFolderInfoStructure 
        '
        ' Description:
        ' CAFreeItemPathStructure frees item path structure, which was 
        ' allocated when CAGetItemPaths was called
        '
        ' Parameters:
        ' pFolderInfo		[in]	Content specific folder structure
        '							
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_NOT_INITIALIZED
        ' ECONA_INVALID_POINTER
        ' ECONA_UNKNOWN_ERROR

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAFreeFolderInfoStructure(ByVal pFolderInfo As IntPtr) As Integer

        End Function

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAFreeFolderInfoStructure(ByRef pFolderInfo As CA_FOLDER_INFO) As Integer

        End Function

        '=========================================================
        ' CAGetIDList
        '
        ' Description:
        ' Reads content specific ID list from the device.
        '  
        ' In S60 devices, if device is missing support for specific 
        ' content ( SMS , MMS or BOOKMARKS), error code 
        ' ECONA_SYNC_INSTALL_PLUGIN_FIRST will be returned .. 
        '  
        ' Parameters:
        ' hCAHandle		[in]	    Handle to opened CA connection
        ' dwFolderId		[in]	Amount of UID's returned
        ' dwOptions		[in]	    Options for ID list reading. Following options 
        '			                are available : 
        '							    CA_OPTION_USE_CACHE option reads item ID list from 
        '							    from cache DB. If cache database is empty,
        '							    ID listing is read from device.
        '							    CA_OPTION_USE_ONLY_CACHE option reads item ID list
        '							    only from cache DB, and returns amount of ID's in 
        '							    cache
        ' pIDList			[out]	Structure containing list of item ID's 
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_INVALID_HANDLE
        ' ECONA_NOT_INITIALIZED
        ' ECONA_INVALID_POINTER
        ' ECONA_UNKNOWN_ERROR_DEVICE
        ' ECONA_INVALID_PARAMETER
        ' ECONA_CONNECTION_BUSY	
        ' ECONA_CONNECTION_LOST
        ' ECONA_UNKNOWN_ERROR

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAGetIDList(ByVal hCAHandle As IntPtr, ByVal iFolderId As Integer, ByVal iOptions As Integer, ByVal pIDList As IntPtr) As Integer

        End Function

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAGetIDList(ByVal hCAHandle As IntPtr, ByVal iFolderId As Integer, ByVal iOptions As Integer, ByRef pIDList As CA_ID_LIST) As Integer

        End Function



        '=========================================================
        ' CAFreeIdListStructure
        '
        ' Description:
        ' Frees memory allocated in CAGetIdList/CACommitOperation call
        '
        ' Parameters:
        ' pIDList		[in]	ID list structure to be freed
        '								
        ' Return values:
        ' CONA_OK
        ' ECONA_NOT_INITIALIZED
        ' ECONA_INVALID_POINTER
        ' ECONA_UNKNOWN_ERROR

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAFreeIdListStructure(ByVal pIDList As IntPtr) As Integer

        End Function

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAFreeIdListStructure(ByRef pIDList As CA_ID_LIST) As Integer

        End Function

        '=========================================================
        ' CABeginOperation
        '
        ' Description:
        ' Begins operations through Content Access API, returns handle
        ' through which Read/Write/Delete operations are accessed. 
        '  
        ' Parameters:
        ' hCAHandle		[in]	Handle to opened CA connection	
        ' dwParam			[in]	For future use, set to zero.
        ' phOperHandle		[out]	Handle to new operation 
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_INVALID_HANDLE
        ' ECONA_NOT_INITIALIZED
        ' ECONA_INVALID_PARAMETER
        ' ECONA_UNKNOWN_ERROR

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CABeginOperation(ByVal hCAHandle As IntPtr, ByVal iParam As Integer, ByRef phOperHandle As Integer) As Integer

        End Function

        '=========================================================
        ' CAReadItem
        '
        ' Description:
        ' Reads content item from the device.  Output parameter's contain
        ' target specific structures for content
        '  
        ' Parameters:
        ' hOperHandle		[in]	Handle to current operation
        ' pID				[in]	Item to be read from the device 
        ' dwOptions		    [in]	Options for read operation, see 
        '							"Options for item reading" in DAContentAccessDefinitions.vb
        ' dwDataFormat		[in]	Data format used in reading
        ' pCaItemData		[out]	Item data returned
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_INVALID_HANDLE
        ' ECONA_NOT_INITIALIZED
        ' ECONA_INVALID_PARAMETER
        ' ECONA_CONNECTION_BUSY	
        ' ECONA_CONNECTION_LOST
        ' ECONA_UNKNOWN_ERROR

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAReadItem(ByVal hOperHandle As Integer, ByVal pID As IntPtr, ByVal iOptions As Integer, ByVal iDataFormat As Integer, ByVal pItemData As IntPtr) As Integer

        End Function

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAReadItem(ByVal hOperHandle As Integer, ByRef pID As CA_ITEM_ID, ByVal iOptions As Integer, ByVal iDataFormat As Integer, ByRef pItemData As CA_DATA_MSG) As Integer

        End Function

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAReadItem(ByVal hOperHandle As Integer, ByRef pID As CA_ITEM_ID, ByVal iOptions As Integer, ByVal iDataFormat As Integer, ByRef pItemData As CA_MMS_DATA) As Integer

        End Function

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAReadItem(ByVal hOperHandle As Integer, ByRef pID As CA_ITEM_ID, ByVal iOptions As Integer, ByVal iDataFormat As Integer, ByRef pItemData As CA_DATA_CONTACT) As Integer

        End Function

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAReadItem(ByVal hOperHandle As Integer, ByRef pID As CA_ITEM_ID, ByVal iOptions As Integer, ByVal iDataFormat As Integer, ByRef pItemData As CA_DATA_CALENDAR) As Integer

        End Function


        '=========================================================
        ' CAWriteItem
        '
        ' Description:
        ' Writes new item to the device. Actual writing will be committed
        ' when CAEndOperation is called. CA_ITEM_ID parameter contains 
        ' temporary ID for the item to be created which can be used after 
        ' end operation to check result of specific item write.
        '  
        ' Parameters:
        ' hOperHandle		[in] Handle to current operation
        ' pID				[in,out] Contains path info and returns 
        '					temporary ID for writing.
        ' dwOptions		[in]	For future use, must be set to zero
        ' pCaItemData		[in] Item data to be saved to device
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_INVALID_HANDLE
        ' ECONA_NOT_INITIALIZED
        ' ECONA_INVALID_PARAMETER
        ' ECONA_UNKNOWN_ERROR

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAWriteItem(ByVal hOperHandle As Integer, ByVal pID As IntPtr, ByVal iOptions As Integer, ByVal iDataFormat As Integer, ByVal pItemData As IntPtr) As Integer

        End Function

        '=========================================================
        ' CADeleteItem
        '
        ' Description:
        ' Marks item for deletion , actual deletion will be committed
        ' when CAEndOperation is called. Returns temporary ID for deletion
        ' operation.
        '  
        ' Parameters:
        ' hOperHandle		[in]		Handle to current operation
        ' pID				[in,out]	Item to be deleted
        ' dwOptions		[in]		For future use, must be set to zero
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_INVALID_HANDLE
        ' ECONA_NOT_INITIALIZED
        ' ECONA_INVALID_PARAMETER
        ' ECONA_UNKNOWN_ERROR

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CADeleteItem(ByVal hOperHandle As Integer, ByVal pID As IntPtr, ByVal iOptions As Integer) As Integer

        End Function

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CADeleteItem(ByVal hOperHandle As Integer, ByRef pID As CA_ITEM_ID, ByVal iOptions As Integer) As Integer

        End Function


        '=========================================================
        ' CAWriteField
        '
        ' Description:
        ' CAWriteField method is used for updating content item in device.  If item does not 
        ' exist in the device, it will be created. Actual writing to the device is done in 
        ' CACommitOperations method. 
        ' 
        ' Currently only supported with Series 40 device’s phonebook content. Other device types or 
        ' content types will return ECONA_NOT_SUPPORTED_PC error to the caller.
        '  
        ' Parameters:
        ' hOperHandle		[in]		Handle to current operation
        ' pID				[in]		Contains path info and returns temporary ID for writing.	
        ' dwOptions		[in]		For future use, must be set to zero
        ' pDataItem		[in]		Data item containing the data of the field. When updating, dwFieldID member must
        '								be a valid field ID received from CAReadItem call. When adding a new field,dwFieldID
        '								member must be set to zero.
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_INVALID_HANDLE
        ' ECONA_INVALID_PARAMETER
        ' ECONA_NOT_SUPPORTED_PC
        ' ECONA_INVALID_POINTER
        ' ECONA_UNKNOWN_ERROR
        '

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAWriteField(ByVal hOperHandle As Integer, ByVal pID As IntPtr, ByVal dwOptions As Integer, ByVal pDataItem As IntPtr) As Integer

        End Function


        '=========================================================
        ' CADeleteField
        '
        ' Description:
        ' CADeleteField method is used for deleting single field from content item in device.  
        ' Actual deletion will be done in CACommitOperations method. 
        ' 
        ' Currently only supported with Series 40 device’s phonebook content. Other device types 
        ' or content types will return ECONA_NOT_SUPPORTED_PC error to the caller.
        '  
        ' Parameters:
        ' hOperHandle		[in]		Handle to current operation
        ' pID				[in]		ID for the item from where field will be deleted
        ' dwOptions		[in]		For future use, must be set to zero
        ' pDataItem		[in]		Data item containing the data of the field. When deleting, dwFieldID member must
        '								be a valid field ID received from CAReadItem call.
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_INVALID_HANDLE
        ' ECONA_INVALID_PARAMETER
        ' ECONA_NOT_SUPPORTED_PC
        ' ECONA_INVALID_POINTER
        ' ECONA_UNKNOWN_ERROR
        '

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CADeleteField(ByVal hOperHandle As Integer, ByVal pID As IntPtr, ByVal dwOptions As Integer, ByVal pDataItem As IntPtr) As Integer

        End Function

        '=========================================================
        ' CACreateFolder
        '
        ' Description:
        ' Creates folder to the device.  Returns temporary folder ID for the 
        ' folder to be created, and this ID can be used for saving items to folder. 
        ' Actual folder creation to the device will be done when CACommitOperations is called. 
        ' 
        ' Supported content types are :
        ' CA_TARGET_SMS_MESSAGES, CA_TARGET_MMS_MESSAGES,CA_TARGET_BOOKMARKS
        '  
        ' Parameters:
        ' hOperHandle		[in]		Handle to current operation
        ' pID				[in]		Pointer to CA_ITEM_ID structure, temporary ID 
        '								for folder will be returned in this struct.
        '								temporary ID for the folder can be used when writing items to
        '								specified folder
        ' pstrFolderName 	[in]		Folder to be created. Parameter value can contain 
        '								multiple subfolder levels (e.g folder1\folder2). 
        ' 
        '								In sms and mms message cases, folders can only be created 
        '								under folder CA_MESSAGE_FOLDER_USER_FOLDERS. 
        '								Format of this parameter in messaging case would be for example: 
        '								"predefuserfolders\folder1". If "predefuserfolders" part is not 
        '								included to pstrFolderPath parameter, path is considered as invalid
        '
        '								ECONA_NOT_SUPPORTED_DEVICE error will be returned if target 
        '								content does not support folder creation
        '								
        ' Return values:
        ' CONA_OK
        ' ECONA_INVALID_HANDLE
        ' ECONA_INVALID_PARAMETER
        ' ECONA_NOT_SUPPORTED_DEVICE
        ' ECONA_NOT_SUPPORTED_PC
        ' ECONA_INVALID_POINTER
        ' ECONA_UNKNOWN_ERROR
        '

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CACreateFolder(ByVal hOperHandle As Integer, ByVal pID As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrFolderName As String) As Integer

        End Function

        '=========================================================
        ' CADeleteFolder
        '
        ' Description:
        ' Deletes folder from the device. 
        ' Actual deletion will be done in CACommitOperations method. 
        ' 
        ' Supported content types are :
        ' CA_TARGET_SMS_MESSAGES, CA_TARGET_MMS_MESSAGES,CA_TARGET_BOOKMARKS
        '  
        ' Parameters:
        ' hOperHandle		[in]		Handle to current operation
        ' pID				[in]		CA_ITEM_ID structure containing folder ID 
        '								information about the folder to be deleted
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_INVALID_HANDLE
        ' ECONA_INVALID_PARAMETER
        ' ECONA_NOT_SUPPORTED_PC
        ' ECONA_INVALID_POINTER
        ' ECONA_UNKNOWN_ERROR
        '

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CADeleteFolder(ByVal hOperHandle As Integer, ByVal pID As IntPtr) As Integer

        End Function

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CADeleteFolder(ByVal hOperHandle As Integer, ByRef pID As CA_ITEM_ID) As Integer

        End Function

        '=========================================================
        ' CACommitOperations
        '
        ' Description:
        ' Commits write and delete operations to the phone and returns 
        ' status information for those.
        '  
        ' Parameters:
        ' hOperHandle	[in]	Handle to current operation
        ' pIDList		[out]	Item status list for committed operations 
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_INVALID_HANDLE
        ' ECONA_NOT_INITIALIZED
        ' ECONA_INVALID_PARAMETER
        ' ECONA_CONNECTION_BUSY	
        ' ECONA_CONNECTION_LOST
        ' ECONA_UNKNOWN_ERROR

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CACommitOperations(ByVal hOperHandle As Integer, ByVal pIDList As IntPtr) As Integer

        End Function

        '=========================================================
        ' CAEndOperation
        '
        ' Description:
        ' Closes opened operation without committing changes.
        '  
        ' Parameters:
        ' hOperHandle	[in]	Handle to current operation
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_INVALID_HANDLE
        ' ECONA_NOT_INITIALIZED
        ' ECONA_UNKNOWN_ERROR

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAEndOperation(ByVal hOperHandle As Integer) As Integer

        End Function

        '=========================================================
        ' CAFreeItemData
        '
        ' Description:
        ' Frees the item data reserved by Content Access API
        '  
        ' Parameters:
        ' hCAHandle	[in]	Handle to the existing API connection
        ' dwDataFormat	[in]	Data format used when memory was reserved ..
        ' caItemData	[in]	Pointer to the allocated structure ..
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_INVALID_HANDLE
        ' ECONA_NOT_INITIALIZED
        ' ECONA_INVALID_POINTER
        ' ECONA_UNKNOWN_ERROR
        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAFreeItemData(ByVal hCAHandle As IntPtr, ByVal iDataFormat As Integer, ByVal pItemData As IntPtr) As Integer

        End Function

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAFreeItemData(ByVal hCAHandle As IntPtr, ByVal iDataFormat As Integer, ByRef pItemData As CA_DATA_MSG) As Integer

        End Function

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAFreeItemData(ByVal hCAHandle As IntPtr, ByVal iDataFormat As Integer, ByRef pItemData As CA_MMS_DATA) As Integer

        End Function

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAFreeItemData(ByVal hCAHandle As IntPtr, ByVal iDataFormat As Integer, ByRef pItemData As CA_DATA_CONTACT) As Integer

        End Function

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CAFreeItemData(ByVal hCAHandle As IntPtr, ByVal iDataFormat As Integer, ByRef pItemData As CA_DATA_CALENDAR) As Integer

        End Function
        '=========================================================
        ' CARegisterNotifyCallback
        '
        ' Description:
        '	Registers and unregisters callback function for notifications
        '  
        ' Parameters:
        '  hCAHandle	[in]	Handle to the existing API connection
        '	dwState		[in]	Used to define the action:
        '						 API_REGISTER used in registeration
        '						 API_UNREGISTER.used in removing the registeration
        '	pfnNotify	[in]	Function pointer of the call back method
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_INVALID_HANDLE
        ' ECONA_NOT_INITIALIZED
        ' ECONA_UNKNOWN_ERROR

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CARegisterNotifyCallback(ByVal hCAHandle As IntPtr, ByVal iState As Integer, ByVal pfnNotify As CANotifyCallbackDelegate) As Integer

        End Function
        '=========================================================

        '=========================================================
        ' CARegisterOperationCallback
        '
        ' Description:
        '	Registers and unregisters callback function for operation 
        '  notifications
        '  
        ' Parameters:
        '  hOperHandle	[in]	Handle to the existing API connection
        '	dwState		[in]	Used to define the action:
        '						 API_REGISTER used in registeration
        '						 API_UNREGISTER.used in removing the registeration
        '	pfnNotify	[in]	Function pointer of the call back method
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_INVALID_HANDLE
        ' ECONA_NOT_INITIALIZED
        ' ECONA_UNKNOWN_ERROR

        <DllImport("DAAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CARegisterOperationCallback(ByVal hOperHandle As Integer, ByVal iState As Integer, ByVal pfnNotify As CAOperationCallbackDelegate) As Integer

        End Function

        '=========================================================

    End Class

End Class
