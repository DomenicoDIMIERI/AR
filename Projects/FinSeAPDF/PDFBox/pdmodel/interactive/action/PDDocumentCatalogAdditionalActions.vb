Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action.type

Namespace org.apache.pdfbox.pdmodel.interactive.action

    '/**
    ' * This class represents a document catalog's dictionary of actions
    ' * that occur due to events.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Panagiotis Toumasis (ptoumasis@mail.gr)
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDDocumentCatalogAdditionalActions
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
        ' * This will get a JavaScript action to be performed
        ' * before closing a document.
        ' * The name WC stands for "will close".
        ' *
        ' * @return The WC entry of document catalog's additional actions dictionary.
        ' */
        Public Function getWC() As PDAction
            Dim wc As COSDictionary = actions.getDictionaryObject("WC")
            Dim retval As PDAction = Nothing
            If (wc IsNot Nothing) Then
                retval = PDActionFactory.createAction(wc)
            End If
            Return retval
        End Function

        '/**
        ' * This will set a JavaScript action to be performed
        ' * before closing a document.
        ' * The name WC stands for "will close".
        ' *
        ' * @param wc The action to be performed.
        ' */
        Public Sub setWC(ByVal wc As PDAction)
            actions.setItem("WC", wc)
        End Sub

        '/**
        ' * This will get a JavaScript action to be performed
        ' * before saving a document.
        ' * The name WS stands for "will save".
        ' *
        ' * @return The WS entry of document catalog's additional actions dictionary.
        ' */
        Public Function getWS() As PDAction
            Dim ws As COSDictionary = actions.getDictionaryObject("WS")
            Dim retval As PDAction = Nothing
            If (ws IsNot Nothing) Then
                retval = PDActionFactory.createAction(ws)
            End If
            Return retval
        End Function

        '/**
        ' * This will set a JavaScript action to be performed
        ' * before saving a document.
        ' * The name WS stands for "will save".
        ' *
        ' * @param ws The action to be performed.
        ' */
        Public Sub setWS(ByVal ws As PDAction)
            actions.setItem("WS", ws)
        End Sub

        '/**
        ' * This will get a JavaScript action to be performed
        ' * after saving a document.
        ' * The name DS stands for "did save".
        ' *
        ' * @return The DS entry of document catalog's additional actions dictionary.
        ' */
        Public Function getDS() As PDAction
            Dim ds As COSDictionary = actions.getDictionaryObject("DS")
            Dim retval As PDAction = Nothing
            If (ds IsNot Nothing) Then
                retval = PDActionFactory.createAction(ds)
            End If
            Return retval
        End Function

        '/**
        ' * This will set a JavaScript action to be performed
        ' * after saving a document.
        ' * The name DS stands for "did save".
        ' *
        ' * @param ds The action to be performed.
        ' */
        Public Sub setDS(ByVal ds As PDAction)
            actions.setItem("DS", ds)
        End Sub

        '/**
        ' * This will get a JavaScript action to be performed
        ' * before printing a document.
        ' * The name WP stands for "will print".
        ' *
        ' * @return The WP entry of document catalog's additional actions dictionary.
        ' */
        Public Function getWP() As PDAction
            Dim wp As COSDictionary = actions.getDictionaryObject("WP")
            Dim retval As PDAction = Nothing
            If (wp IsNot Nothing) Then
                retval = PDActionFactory.createAction(wp)
            End If
            Return retval
        End Function

        '/**
        ' * This will set a JavaScript action to be performed
        ' * before printing a document.
        ' * The name WP stands for "will print".
        ' *
        ' * @param wp The action to be performed.
        ' */
        Public Sub setWP(ByVal wp As PDAction)
            actions.setItem("WP", wp)
        End Sub

        '/**
        ' * This will get a JavaScript action to be performed
        ' * after printing a document.
        ' * The name DP stands for "did print".
        ' *
        ' * @return The DP entry of document catalog's additional actions dictionary.
        ' */
        Public Function getDP() As PDAction
            Dim dp As COSDictionary = actions.getDictionaryObject("DP")
            Dim retval As PDAction = Nothing
            If (dp IsNot Nothing) Then
                retval = PDActionFactory.createAction(dp)
            End If
            Return retval
        End Function

        '/**
        ' * This will set a JavaScript action to be performed
        ' * after printing a document.
        ' * The name DP stands for "did print".
        ' *
        ' * @param dp The action to be performed.
        ' */
        Public Sub setDP(ByVal dp As PDAction)
            actions.setItem("DP", dp)
        End Sub

    End Class

End Namespace
