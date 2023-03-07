Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.interactive.documentnavigation.destination


    '/**
    ' * This represents a destination to a page and the page contents will be magnified to just
    ' * fit on the screen.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class PDPageFitDestination
        Inherits PDPageDestination

        ''' <summary>
        ''' The type of this destination.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend Const TYPE As String = "Fit"

        ''' <summary>
        ''' The type of this destination.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Friend Const TYPE_BOUNDED As String = "FitB"

        '/**
        ' * Default constructor.
        ' *
        ' */
        Public Sub New()
            MyBase.New()
            array.growToSize(2)
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
