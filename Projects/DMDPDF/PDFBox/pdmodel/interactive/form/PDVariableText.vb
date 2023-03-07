Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util
Imports System.IO

Namespace org.apache.pdfbox.pdmodel.interactive.form

    '/**
    ' * A class for handling PDF fields that display text.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.7 $
    ' */
    Public MustInherit Class PDVariableText
        Inherits PDField
        '/**
        ' * A Ff flag.
        ' */
        Public Const FLAG_MULTILINE = 1 << 12
        Public Const FLAG_PASSWORD = 1 << 13
        Public Const FLAG_FILE_SELECT = 1 << 20
        Public Const FLAG_DO_NOT_SPELL_CHECK = 1 << 22
        Public Const FLAG_DO_NOT_SCROLL = 1 << 23
        Public Const FLAG_COMB = 1 << 24
        Public Const FLAG_RICH_TEXT = 1 << 25

        ''' <summary>
        ''' Default appearance.
        ''' </summary>
        ''' <remarks></remarks>
        Private da As COSString

        Private appearance As PDAppearance


        ''' <summary>
        ''' A Q value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const QUADDING_LEFT = 0
        Public Const QUADDING_CENTERED = 1
        Public Const QUADDING_RIGHT = 2

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
            da = field.getDictionaryObject(COSName.DA)
        End Sub

        '/**
        ' * @see org.apache.pdfbox.pdmodel.interactive.form.PDField#setValue(java.lang.String)
        ' *
        ' * @param value The new value for this text field.
        ' *
        ' * @throws IOException If there is an error calculating the appearance stream.
        ' */
        Public Overrides Sub setValue(ByVal value As String)  'throws IOException
            Dim fieldValue As COSString = New COSString(value)
            getDictionary().setItem(COSName.V, fieldValue)

            '//hmm, not sure what the case where the DV gets set to the field
            '//value, for now leave blank until we can come up with a case
            '//where it needs to be in there
            '//getDictionary().setItem( COSName.getPDFName( "DV" ), fieldValue );
            If (appearance Is Nothing) Then
                Me.appearance = New PDAppearance(getAcroForm(), Me)
            End If
            appearance.setAppearanceValue(value)
        End Sub

        '/**
        ' * getValue gets the fields value to as a string.
        ' *
        ' * @return The string value of this field.
        ' *
        ' * @throws IOException If there is an error getting the value.
        ' */
        Public Overrides Function getValue() As String 'throws IOException
            Return getDictionary().getString(COSName.V)
        End Function

        '/**
        ' * @return true if the field is multiline
        ' */
        Public Function isMultiline() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_MULTILINE)
        End Function

        '/**
        ' * Set the multiline bit.
        ' *
        ' * @param multiline The value for the multiline.
        ' */
        Public Sub setMultiline(ByVal multiline As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_MULTILINE, multiline)
        End Sub

        '/**
        ' * @return true if the field is a password field.
        ' */
        Public Function isPassword() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_PASSWORD)
        End Function

        '/**
        ' * Set the password bit.
        ' *
        ' * @param password The value for the password.
        ' */
        Public Sub setPassword(ByVal password As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_PASSWORD, password)
        End Sub

        '/**
        ' * @return true if the field is a file select field.
        ' */
        Public Function isFileSelect() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_FILE_SELECT)
        End Function

        '/**
        ' * Set the file select bit.
        ' *
        ' * @param fileSelect The value for the fileSelect.
        ' */
        Public Sub setFileSelect(ByVal fileSelect As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_FILE_SELECT, fileSelect)
        End Sub

        '/**
        ' * @return true if the field is not suppose to spell check.
        ' */
        Public Function doNotSpellCheck() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_DO_NOT_SPELL_CHECK)
        End Function

        '/**
        ' * Set the doNotSpellCheck bit.
        ' *
        ' * @param doNotSpellCheck The value for the doNotSpellCheck.
        ' */
        Public Sub setDoNotSpellCheck(ByVal doNotSpellCheck As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_DO_NOT_SPELL_CHECK, doNotSpellCheck)
        End Sub

        '/**
        ' * @return true if the field is not suppose to scroll.
        ' */
        Public Function doNotScroll() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_DO_NOT_SCROLL)
        End Function

        '/**
        ' * Set the doNotScroll bit.
        ' *
        ' * @param doNotScroll The value for the doNotScroll.
        ' */
        Public Sub setDoNotScroll(ByVal doNotScroll As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_DO_NOT_SCROLL, doNotScroll)
        End Sub

        '/**
        ' * @return true if the field is not suppose to comb the text display.
        ' */
        Public Function shouldComb() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_COMB)
        End Function

        '/**
        ' * Set the comb bit.
        ' *
        ' * @param comb The value for the comb.
        ' */
        Public Sub setComb(ByVal comb As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_COMB, comb)
        End Sub

        '/**
        ' * @return true if the field is a rich text field.
        ' */
        Public Function isRichText() As Boolean
            Return BitFlagHelper.getFlag(getDictionary(), COSName.FF, FLAG_RICH_TEXT)
        End Function

        '/**
        ' * Set the richText bit.
        ' *
        ' * @param richText The value for the richText.
        ' */
        Public Sub setRichText(ByVal richText As Boolean)
            BitFlagHelper.setFlag(getDictionary(), COSName.FF, FLAG_RICH_TEXT, richText)
        End Sub

        '/**
        ' * @return the DA element of the dictionary object
        ' */
        Protected Friend Function getDefaultAppearance() As COSString
            Return da
        End Function

        '/**
        ' * This will get the 'quadding' or justification of the text to be displayed.
        ' * 0 - Left(default)<br/>
        ' * 1 - Centered<br />
        ' * 2 - Right<br />
        ' * Please see the QUADDING_CONSTANTS.
        ' *
        ' * @return The justification of the text strings.
        ' */
        Public Function getQ() As Integer
            Dim retval As Integer = 0
            Dim number As COSNumber = getDictionary().getDictionaryObject(COSName.Q)
            If (number IsNot Nothing) Then
                retval = number.intValue()
            End If
            Return retval
        End Function

        '/**
        ' * This will set the quadding/justification of the text.  See QUADDING constants.
        ' *
        ' * @param q The new text justification.
        ' */
        Public Sub setQ(ByVal q As Integer)
            getDictionary().setInt(COSName.Q, q)
        End Sub

    End Class

End Namespace
