
'==============================================================================
'* Content Access API 3.2 
'*
'Filename    : DAContentAccessDefinitions.vb
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

Partial Public Class Nokia

    Partial Public Class APIS


        'Public NotInheritable Class DAContentAccessDefinitions
        '    Private Sub New()
        '    End Sub
        '//////////////////////////////////////////////////////
        ' Content Access operation type defintions 
        '
        ' Info: 
        ' Operation type definitions are used in notifications 
        ' to tell notification type
        '		
        Public Const CA_OPERATION_READ As Integer = &H1
        Public Const CA_OPERATION_WRITE As Integer = &H2
        Public Const CA_OPERATION_DELETE As Integer = &H3
        Public Const CA_OPERATION_FIELD_WRITE As Integer = &H4
        Public Const CA_OPERATION_FIELD_DELETE As Integer = &H5
        Public Const CA_OPERATION_CREATE_FOLDER As Integer = &H6
        Public Const CA_OPERATION_DELETE_FOLDER As Integer = &H7
        Public Const CA_OPERATION_RENAME_FOLDER As Integer = &H8

        '////////////////////////////////////////////////////
        ' Folder access options 
        '
        ' Info: 
        ' Definitions for folder and folder content access 
        ' rights. These are returned when item paths are 
        ' queried from device.
        '
        ' Values have following meaning: 
        '		
        ' CA_FOLDER_ACCESS_BROWSE		Permission to browse specified folder content
        ' CA_FOLDER_ACCESS_CREATE		Permission to create folder to specified folder
        ' CA_FOLDER_ACCESS_DELETE		Permission to delete folder from specified folder 
        ' CA_FOLDER_ACCESS_SEND 		Permission to use folder for sending items (sms / mms messages)
        ' CA_FOLDER_ACCESS_READ_ITEM	Permission to read items from specified folder
        ' CA_FOLDER_ACCESS_WRITE_ITEM	Permission to write items to specified folder
        ' CA_FOLDER_ACCESS_DELETE_ITEM	Permission to delete items from specified folder

        Public Const CA_FOLDER_ACCESS_BROWSE As Integer = &H1
        Public Const CA_FOLDER_ACCESS_CREATE As Integer = &H2
        Public Const CA_FOLDER_ACCESS_DELETE As Integer = &H4
        Public Const CA_FOLDER_ACCESS_SEND As Integer = &H8

        Public Const CA_FOLDER_ACCESS_READ_ITEM As Integer = &H100
        Public Const CA_FOLDER_ACCESS_WRITE_ITEM As Integer = &H200
        Public Const CA_FOLDER_ACCESS_DELETE_ITEM As Integer = &H400

        '//////////////////////////////////////////////////
        ' Options for item reading and ID listing 
        '
        ' Info: 
        ' Defines options for CAReadItem/CAGetIdList method
        ' 
        ' CA_OPTION_USE_CACHE means that cache will be used 
        ' if available. 
        '
        ' CA_OPTION_UPDATE_ITEM_STATUS flag is supported for 
        ' SMS and MMS target contents. If flag is used, message
        ' status is changed from unread to read. 
        '
        ' CA_OPTION_USE_ONLY_CACHE flag reads item or ID listing 
        ' only from cache DB. Device is not accessed at all when 
        ' this option has been selected. 
        ' 
        Public Const CA_OPTION_USE_CACHE As Integer = &H1
        Public Const CA_OPTION_UPDATE_ITEM_STATUS As Integer = &H2
        Public Const CA_OPTION_USE_ONLY_CACHE As Integer = &H4

        '//////////////////////////////////////////////////
        ' Options for item writing
        '
        ' Info: 
        ' Defines options for CAWriteItem operation
        ' 
        ' CA_OPTION_REQUEST_MSG_DELIVERY will request for 
        ' delivery reports when sending messages. This option
        ' is disabled in other content types. 
        '
        ' Currently supported only in Series 40
        ' 
        ' 
        Public Const CA_OPTION_REQUEST_MSG_DELIVERY As Integer = &H1

        '=========================================================
        ' Structure definitions 
        '=========================================================
        ' CA_FOLDER_INFO
        '
        ' Description:
        ' Folder structure for the specific content type. For some 
        ' content types, sub folder structures can be returned also.
        '  
        ' Parameters:
        ' dwSize				sizeof(CA_FOLDER_INFO), must be set! 
        ' dwFolderId			Folder ID 
        ' pstrName				Name of the folder 
        ' pstrPath				Complete path for the folder
        ' dwSubFolderCount		Amount of sub folders in folder
        ' pSubFolders			Sub folder array 
        ' pParent				Parent folder for current folder
        '
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)>
        Public Structure CA_FOLDER_INFO
            Public iSize As Integer
            Public iFolderId As Integer
            Public iOptions As Integer
            '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrName As String
            '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrPath As String
            Private _pstrName As IntPtr
            Private _pstrPath As IntPtr

            Public iSubFolderCount As Integer
            Public pSubFolders As IntPtr
            Public pParent As IntPtr


            Public Property pstrName As String
                Get
                    Return PtrToStr(Me._pstrName)
                End Get
                Set(value As String)
                    Me._pstrName = StrToPtr(value)
                End Set
            End Property

            Public Property pstrPath As String
                Get
                    Return PtrToStr(Me._pstrPath)
                End Get
                Set(value As String)
                    Me._pstrPath = StrToPtr(value)
                End Set
            End Property
        End Structure

      
        '=========================================================
        ' CA_ITEM_ID
        '
        ' Description:
        ' ID for the specific content item.
        '  
        ' Parameters:
        ' dwSize			sizeof(CA_ITEM_ID), must be set! 
        ' dwFolderId		Target Folder for the operations, referencing to dwFolderId
        '					member in CA_FOLDER_INFO
        ' dwTemporaryID	Temporary operation ID for write and delete operations, can
        '					be used when mapping with operation statuses
        ' dwUidLen			Lenght of the UID data 
        ' pbUid			UID of the item
        ' dwStatus			Status information for the operation
        ' 
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)>
        Public Structure CA_ITEM_ID
            Dim iSize As Integer
            Dim iFolderId As Integer
            Dim iTemporaryID As Integer
            Dim iUidLen As Integer
            Dim pbUid As IntPtr
            Dim iStatus As Integer
        End Structure

        '=========================================================
        ' CA_ID_LIST
        '
        ' Description:
        ' Structure containing content specific ID list.
        '  
        ' Parameters:
        ' dwSize			sizeof(CA_ID_LIST), must be set! 		
        ' dwUIDCount		Amount of Id's in pUIDs list.	
        ' pUIDs			Actual UID data
        '
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Public Structure CA_ID_LIST
            Dim iSize As Integer
            Dim iUIDCount As Integer
            Dim pUIDs As IntPtr
        End Structure

        '=========================================================
        ' CANotifyCallbackFunction
        '
        '	This is the function prototype of the callback method
        '
        '	DWORD CANotifyCallbackFunction( CAHANDLE hCAHandle, DWORD dwReason,CA_ITEM_ID *pItemID);
        '
        ' Parameters:
        ' hCAHandle	Handle to opened CA connection
        ' dwReason		Reason for notification. 
        ' dwParam		For future use, this is set to zero
        ' pItemID		Item ID information for notification.
        '	
        Public Delegate Function CANotifyCallbackDelegate(ByVal hCAHandle As Integer, ByVal iReason As Integer, ByVal iParam As Integer, ByVal pItemID As IntPtr) As Integer

        '=========================================================
        ' CAOperationCallbackFunction
        '
        '	This is the function prototype of the callback method
        '
        '	DWORD CAOperationCallbackFunction(CAOPERATIONHANDLE hOperHandle,DWORD dwOperation, DWORD dwCurrent, DWORD dwTotalAmount,CA_ITEM_ID * pItemID);
        '
        ' Parameters:
        ' hOperHandle		Handle to started operation
        ' dwOperation		Operation info
        ' dwCurrent		Operation progress 
        ' dwTotalAmount	Total amount of operations to perform
        ' dwStatus			Contains percentage value of the operation progress.
        ' pItemID			Item ID information related to this operation
        '		
        Public Delegate Function CAOperationCallbackDelegate(ByVal hOperHandle As Integer, ByVal iOperation As Integer, ByVal iCurrent As Integer, ByVal iTotalAmount As Integer, ByVal iStatus As Integer, ByVal pItemID As IntPtr) As Integer

        ' Reason definitions 
        Public Const CA_REASON_ENUMERATING As Integer = &H1
        Public Const CA_REASON_ITEM_ADDED As Integer = &H2
        Public Const CA_REASON_ITEM_DELETED As Integer = &H3
        Public Const CA_REASON_ITEM_UPDATED As Integer = &H4
        Public Const CA_REASON_ITEM_MOVED As Integer = &H5
        Public Const CA_REASON_ITEM_REPLACED As Integer = &H6
        Public Const CA_REASON_CONNECTION_LOST As Integer = &H7
        Public Const CA_REASON_MSG_DELIVERY As Integer = &H8

    End Class

End Class


