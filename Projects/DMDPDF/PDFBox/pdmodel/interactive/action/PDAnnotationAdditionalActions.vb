Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action.type

Namespace org.apache.pdfbox.pdmodel.interactive.action

    '/**
    ' * This class represents an annotation's dictionary of actions
    ' * that occur due to events.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Panagiotis Toumasis (ptoumasis@mail.gr)
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDAnnotationAdditionalActions
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
        ' * This will get an action to be performed when the cursor
        ' * enters the annotation's active area.
        ' *
        ' * @return The E entry of annotation's additional actions dictionary.
        ' */
        Public Function getE() As PDAction
            Dim e As COSDictionary = actions.getDictionaryObject("E")
            Dim retval As PDAction = Nothing
            If (e IsNot Nothing) Then
                retval = PDActionFactory.createAction(e)
            End If
            Return retval
        End Function

        '/**
        ' * This will set an action to be performed when the cursor
        ' * enters the annotation's active area.
        ' *
        ' * @param e The action to be performed.
        ' */
        Public Sub setE(ByVal e As PDAction)
            actions.setItem("E", e)
        End Sub

        '/**
        ' * This will get an action to be performed when the cursor
        ' * exits the annotation's active area.
        ' *
        ' * @return The X entry of annotation's additional actions dictionary.
        ' */
        Public Function getX() As PDAction
            Dim x As COSDictionary = actions.getDictionaryObject("X")
            Dim retval As PDAction = Nothing
            If (x IsNot Nothing) Then
                retval = PDActionFactory.createAction(x)
            End If
            Return retval
        End Function

        '/**
        ' * This will set an action to be performed when the cursor
        ' * exits the annotation's active area.
        ' *
        ' * @param x The action to be performed.
        ' */
        Public Sub setX(ByVal x As PDAction)
            actions.setItem("X", x)
        End Sub

        '/**
        ' * This will get an action to be performed when the mouse button
        ' * is pressed inside the annotation's active area.
        ' * The name D stands for "down".
        ' *
        ' * @return The d entry of annotation's additional actions dictionary.
        ' */
        Public Function getD() As PDAction
            Dim d As COSDictionary = actions.getDictionaryObject("D")
            Dim retval As PDAction = Nothing
            If (d IsNot Nothing) Then
                retval = PDActionFactory.createAction(d)
            End If
            Return retval
        End Function

        '/**
        ' * This will set an action to be performed when the mouse button
        ' * is pressed inside the annotation's active area.
        ' * The name D stands for "down".
        ' *
        ' * @param d The action to be performed.
        ' */
        Public Sub setD(ByVal d As PDAction)
            actions.setItem("D", d)
        End Sub

        '/**
        ' * This will get an action to be performed when the mouse button
        ' * is released inside the annotation's active area.
        ' * The name U stands for "up".
        ' *
        ' * @return The U entry of annotation's additional actions dictionary.
        ' */
        Public Function getU() As PDAction
            Dim u As COSDictionary = actions.getDictionaryObject("U")
            Dim retval As PDAction = Nothing
            If (u IsNot Nothing) Then
                retval = PDActionFactory.createAction(u)
            End If
            Return retval
        End Function

        '/**
        ' * This will set an action to be performed when the mouse button
        ' * is released inside the annotation's active area.
        ' * The name U stands for "up".
        ' *
        ' * @param u The action to be performed.
        ' */
        Public Sub setU(ByVal u As PDAction)
            actions.setItem("U", u)
        End Sub

        '/**
        ' * This will get an action to be performed when the annotation
        ' * receives the input focus.
        ' *
        ' * @return The Fo entry of annotation's additional actions dictionary.
        ' */
        Public Function getFo() As PDAction
            Dim fo As COSDictionary = actions.getDictionaryObject("Fo")
            Dim retval As PDAction = Nothing
            If (fo IsNot Nothing) Then
                retval = PDActionFactory.createAction(fo)
            End If
            Return retval
        End Function

        '/**
        ' * This will set an action to be performed when the annotation
        ' * receives the input focus.
        ' *
        ' * @param fo The action to be performed.
        ' */
        Public Sub setFo(ByVal fo As PDAction)
            actions.setItem("Fo", fo)
        End Sub

        '/**
        ' * This will get an action to be performed when the annotation
        ' * loses the input focus.
        ' * The name Bl stands for "blurred".
        ' *
        ' * @return The Bl entry of annotation's additional actions dictionary.
        ' */
        Public Function getBl() As PDAction
            Dim bl As COSDictionary = actions.getDictionaryObject("Bl")
            Dim retval As PDAction = Nothing
            If (bl IsNot Nothing) Then
                retval = PDActionFactory.createAction(bl)
            End If
            Return retval
        End Function

        '/**
        ' * This will set an action to be performed when the annotation
        ' * loses the input focus.
        ' * The name Bl stands for "blurred".
        ' *
        ' * @param bl The action to be performed.
        ' */
        Public Sub setBl(ByVal bl As PDAction)
            actions.setItem("Bl", bl)
        End Sub

        '/**
        ' * This will get an action to be performed when the page containing
        ' * the annotation is opened. The action is executed after the O action
        ' * in the page's additional actions dictionary and the OpenAction entry
        ' * in the document catalog, if such actions are present.
        ' *
        ' * @return The PO entry of annotation's additional actions dictionary.
        ' */
        Public Function getPO() As PDAction
            Dim po As COSDictionary = actions.getDictionaryObject("PO")
            Dim retval As PDAction = Nothing
            If (po IsNot Nothing) Then
                retval = PDActionFactory.createAction(po)
            End If
            Return retval
        End Function

        '/**
        ' * This will set an action to be performed when the page containing
        ' * the annotation is opened. The action is executed after the O action
        ' * in the page's additional actions dictionary and the OpenAction entry
        ' * in the document catalog, if such actions are present.
        ' *
        ' * @param po The action to be performed.
        ' */
        Public Sub setPO(ByVal po As PDAction)
            actions.setItem("PO", po)
        End Sub

        '/**
        ' * This will get an action to be performed when the page containing
        ' * the annotation is closed. The action is executed before the C action
        ' * in the page's additional actions dictionary, if present.
        ' *
        ' * @return The PC entry of annotation's additional actions dictionary.
        ' */
        Public Function getPC() As PDAction
            Dim pc As COSDictionary = actions.getDictionaryObject("PC")
            Dim retval As PDAction = Nothing
            If (pc IsNot Nothing) Then
                retval = PDActionFactory.createAction(pc)
            End If
            Return retval
        End Function

        '/**
        ' * This will set an action to be performed when the page containing
        ' * the annotation is closed. The action is executed before the C action
        ' * in the page's additional actions dictionary, if present.
        ' *
        ' * @param pc The action to be performed.
        ' */
        Public Sub setPC(ByVal pc As PDAction)
            actions.setItem("PC", pc)
        End Sub

        '/**
        ' * This will get an action to be performed when the page containing
        ' * the annotation becomes visible in the viewer application's user interface.
        ' *
        ' * @return The PV entry of annotation's additional actions dictionary.
        ' */
        Public Function getPV() As PDAction
            Dim pv As COSDictionary = actions.getDictionaryObject("PV")
            Dim retval As PDAction = Nothing
            If (pv IsNot Nothing) Then
                retval = PDActionFactory.createAction(pv)
            End If
            Return retval
        End Function

        '/**
        ' * This will set an action to be performed when the page containing
        ' * the annotation becomes visible in the viewer application's user interface.
        ' *
        ' * @param pv The action to be performed.
        ' */
        Public Sub setPV(ByVal pv As PDAction)
            actions.setItem("PV", pv)
        End Sub

        '/**
        ' * This will get an action to be performed when the page containing the annotation
        ' * is no longer visible in the viewer application's user interface.
        ' *
        ' * @return The PI entry of annotation's additional actions dictionary.
        ' */
        Public Function getPI() As PDAction
            Dim pi As COSDictionary = actions.getDictionaryObject("PI")
            Dim retval As PDAction = Nothing
            If (pi IsNot Nothing) Then
                retval = PDActionFactory.createAction(pi)
            End If
            Return retval
        End Function

        '/**
        ' * This will set an action to be performed when the page containing the annotation
        ' * is no longer visible in the viewer application's user interface.
        ' *
        ' * @param pi The action to be performed.
        ' */
        Public Sub setPI(ByVal pi As PDAction)
            actions.setItem("PI", pi)
        End Sub

    End Class

End Namespace
