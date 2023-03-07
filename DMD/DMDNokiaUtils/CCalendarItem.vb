Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS
Imports DMD.Internals

Partial Class Nokia

    Public Enum CalendarItemType As Integer
        Meeting = CA_CALENDAR_ITEM_MEETING
        [Call] = CA_CALENDAR_ITEM_CALL
        Birthday = CA_CALENDAR_ITEM_BIRTHDAY
        Memo = CA_CALENDAR_ITEM_MEMO
        Reminder = CA_CALENDAR_ITEM_REMINDER
        Note = CA_CALENDAR_ITEM_NOTE
        ToDo = CA_CALENDAR_ITEM_TODO
    End Enum

    Public Enum RecurrenceTypes As Integer
        None = CA_CALENDAR_RECURRENCE_NONE
        Daily = CA_CALENDAR_RECURRENCE_DAILY
        weekly = CA_CALENDAR_RECURRENCE_WEEKLY
        Monthly = CA_CALENDAR_RECURRENCE_MONTHLY
        Yearly = CA_CALENDAR_RECURRENCE_YEARLY
    End Enum

    Public Enum CalendarItemStatuses As Integer
        NeedsNoAction = CA_CALENDAR_TODO_STATUS_NEEDS_ACTION
        Completed = CA_CALENDAR_TODO_STATUS_COMPLETED
    End Enum

    ''' <summary>
    ''' Rappresenta una voce del calendario
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CCalendarItem
        Inherits CBaseItem

        Private m_ItemType As CalendarItemType
        Private m_Description As String
        Private m_Location As String
        Private m_Detail As String
        Private m_noteStartDate As Date?
        Private m_noteEndDate As Date?
        Private m_noteAlarmTime As Date?
        Private m_iRecurrence As RecurrenceTypes
        Private m_RecurrenceInterval As Integer
        Private m_recurrenceEndDate As Date?
        Private m_Number As String
        Private m_Year As Integer?
        Private m_Priority As Integer?
        Private m_ToDoStatus As CalendarItemStatuses

        Public Sub New()
            Me.m_Description = ""
            Me.m_Location = ""
            Me.m_Detail = ""
            Me.m_ItemType = CalendarItemType.Note
            Me.m_noteStartDate = Nothing
            Me.m_noteEndDate = Nothing
            Me.m_noteAlarmTime = Nothing
            Me.m_iRecurrence = RecurrenceTypes.None
            Me.m_RecurrenceInterval = 0
            Me.m_recurrenceEndDate = Nothing
            Me.m_Number = ""
            Me.m_Year = Nothing
            Me.m_Priority = Nothing
            Me.m_ToDoStatus = CalendarItemStatuses.NeedsNoAction
        End Sub

        Friend Sub New(ByVal calendar As CCalendarFolder)
            Me.New()
            Me.SetDevice(calendar.Device)
            Me.SetParentFolder(calendar)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il tipo dell'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ItemType As CalendarItemType
            Get
                Return Me.m_ItemType
            End Get
            Set(value As CalendarItemType)
                Me.m_ItemType = value
            End Set
        End Property

        '===================================================================
        ' CalendarItemType2String
        ' 
        ' Converts calendar item type values to string
        ' 
        '===================================================================
        Public Property ItemTypeEx() As String
            Get
                Select Case Me.ItemType
                    Case CalendarItemType.Meeting : Return "Meeting"
                    Case CalendarItemType.Call : Return "Call"
                    Case CalendarItemType.Birthday : Return "Birthday"
                    Case CalendarItemType.Memo : Return "Memo"
                    Case CalendarItemType.Reminder : Return "Reminder"
                    Case CalendarItemType.Note : Return "Note"
                    Case CalendarItemType.ToDo : Return "Todo"
                    Case Else : Return "Unknown"
                End Select
            End Get
            Set(value As String)
                Select Case value
                    Case "Meeting" : Me.ItemType = CalendarItemType.Meeting
                    Case "Call" : Me.ItemType = CalendarItemType.Call
                    Case "Birthday" : Me.ItemType = CalendarItemType.Birthday
                    Case "Memo" : Me.ItemType = CalendarItemType.Memo
                    Case "Reminder" : Me.ItemType = CalendarItemType.Reminder
                    Case "Note" : Me.ItemType = CalendarItemType.Note
                    Case "Todo" : Me.ItemType = CalendarItemType.ToDo
                    Case Else : Throw New ArgumentException("Tipo non riconosciuto")
                End Select
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di inizio dell'evento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StartDate As Date?
            Get
                ' Get note start date and format it
                Return Me.m_noteStartDate
            End Get
            Set(value As Date?)
                Me.m_noteStartDate = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di termine dell'evento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EndDate As Date?
            Get
                ' Get note end date and format it
                Return Me.m_noteEndDate
            End Get
            Set(value As Date?)
                Me.m_noteEndDate = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora della sveglia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AlarmTime As Date?
            Get
                ' Get note alarm time and format it
                Return Me.m_noteAlarmTime
            End Get
            Set(value As Date?)
                Me.m_noteAlarmTime = value
            End Set
        End Property

        Public Property RecurrenceType As RecurrenceTypes
            Get
                ' Show recurrence
                Return Me.m_iRecurrence
            End Get
            Set(value As RecurrenceTypes)
                Me.m_iRecurrence = value
            End Set
        End Property

        Public ReadOnly Property RecurrenceTypeEx As String
            Get
                Select Case Me.RecurrenceType
                    Case RecurrenceTypes.None : Return "None"
                    Case RecurrenceTypes.Daily : Return "Daily"
                    Case RecurrenceTypes.weekly : Return "Weekly"
                    Case RecurrenceTypes.Monthly : Return "Monthly"
                    Case RecurrenceTypes.Yearly : Return "Yearly"
                    Case Else : Return "Unknown (" & Me.RecurrenceType & ")"
                End Select
            End Get
        End Property

        Public Property RecurrenceInterval As Integer
            Get
                Return Me.m_RecurrenceInterval
            End Get
            Set(value As Integer)
                Me.m_RecurrenceInterval = value
            End Set
        End Property

        Public Property Description As String
            Get
                Return Me.m_Description
            End Get
            Set(value As String)
                Me.m_Description = value
            End Set
        End Property

        Public Property Location As String
            Get
                Return Me.m_Location
            End Get
            Set(value As String)
                Me.m_Location = value
            End Set
        End Property

        Public Property Detail As String
            Get
                Return Me.m_Detail
            End Get
            Set(value As String)
                Me.m_Detail = value
            End Set
        End Property


        Public Property RecurrenceDate As Date?
            Get
                ' Get recurrence end date and format it
                Return Me.m_recurrenceEndDate
            End Get
            Set(value As Date?)
                Me.m_recurrenceEndDate = value
            End Set
        End Property


        Public Property Number As String
            Get
                Return Me.m_Number
            End Get
            Set(value As String)
                Me.m_Number = value
            End Set
        End Property

        Public Property Year As Integer?
            Get
                Return Me.m_Year
            End Get
            Set(value As Integer?)
                Me.m_Year = value
            End Set
        End Property

        Public Property Priority As Integer?
            Get
                Return Me.m_Priority
            End Get
            Set(value As Integer?)
                Me.m_Priority = value
            End Set
        End Property

        Public ReadOnly Property PriorityEx As String
            Get
                If (Me.Priority.HasValue = False) Then Return ""
                Select Case Me.Priority
                    Case CA_CALENDAR_TODO_PRIORITY_HIGH : Return "High"
                    Case CA_CALENDAR_TODO_PRIORITY_NORMAL : Return "Normal"
                    Case CA_CALENDAR_TODO_PRIORITY_LOW : Return "Low"
                    Case Else : Return "Unknown (" & Me.Priority.Value.ToString() & ")"
                End Select
            End Get
        End Property

        Public Property ToDoStatus As CalendarItemStatuses
            Get
                Return Me.m_ToDoStatus
            End Get
            Set(value As CalendarItemStatuses)
                Me.m_ToDoStatus = value
            End Set
        End Property

        Public ReadOnly Property ToDoStatusEx As String
            Get
                Select Case Me.ToDoStatus
                    Case CalendarItemStatuses.NeedsNoAction : Return "Needs action"
                    Case CalendarItemStatuses.NeedsNoAction : Return "Completed"
                    Case Else : Return "Unknown (" & CInt(Me.m_ToDoStatus) & ")"
                End Select

            End Get
        End Property

        '===================================================================
        ' ReadCalendarItem
        '
        ' Reads calendar item from device
        '
        '===================================================================
        Friend Sub ReadCalendarItem(ByVal hOperHandle As Integer, ByVal UID As CA_ITEM_ID)
            Me.UID = UID
            ' Read calendar item from the connected device
            Dim dataCalendar As CA_DATA_CALENDAR
            dataCalendar.iSize = Marshal.SizeOf(dataCalendar)
            dataCalendar.bItemCount = 0
            dataCalendar.iInfoField = 0
            dataCalendar.iRecurrence = 0
            dataCalendar.iAlarmState = 0
            dataCalendar.noteAlarmTime.iSize = Marshal.SizeOf(dataCalendar.noteAlarmTime)
            dataCalendar.noteEndDate.iSize = Marshal.SizeOf(dataCalendar.noteEndDate)
            dataCalendar.noteStartDate.iSize = Marshal.SizeOf(dataCalendar.noteStartDate)
            dataCalendar.pDataItems = IntPtr.Zero
            dataCalendar.recurrenceEndDate.iSize = Marshal.SizeOf(dataCalendar.recurrenceEndDate)

            Dim iRet As Integer = CAReadItem(hOperHandle, CType(Me.UID, CA_ITEM_ID), CA_OPTION_USE_CACHE, CA_DATA_FORMAT_STRUCT, dataCalendar)
            If iRet = CONA_OK Then
                Me.GetCalendarDetails(dataCalendar)
            Else
                ShowErrorMessage("CAReadItem", iRet)
                'Exit For  ' Reading failed, quit loop.
            End If
            ' Free memory allocated by DA API
            iRet = CAFreeItemData(Me.ParentFolder.GetConnectionHandle, CA_DATA_FORMAT_STRUCT, dataCalendar)
            If iRet <> CONA_OK Then ShowErrorMessage("DAFreeItemData", iRet)

        End Sub


        '===================================================================
        ' GetCalendarDetails
        '
        ' Read selected calendar item from phone and show details in list view.
        '
        '===================================================================
        Private Sub GetCalendarDetails(ByVal dataCalendar As CA_DATA_CALENDAR)
            Me.m_ItemType = CType(dataCalendar.iInfoField, CalendarItemType)
            Me.m_iRecurrence = CType(CA_GET_RECURRENCE(dataCalendar.iRecurrence), RecurrenceTypes)
            Me.m_RecurrenceInterval = CA_GET_RECURRENCE_INTERVAL(dataCalendar.iRecurrence)
            Me.m_noteStartDate = CType(dataCalendar.noteStartDate, Date?)
            Me.m_noteEndDate = CType(dataCalendar.noteEndDate, Date?)
            Me.m_noteAlarmTime = CType(dataCalendar.noteAlarmTime, Date?)
            Me.m_recurrenceEndDate = CType(dataCalendar.recurrenceEndDate, Date?)

            ' Get SubItems
            If dataCalendar.bItemCount > 0 Then
                Dim pDataItems As Int64 = 0
                Dim counter As Integer = 0
                For counter = 0 To dataCalendar.bItemCount Step 1
                    Dim dataItem As CA_DATA_ITEM
                    pDataItems = dataCalendar.pDataItems.ToInt64 + (counter * Marshal.SizeOf(GetType(CA_DATA_ITEM)))
                    dataItem = CType(Marshal.PtrToStructure(IntPtr.op_Explicit(pDataItems), GetType(CA_DATA_ITEM)), CA_DATA_ITEM)
                    If dataItem.iFieldType = CA_FIELD_TYPE_CALENDAR Then
                        If dataItem.iFieldSubType = CA_FIELD_SUB_TYPE_DESCRIPTION Then
                            Me.m_Description = Marshal.PtrToStringUni(dataItem.pCustomData)
                        ElseIf dataItem.iFieldSubType = CA_FIELD_SUB_TYPE_LOCATION Then
                            Me.m_Location = Marshal.PtrToStringUni(dataItem.pCustomData)
                        ElseIf dataItem.iFieldSubType = CA_FIELD_SUB_TYPE_ITEM_DATA Then
                            If dataCalendar.iInfoField = CA_CALENDAR_ITEM_MEETING Then
                                Me.m_Detail = Marshal.PtrToStringUni(dataItem.pCustomData)
                            ElseIf dataCalendar.iInfoField = CA_CALENDAR_ITEM_CALL Then
                                Me.m_Number = Marshal.PtrToStringUni(dataItem.pCustomData)
                            ElseIf dataCalendar.iInfoField = CA_CALENDAR_ITEM_BIRTHDAY Then
                                Me.m_Year = dataItem.pCustomData.ToInt32
                            End If
                        ElseIf dataItem.iFieldSubType = CA_FIELD_SUB_TYPE_TODO_PRIORITY Then
                            Me.m_Priority = dataItem.pCustomData.ToInt32
                        ElseIf dataItem.iFieldSubType = CA_FIELD_SUB_TYPE_TODO_STATUS Then
                            Me.m_ToDoStatus = CType(dataItem.pCustomData.ToInt32, CalendarItemStatuses)
                        End If
                    End If
                Next
            End If

            ' Free memory allocated by DA API

        End Sub

        Protected Overrides Sub InternalDelete()
            Throw New NotImplementedException()
        End Sub


    End Class

End Class