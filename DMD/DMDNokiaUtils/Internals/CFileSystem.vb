Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    ''' <summary>
    ''' Rappresenta una cartella che contiene SMS
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CFileSystem
        Implements IDisposable

        Public Event FileOperation(ByVal sender As Object, ByVal e As NokiaFileOperationEventArgs)

        Private pFSCallBack As DMD.Nokia.APIS.FSNotifyCallbackDelegate
        Private m_hFS As Integer
        Private m_Device As NokiaDevice
        Private m_MemoryTypes As CMemoryTypesCollection
        Private m_Root As NokiaFolderInfo
        Private m_InstalledMedia As DeviceMediaCollection
        Private m_FileOperation As FileOperation            'Operazione su file attiva

        Public Sub New()
            Me.m_Device = Nothing
            Me.m_MemoryTypes = Nothing
        End Sub

        Friend Sub New(ByVal device As Nokia.NokiaDevice)
            Me.New()
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            Me.SetDevice(device)
        End Sub

        Public ReadOnly Property Device As NokiaDevice
            Get
                Return Me.m_Device
            End Get
        End Property

        Protected Friend Sub SetDevice(ByVal device As NokiaDevice)
            Me.m_Device = device
            ' Initialize Nokia PC Suite Connectivity API

            ' Initialize File System API
            Dim iRet As Integer = FSAPI_Initialize(FSAPI_VERSION_32, IntPtr.Zero)
            If iRet <> CONA_OK Then ShowErrorMessage("FSAPI_Initialize", iRet)

        End Sub

        Public ReadOnly Property MemoryTypes As CMemoryTypesCollection
            Get
                SyncLock Me.Device
                    If (Me.m_MemoryTypes Is Nothing) Then Me.m_MemoryTypes = New CMemoryTypesCollection(Me)
                    Return Me.m_MemoryTypes
                End SyncLock
            End Get
        End Property

        Friend Function GetHandle() As Integer
            If (Me.m_hFS <> 0) Then Return Me.m_hFS
            Dim iMedia As Integer = API_MEDIA_ALL
            Dim iDeviceID As Integer
            Dim ret As Integer = CONAOpenFS(Me.Device.SerialNumber, iMedia, Me.m_hFS, iDeviceID)
            If ret <> CONA_OK Then ShowErrorMessage("CONAOpenFS", ret)

            '' Register file system notification callback function
            pFSCallBack = AddressOf FSNotifyCallback
            Dim iResult As Integer = CONARegisterFSNotifyCallback(m_hFS, API_REGISTER, pFSCallBack)
            If iResult <> CONA_OK Then ShowErrorMessage("FileOperationListener::StartListening(): CONARegisterFSNotifyCallback", iResult)
            
            Return Me.m_hFS
        End Function

        '===================================================================
        ' FSNotifyCallback
        '
        ' Call back function for file operation notifications
        '===================================================================
        Public Function FSNotifyCallback(ByVal iOperation As Integer, ByVal iStatus As Integer, ByVal iTransferredBytes As Integer, ByVal iAllBytes As Integer) As Integer
            Me.m_FileOperation.Percentage = iStatus
            Dim e As New NokiaFileOperationEventArgs(Me.m_FileOperation)
            RaiseEvent FileOperation(Me, e)
            If (e.Cancel) Then
                Return ECONA_CANCELLED
            Else
                Return CONA_OK
            End If
        End Function

        Friend Sub CloseHandle()
            If (Me.m_hFS = 0) Then Return

            '' Unregister file system notification callback function
            Dim iResult As Integer = CONARegisterFSNotifyCallback(m_hFS, API_UNREGISTER, pFSCallBack)
            If iResult <> CONA_OK Then ShowErrorMessage("FileOperationListener::StopListening(): CONARegisterFSNotifyCallback", iResult)

            Dim ret As Integer = CONACloseFS(Me.m_hFS)
            If ret <> CONA_OK Then ShowErrorMessage("CONACloseFS", ret)
            Me.m_hFS = 0
        End Sub

        Public Sub CopyFilesToDevice(ByVal pcFilePath As String, ByVal deviceFolder As String) 'As FileOperationListener
            Dim hFS As Integer = Me.GetHandle
            Dim strPCFolder As String = System.IO.Path.GetDirectoryName(pcFilePath)
            Dim strPCFile As String = System.IO.Path.GetFileName(pcFilePath)

            Me.m_FileOperation = New FileOperation(FileOperationTypes.Copy, pcFilePath, deviceFolder)
            Dim iResult As Integer = CONACopyFile(hFS, CONA_DIRECT_PC_TO_PHONE Or CONA_RENAME, strPCFile, strPCFolder, deviceFolder)
            Me.m_FileOperation = Nothing

            If iResult = CONA_OK Then
                '  MsgBox("Copy completed succesfully!")
            ElseIf iResult = ECONA_CANCELLED Then
                ' MsgBox("Copy was cancelled.")
                Throw New OperationCanceledException
            Else
                ShowErrorMessage("CopyFilesToDevice: CONACopyFile failed!", iResult)
            End If
        End Sub

        Public Sub MoveFilesToDevice(ByVal pcFilePath As String, ByVal deviceFolder As String)
            Dim hFS As Integer = Me.GetHandle
            Dim strPCFolder As String = System.IO.Path.GetDirectoryName(pcFilePath)
            Dim strPCFile As String = System.IO.Path.GetFileName(pcFilePath)
            Me.m_FileOperation = New FileOperation(FileOperationTypes.Move, pcFilePath, deviceFolder)
            Dim iResult As Integer = CONAMoveFile(hFS, CONA_DIRECT_PC_TO_PHONE, strPCFile, strPCFolder, deviceFolder)
            Me.m_FileOperation = Nothing
            If iResult = CONA_OK Then

            ElseIf iResult = ECONA_CANCELLED Then
                Throw New OperationCanceledException
            Else
                ShowErrorMessage("MoveFilesToDevice: CONAMoveFile failed!", iResult)
            End If
        End Sub

        Public Sub CopyFilesFromDevice(ByVal deviceFilePath As String, ByVal pcFolder As String) 'As FileOperationListener
            Dim hFS As Integer = Me.GetHandle
            Dim strPhoneFolder As String = System.IO.Path.GetDirectoryName(deviceFilePath)
            Dim strPhoneFile As String = System.IO.Path.GetFileName(deviceFilePath)

            Me.m_FileOperation = New FileOperation(FileOperationTypes.Copy, deviceFilePath, pcFolder)
            Dim iResult As Integer = CONACopyFile(hFS, CONA_DIRECT_PHONE_TO_PC Or CONA_RENAME, strPhoneFile, strPhoneFolder, pcFolder)
            Me.m_FileOperation = Nothing

            If iResult = CONA_OK Then
                '  MsgBox("Copy completed succesfully!")
            ElseIf iResult = ECONA_CANCELLED Then
                ' MsgBox("Copy was cancelled.")
                Throw New OperationCanceledException
            Else
                ShowErrorMessage("CopyFilesFromDevice: CONACopyFile failed!", iResult)
            End If
        End Sub

        Public Sub CreateDeviceFolder(ByVal folderName As String)
            Dim hFS As Integer = Me.GetHandle
            Dim strFolderName As String = System.IO.Path.GetFileName(folderName)
            Dim strPhoneFolder As String = System.IO.Path.GetDirectoryName(folderName)
            Me.m_FileOperation = New FileOperation(FileOperationTypes.Create, folderName)
            Dim iResult As Integer = CONACreateFolder(hFS, strFolderName, strPhoneFolder)
            Me.m_FileOperation = Nothing
            If iResult = CONA_OK Then

            ElseIf iResult = ECONA_CANCELLED Then
                Throw New OperationCanceledException
            Else
                ShowErrorMessage("FileBrowser::BTN_Create_Click(): CONACreateFolder failed!", iResult)
            End If
        End Sub

        Public Sub MoveFilesFromDevice(ByVal deviceFilePath As String, ByVal pcFolder As String) 'As FileOperationListener
            Dim hFS As Integer = Me.GetHandle
            Dim strPhoneFolder As String = System.IO.Path.GetDirectoryName(deviceFilePath)
            Dim strPhoneFile As String = System.IO.Path.GetFileName(deviceFilePath)

            Me.m_FileOperation = New FileOperation(FileOperationTypes.Copy, deviceFilePath, pcFolder)
            Dim iResult As Integer = CONAMoveFile(hFS, CONA_DIRECT_PHONE_TO_PC, strPhoneFile, strPhoneFolder, pcFolder)
            Me.m_FileOperation = Nothing

            If iResult = CONA_OK Then
                '  MsgBox("Copy completed succesfully!")
            ElseIf iResult = ECONA_CANCELLED Then
                ' MsgBox("Copy was cancelled.")
                Throw New OperationCanceledException
            Else
                ShowErrorMessage("CopyFilesFromDevice: CONACopyFile failed!", iResult)
            End If
        End Sub

        Public Sub RenameDeviceFolder(ByVal oldName As String, ByVal newName As String)
            Dim hFS As Integer = Me.GetHandle
            Dim strOldName As String = System.IO.Path.GetFileName(oldName)
            Dim strNewName As String = System.IO.Path.GetFileName(newName)
            Dim strPhoneFolder As String = System.IO.Path.GetDirectoryName(newName)
            Dim iResult As Integer = CONARenameFolder(hFS, strOldName, strNewName, strPhoneFolder)
            If iResult <> CONA_OK Then
                ShowErrorMessage("RenameDeviceFolder: CONARenameFile failed!", iResult)
            End If
        End Sub

        Public Sub RenameDeviceFile(ByVal oldName As String, ByVal newName As String)
            Dim hFS As Integer = Me.GetHandle
            Dim strOldName As String = System.IO.Path.GetFileName(oldName)
            Dim strNewName As String = System.IO.Path.GetFileName(newName)
            Dim strPhoneFolder As String = System.IO.Path.GetDirectoryName(newName)
            Dim iResult As Integer = CONARenameFile(hFS, strOldName, strNewName, strPhoneFolder)
            If iResult <> CONA_OK Then
                ShowErrorMessage("RenameDeviceFolder: CONARenameFile failed!", iResult)
            End If
        End Sub

        Public Sub DeleteDeviceFile(ByVal fileName As String)
            Dim hFS As Integer = Me.GetHandle
            Me.m_FileOperation = New FileOperation(FileOperationTypes.Delete, fileName)
            Dim strPhoneFile As String = System.IO.Path.GetFileName(fileName)
            Dim strPhoneFolder As String = System.IO.Path.GetDirectoryName(fileName)
            Dim iResult As Integer = CONADeleteFile(hFS, strPhoneFile, strPhoneFolder)
            Me.m_FileOperation = Nothing
            If iResult = CONA_OK Then
            ElseIf iResult = ECONA_CANCELLED Then
                Throw New OperationCanceledException("Delete file was cancelled.")
            Else
                ShowErrorMessage("FileBrowser::BTN_Delete_Click(): CONADeleteFile failed!", iResult)
            End If
        End Sub

        Public Sub DeleteDeviceFolder(ByVal folderName As String)
            Dim hFS As Integer = Me.GetHandle
            Me.m_FileOperation = New FileOperation(FileOperationTypes.Delete, folderName)
            Dim strFolder As String = System.IO.Path.GetFileName(folderName)
            Dim strPhoneFolder As String = System.IO.Path.GetDirectoryName(folderName)
            Dim iResult As Integer = CONADeleteFolder(hFS, strFolder, CONA_DELETE_FOLDER_WITH_FILES, strPhoneFolder)
            Me.m_FileOperation = Nothing
            If iResult = CONA_OK Then

            ElseIf iResult = ECONA_CANCELLED Then
                Throw New OperationCanceledException("Delete file was cancelled.")
            Else
                ShowErrorMessage("FileBrowser::BTN_Delete_Click(): CONADeleteFile failed!", iResult)
            End If
        End Sub

        Public Function GetFileInfo(ByVal fileName As String) As NokiaFileInfo
            Return New NokiaFileInfo(Me.Device, fileName)
        End Function

        Public Function GetFolderInfo(ByVal folderName As String) As NokiaFolderInfo
            Return New NokiaFolderInfo(Me.Device, folderName)
        End Function


        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Overridable Sub Dispose() Implements IDisposable.Dispose
            Me.CloseHandle()

            ' Terminate File Ssytem API
            Dim iRet As Integer = FSAPI_Terminate(IntPtr.Zero)
            If iRet <> CONA_OK Then ShowErrorMessage("FSAPI_Terminate", iRet)

            Me.pFSCallBack = Nothing
            Me.m_Device = Nothing
            Me.m_MemoryTypes = Nothing
            Me.m_Root = Nothing
            Me.m_InstalledMedia = Nothing
            Me.m_FileOperation = Nothing
        End Sub

        'Public ReadOnly Property Root() As NokiaFolderInfo
        '    Get
        '        SyncLock Me.Device
        '            If (Me.m_Root Is Nothing) Then Me.m_Root = Me.GetFolderInfo("\")
        '            Return Me.m_Root
        '        End SyncLock
        '    End Get
        'End Property

        Public ReadOnly Property InstalledMedia As DeviceMediaCollection
            Get
                SyncLock Me
                    If (Me.m_InstalledMedia Is Nothing) Then Me.m_InstalledMedia = New DeviceMediaCollection(Me.Device)
                    Return Me.m_InstalledMedia
                End SyncLock
            End Get
        End Property
         
    End Class

End Namespace