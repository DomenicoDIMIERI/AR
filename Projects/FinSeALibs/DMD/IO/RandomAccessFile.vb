Namespace Io

    Public Class RandomAccessFile

        Private _file As System.IO.Stream
        Private _mode As String
        Private m_Reader As System.IO.BinaryReader
        Private m_Writer As System.IO.BinaryWriter

        Sub New(file As DMD.Io.File, mode As String)
            ' TODO: Complete member initialization 
            Me._file = New System.IO.FileStream(file.getFullName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite)
            Me._mode = mode
            Me.m_Reader = New System.IO.BinaryReader(Me._file)
            Me.m_Writer = New System.IO.BinaryWriter(Me._file)
        End Sub

        ''' <summary>
        ''' Closes this random access file stream and releases any system resources associated with the stream.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub close()
            Me.m_Reader.Close()
            Me.m_Writer.Close()
            Me._file.Close()
        End Sub

        ''' <summary>
        ''' Returns the unique FileChannel object associated with this file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getChannel() As FileChannel
            Throw New NotImplementedException
        End Function

        ''' <summary>
        ''' Returns the opaque file descriptor object associated with this stream.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getFD() As FileDescriptor

        End Function

        ''' <summary>
        ''' Returns the current offset in this file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getFilePointer() As Long
            Return Me._file.Position
        End Function

        ''' <summary>
        ''' Returns the length of this file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function length() As Long
            Return Me._file.Length
        End Function

        ''' <summary>
        ''' Reads a byte of data from this file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function read() As Integer
            Return Me._file.ReadByte
        End Function

        ''' <summary>
        ''' Reads up to b.length bytes of data from this file into an array of bytes.
        ''' </summary>
        ''' <param name="b"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function read(ByVal b() As Byte) As Integer
            Return Me._file.Read(b, 0, b.Length)
        End Function

        ''' <summary>
        ''' Reads up to len bytes of data from this file into an array of bytes.
        ''' </summary>
        ''' <param name="b"></param>
        ''' <param name="off"></param>
        ''' <param name="len"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function read(ByVal b() As Byte, ByVal off As Integer, ByVal len As Integer) As Integer
            Return Me._file.Read(b, off, len)
        End Function

        ''' <summary>
        ''' Reads a boolean from this file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function readBoolean() As Boolean
            Return Me.m_Reader.ReadBoolean
        End Function

        ''' <summary>
        ''' Reads a signed eight-bit value from this file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function readByte() As Byte
            Return Me.m_Reader.ReadByte
        End Function

        ''' <summary>
        ''' Reads a character from this file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function readChar() As Char
            Return Me.m_Reader.ReadChar
        End Function

        ''' <summary>
        ''' Reads a double from this file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function readDouble() As Double
            Return Me.m_Reader.ReadDouble
        End Function

        ''' <summary>
        ''' Reads a float from this file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function readFloat() As Single
            Return Me.m_Reader.ReadSingle
        End Function

        ''' <summary>
        ''' Reads b.length bytes from this file into the byte array, starting at the current file pointer.
        ''' </summary>
        ''' <param name="b"></param>
        ''' <remarks></remarks>
        Public Sub readFully(ByVal b() As Byte)
            Me.m_Reader.Read(b, 0, b.Length)
        End Sub

        ''' <summary>
        ''' Reads exactly len bytes from this file into the byte array, starting at the current file pointer.
        ''' </summary>
        ''' <param name="b"></param>
        ''' <param name="off"></param>
        ''' <param name="len"></param>
        ''' <remarks></remarks>
        Public Sub readFully(ByVal b() As Byte, ByVal off As Integer, ByVal len As Integer)
            Me.m_Reader.Read(b, off, len)
        End Sub

        ''' <summary>
        ''' Reads a signed 32-bit integer from this file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function readInt() As Integer
            Return Me.m_Reader.ReadInt32
        End Function

        ''' <summary>
        ''' Reads the next line of text from this file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function readLine() As String
            Dim ret As New System.Text.StringBuilder
            Dim ch As Integer = Me.read
            While ch <> -1 AndAlso ch <> 0
                ret.Append(Convert.ToChar(ch))
            End While
            Return ret.ToString
        End Function

        ''' <summary>
        ''' Reads a signed 64-bit integer from this file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function readLong() As Long
            Return Me.m_Reader.ReadInt64
        End Function

        ''' <summary>
        ''' Reads a signed 16-bit number from this file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function readShort() As Short
            Return Me.m_Reader.ReadInt16
        End Function

        ''' <summary>
        ''' Reads an unsigned eight-bit number from this file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function readUnsignedByte() As Integer
            Return Me.m_Reader.ReadByte
        End Function

        ''' <summary>
        ''' Reads an unsigned 16-bit number from this file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function readUnsignedShort() As Integer
            Return Me.m_Reader.ReadUInt16
        End Function

        ''' <summary>
        ''' Reads in a string from this file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function readUTF() As String
            Dim ret As New System.Text.StringBuilder
            Dim ch As Integer = Me.read
            While ch <> -1 AndAlso ChrW(ch) <> vbNullChar
                ret.Append(Convert.ToChar(ch))
            End While
            Return ret.ToString
        End Function

        ''' <summary>
        ''' Sets the file-pointer offset, measured from the beginning of this file, at which the next read or write occurs.
        ''' </summary>
        ''' <param name="pos"></param>
        ''' <remarks></remarks>
        Public Sub seek(ByVal pos As Long)
            Me._file.Position = pos
        End Sub

        ''' <summary>
        ''' Sets the length of this file.
        ''' </summary>
        ''' <param name="newLength"></param>
        ''' <remarks></remarks>
        Public Sub setLength(ByVal newLength As Long)
            Me._file.SetLength(newLength)
        End Sub

        ''' <summary>
        ''' Attempts to skip over n bytes of input discarding the skipped bytes.
        ''' </summary>
        ''' <param name="n"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function skipBytes(ByVal n As Integer) As Integer
            Dim p As Long = Me._file.Position
            Me._file.Position += n
            Return Me._file.Position
        End Function

        ''' <summary>
        ''' Writes b.length bytes from the specified byte array to this file, starting at the current file pointer.
        ''' </summary>
        ''' <param name="b"></param>
        ''' <remarks></remarks>
        Public Sub write(ByVal b() As Byte)
            Me.m_Writer.Write(b, 0, b.Length)
        End Sub

        ''' <summary>
        ''' Writes len bytes from the specified byte array starting at offset off to this file.
        ''' </summary>
        ''' <param name="b"></param>
        ''' <param name="off"></param>
        ''' <param name="len"></param>
        ''' <remarks></remarks>
        Public Sub write(ByVal b() As Byte, ByVal off As Integer, ByVal len As Integer)
            Me.m_Writer.Write(b, off, len)
        End Sub

        ''' <summary>
        ''' Writes the specified byte to this file.
        ''' </summary>
        ''' <param name="b"></param>
        ''' <remarks></remarks>
        Public Sub write(ByVal b As Integer)
            Me.m_Writer.Write(CByte(b))
        End Sub

        ''' <summary>
        ''' Writes a boolean to the file as a one-byte value.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <remarks></remarks>
        Public Sub writeBoolean(ByVal v As Boolean)
            Me.m_Writer.Write(v)
        End Sub

        ''' <summary>
        ''' Writes a byte to the file as a one-byte value.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <remarks></remarks>
        Public Sub writeByte(ByVal v As Integer)
            Me.m_Writer.Write(v)
        End Sub

        ''' <summary>
        ''' Writes the string to the file as a sequence of bytes.
        ''' </summary>
        ''' <param name="s"></param>
        ''' <remarks></remarks>
        Public Sub writeBytes(ByVal s As String)
            Me.m_Writer.Write(Sistema.Strings.GetBytes(s & vbNullChar))
        End Sub

        ''' <summary>
        ''' Writes a char to the file as a two-byte value, high byte first.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <remarks></remarks>
        Public Sub writeChar(ByVal v As Char)
            Me.m_Writer.Write(v)
        End Sub

        ''' <summary>
        ''' Writes a string to the file as a sequence of characters.
        ''' </summary>
        ''' <param name="s"></param>
        ''' <remarks></remarks>
        Public Sub writeChars(ByVal s As String)
            Me.m_Writer.Write(s)
        End Sub

        ''' <summary>
        ''' Converts the double argument to a long using the doubleToLongBits method in class Double, and then writes that long value to the file as an eight-byte quantity, high byte first.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <remarks></remarks>
        Public Sub writeDouble(ByVal v As Double)
            Me.m_Writer.Write(v)
        End Sub

        ''' <summary>
        ''' Converts the float argument to an int using the floatToIntBits method in class Float, and then writes that int value to the file as a four-byte quantity, high byte first.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <remarks></remarks>
        Public Sub writeFloat(ByVal v As Single)
            Me.m_Writer.Write(v)
        End Sub

        ''' <summary>
        ''' Writes an int to the file as four bytes, high byte first.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <remarks></remarks>
        Public Sub writeInt(ByVal v As Integer)
            Me.m_Writer.Write(v)
        End Sub

        ''' <summary>
        ''' Writes a long to the file as eight bytes, high byte first.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <remarks></remarks>
        Public Sub writeLong(ByVal v As Long)
            Me.m_Writer.Write(v)
        End Sub

        ''' <summary>
        ''' Writes a short to the file as two bytes, high byte first.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <remarks></remarks>
        Public Sub writeShort(ByVal v As Short)
            Me.m_Writer.Write(v)
        End Sub

        ''' <summary>
        ''' Writes a string to the file using modified UTF-8 encoding in a machine-independent manner.
        ''' </summary>
        ''' <param name="str"></param>
        ''' <remarks></remarks>
        Public Sub writeUTF(ByVal str As String)
            Me.m_Writer.Write(Sistema.Strings.GetBytes(str))
        End Sub


        'Methods inherited from class java.lang.Object
        '        clone, Equals, Finalize, getClass, hashCode, notify, notifyAll, ToString, wait, wait, wait


    End Class

End Namespace