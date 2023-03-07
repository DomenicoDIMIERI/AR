Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Partial Class Nokia
     

    ''' <summary>
    ''' Rappresenta una cartella del filesystem
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NokiaFolderInfo
        Implements IDisposable


        Private m_Device As NokiaDevice
        Private m_Name As String
        Private m_Location As String
        Private m_Attributes As NokiaFileAttributes
        Private m_DateLastModified As Date?
        Private m_Label As String
        Private m_MemoryType As String
        Private m_IdentificationID As String
        Private m_FreeMemory As Int64
        Private m_TotalMemory As Int64
        Private m_UsedMemory As Int64
        Private m_NumberOfFiles As Integer
        Private m_NumberOfFolders As Integer
        Private m_SizeOfFolderContent As Int64
        Private m_ParentFolder As NokiaFolderInfo

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Device = Nothing
            Me.m_Name = ""
            Me.m_Location = ""
            Me.m_Label = ""
            Me.m_MemoryType = ""
            Me.m_IdentificationID = ""
            Me.m_FreeMemory = -1
            Me.m_TotalMemory = -1
            Me.m_UsedMemory = -1
            Me.m_NumberOfFiles = -1
            Me.m_NumberOfFolders = -1
            Me.m_SizeOfFolderContent = -1
            Me.m_ParentFolder = Nothing
        End Sub

        Friend Sub New(ByVal device As Nokia.NokiaDevice)
            Me.New()
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            Me.SetDevice(device)
        End Sub

        Friend Sub New(ByVal device As Nokia.NokiaDevice, ByVal fileName As String)
            Me.New(device)
            Me.SetFileName(fileName)
        End Sub

        Public ReadOnly Property ParentFolder As NokiaFolderInfo
            Get
                Return Me.m_ParentFolder
            End Get
        End Property

        Protected Friend Sub SetParentFolder(ByVal value As NokiaFolderInfo)
            Me.m_ParentFolder = value
        End Sub

        Public ReadOnly Property Device As NokiaDevice
            Get
                Return Me.m_Device
            End Get
        End Property

        Protected Friend Sub SetDevice(ByVal device As NokiaDevice)
            Me.m_Device = device
        End Sub

        Public ReadOnly Property FullPath As String
            Get
                'If (Me.m_Location = "") Then Return Me.m_Name
                Return Me.m_Location & "\" & Me.m_Name
            End Get
        End Property

        Public ReadOnly Property FileName As String
            Get
                Return Me.m_Name
            End Get
        End Property

        Public ReadOnly Property DirectoryName As String
            Get
                Return Me.m_Location
            End Get
        End Property

        Public ReadOnly Property Extension As String
            Get
                Return System.IO.Path.GetExtension(Me.m_Name)
            End Get
        End Property

        Public ReadOnly Property BaseName As String
            Get
                Return System.IO.Path.GetFileNameWithoutExtension(Me.m_Name)
            End Get
        End Property

        Public ReadOnly Property Attributes As NokiaFileAttributes
            Get
                Return Me.m_Attributes
            End Get
        End Property

        Public ReadOnly Property AttributesEx As String
            Get
                Dim values() As NokiaFileAttributes = CType([Enum].GetValues(GetType(NokiaFileAttributes)), NokiaFileAttributes())
                Dim ret As String = ""
                For Each v As NokiaFileAttributes In values
                    If v <> NokiaFileAttributes.None Then
                        If (Me.m_Attributes And v) = v Then
                            If (ret <> "") Then ret &= ","
                            ret &= [Enum].GetName(GetType(NokiaFileAttributes), v)
                        End If
                    End If
                Next
                Return ret
            End Get
        End Property

        Public ReadOnly Property DateLastModified As Date?
            Get
                Return Me.m_DateLastModified
            End Get
        End Property

        Public ReadOnly Property Label As String
            Get
                Return Me.m_Label
            End Get
        End Property

        Public ReadOnly Property MemoryType As String
            Get
                Return Me.m_MemoryType
            End Get
        End Property

        Public ReadOnly Property IdentificationID As String
            Get
                Return Me.m_IdentificationID
            End Get
        End Property

        Public ReadOnly Property FreeMemory As Int64
            Get
                Return Me.m_FreeMemory
            End Get
        End Property

        Public ReadOnly Property TotalMemory As Int64
            Get
                Return Me.m_TotalMemory
            End Get
        End Property

        Public ReadOnly Property UsedMemory As Int64
            Get
                Return Me.m_UsedMemory
            End Get
        End Property

        Public ReadOnly Property NumberOfFiles As Integer
            Get
                Return Me.m_NumberOfFiles
            End Get
        End Property

        Public ReadOnly Property NumberOfFolders As Integer
            Get
                Return Me.m_NumberOfFolders
            End Get
        End Property

        Public ReadOnly Property SizeOfFolderContent As Int64
            Get
                Return Me.m_SizeOfFolderContent
            End Get
        End Property


        Protected Friend Sub SetFileName(ByVal value As String)
            Me.m_Name = value
            'If (Right(Me.m_FullPath, 1) = "\") Then Me.m_FullPath = Left(Me.m_FullPath, Len(Me.m_FullPath) - 1)
            Me.Refresh()
        End Sub

        Public Sub Refresh()
            'ptrFileInfo = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CONAPI_FILE_INFO)))
            'Marshal.StructureToPtr(stFileInfo, ptrFileInfo, True)
            Dim info As CONAPI_FOLDER_CONTENT
            Dim hFS As Integer = Me.Device.FileSystem.GetHandle

            info.iSize = Marshal.SizeOf(GetType(CONAPI_FOLDER_CONTENT))

            Dim fName As String = Me.m_Name
            Dim iResult As Integer
            If (Me.m_Name = "\" OrElse Me.m_Name = "\\" OrElse Me.m_Name = "") Then
                iResult = CONAGetFolderInfo(hFS, CONA_GET_FOLDER_INFO, vbNullString, vbNullString, info, Nothing)
            Else
                iResult = CONAGetFolderInfo(hFS, CONA_GET_FOLDER_INFO, "", Me.m_Location & "\" & Me.m_Name, info, Nothing)
            End If
            If iResult <> CONA_OK Then ShowErrorMessage("SetFileName", iResult)

            Dim stFolderInfo As CONAPI_FOLDER_INFO2 = CType(Marshal.PtrToStructure(info.pFolderInfo, GetType(CONAPI_FOLDER_INFO2)), CONAPI_FOLDER_INFO2)

            '"Name" : stFileInfo.pstrName)

            'listItem.Text = "File permission"
            Me.m_Attributes = CType(stFolderInfo.iAttributes, NokiaFileAttributes) ' Permissions2String(stFileInfo.iAttributes))

            Me.m_DateLastModified = GetLocalFormattedDate(stFolderInfo.tFolderTime)

            'dlgFileInfo.Text = "Folder Info"
            Me.m_Name = stFolderInfo.pstrName
            If (Me.m_Name = "\\") Then Me.m_Name = "\"
            
            Me.m_Location = stFolderInfo.pstrLocation

            Me.m_Label = stFolderInfo.pstrLabel

            Me.m_MemoryType = stFolderInfo.pstrMemoryType

            Me.m_IdentificationID = stFolderInfo.pstrID

            Me.m_FreeMemory = stFolderInfo.dlFreeMemory

            Me.m_TotalMemory = stFolderInfo.dlTotalMemory

            Me.m_UsedMemory = stFolderInfo.dlUsedMemory

            Me.m_NumberOfFiles = stFolderInfo.iContainFiles

            Me.m_NumberOfFolders = stFolderInfo.iContainFolders

            Me.m_SizeOfFolderContent = stFolderInfo.dlTotalSize
        End Sub

        Public Function GetAllFolders() As NokiaFolderInfo()
            Return Me.GetAllFolders(NokiaFindFileFlags.None)
        End Function

        Public Function GetAllFolders(ByVal options As NokiaFindFileFlags) As NokiaFolderInfo()
            Dim hFind As Integer = 0
            Dim fullPath As String = Me.FullPath
            If (Right(fullPath, 1) = "\") Then fullPath = Left(fullPath, Len(fullPath) - 1)
            Dim iResult As Integer = CONAFindBegin(Me.Device.FileSystem.GetHandle, options, hFind, FullPath)
            If iResult <> CONA_OK Then ShowErrorMessage("CONAFindBegin(): CONAFindEnd failed!", iResult)

            Dim ret As New System.Collections.ArrayList

            ' Allocate memory for buffer
            Dim info As CONAPI_FOLDER_INFO 'IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType()))
            iResult = CONAFindNextFolder(hFind, info)
            While iResult = CONA_OK
                ' Copy data from buffer
                'FolderInfo = Marshal.PtrToStructure(Buffer, GetType(CONAPI_FOLDER_INFO))
                'Dim index As Integer = Me.Items.Add("[" + FolderInfo.pstrName + "]")
                ' Setting folder name as itemdata
                Dim item As New NokiaFolderInfo(Me.Device)
                item.SetParentFolder(Me)
                item.FromInfo(info, Me.FullPath)
                If (item.Attributes And NokiaFileAttributes.Folder) = NokiaFileAttributes.Folder Then ret.Add(item)

                iResult = CONAFreeFolderInfoStructure(info)
                If iResult <> CONA_OK Then ShowErrorMessage("PhoneListBox::ShowFolders(): CONAFreeFolderInfoStructure failed", iResult)
                iResult = CONAFindNextFolder(hFind, info)
            End While
            If iResult <> ECONA_ALL_LISTED And iResult <> CONA_OK Then ShowErrorMessage("PhoneListBox::ShowFolders(): CONAFindNextFolder failed!", iResult)

            Return CType(ret.ToArray(GetType(NokiaFolderInfo)), NokiaFolderInfo())
        End Function

        Public Function GetAllFiles() As NokiaFileInfo()
            Return Me.GetAllFiles(NokiaFindFileFlags.None)
        End Function

        Public Function GetAllFiles(ByVal options As NokiaFindFileFlags) As NokiaFileInfo()
            Dim hFind As Integer = 0
            Dim fullPath As String = Me.FullPath
            If (Right(fullPath, 1) = "\") Then fullPath = Left(fullPath, Len(fullPath) - 1)
            Dim iResult As Integer = CONAFindBegin(Me.Device.FileSystem.GetHandle, options, hFind, fullPath)
            If iResult <> CONA_OK Then ShowErrorMessage("CONAFindBegin(): CONAFindEnd failed!", iResult)
            Dim ret As New System.Collections.ArrayList
            ' Allocate memory for buffer
            Dim info As CONAPI_FILE_INFO '= Marshal.AllocHGlobal(Marshal.SizeOf(GetType()))
            iResult = CONAFindNextFile(hFind, info)
            While iResult = CONA_OK
                ' Copy data from buffer
                'FileInfo = Marshal.PtrToStructure(Buffer, GetType(CONAPI_FILE_INFO))
                ' Setting file name as itemdata
                Dim item As New NokiaFileInfo(Me.Device)
                item.SetParentFolder(Me)
                item.FromInfo(info)
                If Not ( _
                    (item.Attributes And NokiaFileAttributes.Folder) = NokiaFileAttributes.Folder
                    ) Then
                    ret.Add(item)
                End If
                iResult = CONAFreeFileInfoStructure(info)
                If iResult <> CONA_OK Then ShowErrorMessage("PhoneListBox::ShowFiles(): CONAFreeFileInfoStructure failed!", iResult)
                iResult = CONAFindNextFile(hFind, info)
            End While
            If iResult <> ECONA_ALL_LISTED And iResult <> CONA_OK Then ShowErrorMessage("PhoneListBox::ShowFiles(): CONAFindNextFile failed!", iResult)

            Return CType(ret.ToArray(GetType(NokiaFileInfo)), NokiaFileInfo())
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If

            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

        Friend Sub FromInfo(stFolderInfo As CONAPI_FOLDER_INFO, ByVal location As String)

            'Dim stFolderInfo As CONAPI_FOLDER_INFO2 = Marshal.PtrToStructure(info.pFolderInfo, GetType(CONAPI_FOLDER_INFO2))

            '"Name" : stFileInfo.pstrName)

            'listItem.Text = "File permission"
            Me.m_Attributes = CType(stFolderInfo.iAttributes, NokiaFileAttributes) ' Permissions2String(stFileInfo.iAttributes))

            Me.m_DateLastModified = GetLocalFormattedDate(stFolderInfo.tFolderTime)

            'dlgFileInfo.Text = "Folder Info"
            Me.m_Name = stFolderInfo.pstrName
            Me.m_Location = location
            Me.m_Label = stFolderInfo.pstrLabel
            Me.m_MemoryType = stFolderInfo.pstrMemoryType
            'Me.Refresh()
          

            'Dim info As CONAPI_FOLDER_CONTENT
            'Dim hFS As Integer = Me.Device.FileSystem.GetHandle

            'info.iSize = Marshal.SizeOf(GetType(CONAPI_FOLDER_CONTENT))

            'Dim iResult As Integer = CONAGetFolderInfo(hFS, CONA_GET_FOLDER_INFO, Me.m_Name, Me.m_Location, info, Nothing)
            'If iResult <> CONA_OK Then ShowErrorMessage("SetFileName", iResult)

            'Dim stFolderInfo2 As CONAPI_FOLDER_INFO2 = Marshal.PtrToStructure(info.pFolderInfo, GetType(CONAPI_FOLDER_INFO2))

            'Me.m_Name = stFolderInfo2.pstrName
            'Me.m_Location = stFolderInfo2.pstrLocation

            ''listItem.Text = "File permission"
            'Me.m_Attributes = stFolderInfo2.iAttributes ' Permissions2String(stFileInfo.iAttributes))

            'Me.m_DateLastModified = GetLocalFormattedDate(stFolderInfo2.tFolderTime)

            ''dlgFileInfo.Text = "Folder Info"
            ''"Name -> stFolderInfo.pstrName)

            'Me.m_Label = stFolderInfo2.pstrLabel

            'Me.m_MemoryType = stFolderInfo2.pstrMemoryType

            'Me.m_IdentificationID = stFolderInfo2.pstrID

            'Me.m_FreeMemory = stFolderInfo2.dlFreeMemory

            'Me.m_TotalMemory = stFolderInfo2.dlTotalMemory

            'Me.m_UsedMemory = stFolderInfo2.dlUsedMemory

            'Me.m_NumberOfFiles = stFolderInfo2.iContainFiles

            'Me.m_NumberOfFolders = stFolderInfo2.iContainFolders

            'Me.m_SizeOfFolderContent = stFolderInfo2.dlTotalSize

        End Sub

        Public Overrides Function ToString() As String
            If (Me.m_Label <> "") Then
                Return Me.m_Label
            Else
                Return Me.FileName
            End If
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class