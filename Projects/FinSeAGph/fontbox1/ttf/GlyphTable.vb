Namespace org.fontbox.ttf

    '/**
    ' * A table in a true type font.
    ' * 
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class GlyphTable
        Inherits TTFTable

        ''' <summary>
        ''' Tag to identify this table.
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
            Dim post As PostScriptTable = ttf.getPostScript()
            Dim offsets() As Long = loc.getOffsets()
            Dim numGlyphs As Integer = maxp.getNumGlyphs()
            glyphs = Array.CreateInstance(GetType(GlyphData), numGlyphs)
            Dim glyphNames() As String = post.getGlyphNames()
            For i As Integer = 0 To numGlyphs - 1 - 1
                Dim glyph As New GlyphData()
                data.seek(getOffset() + offsets(i))
                glyph.initData(ttf, data)
                glyphs(i) = glyph
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
            Me.glyphs = glyphsValue
        End Sub

    End Class

End Namespace
