Imports FinSeA.Drawings
Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common.function

Namespace org.apache.pdfbox.pdmodel.graphics.color

    '/**
    ' * This class represents a Separation color space.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.5 $
    ' */
    Public Class PDSeparation
        Inherits PDColorSpace
        '/**
        ' * Log instance.
        ' */
        'private static final Log log = LogFactory.getLog(PDSeparation.class);

        ''' <summary>
        ''' The name of this color space.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME As String = "Separation"


        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            array = New COSArray()
            array.add(COSName.SEPARATION)
            array.add(COSName.getPDFName(""))
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param separation The array containing all separation information.
        ' */
        Public Sub New(ByVal separation As COSArray)
            array = separation
        End Sub

        '/**
        ' * This will return the name of the color space.  For a PDSeparation object
        ' * this will always return "Separation"
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
            Return getAlternateColorSpace().getNumberOfComponents()
        End Function

        '/**
        ' * Create a Java colorspace for this colorspace.
        ' *
        ' * @return A color space that can be used for Java AWT operations.
        ' *
        ' * @throws IOException If there is an error creating the color space.
        ' */
        Protected Overrides Function createColorSpace() As ColorSpace 'throws IOException
            Try
                Dim alt As PDColorSpace = getAlternateColorSpace()
                Return alt.getJavaColorSpace()
            Catch ioexception As IOException
                LOG.error(ioexception.Message, ioexception)
                Throw ioexception
            Catch exception As Exception
                LOG.error(exception.Message, exception)
                Throw New IOException("Failed to Create ColorSpace")
            End Try
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
            LOG.info("About to create ColorModel for " & getAlternateColorSpace().toString())
            Return getAlternateColorSpace().createColorModel(bpc)
        End Function

        '/**
        ' * This will get the separation name.
        ' *
        ' * @return The name in the separation.
        ' */
        Public Function getColorantName() As String
            Dim name As COSName = array.getObject(1)
            Return name.getName()
        End Function

        '/**
        ' * This will set the separation name.
        ' *
        ' * @param name The separation name.
        ' */
        Public Sub setColorantName(ByVal name As String)
            array.set(1, COSName.getPDFName(name))
        End Sub

        '/**
        ' * This will get the alternate color space for this separation.
        ' *
        ' * @return The alternate color space.
        ' *
        ' * @throws IOException If there is an error getting the alternate color space.
        ' */
        Public Function getAlternateColorSpace() As PDColorSpace ' throws IOException
            Dim alternate As COSBase = array.getObject(2)
            Dim cs As PDColorSpace = PDColorSpaceFactory.createColorSpace(alternate)
            Return cs
        End Function

        '/**
        ' * This will set the alternate color space.
        ' *
        ' * @param cs The alternate color space.
        ' */
        Public Sub setAlternateColorSpace(ByVal cs As PDColorSpace)
            Dim space As COSBase = Nothing
            If (cs IsNot Nothing) Then
                space = cs.getCOSObject()
            End If
            array.set(2, space)
        End Sub

        '/**
        ' * This will get the tint transform function.
        ' *
        ' * @return The tint transform function.
        ' *
        ' * @throws IOException If there is an error creating the PDFunction
        ' */
        Public Function getTintTransform() As PDFunction ' throws IOException
            Return PDFunction.create(array.getObject(3))
        End Function

        '/**
        ' * This will set the tint transform function.
        ' *
        ' * @param tint The tint transform function.
        ' */
        Public Sub setTintTransform(ByVal tint As PDFunction)
            array.set(3, tint)
        End Sub

        '/**
        ' * Returns the components of the color in the alternate colorspace for the given tint value.
        ' * @param tintValue the tint value
        ' * @return COSArray with the color components
        ' * @throws IOException If the tint function is not supported
        ' */
        Public Function calculateColorValues(ByVal tintValue As COSBase) As COSArray 'throws IOException
            Dim tintTransform As PDFunction = getTintTransform()
            Dim tint As COSArray = New COSArray()
            tint.add(tintValue)
            Return tintTransform.eval(tint)
        End Function

    End Class

End Namespace
