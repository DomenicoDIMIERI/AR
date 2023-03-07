'
'==============================================================================
' Local Connectivity API 3.2
'
'Filename    : PCCSErrors.vb
'Description : Error Definitions
'Version     : 3.2
'
'Copyright (c) 2005, 2006, 2007 Nokia Corporation.
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

Partial Public Class Nokia

    Partial Public Class APIS

        'Public NotInheritable Class PCCSErrors
        '    Private Sub New()
        '    End Sub

        '///////////////////////////////////////////////////////////
        '// Connectivity API errors
        '///////////////////////////////////////////////////////////

        Public Const CONA_OK As Integer = &H0                           ' Everything ok
        Public Const CONA_OK_UPDATED_MEMORY_VALUES As Integer = &H1     ' Everything ok, given data is updated because (free, used and total) memory values are changed!
        Public Const CONA_OK_UPDATED_MEMORY_AND_FILES As Integer = &H2  ' Everything ok, given data is updated because files and memory values are changed!
        Public Const CONA_OK_UPDATED As Integer = &H4                   ' Everything ok, given data is updated, unknown reason.
        Public Const CONA_OK_BUT_USER_ACTION_NEEDED As Integer = &H100  ' Everything ok, but operation needs some user action (device side)
        Public Const CONA_WAIT_CONNECTION_IS_BUSY As Integer = &H101    ' Operation started ok but other application is reserved connection, please wait.
        '                                                                 This result code comes via FS nofication when ConnAPI is initialized by value 20 or bigger.

        ' Common error codes:
        Public Const ECONA_INIT_FAILED As Integer = &H80100000 ' DLL initialization failed
        Public Const ECONA_INIT_FAILED_COM_INTERFACE As Integer = &H80100002 ' Failed to get connection to System.
        Public Const ECONA_NOT_INITIALIZED As Integer = &H80100004 ' API is not initialized
        Public Const ECONA_UNSUPPORTED_API_VERSION As Integer = &H80100005 ' Failed, not supported API version
        Public Const ECONA_NOT_SUPPORTED_MANUFACTURER As Integer = &H80100006 ' Failed, not supported manufacturer
        Public Const ECONA_UNKNOWN_ERROR As Integer = &H80100010 ' Failed, unknown error
        Public Const ECONA_UNKNOWN_ERROR_DEVICE As Integer = &H80100011 ' Failed, unknown error from Device
        Public Const ECONA_INVALID_POINTER As Integer = &H80100012 ' Required pointer is invalid
        Public Const ECONA_INVALID_PARAMETER As Integer = &H80100013 ' Invalid Parameter value
        Public Const ECONA_INVALID_HANDLE As Integer = &H80100014 ' Invalid HANDLE
        Public Const ECONA_NOT_ENOUGH_MEMORY As Integer = &H80100015 ' Memory allocation failed in PC
        Public Const ECONA_WRONG_THREAD As Integer = &H80100016    ' Failed, Called interface was marshalled for a different thread.
        Public Const ECONA_REGISTER_ALREADY_DONE As Integer = &H80100017 ' Failed, notification interface is already registered.
        Public Const ECONA_CANCELLED As Integer = &H80100020 ' Operation cancelled by ConnectivityAPI-User
        Public Const ECONA_NOTHING_TO_CANCEL As Integer = &H80100021 ' No running functions, or cancel has called too late.
        Public Const ECONA_FAILED_TIMEOUT As Integer = &H80100022 ' Operation failed because of timeout
        Public Const ECONA_NOT_SUPPORTED_DEVICE As Integer = &H80100023 ' Device do not support operation
        Public Const ECONA_NOT_SUPPORTED_PC As Integer = &H80100024 ' ConnectivityAPI do not support operation (not implemented)
        Public Const ECONA_NOT_FOUND As Integer = &H80100025       ' Item was not found
        Public Const ECONA_FAILED As Integer = &H80100026 ' Failed, the called operation failed.

        Public Const ECONA_API_NOT_FOUND As Integer = &H80100100 ' Needed API module was not found from the system
        Public Const ECONA_API_FUNCTION_NOT_FOUND As Integer = &H80100101 ' Called API function was not found from the loaded API module

        ' Device manager and device connection related errors:
        Public Const ECONA_DEVICE_NOT_FOUND As Integer = &H80200000 ' Given phone is not connected (refresh device list)
        Public Const ECONA_NO_CONNECTION_VIA_MEDIA As Integer = &H80200001 ' Phone is connected but not via given Media
        Public Const ECONA_NO_CONNECTION_VIA_DEVID As Integer = &H80200002 ' Phone is not connected with given DevID
        Public Const ECONA_INVALID_CONNECTION_TYPE As Integer = &H80200003 ' Connection type was invalid
        Public Const ECONA_NOT_SUPPORTED_CONNECTION_TYPE As Integer = &H80200004 ' Device do not support connection type
        Public Const ECONA_CONNECTION_BUSY As Integer = &H80200005 ' Other application is recerved connection
        Public Const ECONA_CONNECTION_LOST As Integer = &H80200006 ' Connection is lost to Device
        Public Const ECONA_CONNECTION_REMOVED As Integer = &H80200007 ' Connection removed, other application is reserved connection.
        Public Const ECONA_CONNECTION_FAILED As Integer = &H80200008 ' Connection failed, unknown reason
        Public Const ECONA_SUSPEND As Integer = &H80200009 ' Connection removed, PC goes suspend state
        Public Const ECONA_NAME_ALREADY_EXISTS As Integer = &H8020000A ' Friendly name already exist
        Public Const ECONA_MEDIA_IS_NOT_WORKING As Integer = &H8020000B     ' Failed, target media is active but it is not working (e.g. BT-hardware stopped or removed)
        Public Const ECONA_CACHE_IS_NOT_AVAILABLE As Integer = &H8020000C   ' Failed, cache is not available (CONASearchDevices)
        Public Const ECONA_MEDIA_IS_NOT_ACTIVE As Integer = &H8020000D      ' Failed, target media is active (or ready yet)
        Public Const ECONA_PORT_OPEN_FAILED As Integer = &H8020000E ' Port opening failed (only when media is API_MEDIA_SERIAL and COM port is changed).

        ' Device paring releated errors:
        Public Const ECONA_DEVICE_PAIRING_FAILED As Integer = &H80200100    ' Failed, pairing failed
        Public Const ECONA_DEVICE_PASSWORD_WRONG As Integer = &H80200101    ' Failed, wrong password on device. 
        Public Const ECONA_DEVICE_PASSWORD_INVALID As Integer = &H80200102  ' Failed, password includes invalid characters or missing. 

        ' File System errors:
        Public Const ECONA_ALL_LISTED As Integer = &H80300000 ' All items are listed
        Public Const ECONA_MEMORY_FULL As Integer = &H80300001 ' Device memory full

        ' File System errors for file functions:
        Public Const ECONA_FILE_NAME_INVALID As Integer = &H80400001 ' File name includes invalid characters in Device or PC
        Public Const ECONA_FILE_NAME_TOO_LONG As Integer = &H80400002 ' File name includes too many characters in Device or PC
        Public Const ECONA_FILE_ALREADY_EXIST As Integer = &H80400003 ' File already exists in Device or PC
        Public Const ECONA_FILE_NOT_FOUND As Integer = &H80400004 ' File does not exist in Device or PC
        Public Const ECONA_FILE_NO_PERMISSION As Integer = &H80400005 ' Not allowed to perform required operation to file in Device or PC
        Public Const ECONA_FILE_COPYRIGHT_PROTECTED As Integer = &H80400006 ' Not allowed to perform required operation to file in Device or PC
        Public Const ECONA_FILE_BUSY As Integer = &H80400007 ' Other application has reserved file in Device or PC
        Public Const ECONA_FILE_TOO_BIG_DEVICE As Integer = &H80400008 ' Device rejected the operation because file size is too big
        Public Const ECONA_FILE_TYPE_NOT_SUPPORTED As Integer = &H80400009 ' Device rejected the operation because file unsupported type
        Public Const ECONA_FILE_NO_PERMISSION_ON_PC As Integer = &H8040000A
        Public Const ECONA_FILE_EXIST As Integer = &H8040000B     ' File move or rename: File is copied to target path with new name but removing the source file failed. 
        Public Const ECONA_FILE_CONTENT_NOT_FOUND As Integer = &H8040000C  ' Specified file content does not found (e.g. unknown file section or stored position).
        Public Const ECONA_FILE_OLD_FORMAT As Integer = &H8040000D  ' Specified file content supports old engine.
        Public Const ECONA_FILE_INVALID_DATA As Integer = &H8040000E ' Specified file data is invalid.

        ' File System errors for folder functions:
        Public Const ECONA_INVALID_DATA_DEVICE As Integer = &H80500000 ' Device's folder contains invalid data
        Public Const ECONA_CURRENT_FOLDER_NOT_FOUND As Integer = &H80500001 ' Current folder is invalid in device (e.g MMC card removed).
        Public Const ECONA_FOLDER_PATH_TOO_LONG As Integer = &H80500002 ' Current folder max unicode charaters count is limited to 255.
        Public Const ECONA_FOLDER_NAME_INVALID As Integer = &H80500003 ' Folder name includes invalid characters in Device or PC
        Public Const ECONA_FOLDER_ALREADY_EXIST As Integer = &H80500004 ' Folder is already exists in target folder
        Public Const ECONA_FOLDER_NOT_FOUND As Integer = &H80500005 ' Folder does not exists in target folder
        Public Const ECONA_FOLDER_NO_PERMISSION As Integer = &H80500006 ' Not allowed to perform required operation to folder in Devic
        Public Const ECONA_FOLDER_NOT_EMPTY As Integer = &H80500007 ' Not allowed to perform required operation because folder is not empty
        Public Const ECONA_FOLDER_NO_PERMISSION_ON_PC As Integer = &H80500008 ' Not allowed to perform required operation to folder in PC

        ' Application installation error:
        Public Const ECONA_DEVICE_INSTALLER_BUSY As Integer = &H80600000 ' Cannot start Device's installer

        'Syncronization specific error codes :
        Public Const ECONA_UI_NOT_IDLE_DEVICE As Integer = &H80700000       ' Failed, device rejects the operation. Maybe device's UI was not IDLE-state.
        Public Const ECONA_SYNC_CLIENT_BUSY_DEVICE As Integer = &H80700001  ' Failed, device's SA sync client is busy.
        Public Const ECONA_UNAUTHORIZED_DEVICE As Integer = &H80700002      ' Failed, device rejects the operation. No permission.
        Public Const ECONA_DATABASE_LOCKED_DEVICE As Integer = &H80700003   ' Failed, device rejects the operation. Device is locked.
        Public Const ECONA_SETTINGS_NOT_OK_DEVICE As Integer = &H80700004   ' Failed, device rejects the operation. Maybe settings in Sync profile are wrong on Device.
        Public Const ECONA_SYNC_ITEM_TOO_BIG As Integer = &H80700501        ' 
        Public Const ECONA_SYNC_ITEM_REJECT As Integer = &H80700502         ' All commands,Device reject the operation...
        Public Const ECONA_SYNC_INSTALL_PLUGIN_FIRST As Integer = &H80700506 ' 

        ' Versit conversion specific error codes :			
        Public Const ECONA_VERSIT_INVALID_PARAM As Integer = &H80800001    ' Invalid parameters passed to versit converter 
        Public Const ECONA_VERSIT_UNKNOWN_TYPE As Integer = &H80800002    ' Failed, trying to convert versit formats not supported in VersitConverter
        Public Const ECONA_VERSIT_INVALID_VERSIT_OBJECT As Integer = &H80800003  ' Failed, validation of versit data not passed, contains invalid data

        ' Database specific error codes :
        Public Const ECONA_DB_TRANSACTION_ALREADY_STARTED As Integer = &H80800100 ' Another transaction is already in progress.
        Public Const ECONA_DB_TRANSACTION_FAILED As Integer = &H80800101        ' Some of operations within a transaction failed and transaction was rolled back.

        ' Backup specific error codes
        Public Const ECONA_DEVICE_BATTERY_LEVEL_TOO_LOW As Integer = &H80900000 ' Failed, device rejects the restore operation. Device's battery level is low.
        Public Const ECONA_DEVICE_BUSY As Integer = &H80900001                  ' Failed, device rejects the backup/resore operation. Device's backup server busy.

    End Class

End Class