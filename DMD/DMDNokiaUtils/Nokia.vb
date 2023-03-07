Option Strict Off
Option Explicit On

Imports System.Runtime.InteropServices
Imports DMD.Internals
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Public Class Nokia

    Public Enum SMSFolderTypes As Integer
        Root = CA_MESSAGE_FOLDER_ROOT
        InBox = CA_MESSAGE_FOLDER_INBOX
        OutBox = CA_MESSAGE_FOLDER_OUTBOX
        Sent = CA_MESSAGE_FOLDER_SENT
        Archive = CA_MESSAGE_FOLDER_ARCHIVE
        Drafts = CA_MESSAGE_FOLDER_DRAFTS
        Templates = CA_MESSAGE_FOLDER_TEMPLATES
        UserFolder = CA_MESSAGE_FOLDER_USER_FOLDERS
    End Enum

    Public Enum NokiaErrorCodes As Integer
        ''' <summary>
        ''' CONA_OK: Succeeded.
        ''' </summary>
        ''' <remarks></remarks>
        CONA_Ok = DMD.Nokia.APIS.CONA_OK

        ''' <summary>
        ''' CONA_OK_UPDATED_MEMORY_VALUES: Everything OK, given data is updated because (free, used and total) memory values are changed!
        ''' </summary>
        ''' <remarks></remarks>
        CONA_OK_UPDATED_MEMORY_VALUES = DMD.Nokia.APIS.CONA_OK_UPDATED_MEMORY_VALUES

        ''' <summary>
        ''' CONA_OK_UPDATED_MEMORY_AND_FILES: Everything OK, given data is updated because files and memory values are changed!
        ''' </summary>
        ''' <remarks></remarks>
        CONA_OK_UPDATED_MEMORY_AND_FILES = DMD.Nokia.APIS.CONA_OK_UPDATED_MEMORY_AND_FILES

        ''' <summary>
        ''' "CONA_OK_UPDATED: Everything OK, given data is updated, unknown reason.
        ''' </summary>
        ''' <remarks></remarks>
        CONA_OK_UPDATED = DMD.Nokia.APIS.CONA_OK_UPDATED

        ''' <summary>
        ''' CONA_OK_BUT_USER_ACTION_NEEDED: Operation needs some user action on Device
        ''' </summary>
        ''' <remarks></remarks>
        CONA_OK_BUT_USER_ACTION_NEEDED = DMD.Nokia.APIS.CONA_OK_BUT_USER_ACTION_NEEDED

        ''' <summary>
        ''' CONA_WAIT_CONNECTION_IS_BUSY: Operation started ok but other application has reserved the connection
        ''' </summary>
        ''' <remarks></remarks>
        CONA_WAIT_CONNECTION_IS_BUSY = DMD.Nokia.APIS.CONA_WAIT_CONNECTION_IS_BUSY

        ' Common error codes:

        ''' <summary>
        ''' ECONA_INIT_FAILED: DLL initialization failed.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_INIT_FAILED = DMD.Nokia.APIS.ECONA_INIT_FAILED

        ''' <summary>
        ''' ECONA_INIT_FAILED_COM_INTERFACE: Failed to get connection to system.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_INIT_FAILED_COM_INTERFACE = DMD.Nokia.APIS.ECONA_INIT_FAILED_COM_INTERFACE

        ''' <summary>
        ''' ECONA_NOT_INITIALIZED: API is not initialized.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_NOT_INITIALIZED = DMD.Nokia.APIS.ECONA_NOT_INITIALIZED

        ''' <summary>
        ''' ECONA_UNSUPPORTED_API_VERSION: API version not supported.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_UNSUPPORTED_API_VERSION = DMD.Nokia.APIS.ECONA_UNSUPPORTED_API_VERSION

        ''' <summary>
        ''' ECONA_NOT_SUPPORTED_MANUFACTURER: Manufacturer is not supported.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_NOT_SUPPORTED_MANUFACTURER = DMD.Nokia.APIS.ECONA_NOT_SUPPORTED_MANUFACTURER

        ''' <summary>
        ''' ECONA_UNKNOWN_ERROR: Failed, unknown error.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_UNKNOWN_ERROR = DMD.Nokia.APIS.ECONA_UNKNOWN_ERROR

        ''' <summary>
        ''' ECONA_UNKNOWN_ERROR_DEVICE: Failed, unknown error from device.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_UNKNOWN_ERROR_DEVICE = DMD.Nokia.APIS.ECONA_UNKNOWN_ERROR_DEVICE

        ''' <summary>
        ''' ECONA_INVALID_POINTER: Required pointer is invalid.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_INVALID_POINTER = DMD.Nokia.APIS.ECONA_INVALID_POINTER

        ''' <summary>
        ''' ECONA_INVALID_PARAMETER: Invalid parameter value.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_INVALID_PARAMETER = DMD.Nokia.APIS.ECONA_INVALID_PARAMETER

        ''' <summary>
        ''' ECONA_INVALID_HANDLE: Invalid handle.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_INVALID_HANDLE = DMD.Nokia.APIS.ECONA_INVALID_HANDLE

        ''' <summary>
        ''' ECONA_NOT_ENOUGH_MEMORY: Memory allocation failed in PC.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_NOT_ENOUGH_MEMORY = DMD.Nokia.APIS.ECONA_NOT_ENOUGH_MEMORY

        ''' <summary>
        ''' ECONA_WRONG_THREAD: Failed, called interface was marshalled for a different thread.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_WRONG_THREAD = DMD.Nokia.APIS.ECONA_WRONG_THREAD

        ''' <summary>
        ''' ECONA_REGISTER_ALREADY_DONE: Failed, notification interface is already registered.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_REGISTER_ALREADY_DONE = DMD.Nokia.APIS.ECONA_REGISTER_ALREADY_DONE

        ''' <summary>
        ''' ECONA_CANCELLED: Operation cancelled by ConnectivityAPI-User.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_CANCELLED = DMD.Nokia.APIS.ECONA_CANCELLED

        ''' <summary>
        ''' ECONA_NOTHING_TO_CANCEL: No running functions.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_NOTHING_TO_CANCEL = DMD.Nokia.APIS.ECONA_NOTHING_TO_CANCEL

        ''' <summary>
        ''' ECONA_FAILED_TIMEOUT: Operation failed because of timeout.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FAILED_TIMEOUT = DMD.Nokia.APIS.ECONA_FAILED_TIMEOUT

        ''' <summary>
        ''' ECONA_NOT_SUPPORTED_DEVICE: Device does not support operation.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_NOT_SUPPORTED_DEVICE = DMD.Nokia.APIS.ECONA_NOT_SUPPORTED_DEVICE

        ''' <summary>
        ''' ECONA_NOT_SUPPORTED_PC: Connectivity API does not support operation (not implemented).
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_NOT_SUPPORTED_PC = DMD.Nokia.APIS.ECONA_NOT_SUPPORTED_PC

        ''' <summary>
        ''' ECONA_NOT_FOUND: Item was not found
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_NOT_FOUND = DMD.Nokia.APIS.ECONA_NOT_FOUND

        ''' <summary>
        ''' ECONA_FAILED: The called operation failed.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FAILED = DMD.Nokia.APIS.ECONA_FAILED

        ''' <summary>
        ''' ECONA_API_NOT_FOUND: Needed API module was not found from the system
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_API_NOT_FOUND = DMD.Nokia.APIS.ECONA_API_NOT_FOUND

        ''' <summary>
        ''' ECONA_API_FUNCTION_NOT_FOUND: Called API function was not found from the loaded API module
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_API_FUNCTION_NOT_FOUND = DMD.Nokia.APIS.ECONA_API_FUNCTION_NOT_FOUND


        ' Device manager and device connection related errors:

        ''' <summary>
        ''' ECONA_DEVICE_NOT_FOUND: Given phone is not connected (refresh device list).
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_DEVICE_NOT_FOUND = DMD.Nokia.APIS.ECONA_DEVICE_NOT_FOUND

        ''' <summary>
        ''' ECONA_NO_CONNECTION_VIA_MEDIA: Phone is connected but not via given media.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_NO_CONNECTION_VIA_MEDIA = DMD.Nokia.APIS.ECONA_NO_CONNECTION_VIA_MEDIA

        ''' <summary>
        ''' ECONA_NO_CONNECTION_VIA_DEVID: Phone is not connected with given DevID.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_NO_CONNECTION_VIA_DEVID = DMD.Nokia.APIS.ECONA_NO_CONNECTION_VIA_DEVID

        ''' <summary>
        ''' ECONA_INVALID_CONNECTION_TYPE: Connection type was invalid.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_INVALID_CONNECTION_TYPE = DMD.Nokia.APIS.ECONA_INVALID_CONNECTION_TYPE

        ''' <summary>
        ''' ECONA_NOT_SUPPORTED_CONNECTION_TYPE: Device does not support connection type.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_NOT_SUPPORTED_CONNECTION_TYPE = DMD.Nokia.APIS.ECONA_NOT_SUPPORTED_CONNECTION_TYPE

        ''' <summary>
        ''' ECONA_CONNECTION_BUSY: Other application has reserved connection.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_CONNECTION_BUSY = DMD.Nokia.APIS.ECONA_CONNECTION_BUSY

        ''' <summary>
        ''' ECONA_CONNECTION_LOST: Connection lost to device.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_CONNECTION_LOST = DMD.Nokia.APIS.ECONA_CONNECTION_LOST

        ''' <summary>
        ''' ECONA_CONNECTION_REMOVED: Connection removed, other application has reserved connection.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_CONNECTION_REMOVED = DMD.Nokia.APIS.ECONA_CONNECTION_REMOVED

        ''' <summary>
        ''' ECONA_CONNECTION_FAILED: Connection failed, unknown reason.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_CONNECTION_FAILED = DMD.Nokia.APIS.ECONA_CONNECTION_FAILED

        ''' <summary>
        ''' ECONA_SUSPEND: Connection removed, PC goes to standby state.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_SUSPEND = DMD.Nokia.APIS.ECONA_SUSPEND

        ''' <summary>
        ''' ECONA_NAME_ALREADY_EXISTS: Friendly name already exists.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_NAME_ALREADY_EXISTS = DMD.Nokia.APIS.ECONA_NAME_ALREADY_EXISTS

        ''' <summary>
        ''' ECONA_MEDIA_IS_NOT_WORKING: Target media is active but it is not working (e.g. BT hardware stopped or removed).
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_MEDIA_IS_NOT_WORKING = DMD.Nokia.APIS.ECONA_MEDIA_IS_NOT_WORKING

        ''' <summary>
        ''' ECONA_CACHE_IS_NOT_AVAILABLE: Cache is not available (CONASearchDevices).
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_CACHE_IS_NOT_AVAILABLE = DMD.Nokia.APIS.ECONA_CACHE_IS_NOT_AVAILABLE

        ''' <summary>
        ''' ECONA_MEDIA_IS_NOT_ACTIVE: Target media is busy (or not ready yet).
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_MEDIA_IS_NOT_ACTIVE = DMD.Nokia.APIS.ECONA_MEDIA_IS_NOT_ACTIVE

        ''' <summary>
        ''' ECONA_PORT_OPEN_FAILED: Cannot open the changed COM port.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_PORT_OPEN_FAILED = DMD.Nokia.APIS.ECONA_PORT_OPEN_FAILED

        ' Device pairing related errors:

        ''' <summary>
        ''' ECONA_DEVICE_PAIRING_FAILED: Pairing failed.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_DEVICE_PAIRING_FAILED = DMD.Nokia.APIS.ECONA_DEVICE_PAIRING_FAILED

        ''' <summary>
        ''' ECONA_DEVICE_PASSWORD_WRONG: Wrong password on device.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_DEVICE_PASSWORD_WRONG = DMD.Nokia.APIS.ECONA_DEVICE_PASSWORD_WRONG

        ''' <summary>
        ''' ECONA_DEVICE_PASSWORD_INVALID: Password includes invalid characters or is missing.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_DEVICE_PASSWORD_INVALID = DMD.Nokia.APIS.ECONA_DEVICE_PASSWORD_INVALID

        ' File System errors:

        ''' <summary>
        ''' ECONA_ALL_LISTED: All items are listed.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_ALL_LISTED = DMD.Nokia.APIS.ECONA_ALL_LISTED

        ''' <summary>
        ''' ECONA_MEMORY_FULL: Device memory full.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_MEMORY_FULL = DMD.Nokia.APIS.ECONA_MEMORY_FULL


        ' File System errors for file functions:

        ''' <summary>
        ''' ECONA_FILE_NAME_INVALID: File name contains invalid characters in Device or PC.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FILE_NAME_INVALID = DMD.Nokia.APIS.ECONA_FILE_NAME_INVALID

        ''' <summary>
        ''' ECONA_FILE_NAME_TOO_LONG: File name contains too many characters in Device or PC.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FILE_NAME_TOO_LONG = DMD.Nokia.APIS.ECONA_FILE_NAME_TOO_LONG

        ''' <summary>
        ''' ECONA_FILE_ALREADY_EXIST: File already exists in Device or PC.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FILE_ALREADY_EXIST = DMD.Nokia.APIS.ECONA_FILE_ALREADY_EXIST

        ''' <summary>
        ''' ECONA_FILE_NOT_FOUND: File does not exist in Device or PC.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FILE_NOT_FOUND = DMD.Nokia.APIS.ECONA_FILE_NOT_FOUND

        ''' <summary>
        ''' ECONA_FILE_NO_PERMISSION: Not allowed to perform required operation to file in device or PC.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FILE_NO_PERMISSION = DMD.Nokia.APIS.ECONA_FILE_NO_PERMISSION

        ''' <summary>
        ''' ECONA_FILE_COPYRIGHT_PROTECTED: Not allowed to perform required operation to file in device or PC.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FILE_COPYRIGHT_PROTECTED = DMD.Nokia.APIS.ECONA_FILE_COPYRIGHT_PROTECTED

        ''' <summary>
        ''' ECONA_FILE_BUSY: Other application has reserved file in device or PC.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FILE_BUSY = DMD.Nokia.APIS.ECONA_FILE_BUSY

        ''' <summary>
        ''' ECONA_FILE_TOO_BIG_DEVICE: Device rejected the operation because file size is too big.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FILE_TOO_BIG_DEVICE = DMD.Nokia.APIS.ECONA_FILE_TOO_BIG_DEVICE

        ''' <summary>
        ''' ECONA_FILE_TYPE_NOT_SUPPORTED: Device rejected the operation because file has unsupported type.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FILE_TYPE_NOT_SUPPORTED = DMD.Nokia.APIS.ECONA_FILE_TYPE_NOT_SUPPORTED

        ''' <summary>
        ''' ECONA_FILE_NO_PERMISSION_ON_PC: Not allowed to perform required operation to file in PC.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FILE_NO_PERMISSION_ON_PC = DMD.Nokia.APIS.ECONA_FILE_NO_PERMISSION_ON_PC

        ''' <summary>
        ''' ECONA_FILE_EXIST: File move or rename: File is copied to target path with new name but removing the source file failed.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FILE_EXIST = DMD.Nokia.APIS.ECONA_FILE_EXIST

        ''' <summary>
        ''' ECONA_FILE_CONTENT_NOT_FOUND: Specified file content does not found (e.g. unknown file section or stored position)
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FILE_CONTENT_NOT_FOUND = DMD.Nokia.APIS.ECONA_FILE_CONTENT_NOT_FOUND

        ''' <summary>
        ''' ECONA_FILE_OLD_FORMAT: Specified file content supports old engine
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FILE_OLD_FORMAT = DMD.Nokia.APIS.ECONA_FILE_OLD_FORMAT

        ''' <summary>
        ''' ECONA_FILE_INVALID_DATA: Specified file data is invalid
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FILE_INVALID_DATA = DMD.Nokia.APIS.ECONA_FILE_INVALID_DATA

        ' File System errors for folder functions:

        ''' <summary>
        ''' ECONA_INVALID_DATA_DEVICE: Device's folder contains invalid data.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_INVALID_DATA_DEVICE = DMD.Nokia.APIS.ECONA_INVALID_DATA_DEVICE

        ''' <summary>
        ''' ECONA_CURRENT_FOLDER_NOT_FOUND: Current folder is invalid in device (e.g MMC card removed).
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_CURRENT_FOLDER_NOT_FOUND = DMD.Nokia.APIS.ECONA_CURRENT_FOLDER_NOT_FOUND

        ''' <summary>
        ''' ECONA_FOLDER_PATH_TOO_LONG: Maximum folder path length is 255 characters.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FOLDER_PATH_TOO_LONG = DMD.Nokia.APIS.ECONA_FOLDER_PATH_TOO_LONG

        ''' <summary>
        ''' ECONA_FOLDER_NAME_INVALID: Folder name includes invalid characters in Device or PC.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FOLDER_NAME_INVALID = DMD.Nokia.APIS.ECONA_FOLDER_NAME_INVALID

        ''' <summary>
        ''' ECONA_FOLDER_ALREADY_EXIST: Folder already exists in target folder.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FOLDER_ALREADY_EXIST = DMD.Nokia.APIS.ECONA_FOLDER_ALREADY_EXIST

        ''' <summary>
        ''' ECONA_FOLDER_NOT_FOUND: Folder does not exist in target folder.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FOLDER_NOT_FOUND = DMD.Nokia.APIS.ECONA_FOLDER_NOT_FOUND

        ''' <summary>
        ''' ECONA_FOLDER_NO_PERMISSION: Not allowed to perform required operation to folder in Device.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FOLDER_NO_PERMISSION = DMD.Nokia.APIS.ECONA_FOLDER_NO_PERMISSION

        ''' <summary>
        ''' ECONA_FOLDER_NOT_EMPTY: Not allowed to perform required operation because folder is not empty.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FOLDER_NOT_EMPTY = DMD.Nokia.APIS.ECONA_FOLDER_NOT_EMPTY

        ''' <summary>
        ''' ECONA_FOLDER_NO_PERMISSION_ON_PC: Not allowed to perform required operation to folder in PC.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_FOLDER_NO_PERMISSION_ON_PC = DMD.Nokia.APIS.ECONA_FOLDER_NO_PERMISSION_ON_PC

        ' Application installation error:

        ''' <summary>
        ''' ECONA_DEVICE_INSTALLER_BUSY: Cannot start device's installer.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_DEVICE_INSTALLER_BUSY = DMD.Nokia.APIS.ECONA_DEVICE_INSTALLER_BUSY

        ' Syncronization specific error codes:

        ''' <summary>
        ''' ECONA_UI_NOT_IDLE_DEVICE: Device rejects the operation. Maybe device's UI is not in idle state.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_UI_NOT_IDLE_DEVICE = DMD.Nokia.APIS.ECONA_UI_NOT_IDLE_DEVICE

        ''' <summary>
        ''' ECONA_SYNC_CLIENT_BUSY_DEVICE: Device's SA sync client is busy.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_SYNC_CLIENT_BUSY_DEVICE = DMD.Nokia.APIS.ECONA_SYNC_CLIENT_BUSY_DEVICE

        ''' <summary>
        ''' ECONA_UNAUTHORIZED_DEVICE: Device rejects the operation. No permission.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_UNAUTHORIZED_DEVICE = DMD.Nokia.APIS.ECONA_UNAUTHORIZED_DEVICE

        ''' <summary>
        ''' ECONA_DATABASE_LOCKED_DEVICE: Device rejects the operation. Device is locked.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_DATABASE_LOCKED_DEVICE = DMD.Nokia.APIS.ECONA_DATABASE_LOCKED_DEVICE

        ''' <summary>
        ''' ECONA_SETTINGS_NOT_OK_DEVICE: Device rejects the operation. Maybe settings in Sync profile are wrong on Device.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_SETTINGS_NOT_OK_DEVICE = DMD.Nokia.APIS.ECONA_SETTINGS_NOT_OK_DEVICE

        ''' <summary>
        ''' ECONA_SYNC_ITEM_TOO_BIG: Device rejected the operation
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_SYNC_ITEM_TOO_BIG = DMD.Nokia.APIS.ECONA_SYNC_ITEM_TOO_BIG

        ''' <summary>
        ''' ECONA_SYNC_ITEM_REJECT: Device rejected the operation
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_SYNC_ITEM_REJECT = DMD.Nokia.APIS.ECONA_SYNC_ITEM_REJECT

        ''' <summary>
        ''' ECONA_SYNC_INSTALL_PLUGIN_FIRST: Device rejected the operation
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_SYNC_INSTALL_PLUGIN_FIRST = DMD.Nokia.APIS.ECONA_SYNC_INSTALL_PLUGIN_FIRST

        ' Versit conversion specific error codes:			

        ''' <summary>
        ''' ECONA_VERSIT_INVALID_PARAM: Invalid parameters passed to versit converter.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_VERSIT_INVALID_PARAM = DMD.Nokia.APIS.ECONA_VERSIT_INVALID_PARAM

        ''' <summary>
        ''' ECONA_VERSIT_UNKNOWN_TYPE: Failed, trying to convert versit formats not supported in VersitConverter.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_VERSIT_UNKNOWN_TYPE = DMD.Nokia.APIS.ECONA_VERSIT_UNKNOWN_TYPE

        ''' <summary>
        ''' ECONA_VERSIT_INVALID_VERSIT_OBJECT: Failed, validation of versit data not passed, contains invalid data.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_VERSIT_INVALID_VERSIT_OBJECT = DMD.Nokia.APIS.ECONA_VERSIT_INVALID_VERSIT_OBJECT

        ' Database specific error codes:

        ''' <summary>
        ''' Another transaction is already in progress
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_DB_TRANSACTION_ALREADY_STARTED = DMD.Nokia.APIS.ECONA_DB_TRANSACTION_ALREADY_STARTED

        ''' <summary>
        ''' Some of operations within a transaction failed and transaction was rolled back
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_DB_TRANSACTION_FAILED = DMD.Nokia.APIS.ECONA_DB_TRANSACTION_FAILED

        ' Backup specific error codes:

        ''' <summary>
        ''' Failed, device rejects the restore operation. Device's battery level is low.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_DEVICE_BATTERY_LEVEL_TOO_LOW = DMD.Nokia.APIS.ECONA_DEVICE_BATTERY_LEVEL_TOO_LOW

        ''' <summary>
        ''' Failed, device rejects the backup/resore operation. Device's backup server busy.
        ''' </summary>
        ''' <remarks></remarks>
        ECONA_DEVICE_BUSY = DMD.Nokia.APIS.ECONA_DEVICE_BUSY

    End Enum

    ''' <summary>
    ''' Parametri per gli eventi relativi ai Device
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class DeviceEventArgs
        Inherits System.EventArgs

        Private m_Device As NokiaDevice

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Device = Nothing
        End Sub

        Public Sub New(ByVal device As NokiaDevice)
            Me.New
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            Me.m_Device = device
        End Sub

        Public ReadOnly Property Device As NokiaDevice
            Get
                Return Me.m_Device
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

    ''' <summary>
    ''' Eccezione
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class NokiaException
        Inherits System.Exception

        Private m_Code As Integer
        Private m_Message As String

        Public Sub New(ByVal code As NokiaErrorCodes, ByVal strError As String)
            MyBase.New(CONAError2String(code) & " (0x" & Hex(code) & ") in " & strError)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Code = code
        End Sub

        ''' <summary>
        ''' Restituisce il codice di errore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ErrorCode As NokiaErrorCodes
            Get
                Return Me.m_Code
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

    ''' <summary>
    ''' Informazioni su un'operazione asincrona
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ProgressEventArgs
        Inherits System.EventArgs

        Private m_Progress As Integer
        Private m_Cancel As Boolean

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Progress = 0
            Me.m_Cancel = False
        End Sub

        Public Sub New(ByVal progress As Integer)
            Me.New()
            Me.m_Progress = progress
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Valore compreso tra 0 e 100 che indica la percentuale di completamento dell'operazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Progress As Integer
            Get
                Return Me.m_Progress
            End Get
        End Property


        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica al sistema se annullare la ricerca
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cancel As Boolean
            Get
                Return Me.m_Cancel
            End Get
            Set(value As Boolean)
                Me.m_Cancel = value
            End Set
        End Property

    End Class

    Public Enum FileOperationTypes As Integer
        Create
        Delete
        Rename
        Move
        Copy
    End Enum

    Public Class NokiaFileOperationEventArgs
        Inherits ProgressEventArgs

        Private m_Operation As FileOperationTypes
        Private m_SourceName As String
        Private m_TargetName As String

        Public Sub New()

        End Sub

        Public Sub New(ByVal op As FileOperation)
            MyBase.New(op.Percentage)
            Me.m_Operation = op.Type
            Me.m_SourceName = op.SourceName
            Me.m_TargetName = op.TargetName
        End Sub

        Public ReadOnly Property Operation As FileOperationTypes
            Get
                Return Me.m_Operation
            End Get
        End Property

        Public ReadOnly Property SourceName As String
            Get
                Return Me.m_SourceName
            End Get
        End Property

        Public ReadOnly Property TargetName As String
            Get
                Return Me.m_TargetName
            End Get
        End Property

    End Class

    ''' <summary>
    ''' Evento generato quando viene collegata una periferica 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Shared Event DeviceAdded(ByVal sender As Object, ByVal e As DeviceEventArgs)

    ''' <summary>
    ''' Evento generato quando viene rimossa una periferica 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Shared Event DeviceRemoved(ByVal sender As Object, ByVal e As DeviceEventArgs)


    ''' <summary>
    ''' Evento generato dalla ricerca delle periferiche bluetooth per informare il sistema della percentuale di completamento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Shared Event BTFindProgress(ByVal sender As Object, ByVal e As ProgressEventArgs)


    Private Delegate Sub InsertNotificationDelegate(ByVal strNotification As String)



    Private Shared pDeviceCallBack As DeviceNotifyCallbackDelegate      'Callback per le notifiche relative alle periferiche attaccate o rimosse
    Private Shared m_pfnSearchNotify As SearchCallbackDelegate          'Callback per le notifiche relative alla ricerca delle periferiche bluetooth
    Private Shared m_Devices As CDevicesCollection                      'Collezione di periferiche trovate
    Private Shared m_hDMHandle As Integer = 0                           'Device manager handle

    Private Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Public Shared Function GetHandle() As Integer
        CheckInitialized()
        Return m_hDMHandle
    End Function

    Public Shared ReadOnly Property Devices As CDevicesCollection
        Get
            CheckInitialized()
            Return m_Devices
        End Get
    End Property

    Private Shared Sub CheckInitialized()
        If m_hDMHandle = 0 Then Initialize()
    End Sub


    Public Shared Function GetInstalledVersion() As Version
        CheckInitialized()
        Dim ver As Integer = DMAPI_GetAPIVersion()
        Return New Version(ver, 0)
    End Function

    '===================================================================
    ' PIMNavigator_Load
    '
    ' Initialization of PIM Navigator dialog
    '
    '===================================================================
    Friend Shared Sub Initialize()
        m_pfnSearchNotify = AddressOf BTPairingNotifyCallback

        ' Initialize Device Management APi
        Dim iResult As Integer = DMAPI_Initialize(DMAPI_VERSION_32, vbNullString)
        ' Initialize Data Access API
        iResult = CAAPI_Initialize(CAAPI_VERSION_30, Nothing)

        ' Get Device management handle
        iResult = CONAOpenDM(m_hDMHandle)
        If iResult <> CONA_OK Then ShowErrorMessage("CONAOpenDM", iResult)

        ' Register device notification callback function
        pDeviceCallBack = AddressOf DeviceNotifyCallback
        iResult = CONARegisterNotifyCallback(m_hDMHandle, API_REGISTER, pDeviceCallBack)
        If iResult <> CONA_OK Then ShowErrorMessage("CONARegisterNotifyCallback", iResult)

        If m_hDMHandle = 0 Then Return

        Dim iRet As Integer = CONARefreshDeviceList(m_hDMHandle, 0)
        If iRet <> CONA_OK Then ShowErrorMessage("CONARefreshDeviceList", iRet)

        ' Dim pDevices() As CONAPI_DEVICE
        m_Devices = New CDevicesCollection
        Dim iDeviceCount As Integer = 0
        iRet = CONAGetDeviceCount(m_hDMHandle, iDeviceCount)
        If iRet <> CONA_OK Then ShowErrorMessage("CONAGetDeviceCount", iRet)
        If iRet = CONA_OK And iDeviceCount > 0 Then
            ' Allocate memory for buffer
            Dim buffer As IntPtr = Marshal.AllocHGlobal(iDeviceCount * Marshal.SizeOf(GetType(CONAPI_DEVICE)))
            ' Get list of currently connected devices
            iRet = CONAGetDevices(m_hDMHandle, iDeviceCount, buffer)
            If iRet <> CONA_OK Then
                ShowErrorMessage("CONAGetDevices", iRet)
            Else
                ' Add each device to the tree view
                For iDeviceIndex As Integer = 0 To iDeviceCount - 1
                    ' Calculate beginning of CONAPI_DEVICE structure of item 'i'
                    Dim iPtr As Int64 = buffer.ToInt64 + iDeviceIndex * Marshal.SizeOf(GetType(CONAPI_DEVICE))
                    ' Convert integer to pointer
                    Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
                    ' Copy data from buffer
                    Dim device As New NokiaDevice
                    device.InitializeFromPtr(ptr)
                    m_Devices.Add(device)
                Next
            End If
            Marshal.FreeHGlobal(buffer) 'FORSE POSSIAMO???
        End If
    End Sub


    ''===================================================================
    '' Destructor
    ''
    ''===================================================================
    'Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    '    Dim ret As Integer

    '    If Not bDisposed Then
    '        If m_hDMHandle <> 0 Then
    '            ' Unregister device notification callback function
    '            ret = CONARegisterNotifyCallback(m_hDMHandle, API_UNREGISTER, pDeviceCallBack)
    '            If ret <> CONA_OK Then ShowErrorMessage("CONARegisterNotifyCallback", ret)
    '            ' Close device management handle
    '            ret = CONACloseDM(m_hDMHandle)
    '            If ret <> CONA_OK Then ShowErrorMessage("CONACloseDM", ret)
    '        End If
    '        ' Terminate Device Management APi
    '        Dim iRet As Integer = DMAPI_Terminate(0)
    '        If iRet <> CONA_OK Then ShowErrorMessage("DMAPI_Terminate", iRet)

    '        MyBase.Dispose(disposing)
    '    End If
    '    bDisposed = True
    'End Sub

    Public Shared Sub Terminate()
        If m_Devices IsNot Nothing Then
            For Each device As NokiaDevice In m_Devices
                device.Dispose()
            Next
            m_Devices = Nothing
        End If

        Dim iResult As Integer
        If (m_hDMHandle <> 0) Then
            ' Unregister device notification callback function
            iResult = CONARegisterNotifyCallback(m_hDMHandle, API_UNREGISTER, pDeviceCallBack)
            If iResult <> CONA_OK Then ShowErrorMessage("CONARegisterNotifyCallback", iResult)

            ' Close device management handle
            iResult = CONACloseDM(m_hDMHandle)
            If iResult <> CONA_OK Then ShowErrorMessage("CONACloseDM", iResult)

            m_hDMHandle = 0
        End If

        ' Uninitialize Content Access API
        iResult = CAAPI_Terminate(0)
        If iResult <> CONA_OK Then ShowErrorMessage("CONAUninitialize", iResult)

        ' Uninitialize Device Management API
        iResult = DMAPI_Terminate(0)
    End Sub

     

    '===================================================================
    '
    ' Structure to map CA_FOLDER_INFO "permanently" into managed memory 
    ' 
    '===================================================================
    Private Structure CAFolderInfo
        Dim iFolderId As Integer
        Dim iOptions As Integer
        Dim strName As String
        Dim strPath As String
        Dim iSubfolderCount As Boolean
        Dim bIsChild As Boolean
    End Structure

    '===================================================================
    ' MapCFIToCAFolderInfo
    ' 
    ' Maps CA_FOLDER_INFO structure to CaFolderInfo structure
    '   NOTE: Mapping is simple, parent and child folder info is lost
    ' 
    '===================================================================
    Private Function MapCFIToCAFolderInfo(ByVal CFI As CA_FOLDER_INFO) As CAFolderInfo
        MapCFIToCAFolderInfo.iFolderId = CFI.iFolderId
        MapCFIToCAFolderInfo.iOptions = CFI.iOptions
        MapCFIToCAFolderInfo.strName = CFI.pstrName
        MapCFIToCAFolderInfo.strPath = CFI.pstrPath
        MapCFIToCAFolderInfo.iSubfolderCount = CFI.iSubFolderCount
        MapCFIToCAFolderInfo.bIsChild = (CFI.pParent <> IntPtr.Zero)
        Return MapCFIToCAFolderInfo
    End Function

    '===================================================================
    ' MapCAFolderInfoToCFI
    ' 
    ' Maps CaFolderInfo structure to CA_FOLDER_INFO structure
    '   NOTE: Mapping is simple, parent and child folder info is missing
    ' 
    '===================================================================
    Private Function MapCAFolderInfoToCFI(ByVal caFolder As CAFolderInfo) As CA_FOLDER_INFO
        MapCAFolderInfoToCFI.iSize = Marshal.SizeOf(GetType(CA_FOLDER_INFO))
        MapCAFolderInfoToCFI.iFolderId = caFolder.iFolderId
        MapCAFolderInfoToCFI.iOptions = caFolder.iOptions
        MapCAFolderInfoToCFI.pstrName = caFolder.strName
        MapCAFolderInfoToCFI.pstrPath = caFolder.strPath
        MapCAFolderInfoToCFI.iSubFolderCount = 0
        MapCAFolderInfoToCFI.pSubFolders = IntPtr.Zero
        MapCAFolderInfoToCFI.pParent = IntPtr.Zero
    End Function

    '===================================================================
    '
    ' Structure to map CA_ITEM_ID "permanently" into managed memory 
    ' 
    '===================================================================
    Friend Structure CAItemID
        Dim iFolderId As Integer
        Dim iTemporaryID As Integer
        Dim abUID() As Byte
        Dim iStatus As Integer



        ''===================================================================
        '' MapCAItemIDToUID
        '' 
        '' Maps CaItemID structure to CA_ITEM_ID structure
        '' Remember to free allocated memory after use (FreeUIDMappingMemory).
        '' 
        ''===================================================================
        Public Shared Narrowing Operator CType(ByVal item As CAItemID) As CA_ITEM_ID
            Dim ret As New CA_ITEM_ID
            ret.iSize = Marshal.SizeOf(GetType(CA_ITEM_ID))
            ret.iFolderId = item.iFolderId
            ret.iTemporaryID = item.iTemporaryID
            ret.iStatus = item.iStatus
            If item.abUID Is Nothing Then
                ret.iUidLen = 0
                ret.pbUid = IntPtr.Zero
            Else
                Dim iSize As Integer = item.abUID.GetUpperBound(0) - item.abUID.GetLowerBound(0)
                ret.iUidLen = iSize
                ret.pbUid = Marshal.AllocHGlobal(iSize)
                Marshal.Copy(item.abUID, 0, ret.pbUid, iSize)
            End If
            Return ret
        End Operator




        Public Shared Widening Operator CType(ByVal UID As CA_ITEM_ID) As CAItemID
            Dim ret As New CAItemID
            ret.iFolderId = UID.iFolderId
            ret.iTemporaryID = UID.iTemporaryID
            ret.iStatus = UID.iStatus
            If UID.iUidLen > 0 Then
                ReDim ret.abUID(UID.iUidLen)
                Marshal.Copy(UID.pbUid, ret.abUID, 0, UID.iUidLen)
            Else
                ret.abUID = Nothing
            End If
            Return ret
        End Operator


    End Structure

    





    '===================================================================
    ' DeviceNotifyCallback
    '
    ' Callback function for device connection notifications
    '
    '===================================================================
    Private Shared Function DeviceNotifyCallback(ByVal iStatus As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrSerialNumber As String) As Integer
        SyncLock Devices
            Dim dev As NokiaDevice
            ' Refresh tree view after next timer tick
            Select Case GET_CONAPI_CB_STATUS(iStatus)
                Case CONAPI_DEVICE_LIST_UPDATED ' List is updated. No any specific information.

                Case CONAPI_DEVICE_ADDED ' A new device is added to the list.
                    dev = Devices.GetItemBySerialNumber(pstrSerialNumber)
                    If (dev IsNot Nothing) Then Throw New Exception("Errore del driver: periferica già aggiunta")
                    dev = New Nokia.NokiaDevice
                    dev.FromSerialNumber(pstrSerialNumber)
                    Devices.Add(dev)
                    RaiseEvent DeviceAdded(Nothing, New DeviceEventArgs(dev))
                Case CONAPI_DEVICE_REMOVED  ' Device is removed from the list.
                    dev = Devices.GetItemBySerialNumber(pstrSerialNumber)
                    If (dev IsNot Nothing) Then
                        Devices.Remove(dev)
                        RaiseEvent DeviceRemoved(Nothing, New DeviceEventArgs(dev))
                    End If
                Case CONAPI_DEVICE_UPDATED ' Device is updated. A connection is added or removed
                    Debug.Print("'??")
            End Select
            Return CONA_OK
        End SyncLock
    End Function


     


    '





    ''===================================================================
    '' BTN_Save_Click
    ''
    '' User has clicked "Save to File" button
    ''
    ''===================================================================
    'Private Sub BTN_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_Save.Click
    '    Dim strFilter As String
    '    If TVW_Navigator.SelectedNode.ImageIndex = m_iIconContactIndex Then
    '        strFilter = "vCard files (*.vcf)|*.vcf||"
    '    ElseIf TVW_Navigator.SelectedNode.ImageIndex = m_iIconSMSIndex Then
    '        strFilter = "vMessage files (*.vmg)|*.vmg||"
    '    ElseIf TVW_Navigator.SelectedNode.ImageIndex = m_iIconBookmarkItemIndex Then
    '        strFilter = "vBookmark files (*.vbk)|*.vbk||"
    '    ElseIf TVW_Navigator.SelectedNode.ImageIndex = m_iIconCalendarItemIndex Then
    '        strFilter = "vCalendar files (*.vcs)|*.vcs||"
    '    ElseIf TVW_Navigator.SelectedNode.ImageIndex = m_iIconMMSIndex Then
    '        strFilter = "MMS files (*.mms)|*.mms||"
    '    Else
    '        Return
    '    End If
    '    Dim fileDlg As New System.Windows.Forms.SaveFileDialog
    '    fileDlg.Filter = strFilter
    '    If fileDlg.ShowDialog = Windows.Forms.DialogResult.OK Then
    '        Dim hOperHandle As Integer
    '        Dim iRet As Integer = CABeginOperation(m_hCurrentConnection, 0, hOperHandle)
    '        If iRet <> CONA_OK Then
    '            ShowErrorMessage("CABeginOperation", iRet)
    '        End If
    '        ' Read contact item data from device
    '        Dim UID As CA_ITEM_ID = MapCAItemIDToUID(TVW_Navigator.SelectedNode.Tag)
    '        Dim bufId As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(UID))
    '        Marshal.StructureToPtr(UID, bufId, True)
    '        Dim dataVersit As CA_DATA_VERSIT
    '        dataVersit.iSize = Marshal.SizeOf(dataVersit)
    '        Dim bufData As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(dataVersit))
    '        Marshal.StructureToPtr(dataVersit, bufData, True)
    '        iRet = CAReadItem(hOperHandle, bufId, CA_OPTION_USE_CACHE, CA_DATA_FORMAT_VERSIT, bufData)
    '        If iRet = CONA_OK Then
    '            dataVersit = Marshal.PtrToStructure(bufData, GetType(CA_DATA_VERSIT))
    '            Dim bVersitObject As Byte()
    '            ReDim bVersitObject(dataVersit.iDataLength)
    '            Marshal.Copy(dataVersit.pbVersitObject, bVersitObject, 0, dataVersit.iDataLength)
    '            Dim iFr As Short
    '            iFr = FreeFile()
    '            FileOpen(iFr, fileDlg.FileName, OpenMode.Binary)
    '            FilePut(iFr, bVersitObject)
    '            FileClose(iFr)
    '        Else
    '            ShowErrorMessage("CAReadItem", iRet)
    '        End If
    '        Marshal.FreeHGlobal(bufId)
    '        Marshal.FreeHGlobal(bufData)
    '        iRet = CAEndOperation(hOperHandle)
    '        If iRet <> CONA_OK Then
    '            ShowErrorMessage("CAEndOperation", iRet)
    '        End If
    '        FreeUIDMappingMemory(UID)
    '    End If
    'End Sub


    ''===================================================================
    '' ShowNotification
    ''
    '' Asynchronously inserts notification text to Notifications dialog 
    ''
    ''===================================================================
    'Public Sub ShowNotification(ByVal strNotification As String)
    '    If NotificationsDialog Is Nothing Then
    '        Exit Sub
    '    End If
    '    If NotificationsDialog.IsDisposed Then
    '        Exit Sub
    '    End If
    '    ' Insert text to Notifications dialog asynchronously, so that UI is not blocked
    '    BeginInvoke(New InsertNotificationDelegate(AddressOf NotificationsDialog.InsertNotification), New Object() {strNotification})
    'End Sub



      
    Public Shared Function GetBluetoothDevices(Optional ByVal timeoutSeconds As Integer = 240) As NokiaBTDevice()
        CheckInitialized()

        ' This is the maximum time to be spent to the device search, in seconds.
        ' The required time may vary a lot, make sure it is big enough. If this
        ' timeout value is too small, CONASearchDevices returns ECONA_FAILED_TIMEOUT,
        ' and PC Suite Connectivity API doesn't find all available devices.
        'Const iMaxSearchTime As Integer = 240

        Dim iRet As Integer = ECONA_UNKNOWN_ERROR
        Dim iDeviceCount As Integer = 0
        Dim i As Integer = 0
        Dim pInfo As IntPtr
        'Dim Info() As CONAPI_CONNECTION_INFO

        iRet = CONASearchDevices(m_hDMHandle, API_MEDIA_BLUETOOTH Or CONAPI_GET_ALL_PHONES, timeoutSeconds, m_pfnSearchNotify, iDeviceCount, pInfo)

        Dim ret As New System.Collections.ArrayList

        For i = 0 To iDeviceCount - 1
            ' Calculate beginning of CONAPI_CONNECTION_INFO structure of item 'i'
            Dim iPtr As Int64 = pInfo.ToInt64 + i * Marshal.SizeOf(GetType(CONAPI_CONNECTION_INFO))
            ' Convert integer to pointer
            Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
            ' Copy data from buffer
            Dim val As CONAPI_CONNECTION_INFO = Marshal.PtrToStructure(ptr, GetType(CONAPI_CONNECTION_INFO))

            Dim item As New NokiaBTDevice
            item.FromInfo(val)
            ret.Add(item)
        Next

        If iRet = ECONA_CANCELLED Then
            Throw New OperationCanceledException '.Add("Search cancelled")
        ElseIf iRet = ECONA_FAILED_TIMEOUT Then
            Throw New TimeoutException '.Items.Add("Timeout reached")
        ElseIf iRet <> CONA_OK Then
            ShowErrorMessage("CONASearchDevices failed.", iRet)
        End If

        If iDeviceCount > 0 Then
            iRet = CONAFreeConnectionInfoStructures(iDeviceCount, pInfo)
            If iRet <> CONA_OK Then
                ShowErrorMessage("CONAFreeConnectionInfoStructures failed.", iRet)
            End If
        End If

        Return ret.ToArray(GetType(NokiaBTDevice))
    End Function

    '===================================================================
    ' BTPairingNotifyCallback
    '
    ' BT device searching callback funtion. Shows the progress dialog
    ' at the beginning of search and sets the progress dialog progress
    ' bar to correct position.
    ' If function m_pProgressDlg.IsCancelled returns TRUE, this
    ' function returns ECONA_CANCELLED, and connectivity API stops
    ' the device search.
    ' In other situations this function must always return CONA_OK.
    '===================================================================
    Private Shared Function BTPairingNotifyCallback(ByVal iState As Integer, ByVal pConnInfoStructure As IntPtr) As Integer
        ' iState value grows from 0 to 100
        'If iState < 100 Then
        ' Update progress bar
        'm_pProgressDlg.SetProgress(iState)
        'End If
        'If m_pProgressDlg.IsCancelled Then
        '' If user has clicked Cancel, return ECONA_CANCELLED

        'End If
        'Application.DoEvents()
        Dim e As New ProgressEventArgs(iState)
        RaiseEvent BTFindProgress(Nothing, e)

        If (e.Cancel) Then
            Return ECONA_CANCELLED
        Else
            Return CONA_OK
        End If
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class
