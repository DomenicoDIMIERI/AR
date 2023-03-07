Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.action.type

Namespace org.apache.pdfbox.pdmodel.interactive.action

    '/**
    ' * This class will take a dictionary and determine which type of action to create.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.5 $
    ' */
    Public NotInheritable Class PDActionFactory

        '/**
        ' * Utility Class.
        ' */
        Private Sub New()
            'utility class
        End Sub

        '/**
        ' * This will create the correct type of action based on the type specified
        ' * in the dictionary.
        ' *
        ' * @param action An action dictionary.
        ' *
        ' * @return An action of the correct type.
        ' */
        Public Shared Function createAction(ByVal action As COSDictionary) As PDAction
            Dim retval As PDAction = Nothing
            If (action IsNot Nothing) Then
                Dim type As String = action.getNameAsString("S")
                If (PDActionJavaScript.SUB_TYPE.equals(type)) Then
                    retval = New PDActionJavaScript(action)
                ElseIf (PDActionGoTo.SUB_TYPE.equals(type)) Then
                    retval = New PDActionGoTo(action)
                ElseIf (PDActionLaunch.SUB_TYPE.Equals(type)) Then
                    retval = New PDActionLaunch(action)
                ElseIf (PDActionRemoteGoTo.SUB_TYPE.Equals(type)) Then
                    retval = New PDActionRemoteGoTo(action)
                ElseIf (PDActionURI.SUB_TYPE.Equals(type)) Then
                    retval = New PDActionURI(action)
                End If
            End If
            Return retval
        End Function

    End Class

End Namespace
