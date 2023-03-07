'Filename    : CONADeviceManagement.vb
'Part of     : PCSAPI VB.NET examples
'Description : Device management API, converted from CONADeviceManagement.h
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
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Partial Public Class Nokia

    Partial Public Class APIS


        'Public NotInheritable Class CONADeviceManagement
        '    Private Sub New()
        '    End Sub
        '///////////////////////////////////////////////////////////
        '// Device management API
        '///////////////////////////////////////////////////////////

        '	//=========================================================
        '	// Device Management API versions 
        '	//
        Public Const DMAPI_VERSION_30 As Short = 30
        Public Const DMAPI_VERSION_31 As Short = 31
        Public Const DMAPI_VERSION_32 As Short = 32
        '	//=========================================================

        '	//=========================================================
        '	// DMAPI_Initialize
        '	//
        '	// Description:
        '	//	DMAPI_Initialize initializes the API. This must be called once and before any other DMAPI call!
        '	//  It's not allowed to call this function like this 
        '	//		DMAPI_Initialize(DMAPI_GetAPIVersion(), NULL);
        '	//	You must call it like this
        '	//		DMAPI_Initialize(DMAPI_VERSION_30, NULL);
        '	//
        '	// Parameters:
        '	//	dwAPIVersion	[in] DMAPI version requested.
        '	//	pdwParam		[in] Reserved for future use. Must be NULL.
        '	//
        '	// Return values:
        '	//
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function DMAPI_Initialize(ByVal dwAPIVersion As Integer, ByVal pdwParam As Integer) As Integer

        End Function

        '	//=========================================================

        '	//=========================================================
        '	// DMAPI_Terminate
        '	//
        '	// Description:
        '	//	DMAPI_Terminate terminates the API. This must be called once and as the last DMAPI call!
        '	//
        '	// Parameters:
        '	//	pdwParam		[in] Reserved for future use. Must be NULL.
        '	//
        '	// Return values:
        '	//
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function DMAPI_Terminate(ByVal iValue As Integer) As Integer

        End Function

        '	//=========================================================

        '	//=========================================================
        '	// DMAPI_GetAPIVersion
        '	//
        '	// Description:
        '	//	Returns currently installed version of this DMAPI. 	
        '	//
        '	// Parameters:
        '	//
        '	// Return values:
        '	//	API version number. 
        '	//
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function DMAPI_GetAPIVersion() As Integer

        End Function

        '	//=========================================================

        '///////////////////////////////////////////////////////////
        '// Device management API
        '///////////////////////////////////////////////////////////

        '   //=========================================================
        '   // CONAOpenDM
        '   //
        '   // Description:
        '   //  Returns the handle to the device manager
        '   //
        '   // Parameters:
        '   //  phDMHandle      [out] Device manager handle
        '   //
        '   // Return values:
        '   // CONA_OK
        '   // ECONA_INVALID_POINTER
        '   // ECONA_NOT_INITIALIZED
        '   // ECONA_INIT_FAILED_COM_INTERFACE
        '   // ECONA_UNKNOWN_ERROR
        '   //
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAOpenDM(ByRef hDMHandle As Integer) As Integer

        End Function

        '   //=========================================================

        '   //=========================================================
        '   // CONACloseDM
        '   //
        '   // Description:
        '   //  Closes the handle to the device manager
        '   //
        '   // Parameters:
        '   //  hDMHandle       [in] Device manager handle
        '   //
        '   // Return values:
        '   // CONA_OK
        '   // ECONA_INVALID_POINTER
        '   // ECONA_NOT_INITIALIZED
        '   // ECONA_UNKNOWN_ERROR
        '   //
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONACloseDM(ByVal hDMHandle As Integer) As Integer

        End Function

        '   //=========================================================

        '   //=========================================================
        '   // CONAGetDeviceCount
        '   //
        '   // Description:
        '   //  Returns number of available devices
        '   //
        '   // Parameters:
        '   //  hDMHandle       [in]  Device manager handle
        '   //  iCount          [out] Number of devices
        '   //
        '   // Return values:
        '   // CONA_OK
        '   // ECONA_INVALID_POINTER
        '   // ECONA_NOT_INITIALIZED
        '   // ECONA_UNKNOWN_ERROR
        '   //
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CONAGetDeviceCount(ByVal hDMHandle As IntPtr, ByRef iCount As Integer) As Integer

        End Function
        '   //=========================================================

        '   //=========================================================
        '   // CONAGetDevices
        '   //
        '   // Description:
        '   //  Returns needed number of devices
        '   //
        '   // Parameters:
        '   //  hDMHandle       [in]  Device manager handle
        '   //  iCount          [in,out] In: Number of allocated CONAPI_DEVICE structs.
        '   //                           Out: Number of used CONAPI_DEVICE structs.
        '   // pDevices         [out]    Pointer to receiving CONAPI_DEVICE structures.
        '   //
        '   // Return values:
        '   // CONA_OK
        '   // ECONA_INVALID_POINTER
        '   // ECONA_INVALID_PARAMETER
        '   // ECONA_NOT_INITIALIZED
        '   // ECONA_UNKNOWN_ERROR
        '   //
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CONAGetDevices(ByVal hDMHandle As IntPtr, ByRef iCount As Integer, ByVal pDevices As IntPtr) As Integer

        End Function

        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)>
        Public Shared Function CONAGetDevices(ByVal hDMHandle As IntPtr, ByRef iCount As Integer, ByRef pDevices As CONAPI_DEVICE) As Integer

        End Function

        '   //=========================================================

        '   //=========================================================
        '   // CONAGetDevice
        '   //
        '   // Description:
        '   //  Returns information about selected device
        '   //
        '   // Parameters:
        '   //  hDMHandle           [in]  Device manager handle
        '   //  pstrSerialNumber    [in]  Serial number of the device
        '   //  pDevice             [out] Pointer to device struct
        '   //
        '   // Return values:
        '   // CONA_OK
        '   // ECONA_INVALID_POINTER
        '   // ECONA_DEVICE_NOT_FOUND
        '   // ECONA_NOT_INITIALIZED
        '   // ECONA_UNKNOWN_ERROR
        '   //
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAGetDevice(ByVal hDMHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrSerialNumber As String, ByVal pDevice As IntPtr) As Integer

        End Function

        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAGetDevice(ByVal hDMHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrSerialNumber As String, ByRef pDevice As CONAPI_DEVICE) As Integer

        End Function

        '   //=========================================================

        '   //=========================================================
        '   // CONAFreeDeviceStructure
        '   //
        '   // Description:
        '   //  CONAFreeDeviceStructure release the memory, which
        '   //  ConnectivitAPI is allocated inside CONAPI_DEVICE structs.
        '   //
        '   // Parameters:
        '   //  iCount          [in] Number of used CONAPI_DEVICE structs
        '   //  pDevices        [in] Pointer to CONAPI_DEVICE struct list
        '   //
        '   // Return values:
        '   //  CONA_OK
        '   //  ECONA_INVALID_POINTER
        '   //  ECONA_INVALID_PARAMETER
        '   //  ECONA_UNKNOWN_ERROR
        '   //
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAFreeDeviceStructure(ByVal iCount As Integer, ByVal pDevices As IntPtr) As Integer

        End Function

        '   //=========================================================

        '   //=========================================================
        '   // CONARefreshDeviceList
        '   //
        '   // Description:
        '   //  Starts device list refreshing. All changes are notified throught the
        '   //  notifications.
        '   //
        '   // Parameters:
        '   //  hDMHandle       [in] Device manager handle
        '   //  iValue          [in] Reserved for future use. Must be zero.
        '   //
        '   // Return values:
        '   // CONA_OK
        '   // ECONA_INVALID_PARAMETER
        '   // ECONA_NOT_INITIALIZED
        '   // ECONA_UNKNOWN_ERROR

        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONARefreshDeviceList(ByVal hDMHandle As Integer, ByVal iValue As Integer) As Integer

        End Function

        '   //=========================================================

        '   //=========================================================
        '   // CONARenameFriendlyName
        '   //
        '   // Description:
        '   //  Sets a new friendly name for the device
        '   //
        '   // Parameters:
        '   //  hDMHandle           [in] Device manager handle
        '   //  pstrSerialNumber    [in] Serial number of the device.
        '   //  pstrNewFriendlyName [in] New Device Friendly Name .
        '   //
        '   // Return values:
        '   // CONA_OK
        '   // ECONA_INVALID_POINTER
        '   // ECONA_NAME_ALREADY_EXISTS
        '   // ECONA_NOT_INITIALIZED
        '   // ECONA_UNKNOWN_ERROR
        '   //
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONARenameFriendlyName(ByVal hDMHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrSerialNumber As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrNewFriendlyName As String) As Integer

        End Function


        '   //=========================================================

        '   //=========================================================
        '   // CONARegisterNotifyCallback
        '   //
        '   // Description:
        '   //  Registers notification call back function to connectivity API
        '   //
        '   // Parameters:
        '   //  hDMHandle       [in] Device manager handle
        '   //  iState          [in] Used to define the action
        '   //                       API_REGISTER used in registeration
        '   //                       API_REGISTER used in removing the registeration
        '   //  pfnNotify       [in] Function pointer of the call back method
        '   //
        '   // Return values:
        '   // CONA_OK
        '   // ECONA_INVALID_POINTER
        '   // ECONA_INVALID_PARAMETER
        '   // ECONA_NOT_INITIALIZED
        '   // ECONA_UNKNOWN_ERROR
        '   //
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONARegisterNotifyCallback(ByVal hDMHandle As Integer, ByVal iState As Integer, ByVal pfnNotify As DeviceNotifyCallbackDelegate) As Integer

        End Function

        '   //=========================================================
        '   //=========================================================
        '   // CONASearchDevices 
        '   //
        '   // Description:
        '   //	CCONASearchDevices functions search all devices from the target media. 
        '   //  The target media that can be used is Bluetooth at the moment.
        '   //
        '   //  CONASearchDevices allocates and sets devices information to 
        '   //  CONAPI_CONNECTION_INFO structs and returns pointer to structs.  
        '   //  Connectivity API-user MUST releases the returned pointer by calling the 
        '   //  CONAFreeConnectionInfoStructures function. 
        '   //
        '   //	Every CONAPI_CONNECTION_INFO struct includes the media and device information.
        '   //	The struct's dwState parameter defines the paired and trusted information from 
        '   //  device. It has the following values:
        '   //  Parameter value					Description					Macros for check the values 
        '   //																(If value is true, macro returns 1)
        '   //	CONAPI_DEVICE_UNPAIRED			Device in not paired.		CONAPI_IS_DEVICE_UNPAIRED(dwState)
        '   //	CONAPI_DEVICE_PAIRED			Device is paired.			CONAPI_IS_DEVICE_PAIRED(dwState)
        '   //	CONAPI_DEVICE_PCSUITE_TRUSTED	Device is PC Suite trusted.	CONAPI_IS_PCSUITE_TRUSTED(dwState)
        '   //
        '   //	Connectivity API can add more values afterwards so Connectivity API-user should 
        '   //	always use defined macros to check those values!

        '   //
        '   // Parameters:
        '   //	hDMHandle				[in] Existing device management handle.
        '   //	dwSearchOptions			[in] Search options values:
        '   //			API_MEDIA_BLUETOOTH: Get devices from bluetooth media. 
        '   //				This value must be used.
        '   //			CONAPI_ALLOW_TO_USE_CACHE: Get all devices from cache if available. 
        '   //				If cache is not available function fails with error: ECONA_CACHE_IS_NOT_AVAILABLE.
        '   //				This value is optional and can be used with other values.
        '   //			One of the next values can be used at the time:
        '   //			CONAPI_GET_ALL_PHONES: Get all phones from target media. Includes unpaired, 
        '   //				paired and PC Suite trusted phones. 
        '   //			CONAPI_GET_PAIRED_PHONES:Get all paired phones from target media. Includes 
        '   //				paired (and PC Suite trusted) phones. 
        '   //			CONAPI_GET_TRUSTED_PHONES:Get all PC Suite trusted phones from target media. 
        '   //				Includes all PC Suite trusted phones, which are paired.
        '   //	dwSearchTime			[in]  Maximum search time in seconds. Note: Bluetooth device
        '   //			discovery can takes several minutes if there are a lot of devices on range!
        '   //	pfnSearchNotify			[in]  Pointer to search notification callback function. Value
        '   //			can be NULL if notification is not needed.
        '   //	pdwNumberOfStructures	[out] Number of CONAPI_CONNECTION_INFO structures. 
        '   //	ppConnInfoStructures	[out] Pointer to CONAPI_CONNECTION_INFO structure(s). 

        '   //	hDMHandle			[in] Device manager handle
        '   //	pstrSerialNumber	[in] Serial number of the device.
        '   //	pstrNewFriendlyName [in] New Device Friendly Name .
        '   //
        '   // Return values:
        '   // CONA_OK
        '   // ECONA_INVALID_POINTER
        '   // ECONA_INVALID_HANDLE
        '   // ECONA_DEVICE_NOT_FOUND
        '   // ECONA_FAILED_TIMEOUT
        '   // ECONA_NO_CONNECTION_VIA_MEDIA
        '   // ECONA_MEDIA_IS_NOT_WORKING
        '   // ECONA_CACHE_IS_NOT_AVAILABLE
        '   // ECONA_SUSPEND
        '   // ECONA_NOT_ENOUGH_MEMORY
        '   // ECONA_NOT_INITIALIZED
        '   // ECONA_NOT_SUPPORTED_PC
        '   // ECONA_CANCELLED
        '   // ECONA_UNKNOWN_ERROR

        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONASearchDevices(ByVal hDMHandle As Integer, ByVal dwSearchOptions As Integer, ByVal dwSearchTime As Integer, ByVal pfnSearchNotify As SearchCallbackDelegate, ByRef pdwNumberOfStructures As Integer, ByRef pConnInfoStructures As IntPtr) As Integer

        End Function

        '   //=========================================================

        '   //=========================================================
        '   // CONAFreeConnectionInfoStructures
        '   //
        '   // Description:
        '   //	CONAFreeDeviceStructure releases the CONAPI_CONNECTION_INFO structs, 
        '   //	which CONASearchDevices function is allocated.
        '   //
        '   // Parameters:
        '   //	dwNumberOfStructures	[in] Number of CONAPI_CONNECTION_INFO structures.
        '   //	ppConnInfoStructures	[in] Pointer to CONAPI_CONNECTION_INFO structure(s).
        '   //
        '   // Return values:
        '   //	CONA_OK
        '   //	ECONA_INVALID_POINTER
        '   //	ECONA_INVALID_PARAMETER
        '   //	ECONA_UNKNOWN_ERROR

        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAFreeConnectionInfoStructures(ByVal dwNumberOfStructures As Integer, ByRef pConnInfoStructures As IntPtr) As Integer

        End Function

        '   //=========================================================

        '   //=========================================================
        '   // CONAChangeDeviceTrustedState
        '   //
        '   // Description:
        '   //	CONAChangeDeviceTrustedState functions changes device's 
        '   //	trusted state. It has the following operation values:
        '   //	Value							Description
        '   //	CONAPI_PAIR_DEVICE				Pair device.
        '   //	CONAPI_UNPAIR_DEVICE			Unpair device from Bluetooth stack.
        '   //	CONAPI_SET_PCSUITE_TRUSTED		Set device to PC Suite trusted state.
        '   //		System recognise the device and sets it to trusted state. Connectivity 
        '   //		API starts sends device notification from device.Note: Device must be 
        '   //		paired. Value can be used with CONAPI_PAIR_DEVICE. 
        '   //	CONAPI_SET_PCSUITE_UNTRUSTED	Remove PC Suite trusted information from 
        '   //		System. Connectivity API does not send device notification from device 
        '   //		anymore.Note: Device can be paired or unpaired state. Value can be used 
        '   //		with CONAPI_UNPAIR_DEVICE.

        '   //
        '   // Parameters:
        '   //	hDMHandle			[in] Existing device management handle.
        '   //	dwTrustedOperation	[in] Operation values: 
        '   //								CONAPI_PAIR_DEVICE
        '   //								CONAPI_UNPAIR_DEVICE
        '   //								CONAPI_SET_PCSUITE_TRUSTED 
        '   //								CONAPI_SET_PCSUITE_UNTRUSTED
        '   //	pstrAddress			[in] Device address. If device is connected via Bluetooth 
        '   //				media, address must be Device BT address (see pstrAddress parameter 
        '   //				from CONAPI_CONNECTION_INFO Structure)
        '   //	pstrPassword		[in] Password string for pairing. String can include only 
        '   //				the numbers (0-9) characters.Value used only with CONAPI_PAIR_DEVICE 
        '   //				operation. With other operations value should be NULL.
        '   //	pstrName			[in] Reserved for future use, the value must be NULL.

        '   //
        '   // Return values:
        '   //	CONA_OK
        '   //	ECONA_INVALID_POINTER
        '   //	ECONA_INVALID_HANDLE
        '   //	ECONA_DEVICE_NOT_FOUND
        '   //	ECONA_NOT_SUPPORTED_DEVICE
        '   //	ECONA_CONNECTION_FAILED
        '   //	ECONA_CONNECTION_FAILED
        '   //	ECONA_CONNECTION_BUSY
        '   //	ECONA_CONNECTION_LOST
        '   //	ECONA_DEVICE_PAIRING_FAILED
        '   //	ECONA_DEVICE_ PASSWORD_WRONG
        '   //	ECONA_DEVICE_ PASSWORD_INVALID
        '   //	ECONA_FAILED_TIMEOUT
        '   //	ECONA_NO_CONNECTION_VIA_MEDIA
        '   //	ECONA_MEDIA_IS_NOT_WORKING
        '   //	ECONA_SUSPEND
        '   //	ECONA_NOT_ENOUGH_MEMORY
        '   //	ECONA_NOT_INITIALIZED
        '   //	ECONA_NOT_SUPPORTED_PC
        '   //	ECONA_UNKNOWN_ERROR


        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAChangeDeviceTrustedState(ByVal hDMHandle As Integer, ByVal dwTrustedOperation As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrAddress As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrPassword As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrName As String) As Integer

        End Function

        '   //=========================================================
        '
        '   //=========================================================
        '   // CONAGetDeviceInfo
        '   //
        '   // Description:
        '   //	CONAGetDeviceInfo function sets the device specific information to the structure. 
        '   //  The structure type and allocation depends on which information is requested.
        '   //	The structure must be freed or clear by calling the CONAFreeDeviceInfoStructure function.
        '   //
        '   // Parameters:
        '   //	hDMHandle			[in]  Existing device management handle.
        '   //	pstrSerialNumber	[in]  Device's serial number.
        '   //	dwStructureType		[in]  Structure type value:
        '   //								CONAPI_DEVICE_GENERAL_INFO
        '   //								CONAPI_DEVICE_PRODUCT_INFO
        '   //								CONAPI_DEVICE_PROPERTIES_INFO
        '   //								CONAPI_DEVICE_ICON_INFO
        '   //	ppStructure			[out] Pointer to the information structure.
        '
        '   // 
        '   // Return values:
        '   // CONA_OK
        '   // ECONA_INVALID_POINTER
        '   // ECONA_INVALID_PARAMETER
        '   // ECONA_INVALID_HANDLE
        '   // ECONA_DEVICE_NOT_FOUND
        '   // ECONA_NOT_INITIALIZED
        '   // ECONA_NOT_SUPPORTED_DEVICE
        '   // ECONA_NOT_SUPPORTED_MANUFACTURER
        '   // ECONA_NOT_ENOUGH_MEMORY
        '   // ECONA_UNKNOWN_ERROR
        '   //
        '	PCCS_DMAPI CONAGetDeviceInfo(
        '						DMHANDLE	 hDMHandle, 
        '						const WCHAR* pstrSerialNumber, 
        '						DWORD		 dwStructureType,
        '						LPVOID*		 ppStructure
        '						);
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAGetDeviceInfo(ByVal hDMHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrSerialNumber As String, ByVal dwStructureType As Integer, ByRef ppDeviceInfo As IntPtr) As Integer

        End Function

        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAGetDeviceInfo(ByVal hDMHandle As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrSerialNumber As String, ByVal dwStructureType As Integer, ByRef ppDeviceInfo As CONAPI_DEVICE) As Integer

        End Function


        '   //========================================================= 
        '
        '   //=========================================================
        '   // CONAFreeDeviceInfoStructure
        '   //
        '   // Description:
        '   //	CONAFreeDeviceInfoStructure releases the (e.g.  
        '   //  CONAPI_DEVICE_GEN_INFO) structure that CONAGetDeviceInfo function has allocated.
        '   //
        '   // Parameters:
        '   //	dwStructureType	[in] Structure type value:
        '   //							CONAPI_DEVICE_GENERAL_INFO
        '   //							CONAPI_DEVICE_PRODUCT_INFO
        '   //							CONAPI_DEVICE_PROPERTIES_INFO
        '   //							CONAPI_DEVICE_ICON_INFO
        '   //	pStructure		[in] Pointer to the structure that the CONAGetDeviceInfo function has allocated
        '   //
        '   // Return values:
        '   //	CONA_OK
        '   //	ECONA_INVALID_POINTER
        '   //	ECONA_INVALID_PARAMETER
        '   //	ECONA_UNKNOWN_ERROR
        '   //
        '	PCCS_DMAPI CONAFreeDeviceInfoStructure( DWORD	dwStructureType, LPVOID	pStructure );
        <DllImport("ConnAPI", CharSet:=CharSet.Unicode)> _
        Public Shared Function CONAFreeDeviceInfoStructure(ByVal dwStructureType As Integer, ByVal pDeviceInfo As IntPtr) As Integer

        End Function

        '   //=========================================================

    End Class

End Class
