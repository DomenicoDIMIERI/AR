'Filename    : CONAFileSystem.vb
'Part of     : Connectivity API VB.NET examples
'Description : Connectivity API data definitions, converted from CONAFileSystem.h
'Version     : 3.2
'
'This example is only to be used with PC Connectivity API version 3.2.
'Compability ("as is") with future versions is not quaranteed.
'
'Copyright (c) 2007 Nokia Corporation.
'
'This material, including but not limited to documentation and any related
'computer programs, is protected by intellectual property rights of Nokia
'Corporation and/or its licensors.
'All rights are reserved. Reproducing, modifying, translating, or
'distributing any or all of this material requires the prior written consent
'of Nokia Corporation. Nokia Corporation retains the right to make changes
'to this material at any time without notice. A copyright license is hereby
'granted to download and print a copy of this material for personal use only.
'No other license to any other intellectual property rights is granted. The
'material is provided "as is" without warranty of any kind, either express or
'implied, including without limitation, any warranty of non-infringement,
'merchantability and fitness for a particular purpose. In no event shall
'Nokia Corporation be liable for any direct, indirect, special, incidental,
'or consequential loss or damages, including but not limited to, lost profits
'or revenue,loss of use, cost of substitute program, or loss of data or
'equipment arising out of the use or inability to use the material, even if
'Nokia Corporation has been advised of the likelihood of such damages occurring.

Option Strict Off
Option Explicit On 

Imports System.Runtime.InteropServices

