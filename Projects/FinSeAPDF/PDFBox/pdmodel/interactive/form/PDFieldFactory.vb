Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.annotation
Imports System.IO


Namespace org.apache.pdfbox.pdmodel.interactive.form

    '/**
    ' * This is the Factory for creating and returning the correct
    ' * field elements.
    ' *
    ' * @author sug
    ' * @version $Revision: 1.8 $
    ' */
    Public Class PDFieldFactory

        Private Const RADIO_BITMASK = 32768
        Private Const PUSHBUTTON_BITMASK = 65536
        Private Const RADIOS_IN_UNISON_BITMASK = 33554432

        Private Const FIELD_TYPE_BTN = "Btn"
        Private Const FIELD_TYPE_TX = "Tx"
        Private Const FIELD_TYPE_CH = "Ch"
        Private Const FIELD_TYPE_SIG = "Sig"

        '/**
        ' * Utility class so no constructor.
        ' */
        Private Sub New()
        End Sub

        '/**
        ' * This method creates a COSField subclass from the given field.
        ' * The field is a PDF Dictionary object that must represent a
        ' * field element. - othewise null is returned
        ' *
        ' * @param acroForm The form that the field will be part of.
        ' * @param field The dictionary representing a field element
        ' *
        ' * @return a subclass to COSField according to the kind of field passed to createField
        ' * @throws IOException If there is an error determining the field type.
        ' */
        Public Shared Function createField(ByVal acroForm As PDAcroForm, ByVal field As COSDictionary) As PDField
            Dim pdField As PDField = New PDUnknownField(acroForm, field)
            If (isButton(pdField)) Then
                Dim flags As Integer = pdField.getFieldFlags()
                'BJL, I have found that the radio flag bit is not always set
                'and that sometimes there is just a kids dictionary.
                'so, if there is a kids dictionary then it must be a radio button
                'group.
                Dim kids As COSArray = field.getDictionaryObject(COSName.getPDFName("Kids"))
                If (kids IsNot Nothing OrElse isRadio(flags)) Then
                    pdField = New PDRadioCollection(acroForm, field)
                ElseIf (isPushButton(flags)) Then
                    pdField = New PDPushButton(acroForm, field)
                Else
                    pdField = New PDCheckbox(acroForm, field)
                End If
            ElseIf (isChoiceField(pdField)) Then
                pdField = New PDChoiceField(acroForm, field)
            ElseIf (isTextbox(pdField)) Then
                pdField = New PDTextbox(acroForm, field)
            ElseIf (isSignature(pdField)) Then
                pdField = New PDSignatureField(acroForm, field)
            Else
                'do nothing and return an unknown field type.
            End If
            Return pdField
        End Function

        '/**
        ' * This method determines if the given
        ' * field is a radiobutton collection.
        ' *
        ' * @param flags The field flags.
        ' *
        ' * @return the result of the determination
        ' */
        Private Shared Function isRadio(ByVal flags As Integer) As Boolean
            Return (flags And RADIO_BITMASK) > 0
        End Function

        '/**
        ' * This method determines if the given
        ' * field is a pushbutton.
        ' *
        ' * @param flags The field flags.
        ' *
        ' * @return the result of the determination
        ' */
        Private Shared Function isPushButton(ByVal flags As Integer) As Boolean
            Return (flags And PUSHBUTTON_BITMASK) > 0
        End Function

        '/**
        ' * This method determines if the given field is a choicefield
        ' * Choicefields are either listboxes or comboboxes.
        ' *
        ' * @param field the field to determine
        ' * @return the result of the determination
        ' */
        Private Shared Function isChoiceField(ByVal field As PDField) As Boolean
            Return FIELD_TYPE_CH.Equals(field.findFieldType())
        End Function

        '/**
        ' * This method determines if the given field is a button.
        ' *
        ' * @param field the field to determine
        ' * @return the result of the determination
        ' *
        ' * @throws IOException If there is an error determining the field type.
        ' */
        Private Shared Function isButton(ByVal field As PDField) As Boolean
            Dim ft As String = field.findFieldType()
            Dim retval As Boolean = FIELD_TYPE_BTN.Equals(ft)
            Dim kids As List = field.getKids()
            If (ft Is Nothing AndAlso kids IsNot Nothing AndAlso kids.size() > 0) Then
                'sometimes if it is a button the type is only defined by one
                'of the kids entries
                Dim obj As Object = kids.get(0)
                Dim kidDict As COSDictionary = Nothing
                If (TypeOf (obj) Is PDField) Then
                    kidDict = DirectCast(obj, PDField).getDictionary()
                ElseIf (TypeOf (obj) Is PDAnnotationWidget) Then
                    kidDict = DirectCast(obj, PDAnnotationWidget).getDictionary()
                Else
                    Throw New IOException("Error:Unexpected type of kids field:" & obj.ToString)
                End If
                retval = isButton(New PDUnknownField(field.getAcroForm(), kidDict))
            End If
            Return retval
        End Function

        '/**
        '  * This method determines if the given field is a signature.
        '  *
        '  * @param field the field to determine
        '  * @return the result of the determination
        '  */
        Private Shared Function isSignature(ByVal field As PDField) As Boolean
            Return FIELD_TYPE_SIG.Equals(field.findFieldType())
        End Function

        '/**
        ' * This method determines if the given field is a Textbox.
        ' *
        ' * @param field the field to determine
        ' * @return the result of the determination
        ' */
        Private Shared Function isTextbox(ByVal field As PDField) As Boolean 'throws IOException
            Return FIELD_TYPE_TX.Equals(field.findFieldType())
        End Function


    End Class

End Namespace
