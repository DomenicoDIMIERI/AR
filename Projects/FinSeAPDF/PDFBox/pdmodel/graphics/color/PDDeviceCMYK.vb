Imports FinSeA.Drawings
Imports FinSeA.Io
'imports  java.awt.Transparency;
Imports System.Drawing

Namespace org.apache.pdfbox.pdmodel.graphics.color


    '/**
    ' * This class represents a CMYK color space.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.6 $
    ' */
    Public Class PDDeviceCMYK
        Inherits PDColorSpace

        ''' <summary>
        ''' The single instance of this class.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly INSTANCE As PDDeviceCMYK = New PDDeviceCMYK()

        ''' <summary>
        ''' The name of this color space.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME As String = "DeviceCMYK"

        ''' <summary>
        ''' The abbreviated name of this color space.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ABBREVIATED_NAME As String = "CMYK"

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
            Return 4
        End Function

        '/**
        ' * Create a Java colorspace for this colorspace.
        ' *
        ' * @return A color space that can be used for Java AWT operations.
        ' */
        Protected Overrides Function createColorSpace() As ColorSpace
            Return New ColorSpaceCMYK()
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
            Dim nbBits() As Integer = {bpc, bpc, bpc, bpc}
            Dim componentColorModel As ComponentColorModel = New ComponentColorModel(getJavaColorSpace(), nbBits, False, False, Transparency.Mode.OPAQUE, DataBuffer.TYPE_BYTE)
            Return componentColorModel
        End Function


    End Class

End Namespace