Partial Class Nokia

    Partial Class APIS

        '=========================================================
        ' File System API versions 
        '
        Public Const FSAPI_VERSION_30 As Short = 30
        Public Const FSAPI_VERSION_31 As Short = 31
        Public Const FSAPI_VERSION_32 As Short = 32

        '=========================================================
        '
        '=========================================================
        ' FSAPI_Initialize
        '
        ' Description:
        '	FSAPI_Initialize initializes the API. This must be called once and before any other FSAPI call!
        '  It's not allowed to call this function like this 
        '		FSAPI_Initialize(FSAPI_GetAPIVersion(), NULL);
        '	You must call it like this
        '		FSAPI_Initialize(FSAPI_VERSION_32, NULL);
        '
        ' Parameters:
        '	dwAPIVersion	[in] FSAPI version requested.
        '	pdwParam		[in] Reserved for future use. Must be NULL.
        '
        ' Return values:
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function FSAPI_Initialize(ByVal iAPIVersion As Integer, ByVal pdwParam As IntPtr) As Integer

        End Function

        '=========================================================
        '
        '=========================================================
        ' FSAPI_Terminate
        '
        ' Description:
        '	FSAPI_Terminate terminates the API. This must be called once and as the last FSAPI call!
        '
        ' Parameters:
        '	pdwParam		[in] Reserved for future use. Must be NULL.
        '
        ' Return values:
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function FSAPI_Terminate(ByVal pdwParam As IntPtr) As Integer

        End Function


        '=========================================================
        '
        '=========================================================
        ' FSAPI_GetAPIVersion
        '
        ' Description:
        '	Returns currently installed version of this FSAPI.
        '
        ' Parameters:
        '
        ' Return values:
        '	API version number. 
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function FSAPI_GetAPIVersion() As Integer

        End Function


        '///////////////////////////////////////////////////////////
        ' ConnectivityAPI File System functions
        '///////////////////////////////////////////////////////////

        '=========================================================
        ' CONAOpenFS
        '
        ' Description:
        '	CONAOpenFS opens a file system (FS) connection to the target 
        '	device. The function sets the FS Connection handle, which 
        '	identifies different client connections. It will be placed 
        '	into the given pointer. 
        '	
        '	If the media type is API_MEDIA_ALL, the ServiceLayer selects 
        '	the used media type automatically by applying the following rule:
        '	If there is an open FS connection to the device, it will be used. 
        '	Otherwise the ServiceLayer tries to open the FS connection in the 
        '	following order: USB, Seria, Irda and BT (if the device is 
        '	connected via medias). The function also sets the DeviceID, 
        '	which can be used to identify the used media type.
        '	
        '	If the Serial Number is an empty string, the function tries to 
        '	open the FS connection by using the piDeviceID parameter. The 
        '	piMedia parameter is not used in the exemplary case.
        '
        ' Parameters:
        '	pstrSerialNumber [in]     The device's serial number, which must be set.
        '	piMedia		     [in,out] In: Media type, which defines the target
        '					          media for the Device's FS connection.
        '							  Out:If the value is API_MEDIA_ALL, the 
        '							  target media is selected automatically 
        '							  and the used media type is returned.
        '	phFSHandle		 [out]	  Handle to the opened FS connection
        '	piDeviceID		 [in,out] In: Device ID. Used only when the Serial 
        '							  number is empty string.
        '							  Out: Returned Device ID value of the connected device.
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_DEVICE_NOT_FOUND
        ' ECONA_NO_CONNECTION_VIA_MEDIA
        ' ECONA_NOT_SUPPORTED_DEVICE
        ' ECONA_CONNECTION_FAILED
        ' ECONA_CONNECTION_BUSY
        ' ECONA_CONNECTION_LOST
        ' ECONA_FAILED_TIMEOUT
        ' ECONA_INVALID_POINTER
        ' ECONA_NOT_INITIALIZED
        ' ECONA_NOT_SUPPORTED_MANUFACTURER
        ' ECONA_NOT_ENOUGH_MEMORY
        ' ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAOpenFS(<MarshalAs(UnmanagedType.LPWStr)> ByVal pstrSerialNumber As String, ByRef piMedia As Integer, ByRef phFSHandle As Integer, ByRef piDeviceID As Integer) As Integer

        End Function
        '=========================================================

        '=========================================================
        ' CONACloseFS
        '
        ' Description:
        '	CONACloseFS closes the given file system connection.
        '
        ' Parameters:
        '	hFSHandle	[in] Existing file system handle
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONACloseFS(ByVal hFSHandle As Integer) As Integer

        End Function
        '=========================================================

        '=========================================================
        ' CONARegisterFSNotifyCallback
        '
        ' Description:
        ' CONARegisterFSNotifyCallback registers or unregisters the 
        ' callback function for the file functions' status notifications.
        ' 
        ' During file operations, the connectionAPI calls this function 
        ' with the parameter's state (0-100%), connection handle, and 
        ' operation code.

        '
        ' Parameters:
        '	hFSHandle	[in] File System handle
        '	iState		[in] Register flag:
        '					  API_REGISTER:   Registers a callback function
        '					  API_UNREGISTER: Unregisters a callback function
        '	pFSNotify	[in] Pointer to a function.
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_POINTER
        '	ECONA_INVALID_PARAMETER
        '	ECONA_NOT_INITIALIZED
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONARegisterFSNotifyCallback(ByVal hFSHandle As Integer, ByVal iState As Integer, ByVal pFSNotify As FSNotifyCallbackDelegate) As Integer

        End Function
        '=========================================================

        '=========================================================
        ' CONARefreshDeviceMemoryValues
        '
        ' Description:
        '	CONARefreshDeviceMemoryValues refreshes the memory values 
        ' from the device. The function caches the memory values, so 
        ' it must be called if it is required to get the latest information.
        '
        ' Parameters:
        '	hFSHandle	[in] File system handle
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_INVALID_DATA_DEVICE
        '	ECONA_CANCELLED
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONARefreshDeviceMemoryValues(ByVal hFSHandle As Integer) As Integer

        End Function
        '=========================================================

        '=========================================================
        ' CONAGetMemoryTypes
        '
        ' Description:
        ' CONAGetMemoryTypes receives all memory types that currently 
        ' exist in the device. The format of its string is 
        ' "Memory type string" comma "next Memory type string". An 
        ' example of a string is "DEV,MMC,APPL". 
        ' 
        ' The device can include different memory types. For example, 
        ' the memory types can be "DEV" (device memory), "MMC" 
        ' (memory card), or "APPL" (device special memory).	
        '
        ' Parameters:
        '	hFSHandle			[in]  File system handle.
        '	ppstrMemoryTypes	[out] Memory types.
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_POINTER
        '	ECONA_NOT_SUPPORTED_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAGetMemoryTypes(ByVal hFSHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppstrMemoryTypes As String) As Integer

        End Function
        '=========================================================

        '=========================================================
        ' CONAGetMemoryValues
        '
        ' Description:
        '	CONAGetDeviceMemory returns info about the given device memory type.
        '
        ' Parameters:
        '	hFSHandle		[in]  File System handle.
        '	pstrMemoryType	[in]  Inquired device memory.E.g. "DEV", "MMC", 
        '                        or "APPL".If the value is NULL, the used value is "DEV".
        '	pdlFree			[out] Pointer to the variable receiving the 
        '						  information on the amount of free memory.
        '						  If not available, the value is -1.
        '	pdlTotal		[out] Pointer to the variable receiving 
        '						  the information on the total memory.
        '						  If not available, the value is -1.
        '	pdlUsed			[out] Pointer to the variable receiving 
        '						  the information on the used memory.
        '						  If not available, the value is -1.
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_POINTER
        '	ECONA_INVALID_PARAMETER
        '	ECONA_NOT_SUPPORTED_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAGetMemoryValues(ByVal hFSHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrMemoryType As String, ByRef pdlFree As Int64, ByRef pdlTotal As Int64, ByRef pdlUsed As Int64) As Integer

        End Function
        '=========================================================

        '=========================================================
        ' CONASetCurrentFolder
        '
        ' Description:
        ' CONASetCurrentFolder sets the device's current folder. All 
        ' file and folder operations with a relative path reference 
        ' will use this value. After the connection is established, 
        ' the default value is the device's root-folder. Only backslashes 
        ' ("\") are permissible folder separators.
        ' 
        ' Any other FS operation does not change the current folder. 
        ' If CONASetCurrentFolder fails, the currect folder is not changed. 
        '
        ' Parameters:
        '	hFSHandle	[in] File System handle
        '	pstrPath	[in] Name of the new current folder. Its value may 
        '					 be a relative or absolute reference. If the 
        '					 absolute path is used, the first two characters 
        '					 must be backslashes "\\". If you set the current 
        '					 folder to the root-folder, the path must include 
        '					 only two backslash characters "\\".
        '					 If the current folder is required to move sub folder, 
        '					 the path must not include any backslashes.
        '					 If the current folder is required to change the 
        '					 parent folder, the first two characters must be dots
        '					 and the third character must be a backslash "..\". 
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_PARAMETER
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_CURRENT_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_PATH_TOO_LONG
        '	ECONA_FOLDER_NAME_INVALID_DEVICE
        '	ECONA_FOLDER_NO_PERMISSION
        '	ECONA_CANCELLED
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONASetCurrentFolder(ByVal hFSHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrPath As String) As Integer

        End Function
        '=========================================================

        '=========================================================
        ' CONAGetCurrentFolder
        '
        ' Description:
        ' CONAGetCurrentFolder returns the current folder, which is 
        ' set by using the CONASetCurrentFolder function.
        '
        ' Parameters:
        '	hFSHandle		[in]  File System handle
        '	ppstrPath		[out] Pointer to the WCHAR receiving 
        '						  the current folder.
        '	
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_POINTER
        '	ECONA_CONNECTION_LOST
        '	ECONA_CANCELLED
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_NOT_INITIALIZED
        '	ECONA_UNKNOWN_ERROR	
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAGetCurrentFolder(ByVal hFSHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppstrPath As String) As Integer

        End Function
        '=========================================================

        '=========================================================
        ' CONAFindBegin
        '
        ' Description:
        ' CONAFindBegin starts find files and subfolders from currect 
        ' folder or target path. If target path is used, path must be 
        ' absolute path.Find option:
        ' CONA_FIND_USE_CACHE	  Use cache if it is available.
        '
        ' Parameters:
        '	hFSHandle		[in]  File System handle
        '	iFindOptions	[in]  Find options:If CONA_FIND_USE_CACHE option
        '						  is not used, value must be 0.
        '	pFileHandle		[out] Pointer to find handle
        '	pstrTargetPath	[in]  Target path. If the path is NULL, 
        '						  the current folder is used 
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_PARAMETER
        '	ECONA_INVALID_POINTER
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_INVALID_DATA_DEVICE
        '	ECONA_CURRENT_FOLDER_NOT_FOUND
        '	ECONA_CANCELLED
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_NOT_INITIALIZED
        '  ECONA_NOT_ENOUGH_MEMORY
        '	ECONA_UNKNOWN_ERROR
        '	ECONA_FOLDER_PATH_TOO_LONG
        '	ECONA_FOLDER_NAME_INVALID_DEVICE
        '	ECONA_FOLDER_NO_PERMISSION
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAFindBegin(ByVal hFSHandle As Integer, ByVal iFindOptions As Integer, ByRef phFindHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function
        '=========================================================

        '=========================================================
        ' CONAFindNextFolder
        '
        ' Description:
        ' CONAFindNextFolder returns the folder information for one 
        ' folder and continues the search to the next folder. The 
        ' folder information is returned with the CONAPI_FOLDER_INFO 
        ' structure. The allocated memory MUST be freed by calling 
        ' the CONAFreeFolderInfoStructure function.
        '
        ' Parameters:
        '	hFindHandle		[in]  Find handle
        '	pFolderInfo		[out] Pointer to the folder's info struct.
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_ALL_LISTED
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_POINTER
        '	ECONA_NOT_INITIALIZED
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAFindNextFolder(ByVal hFindHandle As Integer, ByVal pFolderInfo As IntPtr) As Integer

        End Function

        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAFindNextFolder(ByVal hFindHandle As Integer, ByRef pFolderInfo As CONAPI_FOLDER_INFO) As Integer

        End Function


        '=========================================================

        '=========================================================
        ' CONAFindNextFile
        '
        ' Description:
        ' CONAFindNextFile returns file information for one file, 
        ' and continues the search to the next file. File information 
        ' is returned with the CONAPI_FILE_INFO structure. Allocated 
        ' memory MUST be freed by calling CONAFreeFileInfoStructure function.
        '
        ' Parameters:
        '	hFindHandle		[in]  Find handle
        '	pFileInfo		[out] Pointer to the file's info struct.
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_ALL_LISTED
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_POINTER
        '	ECONA_NOT_INITIALIZED
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAFindNextFile(ByVal hFindHandle As Integer, ByVal pFileInfo As IntPtr) As Integer

        End Function

        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAFindNextFile(ByVal hFindHandle As Integer, ByRef pFileInfo As CONAPI_FILE_INFO) As Integer

        End Function

        '=========================================================

        '=========================================================
        ' CONAFindEnd
        '
        ' Description:
        ' CONAFindEnd closes find handle.
        '
        '
        ' Parameters:
        '	hFindHandle		[in]  Find handle
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAFindEnd(ByVal hFindHandle As Integer) As Integer

        End Function

        '=========================================================

        '=========================================================
        ' CONACreateFolder
        '
        ' Description:
        ' CONACreateFolder creates a new folder onto the device. 
        ' The function fails if the folder already exists on the device.
        '
        ' Parameters:
        '	hFSHandle		[in] File System handle
        '	pstrFolderName	[in] New folder name string
        '	pstrTargetPath	[in] Target path. If path is NULL, current 
        '						 folder is used. 
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_PARAMETER
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_INVALID_DATA_DEVICE
        '	ECONA_CURRENT_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_PATH_TOO_LONG
        '	ECONA_FOLDER_NAME_INVALID_DEVICE
        '	ECONA_FOLDER_ALREADY_EXIST
        '	ECONA_FOLDER_NO_PERMISSION
        '	ECONA_CANCELLED
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONACreateFolder(ByVal hFSHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrFolderName As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function
        '=========================================================

        '=========================================================
        ' CONADeleteFolder
        '
        ' Description:
        ' CONADeleteFolder deletes a folder from the device. This 
        ' function does not delete the folder if the folder includes 
        ' any subfolder(s)! Only an empty folder and a folder that 
        ' contains files can be deleted. If the folder does not include 
        ' a DELETE-permission, the function fails.
        '
        ' Note: No rollback option! Deleted data cannot be restored!
        '
        ' Parameters:
        '	hFSHandle		[in] File System handle
        '	pstrFolderName	[in] Folder name string
        '	iDeleteOptions	[in] Delete options. Possible values: 
        '			CONA_DELETE_FOLDER_EMPTY:		Only an empty folder can be deleted.
        '			CONA_DELETE_FOLDER_WITH_FILES:  Only a folder that contains file(s) can be deleted.
        '	pstrTargetPath	[in] Target path. If path is NULL, current
        '						 folder is used.  
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_PARAMETER
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_INVALID_DATA_DEVICE
        '	ECONA_CURRENT_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_PATH_TOO_LONG
        '	ECONA_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_NOT_EMPTY
        '	ECONA_FOLDER_NO_PERMISSION
        '	ECONA_CANCELLED
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONADeleteFolder(ByVal hFSHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrFolderName As String, ByVal iDeleteOptions As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function
        '=========================================================

        '=========================================================
        ' CONARenameFolder
        '
        ' Description:
        ' CONARenameFolder renames a folder on the device. Only an 
        ' empty folder can be renamed at the moment.

        '
        ' Parameters:
        '	hFSHandle		  [in] File System handle
        '	pstrFolderName	  [in] Folder name string.
        '	pstrFolderNameNew [in] Folder's new name string.
        '	pstrTargetPath	  [in] Target path. If the path is NULL, 
        '                         current folder is used. 
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_PARAMETER
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_INVALID_DATA_DEVICE
        '	ECONA_CURRENT_FOLDER_NOT_FOUND
        '  ECONA_FOLDER_PATH_TOO_LONG
        '	ECONA_FOLDER_ALREADY_EXIST
        '	ECONA_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_NOT_EMPTY
        '	ECONA_FOLDER_NO_PERMISSION
        '	ECONA_CANCELLED
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONARenameFolder(ByVal hFSHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrFolderName As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrFolderNameNew As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function
        '=========================================================
        '
        '=========================================================
        ' CONAGetFolderInfo
        '
        ' Description:
        ' CONAGetFolderInfo sets required information to  
        ' CONAPI_FOLDER_CONTENT structure. The Connectivity API-user 
        ' MUST initialize the CONAPI_FOLDER_CONTENT structure before 
        ' calling this function!
        '
        ' When option CONA_GET_FOLDER_INFO is used, it adds the next
        ' data to CONAPI_FOLDER_CONTENT structure:
        '		CONAPI_FOLDER_INFO2 structure from target folder. The 
        '		number of files and folders in target folder and total 
        '		size of folder content are not set.
        '
        ' When option CONA_GET_FOLDER_CONTENT is used, it adds the 
        ' next data to CONAPI_FOLDER_CONTENT structure:
        '		CONAPI_FOLDER_INFO2 structure from target folder. The 
        '		number of files and folders in target folder and total 
        '		size of folder content are not set.
        '		CONAPI_FILE_INFO structure(s) from file(s), which 
        '		are in target folder.
        '		CONAPI_FOLDER_CONTENT structure(s) from sub folder(s), 
        '		which are in target folder. Every structure includes 
        '		only CONAPI_FOLDER_INFO2 structure.
        '
        ' When option CONA_GET_FOLDER_AND_SUB_FOLDERS_CONTENT is used, 
        ' it adds the next data to CONAPI_FOLDER_CONTENT structure:
        '		CONAPI_FILE_INFO structure(s) from file(s), which 
        '		are in target folder.
        '		CONAPI_FOLDER_CONTENT structure(s) from sub folder(s), 
        '		which are in target folder. Every structure includes 
        '		CONAPI_FOLDER_INFO2 structure. If sub folder(s) contain
        '		file(s) or/and folder(s) those are also included. 
        '
        ' Allocated memory must be freed by calling the 
        ' CONAFreeFolderContentStructure function.
        ' 
        '
        ' Parameters:
        '	hFSHandle		[in]  File System handle
        '	dwOptions		[in]  Get folder info options:
        '		CONAPI_ALLOW_TO_USE_CACHE:	If the cached folder data 
        '			is available, it will be used. This value is  
        '			optional and can be used with other values.
        '		CONA_COMPARE_AND_UPDATE_IF_NEEDED: Compare given folder 
        '			content structure and returns one of the value:
        '			CONA_OK: Structure is not updated because it 
        '				is up-to-date.
        '			CONA_OK_UPDATED_MEMORY_VALUES: Structure is updated 
        '				because of free, used and total memory values 
        '				are updated.
        '			CONA_OK_UPDATED_MEMORY_AND_FILES: Structure is 
        '				updated because of file(s) and memory values 
        '				are updated.
        '			CONA_OK_UPDATED: Structure is updated, unknown reason. 
        '			This value is optional and can be used with other values.
        '		The next values must be used one at a time:
        '			CONA_GET_FOLDER_INFO
        '			CONA_GET_FOLDER_CONTENT
        '			CONA_GET_FOLDER_AND_SUB_FOLDERS_CONTENT
        '	pstrFolderName	[in] Name of target folder or drive. This value
        '		can be NULL. If target folder is device’s root folder, value 
        '		must include an empty string or NULL and target (or the 
        '		current folder) path must be “\\”.
        '	pstrTargetPath	[in] Target path. If the path is NULL or empty 
        '		string, the current folder is used. 
        '	pFolderContent	[out] Pointer of the CONAPI_FOLDER_CONTENT 
        '		structure. Connectivity API-user MUST initializes structure 
        '		before of call this function!
        '	pFSFolderInfoNotify	[in] Pointer to a callback function. The 
        '		Connectivity API calls this function only when option 
        '		CONA_GET_FOLDER_AND_SUB_FOLDERS_CONTENT is used. This value  
        '		is option and must be NULL if not used.
        '
        ' Return values:
        ' CONA_OK							
        ' CONA_OK_UPDATED_MEMORY_VALUES	 
        ' CONA_OK_UPDATED_MEMORY_AND_FILES	
        ' CONA_OK_UPDATED	
        ' ECONA_INVALID_HANDLE	
        ' ECONA_INVALID_PARAMETER	
        ' ECONA_INVALID_POINTER	
        ' ECONA_CONNECTION_BUSY	
        ' ECONA_CONNECTION_LOST	
        ' ECONA_INVALID_DATA_DEVICE
        ' ECONA_CURRENT_FOLDER_NOT_FOUND
        ' ECONA_FOLDER_NOT_FOUND
        ' ECONA_FOLDER_PATH_TOO_LONG
        ' ECONA_FOLDER_NAME_INVALID
        ' ECONA_FOLDER_NO_PERMISSION
        ' ECONA_CANCELLED
        ' ECONA_FAILED_TIMEOUT
        ' ECONA_UNKNOWN_ERROR_DEVICE
        ' ECONA_NOT_INITIALIZED
        ' ECONA_NOT_ENOUGH_MEMORY
        ' ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAGetFolderInfo(ByVal hFSHandle As Integer, ByVal iOptions As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrFolderName As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String, ByVal pFolderContent As IntPtr, ByVal pFSFolderInfoNotify As FSFolderInfoCallbackDelegate) As Integer

        End Function

        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAGetFolderInfo(ByVal hFSHandle As Integer, ByVal iOptions As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrFolderName As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String, ByRef pFolderContent As CONAPI_FOLDER_CONTENT, ByVal pFSFolderInfoNotify As FSFolderInfoCallbackDelegate) As Integer

        End Function



        '=========================================================
        '
        '=========================================================
        ' CONAMoveFolder
        '
        ' Description:
        ' CONAMoveFolder moves a folder from the source folder to destination folder.
        '
        ' If the device is supporting move a folder function, CONAPI_FS_EXTENDED_OPERATIONS
        ' value is set in CONAPI_DEVICE_GEN_INFO structure.  For more details, see 
        ' CONAGetDeviceInfo function of Device Management API. NOTE: This function is 
        ' not supported with all devices!
        '
        ' Note: The function stops when the first error occurs. No rollback option! 
        ' All moved items stay in destination folder if the error occurs. 
        '
        ' Parameters:
        '	hFSHandle		[in] File System handle
        '	dwOptions		[in] Option: 
        '		CONA_DIRECT_PHONE_TO_PHONE: Moves a folder inside the 
        '			device. Must be used.
        '		CONA_RENAME:				Renames the folder if the same  
        '			named folder is already exists in the destination path.
        '	pstrSource		[in] The path to source folder to be moved. If value 
        '		inludes only the source folder name, the current folder path is used.
        '	pstrDestination	[in] The path where the folder is to be moved. If the 
        '		destination path ends with path separator ('\'), it is assumed that  
        '		destination folder is exist and source folder to be moved to  
        '		destination folder. Otherwise, it is assumed that destionation folder 
        '		is not to exist and content of source folder to be moved to created 
        '		destination folder.
        '	pParam			[in] Reserved for future use. Must be NULL.
        '									
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_PARAMETER
        '	ECONA_INVALID_POINTER
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_INVALID_DATA_DEVICE
        '	ECONA_MEMORY_FULL
        '	ECONA_CURRENT_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_PATH_TOO_LONG
        '	ECONA_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_NO_PERMISSION
        '	ECONA_FOLDER_NAME_INVALID
        '	ECONA_FOLDER_ALREADY_EXIST
        '	ECONA_CANCELLED
        '	ECONA_NOTHING_TO_CANCEL
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_NOT_SUPPORTED_DEVICE
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_NOT_ENOUGH_MEMORY
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function FunctionCONAMoveFolder(ByVal hFSHandle As Integer, ByVal iOptions As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrSource As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrDestination As String, ByVal pParam As IntPtr) As Integer

        End Function

        '=========================================================
        '
        '=========================================================
        ' CONACopyFolder
        '
        ' Description:
        ' CONACopyFolder copies a folder from the source folder to destination folder. 
        '
        ' If the device is supporting copy a folder function, CONAPI_FS_EXTENDED_OPERATIONS
        ' value is set in CONAPI_DEVICE_GEN_INFO structure.  For more details, see 
        ' CONAGetDeviceInfo function of Device Management API. NOTE: This function is 
        ' not supported with all devices!
        '
        ' Note: The function stops when the first error occurs. No rollback option! 
        ' All copied items stay in destination folder if the error occurs. 
        '
        ' Parameters:
        '	hFSHandle		[in] File System handle
        '	dwOptions		[in] Options: 
        '		CONA_DIRECT_PHONE_TO_PHONE: Copies the folder inside the 
        '			device. Must be used.
        '		CONA_RENAME:				Renames the folder if the same  
        '			named folder is already exists in the destination path.
        '	pstrSource		[in] The path to the source folder to be copied. If value 
        '		inludes only the source folder name, the current folder path is used.
        '	pstrDestination	[in] The path where the folder is to be copied. If the 
        '		destination path ends with path separator ('\'), it is assumed that 
        '		destination folder is exists and source folder to be copied to  
        '		destination folder. Otherwise, it is assumed that destionation folder 
        '		is not to exist and content of source folder to be copied to created 
        '		destination folder.
        '	pParam			[in] Reserved for future use. Must be NULL.
        '									
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_PARAMETER
        '	ECONA_INVALID_POINTER
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_MEMORY_FULL
        '	ECONA_CURRENT_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_PATH_TOO_LONG
        '	ECONA_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_NO_PERMISSION
        '	ECONA_FOLDER_NAME_INVALID
        '	ECONA_FOLDER_ALREADY_EXIST
        '	ECONA_CANCELLED
        '	ECONA_NOTHING_TO_CANCEL
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_NOT_SUPPORTED_DEVICE
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_NOT_ENOUGH_MEMORY
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONACopyFolder(ByVal hFSHandle As Integer, ByVal iOptions As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrSource As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrDestination As String, ByVal pParam As IntPtr) As Integer

        End Function

        '=========================================================
        '
        '=========================================================
        ' CONAGetFileInfo
        '
        ' Description:
        ' CONAGetFileInfo returns the file data in CONAPI_FILE_INFO 
        ' structure. If the cached file data is available that will be used. 
        ' Allocated memory MUST be freed by calling the 
        ' CONAFreeFileInfoStructure function.
        '
        ' Parameters:
        '	hFSHandle		[in]  File System handle
        '	pstrFileName	[in]  File name 
        '	pFileInfo		[out] Pointer to the structure receiving
        '						  the file information.
        '	pstrTargetPath	[in]  Target path. If the path is NULL, 
        '						  the current folder is used.  
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_PARAMETER
        '	ECONA_INVALID_POINTER
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_INVALID_DATA_DEVICE
        '	ECONA_CURRENT_FOLDER_NOT_FOUND
        '	ECONA_CANCELLED
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_NOT_ENOUGH_MEMORY
        '	ECONA_UNKNOWN_ERROR
        '	ECONA_FOLDER_PATH_TOO_LONG
        '	ECONA_FOLDER_NAME_INVALID_DEVICE
        '	ECONA_FOLDER_NO_PERMISSION
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAGetFileInfo(ByVal hFSHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrFileName As String, ByVal pFileInfo As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function

        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAGetFileInfo(ByVal hFSHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrFileName As String, ByRef pFileInfo As CONAPI_FILE_INFO, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function



        '=========================================================
        '
        '=========================================================
        ' CONAGetFileMetadata
        '
        ' Description:
        '  CONAGetFileMetadata function allocates and sets the file metadata  
        '  to CONAPI_FILE_xxxx_METADATA structure and returns pointer to 
        '  structure. The returned stucture depends of the target file.
        '
        '  Connectivity API-user MUST releases the returned pointer by 
        '  calling the CONAFreeFileMetadataStructure function. 
        '
        ' Parameters:
        '	hFSHandle		[in]  File System handle
        '	pstrFileName	[in]  File name 
        '	dwOptions		[in]  Metadata options. Possible values:
        '		CONAPI_GET_METADATA:		Gets file metadata. Must be used.
        '		CONAPI_ALLOW_TO_USE_CACHE:	If the cached metadata of file 
        '			is available, it will be used. This value is  
        '			and can be used with other values.
        '	pdwFileMetadataType [out] Type of returned metadata stucture.
        '			Possible returned values:
        '		CONA_TYPE_OF_AUDIO_METADATA:	ppFileMetadata includes pointer 
        '			to CONAPI_FILE_AUDIO_METADATA structure.
        '		CONA_TYPE_OF_IMAGE_METADATA:	ppFileMetadata includes pointer 
        '			to CONAPI_FILE_IMAGE_METADATA structure.
        '	ppFileMetadata	[out] Pointer to the structure receiving
        '			the file metadata. Connectivity API-user must 
        '			initialize input pointer to NULL.
        '	pstrTargetPath	[in]  Target path. If the path is NULL, 
        '						  the current folder is used.  
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_PARAMETER
        '	ECONA_INVALID_POINTER
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_INVALID_DATA_DEVICE
        '	ECONA_CURRENT_FOLDER_NOT_FOUND
        '	ECONA_FILE_TYPE_NOT_SUPPORTED;
        '	ECONA_CANCELLED
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_FILE_NOT_FOUND
        '	ECONA_NOT_INITIALIZED
        '	ECONA_NOT_ENOUGH_MEMORY
        '	ECONA_UNKNOWN_ERROR
        '	ECONA_FOLDER_PATH_TOO_LONG
        '	ECONA_FOLDER_NAME_INVALID
        '	ECONA_FOLDER_NO_PERMISSION
        '	ECONA_NOT_SUPPORTED_PC
        '	ECONA_NOT_SUPPORTED_DEVICE
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAGetFileMetadata(ByVal hFSHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrFileName As String, ByVal dwOptions As Integer, ByRef pdwFileMetadataType As Integer, ByRef ppFileMetadata As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function

        '=========================================================

        '=========================================================
        ' CONADeleteFile
        '
        ' Description:
        '	CONADeleteFile deletes a file from the device's folder. 
        '	Note: No rollback option! Deleted data cannot be restored!
        '
        ' Parameters:
        '	hFSHandle		[in] File System handle
        '	pstrFileName	[in] File name 
        '	pstrTargetPath	[in] Target path. If the path is NULL, 
        '                       the current folder is used.  
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_PARAMETER
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_INVALID_DATA_DEVICE
        '	ECONA_CURRENT_FOLDER_NOT_FOUND
        '	ECONA_FILE_NOT_FOUND
        '	ECONA_FILE_NO_PERMISSION
        '	ECONA_CANCELLED
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONADeleteFile(ByVal hFSHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrFileName As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function
        '=========================================================

        '=========================================================
        ' CONAMoveFile
        '
        ' Description:
        '	CONAMoveFile copy file to target folder on PC or Device 
        '  from Device or PC source folder and removes a file from 
        '  the source folder on the Device or PC. 
        '
        '  Note: No rollback option! If an error occurs during the
        '	execution of the command (e.g. connection lost: file from 
        '  target folder will be deleted if it is changed), the 
        '  changes that have been already made are not cancelled.
        '  
        '  If the current folder is used, the path to the device's 
        '  source or target folder does not need to be set. PC side 
        '  path can also include the target/source file name. If path 
        '  to device's target or source folder is given, it cannot 
        '  include file name.
        '  
        '
        ' Parameters:
        '	hFSHandle		[in] File System handle
        '
        '	dwOptions		[in] Direction options:
        '		CONA_DIRECT_PHONE_TO_PC:	Moves a file from the device to the PC.
        '		CONA_DIRECT_PC_TO_PHONE:	Moves a file from the PC to the device.
        '		CONA_DIRECT_PHONE_TO_PHONE: Moves a file inside the device.
        '						 Other option:
        '		CONA_OVERWRITE:				Moves the file even if the target folder 
        '			contains a file of the same name. The file will be replaced.
        '
        '	pstrFileName	[in] File name. With direction options:
        '		CONA_DIRECT_PC_TO_PHONE and CONA_DIRECT_PHONE_TO_PC: The value can 
        '			be NULL, if the file name is given with the PC path.
        '		CONA_DIRECT_PHONE_TO_PHONE:	The value must include the file name.
        '
        '	pstrSourcePath	[in] Source folder path. With direction options:
        '		CONA_DIRECT_PHONE_TO_PHONE and CONA_DIRECT_PHONE_TO_PC:  The value 
        '			can be NULL, if the current folder folder is used.
        '		CONA_DIRECT_PC_TO_PHONE:	The value can also include the source 
        '			file name. (See *1)
        '
        '	pstrTargetPath	[in] Target (destination) folder path. With direction options:
        '		CONA_DIRECT_PHONE_TO_PC and CONA_DIRECT_PHONE_TO_PHONE: The value 
        '			can also include the target file name. (See *1). 
        '		CONA_DIRECT_PC_TO_PHONE:	The value can be NULL, if the current 
        '			folder folder is used.
        '
        '	*1) If the path is given without a file name, the last chracter must be 
        '	a backslash ('\'). Otherwise the Connectivity API will assume that path 
        '	includes the target/source file name.								
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_PARAMETER
        '	ECONA_INVALID_POINTER
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_INVALID_DATA_DEVICE
        '	ECONA_MEMORY_FULL
        '	ECONA_CURRENT_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_PATH_TOO_LONG
        '	ECONA_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_NO_PERMISSION
        '  ¨ECONA_FOLDER_NO_PERMISSION_ON_PC
        '	ECONA_FOLDER_NAME_INVALID
        '	ECONA_FILE_TOO_BIG_DEVICE
        '	ECONA_FILE_NAME_INVALID
        '	ECONA_FILE_NAME_TOO_LONG
        '	ECONA_FILE_NOT_FOUND
        '	ECONA_FILE_ALREADY_EXIST
        '	ECONA_FILE_COPYRIGHT_PROTECTED
        '	ECONA_FILE_NO_PERMISSION
        '	ECONA_FILE_NO_PERMISSION_ON_PC
        '	ECONA_FILE_BUSY
        '	ECONA_FILE_TYPE_NOT_SUPPORTED
        '	ECONA_FILE_EXIST
        '	ECONA_CANCELLED
        '	ECONA_NOTHING_TO_CANCEL
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_NOT_SUPPORTED_DEVICE
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_NOT_ENOUGH_MEMORY
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAMoveFile(ByVal hFSHandle As Integer, ByVal iOptions As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrFileName As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrSourcePath As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function

        '=========================================================

        '=========================================================
        ' CONACopyFile
        '
        ' Description:
        '	CONACopyFile copy file to target folder on PC or Device 
        '	from Device or PC source folder 
        '	
        '	If current folder is used, path to device folder do not 
        '	need to set. Current folder can be used for source and 
        '	target location. PC side path can also include the 
        '	target/source file name. If path to device's target or 
        '	source folder is given, it cannot include file name.
        '	
        '  Note: No rollback option! If an error occurs during the
        '	execution of the command (e.g. connection lost: file from 
        '  target folder will be deleted if it is changed), the 
        '  changes that have been already made are not cancelled.
        '	
        '
        ' Parameters:
        '	hFSHandle		[in] File System handle
        '
        '	dwOptions		[in] Direction options:
        '		CONA_DIRECT_PHONE_TO_PC:	Copies a file from the device to the PC.
        '		CONA_DIRECT_PC_TO_PHONE:	Copies a file from the PC to the device.
        '		CONA_DIRECT_PHONE_TO_PHONE: Copies a file inside the device.
        '						 Other options:
        '		CONA_RENAME:				Renames the file if the same named 
        '			file is already exists in the target folder.
        '		CONA_OVERWRITE:				Copies the file even if the target folder 
        '			contains a file of the same name. The file will be replaced.
        '
        '	pstrFileName	[in] File name. With direction options:
        '		CONA_DIRECT_PC_TO_PHONE and CONA_DIRECT_PHONE_TO_PC: The value can 
        '			be NULL, if the file name is given with the PC path.
        '		CONA_DIRECT_PHONE_TO_PHONE:	The value must include the file name.
        '
        '	pstrSourcePath	[in] Source folder path. With direction options:
        '		CONA_DIRECT_PHONE_TO_PHONE and CONA_DIRECT_PHONE_TO_PC:  The value 
        '			can be NULL, if the current folder folder is used.
        '		CONA_DIRECT_PC_TO_PHONE:	The value can also include the source 
        '			file name. (See *1)
        '
        '	pstrTargetPath	[in] Target (destination) folder path. With direction options:
        '		CONA_DIRECT_PHONE_TO_PC and CONA_DIRECT_PHONE_TO_PHONE: The value 
        '			can also include the target file name. (See *1). 
        '		CONA_DIRECT_PC_TO_PHONE:	The value can be NULL, if the current 
        '			folder folder is used.
        '
        '	*1) If the path is given without a file name, the last chracter must be 
        '	a backslash ('\'). Otherwise the Connectivity API will assume that path 
        '	includes the target/source file name.
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_PARAMETER
        '	ECONA_INVALID_POINTER
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_INVALID_DATA_DEVICE
        '	ECONA_MEMORY_FULL
        '	ECONA_CURRENT_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_PATH_TOO_LONG
        '	ECONA_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_NO_PERMISSION
        '	ECONA_FOLDER_NO_PERMISSION_ON_PC
        '	ECONA_FOLDER_NAME_INVALID
        '	ECONA_FILE_TOO_BIG_DEVICE
        '	ECONA_FILE_NAME_INVALID
        '	ECONA_FILE_NAME_TOO_LONG
        '	ECONA_FILE_NOT_FOUND
        '	ECONA_FILE_ALREADY_EXIST
        '	ECONA_FILE_COPYRIGHT_PROTECTED
        '	ECONA_FILE_NO_PERMISSION
        '	ECONA_FILE_NO_PERMISSION_ON_PC
        '	ECONA_FILE_BUSY
        '	ECONA_FILE_TYPE_NOT_SUPPORTED
        '	ECONA_CANCELLED
        '	ECONA_NOTHING_TO_CANCEL
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_NOT_SUPPORTED_DEVICE
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_NOT_ENOUGH_MEMORY
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONACopyFile(ByVal hFSHandle As Integer, ByVal iOptions As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrFileName As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrSourcePath As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function

        '=========================================================

        '=========================================================
        ' CONARenameFile
        '
        ' Description:
        '	CONARenameFile renames the given file.
        '
        ' Parameters:
        '	hFSHandle		[in] File System handle.
        '	pstrFileName	[in] Target file name.
        '	pstrNewFileName	[in] New file name.
        '	pstrTargetPath	[in] Target path. If the path is NULL, 
        '						 the current folder is used. 
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_PARAMETER
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_INVALID_DATA_DEVICE
        '	ECONA_MEMORY_FULL
        '	ECONA_CURRENT_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_PATH_TOO_LONG
        '	ECONA_FILE_NAME_INVALID
        '	ECONA_FILE_NAME_TOO_LONG
        '	ECONA_FILE_NOT_FOUND
        '	ECONA_FILE_ALREADY_EXIST
        '	ECONA_FILE_COPYRIGHT_PROTECTED
        '	ECONA_FILE_NO_PERMISSION
        '	ECONA_FILE_BUSY
        '	ECONA_FILE_TYPE_NOT_SUPPORTED
        '   ECONA_FILE_EXIST
        '	ECONA_NOT_SUPPORTED_DEVICE
        '	ECONA_CANCELLED
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_NOT_ENOUGH_MEMORY
        '	ECONA_UNKNOWN_ERROR
        '	
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONARenameFile(ByVal hFSHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrFileName As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrNewFileName As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function

        '=========================================================

        '=========================================================
        ' CONAReadFile
        '
        ' Description:
        '	CONAReadFile reads a file from the device. The Connectivity
        '	API will allocate memory for the file data automatically. 
        '	If the file is empty, pFileData is NULL. The file data 
        '	buffer must be released by using the CONAFreeFileData function.
        '
        ' Parameters:
        '	hFSHandle		  [in]  File System handle
        '	pFileInfo		  [in,out]  Target file info and File name 
        '			(or File type) must exist. File size value should be zero.
        '			If it is something else, only that size will be read 
        '          from the beginning of the file.The Connectivity API 
        '			sets the file time (if available) and file size 
        '          (number of bytes in FileData).
        '	ppFileData		  [out] Pointer to the pointer of the file data  
        '	iLimitedReadSize [in]  Reads only the number of bytes from 
        '			the beginning of the file. Value MUST be 0 (zero) when 
        '			the whole file is targeted to be read.
        '	pstrTargetPath	  [in]  Target path. If the path is NULL, 
        '							the current folder is used. 
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_PARAMETER
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_CURRENT_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_PATH_TOO_LONG
        '	ECONA_FILE_NAME_INVALID
        '	ECONA_FILE_NOT_FOUND
        '	ECONA_FILE_COPYRIGHT_PROTECTED
        '	ECONA_FILE_NO_PERMISSION
        '	ECONA_FILE_BUSY
        '	ECONA_CANCELLED
        '	ECONA_NOT_SUPPORTED_DEVICE
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_UNKNOWN_ERROR
        '	
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAReadFile(ByVal hFSHandle As Integer, ByVal pFileInfo As IntPtr, ByRef ppFileData As IntPtr, ByVal iLimitedReadSize As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function

        '=========================================================

        '=========================================================
        ' CONAWriteFile
        '
        ' Description:
        '	CONAWriteFile writes a file onto the device. The file 
        '	data buffer must be allocated by using the CONAAllocFileData 
        '	function, and afterwards the CONAWriteFile buffer must be 
        '	released by using the CONAFreeFileData function. 
        '
        '	If target folder includes same named file, it will be overwrited!
        '
        '	Note: No rollback option! Overwrited data cannot be restored!
        '
        ' Parameters:
        '	hFSHandle		[in] File System handle
        '	pFileInfo		[in] Writed file info, File name and 
        '						 file size must be set. 
        '	pFileData		[in] Pointer to file data 
        '	pstrTargetPath	[in] Target path. If the path is NULL, 
        '						 the current folder is used. 
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_PARAMETER
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_FILE_NAME_INVALID
        '	ECONA_INVALID_DATA_DEVICE
        '	ECONA_MEMORY_FULL
        '	ECONA_CURRENT_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_PATH_TOO_LONG
        '	ECONA_FILE_NAME_INVALID
        '	ECONA_FILE_NAME_TOO_LONG
        '	ECONA_FILE_ALREADY_EXIST
        '	ECONA_FILE_TYPE_NOT_SUPPORTED
        '	ECONA_CANCELLED
        '	ECONA_NOT_SUPPORTED_DEVICE
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_UNKNOWN_ERROR
        '	
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAWriteFile(ByVal hFSHandle As Integer, ByVal pFileInfo As IntPtr, ByVal pFileData As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function


        '=========================================================

        '=========================================================
        ' CONAReadFileInBlocks
        '
        ' Description:
        '	CONAReadFileInBlocks function is used to read a file in blocks 
        '	from the device.  
        '	Connectivity API delivers file data blocks via notifications. 
        '	Connectivity API-user must define the maximum file data block 
        '	size used in notifications.
        '	The minimum allowed block size is 64 KBytes (0xFFFF) and maximum 
        '	allowed block size is 32 MBytes (0x1FFFFFF). 
        '
        ' Parameters:
        '	hFSHandle			[in] File System handle
        '	pFileInfo			[in,out] Target file info structure pointer
        '		In: File name must be set. 
        '		Out: The Connectivity API sets the file modified time stamp (if available), 
        '		file size, file MIME type (if available) and file attributes. If dwOptions includes  
        '		CONA_NOT_SET_FILE_DETAILS value, Connectivity API is not update file info structure.
        '	dwOptions			[in] Options:
        '		CONA_NOT_SET_FILE_DETAILS: Optional value. Connectivity API is not update file info (pFileInfo) structure.
        '		CONA_USE_IF_NOTICATION:	pFSBlockDataNotify contain pointer to IFSAPIBlockNotify class
        '		CONA_USE_CB_NOTICATION:	pFSBlockDataNotify contain pointer to CONABlockDataCallbackFunction callback function.
        '	pFSBlockDataNotify	[in] Pointer to a notification function.
        '	dwFileDataBlockSize	[in] Used data block size.
        '	pstrTargetPath		[in] Target path. If the path is NULL, the current folder is used. 
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_PARAMETER
        '	ECONA_INVALID_POINTER
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_CURRENT_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_PATH_TOO_LONG
        '	ECONA_FOLDER_NAME_INVALID
        '	ECONA_FILE_NAME_INVALID
        '	ECONA_FILE_NOT_FOUND
        '	ECONA_FILE_COPYRIGHT_PROTECTED
        '	ECONA_FILE_NO_PERMISSION
        '	ECONA_FILE_BUSY
        '	ECONA_CANCELLED
        '	ECONA_NOT_SUPPORTED_DEVICE
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_NOT_ENOUGH_MEMORY
        '	ECONA_UNKNOWN_ERROR
        '	
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAReadFileInBlocks(ByVal hFSHandle As Integer, ByVal pFileInfo As IntPtr, ByVal iOptions As Integer, ByVal pFSBlockDataNotify As FSBlockDataCallbackDelegate, ByVal iFileDataBlockSize As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function

        '=========================================================
        '
        '=========================================================
        ' CONAWriteFileInBlocks
        '
        ' Description:
        '	CONAWriteFileInBlocks function is used to write a file in blocks onto the device.
        '	Connectivity API requests file data blocks via notifications. 
        '	Connectivity API-user must define the maximum file data block 
        '	size used in notifications.
        '	The minimum allowed block size is 64 KBytes (0xFFFF) and maximum 
        '	allowed block size is 32 MBytes (0x1FFFFFF). 
        ' 	
        '	The tFileTime-struct is used to set modified time stamp to
        '	writed file. If the value is zero or invalid, PC's current time is used. 
        '
        '	If the file size is unknown, dwFileSize parameter must be -1 (0xFFFFFFFF). If
        '	file size is available, value should be set in dwFileSize parameter.
        '	
        '	If target folder includes same named file, it will be overwrited!
        '
        '	Note: No rollback option! Overwrited data cannot be restored!
        '
        ' Parameters:
        '	hFSHandle			[in] File System handle
        '	pFileInfo			[in] Writed file info, File name must be set. If file time 
        '		is set and value is valid, that time send to device. Otherwise PC's current time is used.
        '	dwOptions			[in] Options:
        '		CONA_USE_IF_NOTICATION:	pFSBlockDataNotify contain pointer to IFSAPIBlockNotify class
        '		CONA_USE_CB_NOTICATION:	pFSBlockDataNotify contain pointer to CONABlockDataCallbackFunction callback function.
        '	pFSBlockDataNotify	[in] Pointer to a notification function.
        '	dwFileDataBlockSize	[in] Used data block size.
        '	pstrTargetPath		[in] Target path. If the path is NULL, the current folder is used. 
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_INVALID_PARAMETER
        '	ECONA_INVALID_POINTER
        '	ECONA_CONNECTION_BUSY
        '	ECONA_CONNECTION_LOST
        '	ECONA_INVALID_DATA_DEVICE
        '	ECONA_MEMORY_FULL
        '	ECONA_CURRENT_FOLDER_NOT_FOUND
        '	ECONA_FOLDER_PATH_TOO_LONG
        '	ECONA_FOLDER_NO_PERMISSION
        '	ECONA_FOLDER_NAME_INVALID
        '	ECONA_FILE_NAME_INVALID
        '	ECONA_FILE_NAME_TOO_LONG
        '	ECONA_FILE_ALREADY_EXIST
        '	ECONA_FILE_TYPE_NOT_SUPPORTED
        '	ECONA_CANCELLED
        '	ECONA_NOTHING_TO_CANCEL
        '	ECONA_NOT_SUPPORTED_DEVICE
        '	ECONA_FAILED_TIMEOUT
        '	ECONA_UNKNOWN_ERROR_DEVICE
        '	ECONA_NOT_INITIALIZED
        '	ECONA_UNKNOWN_ERROR
        '	
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAWriteFileInBlocks(ByVal hFSHandle As Integer, ByVal pFileInfo As IntPtr, ByVal iOptions As Integer, ByVal pFSBlockDataNotify As FSBlockDataCallbackDelegate, ByVal dwFileDataBlockSize As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function

        '=========================================================

        '=========================================================
        ' CONACancel
        '
        ' Description:
        '	Cancels the ongoing operation. If there is not a running 
        '	operation, the Cancel function fails with the error code 
        '  ECONA_NOTHING_TO_CANCEL.
        '
        ' Parameters:
        '	hFSHandle		[in] File System handle
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_HANDLE
        '	ECONA_NOTHING_TO_CANCEL
        '	ECONA_NOT_INITIALIZED
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONACancel(ByVal hFSHandle As Integer) As Integer

        End Function

        '=========================================================

        '=========================================================
        ' CONAFreeFileInfoStructure 
        '
        ' Description:
        '	CONAFreeFileInfoStructure frees a given list of CONAPI_FILE_INFO structs.
        '
        ' Parameters:
        '	pFileInfo		[in] Pointer to a file data struct.
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_POINTER
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAFreeFileInfoStructure(ByVal pFileInfo As IntPtr) As Integer

        End Function

        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAFreeFileInfoStructure(ByRef pFileInfo As CONAPI_FILE_INFO) As Integer

        End Function


        '=========================================================

        '=========================================================
        ' CONAFreeFolderInfoStructure
        '
        ' Description:
        '	CONAFreeFolderInfoStructure frees a given list of CONAPI_FOLDER_INFO structs.
        '
        ' Parameters:
        '	pFolderInfo		[in] Pointer to  afolder data struct.
        '
        ' Return values: 
        '	CONA_OK
        '	ECONA_INVALID_POINTER
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAFreeFolderInfoStructure(ByVal pFolderInfo As IntPtr) As Integer

        End Function

        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAFreeFolderInfoStructure(ByRef pFolderInfo As CONAPI_FOLDER_INFO) As Integer

        End Function

        '=========================================================
        ' CONAFreeFolderContentStructure
        '
        ' Description:
        '	CONAFreeFolderContentStructure frees all data from a given CONAPI_FOLDER_CONTENT struct.
        '
        ' Parameters:
        '	pFolder		[in] Pointer to a folder content struct.
        '
        ' Return values: 
        '	CONA_OK
        '	ECONA_INVALID_POINTER
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAFreeFolderContentStructure(ByVal ByValpFolder As IntPtr) As Integer

        End Function

        '=========================================================
        '
        '=========================================================
        ' CONAAllocFileDataMemory
        '
        ' Description:
        '	CONAAllocFileDataMemory function returns allocated memory 
        '	for file data.  The allocated memory format is an unsigned 
        '	char, and the memory must be freed by using the 
        '	CONAFreeFileDataMemory function.
        '
        ' Parameters:
        '	iDataSize			[in] Allocated memory size in bytes.
        '
        ' Return values:
        '	Unsigned char* - allocated memory pointer. Returns NULL if it fails.
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAAllocFileDataMemory(ByVal iDataSize As Integer) As IntPtr

        End Function

        '=========================================================

        '=========================================================
        ' CONAFreeFileDataMemory
        '
        ' Description:
        '	CONAFreeFileDataMemory frees file data memory.
        '
        ' Parameters:
        '	pFileData			[in] Pointer to file data.
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_POINTER
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAFreeFileDataMemory(ByVal pFileData As IntPtr) As Integer

        End Function

        '=========================================================
        ' CONAFreeFileMetadataStructure
        '
        ' Description:
        '	CONAFreeFileMetadataStructure frees file metadata structure.
        '
        ' Parameters:
        '	dwFileMetadataType		[in] Type of metadata stucture.
        '	ppFileMetadata			[in] Pointer to file metadata structure.
        '
        ' Return values:
        '	CONA_OK
        '  ECONA_INVALID_PARAMETER
        '	ECONA_INVALID_POINTER
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAFreeFileMetadataStructure(ByVal dwFileMetadataType As Integer, ByRef ppFileMetadata As IntPtr) As Integer

        End Function


        '=========================================================

    End Class

End Class
