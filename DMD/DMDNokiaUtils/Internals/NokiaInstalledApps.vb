Imports System.Runtime.InteropServices
Imports DMD.Internals
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Namespace Internals

    ''' <summary>
    ''' Rappresenta una cartella del calendario
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NokiaInstalledApps
        Inherits System.Collections.ReadOnlyCollectionBase



        Private m_Device As Nokia.NokiaDevice

        Public Sub New()

        End Sub

        Friend Sub New(ByVal device As Nokia.NokiaDevice)
            Me.New()
            Me.Load(device)
        End Sub

        Default Public ReadOnly Property Item(ByVal index As Integer) As NokiaInstalledApp
            Get
                Return DirectCast(Me.InnerList.Item(index), NokiaInstalledApp)
            End Get
        End Property

        Friend Sub Load(ByVal device As NokiaDevice)
            If (device Is Nothing) Then Throw New ArgumentNullException("device")
            Me.InnerList.Clear()
            Me.m_Device = device


            ' Phone is selected
            Dim iMedia As Integer
            Dim iResult As Integer
            Dim iCount As Integer
            Dim resPtr As IntPtr
            Dim i As Integer
            ' Clear dialog application list
            iMedia = API_MEDIA_ALL
            ' Create FS connection

            ' List installed applications in phone
            iCount = 0
            resPtr = IntPtr.Zero
            iResult = CONAListApplications(device.FileSystem.GetHandle, CONA_LIST_ALL_APPICATIONS, iCount, resPtr)
            If iResult <> CONA_OK Then ShowErrorMessage("CONAListApplications", iResult)

            ' Add each application found to the dialog application list box
            If iCount = 0 Then Exit Sub

            ' Map pointer to application info structure
            Dim pApplication As CONAPI_APPLICATION_INFO = CType(Marshal.PtrToStructure(resPtr, GetType(CONAPI_APPLICATION_INFO)), CONAPI_APPLICATION_INFO)
            ' Loop trough array of application info
            For i = 0 To iCount - 1
                ' Calculate beginning of CONAPI_APPLICATION_INFO structure of item 'i'
                Dim iPtr As Integer = resPtr.ToInt32 + i * Marshal.SizeOf(GetType(CONAPI_APPLICATION_INFO))
                ' Convert integer to pointer
                Dim tmpPtr As IntPtr = IntPtr.op_Explicit(iPtr)
                ' Copy data from buffer
                pApplication = CType(Marshal.PtrToStructure(tmpPtr, GetType(CONAPI_APPLICATION_INFO)), CONAPI_APPLICATION_INFO)

                Dim app As New NokiaInstalledApp(device)
                app.FromInfo(pApplication)

                Me.InnerList.Add(app)
            Next

            ' Free reserved application info resources
            iResult = CONAFreeApplicationInfoStructures(iCount, resPtr)
            If iResult <> CONA_OK Then ShowErrorMessage("CONAFreeApplicationInfoStructures", iResult)
        End Sub

        Friend Sub Remove(ByVal app As NokiaInstalledApp)
            Me.InnerList.Remove(app)
        End Sub

        Friend Sub Add(ByVal app As NokiaInstalledApp)
            Me.InnerList.Add(app)
        End Sub

        Public Function InstallTheme(ByVal strFile As String) As NokiaInstalledApp
            Dim strNthFile As String
            Dim path As String
            Dim nthFile As CONAPI_APPLICATION_FILE
            Dim dwOptions As Integer = CONA_DEFAULT_FOLDER Or CONA_OVERWRITE Or CONA_WAIT_THAT_USER_ACTION_IS_DONE

            If Trim(strFile) = "" Then Throw New ArgumentNullException("File name is missing")
            
            strNthFile = System.IO.Path.GetFileName(strFile)
            nthFile.pstrFileName = strNthFile
            path = System.IO.Path.GetDirectoryName(strFile)
            Dim ret As Integer = CONAInstallApplication(Me.m_Device.FileSystem.GetHandle, CONA_APPLICATION_TYPE_THEMES, nthFile, dwOptions, path, vbNullString)
            If ret = CONA_OK Then
                'MsgBox("Application installation succeeded")
            ElseIf ret = CONA_OK_BUT_USER_ACTION_NEEDED Then
                ' Either device does not support waiting or the maximum waiting time exceeded
                Me.m_Device.OnRequireUserAction(New System.EventArgs)
            Else
                ShowErrorMessage("CONAInstallApplication", ret)
            End If

            Return Nothing
        End Function

        Public Function InstallSymbianApplication(ByVal strFile As String) As NokiaInstalledApp
            Dim sisFile As String
            Dim path As String
            Dim sisFiles As CONAPI_APPLICATION_SIS
            Dim dwOptions As Integer = CONA_DEFAULT_FOLDER Or CONA_OVERWRITE Or CONA_WAIT_THAT_USER_ACTION_IS_DONE

            If Trim(strFile) = "" Then Throw New ArgumentNullException("File name is missing")

            sisFile = System.IO.Path.GetFileName(strFile)
            sisFiles.pstrFileNameSis = sisFile
            path = System.IO.Path.GetDirectoryName(strFile)
            Dim ret As Integer = CONAInstallApplication(Me.m_Device.FileSystem.GetHandle, CONA_APPLICATION_TYPE_SIS, sisFiles, dwOptions, path, vbNullString)
            If ret = CONA_OK Then

            ElseIf ret = CONA_OK_BUT_USER_ACTION_NEEDED Then
                ' Either device does not support waiting or the maximum waiting time exceeded
                Me.m_Device.OnRequireUserAction(New System.EventArgs)
            Else
                ShowErrorMessage("CONAInstallApplication", ret)
            End If

            Return Nothing
        End Function

        Public Function InstallNGageApplication(ByVal strFile As String) As NokiaInstalledApp
            Dim nGageFile As String
            Dim sourcePath As String
            Dim targetPath As String = "\\C:\Data\" 'Phone memory is used
            Dim nGageFolder As String = "N-Gage"
            Dim iOptions As Integer = CONA_DIRECT_PC_TO_PHONE Or CONA_OVERWRITE
            Dim iResult As Integer

            If Trim(strFile) = "" Then Throw New ArgumentNullException("File name is missing")

            ' Split source file name in path and file name
            nGageFile = System.IO.Path.GetFileName(strFile)
            sourcePath = System.IO.Path.GetDirectoryName(strFile)
            ' Create n-gage folder on Phone
            iResult = CONACreateFolder(Me.m_Device.FileSystem.GetHandle, nGageFolder, targetPath)
            If iResult <> CONA_OK And iResult <> ECONA_FOLDER_ALREADY_EXIST Then ShowErrorMessage("CONACreateFolder", iResult)

            ' Append n-gage foldr to target path
            targetPath = targetPath & nGageFolder
            ' N-Gage application is installed by copying file to predefined folder,
            Dim ret As Integer = CONACopyFile(Me.m_Device.FileSystem.GetHandle, iOptions, nGageFile, sourcePath, targetPath)
            If ret <> CONA_OK Then ShowErrorMessage("CONACopyFile", ret)

            Return Nothing
        End Function

        Public Function InstallJavaApplication(ByVal strJarFile As String, ByVal strJadFile As String) As NokiaInstalledApp
            Dim jarFile As String
            Dim jadFile As String
            Dim path As String
            Dim javaFiles As CONAPI_APPLICATION_JAVA
            Dim dwOptions As Integer = CONA_DEFAULT_FOLDER Or CONA_OVERWRITE Or CONA_WAIT_THAT_USER_ACTION_IS_DONE

            If Trim(strJarFile) = "" Then Throw New ArgumentNullException("File name is missing")

            jarFile = System.IO.Path.GetFileName(strJarFile)
            jadFile = System.IO.Path.GetFileName(strJadFile)
            javaFiles.pstrFileNameJar = jarFile
            If Len(jadFile) = 0 Then
                javaFiles.pstrFileNameJad = vbNullString
            Else
                javaFiles.pstrFileNameJad = jadFile
            End If
            path = System.IO.Path.GetDirectoryName(strJarFile)

            Dim ret As Integer = CONAInstallApplication(Me.m_Device.FileSystem.GetHandle, CONA_APPLICATION_TYPE_JAVA, javaFiles, dwOptions, path, vbNullString)
            If ret = CONA_OK_BUT_USER_ACTION_NEEDED Then
                ' Either device does not support waiting or the maximum waiting time exceeded
                'MsgBox("User action needed on the device side")
                Me.m_Device.OnRequireUserAction(New System.EventArgs)
            ElseIf ret <> CONA_OK Then
                ShowErrorMessage("CONAInstallApplication", ret)
            End If

            'TO DO: Recover the new installed app info
            Return Nothing
        End Function

    End Class

End Namespace