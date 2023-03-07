Imports System.IO
Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.documentnavigation.destination


    '/**
    ' * This represents a destination in a PDF document.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.6 $
    ' */
    Public MustInherit Class PDDestination
        Implements PDDestinationOrAction

        '/**
        ' * This will create a new destination depending on the type of COSBase
        ' * that is passed in.
        ' *
        ' * @param base The base level object.
        ' *
        ' * @return A new destination.
        ' *
        ' * @throws IOException If the base cannot be converted to a Destination.
        ' */
        Public Shared Function create(ByVal base As COSBase) As PDDestination 'throws IOException
            Dim retval As PDDestination = Nothing
            If (base Is Nothing) Then
                'this is ok, just return null.
            ElseIf (TypeOf (base) Is COSArray AndAlso DirectCast(base, COSArray).size() > 0) Then
                Dim array As COSArray = base
                Dim type As COSName = array.getObject(1)
                Dim typeString As String = type.getName()
                If (typeString.Equals(PDPageFitDestination.TYPE) OrElse typeString.Equals(PDPageFitDestination.TYPE_BOUNDED)) Then
                    retval = New PDPageFitDestination(array)
                ElseIf (typeString.Equals(PDPageFitHeightDestination.TYPE) OrElse typeString.Equals(PDPageFitHeightDestination.TYPE_BOUNDED)) Then
                    retval = New PDPageFitHeightDestination(array)
                ElseIf (typeString.Equals(PDPageFitRectangleDestination.TYPE)) Then
                    retval = New PDPageFitRectangleDestination(array)
                ElseIf (typeString.Equals(PDPageFitWidthDestination.TYPE) OrElse typeString.Equals(PDPageFitWidthDestination.TYPE_BOUNDED)) Then
                    retval = New PDPageFitWidthDestination(array)
                ElseIf (typeString.Equals(PDPageXYZDestination.TYPE)) Then
                    retval = New PDPageXYZDestination(array)
                Else
                    Throw New IOException("Unknown destination type:" & type.toString)
                End If
            ElseIf (TypeOf (base) Is COSString) Then
                retval = New PDNamedDestination(DirectCast(base, COSString))
            ElseIf (TypeOf (base) Is COSName) Then
                retval = New PDNamedDestination(DirectCast(base, COSName))
            Else
                Throw New IOException("Error: can't convert to Destination " & base.ToString)
            End If
            Return retval
        End Function

        ''/**
        '' * Return a string representation of this class.
        '' *
        '' * @return A human readable string.
        '' */
        'Public Overrides Function toString() As String
        '    Return MyBase.ToString()
        'End Function

        Public MustOverride Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject

    End Class

End Namespace
