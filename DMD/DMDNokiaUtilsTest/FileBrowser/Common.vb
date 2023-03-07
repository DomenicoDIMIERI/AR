'Filename    : Common.vb
'Part of     : Phone Navigator VB.NET example
'Description : Main module of VBFileBrowser.NET example application
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

Imports System.Runtime.InteropServices

Module Common
    Public Const PHONELIST_STATE_PHONELIST As Short = 1
    Public Const PHONELIST_STATE_PHONECONTENT As Short = 2

    Public MainForm As FileBrowser
    Public pDeviceCallBack As DeviceNotifyCallbackDelegate

    Public Structure SYSTEMTIME
        Dim wYear As UInt16
        Dim wMonth As UInt16
        Dim wDayOfWeek As UInt16
        Dim wDay As UInt16
        Dim wHour As UInt16
        Dim wMinute As UInt16
        Dim wSecond As UInt16
        Dim wMilliseconds As UInt16
    End Structure

    'Public Declare Function SystemTimeToFileTime Lib "kernel32" (ByRef lpSystemTime As SYSTEMTIME, ByRef lpFileTime As System.Runtime.InteropServices.ComTypes.FILETIME) As Integer
    Public Declare Function FileTimeToSystemTime Lib "kernel32" (ByRef lpFileTime As System.Runtime.InteropServices.ComTypes.FILETIME, ByRef lpSystemTime As SYSTEMTIME) As Integer

    '===================================================================
    ' DeviceNotifyCallback
    '
    ' Callback function for device connection notifications
    '
    '===================================================================
    Public Function DeviceNotifyCallback(ByRef iStatus As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal pstrSerialNumber As String) As Integer
        On Error Resume Next

        DeviceNotifyCallback = CONA_OK
        If MainForm.LBX_PhoneFiles.GetState = PHONELIST_STATE_PHONELIST Then
            MainForm.bRefreshPhoneListBox = True
        Else
            If GET_CONAPI_CB_STATUS(iStatus) = CONAPI_DEVICE_REMOVED Then
                MainForm.bRefreshPhoneListBox = (pstrSerialNumber = MainForm.LBX_PhoneFiles.GetCurrentSN())
            End If
        End If
    End Function
End Module
