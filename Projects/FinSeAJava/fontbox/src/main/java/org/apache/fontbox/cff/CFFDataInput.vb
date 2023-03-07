Imports System.IO

Namespace org.apache.fontbox.cff

    '/**
    ' * This is specialized DataInput. It's used to parse a CFFFont.
    ' * 
    ' * @author Villu Ruusmann
    ' * @version $Revision$
    ' */
    Public Class CFFDataInput
        Inherits DataInput

        '/**
        ' * Constructor.
        ' * @param buffer the buffer to be read 
        ' */
        Public Sub New(ByVal buffer() As Byte)
            MyBase.New(buffer)
        End Sub

        '/**
        ' * Read one single Card8 value from the buffer. 
        ' * @return the card8 value
        ' * @throws IOException if an error occurs during reading
        ' */
        Public Function readCard8() As Integer ' throws IOException
            Return readUnsignedByte()
        End Function

        '/**
        ' * Read one single Card16 value from the buffer.
        ' * @return the card16 value
        ' * @throws IOException if an error occurs during reading
        ' */
        Public Function readCard16() As Integer 'throws IOException
            Return readUnsignedShort()
        End Function

        '/**
        ' * Read the offset from the buffer.
        ' * @param offSize the given offsize
        ' * @return the offset
        ' * @throws IOException if an error occurs during reading
        ' */
        Public Function readOffset(ByVal offSize As Integer) As Integer 'throws IOException
            Dim value As Integer = 0
            For i As Integer = 0 To offSize - 1
                value = value << 8 Or readUnsignedByte()
            Next
            Return value
        End Function

        '/**
        ' * Read the offsize from the buffer.
        ' * @return the offsize
        ' * @throws IOException if an error occurs during reading
        ' */
        Public Function readOffSize() As Integer ' throws IOException
            Return readUnsignedByte()
        End Function

        '/**
        ' * Read a SID from the buffer.
        ' * @return the SID
        ' * @throws IOException if an error occurs during reading
        ' */
        Public Function readSID() As Integer ' throws IOException
            Return readUnsignedShort()
        End Function


    End Class

End Namespace