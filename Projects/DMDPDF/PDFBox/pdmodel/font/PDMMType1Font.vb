Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.font

    '/**
    ' * This is implementation of the Multiple Master Type1 Font.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDMMType1Font
        Inherits PDSimpleFont
        '/**
        ' * Constructor.
        ' */

        Public Sub New()
            MyBase.New()
            font.setItem(COSName.SUBTYPE, COSName.MM_TYPE1)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param fontDictionary The font dictionary according to the PDF specification.
        ' */
        Public Sub New(ByVal fontDictionary As COSDictionary)
            MyBase.New(fontDictionary)
        End Sub

    End Class

End Namespace
