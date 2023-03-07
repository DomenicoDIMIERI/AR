Imports FinSeA.org.apache.pdfbox.cos
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.interactive.form

    '/**
    ' * A class for handling the PDF field as a PDPushButton.
    ' *
    ' * @author sug
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PDPushButton
        Inherits PDField

        '/**
        ' * @see org.apache.pdfbox.pdmodel.field.PDField#COSField(org.apache.pdfbox.cos.COSDictionary)
        ' *
        ' * @param theAcroForm The acroForm for this field.
        ' * @param field The field for this push button.
        ' */
        Public Sub New(ByVal theAcroForm As PDAcroForm, ByVal field As COSDictionary)
            MyBase.New(theAcroForm, field)
        End Sub

        '/**
        ' * @see as.interactive.pdf.form.cos.COSField#setValue(java.lang.String)
        ' *
        ' * @param value The new value for the field.
        ' *
        ' * @throws IOException If there is an error creating the appearance stream.
        ' */
        Public Overrides Sub setValue(ByVal value As String)  'throws IOException
            Dim fieldValue As COSString = New COSString(value)
            getDictionary().setItem(COSName.getPDFName("V"), fieldValue)
            getDictionary().setItem(COSName.getPDFName("DV"), fieldValue)
        End Sub

        '/**
        ' * getValue gets the fields value to as a string.
        ' *
        ' * @return The string value of this field.
        ' *
        ' * @throws IOException If there is an error getting the value.
        ' */
        Public Overrides Function getValue() As String ' throws IOException
            Return getDictionary().getString("V")
        End Function


    End Class

End Namespace
