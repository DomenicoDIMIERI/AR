Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Partial Class Nokia

    <Flags> _
    Public Enum NokiaFileAttributes As Integer
        None = 0
        Read = CONA_FPERM_READ
        Write = CONA_FPERM_WRITE
        [Delete] = CONA_FPERM_DELETE
        Hidden = CONA_FPERM_HIDDEN
        Folder = CONA_FPERM_FOLDER
        Drive = CONA_FPERM_DRIVE
        [Root] = CONA_FPERM_ROOT
    End Enum

    ''' <summary>
    ''' Rappresenta un file del filesystem sulla periferica
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NokiaFileInfo
        Implements IDisposable


        Private m_Device As NokiaDevice
        Private m_FullPath As String
        Private m_Attributes As NokiaFileAttributes
        Private m_DateLastModified As Date
        Private m_Size As Int64
        Private m_MIMEType As String
        Private m_ParentFolder As NokiaFolderInfo

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Device = Nothing
            Me.m_FullPath = ""
            Me.m_Size = 0
            Me.m_MIMEType = ""
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
                Return Me.m_FullPath
            End Get
        End Property

        Public ReadOnly Property FileName As String
            Get
                Return System.IO.Path.GetFileName(Me.m_FullPath)
            End Get
        End Property

        Public ReadOnly Property DirectoryName As String
            Get
                Return System.IO.Path.GetDirectoryName(Me.m_FullPath)
            End Get
        End Property

        Public ReadOnly Property Extension As String
            Get
                Return System.IO.Path.GetExtension(Me.m_FullPath)
            End Get
        End Property

        Public ReadOnly Property BaseName As String
            Get
                Return System.IO.Path.GetFileNameWithoutExtension(Me.m_FullPath)
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

        Public ReadOnly Property DateLastModified As Date
            Get
                Return Me.m_DateLastModified
            End Get
        End Property

        Public ReadOnly Property Size As Int64
            Get
                Return Me.m_Size
            End Get
        End Property

        Public ReadOnly Property MIMEType As String
            Get
                Return Me.m_MIMEType
            End Get
        End Property


        Protected Friend Sub SetFileName(ByVal value As String)
            Me.m_FullPath = value
            Me.Refresh()
        End Sub

        Public ReadOnly Property ParentFolder As NokiaFolderInfo
            Get
                Return Me.m_ParentFolder
            End Get
        End Property

        Protected Friend Sub SetParentFolder(ByVal value As NokiaFolderInfo)
            Me.m_ParentFolder = value
        End Sub

        Public Sub Refresh()
            'ptrFileInfo = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(CONAPI_FILE_INFO)))
            'Marshal.StructureToPtr(stFileInfo, ptrFileInfo, True)
            Dim info As CONAPI_FILE_INFO
            Dim hFS As Integer = Me.Device.FileSystem.GetHandle

            Dim iResult As Integer = CONAGetFileInfo(hFS, Me.FileName, info, Me.DirectoryName)
            If iResult <> CONA_OK Then ShowErrorMessage("SetFileName", iResult)

            '"Name" : stFileInfo.pstrName)

            'listItem.Text = "File permission"
            Me.m_Attributes = CType(info.iAttributes, NokiaFileAttributes) ' Permissions2String(stFileInfo.iAttributes))

            Me.m_DateLastModified = CType(GetLocalFormattedDate(info.tFileTime), Date)

            Me.m_Size = info.iFileSize

            Me.m_MIMEType = info.pstrMIMEType
        End Sub



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

        Friend Sub FromInfo(info As CONAPI_FILE_INFO)
            Me.m_FullPath = info.pstrName

            'listItem.Text = "File permission"
            Me.m_Attributes = CType(info.iAttributes, NokiaFileAttributes) ' Permissions2String(stFileInfo.iAttributes))

            Me.m_DateLastModified = CType(GetLocalFormattedDate(info.tFileTime), Date)

            Me.m_Size = info.iFileSize

            Me.m_MIMEType = info.pstrMIMEType
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_FullPath
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class