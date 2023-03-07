Namespace org.apache.pdfbox.util

    '/**
    ' * This class is a comparator for TextPosition operators.  It handles
    ' * pages with text in different directions by grouping the text based
    ' * on direction and sorting in that direction. This allows continuous text
    ' * in a given direction to be more easily grouped together.  
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.7 $
    ' */
    Public Class TextPositionComparator
        Implements Global.System.Collections.IComparer

        Public Function compare(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements Global.System.Collections.IComparer.Compare
            Dim retval As Integer = 0
            Dim pos1 As TextPosition = o1
            Dim pos2 As TextPosition = o2

            ' Only compare text that is in the same direction. */
            If (pos1.getDir() < pos2.getDir()) Then
                Return -1
            ElseIf (pos1.getDir() > pos2.getDir()) Then
                Return 1
            End If

            ' Get the text direction adjusted coordinates
            Dim x1 As Single = pos1.getXDirAdj()
            Dim x2 As Single = pos2.getXDirAdj()

            Dim pos1YBottom As Single = pos1.getYDirAdj()
            Dim pos2YBottom As Single = pos2.getYDirAdj()
            ' note that the coordinates have been adjusted so 0,0 is in upper left
            Dim pos1YTop As Single = pos1YBottom - pos1.getHeightDir()
            Dim pos2YTop As Single = pos2YBottom - pos2.getHeightDir()

            Dim yDifference As Single = Math.Abs(pos1YBottom - pos2YBottom)
            'we will do a simple tolerance comparison.
            If (yDifference < 0.10000000000000001 OrElse (pos2YBottom >= pos1YTop AndAlso pos2YBottom <= pos1YBottom) OrElse (pos1YBottom >= pos2YTop AndAlso pos1YBottom <= pos2YBottom)) Then
                If (x1 < x2) Then
                    retval = -1
                ElseIf (x1 > x2) Then
                    retval = 1
                Else
                    retval = 0
                End If
            ElseIf (pos1YBottom < pos2YBottom) Then
                retval = -1
            Else
                Return 1
            End If
            Return retval
        End Function


    End Class

End Namespace
