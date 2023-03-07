Namespace Drawings

    Public Interface PathIterator

        Enum Fields As Integer
            ''' <summary>
            ''' The segment type constant that specifies that the preceding subpath should be closed by appending a line segment back to the point corresponding to the most recent SEG_MOVETO.
            ''' </summary>
            ''' <remarks></remarks>
            SEG_CLOSE

            ''' <summary>
            ''' The segment type constant for the set of 3 points that specify a cubic parametric curve to be drawn from the most recently specified point.
            ''' </summary>
            ''' <remarks></remarks>
            SEG_CUBICTO

            ''' <summary>
            ''' The segment type constant for a point that specifies the end point of a line to be drawn from the most recently specified point.
            ''' </summary>
            ''' <remarks></remarks>
            SEG_LINETO

            ''' <summary>
            ''' The segment type constant for a point that specifies the starting location for a new subpath.
            ''' </summary>
            ''' <remarks></remarks>
            SEG_MOVETO

            ''' <summary>
            ''' The segment type constant for the pair of points that specify a quadratic parametric curve to be drawn from the most recently specified point.
            ''' </summary>
            ''' <remarks></remarks>
            SEG_QUADTO

            ''' <summary>
            ''' The winding rule constant for specifying an even-odd rule for determining the interior of a path.
            ''' </summary>
            ''' <remarks></remarks>
            WIND_EVEN_ODD

            ''' <summary>
            ''' The winding rule constant for specifying a non-zero rule for determining the interior of a path.
            ''' </summary>
            ''' <remarks></remarks>
            WIND_NON_ZERO
        End Enum


    End Interface

End Namespace