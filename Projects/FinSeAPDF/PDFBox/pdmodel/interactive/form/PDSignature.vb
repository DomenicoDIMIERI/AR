Imports FinSeA.org.apache.pdfbox.cos
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.interactive.form

    '/**
    ' * A class for handling the PDF field as a signature.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.5 $
    ' * 
    ' * @deprecated Use {@link PDSignatureField} instead (see PDFBOX-1513).
    ' */
    Public Class PDSignature
        Inherits PDField

        '/**
        ' * @see PDField#PDField(PDAcroForm,COSDictionary)
        ' *
        ' * @param theAcroForm The acroForm for this field.
        ' * @param field The dictionary for the signature.
        ' */
        Public Sub New(ByVal theAcroForm As PDAcroForm, ByVal field As COSDictionary)
            MyBase.New(theAcroForm, field)
            Throw New RuntimeException("The usage of " & Me.GetType().Name & " is deprecated. Please use PDSignatureField instead (see PDFBOX-1513)")
        End Sub

        '/**
        ' * @see PDField#setValue(java.lang.String)
        ' *
        ' * @param value The new value for the field.
        ' *
        ' * @throws IOException If there is an error creating the appearance stream.
        ' */
        Public Overrides Sub setValue(ByVal value As String) 'throws IOException
            Throw New NotImplementedException("Not yet implemented")
        End Sub

        '/**
        ' * @see PDField#setValue(java.lang.String)
        ' *
        ' * @return The string value of this field.
        ' *
        ' * @throws IOException If there is an error creating the appearance stream.
        ' */
        Public Overrides Function getValue() As String ' throws IOException
            Throw New NotImplementedException("Not yet implemented")
        End Function

        '/**
        ' * Return a string rep of this object.
        ' *
        ' * @return A string rep of this object.
        ' */
        Public Overrides Function toString() As String
            Return "PDSignature"
        End Function

    End Class

End Namespace
