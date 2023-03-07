Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    ' * This class represents a PDF /AP entry the appearance dictionary.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDAppearanceDictionary
        Implements COSObjectable

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDAppearanceDictionary.class);

        Private dictionary As COSDictionary

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            dictionary = New COSDictionary()
            'the N entry is required.
            dictionary.setItem(COSName.N, New COSDictionary())
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param dict The annotations dictionary.
        ' */
        Public Sub New(ByVal dict As COSDictionary)
            dictionary = dict
        End Sub

        '/**
        ' * returns the dictionary.
        ' * @return the dictionary
        ' */
        Public Function getDictionary() As COSDictionary
            Return dictionary
        End Function

        '/**
        ' * returns the dictionary.
        ' * @return the dictionary
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return dictionary
        End Function

        '/**
        ' * This will return a list of appearances.  In the case where there is
        ' * only one appearance the map will contain one entry whose key is the string
        ' * "default".
        ' *
        ' * @return A list of key(java.lang.String) value(PDAppearanceStream) pairs
        ' */
        Public Function getNormalAppearance() As Map(Of String, PDAppearanceStream)
            Dim ap As COSBase = dictionary.getDictionaryObject(COSName.N)
            If (ap Is Nothing) Then
                Return Nothing
            ElseIf (TypeOf (ap) Is COSStream) Then
                Dim aux As COSStream = ap
                ap = New COSDictionary()
                DirectCast(ap, COSDictionary).setItem(COSName.DEFAULT, aux)
            End If
            Dim map As COSDictionary = ap
            Dim actuals As Map(Of String, PDAppearanceStream) = New HashMap(Of String, PDAppearanceStream)()
            Dim retval As Map(Of String, PDAppearanceStream) = New COSDictionaryMap(Of String, PDAppearanceStream)(actuals, map)
            For Each asName As COSName In map.keySet()
                Dim stream As COSBase = map.getDictionaryObject(asName)
                ' PDFBOX-1599: this is just a workaround. The given PDF provides "null" as stream 
                ' which leads to a COSName("null") value and finally to a ClassCastExcpetion
                If (TypeOf (stream) Is COSStream) Then
                    Dim [as] As COSStream = stream
                    actuals.put(asName.getName(), New PDAppearanceStream([as]))
                Else
                    LOG.debug("non-conformance workaround: ignore null value for appearance stream.")
                End If
            Next
            Return retval
        End Function

        '/**
        ' * This will set a list of appearances.  If you would like to set the single
        ' * appearance then you should use the key "default", and when the PDF is written
        ' * back to the filesystem then there will only be one stream.
        ' *
        ' * @param appearanceMap The updated map with the appearance.
        ' */
        Public Sub setNormalAppearance(ByVal appearanceMap As Map(Of String, PDAppearanceStream))
            dictionary.setItem(COSName.N, COSDictionaryMap.convert(appearanceMap))
        End Sub

        '/**
        ' * This will set the normal appearance when there is only one appearance
        ' * to be shown.
        ' *
        ' * @param ap The appearance stream to show.
        ' */
        Public Sub setNormalAppearance(ByVal ap As PDAppearanceStream)
            dictionary.setItem(COSName.N, ap.getStream())
        End Sub

        '/**
        ' * This will return a list of appearances.  In the case where there is
        ' * only one appearance the map will contain one entry whose key is the string
        ' * "default".  If there is no rollover appearance then the normal appearance
        ' * will be returned.  Which means that this method will never return null.
        ' *
        ' * @return A list of key(java.lang.String) value(PDAppearanceStream) pairs
        ' */
        Public Function getRolloverAppearance() As Map(Of String, PDAppearanceStream)
            Dim retval As Map(Of String, PDAppearanceStream) = Nothing
            Dim ap As COSBase = dictionary.getDictionaryObject(COSName.R)
            If (ap Is Nothing) Then
                retval = getNormalAppearance()
            Else
                If (TypeOf (ap) Is COSStream) Then
                    Dim aux As COSStream = ap
                    ap = New COSDictionary()
                    DirectCast(ap, COSDictionary).setItem(COSName.DEFAULT, aux)
                End If
                Dim map As COSDictionary = ap
                Dim actuals As Map(Of String, PDAppearanceStream) = New HashMap(Of String, PDAppearanceStream)()
                retval = New COSDictionaryMap(Of String, PDAppearanceStream)(actuals, map)
                For Each asName As COSName In map.keySet()
                    Dim stream As COSBase = map.getDictionaryObject(asName)
                    ' PDFBOX-1599: this is just a workaround. The given PDF provides "null" as stream 
                    ' which leads to a COSName("null") value and finally to a ClassCastExcpetion
                    If (TypeOf (stream) Is COSStream) Then
                        Dim [as] As COSStream = stream
                        actuals.put(asName.getName(), New PDAppearanceStream([as]))
                    Else
                        LOG.debug("non-conformance workaround: ignore null value for appearance stream.")
                    End If
                Next
            End If
            Return retval
        End Function

        '/**
        ' * This will set a list of appearances.  If you would like to set the single
        ' * appearance then you should use the key "default", and when the PDF is written
        ' * back to the filesystem then there will only be one stream.
        ' *
        ' * @param appearanceMap The updated map with the appearance.
        ' */
        Public Sub setRolloverAppearance(ByVal appearanceMap As Map(Of String, PDAppearanceStream))
            dictionary.setItem(COSName.R, COSDictionaryMap.convert(appearanceMap))
        End Sub

        '/**
        ' * This will set the rollover appearance when there is rollover appearance
        ' * to be shown.
        ' *
        ' * @param ap The appearance stream to show.
        ' */
        Public Sub setRolloverAppearance(ByVal ap As PDAppearanceStream)
            dictionary.setItem(COSName.R, ap.getStream())
        End Sub

        '/**
        ' * This will return a list of appearances.  In the case where there is
        ' * only one appearance the map will contain one entry whose key is the string
        ' * "default".  If there is no rollover appearance then the normal appearance
        ' * will be returned.  Which means that this method will never return null.
        ' *
        ' * @return A list of key(java.lang.String) value(PDAppearanceStream) pairs
        ' */
        Public Function getDownAppearance() As Map(Of String, PDAppearanceStream)
            Dim retval As Map(Of String, PDAppearanceStream) = Nothing
            Dim ap As COSBase = dictionary.getDictionaryObject(COSName.D)
            If (ap Is Nothing) Then
                retval = getNormalAppearance()
            Else
                If (TypeOf (ap) Is COSStream) Then
                    Dim aux As COSStream = ap
                    ap = New COSDictionary()
                    DirectCast(ap, COSDictionary).setItem(COSName.DEFAULT, aux)
                End If
                Dim map As COSDictionary = ap
                Dim actuals As Map(Of String, PDAppearanceStream) = New HashMap(Of String, PDAppearanceStream)()
                retval = New COSDictionaryMap(Of String, PDAppearanceStream)(actuals, map)
                For Each asName As COSName In map.keySet()
                    Dim stream As COSBase = map.getDictionaryObject(asName)
                    '// PDFBOX-1599: this is just a workaround. The given PDF provides "null" as stream 
                    '/ which leads to a COSName("null") value and finally to a ClassCastExcpetion
                    If (TypeOf (stream) Is COSStream) Then
                        Dim [as] As COSStream = stream
                        actuals.put(asName.getName(), New PDAppearanceStream([as]))
                    Else
                        LOG.debug("non-conformance workaround: ignore null value for appearance stream.")
                    End If
                Next
            End If
            Return retval
        End Function

        '/**
        ' * This will set a list of appearances.  If you would like to set the single
        ' * appearance then you should use the key "default", and when the PDF is written
        ' * back to the filesystem then there will only be one stream.
        ' *
        ' * @param appearanceMap The updated map with the appearance.
        ' */
        Public Sub setDownAppearance(ByVal appearanceMap As Map(Of String, PDAppearanceStream))
            dictionary.setItem(COSName.D, COSDictionaryMap.convert(appearanceMap))
        End Sub

        '/**
        ' * This will set the down appearance when there is down appearance
        ' * to be shown.
        ' *
        ' * @param ap The appearance stream to show.
        ' */
        Public Sub setDownAppearance(ByVal ap As PDAppearanceStream)
            dictionary.setItem(COSName.D, ap.getStream())
        End Sub

    End Class

End Namespace
