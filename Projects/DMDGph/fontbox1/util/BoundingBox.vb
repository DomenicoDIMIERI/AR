Imports System.Drawing

Namespace org.fontbox.util

    '/**
    ' * This is an implementation of a bounding box.  This was originally written for the
    ' * AMF parser.
    ' *
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class BoundingBox

        Private lowerLeftX As Single
        Private lowerLeftY As Single
        Private upperRightX As Single
        Private upperRightY As Single

        '/**
        ' * Getter for property lowerLeftX.
        ' *
        ' * @return Value of property lowerLeftX.
        ' */
        Public Function getLowerLeftX() As Single
            Return lowerLeftX
        End Function

        '/**
        ' * Setter for property lowerLeftX.
        ' *
        ' * @param lowerLeftXValue New value of property lowerLeftX.
        ' */
        Public Sub setLowerLeftX(ByVal lowerLeftXValue As Single)
            Me.lowerLeftX = lowerLeftXValue
        End Sub

        '/**
        ' * Getter for property lowerLeftY.
        ' *
        ' * @return Value of property lowerLeftY.
        ' */
        Public Function getLowerLeftY() As Single
            Return lowerLeftY
        End Function

        '/**
        ' * Setter for property lowerLeftY.
        ' *
        ' * @param lowerLeftYValue New value of property lowerLeftY.
        ' */
        Public Sub setLowerLeftY(ByVal lowerLeftYValue As Single)
            Me.lowerLeftY = lowerLeftYValue
        End Sub

        '/**
        ' * Getter for property upperRightX.
        ' *
        ' * @return Value of property upperRightX.
        ' */
        Public Function getUpperRightX() As Single
            Return upperRightX
        End Function

        '/**
        ' * Setter for property upperRightX.
        ' *
        ' * @param upperRightXValue New value of property upperRightX.
        ' */
        Public Sub setUpperRightX(ByVal upperRightXValue As Single)
            Me.upperRightX = upperRightXValue
        End Sub

        '/**
        ' * Getter for property upperRightY.
        ' *
        ' * @return Value of property upperRightY.
        ' */
        Public Function getUpperRightY() As Single
            Return upperRightY
        End Function

        '/**
        ' * Setter for property upperRightY.
        ' *
        ' * @param upperRightYValue New value of property upperRightY.
        ' */
        Public Sub setUpperRightY(ByVal upperRightYValue As Single)
            Me.upperRightY = upperRightYValue
        End Sub

        '/**
        ' * This will get the width of this rectangle as calculated by
        ' * upperRightX - lowerLeftX.
        ' *
        ' * @return The width of this rectangle.
        ' */
        Public Function getWidth() As Single
            Return getUpperRightX() - getLowerLeftX()
        End Function

        '/**
        ' * This will get the height of this rectangle as calculated by
        ' * upperRightY - lowerLeftY.
        ' *
        ' * @return The height of this rectangle.
        ' */
        Public Function getHeight() As Single
            Return getUpperRightY() - getLowerLeftY()
        End Function

        '/**
        ' * Checks if a point is inside this rectangle.
        ' * 
        ' * @param x The x coordinate.
        ' * @param y The y coordinate.
        ' * 
        ' * @return true If the point is on the edge or inside the rectangle bounds. 
        ' */
        Public Function contains(ByVal x As Single, ByVal y As Single) As Boolean
            Return x >= lowerLeftX AndAlso x <= upperRightX AndAlso y >= lowerLeftY AndAlso y <= upperRightY
        End Function

        '/**
        ' * Checks if a point is inside this rectangle.
        ' * 
        ' * @param point The point to check
        ' * 
        ' * @return true If the point is on the edge or inside the rectangle bounds. 
        ' */
        Public Function contains(ByVal point As PointF) As Boolean
            Return contains(point.X, point.Y)
        End Function

        '/**
        ' * This will return a string representation of this rectangle.
        ' *
        ' * @return This object as a string.
        ' */
        Public Overrides Function toString() As String
            Return "[" & getLowerLeftX() & "," & getLowerLeftY() & "," & getUpperRightX() & "," & getUpperRightY() & "]"
        End Function

    End Class

End Namespace