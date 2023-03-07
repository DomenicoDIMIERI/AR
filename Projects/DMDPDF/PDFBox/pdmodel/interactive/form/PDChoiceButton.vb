Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.form

    '/**
    ' * This holds common functionality for check boxes and radio buttons.
    ' *
    ' * @author sug
    ' * @version $Revision: 1.4 $
    ' */
    Public MustInherit Class PDChoiceButton
        Inherits PDField

        '/**
        ' * @see PDField#PDField(PDAcroForm,org.apache.pdfbox.cos.COSDictionary)
        ' *
        ' * @param theAcroForm The acroForm for this field.
        ' * @param field The field for this button.
        ' */
        Public Sub New(ByVal theAcroForm As PDAcroForm, ByVal field As COSDictionary)
            MyBase.New(theAcroForm, field)
        End Sub

        '/**
        ' * This will get the option values "Opt" entry of the pdf button.
        ' *
        ' * @return A list of java.lang.String values.
        ' */
        Public Function getOptions() As List
            Dim retval As List = Nothing
            Dim array As COSArray = getDictionary().getDictionaryObject(COSName.getPDFName("Opt"))
            If (array IsNot Nothing) Then
                Dim strings As List = New ArrayList()
                For i As Integer = 0 To array.size() - 1
                    strings.add(DirectCast(array.getObject(i), COSString).getString())
                Next
                retval = New COSArrayList(strings, array)
            End If
            Return retval
        End Function

        '/**
        ' * This will will set the list of options for this button.
        ' *
        ' * @param options The list of options for the button.
        ' */
        Public Sub setOptions(ByVal options As List)
            getDictionary().setItem(COSName.getPDFName("Opt"), COSArrayList.converterToCOSArray(options))
        End Sub

    End Class

End Namespace
