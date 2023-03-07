Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.common

    '/**
    ' * This class will be used for matrix manipulation.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PDMatrix
        Implements COSObjectable

        Private matrix As COSArray

        '// the number of row elements depends on the number of elements
        '   // within the given matrix
        '   // 3x3 e.g. Matrix of a CalRGB colorspace dictionary
        '   // 3x2 e.g. FontMatrix of a type 3 font
        Private numberOfRowElements As Integer = 3

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            matrix = New COSArray()
            matrix.add(New COSFloat(1.0F))
            matrix.add(New COSFloat(0.0F))
            matrix.add(New COSFloat(0.0F))
            matrix.add(New COSFloat(0.0F))
            matrix.add(New COSFloat(1.0F))
            matrix.add(New COSFloat(0.0F))
            matrix.add(New COSFloat(0.0F))
            matrix.add(New COSFloat(0.0F))
            matrix.add(New COSFloat(1.0F))
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param array The array that describes the matrix.
        ' */
        Public Sub New(ByVal array As COSArray)
            If (array.size() = 6) Then
                numberOfRowElements = 2
            End If
            matrix = array
        End Sub

        '/**
        ' * This will get the underlying array value.
        ' *
        ' * @return The cos object that this object wraps.
        ' */
        Public Function getCOSArray() As COSArray
            Return matrix
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return matrix
        End Function


        '/**
        ' * This will get a matrix value at some point.
        ' *
        ' * @param row The row to get the value from.
        ' * @param column The column to get the value from.
        ' *
        ' * @return The value at the row/column position.
        ' */
        Public Function getValue(ByVal row As Integer, ByVal column As Integer) As Single
            Return DirectCast(matrix.get(row * numberOfRowElements + column), COSNumber).floatValue()
        End Function

        '/**
        ' * This will set a value at a position.
        ' *
        ' * @param row The row to set the value at.
        ' * @param column the column to set the value at.
        ' * @param value The value to set at the position.
        ' */
        Public Sub setValue(ByVal row As Integer, ByVal column As Integer, ByVal value As Single)
            matrix.set(row * numberOfRowElements + column, New COSFloat(value))
        End Sub

    End Class

End Namespace
