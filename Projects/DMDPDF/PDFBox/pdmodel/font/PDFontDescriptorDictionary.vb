Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.font

    '/**
    ' * This class represents an implementation to the font descriptor that gets its
    ' * information from a COS Dictionary.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDFontDescriptorDictionary
        Inherits PDFontDescriptor
        Implements COSObjectable

        Private dic As COSDictionary
        Private xHeight As Single = Single.NegativeInfinity ' NFloat.NEGATIVE_INFINITY
        Private capHeight As Single = Single.NegativeInfinity ' NFloat.NEGATIVE_INFINITY;
        Private flags As Integer = -1

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            dic = New COSDictionary()
            dic.setItem(COSName.TYPE, COSName.FONT_DESC)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param desc The wrapped COS Dictionary.
        ' */
        Public Sub New(ByVal desc As COSDictionary)
            dic = desc
        End Sub

        '/**
        ' * This will get the dictionary for this object.
        ' *
        ' * @return The COS dictionary.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return dic
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return dic
        End Function

        '/**
        ' * Get the font name.
        ' *
        ' * @return The name of the font.
        ' */
        Public Overrides Function getFontName() As String
            Dim retval As String = ""
            Dim name As COSName = dic.getDictionaryObject(COSName.FONT_NAME)
            If (name IsNot Nothing) Then
                retval = name.getName()
            End If
            Return retval
        End Function

        '/**
        ' * This will set the font name.
        ' *
        ' * @param fontName The new name for the font.
        ' */
        Public Overrides Sub setFontName(ByVal fontName As String)
            Dim name As COSName = Nothing
            If (fontName IsNot Nothing) Then
                name = COSName.getPDFName(fontName)
            End If
            dic.setItem(COSName.FONT_NAME, name)
        End Sub

        '/**
        ' * A string representing the preferred font family.
        ' *
        ' * @return The font family.
        ' */
        Public Overrides Function getFontFamily() As String
            Dim retval As String = ""
            Dim name As COSString = dic.getDictionaryObject(COSName.FONT_FAMILY)
            If (name IsNot Nothing) Then
                retval = name.getString()
            End If
            Return retval
        End Function

        '/**
        ' * This will set the font family.
        ' *
        ' * @param fontFamily The font family.
        ' */
        Public Overrides Sub setFontFamily(ByVal fontFamily As String)
            Dim name As COSString = Nothing
            If (fontFamily <> "") Then
                name = New COSString(fontFamily)
            End If
            dic.setItem(COSName.FONT_FAMILY, name)
        End Sub

        '/**
        ' * The weight of the font.  According to the PDF spec "possible values are
        ' * 100, 200, 300, 400, 500, 600, 700, 800 or 900"  Where a higher number is
        ' * more weight and appears to be more bold.
        ' *
        ' * @return The font weight.
        ' */
        Public Overrides Function getFontWeight() As Single
            Return dic.getFloat(COSName.FONT_WEIGHT, 0)
        End Function

        '/**
        ' * Set the weight of the font.
        ' *
        ' * @param fontWeight The new weight of the font.
        ' */
        Public Overrides Sub setFontWeight(ByVal fontWeight As Single)
            dic.setFloat(COSName.FONT_WEIGHT, fontWeight)
        End Sub

        '/**
        ' * A string representing the preferred font stretch.
        ' * According to the PDF Spec:
        ' * The font stretch value; it must be one of the following (ordered from
        ' * narrowest to widest): UltraCondensed, ExtraCondensed, Condensed, SemiCondensed,
        ' * Normal, SemiExpanded, Expanded, ExtraExpanded or UltraExpanded.
        ' *
        ' * @return The stretch of the font.
        ' */
        Public Overrides Function getFontStretch() As String
            Dim retval As String = ""
            Dim name As COSName = dic.getDictionaryObject(COSName.FONT_STRETCH)
            If (name IsNot Nothing) Then
                retval = name.getName()
            End If
            Return retval
        End Function

        '/**
        ' * This will set the font stretch.
        ' *
        ' * @param fontStretch The new stretch for the font.
        ' */
        Public Overrides Sub setFontStretch(ByVal fontStretch As String)
            Dim name As COSName = Nothing
            If (fontStretch <> "") Then
                name = COSName.getPDFName(fontStretch)
            End If
            dic.setItem(COSName.FONT_STRETCH, name)
        End Sub

        '/**
        ' * This will get the font flags.
        ' *
        ' * @return The font flags.
        ' */
        Public Overrides Function getFlags() As Integer
            If (flags = -1) Then
                flags = dic.getInt(COSName.FLAGS, 0)
            End If
            Return flags
        End Function

        '/**
        ' * This will set the font flags.
        ' *
        ' * @param flags The new font flags.
        ' */
        Public Overrides Sub setFlags(ByVal flags As Integer)
            dic.setInt(COSName.FLAGS, flags)
            Me.flags = flags
        End Sub

        '/**
        ' * This will get the fonts bounding box.
        ' *
        ' * @return The fonts bounding box.
        ' */
        Public Overrides Function getFontBoundingBox() As PDRectangle
            Dim rect As COSArray = dic.getDictionaryObject(COSName.FONT_BBOX)
            Dim retval As PDRectangle = Nothing
            If (rect IsNot Nothing) Then
                retval = New PDRectangle(rect)
            End If
            Return retval
        End Function

        '/**
        ' * Set the fonts bounding box.
        ' *
        ' * @param rect The new bouding box.
        ' */
        Public Overrides Sub setFontBoundingBox(ByVal rect As PDRectangle)
            Dim array As COSArray = Nothing
            If (rect IsNot Nothing) Then
                array = rect.getCOSArray()
            End If
            dic.setItem(COSName.FONT_BBOX, array)
        End Sub

        '/**
        ' * This will get the italic angle for the font.
        ' *
        ' * @return The italic angle.
        ' */
        Public Overrides Function getItalicAngle() As Single
            Return dic.getFloat(COSName.ITALIC_ANGLE, 0)
        End Function

        '/**
        ' * This will set the italic angle for the font.
        ' *
        ' * @param angle The new italic angle for the font.
        ' */
        Public Overrides Sub setItalicAngle(ByVal angle As Single)
            dic.setFloat(COSName.ITALIC_ANGLE, angle)
        End Sub

        '/**
        ' * This will get the ascent for the font.
        ' *
        ' * @return The ascent.
        ' */
        Public Overrides Function getAscent() As Single
            Return dic.getFloat(COSName.ASCENT, 0)
        End Function

        '/**
        ' * This will set the ascent for the font.
        ' *
        ' * @param ascent The new ascent for the font.
        ' */
        Public Overrides Sub setAscent(ByVal ascent As Single)
            dic.setFloat(COSName.ASCENT, ascent)
        End Sub

        '/**
        ' * This will get the descent for the font.
        ' *
        ' * @return The descent.
        ' */
        Public Overrides Function getDescent() As Single
            Return dic.getFloat(COSName.DESCENT, 0)
        End Function

        '/**
        ' * This will set the descent for the font.
        ' *
        ' * @param descent The new descent for the font.
        ' */
        Public Overrides Sub setDescent(ByVal descent As Single)
            dic.setFloat(COSName.DESCENT, descent)
        End Sub

        '/**
        ' * This will get the leading for the font.
        ' *
        ' * @return The leading.
        ' */
        Public Overrides Function getLeading() As Single
            Return dic.getFloat(COSName.LEADING, 0)
        End Function

        '/**
        '* This will set the leading for the font.
        '*
        '* @param leading The new leading for the font.
        '*/
        Public Overrides Sub setLeading(ByVal leading As Single)
            dic.setFloat(COSName.LEADING, leading)
        End Sub

        '/**
        ' * This will get the CapHeight for the font.
        ' *
        ' * @return The cap height.
        ' */
        Public Overrides Function getCapHeight() As Single
            If (capHeight = Single.NegativeInfinity) Then
                '/* We observed a negative value being returned with
                ' * the Scheherazade font. PDFBOX-429 was logged for Me.
                ' * We are not sure if returning the absolute value
                ' * is the correct fix, but it seems to work.  */
                capHeight = Math.Abs(dic.getFloat(COSName.CAP_HEIGHT, 0))
            End If
            Return capHeight
        End Function


        '/**
        ' * This will set the cap height for the font.
        ' *
        ' * @param capHeight The new cap height for the font.
        ' */
        Public Overrides Sub setCapHeight(ByVal capHeight As Single)
            dic.setFloat(COSName.CAP_HEIGHT, capHeight)
            Me.capHeight = capHeight
        End Sub

        '/**
        ' * This will get the x height for the font.
        ' *
        ' * @return The x height.
        ' */
        Public Overrides Function getXHeight() As Single
            If (xHeight = Single.NegativeInfinity) Then
                '/* We observed a negative value being returned with
                ' * the Scheherazade font. PDFBOX-429 was logged for Me.
                ' * We are not sure if returning the absolute value
                ' * is the correct fix, but it seems to work.  */
                xHeight = Math.Abs(dic.getFloat(COSName.XHEIGHT, 0))
            End If
            Return xHeight
        End Function

        '/**
        ' * This will set the x height for the font.
        ' *
        ' * @param xHeight The new x height for the font.
        ' */
        Public Overrides Sub setXHeight(ByVal xHeight As Single)
            dic.setFloat(COSName.XHEIGHT, xHeight)
            Me.xHeight = xHeight
        End Sub

        '/**
        ' * This will get the stemV for the font.
        ' *
        ' * @return The stem v value.
        ' */
        Public Overrides Function getStemV() As Single
            Return dic.getFloat(COSName.STEM_V, 0)
        End Function

        '/**
        ' * This will set the stem V for the font.
        ' *
        ' * @param stemV The new stem v for the font.
        ' */
        Public Overrides Sub setStemV(ByVal stemV As Single)
            dic.setFloat(COSName.STEM_V, stemV)
        End Sub

        '/**
        ' * This will get the stemH for the font.
        ' *
        ' * @return The stem h value.
        ' */
        Public Overrides Function getStemH() As Single
            Return dic.getFloat(COSName.STEM_H, 0)
        End Function

        '/**
        ' * This will set the stem H for the font.
        ' *
        ' * @param stemH The new stem h for the font.
        ' */
        Public Overrides Sub setStemH(ByVal stemH As Single)
            dic.setFloat(COSName.STEM_H, stemH)
        End Sub

        '/**
        ' * This will get the average width for the font.
        ' *
        ' * @return The average width value.
        ' */
        Public Overrides Function getAverageWidth() As Single
            Return dic.getFloat(COSName.AVG_WIDTH, 0)
        End Function

        '/**
        ' * This will set the average width for the font.
        ' *
        ' * @param averageWidth The new average width for the font.
        ' */
        Public Overrides Sub setAverageWidth(ByVal averageWidth As Single)
            dic.setFloat(COSName.AVG_WIDTH, averageWidth)
        End Sub

        '/**
        ' * This will get the max width for the font.
        ' *
        ' * @return The max width value.
        ' */
        Public Overrides Function getMaxWidth() As Single
            Return dic.getFloat(COSName.MAX_WIDTH, 0)
        End Function

        '/**
        ' * This will set the max width for the font.
        ' *
        ' * @param maxWidth The new max width for the font.
        ' */
        Public Overrides Sub setMaxWidth(ByVal maxWidth As Single)
            dic.setFloat(COSName.MAX_WIDTH, maxWidth)
        End Sub

        '/**
        ' * This will get the missing width for the font.
        ' *
        ' * @return The missing width value.
        ' */
        Public Overrides Function getMissingWidth() As Single
            Return dic.getFloat(COSName.MISSING_WIDTH, 0)
        End Function

        '/**
        ' * This will set the missing width for the font.
        ' *
        ' * @param missingWidth The new missing width for the font.
        ' */
        Public Overrides Sub setMissingWidth(ByVal missingWidth As Single)
            dic.setFloat(COSName.MISSING_WIDTH, missingWidth)
        End Sub

        '/**
        ' * This will get the character set for the font.
        ' *
        ' * @return The character set value.
        ' */
        Public Overrides Function getCharSet() As String
            Dim retval As String = ""
            Dim name As COSString = dic.getDictionaryObject(COSName.CHAR_SET)
            If (name IsNot Nothing) Then
                retval = name.getString()
            End If
            Return retval
        End Function

        '/**
        ' * This will set the character set for the font.
        ' *
        ' * @param charSet The new character set for the font.
        ' */
        Public Overrides Sub setCharacterSet(ByVal charSet As String)
            Dim name As COSString = Nothing
            If (charSet <> "") Then
                name = New COSString(charSet)
            End If
            dic.setItem(COSName.CHAR_SET, name)
        End Sub

        '/**
        ' * A stream containing a Type 1 font program.
        ' *
        ' * @return A stream containing a Type 1 font program.
        ' */
        Public Function getFontFile() As PDStream
            Dim retval As PDStream = Nothing
            Dim stream As COSStream = dic.getDictionaryObject(COSName.FONT_FILE)
            If (stream IsNot Nothing) Then
                retval = New PDStream(stream)
            End If
            Return retval
        End Function

        '/**
        ' * Set the type 1 font program.
        ' *
        ' * @param type1Stream The type 1 stream.
        ' */
        Public Sub setFontFile(ByVal type1Stream As PDStream)
            dic.setItem(COSName.FONT_FILE, type1Stream)
        End Sub

        '/**
        ' * A stream containing a true type font program.
        ' *
        ' * @return A stream containing a true type font program.
        ' */
        Public Function getFontFile2() As PDStream
            Dim retval As PDStream = Nothing
            Dim stream As COSStream = dic.getDictionaryObject(COSName.FONT_FILE2)
            If (stream IsNot Nothing) Then
                retval = New PDStream(stream)
            End If
            Return retval
        End Function

        '/**
        ' * Set the true type font program.
        ' *
        ' * @param ttfStream The true type stream.
        ' */
        Public Sub setFontFile2(ByVal ttfStream As PDStream)
            dic.setItem(COSName.FONT_FILE2, ttfStream)
        End Sub

        '/**
        ' * A stream containing a font program that is not true type or type 1.
        ' *
        ' * @return A stream containing a font program.
        ' */
        Public Function getFontFile3() As PDStream
            Dim retval As PDStream = Nothing
            Dim stream As COSStream = dic.getDictionaryObject(COSName.FONT_FILE3)
            If (stream IsNot Nothing) Then
                retval = New PDStream(stream)
            End If
            Return retval
        End Function

        '/**
        ' * Set a stream containing a font program that is not true type or type 1.
        ' *
        ' * @param stream The font program stream.
        ' */
        Public Sub setFontFile3(ByVal stream As PDStream)
            dic.setItem(COSName.FONT_FILE3, stream)
        End Sub

    End Class

End Namespace
