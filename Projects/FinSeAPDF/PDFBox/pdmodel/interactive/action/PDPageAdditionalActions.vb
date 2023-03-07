Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action.type

Namespace org.apache.pdfbox.pdmodel.interactive.action

    '/**
    ' * This class represents a page object's dictionary of actions
    ' * that occur due to events.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Panagiotis Toumasis (ptoumasis@mail.gr)
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDPageAdditionalActions
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
        ' * This will get an action to be performed when the page
        ' * is opened. This action is independent of any that may be
        ' * defined by the OpenAction entry in the document catalog,
        ' * and is executed after such an action.
        ' *
        ' * @return The O entry of page object's additional actions dictionary.
        ' */
        Public Function getO() As PDAction
            Dim o As COSDictionary = actions.getDictionaryObject("O")
            Dim retval As PDAction = Nothing
            If (o IsNot Nothing) Then
                retval = PDActionFactory.createAction(o)
            End If
            Return retval
        End Function

        '/**
        ' * This will set an action to be performed when the page
        ' * is opened. This action is independent of any that may be
        ' * defined by the OpenAction entry in the document catalog,
        ' * and is executed after such an action.
        ' *
        ' * @param o The action to be performed.
        ' */
        Public Sub setO(ByVal o As PDAction)
            actions.setItem("O", o)
        End Sub

        '/**
        ' * This will get an action to be performed when the page
        ' * is closed. This action applies to the page being closed,
        ' * and is executed before any other page opened.
        ' *
        ' * @return The C entry of page object's additional actions dictionary.
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
        ' * This will set an action to be performed when the page
        ' * is closed. This action applies to the page being closed,
        ' * and is executed before any other page opened.
        ' *
        ' * @param c The action to be performed.
        ' */
        Public Sub setC(ByVal c As PDAction)
            actions.setItem("C", c)
        End Sub

    End Class

End Namespace
