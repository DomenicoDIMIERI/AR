Imports System.IO

Namespace org.apache.fontbox.ttf

    '/**
    ' * A table in a true type font.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * 
    ' */
    Public Class GlyphTable
        Inherits TTFTable

        ''' <summary>
        ''' Tag to identify Me table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TAG = "glyf"

        Private glyphs As GlyphData()

        '/**
        ' * This will read the required data from the stream.
        ' * 
        ' * @param ttf The font that is being read.
        ' * @param data The stream to read the data from.
        ' * @throws IOException If there is an error reading the data.
        ' */
        Public Overrides Sub initData(ByVal ttf As TrueTypeFont, ByVal data As TTFDataStream)
            Dim maxp As MaximumProfileTable = ttf.getMaximumProfile()
            Dim loc As IndexToLocationTable = ttf.getIndexToLocation()
            ' the glyph offsets
            Dim offsets() As Long = loc.getOffsets()
            ' number of glyphs
            Dim numGlyphs As Integer = maxp.getNumGlyphs()
            ' the end of the glyph table
            Dim endOfGlyphs As Long = offsets(numGlyphs)
            Dim offset As Long = getOffset()
            glyphs = Array.CreateInstance(GetType(GlyphData), numGlyphs)
            For i As Integer = 0 To numGlyphs - 1
                ' end of glyphs reached?
                If (endOfGlyphs = offsets(i)) Then
                    Exit For
                End If
                ' the current glyph isn't defined
                ' if the next offset equals the current index
                If (offsets(i) = offsets(i + 1)) Then
                    Continue For
                End If
                glyphs(i) = New GlyphData()
                data.seek(offset + offsets(i))
                glyphs(i).initData(ttf, data)
            Next
            For i As Integer = 0 To numGlyphs - 1
                Dim glyph As GlyphData = glyphs(i)
                ' resolve composite glyphs
                If (glyph IsNot Nothing AndAlso glyph.getDescription().isComposite()) Then
                    glyph.getDescription().resolve()
                End If
            Next
        End Sub

        '/**
        ' * @return Returns the glyphs.
        ' */
        Public Function getGlyphs() As GlyphData()
            Return glyphs
        End Function

        '/**
        ' * @param glyphsValue The glyphs to set.
        ' */
        Public Sub setGlyphs(ByVal glyphsValue() As GlyphData)
            glyphs = glyphsValue
        End Sub

    End Class

End Namespace
