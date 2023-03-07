Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS
Imports DMD.Internals

Partial Class Nokia

    ''' <summary>
    ''' Rappresenta un bookmark
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CBookMarkItem
        Inherits CBaseItem

        Private m_Title As String
        Private m_BookMarkUrl As String
        Private m_UrlShortCut As String

        Public Sub New()
            'dataBookmark.pstrBookMarkUrl = ""
            'dataBookmark.pstrTitle = ""
            'dataBookmark.pstrUrlShortcut = ""
        End Sub

        Friend Sub New(ByVal folder As CBookMarkFolder)
            Me.New()
            If (folder Is Nothing) Then Throw New ArgumentNullException("folder")
            Me.SetDevice(folder.Device)
            Me.SetParentFolder(folder)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il testo del bookmaker
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Title As String
            Get
                Return Me.m_Title
            End Get
            Set(value As String)
                Me.m_Title = value
            End Set
        End Property

        Public Property BookMarkUrl As String
            Get
                Return Me.m_BookMarkUrl
            End Get
            Set(value As String)
                Me.m_BookMarkUrl = value
            End Set
        End Property

        Public Property UrlShortCut As String
            Get
                Return Me.m_UrlShortCut
            End Get
            Set(value As String)
                Me.m_UrlShortCut = value
            End Set
        End Property

        Protected Overrides Sub InternalDelete()
            Throw New NotImplementedException()
        End Sub

        Friend Sub SetData(ByVal dataBookmark As CA_DATA_BOOKMARK)
            Me.m_Title = dataBookmark.pstrTitle
            Me.m_BookMarkUrl = dataBookmark.pstrBookMarkUrl
            Me.m_UrlShortCut = dataBookmark.pstrUrlShortcut
        End Sub

        '===================================================================
        ' FreeBookmarkData
        '
        ' Frees allocated memory of Bookmark
        '
        '===================================================================
        Private Sub FreeBookmarkData()
            '' Free memory allocated by DA API
            'Dim iRet As Integer = CAFreeItemData(Me.Device.BookMarks.m_hBookmark, CA_DATA_FORMAT_STRUCT, m_pBookmarkBuffer)
            'If iRet <> CONA_OK Then ShowErrorMessage("DAFreeItemData", iRet)
            'Marshal.FreeHGlobal(m_pBookmarkBuffer)
            'm_pBookmarkBuffer = IntPtr.Zero
        End Sub



        ''===================================================================
        '' GetBookmarkDetails
        ''
        '' Read selected Bookmark item from phone and show details in list view.
        ''
        ''===================================================================
        'Private Sub GetBookmarkDetails(ByVal pUID As CA_ITEM_ID)
        '    Dim hOperHandle As Integer = 0
        '    Dim iRet As Integer = CABeginOperation(Me.Device.BookMarks.m_hBookmark, 0, hOperHandle)
        '    If iRet <> CONA_OK Then
        '        ShowErrorMessage("CABeginOperation", iRet)
        '    End If
        '    Dim dataBookmark As CA_DATA_BOOKMARK = New CA_DATA_BOOKMARK
        '    iRet = ReadBookmarkItem(pUID, dataBookmark)
        '    If iRet = CONA_OK Then
        '        ' Bookmark data
        '        Dim itemA As New System.Windows.Forms.ListViewItem
        '        itemA.Text = "Title"
        '        LVW_ItemList.Items.Add(itemA)
        '        itemA.SubItems.Add(dataBookmark.pstrTitle)
        '        Dim itemB As New System.Windows.Forms.ListViewItem
        '        itemB.Text = "URL"
        '        LVW_ItemList.Items.Add(itemB)
        '        itemB.SubItems.Add(dataBookmark.pstrBookMarkUrl)
        '        Dim itemC As New System.Windows.Forms.ListViewItem
        '        itemC.Text = "URL Shortcut"
        '        LVW_ItemList.Items.Add(itemC)
        '        itemC.SubItems.Add(dataBookmark.pstrUrlShortcut)
        '        ' Free memory allocated by DA API
        '        FreeBookmarkData()
        '    Else
        '        ShowErrorMessage("CAReadItem", iRet)
        '    End If
        '    iRet = CAEndOperation(hOperHandle)
        '    If iRet <> CONA_OK Then
        '        ShowErrorMessage("CAEndOperation", iRet)
        '    End If
        'End Sub



        ''===================================================================
        '' ReadBookmarkItem
        ''
        '' Reads Bookmark item from device
        ''
        ''===================================================================
        'Private Function ReadBookmarkItem(ByVal UID As CA_ITEM_ID, ByRef dataBookmark As CA_DATA_BOOKMARK) As Integer
        '    Dim hOperHandle As Integer
        '    Dim iRet As Integer = CABeginOperation(m_hCurrentConnection, 0, hOperHandle)
        '    If iRet <> CONA_OK Then
        '        ShowErrorMessage("CABeginOperation", iRet)
        '    End If
        '    dataBookmark.iSize = Marshal.SizeOf(dataBookmark)
        '    dataBookmark.pstrBookMarkUrl = ""
        '    dataBookmark.pstrTitle = ""
        '    dataBookmark.pstrUrlShortcut = ""
        '    ' Allocate memory for buffers
        '    Dim buffer As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(UID))
        '    Marshal.StructureToPtr(UID, buffer, True)
        '    m_pBookmarkBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(dataBookmark))
        '    Marshal.StructureToPtr(dataBookmark, m_pBookmarkBuffer, True)
        '    iRet = CAReadItem(hOperHandle, buffer, CA_OPTION_USE_CACHE, CA_DATA_FORMAT_STRUCT, m_pBookmarkBuffer)
        '    If iRet = CONA_OK Then
        '        ' Copy data from buffer
        '        dataBookmark = Marshal.PtrToStructure(m_pBookmarkBuffer, GetType(CA_DATA_BOOKMARK))
        '    Else
        '        ShowErrorMessage("CAReadItem", iRet)
        '        Marshal.FreeHGlobal(m_pBookmarkBuffer)
        '    End If
        '    Marshal.FreeHGlobal(buffer)
        '    Dim iResult As Integer = CAEndOperation(hOperHandle)
        '    If iResult <> CONA_OK Then
        '        ShowErrorMessage("CAEndOperation", iResult)
        '    End If
        '    Return iRet
        'End Function

        Friend Sub ReadBookMark(ByVal hOperHandle As Integer, UID As CA_ITEM_ID)
            Dim bufId As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(UID))
            Marshal.StructureToPtr(UID, bufId, True)

            Dim data As New CA_DATA_BOOKMARK
            data.iSize = Marshal.SizeOf(data)

            Dim bufData As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(data))
            Marshal.StructureToPtr(data, bufData, True)

            Dim iRet As Integer = CAReadItem(hOperHandle, bufId, CA_OPTION_USE_CACHE, CA_DATA_FORMAT_STRUCT, bufData)
            If iRet = CONA_OK Then
                data = CType(Marshal.PtrToStructure(bufData, GetType(CA_DATA_BOOKMARK)), CA_DATA_BOOKMARK)
                Me.FromInfo(data)
            Else
                ShowErrorMessage("CAReadItem", iRet)
                ' Reading failed, quit loop.
            End If
            ' Free memory allocated by DA API
            iRet = CAFreeItemData(Me.ParentFolder.GetConnectionHandle, CA_DATA_FORMAT_STRUCT, bufData)
            If iRet <> CONA_OK Then ShowErrorMessage("DAFreeItemData", iRet)

            Marshal.FreeHGlobal(bufData)
            Marshal.FreeHGlobal(bufId)
        End Sub

        Friend Sub FromInfo(ByVal info As CA_DATA_BOOKMARK)
            Me.m_Title = info.pstrTitle
            Me.m_BookMarkUrl = info.pstrBookMarkUrl
            Me.m_UrlShortCut = info.pstrUrlShortcut
        End Sub


    End Class

End Class