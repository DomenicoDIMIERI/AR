Namespace org.apache.fontbox.cmap

    '/**
    ' * This represents a single entry in the codespace range.
    ' *
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class CodespaceRange

        Private start() As Byte
        Private [end]() As Byte

        '/**
        ' * Creates a new instance of CodespaceRange.
        ' */
        Public Sub New()
        End Sub


        '/** Getter for property end.
        ' * @return Value of property end.
        ' *
        ' */
        Public Function getEnd() As Byte()
            Return Me.end
        End Function

        '/** Setter for property end.
        ' * @param endBytes New value of property end.
        ' *
        ' */
        Public Sub setEnd(ByVal endBytes() As Byte)
            Me.end = endBytes
        End Sub

        '/** Getter for property start.
        ' * @return Value of property start.
        ' *
        ' */
        Public Function getStart() As Byte()
            Return Me.start
        End Function

        '/** Setter for property start.
        ' * @param startBytes New value of property start.
        ' *
        ' */
        Public Sub setStart(ByVal startBytes() As Byte)
            start = startBytes
        End Sub

        '/**
        ' *  Check whether the given byte array is in Me codespace range or ot.
        ' *  @param code The byte array to look for in the codespace range.
        ' *  @param offset The starting offset within the byte array.
        ' *  @param length The length of the part of the array.
        ' *  
        ' *  @return true if the given byte array is in the codespace range.
        ' */
        Public Function isInRange(ByVal code() As Byte, ByVal offset As Integer, ByVal length As Integer) As Boolean
            If (length < start.Length OrElse length > [end].Length) Then
                Return False
            End If

            If ([end].Length = length) Then
                For i As Integer = 0 To [end].Length - 1
                    Dim endInt As Integer = CInt(Me.end(i)) And &HFF
                    Dim codeInt As Integer = CInt(code(offset + i)) And &HFF
                    If (endInt < codeInt) Then
                        Return False
                    End If
                Next
            End If
            If (start.Length = length) Then
                For i As Integer = 0 To [end].Length - 1
                    Dim startInt As Integer = CInt(start(i)) And &HFF
                    Dim codeInt As Integer = CInt(code(offset + i)) And &HFF
                    If (startInt > codeInt) Then
                        Return False
                    End If
                Next
            End If
            Return True
        End Function


    End Class

End Namespace