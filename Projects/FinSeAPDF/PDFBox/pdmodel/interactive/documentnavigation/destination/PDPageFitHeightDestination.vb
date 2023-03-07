Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.interactive.documentnavigation.destination

    '/**
    ' * This represents a destination to a page at a x location and the height is magnified
    ' * to just fit on the screen.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDPageFitHeightDestination
        Inherits PDPageDestination

        ''' <summary>
        ''' The type of this destination.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend Const TYPE As String = "FitV"

        ''' <summary>
        ''' The type of this destination.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend Const TYPE_BOUNDED As String = "FitBV"

        '/**
        ' * Default constructor.
        ' *
        ' */
        Public Sub New()
            MyBase.New()
            array.growToSize(3)
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
        ' * A flag indicating if this page destination should just fit bounding box of the PDF.
        ' *
        ' * @return true If the destination should fit just the bounding box.
        ' */
        Public Function fitBoundingBox() As Boolean
            Return TYPE_BOUNDED.Equals(array.getName(1))
        End Function

        '/**
        ' * Set if this page destination should just fit the bounding box.  The default is false.
        ' *
        ' * @param fitBoundingBox A flag indicating if this should fit the bounding box.
        ' */
        Public Sub setFitBoundingBox(ByVal fitBoundingBox As Boolean)
            array.growToSize(2)
            If (fitBoundingBox) Then
                array.setName(1, TYPE_BOUNDED)
            Else
                array.setName(1, TYPE)
            End If
        End Sub

    End Class

End Namespace
