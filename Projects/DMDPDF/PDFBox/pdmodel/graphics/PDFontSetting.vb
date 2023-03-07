Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.font
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.graphics

    '/**
    ' * This class represents a font setting used for the graphics state.  A font setting is a font and a
    ' * font size.  Maybe there is a better name for this?
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDFontSetting
        Implements COSObjectable

        Private fontSetting As COSArray = Nothing

        '/**
        ' * Creates a blank font setting, font will be null, size will be 1.
        ' */
        Public Sub New()
            Dim tmp As COSBase = Nothing
            fontSetting = New COSArray()
            fontSetting.add(tmp)
            fontSetting.add(New COSFloat(1))
        End Sub

        '/**
        ' * Constructs a font setting from an existing array.
        ' *
        ' * @param fs The new font setting value.
        ' */
        Public Sub New(ByVal fs As COSArray)
            fontSetting = fs
        End Sub

   
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return fontSetting
        End Function

        '/**
        ' * This will get the font for this font setting.
        ' *
        ' * @return The font for this setting of null if one was not found.
        ' *
        ' * @throws IOException If there is an error getting the font.
        ' */
        Public Function getFont() As PDFont ' throws IOException
            Dim retval As PDFont = Nothing
            Dim font As COSBase = fontSetting.get(0)
            If (TypeOf (font) Is COSDictionary) Then
                retval = PDFontFactory.createFont(font)
            End If
            Return retval
        End Function

        '/**
        ' * This will set the font for this font setting.
        ' *
        ' * @param font The new font.
        ' */
        Public Sub setFont(ByVal font As PDFont)
            fontSetting.set(0, font)
        End Sub

        '/**
        ' * This will get the size of the font.
        ' *
        ' * @return The size of the font.
        ' */
        Public Function getFontSize() As Single
            Dim size As COSNumber = fontSetting.get(1)
            Return size.floatValue()
        End Function

        '/**
        ' * This will set the size of the font.
        ' *
        ' * @param size The new size of the font.
        ' */
        Public Sub setFontSize(ByVal size As Single)
            fontSetting.set(1, New COSFloat(size))
        End Sub

    End Class

End Namespace
