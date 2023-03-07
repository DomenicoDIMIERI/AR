Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.annotation
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.digitalsignature
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.interactive.form

    '/**
    ' * A class for handling the PDF field as a signature.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Thomas Chojecki
    ' * @version $Revision: 1.5 $
    ' */
    Public Class PDSignatureField
        Inherits PDField

        '/**
        ' * @see PDField#PDField(PDAcroForm,COSDictionary)
        ' *
        ' * @param theAcroForm The acroForm for this field.
        ' * @param field The dictionary for the signature.
        ' * @throws IOException If there is an error while resolving partital name for the signature field
        ' */
        Public Sub New(ByVal theAcroForm As PDAcroForm, ByVal field As COSDictionary) ' throws IOException
            MyBase.New(theAcroForm, field)
            ' dirty hack to avoid npe caused through getWidget() method
            getDictionary().setItem(COSName.TYPE, COSName.ANNOT)
            getDictionary().setName(COSName.SUBTYPE, PDAnnotationWidget.SUB_TYPE)
        End Sub

        '/**
        ' * @see PDField#PDField(PDAcroForm)
        ' *
        ' * @param theAcroForm The acroForm for this field.
        ' * @throws IOException If there is an error while resolving partial name for the signature field
        ' *         or getting the widget object.
        ' */
        Public Sub New(ByVal theAcroForm As PDAcroForm)  'throws IOException
            MyBase.New(theAcroForm)
            getDictionary().setItem(COSName.FT, COSName.SIG)
            getWidget().setLocked(True)
            getWidget().setPrinted(True)
            setPartialName(generatePartialName())
            getDictionary().setItem(COSName.TYPE, COSName.ANNOT)
            getDictionary().setName(COSName.SUBTYPE, PDAnnotationWidget.SUB_TYPE)
        End Sub

        '/**
        ' * Generate a unique name for the signature.
        ' * @return
        ' * @throws IOException If there is an error while getting the list of fields.
        ' */
        Private Function generatePartialName() As String  'throws IOException
            Dim acroForm As PDAcroForm = getAcroForm()
            Dim fields As List = acroForm.getFields()

            Dim fieldName As String = "Signature"
            Dim i As Integer = 1

            Dim sigNames As [Set](Of String) = New HashSet(Of String)()

            For Each [object] As Object In fields
                If (TypeOf ([object]) Is PDSignatureField) Then
                    sigNames.add(DirectCast([object], PDSignatureField).getPartialName())
                End If
            Next

            While (sigNames.contains(fieldName + i))
                i += 1
            End While
            Return fieldName + i
        End Function

        '/**
        ' * @see PDField#setValue(java.lang.String)
        ' *
        ' * @param value The new value for the field.
        ' *
        ' * @throws IOException If there is an error creating the appearance stream.
        ' * @deprecated use setSignature(PDSignature) instead
        ' */
        <Obsolete> _
        Public Overrides Sub setValue(ByVal value As String)  'throws IOException
            Throw New RuntimeException("Can't set signature as String, use setSignature(PDSignature) instead")
        End Sub

        '/**
        ' * @see PDField#setValue(java.lang.String)
        ' *
        ' * @return The string value of this field.
        ' *
        ' * @throws IOException If there is an error creating the appearance stream.
        ' * @deprecated use getSignature() instead
        ' */
        <Obsolete> _
        Public Overrides Function getValue() As String ' throws IOException
            Throw New RuntimeException("Can't get signature as String, use getSignature() instead.")
        End Function

        '/**
        ' * Return a string rep of this object.
        ' *
        ' * @return A string rep of this object.
        ' */
        Public Overrides Function toString() As String
            Return "PDSignature"
        End Function

        '/**
        ' * Add a signature dictionary to the signature field.
        ' * 
        ' * @param value is the PDSignature 
        ' */
        Public Sub setSignature(ByVal value As digitalsignature.PDSignature)
            getDictionary().setItem(COSName.V, value)
        End Sub

        '/**
        ' * Get the signature dictionary.
        ' * 
        ' * @return the signature dictionary
        ' * 
        ' */
        Public Function getSignature() As digitalsignature.PDSignature
            Dim dictionary As COSBase = getDictionary().getDictionaryObject(COSName.V)
            If (dictionary Is Nothing) Then
                Return Nothing
            End If
            Return New digitalsignature.PDSignature(dictionary)
        End Function

        '/**
        ' * <p>(Optional; PDF 1.5) A seed value dictionary containing information
        ' * that constrains the properties of a signature that is applied to the
        ' * field.</p>
        ' *
        ' * @return the seed value dictionary as PDSeedValue
        ' */
        Public Function getSeedValue() As PDSeedValue
            Dim dict As COSDictionary = getDictionary().getDictionaryObject(COSName.SV)
            Dim sv As PDSeedValue = Nothing
            If (dict IsNot Nothing) Then
                sv = New PDSeedValue(dict)
            End If
            Return sv
        End Function

        '/**
        ' * <p>(Optional; PDF 1.) A seed value dictionary containing information
        ' * that constrains the properties of a signature that is applied to the
        ' * field.</p>
        ' *
        ' * @param sv is the seed value dictionary as PDSeedValue
        ' */
        Public Sub setSeedValue(ByVal sv As PDSeedValue)
            If (sv IsNot Nothing) Then
                getDictionary().setItem(COSName.SV, sv.getCOSObject())
            End If
        End Sub

    End Class

End Namespace
