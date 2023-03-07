Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.interactive.form

    '/**
    ' * A class for handling the PDF field as a textbox.
    ' *
    ' * @author sug
    ' * 
    ' */
    Public Class PDTextbox
        Inherits PDVariableText

        '/**
        ' * @see PDField#PDField(PDAcroForm,COSDictionary)
        ' *
        ' * @param theAcroForm The acroform.
        ' */
        Public Sub New(ByVal theAcroForm As PDAcroForm)
            MyBase.New(theAcroForm)
        End Sub

        '/**
        ' * @see org.apache.pdfbox.pdmodel.interactive.form.PDField#PDField(PDAcroForm,COSDictionary)
        ' *
        ' * @param theAcroForm The acroForm for this field.
        ' * @param field The field's dictionary.
        ' */
        Public Sub New(ByVal theAcroForm As PDAcroForm, ByVal field As COSDictionary)
            MyBase.New(theAcroForm, field)
        End Sub

        '/**
        ' * Returns the maximum number of characters of the text field.
        ' * 
        ' * @return the maximum number of characters, returns -1 if the value isn't present
        ' */
        Public Function getMaxLen() As Integer
            Return getDictionary().getInt(COSName.MAX_LEN)
        End Function

        '/**
        ' * Sets the maximum number of characters of the text field.
        ' * 
        ' * @param maxLen the maximum number of characters
        ' */
        Public Sub setMaxLen(ByVal maxLen As Integer)
            getDictionary().setInt(COSName.MAX_LEN, maxLen)
        End Sub

    End Class

End Namespace