Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.pdmodel.interactive.form

    '/**
    ' * A class for handling the PDF field as a Radio Collection.
    ' * This class automatically keeps track of the child radio buttons
    ' * in the collection.
    ' *
    ' * @see PDCheckbox
    ' * @author sug
    ' * @version $Revision: 1.13 $
    ' */
    Public Class PDRadioCollection
        Inherits PDChoiceButton

        '/**
        '    * A Ff flag.
        '    */
        Public Const FLAG_RADIOS_IN_UNISON = 1 << 25

        '/**
        ' * @param theAcroForm The acroForm for this field.
        ' * @param field The field that makes up the radio collection.
        ' *
        ' * {@inheritDoc}
        ' */
        Public Sub New(ByVal theAcroForm As PDAcroForm, ByVal field As COSDictionary)
            MyBase.New(theAcroForm, field)
        End Sub

        '/**
        ' * From the PDF Spec <br/>
        ' * If set, a group of radio buttons within a radio button field that
        ' * use the same value for the on state will turn on and off in unison; that is if
        ' * one is checked, they are all checked. If clear, the buttons are mutually exclusive
        ' * (the same behavior as HTML radio buttons).
        ' *
        ' * @param radiosInUnison The new flag for radiosInUnison.
        ' */
        Public Sub setRadiosInUnison(ByVal radiosInUnison As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_RADIOS_IN_UNISON, radiosInUnison)
        End Sub

        '/**
        ' *
        ' * @return true If the flag is set for radios in unison.
        ' */
        Public Function isRadiosInUnison() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_RADIOS_IN_UNISON)
        End Function

        '/**
        ' * This setValue method iterates the collection of radiobuttons
        ' * and checks or unchecks each radiobutton according to the
        ' * given value.
        ' * If the value is not represented by any of the radiobuttons,
        ' * then none will be checked.
        ' *
        ' * {@inheritDoc}
        ' */
        Public Overrides Sub setValue(ByVal value As String)  'throws IOException
            getDictionary().setString(COSName.V, value)
            Dim kids As List = getKids()
            For i As Integer = 0 To kids.size() - 1
                Dim field As PDField = kids.get(i)
                If (TypeOf (field) Is PDCheckbox) Then
                    Dim btn As PDCheckbox = field
                    If (btn.getOnValue().equals(value)) Then
                        btn.check()
                    Else
                        btn.unCheck()
                    End If
                End If
            Next
        End Sub

        '/**
        ' * getValue gets the fields value to as a string.
        ' *
        ' * @return The string value of this field.
        ' *
        ' * @throws IOException If there is an error getting the value.
        ' */
        Public Overrides Function getValue() As String  'throws IOException
            Dim retval As String = ""
            Dim kids As List = getKids()
            For i As Integer = 0 To kids.size() - 1
                Dim kid As PDField = kids.get(i)
                If (TypeOf (kid) Is PDCheckbox) Then
                    Dim btn As PDCheckbox = kid
                    If (btn.isChecked()) Then
                        retval = btn.getOnValue()
                    End If
                End If
            Next
            If (retval Is Nothing) Then
                retval = getDictionary().getNameAsString(COSName.V)
            End If
            Return retval
        End Function


        '/**
        ' * This will return a list of PDField objects that are part of this radio collection.
        ' *
        ' * @see PDField#getWidget()
        ' * @return A list of PDWidget objects.
        ' * @throws IOException if there is an error while creating the children objects.
        ' */
        '@SuppressWarnings("unchecked")
        Public Overrides Function getKids() As List(Of COSObjectable) ' throws IOException
            Dim retval As List = Nothing
            Dim kids As COSArray = getDictionary().getDictionaryObject(COSName.KIDS)
            If (kids IsNot Nothing) Then
                Dim kidsList As List = New ArrayList()
                For i As Integer = 0 To kids.size() - 1
                    kidsList.add(PDFieldFactory.createField(getAcroForm(), kids.getObject(i)))
                Next
                retval = New COSArrayList(kidsList, kids)
            End If
            Return retval
        End Function

    End Class

End Namespace
