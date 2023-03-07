Namespace org.fontbox.cmap

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
            Return Me.[end]
        End Function

        '/** Setter for property end.
        ' * @param endBytes New value of property end.
        ' *
        ' */
        Public Sub setEnd(ByVal endBytes() As Byte)
            [end] = endBytes
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

    End Class

End Namespace