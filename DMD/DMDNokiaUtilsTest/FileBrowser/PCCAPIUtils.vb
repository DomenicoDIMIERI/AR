'Filename    : PCCAPIUtils.vb
'Part of     : PCCAPI Example codes
'Description : Helper utilities, error management
'Version     : 3.2
'
'This example is only to be used with PC Connectivity API version 3.2.
'Compability ("as is") with future versions is not quaranteed.
'
'Copyright (c) 2005-2007 Nokia Corporation.
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

Module PCCAPIUtils
    '===================================================================
	' CONAError2String --  Returns error text for given CONA error code
	'
	'
	'===================================================================
    Public Function CONAError2String(ByVal errorCode As Integer) As String
        CONAError2String = ""

        If errorCode = CONA_OK Then
            CONAError2String = "CONA_OK: Succeeded."
        ElseIf errorCode = CONA_OK_UPDATED_MEMORY_VALUES Then
            CONAError2String = "CONA_OK_UPDATED_MEMORY_VALUES: Everything OK, given data is updated because (free, used and total) memory values are changed!"
        ElseIf errorCode = CONA_OK_UPDATED_MEMORY_AND_FILES Then
            CONAError2String = "CONA_OK_UPDATED_MEMORY_AND_FILES: Everything OK, given data is updated because files and memory values are changed!"
        ElseIf errorCode = CONA_OK_UPDATED Then
            CONAError2String = "CONA_OK_UPDATED: Everything OK, given data is updated, unknown reason."
        ElseIf errorCode = CONA_OK_BUT_USER_ACTION_NEEDED Then
            CONAError2String = "CONA_OK_BUT_USER_ACTION_NEEDED: Operation needs some user action on Device"
        ElseIf errorCode = CONA_WAIT_CONNECTION_IS_BUSY Then
            CONAError2String = "CONA_WAIT_CONNECTION_IS_BUSY: Operation started ok but other application has reserved the connection"

            ' Common error codes:
        ElseIf errorCode = ECONA_INIT_FAILED Then
            CONAError2String = "ECONA_INIT_FAILED: DLL initialization failed."
        ElseIf errorCode = ECONA_INIT_FAILED_COM_INTERFACE Then
            CONAError2String = "ECONA_INIT_FAILED_COM_INTERFACE: Failed to get connection to system."
        ElseIf errorCode = ECONA_NOT_INITIALIZED Then
            CONAError2String = "ECONA_NOT_INITIALIZED: API is not initialized."
        ElseIf errorCode = ECONA_UNSUPPORTED_API_VERSION Then
            CONAError2String = "ECONA_UNSUPPORTED_API_VERSION: API version not supported."
        ElseIf errorCode = ECONA_NOT_SUPPORTED_MANUFACTURER Then
            CONAError2String = "ECONA_NOT_SUPPORTED_MANUFACTURER: Manufacturer is not supported."

        ElseIf errorCode = ECONA_UNKNOWN_ERROR Then
            CONAError2String = "ECONA_UNKNOWN_ERROR: Failed, unknown error."
        ElseIf errorCode = ECONA_UNKNOWN_ERROR_DEVICE Then
            CONAError2String = "ECONA_UNKNOWN_ERROR_DEVICE: Failed, unknown error from device."
        ElseIf errorCode = ECONA_INVALID_POINTER Then
            CONAError2String = "ECONA_INVALID_POINTER: Required pointer is invalid."
        ElseIf errorCode = ECONA_INVALID_PARAMETER Then
            CONAError2String = "ECONA_INVALID_PARAMETER: Invalid parameter value."
        ElseIf errorCode = ECONA_INVALID_HANDLE Then
            CONAError2String = "ECONA_INVALID_HANDLE: Invalid handle."
        ElseIf errorCode = ECONA_NOT_ENOUGH_MEMORY Then
            CONAError2String = "ECONA_NOT_ENOUGH_MEMORY: Memory allocation failed in PC."
        ElseIf errorCode = ECONA_WRONG_THREAD Then
            CONAError2String = "ECONA_WRONG_THREAD: Failed, called interface was marshalled for a different thread."
        ElseIf errorCode = ECONA_REGISTER_ALREADY_DONE Then
            CONAError2String = "ECONA_REGISTER_ALREADY_DONE: Failed, notification interface is already registered."

        ElseIf errorCode = ECONA_CANCELLED Then
            CONAError2String = "ECONA_CANCELLED: Operation cancelled by ConnectivityAPI-User."
        ElseIf errorCode = ECONA_NOTHING_TO_CANCEL Then
            CONAError2String = "ECONA_NOTHING_TO_CANCEL: No running functions."
        ElseIf errorCode = ECONA_FAILED_TIMEOUT Then
            CONAError2String = "ECONA_FAILED_TIMEOUT: Operation failed because of timeout."
        ElseIf errorCode = ECONA_NOT_SUPPORTED_DEVICE Then
            CONAError2String = "ECONA_NOT_SUPPORTED_DEVICE: Device does not support operation."
        ElseIf errorCode = ECONA_NOT_SUPPORTED_PC Then
            CONAError2String = "ECONA_NOT_SUPPORTED_PC: Connectivity API does not support operation (not implemented)."
        ElseIf errorCode = ECONA_NOT_FOUND Then
            CONAError2String = "ECONA_NOT_FOUND: Item was not found"
        ElseIf errorCode = ECONA_FAILED Then
            CONAError2String = "ECONA_FAILED: The called operation failed."

        ElseIf errorCode = ECONA_API_NOT_FOUND Then
            CONAError2String = "ECONA_API_NOT_FOUND: Needed API module was not found from the system"
        ElseIf errorCode = ECONA_API_FUNCTION_NOT_FOUND Then
            CONAError2String = "ECONA_API_FUNCTION_NOT_FOUND: Called API function was not found from the loaded API module"

            ' Device manager and device connection related errors:
        ElseIf errorCode = ECONA_DEVICE_NOT_FOUND Then
            CONAError2String = "ECONA_DEVICE_NOT_FOUND: Given phone is not connected (refresh device list)."
        ElseIf errorCode = ECONA_NO_CONNECTION_VIA_MEDIA Then
            CONAError2String = "ECONA_NO_CONNECTION_VIA_MEDIA: Phone is connected but not via given media."
        ElseIf errorCode = ECONA_NO_CONNECTION_VIA_DEVID Then
            CONAError2String = "ECONA_NO_CONNECTION_VIA_DEVID: Phone is not connected with given DevID."
        ElseIf errorCode = ECONA_INVALID_CONNECTION_TYPE Then
            CONAError2String = "ECONA_INVALID_CONNECTION_TYPE: Connection type was invalid."
        ElseIf errorCode = ECONA_NOT_SUPPORTED_CONNECTION_TYPE Then
            CONAError2String = "ECONA_NOT_SUPPORTED_CONNECTION_TYPE: Device does not support connection type."
        ElseIf errorCode = ECONA_CONNECTION_BUSY Then
            CONAError2String = "ECONA_CONNECTION_BUSY: Other application has reserved connection."
        ElseIf errorCode = ECONA_CONNECTION_LOST Then
            CONAError2String = "ECONA_CONNECTION_LOST: Connection lost to device."
        ElseIf errorCode = ECONA_CONNECTION_REMOVED Then
            CONAError2String = "ECONA_CONNECTION_REMOVED: Connection removed, other application has reserved connection."
        ElseIf errorCode = ECONA_CONNECTION_FAILED Then
            CONAError2String = "ECONA_CONNECTION_FAILED: Connection failed, unknown reason."
        ElseIf errorCode = ECONA_SUSPEND Then
            CONAError2String = "ECONA_SUSPEND: Connection removed, PC goes to standby state."
        ElseIf errorCode = ECONA_NAME_ALREADY_EXISTS Then
            CONAError2String = "ECONA_NAME_ALREADY_EXISTS: Friendly name already exists."
        ElseIf errorCode = ECONA_MEDIA_IS_NOT_WORKING Then
            CONAError2String = "ECONA_MEDIA_IS_NOT_WORKING: Target media is active but it is not working (e.g. BT hardware stopped or removed)."
        ElseIf errorCode = ECONA_CACHE_IS_NOT_AVAILABLE Then
            CONAError2String = "ECONA_CACHE_IS_NOT_AVAILABLE: Cache is not available (CONASearchDevices)."
        ElseIf errorCode = ECONA_MEDIA_IS_NOT_ACTIVE Then
            CONAError2String = "ECONA_MEDIA_IS_NOT_ACTIVE: Target media is busy (or not ready yet)."
        ElseIf errorCode = ECONA_PORT_OPEN_FAILED Then
            CONAError2String = "ECONA_PORT_OPEN_FAILED: Cannot open the changed COM port."

            ' Device pairing related errors:
        ElseIf errorCode = ECONA_DEVICE_PAIRING_FAILED Then
            CONAError2String = "ECONA_DEVICE_PAIRING_FAILED: Pairing failed."
        ElseIf ECONA_DEVICE_PASSWORD_WRONG Then
            errorCode = CONAError2String = "ECONA_DEVICE_PASSWORD_WRONG: Wrong password on device."
        ElseIf ECONA_DEVICE_PASSWORD_INVALID Then
            errorCode = CONAError2String = "ECONA_DEVICE_PASSWORD_INVALID: Password includes invalid characters or is missing."

            ' File System errors:
        ElseIf errorCode = ECONA_ALL_LISTED Then
            CONAError2String = "ECONA_ALL_LISTED: All items are listed."
        ElseIf errorCode = ECONA_MEMORY_FULL Then
            CONAError2String = "ECONA_MEMORY_FULL: Device memory full."

            ' File System errors for file functions:
        ElseIf errorCode = ECONA_FILE_NAME_INVALID Then
            CONAError2String = "ECONA_FILE_NAME_INVALID: File name contains invalid characters in Device or PC."
        ElseIf errorCode = ECONA_FILE_NAME_TOO_LONG Then
            CONAError2String = "ECONA_FILE_NAME_TOO_LONG: File name contains too many characters in Device or PC."
        ElseIf errorCode = ECONA_FILE_ALREADY_EXIST Then
            CONAError2String = "ECONA_FILE_ALREADY_EXIST: File already exists in Device or PC."
        ElseIf errorCode = ECONA_FILE_NOT_FOUND Then
            CONAError2String = "ECONA_FILE_NOT_FOUND: File does not exist in Device or PC."
        ElseIf errorCode = ECONA_FILE_NO_PERMISSION Then
            CONAError2String = "ECONA_FILE_NO_PERMISSION: Not allowed to perform required operation to file in device or PC."
        ElseIf errorCode = ECONA_FILE_COPYRIGHT_PROTECTED Then
            CONAError2String = "ECONA_FILE_COPYRIGHT_PROTECTED: Not allowed to perform required operation to file in device or PC."
        ElseIf errorCode = ECONA_FILE_BUSY Then
            CONAError2String = "ECONA_FILE_BUSY: Other application has reserved file in device or PC."
        ElseIf errorCode = ECONA_FILE_TOO_BIG_DEVICE Then
            CONAError2String = "ECONA_FILE_TOO_BIG_DEVICE: Device rejected the operation because file size is too big."
        ElseIf errorCode = ECONA_FILE_TYPE_NOT_SUPPORTED Then
            CONAError2String = "ECONA_FILE_TYPE_NOT_SUPPORTED: Device rejected the operation because file has unsupported type."
        ElseIf errorCode = ECONA_FILE_NO_PERMISSION_ON_PC Then
            CONAError2String = "ECONA_FILE_NO_PERMISSION_ON_PC: Not allowed to perform required operation to file in PC."
        ElseIf errorCode = ECONA_FILE_EXIST Then
            CONAError2String = "ECONA_FILE_EXIST: File move or rename: File is copied to target path with new name but removing the source file failed."
        ElseIf errorCode = ECONA_FILE_CONTENT_NOT_FOUND Then
            CONAError2String = "ECONA_FILE_CONTENT_NOT_FOUND: Specified file content does not found (e.g. unknown file section or stored position)"
        ElseIf errorCode = ECONA_FILE_OLD_FORMAT Then
            CONAError2String = "ECONA_FILE_OLD_FORMAT: Specified file content supports old engine"
        ElseIf errorCode = ECONA_FILE_INVALID_DATA Then
            CONAError2String = "ECONA_FILE_INVALID_DATA: Specified file data is invalid"

            ' File System errors for folder functions:
        ElseIf errorCode = ECONA_INVALID_DATA_DEVICE Then
            CONAError2String = "ECONA_INVALID_DATA_DEVICE: Device's folder contains invalid data."
        ElseIf errorCode = ECONA_CURRENT_FOLDER_NOT_FOUND Then
            CONAError2String = "ECONA_CURRENT_FOLDER_NOT_FOUND: Current folder is invalid in device (e.g MMC card removed)."
        ElseIf errorCode = ECONA_FOLDER_PATH_TOO_LONG Then
            CONAError2String = "ECONA_FOLDER_PATH_TOO_LONG: Maximum folder path length is 255 characters."
        ElseIf errorCode = ECONA_FOLDER_NAME_INVALID Then
            CONAError2String = "ECONA_FOLDER_NAME_INVALID: Folder name includes invalid characters in Device or PC."
        ElseIf errorCode = ECONA_FOLDER_ALREADY_EXIST Then
            CONAError2String = "ECONA_FOLDER_ALREADY_EXIST: Folder already exists in target folder."
        ElseIf errorCode = ECONA_FOLDER_NOT_FOUND Then
            CONAError2String = "ECONA_FOLDER_NOT_FOUND: Folder does not exist in target folder."
        ElseIf errorCode = ECONA_FOLDER_NO_PERMISSION Then
            CONAError2String = "ECONA_FOLDER_NO_PERMISSION: Not allowed to perform required operation to folder in Device."
        ElseIf errorCode = ECONA_FOLDER_NOT_EMPTY Then
            CONAError2String = "ECONA_FOLDER_NOT_EMPTY: Not allowed to perform required operation because folder is not empty."
        ElseIf errorCode = ECONA_FOLDER_NO_PERMISSION_ON_PC Then
            CONAError2String = "ECONA_FOLDER_NO_PERMISSION_ON_PC: Not allowed to perform required operation to folder in PC."

            ' Application installation error:
        ElseIf errorCode = ECONA_DEVICE_INSTALLER_BUSY Then
            CONAError2String = "ECONA_DEVICE_INSTALLER_BUSY: Cannot start device's installer."

            ' Syncronization specific error codes:
        ElseIf errorCode = ECONA_UI_NOT_IDLE_DEVICE Then
            CONAError2String = "ECONA_UI_NOT_IDLE_DEVICE: Device rejects the operation. Maybe device's UI is not in idle state."
        ElseIf errorCode = ECONA_SYNC_CLIENT_BUSY_DEVICE Then
            CONAError2String = "ECONA_SYNC_CLIENT_BUSY_DEVICE: Device's SA sync client is busy."
        ElseIf errorCode = ECONA_UNAUTHORIZED_DEVICE Then
            CONAError2String = "ECONA_UNAUTHORIZED_DEVICE: Device rejects the operation. No permission."
        ElseIf errorCode = ECONA_DATABASE_LOCKED_DEVICE Then
            CONAError2String = "ECONA_DATABASE_LOCKED_DEVICE: Device rejects the operation. Device is locked."
        ElseIf errorCode = ECONA_SETTINGS_NOT_OK_DEVICE Then
            CONAError2String = "ECONA_SETTINGS_NOT_OK_DEVICE: Device rejects the operation. Maybe settings in Sync profile are wrong on Device."
        ElseIf errorCode = ECONA_SYNC_ITEM_TOO_BIG Then
            CONAError2String = "ECONA_SYNC_ITEM_TOO_BIG: Device rejected the operation"
        ElseIf errorCode = ECONA_SYNC_ITEM_REJECT Then
            CONAError2String = "ECONA_SYNC_ITEM_REJECT: Device rejected the operation"
        ElseIf errorCode = ECONA_SYNC_INSTALL_PLUGIN_FIRST Then
            CONAError2String = "ECONA_SYNC_INSTALL_PLUGIN_FIRST: Device rejected the operation"

            ' Versit conversion specific error codes:			
        ElseIf errorCode = ECONA_VERSIT_INVALID_PARAM Then
            CONAError2String = "ECONA_VERSIT_INVALID_PARAM: Invalid parameters passed to versit converter."
        ElseIf errorCode = ECONA_VERSIT_UNKNOWN_TYPE Then
            CONAError2String = "ECONA_VERSIT_UNKNOWN_TYPE: Failed, trying to convert versit formats not supported in VersitConverter."
        ElseIf errorCode = ECONA_VERSIT_INVALID_VERSIT_OBJECT Then
            CONAError2String = "ECONA_VERSIT_INVALID_VERSIT_OBJECT: Failed, validation of versit data not passed, contains invalid data."
            ' Database specific error codes:
        ElseIf errorCode = ECONA_DB_TRANSACTION_ALREADY_STARTED Then
            CONAError2String = "Another transaction is already in progress"
        ElseIf errorCode = ECONA_DB_TRANSACTION_FAILED Then
            CONAError2String = "Some of operations within a transaction failed and transaction was rolled back"

            ' Backup specific error codes:
        ElseIf errorCode = ECONA_DEVICE_BATTERY_LEVEL_TOO_LOW Then
            CONAError2String = "Failed, device rejects the restore operation. Device's battery level is low."
        ElseIf errorCode = ECONA_DEVICE_BUSY Then
            CONAError2String = "Failed, device rejects the backup/resore operation. Device's backup server busy."

        Else
            CONAError2String = "Undefined error code" ' shouldn't occur
        End If
    End Function
	
    '===================================================================
    ' ErrorMessageDlg --  Show an errormessage
    '
    '
    '===================================================================
    Public Sub ShowErrorMessage(ByRef strError As String, ByRef errorCode As Integer)
        Dim strMessage As String = strError & Chr(13) & Chr(10) & Chr(13) & Chr(10) & "Error: 0x" & Hex(errorCode) & Chr(13) & Chr(10) & CONAError2String(errorCode)
        MsgBox(strMessage)
    End Sub

    '===================================================================
    ' Long2MediaString --  Returns name of the media for given Long
    '
    '
    '===================================================================
	Public Function Long2MediaString(ByVal media As Integer) As String
        If media = API_MEDIA_IRDA Then
            Long2MediaString = "IrDA"
        ElseIf media = API_MEDIA_SERIAL Then
            Long2MediaString = "Serial"
        ElseIf media = API_MEDIA_BLUETOOTH Then
            Long2MediaString = "Bluetooth"
        ElseIf media = API_MEDIA_USB Then
            Long2MediaString = "USB"
        Else
            Long2MediaString = "Unknown media" ' shouldn't occur
        End If
    End Function

    '===================================================================
    ' Permissions2String
    ' 
    ' Converts file attributes to string
    ' 
    '===================================================================
    Public Function Permissions2String(ByVal iAttributes As Integer) As String
        Dim strAttributes As String
        strAttributes = ""
        If iAttributes And CONA_FPERM_READ Then
            strAttributes += "R"
        End If
        If iAttributes And CONA_FPERM_WRITE Then
            strAttributes += "W"
        End If
        If iAttributes And CONA_FPERM_DELETE Then
            strAttributes += "D"
        End If
        If iAttributes And CONA_FPERM_HIDDEN Then
            strAttributes += "H"
        End If
        If iAttributes And CONA_FPERM_FOLDER Then
            strAttributes += " Folder"
        End If
        If iAttributes And CONA_FPERM_DRIVE Then
            strAttributes += " Drive"
        End If
        If iAttributes And CONA_FPERM_ROOT Then
            strAttributes += " Root"
        End If
        Return strAttributes
    End Function
End Module
