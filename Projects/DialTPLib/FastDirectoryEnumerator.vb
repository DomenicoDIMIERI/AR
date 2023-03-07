Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Runtime.ConstrainedExecution
Imports System.Runtime.InteropServices
Imports System.Security.Permissions
Imports Microsoft.Win32.SafeHandles

Namespace CodeProject

    ''' <summary>
    ''' Contains information about a file returned by the 
    ''' <see cref="FastDirectoryEnumerator"/> class.
    ''' </summary>
    <Serializable>
    Public Class FileData
        Implements IComparable

        ''' <summary>
        ''' Attributes of the file.
        ''' </summary>
        Public ReadOnly Attributes As FileAttributes

        Public ReadOnly Property CreationTime As DateTime
            Get
                Return Me.CreationTimeUtc.ToLocalTime()
            End Get
        End Property

        ''' <summary>
        ''' File creation time in UTC
        ''' </summary>
        Public ReadOnly CreationTimeUtc As DateTime

        ''' <summary>
        ''' Gets the last access time in local time.
        ''' </summary>
        Public ReadOnly Property LastAccesTime As DateTime
            Get
                Return Me.LastAccessTimeUtc.ToLocalTime()
            End Get
        End Property

        ''' <summary>
        ''' File last access time in UTC
        ''' </summary>
        Public ReadOnly LastAccessTimeUtc As DateTime

        ''' <summary>
        ''' Gets the last access time in local time.
        ''' </summary>
        Public ReadOnly Property LastWriteTime As DateTime
            Get
                Return Me.LastWriteTimeUtc.ToLocalTime()
            End Get
        End Property

        ''' <summary>
        ''' File last write time in UTC
        ''' </summary>
        Public ReadOnly LastWriteTimeUtc As DateTime

        ''' <summary>
        ''' Size of the file in bytes
        ''' </summary>
        Public ReadOnly Size As Long

        ''' <summary>
        ''' Name of the file
        ''' </summary>
        Public ReadOnly Name As String

        ''' <summary>
        ''' Full path to the file.
        ''' </summary>
        Public ReadOnly Path As String

        ''' <summary>
        ''' Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        ''' </summary>
        ''' <returns>
        ''' A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        ''' </returns>
        Public Overrides Function ToString() As String
            Return Me.Name
        End Function

        ''' <summary>
        ''' Initializes a new instance of the <see cref="FileData"/> class.
        ''' </summary>
        ''' <param name="dir">The directory that the file is stored at</param>
        ''' <param name="findData">WIN32_FIND_DATA structure that this
        ''' object wraps.</param>
        Friend Sub New(ByVal dir As String, ByVal findData As WIN32_FIND_DATA)
            Me.Attributes = findData.dwFileAttributes


            Me.CreationTimeUtc = ConvertDateTime(findData.ftCreationTime_dwHighDateTime, findData.ftCreationTime_dwLowDateTime)

            Me.LastAccessTimeUtc = ConvertDateTime(findData.ftLastAccessTime_dwHighDateTime, findData.ftLastAccessTime_dwLowDateTime)

            Me.LastWriteTimeUtc = ConvertDateTime(findData.ftLastWriteTime_dwHighDateTime, findData.ftLastWriteTime_dwLowDateTime)

            Me.Size = CombineHighLowInts(findData.nFileSizeHigh, findData.nFileSizeLow)

            Me.Name = findData.cFileName
            Me.Path = System.IO.Path.Combine(dir, findData.cFileName)
        End Sub

        Private Shared Function CombineHighLowInts(ByVal high As UInteger, ByVal low As UInteger) As Long
            Return ((CLng(high)) << &H20) Or low
        End Function

        Private Shared Function ConvertDateTime(ByVal high As UInteger, ByVal low As UInteger) As DateTime
            Dim fileTime As Long = CombineHighLowInts(high, low)
            Return DateTime.FromFileTimeUtc(fileTime)
        End Function

        Public Sub Delete()
            System.IO.File.Delete(Me.Path)
        End Sub

        Public Function CompareTo(ByVal value As FileData) As Integer
            Return String.Compare(Me.Name, value.Name, False)
        End Function

        Private Function _CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Throw New NotImplementedException()
        End Function
    End Class

    ''' <summary>
    ''' Contains information about the file that is found 
    ''' by the FindFirstFile or FindNextFile functions.
    ''' </summary>
    <Serializable, StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto), BestFitMapping(False)>
    Friend Structure WIN32_FIND_DATA
        Public dwFileAttributes As FileAttributes
        Public ftCreationTime_dwLowDateTime As UInteger
        Public ftCreationTime_dwHighDateTime As UInteger
        Public ftLastAccessTime_dwLowDateTime As UInteger
        Public ftLastAccessTime_dwHighDateTime As UInteger
        Public ftLastWriteTime_dwLowDateTime As UInteger
        Public ftLastWriteTime_dwHighDateTime As UInteger
        Public nFileSizeHigh As UInteger
        Public nFileSizeLow As UInteger
        Public dwReserved0 As Integer
        Public dwReserved1 As Integer
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)> Public cFileName As String
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=14)> Public cAlternateFileName As String

        ''' <summary>
        ''' Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        ''' </summary>
        ''' <returns>
        ''' A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        ''' </returns>
        Public Overrides Function ToString() As String
            Return "File name=" & Me.cFileName
        End Function
    End Structure

    ''' <summary>
    ''' A fast enumerator of files in a directory.  Use this if you need to get attributes for 
    ''' all files in a directory.
    ''' </summary>
    ''' <remarks>
    ''' This enumerator is substantially faster than using <see cref="Directory.GetFiles(String)"/>
    ''' and then creating a new FileInfo object for each path.  Use this version when you 
    ''' will need to look at the attibutes of each file returned (for example, you need
    ''' to check each file in a directory to see if it was modified after a specific date).
    ''' </remarks>
    Public NotInheritable Class FastDirectoryEnumerator


        ''' <summary>
        ''' Gets <see cref="FileData"/> for all the files in a directory.
        ''' </summary>
        ''' <param name="path">The path to search.</param>
        ''' <returns>An object that implements <see cref="IEnumerable{FileData}"/> and 
        ''' allows you to enumerate the files in the given directory.</returns>
        ''' <exception cref="ArgumentNullException">
        ''' <paramref name="path"/> is a null reference (Nothing in VB)
        ''' </exception>
        Public Shared Function EnumerateFiles(ByVal path As String) As IEnumerable(Of FileData)
            Return FastDirectoryEnumerator.EnumerateFiles(path, "*")
        End Function

        ''' <summary>
        ''' Gets <see cref="FileData"/> for all the files in a directory that match a 
        ''' specific filter.
        ''' </summary>
        ''' <param name="path">The path to search.</param>
        ''' <param name="searchPattern">The search string to match against files in the path.</param>
        ''' <returns>An object that implements <see cref="IEnumerable{FileData}"/> and 
        ''' allows you to enumerate the files in the given directory.</returns>
        ''' <exception cref="ArgumentNullException">
        ''' <paramref name="path"/> is a null reference (Nothing in VB)
        ''' </exception>
        ''' <exception cref="ArgumentNullException">
        ''' <paramref name="filter"/> is a null reference (Nothing in VB)
        ''' </exception>
        Public Shared Function EnumerateFiles(ByVal path As String, ByVal searchPattern As String) As IEnumerable(Of FileData)
            Return FastDirectoryEnumerator.EnumerateFiles(path, searchPattern, SearchOption.TopDirectoryOnly)
        End Function

        ''' <summary>
        ''' Gets <see cref="FileData"/> for all the files in a directory that 
        ''' match a specific filter, optionally including all sub directories.
        ''' </summary>
        ''' <param name="path">The path to search.</param>
        ''' <param name="searchPattern">The search string to match against files in the path.</param>
        ''' <param name="searchOption">
        ''' One of the SearchOption values that specifies whether the search 
        ''' operation should include all subdirectories or only the current directory.
        ''' </param>
        ''' <returns>An object that implements <see cref="IEnumerable{FileData}"/> and 
        ''' allows you to enumerate the files in the given directory.</returns>
        ''' <exception cref="ArgumentNullException">
        ''' <paramref name="path"/> is a null reference (Nothing in VB)
        ''' </exception>
        ''' <exception cref="ArgumentNullException">
        ''' <paramref name="filter"/> is a null reference (Nothing in VB)
        ''' </exception>
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' <paramref name="searchOption"/> is not one of the valid values of the
        ''' <see cref="System.IO.SearchOption"/> enumeration.
        ''' </exception>
        Public Shared Function EnumerateFiles(ByVal path As String, ByVal searchPattern As String, ByVal searchOption As SearchOption) As IEnumerable(Of FileData)
            If (String.IsNullOrEmpty(path)) Then Throw New ArgumentNullException("path")
            If (String.IsNullOrEmpty(searchPattern)) Then Throw New ArgumentNullException("searchPattern")
            If ((searchOption <> SearchOption.TopDirectoryOnly) AndAlso (searchOption <> SearchOption.AllDirectories)) Then Throw New ArgumentOutOfRangeException("searchOption")

            Dim fullPath As String = System.IO.Path.GetFullPath(path)

            Return New FileEnumerable(fullPath, searchPattern, searchOption)
        End Function

        ''' <summary>
        ''' Gets <see cref="FileData"/> for all the files in a directory that match a 
        ''' specific filter.
        ''' </summary>
        ''' <param name="path">The path to search.</param>
        ''' <param name="searchPattern">The search string to match against files in the path.</param>
        ''' <returns>An object that implements <see cref="IEnumerable{FileData}"/> and 
        ''' allows you to enumerate the files in the given directory.</returns>
        ''' <exception cref="ArgumentNullException">
        ''' <paramref name="path"/> is a null reference (Nothing in VB)
        ''' </exception>
        ''' <exception cref="ArgumentNullException">
        ''' <paramref name="filter"/> is a null reference (Nothing in VB)
        ''' </exception>
        Public Shared Function GetFiles(ByVal path As String, ByVal searchPattern As String, ByVal searchOption As SearchOption) As FileData()
            Dim e As IEnumerable(Of FileData) = FastDirectoryEnumerator.EnumerateFiles(path, searchPattern, searchOption)
            Dim list As New List(Of FileData)(e)

            Dim retval As FileData() = CType(Array.CreateInstance(GetType(FileData), list.Count), FileData())
            list.CopyTo(retval)

            Return retval
        End Function

        ''' <summary>
        ''' Provides the implementation of the 
        ''' <see cref="T:System.Collections.Generic.IEnumerable`1"/> interface
        ''' </summary>
        Private Class FileEnumerable
            Implements IEnumerable(Of FileData)

            Private ReadOnly m_path As String
            Private ReadOnly m_filter As String
            Private ReadOnly m_searchOption As SearchOption

            ''' <summary>
            ''' Initializes a new instance of the <see cref="FileEnumerable"/> class.
            ''' </summary>
            ''' <param name="path">The path to search.</param>
            ''' <param name="filter">The search string to match against files in the path.</param>
            ''' <param name="searchOption">
            ''' One of the SearchOption values that specifies whether the search 
            ''' operation should include all subdirectories or only the current directory.
            ''' </param>
            Public Sub New(ByVal path As String, ByVal filter As String, ByVal searchOption As SearchOption)
                m_path = path
                m_filter = filter
                m_searchOption = searchOption
            End Sub

#Region "IEnumerable<FileData> Members"

            ''' <summary>
            ''' Returns an enumerator that iterates through the collection.
            ''' </summary>
            ''' <returns>
            ''' A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can 
            ''' be used to iterate through the collection.
            ''' </returns>
            Public Function GetEnumerator() As IEnumerator(Of FileData) Implements IEnumerable(Of FileData).GetEnumerator
                Return New FileEnumerator(m_path, m_filter, m_searchOption)
            End Function

