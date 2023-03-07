Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    ''' <summary>
    ''' Rappresenta il calendario
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CCalendar
        Inherits CCalendarFolder

        Private m_hCalendar As IntPtr = IntPtr.Zero
        Private m_pCalendarBuffer As IntPtr = IntPtr.Zero
        
        Public Sub New()

        End Sub

        Public Sub New(ByVal device As NokiaDevice)
            Me.New()
            Me.SetDevice(device)
        End Sub


        '===================================================================
        ' GetCalendarFolder
        '
        ' Gets Calendar folder info and creates folder in tree view
        '
        '===================================================================
        Protected Overrides Sub InitializeFolderData()
            ' Check PIM connection to Calendar folders and open it if needed
            ' Get Calendar folder info
            folderInfo.iSize = Marshal.SizeOf(folderInfo)
            Dim iRet As Integer = CAGetFolderInfo(Me.GetConnectionHandle, Me.folderInfo)
            If iRet <> CONA_OK Then ShowErrorMessage("CAGetFolderInfo", iRet)
            Me.SetName(Me.folderInfo.pstrName)
            Me.SetPath(Me.folderInfo.pstrPath)
            iRet = Me.SubFolders.Count
            Dim iResult As Integer = CAFreeFolderInfoStructure(Me.folderInfo)
            If iResult <> CONA_OK Then ShowErrorMessage("CAFreeFolderInfoStructure", iResult)
            'Marshal.FreeHGlobal(bufItem)
        End Sub

        '===================================================================
        ' CloseCalendarConnection
        '
        ' Close PIM connection to Calendar folders
        '
        '===================================================================
        Friend Function CloseCalendarConnection() As Integer
            Dim iRet As Integer = CONA_OK
            If Not Me.m_hCalendar.Equals(IntPtr.Zero) Then
                ' Unregister CallBack
                Dim iResult As Integer = CARegisterNotifyCallback(m_hCalendar, API_UNREGISTER, Me.Device.pCANotifyCallBack)

                ' Close PIM connection
                iRet = DACloseCA(m_hCalendar)
                If iRet <> CONA_OK Then ShowErrorMessage("DACloseCA", iRet)
                Me.m_hCalendar = IntPtr.Zero
            End If
            Return iRet
        End Function

        Friend Overrides Function GetConnectionHandle() As IntPtr
            If Me.m_hCalendar.Equals(IntPtr.Zero) Then
                ' No PIM connection, open it
                Dim pstrSerialNumber As IntPtr = Marshal.StringToCoTaskMemUni(Me.Device.SerialNumber)
                Dim iMedia As Integer = API_MEDIA_ALL
                Dim iTarget As Integer = CA_TARGET_CALENDAR
                Dim iRet As Integer = DAOpenCA(pstrSerialNumber, iMedia, iTarget, Me.m_hCalendar)
                If iRet <> CONA_OK And iRet <> ECONA_NOT_SUPPORTED_DEVICE Then ShowErrorMessage("DAOpenCA", iRet)
                Marshal.FreeCoTaskMem(pstrSerialNumber)

                ' Register CA notification callback function
                If Not Me.m_hCalendar.Equals(IntPtr.Zero) Then
                    Dim iResult As Integer = CARegisterNotifyCallback(Me.m_hCalendar, API_REGISTER, Me.Device.pCANotifyCallBack)
                    If iResult <> CONA_OK Then ShowErrorMessage("CARegisterNotifyCallback", iResult)
                End If
            End If
            Return Me.m_hCalendar
        End Function





        Protected Overrides Sub InternalDelete()
            Throw New NotSupportedException
        End Sub

        ''===================================================================
        '' ShowNewCalendarDlg
        ''
        '' Shows "New Calendar item" dialog and writes Calendar entry to device
        ''
        ''===================================================================
        'Private Sub ShowNewCalendarDlg()
        '    ' Open "New Text Message" dialog
        '    Dim dlg As New CalendarItemDlg
        '    If dlg.ShowDialog() <> Windows.Forms.DialogResult.OK Then
        '        Exit Sub
        '    End If

        '    CheckCalendarConnection()

        '    Dim dataCalendar As CA_DATA_CALENDAR
        '    dataCalendar.iSize = Marshal.SizeOf(dataCalendar)
        '    dataCalendar.iInfoField = CA_CALENDAR_ITEM_MEETING + dlg.ComboBoxType.SelectedIndex

        '    ConvertToPIMDate(dlg.DTPickerNoteBeginDate.Value, dataCalendar.noteStartDate)
        '    ConvertToPIMDate(dlg.DTPickerNoteEndDate.Value, dataCalendar.noteEndDate)
        '    dataCalendar.iAlarmState = CA_CALENDAR_ALARM_NOT_SET + dlg.ComboAlarm.SelectedIndex
        '    If (dataCalendar.iAlarmState = CA_CALENDAR_ALARM_NOT_SET) Then
        '        GetEmptyPIMDate(dataCalendar.noteAlarmTime)
        '    Else
        '        ConvertToPIMDate(dlg.DTPickerAlarmDate.Value, dataCalendar.noteAlarmTime)
        '    End If
        '    dataCalendar.iRecurrence = CA_CALENDAR_RECURRENCE_NONE + dlg.ComboRecurrence.SelectedIndex
        '    If (dataCalendar.iRecurrence = CA_CALENDAR_RECURRENCE_NONE) Then
        '        GetEmptyPIMDate(dataCalendar.recurrenceEndDate)
        '    Else
        '        CA_SET_RECURRENCE_INTERVAL(dataCalendar.iRecurrence, 1)
        '        If dlg.CheckBoxRecEnd.Checked Then
        '            ConvertToPIMDate(dlg.DTPickerAlarmDate.Value, dataCalendar.recurrenceEndDate)
        '        Else
        '            GetEmptyPIMDate(dataCalendar.recurrenceEndDate)
        '        End If
        '    End If

        '    If dataCalendar.iInfoField = CA_CALENDAR_ITEM_MEETING Then
        '        Dim iCount As Integer = 0
        '        If dlg.TextBoxNote.Text.Length > 0 Then
        '            iCount += 1
        '        End If
        '        If dlg.TextBoxLocation.Text.Length > 0 Then
        '            iCount += 1
        '        End If
        '        dataCalendar.bItemCount = iCount
        '        dataCalendar.pDataItems = GetMeetingDescriptionLocationBuffer(dlg.TextBoxNote.Text, dlg.TextBoxLocation.Text, iCount)
        '    ElseIf dataCalendar.iInfoField = CA_CALENDAR_ITEM_CALL Then
        '        Dim iCount As Integer = 0
        '        If dlg.TextBoxNote.Text.Length > 0 Then
        '            iCount += 1
        '        End If
        '        If dlg.TextBoxLocation.Text.Length > 0 Then
        '            iCount += 1
        '        End If
        '        dataCalendar.bItemCount = iCount
        '        dataCalendar.pDataItems = GetCallNameNumberBuffer(dlg.TextBoxNote.Text, dlg.TextBoxLocation.Text, iCount)
        '    ElseIf dataCalendar.iInfoField = CA_CALENDAR_ITEM_BIRTHDAY Then
        '        Dim iCount As Integer = 0
        '        If dlg.TextBoxNote.Text.Length > 0 Then
        '            iCount += 1
        '        End If
        '        If dlg.TextBoxLocation.Text.Length > 0 Then
        '            iCount += 1
        '        End If
        '        dataCalendar.bItemCount = iCount
        '        dataCalendar.pDataItems = GetBirthDayNameYearBuffer(dlg.TextBoxNote.Text, dlg.TextBoxLocation.Text, iCount)
        '    ElseIf dataCalendar.iInfoField = CA_CALENDAR_ITEM_MEMO Then
        '        Dim iCount As Integer = 0
        '        If dlg.TextBoxNote.Text.Length > 0 Then
        '            iCount += 1
        '        End If
        '        dataCalendar.bItemCount = iCount
        '        dataCalendar.pDataItems = GetMemoBuffer(dlg.TextBoxNote.Text, iCount)
        '    ElseIf dataCalendar.iInfoField = CA_CALENDAR_ITEM_REMINDER Then
        '        Dim iCount As Integer = 0
        '        If dlg.TextBoxNote.Text.Length > 0 Then
        '            iCount += 1
        '        End If
        '        dataCalendar.bItemCount = iCount
        '        ' reuse getmemobuffer function
        '        dataCalendar.pDataItems = GetMemoBuffer(dlg.TextBoxNote.Text, iCount)
        '    ElseIf dataCalendar.iInfoField = CA_CALENDAR_ITEM_NOTE Then
        '        Dim iCount As Integer = 0
        '        If dlg.TextBoxNote.Text.Length > 0 Then
        '            iCount += 1
        '        End If
        '        dataCalendar.bItemCount = iCount
        '        ' reuse getmemobuffer function
        '        dataCalendar.pDataItems = GetMemoBuffer(dlg.TextBoxNote.Text, iCount)
        '    ElseIf dataCalendar.iInfoField = CA_CALENDAR_ITEM_TODO Then
        '        Dim iTodoPriority As Integer = dlg.ComboTodoPrior.SelectedIndex + CA_CALENDAR_TODO_PRIORITY_HIGH
        '        Dim iTodoAction As Integer = dlg.ComboTodoAction.SelectedIndex + CA_CALENDAR_TODO_STATUS_NEEDS_ACTION
        '        Dim iCount As Integer = 0
        '        If iTodoPriority > 0 Then
        '            iCount += 1
        '        End If
        '        If iTodoAction > 0 Then
        '            iCount += 1
        '        End If
        '        If dlg.TextBoxNote.Text.Length > 0 Then
        '            iCount += 1
        '        End If
        '        dataCalendar.bItemCount = iCount
        '        dataCalendar.pDataItems = GetTodoBuffer(dlg.TextBoxNote.Text, iTodoPriority, iTodoAction, iCount)
        '    End If

        '    Dim calendarNode As System.Windows.Forms.TreeNode
        '    If TVW_Navigator.SelectedNode.ImageIndex = m_iIconCalendarIndex Then
        '        calendarNode = TVW_Navigator.SelectedNode
        '    Else
        '        calendarNode = TVW_Navigator.SelectedNode.Parent
        '    End If
        '    Dim folderInfo As CA_FOLDER_INFO = MapCAFolderInfoToCFI(calendarNode.Tag)

        '    ' Write new Calendar item to currently connected device
        '    Dim hOperHandle As Integer = 0
        '    Dim iRet As Integer = CABeginOperation(m_hCurrentConnection, 0, hOperHandle)
        '    If iRet <> CONA_OK Then ShowErrorMessage("CABeginOperation", iRet)
        '    CARegisterOperationCallback(hOperHandle, API_REGISTER, pCAOperationCallback)
        '    If iRet <> CONA_OK Then ShowErrorMessage("CARegisterOperationCallback", iRet)
        '    Dim itemUid As CA_ITEM_ID
        '    itemUid.iSize = Marshal.SizeOf(itemUid)
        '    itemUid.iFolderId = folderInfo.iFolderId
        '    itemUid.iStatus = 0
        '    itemUid.iTemporaryID = 0
        '    itemUid.iUidLen = 0
        '    itemUid.pbUid = IntPtr.Zero
        '    Dim buf As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_ITEM_ID)))
        '    Marshal.StructureToPtr(itemUid, buf, True)
        '    ' Allocate memory for buffer
        '    Dim buf2 As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CA_DATA_CALENDAR)))
        '    Marshal.StructureToPtr(dataCalendar, buf2, True)
        '    iRet = CAWriteItem(hOperHandle, buf, 0, CA_DATA_FORMAT_STRUCT, buf2)
        '    If iRet <> CONA_OK Then ShowErrorMessage("DAWriteItem", iRet)
        '    Marshal.FreeHGlobal(buf2)
        '    Marshal.FreeHGlobal(buf)

        '    'If iRet <> CONA_OK Then ShowErrorMessage("DAFreeItemData", iRet)
        '    FreeCalendarDataAllocations(dataCalendar)

        '    iRet = CACommitOperations(hOperHandle, IntPtr.Zero)
        '    If iRet <> CONA_OK Then ShowErrorMessage("CACommitOperations", iRet)
        '    CARegisterOperationCallback(hOperHandle, API_UNREGISTER, pCAOperationCallback)
        '    If iRet <> CONA_OK Then ShowErrorMessage("CARegisterOperationCallback", iRet)
        '    iRet = CAEndOperation(hOperHandle)
        '    If iRet <> CONA_OK Then ShowErrorMessage("CAEndOperation", iRet)
        '    calendarNode.Nodes.Clear()
        '    GetCalendar(GetCurrentDevice(), calendarNode)
        'End Sub


      

        '===================================================================
        ' GetMeetingDescriptionLocationBuffer
        '
        ' Fills meeting items in CA_DATA_ITEM struct and copies it to
        ' unmanaged memory buffer
        '
        '===================================================================
        Private Function GetMeetingDescriptionLocationBuffer(ByVal strDescription As String, ByVal strLocation As String, ByVal iCount As Integer) As IntPtr
            Dim dataItem() As CA_DATA_ITEM
            ReDim dataItem(iCount)
            Dim iIndex As Integer
            If strDescription.Length > 0 Then
                ' Description
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CALENDAR
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_DESCRIPTION
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strDescription)
                iIndex += 1
            End If
            If strLocation.Length > 0 Then
                ' Location
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CALENDAR
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_LOCATION
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strLocation)
                iIndex += 1
            End If
            ' Allocate memory for buffer
            Dim iSize As Integer = iCount * Marshal.SizeOf(GetType(CA_DATA_ITEM))
            Dim bufDataItem As IntPtr = Marshal.AllocHGlobal(iSize)
            For iIndex = 0 To iCount - 1
                ' Calculate beginning of CA_DATA_ITEM structure of item 'iIndex'
                Dim iPtr As Int64 = bufDataItem.ToInt64 + iIndex * Marshal.SizeOf(GetType(CA_DATA_ITEM))
                ' Convert integer to pointer
                Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
                Marshal.StructureToPtr(dataItem(iIndex), ptr, True)
            Next
            Return bufDataItem
        End Function

        '===================================================================
        ' GetCallNameNumberBuffer
        '
        ' Fills meeting items in CA_DATA_ITEM struct and copies it to
        ' unmanaged memory buffer
        '
        '===================================================================
        Private Function GetCallNameNumberBuffer(ByVal strName As String, ByVal strNumber As String, ByVal iCount As Integer) As IntPtr
            Dim dataItem() As CA_DATA_ITEM
            ReDim dataItem(iCount)
            Dim iIndex As Integer
            If strName.Length > 0 Then
                ' Name
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CALENDAR
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_DESCRIPTION
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strName)
                iIndex += 1
            End If
            If strNumber.Length > 0 Then
                ' Number
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CALENDAR
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_ITEM_DATA
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strNumber)
                iIndex += 1
            End If
            ' Allocate memory for buffer
            Dim iSize As Integer = iCount * Marshal.SizeOf(GetType(CA_DATA_ITEM))
            Dim bufDataItem As IntPtr = Marshal.AllocHGlobal(iSize)
            For iIndex = 0 To iCount - 1
                ' Calculate beginning of CA_DATA_ITEM structure of item 'iIndex'
                Dim iPtr As Int64 = bufDataItem.ToInt64 + iIndex * Marshal.SizeOf(GetType(CA_DATA_ITEM))
                ' Convert integer to pointer
                Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
                Marshal.StructureToPtr(dataItem(iIndex), ptr, True)
            Next
            Return bufDataItem
        End Function

        '===================================================================
        ' GetBirthDayNameYearBuffer
        '
        ' Fills meeting items in CA_DATA_ITEM struct and copies it to
        ' unmanaged memory buffer
        '
        '===================================================================
        Private Function GetBirthDayNameYearBuffer(ByVal strName As String, ByVal strYear As String, ByVal iCount As Integer) As IntPtr
            Dim dataItem() As CA_DATA_ITEM
            ReDim dataItem(iCount)
            Dim iIndex As Integer
            If strName.Length > 0 Then
                ' Description
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CALENDAR
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_DESCRIPTION
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strName)
                iIndex += 1
            End If
            If strYear.Length > 0 Then
                ' Location
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CALENDAR
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_ITEM_DATA
                Dim dwYear As Int32 = Int32.Parse(strYear)
                dataItem(iIndex).pCustomData = IntPtr.op_Explicit(dwYear)
                iIndex += 1
            End If
            ' Allocate memory for buffer
            Dim iSize As Integer = iCount * Marshal.SizeOf(GetType(CA_DATA_ITEM))
            Dim bufDataItem As IntPtr = Marshal.AllocHGlobal(iSize)
            For iIndex = 0 To iCount - 1
                ' Calculate beginning of CA_DATA_ITEM structure of item 'iIndex'
                Dim iPtr As Int64 = bufDataItem.ToInt64 + iIndex * Marshal.SizeOf(GetType(CA_DATA_ITEM))
                ' Convert integer to pointer
                Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
                Marshal.StructureToPtr(dataItem(iIndex), ptr, True)
            Next
            Return bufDataItem
        End Function


        '===================================================================
        ' GetMemoBuffer
        '
        ' Fills meeting items in CA_DATA_ITEM struct and copies it to
        ' unmanaged memory buffer
        '
        '===================================================================
        Private Function GetMemoBuffer(ByVal strMemo As String, ByVal iCount As Integer) As IntPtr
            Dim dataItem() As CA_DATA_ITEM
            ReDim dataItem(iCount)
            Dim iIndex As Integer
            If strMemo.Length > 0 Then
                ' Description
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CALENDAR
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_DESCRIPTION
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strMemo)
                iIndex += 1
            End If
            ' Allocate memory for buffer
            Dim iSize As Integer = iCount * Marshal.SizeOf(GetType(CA_DATA_ITEM))
            Dim bufDataItem As IntPtr = Marshal.AllocHGlobal(iSize)
            For iIndex = 0 To iCount - 1
                ' Calculate beginning of CA_DATA_ITEM structure of item 'iIndex'
                Dim iPtr As Int64 = bufDataItem.ToInt64 + iIndex * Marshal.SizeOf(GetType(CA_DATA_ITEM))
                ' Convert integer to pointer
                Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
                Marshal.StructureToPtr(dataItem(iIndex), ptr, True)
            Next
            Return bufDataItem
        End Function

        '===================================================================
        ' GetTodoBuffer
        '
        ' Fills meeting items in CA_DATA_ITEM struct and copies it to
        ' unmanaged memory buffer
        '
        '===================================================================
        Private Function GetTodoBuffer(ByVal strTodo As String, ByVal iTodoPriority As Integer, ByVal iTodoAction As Integer, ByVal iCount As Integer) As IntPtr
            Dim dataItem() As CA_DATA_ITEM
            ReDim dataItem(iCount)
            Dim iIndex As Integer
            If strTodo.Length > 0 Then
                ' Description
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CALENDAR
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_DESCRIPTION
                dataItem(iIndex).pCustomData = Marshal.StringToCoTaskMemUni(strTodo)
                iIndex += 1
            End If
            If iTodoPriority > 0 Then
                ' Prt
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CALENDAR
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_TODO_PRIORITY
                dataItem(iIndex).pCustomData = IntPtr.op_Explicit(iTodoPriority)
                iIndex += 1
            End If
            If iTodoAction > 0 Then
                ' Description
                dataItem(iIndex).iSize = Marshal.SizeOf(GetType(CA_DATA_ITEM))
                dataItem(iIndex).iFieldType = CA_FIELD_TYPE_CALENDAR
                dataItem(iIndex).iFieldSubType = CA_FIELD_SUB_TYPE_TODO_STATUS
                dataItem(iIndex).pCustomData = IntPtr.op_Explicit(iTodoAction)
                iIndex += 1
            End If
            ' Allocate memory for buffer
            Dim iSize As Integer = iCount * Marshal.SizeOf(GetType(CA_DATA_ITEM))
            Dim bufDataItem As IntPtr = Marshal.AllocHGlobal(iSize)
            For iIndex = 0 To iCount - 1
                ' Calculate beginning of CA_DATA_ITEM structure of item 'iIndex'
                Dim iPtr As Int64 = bufDataItem.ToInt64 + iIndex * Marshal.SizeOf(GetType(CA_DATA_ITEM))
                ' Convert integer to pointer
                Dim ptr As IntPtr = IntPtr.op_Explicit(iPtr)
                Marshal.StructureToPtr(dataItem(iIndex), ptr, True)
            Next
            Return bufDataItem
        End Function

        '===================================================================
        ' FreeCalendarDataAllocations
        '
        ' Free's gobal memory allocated for clandar write
        '
        '===================================================================
        Private Sub FreeCalendarDataAllocations(ByVal dataCalendar As CA_DATA_CALENDAR)
            Dim pDataItem As IntPtr = dataCalendar.pDataItems
            Dim iItemCount As Byte = dataCalendar.bItemCount
            If iItemCount > 0 Then
                For iIndex As Integer = 0 To iItemCount - 1
                    Dim dataItem As CA_DATA_ITEM = CType(Marshal.PtrToStructure(IntPtr.op_Explicit(pDataItem.ToInt64 + iIndex * Marshal.SizeOf(GetType(CA_DATA_ITEM))), GetType(CA_DATA_ITEM)), CA_DATA_ITEM)
                    If dataItem.iFieldSubType <> CA_FIELD_SUB_TYPE_TODO_PRIORITY And dataItem.iFieldSubType <> CA_FIELD_SUB_TYPE_TODO_STATUS Then
                        If dataItem.iFieldSubType = CA_FIELD_SUB_TYPE_ITEM_DATA Then
                            If dataCalendar.iInfoField <> CA_CALENDAR_ITEM_BIRTHDAY Then
                                Marshal.FreeCoTaskMem(dataItem.pCustomData)
                            End If
                        Else
                            Marshal.FreeCoTaskMem(dataItem.pCustomData)
                        End If
                    End If
                Next
            End If
            Marshal.FreeHGlobal(pDataItem)
        End Sub

        
    End Class

End Namespace