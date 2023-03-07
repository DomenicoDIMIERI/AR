Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.graphics.color

    '/**
    ' * A gamma array, or collection of three floating point parameters used for
    ' * color operations.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDGamma
        Implements COSObjectable

        Private values As COSArray = Nothing

        ''' <summary>
        ''' Constructor.  Defaults all values to 0, 0, 0.
        ''' </summary>
        ''' <remarks></remarks>
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
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return values
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSArray() As COSArray
            Return values
        End Function

        '/**
        ' * This will get the r value of the tristimulus.
        ' *
        ' * @return The R value.
        ' */
        Public Function getR() As Single
            Return DirectCast(values.get(0), COSNumber).floatValue()
        End Function

        '/**
        ' * This will set the r value of the tristimulus.
        ' *
        ' * @param r The r value for the tristimulus.
        ' */
        Public Sub setR(ByVal r As Single)
            values.set(0, New COSFloat(r))
        End Sub

        '/**
        ' * This will get the g value of the tristimulus.
        ' *
        ' * @return The g value.
        ' */
        Public Function getG() As Single
            Return DirectCast(values.get(1), COSNumber).floatValue()
        End Function

        '/**
        ' * This will set the g value of the tristimulus.
        ' *
        ' * @param g The g value for the tristimulus.
        ' */
        Public Sub setG(ByVal g As Single)
            values.set(1, New COSFloat(g))
        End Sub

        '/**
        ' * This will get the b value of the tristimulus.
        ' *
        ' * @return The B value.
        ' */
        Public Function getB() As Single
            Return DirectCast(values.get(2), COSNumber).floatValue()
        End Function

        '/**
        ' * This will set the b value of the tristimulus.
        ' *
        ' * @param b The b value for the tristimulus.
        ' */
        Public Sub setB(ByVal b As Single)
            values.set(2, New COSFloat(b))
        End Sub


    End Class

End Namespace