#End Region


#Region "IEnumerable Members"

            ''' <summary>
            ''' Returns an enumerator that iterates through a collection.
            ''' </summary>
            ''' <returns>
            ''' An <see cref="T:System.Collections.IEnumerator"/> object that can be 
            ''' used to iterate through the collection.
            ''' </returns>
            Private Function _GetEnumerator() As System.Collections.IEnumerator Implements IEnumerable.GetEnumerator
                Return New FileEnumerator(m_path, m_filter, m_searchOption)
            End Function

#End Region
        End Class


        ''' <summary>
        ''' Wraps a FindFirstFile handle.
        ''' </summary>
        Private NotInheritable Class SafeFindHandle
            Inherits SafeHandleZeroOrMinusOneIsInvalid

            <ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)>
            <DllImport("kernel32.dll")>
            Private Shared Function FindClose(ByVal handle As IntPtr) As Boolean

            End Function


            ''' <summary>
            ''' Initializes a new instance of the <see cref="SafeFindHandle"/> class.
            ''' </summary>
            <SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode:=True)>
            Friend Sub New()
                MyBase.New(True)
            End Sub

            ''' <summary>
            ''' When overridden in a derived class, executes the code required to free the handle.
            ''' </summary>
            ''' <returns>
            ''' true if the handle is released successfully; otherwise, in the 
            ''' event of a catastrophic failure, false. In this case, it 
            ''' generates a releaseHandleFailed MDA Managed Debugging Assistant.
            ''' </returns>
            Protected Overrides Function ReleaseHandle() As Boolean
                Return FindClose(MyBase.handle)
            End Function

        End Class

        ''' <summary>
        ''' Provides the implementation of the 
        ''' <see cref="T:System.Collections.Generic.IEnumerator`1"/> interface
        ''' </summary>
        <System.Security.SuppressUnmanagedCodeSecurity>
        Private Class FileEnumerator
            Implements IEnumerator(Of FileData)

            <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
            Private Shared Function FindFirstFile(ByVal fileName As String, ByRef data As WIN32_FIND_DATA) As SafeFindHandle

            End Function


            <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
            Private Shared Function FindNextFile(ByVal hndFindFile As SafeFindHandle, ByRef lpFindFileData As WIN32_FIND_DATA) As Boolean '<MarshalAs(UnmanagedType.LPStruct)>

            End Function

            ''' <summary>
            ''' Hold context information about where we current are in the directory search.
            ''' </summary>
            Private Class SearchContext
                Public ReadOnly Path As String
                Public SubdirectoriesToProcess As Stack(Of String)

                Public Sub New(ByVal path As String)
                    Me.Path = path
                End Sub
            End Class

            Private m_path As String
            Private m_filter As String
            Private m_searchOption As SearchOption
            Private m_contextStack As Stack(Of SearchContext)
            Private m_currentContext As SearchContext

            Private m_hndFindFile As SafeFindHandle
            Private m_win_find_data As WIN32_FIND_DATA = New WIN32_FIND_DATA()

            ''' <summary>
            ''' Initializes a new instance of the <see cref="FileEnumerator"/> class.
            ''' </summary>
            ''' <param name="path">The path to search.</param>
            ''' <param name="filter">The search string to match against files in the path.</param>
            ''' <param name="searchOption">
            ''' One of the SearchOption values that specifies whether the search 
            ''' operation should include all subdirectories or only the current directory.
            ''' </param>
            Public Sub New(ByVal path As String, ByVal filter As String, ByVal searchOption As SearchOption)
                m_path = path
                m_filter = filter
                m_searchOption = searchOption
                m_currentContext = New SearchContext(path)

                If (m_searchOption = SearchOption.AllDirectories) Then
                    m_contextStack = New Stack(Of SearchContext)()
                End If
            End Sub

#Region "IEnumerator<FileData> Members"

            ''' <summary>
            ''' Gets the element in the collection at the current position of the enumerator.
            ''' </summary>
            ''' <value></value>
            ''' <returns>
            ''' The element in the collection at the current position of the enumerator.
            ''' </returns>
            Public ReadOnly Property Current As FileData Implements IEnumerator(Of FileData).Current
                Get
                    Return New FileData(m_path, m_win_find_data)
                End Get
            End Property

