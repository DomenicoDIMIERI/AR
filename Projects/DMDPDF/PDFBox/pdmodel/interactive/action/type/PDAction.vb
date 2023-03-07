Imports FinSeA
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action

Namespace org.apache.pdfbox.pdmodel.interactive.action.type

    '/**
    ' * This represents an action that can be executed in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @author Panagiotis Toumasis (ptoumasis@mail.gr)
    ' * @version $Revision: 1.3 $
    ' */
    Public MustInherit Class PDAction
        Implements PDDestinationOrAction

        ''' <summary>
        ''' The type of PDF object.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TYPE As String = "Action"

        ''' <summary>
        ''' The action dictionary.
        ''' </summary>
        ''' <remarks></remarks>
        Protected action As COSDictionary

        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            action = New COSDictionary()
            setActionType(TYPE)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param a The action dictionary.
        ' */
        Public Sub New(ByVal a As COSDictionary)
            action = a
        End Sub

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return action
        End Function

        '/**
        ' * Convert this standard java object to a COS object.
        ' *
        ' * @return The cos object that matches this Java object.
        ' */
        Public Function getCOSDictionary() As COSDictionary
            Return action
        End Function

        '/**
        ' * This will get the type of PDF object that the actions dictionary describes.
        ' * If present must be Action for an action dictionary.
        ' *
        ' * @return The Type of PDF object.
        ' */
        Public Function getActionType() As String
            Return action.getNameAsString("Type")
        End Function

        '/**
        ' * This will set the type of PDF object that the actions dictionary describes.
        ' * If present must be Action for an action dictionary.
        ' *
        ' * @param type The new Type for the PDF object.
        ' */
        Public Sub setActionType(ByVal type As String)
            action.setName("Type", type)
        End Sub

        '/**
        ' * This will get the type of action that the actions dictionary describes.
        ' * If present, must be Action for an action dictionary.
        ' *
        ' * @return The S entry of actions dictionary.
        ' */
        Public Function getSubType() As String
            Return action.getNameAsString("S")
        End Function

        '/**
        ' * This will set the type of action that the actions dictionary describes.
        ' * If present, must be Action for an action dictionary.
        ' *
        ' * @param s The new type of action.
        ' */
        Public Sub setSubType(ByVal s As String)
            action.setName("S", s)
        End Sub

        '/**
        ' * This will get the next action, or sequence of actions, to be performed after this one.
        ' * The value is either a single action dictionary or an array of action dictionaries
        ' * to be performed in order.
        ' *
        ' * @return The Next action or sequence of actions.
        ' */
        Public Function getNext() As List
            Dim retval As List = Nothing
            Dim [next] As COSBase = action.getDictionaryObject("Next")
            If (TypeOf ([next]) Is COSDictionary) Then
                Dim pdAction As PDAction = PDActionFactory.createAction([next])
                retval = New COSArrayList(pdAction, [next], action, COSName.getPDFName("Next"))
            ElseIf (TypeOf ([next]) Is COSArray) Then
                Dim array As COSArray = [next]
                Dim actions As List = New ArrayList()
                For i As Integer = 0 To array.size() - 1
                    actions.add(PDActionFactory.createAction(array.getObject(i)))
                Next
                retval = New COSArrayList(actions, array)
            End If

            Return retval
        End Function

        '/**
        ' * This will set the next action, or sequence of actions, to be performed after this one.
        ' * The value is either a single action dictionary or an array of action dictionaries
        ' * to be performed in order.
        ' *
        ' * @param next The Next action or sequence of actions.
        ' */
        Public Sub setNext(ByVal [next] As List)
            action.setItem("Next", COSArrayList.converterToCOSArray([next]))
        End Sub
    End Class

End Namespace
