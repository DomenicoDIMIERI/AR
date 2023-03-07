Namespace org.apache.fontbox.ttf

    '/**
    ' * Specifies access to glyph description classes, simple and composite.
    ' * 
    ' * This class is based on code from Apache Batik a subproject of Apache XMLGraphics.
    ' * see http://xmlgraphics.apache.org/batik/ for further details.
    ' * 
    ' */
    Public Interface GlyphDescription

        '/** 
        ' * Returns the index of the ending point of the given contour.
        ' * 
        ' * @param i the number of the contour
        ' * @return the index of the ending point of the given contour
        ' */
        Function getEndPtOfContours(ByVal i As Integer) As Integer

        '/**
        ' * Returns the flags of the given point.
        ' * @param i the given point
        ' * @return the flags value for the given point
        ' */
        Function getFlags(ByVal i As Integer) As Byte

        '/**
        ' * Returns the x coordinate of the given point.
        ' * @param i the given point
        ' * @return the x coordinate value for the given point
        ' */
        Function getXCoordinate(ByVal i As Integer) As Short

        '/**
        ' * Returns the y coordinate of the given point.
        ' * @param i the given point
        ' * @return the y coordinate value for the given point
        ' */
        Function getYCoordinate(ByVal i As Integer) As Short

        '/**
        ' * Returns whether Me point is a composite or not.
        ' * @return true if Me point is a composite
        ' */
        Function isComposite() As Boolean

        '/**
        ' * Returns the number of points.
        ' * @return the number of points
        ' */
        Function getPointCount() As Integer

        '/**
        ' * Returns the number of contours.
        ' * @return the number of contours
        ' */
        Function getContourCount() As Integer

        '/**
        ' * Resolve all parts of an composite glyph.
        ' */
        Sub resolve()

    End Interface

End Namespace
