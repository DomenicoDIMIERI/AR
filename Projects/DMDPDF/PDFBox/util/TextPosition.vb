Imports FinSeA.org.apache.pdfbox.pdmodel
Imports FinSeA.org.apache.pdfbox.pdmodel.font
Imports System.Text
Imports FinSeA.Text

Namespace org.apache.pdfbox.util

    '/**
    ' * This represents a string and a position on the screen of those characters.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.12 $
    ' */
    Public Class TextPosition

        '/* TextMatrix for the start of the text object.  Coordinates
        ' * are in display units and have not been adjusted. */
        Private textPos As Matrix

        ' ending X and Y coordinates in display units
        Private endX As Single
        Private endY As Single

        Private maxTextHeight As Single ' maximum height of text, in display units
        Private rot As Integer ' 0, 90, 180, 270 degrees of page rotation
        Private x As Single = Single.NegativeInfinity
        Private y As Single = Single.NegativeInfinity
        Private pageHeight As Single
        Private pageWidth As Single
        Private widths As Single()
        Private widthOfSpace As Single ' width of a space, in display units
        Private str As String
        Private unicodeCP() As Integer
        Private font As PDFont
        Private fontSize As Single
        Private fontSizePt As Integer
        ' TODO remove unused value
        Private wordSpacing As Single ' word spacing value, in display units

        '/**
        ' *  Constructor.
        ' */
        Protected Sub New()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param page Page that the text is located in
        ' * @param textPositionSt TextMatrix for start of text (in display units)
        ' * @param textPositionEnd TextMatrix for end of text (in display units)
        ' * @param maxFontH Maximum height of text (in display units)
        ' * @param individualWidths The width of each individual character. (in ? units)
        ' * @param spaceWidth The width of the space character. (in display units)
        ' * @param string The character to be displayed.
        ' * @param currentFont The current for for this text position.
        ' * @param fontSizeValue The new font size.
        ' * @param fontSizeInPt The font size in pt units.
        ' * @param ws The word spacing parameter (in display units)
        ' */
        Public Sub New(ByVal page As PDPage, ByVal textPositionSt As Matrix, ByVal textPositionEnd As Matrix, ByVal maxFontH As Single, ByVal individualWidths As Single(), ByVal spaceWidth As Single, ByVal [string] As String, ByVal currentFont As PDFont, ByVal fontSizeValue As Single, ByVal fontSizeInPt As Integer, ByVal ws As Single)
            Me.textPos = textPositionSt

            Me.endX = textPositionEnd.getXPosition()
            Me.endY = textPositionEnd.getYPosition()

            Me.rot = page.findRotation()
            ' make sure it is 0 to 270 and no negative numbers
            If (Me.rot < 0) Then
                rot += 360
            ElseIf (rot >= 360) Then
                rot -= 360
            End If

            Me.maxTextHeight = maxFontH
            Me.pageHeight = page.findMediaBox().getHeight()
            Me.pageWidth = page.findMediaBox().getWidth()

            Me.widths = individualWidths
            Me.widthOfSpace = spaceWidth
            Me.str = [string]
            Me.font = currentFont
            Me.fontSize = fontSizeValue
            Me.fontSizePt = fontSizeInPt
            Me.wordSpacing = ws
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param pageRotation rotation of the page that the text is located in
        ' * @param pageWidthValue rotation of the page that the text is located in
        ' * @param pageHeightValue rotation of the page that the text is located in
        ' * @param textPositionSt TextMatrix for start of text (in display units)
        ' * @param textPositionEnd TextMatrix for end of text (in display units)
        ' * @param maxFontH Maximum height of text (in display units)
        ' * @param individualWidth The width of the given character/string. (in ? units)
        ' * @param spaceWidth The width of the space character. (in display units)
        ' * @param string The character to be displayed.
        ' * @param currentFont The current for for this text position.
        ' * @param fontSizeValue The new font size.
        ' * @param fontSizeInPt The font size in pt units.
        ' *
        ' * @deprecated Use {@link TextPosition(int, Single, Single, Matrix, Single, Single, Single, Single, Single, 
        ' * String, PDFont, Single, int)} instead.
        ' */
        Public Sub New(ByVal pageRotation As Integer, ByVal pageWidthValue As Single, ByVal pageHeightValue As Single, ByVal textPositionSt As Matrix, ByVal textPositionEnd As Matrix, ByVal maxFontH As Single, ByVal individualWidth As Single, ByVal spaceWidth As Single, ByVal [string] As String, ByVal currentFont As PDFont, ByVal fontSizeValue As Single, ByVal fontSizeInPt As Integer)
            Me.New(pageRotation, pageWidthValue, pageHeightValue, textPositionSt, textPositionEnd.getXPosition(), textPositionEnd.getYPosition(), maxFontH, individualWidth, spaceWidth, [string], Nothing, currentFont, fontSizeValue, fontSizeInPt)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param pageRotation rotation of the page that the text is located in
        ' * @param pageWidthValue rotation of the page that the text is located in
        ' * @param pageHeightValue rotation of the page that the text is located in
        ' * @param textPositionSt TextMatrix for start of text (in display units)
        ' * @param endXValue x coordinate of the end position
        ' * @param endYValue y coordinate of the end position
        ' * @param maxFontH Maximum height of text (in display units)
        ' * @param individualWidth The width of the given character/string. (in ? units)
        ' * @param spaceWidth The width of the space character. (in display units)
        ' * @param string The character to be displayed.
        ' * @param currentFont The current for for this text position.
        ' * @param fontSizeValue The new font size.
        ' * @param fontSizeInPt The font size in pt units.
        ' * 
        ' * @deprecated use {@link #TextPosition(int, Single, Single, Matrix, Single, Single, Single, Single, Single, 
        ' * String, int[], PDFont, Single, int)} insetad
        ' */
        Public Sub New(ByVal pageRotation As Integer, ByVal pageWidthValue As Single, ByVal pageHeightValue As Single, ByVal textPositionSt As Matrix, ByVal endXValue As Single, ByVal endYValue As Single, ByVal maxFontH As Single, ByVal individualWidth As Single, ByVal spaceWidth As Single, ByVal [string] As String, ByVal currentFont As PDFont, ByVal fontSizeValue As Single, ByVal fontSizeInPt As Integer)
            Me.New(pageRotation, pageWidthValue, pageHeightValue, textPositionSt, endXValue, endYValue, maxFontH, individualWidth, spaceWidth, [string], Nothing, currentFont, fontSizeValue, fontSizeInPt)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param pageRotation rotation of the page that the text is located in
        ' * @param pageWidthValue rotation of the page that the text is located in
        ' * @param pageHeightValue rotation of the page that the text is located in
        ' * @param textPositionSt TextMatrix for start of text (in display units)
        ' * @param endXValue x coordinate of the end position
        ' * @param endYValue y coordinate of the end position
        ' * @param maxFontH Maximum height of text (in display units)
        ' * @param individualWidth The width of the given character/string. (in ? units)
        ' * @param spaceWidth The width of the space character. (in display units)
        ' * @param string The character to be displayed.
        ' * @param codePoints An array containing the codepoints of the given string.
        ' * @param currentFont The current font for this text position.
        ' * @param fontSizeValue The new font size.
        ' * @param fontSizeInPt The font size in pt units.
        ' */
        Public Sub New(ByVal pageRotation As Integer, ByVal pageWidthValue As Single, ByVal pageHeightValue As Single, ByVal textPositionSt As Matrix, ByVal endXValue As Single, ByVal endYValue As Single, ByVal maxFontH As Single, ByVal individualWidth As Single, ByVal spaceWidth As Single, ByVal [string] As String, ByVal codePoints() As Integer, ByVal currentFont As PDFont, ByVal fontSizeValue As Single, ByVal fontSizeInPt As Integer)
            Me.textPos = textPositionSt

            Me.endX = endXValue
            Me.endY = endYValue

            Me.rot = pageRotation
            ' make sure it is 0 to 270 and no negative numbers
            If (Me.rot < 0) Then
                rot += 360
            ElseIf (rot >= 360) Then
                rot -= 360
            End If

            Me.maxTextHeight = maxFontH
            Me.pageHeight = pageHeightValue
            Me.pageWidth = pageWidthValue

            Me.widths = New Single() {individualWidth}
            Me.widthOfSpace = spaceWidth
            Me.str = [string]
            Me.unicodeCP = codePoints
            Me.font = currentFont
            Me.fontSize = fontSizeValue
            Me.fontSizePt = fontSizeInPt
        End Sub

        '/**
        ' * Return the string of characters stored in this object.
        ' *
        ' * @return The string on the screen.
        ' */
        Public Function getCharacter() As String
            Return str
        End Function

        '/**
        ' * Return the codepoints of the characters stored in this object.
        ' *
        ' * @return an array containing all codepoints.
        ' */
        Public Function getCodePoints() As Integer()
            Return unicodeCP
        End Function

        '/**
        ' * Return the Matrix textPos stored in this object.
        ' *
        ' * @return The Matrix containing all infos of the starting textposition
        ' */
        Public Function getTextPos() As Matrix
            Return textPos
        End Function

        '/**
        ' * Return the direction/orientation of the string in this object
        ' * based on its text matrix.
        ' * @return The direction of the text (0, 90, 180, or 270)
        ' */
        Public Function getDir() As Single
            Dim a As Single = textPos.getValue(0, 0)
            Dim b As Single = textPos.getValue(0, 1)
            Dim c As Single = textPos.getValue(1, 0)
            Dim d As Single = textPos.getValue(1, 1)

            ' 12 0   left to right
            ' 0 12
            If ((a > 0) AndAlso (Math.Abs(b) < d) AndAlso (Math.Abs(c) < a) AndAlso (d > 0)) Then
                Return 0
                '// -12 0   right to left (upside down)
                ' 0 -12
            ElseIf ((a < 0) AndAlso (Math.Abs(b) < Math.Abs(d)) AndAlso (Math.Abs(c) < Math.Abs(a)) AndAlso (d < 0)) Then
                Return 180
                ' 0  12    up
                ' -12 0
            ElseIf ((Math.Abs(a) < Math.Abs(c)) AndAlso (b > 0) AndAlso (c < 0) AndAlso (Math.Abs(d) < b)) Then
                Return 90
                ' 0  -12   down
                ' 12 0
            ElseIf ((Math.Abs(a) < c) AndAlso (b < 0) AndAlso (c > 0) AndAlso (Math.Abs(d) < Math.Abs(b))) Then
                Return 270
            End If
            Return 0
        End Function

        '/**
        ' * Return the X starting coordinate of the text, adjusted by
        ' * the given rotation amount.  The rotation adjusts where the 0,0
        ' * location is relative to the text.
        ' *
        ' * @param rotation Rotation to apply (0, 90, 180, or 270).  0 will perform no adjustments.
        ' * @return X coordinate
        ' */
        Private Function getXRot(ByVal rotation As Single) As Single
            If (rotation = 0) Then
                Return textPos.getValue(2, 0)
            ElseIf (rotation = 90) Then
                Return textPos.getValue(2, 1)
            ElseIf (rotation = 180) Then
                Return pageWidth - textPos.getValue(2, 0)
            ElseIf (rotation = 270) Then
                Return pageHeight - textPos.getValue(2, 1)
            End If
            Return 0
        End Function

        '/**
        ' * This will get the page rotation adjusted x position of the character.
        ' * This is adjusted based on page rotation so that the upper
        ' * left is 0,0.
        ' *
        ' * @return The x coordinate of the character.
        ' */
        Public Function getX() As Single
            If (x = Single.NegativeInfinity) Then
                x = getXRot(rot)
            End If
            Return x
        End Function

        '/**
        ' * This will get the text direction adjusted x position of the character.
        ' * This is adjusted based on text direction so that the first character
        ' * in that direction is in the upper left at 0,0.
        ' *
        ' * @return The x coordinate of the text.
        ' */
        Public Function getXDirAdj() As Single
            Return getXRot(getDir())
        End Function

        '/**
        ' * This will get the y position of the character with 0,0 in lower left.
        ' * This will be adjusted by the given rotation.
        ' * @param rotation Rotation to apply to text to adjust the 0,0 location (0,90,180,270)
        ' *
        ' * @return The y coordinate of the text
        ' */
        Private Function getYLowerLeftRot(ByVal rotation As Single) As Single
            If (rotation = 0) Then
                Return textPos.getValue(2, 1)
            ElseIf (rotation = 90) Then
                Return pageWidth - textPos.getValue(2, 0)
            ElseIf (rotation = 180) Then
                Return pageHeight - textPos.getValue(2, 1)
            ElseIf (rotation = 270) Then
                Return textPos.getValue(2, 0)
            End If
            Return 0
        End Function

        '/**
        ' * This will get the y position of the text, adjusted so that 0,0 is upper left and
        ' * it is adjusted based on the page rotation.
        ' *
        ' * @return The adjusted y coordinate of the character.
        ' */
        Public Function getY() As Single
            If (y = Single.NegativeInfinity) Then
                If ((rot = 0) OrElse (rot = 180)) Then
                    y = pageHeight - getYLowerLeftRot(rot)
                Else
                    y = pageWidth - getYLowerLeftRot(rot)
                End If
            End If
            Return y
        End Function

        '/**
        ' * This will get the y position of the text, adjusted so that 0,0 is upper left and
        ' * it is adjusted based on the text direction.
        ' *
        ' * @return The adjusted y coordinate of the character.
        ' */
        Public Function getYDirAdj() As Single
            Dim dir As Single = getDir()
            ' some PDFBox code assumes that the 0,0 point is in upper left, not lower left
            If ((dir = 0) OrElse (dir = 180)) Then
                Return pageHeight - getYLowerLeftRot(dir)
            Else
                Return pageWidth - getYLowerLeftRot(dir)
            End If
        End Function



        '/**
        ' * Get the length or width of the text, based on a given rotation.
        ' *
        ' * @param rotation Rotation that was used to determine coordinates (0,90,180,270)
        ' * @return Width of text in display units
        ' */
        Private Function getWidthRot(ByVal rotation As Single) As Single
            If ((rotation = 90) OrElse (rotation = 270)) Then
                Return Math.Abs(endY - textPos.getYPosition())
            Else
                Return Math.Abs(endX - textPos.getXPosition())
            End If
        End Function

        '/**
        ' * This will get the width of the string when page rotation adjusted coordinates are used.
        ' *
        ' * @return The width of the text in display units.
        ' */
        Public Function getWidth() As Single
            Return getWidthRot(rot)
        End Function

        '/**
        ' * This will get the width of the string when text direction adjusted coordinates are used.
        ' *
        ' * @return The width of the text in display units.
        ' */
        Public Function getWidthDirAdj() As Single
            Return getWidthRot(getDir())
        End Function

        '/**
        ' * This will get the maximum height of all characters in this string.
        ' *
        ' * @return The maximum height of all characters in this string.
        ' */
        Public Function getHeight() As Single
            Return maxTextHeight
        End Function

        '/**
        ' * This will get the maximum height of all characters in this string.
        ' *
        ' * @return The maximum height of all characters in this string.
        ' */
        Public Function getHeightDir() As Single
            ' this is not really a rotation-dependent calculation, but this is defined for symmetry.
            Return maxTextHeight
        End Function

        '/**
        ' * This will get the font size that this object is
        ' * suppose to be drawn at.
        ' *
        ' * @return The font size.
        ' */
        Public Function getFontSize() As Single
            Return fontSize
        End Function

        '/**
        ' * This will get the font size in pt.
        ' * To get this size we have to multiply the pdf-fontsize and the scaling from the textmatrix
        ' *
        ' * @return The font size in pt.
        ' */
        Public Function getFontSizeInPt() As Single
            Return fontSizePt
        End Function

        '/**
        ' * This will get the font for the text being drawn.
        ' *
        ' * @return The font size.
        ' */
        Public Function getFont() As PDFont
            Return font
        End Function

        '/**
        ' * This will get the current word spacing.
        ' *
        ' * @return The current word spacing.
        ' */
        <Obsolete()> _
        Public Function getWordSpacing() As Single
            Return wordSpacing
        End Function

        '/**
        ' * This will get the width of a space character.  This is useful for some
        ' * algorithms such as the text stripper, that need to know the width of a
        ' * space character.
        ' *
        ' * @return The width of a space character.
        ' */
        Public Function getWidthOfSpace() As Single
            Return widthOfSpace
        End Function

        '/**
        ' * @return Returns the xScale.
        ' */
        Public Function getXScale() As Single
            Return textPos.getXScale()
        End Function

        '/**
        ' * @return Returns the yScale.
        ' */
        Public Function getYScale() As Single
            Return textPos.getYScale()
        End Function

        '/**
        ' * Get the widths of each individual character.
        ' *
        ' * @return An array that is the same length as the length of the string.
        ' */
        Public Function getIndividualWidths() As Single()
            Return widths
        End Function

        '/**
        ' * Show the string data for this text position.
        ' *
        ' * @return A human readable form of this object.
        ' */
        Public Overrides Function toString() As String
            Return getCharacter()
        End Function


        '/**
        ' * Determine if this TextPosition logically contains
        ' * another (i.e. they overlap and should be rendered on top
        ' * of each other).
        ' * @param tp2 The other TestPosition to compare against
        ' *
        ' * @return True if tp2 is contained in the bounding box of this text.
        ' */
        Public Function contains(ByVal tp2 As TextPosition) As Boolean
            Dim thisXstart As Double = getXDirAdj()
            Dim thisXend As Double = getXDirAdj() + getWidthDirAdj()

            Dim tp2Xstart As Double = tp2.getXDirAdj()
            Dim tp2Xend As Double = tp2.getXDirAdj() + tp2.getWidthDirAdj()

            '/*
            ' * No X overlap at all so return as soon as possible.
            ' */
            If (tp2Xend <= thisXstart OrElse tp2Xstart >= thisXend) Then
                Return False
            End If
            '/*
            ' * No Y overlap at all so return as soon as possible.
            ' * Note: 0.0 is in the upper left and y-coordinate is
            ' * top of TextPosition
            ' */
            If ((tp2.getYDirAdj() + tp2.getHeightDir() < getYDirAdj()) OrElse (tp2.getYDirAdj() > getYDirAdj() + getHeightDir())) Then
                Return False
                '/* We're going to calculate the percentage of overlap. If its less
                ' * than a 15% x-coordinate overlap then we'll return false because its negligible.
                ' * .15 was determined by trial and error in the regression test files.
                ' */
            ElseIf ((tp2Xstart > thisXstart) AndAlso (tp2Xend > thisXend)) Then
                Dim overlap As Double = thisXend - tp2Xstart
                Dim overlapPercent As Double = overlap / getWidthDirAdj()
                Return (overlapPercent > 0.15)
            ElseIf ((tp2Xstart < thisXstart) AndAlso (tp2Xend < thisXend)) Then
                Dim overlap As Double = tp2Xend - thisXstart
                Dim overlapPercent As Double = overlap / getWidthDirAdj()
                Return (overlapPercent > 0.15)
            End If
            Return True
        End Function

        '/**
        ' * Merge a single character TextPosition into the current object.
        ' * This is to be used only for cases where we have a diacritic that
        ' * overlaps an existing TextPosition.  In a graphical display, we could
        ' * overlay them, but for text extraction we need to merge them. Use the
        ' * contains() method to test if two objects overlap.
        ' *
        ' * @param diacritic TextPosition to merge into the current TextPosition.
        ' * @param normalize Instance of TextNormalize class to be used to normalize diacritic
        ' */
        Public Sub mergeDiacritic(ByVal diacritic As TextPosition, ByVal normalize As TextNormalize)
            If (diacritic.getCharacter().Length() > 1) Then
                Return
            End If

            Dim diacXStart As Single = diacritic.getXDirAdj()
            Dim diacXEnd As Single = diacXStart + diacritic.widths(0)

            Dim currCharXStart As Single = getXDirAdj()

            Dim strLen As Integer = str.Length()
            Dim wasAdded As Boolean = False

            For i As Integer = 0 To strLen - 1
                If (wasAdded) Then Exit For
                Dim currCharXEnd As Single = currCharXStart + widths(i)

                '/*
                ' * This is the case where there is an overlap of the diacritic character with
                ' * the current character and the previous character. If no previous character,
                ' * just append the diacritic after the current one.
                ' */
                If (diacXStart < currCharXStart AndAlso diacXEnd <= currCharXEnd) Then
                    If (i = 0) Then
                        insertDiacritic(i, diacritic, normalize)
                    Else
                        Dim distanceOverlapping1 As Single = diacXEnd - currCharXStart
                        Dim percentage1 As Single = distanceOverlapping1 / widths(i)

                        Dim distanceOverlapping2 As Single = currCharXStart - diacXStart
                        Dim percentage2 As Single = distanceOverlapping2 / widths(i - 1)

                        If (percentage1 >= percentage2) Then
                            insertDiacritic(i, diacritic, normalize)
                        Else
                            insertDiacritic(i - 1, diacritic, normalize)
                        End If
                    End If
                    wasAdded = True
                    'diacritic completely covers this character and therefore we assume that
                    'this is the character the diacritic belongs to
                ElseIf (diacXStart < currCharXStart AndAlso diacXEnd > currCharXEnd) Then
                    insertDiacritic(i, diacritic, normalize)
                    wasAdded = True
                    'Otherwise, The diacritic modifies this character because its completely
                    'contained by the character width
                ElseIf (diacXStart >= currCharXStart AndAlso diacXEnd <= currCharXEnd) Then
                    insertDiacritic(i, diacritic, normalize)
                    wasAdded = True
                    ' Last character in the TextPosition so we add diacritic to the end
                ElseIf (diacXStart >= currCharXStart AndAlso diacXEnd > currCharXEnd AndAlso i = (strLen - 1)) Then
                    insertDiacritic(i, diacritic, normalize)
                    wasAdded = True
                End If
                ' Couldn't find anything useful so we go to the next character in the TextPosition
                currCharXStart += widths(i)
            Next
        End Sub

        '/**
        ' * Inserts the diacritic TextPosition to the str of this TextPosition
        ' * and updates the widths array to include the extra character width.
        ' * @param i current character
        ' * @param diacritic The diacritic TextPosition
        ' * @param normalize Instance of TextNormalize class to be used to normalize diacritic
        ' */
        Private Sub insertDiacritic(ByVal i As Integer, ByVal diacritic As TextPosition, ByVal normalize As TextNormalize)
            '/* we add the diacritic to the right or left of the character
            ' * depending on the direction of the character.  Note that this
            ' * is only required because the text is currently stored in
            ' * presentation order and not in logical order.
            ' */
            Dim dir As Integer = NChar.getDirectionality(str.Chars(i))
            Dim buf As StringBuffer = New StringBuffer()

            buf.append(str.Substring(0, i))

            Dim widths2 As Single() = Array.CreateInstance(GetType(Single), widths.Length + 1)
            Array.Copy(widths, 0, widths2, 0, i)

            If ((dir = NChar.DIRECTIONALITY_RIGHT_TO_LEFT) OrElse (dir = NChar.DIRECTIONALITY_RIGHT_TO_LEFT_ARABIC) OrElse (dir = NChar.DIRECTIONALITY_RIGHT_TO_LEFT_EMBEDDING) OrElse (dir = NChar.DIRECTIONALITY_RIGHT_TO_LEFT_OVERRIDE)) Then
                buf.append(normalize.normalizeDiac(diacritic.getCharacter()))
                widths2(i) = 0
                buf.append(str.Chars(i))
                widths2(i + 1) = widths(i)
            Else
                buf.append(str.Chars(i))
                widths2(i) = widths(i)
                buf.append(normalize.normalizeDiac(diacritic.getCharacter()))
                widths2(i + 1) = 0
            End If

            ' Get the rest of the string
            buf.append(str.Substring(i + 1, str.Length()))
            Array.Copy(widths, i + 1, widths2, i + 2, widths.Length - i - 1)

            str = buf.ToString()
            widths = widths2
        End Sub

        '/**
        ' *
        ' * @return True if the current character is a diacritic char.
        ' */
        Public Function isDiacritic() As Boolean
            Dim cText As String = Me.getCharacter()
            If (cText.Length() <> 1) Then
                Return False
            End If
            Dim type As Integer = NChar.GetCharType(cText.Chars(0))
            Return (type = NChar.NON_SPACING_MARK OrElse type = NChar.MODIFIER_SYMBOL OrElse type = NChar.MODIFIER_LETTER)
        End Function

    End Class

End Namespace