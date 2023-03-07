Imports System.Runtime.InteropServices
Imports DMD.Internals
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Partial Class Nokia


    Public Enum SMSMessageStatuses As Integer
        ''' <summary>
        ''' Message has been sent 
        ''' </summary>
        ''' <remarks></remarks>
        Sent = CA_MESSAGE_STATUS_SENT

        ''' <summary>
        ''' Message hasn't been read 
        ''' </summary>
        ''' <remarks></remarks>
        Unread = CA_MESSAGE_STATUS_UNREAD

        ''' <summary>
        ''' Message has been read 
        ''' </summary>
        ''' <remarks></remarks>
        Read = CA_MESSAGE_STATUS_READ

        ''' <summary>
        ''' Message is a draft 
        ''' </summary>
        ''' <remarks></remarks>
        Draft = CA_MESSAGE_STATUS_DRAFT

        ''' <summary>
        ''' Message is pending 
        ''' </summary>
        ''' <remarks></remarks>
        Pending = CA_MESSAGE_STATUS_PENDING

        ''' <summary>
        ''' Message has been delivered 
        ''' </summary>
        ''' <remarks></remarks>
        Delivered = CA_MESSAGE_STATUS_DELIVERED

        ''' <summary>
        ''' Message is being sent
        ''' </summary>
        ''' <remarks></remarks>
        Sending = CA_MESSAGE_STATUS_SENDING

    End Enum

    Public Class CSMSMessage
        Inherits CBaseItem

        Private dataSMS As CA_DATA_MSG
        Private m_Date As Date?
        Private m_Status As Integer
        Private m_Message As String
        Private m_Details As System.Collections.Specialized.StringDictionary
        Private m_Addresses As System.Collections.ArrayList

        Public Sub New()
            Me.m_Details = Nothing
            Me.m_Status = SMSMessageStatuses.Draft
            Me.m_Date = Nothing
            Me.m_Message = vbNullString
            Me.m_Addresses = New System.Collections.ArrayList
        End Sub

        Public Sub New(ByVal folder As CSMSFolder)
            Me.New()
            Me.SetDevice(folder.Device)
            Me.SetParentFolder(folder)
        End Sub

        Public Property Status As SMSMessageStatuses
            Get
                Return CType(CA_GET_MESSAGE_STATUS(Me.m_Status), SMSMessageStatuses)
            End Get
            Set(value As SMSMessageStatuses)
                Me.m_Status = value
            End Set
        End Property

        Public ReadOnly Property StatusEx As String
            Get
                Select Case Me.Status
                    Case SMSMessageStatuses.Sent : Return "Sent"
                    Case SMSMessageStatuses.Unread : Return "Hasn't been read"
                    Case SMSMessageStatuses.Read : Return "Has been read"
                    Case SMSMessageStatuses.Draft : Return "Draft"
                    Case SMSMessageStatuses.Pending : Return "Pending"
                    Case SMSMessageStatuses.Delivered : Return "Delivered"
                    Case SMSMessageStatuses.Sending : Return "Being sent"
                    Case Else : Return "Unknown"
                End Select
            End Get
        End Property

        Public Property [Date] As Date?
            Get
                Return Me.m_Date
            End Get
            Set(value As Date?)
                Me.m_Date = value
            End Set
        End Property

        Public Property Message As String
            Get
                Return Me.m_Message
            End Get
            Set(value As String)
                Me.m_Message = value
            End Set
        End Property

        Public ReadOnly Property Addresses As System.Collections.ArrayList
            Get
                Return Me.m_Addresses
            End Get
        End Property

        'Friend Sub SetUID(ByVal uid As CA_ITEM_ID)
        '    Me.UID = uid
        '    Me.GetSMSDetails()
        'End Sub

        '===================================================================
        ' ReadSMS
        '
        ' Reads SMS message from device
        '
        '===================================================================
        Friend Function ReadSMS(ByVal hOperHandle As Integer, ByVal UID As CA_ITEM_ID) As Integer

            Dim iRet As Integer


            Me.UID = UID
            dataSMS.messageDate.iSize = Marshal.SizeOf(dataSMS.messageDate)
            dataSMS.iSize = Marshal.SizeOf(dataSMS)

            ' Allocate memory for buffer
            iRet = CAReadItem(hOperHandle, CType(Me.UID, CA_ITEM_ID), CA_OPTION_USE_CACHE, CA_DATA_FORMAT_STRUCT, dataSMS)
            ' iRet = CAReadItem(hOperHandle, buffer, 0, CA_DATA_FORMAT_STRUCT, m_pSMSBuffer)
            If iRet <> CONA_OK Then ShowErrorMessage("CAReadItem", iRet)
                
            Me.GetSMSDetails(dataSMS)

            ' Free memory allocated by DA API
            iRet = CAFreeItemData(Me.ParentFolder.GetConnectionHandle, CA_DATA_FORMAT_STRUCT, dataSMS)
            If iRet <> CONA_OK Then ShowErrorMessage("DAFreeItemData", iRet)

            Return iRet
        End Function



        '===================================================================
        ' GetSMSDetails
        '
        ' Read selected SMS from phone and show details in list view.
        '
        '===================================================================
        Private Sub GetSMSDetails(ByVal dataMsg As CA_DATA_MSG)
            ' Read SMS item data from device
            Me.m_Details = New System.Collections.Specialized.StringDictionary
            ' Addresses
            For i As Integer = 0 To dataMsg.bAddressCount - 1
                ' Calculate beginning of CA_DATA_ITEM structure of item 'i'
                Dim iPtr As Int64 = dataMsg.pAddress.ToInt64 + i * Marshal.SizeOf(GetType(CA_DATA_ADDRESS))
                ' Convert integer to pointer
                Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
                Dim itemData As CA_DATA_ADDRESS = CType(Marshal.PtrToStructure(ptr, GetType(CA_DATA_ADDRESS)), CA_DATA_ADDRESS)
                ' Add item to list view
                Me.m_Addresses.Add(Trim(itemData.pstrAddress))
            Next

            If dataMsg.iDataLength > 0 Then
                If (CA_GET_DATA_FORMAT(dataMsg.iInfoField)) = CA_DATA_FORMAT_UNICODE Then
                    If Not IntPtr.Zero.Equals(dataMsg.pbData) Then
                        ' Add item to list view
                        Me.m_Message = Marshal.PtrToStringUni(dataMsg.pbData, CInt(dataMsg.iDataLength / 2))
                    Else
                        ' No data, GMS or other kind of message?
                    End If
                Else
                    ' Message in data format
                    'Debug.Print(dataMsg.pbData)
                End If
            End If
            ' Get message date and format it
            Me.m_Date = CType(dataMsg.messageDate, Date?)
            ' Message status
            Me.m_Status = dataMsg.iInfoField
            'FreeSMSData()
            'End If
        End Sub

        Public Overrides Function ToString() As String
            Dim buffer As New System.Text.StringBuilder
            buffer.Append("SMS Message [")
            For Each number As String In Me.Addresses
                buffer.Append(number)
                buffer.Append(" ")
            Next
            buffer.Append("] ")
            buffer.Append(Me.Message)
            Return buffer.ToString
        End Function
       


        Protected Overrides Sub InternalDelete()
            'If MsgBox("Are you sure you want to delete selected item?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Confirm Item Delete") = MsgBoxResult.Yes Then
            Dim hOperHandle As Integer = 0
            Dim iRet As Integer = CABeginOperation(Me.ParentFolder.GetConnectionHandle, 0, hOperHandle)
            If iRet <> CONA_OK Then ShowErrorMessage("CABeginOperation", iRet)

            ' Deletes PIM item from currently connected device
            Dim UID As CA_ITEM_ID = CType(Me.UID, CA_ITEM_ID)
            iRet = CADeleteItem(hOperHandle, UID, 0)
            If iRet <> CONA_OK Then ShowErrorMessage("DADeleteItem", iRet)
            iRet = CACommitOperations(hOperHandle, IntPtr.Zero)
            If iRet <> CONA_OK Then ShowErrorMessage("CACommitOperations", iRet)
            iRet = CAEndOperation(hOperHandle)
            If iRet <> CONA_OK Then ShowErrorMessage("CAEndOperation", iRet)
        End Sub
    End Class

End Class