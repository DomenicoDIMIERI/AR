Imports System.IO

Namespace org.apache.pdfbox.io


    '/**
    ' * Provides {@link InputStream} access to portions of a file combined with
    ' * buffered reading of content. Start of next bytes to read can be set via seek
    ' * method.
    ' * 
    ' * File is accessed via {@link RandomAccessFile} and is read in byte chunks
    ' * which are cached.
    ' * 
    ' * @author Timo Boehme (timo.boehme at ontochem com)
    ' */
    Public Class RandomAccessBufferedFileInputStream
        Inherits FinSeA.Io.InputStream
        Implements RandomAccessRead

        Private pageSizeShift As Integer = 12
        Private pageSize As Integer = 1 << pageSizeShift
        Private pageOffsetMask As Integer = -1L << pageSizeShift
        Private maxCachedPages As Integer = 1000

        Private lastRemovedCachePage() As Byte = Nothing

        ' Create a LRU page cache. */
        Private pageCache As New LHM(Of Integer, Byte())(maxCachedPages, 0.75F, True)

#Region "LH"

        Private Class LHM(Of K, V)
            Inherits LinkedHashMap(Of K, V)

            Private maxCachedPages As K
            Private lastRemovedCachePage As V
            Private _p2 As Single
            Private _p3 As Boolean

            Sub New(maxCachedPages As K, p2 As Single, p3 As Boolean)
                ' TODO: Complete member initialization 
                Me.maxCachedPages = maxCachedPages
                _p2 = p2
                _p3 = p3
            End Sub


            Protected Overrides Function removeEldestEntry(ByVal _eldest As Map.Entry(Of K, V)) As Boolean
                Dim doRemove As Boolean = size() > Convert.ToInt32(maxCachedPages)
                If (doRemove) Then
                    lastRemovedCachePage = _eldest.Value
                End If
                Return doRemove
            End Function

        End Class

#End Region
        'Private Shared serialVersionUID As Integer = -6302488539257741101L


        Private curPageOffset As Integer = -1
        Private curPage As Byte() = Array.CreateInstance(GetType(Byte), pageSize)
        Private offsetWithinPage As Integer = 0

        Private raFile As RandomAccessFile
        Private fileLength As Integer
        Private fileOffset As Integer = 0

        ''' <summary>
        ''' Create input stream instance for given file. 
        ''' </summary>
        ''' <param name="_file"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal _file As FinSeA.Io.File) 'throws(FileNotFoundException, IOException)
            raFile = New RandomAccessFile(_file, "r")
            fileLength = _file.Length
            seek(0)
        End Sub

        '// ------------------------------------------------------------------------
        '/**
        ' *  Returns offset in file at which next byte would be read.
        ' *  
        ' *  @deprecated  use {@link #getPosition()} instead
        ' */
        Public Function getFilePointer() As Integer
            Return fileOffset
        End Function

        '// ------------------------------------------------------------------------
        '/** Returns offset in file at which next byte would be read. */
        Public Function getPosition() As Long Implements RandomAccessRead.getPosition
            Return fileOffset
        End Function

        '// ------------------------------------------------------------------------
        '/**
        ' * Seeks to new position. If new position is outside of current page the new
        ' * page is either taken from cache or read from file and added to cache.
        ' */
        Public Overrides Sub seek(ByVal newOffset As Long) Implements RandomAccessRead.seek
            Dim newPageOffset As Long = newOffset And pageOffsetMask
            If (newPageOffset <> curPageOffset) Then
                Dim newPage() As Byte = pageCache.get(newPageOffset)
                If (newPage Is Nothing) Then
                    raFile.Seek(newPageOffset)
                    newPage = readPage()
                    pageCache.put(newPageOffset, newPage)
                End If
                curPageOffset = newPageOffset
                curPage = newPage
            End If

            offsetWithinPage = (newOffset - curPageOffset)
            fileOffset = newOffset
        End Sub

        '// ------------------------------------------------------------------------
        '/**
        ' * Reads a page with data from current file position. If we have a
        ' * previously removed page from cache the buffer of this page is reused.
        ' * Otherwise a new byte buffer is created.
        ' */
        Private Function readPage() As Byte() ' throws IOException
            Dim page() As Byte

            If (lastRemovedCachePage IsNot Nothing) Then
                page = lastRemovedCachePage
                lastRemovedCachePage = Nothing
            Else
                page = Array.CreateInstance(GetType(Byte), pageSize)
            End If

            Dim readBytes As Integer = 0
            While (readBytes < pageSize)
                Dim curBytesRead As Integer = raFile.Read(page, readBytes, pageSize - readBytes)
                If (curBytesRead < 0) Then ' EOF
                    Exit While
                End If
                readBytes += curBytesRead
            End While

            Return page
        End Function

        '// ------------------------------------------------------------------------
        '@Override
        Public Overrides Function read() As Integer Implements SequentialRead.read
            If (fileOffset >= fileLength) Then Return -1

            If (offsetWithinPage = pageSize) Then
                seek(fileOffset)
            End If

            fileOffset += 1
            offsetWithinPage += 1
            Return curPage(offsetWithinPage - 1) And &HFF
        End Function

        Public Overrides Function read(ByVal b() As Byte, ByVal off As Integer, ByVal len As Integer) As Integer Implements SequentialRead.read
            If (fileOffset >= fileLength) Then Return -1
            If (offsetWithinPage = pageSize) Then
                seek(fileOffset)
            End If

            Dim commonLen As Integer = Math.Min(pageSize - offsetWithinPage, len)
            If ((fileLength - fileOffset) < pageSize) Then
                commonLen = Math.Min(commonLen, (fileLength - fileOffset))
            End If
            Array.Copy(curPage, offsetWithinPage, b, off, commonLen)

            offsetWithinPage += commonLen
            fileOffset += commonLen

            Return commonLen
        End Function

        Public Overrides Function available() As Integer ' throws IOException
            Return Math.Min(fileLength - fileOffset, Integer.MaxValue)
        End Function

        Public Overrides Function skip(ByVal n As Long) As Long ' throws IOException
            ' test if we have to reduce skip count because of EOF
            Dim toSkip As Long = n

            If (fileLength - fileOffset < toSkip) Then
                toSkip = fileLength - fileOffset
            End If

            If ((toSkip < pageSize) AndAlso ((offsetWithinPage + toSkip) <= pageSize)) Then
                ' we can skip within current page
                offsetWithinPage += toSkip
                fileOffset += toSkip
            Else
                ' seek to the page we will get after skipping
                seek(fileOffset + toSkip)
            End If

            Return toSkip
        End Function

        Public Overrides ReadOnly Property Length As Long Implements RandomAccessRead.length
            Get
                Return fileLength
            End Get
        End Property

        Public Overrides Sub close() Implements SequentialRead.close
            raFile.Close()
            pageCache.clear()
        End Sub

    End Class

End Namespace