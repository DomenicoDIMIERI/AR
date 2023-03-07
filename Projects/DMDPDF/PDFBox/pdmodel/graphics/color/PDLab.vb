Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.Drawings
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.graphics.color


    '/**
    ' * This class represents a Lab color space.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDLab
        Inherits PDColorSpace

        ''' <summary>
        ''' The name of this color space.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME As String = "Lab"

        'private COSArray array;
        Private dictionary As COSDictionary

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            array = New COSArray()
            dictionary = New COSDictionary()
            array.add(COSName.LAB)
            array.add(dictionary)
        End Sub

        '/**
        ' * Constructor with array.
        ' *
        ' * @param lab The underlying color space.
        ' */
        Public Sub New(ByVal lab As COSArray)
            array = lab
            dictionary = array.getObject(1)
        End Sub

        '/**
        ' * This will return the name of the color space.
        ' *
        ' * @return The name of the color space.
        ' */
        Public Overrides Function getName() As String
            Return NAME
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Overrides Function getCOSObject() As COSBase
            Return array
        End Function

        '/**
        ' * Create a Java colorspace for this colorspace.
        ' *
        ' * @return A color space that can be used for Java AWT operations.
        ' *
        ' * @throws IOException If there is an error creating the color space.
        ' */
        Protected Overrides Function createColorSpace() As ColorSpace ' throws IOException
            Return New ColorSpaceLab(getWhitepoint(), getBlackPoint(), getARange(), getBRange())
        End Function

        '/**
        ' * Create a Java color model for this colorspace.
        ' *
        ' * @param bpc The number of bits per component.
        ' *
        ' * @return A color model that can be used for Java AWT operations.
        ' *
        ' * @throws IOException If there is an error creating the color model.
        ' */
        Public Overrides Function createColorModel(ByVal bpc As Integer) As ColorModel '  throws IOException
            Dim nBits() As Integer = {bpc, bpc, bpc}
            Return New ComponentColorModel(getJavaColorSpace(), nBits, False, False, Transparency.Mode.OPAQUE, DataBuffer.TYPE_BYTE)
        End Function

        '/**
        ' * This will get the number of components that this color space is made up of.
        ' *
        ' * @return The number of components in this color space.
        ' *
        ' * @throws IOException If there is an error getting the number of color components.
        ' */
        Public Overrides Function getNumberOfComponents() As Integer ' throws IOException
            Return 3
        End Function

        '/**
        ' * This will return the whitepoint tristimulus.  As this is a required field
        ' * this will never return null.  A default of 1,1,1 will be returned if the
        ' * pdf does not have any values yet.
        ' *
        ' * @return The whitepoint tristimulus.
        ' */
        Public Function getWhitepoint() As PDTristimulus
            Dim wp As COSArray = dictionary.getDictionaryObject(COSName.WHITE_POINT)
            If (wp Is Nothing) Then
                wp = New COSArray()
                wp.add(New COSFloat(1.0F))
                wp.add(New COSFloat(1.0F))
                wp.add(New COSFloat(1.0F))
                dictionary.setItem(COSName.WHITE_POINT, wp)
            End If
            Return New PDTristimulus(wp)
        End Function

        '/**
        ' * This will set the whitepoint tristimulus.  As this is a required field
        ' * this null should not be passed into this function.
        ' *
        ' * @param wp The whitepoint tristimulus.
        ' */
        Public Sub setWhitepoint(ByVal wp As PDTristimulus)
            Dim wpArray As COSBase = wp.getCOSObject()
            If (wpArray IsNot Nothing) Then
                dictionary.setItem(COSName.WHITE_POINT, wpArray)
            End If
        End Sub

        '/**
        ' * This will return the BlackPoint tristimulus.  This is an optional field but
        ' * has defaults so this will never return null.
        ' * A default of 0,0,0 will be returned if the pdf does not have any values yet.
        ' *
        ' * @return The blackpoint tristimulus.
        ' */
        Public Function getBlackPoint() As PDTristimulus
            Dim bp As COSArray = dictionary.getDictionaryObject(COSName.BLACK_POINT)
            If (bp Is Nothing) Then
                bp = New COSArray()
                bp.add(New COSFloat(0.0F))
                bp.add(New COSFloat(0.0F))
                bp.add(New COSFloat(0.0F))
                dictionary.setItem(COSName.BLACK_POINT, bp)
            End If
            Return New PDTristimulus(bp)
        End Function

        '/**
        ' * This will set the BlackPoint tristimulus.  As this is a required field
        ' * this null should not be passed into this function.
        ' *
        ' * @param bp The BlackPoint tristimulus.
        ' */
        Public Sub setBlackPoint(ByVal bp As PDTristimulus)
            Dim bpArray As COSBase = Nothing
            If (bp IsNot Nothing) Then
                bpArray = bp.getCOSObject()
            End If
            dictionary.setItem(COSName.BLACK_POINT, bpArray)
        End Sub

        Private Function getRangeArray() As COSArray
            Dim range As COSArray = dictionary.getDictionaryObject(COSName.RANGE)
            If (range Is Nothing) Then
                range = New COSArray()
                dictionary.setItem(COSName.RANGE, array)
                range.add(New COSFloat(-100))
                range.add(New COSFloat(100))
                range.add(New COSFloat(-100))
                range.add(New COSFloat(100))
            End If
            Return range
        End Function

        '/**
        ' * This will get the valid range for the a component.  If none is found
        ' * then the default will be returned, which is -100 to 100.
        ' *
        ' * @return The a range.
        ' */
        Public Function getARange() As PDRange
            Dim range As COSArray = getRangeArray()
            Return New PDRange(range, 0)
        End Function

        '/**
        ' * This will set the a range for this color space.
        ' *
        ' * @param range The new range for the a component.
        ' */
        Public Sub setARange(ByVal range As PDRange)
            Dim rangeArray As COSArray = Nothing
            'if null then reset to defaults
            If (range Is Nothing) Then
                rangeArray = getRangeArray()
                rangeArray.set(0, New COSFloat(-100))
                rangeArray.set(1, New COSFloat(100))
            Else
                rangeArray = range.getCOSArray()
            End If
            dictionary.setItem(COSName.RANGE, rangeArray)
        End Sub

        '/**
        ' * This will get the valid range for the b component.  If none is found
        ' * then the default will be returned, which is -100 to 100.
        ' *
        ' * @return The b range.
        ' */
        Public Function getBRange() As PDRange
            Dim range As COSArray = getRangeArray()
            Return New PDRange(range, 1)
        End Function

        '/**
        ' * This will set the b range for this color space.
        ' *
        ' * @param range The new range for the b component.
        ' */
        Public Sub setBRange(ByVal range As PDRange)
            Dim rangeArray As COSArray = Nothing
            'if null then reset to defaults
            If (range Is Nothing) Then
                rangeArray = getRangeArray()
                rangeArray.set(2, New COSFloat(-100))
                rangeArray.set(3, New COSFloat(100))
            Else
                rangeArray = range.getCOSArray()
            End If
            dictionary.setItem(COSName.RANGE, rangeArray)
        End Sub

    End Class

End Namespace
