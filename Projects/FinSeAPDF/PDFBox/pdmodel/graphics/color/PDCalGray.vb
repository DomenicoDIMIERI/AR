Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.Drawings
Imports System.Drawing
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.graphics.color


    '/**
    ' * This class represents a Cal Gray color space.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.5 $
    ' */
    Public Class PDCalGray
        Inherits PDColorSpace

        ''' <summary>
        ''' The name of this color space.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME As String = "CalGray"

        'Private array As COSArray
        Private dictionary As COSDictionary

        ''' <summary>
        ''' Constructor.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            array = New COSArray()
            dictionary = New COSDictionary()
            array.add(COSName.CALGRAY)
            array.add(dictionary)
        End Sub

        '/**
        ' * Constructor with array.
        ' *
        ' * @param gray The underlying color space.
        ' */
        Public Sub New(ByVal gray As COSArray)
            array = gray
            dictionary = array.getObject(1)
        End Sub

        '/**
        ' * This will get the number of components that this color space is made up of.
        ' *
        ' * @return The number of components in this color space.
        ' *
        ' * @throws IOException If there is an error getting the number of color components.
        ' */
        Public Overrides Function getNumberOfComponents() As Integer 'throws IOException
            Return 1
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
        ' *
        ' * @throws IOException If there is an error creating the color space.
        ' */
        Protected Overrides Function createColorSpace() As ColorSpace ' throws IOException
            Throw New NotImplementedException ' New IOException("Not implemented")
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
            Throw New NotImplementedException ' new IOException( "Not implemented" );
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
        ' * This will get the gamma value.  If none is present then the default of 1
        ' * will be returned.
        ' *
        ' * @return The gamma value.
        ' */
        Public Function getGamma() As Single
            Dim retval As Single = 1.0F
            Dim gamma As COSNumber = dictionary.getDictionaryObject(COSName.GAMMA)
            If (gamma IsNot Nothing) Then
                retval = gamma.floatValue()
            End If
            Return retval
        End Function

        '/**
        ' * Set the gamma value.
        ' *
        ' * @param value The new gamma value.
        ' */
        Public Sub setGamma(ByVal value As Single)
            dictionary.setItem(COSName.GAMMA, New COSFloat(value))
        End Sub

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

    End Class

End Namespace
