Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports System.IO
Imports FinSeA.Drawings

Namespace org.apache.pdfbox.pdmodel.graphics.color


    '/**
    ' * This class represents a color space in a pdf document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.5 $
    ' */
    Public MustInherit Class PDColorSpace
        Implements COSObjectable

        ''' <summary>
        ''' array for the given parameters. 
        ''' </summary>
        ''' <remarks></remarks>
        Protected array As COSArray

        ''' <summary>
        ''' Cached Java AWT color space.
        ''' </summary>
        ''' <remarks>@see #getJavaColorSpace()</remarks>
        Private colorSpace As ColorSpace = Nothing

        '/**
        ' * This will return the name of the color space.
        ' *
        ' * @return The name of the color space.
        ' */
        Public MustOverride Function getName() As String

        '/**
        ' * This will get the number of components that this color space is made up of.
        ' *
        ' * @return The number of components in this color space.
        ' *
        ' * @throws IOException If there is an error getting the number of color components.
        ' */
        Public MustOverride Function getNumberOfComponents() As Integer ' throws IOException;

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Overridable Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return COSName.getPDFName(getName())
        End Function

        '/**
        ' * Returns the Java AWT color space for this instance.
        ' *
        ' * @return Java AWT color space
        ' * @throws IOException if the color space can not be created
        ' */
        Public Function getJavaColorSpace() As ColorSpace ' throws IOException {
            If (colorSpace Is Nothing) Then
                colorSpace = createColorSpace()
            End If
            Return colorSpace
        End Function

        '/**
        ' * Create a Java colorspace for this colorspace.
        ' *
        ' * @return A color space that can be used for Java AWT operations.
        ' *
        ' * @throws IOException If there is an error creating the color space.
        ' */
        Protected MustOverride Function createColorSpace() As ColorSpace ' throws IOException;

        '/**
        ' * Create a Java color model for this colorspace.
        ' *
        ' * @param bpc The number of bits per component.
        ' *
        ' * @return A color model that can be used for Java AWT operations.
        ' *
        ' * @throws IOException If there is an error creating the color model.
        ' */
        Public MustOverride Function createColorModel(ByVal bpc As Integer) As ColorModel ' throws IOException;

        '/*
        '        Don() 't just tell me its color type -- tell me its contents!
        '*/
        '/**
        ' * {@inheritDoc}
        ' */
        Public Overrides Function toString() As String
            Dim ret As String = Me.getName() & "{ "
            If (array IsNot Nothing) Then
                ret &= array.toString
            End If
            ret &= " }"
            Return ret
        End Function

    End Class

End Namespace
