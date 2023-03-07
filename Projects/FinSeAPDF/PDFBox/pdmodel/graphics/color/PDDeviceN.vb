Imports FinSeA.Drawings
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.common.function
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.graphics.color


    '/**
    ' * This class represents a DeviceN color space.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDDeviceN
        Inherits PDColorSpace

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDDeviceN.class);

        Private Const COLORANT_NAMES As Integer = 1
        Private Const ALTERNATE_CS As Integer = 2
        Private Const TINT_TRANSFORM As Integer = 3
        Private Const DEVICEN_ATTRIBUTES As Integer = 4

        Private tintTransform As PDFunction = Nothing
        Private alternateCS As PDColorSpace = Nothing
        Private deviceNAttributes As PDDeviceNAttributes = Nothing

        ''' <summary>
        ''' The name of this color space.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const NAME As String = "DeviceN"

        ' Private array As COSArray

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            array = New COSArray()
            array.add(COSName.DEVICEN)
            ' add some placeholder
            array.add(COSNull.NULL)
            array.add(COSNull.NULL)
            array.add(COSNull.NULL)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param csAttributes The array containing all colorspace information.
        ' */
        Public Sub New(ByVal csAttributes As COSArray)
            array = csAttributes
        End Sub

        '/**
        ' * This will return the name of the color space.  For a PDDeviceN object
        ' * this will always return "DeviceN"
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
            Return getColorantNames().size()
        End Function

        '/**
        ' * Create a Java colorspace for this colorspace.
        ' *
        ' * @return A color space that can be used for Java AWT operations.
        ' *
        ' * @throws IOException If there is an error creating the color space.
        ' */
        Protected Overrides Function createColorSpace() As ColorSpace ' throws IOException
            Try
                Return getAlternateColorSpace().getJavaColorSpace()
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
        Public Overrides Function createColorModel(ByVal bpc As Integer) As ColorModel '  throws IOException
            LOG.info("About to create ColorModel for " & getAlternateColorSpace().toString())
            Return getAlternateColorSpace().createColorModel(bpc)
        End Function

        '/**
        ' * This will get the colorant names.  A list of string objects.
        ' *
        ' * @return A list of colorants
        ' */
        Public Function getColorantNames() As List(Of String)
            Dim names As COSArray = array.getObject(COLORANT_NAMES)
            Return COSArrayList.convertCOSNameCOSArrayToList(names)
        End Function

        '/**
        ' * This will set the list of colorants.
        ' *
        ' * @param names The list of colorant names.
        ' */
        Public Sub setColorantNames(ByVal names As List(Of String))
            Dim namesArray As COSArray = COSArrayList.convertStringListToCOSNameCOSArray(names)
            array.set(COLORANT_NAMES, namesArray)
        End Sub

        '/**
        ' * This will get the alternate color space for this separation.
        ' *
        ' * @return The alternate color space.
        ' *
        ' * @throws IOException If there is an error getting the alternate color space.
        ' */
        Public Function getAlternateColorSpace() As PDColorSpace ' throws IOException
            If (alternateCS Is Nothing) Then
                Dim alternate As COSBase = array.getObject(ALTERNATE_CS)
                alternateCS = PDColorSpaceFactory.createColorSpace(alternate)
            End If
            Return alternateCS
        End Function

        '/**
        ' * This will set the alternate color space.
        ' *
        ' * @param cs The alternate color space.
        ' */
        Public Sub setAlternateColorSpace(ByVal cs As PDColorSpace)
            alternateCS = cs
            Dim space As COSBase = Nothing
            If (cs IsNot Nothing) Then
                space = cs.getCOSObject()
            End If
            array.set(ALTERNATE_CS, space)
        End Sub

        '/**
        ' * This will get the tint transform function.
        ' *
        ' * @return The tint transform function.
        ' *
        ' * @throws IOException if there is an error creating the function.
        ' */
        Public Function getTintTransform() As PDFunction 'throws IOException
            If (tintTransform Is Nothing) Then
                tintTransform = PDFunction.create(array.getObject(TINT_TRANSFORM))
            End If
            Return tintTransform
        End Function

        '/**
        ' * This will set the tint transform function.
        ' *
        ' * @param tint The tint transform function.
        ' */
        Public Sub setTintTransform(ByVal tint As PDFunction)
            tintTransform = tint
            array.set(TINT_TRANSFORM, tint)
        End Sub

        '/**
        ' * This will get the attributes that are associated with the deviceN
        ' * color space.
        ' *
        ' * @return The DeviceN attributes.
        ' */
        Public Function getAttributes() As PDDeviceNAttributes
            If (deviceNAttributes Is Nothing AndAlso array.size() > DEVICEN_ATTRIBUTES) Then ' the DeviceN contains an attributes dictionary
                deviceNAttributes = New PDDeviceNAttributes(array.getObject(DEVICEN_ATTRIBUTES))
            End If
            Return deviceNAttributes
        End Function

        '/**
        ' * This will set the color space attributes.  If null is passed in then
        ' * all attribute will be removed.
        ' *
        ' * @param attributes The color space attributes.
        ' */
        Public Sub setAttributes(ByVal attributes As PDDeviceNAttributes)
            deviceNAttributes = attributes
            If (attributes Is Nothing) Then
                array.remove(DEVICEN_ATTRIBUTES)
            Else
                'make sure array is large enough
                While (array.size() <= DEVICEN_ATTRIBUTES + 1)
                    array.add(COSNull.NULL)
                End While
                array.set(DEVICEN_ATTRIBUTES, attributes.getCOSDictionary())
            End If
        End Sub

        '/**
        ' * Returns the components of the color in the alternate colorspace for the given tint value.
        ' * @param tintValues a list containing the tint values
        ' * @return COSArray with the color components
        ' * @throws IOException If the tint function is not supported
        ' */
        Public Function calculateColorValues(ByVal tintValues As List(Of COSBase)) As COSArray 'throws IOException
            Dim tintTransform As PDFunction = getTintTransform()
            Dim tint As COSArray = New COSArray()
            tint.addAll(tintValues)
            Return tintTransform.eval(tint)
        End Function

    End Class

End Namespace
