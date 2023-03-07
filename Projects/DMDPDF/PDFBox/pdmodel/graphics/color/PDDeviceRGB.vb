Imports FinSeA.Drawings
Imports FinSeA.Io

Namespace org.apache.pdfbox.pdmodel.graphics.color


    '/**
    ' * This class represents an RGB color space.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.8 $
    ' */
    Public Class PDDeviceRGB
        Inherits PDColorSpace

        ''' <summary>
        ''' The name of this color space.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME As String = "DeviceRGB"

        ''' <summary>
        ''' The abbreviated name of this color space.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ABBREVIATED_NAME As String = "RGB"

        ''' <summary>
        ''' This is the single instance of this class.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly INSTANCE As PDDeviceRGB = New PDDeviceRGB()

        ''' <summary>
        ''' This class is immutable.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()
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
        ' * Create a Java colorspace for this colorspace.
        ' *
        ' * @return A color space that can be used for Java AWT operations.
        ' */
        Protected Overrides Function createColorSpace() As ColorSpace
            Return ColorSpace.getInstance(ColorSpace.CSEnum.CS_sRGB)
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
        Public Overrides Function createColorModel(ByVal bpc As Integer) As ColorModel  ' throws IOException
            Dim nbBits() As Integer = {bpc, bpc, bpc}
            Return New ComponentColorModel(getJavaColorSpace(), nbBits, False, False, Transparency.Mode.OPAQUE, DataBuffer.TYPE_BYTE)
        End Function


    End Class

End Namespace
