Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.Drawings
Imports System.Drawing
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.graphics.color


    '/**
    ' * This class represents a Cal RGB color space.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PDCalRGB
        Inherits PDColorSpace

        ''' <summary>
        ''' The name of this color space.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME As String = "CalRGB"

        'Private array As COSArray
        Private dictionary As COSDictionary

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            array = New COSArray()
            dictionary = New COSDictionary()
            array.add(COSName.CALRGB)
            array.add(dictionary)
        End Sub

        '/**
        ' * Constructor with array.
        ' *
        ' * @param rgb The underlying color space.
        ' */
        Public Sub New(ByVal rgb As COSArray)
            array = rgb
            dictionary = array.getObject(1)
        End Sub

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
        ' * This will return the name of the color space.
        ' *
        ' * @return The name of the color space.
        ' */
        Public Overrides Function getName() As String
            Return NAME
        End Function

        '/**
        ' * Create a Java colorspace for this colorspace.
        ' *
        ' * @return A color space that can be used for Java AWT operations.
        ' */
        Protected Overrides Function createColorSpace() As ColorSpace
            Return New ColorSpaceCalRGB(getGamma(), getWhitepoint(), getBlackPoint(), getLinearInterpretation())
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
        Public Overrides Function createColorModel(ByVal bpc As Integer) As ColorModel  'throws IOException
            Dim nBits() As Integer = {bpc, bpc, bpc}
            Return New ComponentColorModel(getJavaColorSpace(), nBits, False, False, Transparency.Mode.OPAQUE, DataBuffer.TYPE_BYTE)
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

        '/**
        ' * This will get the gamma value.  If none is present then the default of 1,1,1
        ' * will be returned.
        ' *
        ' * @return The gamma value.
        ' */
        Public Function getGamma() As PDGamma
            Dim gamma As COSArray = dictionary.getDictionaryObject(COSName.GAMMA)
            If (gamma Is Nothing) Then
                gamma = New COSArray()
                gamma.add(New COSFloat(1.0F))
                gamma.add(New COSFloat(1.0F))
                gamma.add(New COSFloat(1.0F))
                dictionary.setItem(COSName.GAMMA, gamma)
            End If
            Return New PDGamma(gamma)
        End Function

        '/**
        ' * Set the gamma value.
        ' *
        ' * @param value The new gamma value.
        ' */
        Public Sub setGamma(ByVal value As PDGamma)
            Dim gamma As COSArray = Nothing
            If (value IsNot Nothing) Then
                gamma = value.getCOSArray()
            End If
            dictionary.setItem(COSName.GAMMA, gamma)
        End Sub

        '/**
        ' * This will get the linear interpretation array.  This is guaranteed to not
        ' * return null.  If the underlying dictionary contains null then the identity
        ' * matrix will be returned.
        ' *
        ' * @return The linear interpretation matrix.
        ' */
        Public Function getLinearInterpretation() As PDMatrix
            Dim retval As PDMatrix = Nothing
            Dim matrix As COSArray = dictionary.getDictionaryObject(COSName.MATRIX)
            If (matrix Is Nothing) Then
                retval = New PDMatrix()
                setLinearInterpretation(retval)
            Else
                retval = New PDMatrix(matrix)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the linear interpretation matrix.  Passing in null will
        ' * clear the matrix.
        ' *
        ' * @param matrix The new linear interpretation matrix.
        ' */
        Public Sub setLinearInterpretation(ByVal matrix As PDMatrix)
            Dim matrixArray As COSArray = Nothing
            If (matrix IsNot Nothing) Then
                matrixArray = matrix.getCOSArray()
            End If
            dictionary.setItem(COSName.MATRIX, matrixArray)
        End Sub

    End Class

End Namespace
