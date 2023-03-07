Imports FinSeA.Io
Imports System.IO

Namespace org.apache.pdfbox.io

    '/**
    ' * This class represents an ASCII85 stream.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' *
    ' */
    Public Class ASCII85InputStream
        Inherits FilterInputStream

        Private index As Integer
        Private n As Integer
        Private eof As Boolean

        Private ascii() As Byte
        Private b() As Byte

        Private Shared ReadOnly TERMINATOR As Byte = Asc("~")
        Private Shared ReadOnly OFFSET As Byte = Asc("!")
        Private Shared ReadOnly NEWLINE As Byte = Asc(vbLf) ''\n';
        Private Shared ReadOnly [RETURN] As Byte = Asc(vbCr) ''\r'
        Private Shared ReadOnly SPACE As Byte = Asc(" ")
        Private Shared ReadOnly PADDING_U As Byte = Asc("u")
        Private Shared ReadOnly Z As Byte = Asc("z")

        ''' <summary>
        '''Constructor. 
        ''' </summary>
        ''' <param name="is">The input stream to actually read from.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal [is] As Stream)
            MyBase.New([is])
            Me.index = 0
            Me.n = 0
            Me.eof = False
            ReDim Me.ascii(5 - 1)
            ReDim Me.b(4 - 1)
        End Sub

        '/**
        ' * This will read the next byte from the stream.
        ' *
        ' * @return The next byte read from the stream.
        ' *
        ' * @throws IOException If there is an error reading from the wrapped stream.
        ' */
        Public Overrides Function read() As Integer 'throws IOException
            Dim k As Integer
            Dim z As Byte

            If (index >= n) Then
                If (eof) Then
                    Return -1
                End If
                index = 0
                Do
                    Dim zz As Integer = [in].read()
                    If (zz = -1) Then
                        eof = True
                        Return -1
                    End If
                    z = zz
                Loop While (z = NEWLINE OrElse z = [RETURN] OrElse z = SPACE)

                If (z = TERMINATOR) Then
                    eof = True
                    ascii = Nothing
                    b = Nothing
                    n = 0
                    Return -1
                ElseIf (z = z) Then
                    b(0) = 0 : b(1) = 0 : b(2) = 0 : b(3) = 0
                    n = 4
                Else
                    ascii(0) = z ' may be EOF here....
                    For k = 1 To 5 - 1
                        Do
                            Dim zz As Integer = [in].read()
                            If (zz = -1) Then
                                eof = True
                                Return -1
                            End If
                            z = zz
                        Loop While (z = NEWLINE OrElse z = [RETURN] OrElse z = SPACE)
                        ascii(k) = z
                        If (z = TERMINATOR) Then
                            ' don't include ~ as padding byte
                            ascii(k) = PADDING_U
                            Exit For
                        End If
                    Next
                    n = k - 1
                    If (n = 0) Then
                        eof = True
                        ascii = Nothing
                        b = Nothing
                        Return -1
                    End If
                    If (k < 5) Then
                        k += 1
                        While (k < 5) 'for (++k; k < 5; ++k)
                            k += 1
                            ' use 'u' for padding
                            ascii(k) = PADDING_U
                        End While
                        eof = True
                    End If
                    ' decode stream
                    Dim t As Integer = 0
                    For k = 0 To 5 - 1
                        z = (ascii(k) - OFFSET)
                        If (z < 0 OrElse z > 93) Then
                            n = 0
                            eof = True
                            ascii = Nothing
                            b = Nothing
                            Throw New IOException("Invalid data in Ascii85 stream")
                        End If
                        t = (t * 85L) + z
                    Next
                    For k = 3 To 0 Step -1
                        b(k) = (t And &HFFL)
                        t >>= 8
                    Next
                End If
            End If
            Dim ret As Integer = b(index) And &HFF : index += 1
            Return ret
        End Function

        '/**
        ' * This will read a chunk of data.
        ' *
        ' * @param data The buffer to write data to.
        ' * @param offset The offset into the data stream.
        ' * @param len The number of byte to attempt to read.
        ' *
        ' * @return The number of bytes actually read.
        ' *
        ' * @throws IOException If there is an error reading data from the underlying stream.
        ' */
        Public Overrides Function read(ByVal data() As Byte, ByVal offset As Integer, ByVal len As Integer) As Integer ' throws IOException
            If (eof AndAlso index >= n) Then Return -1
            For i As Integer = 0 To len - 1
                If (index < n) Then
                    data(i + offset) = b(index) : index += 1
                Else
                    Dim t As Integer = read()
                    If (t = -1) Then Return i
                    data(i + offset) = t
                End If
            Next
            Return len
        End Function

        '/**
        ' * This will close the underlying stream and release any resources.
        ' *
        ' * @throws IOException If there is an error closing the underlying stream.
        ' */
        Public Overrides Sub close() 'throws IOException
            ascii = Nothing
            eof = True
            b = Nothing
            MyBase.Close()
        End Sub

        '/**
        ' * non supported interface methods.
        ' *
        ' * @return False always.
        ' */
        Public Overrides Function markSupported() As Boolean
            Return False
        End Function

        '/**
        ' * Unsupported.
        ' *
        ' * @param nValue ignored.
        ' *
        ' * @return Always zero.
        ' */
        Public Overrides Function skip(ByVal nValue As Long) As Long
            Return 0
        End Function

        '/**
        ' * Unsupported.
        ' *
        ' * @return Always zero.
        ' */
        Public Overrides Function available() As Integer
            Return 0
        End Function

        '/**
        ' * Unsupported.
        ' *
        ' * @param readlimit ignored.
        ' */
        Public Overrides Sub mark(ByVal readlimit As Integer)
        End Sub

        '/**
        ' * Unsupported.
        ' *
        ' * @throws IOException telling that this is an unsupported action.
        ' */
        Public Overrides Sub reset() 'throws IOException
            Throw New IOException("Reset is not supported")
        End Sub

    End Class

End Namespace


