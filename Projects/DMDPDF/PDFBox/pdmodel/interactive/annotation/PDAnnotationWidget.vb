Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action.type

Namespace org.apache.pdfbox.pdmodel.interactive.annotation

    '/**
    ' * This is the class that represents a widget.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.3 $
    ' */
    Public Class PDAnnotationWidget
        Inherits PDAnnotation

        ''' <summary>
        ''' The type of annotation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SUB_TYPE As String = "Widget"


        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            MyBase.New()
            getDictionary().setName(COSName.SUBTYPE, SUB_TYPE)
        End Sub

        ' /**
        '* Creates a PDWidget from a COSDictionary, expected to be
        '* a correct object definition for a field in PDF.
        '*
        '* @param field the PDF objet to represent as a field.
        '*/
        Public Sub New(ByVal field As COSDictionary)
            MyBase.New(field)
        End Sub

        '/**
        ' * Returns the highlighting mode. Default value: <code>I</code>
        ' * <dl>
        ' *   <dt><code>N</code></dt>
        ' *     <dd>(None) No highlighting.</dd>
        ' *   <dt><code>I</code></dt>
        ' *     <dd>(Invert) Invert the contents of the annotation rectangle.</dd>
        ' *   <dt><code>O</code></dt>
        ' *     <dd>(Outline) Invert the annotation's border.</dd>
        ' *   <dt><code>P</code></dt>
        ' *     <dd>(Push) Display the annotation's down appearance, if any. If no
        ' *      down appearance is defined, the contents of the annotation rectangle
        ' *      shall be offset to appear as if it were pushed below the surface of
        ' *      the page</dd>
        ' *   <dt><code>T</code></dt>
        ' *     <dd>(Toggle) Same as <code>P</code> (which is preferred).</dd>
        ' * </dl>
        ' * 
        ' * @return the highlighting mode
        ' */
        Public Function getHighlightingMode() As String
            Return Me.getDictionary().getNameAsString(COSName.H, "I")
        End Function

        '/**
        ' * Sets the highlighting mode.
        ' * <dl>
        ' *   <dt><code>N</code></dt>
        ' *     <dd>(None) No highlighting.</dd>
        ' *   <dt><code>I</code></dt>
        ' *     <dd>(Invert) Invert the contents of the annotation rectangle.</dd>
        ' *   <dt><code>O</code></dt>
        ' *     <dd>(Outline) Invert the annotation's border.</dd>
        ' *   <dt><code>P</code></dt>
        ' *     <dd>(Push) Display the annotation's down appearance, if any. If no
        ' *      down appearance is defined, the contents of the annotation rectangle
        ' *      shall be offset to appear as if it were pushed below the surface of
        ' *      the page</dd>
        ' *   <dt><code>T</code></dt>
        ' *     <dd>(Toggle) Same as <code>P</code> (which is preferred).</dd>
        ' * </dl>
        ' * 
        ' * @param highlightingMode the highlighting mode
        ' *  the defined values
        ' */
        Public Sub setHighlightingMode(ByVal highlightingMode As String)
            If ((highlightingMode Is Nothing) OrElse "N".Equals(highlightingMode) OrElse "I".Equals(highlightingMode) OrElse "O".Equals(highlightingMode) OrElse "P".Equals(highlightingMode) OrElse "T".Equals(highlightingMode)) Then
                Me.getDictionary().setName(COSName.H, highlightingMode)
            Else
                Throw New ArgumentOutOfRangeException("Valid values for highlighting mode are 'N', 'N', 'O', 'P' or 'T'")
            End If
        End Sub

        '/**
        ' * Returns the appearance characteristics dictionary.
        ' * 
        ' * @return the appearance characteristics dictionary
        ' */
        Public Function getAppearanceCharacteristics() As PDAppearanceCharacteristicsDictionary
            Dim mk As COSBase = Me.getDictionary().getDictionaryObject(COSName.getPDFName("MK"))
            If (TypeOf (mk) Is COSDictionary) Then
                Return New PDAppearanceCharacteristicsDictionary(mk)
            End If
            Return Nothing
        End Function

        '/**
        ' * Sets the appearance characteristics dictionary.
        ' * 
        ' * @param appearanceCharacteristics the appearance characteristics dictionary
        ' */
        Public Sub setAppearanceCharacteristics(ByVal appearanceCharacteristics As PDAppearanceCharacteristicsDictionary)
            Me.getDictionary().setItem("MK", appearanceCharacteristics)
        End Sub

        '/**
        ' * Get the action to be performed when this annotation is to be activated.
        ' *
        ' * @return The action to be performed when this annotation is activated.
        ' */
        Public Function getAction() As PDAction
            Dim action As COSDictionary = Me.getDictionary().getDictionaryObject(COSName.A)
            Return PDActionFactory.createAction(action)
        End Function

        '/**
        ' * Set the annotation action.
        ' * As of PDF 1.6 this is only used for Widget Annotations
        ' * @param action The annotation action.
        ' */
        Public Sub setAction(ByVal action As PDAction)
            Me.getDictionary().setItem(COSName.A, action)
        End Sub

        '/**
        ' * Get the additional actions for this field.  This will return null
        ' * if there are no additional actions for this field.
        ' * As of PDF 1.6 this is only used for Widget Annotations.
        ' *
        ' * @return The actions of the field.
        ' */
        Public Function getActions() As PDAnnotationAdditionalActions
            Dim aa As COSDictionary = Me.getDictionary().getDictionaryObject("AA")
            Dim retval As PDAnnotationAdditionalActions = Nothing
            If (aa IsNot Nothing) Then
                retval = New PDAnnotationAdditionalActions(aa)
            End If
            Return retval
        End Function

        '/**
        ' * Set the actions of the field.
        ' *
        ' * @param actions The field actions.
        ' */
        Public Sub setActions(ByVal actions As PDAnnotationAdditionalActions)
            Me.getDictionary().setItem("AA", actions)
        End Sub

        '/**
        ' * This will set the border style dictionary, specifying the width and dash
        ' * pattern used in drawing the line.
        ' *
        ' * @param bs the border style dictionary to set.
        ' *
        ' */
        Public Sub setBorderStyle(ByVal bs As PDBorderStyleDictionary)
            Me.getDictionary().setItem("BS", bs)
        End Sub

        '/**
        ' * This will retrieve the border style dictionary, specifying the width and
        ' * dash pattern used in drawing the line.
        ' *
        ' * @return the border style dictionary.
        ' */
        Public Function getBorderStyle() As PDBorderStyleDictionary
            Dim bs As COSDictionary = Me.getDictionary().getItem(COSName.getPDFName("BS"))
            If (bs IsNot Nothing) Then
                Return New PDBorderStyleDictionary(bs)
            Else
                Return Nothing
            End If
        End Function

        '    // TODO where to get acroForm from?
        '//    public PDField getParent() throws IOException
        '//    {
        '//        COSBase parent = Me.getDictionary().getDictionaryObject(COSName.PARENT);
        '//        if (parent instanceof COSDictionary)
        '//        {
        '//            PDAcroForm acroForm = null;
        '//            return PDFieldFactory.createField(acroForm, (COSDictionary) parent);
        '//        }
        '//        return null;
        '//    }


    End Class

End Namespace