#End Region

#Region "IDisposable Members"

            ''' <summary>
            ''' Performs application-defined tasks associated with freeing, releasing, 
            ''' or resetting unmanaged resources.
            ''' </summary>
            Public Sub Dispose() Implements IDisposable.Dispose
                If (m_hndFindFile IsNot Nothing) Then
                    m_hndFindFile.Dispose()
                    m_hndFindFile = Nothing
                End If
            End Sub

#End Region

#Region "IEnumerator Members"

            ''' <summary>
            ''' Gets the element in the collection at the current position of the enumerator.
            ''' </summary>
            ''' <value></value>
            ''' <returns>
            ''' The element in the collection at the current position of the enumerator.
            ''' </returns>
            Private ReadOnly Property _Current As Object Implements System.Collections.IEnumerator.Current
                Get
                    Return New FileData(m_path, m_win_find_data)
                End Get
            End Property

            ''' <summary>
            ''' Advances the enumerator to the next element of the collection.
            ''' </summary>
            ''' <returns>
            ''' true if the enumerator was successfully advanced to the next element; 
            ''' false if the enumerator has passed the end of the collection.
            ''' </returns>
            ''' <exception cref="T:System.InvalidOperationException">
            ''' The collection was modified after the enumerator was created.
            ''' </exception>
            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                Dim retval As Boolean = False

                'If the handle Is null, this Is first call to MoveNext in the current 
                ' directory.  In that case, start a New search.
                If (m_currentContext.SubdirectoriesToProcess Is Nothing) Then
                    If (m_hndFindFile Is Nothing) Then

                        Dim p As New FileIOPermission(FileIOPermissionAccess.PathDiscovery, m_path)
                        p.Demand()

                        Dim searchPath As String = System.IO.Path.Combine(m_path, m_filter)
                        m_hndFindFile = FindFirstFile(searchPath, m_win_find_data)
                        retval = Not m_hndFindFile.IsInvalid
                    Else
                        'Otherwise, find the next item.
                        retval = FindNextFile(m_hndFindFile, m_win_find_data)
                    End If
                End If

                'If the call to FindNextFile Or FindFirstFile succeeded...
                If (retval) Then
                    If ((m_win_find_data.dwFileAttributes And FileAttributes.Directory) = FileAttributes.Directory) Then
                        'Ignore folders for now.   We call MoveNext recursively here to 
                        ' move to the next item that FindNextFile will return.
                        Return MoveNext()
                    End If
                ElseIf (m_searchOption = SearchOption.AllDirectories) Then
                    'SearchContext context = New SearchContext(m_hndFindFile, m_path);
                    'm_contextStack.Push(context);
                    'm_path = Path.Combine(m_path, m_win_find_data.cFileName);
                    'm_hndFindFile = null;

                    If (m_currentContext.SubdirectoriesToProcess Is Nothing) Then
                        Dim subDirectories As String() = Directory.GetDirectories(m_path)
                        m_currentContext.SubdirectoriesToProcess = New Stack(Of String)(subDirectories)
                    End If

                    If (m_currentContext.SubdirectoriesToProcess.Count > 0) Then
                        Dim subDir As String = m_currentContext.SubdirectoriesToProcess.Pop()

                        m_contextStack.Push(m_currentContext)
                        m_path = subDir
                        m_hndFindFile = Nothing
                        m_currentContext = New SearchContext(m_path)
                        Return MoveNext()
                    End If

                    'If there are no more files in this directory And we are 
                    'in a sub directory, pop back up to the parent directory And
                    'continue the search from there.
                    If (m_contextStack.Count > 0) Then
                        m_currentContext = m_contextStack.Pop()
                        m_path = m_currentContext.Path
                        If (m_hndFindFile IsNot Nothing) Then
                            m_hndFindFile.Close()
                            m_hndFindFile = Nothing
                        End If

                        Return MoveNext()
                    End If
                End If

                Return retval
            End Function

            ''' <summary>
            ''' Sets the enumerator to its initial position, which is before the first element in the collection.
            ''' </summary>
            ''' <exception cref="T:System.InvalidOperationException">
            ''' The collection was modified after the enumerator was created.
            ''' </exception>
            Public Sub Reset() Implements IEnumerator.Reset
                m_hndFindFile = Nothing
            End Sub

#End Region
        End Class
    End Class

End Namespace