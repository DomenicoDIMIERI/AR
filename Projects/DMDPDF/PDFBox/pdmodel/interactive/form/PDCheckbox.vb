Imports FinSeA.org.apache.pdfbox.cos
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.interactive.form

    '/**
    ' * A class for handling the PDF field as a checkbox.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author sug
    ' * @version $Revision: 1.11 $
    ' */
    Public Class PDCheckbox
        Inherits PDChoiceButton

        Private Shared ReadOnly KEY As COSName = COSName.getPDFName("AS")
        Private Shared ReadOnly OFF_VALUE As COSName = COSName.getPDFName("Off")

        Private value As COSName

        '/**
        ' * @see PDField#PDField(PDAcroForm,COSDictionary)
        ' *
        ' * @param theAcroForm The acroForm for this field.
        ' * @param field The checkbox field dictionary
        ' */
        Public Sub New(ByVal theAcroForm As PDAcroForm, ByVal field As COSDictionary)
            MyBase.New(theAcroForm, field)
            Dim ap As COSDictionary = field.getDictionaryObject(COSName.getPDFName("AP"))
            If (ap IsNot Nothing) Then
                Dim n As COSBase = ap.getDictionaryObject(COSName.getPDFName("N"))
                If (TypeOf (n) Is COSDictionary) Then
                    For Each name As COSName In DirectCast(n, COSDictionary).keySet()
                        If (Not name.equals(OFF_VALUE)) Then
                            value = name
                        End If
                    Next
                End If
            Else
                value = getDictionary().getDictionaryObject("V")
            End If
        End Sub

        '/**
        ' * This will tell if this radio button is currently checked or not.
        ' *
        ' * @return true If the radio button is checked.
        ' */
        Public Function isChecked() As Boolean
            Dim retval As Boolean = False
            Dim onValue As String = getOnValue()
            Dim radioValue As COSName = getDictionary().getDictionaryObject(KEY)
            If (radioValue IsNot Nothing AndAlso value IsNot Nothing AndAlso radioValue.getName().Equals(onValue)) Then
                retval = True
            End If

            Return retval
        End Function

        '/**
        ' * Checks the radiobutton.
        ' */
        Public Sub check()
            getDictionary().setItem(KEY, value)
        End Sub

        '/**
        ' * Unchecks the radiobutton.
        ' */
        Public Sub unCheck()
            getDictionary().setItem(KEY, OFF_VALUE)
        End Sub

    
        Public Overrides Sub setValue(ByVal newValue As String)
            getDictionary().setName("V", newValue)
            If (newValue Is Nothing) Then
                getDictionary().setItem(KEY, OFF_VALUE)
            Else
                getDictionary().setName(KEY, newValue)
            End If
        End Sub

        '/**
        ' * This will get the value of the radio button.
        ' *
        ' * @return The value of the radio button.
        ' */
        Public Function getOffValue() As String
            Return OFF_VALUE.getName()
        End Function

        '/**
        ' * This will get the value of the radio button.
        ' *
        ' * @return The value of the radio button.
        ' */
        Public Function getOnValue() As String
            Dim retval As String = vbNullString
            Dim ap As COSDictionary = getDictionary().getDictionaryObject(COSName.getPDFName("AP"))
            Dim n As COSBase = ap.getDictionaryObject(COSName.getPDFName("N"))

            'N can be a COSDictionary or a COSStream
            If (TypeOf (n) Is COSDictionary) Then
                For Each key As COSName In DirectCast(n, COSDictionary).keySet()
                    If (Not key.equals(OFF_VALUE)) Then
                        retval = key.getName()
                    End If
                Next
            End If
            Return retval
        End Function

        '/**
        ' * getValue gets the fields value to as a string.
        ' *
        ' * @return The string value of this field.
        ' *
        ' * @throws IOException If there is an error getting the value.
        ' */
        Public Overrides Function getValue() As String 'throws IOException
            Return getDictionary().getNameAsString("V")
        End Function

    End Class

End Namespace