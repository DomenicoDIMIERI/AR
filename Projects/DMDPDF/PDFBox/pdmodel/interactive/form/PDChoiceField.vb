Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.interactive.form

    '/**
    ' * A class for handling the PDF field as a choicefield.
    ' *
    ' * @author sug
    ' * @version $Revision: 1.7 $
    ' */
    Public Class PDChoiceField
        Inherits PDVariableText
        '/**
        ' * A Ff flag.
        ' */
        Public Const FLAG_COMBO = 1 << 17
        Public Const FLAG_EDIT = 1 << 18

        '/**
        ' * @see org.apache.pdfbox.pdmodel.interactive.form.PDField#PDField(PDAcroForm,COSDictionary)
        ' *
        ' * @param theAcroForm The acroForm for this field.
        ' * @param field The field for this choice field.
        ' */
        Public Sub New(ByVal theAcroForm As PDAcroForm, ByVal field As COSDictionary)
            MyBase.New(theAcroForm, field)
        End Sub

        '/**
        ' * @see org.apache.pdfbox.pdmodel.interactive.form.PDField#setValue(java.lang.String)
        ' *
        ' * @param optionValue The new value for this text field.
        ' *
        ' * @throws IOException If there is an error calculating the appearance stream or the value in not one
        ' *   of the existing options.
        ' */
        Public Overrides Sub setValue(ByVal optionValue As String)  'throws IOException
            Dim indexSelected As Integer = -1
            Dim options As COSArray = getDictionary().getDictionaryObject(COSName.OPT)
            Dim fieldFlags As Integer = getFieldFlags()
            Dim isEditable As Boolean = (FLAG_COMBO And fieldFlags) <> FLAG_COMBO AndAlso (FLAG_EDIT And fieldFlags) <> FLAG_EDIT

            If (options.size() = 0 AndAlso Not isEditable) Then
                Throw New IOException("Error: You cannot set a value for a choice field if there are no options.")
            Else
                ' YXJ: Changed the order of the loops. Acrobat produces PDF's
                ' where sometimes there is 1 string and the rest arrays.
                ' This code works either way.
                For i As Integer = 0 To options.size() - 1
                    If (indexSelected <> -1) Then Exit For
                    Dim [option] As COSBase = options.getObject(i)
                    If (TypeOf ([option]) Is COSArray) Then
                        Dim keyValuePair As COSArray = [option]
                        Dim key As COSString = keyValuePair.getObject(0)
                        Dim value As COSString = keyValuePair.getObject(1)
                        If (optionValue.Equals(key.getString()) OrElse optionValue.Equals(value.getString())) Then
                            'have the parent draw the appearance stream with the value
                            MyBase.setValue(value.getString())
                            'but then use the key as the V entry
                            getDictionary().setItem(COSName.V, key)
                            indexSelected = i
                        End If
                    Else
                        Dim value As COSString = [option]
                        If (optionValue.Equals(value.getString())) Then
                            MyBase.setValue(optionValue)
                            indexSelected = i
                        End If
                    End If
                Next
            End If
            If (indexSelected = -1 AndAlso isEditable) Then
                MyBase.setValue(optionValue)
            ElseIf (indexSelected = -1) Then
                Throw New IOException("Error: '" & optionValue & "' was not an available option.")
            Else
                Dim indexArray As COSArray = getDictionary().getDictionaryObject(COSName.I)
                If (indexArray IsNot Nothing) Then
                    indexArray.clear()
                    indexArray.add(COSInteger.get(indexSelected))
                End If
            End If
        End Sub

    End Class

End Namespace
