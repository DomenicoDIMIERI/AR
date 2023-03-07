Imports System.IO
'import java.util.Map;

Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.font


    '/**
    ' * This will create the correct type of font based on information in the dictionary.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.6 $
    ' */
    Public NotInheritable Class PDFontFactory

        '/**
        ' * private constructor, should only use static methods in this class.
        ' */
        Private Sub New()
        End Sub

        '/**
        ' * Logger instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDFontFactory.class);

        '/**
        ' * This will create the correct font based on information in the dictionary.
        ' *
        ' * @param dic The populated dictionary.
        ' *
        ' * @param fontCache A Map to cache already created fonts
        ' *
        ' * @return The corrent implementation for the font.
        ' *
        ' * @throws IOException If the dictionary is not valid.
        ' * 
        ' * @deprecated due to some side effects font caching is no longer supported, 
        ' * use {@link #createFont(COSDictionary)} instead
        ' */
        Public Shared Function createFont(ByVal dic As COSDictionary, ByVal fontCache As Map) As PDFont 'throws IOException
            Return createFont(dic)
        End Function

        '/**
        ' * This will create the correct font based on information in the dictionary.
        ' *
        ' * @param dic The populated dictionary.
        ' *
        ' * @return The corrent implementation for the font.
        ' *
        ' * @throws IOException If the dictionary is not valid.
        ' */
        Public Shared Function createFont(ByVal dic As COSDictionary) As PDFont  't hrows IOException
            Dim retval As PDFont = Nothing

            Dim type As COSName = dic.getDictionaryObject(COSName.TYPE)
            If (type IsNot Nothing AndAlso Not COSName.FONT.equals(type)) Then
                Throw New IOException("Cannot create font if /Type is not /Font.  Actual=" & type.toString)
            End If

            Dim subType As COSName = dic.getDictionaryObject(COSName.SUBTYPE)
            If (subType Is Nothing) Then
                Throw New IOException("Cannot create font as /SubType is not set.")
            End If
            If (subType.equals(COSName.TYPE1)) Then
                retval = New PDType1Font(dic)
            ElseIf (subType.equals(COSName.MM_TYPE1)) Then
                retval = New PDMMType1Font(dic)
            ElseIf (subType.equals(COSName.TRUE_TYPE)) Then
                retval = New PDTrueTypeFont(dic)
            ElseIf (subType.equals(COSName.TYPE3)) Then
                retval = New PDType3Font(dic)
            ElseIf (subType.equals(COSName.TYPE0)) Then
                retval = New PDType0Font(dic)
            ElseIf (subType.equals(COSName.CID_FONT_TYPE0)) Then
                retval = New PDCIDFontType0Font(dic)
            ElseIf (subType.equals(COSName.CID_FONT_TYPE2)) Then
                retval = New PDCIDFontType2Font(dic)
            Else
                LOG.warn("Substituting TrueType for unknown font subtype=" & subType.getName())
                retval = New PDTrueTypeFont(dic)
            End If
            Return retval
        End Function


    End Class

End Namespace
