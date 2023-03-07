Imports FinSeA.Drawings
Imports FinSeA.org.apache.pdfbox.util
Imports System.Drawing

Namespace org.apache.pdfbox.pdmodel.graphics.shading

    '/**
    ' * This represents the Paint of an axial shading.
    ' * 
    ' * @author lehmi
    ' * @version $Revision: $
    ' * 
    ' */
    Public Class AxialShadingPaint
        Implements Paint

        Private shading As PDShadingType2
        Private currentTransformationMatrix As Matrix
        Private pageHeight As Integer

        '/**
        ' * Constructor.
        ' * 
        ' * @param shadingType2 the shading resources
        ' * @param ctm current transformation matrix
        ' * @param pageSizeValue size of the current page
        ' */
        Public Sub New(ByVal shadingType2 As PDShadingType2, ByVal ctm As Matrix, ByVal pageHeightValue As Integer)
            shading = shadingType2
            currentTransformationMatrix = ctm
            pageHeight = pageHeightValue
        End Sub

        Public Function getTransparency() As Integer
            Return 0
        End Function

        Public Function createContext(ByVal cm As ColorModel, ByVal deviceBounds As RectangleF, ByVal userBounds As RectangleF, ByVal xform As AffineTransform, ByVal hints As RenderingHints) As PaintContext
            Return New AxialShadingContext(shading, cm, xform, currentTransformationMatrix, pageHeight)
        End Function

    End Class

End Namespace
