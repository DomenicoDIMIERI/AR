Imports FinSeA.Drawings

'import java.awt.image.ColorModel;
'import java.awt.image.ComponentColorModel;
'import java.awt.image.DataBuffer;
'import java.awt.Transparency;

'import java.io.IOException;

Namespace org.apache.pdfbox.pdmodel.graphics.color


    '/**
    ' * This class represents a Gray color space.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.6 $
    ' */
    Public Class PDDeviceGray
        Inherits PDColorSpace

        ''' <summary>
        ''' The name of this color space.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME As String = "DeviceGray"

        ''' <summary>
        ''' The abbreviated name of this color space.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ABBREVIATED_NAME As String = "G"

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
            Return 1
        End Function

        '/**
        ' * Create a Java colorspace for this colorspace.
        ' *
        ' * @return A color space that can be used for Java AWT operations.
        ' */
        Protected Overrides Function createColorSpace() As ColorSpace
            Return ColorSpace.getInstance(ColorSpace.CS_GRAY)
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
        Public Overrides Function createColorModel(ByVal bpc As Integer) As ColorModel ' throws IOException
            Dim cs As ColorSpace = ColorSpace.getInstance(ColorSpace.CS_GRAY)
            Dim nBits() As Integer = {bpc}
            Dim colorModel As ColorModel = New ComponentColorModel(cs, nBits, False, False, Transparency.Mode.OPAQUE, DataBuffer.TYPE_BYTE)
            Return colorModel
        End Function

    End Class

End Namespace
