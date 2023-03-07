Imports System.Drawing
Imports System.IO
Imports FinSeA.Drawings
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.font

    '/**
    ' * This is implementation of the Type0 Font.
    ' * See <a href="https://issues.apache.org/jira/browse/PDFBOX-605">PDFBOX-605</a>
    ' * for the related improvement issue.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.9 $
    ' */
    Public Class PDType0Font
        Inherits PDSimpleFont

        '/**
        ' * Log instance.
        ' */
        'private static final Log LOG = LogFactory.getLog(PDType0Font.class);

        Private descendantFontArray As COSArray
        Private descendantFont As PDFont
        Private descendantFontDictionary As COSDictionary
        Private awtFont As JFont

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New()
            font.setItem(COSName.SUBTYPE, COSName.TYPE0)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param fontDictionary The font dictionary according to the PDF specification.
        ' */
        Public Sub New(ByVal fontDictionary As COSDictionary)
            MyBase.New(fontDictionary)
            descendantFontDictionary = getDescendantFonts().getObject(0)
            If (descendantFontDictionary IsNot Nothing) Then
                Try
                    descendantFont = PDFontFactory.createFont(descendantFontDictionary)
                Catch exception As IOException
                    LOG.error("Error while creating the descendant font!")
                End Try
            End If
        End Sub


        Public Overrides Function getawtFont() As JFont ' throws IOException
            If (awtFont Is Nothing) Then
                If (descendantFont IsNot Nothing) Then
                    awtFont = DirectCast(descendantFont, PDSimpleFont).getawtFont()
                    If (awtFont IsNot Nothing) Then
                        setIsFontSubstituted(DirectCast(descendantFont, PDSimpleFont).isFontSubstituted())
                        '/*
                        ' * Fix Oracle JVM Crashes.
                        ' * Tested with Oracle JRE 6.0_45-b06 and 7.0_21-b11
                        ' */
                        awtFont.canDisplay(1)
                    End If
                End If
                If (awtFont Is Nothing) Then
                    awtFont = FontManager.getStandardFont()
                    LOG.info("Using font " & awtFont.getName() & " instead of " & descendantFont.getFontDescriptor().getFontName())
                    setIsFontSubstituted(True)
                End If
            End If
            Return awtFont
        End Function

        '/**
        ' * This will get the fonts bounding box.
        ' *
        ' * @return The fonts bounding box.
        ' *
        ' * @throws IOException If there is an error getting the bounding box.
        ' */
        Public Overrides Function getFontBoundingBox() As PDRectangle 'throws IOException
            Throw New NotImplementedException("getFontBoundingBox")
        End Function

        '/**
        ' * This will get the font width for a character.
        ' *
        ' * @param c The character code to get the width for.
        ' * @param offset The offset into the array.
        ' * @param length The length of the data.
        ' *
        ' * @return The width is in 1000 unit of text space, ie 333 or 777
        ' *
        ' * @throws IOException If an error occurs while parsing.
        ' */
        Public Overrides Function getFontWidth(ByVal c() As Byte, ByVal offset As Integer, ByVal length As Integer) As Single ' throws IOException
            Return descendantFont.getFontWidth(c, offset, length)
        End Function

        '/**
        ' * This will get the font height for a character.
        ' *
        ' * @param c The character code to get the height for.
        ' * @param offset The offset into the array.
        ' * @param length The length of the data.
        ' *
        ' * @return The width is in 1000 unit of text space, ie 333 or 777
        ' *
        ' * @throws IOException If an error occurs while parsing.
        ' */
        Public Overrides Function getFontHeight(ByVal c() As Byte, ByVal offset As Integer, ByVal length As Integer) As Single ' throws IOException
            Return descendantFont.getFontHeight(c, offset, length)
        End Function

        '/**
        ' * This will get the average font width for all characters.
        ' *
        ' * @return The width is in 1000 unit of text space, ie 333 or 777
        ' *
        ' * @throws IOException If an error occurs while parsing.
        ' */
        Public Overrides Function getAverageFontWidth() As Single ' throws IOException
            Return descendantFont.getAverageFontWidth()
        End Function

        Private Function getDescendantFonts() As COSArray
            If (descendantFontArray Is Nothing) Then
                descendantFontArray = font.getDictionaryObject(COSName.DESCENDANT_FONTS)
            End If
            Return descendantFontArray
        End Function

        Public Overrides Function getFontWidth(ByVal charCode As Integer) As Single
            Return descendantFont.getFontWidth(charCode)
        End Function

        Public Overrides Function encode(ByVal c() As Byte, ByVal offset As Integer, ByVal length As Integer) As String ' throws IOException
            Dim retval As String = ""
            If (hasToUnicode()) Then
                retval = MyBase.encode(c, offset, length)
            End If

            If (retval Is Nothing) Then
                Dim result As Integer = cmap.lookupCID(c, offset, length)
                If (result <> -1) Then
                    retval = descendantFont.cmapEncoding(result, 2, True, Nothing)
                End If
            End If
            Return retval
        End Function

        '/**
        ' *
        ' * Provides the descendant font.
        ' * @return the descendant font.
        ' *
        ' */
        Protected Friend Function getDescendantFont() As PDFont
            Return descendantFont
        End Function


    End Class

End Namespace