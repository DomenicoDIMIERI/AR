Imports System.Runtime.InteropServices
Imports DMD.Internals
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Partial Class Nokia

    Public Enum MMSMessageStatuses As Integer
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

    Public Class CMMSMessage
        Inherits CBaseItem

        Private m_Addresses As System.Collections.ArrayList
        Private m_Data As Date?
        Private m_Details As System.Collections.Specialized.StringDictionary
        Private m_Status As Integer
        Private m_Message As String

        Public Sub New()
            Me.m_Addresses = New System.Collections.ArrayList
            Me.m_Details = Nothing
            Me.m_Data = Nothing
            Me.m_Message = ""
        End Sub

        Friend Sub New(ByVal folder As CMMSFolder)
            Me.New()
            Me.SetDevice(folder.Device)
            Me.SetParentFolder(folder)
        End Sub

        Public ReadOnly Property Details As System.Collections.Specialized.StringDictionary
            Get
                If (Me.m_Details Is Nothing) Then Me.m_Details = New System.Collections.Specialized.StringDictionary
                Return Me.m_Details
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dell'MMS
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Data As Date?
            Get
                Return Me.m_Data
            End Get
            Set(value As Date?)
                Me.m_Data = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo del destinatario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Addresses As System.Collections.ArrayList
            Get
                Return Me.m_Addresses
            End Get
        End Property

        Public Property Status As MMSMessageStatuses
            Get
                Return CType(CA_GET_MESSAGE_STATUS(Me.m_Status), MMSMessageStatuses)
            End Get
            Set(value As MMSMessageStatuses)
                Me.m_Status = value
            End Set
        End Property

        Public ReadOnly Property StatusEx As String
            Get
                 Select Me.Status
                    Case MMSMessageStatuses.Sent : Return "Sent"
                    Case MMSMessageStatuses.Unread : Return "Hasn't been read"
                    Case MMSMessageStatuses.Read : Return "Has been read"
                    Case MMSMessageStatuses.Draft : Return "Draft"
                    Case MMSMessageStatuses.Pending : Return "Pending"
                    Case MMSMessageStatuses.Delivered : Return "Delivered"
                    Case MMSMessageStatuses.Sending : Return "Being sent"
                    Case Else : Return "Unknown"
                End Select
            End Get
        End Property

        Public Property Message As String
            Get
                Return Me.m_Message
            End Get
            Set(value As String)
                Me.m_Message = value
            End Set
        End Property

        '===================================================================
        ' ReadMMS
        '
        ' Reads MMS message from device
        '
        '===================================================================
        Friend Function ReadMMS(ByVal hOperHandle As Integer, ByVal UID As CA_ITEM_ID) As Integer ', ByRef dataMMS As CA_MMS_DATA) As Integer
            Me.UID = UID
            
            Dim dataMMS As CA_MMS_DATA
            dataMMS.iSize = Marshal.SizeOf(dataMMS)
            dataMMS.messageDate.iSize = Marshal.SizeOf(dataMMS.messageDate)

            ' Allocate memory for buffer
            Dim iRet As Integer = CAReadItem(hOperHandle, CType(Me.UID, CA_ITEM_ID), CA_OPTION_USE_CACHE, CA_DATA_FORMAT_STRUCT, dataMMS)
            If iRet <> CONA_OK Then ShowErrorMessage("CAReadItem", iRet)

            Me.GetMMSDetails(dataMMS)

            iRet = CAFreeItemData(Me.ParentFolder.GetConnectionHandle, CA_DATA_FORMAT_STRUCT, dataMMS)
            If iRet <> CONA_OK Then ShowErrorMessage("DAFreeItemData", iRet)

            Return iRet
        End Function

      

        '===================================================================
        ' GetMMSDetails
        '
        ' Read selected MMS from phone and show details in list view.
        '
        '===================================================================
        Private Sub GetMMSDetails(ByVal dataMsg As CA_MMS_DATA)
            ' Addresses
            For i As Integer = 0 To dataMsg.bAddressCount - 1
                ' Calculate beginning of CA_DATA_ITEM structure of item 'i'
                Dim iPtr As Int64 = dataMsg.pAddress.ToInt64 + i * Marshal.SizeOf(GetType(CA_DATA_ADDRESS))
                ' Convert integer to pointer
                Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
                Dim itemData As CA_DATA_ADDRESS
                itemData = CType(Marshal.PtrToStructure(ptr, GetType(CA_DATA_ADDRESS)), CA_DATA_ADDRESS)
                Me.m_Addresses.Add(itemData.pstrAddress)
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
                End If
            End If

            ' Get message date and format it
            Me.m_Data = CType(dataMsg.messageDate, Date?)
            ' Message status
            Me.m_Status = CType(dataMsg.iInfoField, MMSMessageStatuses)

        End Sub


        Protected Overrides Sub InternalDelete()

        End Sub
    End Class

End Class