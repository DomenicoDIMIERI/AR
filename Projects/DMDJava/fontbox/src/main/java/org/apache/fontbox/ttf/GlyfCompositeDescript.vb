Imports System.IO
Imports FinSeA.java

Namespace org.apache.fontbox.ttf

    '/**
    ' * Glyph description for composite glyphs. Composite glyphs are made up of one or more simple glyphs, usually with some
    ' * sort of transformation applied to each.
    ' * 
    ' * This class is based on code from Apache Batik a subproject of Apache XMLGraphics. see
    ' * http://xmlgraphics.apache.org/batik/ for further details.
    ' */
    Public Class GlyfCompositeDescript
        Inherits GlyfDescript

        Private components As List(Of GlyfCompositeComp) = New ArrayList(Of GlyfCompositeComp)()
        Private glyphs As GlyphData() = Nothing
        Private beingResolved As Boolean = False
        Private resolved As Boolean = False

        '/**
        ' * Constructor.
        ' * 
        ' * @param bais the stream to be read
        ' * @param glyphTable the Glyphtable containing all glyphs
        ' * @throws IOException is thrown if something went wrong
        ' */
        Public Sub New(ByVal bais As TTFDataStream, ByVal glyphTable As GlyphTable)
            MyBase.New(-1, bais)

            glyphs = glyphTable.getGlyphs()

            ' Get all of the composite components
            Dim comp As GlyfCompositeComp
            Do
                comp = New GlyfCompositeComp(bais)
                components.add(comp)
            Loop While ((comp.getFlags() And GlyfCompositeComp.MORE_COMPONENTS) = GlyfCompositeComp.MORE_COMPONENTS)

            ' Are there hinting instructions to read?
            If ((comp.getFlags() And GlyfCompositeComp.WE_HAVE_INSTRUCTIONS) = GlyfCompositeComp.WE_HAVE_INSTRUCTIONS) Then
                readInstructions(bais, (bais.readUnsignedShort()))
            End If
        End Sub

        Public Overrides Sub resolve()
            If (resolved) Then
                Return
            End If
            If (beingResolved) Then
                LOG.error("Circular reference in GlyfCompositeDesc")
                Return
            End If
            beingResolved = True

            Dim firstIndex As Integer = 0
            Dim firstContour As Integer = 0

            Dim i As Iterator(Of GlyfCompositeComp) = components.iterator()
            While (i.hasNext())
                Dim comp As GlyfCompositeComp = i.next()
                comp.setFirstIndex(firstIndex)
                comp.setFirstContour(firstContour)

                Dim desc As GlyphDescription
                desc = getGlypDescription(comp.getGlyphIndex())
                If (desc IsNot Nothing) Then
                    desc.resolve()
                    firstIndex += desc.getPointCount()
                    firstContour += desc.getContourCount()
                End If
            End While
            resolved = True
            beingResolved = False
        End Sub

        Public Overrides Function getEndPtOfContours(ByVal i As Integer) As Integer
            Dim c As GlyfCompositeComp = getCompositeCompEndPt(i)
            If (c IsNot Nothing) Then
                Dim gd As GlyphDescription = getGlypDescription(c.getGlyphIndex())
                Return gd.getEndPtOfContours(i - c.getFirstContour()) + c.getFirstIndex()
            End If
            Return 0
        End Function

        Public Overrides Function getFlags(ByVal i As Integer) As Byte
            Dim c As GlyfCompositeComp = getCompositeComp(i)
            If (c IsNot Nothing) Then
                Dim gd As GlyphDescription = getGlypDescription(c.getGlyphIndex())
                Return gd.getFlags(i - c.getFirstIndex())
            End If
            Return 0
        End Function


        Public Overrides Function getXCoordinate(ByVal i As Integer) As Short
            Dim c As GlyfCompositeComp = getCompositeComp(i)
            If (c IsNot Nothing) Then
                Dim gd As GlyphDescription = getGlypDescription(c.getGlyphIndex())
                Dim n As Integer = i - c.getFirstIndex()
                Dim x As Integer = gd.getXCoordinate(n)
                Dim y As Integer = gd.getYCoordinate(n)
                Dim x1 As Short = c.scaleX(x, y)
                x1 += c.getXTranslate()
                Return x1
            End If
            Return 0
        End Function

        Public Overrides Function getYCoordinate(ByVal i As Integer) As Short
            Dim c As GlyfCompositeComp = getCompositeComp(i)
            If (c IsNot Nothing) Then
                Dim gd As GlyphDescription = getGlypDescription(c.getGlyphIndex())
                Dim n As Integer = i - c.getFirstIndex()
                Dim x As Integer = gd.getXCoordinate(n)
                Dim y As Integer = gd.getYCoordinate(n)
                Dim y1 As Short = c.scaleY(x, y)
                y1 += c.getYTranslate()
                Return y1
            End If
            Return 0
        End Function

        Public Overrides Function isComposite() As Boolean
            Return True
        End Function


        Public Overrides Function getPointCount() As Integer
            If (Not resolved) Then
                LOG.error("getPointCount called on unresolved GlyfCompositeDescript")
            End If
            Dim c As GlyfCompositeComp = components.get(components.size() - 1)
            Return c.getFirstIndex() + getGlypDescription(c.getGlyphIndex()).getPointCount()
        End Function

        Public Overrides Function getContourCount() As Integer
            If (Not resolved) Then
                LOG.error("getContourCount called on unresolved GlyfCompositeDescript")
            End If
            Dim c As GlyfCompositeComp = components.get(components.size() - 1)
            Return c.getFirstContour() + getGlypDescription(c.getGlyphIndex()).getContourCount()
        End Function

        Public Function getComponentCount() As Integer
            Return components.size()
        End Function

        Private Function getCompositeComp(ByVal i As Integer) As GlyfCompositeComp
            Dim c As GlyfCompositeComp
            For n As Integer = 0 To components.size() - 1
                c = components.get(n)
                Dim gd As GlyphDescription = getGlypDescription(c.getGlyphIndex())
                If (c.getFirstIndex() <= i AndAlso i < (c.getFirstIndex() + gd.getPointCount())) Then
                    Return c
                End If
            Next
            Return Nothing
        End Function

        Private Function getCompositeCompEndPt(ByVal i As Integer) As GlyfCompositeComp
            Dim c As GlyfCompositeComp
            For j As Integer = 0 To components.size() - 1
                c = components.get(j)
                Dim gd As GlyphDescription = getGlypDescription(c.getGlyphIndex())
                If (c.getFirstContour() <= i AndAlso i < (c.getFirstContour() + gd.getContourCount())) Then
                    Return c
                End If
            Next
            Return Nothing
        End Function

        Private Function getGlypDescription(ByVal index As Integer) As GlyphDescription
            If (glyphs IsNot Nothing AndAlso index < glyphs.Length) Then
                Dim glyph As GlyphData = glyphs(index)
                If (glyph IsNot Nothing) Then
                    Return glyph.getDescription()
                End If
            End If
            Return Nothing
        End Function

    End Class

End Namespace
