Imports System.IO
Imports System.Drawing
Imports FinSeA.Drawings
Imports FinSeA.org.apache.fontbox.afm
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.fontbox.util


Namespace org.apache.pdfbox.pdmodel.font

    '/**
    ' * This class represents the font descriptor when the font information
    ' * is coming from an AFM file.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PDFontDescriptorAFM
        Inherits PDFontDescriptor

        Private afm As FontMetric

        '/**
        ' * Constructor.
        ' *
        ' * @param afmFile The AFM file.
        ' */
        Public Sub New(ByVal afmFile As FontMetric)
            afm = afmFile
        End Sub

        '/**
        ' * Get the font name.
        ' *
        ' * @return The name of the font.
        ' */
        Public Overrides Function getFontName() As String
            Return afm.getFontName()
        End Function

        '/**
        ' * This will set the font name.
        ' *
        ' * @param fontName The new name for the font.
        ' */
        Public Overrides Sub setFontName(ByVal fontName As String)
            Me.RaiseUnsupported()
        End Sub

        Private Sub RaiseUnsupported()
            Throw New NotSupportedException("The AFM Font descriptor is immutable")
        End Sub

        '/**
        ' * A string representing the preferred font family.
        ' *
        ' * @return The font family.
        ' */
        Public Overrides Function getFontFamily() As String
            Return afm.getFamilyName()
        End Function

        '/**
        ' * This will set the font family.
        ' *
        ' * @param fontFamily The font family.
        ' */
        Public Overrides Sub setFontFamily(ByVal fontFamily As String)
            Me.RaiseUnsupported()
        End Sub

        '/**
        ' * The weight of the font.  According to the PDF spec "possible values are
        ' * 100, 200, 300, 400, 500, 600, 700, 800 or 900"  Where a higher number is
        ' * more weight and appears to be more bold.
        ' *
        ' * @return The font weight.
        ' */
        Public Overrides Function getFontWeight() As Single
            Dim weight As String = afm.getWeight()
            Dim retval As Single = 500
            If (weight <> "" AndAlso LCase(weight).Equals("bold")) Then
                retval = 900
            ElseIf (weight <> "" AndAlso LCase(weight).Equals("light")) Then
                retval = 100
            End If
            Return retval
        End Function

        '/**
        ' * Set the weight of the font.
        ' *
        ' * @param fontWeight The new weight of the font.
        ' */
        Public Overrides Sub setFontWeight(ByVal fontWeight As Single)
            Me.RaiseUnsupported()
        End Sub

        '/**
        ' * A string representing the preferred font stretch.
        ' *
        ' * @return The font stretch.
        ' */
        Public Overrides Function getFontStretch() As String
            Return ""
        End Function

        '/**
        ' * This will set the font stretch.
        ' *
        ' * @param fontStretch The font stretch
        ' */
        Public Overrides Sub setFontStretch(ByVal fontStretch As String)
            Me.RaiseUnsupported()
        End Sub

        '/**
        ' * This will get the font flags.
        ' *
        ' * @return The font flags.
        ' */
        Public Overrides Function getFlags() As Integer
            'I believe that the only flag that AFM supports is the is fixed pitch
            Return IIf(afm.isFixedPitch(), 1, 0)
        End Function

        '/**
        ' * This will set the font flags.
        ' *
        ' * @param flags The new font flags.
        ' */
        Public Overrides Sub setFlags(ByVal flags As Integer)
            Me.RaiseUnsupported()
        End Sub

        '/**
        ' * This will get the fonts bouding box.
        ' *
        ' * @return The fonts bouding box.
        ' */
        Public Overrides Function getFontBoundingBox() As PDRectangle
            Dim box As BoundingBox = afm.getFontBBox()
            Dim retval As PDRectangle = Nothing
            If (box IsNot Nothing) Then
                retval = New PDRectangle(box)
            End If
            Return retval
        End Function

        '/**
        ' * Set the fonts bounding box.
        ' *
        ' * @param rect The new bouding box.
        ' */
        Public Overrides Sub setFontBoundingBox(ByVal rect As PDRectangle)
            Me.RaiseUnsupported()
        End Sub

        '/**
        ' * This will get the italic angle for the font.
        ' *
        ' * @return The italic angle.
        ' */
        Public Overrides Function getItalicAngle() As Single
            Return afm.getItalicAngle()
        End Function

        '/**
        ' * This will set the italic angle for the font.
        ' *
        ' * @param angle The new italic angle for the font.
        ' */
        Public Overrides Sub setItalicAngle(ByVal angle As Single)
            Me.RaiseUnsupported()
        End Sub

        '/**
        ' * This will get the ascent for the font.
        ' *
        ' * @return The ascent.
        ' */
        Public Overrides Function getAscent() As Single
            Return afm.getAscender()
        End Function

        '/**
        ' * This will set the ascent for the font.
        ' *
        ' * @param ascent The new ascent for the font.
        ' */
        Public Overrides Sub setAscent(ByVal ascent As Single)
            Me.RaiseUnsupported()
        End Sub

        '/**
        ' * This will get the descent for the font.
        ' *
        ' * @return The descent.
        ' */
        Public Overrides Function getDescent() As Single
            Return afm.getDescender()
        End Function

        '/**
        ' * This will set the descent for the font.
        ' *
        ' * @param descent The new descent for the font.
        ' */
        Public Overrides Sub setDescent(ByVal descent As Single)
            Me.RaiseUnsupported()
        End Sub

        '/**
        ' * This will get the leading for the font.
        ' *
        ' * @return The leading.
        ' */
        Public Overrides Function getLeading() As Single
            'AFM does not support setting the leading so we will just ignore it.
            Return 0.0F
        End Function

        '/**
        ' * This will set the leading for the font.
        ' *
        ' * @param leading The new leading for the font.
        ' */
        Public Overrides Sub setLeading(ByVal leading As Single)
            Me.RaiseUnsupported()
        End Sub

        '/**
        ' * This will get the CapHeight for the font.
        ' *
        ' * @return The cap height.
        ' */
        Public Overrides Function getCapHeight() As Single
            Return afm.getCapHeight()
        End Function

        '/**
        ' * This will set the cap height for the font.
        ' *
        ' * @param capHeight The new cap height for the font.
        ' */
        Public Overrides Sub setCapHeight(ByVal capHeight As Single)
            Me.RaiseUnsupported()
        End Sub

        '/**
        ' * This will get the x height for the font.
        ' *
        ' * @return The x height.
        ' */
        Public Overrides Function getXHeight() As Single
            Return afm.getXHeight()
        End Function

        '/**
        ' * This will set the x height for the font.
        ' *
        ' * @param xHeight The new x height for the font.
        ' */
        Public Overrides Sub setXHeight(ByVal xHeight As Single)
            Me.RaiseUnsupported()
        End Sub

        '/**
        ' * This will get the stemV for the font.
        ' *
        ' * @return The stem v value.
        ' */
        Public Overrides Function getStemV() As Single
            'afm does not have a stem v
            Return 0
        End Function

        '/**
        ' * This will set the stem V for the font.
        ' *
        ' * @param stemV The new stem v for the font.
        ' */
        Public Overrides Sub setStemV(ByVal stemV As Single)
            Me.RaiseUnsupported()
        End Sub

        '/**
        ' * This will get the stemH for the font.
        ' *
        ' * @return The stem h value.
        ' */
        Public Overrides Function getStemH() As Single
            'afm does not have a stem h
            Return 0
        End Function

        '/**
        ' * This will set the stem H for the font.
        ' *
        ' * @param stemH The new stem h for the font.
        ' */
        Public Overrides Sub setStemH(ByVal stemH As Single)
            Me.RaiseUnsupported()
        End Sub

        '/**
        ' * This will get the average width for the font.
        ' *
        ' * @return The average width value.
        ' *
        ' * @throws IOException If there is an error calculating the average width.
        ' */
        Public Overrides Function getAverageWidth() As Single 'throws IOException
            Return afm.getAverageCharacterWidth()
        End Function

        '/**
        ' * This will set the average width for the font.
        ' *
        ' * @param averageWidth The new average width for the font.
        ' */
        Public Overrides Sub setAverageWidth(ByVal averageWidth As Single)
            Me.RaiseUnsupported()
        End Sub

        '/**
        ' * This will get the max width for the font.
        ' *
        ' * @return The max width value.
        ' */
        Public Overrides Function getMaxWidth() As Single
            'afm does not support max width;
            Return 0
        End Function

        '/**
        ' * This will set the max width for the font.
        ' *
        ' * @param maxWidth The new max width for the font.
        ' */
        Public Overrides Sub setMaxWidth(ByVal maxWidth As Single)
            Me.RaiseUnsupported()
        End Sub

        '/**
        ' * This will get the missing width for the font.
        ' *
        ' * @return The missing width value.
        ' */
        Public Overrides Function getMissingWidth() As Single
            Return 0
        End Function

        '/**
        ' * This will set the missing width for the font.
        ' *
        ' * @param missingWidth The new missing width for the font.
        ' */
        Public Overrides Sub setMissingWidth(ByVal missingWidth As Single)
            Me.RaiseUnsupported()
        End Sub

        '/**
        ' * This will get the character set for the font.
        ' *
        ' * @return The character set value.
        ' */
        Public Overrides Function getCharSet() As String
            Return afm.getCharacterSet()
        End Function

        '/**
        ' * This will set the character set for the font.
        ' *
        ' * @param charSet The new character set for the font.
        ' */
        Public Overrides Sub setCharacterSet(ByVal charSet As String)
            Me.RaiseUnsupported()
        End Sub


    End Class

End Namespace
