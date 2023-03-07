Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.graphics.color

    '/**
    ' * A tristimulus, or collection of three floating point parameters used for
    ' * color operations.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDTristimulus
        Implements COSObjectable

        Private values As COSArray = Nothing

        '/**
        ' * Constructor.  Defaults all values to 0, 0, 0.
        ' */
        Public Sub New()
            values = New COSArray()
            values.add(New COSFloat(0.0F))
            values.add(New COSFloat(0.0F))
            values.add(New COSFloat(0.0F))
        End Sub

        '/**
        ' * Constructor from COS object.
        ' *
        ' * @param array The array containing the XYZ values.
        ' */
        Public Sub New(ByVal array As COSArray)
            values = array
        End Sub

        '/**
        ' * Constructor from COS object.
        ' *
        ' * @param array The array containing the XYZ values.
        ' */
        Public Sub New(ByVal array As Single())
            values = New COSArray()
            For i As Integer = 0 To Math.Min(3, array.Length) - 1
                values.add(New COSFloat(array(i)))
            Next
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return values
        End Function

        '/**
        ' * This will get the x value of the tristimulus.
        ' *
        ' * @return The X value.
        ' */
        Public Function getX() As Single
            Return DirectCast(values.get(0), COSNumber).floatValue()
        End Function

        '/**
        ' * This will set the x value of the tristimulus.
        ' *
        ' * @param x The x value for the tristimulus.
        ' */
        Public Sub setX(ByVal x As Single)
            values.set(0, New COSFloat(x))
        End Sub

        '/**
        ' * This will get the y value of the tristimulus.
        ' *
        ' * @return The Y value.
        ' */
        Public Function getY() As Single
            Return DirectCast(values.get(1), COSNumber).floatValue()
        End Function

        '/**
        ' * This will set the y value of the tristimulus.
        ' *
        ' * @param y The y value for the tristimulus.
        ' */
        Public Sub setY(ByVal y As Single)
            values.set(1, New COSFloat(y))
        End Sub

        '/**
        ' * This will get the z value of the tristimulus.
        ' *
        ' * @return The Z value.
        ' */
        Public Function getZ() As Single
            Return DirectCast(values.get(2), COSNumber).floatValue()
        End Function

        '/**
        ' * This will set the z value of the tristimulus.
        ' *
        ' * @param z The z value for the tristimulus.
        ' */
        Public Sub setZ(ByVal z As Single)
            values.set(2, New COSFloat(z))
        End Sub

    End Class


End Namespace