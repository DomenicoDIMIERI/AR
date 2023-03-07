'Filename    : CONADefinitions.vb
'Part of     : PCSAPI VB.NET examples
'Description : Connectivity API data definitions, converted from CONADefinitions.h
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

Partial Public Class Nokia

    Partial Public Class APIS

        'Public NotInheritable Class CONADefinitions
        '    Private Sub New()
        '    End Sub

        Private Shared Function PtrToStr(ByVal ptr As System.IntPtr) As String
            Return Marshal.PtrToStringUni(ptr)
        End Function

        Private Shared Function StrToPtr(ByVal value As String) As IntPtr
            If (value = vbNullString) Then Return IntPtr.Zero
            Return Marshal.StringToCoTaskMemUni(value)
        End Function

        '=========================================================
        ' Device definitions used in Connectivity API
        '
        'Connection info structure
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure CONAPI_CONNECTION_INFO
            Dim iDeviceID As Integer
            Dim iMedia As Integer
            Private _pstrDeviceName As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrDeviceName As String
            Private _pstrAddress As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrAddress As String
            Dim iState As Integer

            Public Property pstrDeviceName As String
                Get
                    Return PtrToStr(Me._pstrDeviceName)
                End Get
                Set(value As String)
                    Me._pstrDeviceName = StrToPtr(value)
                End Set
            End Property

            Public Property pstrAddress As String
                Get
                    Return PtrToStr(Me._pstrAddress)
                End Get
                Set(value As String)
                    Me._pstrAddress = StrToPtr(value)
                End Set
            End Property
        End Structure

        'Device info structure
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)>
        Public Structure CONAPI_DEVICE
            Dim _pstrSerialNumber As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrSerialNumber As String
            Dim _pstrFriendlyName As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrFriendlyName As String
            Dim _pstrModel As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrModel As String
            Dim _pstrManufacturer As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrManufacturer As String
            Dim iNumberOfItems As Integer
            Dim pItems As IntPtr    'Pointer to CONAPI_CONNECTION_INFO structures

            Public Property pstrSerialNumber As String
                Get
                    Return PtrToStr(Me._pstrSerialNumber)
                End Get
                Set(value As String)
                    Me._pstrSerialNumber = StrToPtr(value)
                End Set
            End Property

            Public Property pstrFriendlyName As String
                Get
                    Return PtrToStr(Me._pstrFriendlyName)
                End Get
                Set(value As String)
                    Me._pstrFriendlyName = StrToPtr(value)
                End Set
            End Property

            Public Property pstrModel As String
                Get
                    Return PtrToStr(Me._pstrModel)
                End Get
                Set(value As String)
                    Me._pstrModel = StrToPtr(value)
                End Set
            End Property

            Public Property pstrManufacturer As String
                Get
                    Return PtrToStr(Me._pstrManufacturer)
                End Get
                Set(value As String)
                    Me._pstrManufacturer = StrToPtr(value)
                End Set
            End Property

        End Structure

        ' General device info structure
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure CONAPI_DEVICE_GEN_INFO
            Dim iSize As Integer
            Dim iType As Integer
            Private _pstrTypeName As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrTypeName As String
            Private _pstrSWVersion As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrSWVersion As String
            Private _pstrUsedLanguage As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrUsedLanguage As String
            Dim iSyncSupport As Integer
            Dim iFileSystemSupport As Integer

            Public Property pstrTypeName As String
                Get
                    Return PtrToStr(Me._pstrTypeName)
                End Get
                Set(value As String)
                    Me._pstrTypeName = StrToPtr(value)
                End Set
            End Property

            Public Property pstrSWVersion As String
                Get
                    Return PtrToStr(Me._pstrSWVersion)
                End Get
                Set(value As String)
                    Me._pstrSWVersion = StrToPtr(value)
                End Set
            End Property

            Public Property pstrUsedLanguage As String
                Get
                    Return PtrToStr(Me._pstrUsedLanguage)
                End Get
                Set(value As String)
                    Me._pstrUsedLanguage = StrToPtr(value)
                End Set
            End Property
        End Structure

        ' Device product info structure
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure CONAPI_DEVICE_INFO_PRODUCT
            Dim iSize As Integer
            Private _pstrProductCode As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrProductCode As String

            Public Property pstrProductCode As String
                Get
                    Return PtrToStr(Me._pstrProductCode)
                End Get
                Set(value As String)
                    Me._pstrProductCode = StrToPtr(value)
                End Set
            End Property
        End Structure

        ' Device device icon structure
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure CONAPI_DEVICE_INFO_ICON
            Dim iSize As Integer                                        ' [in] Size
            Dim iParam As Integer                                       ' [in] Reserved for future use. Must be 0.
            Private _pstrTarget As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrTarget As String  ' [in] Target drive info. Must include memory type (e.g. "MMC" or "DEV").
            Dim iDataLength As Integer                                  ' [out]Icon data length.
            Dim pData As IntPtr                                         ' [out]Pointre to icon data.

            Public Property pstrTarget As String
                Get
                    Return PtrToStr(Me._pstrTarget)
                End Get
                Set(value As String)
                    Me._pstrTarget = StrToPtr(value)
                End Set
            End Property
        End Structure

        ' Device property info structure
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure CONAPI_GET_PROPERTY
            Dim iSize As Integer                                                ' [in] Size
            Dim iTargetPropertyType As Integer                                  ' [in] Target property type
            Private _pstrPropertyName As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrPropertyName As String    ' [in] Target Property name
            Dim iResult As Integer                                          ' [out] Result code. CONA_OK if succeeded, otherwise error code
            Private _pstrPropertyValue As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrPropertyValue As String   ' [out] Result string. If not found pointer is NULL 

            Public Property pstrPropertyName As String
                Get
                    Return PtrToStr(Me._pstrPropertyName)
                End Get
                Set(value As String)
                    Me._pstrPropertyName = StrToPtr(value)
                End Set
            End Property

            Public Property pstrPropertyValue As String
                Get
                    Return PtrToStr(Me._pstrPropertyValue)
                End Get
                Set(value As String)
                    Me._pstrPropertyValue = StrToPtr(value)
                End Set
            End Property
        End Structure

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure CONAPI_DEVICE_INFO_PROPERTIES
            Dim iSize As Integer                    ' [in] Size
            Dim iNumberOfStructs As Integer         ' [in] Count of CONAPI_GET_PROPERTY struct
            Dim pGetPropertyInfoStructs As IntPtr   ' [in] Pointer toCONAPI_GET_PROPERTY structs
        End Structure

        ' ----------------------------------------------------

        ' Search definitions used with CONASearchDevices function:
        Public Const CONAPI_DEVICE_NOT_FUNCTIONAL As Integer = &H0  ' Device is not working or unsupported device.
        Public Const CONAPI_DEVICE_UNPAIRED As Integer = &H1        ' Device is not paired
        Public Const CONAPI_DEVICE_PAIRED As Integer = &H2          ' Device is paired
        Public Const CONAPI_DEVICE_PCSUITE_TRUSTED As Integer = &H4 ' Device is PC Suite trusted
        Public Const CONAPI_DEVICE_WRONG_MODE As Integer = &H8      ' Device is connected in wrong mode.

        Public Const CONAPI_ALLOW_TO_USE_CACHE As Integer = &H1000  ' Get all devices from cache if available
        Public Const CONAPI_GET_ALL_PHONES As Integer = &H2000      ' Get all phones from target media
        Public Const CONAPI_GET_PAIRED_PHONES As Integer = &H4000   ' Get all paired phones from target media
        Public Const CONAPI_GET_TRUSTED_PHONES As Integer = &H8000  ' Get all PC Suite trusted phones from target media.

        ' Search macros used to check device's trusted/paired state: 
        Public Shared Function CONAPI_IS_DEVICE_UNPAIRED(ByVal iState As Integer) As Integer
            CONAPI_IS_DEVICE_UNPAIRED = (iState And &H1)       ' Returns 1 if true
        End Function
        Public Shared Function CONAPI_IS_DEVICE_PAIRED(ByVal iState As Integer) As Integer
            CONAPI_IS_DEVICE_PAIRED = ((iState >> 1) And &H1)  ' Returns 1 if true
        End Function
        Public Shared Function CONAPI_IS_PCSUITE_TRUSTED(ByVal iState As Integer) As Integer
            CONAPI_IS_PCSUITE_TRUSTED = ((iState >> 2) And &H1) ' Returns 1 if true
        End Function

        ' Definitions used with CONAChangeDeviceTrustedState function:
        Public Const CONAPI_PAIR_DEVICE As Integer = &H100           ' Pair device
        Public Const CONAPI_UNPAIR_DEVICE As Integer = &H200         ' Unpair device
        Public Const CONAPI_SET_PCSUITE_TRUSTED As Integer = &H400   ' Set device to PC Suite trusted 
        Public Const CONAPI_SET_PCSUITE_UNTRUSTED As Integer = &H800 ' Remove PC Suite trusted information.
        ' Definitions used with CONAGetDeviceInfo function:
        Public Const CONAPI_DEVICE_GENERAL_INFO As Integer = &H10000      ' Get CONAPI_DEVICE_GEN_INFO struct.
        Public Const CONAPI_DEVICE_PRODUCT_INFO As Integer = &H100000     ' Get CONAPI_DEVICE_INFO_PRODUCT struct.
        Public Const CONAPI_DEVICE_PROPERTIES_INFO As Integer = &H1000000 ' Get CONAPI_DEVICE_INFO_PROPERTIES struct.
        Public Const CONAPI_DEVICE_ICON_INFO As Integer = &H10000000      ' Get CONAPI_DEVICE_ICON struct.

        ' Definitions used with CONAPI_DEVICE_INFO_PROPERTIES struct
        Public Const CONAPI_DEVICE_GET_PROPERTY As Integer = &H1     ' Get value from configuration file.
        '                                                            ' pstrPropertyName must be include target property name.
        Public Const CONAPI_DEVICE_IS_APP_SUPPORTED As Integer = &H2 ' Check is the application supported in configuration file.
        '                                                            ' pstrPropertyName must be include target application name.
        ' The next properties are returned from device's OBEX Capability object:
        Public Const CONAPI_DEVICE_GET_CURRENT_NETWORK As Integer = &H1000004   ' Get Current Network string.
        Public Const CONAPI_DEVICE_GET_COUNTRY_CODE As Integer = &H2000004      ' Get Country Code string.
        Public Const CONAPI_DEVICE_GET_NETWORK_ID As Integer = &H3000004        ' Get Network ID string.
        Public Const CONAPI_DEVICE_GET_VERSION As Integer = &H100004            ' Get Version string from CONAPI_CO_xxx_SERVICE Service.
        Public Const CONAPI_DEVICE_GET_UUID As Integer = &H200004               ' Get UUID string from CONAPI_CO_xxx_SERVICE Service.
        Public Const CONAPI_DEVICE_GET_OBJTYPE As Integer = &H300004            ' Get Object type string from CONAPI_CO_xxx_SERVICE Service.
        Public Const CONAPI_DEVICE_GET_FILEPATH As Integer = &H400004           ' Get file path string from CONAPI_CO_xxx_SERVICE Service.
        '                                                                       ' pstrPropertyName must be include type of file.
        Public Const CONAPI_DEVICE_GET_FOLDERPATH As Integer = &H500004         ' Get folder path string from CONAPI_CO_xxx_SERVICE Service.
        '                                                                       ' pstrPropertyName must be include type of folder (e.g. "Images").
        Public Const CONAPI_DEVICE_GET_FOLDERMEMTYPE As Integer = &H600004      ' Get folder memory type string from CONAPI_CO_xxx_SERVICE Service. 
        '                                                                       ' pstrPropertyName must be include type of folder.
        Public Const CONAPI_DEVICE_GET_FOLDEREXCLUDE As Integer = &H700004      ' Get folder exclude path string from CONAPI_CO_xxx_SERVICE Service.
        '                                                                       ' pstrPropertyName must be include type of folder.
        Public Const CONAPI_DEVICE_GET_ALL_VALUES As Integer = &H800004         ' Get all values from CONAPI_CO_xxx_SERVICE Service. Values are separated with hash mark (#).
        '                                                                       ' pstrPropertyName must be include type of item.
        ' Definitions for Services
        Public Const CONAPI_DS_SERVICE As Integer = &H1000              ' Data Synchronication Service
        Public Const CONAPI_DM_SERVICE As Integer = &H2000              ' Device Management Service
        Public Const CONAPI_NEF_SERVICE As Integer = &H3000             ' NEF Service
        Public Const CONAPI_DS_SMS_SERVICE As Integer = &H4000          ' Data Synchronication SMS Service
        Public Const CONAPI_DS_MMS_SERVICE As Integer = &H5000          ' Data Synchronication MMS Service
        Public Const CONAPI_DS_BOOKMARKS_SERVICE As Integer = &H6000    ' Data Synchronication Bookmarks Service
        Public Const CONAPI_FOLDER_BROWSING_SERVICE As Integer = &H7000 ' Folder-Browsing Service
        Public Const CONAPI_USER_DEFINED_SERVICE As Integer = &H8000    ' User defined Service. The service name must be set to pstrPropertyName. 
        ' Definitions used with General device info structure
        ' Device types:
        Public Const CONAPI_UNKNOWN_DEVICE As Integer = &H0            ' Unknown device.
        Public Const CONAPI_SERIES40_DEVICE As Integer = &H1000001     ' Series 40 device
        Public Const CONAPI_SERIES60_2ED_DEVICE As Integer = &H2000010 ' Series 60 the 2nd edition device.
        Public Const CONAPI_SERIES60_3ED_DEVICE As Integer = &H2000020 ' Series 60 the 3nd edition device.
        Public Const CONAPI_SERIES80_DEVICE As Integer = &H2000100     ' Series 80 device.
        Public Const CONAPI_NOKIA7710_DEVICE As Integer = &H2001000    ' Nokia 7710 device.
        ' Synchronication support:
        Public Const CONAPI_SYNC_NOT_SUPPORTED As Integer = &H0     ' Device is not supporting synchronication.
        Public Const CONAPI_SYNC_SA_DS As Integer = &H1             ' Device is supporting Server Alerted (SA) Data Synchronication. 
        Public Const CONAPI_SYNC_SA_DM As Integer = &H2             ' Device is supporting Server Alerted (SA) Device Management. 
        Public Const CONAPI_SYNC_CI_DS As Integer = &H10            ' Device is supporting Client Initated (CI) Data Synchronication.
        ' File System support: 
        Public Const CONAPI_FS_NOT_SUPPORTED As Integer = &H0               ' Device is not support file system.
        Public Const CONAPI_FS_SUPPORTED As Integer = &H1                   ' Device is support file system.
        Public Const CONAPI_FS_INSTALL_JAVA_APPLICATIONS As Integer = &H10  ' Device is supporting Java MIDlet installation.
        Public Const CONAPI_FS_INSTALL_SIS_APPLICATIONS As Integer = &H20   ' Device is supporting SIS applications installation. 
        Public Const CONAPI_FS_INSTALL_SISX_APPLICATIONS As Integer = &H40  ' Device supports SISX applications' installation. 
        Public Const CONAPI_FS_FILE_CONVERSION As Integer = &H100           ' Device is supporting file conversion.
        Public Const CONAPI_FS_LIST_APPLICATIONS As Integer = &H200         ' Device supports installed applications' listing.
        Public Const CONAPI_FS_UNINSTALL_APPLICATIONS As Integer = &H400    ' Device supports installed applications' uninstallation.
        Public Const CONAPI_FS_EXTENDED_OPERATIONS As Integer = &H800       ' Device supports extended File System operations (e.g. Copy folder).

        ' Definitions used in CONASetDeviceListOption function
        ' Option types:
        Public Const DMAPI_OPTION_SET_MANUFACTURER As Integer = &H1 ' pstrValue contains the manufacturer name

        ' ----------------------------------------------------
        ' DeviceNotifyCallbackFunction
        '
        '	This is the function prototype of the callback method
        '
        '	DWORD DeviceNotifyCallbackFunction(	DWORD dwStatus, WCHAR* pstrSerialNumber);
        '	
        '	Status value uses the following format:
        '
        '		----------------DWORD------------------
        '		WORD for info		WORD for status
        '		0000 0000 0000 0000 0000 0000 0000 0000
        '
        '	Status value is the one of the values defined below describing main reason for the notification.
        '	Info part consist of two parts:
        '		LOBYTE: Info part contains change info value. See info values below.
        '		HIBYTE:	Info data value. Depends of info value.
        '	See info value definitions for more information.
        '	Use predefined macros to extract needed part from the status value.
        '
        Public Delegate Function DeviceNotifyCallbackDelegate(ByVal iStatus As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrSerialNumber As String) As Integer

        'Device callback status values
        Public Const CONAPI_DEVICE_LIST_UPDATED As Integer = &H0 ' List is updated. No any specific information.
        Public Const CONAPI_DEVICE_ADDED As Integer = &H1        ' A new device is added to the list.
        Public Const CONAPI_DEVICE_REMOVED As Integer = &H2      ' Device is removed from the list.
        Public Const CONAPI_DEVICE_UPDATED As Integer = &H4      ' Device is updated. A connection is added or removed
        ' Device callback info values
        Public Const CONAPI_CONNECTION_ADDED As Integer = &H1   ' Note! HIBYTE == media, LOBYTE == CONAPI_CONNECTION_ADDED
        Public Const CONAPI_CONNECTION_REMOVED As Integer = &H2 ' Note! HIBYTE == media, LOBYTE == CONAPI_CONNECTION_REMOVED
        Public Const CONAPI_DEVICE_RENAMED As Integer = &H4     ' Friendly name of the device is changed

        ' Device callback macros
        Public Shared Function GET_CONAPI_CB_STATUS(ByVal iStatus As Integer) As Integer
            GET_CONAPI_CB_STATUS = (&HFFFF And iStatus)
        End Function
        Public Shared Function GET_CONAPI_CB_INFO(ByVal iStatus As Integer) As Integer
            GET_CONAPI_CB_INFO = ((&HFF0000 And iStatus) >> 16)
        End Function
        Public Shared Function GET_CONAPI_CB_INFO_DATA(ByVal iStatus As Integer) As Integer
            GET_CONAPI_CB_INFO_DATA = ((&HFF000000 & iStatus) >> 24)
        End Function

        ' ----------------------------------------------------------------------
        ' DeviceSearchOperationCallbackFunction
        '
        ' Description
        ' Device Search operation callback functions are defined as: 
        '	DWORD (DeviceSearchOperationCallbackFunction)(DWORD dwState, 
        '					CONAPI_CONNECTION_INFO* pConnInfoStructure)
        '
        '	The Connectivity API calls this function at least every time period 
        '	(or if the System has found the device during this time) and adds one 
        '	to the function state value. The used time period counted by using 
        '	dwSearchTime parameter. E.g. If dwSearchTime paramater value is 240,
        '	time period  (240/100) is 2.4 seconds.
        '	If the function state is 100 and any device does not have found during 
        '	this (dwSearchTime) time the CONASearchDevices function fails with the 
        '	error code ECONA_FAILED_TIMEOUT.
        '
        ' Parameters
        '	dwState				[in] Function state (0-100%).
        '	pConnInfoStructure	[in] Reserved for future use, the value is NULL.
        '
        ' Return values
        ' The Connectivity API-user must return the CONA_OK value. If the callback 
        ' function returns the error code ECONA_CANCELLED to the Connectivity API, 
        ' the CONASearchDevices function will be cancelled with the error code ECONA_CANCELLED.
        '
        ' Type definition: 
        Public Delegate Function SearchCallbackDelegate(ByVal iState As Integer, ByVal pConnInfoStructure As IntPtr) As Integer

        '================================
        ' File system API definitions
        '===============================

        'Used for changing current folder:
        Public Const GO_TO_ROOT_FOLDER As String = "\\"
        Public Const GO_TO_PARENT_FOLDER As String = "..\"
        Public Const FOLDER_SEPARATOR As String = "\"

        'Options for CONADeleteFolder:
        Public Const CONA_DELETE_FOLDER_EMPTY As Integer = &H0
        Public Const CONA_DELETE_FOLDER_WITH_FILES As Integer = &H1

        'Direction options for CONACopyFile and CONAMoveFile:
        Public Const CONA_DIRECT_PHONE_TO_PC As Integer = &H2
        Public Const CONA_DIRECT_PC_TO_PHONE As Integer = &H4
        Public Const CONA_DIRECT_PHONE_TO_PHONE As Integer = &H8        ' Not used at the moment.

        'Other options for CONACopyFile and CONAMoveFile:
        Public Const CONA_OVERWRITE As Integer = &H10
        Public Const CONA_RENAME As Integer = &H20           ' Used only with CONACopyFile
        Public Const CONA_TRANSFER_ALL As Integer = &H40     ' Not used at the moment.

        'Options for CONAFindBegin:
        Public Const CONA_FIND_USE_CACHE As Integer = &H80

        'Attribute defines for CONAPI_FOLDER_INFO and CONAPI_FILE_INFO structures:
        Public Const CONA_FPERM_READ As Integer = &H100          'Both structure
        Public Const CONA_FPERM_WRITE As Integer = &H200         'Both structure
        Public Const CONA_FPERM_DELETE As Integer = &H400        'Both structure
        Public Const CONA_FPERM_FOLDER As Integer = &H800        'Only for CONAPI_FOLDER_INFO
        Public Const CONA_FPERM_DRIVE As Integer = &H1000        'Only for CONAPI_FOLDER_INFO
        Public Const CONA_FPERM_HIDDEN As Integer = &H2000       ' Only for CONAPI_FOLDER_INFO2
        Public Const CONA_FPERM_ROOT As Integer = &H4000         ' Only for CONAPI_FOLDER_INFO2

        'Options for CONAGetFolderInfo
        Public Const CONA_GET_FOLDER_INFO As Integer = &H1                    ' Gets target folder info
        Public Const CONA_GET_FOLDER_CONTENT As Integer = &H2                 ' Gets target folder info and contents
        Public Const CONA_GET_FOLDER_AND_SUB_FOLDERS_CONTENT As Integer = &H4 ' Gets target folder info, content and sub folder(s) contents also
        Public Const CONA_COMPARE_AND_UPDATE_IF_NEEDED As Integer = &H100     ' Compare exist folder content. If change has happened, updates content
        '                                                                       and returns CONA_OK_UPDATED. If no change, returns CONA_OK.

        Public Const CONA_DEFAULT_FOLDER As Integer = &H10000                'Used only with CONAInstallApplication
        Public Const CONA_INFORM_IF_USER_ACTION_NEEDED As Integer = &H20000  'Used only with CONAInstallApplication
        Public Const CONA_WAIT_THAT_USER_ACTION_IS_DONE As Integer = &H40000 'Used only with CONAInstallApplication

        Public Const CONA_USE_IF_NOTICATION As Integer = &H1000000      ' Used only with CONAReadFileInBlocks and CONAWriteFileInBlocks
        Public Const CONA_USE_CB_NOTICATION As Integer = &H2000000      ' Used only with CONAReadFileInBlocks and CONAWriteFileInBlocks
        Public Const CONA_NOT_SET_FILE_DETAILS As Integer = &H4000000   ' Used only with CONAReadFileInBlocks
        Public Const CONA_ALL_DATA_SENT As Integer = &H8000000          ' Used only with IFSAPIBlockNotify and CONABlockDataCallbackFunction

        Public Function CONA_IS_ALL_DATA_RECEIVED(ByVal iState As Integer) As Integer 'Used only with IFSAPIBlockNotify and CONABlockDataCallbackFunction
            CONA_IS_ALL_DATA_RECEIVED = ((iState >> 27) And &H1)
        End Function

        'Options for CONAGetFileMetadata 
        Public Const CONAPI_GET_METADATA As Integer = &H8           ' Used only with CONAGetFileMetadata
        Public Const CONA_TYPE_OF_AUDIO_METADATA As Integer = &H10  ' Used only with CONAGetFileMetadata and CONAFreeFileMetadataStructure
        Public Const CONA_TYPE_OF_IMAGE_METADATA As Integer = &H20  ' Used only with CONAGetFileMetadata and CONAFreeFileMetadataStructure


        ' ----------------------------------------------------
        ' Folder info structure
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure CONAPI_FOLDER_INFO
            '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrName As String   ' Folder or Drive name
            Private _pstrName As IntPtr ' Folder or Drive name
            Dim iAttributes As Integer   ' Folder or Drive type and permission 
            Dim tFolderTime As ComTypes.FILETIME   ' Folder time
            Private _pstrLabel As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrLabel As String   ' Drive lable name 
            Private _pstrMemoryType As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrMemoryType As String ' Folder or Drive memory type

            Public Property pstrName As String
                Get
                    Return PtrToStr(Me._pstrName)
                End Get
                Set(value As String)
                    Me._pstrName = StrToPtr(value)
                End Set
            End Property

            Public Property pstrLabel As String
                Get
                    Return PtrToStr(Me._pstrLabel)
                End Get
                Set(value As String)
                    Me._pstrLabel = StrToPtr(value)
                End Set
            End Property

            Public Property pstrMemoryType As String
                Get
                    Return PtrToStr(Me._pstrMemoryType)
                End Get
                Set(value As String)
                    Me._pstrMemoryType = StrToPtr(value)
                End Set
            End Property
        End Structure

        ' File info structure
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure CONAPI_FILE_INFO
            Private _pstrName As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrName As String   ' File name
            Dim iAttributes As Integer  ' File permission
            Dim tFileTime As ComTypes.FILETIME   ' File modified time
            Dim iFileSize As Integer    ' File size
            Private _pstrMIMEType As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrMIMEType As String  ' File MIME type

            Public Property pstrName As String
                Get
                    Return PtrToStr(Me._pstrName)
                End Get
                Set(value As String)
                    Me._pstrName = StrToPtr(value)
                End Set
            End Property

            Public Property pstrMIMEType As String
                Get
                    Return PtrToStr(Me._pstrMIMEType)
                End Get
                Set(value As String)
                    Me._pstrMIMEType = StrToPtr(value)
                End Set
            End Property
        End Structure

        ' Folder info structure
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure CONAPI_FOLDER_INFO2
            Dim iSize As Integer        ' Size of struct
            Private _pstrName As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrName As String     ' Folder or Drive name
            Private _pstrLocation As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrLocation As String    ' Absolute location path to folder or drive
            Dim iAttributes As Integer      ' Folder or Drive type and permission 
            Dim tFolderTime As ComTypes.FILETIME     ' Folder time
            Private _pstrLabel As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrLabel As String     ' Drive lable name 
            Private _pstrMemoryType As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrMemoryType As String    ' Folder or Drive memory type
            Private _pstrID As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrID As String      ' Identification ID
            Dim dlFreeMemory As Long     ' Free memory in drive
            Dim dlTotalMemory As Long     ' Total memory in drive
            Dim dlUsedMemory As Long    ' Used memory in drive
            Dim iContainFiles As Integer      ' Number of files in target folder or drive
            Dim iContainFolders As Integer  ' Number of folders in target folder or drive
            Dim dlTotalSize As Long    ' Size of folder content (including content of subfolders)
            Private _pstrValue As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrValue As String    ' Reserved for future

            Public Property pstrName As String
                Get
                    Return PtrToStr(Me._pstrName)
                End Get
                Set(value As String)
                    Me._pstrName = StrToPtr(value)
                End Set
            End Property

            Public Property pstrLocation As String
                Get
                    Return PtrToStr(Me._pstrLocation)
                End Get
                Set(value As String)
                    Me._pstrLocation = StrToPtr(value)
                End Set
            End Property

            Public Property pstrLabel As String
                Get
                    Return PtrToStr(Me._pstrLabel)
                End Get
                Set(value As String)
                    Me._pstrLabel = StrToPtr(value)
                End Set
            End Property

            Public Property pstrMemoryType As String
                Get
                    Return PtrToStr(Me._pstrMemoryType)
                End Get
                Set(value As String)
                    Me._pstrMemoryType = StrToPtr(value)
                End Set
            End Property

            Public Property pstrID As String
                Get
                    Return PtrToStr(Me._pstrID)
                End Get
                Set(value As String)
                    Me._pstrID = StrToPtr(value)
                End Set
            End Property

            Public Property pstrValue As String
                Get
                    Return PtrToStr(Me._pstrValue)
                End Get
                Set(value As String)
                    Me._pstrValue = StrToPtr(value)
                End Set
            End Property
        End Structure

        ' Folder content structure
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure CONAPI_FOLDER_CONTENT
            Dim iSize As Integer       ' Size of struct
            ' CONAPI_FOLDER_INFO2*	pFolderInfo;				' Folder info struct
            Dim pFolderInfo As IntPtr
            Private _pstrPath As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrPath As String      ' Absolute path of sub files and sub folders
            Dim iNumberOfFileInfo As Integer        ' Number of file structs
            ' CONAPI_FILE_INFO*		pFileInfo;					' File structs
            Dim pFileInfo As IntPtr
            Dim iNumberOfSubFolderContent As Integer      ' Number of file structs
            ' CONAPI_FOLDER_CONTENT*  pSubFolderContent;			' File structs
            Dim pSubFolderContent As IntPtr
            ' CONAPI_FOLDER_CONTENT*	pParentFolder;				' Pointer to the parent folder content struct
            Dim pParentFolder As IntPtr
            Private _pstrValue As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrValue As String      ' Reserved for future	

            Public Property pstrPath As String
                Get
                    Return PtrToStr(Me._pstrPath)
                End Get
                Set(value As String)
                    Me._pstrPath = StrToPtr(value)
                End Set
            End Property

            Public Property pstrValue As String
                Get
                    Return PtrToStr(Me._pstrValue)
                End Get
                Set(value As String)
                    Me._pstrValue = StrToPtr(value)
                End Set
            End Property
        End Structure

        ' File CONAPI_FILE_AUDIO_METADATA structure
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure CONAPI_FILE_AUDIO_METADATA
            Dim iSize As Integer                                                'Size of struct
            Private _pstrAlbum As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrAlbum As String           'Album	
            Private _pstrAlbumTrack As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrAlbumTrack As String      'AlbumTrack
            Private _pstrArtist As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrArtist As String          'Artist		
            Private _pstrComment As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrComment As String         'Comment
            Private _pstrComposer As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrComposer As String        'Composer
            Private _pstrCopyright As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrCopyright As String       'Copyright
            Private _pstrDate As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrDate As String            'Date
            Private _pstrDuration As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrDuration As String        'Duration
            Private _pstrGenre As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrGenre As String           'Genre
            Private _pstrOriginalArtist As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrOriginalArtist As String  'Original Artist
            Private _pstrRating As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrRating As String          'Rating
            Private _pstrTitle As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrTitle As String           'Title
            Private _pstrUniqueFileId As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrUniqueFileId As String    'Unique File Identifier
            Private _pstrUrl As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrUrl As String             'Url
            Private _pstrUserUrl As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrUserUrl As String         'User Url
            Private _pstrVendor As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrVendor As String          'Vendor
            Private _pstrYear As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrYear As String            'Year
            Private _pstrValue As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrValue As String           'Reserved for future
            Dim iID3Version As Integer                                          'Version (0 = NonID3, 1 = Ver1, 2 = Ver2)
            Dim iValue As Integer                                               'Reserved for future
            Dim iJpegLenght As Integer                                          'Jpeg data lenght
            Dim pJpeg As IntPtr                                                 'Jpeg data

            Public Property pstrAlbum As String
                Get
                    Return PtrToStr(Me._pstrAlbum)
                End Get
                Set(value As String)
                    Me._pstrAlbum = StrToPtr(value)
                End Set
            End Property

            Public Property pstrAlbumTrack As String
                Get
                    Return PtrToStr(Me._pstrAlbumTrack)
                End Get
                Set(value As String)
                    Me._pstrAlbumTrack = StrToPtr(value)
                End Set
            End Property

            Public Property pstrArtist As String
                Get
                    Return PtrToStr(Me._pstrArtist)
                End Get
                Set(value As String)
                    Me._pstrArtist = StrToPtr(value)
                End Set
            End Property

            Public Property pstrComment As String
                Get
                    Return PtrToStr(Me._pstrComment)
                End Get
                Set(value As String)
                    Me._pstrComment = StrToPtr(value)
                End Set
            End Property

            Public Property pstrComposer As String
                Get
                    Return PtrToStr(Me._pstrComposer)
                End Get
                Set(value As String)
                    Me._pstrComposer = StrToPtr(value)
                End Set
            End Property

            Public Property pstrCopyright As String
                Get
                    Return PtrToStr(Me._pstrCopyright)
                End Get
                Set(value As String)
                    Me._pstrCopyright = StrToPtr(value)
                End Set
            End Property

            Public Property pstrDate As String
                Get
                    Return PtrToStr(Me._pstrDate)
                End Get
                Set(value As String)
                    Me._pstrDate = StrToPtr(value)
                End Set
            End Property

            Public Property pstrDuration As String
                Get
                    Return PtrToStr(Me._pstrDuration)
                End Get
                Set(value As String)
                    Me._pstrDuration = StrToPtr(value)
                End Set
            End Property

            Public Property pstrGenre As String
                Get
                    Return PtrToStr(Me._pstrGenre)
                End Get
                Set(value As String)
                    Me._pstrGenre = StrToPtr(value)
                End Set
            End Property

            Public Property pstrOriginalArtist As String
                Get
                    Return PtrToStr(Me._pstrOriginalArtist)
                End Get
                Set(value As String)
                    Me._pstrOriginalArtist = StrToPtr(value)
                End Set
            End Property

            Public Property pstrRating As String
                Get
                    Return PtrToStr(Me._pstrRating)
                End Get
                Set(value As String)
                    Me._pstrRating = StrToPtr(value)
                End Set
            End Property

            Public Property pstrTitle As String
                Get
                    Return PtrToStr(Me._pstrTitle)
                End Get
                Set(value As String)
                    Me._pstrTitle = StrToPtr(value)
                End Set
            End Property

            Public Property pstrUniqueFileId As String
                Get
                    Return PtrToStr(Me._pstrUniqueFileId)
                End Get
                Set(value As String)
                    Me._pstrUniqueFileId = StrToPtr(value)
                End Set
            End Property

            Public Property pstrUrl As String
                Get
                    Return PtrToStr(Me._pstrUrl)
                End Get
                Set(value As String)
                    Me._pstrUrl = StrToPtr(value)
                End Set
            End Property

            Public Property pstrUserUrl As String
                Get
                    Return PtrToStr(Me._pstrUserUrl)
                End Get
                Set(value As String)
                    Me._pstrUserUrl = StrToPtr(value)
                End Set
            End Property

            Public Property pstrVendor As String
                Get
                    Return PtrToStr(Me._pstrVendor)
                End Get
                Set(value As String)
                    Me._pstrVendor = StrToPtr(value)
                End Set
            End Property

            Public Property pstrYear As String
                Get
                    Return PtrToStr(Me._pstrYear)
                End Get
                Set(value As String)
                    Me._pstrYear = StrToPtr(value)
                End Set
            End Property

            Public Property pstrValue As String
                Get
                    Return PtrToStr(Me._pstrValue)
                End Get
                Set(value As String)
                    Me._pstrValue = StrToPtr(value)
                End Set
            End Property

        End Structure

        ' File CONAPI_FILE_IMAGE_METADATA structure
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure CONAPI_FILE_IMAGE_METADATA

            Dim iSize As Integer                                            ' Size of struct
            Private _pstrCopyright As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrCopyright As String   ' Copyright
            Private _pstrDateTime As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrDateTime As String    ' DateTime
            Private _pstrDateTimeDigitized As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrDateTimeDigitized As String 'Date Time Digitized
            Private _pstrDateTimeOriginal As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrDateTimeOriginal As String 'Date Time Original
            Private _pstrImageDescription As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrImageDescription As String 'Image description
            Private _pstrMake As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrMake As String        ' Make
            Private _pstrMakerNote As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrMakerNote As String   ' Maker Note
            Private _pstrModel As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrModel As String       ' Model
            Private _pstrIsoSpeedRatings As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrIsoSpeedRatings As String 'Iso Speed Ratings
            Private _pstrRelatedSoundFile As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrRelatedSoundFile As String 'Related Sound File
            Private _pstrSoftware As IntPtr  '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrSoftware As String    ' Software
            Private _pstrUserComment As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrUserComment As String ' User Comment
            Private _pstrValue As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrValue As String       ' Reserved for future
            Private _dwThumbnailLenght As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim dwThumbnailLenght As String ' Thumbnail image data lenght
            Dim pThumbnail As IntPtr                                        ' Thumbnail image data
            Dim lExistValuesMask As Long                                    ' Infomation bit-mask for DWORD value which are exist & set.
            Dim iApertureValueNum As Integer                                ' Aperture Value numerator.			If exist, dlExistValuesMask includes bit 0x0000000000000001.
            Dim iApertureValueDen As Integer                                ' Aperture Value denominator.		If exist, dlExistValuesMask includes bit 0x0000000000000002.
            Dim iBrightnessValueNum As Integer                              ' Brightness Value numerator.		If exist, dlExistValuesMask includes bit 0x0000000000000004.
            Dim iBrightnessValueDen As Integer                              ' Brightness Value denominator.		If exist, dlExistValuesMask includes bit 0x0000000000000008.
            Dim iColorSpace As Integer                                      ' Color Space. 					    If exist, dlExistValuesMask includes bit 0x0000000000000010.
            Dim iComponentsConf As Integer                                  ' Components Configuration (4 x 8bit values). If exist, dlExistValuesMask includes bit 0x0000000000000020.
            Dim iContrast As Integer                                        ' Contrast. 							If exist, dlExistValuesMask includes bit 0x0000000000000040.
            Dim iCustomRendered As Integer                                  ' Custom Rendered. 					If exist, dlExistValuesMask includes bit 0x0000000000000080.
            Dim iDigitalZoomRatioNum As Integer                             ' Digital Zoom Ratio numerator.		If exist, dlExistValuesMask includes bit 0x0000000000000100.
            Dim iDigitalZoomRatioDen As Integer                             ' Digital Zoom Ratio denominator. 	If exist, dlExistValuesMask includes bit 0x0000000000000200.
            Dim iExifVersion As Integer                                     ' Exif Version. 						If exist, dlExistValuesMask includes bit 0x0000000000000400.
            Dim iExposureBiasNum As Integer                                 ' Exposure Bias Value numerator. 	If exist, dlExistValuesMask includes bit 0x0000000000000800.
            Dim iExposureBiasDen As Integer                                 ' Exposure Bias Value denominator. 	If exist, dlExistValuesMask includes bit 0x0000000000001000.
            Dim iExposureMode As Integer                                    ' Exposure Mode. 					If exist, dlExistValuesMask includes bit 0x0000000000002000.
            Dim iExposureProgram As Integer                                 ' Exposure Program. 					If exist, dlExistValuesMask includes bit 0x0000000000004000.
            Dim iExposureTime1 As Integer                                   ' Exposure Time 1.					If exist, dlExistValuesMask includes bit 0x0000000000008000.
            Dim iExposureTime2 As Integer                                   ' Exposure Time 2.					If exist, dlExistValuesMask includes bit 0x0000000000010000.
            Dim iFileSource As Integer                                      ' File Source.						If exist, dlExistValuesMask includes bit 0x0000000000020000.
            Dim iFlash As Integer                                           ' Flash.								If exist, dlExistValuesMask includes bit 0x0000000000040000.
            Dim iFlashPixVersion As Integer                                 ' Flash Pix Version. 				If exist, dlExistValuesMask includes bit 0x0000000000080000.
            Dim iGainControl As Integer                                     ' Gain Control.						If exist, dlExistValuesMask includes bit 0x0000000000100000.
            Dim iGpsVersion As Integer                                      ' Gps Version.						If exist, dlExistValuesMask includes bit 0x0000000000200000.
            Dim iLightSource As Integer                                     ' Light Source.						If exist, dlExistValuesMask includes bit 0x0000000000400000.
            Dim iMeteringMode As Integer                                    ' Metering Mode.						If exist, dlExistValuesMask includes bit 0x0000000000800000.
            Dim iOrientation As Integer                                     ' Orientation.						If exist, dlExistValuesMask includes bit 0x0000000001000000.
            Dim iPixelXDimension As Integer                                 ' Pixel X Dimension. 				If exist, dlExistValuesMask includes bit 0x0000000002000000.
            Dim iPixelYDimension As Integer                                 ' Pixel Y Dimension. 				If exist, dlExistValuesMask includes bit 0x0000000004000000.
            Dim iResolutionUnit As Integer                                  ' Resolution Unit.					If exist, dlExistValuesMask includes bit 0x0000000008000000.
            Dim iSaturation As Integer                                      ' Saturation.						If exist, dlExistValuesMask includes bit 0x0000000010000000.
            Dim iSceneCaptureType As Integer                                ' SceneCapture Type. 				If exist, dlExistValuesMask includes bit 0x0000000020000000.
            Dim iSharpness As Integer                                       ' Sharpness.							If exist, dlExistValuesMask includes bit 0x0000000040000000.
            Dim iShutterSpeedValueNum As Integer                            ' Shutter Speed Value numerator.		If exist, dlExistValuesMask includes bit 0x0000000080000000.
            Dim iShutterSpeedValueDen As Integer                            ' Shutter Speed Value denominator.	If exist, dlExistValuesMask includes bit 0x0000000100000000.
            Dim iThumbnailCompression As Integer                            ' Thumbnail Compression.				If exist, dlExistValuesMask includes bit 0x0000000200000000.
            Dim iThumbnailJpegIFormat As Integer                            ' Thumbnail Jpeg Interchange Format. If exist, dlExistValuesMask includes bit 0x0000000400000000.
            Dim iThumbnailJpegIFormatLen As Integer                         ' Thumbnail Jpeg Interchange Format Length. If exist, dlExistValuesMask includes bit 0x0000000800000000.
            Dim iThumbnailResUnit As Integer                                ' Thumbnail ResolutionUnit.			If exist, dlExistValuesMask includes bit 0x0000001000000000.
            Dim iThumbnailXResNum As Integer                                ' Thumbnail XResolution numerator.	If exist, dlExistValuesMask includes bit 0x0000002000000000.
            Dim iThumbnailXResDen As Integer                                ' Thumbnail XResolution denominator. If exist, dlExistValuesMask includes bit 0x0000004000000000.
            Dim iThumbnailYResNum As Integer                                ' Thumbnail XResolution numerator.	If exist, dlExistValuesMask includes bit 0x0000008000000000.
            Dim iThumbnailYResDen As Integer                                ' Thumbnail YResolution denominator. If exist, dlExistValuesMask includes bit 0x0000010000000000.
            Dim iWhiteBalance As Integer                                    ' White Balance.						If exist, dlExistValuesMask includes bit 0x0000020000000000.
            Dim iXResolutionNum As Integer                                  ' X Resolution numerator.			If exist, dlExistValuesMask includes bit 0x0000040000000000.
            Dim iXResolutionDen As Integer                                  ' X Resolution denominator.			If exist, dlExistValuesMask includes bit 0x0000080000000000.
            Dim iYResolutionNum As Integer                                  ' Y Resolution numerator.			If exist, dlExistValuesMask includes bit 0x0000100000000000.
            Dim iYResolutionDen As Integer                                  ' Y Resolution denominator.			If exist, dlExistValuesMask includes bit 0x0000200000000000.
            Dim iYCbCrPosData As Integer                                    ' YCbCr Positioning data.			If exist, dlExistValuesMask includes bit 0x0000400000000000.
            Dim iValue As Integer                                           ' Reserved for future

            Public Property pstrCopyright As String
                Get
                    Return PtrToStr(Me._pstrCopyright)
                End Get
                Set(value As String)
                    Me._pstrCopyright = StrToPtr(value)
                End Set
            End Property

            Public Property pstrDateTime As String
                Get
                    Return PtrToStr(Me._pstrDateTime)
                End Get
                Set(value As String)
                    Me._pstrDateTime = StrToPtr(value)
                End Set
            End Property

            Public Property pstrDateTimeDigitized As String
                Get
                    Return PtrToStr(Me._pstrDateTimeDigitized)
                End Get
                Set(value As String)
                    Me._pstrDateTimeDigitized = StrToPtr(value)
                End Set
            End Property

            Public Property pstrDateTimeOriginal As String
                Get
                    Return PtrToStr(Me._pstrDateTimeOriginal)
                End Get
                Set(value As String)
                    Me._pstrDateTimeOriginal = StrToPtr(value)
                End Set
            End Property

            Public Property pstrImageDescription As String
                Get
                    Return PtrToStr(Me._pstrImageDescription)
                End Get
                Set(value As String)
                    Me._pstrImageDescription = StrToPtr(value)
                End Set
            End Property

            Public Property pstrMake As String
                Get
                    Return PtrToStr(Me._pstrMake)
                End Get
                Set(value As String)
                    Me._pstrMake = StrToPtr(value)
                End Set
            End Property

            Public Property pstrMakerNote As String
                Get
                    Return PtrToStr(Me._pstrMakerNote)
                End Get
                Set(value As String)
                    Me._pstrMakerNote = StrToPtr(value)
                End Set
            End Property

            Public Property pstrModel As String
                Get
                    Return PtrToStr(Me._pstrModel)
                End Get
                Set(value As String)
                    Me._pstrModel = StrToPtr(value)
                End Set
            End Property

            Public Property pstrIsoSpeedRatings As String
                Get
                    Return PtrToStr(Me._pstrIsoSpeedRatings)
                End Get
                Set(value As String)
                    Me._pstrIsoSpeedRatings = StrToPtr(value)
                End Set
            End Property

            Public Property pstrRelatedSoundFile As String
                Get
                    Return PtrToStr(Me._pstrRelatedSoundFile)
                End Get
                Set(value As String)
                    Me._pstrRelatedSoundFile = StrToPtr(value)
                End Set
            End Property

            Public Property pstrSoftware As String
                Get
                    Return PtrToStr(Me._pstrSoftware)
                End Get
                Set(value As String)
                    Me._pstrSoftware = StrToPtr(value)
                End Set
            End Property

            Public Property pstrUserComment As String
                Get
                    Return PtrToStr(Me._pstrUserComment)
                End Get
                Set(value As String)
                    Me._pstrUserComment = StrToPtr(value)
                End Set
            End Property

            Public Property pstrValue As String
                Get
                    Return PtrToStr(Me._pstrValue)
                End Get
                Set(value As String)
                    Me._pstrValue = StrToPtr(value)
                End Set
            End Property

            Public Property dwThumbnailLenght As String
                Get
                    Return PtrToStr(Me._dwThumbnailLenght)
                End Get
                Set(value As String)
                    Me._dwThumbnailLenght = StrToPtr(value)
                End Set
            End Property
        End Structure

        ' ----------------------------------------------------
        ' FSNotifyCallbackDelegate function:
        '
        ' ----------------------------------------------------
        '	This is the function prototype of the callback method
        '
        Public Delegate Function FSNotifyCallbackDelegate(ByVal iOperation As Integer, ByVal iStatus As Integer, ByVal iTransferredBytes As Integer, ByVal iAllBytes As Integer) As Integer

        ' ----------------------------------------------------
        ' FSFolderInfoCallbackDelegate function:
        '
        ' ----------------------------------------------------
        '	This is the function prototype of the callback method
        '
        ' typedef DWORD (CALLBACK *PFN_CONA_FS_FOLDERINFO_CALLBACK)(LPCONAPI_FOLDER_INFO2 pFolderInfo);
        Public Delegate Function FSFolderInfoCallbackDelegate(ByVal pFolderInfo As IntPtr) As Integer

        ' ----------------------------------------------------
        ' CONABlockDataCallbackFunction function:
        '
        ' Callback function prototype:
        ' typedef DWORD (CALLBACK *PFN_CONA_FS_BLOCKDATA_CALLBACK)(
        '								DWORD dwFSFunction,
        '								DWORD *pdwState,
        '								const DWORD dwSizeOfFileDataBlockBuffer,
        '								DWORD *pdwFileDataBlockLenght,
        '								unsigned char* pFileDataBlock);
        Public Delegate Function FSBlockDataCallbackDelegate(ByVal iFSFunction As Integer, ByRef iState As Integer, ByVal iSizeOfFileDataBlockBuffer As Integer, ByRef iFileDataBlockLenght As Integer, ByVal pFileDataBlock As IntPtr) As Integer


        ' ----------------------------------------------------
        ' FSFunction values:
        Public Const CONARefreshDeviceMemoryValuesNtf As Integer = &H1
        Public Const CONASetCurrentFolderNtf As Integer = &H2
        Public Const CONAFindBeginNtf As Integer = &H4
        Public Const CONACreateFolderNtf As Integer = &H8
        Public Const CONADeleteFolderNtf As Integer = &H10
        Public Const CONARenameFolderNtf As Integer = &H20
        Public Const CONAGetFileInfoNtf As Integer = &H40
        Public Const CONADeleteFileNtf As Integer = &H80
        Public Const CONAMoveFileNtf As Integer = &H100
        Public Const CONACopyFileNtf As Integer = &H200
        Public Const CONARenameFileNtf As Integer = &H400
        Public Const CONAReadFileNtf As Integer = &H800
        Public Const CONAWriteFileNtf As Integer = &H1000
        Public Const CONAConnectionLostNtf As Integer = &H2000
        Public Const CONAInstallApplicationNtf As Integer = &H4000
        Public Const CONAConvertFileNtf As Integer = &H8000
        Public Const CONAGetFolderInfoNtf As Integer = &H10000
        Public Const CONAListApplicationNtf As Integer = &H20000
        Public Const CONAUninstallApplicationNtf As Integer = &H40000
        Public Const CONAReadFileInBlocksNtf As Integer = &H80000
        Public Const CONAWriteFileInBlocksNtf As Integer = &H100000
        Public Const CONAMoveFolderNtf As Integer = &H200000
        Public Const CONACopyFolderNtf As Integer = &H400000
        Public Const CONAGetFileMetadataNtf As Integer = &H800000

        ' The next function do not send notifications:
        '	CONAOpenFS					
        '	CONACloseFS				
        '	CONARegisterFSNotifyCallback
        '	CONAGetMemoryTypes 			
        '	CONAGetMemoryValues			
        '	CONAGetCurrentFolder	
        '	CONAFindNextFolder		
        '	CONAFindNextFile		
        '	CONAFindEnd					
        '	CONACancel				

        ' Possible error codes value in dwStatus parameter when 
        ' FSFunction value is CONAConnectionLost:
        '   ECONA_CONNECTION_LOST
        '   ECONA_CONNECTION_REMOVED
        '   ECONA_CONNECTION_FAILED
        '   ECONA_SUSPEND

        ' ----------------------------------------------------
        ' CONAMediaCallback
        '
        '	This is the function prototype of the callback method
        '
        '	DWORD CALLBACK CONAMediaCallback(DWORD  dwStatus, API_MEDIA* pMedia);

        Public Delegate Function MediaCallbackDelegate(ByVal iStatus As Integer, ByVal pMedia As IntPtr) As Integer
        ' ----------------------------------------------------
        ' Media info structure
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure API_MEDIA
            Dim iSize As Integer
            Dim iMedia As Integer
            Private _pstrDescription As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrDescription As String
            Dim iState As Integer
            Dim iOptions As Integer
            Dim iMediaData As Integer
            Private _pstrID As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrID As String

            Public Property pstrDescription As String
                Get
                    Return PtrToStr(Me._pstrDescription)
                End Get
                Set(value As String)
                    Me._pstrDescription = StrToPtr(value)
                End Set
            End Property

            Public Property pstrID As String
                Get
                    Return PtrToStr(Me._pstrID)
                End Get
                Set(value As String)
                    Me._pstrID = StrToPtr(value)
                End Set
            End Property
        End Structure

        'Synchronication support:
        Public Const API_MEDIA_ACTIVE As Integer = &H1            ' Media is active.
        Public Const API_MEDIA_NOT_ACTIVE As Integer = &H2        ' Media is not active. 
        Public Const API_MEDIA_IC_SUPPORTED As Integer = &H10     ' Media is supporting incoming connections. 
        Public Const API_MEDIA_IC_NOT_SUPPORTED As Integer = &H20 ' Media is not supporting incoming connections.

        Public Shared Function CONAPI_GET_MEDIA_TYPE(ByVal iMedia As Integer) As Integer
            CONAPI_GET_MEDIA_TYPE = &HFF And iMedia
        End Function

        Public Shared Function CONAPI_IS_MEDIA_ACTIVE(ByVal iState As Integer) As Integer
            CONAPI_IS_MEDIA_ACTIVE = &H1 And iState
        End Function

        Public Shared Function CONAPI_IS_MEDIA_UNACTIVE(ByVal iState As Integer) As Integer
            CONAPI_IS_MEDIA_UNACTIVE = (&H2 And iState) >> 1
        End Function
        Public Shared Function CONAPI_IS_IC_SUPPORTED(ByVal iOptions As Integer) As Integer
            CONAPI_IS_IC_SUPPORTED = (&H10 And iOptions) >> 4
        End Function

        Public Shared Function CONAPI_IS_IC_UNSUPPORTED(ByVal iOptions As Integer) As Integer
            CONAPI_IS_IC_UNSUPPORTED = (&H20 And iOptions) >> 5
        End Function


         

    End Class


End Class