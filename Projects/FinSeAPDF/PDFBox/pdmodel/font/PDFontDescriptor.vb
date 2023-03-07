Imports System.IO
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.font

    '/**
    ' * This class represents an interface to the font description.  This will depend
    ' * on the font type for the actual implementation.  If it is a AFM/cmap/or embedded font.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public MustInherit Class PDFontDescriptor

        ''' <summary>
        ''' A font descriptor flag.  See PDF Reference for description.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FLAG_FIXED_PITCH As Integer = 1

        ''' <summary>
        ''' A font descriptor flag.  See PDF Reference for description.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FLAG_SERIF = 2

        ''' <summary>
        ''' A font descriptor flag.  See PDF Reference for description.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FLAG_SYMBOLIC = 4


        ''' <summary>
        ''' A font descriptor flag.  See PDF Reference for description.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FLAG_SCRIPT = 8

        ''' <summary>
        ''' A font descriptor flag.  See PDF Reference for description.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FLAG_NON_SYMBOLIC = 32

        ''' <summary>
        ''' A font descriptor flag.  See PDF Reference for description.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FLAG_ITALIC = 64

        ''' <summary>
        ''' A font descriptor flag.  See PDF Reference for description.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FLAG_ALL_CAP = 65536

        ''' <summary>
        ''' A font descriptor flag.  See PDF Reference for description.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FLAG_SMALL_CAP = 131072

        ''' <summary>
        ''' A font descriptor flag.  See PDF Reference for description.
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FLAG_FORCE_BOLD = 262144


        '/**
        ' * Get the font name.
        ' *
        ' * @return The name of the font.
        ' */
        Public MustOverride Function getFontName() As String

        '/**
        ' * This will set the font name.
        ' *
        ' * @param fontName The new name for the font.
        ' */
        Public MustOverride Sub setFontName(ByVal fontName As String)

        '/**
        ' * A string representing the preferred font family.
        ' *
        ' * @return The font family.
        ' */
        Public MustOverride Function getFontFamily() As String

        '/**
        ' * This will set the font family.
        ' *
        ' * @param fontFamily The font family.
        ' */
        Public MustOverride Sub setFontFamily(ByVal fontFamily As String)

        '/**
        ' * A string representing the preferred font stretch.
        ' * According to the PDF Spec:
        ' * The font stretch value; it must be one of the following (ordered from
        ' * narrowest to widest): UltraCondensed, ExtraCondensed, Condensed, SemiCondensed,
        ' * Normal, SemiExpanded, Expanded, ExtraExpanded or UltraExpanded.
        ' *
        ' * @return The font stretch.
        ' */
        Public MustOverride Function getFontStretch() As String

        '/**
        ' * This will set the font stretch.
        ' *
        ' * @param fontStretch The font stretch
        ' */
        Public MustOverride Sub setFontStretch(ByVal fontStretch As String)

        '/**
        ' * The weight of the font.  According to the PDF spec "possible values are
        ' * 100, 200, 300, 400, 500, 600, 700, 800 or 900"  Where a higher number is
        ' * more weight and appears to be more bold.
        ' *
        ' * @return The font weight.
        ' */
        Public MustOverride Function getFontWeight() As Single

        '/**
        ' * Set the weight of the font.
        ' *
        ' * @param fontWeight The new weight of the font.
        ' */
        Public MustOverride Sub setFontWeight(ByVal fontWeight As Single)

        '/**
        ' * This will get the font flags.
        ' *
        ' * @return The font flags.
        ' */
        Public MustOverride Function getFlags() As Integer

        '/**
        ' * This will set the font flags.
        ' *
        ' * @param flags The new font flags.
        ' */
        Public MustOverride Sub setFlags(ByVal flags As Integer)

        '/**
        ' * A convenience method that checks the flag bit.
        ' *
        ' * @return The flag value.
        ' */
        Public Function isFixedPitch() As Boolean
            Return isFlagBitOn(FLAG_FIXED_PITCH)
        End Function

        '/**
        ' * A convenience method that sets the flag bit.
        ' *
        ' * @param flag The flag value.
        ' */
        Public Sub setFixedPitch(ByVal flag As Boolean)
            setFlagBit(FLAG_FIXED_PITCH, flag)
        End Sub

        '/**
        ' * A convenience method that checks the flag bit.
        ' *
        ' * @return The flag value.
        ' */
        Public Function isSerif() As Boolean
            Return isFlagBitOn(FLAG_SERIF)
        End Function

        '/**
        ' * A convenience method that sets the flag bit.
        ' *
        ' * @param flag The flag value.
        ' */
        Public Sub setSerif(ByVal flag As Boolean)
            setFlagBit(FLAG_SERIF, flag)
        End Sub

        '/**
        ' * A convenience method that checks the flag bit.
        ' *
        ' * @return The flag value.
        ' */
        Public Function isSymbolic() As Boolean
            Return isFlagBitOn(FLAG_SYMBOLIC)
        End Function

        '/**
        ' * A convenience method that sets the flag bit.
        ' *
        ' * @param flag The flag value.
        ' */
        Public Sub setSymbolic(ByVal flag As Boolean)
            setFlagBit(FLAG_SYMBOLIC, flag)
        End Sub

        '/**
        ' * A convenience method that checks the flag bit.
        ' *
        ' * @return The flag value.
        ' */
        Public Function isScript() As Boolean
            Return isFlagBitOn(FLAG_SCRIPT)
        End Function

        '/**
        ' * A convenience method that sets the flag bit.
        ' *
        ' * @param flag The flag value.
        ' */
        Public Sub setScript(ByVal flag As Boolean)
            setFlagBit(FLAG_SCRIPT, flag)
        End Sub

        '/**
        ' * A convenience method that checks the flag bit.
        ' *
        ' * @return The flag value.
        ' */
        Public Function isNonSymbolic() As Boolean
            Return isFlagBitOn(FLAG_NON_SYMBOLIC)
        End Function

        '/**
        ' * A convenience method that sets the flag bit.
        ' *
        ' * @param flag The flag value.
        ' */
        Public Sub setNonSymbolic(ByVal flag As Boolean)
            setFlagBit(FLAG_NON_SYMBOLIC, flag)
        End Sub

        '/**
        ' * A convenience method that checks the flag bit.
        ' *
        ' * @return The flag value.
        ' */
        Public Function isItalic() As Boolean
            Return isFlagBitOn(FLAG_ITALIC)
        End Function

        '/**
        ' * A convenience method that sets the flag bit.
        ' *
        ' * @param flag The flag value.
        ' */
        Public Sub setItalic(ByVal flag As Boolean)
            setFlagBit(FLAG_ITALIC, flag)
        End Sub

        '/**
        ' * A convenience method that checks the flag bit.
        ' *
        ' * @return The flag value.
        ' */
        Public Function isAllCap() As Boolean
            Return isFlagBitOn(FLAG_ALL_CAP)
        End Function

        '/**
        ' * A convenience method that sets the flag bit.
        ' *
        ' * @param flag The flag value.
        ' */
        Public Sub setAllCap(ByVal flag As Boolean)
            setFlagBit(FLAG_ALL_CAP, flag)
        End Sub

        '/**
        ' * A convenience method that checks the flag bit.
        ' *
        ' * @return The flag value.
        ' */
        Public Function isSmallCap() As Boolean
            Return isFlagBitOn(FLAG_SMALL_CAP)
        End Function

        '/**
        ' * A convenience method that sets the flag bit.
        ' *
        ' * @param flag The flag value.
        ' */
        Public Sub setSmallCap(ByVal flag As Boolean)
            setFlagBit(FLAG_SMALL_CAP, flag)
        End Sub

        '/**
        ' * A convenience method that checks the flag bit.
        ' *
        ' * @return The flag value.
        ' */
        Public Function isForceBold() As Boolean
            Return isFlagBitOn(FLAG_FORCE_BOLD)
        End Function

        '/**
        ' * A convenience method that sets the flag bit.
        ' *
        ' * @param flag The flag value.
        ' */
        Public Sub setForceBold(ByVal flag As Boolean)
            setFlagBit(FLAG_FORCE_BOLD, flag)
        End Sub

        Private Function isFlagBitOn(ByVal bit As Integer) As Boolean
            Return (getFlags() And bit) = bit
        End Function

        Private Sub setFlagBit(ByVal bit As Integer, ByVal value As Boolean)
            Dim flags As Integer = getFlags()
            If (value) Then
                flags = flags Or bit
            Else
                flags = flags And (&HFFFFFFFF Xor bit)
            End If
            setFlags(flags)
        End Sub

        '/**
        ' * This will get the fonts bouding box.
        ' *
        ' * @return The fonts bouding box.
        ' */
        Public MustOverride Function getFontBoundingBox() As PDRectangle

        '/**
        ' * Set the fonts bounding box.
        ' *
        ' * @param rect The new bouding box.
        ' */
        Public MustOverride Sub setFontBoundingBox(ByVal rect As PDRectangle)

        '/**
        ' * This will get the italic angle for the font.
        ' *
        ' * @return The italic angle.
        ' */
        Public MustOverride Function getItalicAngle() As Single

        '/**
        ' * This will set the italic angle for the font.
        ' *
        ' * @param angle The new italic angle for the font.
        ' */
        Public MustOverride Sub setItalicAngle(ByVal angle As Single)

        '/**
        ' * This will get the ascent for the font.
        ' *
        ' * @return The ascent.
        ' */
        Public MustOverride Function getAscent() As Single

        '/**
        ' * This will set the ascent for the font.
        ' *
        ' * @param ascent The new ascent for the font.
        ' */
        Public MustOverride Sub setAscent(ByVal ascent As Single)

        '/**
        ' * This will get the descent for the font.
        ' *
        ' * @return The descent.
        ' */
        Public MustOverride Function getDescent() As Single

        '/**
        ' * This will set the descent for the font.
        ' *
        ' * @param descent The new descent for the font.
        ' */
        Public MustOverride Sub setDescent(ByVal descent As Single)

        '/**
        ' * This will get the leading for the font.
        ' *
        ' * @return The leading.
        ' */
        Public MustOverride Function getLeading() As Single

        '/**
        ' * This will set the leading for the font.
        ' *
        ' * @param leading The new leading for the font.
        ' */
        Public MustOverride Sub setLeading(ByVal leading As Single)

        '/**
        ' * This will get the CapHeight for the font.
        ' *
        ' * @return The cap height.
        ' */
        Public MustOverride Function getCapHeight() As Single

        '/**
        ' * This will set the cap height for the font.
        ' *
        ' * @param capHeight The new cap height for the font.
        ' */
        Public MustOverride Sub setCapHeight(ByVal capHeight As Single)

        '/**
        ' * This will get the x height for the font.
        ' *
        ' * @return The x height.
        ' */
        Public MustOverride Function getXHeight() As Single

        '/**
        ' * This will set the x height for the font.
        ' *
        ' * @param xHeight The new x height for the font.
        ' */
        Public MustOverride Sub setXHeight(ByVal xHeight As Single)

        '/**
        ' * This will get the stemV for the font.
        ' *
        ' * @return The stem v value.
        ' */
        Public MustOverride Function getStemV() As Single

        '/**
        ' * This will set the stem V for the font.
        ' *
        ' * @param stemV The new stem v for the font.
        ' */
        Public MustOverride Sub setStemV(ByVal stemV As Single)

        '/**
        ' * This will get the stemH for the font.
        ' *
        ' * @return The stem h value.
        ' */
        Public MustOverride Function getStemH() As Single

        '/**
        ' * This will set the stem H for the font.
        ' *
        ' * @param stemH The new stem h for the font.
        ' */
        Public MustOverride Sub setStemH(ByVal stemH As Single)

        '/**
        ' * This will get the average width for the font.  This is part of the
        ' * definition in the font description.  If it is not present then PDFBox
        ' * will make an attempt to calculate it.
        ' *
        ' * @return The average width value.
        ' *
        ' * @throws IOException If there is an error calculating the average width.
        ' */
        Public MustOverride Function getAverageWidth() As Single ' throws IOException;

        '/**
        ' * This will set the average width for the font.
        ' *
        ' * @param averageWidth The new average width for the font.
        ' */
        Public MustOverride Sub setAverageWidth(ByVal averageWidth As Single)

        '/**
        ' * This will get the max width for the font.
        ' *
        ' * @return The max width value.
        ' */
        Public MustOverride Function getMaxWidth() As Single

        '/**
        ' * This will set the max width for the font.
        ' *
        ' * @param maxWidth The new max width for the font.
        ' */
        Public MustOverride Sub setMaxWidth(ByVal maxWidth As Single)

        '/**
        ' * This will get the character set for the font.
        ' *
        ' * @return The character set value.
        ' */
        Public MustOverride Function getCharSet() As String

        '/**
        ' * This will set the character set for the font.
        ' *
        ' * @param charSet The new character set for the font.
        ' */
        Public MustOverride Sub setCharacterSet(ByVal charSet As String)

        '/**
        ' * This will get the missing width for the font.
        ' *
        ' * @return The missing width value.
        ' */
        Public MustOverride Function getMissingWidth() As Single

        '/**
        ' * This will set the missing width for the font.
        ' *
        ' * @param charSet The new missing width for the font.
        ' */
        Public MustOverride Sub setMissingWidth(ByVal missingWidth As Single)

    End Class

End Namespace
