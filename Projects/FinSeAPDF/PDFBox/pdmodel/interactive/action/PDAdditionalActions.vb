Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action.type

Namespace org.apache.pdfbox.pdmodel.interactive.action

    '/**
    ' * This represents a dictionary of actions that occur due to events.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.4 $
    ' */
    Public Class PDAdditionalActions
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
        ' * Get the F action.
        ' *
        ' * @return The F action.
        ' */
        Public Function getF() As PDAction
            Return PDActionFactory.createAction(actions.getDictionaryObject("F"))
        End Function

        '/**
        ' * Set the F action.
        ' *
        ' * @param action Get the F action.
        ' */
        Public Sub setF(ByVal action As PDAction)
            actions.setItem("F", action)
        End Sub

    End Class

End Namespace
