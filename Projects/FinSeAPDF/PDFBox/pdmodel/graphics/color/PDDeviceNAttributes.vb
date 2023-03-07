Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.Io

Namespace org.apache.pdfbox.pdmodel.graphics.color


    '/**
    ' * This class represents attributes for a DeviceN color space.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDDeviceNAttributes

        Private dictionary As COSDictionary

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            dictionary = New COSDictionary()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param attributes A dictionary that has all of the attributes.
        ' */
        Public Sub New(ByVal attributes As COSDictionary)
            dictionary = attributes
        End Sub

        '/**
        ' * This will get the underlying cos dictionary.
        ' *
        ' * @return The dictionary that this object wraps.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return dictionary
        End Function

        '/**
        ' * This will get a map of colorants.  See the PDF Reference for more details about
        ' * this attribute.  The map will contain a java.lang.String as the key, a colorant name,
        ' * and a PDColorSpace as the value.
        ' *
        ' * @return The colorant map.
        ' *
        ' * @throws IOException If there is an error getting the colorspaces.
        ' */
        Public Function getColorants() As Map ' throws IOException
            Dim actuals As Map = New HashMap()
            Dim colorants As COSDictionary = dictionary.getDictionaryObject(COSName.COLORANTS)
            If (colorants Is Nothing) Then
                colorants = New COSDictionary()
                dictionary.setItem(COSName.COLORANTS, colorants)
            End If
            For Each name As COSName In colorants.keySet()
                Dim value As COSBase = colorants.getDictionaryObject(name)
                actuals.put(name.getName(), PDColorSpaceFactory.createColorSpace(value))
            Next
            Return New COSDictionaryMap(actuals, colorants)
        End Function

        '/**
        ' * This will replace the existing colorant attribute.  The key should be strings
        ' * and the values should be PDColorSpaces.
        ' *
        ' * @param colorants The map of colorants.
        ' */
        Public Sub setColorants(ByVal colorants As Map)
            Dim colorantDict As COSDictionary = Nothing
            If (colorants IsNot Nothing) Then
                colorantDict = COSDictionaryMap.convert(colorants)
            End If
            dictionary.setItem(COSName.COLORANTS, colorantDict)
        End Sub


    End Class

End Namespace
