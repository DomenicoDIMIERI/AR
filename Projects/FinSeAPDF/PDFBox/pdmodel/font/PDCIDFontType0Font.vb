Imports System.Drawing
Imports System.IO
Imports FinSeA.Drawings

Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.font


    '/**
    ' * This is implementation of the CIDFontType0 Font.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.6 $
    ' */
    Public Class PDCIDFontType0Font
        Inherits PDCIDFont

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New()
            font.setItem(COSName.SUBTYPE, COSName.CID_FONT_TYPE0)
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
        ' * Returns the AWT font that corresponds with this CIDFontType0 font.
        ' * By default we try to look up a system font with the same name. If that
        ' * fails and the font file is embedded in the PDF document, we try to
        ' * generate the AWT font using the {@link PDType1CFont} class. Ideally
        ' * the embedded font would be used always if available, but since the
        ' * code doesn't work correctly for all fonts yet we opt to use the
        ' * system font by default.
        ' *
        ' * @return AWT font, or <code>null</code> if not available
        ' */
        Public Overrides Function getawtFont() As JFont 'Font  throws IOException
            Dim fd As PDFontDescriptor = getFontDescriptor()
            Dim awtFont As JFont = FontManager.getAwtFont(fd.getFontName())

            If (awtFont Is Nothing AndAlso TypeOf (fd) Is PDFontDescriptorDictionary) Then
                Dim fdd As PDFontDescriptorDictionary = fd
                If (fdd.getFontFile3() IsNot Nothing) Then
                    ' Create a font with the embedded data
                    ' TODO: This still doesn't work right for
                    ' some embedded fonts
                    Dim tmp As New PDType1CFont(font)
                    awtFont = tmp.getawtFont()
                End If
            End If

            Return awtFont
        End Function


    End Class

End Namespace