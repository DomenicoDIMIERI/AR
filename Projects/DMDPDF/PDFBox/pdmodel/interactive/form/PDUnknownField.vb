Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.interactive.form

    '/**
    ' * This class represents a form field with an unknown type.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDUnknownField
        Inherits PDField

        '/**
        '    * @see org.apache.pdfbox.pdmodel.interactive.form.PDField#PDField(PDAcroForm, COSDictionary)
        '    *
        '    * @param theAcroForm The acroForm for this field.
        '    * @param field The field's dictionary.
        '    */
        Public Sub New(ByVal theAcroForm As PDAcroForm, ByVal field As COSDictionary)
            MyBase.New(theAcroForm, field)
        End Sub

        Public Overrides Sub setValue(ByVal value As String)  'throws IOException
            '    //do nothing
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Overrides Function getValue() As String  ' throws IOException
            Return vbNullString
        End Function

    End Class

End Namespace
