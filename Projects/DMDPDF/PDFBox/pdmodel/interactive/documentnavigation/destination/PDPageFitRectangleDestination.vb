Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.interactive.documentnavigation.destination

    '/**
    ' * This represents a destination to a page at a y location and the width is magnified
    ' * to just fit on the screen.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDPageFitRectangleDestination
        Inherits PDPageDestination

        ''' <summary>
        ''' The type of this destination.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend Const TYPE As String = "FitR"

        '/**
        ' * Default constructor.
        ' *
        ' */
        Public Sub New()
            MyBase.New()
            array.growToSize(6)
            array.setName(1, TYPE)
        End Sub

        '/**
        ' * Constructor from an existing destination array.
        ' *
        ' * @param arr The destination array.
        ' */
        Public Sub New(ByVal arr As COSArray)
            MyBase.New(arr)
        End Sub

        '/**
        ' * Get the left x coordinate.  A return value of -1 implies that the current x-coordinate
        ' * will be used.
        ' *
        ' * @return The left x coordinate.
        ' */
        Public Function getLeft() As Integer
            Return array.getInt(2)
        End Function

        '/**
        ' * Set the left x-coordinate, a value of -1 implies that the current x-coordinate
        ' * will be used.
        ' * @param x The left x coordinate.
        ' */
        Public Sub setLeft(ByVal x As Integer)
            array.growToSize(3)
            If (x = -1) Then
                Dim tmp As COSBase = Nothing
                array.set(2, tmp)
            Else
                array.setInt(2, x)
            End If
        End Sub

        '/**
        ' * Get the bottom y coordinate.  A return value of -1 implies that the current y-coordinate
        ' * will be used.
        ' *
        ' * @return The bottom y coordinate.
        ' */
        Public Function getBottom() As Integer
            Return array.getInt(3)
        End Function

        '/**
        ' * Set the bottom y-coordinate, a value of -1 implies that the current y-coordinate
        ' * will be used.
        ' * @param y The bottom y coordinate.
        ' */
        Public Sub setBottom(ByVal y As Integer)
            array.growToSize(6)
            If (y = -1) Then
                Dim tmp As COSBase = Nothing
                array.set(3, tmp)
            Else
                array.setInt(3, y)
            End If
        End Sub

        '/**
        ' * Get the right x coordinate.  A return value of -1 implies that the current x-coordinate
        ' * will be used.
        ' *
        ' * @return The right x coordinate.
        ' */
        Public Function getRight() As Integer
            Return array.getInt(4)
        End Function

        '/**
        ' * Set the right x-coordinate, a value of -1 implies that the current x-coordinate
        ' * will be used.
        ' * @param x The right x coordinate.
        ' */
        Public Sub setRight(ByVal x As Integer)
            array.growToSize(6)
            If (x = -1) Then
                Dim tmp As COSBase = Nothing
                array.set(4, tmp)
            Else
                array.setInt(4, x)
            End If
        End Sub


        '/**
        ' * Get the top y coordinate.  A return value of -1 implies that the current y-coordinate
        ' * will be used.
        ' *
        ' * @return The top y coordinate.
        ' */
        Public Function getTop() As Integer
            Return array.getInt(5)
        End Function

        '/**
        ' * Set the top y-coordinate, a value of -1 implies that the current y-coordinate
        ' * will be used.
        ' * @param y The top ycoordinate.
        ' */
        Public Sub setTop(ByVal y As Integer)
            array.growToSize(6)
            If (y = -1) Then
                Dim tmp As COSBase = Nothing
                array.set(5, tmp)
            Else
                array.setInt(5, y)
            End If
        End Sub

    End Class

End Namespace

