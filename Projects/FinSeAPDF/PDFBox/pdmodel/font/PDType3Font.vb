Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Imports System.Drawing
Imports System.Drawing.Image
Imports FinSeA.Drawings

Imports System.IO

Namespace org.apache.pdfbox.pdmodel.font

    '/**
    ' * This is implementation of the Type3 Font.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.8 $
    ' */
    Public Class PDType3Font
        Inherits PDSimpleFont

        'A map of character code to java.awt.Image for the glyph
        Private images As Map(Of NChar, BufferedImage) = New HashMap(Of NChar, BufferedImage)

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New()
            font.setItem(COSName.SUBTYPE, COSName.TYPE3)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param fontDictionary The font dictionary according to the PDF specification.
        ' */
        Public Sub New(ByVal fontDictionary As COSDictionary)
            MyBase.New(fontDictionary)
        End Sub

        '/**
        ' * Type3 fonts have their glyphs defined as a content stream.  This
        ' * will create the image that represents that character
        ' *
        ' * @throws IOException If there is an error creating the image.
        ' */
        Private Function createImageIfNecessary(ByVal character As Char) As BufferedImage
            Dim c As NChar = New NChar(character)
            Dim retval As BufferedImage = images.get(c)
            If (retval Is Nothing) Then
                Dim charProcs As COSDictionary = font.getDictionaryObject(COSName.CHAR_PROCS)
                Dim stream As COSStream = charProcs.getDictionaryObject(COSName.getPDFName("" & character))
                If (stream IsNot Nothing) Then
                    Dim parser As Type3StreamParser = New Type3StreamParser()
                    retval = parser.createImage(stream)
                    images.put(c, retval)
                Else
                    'stream should not be null!!
                End If
            End If
            Return retval
        End Function

        Public Overrides Sub drawString(ByVal [string] As String, ByVal codePoints() As Integer, ByVal g As Graphics2D, ByVal fontSize As Single, ByVal at As AffineTransform, ByVal x As Single, ByVal y As Single)
            For i As Integer = 0 To [string].Length() - 1
                'todo need to use image observers and such
                Dim c As Char = [string].Chars(i)
                Dim image As BufferedImage = createImageIfNecessary(c)
                If (image IsNot Nothing) Then
                    Dim newWidth As Integer = (0.12 * image.getWidth)
                    Dim newHeight As Integer = (0.12 * image.getHeight)
                    If (newWidth > 0 AndAlso newHeight > 0) Then
                        image = image.getScaledInstance(newWidth, newHeight, image.SCALE_SMOOTH)
                        g.DrawImage(image, x, y, Nothing)
                        x += newWidth
                    End If
                End If
            Next
        End Sub

        '/**
        ' * Set the font matrix for this type3 font.
        ' *
        ' * @param matrix The font matrix for this type3 font.
        ' */
        Public Sub setFontMatrix(ByVal matrix As PDMatrix)
            font.setItem(COSName.FONT_MATRIX, matrix)
        End Sub

    End Class

End Namespace
