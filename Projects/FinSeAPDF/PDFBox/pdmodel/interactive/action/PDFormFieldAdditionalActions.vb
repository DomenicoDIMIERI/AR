Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action.type

Namespace org.apache.pdfbox.pdmodel.interactive.action

    '/**
    ' * This class represents a form field's dictionary of actions
    ' * that occur due to events.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Panagiotis Toumasis (ptoumasis@mail.gr)
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDFormFieldAdditionalActions
        Implements COSObjectable

        Private actions As COSDictionary

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            actions = New COSDictionary()
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param a The action dictionary.
        ' */
        Public Sub New(ByVal a As COSDictionary)
            actions = a
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return actions
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return actions
        End Function

        '/**
        ' * This will get a JavaScript action to be performed when the user
        ' * types a keystroke into a text field or combo box or modifies the
        ' * selection in a scrollable list box. This allows the keystroke to
        ' * be checked for validity and rejected or modified.
        ' *
        ' * @return The K entry of form field's additional actions dictionary.
        ' */
        Public Function getK() As PDAction
            Dim k As COSDictionary = actions.getDictionaryObject("K")
            Dim retval As PDAction = Nothing
            If (k IsNot Nothing) Then
                retval = PDActionFactory.createAction(k)
            End If
            Return retval
        End Function

        '/**
        ' * This will set a JavaScript action to be performed when the user
        ' * types a keystroke into a text field or combo box or modifies the
        ' * selection in a scrollable list box. This allows the keystroke to
        ' * be checked for validity and rejected or modified.
        ' *
        ' * @param k The action to be performed.
        ' */
        Public Sub setK(ByVal k As PDAction)
            actions.setItem("K", k)
        End Sub

        '/**
        ' * This will get a JavaScript action to be performed before
        ' * the field is formatted to display its current value. This
        ' * allows the field's value to be modified before formatting.
        ' *
        ' * @return The F entry of form field's additional actions dictionary.
        ' */
        Public Function getF() As PDAction
            Dim f As COSDictionary = actions.getDictionaryObject("F")
            Dim retval As PDAction = Nothing
            If (f IsNot Nothing) Then
                retval = PDActionFactory.createAction(f)
            End If
            Return retval
        End Function

        '/**
        ' * This will set a JavaScript action to be performed before
        ' * the field is formatted to display its current value. This
        ' * allows the field's value to be modified before formatting.
        ' *
        ' * @param f The action to be performed.
        ' */
        Public Sub setF(ByVal f As PDAction)
            actions.setItem("F", f)
        End Sub

        '/**
        ' * This will get a JavaScript action to be performed
        ' * when the field's value is changed. This allows the
        ' * new value to be checked for validity.
        ' * The name V stands for "validate".
        ' *
        ' * @return The V entry of form field's additional actions dictionary.
        ' */
        Public Function getV() As PDAction
            Dim v As COSDictionary = actions.getDictionaryObject("V")
            Dim retval As PDAction = Nothing
            If (v IsNot Nothing) Then
                retval = PDActionFactory.createAction(v)
            End If
            Return retval
        End Function

        '/**
        ' * This will set a JavaScript action to be performed
        ' * when the field's value is changed. This allows the
        ' * new value to be checked for validity.
        ' * The name V stands for "validate".
        ' *
        ' * @param v The action to be performed.
        ' */
        Public Sub setV(ByVal v As PDAction)
            actions.setItem("V", v)
        End Sub

        '/**
        ' * This will get a JavaScript action to be performed in order to recalculate
        ' * the value of this field when that of another field changes. The order in which
        ' * the document's fields are recalculated is defined by the CO entry in the
        ' * interactive form dictionary.
        ' * The name C stands for "calculate".
        ' *
        ' * @return The C entry of form field's additional actions dictionary.
        ' */
        Public Function getC() As PDAction
            Dim c As COSDictionary = actions.getDictionaryObject("C")
            Dim retval As PDAction = Nothing
            If (c IsNot Nothing) Then
                retval = PDActionFactory.createAction(c)
            End If
            Return retval
        End Function

        '/**
        ' * This will set a JavaScript action to be performed in order to recalculate
        ' * the value of this field when that of another field changes. The order in which
        ' * the document's fields are recalculated is defined by the CO entry in the
        ' * interactive form dictionary.
        ' * The name C stands for "calculate".
        ' *
        ' * @param c The action to be performed.
        ' */
        Public Sub setC(ByVal c As PDAction)
            actions.setItem("C", c)
        End Sub

    End Class

End Namespace
