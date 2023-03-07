Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.Drawings
Imports FinSeA.Io
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.graphics.color

    '/**
    ' * This class represents a Pattern color space.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDPattern
        Inherits PDColorSpace

        'Private array As COSArray

        '/**
        ' * The name of this color space.
        ' */
        Public Const NAME As String = "Pattern"

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            array = New COSArray()
            array.add(COSName.PATTERN)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param pattern The pattern array.
        ' */
        Public Sub New(ByVal pattern As COSArray)
            array = pattern
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
            Return -1
        End Function

        '/**
        ' * Create a Java colorspace for this colorspace.
        ' *
        ' * @return A color space that can be used for Java AWT operations.
        ' *
        ' * @throws IOException If there is an error creating the color space.
        ' */
        Protected Overrides Function createColorSpace() As ColorSpace ' throws IOException
            Throw New NotImplementedException '("Not implemented")
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
            Throw New NotImplementedException '( "Not implemented" );
        End Function

    End Class

End Namespace