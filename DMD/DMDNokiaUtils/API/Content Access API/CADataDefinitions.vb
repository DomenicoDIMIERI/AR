'==============================================================================
'* Nokia CA Data Definitions 
'*
'*  Copyright (c) 2005-2007 Nokia Corporation.
'*  This material, including documentation and any related 
'*  computer programs, is protected by copyright controlled by 
'*  Nokia Corporation. All rights are reserved. Copying, 
'*  including reproducing, storing, adapting or translating, any 
'*  or all of this material requires the prior written consent of 
''*  Nokia Corporation. This material also contains confidential 
'*  information which may not be disclosed to others without the 
'*  prior written consent of Nokia Corporation.
'* 
'==============================================================================

Option Strict Off
Option Explicit On 

Imports System.Runtime.InteropServices

Partial Public Class Nokia

    Partial Public Class APIS

        'Public NotInheritable Class CADataDefinitions
        '    Private Sub New()
        '    End Sub


        '=========================================================
        'Constants and definitions for API 
        '=========================================================

        '////////////////////////////////////////////////////
        ' Available CA connection targets
        '
        ' Info: 
        ' PIM connection targets
        ' 
        ' 
        Public Const CA_TARGET_CONTACTS As Integer = &H1
        Public Const CA_TARGET_CALENDAR As Integer = &H2
        Public Const CA_TARGET_NOTES As Integer = &H3
        Public Const CA_TARGET_SMS_MESSAGES As Integer = &H4
        Public Const CA_TARGET_MMS_MESSAGES As Integer = &H5
        Public Const CA_TARGET_BOOKMARKS As Integer = &H6

        '////////////////////////////////////////////////////
        ' Available data formats 
        '
        ' Info: 
        ' Data formats. 	
        ' 
        Public Const CA_DATA_FORMAT_STRUCT As Integer = &H1
        Public Const CA_DATA_FORMAT_VERSIT As Integer = &H2


        '////////////////////////////////////////////////////
        '
        ' Info
        ' Macros for data handling in CA_DATA_MSG 
        '
        Public Shared Function CA_GET_DATA_FORMAT(ByVal Info As Integer) As Integer
            CA_GET_DATA_FORMAT = Info And &HF
        End Function

        Public Shared Function CA_GET_DATA_CODING(ByVal Info As Integer) As Integer
            CA_GET_DATA_CODING = Info And &HF0
        End Function

        Public Shared Function CA_GET_MESSAGE_STATUS(ByVal Info As Integer) As Integer
            CA_GET_MESSAGE_STATUS = (Info And &HF00) >> 8
        End Function

        Public Shared Function CA_GET_MESSAGE_TYPE(ByVal Info As Integer) As Integer
            CA_GET_MESSAGE_TYPE = (Info And &HF000) >> 12
        End Function

        Public Shared Sub CA_SET_DATA_FORMAT(ByRef Info As Integer, ByVal Value As Integer)
            Info = Info Or (Value And &HF)
        End Sub

        Public Shared Sub CA_SET_DATA_CODING(ByRef Info As Integer, ByVal Value As Integer)
            Info = Info Or (Value And &HF0)
        End Sub

        Public Shared Sub CA_SET_MESSAGE_STATUS(ByRef Info As Integer, ByVal Value As Integer)
            Info = Info Or ((Value And &HF) << 8)
        End Sub

        Public Shared Sub CA_SET_MESSAGE_TYPE(ByRef Info As Integer, ByVal Value As Integer)
            Info = Info Or ((Value And &HF) << 12)
        End Sub

        '///////////////////////////////////////////////////
        '
        ' Info
        ' Macros for data dwRecurrence in CA_DATA_CALENDAR
        '
        Public Shared Function CA_GET_RECURRENCE(ByVal iInfo As Integer) As Integer
            CA_GET_RECURRENCE = iInfo And &HFF
        End Function

        Public Shared Sub CA_SET_RECURRENCE(ByRef iInfo As Integer, ByVal iValue As Integer)
            iInfo = iInfo Or iValue
        End Sub

        Public Shared Function CA_GET_RECURRENCE_INTERVAL(ByVal iInfo As Integer) As Integer
            CA_GET_RECURRENCE_INTERVAL = (iInfo >> 8) And &HFF
        End Function

        Public Shared Sub CA_SET_RECURRENCE_INTERVAL(ByRef iInfo As Integer, ByVal iValue As Integer)
            iInfo = iInfo Or (iValue << 8)
        End Sub

        '////////////////////////////////////////////////////
        ' Input data format definitions
        '
        ' Info: 
        ' This format is used in CA_DATA_MSG structure's 
        ' dwInfoField parameter to tell the format of the data
        ' in data buffer
        '
        ' Also used in CA_DATA_VERSIT structure's dwInfoField	
        ' to inform about format of the data in structure
        ' 
        Public Const CA_DATA_FORMAT_UNICODE As Integer = &H1
        Public Const CA_DATA_FORMAT_DATA As Integer = &H2

        '////////////////////////////////////////////////////
        ' Input data coding definitions 
        '
        ' Info: 
        ' This format is used in CA_DATA_MSG structure 
        ' to inform about the coding of the data 
        ' 
        Public Const CA_DATA_CODING_7BIT As Integer = &H10
        Public Const CA_DATA_CODING_8BIT As Integer = &H20
        Public Const CA_DATA_CODING_UNICODE As Integer = &H40

        '////////////////////////////////////////////////////
        ' Message type definitions
        '
        ' Info: 
        ' Message type definitions 
        ' 
        Public Const CA_SMS_DELIVER As Integer = &H1
        Public Const CA_SMS_SUBMIT As Integer = &H3

        '//////////////////////////////////////////////////
        ' Message folder definitions
        '
        ' Info: 
        ' Message folder definitions. 
        ' 
        ' Folder ID definitions 
        Public Const CA_MESSAGE_FOLDER_ROOT As Integer = &H0
        Public Const CA_MESSAGE_FOLDER_INBOX As Integer = &H1
        Public Const CA_MESSAGE_FOLDER_OUTBOX As Integer = &H2
        Public Const CA_MESSAGE_FOLDER_SENT As Integer = &H3
        Public Const CA_MESSAGE_FOLDER_ARCHIVE As Integer = &H4
        Public Const CA_MESSAGE_FOLDER_DRAFTS As Integer = &H5
        Public Const CA_MESSAGE_FOLDER_TEMPLATES As Integer = &H6
        Public Const CA_MESSAGE_FOLDER_USER_FOLDERS As Integer = &H10



        '//////////////////////////////////////////////////
        ' Phonebook memory definitions 
        '
        ' Info: 
        ' Phonebook memory definitions 
        ' 
        ' Folder ID definitions 
        Public Const CA_PHONEBOOK_MEMORY_PHONE As Integer = &H1
        Public Const CA_PHONEBOOK_MEMORY_SIM As Integer = &H2

        '////////////////////////////////////////////////////
        ' Message status definitions ..
        '
        ' Info: 
        ' SMS Message status definitions .. 
        ' 
        Public Const CA_MESSAGE_STATUS_SENT As Integer = &H2        ' Message has been sent 
        Public Const CA_MESSAGE_STATUS_UNREAD As Integer = &H4      ' Message hasn't been read 
        Public Const CA_MESSAGE_STATUS_READ As Integer = &H5        ' Message has been read 
        Public Const CA_MESSAGE_STATUS_DRAFT As Integer = &H6       ' Message is a draft 
        Public Const CA_MESSAGE_STATUS_PENDING As Integer = &H7     ' Message is pending 
        Public Const CA_MESSAGE_STATUS_DELIVERED As Integer = &H9   ' Message has been delivered 
        Public Const CA_MESSAGE_STATUS_SENDING As Integer = &HC     ' Message is being sent

        '////////////////////////////////////////////////////
        ' Address type definitions 
        '
        ' Info: 
        ' Type of address passed in address structure
        ' 
        Public Const CA_MSG_ADDRESS_TYPE_EMAIL As Integer = &H1
        Public Const CA_MSG_ADDRESS_TYPE_NUMBER As Integer = &H2

        '////////////////////////////////////////////////////
        ' Calendar item type definitions 
        '
        ' Info: 
        ' Defines different calendar items. Used in CA_DATA_CALENDAR in dwInfoField.
        ' 
        Public Const CA_CALENDAR_ITEM_MEETING As Integer = &H1
        Public Const CA_CALENDAR_ITEM_CALL As Integer = &H2
        Public Const CA_CALENDAR_ITEM_BIRTHDAY As Integer = &H3
        Public Const CA_CALENDAR_ITEM_MEMO As Integer = &H4
        Public Const CA_CALENDAR_ITEM_REMINDER As Integer = &H5
        Public Const CA_CALENDAR_ITEM_NOTE As Integer = &H6
        Public Const CA_CALENDAR_ITEM_TODO As Integer = &H7


        '//////////////////////////////////////////////////////
        ' Field type definitions 
        '
        ' Info: 
        ' Field type values for data items
        '	
        ' For contacts
        Public Const CA_FIELD_TYPE_CONTACT_PI As Integer = &H1       ' Personal information
        Public Const CA_FIELD_TYPE_CONTACT_NUMBER As Integer = &H2   ' Number information
        Public Const CA_FIELD_TYPE_CONTACT_ADDRESS As Integer = &H3  ' Address information
        Public Const CA_FIELD_TYPE_CONTACT_GENERAL As Integer = &H4  ' General information
        ' For calendar
        Public Const CA_FIELD_TYPE_CALENDAR As Integer = &H10        ' Calendar item

        '//////////////////////////////////////////////////////
        'Recurrence values for calendar items 
        '
        'Info: 
        'Recurrence values to be used in calendar interfaces
        '		
        Public Const CA_CALENDAR_RECURRENCE_NONE As Integer = &H0
        Public Const CA_CALENDAR_RECURRENCE_DAILY As Integer = &H1
        Public Const CA_CALENDAR_RECURRENCE_WEEKLY As Integer = &H2
        Public Const CA_CALENDAR_RECURRENCE_MONTHLY As Integer = &H3
        Public Const CA_CALENDAR_RECURRENCE_YEARLY As Integer = &H4

        '////////////////////////////////////////////////////
        ' Calendar TODO item priority values 
        '
        ' Info: 
        '
        Public Const CA_CALENDAR_TODO_PRIORITY_HIGH As Integer = &H1
        Public Const CA_CALENDAR_TODO_PRIORITY_NORMAL As Integer = &H2
        Public Const CA_CALENDAR_TODO_PRIORITY_LOW As Integer = &H3

        '//////////////////////////////////////////////////
        ' Calendar TODO item status definitions
        '
        ' Info: 
        '
        Public Const CA_CALENDAR_TODO_STATUS_NEEDS_ACTION As Integer = &H1
        Public Const CA_CALENDAR_TODO_STATUS_COMPLETED As Integer = &H2

        '//////////////////////////////////////////////////
        ' Calendar Alarm State value definitions
        '
        ' Info: 
        ' CA_CALENDAR_ALARM_SILENT value is not supported 
        ' in S60 devices.
        '
        Public Const CA_CALENDAR_ALARM_NOT_SET As Integer = &H1
        Public Const CA_CALENDAR_ALARM_SILENT As Integer = &H2
        Public Const CA_CALENDAR_ALARM_WITH_TONE As Integer = &H3

        '////////////////////////////////////////////////////
        ' Field sub type definitions 
        '
        ' Info: 
        '	Field sub type definitions for data items.
        '													             ___Description_________|___Valid CA_DATA_ITEM member_______	
        ' Personal information								             |						|									|
        Public Const CA_FIELD_SUB_TYPE_NAME As Integer = &H1              ' Name field			|   pstrText
        Public Const CA_FIELD_SUB_TYPE_FN As Integer = &H2                ' First name			|	pstrText
        Public Const CA_FIELD_SUB_TYPE_MN As Integer = &H3                ' Midle name			|	pstrText
        Public Const CA_FIELD_SUB_TYPE_LN As Integer = &H4                ' Last name			|	pstrText
        Public Const CA_FIELD_SUB_TYPE_TITLE As Integer = &H5             ' Title				|	pstrText
        Public Const CA_FIELD_SUB_TYPE_SUFFIX As Integer = &H6            ' Suffix				|	pstrText
        Public Const CA_FIELD_SUB_TYPE_COMPANY As Integer = &H7           ' Company				|	pstrText
        Public Const CA_FIELD_SUB_TYPE_JOB_TITLE As Integer = &H8         ' Job title			|	pstrText
        Public Const CA_FIELD_SUB_TYPE_BIRTHDAY As Integer = &H9          ' Birthday				|	pCustomData as CA_DATA_DATE
        Public Const CA_FIELD_SUB_TYPE_PICTURE As Integer = &HA           ' Picture				|	pCustomData as CA_DATA_PICTURE
        Public Const CA_FIELD_SUB_TYPE_NICKNAME As Integer = &HB          ' Nickname				|	pstrText
        Public Const CA_FIELD_SUB_TYPE_FORMAL_NAME As Integer = &HC       ' Formal name			|	pstrText
        Public Const CA_FIELD_SUB_TYPE_GIVEN_NAME_PRONUNCIATION As Integer = &HD   ' Pronunciation field	|	pstrText
        Public Const CA_FIELD_SUB_TYPE_FAMILY_NAME_PRONUNCIATION As Integer = &HE  ' Pronunciation field	|	pstrText
        Public Const CA_FIELD_SUB_TYPE_COMPANY_NAME_PRONUNCIATION As Integer = &HF ' Pronunciation field	|	pstrText

        ' Numbers
        Public Const CA_FIELD_SUB_TYPE_TEL As Integer = &H20              ' Telephone			|	pstrText
        Public Const CA_FIELD_SUB_TYPE_TEL_HOME As Integer = &H21         ' Home number			|	pstrText
        Public Const CA_FIELD_SUB_TYPE_TEL_WORK As Integer = &H22         ' Work number			|	pstrText
        Public Const CA_FIELD_SUB_TYPE_TEL_PREF As Integer = &H23         ' Preferred number		|	pstrText
        Public Const CA_FIELD_SUB_TYPE_TEL_CAR As Integer = &H24          ' Car number			|	pstrText
        Public Const CA_FIELD_SUB_TYPE_TEL_DATA As Integer = &H25         ' Data number			|	pstrText
        Public Const CA_FIELD_SUB_TYPE_PAGER As Integer = &H30            ' Pager				|	pstrText

        Public Const CA_FIELD_SUB_TYPE_MOBILE As Integer = &H40           ' Mobile				|	pstrText
        Public Const CA_FIELD_SUB_TYPE_MOBILE_HOME As Integer = &H41      ' Mobile				|   pstrText
        Public Const CA_FIELD_SUB_TYPE_MOBILE_WORK As Integer = &H42      ' Mobile				|   pstrText	
        Public Const CA_FIELD_SUB_TYPE_FAX As Integer = &H50              ' Fax					|	pstrText
        Public Const CA_FIELD_SUB_TYPE_FAX_HOME As Integer = &H51         ' Fax					|   pstrText
        Public Const CA_FIELD_SUB_TYPE_FAX_WORK As Integer = &H52         ' Fax					|   pstrText

        Public Const CA_FIELD_SUB_TYPE_VIDEO As Integer = &H60            ' Video call number	|   pstrText
        Public Const CA_FIELD_SUB_TYPE_VIDEO_HOME As Integer = &H61       ' Video call number	|   pstrText
        Public Const CA_FIELD_SUB_TYPE_VIDEO_WORK As Integer = &H62       ' Video call number	|   pstrText
        Public Const CA_FIELD_SUB_TYPE_VOIP As Integer = &H70             ' Voice Over IP		|   pstrText
        Public Const CA_FIELD_SUB_TYPE_VOIP_HOME As Integer = &H71        ' Voice Over IP		|   pstrText
        Public Const CA_FIELD_SUB_TYPE_VOIP_WORK As Integer = &H72        ' Voice Over IP		|   pstrText

        ' Addresses
        Public Const CA_FIELD_SUB_TYPE_POSTAL As Integer = &H100          ' Postal address		|	pCustomData as CA_DATA_POSTAL_ADDRESS
        Public Const CA_FIELD_SUB_TYPE_POSTAL_BUSINESS As Integer = &H101 ' Business address	|	pCustomData as CA_DATA_POSTAL_ADDRESS
        Public Const CA_FIELD_SUB_TYPE_POSTAL_PRIVATE As Integer = &H102  ' Private address		|	pCustomData as CA_DATA_POSTAL_ADDRESS
        Public Const CA_FIELD_SUB_TYPE_EMAIL As Integer = &H103           ' Email address		|	pstrText
        Public Const CA_FIELD_SUB_TYPE_EMAIL_HOME As Integer = &H104      ' Email address		|	pstrText
        Public Const CA_FIELD_SUB_TYPE_EMAIL_WORK As Integer = &H105      ' Email address		|	pstrText
        Public Const CA_FIELD_SUB_TYPE_WEB As Integer = &H110             ' Web address			|	pstrText
        Public Const CA_FIELD_SUB_TYPE_WEB_HOME As Integer = &H111        ' Web address			|	pstrText
        Public Const CA_FIELD_SUB_TYPE_WEB_WORK As Integer = &H112        ' Web address			|	pstrText	
        Public Const CA_FIELD_SUB_TYPE_PTT As Integer = &H120             ' PTT address			|	pstrText
        Public Const CA_FIELD_SUB_TYPE_SIP_FOR_VIDEO As Integer = &H121   ' SIP for Video sharing|	pstrText
        Public Const CA_FIELD_SUB_TYPE_SWIS As Integer = &H130            ' SWIS				|   pstrText

        ' General fields
        Public Const CA_FIELD_SUB_TYPE_NOTE As Integer = &H200            ' Note field			|	pstrText
        Public Const CA_FIELD_SUB_TYPE_DTMF As Integer = &H201            ' DTMF field			|	pstrText
        Public Const CA_FIELD_SUB_TYPE_UID As Integer = &H202             ' UID field			|	pstrText
        Public Const CA_FIELD_SUB_TYPE_WIRELESS_VILLAGE As Integer = &H203 ' Village user ID	|	pstrText

        ' Calandar sub types
        Public Const CA_FIELD_SUB_TYPE_DESCRIPTION As Integer = &H300     ' Description			|	pstrText
        Public Const CA_FIELD_SUB_TYPE_LOCATION As Integer = &H301        ' Location				|	pstrText
        Public Const CA_FIELD_SUB_TYPE_ITEM_DATA As Integer = &H302       ' Generic Item data	|	pstrText / dwData
        Public Const CA_FIELD_SUB_TYPE_TODO_PRIORITY As Integer = &H303   ' Todo priotiy			|	dwData
        Public Const CA_FIELD_SUB_TYPE_TODO_STATUS As Integer = &H304     ' Todo status			|	dwData

        ' CA_FIELD_SUB_TYPE_ITEM_DATA	usage varies among different calendar item types.
        ' In CA_CALENDAR_ITEM_MEETING	Meeting detail information in pstrText parameter
        ' in CA_CALENDAR_ITEM_CALL,		Phone number information in pstrText
        ' In CA_CALENDAR_ITEM_BIRTHDAY	Birth year information in dwData paramter
        '////////////////////////////////////////////////////
        ' CA_DATA_DATE
        '
        ' Description:
        ' This structure contains date information in separated 
        ' members.
        '  
        ' Parameters:
        '	dwSize			Size of the structure (must be set!)
        '	wYear			Year information in four digit format (2005) 
        '	bMonth			Month information (1 - 12, january = 1) 
        '	bDay			Day of month (1-31)
        '	bHour			Hours after midnight (0-23)			
        '	bMinute			Minutes after hour (0-59)
        '	bSecond			Seconds after minute (0-59)
        '  lTimeZoneBias	Time zone bias in minutes (+120 for GMT+0200)
        '  lBias			Daylight bias.This contains value for offset in minutes 
        '					for time zone, for example +60. 

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)> _
        Public Structure CA_DATA_DATE
            Dim iSize As Integer
            Dim wYear As UInt16
            Dim bMonth As Byte
            Dim bDay As Byte
            Dim bHour As Byte
            Dim bMinute As Byte
            Dim bSecond As Byte
            Dim lTimeZoneBias As Int32
            Dim lBias As Int32

            Public Sub New(ByVal year As Integer, ByVal month As Integer, ByVal day As Integer, ByVal hour As Integer, ByVal minute As Integer, ByVal second As Integer)
                Me.iSize = Marshal.SizeOf(Me)
                Me.wYear = year
                Me.bMonth = month
                Me.bDay = day
                Me.bHour = hour
                Me.bMinute = minute
                Me.bSecond = second
            End Sub

            '===================================================================
            ' IsEmptyPIMDate
            ' 
            ' Tests CA_DATA_DATE is empty
            ' 
            '===================================================================
            Public Function IsEmptyPIMDate() As Boolean
                If Me.bDay = 0 And _
                  Me.lTimeZoneBias = 0 And _
                  Me.bHour = 0 And _
                  Me.bMinute = 0 And _
                  Me.bMonth = 0 And _
                  Me.bSecond = 0 And _
                  Me.lBias = 0 And _
                  Me.wYear.Equals(UInt16.Parse(0)) Then
                    Return True
                End If
                Return False
            End Function

            '===================================================================
            ' ConvertPIMDataDate
            ' 
            ' Converts CA_DATA_DATE to Date
            ' 
            '===================================================================
            Public Shared Narrowing Operator CType(ByVal pimDate As CA_DATA_DATE) As Date?
                If pimDate.IsEmptyPIMDate() Then Return Nothing
                Dim dateReturn As New Date(System.Convert.ToInt32(pimDate.wYear), pimDate.bMonth, pimDate.bDay, pimDate.bHour, pimDate.bMinute, pimDate.bSecond, DateTimeKind.Utc)
                dateReturn = DateAdd(DateInterval.Minute, pimDate.lTimeZoneBias, dateReturn)
                dateReturn = DateAdd(DateInterval.Minute, pimDate.lBias, dateReturn)
                Return dateReturn
            End Operator

            Public Shared Widening Operator CType(ByVal sysDate As Date) As CA_DATA_DATE
                Return New CA_DATA_DATE(sysDate.Year, sysDate.Month, sysDate.Day, sysDate.Hour, sysDate.Minute, sysDate.Second)
            End Operator


        End Structure

        '/////////////////////////////////////////////////////
        ' CA_DATA_ADDRESS
        '
        ' Description:
        ' This structure contains address information of the message. 
        '  
        ' Parameters:
        '	dwSize			Size of the structure (must be set!)
        '  dwAddressInfo	Contains type information of address 
        '					See "Address types" definition on top
        '  pstrAddress		Address data in unicode format
        '
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto, Pack:=4)>
        Public Structure CA_DATA_ADDRESS
            Dim iSize As Integer
            Dim iAddressInfo As Integer
            '<MarshalAs(UnmanagedType.LPWStr)>         Dim pstrAddress As String
            Private _pstrAddress As IntPtr

            Public Property pstrAddress As String
                Get
                    Return PtrToStr(Me._pstrAddress)
                End Get
                Set(value As String)
                    Me._pstrAddress = StrToPtr(value)
                End Set
            End Property
        End Structure
 

        '/////////////////////////////////////////////////////
        ' CA_DATA_POSTAL_ADDRESS
        '
        ' Description:
        ' This structure contains postal address information. 
        '  
        ' Parameters:
        '	dwSize				Size of the structure (must be set!)
        '	pstrPOBox			PO Box address
        '	pstrStreet			Street address
        '	pstrPostalCode		Postal code information
        '	pstrCity			City 
        '	pstrState			State
        '	pstrCountry			Country 
        '	pstrExtendedData	Extended address information
        '
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)> _
        Public Structure CA_DATA_POSTAL_ADDRESS
            Dim iSize As Integer
            Private _pstrPOBox As IntPtr ''<MarshalAs(UnmanagedType.LPWStr)> Dim pstrPOBox As String
            Private _pstrStreet As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrStreet As String
            Private _pstrPostalCode As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrPostalCode As String
            Private _pstrCity As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrCity As String
            Private _pstrState As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrState As String
            Private _pstrCountry As IntPtr ' <MarshalAs(UnmanagedType.LPWStr)> Dim pstrCountry As String
            Private _pstrExtendedData As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrExtendedData As String

            Public Property pstrPOBox As String
                Get
                    Return PtrToStr(Me._pstrPOBox)
                End Get
                Set(value As String)
                    Me._pstrPOBox = StrToPtr(value)
                End Set
            End Property

            Public Property pstrStreet As String '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrStreet As String
                Get
                    Return PtrToStr(Me._pstrStreet)
                End Get
                Set(value As String)
                    Me._pstrStreet = StrToPtr(value)
                End Set
            End Property

            Public Property pstrPostalCode As String '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrPostalCode As String
                Get
                    Return PtrToStr(Me._pstrPostalCode)
                End Get
                Set(value As String)
                    Me._pstrPostalCode = StrToPtr(value)
                End Set
            End Property

            Public Property pstrCity As String '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrCity As String
                Get
                    Return PtrToStr(Me._pstrCity)
                End Get
                Set(value As String)
                    Me._pstrCity = StrToPtr(value)
                End Set
            End Property

            Public Property pstrState As String
                Get
                    Return PtrToStr(Me._pstrState)
                End Get
                Set(value As String)
                    Me._pstrState = StrToPtr(value)
                End Set
            End Property

            Public Property pstrCountry As String
                Get
                    Return PtrToStr(Me._pstrCountry)
                End Get
                Set(value As String)
                    Me._pstrCountry = StrToPtr(value)
                End Set
            End Property

            Public Property pstrExtendedData As String
                Get
                    Return PtrToStr(Me._pstrExtendedData)
                End Get
                Set(value As String)
                    Me._pstrExtendedData = StrToPtr(value)
                End Set
            End Property

        End Structure

        '/////////////////////////////////////////////////////
        ' CA_DATA_PICTURE
        '
        ' Description:
        ' This structure contains picture information and data
        '  
        ' Parameters:
        '	dwSize			Size of the structure (must be set!)
        '	pstrFileName	Picture file name/picture format  
        '	dwDataLen		Picture data buffer length
        '	pbData			picture data
        '
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)>
        Public Structure CA_DATA_PICTURE
            Dim iSize As Integer
            '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrFileName As String
            Private _pstrFileName As IntPtr
            Dim iDataLen As Integer
            Dim pbData As IntPtr

            Public Property pstrFileName As String
                Get
                    Return PtrToStr(Me._pstrFileName)
                End Get
                Set(value As String)
                    Me._pstrFileName = StrToPtr(value)
                End Set
            End Property
        End Structure

        '/////////////////////////////////////////////////////
        ' CA_DATA_ITEM
        '
        ' Info: 
        ' Generic data structure used with contacts and calendar item
        ' dwFieldType defines the format of the data (which union member
        ' contains valid data) . 
        '
        ' Parameters:
        '	dwSize			Size of the structure (must be set!)
        '	dwFieldId		Field specific ID used in field access operations in content Access API. 
        '					Identifies certain field in content item. This is returned in CAReadItem. 
        '	dwFieldType		For different field types:
        '					see "Field type definitions " on top of this header
        '	dwFieldSubType	For different field sub types:
        '					see "Field sub type definitions" on top of this header
        '	ItemData		According to defined field sub type parameter, this union is 
        '					filled with valid data. See details in table
        '					"Field sub type definitions "
        '
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)>
        Public Structure CA_DATA_ITEM
            Dim iSize As Integer
            Dim iFieldId As Integer
            Dim iFieldType As Integer
            Dim iFieldSubType As Integer
            Dim pCustomData As IntPtr
            'union
            '{
            '	DWORD	dwData;
            '	WCHAR*	pstrText;			
            '	LPVOID	pCustomData;
            '} ItemData;
        End Structure

        '/////////////////////////////////////////////////////
        ' CA_DATA_CONTACT
        '
        ' Info: 
        ' Contact data item. 
        '
        ' Parameters:
        '	dwSize			Size of the structure (must be set!)
        '	bPICount		Amount of personal information fields 
        '	pPIFields		Personal information field data
        '	bNumberCount	Amount of contact number information fields 
        '	pNumberFields	Contact number information data
        '	bAddressCount	Amount of address fields 
        '	pAddressFields	Address field data 
        '	bGeneralCount	Amount of general fields 
        '	pGeneralFields	General field data 
        '
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)>
        Public Structure CA_DATA_CONTACT
            Dim iSize As Integer
            Dim bPICount As Byte
            Dim pPIFields As IntPtr
            Dim bNumberCount As Byte
            Dim pNumberFields As IntPtr
            Dim bAddressCount As Byte
            Dim pAddressFields As IntPtr
            Dim bGeneralCount As Byte
            Dim pGeneralFields As IntPtr
        End Structure

        '/////////////////////////////////////////////////////
        ' CA_DATA_CALENDAR
        '
        ' Info: 
        ' Calendar item structure
        '
        ' Parameters:
        '	dwSize				Size of the structure (must be set!)
        '	dwInfoField			Type of the calendar item
        '	noteStartDate		Start date of the note
        '	noteEndDate			End date of the note
        '   dwAlarmState		    For possible values , see "Calendar Alarm State value definitions"
        '	noteAlarmTime		Alarm time of the note (defines also if no alarm for note)
        '	dwRecurrence		Requrrency of the note
        '	recurrenceEndDate	End date if note has requrrence
        '	bItemCount			Amount of items belonging to note
        '	pDataItems			Calendar data items
        '
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)>
        Public Structure CA_DATA_CALENDAR
            Dim iSize As Integer
            Dim iInfoField As Integer
            Dim noteStartDate As CA_DATA_DATE
            Dim noteEndDate As CA_DATA_DATE
            Dim iAlarmState As Integer
            Dim noteAlarmTime As CA_DATA_DATE
            Dim iRecurrence As Integer
            Dim recurrenceEndDate As CA_DATA_DATE
            Dim bItemCount As Byte
            Dim pDataItems As IntPtr
            Dim bRecurrenceExCount As Byte
            Dim pRecurrenceExceptions As IntPtr
        End Structure

        '/////////////////////////////////////////////////////
        ' CA_DATA_MSG
        '
        ' Description:
        ' CA data stucture for message data
        ' 
        ' 
        '  
        ' Parameters:
        '	dwSize			Size of the structure (must be set!), use sizeof(CA_DATA_MSG)
        '  dwInfoField		Contains status information of message (used encoding,input data 
        '					format, message status 
        '					In dwInfoField following definitions are used now : 
        '					  = &H  = &H value defines format of input data
        '					  = &HX0 value defines how input data is passed to the phone
        '					  = &H  = &H00 value defines status of the message (read / unread...)
        '					  = &HX000 value defines message type (submit / deliver )
        ' 			
        '  dwDataLength	size of pbData byte array 
        '  pbData			Actual user data 
        '  bAddressCount	Amount of addresses included in pAddress array  (currently only one address supported)
        '	pAddress		Pointer to addesses.
        '	messageDate		This struct is used when generating time stamps for the message
        '
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)>
        Public Structure CA_DATA_MSG
            Dim iSize As Integer
            Dim iInfoField As Integer
            Dim iDataLength As Integer
            Dim pbData As IntPtr
            Dim bAddressCount As Byte
            Dim pAddress As IntPtr
            Dim messageDate As CA_DATA_DATE
        End Structure

        '/////////////////////////////////////////////////////
        ' CA_DATA_VERSIT 
        '
        ' Description:
        ' CA data stucture for versit data
        '  
        ' Parameters:
        '	dwSize			Size of the structure (must be set!)
        '	dwInfoField		Contains status information of versit data object
        '					In dwInfoField following definitions are used now : 
        '					  = &H  = &H value defines format of input data, see "Input data format definitions"
        '	dwDataLenght	Lenght of the data
        '	pbVersitObject	Pointer to versit object data.
        '
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)>
        Public Structure CA_DATA_VERSIT
            Dim iSize As Integer
            Dim iInfoField As Integer
            Dim iDataLength As Integer
            Dim pbVersitObject As IntPtr
        End Structure


        '=========================================================
        ' CA_MMS_DATA
        '
        ' Description:
        ' This structure defines MMS message data
        '  
        ' Parameters:
        '	dwSize			The total size of this structure in bytes (sizeof(CA_MMS_DATA)).
        '	dwInfoField		Contains status information of message (message status)
        '					In dwInfoField following definitions are used now : 
        '					  = &H  = &H00 value defines status of the message (read / unread...)	
        '  bAddressCount	Amount of addresses in message (currently only one address supported)
        '					Address is returned when reading the message, but in the writing address
        '					is included in binary MMS
        '  pAddress		Address array 
        '	messageDate		Message date
        '	dwDataLength	Size of MMS data buffer
        '	pbData			Actual MMS data 	
        '
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)>
        Public Structure CA_MMS_DATA
            Dim iSize As Integer
            Dim iInfoField As Integer
            Dim bAddressCount As Byte
            Dim pAddress As IntPtr
            Dim messageDate As CA_DATA_DATE
            Dim iDataLength As Integer
            Dim pbData As IntPtr
        End Structure

        '=========================================================
        ' CA_DATA_NOTE
        '
        ' Description:
        ' This structure defines calendar note data 
        '  
        ' Parameters:
        '	dwSize			The total size of this structure in bytes (sizeof(CA_DATA_ITEM)).
        '	pstrText		Note text
        '
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)>
        Public Structure CA_DATA_NOTE
            Dim iSize As Integer
            Private _pstrText As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrText As String

            Public Property pstrText As String
                Get
                    Return PtrToStr(Me._pstrText)
                End Get
                Set(value As String)
                    Me._pstrText = StrToPtr(value)
                End Set
            End Property
        End Structure

        '=========================================================
        ' CA_DATA_BOOKMARK
        '
        ' Info: 
        ' Bookmark data structure
        '
        ' Parameters:
        '	dwSize			Size of the structure (must be set!)
        '	pstrTitle		Title of bookmark
        '	pstrBookMarkUrl	Bookmark URL 
        '   pstrUrlShortcut Url shortcut
        '	
        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)>
        Public Structure CA_DATA_BOOKMARK
            Dim iSize As Integer
            Private _pstrTitle As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrTitle As String
            Private _pstrBookMarkUrl As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrBookMarkUrl As String
            Private _pstrUrlShortcut As IntPtr '<MarshalAs(UnmanagedType.LPWStr)> Dim pstrUrlShortcut As String

            Public Property pstrTitle As String
                Get
                    Return PtrToStr(Me._pstrTitle)
                End Get
                Set(value As String)
                    Me._pstrTitle = StrToPtr(value)
                End Set
            End Property

            Public Property pstrBookMarkUrl As String
                Get
                    Return PtrToStr(Me._pstrBookMarkUrl)
                End Get
                Set(value As String)
                    Me._pstrBookMarkUrl = StrToPtr(value)
                End Set
            End Property

            Public Property pstrUrlShortcut As String
                Get
                    Return PtrToStr(Me._pstrUrlShortcut)
                End Get
                Set(value As String)
                    Me._pstrUrlShortcut = StrToPtr(value)
                End Set
            End Property
        End Structure

        'End Class


    End Class

End Class