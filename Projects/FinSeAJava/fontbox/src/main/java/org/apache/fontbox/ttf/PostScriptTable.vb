Imports FinSeA.org.apache.fontbox.encoding

Namespace org.apache.fontbox.ttf

    '/**
    ' * A table in a true type font.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * 
    ' */
    Public Class PostScriptTable
        Inherits TTFTable

        Private formatType As Single
        Private italicAngle As Single
        Private underlinePosition As Short
        Private underlineThickness As Short
        Private isFixedPitch As Long
        Private minMemType42 As Long
        Private maxMemType42 As Long
        Private mimMemType1 As Long
        Private maxMemType1 As Long
        Private glyphNames() As String = Nothing

        ''' <summary>
        ''' A tag that identifies Me table type.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TAG = "post"

        '/**
        ' * This will read the required data from the stream.
        ' * 
        ' * @param ttf The font that is being read.
        ' * @param data The stream to read the data from.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Sub initData(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            Dim maxp As MaximumProfileTable = ttf.getMaximumProfile()
            formatType = data.read32Fixed()
            italicAngle = data.read32Fixed()
            underlinePosition = data.readSignedShort()
            underlineThickness = data.readSignedShort()
            isFixedPitch = data.readUnsignedInt()
            minMemType42 = data.readUnsignedInt()
            maxMemType42 = data.readUnsignedInt()
            mimMemType1 = data.readUnsignedInt()
            maxMemType1 = data.readUnsignedInt()

            If (formatType = 1.0F) Then
                '/*
                ' * This TrueType font file contains exactly the 258 glyphs in the standard Macintosh TrueType.
                ' */
                glyphNames = Array.CreateInstance(GetType(String), encoding.Encoding.NUMBER_OF_MAC_GLYPHS)
                Array.Copy(encoding.Encoding.MAC_GLYPH_NAMES, 0, glyphNames, 0, encoding.Encoding.NUMBER_OF_MAC_GLYPHS)
            ElseIf (formatType = 2.0F) Then
                Dim numGlyphs As Integer = data.readUnsignedShort()
                Dim glyphNameIndex() As Integer = Array.CreateInstance(GetType(Integer), numGlyphs)
                glyphNames = Array.CreateInstance(GetType(String), numGlyphs)
                Dim maxIndex As Integer = Integer.MinValue
                For i As Integer = 0 To numGlyphs - 1
                    Dim index As Integer = data.readUnsignedShort()
                    glyphNameIndex(i) = index
                    ' PDFBOX-808: Index numbers between 32768 and 65535 are
                    ' reserved for future use, so we should just ignore them
                    If (index <= 32767) Then
                        maxIndex = Math.Max(maxIndex, index)
                    End If
                Next
                Dim nameArray() As String = Nothing
                If (maxIndex >= encoding.Encoding.NUMBER_OF_MAC_GLYPHS) Then
                    nameArray = Array.CreateInstance(GetType(String), maxIndex - encoding.Encoding.NUMBER_OF_MAC_GLYPHS + 1)
                    For i As Integer = 0 To maxIndex - encoding.Encoding.NUMBER_OF_MAC_GLYPHS + 1 - 1
                        Dim numberOfChars As Integer = data.readUnsignedByte()
                        nameArray(i) = data.readString(numberOfChars)
                    Next
                End If
                For i As Integer = 0 To numGlyphs - 1
                    Dim index As Integer = glyphNameIndex(i)
                    If (index < encoding.Encoding.NUMBER_OF_MAC_GLYPHS) Then
                        glyphNames(i) = encoding.Encoding.MAC_GLYPH_NAMES(index)
                    ElseIf (index >= encoding.Encoding.NUMBER_OF_MAC_GLYPHS AndAlso index <= 32767) Then
                        glyphNames(i) = nameArray(index - encoding.Encoding.NUMBER_OF_MAC_GLYPHS)
                    Else
                        ' PDFBOX-808: Index numbers between 32768 and 65535 are
                        ' reserved for future use, so we should just ignore them
                        glyphNames(i) = ".undefined"
                    End If
                Next
            ElseIf (formatType = 2.5F) Then
                Dim glyphNameIndex() As Integer = Array.CreateInstance(GetType(Integer), maxp.getNumGlyphs())
                For i As Integer = 0 To glyphNameIndex.Length - 1
                    Dim offset As Integer = data.readSignedByte()
                    glyphNameIndex(i) = i + 1 + offset
                Next
                glyphNames = Array.CreateInstance(GetType(String), glyphNameIndex.Length)
                For i As Integer = 0 To glyphNames.Length - 1
                    Dim name As String = encoding.Encoding.MAC_GLYPH_NAMES(glyphNameIndex(i))
                    If (name <> vbNullString) Then
                        glyphNames(i) = name
                    End If
                Next
            ElseIf (formatType = 3.0F) Then
                ' no postscript information is provided.
            End If
        End Sub

        '/**
        ' * @return Returns the formatType.
        ' */
        Public Function getFormatType() As Single
            Return formatType
        End Function

        '/**
        ' * @param formatTypeValue The formatType to set.
        ' */
        Public Sub setFormatType(ByVal formatTypeValue As Single)
            Me.formatType = formatTypeValue
        End Sub

        '/**
        ' * @return Returns the isFixedPitch.
        ' */
        Public Function getIsFixedPitch() As Long
            Return isFixedPitch
        End Function

        '/**
        ' * @param isFixedPitchValue The isFixedPitch to set.
        ' */
        Public Sub setIsFixedPitch(ByVal isFixedPitchValue As Long)
            Me.isFixedPitch = isFixedPitchValue
        End Sub

        '/**
        ' * @return Returns the italicAngle.
        ' */
        Public Function getItalicAngle() As Single
            Return italicAngle
        End Function

        '/**
        ' * @param italicAngleValue The italicAngle to set.
        ' */
        Public Sub setItalicAngle(ByVal italicAngleValue As Single)
            Me.italicAngle = italicAngleValue
        End Sub

        '/**
        ' * @return Returns the maxMemType1.
        ' */
        Public Function getMaxMemType1() As Long
            Return maxMemType1
        End Function

        '/**
        ' * @param maxMemType1Value The maxMemType1 to set.
        ' */
        Public Sub setMaxMemType1(ByVal maxMemType1Value As Long)
            Me.maxMemType1 = maxMemType1Value
        End Sub

        '/**
        ' * @return Returns the maxMemType42.
        ' */
        Public Function getMaxMemType42() As Long
            Return maxMemType42
        End Function

        '/**
        ' * @param maxMemType42Value The maxMemType42 to set.
        ' */
        Public Sub setMaxMemType42(ByVal maxMemType42Value As Long)
            Me.maxMemType42 = maxMemType42Value
        End Sub

        '/**
        ' * @return Returns the mimMemType1.
        ' */
        Public Function getMimMemType1() As Long
            Return mimMemType1
        End Function

        '/**
        ' * @param mimMemType1Value The mimMemType1 to set.
        ' */
        Public Sub setMimMemType1(ByVal mimMemType1Value As Long)
            Me.mimMemType1 = mimMemType1Value
        End Sub

        '/**
        ' * @return Returns the minMemType42.
        ' */
        Public Function getMinMemType42() As Long
            Return minMemType42
        End Function

        '/**
        ' * @param minMemType42Value The minMemType42 to set.
        ' */
        Public Sub setMinMemType42(ByVal minMemType42Value As Long)
            Me.minMemType42 = minMemType42Value
        End Sub

        '/**
        ' * @return Returns the underlinePosition.
        ' */
        Public Function getUnderlinePosition() As Short
            Return underlinePosition
        End Function

        '/**
        ' * @param underlinePositionValue The underlinePosition to set.
        ' */
        Public Sub setUnderlinePosition(ByVal underlinePositionValue As Short)
            Me.underlinePosition = underlinePositionValue
        End Sub

        '/**
        ' * @return Returns the underlineThickness.
        ' */
        Public Function getUnderlineThickness() As Short
            Return underlineThickness
        End Function

        '/**
        ' * @param underlineThicknessValue The underlineThickness to set.
        ' */
        Public Sub setUnderlineThickness(ByVal underlineThicknessValue As Short)
            Me.underlineThickness = underlineThicknessValue
        End Sub

        '/**
        ' * @return Returns the glyphNames.
        ' */
        Public Function getGlyphNames() As String()
            Return glyphNames
        End Function

        '/**
        ' * @param glyphNamesValue The glyphNames to set.
        ' */
        Public Sub setGlyphNames(ByVal glyphNamesValue() As String)
            Me.glyphNames = glyphNamesValue
        End Sub

    End Class

End Namespace
