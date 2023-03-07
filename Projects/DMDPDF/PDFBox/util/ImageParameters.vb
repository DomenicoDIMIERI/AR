Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.color
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.util


    '/**
    ' * This contains all of the image parameters for in inlined image.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class ImageParameters

        Private dictionary As COSDictionary

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            Me.dictionary = New COSDictionary()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param params The image parameters.
        ' */
        Public Sub New(ByVal params As COSDictionary)
            If (params Is Nothing) Then Throw New ArgumentNullException("params")
            Me.dictionary = params
        End Sub

        '/**
        ' * This will get the dictionary that stores the image parameters.
        ' *
        ' * @return The COS dictionary that stores the image parameters.
        ' */
        Public Function getDictionary() As COSDictionary
            Return Me.dictionary
        End Function

        Private Function getCOSObject(ByVal abbreviatedName As COSName, ByVal name As COSName) As COSBase
            Dim retval As COSBase = dictionary.getDictionaryObject(abbreviatedName)
            If (retval Is Nothing) Then
                retval = dictionary.getDictionaryObject(name)
            End If
            Return retval
        End Function

        Private Function getNumberOrNegativeOne(ByVal abbreviatedName As COSName, ByVal name As COSName) As Integer
            Dim retval As Integer = -1
            Dim number As COSNumber = getCOSObject(abbreviatedName, name)
            If (number IsNot Nothing) Then
                retval = number.intValue()
            End If
            Return retval
        End Function

        '/**
        ' * The bits per component of this image.  This will return -1 if one has not
        ' * been set.
        ' *
        ' * @return The number of bits per component.
        ' */
        Public Function getBitsPerComponent() As Integer
            Return getNumberOrNegativeOne(COSName.BPC, COSName.BITS_PER_COMPONENT)
        End Function

        '/**
        ' * Set the number of bits per component.
        ' *
        ' * @param bpc The number of bits per component.
        ' */
        Public Sub setBitsPerComponent(ByVal bpc As Integer)
            dictionary.setInt(COSName.BPC, bpc)
        End Sub


        '/**
        ' * This will get the color space or null if none exists.
        ' *
        ' * @return The color space for this image.
        ' *
        ' * @throws IOException If there is an error getting the colorspace.
        ' */
        Public Function getColorSpace() As PDColorSpace 'throws IOException
            Return getColorSpace(Nothing)
        End Function

        '/**
        ' * This will get the color space or null if none exists.
        ' *
        ' * @param colorSpaces The ColorSpace dictionary from the current resources, if any.
        ' *
        ' * @return The color space for this image.
        ' *
        ' * @throws IOException If there is an error getting the colorspace.
        ' */
        Public Function getColorSpace(ByVal colorSpaces As Map) As PDColorSpace 'throws IOException
            Dim cs As COSBase = getCOSObject(COSName.CS, COSName.COLORSPACE)
            Dim retval As PDColorSpace = Nothing
            If (cs IsNot Nothing) Then
                retval = PDColorSpaceFactory.createColorSpace(cs, colorSpaces)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the color space for this image.
        ' *
        ' * @param cs The color space for this image.
        ' */
        Public Sub setColorSpace(ByVal cs As PDColorSpace)
            Dim base As COSBase = Nothing
            If (cs IsNot Nothing) Then
                base = cs.getCOSObject()
            End If
            Me.dictionary.setItem(COSName.CS, base)
        End Sub

        '/**
        ' * The height of this image.  This will return -1 if one has not
        ' * been set.
        ' *
        ' * @return The height.
        ' */
        Public Function getHeight() As Integer
            Return getNumberOrNegativeOne(COSName.H, COSName.HEIGHT)
        End Function

        '/**
        ' * Set the height of the image.
        ' *
        ' * @param h The height of the image.
        ' */
        Public Sub setHeight(ByVal h As Integer)
            Me.dictionary.setInt(COSName.H, h)
        End Sub

        '/**
        ' * The width of this image.  This will return -1 if one has not
        ' * been set.
        ' *
        ' * @return The width.
        ' */
        Public Function getWidth() As Integer
            Return getNumberOrNegativeOne(COSName.W, COSName.WIDTH)
        End Function

        '/**
        ' * Set the width of the image.
        ' *
        ' * @param w The width of the image.
        ' */
        Public Sub setWidth(ByVal w As Integer)
            Me.dictionary.setInt(COSName.W, w)
        End Sub

        '/**
        ' * This will get the list of filters that are associated with this stream.  Or
        ' * null if there are none.
        ' * @return A list of all encoding filters to apply to this stream.
        ' */
        Public Function getFilters() As List
            Dim retval As List = Nothing
            Dim filters As COSBase = dictionary.getDictionaryObject({"Filter", "F"})
            If (TypeOf (filters) Is COSName) Then
                Dim name As COSName = filters
                retval = New COSArrayList(name.getName(), name, dictionary, COSName.FILTER)
            ElseIf (TypeOf (filters) Is COSArray) Then
                retval = COSArrayList.convertCOSNameCOSArrayToList(filters)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the filters that are part of this stream.
        ' *
        ' * @param filters The filters that are part of this stream.
        ' */
        Public Sub setFilters(ByVal filters As List)
            Dim obj As COSBase = COSArrayList.convertStringListToCOSNameCOSArray(filters)
            dictionary.setItem("Filter", obj)
        End Sub

    End Class

End Namespace