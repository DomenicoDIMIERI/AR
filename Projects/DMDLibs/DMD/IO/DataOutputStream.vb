Namespace Io

    Public Class DataOutputStream
        Inherits FilterOutputStream

        Public Sub New(ByVal out As OutputStream)
            MyBase.New(out)
        End Sub

        ''' <summary>
        ''' Writes a boolean to the underlying output stream as a 1-byte value.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <remarks></remarks>
        Public Sub writeBoolean(ByVal v As Boolean)

        End Sub

        ' ''' <summary>
        ' ''' Writes out a byte to the underlying output stream as a 1-byte value.
        ' ''' </summary>
        ' ''' <param name="v"></param>
        ' ''' <remarks></remarks>
        'Public Sub writeByte(ByVal v As Integer)

        'End Sub

        ''' <summary>
        ''' Writes out the string to the underlying output stream as a sequence of bytes.
        ''' </summary>
        ''' <param name="s"></param>
        ''' <remarks></remarks>
        Public Sub writeBytes(ByVal s As String)

        End Sub

        ''' <summary>
        ''' Writes a char to the underlying output stream as a 2-byte value, high byte first.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <remarks></remarks>
        Public Sub writeChar(ByVal v As Integer)

        End Sub

        ''' <summary>
        ''' Writes a string to the underlying output stream as a sequence of characters.
        ''' </summary>
        ''' <param name="s"></param>
        ''' <remarks></remarks>
        Public Sub writeChars(ByVal s As String)

        End Sub

        ''' <summary>
        ''' Converts the double argument to a long using the doubleToLongBits method in class Double, and then writes that long value to the underlying output stream as an 8-byte quantity, high byte first.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <remarks></remarks>
        Public Sub writeDouble(ByVal v As Double)

        End Sub

        ''' <summary>
        ''' Converts the float argument to an int using the floatToIntBits method in class Float, and then writes that int value to the underlying output stream as a 4-byte quantity, high byte first.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <remarks></remarks>
        Public Sub writeFloat(ByVal v As Single)

        End Sub

        ''' <summary>
        ''' Writes an int to the underlying output stream as four bytes, high byte first.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <remarks></remarks>
        Public Sub writeInt(ByVal v As Integer)

        End Sub

        ''' <summary>
        ''' Writes a long to the underlying output stream as eight bytes, high byte first.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <remarks></remarks>
        Public Sub writeLong(ByVal v As Long)

        End Sub

        ''' <summary>
        ''' Writes a short to the underlying output stream as two bytes, high byte first.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <remarks></remarks>
        Public Sub writeShort(ByVal v As Integer)

        End Sub

        ''' <summary>
        ''' Writes a string to the underlying output stream using modified UTF-8 encoding in a machine-independent manner.
        ''' </summary>
        ''' <param name="str"></param>
        ''' <remarks></remarks>
        Public Sub writeUTF(ByVal str As String)

        End Sub



    End Class

End Namespace