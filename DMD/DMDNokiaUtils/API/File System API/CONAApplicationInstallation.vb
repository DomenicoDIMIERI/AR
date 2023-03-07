'Filename    : CONAApplicationInstallation.vb
'Part of     : Connectivity API VB.NET examples
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


Partial Class Nokia

    Partial Class APIS


        '/////////////////////////////////////////////////////////////////////
        '// Connectivity API Application Installation definitions and function
        '/////////////////////////////////////////////////////////////////////

        ' The next define values used to define which type of struct is used:
        Public Const CONA_APPLICATION_TYPE_SIS As Integer = &H1         ' Use when struct type is CONAPI_APPLICATION_SIS
        Public Const CONA_APPLICATION_TYPE_JAVA As Integer = &H2        ' Use when struct type is CONA_APPLICATION_TYPE_JAVA
        Public Const CONA_APPLICATION_TYPE_THEMES As Integer = &H4      ' Use when struct type is CONAPI_APPLICATION_THEMES
        Public Const CONA_APPLICATION_TYPE_UNKNOWN As Integer = &H8     ' Use only in CONAPI_APPLICATION_INFO struct, unknown application type.

       


        Public Const CONA_DEFAULT_UNINSTALLATION As Integer = &H100     ' Default uninstallation, used with CONAUninstallApplication function.
        Public Const CONA_SILENT_UNINSTALLATION As Integer = &H200      ' Silent uninstallation, used with CONAUninstallApplication function.

        Public Const CONA_LIST_ALL_APPICATIONS As Integer = &H1000      '
        Public Const CONA_LIST_JAVA_APPICATIONS As Integer = &H2000     ' List all installed java applications, used with CONAListApplications function
        Public Const CONA_LIST_THEMES_APPICATIONS As Integer = &H4000   ' List all installed themes, used with CONAListApplications function


        ' The struct for sis applications
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure CONAPI_APPLICATION_SIS
            '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrFileNameSis As String  ' sis application File name, must be set
            Private _pstrFileNameSis As IntPtr

            Public Property pstrFileNameSis As String
                Get
                    Return PtrToStr(Me._pstrFileNameSis)
                End Get
                Set(value As String)
                    Me._pstrFileNameSis = StrToPtr(value)
                End Set
            End Property

            ' The value can also include the file path in PC.
        End Structure

        ' The struct for java applications
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure CONAPI_APPLICATION_JAVA
            '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrFileNameJad As String  ' File name of the jad file. The value can also include the file path in PC. 
            Private _pstrFileNameJad As IntPtr

            'If jad file is not available, the value must be NULL.
            '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrFileNameJar As String  ' File name of the jar file, must be set. 
            Private _pstrFileNameJar As IntPtr

            Public Property pstrFileNameJad As String
                Get
                    Return PtrToStr(Me._pstrFileNameJad)
                End Get
                Set(value As String)
                    Me._pstrFileNameJad = StrToPtr(value)
                End Set
            End Property

            Public Property pstrFileNameJar As String
                Get
                    Return PtrToStr(Me._pstrFileNameJar)
                End Get
                Set(value As String)
                    Me._pstrFileNameJar = StrToPtr(value)
                End Set
            End Property

            ' The value can also include the file path in PC. 
        End Structure

        ' The struct for themes file
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure CONAPI_APPLICATION_FILE
            '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrFileName As String   ' File name of the nth file, must be set
            Private _pstrFileName As IntPtr

            ' The value can also include the file path in PC.
            Dim iOptions As Integer    ' Reserved for future use, the value must be 0.

            Public Property pstrFileName As String
                Get
                    Return PtrToStr(Me._pstrFileName)
                End Get
                Set(value As String)
                    Me._pstrFileName = StrToPtr(value)
                End Set
            End Property

        End Structure

        'The struct for application info, used with CONAListApplications function
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
        Public Structure CONAPI_APPLICATION_INFO
            Dim dwSize As Integer                                               ' Size of struct in bytes.

            '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrName As String            ' Application name. Always exist.
            Private _pstrName As IntPtr

            '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrDescription As String     ' Application description. If not available, pointer is NULL.
            Private _pstrDescription As IntPtr

            '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrVendor As String          ' Application vendor. If not available, pointer is NULL.
            Private _pstrVendor As IntPtr

            '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrVersion As String         ' Application version. If not available, pointer is NULL.
            Private _pstrVersion As IntPtr

            '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrParentAppNam As String    ' Parent application name. This is available if application is augmentation 
            Private _pstrParentAppNam As IntPtr

            '                                                                   ' for some other application. 
            Dim dwApplicationSize As Integer                                    ' Application size in bytes. If not available, value is -1 (0xFFFFFFFF).
            Dim dwApplicationType As CONA_APPLICATION_TYPE                      ' Application type possible values:
            '										                            '   CONA_APPLICATION_TYPE_SIS		:	Sis application
            '								                                    '	CONA_APPLICATION_TYPE_JAVA		:	Java application
            '								                                    '   CONA_APPLICATION_TYPE_THEMES	:	Themes application	
            '								                                    '   CONA_APPLICATION_TYPE_UNKNOWN	:	Application type is not available
            '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrUID As String             ' Application UID string, used with CONAUninstallApplication function.
            Private _pstrUID As IntPtr

            Dim dwOptions As Integer                                            ' Reserved for future use. Value is zero.
            '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrValue As String           ' Reserved for future use. Pointer is NULL.
            Private _pstrValue As IntPtr

            Public Property pstrName As String
                Get
                    Return PtrToStr(Me._pstrName)
                End Get
                Set(value As String)
                    Me._pstrName = StrToPtr(value)
                End Set
            End Property

            Public Property pstrDescription As String
                Get
                    Return PtrToStr(Me._pstrDescription)
                End Get
                Set(value As String)
                    Me._pstrDescription = StrToPtr(value)
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

            Public Property pstrVersion As String
                Get
                    Return PtrToStr(Me._pstrVersion)
                End Get
                Set(value As String)
                    Me._pstrVersion = StrToPtr(value)
                End Set
            End Property

            Public Property pstrParentAppNam As String
                Get
                    Return PtrToStr(Me._pstrParentAppNam)
                End Get
                Set(value As String)
                    Me._pstrParentAppNam = StrToPtr(value)
                End Set
            End Property

            Public Property pstrUID As String
                Get
                    Return PtrToStr(Me._pstrUID)
                End Get
                Set(value As String)
                    Me._pstrUID = StrToPtr(value)
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

        '=========================================================
        ' CONAInstallApplication 
        '
        ' Description:
        '	CONAInstallApplication installs the given application on 
        '	the device. FS's CONACancel function can be used to cancel 
        '	this function. The application file name(s) must be given 
        '	in the CONAPI_APPLICATION_SIS or CONAPI_APPLICATION_JAVA 
        '	structure. 
        '	
        '	The function also checks the amount of free memory from 
        '	the phone before the installation. If there is not enough 
        '	memory left in the target memory drive, the function fails 
        '	with the error code ECONA_MEMORY_FULL.
        '	
        '	If the Application file type is sis, the CONAPI_APPLICATION_SIS 
        '	structure must be used. The iAppicationType parameter's 
        '	value must be CONA_APPLICATION_TYPE_SIS.
        '	
        '	If the Application file type is jar, the CONAPI_APPLICATION_JAVA 
        '	structure must be used. The iAppicationType parameter's 
        '	value must be CONA_APPLICATION_TYPE_JAVA.
        '
        ' Parameters:
        '	hFSHandle			[in] File System handle
        '	iApplicationType	[in] Used struct type: CONA_APPLICATION_TYPE_SIS or 
        '											   CONA_APPLICATION_TYPE_JAVA
        '	pApplicationStruct	[in] Pointer to CONAPI_APPLICATION_SIS or 
        '							 CONAPI_APPLICATION_JAVA struct.
        '	iOptions			[in] Options: 
        '		CONA_DEFAULT_FOLDER: Copies the application to the device's default 
        '		application folder automatically and starts the device's installer 
        '		(if required to do so).Target path or current folder is not used.  
        '		CONA_OVERWRITE:	Overwrites the application file if it already 
        '		exists in the target folder.
        '
        '	pstrSourcePath		[in] Source folder path on the PC. The value must be NULL, 
        '							 if path is given with file name(s) in Application structure.
        '	pstrTargetPath   	[in] Target folder.If NULL, current folder is used.
        '							 If CONA_DEFAULT_FOLDER is used,Application will 
        '							 be installed to device's default application folder. 
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_INVALID_HANDLE
        ' ECONA_INVALID_PARAMETER
        ' ECONA_CONNECTION_BUSY
        ' ECONA_CONNECTION_LOST
        ' ECONA_INVALID_DATA_DEVICE
        ' ECONA_MEMORY_FULL
        ' ECONA_CURRENT_FOLDER_NOT_FOUND
        ' ECONA_FOLDER_PATH_TOO_LONG
        ' ECONA_FOLDER_NOT_FOUND
        ' ECONA_FOLDER_NO_PERMISSION_ON_PC
        ' ECONA_FILE_TOO_BIG_DEVICE
        ' ECONA_FILE_NAME_INVALID
        ' ECONA_FILE_NAME_TOO_LONG
        ' ECONA_FILE_TYPE_NOT_SUPPORTED
        ' ECONA_FILE_NOT_FOUND
        ' ECONA_FILE_ALREADY_EXIST
        ' ECONA_FILE_NO_PERMISSION
        ' ECONA_FILE_NO_PERMISSION_ON_PC
        ' ECONA_FILE_BUSY
        ' ECONA_DEVICE_INSTALLER_BUSY
        ' ECONA_CANCELLED
        ' ECONA_FAILED_TIMEOUT
        ' ECONA_FILE_TYPE_NOT_SUPPORTED
        ' ECONA_NOT_SUPPORTED_DEVICE
        ' ECONA_UNKNOWN_ERROR_DEVICE
        ' ECONA_NOT_INITIALIZED
        ' ECONA_NOT_SUPPORTED_MANUFACTURER
        ' ECONA_UNKNOWN_ERROR

        '"CONAInstallApplication"
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAInstallApplication(ByVal hFSHandle As Integer, ByVal iApplicationType As Integer, ByRef pApplicationStruct As CONAPI_APPLICATION_JAVA, ByVal iOptions As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrSourcePath As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function

        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAInstallApplication(ByVal hFSHandle As Integer, ByVal iApplicationType As Integer, ByRef pApplicationStruct As CONAPI_APPLICATION_SIS, ByVal iOptions As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrSourcePath As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function

        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAInstallApplication(ByVal hFSHandle As Integer, ByVal iApplicationType As Integer, ByRef pApplicationStruct As CONAPI_APPLICATION_FILE, ByVal iOptions As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrSourcePath As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrTargetPath As String) As Integer

        End Function

        '=========================================================


        '=========================================================
        ' CONAListApplications 
        '
        ' Description:
        '	CONAListApplications function list all installed applications on 
        '	the device. FS's CONACancel function can be used to cancel 
        '	this function. 
        '	NOTE 1: Only new Series 60 3nd edition devices are supported at the moment. 
        '	        If target device is not supported function fails with error code ECONA_NOT_SUPPORTED_DEVICE.
        '	NOTE 2: If target device is supported, CONAGetDeviceInfo function returns value CONAPI_FS_LIST_APPLICATIONS.
        '	
        '
        '
        ' Parameters:
        '	hFSHandle			[in] File System handle
        '	dwOptions			[in] Options: 
        '			CONA_LIST_ALL_APPICATIONS: List all installed applications. This value must be used.
        '	pdwNumberOfAppInfoStructures [out] Number of CONAPI_APPLICATION_INFO structures. 
        '	ppAppInfoStructures			 [out] Pointer to CONAPI_APPLICATION_INFO structure(s). 
        '
        ' Return values:
        ' CONA_OK
        ' ECONA_INVALID_HANDLE
        ' ECONA_INVALID_PARAMETER
        ' ECONA_INVALID_POINTER
        ' ECONA_CONNECTION_BUSY
        ' ECONA_CONNECTION_LOST
        ' ECONA_INVALID_DATA_DEVICE
        ' ECONA_MEMORY_FULL
        ' ECONA_CANCELLED
        ' ECONA_FAILED_TIMEOUT
        ' ECONA_NOT_SUPPORTED_DEVICE
        ' ECONA_UNKNOWN_ERROR_DEVICE
        ' ECONA_NOT_INITIALIZED
        ' ECONA_NOT_SUPPORTED_MANUFACTURER
        ' ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAListApplications(ByVal hFSHandle As Integer, ByVal iOptions As Integer, ByRef NumberOfAppInfoStructures As Integer, ByRef ppAppInfoStruct As IntPtr) As Integer

        End Function

        '=========================================================
        '
        '=========================================================
        ' CONAUninstallApplication 
        '
        ' Description:
        '	CONAUninstallApplication function uninstalls application
        '	from device. FS's CONACancel function can be used to cancel 
        '	this function. 
        '	NOTE 1: Only new Series 60 3nd edition devices are supported at the moment. 
        '	        If target device is not supported, function fails with error code 
        '			ECONA_NOT_SUPPORTED_DEVICE. And if target device is supported, 
        '			CONAGetDeviceInfo function returns value CONAPI_FS_UNINSTALL_APPLICATIONS.
        '	
        ' Parameters:
        '	hFSHandle			[in] File System handle
        '	dwOptions			[in] Options: 
        '		CONA_DEFAULT_UNINSTALLATION: Default uninstallation, the user may have to finish the uninstallation from device side.
        '		If user action is needed, a maximum waiting time is 15 minutes for this user action. Whole this waiting time ConnAPI 
        '		sends File System callback nofications and dwState value is CONA_OK_BUT_USER_ACTION_NEEDED.
        '		CONA_SILENT_UNINSTALLATION: Silent uninstallation, no need user action from device side.
        '	pstrName			[in] Target application Name, see CONAPI_APPLICATION_INFO structure. This value must be set.
        '	pstrUID				[in] Target application UID, see CONAPI_APPLICATION_INFO structure. This value must be set.
        '
        ' Return values:
        ' CONA_OK
        ' CONA_OK_BUT_USER_ACTION_NEEDED
        ' ECONA_INVALID_HANDLE
        ' ECONA_INVALID_PARAMETER
        ' ECONA_INVALID_POINTER
        ' ECONA_CONNECTION_BUSY
        ' ECONA_CONNECTION_LOST
        ' ECONA_INVALID_DATA_DEVICE
        ' ECONA_FILE_NO_PERMISSION
        ' ECONA_FILE_NOT_FOUND
        ' ECONA_FILE_BUSY
        ' ECONA_MEMORY_FULL
        ' ECONA_CANCELLED
        ' ECONA_FAILED_TIMEOUT
        ' ECONA_NOT_SUPPORTED_DEVICE
        ' ECONA_UNKNOWN_ERROR_DEVICE
        ' ECONA_NOT_INITIALIZED
        ' ECONA_NOT_SUPPORTED_MANUFACTURER
        ' ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAUninstallApplication(ByVal hFSHandle As Integer, ByVal iOptions As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrAppName As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrAppUID As String) As Integer

        End Function

        '=========================================================

        '=========================================================
        ' CONAFreeApplicationInfoStructures
        '
        ' Description:
        '	CONAFreeApplicationInfoStructures releases the CONAPI_APPLICATION_INFO structs, 
        '	which CONAListApplications function is allocated.
        '
        ' Parameters:
        '	dwNumberOfAppInfoStructures	[in] Number of CONAPI_APPLICATION_INFO structures.
        '	ppAppInfoStructures	[in] Pointer to CONAPI_APPLICATION_INFO structure(s).
        '
        ' Return values:
        '	CONA_OK
        '	ECONA_INVALID_POINTER
        '	ECONA_INVALID_PARAMETER
        '	ECONA_UNKNOWN_ERROR
        '
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAFreeApplicationInfoStructures(ByVal iNumberOfAppInfoStructures As Integer, ByRef ppAppInfoStruct As IntPtr) As Integer

        End Function


    End Class

End Class